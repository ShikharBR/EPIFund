using Inview.Epi.EpiFund.Domain;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Net;
using System.Xml.Linq;
using System.Configuration;
using Inview.Epi.EpiFund.Domain.ViewModel;
using Inview.Epi.EpiFund.Domain.Helpers;
using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.Entity;
using Reliant.Utility.Impersonator;
using System.Drawing;
using WebSupergoo.ABCpdf9;
using System.Runtime.InteropServices;
using WebSupergoo.ABCpdf9.Objects;
using WebSupergoo.ABCpdf9.Operations;
using System.Threading;




namespace Inview.Epi.EpiFund.Business
{
    public class PDFService : IPDFService
    {
        private IEPIContextFactory _factory;
        private string _dataRoot;
        private string _license;

        public PDFService(IEPIContextFactory factory)
        {
            _factory = factory;
            _dataRoot = ConfigurationManager.AppSettings["DataRoot"];
            _license = ConfigurationManager.AppSettings["ABCPdfLicense"];

            // enable TLS1.2 -- RE: @ PW-419
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public List<string> GetFieldsFromForm(string path)
        {
            // create a new PDF reader based on the PDF template document
            PdfReader pdfReader = new PdfReader(path);
            // create and populate a string builder with each of the 
            // field names available in the subject PDF
            List<string> fieldNames = new List<string>();
            if (pdfReader.AcroFields.Fields.Count == 0)
            {
                var s = ReadXfa(pdfReader);
            }
            foreach (var de in pdfReader.AcroFields.Fields)
            {
                fieldNames.Add(de.Key.ToString());
            }
            return fieldNames;
        }

        public string ReadXfa(PdfReader reader)
        {
            XfaForm xfa = new XfaForm(reader);
            XmlDocument doc = xfa.DomDocument;
            reader.Close();

            if (!string.IsNullOrEmpty(doc.DocumentElement.NamespaceURI))
            {
                doc.DocumentElement.SetAttribute("xmlns", "");
                XmlDocument new_doc = new XmlDocument();
                new_doc.LoadXml(doc.OuterXml);
                doc = new_doc;
            }

            var sb = new StringBuilder(4000);
            var Xsettings = new XmlWriterSettings() { Indent = true };
            using (var writer = XmlWriter.Create(sb, Xsettings))
            {
                doc.WriteTo(writer);
            }
            return sb.ToString();
        }

        public void manipulatePdf(String src, String dest)
        {
            PdfReader reader = new PdfReader(src);
            PdfDictionary root = reader.Catalog;
            PdfDictionary form = root.GetAsDict(PdfName.ACROFORM);
            PdfArray fields = form.GetAsArray(PdfName.FIELDS);

            PdfDictionary page;
            PdfArray annots;
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                page = reader.GetPageN(i);
                annots = page.GetAsArray(PdfName.ANNOTS);
                for (int j = 0; j < annots.Size; j++)
                {
                    fields.Add(annots.GetAsIndirectObject(j));
                }
            }
            PdfStamper stamper = new PdfStamper(reader,
                 new FileStream(dest, FileMode.Create));
            stamper.Close();
            reader.Close();
        }

        public MemoryStream Submit1099(MiscellaneousIncomeTemplateModel model)
        {
            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Misc1099.pdf"));
            pdfReader.RemoveUsageRights();

            using (PdfStamper stamper = new PdfStamper(pdfReader, ms))
            {
                AcroFields form = stamper.AcroFields;
                var fieldKeys = form.Fields.Keys;
                string tin = string.Empty;
                string exceed = string.Empty;
                if (model.SecondTIN != null) tin = (bool)model.SecondTIN ? "Yes" : "No";
                if (model.PayerExceededSales != null) exceed = (bool)model.PayerExceededSales ? "Yes" : "No";
                foreach (string fieldKey in fieldKeys)
                {
                    if (fieldKey.Contains("f1_1")) form.SetField(fieldKey, $"{model.PayerName}\r\n{model.PayerAddress}\r\n{model.PayerTelephone}");
                    if (fieldKey.Contains("f1_2")) form.SetField(fieldKey, model.PayerFederalId);
                    if (fieldKey.Contains("f1_3")) form.SetField(fieldKey, model.RecipientFederalId);
                    if (fieldKey.Contains("f1_4")) form.SetField(fieldKey, model.RecipientName);
                    if (fieldKey.Contains("f1_5")) form.SetField(fieldKey, model.RecipientAddress);
                    if (fieldKey.Contains("f1_6")) form.SetField(fieldKey, $"{model.RecipientCity} {model.RecipientState} {model.RecipientZip} {model.RecipientCountry}");
                    if (fieldKey.Contains("f1_7")) form.SetField(fieldKey, model.AccountNumber);
                    if (fieldKey.Contains("c1_2") || fieldKey.Contains("c2_2")) form.SetField(fieldKey, model.FactaFilingRequirement ? "Yes" : "No");
                    if (fieldKey.Contains("c1_3") || fieldKey.Contains("c2_3")) form.SetField(fieldKey, tin);
                    if (fieldKey.Contains("f1_8")) form.SetField(fieldKey, model.Rents != null ? string.Format("{0:n0}", model.Rents) : "");
                    if (fieldKey.Contains("f1_9")) form.SetField(fieldKey, model.Royalties != null ? string.Format("{0:n0}", model.Royalties) : "");
                    if (fieldKey.Contains("f1_10")) form.SetField(fieldKey, model.OtherIncome != null ? string.Format("{0:n0}", model.OtherIncome) : "");
                    if (fieldKey.Contains("f1_11")) form.SetField(fieldKey, model.TaxWithheld != null ? string.Format("{0:n0}", model.TaxWithheld) : "");
                    if (fieldKey.Contains("f1_12")) form.SetField(fieldKey, model.FishingBoatProceeds != null ? string.Format("{0:n0}", model.FishingBoatProceeds) : "");
                    if (fieldKey.Contains("f1_13")) form.SetField(fieldKey, model.MedicalPayments != null ? string.Format("{0:n0}", model.MedicalPayments) : "");
                    if (fieldKey.Contains("f1_14")) form.SetField(fieldKey, model.NonEmployeeCompensation != null ? string.Format("{0:n0}", model.NonEmployeeCompensation) : "");
                    if (fieldKey.Contains("f1_15")) form.SetField(fieldKey, model.SubstitutePayments != null ? string.Format("{0:n0}", model.SubstitutePayments) : "");
                    if (fieldKey.Contains("c1_4") || fieldKey.Contains("c2_4")) form.SetField(fieldKey, exceed);
                    if (fieldKey.Contains("f1_16")) form.SetField(fieldKey, model.CropInsuranceProceeds != null ? string.Format("{0:n0}", model.CropInsuranceProceeds) : "");
                    if (fieldKey.Contains("f1_17") || fieldKey.Contains("f1_18")) form.SetField(fieldKey, ""); // empty textboxes on default form
                    if (fieldKey.Contains("f1_19")) form.SetField(fieldKey, model.ParachutePayments != null ? string.Format("{0:n0}", model.ParachutePayments) : "");
                    if (fieldKey.Contains("f1_20")) form.SetField(fieldKey, model.AttorneyPayment != null ? string.Format("{0:n0}", model.AttorneyPayment) : "");
                    if (fieldKey.Contains("f1_21")) form.SetField(fieldKey, model.Deferrals != null ? string.Format("{0:n0}", model.Deferrals) : "");
                    if (fieldKey.Contains("f1_22")) form.SetField(fieldKey, model.Income != null ? string.Format("{0:n0}", model.Income) : "");
                    if (fieldKey.Contains("f1_23")) form.SetField(fieldKey, model.TaxWithheld1 != null ? string.Format("{0:n0}", model.TaxWithheld1) : "");
                    if (fieldKey.Contains("f1_24")) form.SetField(fieldKey, model.TaxWithheld2 != null ? string.Format("{0:n0}", model.TaxWithheld2) : "");
                    if (fieldKey.Contains("f1_25")) form.SetField(fieldKey, model.StateNumber1 != null ? model.StateNumber1 : "");
                    if (fieldKey.Contains("f1_26")) form.SetField(fieldKey, model.StateNumber2 != null ? model.StateNumber2 : "");
                    if (fieldKey.Contains("f1_27")) form.SetField(fieldKey, model.StateIncome1 != null ? string.Format("{0:n0}", model.StateIncome1) : "");
                    if (fieldKey.Contains("f1_28")) form.SetField(fieldKey, model.StateIncome2 != null ? string.Format("{0:n0}", model.StateIncome2) : "");

                    stamper.FormFlattening = true;
                    stamper.Writer.CloseStream = false;
                }
            }

            ms.Position = 0;
            pdfReader.Close();
            return ms;
        }

        public void SubmitMDA(MDATemplateModel model, byte[] doc, int signingUserId, int documentType)
        {
            var ms = GetMDAPdf(model, doc);
            string xmlBody = string.Empty;

            if (documentType == 1)
            {
                xmlBody =
                        "<envelopeDefinition xmlns=\"http://www.docusign.com/restapi\">" +
                        "<emailSubject>DocuSign API - Signature Request on Document</emailSubject>" +
                        "<status>sent</status>" + 	// "sent" to send immediately, "created" to save as draft in your account
                    // add document(s)
                        "<documents>" +
                        "<document>" +
                        "<documentId>1</documentId>" +
                        "<name>Margin Disclosure and Compensation Agreement</name>" +
                        "</document>" +
                        "</documents>" +
                    // add recipient(s)
                        "<recipients>" +
                        "<signers>" +
                        "<signer>" +
                        "<recipientId>1</recipientId>" +
                        "<email>" + model.Email + "</email>" +
                        "<name>" + model.UserFirstName + " " + model.UserLastName + "</name>" +
                        "<tabs>" +
                        "<initialHereTabs>" +
                        "<initialHere>" +
                        "<tabLabel>i1</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>115</xPosition>" +
                        "<yPosition>717</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>1</pageNumber>" +
                        "<scaleValue>0.7</scaleValue>" +
                        "</initialHere>" +
                        "<initialHere>" +
                        "<tabLabel>i2</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>115</xPosition>" +
                        "<yPosition>717</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>2</pageNumber>" +
                        "<scaleValue>0.7</scaleValue>" +
                        "</initialHere>" +
                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>115</xPosition>" +
                        "<yPosition>717</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>3</pageNumber>" +
                        "<scaleValue>0.7</scaleValue>" +
                        "</initialHere>" +
                        "</initialHereTabs>" +
                        "<signHereTabs>" +
                        "<signHere>" +
                        "<xPosition>140</xPosition>" + // default unit is pixels
                        "<yPosition>131</yPosition>" + // default unit is pixels
                        "<documentId>1</documentId>" +
                        "<pageNumber>3</pageNumber>" +
                        "<scaleValue>0.8</scaleValue>" +
                        "</signHere>" +
                        "</signHereTabs>" +
                        "</tabs>" +
                        "</signer>" +
                        "</signers>" +
                        "</recipients>" +
                        "</envelopeDefinition>";
            }
            else
            {
                xmlBody =
                        "<envelopeDefinition xmlns=\"http://www.docusign.com/restapi\">" +
                        "<emailSubject>DocuSign API - Signature Request on Document</emailSubject>" +
                        "<status>sent</status>" + 	// "sent" to send immediately, "created" to save as draft in your account
                    // add document(s)
                        "<documents>" +
                        "<document>" +
                        "<documentId>1</documentId>" +
                        "<name>Margin Disclosure and Compensation Agreement</name>" +
                        "</document>" +
                        "</documents>" +
                    // add recipient(s)
                        "<recipients>" +
                        "<signers>" +
                        "<signer>" +
                        "<recipientId>1</recipientId>" +
                        "<email>" + model.Email + "</email>" +
                        "<name>" + model.UserFirstName + " " + model.UserLastName + "</name>" +
                        "<tabs>" +
                        "<initialHereTabs>" +
                        "<initialHere>" +
                        "<tabLabel>i1</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>115</xPosition>" +
                        "<yPosition>717</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>1</pageNumber>" +
                        "<scaleValue>0.7</scaleValue>" +
                        "</initialHere>" +
                        "<initialHere>" +
                        "<tabLabel>i2</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>115</xPosition>" +
                        "<yPosition>717</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>2</pageNumber>" +
                        "<scaleValue>0.7</scaleValue>" +
                        "</initialHere>" +
                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>115</xPosition>" +
                        "<yPosition>717</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>3</pageNumber>" +
                        "<scaleValue>0.7</scaleValue>" +
                        "</initialHere>" +
                        "</initialHereTabs>" +
                        "<signHereTabs>" +
                        "<signHere>" +
                        "<xPosition>140</xPosition>" + // default unit is pixels
                        "<yPosition>131</yPosition>" + // default unit is pixels
                        "<documentId>1</documentId>" +
                        "<pageNumber>3</pageNumber>" +
                        "<scaleValue>0.8</scaleValue>" +
                        "</signHere>" +
                        "</signHereTabs>" +
                        "</tabs>" +
                        "</signer>" +
                        "</signers>" +
                        "</recipients>" +
                        "</envelopeDefinition>";
            }

            saveEnvelope(DocumentType.MDA, sendToDocusign(ms, "MDA.pdf", xmlBody), signingUserId, model.Assets.Select(s => s.AssetNumber).ToList());
        }

        public void CreateNCND(byte[] doc, NCNDTemplateModel model)
        {
            var ms = stampNCNDPDF(doc, model, true);
            var context = _factory.Create();
            var user = context.Users.FirstOrDefault(s => s.Username == model.Email);
            var path = Path.Combine(ConfigurationManager.AppSettings["pdfDirectory"], user.UserId.ToString());
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, Guid.NewGuid() + ".pdf");
            File.WriteAllBytes(path, ms.ToArray());
            user.NCNDFileLocation = path;
            user.NCNDSignDate = DateTime.Now;
            var file = new UserFile()
            {
                DateUploaded = DateTime.Now,
                FileLocation = path,
                FileName = "Executed NCND",
                UserId = user.UserId
            };
            context.UserFiles.Add(file);
            context.Save();
        }

        private MemoryStream stampNCNDPDF(byte[] doc, NCNDTemplateModel model, bool stampSignature = false)
        {
            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(doc);
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, ms);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField1[0]", model.Day.ToString() + GetDaySuffix(model.Day));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField2[0]", model.Month.ToString());
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField3[0]", model.Year.ToString().Substring(2, 2));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField4[0]", model.CompanyName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField5[0]", model.StateOfOriginOfCorporateEntity);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField6[0]", EnumHelper.GetEnumDescription(model.TypeOfCorporateEntity));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField7[0]", model.AcronymOfCorporateEntity);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField8[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField9[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField10[0]", model.AcronymOfCorporateEntity);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField11[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField12[0]", model.CompanyName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField13[0]", model.UserAddressLine1 + " " + model.UserAddressLine2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField14[0]", string.Format("{0}, {1} {2}", model.City, model.State, model.Zip));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField15[0]", model.Email);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField16[0]", model.Phone);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField17[0]", model.Fax);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page7[0].TextField18[0]", model.CompanyName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page7[0].TextField19[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page7[0].TextField20[0]", model.CorpTitle);
            if (stampSignature)
            {
                var initials = model.UserFirstName.Substring(0, 1) + model.UserLastName.Substring(0, 1);
                pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField21[0]", initials);
                pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField22[0]", initials);
                pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField23[0]", initials);
                pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField24[0]", initials);
                pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField25[0]", initials);
                pdfMs.AcroFields.SetField("topmostSubform[0].Page6[0].TextField26[0]", initials);
                pdfMs.AcroFields.SetField("topmostSubform[0].Page7[0].TextField27[0]", initials);
                pdfMs.AcroFields.SetField("topmostSubform[0].Page7[0].TextField28[0]", model.UserFirstName + " " + model.UserLastName);
            }
            pdfMs.FormFlattening = true;
            pdfMs.Writer.CloseStream = false;

            if (pdfMs != null)
            {
                pdfMs.Close();
            }
            ms.Position = 0;
            pdfReader.Close();
            return ms;
        }

        public void SubmitNCND(NCNDTemplateModel model, byte[] doc, int signingUserId)
        {
            var ms = stampNCNDPDF(doc, model);
            string xmlBody =
                    "<envelopeDefinition xmlns=\"http://www.docusign.com/restapi\">" +
                    "<emailSubject>DocuSign API - Signature Request on Document</emailSubject>" +
                    "<status>sent</status>" + 	// "sent" to send immediately, "created" to save as draft in your account
                // add document(s)
                    "<documents>" +
                    "<document>" +
                    "<documentId>1</documentId>" +
                    "<name>Margin Disclosure and Compensation Agreement</name>" +
                    "</document>" +
                    "</documents>" +
                // add recipient(s)
                    "<recipients>" +
                    "<signers>" +
                    "<signer>" +
                    "<recipientId>1</recipientId>" +
                    "<email>" + model.Email + "</email>" +
                    "<name>" + model.UserFirstName + " " + model.UserLastName + "</name>" +
                    "<tabs>" +
                    "<initialHereTabs>" +

                    "<initialHere>" +
                    "<tabLabel>i1</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>115</xPosition>" +
                    "<yPosition>717</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>1</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +

                    "<initialHere>" +
                    "<tabLabel>i2</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>115</xPosition>" +
                    "<yPosition>717</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>2</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +

                    "<initialHere>" +
                    "<tabLabel>i3</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>115</xPosition>" +
                    "<yPosition>717</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>3</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +

                    "</initialHereTabs>" +
                    "<signHereTabs>" +

                    "<signHere>" +
                    "<xPosition>140</xPosition>" + // default unit is pixels
                    "<yPosition>137</yPosition>" + // default unit is pixels
                    "<documentId>1</documentId>" +
                    "<pageNumber>3</pageNumber>" +
                    "</signHere>" +

                    "</signHereTabs>" +
                    "</tabs>" +
                    "</signer>" +
                    "</signers>" +
                    "</recipients>" +
                    "</envelopeDefinition>";

            saveEnvelope(DocumentType.NCND, sendToDocusign(ms, "NCND.pdf", xmlBody), signingUserId, null);
        }

        public void SubmitICAgreement(ICAgreementTemplateModel model, byte[] doc, int signingUserId)
        {
            var ms = GetICPdf(model, doc);

            string xmlBody =
                    "<envelopeDefinition xmlns=\"http://www.docusign.com/restapi\">" +
                    "<emailSubject>DocuSign API - Signature Request on Document</emailSubject>" +
                    "<status>sent</status>" + 	// "sent" to send immediately, "created" to save as draft in your account
                // add document(s)
                    "<documents>" +
                    "<document>" +
                    "<documentId>1</documentId>" +
                    "<name>Independent Contractor Services Agreement</name>" +
                    "</document>" +
                    "</documents>" +
                // add recipient(s)
                    "<recipients>" +
                    "<signers>" +
                    "<signer>" +
                    "<recipientId>1</recipientId>" +
                    "<email>" + model.Email + "</email>" +
                    "<name>" + model.FullName + "</name>" +
                    "<tabs>" +
                //"<initialHereTabs>" +
                //"<initialHere>" +
                //"<tabLabel>i1</tabLabel>" +
                //"<recipientId>1</recipientId>" +
                //"<xPosition>100</xPosition>" +
                //"<yPosition>717</yPosition>" +
                //"<documentId>1</documentId>" +
                //"<pageNumber>1</pageNumber>" +
                //"<scaleValue>0.7</scaleValue>" +
                //"</initialHere>" +
                //"<initialHere>" +
                //"<tabLabel>i2</tabLabel>" +
                //"<recipientId>1</recipientId>" +
                //"<xPosition>100</xPosition>" +
                //"<yPosition>717</yPosition>" +
                //"<documentId>1</documentId>" +
                //"<pageNumber>2</pageNumber>" +
                //"<scaleValue>0.7</scaleValue>" +
                //"</initialHere>" +
                //"<initialHere>" +
                //"<tabLabel>i3</tabLabel>" +
                //"<recipientId>1</recipientId>" +
                //"<xPosition>100</xPosition>" +
                //"<yPosition>717</yPosition>" +
                //"<documentId>1</documentId>" +
                //"<pageNumber>3</pageNumber>" +
                //"<scaleValue>0.7</scaleValue>" +
                //"</initialHere>" +
                //"<initialHere>" +
                //"<tabLabel>i4</tabLabel>" +
                //"<recipientId>1</recipientId>" +
                //"<xPosition>100</xPosition>" +
                //"<yPosition>717</yPosition>" +
                //"<documentId>1</documentId>" +
                //"<pageNumber>4</pageNumber>" +
                //"<scaleValue>0.7</scaleValue>" +
                //"</initialHere>" +
                //"<initialHere>" +
                //"<tabLabel>i5</tabLabel>" +
                //"<recipientId>1</recipientId>" +
                //"<xPosition>100</xPosition>" +
                //"<yPosition>717</yPosition>" +
                //"<documentId>1</documentId>" +
                //"<pageNumber>5</pageNumber>" +
                //"<scaleValue>0.7</scaleValue>" +
                //"</initialHere>" +
                //"</initialHereTabs>" +
                    "<signHereTabs>" +
                    "<signHere>" +
                    "<xPosition>50</xPosition>" + // default unit is pixels
                    "<yPosition>609</yPosition>" + // default unit is pixels
                    "<documentId>1</documentId>" +
                    "<pageNumber>5</pageNumber>" +
                    "</signHere>" +
                    "</signHereTabs>" +
                    "</tabs>" +
                    "</signer>" +
                    "</signers>" +
                    "</recipients>" +
                    "</envelopeDefinition>";
            saveEnvelope(DocumentType.ICAgreement, sendToDocusign(ms, "ICAgreement.pdf", xmlBody), signingUserId, null);
        }

        public string GetDocumentFromDocusign(string envelopeId, int userId, DocumentType type)//, List<int> assetNumbers = null)
        {
            //return getDocumentsFromEnvelope(envelopeId, userId, type, assetNumbers).FirstOrDefault();
            return getDocumentsFromEnvelope(envelopeId, userId, type).FirstOrDefault();
        }

        public List<string> getDocumentsFromEnvelope(string envelopeId, int userId, DocumentType type)//, List<int> assetNumbers = null)
        {
            //commented version in test code
            List<string> documentNames = new List<string>();
            string baseURL = "";
            string url = ConfigurationManager.AppSettings["docusignEndpoint"];
            HttpWebRequest request = initializeRequest(url, "GET", null, ConfigurationManager.AppSettings["docusignEmail"], ConfigurationManager.AppSettings["docusignPassword"]);
            string response = getResponseBody(request);
            baseURL = parseDataFromResponse(response, "baseUrl");

            string r = prettyPrintXml(response);
            url = baseURL + "/envelopes?envelopeId=" + envelopeId + "&status=completed";
            request = initializeRequest(url, "GET", null, ConfigurationManager.AppSettings["docusignEmail"], ConfigurationManager.AppSettings["docusignPassword"]);
            response = getResponseBody(request);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(string.Format("<root>{0}</root>", response));
            if (xml.GetElementsByTagName("status")[0].InnerText.ToLower() == "completed")
            {
                url = baseURL + "/envelopes/" + envelopeId + "/documents";
                request = initializeRequest(url, "GET", null, ConfigurationManager.AppSettings["docusignEmail"], ConfigurationManager.AppSettings["docusignPassword"]);
                response = getResponseBody(request);

                Dictionary<string, string> docsList = new Dictionary<string, string>();
                string uri, name;
                using (XmlReader reader = XmlReader.Create(new StringReader(response)))
                {
                    while (reader.Read())
                    {
                        if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "envelopeDocument"))
                        {
                            XmlReader reader2 = reader.ReadSubtree();
                            uri = ""; name = "";
                            while (reader2.Read())
                            {
                                if ((reader2.NodeType == XmlNodeType.Element) && (reader2.Name == "name"))
                                {
                                    name = reader2.ReadString();
                                }
                                if ((reader2.NodeType == XmlNodeType.Element) && (reader2.Name == "uri"))
                                {
                                    uri = reader2.ReadString();
                                }
                            }
                            //if (name.Contains(".pdf")) 
                            docsList.Add(name, uri);
                        }
                    }
                }

                string a = prettyPrintXml(response);
                foreach (KeyValuePair<string, string> kvp in docsList)
                {
                    if (kvp.Key != "Summary")
                    {
                        url = baseURL + kvp.Value;
                        request = initializeRequest(url, "GET", null, ConfigurationManager.AppSettings["docusignEmail"], ConfigurationManager.AppSettings["docusignPassword"]);
                        request.Accept = "application/pdf";
                        HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
                        string path = Path.Combine(ConfigurationManager.AppSettings["pdfDirectory"], userId.ToString());
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        var filePath = Path.Combine(path, Guid.NewGuid().ToString() + ".pdf");
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (var impersonator = new Impersonator("EpiFundUser", "NAP7-FS1", "SupperMovieSmileFarm72"))
                            {
                                using (FileStream outfile = new FileStream(filePath, FileMode.Create))
                                {
                                    webResponse.GetResponseStream().CopyTo(ms);
                                    if (ms.Length > int.MaxValue)
                                    {
                                        throw new NotSupportedException("Cannot write a file larger than 2GB.");
                                    }
                                    outfile.Write(ms.GetBuffer(), 0, (int)ms.Length);
                                }
                            }
                        }
                        // add key here
                        documentNames.Add(filePath);
                    }
                }
                return documentNames;
            }
            else return new List<string>();
        }

        private string sendToDocusign(MemoryStream ms, string document, string xmlBody)
        {
            string url = ConfigurationManager.AppSettings["docusignEndpoint"];
            HttpWebRequest request = initializeRequest(url, "GET", null, ConfigurationManager.AppSettings["docusignEmail"], ConfigurationManager.AppSettings["docusignPassword"]);
            string response = getResponseBody(request);
            string baseURL = parseDataFromResponse(response, "baseUrl");
            url = baseURL + "/envelopes";

            request = initializeRequest(url, "POST", null, ConfigurationManager.AppSettings["docusignEmail"], ConfigurationManager.AppSettings["docusignPassword"]);
            configureMultiPartFormDataRequest(request, xmlBody, document, ms);
            response = getResponseBody(request);
            XDocument doc = XDocument.Load(new StringReader(response));
            var b = doc.Elements().First().Value.Replace("sent", "~").Split('~')[0];
            ms.Close();
            return b;
        }

        private void saveEnvelope(DocumentType type, string envelopeId, int userId, List<int> assetIds = null)
        {
            var context = _factory.Create();
            var envelope = new Domain.Entity.DocusignEnvelope()
            {
                DateCreated = DateTime.Now,
                DocumentType = type,
                EnvelopeId = envelopeId,
                UserId = userId,
                ReceivedSignedDocument = false,
                DateReceived = null
            };
            if (assetIds != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var id in assetIds)
                {
                    sb.Append(id);
                    sb.Append("&");
                }
                envelope.AssetNumbers = sb.ToString();
            }
            context.DocusignEnvelopes.Add(envelope);
            context.Save();
        }

        #region Docusign helper methods
        private void configureMultiPartFormDataRequest(HttpWebRequest request, string xmlBody, string docName, MemoryStream ms)
        {
            // overwrite the default content-type header and set a boundary marker
            request.ContentType = "multipart/form-data; boundary=BOUNDARY";

            // start building the multipart request body
            string requestBodyStart = "\r\n\r\n--BOUNDARY\r\n" +
                "Content-Type: application/xml\r\n" +
                "Content-Disposition: form-data\r\n" +
                "\r\n" +
                xmlBody + "\r\n\r\n--BOUNDARY\r\n" + 	// our xml formatted envelopeDefinition
                "Content-Type: application/pdf\r\n" +
                "Content-Disposition: file; filename=\"" + docName + "\"; documentId=1\r\n" +
                "\r\n";
            string requestBodyEnd = "\r\n--BOUNDARY--\r\n\r\n";

            // read contents of provided document into the request stream

            // write the body of the request
            byte[] bodyStart = System.Text.Encoding.UTF8.GetBytes(requestBodyStart.ToString());
            byte[] bodyEnd = System.Text.Encoding.UTF8.GetBytes(requestBodyEnd.ToString());
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(bodyStart, 0, requestBodyStart.ToString().Length);

            // Read the file contents and write them to the request stream.  We read in blocks of 4096 bytes
            byte[] buf = new byte[4096];
            int len;
            while ((len = ms.Read(buf, 0, 4096)) > 0)
            {
                dataStream.Write(buf, 0, len);
            }
            dataStream.Write(bodyEnd, 0, requestBodyEnd.ToString().Length);
            dataStream.Close();
        }

        private HttpWebRequest initializeRequest(string url, string method, string body, string email, string password)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            addRequestHeaders(request, email, password);
            if (body != null)
                addRequestBody(request, body);
            return request;
        }

        private void addRequestHeaders(HttpWebRequest request, string email, string password)
        {
            // authentication header can be in JSON or XML format.  XML used for this walkthrough:
            string authenticateStr =
                "<DocuSignCredentials>" +
                    "<Username>" + email + "</Username>" +
                    "<Password>" + password + "</Password>" +
                    "<IntegratorKey>" + ConfigurationManager.AppSettings["integratorKey"] + "</IntegratorKey>" + // global (not passed)
                    "</DocuSignCredentials>";
            request.Headers.Add("X-DocuSign-Authentication", authenticateStr);
            request.Accept = "application/xml";
            request.ContentType = "application/xml";
        }

        private void addRequestBody(HttpWebRequest request, string requestBody)
        {
            // create byte array out of request body and add to the request object
            byte[] body = System.Text.Encoding.UTF8.GetBytes(requestBody);
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(body, 0, requestBody.Length);
            dataStream.Close();
        }

        private string getResponseBody(HttpWebRequest request)
        {
            // read the response stream into a local string
            try
            {
                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(webResponse.GetResponseStream());
                string responseText = sr.ReadToEnd();
                return responseText;
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    string errorString = "ERROR: " + httpResponse.StatusDescription.ToString() + " in Business.PDFService";

                    //
                    // EXTENDED ERROR DEBUG INFO
                    //
                    /*
                    using (Stream data = response.GetResponseStream())
                    {
                        errorString += new StreamReader(data).ReadToEnd();
                    }
                    */

                    throw new Exception(errorString);
                    //return errorString;
                }
            }
        }

        private string parseDataFromResponse(string response, string searchToken)
        {
            // look for "searchToken" in the response body and parse its value
            using (XmlReader reader = XmlReader.Create(new StringReader(response)))
            {
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == searchToken))
                        return reader.ReadString();
                }
            }
            return null;
        }

        private string prettyPrintXml(string xml)
        {
            // print nicely formatted xml
            try
            {
                XDocument doc = XDocument.Parse(xml);
                return doc.ToString();
            }
            catch (Exception)
            {
                return xml;
            }
        }
        #endregion

        public string GetDaySuffix(int day)
        {
            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }

        public void SubmitPFS(PersonalFinancialStatementTemplateModel model, byte[] doc, int signingUserId)
        {
            var ms = GetPFSPdf(model, doc);

            string xmlBody =
                    "<envelopeDefinition xmlns=\"http://www.docusign.com/restapi\">" +
                    "<emailSubject>DocuSign API - Signature Request on Document</emailSubject>" +
                    "<status>sent</status>" + 	// "sent" to send immediately, "created" to save as draft in your account
                // add document(s)
                    "<documents>" +
                    "<document>" +
                    "<documentId>1</documentId>" +
                    "<name>Personal Finantial Statement</name>" +
                    "</document>" +
                    "</documents>" +
                // add recipient(s)
                    "<recipients>" +
                    "<signers>" +
                    "<signer>" +
                    "<recipientId>1</recipientId>" +
                    "<email>" + model.Email + "</email>" +
                    "<name>" + model.UserFirstName + " " + model.UserLastName + "</name>" +
                    "<tabs>" +
                    "<signHereTabs>" +
                    "<signHere>" +
                    "<xPosition>75</xPosition>" + // default unit is pixels
                    "<yPosition>130</yPosition>" + // default unit is pixels
                    "<documentId>1</documentId>" +
                    "<pageNumber>3</pageNumber>" +
                    "</signHere>" +
                    "<signHere>" +
                    "<xPosition>75</xPosition>" + // default unit is pixels
                    "<yPosition>190</yPosition>" + // default unit is pixels
                    "<documentId>1</documentId>" +
                    "<pageNumber>3</pageNumber>" +
                    "</signHere>" +
                    "</signHereTabs>" +
                    "</tabs>" +
                    "</signer>" +
                    "</signers>" +
                    "</recipients>" +
                    "</envelopeDefinition>";
            saveEnvelope(DocumentType.PersonalFinancialStatement, sendToDocusign(ms, "PersonalFinancialStatement.pdf", xmlBody), signingUserId, null);
        }

        public void SubmitJVAgreement(JVAgreementTemplateModel model, byte[] doc, int signingUserId)
        {
            var ms = GetJVAPdf(model, doc);

            string xmlBody =
                    "<envelopeDefinition xmlns=\"http://www.docusign.com/restapi\">" +
                    "<emailSubject>DocuSign API - Signature Request on Document</emailSubject>" +
                    "<status>sent</status>" + 	// "sent" to send immediately, "created" to save as draft in your account
                // add document(s)
                    "<documents>" +
                    "<document>" +
                    "<documentId>1</documentId>" +
                    "<name>Joint Venture Marketing Partcipation Agreement</name>" +
                    "</document>" +
                    "</documents>" +
                // add recipient(s)
                    "<recipients>" +
                    "<signers>" +
                    "<signer>" +
                    "<recipientId>1</recipientId>" +
                    "<email>" + model.Email + "</email>" +
                    "<name>" + model.UserFirstName + " " + model.UserLastName + "</name>" +
                    "<tabs>" +
                    "<initialHereTabs>" +
                    "<initialHere>" +
                    "<tabLabel>i1</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>122</xPosition>" +
                    "<yPosition>718</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>1</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +
                    "<initialHere>" +
                    "<tabLabel>i2</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>122</xPosition>" +
                    "<yPosition>718</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>2</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +
                    "<initialHere>" +
                    "<tabLabel>i3</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>122</xPosition>" +
                    "<yPosition>718</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>3</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +
                    "</initialHereTabs>" +
                    "<signHereTabs>" +
                    "<signHere>" +
                    "<xPosition>145</xPosition>" + // default unit is pixels
                    "<yPosition>530</yPosition>" + // default unit is pixels
                    "<documentId>1</documentId>" +
                    "<pageNumber>3</pageNumber>" +
                    "</signHere>" +
                    "</signHereTabs>" +
                    "</tabs>" +
                    "</signer>" +
                    "</signers>" +
                    "</recipients>" +
                    "</envelopeDefinition>";
            saveEnvelope(DocumentType.JVAgreement, sendToDocusign(ms, "JVAgreement.pdf", xmlBody), signingUserId, null);
        }

       

        public void GetBitmapImagesFromPDFForLocalComp(byte[] pdf, string folderName)
        {
            string path = "C:\\Data\\EpiFund\\" + folderName;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            using (var doc = new Doc())
            {
                doc.Read(pdf);
                var pix = doc.ObjectSoup.ToList();
                ExtractorContext c = new ExtractorContext();
                var list = new List<Bitmap>();
                int i = 0;
                foreach (var pic in pix)
                {
                    try
                    {
                        if (pic.GetType() != null && pic.GetType().ToString() == "WebSupergoo.ABCpdf9.Objects.PixMap")
                        {
                            string tempPath = path;
                            var extractor = ObjectExtractor.FromIndirectObject(pic, c);
                            //list.Add(extractor.GetBitmap());
                            var image = extractor.GetBitmap();
                            byte[] byteArray = new byte[0];
                            using (MemoryStream stream = new MemoryStream())
                            {
                                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                stream.Close();

                                byteArray = stream.ToArray();
                            }
                            var filename = Guid.NewGuid() + ".jpg";
                            tempPath = Path.Combine(path, filename);
                            File.WriteAllBytes(tempPath, byteArray);
                        }
                    }
                    catch (Exception ex)
                    {
                        // don't break the entire process because of one embedded image that we can't get
                    }
                    i++;
                }
                var q = i;
            }
        }

        public void SubmitLOI(BindingContingentTemplateModel model, byte[] doc, int signingUserId)
        {
            DateTime dt = DateTime.Now;
            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(doc);
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, ms);
            double ied = 0;
            double bed = 0;
            double top = 0;
            if (model.InitialEarnestDeposit != null)
                ied = (double)model.InitialEarnestDeposit;
            if (model.BalanceEarnestDeposit != null)
                bed = (double)model.BalanceEarnestDeposit;
            if (model.TermsOfPurchase != null)
                top = (double)model.TermsOfPurchase;
            model.EscrowCompanyAddress = model.EscrowCompanyAddress + " " +
                model.EscrowCompanyAddress2 + " " +
                model.EscrowCompanyCity + " " +
                model.EscrowCompanyState + " " +
                model.EscrowCompanyZip;
            model.LegalDescription = model.Address1 + " " +
                model.Address2 + " " +
                model.City + " " +
                model.State + " " +
                model.Zip;
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField1[0]", model.To);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField2[0]", model.From);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField3[0]", model.EmailAddress);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField4[0]", model.Date.ToString("MMMM dd, yyyy"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField5[0]", model.FaxNumber);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField6[0]", model.CareOf);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField7[0]", model.Company);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField8[0]", model.TotalNumberOfPagesIncludingCover.ToString());
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField9[0]", model.WorkPhoneNumber);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField10[0]", model.CellPhoneNumber);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField11[0]", model.BusinessPhoneNumber);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField12[0]", model.CREAquisitionLOI);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField13[0]", model.EmailAddress2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField14[0]", model.BeneficiarySeller);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField15[0]", model.OfficePhone);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField16[0]", model.OfficerOfSeller);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField17[0]", model.WebsiteEmail);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField18[0]", model.Buyer1Name);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField19[0]", model.BuyerAssigneeName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField20[0]", model.ObjectOfPurchase);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField21[0]", model.LegalDescription);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField22[0]", model.AssessorNumber);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField23[0]", model.SecuredMortgages != null ? model.SecuredMortgages.ToString() : "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField24[0]", model.Lender);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField25[0]", model.NoSecuredMortgages ? "X" : "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField26[0]", model.OfferingPurchasePrice.ToString("N"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField27[0]", ied.ToString("N"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField28[0]", bed.ToString("N"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField29[0]", top.ToString("N"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField30[0]", model.Releasing);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField31[0]", model.Terms1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField32[0]", model.Terms2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField33[0]", model.Terms3);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField34[0]", model.DueDiligenceDate);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField35[0]", model.DueDiligenceNumberOfDays);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField36[0]", model.SellerDisclosureDate);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField37[0]", model.SellerDisclosureNumberOfDays);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField38[0]", model.OperatingDisclosureDate);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField39[0]", model.OperatingDisclosureNumberOfDays);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField40[0]", model.ClosingDate);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField41[0]", model.ClosingDateNumberOfDays);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField42[0]", model.FormalDocumentationDate);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField43[0]", model.FormalDocumentationNumberOfDays);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField44[0]", model.CommissionFeesName);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField45[0]", model.CommissionFeesNumber);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField46[0]", model.EscrowCompanyName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField47[0]", model.EscrowCompanyAddress);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField48[0]", model.StateOfCountyAssessors);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField49[0]", model.StateOfPropertyTaxOffice);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField50[0]", model.LOIDate.ToString("MMMM dd, yyyy"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField51[0]", model.LOIDate.ToString("MMMM dd, yyyy"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField52[0]", model.Buyer1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField53[0]", model.BuyerTitle1);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField54[0]", model.Buyer2);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField55[0]", model.BuyerTitle2);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField56[0]", model.SellerReceiver1);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField57[0]", model.SellerReceiver2);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField58[0]", model.SellerReceiver1Officer);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField59[0]", model.SellerReceiver2Officer);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField60[0]", model.SellerReceiver1Title);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField61[0]", model.SellerReceiver2Title);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page6[0].TextField62[0]", model.BuyersAssignee1);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page6[0].TextField63[0]", model.BuyersAssignee2);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page6[0].TextField64[0]", model.BuyersAssignee1Officer);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page6[0].TextField65[0]", model.BuyersAssignee2Officer);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page6[0].TextField66[0]", model.BuyersAssignee1Title);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page6[0].TextField67[0]", model.BuyersAssignee2Title);

            pdfMs.FormFlattening = true;
            pdfMs.Writer.CloseStream = false;

            if (pdfMs != null)
            {
                pdfMs.Close();
            }
            ms.Position = 0;
            pdfReader.Close();

            string xmlBody =
                    "<envelopeDefinition xmlns=\"http://www.docusign.com/restapi\">" +
                    "<emailSubject>DocuSign API - Signature Request on Document</emailSubject>" +
                    "<status>sent</status>" + 	// "sent" to send immediately, "created" to save as draft in your account
                // add document(s)
                    "<documents>" +
                    "<document>" +
                    "<documentId>1</documentId>" +
                    "<name>Binding Contingent</name>" +
                    "</document>" +
                    "</documents>" +
                // add recipient(s)
                    "<recipients>" +
                    "<signers>" +
            #region Buyer
 "<signer>" +
                    "<recipientId>1</recipientId>" +
                    "<email>" + model.EmailAddress + "</email>" +
                    "<name>" + model.Buyer1Name + "</name>" +
                    "<tabs>" +

                    "<initialHereTabs>" +
                    "<initialHere>" +
                    "<tabLabel>BuyerInitial</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>466</xPosition>" +
                    "<yPosition>703</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>1</pageNumber>" +
                    "</initialHere>" +

                    "<initialHere>" +
                    "<tabLabel>BuyerInitial</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>466</xPosition>" +
                    "<yPosition>703</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>2</pageNumber>" +
                    "</initialHere>" +

                    "<initialHere>" +
                    "<tabLabel>BuyerInitial</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>466</xPosition>" +
                    "<yPosition>703</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>3</pageNumber>" +
                    "</initialHere>" +

                    "<initialHere>" +
                    "<tabLabel>BuyerInitial</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>466</xPosition>" +
                    "<yPosition>703</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>4</pageNumber>" +
                    "</initialHere>" +

                    "<initialHere>" +
                    "<tabLabel>BuyerInitial</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>466</xPosition>" +
                    "<yPosition>703</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>5</pageNumber>" +
                    "</initialHere>" +

                    //"<initialHere>" +
                //"<tabLabel>BuyerInitial</tabLabel>" +
                //"<recipientId>1</recipientId>" +
                //"<xPosition>466</xPosition>" +
                //"<yPosition>703</yPosition>" +
                //"<documentId>1</documentId>" +
                //"<pageNumber>6</pageNumber>" +
                //"</initialHere>" +

                    "</initialHereTabs>" +


                    "<signHereTabs>" +
                    "<signHere>" +
                    "<xPosition>62</xPosition>" +
                    "<yPosition>265</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>5</pageNumber>" +
                    "<tabLabel>BuyerSig</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "</signHere>" +

                    //"<signHere>" +
                //"<xPosition>313</xPosition>" +
                //"<yPosition>386</yPosition>" +
                //"<documentId>1</documentId>" +
                //"<pageNumber>5</pageNumber>" +
                //"<tabLabel>BuyerSig</tabLabel>" +
                //"<recipientId>1</recipientId>" +
                //"</signHere>" +
                    "</signHereTabs>" +

                    "<dateSignedTabs>" +
                    "<dateSigned>" +
                    "<documentId>1</documentId>" +
                    "<xPosition>222</xPosition>" +
                    "<yPosition>302</yPosition>" +
                    "<pageNumber>5</pageNumber>" +
                    "<recipientId>1</recipientId>" +
                    "<tabLabel>BuyerDateSigned</tabLabel>" +
                    "</dateSigned>" +

                    //"<dateSigned>" +
                //"<documentId>1</documentId>" +
                //"<xPosition>473</xPosition>" +
                //"<yPosition>420</yPosition>" +
                //"<pageNumber>5</pageNumber>" +
                //"<recipientId>1</recipientId>" +
                //"<tabLabel>BuyerDateSigned</tabLabel>" +
                //"</dateSigned>" +
                    "</dateSignedTabs>" +

                    "</tabs>" +
                    "</signer>" +
            #endregion
            #region Buyer's Assignee
                //"<signer>" +
                //"<recipientId>2</recipientId>" +
                //"<email>" + "everywordintheworldistaken@gmail.com" + "</email>" +
                //"<name>" + "Bookied" + " " + "Email" + "</name>" +
                //"<tabs>" +

                    //"<signHereTabs>" +
                //"<signHere>" +
                //"<xPosition>62</xPosition>" +
                //"<yPosition>69</yPosition>" +
                //"<documentId>1</documentId>" +
                //"<pageNumber>6</pageNumber>" +
                //"<tabLabel>BuyerAssigneeSig</tabLabel>" +
                //"<recipientId>2</recipientId>" +
                //"</signHere>" +

                    //"<signHere>" +
                //"<xPosition>313</xPosition>" +
                //"<yPosition>69</yPosition>" +
                //"<documentId>1</documentId>" +
                //"<pageNumber>6</pageNumber>" +
                //"<tabLabel>BuyerAssigneeSig</tabLabel>" +
                //"<recipientId>2</recipientId>" +
                //"</signHere>" +
                //"</signHereTabs>" +

                    //"<dateSignedTabs>" +
                //"<dateSigned>" +
                //"<documentId>1</documentId>" +
                //"<xPosition>62</xPosition>" +
                //"<yPosition>69</yPosition>" +
                //"<pageNumber>6</pageNumber>" +
                //"<recipientId>2</recipientId>" +
                //"<tabLabel>BuyerAssigneeDateSigned</tabLabel>" +
                //"</dateSigned>" +

                    //"<dateSigned>" +
                //"<documentId>1</documentId>" +
                //"<xPosition>313</xPosition>" +
                //"<yPosition>69</yPosition>" +
                //"<pageNumber>6</pageNumber>" +
                //"<recipientId>2</recipientId>" +
                //"<tabLabel>BuyerAssigneeDateSigned</tabLabel>" +
                //"</dateSigned>" +
                //"</dateSignedTabs>" +

                    //"</tabs>" +
                //"</signer>" +
            #endregion
            #region Seller
                //"<signer>" +
                //"<recipientId>3</recipientId>" +
                //"<email>" + "tycoonist925@gmail.com" + "</email>" +
                //"<name>" + "Todd" + " " + "Henderson" + "</name>" +
                //"<tabs>" +

                    //"<initialHereTabs>" +
                //"<initialHere>" +
                //"<tabLabel>SellerInitial</tabLabel>" +
                //"<recipientId>3</recipientId>" +
                //"<xPosition>85</xPosition>" +
                //"<yPosition>703</yPosition>" +
                //"<documentId>1</documentId>" +
                //"<pageNumber>1</pageNumber>" +
                //"</initialHere>" +

                    //"<initialHere>" +
                //"<tabLabel>SellerInitial</tabLabel>" +
                //"<recipientId>3</recipientId>" +
                //"<xPosition>85</xPosition>" +
                //"<yPosition>703</yPosition>" +
                //"<documentId>1</documentId>" +
                //"<pageNumber>2</pageNumber>" +
                //"</initialHere>" +

                    //"<initialHere>" +
                //"<tabLabel>SellerInitial</tabLabel>" +
                //"<recipientId>3</recipientId>" +
                //"<xPosition>85</xPosition>" +
                //"<yPosition>703</yPosition>" +
                //"<documentId>1</documentId>" +
                //"<pageNumber>3</pageNumber>" +
                //"</initialHere>" +

                    //"<initialHere>" +
                //"<tabLabel>SellerInitial</tabLabel>" +
                //"<recipientId>3</recipientId>" +
                //"<xPosition>85</xPosition>" +
                //"<yPosition>703</yPosition>" +
                //"<documentId>1</documentId>" +
                //"<pageNumber>4</pageNumber>" +
                //"</initialHere>" +

                    //"<initialHere>" +
                //"<tabLabel>SellerInitial</tabLabel>" +
                //"<recipientId>3</recipientId>" +
                //"<xPosition>85</xPosition>" +
                //"<yPosition>703</yPosition>" +
                //"<documentId>1</documentId>" +
                //"<pageNumber>5</pageNumber>" +
                //"</initialHere>" +

                    //"<initialHere>" +
                //"<tabLabel>SellerInitial</tabLabel>" +
                //"<recipientId>3</recipientId>" +
                //"<xPosition>85</xPosition>" +
                //"<yPosition>703</yPosition>" +
                //"<documentId>1</documentId>" +
                //"<pageNumber>6</pageNumber>" +
                //"</initialHere>" +
                //"</initialHereTabs>" +


                    //"<signHereTabs>" +
                //"<signHere>" +
                //"<xPosition>62</xPosition>" +
                //"<yPosition>553</yPosition>" +
                //"<documentId>1</documentId>" +
                //"<pageNumber>5</pageNumber>" +
                //"<tabLabel>SellerSig</tabLabel>" +
                //"<recipientId>3</recipientId>" +
                //"</signHere>" +

                    //"<signHere>" +
                //"<xPosition>313</xPosition>" +
                //"<yPosition>553</yPosition>" +
                //"<documentId>1</documentId>" +
                //"<pageNumber>5</pageNumber>" +
                //"<tabLabel>SellerSig</tabLabel>" +
                //"<recipientId>3</recipientId>" +
                //"</signHere>" +
                //"</signHereTabs>" +

                    //"<dateSignedTabs>" +
                //"<dateSigned>" +
                //"<documentId>1</documentId>" +
                //"<xPosition>112</xPosition>" +
                //"<yPosition>553</yPosition>" +
                //"<pageNumber>5</pageNumber>" +
                //"<recipientId>3</recipientId>" +
                //"<tabLabel>BuyerDateSigned</tabLabel>" +
                //"</dateSigned>" +

                    //"<dateSigned>" +
                //"<documentId>1</documentId>" +
                //"<xPosition>363</xPosition>" +
                //"<yPosition>553</yPosition>" +
                //"<pageNumber>5</pageNumber>" +
                //"<recipientId>3</recipientId>" +
                //"<tabLabel>BuyerDateSigned</tabLabel>" +
                //"</dateSigned>" +
                //"</dateSignedTabs>" +

                    //"</tabs>" +
                //"</signer>" +
            #endregion
                    "</signers>" +
                    "</recipients>" +
                    "</envelopeDefinition>";
            saveEnvelope(DocumentType.LOI, sendToDocusign(ms, "BindingContingent.pdf", xmlBody), signingUserId, null);
        }

        public MemoryStream PreviewLOI(BindingContingentTemplateModel model, byte[] doc)
        {
            DateTime dt = DateTime.Now;
            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(doc);
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, ms);
            double ied = 0;
            double bed = 0;
            double top = 0;
            if (model.InitialEarnestDeposit != null)
                ied = (double)model.InitialEarnestDeposit;
            if (model.BalanceEarnestDeposit != null)
                bed = (double)model.BalanceEarnestDeposit;
            if (model.TermsOfPurchase != null)
                top = (double)model.TermsOfPurchase;
            model.EscrowCompanyAddress = model.EscrowCompanyAddress + " " +
                model.EscrowCompanyAddress2 + " " +
                model.EscrowCompanyCity + " " +
                model.EscrowCompanyState + " " +
                model.EscrowCompanyZip;
            model.LegalDescription = model.Address1 + " " +
                model.Address2 + " " +
                model.City + " " +
                model.State + " " +
                model.Zip;
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField1[0]", model.To);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField2[0]", model.From);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField3[0]", model.EmailAddress);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField4[0]", model.Date.ToString("MMMM dd, yyyy"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField5[0]", model.FaxNumber);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField6[0]", model.CareOf);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField7[0]", model.Company);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField8[0]", model.TotalNumberOfPagesIncludingCover.ToString());
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField9[0]", model.WorkPhoneNumber);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField10[0]", model.CellPhoneNumber);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField11[0]", model.BusinessPhoneNumber);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField12[0]", model.CREAquisitionLOI);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField13[0]", model.EmailAddress2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField14[0]", model.BeneficiarySeller);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField15[0]", model.OfficePhone);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField16[0]", model.OfficerOfSeller);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField17[0]", model.WebsiteEmail);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField18[0]", model.Buyer1Name);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField19[0]", model.BuyerAssigneeName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField20[0]", model.ObjectOfPurchase);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField21[0]", model.LegalDescription);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField22[0]", model.AssessorNumber);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField23[0]", model.SecuredMortgages != null ? model.SecuredMortgages.ToString() : "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField24[0]", model.Lender);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField25[0]", model.NoSecuredMortgages ? "X" : "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField26[0]", model.OfferingPurchasePrice.ToString("N"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField27[0]", ied.ToString("N"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField28[0]", bed.ToString("N"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField29[0]", top.ToString("N"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField30[0]", model.Releasing);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField31[0]", model.Terms1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField32[0]", model.Terms2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField33[0]", model.Terms3);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField34[0]", model.DueDiligenceDate);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField35[0]", model.DueDiligenceNumberOfDays);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField36[0]", model.SellerDisclosureDate);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField37[0]", model.SellerDisclosureNumberOfDays);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField38[0]", model.OperatingDisclosureDate);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField39[0]", model.OperatingDisclosureNumberOfDays);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField40[0]", model.ClosingDate);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField41[0]", model.ClosingDateNumberOfDays);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField42[0]", model.FormalDocumentationDate);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField43[0]", model.FormalDocumentationNumberOfDays);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField44[0]", model.CommissionFeesName);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField45[0]", model.CommissionFeesNumber);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField46[0]", model.EscrowCompanyName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField47[0]", model.EscrowCompanyAddress);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField48[0]", model.StateOfCountyAssessors);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField49[0]", model.StateOfPropertyTaxOffice);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField50[0]", model.LOIDate.ToString("MMMM dd, yyyy"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField51[0]", model.LOIDate.ToString("MMMM dd, yyyy"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField52[0]", model.Buyer1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField53[0]", model.BuyerTitle1);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField54[0]", model.Buyer2);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField55[0]", model.BuyerTitle2);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField56[0]", model.SellerReceiver1);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField57[0]", model.SellerReceiver2);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField58[0]", model.SellerReceiver1Officer);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField59[0]", model.SellerReceiver2Officer);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField60[0]", model.SellerReceiver1Title);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField61[0]", model.SellerReceiver2Title);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page6[0].TextField62[0]", model.BuyersAssignee1);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page6[0].TextField63[0]", model.BuyersAssignee2);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page6[0].TextField64[0]", model.BuyersAssignee1Officer);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page6[0].TextField65[0]", model.BuyersAssignee2Officer);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page6[0].TextField66[0]", model.BuyersAssignee1Title);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page6[0].TextField67[0]", model.BuyersAssignee2Title);

            pdfMs.FormFlattening = true;
            pdfMs.Writer.CloseStream = false;

            if (pdfMs != null)
            {
                pdfMs.Close();
            }
            ms.Position = 0;
            pdfReader.Close();

            return ms;
        }

        #region Websupergoo Extractor Context Helper
        // ===========================================================================
        //	©2013 WebSupergoo. All rights reserved.
        // ===========================================================================

        public sealed class ExtractorContext
        {
            private static readonly string[] _aActionEntryList = {
			"O", "C", "K", "F", "V", "C",
			"E", "X", "D", "U", "Fo", "Bl", "PO", "PC", "PV", "PI",
			"WC", "WS", "DS", "WP", "DP"
		};
            private static readonly string[] _aActionSearchList;

            static ExtractorContext()
            {
                _aActionSearchList = new string[_aActionEntryList.Length];
                for (int i = 0; i < _aActionEntryList.Length; ++i)
                    _aActionSearchList[i] = ',' + _aActionEntryList[i] + ',';
            }

            private List<int> _pageContentList;
            private List<int> _cmapList;
            private SortedDictionary<int, string> _xfaDict;
            private SortedDictionary<int, ScriptType> _scriptDict;
            private SortedDictionary<int, string> _type3GlyphDict;

            public ExtractorContext()
            {
                _pageContentList = new List<int>();
                _cmapList = new List<int>();
                _xfaDict = new SortedDictionary<int, string>();
                _scriptDict = new SortedDictionary<int, ScriptType>();
                _type3GlyphDict = new SortedDictionary<int, string>();
            }

            public List<int> PageContents { get { return _pageContentList; } }
            public List<int> CMaps { get { return _cmapList; } }
            public SortedDictionary<int, string> Xfa { get { return _xfaDict; } }
            public SortedDictionary<int, ScriptType> Scripts { get { return _scriptDict; } }
            public SortedDictionary<int, string> Type3Glyphs { get { return _type3GlyphDict; } }

            public void AddXfa(int id, string name)
            {
                string oldName;
                if (_xfaDict.TryGetValue(id, out oldName))
                {
                    if (oldName == name)
                        return;
                    if (name != null)
                    {
                        if (oldName == "")
                            return;
                        name = "";
                    }
                }
                _xfaDict[id] = name;
            }

            public void AddScript(int id, ScriptType type)
            {
                ScriptType oldType;
                if (_scriptDict.TryGetValue(id, out oldType))
                    oldType.Add(type);
                else
                    _scriptDict[id] = type;
            }

            public void AddAction(Doc doc, int id, ScriptType type)
            {
                int jsId = doc.GetInfoInt(id, "/A*/JS:Ref");
                if (jsId != 0 && doc.GetInfo(id, "/A*/S*:Name") == "JavaScript")
                    AddScript(jsId, type);
            }

            public void AddAdditionalAction(Doc doc, int id, ScriptType type)
            {
                string keys = doc.GetInfo(id, "/AA*:Keys");
                if (keys == "")
                    return;
                keys = ',' + keys + ',';
                for (int i = 0; i < _aActionSearchList.Length; ++i)
                {
                    if (keys.IndexOf(_aActionSearchList[i]) >= 0)
                    {
                        string path = "/AA*/" + _aActionEntryList[i];
                        int jsId = doc.GetInfoInt(id, path + "*/JS:Ref");
                        if (jsId != 0 && doc.GetInfo(id, path + "*/S*:Name") == "JavaScript")
                            AddScript(jsId, type);
                    }
                }
            }

            public void AddType3Glyph(Doc doc, int id)
            {
                string s = doc.GetInfo(id, "/CharProcs*:Keys");
                if (s != "")
                {
                    string[] keys = s.Split(new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries);
                    foreach (string name in keys)
                    {
                        int glyphId = doc.GetInfoInt(id, "/CharProcs*/" + name + ":Ref");
                        if (glyphId != 0)
                        {
                            string oldName;
                            if (_type3GlyphDict.TryGetValue(glyphId, out oldName))
                            {
                                if (oldName == name || oldName == "")
                                    continue;
                                _type3GlyphDict[glyphId] = "";
                            }
                            else
                                _type3GlyphDict[glyphId] = name;
                        }
                    }
                }
            }
        }

        public enum ImageState { None, Loading, Error }

        [StructLayout(LayoutKind.Auto)]
        public struct ImageAndState
        {
            public Image Image;
            public ImageState State;

            public ImageAndState(Image image, ImageState state)
            {
                Image = image;
                State = state;
            }
        }

        public sealed class ScriptType
        {
            public string DocumentName;
            public string PageName;
            public string AnnotationName;

            public static ScriptType NewDocument(string name)
            {
                ScriptType v = new ScriptType();
                v.DocumentName = name;
                return v;
            }
            public static ScriptType NewPage(string name)
            {
                ScriptType v = new ScriptType();
                v.PageName = name;
                return v;
            }
            public static ScriptType NewAnnotation(string name)
            {
                ScriptType v = new ScriptType();
                v.AnnotationName = name;
                return v;
            }

            public void Add(ScriptType v)
            {
                if (DocumentName != v.DocumentName)
                    DocumentName = DocumentName == null ? v.DocumentName : "";
                if (PageName != v.PageName)
                    PageName = PageName == null ? v.PageName : "";
                if (AnnotationName != v.AnnotationName)
                    AnnotationName = AnnotationName == null ? v.AnnotationName : "";
            }
        }

        public class ObjectExtractor
        {
            public delegate void ImageLoadCompleteDelegate(
                ObjectExtractor extractor, ImageAndState imageState);

            public static ObjectExtractor FromIndirectObject(IndirectObject obj, ExtractorContext context)
            {
                if (obj == null)
                    throw new ArgumentNullException("obj", "IndirectObject obj cannot be null.");

                if (obj is StreamObject)
                    return StreamObjectExtractor.FromStreamObject((StreamObject)obj);
                if (obj is Annotation)
                    return new AnnotationExtractor((Annotation)obj, context);
                if (obj is Page)
                    return new PageExtractor((Page)obj, context);
                if (obj is Pages)
                    return new PagesExtractor((Pages)obj);
                if (obj is FontObject)
                    return new FontObjectExtractor((FontObject)obj, context);
                if (obj is Field)
                    return FieldExtractor.FromField((Field)obj);
                if (obj is GraphicsState)
                    return new GraphicsStateExtractor((GraphicsState)obj);
                if (obj is Bookmark)
                    return BookmarkExtractor.FromBookmark((Bookmark)obj);
                if (obj is ColorSpace)
                    return new ColorSpaceExtractor((ColorSpace)obj);
                if (obj is Catalog)
                    return new CatalogExtractor((Catalog)obj, context);

                return new ObjectExtractor(obj);
            }

            private IndirectObject _obj;

            public ObjectExtractor(IndirectObject obj)
            {
                _obj = obj;
            }

            public IndirectObject Object { get { return _obj; } }

            public virtual bool IsAscii { get { return true; } }

            public virtual void Invalidate() { }

            public string GetValue(Doc doc)
            {
                if ((doc == null) || (_obj == null))
                    return "";
                return doc.GetInfo(_obj.ID, "value");
            }

            public virtual void SetValue(Doc doc, string value)
            {
                if ((doc == null) || (_obj == null))
                    return;
                int id = _obj.ID;
                doc.SetInfo(id, "value", value);
                _obj = doc.ObjectSoup[id];
                Invalidate();
            }

            public virtual string[] GetInfo()
            {
                return new string[] { GetIDString(), GetTypeName() };
            }

            public string[] GetErrorInfo(string message)
            {
                return new string[] { GetIDString(), GetTypeName(), "(Error!)", "", message };
            }

            protected string GetIDString() { return _obj.ID.ToString(); }
            protected string GetTypeName() { return _obj.GetType().Name; }

            public virtual ImageAndState GetImage()
            {
                return new ImageAndState(GetBitmap(), ImageState.None);
            }
            public virtual void BeginLoadImage(ImageLoadCompleteDelegate imageLoadComplete)
            {
                throw new NotSupportedException("BeginLoadImage is not supported.");
            }
            public virtual Bitmap GetBitmap() { return null; }
            public virtual string GetAtom() { return Object.Atom.ToString(); }
            public virtual string GetContent() { return null; }

            protected static string FormatList(string[] list)
            {
                if (list == null || list.Length <= 0)
                    return "";

                StringBuilder builder = new StringBuilder();
                builder.Append(list[0]);
                for (int i = 1; i < list.Length; ++i)
                    builder.Append(", ").Append(list[i]);

                return builder.ToString();
            }
        }

        public class CatalogExtractor : ObjectExtractor
        {
            public CatalogExtractor(Catalog obj, ExtractorContext context)
                : base(obj)
            {
                Doc doc = obj.Doc;
                int id = obj.ID;
                int treeId = doc.GetInfoInt(id, "/Names*/JavaScript:Ref");
                if (treeId != 0)
                    AddScriptInTree(doc, context, treeId);

                int count = doc.GetInfoInt(id, "/AcroForm*/XFA*:Count");
                if (count > 0)
                {
                    for (int i = 0; i < count; i += 2)
                    {
                        context.AddXfa(doc.GetInfoInt(id,
                            "/AcroForm*/XFA*[" + (i + 1).ToString() + "]:Ref"),
                            doc.GetInfo(id, "/AcroForm*/XFA*[" + i.ToString() + "]*:Text"));
                    }
                }
                else
                {
                    int xfaId = doc.GetInfoInt(id, "/AcroForm*/XFA:Ref");
                    if (xfaId != 0)
                        context.AddXfa(xfaId, null);
                }
            }
            private static void AddScriptInTree(Doc doc, ExtractorContext context, int nodeId)
            {
                int count = doc.GetInfoInt(nodeId, "/Kids*:Count");
                if (count > 0)
                {
                    for (int i = 0; i < count; ++i)
                    {
                        int childNodeId = doc.GetInfoInt(nodeId, "/Kids*[" + i.ToString() + "]:Ref");
                        if (childNodeId != 0)
                            AddScriptInTree(doc, context, childNodeId);    // recur on childNodeID
                    }
                    return;
                }

                count = doc.GetInfoInt(nodeId, "/Names*:Count");
                for (int i = 0; i < count; i += 2)
                {
                    string path = "/Names*[" + (i + 1).ToString();
                    int jsId = doc.GetInfoInt(nodeId, path + "]*/JS:Ref");
                    if (jsId != 0 && doc.GetInfo(nodeId, path + "]*/S*:Name") == "JavaScript")
                    {
                        context.AddScript(jsId, ScriptType.NewDocument(
                            doc.GetInfo(nodeId, "/Names*[" + i.ToString() + "]*:Text")));
                    }
                }
            }

            public new Catalog Object { get { return (Catalog)base.Object; } }

            public override string[] GetInfo()
            {
                Catalog obj = Object;
                Outline outline = obj.Outline;
                Pages pages = obj.Pages;
                return new string[]{ GetIDString(), GetTypeName(), "", "",
				string.Format("Outline ID:[{0}] Pages ID:[{1}]",
					outline==null? "": outline.ID.ToString(),
					pages==null? "": pages.ID.ToString()) };
            }
        }

        public class ColorSpaceExtractor : ObjectExtractor
        {
            public ColorSpaceExtractor(ColorSpace obj) : base(obj) { }

            public new ColorSpace Object { get { return (ColorSpace)base.Object; } }

            public override string[] GetInfo()
            {
                ColorSpace obj = Object;
                IccProfile iccProfile = obj.IccProfile;
                return new string[]{ GetIDString(), GetTypeName(), obj.Name,
				obj.ColorSpaceType.ToString(),
				string.Format("Components:[{0}] IccProfile ID:[{1}]",
					obj.Components, iccProfile==null? "": iccProfile.ID.ToString()) };
            }
        }

        public class BookmarkExtractor : ObjectExtractor
        {
            public static BookmarkExtractor FromBookmark(Bookmark obj)
            {
                if (obj is Outline)
                    return new OutlineExtractor((Outline)obj);

                return new BookmarkExtractor(obj);
            }

            public BookmarkExtractor(Bookmark obj) : base(obj) { }

            public new Bookmark Object { get { return (Bookmark)base.Object; } }

            public override string[] GetInfo()
            {
                Bookmark obj = Object;
                Bookmark parent = obj.Parent;
                return new string[]{ GetIDString(), GetTypeName(), "", obj.Title,
				string.Format("PageID:[{0}] Parent ID:[{1}] Count:[{2}] Open:[{3}]",
				obj.PageID, parent==null? "": parent.ID.ToString(),
				obj.Count, obj.Open) };
            }
        }

        public class OutlineExtractor : BookmarkExtractor
        {
            public OutlineExtractor(Outline obj) : base(obj) { }

            public new Outline Object { get { return (Outline)base.Object; } }
        }

        public class GraphicsStateExtractor : ObjectExtractor
        {
            public GraphicsStateExtractor(GraphicsState obj) : base(obj) { }

            public new GraphicsState Object { get { return (GraphicsState)base.Object; } }
        }

        public class FieldExtractor : ObjectExtractor
        {
            public static FieldExtractor FromField(Field obj)
            {
                if (obj is Signature)
                    return new SignatureExtractor((Signature)obj);

                return new FieldExtractor(obj);
            }

            public FieldExtractor(Field obj) : base(obj) { }

            public new Field Object { get { return (Field)base.Object; } }

            public override string[] GetInfo()
            {
                Field obj = Object;
                return new string[]{ GetIDString(), GetTypeName(),
				obj.Name, obj.Value,
				string.Format("FieldType:[{0}] Format:[{1}] MultiSelect:[{2}] Options[{3}]",
					obj.FieldType, obj.Format, obj.MultiSelect,
					FormatList(obj.Options)) };
            }
        }

        public class SignatureExtractor : FieldExtractor
        {
            public SignatureExtractor(Signature obj) : base(obj) { }

            public new Signature Object { get { return (Signature)base.Object; } }

            public override string[] GetInfo()
            {
                Signature obj = Object;
                return new string[]{ GetIDString(), GetTypeName(), "",
				obj.Signer, string.Format(
				"IsModified:[{0}] SigningUtcTime:[{1}] Location:[{2}] Reason[{3}]",
				obj.IsModified, obj.SigningUtcTime, obj.Location, obj.Reason) };
            }
        }

        public class FontObjectExtractor : ObjectExtractor
        {
            public FontObjectExtractor(FontObject obj, ExtractorContext context)
                : base(obj)
            {
                Doc doc = obj.Doc;
                int id = obj.ID;
                int cmapId = doc.GetInfoInt(id, "/ToUnicode:Ref");
                if (cmapId != 0)
                    context.CMaps.Add(cmapId);
                context.AddType3Glyph(doc, id);
            }

            public new FontObject Object { get { return (FontObject)base.Object; } }
        }

        public class AnnotationExtractor : ObjectExtractor
        {
            public AnnotationExtractor(Annotation obj, ExtractorContext context)
                : base(obj)
            {
                Doc doc = obj.Doc;
                int id = obj.ID;
                ScriptType scryptType = ScriptType.NewAnnotation(id.ToString());
                context.AddAction(doc, id, scryptType);
                context.AddAdditionalAction(doc, id, scryptType);
            }

            public new Annotation Object { get { return (Annotation)base.Object; } }

            public override string[] GetInfo()
            {
                Annotation obj = Object;
                return new string[]{ GetIDString(), GetTypeName(),
				obj.FullName, obj.SubType,
				string.Format("FieldType:[{0}] FieldValue:[{1}] Contents:[{2}]",
					obj.FieldType, obj.FieldValue, obj.Contents) };
            }
        }

        public class PageExtractor : ObjectExtractor
        {
            private string _atom;
            private int _pageNum;

            public PageExtractor(Page obj, ExtractorContext context)
                : base(obj)
            {
                _atom = obj.Atom.ToString();
                Doc doc = obj.Doc;
                int id = obj.ID;
                doc.Page = id;
                if (doc.Page == id)
                    _pageNum = doc.PageNumber;

                context.AddAdditionalAction(doc, id, ScriptType.NewPage(id.ToString()));

                int count = doc.GetInfoInt(id, "/Contents*:Count");
                if (count > 0)
                {
                    for (int i = 0; i < count; ++i)
                        context.PageContents.Add(doc.GetInfoInt(id,
                            "/Contents*[" + i.ToString() + "]:Ref"));
                }
                else
                {
                    int contentId = doc.GetInfoInt(id, "/Contents:Ref");
                    if (contentId != 0)
                        context.PageContents.Add(contentId);
                }
            }

            public new Page Object { get { return (Page)base.Object; } }

            public override string[] GetInfo()
            {
                Page obj = Object;
                Doc doc = obj.Doc;
                int id = obj.ID;
                doc.Page = id;
                Pages parent = obj.Parent;
                return new string[] { 
				GetIDString(), 
				GetTypeName(),
				_pageNum==0? "": "Page "+_pageNum.ToString(), "",
				string.Format("Rotation:[{0}] Parent ID:[{1}]", obj.Rotation, parent==null? "": parent.ID.ToString()) 
			};
            }

            public override ImageAndState GetImage()
            {
                return new ImageAndState(null, _pageNum == 0 ? ImageState.None : ImageState.Loading);
            }

            public override void BeginLoadImage(ImageLoadCompleteDelegate imageLoadComplete)
            {
                Page obj = Object;
                Doc doc = obj.Doc;
                int id = obj.ID;
                doc.Page = id;
                doc.Rect.SetRect(doc.MediaBox);
                double dimension = Math.Max(doc.MediaBox.Width, doc.MediaBox.Height);
                doc.Rendering.DotsPerInch = dimension < 850 ? 72 : 72 * 850 / dimension;
                RenderOperation operation = null;
                try
                {
                    operation = new RenderOperation(doc);
                    ThreadPool.QueueUserWorkItem(delegate(object state)
                    {
                        Image image = null;
                        try
                        {
                            image = operation.GetBitmap();
                        }
                        catch
                        {
                        }
                        finally
                        {
                            operation.Dispose();
                        }
                        imageLoadComplete(this, new ImageAndState(image, image == null ? ImageState.Error : ImageState.None));
                    });
                }
                catch
                {
                    if (operation != null)
                        operation.Dispose();
                    throw;
                }
            }

            public override Bitmap GetBitmap()
            {
                Page obj = Object;
                Doc doc = obj.Doc;
                int id = obj.ID;
                doc.Page = id;
                if (doc.Page != id)
                    return null;

                doc.Rect.SetRect(doc.MediaBox);
                return doc.Rendering.GetBitmap();
            }

            public override string GetAtom() { return _atom; }
        }

        public class PagesExtractor : ObjectExtractor
        {
            public PagesExtractor(Pages obj) : base(obj) { }

            public new Pages Object { get { return (Pages)base.Object; } }

            public override string[] GetInfo()
            {
                Pages obj = Object;
                Pages parent = obj.Parent;
                return new string[]{ GetIDString(), GetTypeName(), "", "",
				string.Format("Count:[{0}] Parent ID:[{1}]", obj.Count, parent==null? "": parent.ID.ToString()) };
            }
        }

        public class StreamObjectExtractor : ObjectExtractor
        {
            public static StreamObjectExtractor FromStreamObject(StreamObject obj)
            {
                if (obj is PixMap)
                    return new PixMapExtractor((PixMap)obj);
                if (obj is Layer)
                    return LayerExtractor.FromLayer((Layer)obj);
                if (obj is IccProfile)
                    return new IccProfileExtractor((IccProfile)obj);

                return new StreamObjectExtractor(obj);
            }

            private string _atom;
            private string _streamType;	// only if is text

            public StreamObjectExtractor(StreamObject obj) : base(obj) { }

            public new StreamObject Object { get { return (StreamObject)base.Object; } }

            public override bool IsAscii
            {
                get
                {
                    StreamObject so = Object;
                    if (so == null) return true;
                    byte[] data = so.GetData();
                    for (int i = 0; i < data.Length; i++)
                    {
                        if ((data[i] >= 32) && (data[i] < 128))
                            continue;
                        if ((data[i] != '\r') && (data[i] != '\n') && (data[i] != '\t'))
                            return false;
                    }
                    return base.IsAscii;
                }
            }

            public string StreamType
            {
                get { return _streamType; }
                set { _streamType = value; }
            }

            public override void Invalidate()
            {
                _atom = null;
                _streamType = null;
                base.Invalidate();
            }

            public override string[] GetInfo()
            {
                UpdateStreamType();
                StreamObject obj = Object;
                return new string[]{ GetIDString(), GetTypeName(), _streamType,
				obj.Compression.ToString(),
				string.Format("Length:[{0}]", obj.Length) };
            }

            private void UpdateStreamType()
            {
                if (_streamType != null)
                    return;

                StreamObject obj = Object;
                Doc doc = obj.Doc;
                int id = obj.ID;
                string subtype = doc.GetInfo(id, "/Subtype*:Name");
                if (subtype == "Form" || subtype == "PS")
                {
                    string type = subtype == "PS"
                        || doc.GetInfo(id, "/Subtype2*:Name") == "PS" ?
                        "PostScript XObject" : "Form XObject";
                    string name = doc.GetInfo(id, "/Name*:Name");
                    _streamType = name == "" ? type :
                        string.Format("{0}[{1}]", type, name);
                }
                else if (subtype == "XML")
                    _streamType = doc.GetInfo(id, "/Type*:Name") == "Metadata" ?
                        "XML/Metadata" : "XML";
                else if (doc.GetInfo(id, "/PatternType") != "")
                    _streamType = "Pattern";
                else if (doc.GetInfoInt(id, "/FunctionType*:Num") == 4)
                    _streamType = "Function/Type-4";
                else
                    _streamType = "";
            }

            private bool ContainsText()
            {
                StreamObject obj = Object;
                CompressionType[] comps = obj.Compressions;
                CompressionType comp = comps.Length > 0 ? comps[0] : CompressionType.None;
                return (comp == CompressionType.AsciiHex) || (comp == CompressionType.Ascii85);
            }

            public override string GetAtom()
            {
                if (_atom == null)
                    _atom = base.GetAtom();
                return _atom;
            }

            public override string GetContent()
            {
                UpdateStreamType();
                if (_streamType != "")
                {
                    if (_atom == null)
                        _atom = base.GetAtom();
                    StreamObject obj = Object;
                    obj.Decompress();
                    return obj.GetText();
                }
                if (ContainsText())
                {
                    return Object.GetText();
                }
                return base.GetContent();
            }
        }

        public class IccProfileExtractor : StreamObjectExtractor
        {
            public IccProfileExtractor(IccProfile obj) : base(obj) { }

            public new IccProfile Object { get { return (IccProfile)base.Object; } }
        }

        public class PixMapExtractor : StreamObjectExtractor
        {
            private static readonly string[] _colorSpaceFormat = {
			"{0}\u2014{1} comp \xd7 {2} bit",
			"{0}\u2014{1} comp \xd7 {2} bits",
			"{0}\u2014{1} comps \xd7 {2} bit",
			"{0}\u2014{1} comps \xd7 {2} bits"
		};

            public PixMapExtractor(PixMap obj) : base(obj) { }

            public new PixMap Object { get { return (PixMap)base.Object; } }

            public override string[] GetInfo()
            {
                PixMap obj = Object;
                int components = obj.Components;
                int bitsPerComponent = obj.BitsPerComponent;
                return new string[]{ GetIDString(), GetTypeName(),
				obj.Doc.GetInfo(obj.ID, "/Name*:Name"),
				string.Format("{0}\xd7{1} {2}", obj.Width, obj.Height,
					obj.Compression),
				string.Format(_colorSpaceFormat[
					(components<=1? 0: 2)+(bitsPerComponent<=1? 0: 1)],
					obj.ColorSpaceType, components, bitsPerComponent) };
            }

            public override Bitmap GetBitmap()
            {
                return Object.GetBitmap();
            }
        }

        public class LayerExtractor : StreamObjectExtractor
        {
            public static LayerExtractor FromLayer(Layer obj)
            {
                return new LayerExtractor(obj);
            }

            public LayerExtractor(Layer obj) : base(obj) { }

            public new Layer Object { get { return (Layer)base.Object; } }

            public override string[] GetInfo()
            {
                Layer obj = Object;
                return new string[]{ GetIDString(), GetTypeName(), "",
				obj.Compression.ToString(),
				string.Format("Length:[{0}] Rect:[{1}]",
					obj.Length, obj.Rect.String) };
            }
        }

        #endregion


        public MemoryStream GetNCNDPdf(NCNDTemplateModel model, byte[] doc)
        {
            return stampNCNDPDF(doc, model);
        }

        public MemoryStream GetPFSPdf(PersonalFinancialStatementTemplateModel model, byte[] doc)
        {
            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(doc);
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, ms);

            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].asof[0]", DateTime.Now.ToString("MMMM"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].asofDate[0]", DateTime.Now.ToString("yyyy"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].name[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].businessPhone[0]", model.BusinessPhone);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].residentialAddress[0]", model.ResidentialAddress);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].residentialPhone[0]", model.ResidentialPhone);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].cityStateZip[0]", model.City + " " + model.State + " " + model.Zip);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].businessNameOfApplicant[0]", model.BusinessName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].cashOnHand[0]", model.CashOnHand);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].savingsAccount[0]", model.SavingsAccount);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].iraOrOtherRetirementAccount[0]", model.OtherRetirementOrIraAccount);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].accountsAndNotesReceivable[0]", model.AccountsRecievable);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].lifeInsurance-CashSurrenderValue[0]", model.LifeInsurance);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].stocksAndBonds[0]", model.StocksAndBonds);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].realEstate[0]", model.RealEstate);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].automobilesTotalPresentValue[0]", model.Automobiles);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].otherPersonalProperty[0]", model.OtherProperty);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].otherAssets[0]", model.OtherAssets);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].totalLeft[0]", model.AssetsTotal);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].sourceOfIncomeSalary[0]", model.Salary);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].sourceOfIncomeNetInvestment[0]", model.NetInvestmentIncome);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].sourceOfIncomeRealEstate[0]", model.RealEstateIncome);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].sourceOfIncomeOther[0]", model.OtherIncome);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].accountsPayable[0]", model.AccountsPayable);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].notesPayableToBanksAndOthers[0]", model.NotesPayableToOthers);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].installmentAccountAuto[0]", model.InstallmentAccountAuto);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].installmentAccountAutoMontlyPayment[0]", model.InstallmentAccountAutoMonthlyPayment);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].installmentAccountOther[0]", model.InstallmentAccountOther);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].installmentAccountOtherMonthlyPayment[0]", model.InstallmentAccountOtherMonthlyPayment);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].loanOnLifeInsurance[0]", model.LifeInsuranceLoan);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].mortgagesonRealEstate[0]", model.RealEstateMortgage);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].unpaidTaxes[0]", model.LiabilitiesUnpaidTaxes);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].otherLiabilities[0]", model.LiabilitiesOtherLiabilities);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].totalLiabilities[0]", model.LiabilitiesTotalLiabilities);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].netWorth[0]", model.NetWorth);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].rightTotal[0]", model.LiabilitiesTotal);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].liabilitiesAsEndorserOrCoMaker[0]", model.EndorserOrCoMaker);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].liablitiesLegalClaimsAndJudgments[0]", model.LegalClaimsOrJudgments);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].liabilitiesProvisionForFederalTaxIncome[0]", model.FederalIncomeTax);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].liabilitiesOtherSpecialDebt[0]", model.OtherSpecialDebt);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].descriptionOfOtherIncome1[0]", model.OtherIncomeDescription1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].descriptionOfOtherIncome2[0]", model.OtherIncomeDescription2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].descriptionOfOtherIncome3[0]", model.OtherIncomeDescription3);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].nameAndAddrOfNoteHolders1[0]", model.NameAndAddressOfNoteholders1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].nameAndAddrOfNoteHolders2[0]", model.NameAndAddressOfNoteholders2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].nameAndAddrOfNoteHolders3[0]", model.NameAndAddressOfNoteholders3);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].nameAndAddrOfNoteHolders4[0]", model.NameAndAddressOfNoteholders4);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].nameAndAddrOfNoteHolders5[0]", model.NameAndAddressOfNoteholders5);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].originalBalance1[0]", model.OriginalBalance1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].originalBalance2[0]", model.OriginalBalance2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].originalBalance3[0]", model.OriginalBalance3);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].originalBalance4[0]", model.OriginalBalance4);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].originalBalance5[0]", model.OriginalBalance5);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].currentBalance1[0]", model.CurrentBalance1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].currentBalance2[0]", model.CurrentBalance2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].currentBalance3[0]", model.CurrentBalance3);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].currentBalance4[0]", model.CurrentBalance4);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].currentBalance5[0]", model.CurrentBalance5);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].paymentAmount1[0]", model.PaymentAmount1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].paymentAmount2[0]", model.PaymentAmount2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].paymentAmount3[0]", model.PaymentAmount3);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].paymentAmount4[0]", model.PaymentAmount4);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].paymentAmount5[0]", model.PaymentAmount5);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].frequency1[0]", model.Frequency1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].frequency2[0]", model.Frequency2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].frequency3[0]", model.Frequency3);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].frequency4[0]", model.Frequency4);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].frequency5[0]", model.Frequency5);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].hosSecuredOrEndorsedTypeOfCollateral1[0]", model.TypeOfCollateral1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].hosSecuredOrEndorsedTypeOfCollateral2[0]", model.TypeOfCollateral2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].hosSecuredOrEndorsedTypeOfCollateral3[0]", model.TypeOfCollateral3);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].hosSecuredOrEndorsedTypeOfCollateral4[0]", model.TypeOfCollateral4);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].hosSecuredOrEndorsedTypeOfCollateral5[0]", model.TypeOfCollateral5);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].StocksAndBondsNumberOfShares1[0]", model.StocksAndBondsNumberOfShares1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].StocksAndBondsNumberOfShares2[0]", model.StocksAndBondsNumberOfShares2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].StocksAndBondsNumberOfShares3[0]", model.StocksAndBondsNumberOfShares3);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].StocksAndBondsNumberOfShares4[0]", model.StocksAndBondsNumberOfShares4);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].StocksAndBondsNameOfSecurities1[0]", model.StocksAndBondsNameOfSecurities1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].StocksAndBondsNameOfSecurities2[0]", model.StocksAndBondsNameOfSecurities2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].StocksAndBondsNameOfSecurities3[0]", model.StocksAndBondsNameOfSecurities3);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].StocksAndBondsNameOfSecurities4[0]", model.StocksAndBondsNameOfSecurities4);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsCost1[0]", model.StocksAndBondsCost1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsCost2[0]", model.StocksAndBondsCost2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsCost3[0]", model.StocksAndBondsCost3);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsCost4[0]", model.StocksAndBondsCost4);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsMarketValueQuotation-Exchange1[0]", model.StocksAndBondsMarketValue1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsMarketValueQuotation-Exchange2[0]", model.StocksAndBondsMarketValue2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsMarketValueQuotation-Exchange3[0]", model.StocksAndBondsMarketValue3);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsMarketValueQuotation-Exchange4[0]", model.StocksAndBondsMarketValue4);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsDateOfQuotation-Exchange1[0]", model.StocksAndBondsDateOfExchange1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsDateOfQuotation-Exchange2[0]", model.StocksAndBondsDateOfExchange2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsDateOfQuotation-Exchange3[0]", model.StocksAndBondsDateOfExchange3);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsDateOfQuotation-Exchange4[0]", model.StocksAndBondsDateOfExchange4);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsTotalValue1[0]", model.StocksAndBondsTotalValue1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsTotalValue2[0]", model.StocksAndBondsTotalValue2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsTotalValue3[0]", model.StocksAndBondsTotalValue3);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsTotalValue4[0]", model.StocksAndBondsTotalValue4);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].properyATypeOfRealEstate[0]", model.PropertyATypeOfRealEstate);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].properyBTypeOfRealEstate[0]", model.PropertyBTypeOfRealEstate);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].properyCTypeOfRealEstate[0]", model.PropertyCTypeOfRealEstate);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertAAddress[0]", model.PropertyAAddress);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertBAddress[0]", model.PropertyBAddress);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertCAddress[0]", model.PropertyCAddress);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyADatePurchased[0]", model.PropertyADatePurchased);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyBDatePurchased[0]", model.PropertyBDatePurchased);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyCDatePurchased[0]", model.PropertyCDatePurchased);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyAOriginalCost[0]", model.PropertyAOriginalCost);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyBOriginalCost[0]", model.PropertyBOriginalCost);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyCOriginalCost[0]", model.PropertyCOriginalCost);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyAPresentMarketValue[0]", model.PropertyAPresentMarketValue);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyBPresentMarketValue[0]", model.PropertyBPresentMarketValue);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyCPresentMarketValue[0]", model.PropertyCPresentMarketValue);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyANameAndAddrOfMortgageHolder[0]", model.PropertyANameAndAddressOfMortgageHolder);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyBNameAndAddrOfMortgageHolder[0]", model.PropertyBNameAndAddressOfMortgageHolder);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyCNameAndAddrOfMortgageHolder[0]", model.PropertyCNameAndAddressOfMortgageHolder);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyAMortgageAccountNumber[0]", model.PropertyAMortgageAccountNumber);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyBMortgageAccountNumber[0]", model.PropertyBMortgageAccountNumber);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyCMortgageAccountNumber[0]", model.PropertyCMortgageAccountNumber);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyAMortgageBalance[0]", model.PropertyAMortgageBalance);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyBMortgageBalance[0]", model.PropertyBMortgageBalance);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyCMortgageBalance[0]", model.PropertyCMortgageBalance);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyAAmountOfPaymentPerMonth-Year[0]", model.PropertyAAmountOfPaymentRecurring);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyBAmountOfPaymentPerMonth-Year[0]", model.PropertyBAmountOfPaymentRecurring);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyCAmountOfPaymentPerMonth-Year[0]", model.PropertyCAmountOfPaymentRecurring);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyAStatusOfMortgage[0]", model.PropertyAMortgageStatus);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyBStatusOfMortgage[0]", model.PropertyBMortgageStatus);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyCStatusOfMortgage[0]", model.PropertyCMortgageStatus);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].otherPersonalPropertyAndOtherAssets[0]", model.OtherPropertyAssets);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].unpaidTaxes[0]", model.UnpaidTaxes);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].otherLiabilities[0]", model.OtherLiabilities);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].lifeInsuranceHeld[0]", model.LifeInsuranceHeld);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].printName1[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].date1[0]", DateTime.Now.ToString("MM/dd/yyyy"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].ssn1[0]", model.SocialSecurityNumber);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].printName2[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].date2[0]", DateTime.Now.ToString("MM/dd/yyyy"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].ssn2[0]", model.SocialSecurityNumber);

            pdfMs.FormFlattening = true;
            pdfMs.Writer.CloseStream = false;

            if (pdfMs != null)
            {
                pdfMs.Close();
            }
            ms.Position = 0;
            pdfReader.Close();
            return ms;
        }

        public MemoryStream GetJVAPdf(JVAgreementTemplateModel model, byte[] doc)
        {
            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(doc);
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, ms);

            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField1[0]", model.Day.ToString() + GetDaySuffix(model.Day));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField2[0]", model.Month);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField3[0]", model.Year.ToString().Substring(2, 2));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField4[0]", model.CompanyName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField5[0]", model.StateOfOriginOfCorporateEntity);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField6[0]", EnumHelper.GetEnumDescription(model.TypeOfCorporateEntity));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField7[0]", model.AcronymOfCorporateEntity);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField8[0]", model.FullName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField9[0]", model.AcronymOfCorporateEntity);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField10[0]", model.FullName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField11[0]", model.FullName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField12[0]", model.FullName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField13[0]", model.FullName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField14[0]", model.FullName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField15[0]", model.FullName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField16[0]", model.CompanyName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField17[0]", model.FullName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField18[0]", model.CorpTitle);

            //pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField19[0]", model.Initials);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField20[0]", model.Initials);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField21[0]", model.Initials);

            pdfMs.FormFlattening = true;
            pdfMs.Writer.CloseStream = false;

            if (pdfMs != null)
            {
                pdfMs.Close();
            }
            ms.Position = 0;
            pdfReader.Close();
            return ms;
        }

        public MemoryStream GetICPdf(ICAgreementTemplateModel model, byte[] doc)
        {
            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(doc);
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, ms);

            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField1[0]", model.Day.ToString() + GetDaySuffix(model.Day));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField2[0]", model.Month.ToString());
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField3[0]", model.YearFormatted);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField4[0]", model.FullName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField5[0]", model.ExpireDay.ToString() + GetDaySuffix(model.ExpireDay));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField6[0]", model.ExpireMonth.ToString());
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField7[0]", model.ExpirationYear);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField8[0]", model.Day.ToString() + GetDaySuffix(model.Day));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField9[0]", model.Month.ToString());
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField10[0]", model.YearFormatted);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField12[0]", model.UserAddressLine1 + " " + model.UserAddressLine2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField22[0]", string.Format("{0}, {1} {2}", model.City, model.State, model.Zip));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField11[0]", model.FullName);

            //sigs and company name on every page
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField13[0]", model.AcronymOfCorporateEntity);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField14[0]", model.UserFirstName.Substring(0, 1) + "." + model.UserLastName.Substring(0, 1));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField15[0]", model.AcronymOfCorporateEntity);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField16[0]", model.UserFirstName.Substring(0, 1) + "." + model.UserLastName.Substring(0, 1));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField17[0]", model.AcronymOfCorporateEntity);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField18[0]", model.UserFirstName.Substring(0, 1) + "." + model.UserLastName.Substring(0, 1));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField19[0]", model.AcronymOfCorporateEntity);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField20[0]", model.UserFirstName.Substring(0, 1) + "." + model.UserLastName.Substring(0, 1));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField21[0]", model.AcronymOfCorporateEntity);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField22[0]", model.UserFirstName.Substring(0, 1) + "." + model.UserLastName.Substring(0, 1));

            pdfMs.FormFlattening = true;
            pdfMs.Writer.CloseStream = false;

            if (pdfMs != null)
            {
                pdfMs.Close();
            }
            ms.Position = 0;
            pdfReader.Close();
            return ms;
        }

        public MemoryStream GetMDAPdf(MDATemplateModel model, byte[] doc)
        {
            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(doc);
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, ms);

            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField1[0]", model.Day.ToString() + GetDaySuffix(model.Day));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField2[0]", model.Month.ToString().Substring(0, 3));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField3[0]", model.Year.ToString().Substring(2, 2));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField4[0]", model.CompanyName);// model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField5[0]", model.StateOfOriginOfCorporateEntity);// model.UserAddressLine1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField6[0]", EnumHelper.GetEnumDescription(model.TypeOfCorporateEntity));// model.UserAddressLine2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField7[0]", model.FirstNameOfCorporateEntity);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField8[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField9[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField10[0]", model.CompanyName);
            StringBuilder sb = new StringBuilder();
            foreach (var item in model.Assets)
            {
                sb.Append(item.AssetNumber);
                sb.Append("; ");
            }
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField11[0]", sb.ToString());
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField12[0]", model.CompanyName); //model.UserFirstName.Substring(0, 1) + model.UserLastName.Substring(0, 1));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField13[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField14[0]", model.CorpTitle);

            //pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField15[0]", model.UserFirstName.Substring(0, 1) + "." + model.UserLastName.Substring(0, 1));
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField16[0]", model.UserFirstName.Substring(0, 1) + "." + model.UserLastName.Substring(0, 1));
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField17[0]", model.UserFirstName.Substring(0, 1) + "." + model.UserLastName.Substring(0, 1));

            pdfMs.FormFlattening = true;
            pdfMs.Writer.CloseStream = false;

            if (pdfMs != null)
            {
                pdfMs.Close();
            }
            ms.Position = 0;
            pdfReader.Close();
            return ms;
        }

        public void SubmitSellerMDA(MDATemplateModel model, byte[] doc, int signingUserId, int documentType)
        {
            var ms = GetMDAPdf(model, doc);
            string xmlBody = string.Empty;

            if (documentType == 1)
            {
                xmlBody =
                        "<envelopeDefinition xmlns=\"http://www.docusign.com/restapi\">" +
                        "<emailSubject>DocuSign API - Signature Request on Document</emailSubject>" +
                        "<status>sent</status>" + 	// "sent" to send immediately, "created" to save as draft in your account
                    // add document(s)
                        "<documents>" +
                        "<document>" +
                        "<documentId>1</documentId>" +
                        "<name>Margin Disclosure and Compensation Agreement</name>" +
                        "</document>" +
                        "</documents>" +
                    // add recipient(s)
                        "<recipients>" +
                        "<signers>" +
                        "<signer>" +
                        "<recipientId>1</recipientId>" +
                        "<email>" + model.Email + "</email>" +
                        "<name>" + model.UserFirstName + " " + model.UserLastName + "</name>" +
                        "<tabs>" +
                        "<initialHereTabs>" +
                        "<initialHere>" +
                        "<tabLabel>i1</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>115</xPosition>" +
                        "<yPosition>717</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>1</pageNumber>" +
                        "<scaleValue>0.7</scaleValue>" +
                        "</initialHere>" +
                        "<initialHere>" +
                        "<tabLabel>i2</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>115</xPosition>" +
                        "<yPosition>717</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>2</pageNumber>" +
                        "<scaleValue>0.7</scaleValue>" +
                        "</initialHere>" +
                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>115</xPosition>" +
                        "<yPosition>717</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>3</pageNumber>" +
                        "<scaleValue>0.7</scaleValue>" +
                        "</initialHere>" +
                        "</initialHereTabs>" +
                        "<signHereTabs>" +
                        "<signHere>" +
                        "<xPosition>140</xPosition>" + // default unit is pixels
                        "<yPosition>131</yPosition>" + // default unit is pixels
                        "<documentId>1</documentId>" +
                        "<pageNumber>3</pageNumber>" +
                        "<scaleValue>0.8</scaleValue>" +
                        "</signHere>" +
                        "</signHereTabs>" +
                        "</tabs>" +
                        "</signer>" +
                        "</signers>" +
                        "</recipients>" +
                        "</envelopeDefinition>";
            }
            else
            {
                xmlBody =
                        "<envelopeDefinition xmlns=\"http://www.docusign.com/restapi\">" +
                        "<emailSubject>DocuSign API - Signature Request on Document</emailSubject>" +
                        "<status>sent</status>" + 	// "sent" to send immediately, "created" to save as draft in your account
                    // add document(s)
                        "<documents>" +
                        "<document>" +
                        "<documentId>1</documentId>" +
                        "<name>Margin Disclosure and Compensation Agreement</name>" +
                        "</document>" +
                        "</documents>" +
                    // add recipient(s)
                        "<recipients>" +
                        "<signers>" +
                        "<signer>" +
                        "<recipientId>1</recipientId>" +
                        "<email>" + model.Email + "</email>" +
                        "<name>" + model.UserFirstName + " " + model.UserLastName + "</name>" +
                        "<tabs>" +
                        "<initialHereTabs>" +
                        "<initialHere>" +
                        "<tabLabel>i1</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>115</xPosition>" +
                        "<yPosition>717</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>1</pageNumber>" +
                        "<scaleValue>0.7</scaleValue>" +
                        "</initialHere>" +
                        "<initialHere>" +
                        "<tabLabel>i2</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>115</xPosition>" +
                        "<yPosition>717</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>2</pageNumber>" +
                        "<scaleValue>0.7</scaleValue>" +
                        "</initialHere>" +
                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>115</xPosition>" +
                        "<yPosition>717</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>3</pageNumber>" +
                        "<scaleValue>0.7</scaleValue>" +
                        "</initialHere>" +
                        "</initialHereTabs>" +
                        "<signHereTabs>" +
                        "<signHere>" +
                        "<xPosition>140</xPosition>" + // default unit is pixels
                        "<yPosition>131</yPosition>" + // default unit is pixels
                        "<documentId>1</documentId>" +
                        "<pageNumber>3</pageNumber>" +
                        "<scaleValue>0.8</scaleValue>" +
                        "</signHere>" +
                        "</signHereTabs>" +
                        "</tabs>" +
                        "</signer>" +
                        "</signers>" +
                        "</recipients>" +
                        "</envelopeDefinition>";
            }
        }

        public void SubmitNARMDA(MDATemplateModel model, byte[] doc, int signingUserId, int documentType)
        {
            var ms = GetMDAPdf(model, doc);
            string xmlBody = string.Empty;

            if (documentType == 1)
            {
                xmlBody =
                        "<envelopeDefinition xmlns=\"http://www.docusign.com/restapi\">" +
                        "<emailSubject>DocuSign API - Signature Request on Document</emailSubject>" +
                        "<status>sent</status>" + 	// "sent" to send immediately, "created" to save as draft in your account
                    // add document(s)
                        "<documents>" +
                        "<document>" +
                        "<documentId>1</documentId>" +
                        "<name>Margin Disclosure and Compensation Agreement</name>" +
                        "</document>" +
                        "</documents>" +
                    // add recipient(s)
                        "<recipients>" +
                        "<signers>" +
                        "<signer>" +
                        "<recipientId>1</recipientId>" +
                        "<email>" + model.Email + "</email>" +
                        "<name>" + model.UserFirstName + " " + model.UserLastName + "</name>" +
                        "<tabs>" +
                        "<initialHereTabs>" +
                        "<initialHere>" +
                        "<tabLabel>i1</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>115</xPosition>" +
                        "<yPosition>717</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>1</pageNumber>" +
                        "<scaleValue>0.7</scaleValue>" +
                        "</initialHere>" +
                        "<initialHere>" +
                        "<tabLabel>i2</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>115</xPosition>" +
                        "<yPosition>717</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>2</pageNumber>" +
                        "<scaleValue>0.7</scaleValue>" +
                        "</initialHere>" +
                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>115</xPosition>" +
                        "<yPosition>717</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>3</pageNumber>" +
                        "<scaleValue>0.7</scaleValue>" +
                        "</initialHere>" +
                        "</initialHereTabs>" +
                        "<signHereTabs>" +
                        "<signHere>" +
                        "<xPosition>140</xPosition>" + // default unit is pixels
                        "<yPosition>131</yPosition>" + // default unit is pixels
                        "<documentId>1</documentId>" +
                        "<pageNumber>3</pageNumber>" +
                        "<scaleValue>0.8</scaleValue>" +
                        "</signHere>" +
                        "</signHereTabs>" +
                        "</tabs>" +
                        "</signer>" +
                        "</signers>" +
                        "</recipients>" +
                        "</envelopeDefinition>";
            }
            else
            {
                xmlBody =
                        "<envelopeDefinition xmlns=\"http://www.docusign.com/restapi\">" +
                        "<emailSubject>DocuSign API - Signature Request on Document</emailSubject>" +
                        "<status>sent</status>" + 	// "sent" to send immediately, "created" to save as draft in your account
                    // add document(s)
                        "<documents>" +
                        "<document>" +
                        "<documentId>1</documentId>" +
                        "<name>Margin Disclosure and Compensation Agreement</name>" +
                        "</document>" +
                        "</documents>" +
                    // add recipient(s)
                        "<recipients>" +
                        "<signers>" +
                        "<signer>" +
                        "<recipientId>1</recipientId>" +
                        "<email>" + model.Email + "</email>" +
                        "<name>" + model.UserFirstName + " " + model.UserLastName + "</name>" +
                        "<tabs>" +
                        "<initialHereTabs>" +
                        "<initialHere>" +
                        "<tabLabel>i1</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>115</xPosition>" +
                        "<yPosition>717</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>1</pageNumber>" +
                        "<scaleValue>0.7</scaleValue>" +
                        "</initialHere>" +
                        "<initialHere>" +
                        "<tabLabel>i2</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>115</xPosition>" +
                        "<yPosition>717</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>2</pageNumber>" +
                        "<scaleValue>0.7</scaleValue>" +
                        "</initialHere>" +
                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>115</xPosition>" +
                        "<yPosition>717</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>3</pageNumber>" +
                        "<scaleValue>0.7</scaleValue>" +
                        "</initialHere>" +
                        "</initialHereTabs>" +
                        "<signHereTabs>" +
                        "<signHere>" +
                        "<xPosition>140</xPosition>" + // default unit is pixels
                        "<yPosition>131</yPosition>" + // default unit is pixels
                        "<documentId>1</documentId>" +
                        "<pageNumber>3</pageNumber>" +
                        "<scaleValue>0.8</scaleValue>" +
                        "</signHere>" +
                        "</signHereTabs>" +
                        "</tabs>" +
                        "</signer>" +
                        "</signers>" +
                        "</recipients>" +
                        "</envelopeDefinition>";
            }
        }

        public MemoryStream GetSellerIPAPdf(MDATemplateModel model, byte[] doc)
        {
            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(doc);
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, ms);

            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField1[0]", model.Day.ToString() + GetDaySuffix(model.Day));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField2[0]", model.Month.ToString().Substring(0, 3));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField3[0]", model.Year.ToString().Substring(2, 2));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField4[0]", model.CompanyName);// model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField5[0]", model.StateOfOriginOfCorporateEntity);// model.UserAddressLine1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField6[0]", EnumHelper.GetEnumDescription(model.TypeOfCorporateEntity));// model.UserAddressLine2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField7[0]", model.FirstNameOfCorporateEntity);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField8[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField9[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField10[0]", model.CompanyName);
            StringBuilder sb = new StringBuilder();
            foreach (var item in model.Assets)
            {
                sb.Append(item.AssetNumber);
                sb.Append("; ");
            }
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField11[0]", sb.ToString());
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField12[0]", model.CompanyName); //model.UserFirstName.Substring(0, 1) + model.UserLastName.Substring(0, 1));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField13[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField14[0]", model.CorpTitle);

            //pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField15[0]", model.UserFirstName.Substring(0, 1) + "." + model.UserLastName.Substring(0, 1));
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField16[0]", model.UserFirstName.Substring(0, 1) + "." + model.UserLastName.Substring(0, 1));
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField17[0]", model.UserFirstName.Substring(0, 1) + "." + model.UserLastName.Substring(0, 1));

            pdfMs.FormFlattening = true;
            pdfMs.Writer.CloseStream = false;

            if (pdfMs != null)
            {
                pdfMs.Close();
            }
            ms.Position = 0;
            pdfReader.Close();
            return ms;
        }

        public MemoryStream GetNARIPAPdf(MDATemplateModel model, byte[] doc)
        {
            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(doc);
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, ms);

            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField1[0]", model.Day.ToString() + GetDaySuffix(model.Day));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField2[0]", model.Month.ToString().Substring(0, 3));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField3[0]", model.Year.ToString().Substring(2, 2));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField4[0]", model.CompanyName);// model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField5[0]", model.StateOfOriginOfCorporateEntity);// model.UserAddressLine1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField6[0]", EnumHelper.GetEnumDescription(model.TypeOfCorporateEntity));// model.UserAddressLine2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField7[0]", model.FirstNameOfCorporateEntity);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField8[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField9[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField10[0]", model.CompanyName);
            StringBuilder sb = new StringBuilder();
            foreach (var item in model.Assets)
            {
                sb.Append(item.AssetNumber);
                sb.Append("; ");
            }
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField11[0]", sb.ToString());
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField12[0]", model.CompanyName); //model.UserFirstName.Substring(0, 1) + model.UserLastName.Substring(0, 1));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField13[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField14[0]", model.CorpTitle);

            //pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField15[0]", model.UserFirstName.Substring(0, 1) + "." + model.UserLastName.Substring(0, 1));
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField16[0]", model.UserFirstName.Substring(0, 1) + "." + model.UserLastName.Substring(0, 1));
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField17[0]", model.UserFirstName.Substring(0, 1) + "." + model.UserLastName.Substring(0, 1));

            pdfMs.FormFlattening = true;
            pdfMs.Writer.CloseStream = false;

            if (pdfMs != null)
            {
                pdfMs.Close();
            }
            ms.Position = 0;
            pdfReader.Close();
            return ms;
        }
        

        public MemoryStream GetIPABuyerPdf(IPABuyerTemplateModel model, byte[] doc)
        {
            model.DateAccepted = DateTime.Now.ToString("MM/dd/yyyy");
            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(doc);
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, ms);

            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField1[0]", model.Day.ToString() + GetDaySuffix(model.Day));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField2[0]", model.Month.ToString());
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField3[0]", model.Year.ToString().Substring(2, 2));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField4[0]", model.CompanyName);// model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField5[0]", model.StateOfOriginOfCorporateEntity);// model.UserAddressLine1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField6[0]", EnumHelper.GetEnumDescription(model.TypeOfCorporateEntity));// model.UserAddressLine2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField7[0]", model.FirstNameOfCorporateEntity);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField8[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField9[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField10[0]", model.CompanyName);
            StringBuilder sb = new StringBuilder();
            foreach (var item in model.Assets)
            {
                sb.Append(item.AssetNumber);
                sb.Append("; ");
            }
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField11[0]", sb.ToString());

            pdfMs.AcroFields.SetField("topmostSubform[0].Page14[0].TextField12[0]", model.CompanyName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page14[0].TextField13[0]", model.DateAccepted);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page14[0].TextField14[0]", model.DateAccepted);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page14[0].TextField15[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page14[0].TextField16[0]", model.CorpTitle);

            pdfMs.FormFlattening = true;
            pdfMs.Writer.CloseStream = false;

            if (pdfMs != null)
            {
                pdfMs.Close();
            }
            ms.Position = 0;
            pdfReader.Close();
            return ms;
        }

        public MemoryStream GetIPASellerPdf(IPASellerTemplateModel model, byte[] doc)
        {
            model.DateAccepted = DateTime.Now.ToString("MM/dd/yyyy");
            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(doc);
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, ms);

            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField1[0]", model.Day.ToString() + GetDaySuffix(model.Day));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField2[0]", model.Month.ToString());
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField3[0]", model.Year.ToString().Substring(2, 2));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField4[0]", model.CompanyName);// model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField5[0]", model.StateOfOriginOfCorporateEntity);// model.UserAddressLine1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField6[0]", EnumHelper.GetEnumDescription(model.TypeOfCorporateEntity));// model.UserAddressLine2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField7[0]", model.FirstNameOfCorporateEntity);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField8[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField9[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField10[0]", model.CompanyName);
            StringBuilder sb = new StringBuilder();
            foreach (var item in model.Assets)
            {
                sb.Append(item.AssetNumber);
                sb.Append("; ");
            }
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField11[0]", sb.ToString());
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField12[0]", model.PropertyAddress1 + " " + model.PropertyAddress2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField13[0]", model.PropertyCity);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField14[0]", model.PropertyState);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField15[0]", model.PropertyZip);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField16[0]", model.PropertyApns);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField17[0]", model.PropertyCounty);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField18[0]", model.PropertyType);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField19[0]", model.PropertyName);

            pdfMs.AcroFields.SetField("topmostSubform[0].Page13[0].TextField20[0]", model.CompanyName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page13[0].TextField21[0]", model.DateAccepted);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page13[0].TextField22[0]", model.DateAccepted);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page13[0].TextField23[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page13[0].TextField24[0]", model.CorpTitle);

            pdfMs.FormFlattening = true;
            pdfMs.Writer.CloseStream = false;

            if (pdfMs != null)
            {
                pdfMs.Close();
            }
            ms.Position = 0;
            pdfReader.Close();
            return ms;
        }

        public MemoryStream GetIPANARPdf(IPANARTemplateModel model, byte[] doc)
        {
            model.DateAccepted = DateTime.Now.ToString("MM/dd/yyyy");
            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(doc);
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, ms);

            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField1[0]", model.Day.ToString() + GetDaySuffix(model.Day));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField2[0]", model.Month.ToString());
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField3[0]", model.Year.ToString().Substring(2, 2));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField4[0]", model.CompanyName);// model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField5[0]", model.StateOfOriginOfCorporateEntity);

            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField6[0]", model.UserAddressLine1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField7[0]", model.UserAddressLine2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField8[0]", model.UserCity);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField9[0]", model.UserState);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField10[0]", model.UserZip);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField11[0]", model.UserFirstName + " " + model.UserLastName);

            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField12[0]", model.UserFirstName + " " + model.UserLastName);
            StringBuilder sb = new StringBuilder();
            foreach (var item in model.Assets)
            {
                sb.Append(item.AssetNumber);
                sb.Append("; ");
            }
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField13[0]", sb.ToString());

            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField14[0]", model.PropertyAddress1 + " " + model.PropertyAddress2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField15[0]", model.PropertyCity);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField16[0]", model.PropertyState);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField17[0]", model.PropertyZip);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField18[0]", model.PropertyApns);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField19[0]", model.PropertyCounty);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField20[0]", model.PropertyType);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField21[0]", model.PropertyName);

            

            pdfMs.AcroFields.SetField("topmostSubform[0].Page13[0].TextField22[0]", model.CompanyName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page13[0].TextField23[0]", model.DateAccepted);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page13[0].TextField24[0]", model.DateAccepted);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page13[0].TextField25[0]", model.UserFirstName + " " + model.UserLastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page13[0].TextField26[0]", model.CorpTitle);


            pdfMs.FormFlattening = true;
            pdfMs.Writer.CloseStream = false;

            if (pdfMs != null)
            {
                pdfMs.Close();
            }
            ms.Position = 0;
            pdfReader.Close();
            return ms;
        }

        public void SubmitIPABuyer(IPABuyerTemplateModel model, byte[] doc, int signingUserId, int documentType)
        {
            var ms = GetIPABuyerPdf(model, doc);
            string xmlBody = string.Empty;
            xmlBody =
                        "<envelopeDefinition xmlns=\"http://www.docusign.com/restapi\">" +
                        "<emailSubject>DocuSign API - Signature Request on Document</emailSubject>" +
                        "<status>sent</status>" + 	// "sent" to send immediately, "created" to save as draft in your account
                // add document(s)
                        "<documents>" +
                        "<document>" +
                        "<documentId>1</documentId>" +
                        "<name>Internet Portal Agreement</name>" +
                        "</document>" +
                        "</documents>" +
                // add recipient(s)
                        "<recipients>" +
                        "<signers>" +
                        "<signer>" +
                        "<recipientId>1</recipientId>" +
                        "<email>" + model.Email + "</email>" +
                        "<name>" + model.UserFirstName + " " + model.UserLastName + "</name>" +
                        "<tabs>" +

                   #region Tabs
                        "<initialHereTabs>" +

                        "<initialHere>" +
                        "<tabLabel>i1</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>1</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i2</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>2</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>3</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>4</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>5</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>6</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>7</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>8</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>9</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>10</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>11</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>12</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>13</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>14</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "</initialHereTabs>" +
                    #endregion

                        "<signHereTabs>" +

                        "<signHere>" +
                        "<xPosition>72</xPosition>" +
                        "<yPosition>96</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>14</pageNumber>" +
                        "<scaleValue>0.9</scaleValue>" +
                        "</signHere>" +

                        "</signHereTabs>" +
                        "</tabs>" +
                        "</signer>" +
                        "</signers>" +
                        "</recipients>" +
                        "</envelopeDefinition>";
            saveEnvelope(DocumentType.MDA, sendToDocusign(ms, "IPA.pdf", xmlBody), signingUserId, model.Assets.Select(s => s.AssetNumber).ToList());
        }

        public void SubmitIPASeller(IPASellerTemplateModel model, byte[] doc, int signingUserId, int documentType)
        {
            var ms = GetIPASellerPdf(model, doc);
            string xmlBody = string.Empty;
            xmlBody =
                        "<envelopeDefinition xmlns=\"http://www.docusign.com/restapi\">" +
                        "<emailSubject>DocuSign API - Signature Request on Document</emailSubject>" +
                        "<status>sent</status>" + 	// "sent" to send immediately, "created" to save as draft in your account
                // add document(s)
                        "<documents>" +
                        "<document>" +
                        "<documentId>1</documentId>" +
                        "<name>Internet Portal Agreement</name>" +
                        "</document>" +
                        "</documents>" +
                // add recipient(s)
                        "<recipients>" +
                        "<signers>" +
                        "<signer>" +
                        "<recipientId>1</recipientId>" +
                        "<email>" + model.Email + "</email>" +
                        "<name>" + model.UserFirstName + " " + model.UserLastName + "</name>" +
                        "<tabs>" +

            #region Tabs
 "<initialHereTabs>" +

                        "<initialHere>" +
                        "<tabLabel>i1</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>1</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i2</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>2</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>3</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>4</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>5</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>6</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>7</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>8</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>9</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>10</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>11</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>12</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>13</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "</initialHereTabs>" +
            #endregion

 "<signHereTabs>" +

                        "<signHere>" +
                        "<xPosition>72</xPosition>" +
                        "<yPosition>364</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>13</pageNumber>" +
                        "<scaleValue>0.9</scaleValue>" +
                        "</signHere>" +

                        "</signHereTabs>" +
                        "</tabs>" +
                        "</signer>" +
                        "</signers>" +
                        "</recipients>" +
                        "</envelopeDefinition>";
            saveEnvelope(DocumentType.SellerIPA, sendToDocusign(ms, "IPA.pdf", xmlBody), signingUserId, model.Assets.Select(s => s.AssetNumber).ToList());
        }

        public void SubmitIPANAR(IPANARTemplateModel model, byte[] doc, int signingUserId, int documentType)
        {
            var ms = GetIPANARPdf(model, doc);
            string xmlBody = string.Empty;
            xmlBody =
                        "<envelopeDefinition xmlns=\"http://www.docusign.com/restapi\">" +
                        "<emailSubject>DocuSign API - Signature Request on Document</emailSubject>" +
                        "<status>sent</status>" + 	// "sent" to send immediately, "created" to save as draft in your account
                // add document(s)
                        "<documents>" +
                        "<document>" +
                        "<documentId>1</documentId>" +
                        "<name>Internet Portal Agreement</name>" +
                        "</document>" +
                        "</documents>" +
                // add recipient(s)
                        "<recipients>" +
                        "<signers>" +
                        "<signer>" +
                        "<recipientId>1</recipientId>" +
                        "<email>" + model.Email + "</email>" +
                        "<name>" + model.UserFirstName + " " + model.UserLastName + "</name>" +
                        "<tabs>" +

            #region Tabs
 "<initialHereTabs>" +

                        "<initialHere>" +
                        "<tabLabel>i1</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>1</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i2</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>2</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>3</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>4</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>5</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>6</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>7</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>8</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>9</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>10</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>11</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>12</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "<initialHere>" +
                        "<tabLabel>i3</tabLabel>" +
                        "<recipientId>1</recipientId>" +
                        "<xPosition>503</xPosition>" +
                        "<yPosition>676</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>13</pageNumber>" +
                        "<scaleValue>1.0</scaleValue>" +
                        "</initialHere>" +

                        "</initialHereTabs>" +
            #endregion

 "<signHereTabs>" +

                        "<signHere>" +
                        "<xPosition>172</xPosition>" +
                        "<yPosition>85</yPosition>" +
                        "<documentId>1</documentId>" +
                        "<pageNumber>13</pageNumber>" +
                        "<scaleValue>0.9</scaleValue>" +
                        "</signHere>" +

                        "</signHereTabs>" +
                        "</tabs>" +
                        "</signer>" +
                        "</signers>" +
                        "</recipients>" +
                        "</envelopeDefinition>";
            saveEnvelope(DocumentType.NARIPA, sendToDocusign(ms, "IPA.pdf", xmlBody), signingUserId, model.Assets.Select(s => s.AssetNumber).ToList());
        }

        // added method implementations

        public List<ExtractedImageModel> GetBitmapImagesFromPDF(string pdf, string guidId)
        {
            string path = pdf;
            var list = new List<ExtractedImageModel>();
            int i = 0;
            try
            {
                using (var doc = new Doc())
                {
                    doc.Read(path);
                    var pix = doc.ObjectSoup.ToList();
                    ExtractorContext c = new ExtractorContext();
                    foreach (var pic in pix)
                    {
                        try
                        {
                            if (pic.GetType() != null && pic.GetType().ToString() == "WebSupergoo.ABCpdf9.Objects.PixMap")
                            {
                                string tempPath = path;
                                var extractor = ObjectExtractor.FromIndirectObject(pic, c);
                                //list.Add(extractor.GetBitmap());
                                var image = extractor.GetBitmap();
                                byte[] byteArray = new byte[0];
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                    stream.Close();

                                    byteArray = stream.ToArray();
                                }
                                var filename = Guid.NewGuid() + ".jpg";
                                tempPath = Path.Combine(path, filename);
                                File.WriteAllBytes(tempPath, byteArray);
                                list.Add(new ExtractedImageModel()
                                {
                                    Image = tempPath,
                                    IsSelected = false,
                                    //FileByteArray = _fileManager.ConvertImageToBytes(f),
                                    //ThumbnailByte = thumbnail,
                                    //ThumbnailByteBase64 = Convert.ToBase64String(thumbnail),
                                    OrderTemp = i,
                                    Height = image.Height.ToString(),
                                    Width = image.Width.ToString()
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            // don't break the entire process because of one embedded image that we can't get
                        }
                        i++;
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<ExtractedImageModel> GetBitmapImagesFromPDF(byte[] pdf, string guidId)
        {
            string path = Path.Combine(ConfigurationManager.AppSettings["DataRoot"], "ImportFiles", guidId);
            var list = new List<ExtractedImageModel>();
            int i = 0;
            try
            {
                using (var doc = new Doc())
                {
                    doc.Read(pdf);
                    var pix = doc.ObjectSoup.ToList();
                    ExtractorContext c = new ExtractorContext();
                    foreach (var pic in pix)
                    {
                        try
                        {
                            if (pic.GetType() != null && pic.GetType().ToString() == "WebSupergoo.ABCpdf9.Objects.PixMap")
                            {
                                string tempPath = path;
                                var extractor = ObjectExtractor.FromIndirectObject(pic, c);
                                //list.Add(extractor.GetBitmap());
                                var image = extractor.GetBitmap();
                                byte[] byteArray = new byte[0];
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                    stream.Close();

                                    byteArray = stream.ToArray();
                                }
                                var filename = Guid.NewGuid() + ".jpg";
                                tempPath = Path.Combine(path, filename);
                                File.WriteAllBytes(tempPath, byteArray);
                                list.Add(new ExtractedImageModel()
                                {
                                    Image = tempPath,
                                    IsSelected = false,
                                    //FileByteArray = _fileManager.ConvertImageToBytes(f),
                                    //ThumbnailByte = thumbnail,
                                    //ThumbnailByteBase64 = Convert.ToBase64String(thumbnail),
                                    OrderTemp = i,
                                    Height = image.Height.ToString(),
                                    Width = image.Width.ToString()
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            // don't break the entire process because of one embedded image that we can't get
                        }
                        i++;
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
               
    }
}
