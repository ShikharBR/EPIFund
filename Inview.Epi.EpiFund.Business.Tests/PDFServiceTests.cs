using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Inview.Epi.EpiFund.Domain.ViewModel;
using Inview.Epi.EpiFund.Web.Infrastructure;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using Xunit;

namespace Inview.Epi.EpiFund.Business.Tests
{
    public class Accounts
    {
        public List<LoginAccount> loginAccounts { get; set; }
    }

    public class LoginAccount
    {
        public string name { get; set; }
        public string accountId { get; set; }
        public string baseUrl { get; set; }
        public string isDefault { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string siteDescription { get; set; }
    }

    public class PDFServiceTests
    {
        private string _appType = string.Empty;
        private string _docType = string.Empty;

        [Fact]
        public void test()
        {
            UserModel model = new UserModel();
            model.FirstName = "skyler";
            model.LastName = "hamilton";
            model.AddressLine1 = "address 1";
            model.AddressLine2 = "address 2";
            model.Username = "skyler.hamilton@inviewlabs.com";
            model.CompanyName = "inviewlabs";

            RecordPaymentModel payment = new RecordPaymentModel();
            payment.AssetId = new Guid("5e310605-b66d-43e4-8b4a-9e8ef0891fb2");

            //submitMDA(model, payment);
            //submitNCND(model);
            submitJVAgreement();
            //submitICAgreement(model);
            //submitPersonalFinantialStatement();
            //var a = GetFieldsFromForm(@"C:\projects\hg\epifund\Documents\f1099msc--2014fixed.pdf");
            //var a = GetFieldsFromForm(@"C:\projects\hg\epifundTestDocs\Template-Binding Contingent LOI 7-27-14(static).pdf");
            //flattenForm(@"C:\projects\trunk\hg\epifundTestDocs\Personal Financial Statement PDF Auto Fill Template 4-14-14.pdf");
            //var date = DateTime.Now;
            //var year = date.Year;
            //var day = date.Day;
            //var month = date.ToString("MMMM");
            //var newDate = DateTime.ParseExact(day.ToString() + month + year.ToString(), "dMMMMyyyy", CultureInfo.InvariantCulture);
            //submitBindingContingent();
            //submitDocJSON();
        }

        public void flattenForm(string path)
        {
            FileStream fs = File.Create(@"C:\" + DateTime.Now.ToString("yyyyddhhmmss") + ".pdf");
            PdfReader pdfReader = new PdfReader(path);
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, fs);
            pdfMs.FormFlattening = true;
            if (pdfMs != null)
            {
                pdfMs.Close();
            }
            fs.Close();
        }

        public void submitMDA(UserModel model, RecordPaymentModel payment)
        {
            int documentType = 1;
            string xmlBody = string.Empty;
            _appType = "application/xml";
            _docType = "application/pdf";
            DateTime dt = DateTime.Now;
            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(@"C:\projects\hg\epifund\Inview.Epi.EpiFund.Web\Resources\MDATemplateV1.pdf");
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, ms);

            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField1[0]", dt.ToString("dd") + GetDaySuffix(dt.Day));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField2[0]", dt.ToString("MMM"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField3[0]", dt.ToString("yy"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField4[0]", model.FirstName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField5[0]", model.AddressLine1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField6[0]", model.AddressLine2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField7[0]", "HEREINAFTER");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField8[0]", "HERINAFTERAND");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField9[0]", "HEREINAFTER");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField10[0]", model.FirstName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField11[0]", payment.AssetId.ToString());
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField12[0]", model.FirstName.Substring(0, 1) + model.LastName.Substring(0, 1));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField13[0]", model.FirstName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField14[0]", model.CompanyName);

            pdfMs.FormFlattening = true;
            pdfMs.Writer.CloseStream = false;

            if (pdfMs != null)
            {
                pdfMs.Close();
            }
            ms.Position = 0;
            pdfReader.Close();

            //string jsonBody = "{\"emailSubject\":\"API Call for adding signature request to document and sending\"," +
            //            "\"documents\":[{" +
            //                "\"documentId\":\"1\"," +
            //                "\"name\":\"Margin Disclosure and Compensation Agreement\"}]," +
            //                "\"recipients\":{" +
            //                    "\"signers\":[{" +
            //                        "\"email\":\"" + "skyler.hamilton@inviewlabs.com" + "\"," +
            //                        "\"name\":\"" + "S" + " " + "H" + "\"," +
            //                        "\"recipientId\":\"1\"," +
            //                        "\"tabs\":{" +
            //                            "\"signHereTabs\":[" +
            //                                "{" + "\"xPosition\":\"107\"," + "\"yPosition\":\"117\"," + "\"documentId\":\"1\"," + "\"pageNumber\":\"3\"" + "}" + "]}" +
            //                            "\"initialHereTabs\":[" +
            //                                "{" + "\"xPosition\":\"85\"," + "\"yPosition\":\"717\"," + "\"documentId\":\"1\"," + "\"pageNumber\":\"1\"" + "\"scaleValue\":\"0.7\"," + "\"tabLabel\":\"i1\"," + "}" +
            //                                "{" + "\"xPosition\":\"85\"," + "\"yPosition\":\"717\"," + "\"documentId\":\"1\"," + "\"pageNumber\":\"2\"" + "\"scaleValue\":\"0.7\"," + "\"tabLabel\":\"i2\"," + "}" +
            //                                "{" + "\"xPosition\":\"85\"," + "\"yPosition\":\"717\"," + "\"documentId\":\"1\"," + "\"pageNumber\":\"3\"" + "\"scaleValue\":\"0.7\"," + "\"tabLabel\":\"i3\"," + "}" +
            //                            "]}}]}," +
            //            "\"status\":\"sent\"}";
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
                            "<email>" + "skyler.hamilton@inviewlabs.com" + "</email>" +
                            "<name>" + "FFF" + " " + "LLL" + "</name>" +
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
                            "<yPosition>150</yPosition>" + // default unit is pixels
                            "<documentId>1</documentId>" +
                            "<pageNumber>3</pageNumber>" +
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
                    "<email>" + "skyler.hamilton@inviewlabs.com" + "</email>" +
                    "<name>" + "FFF" + " " + "LLL" + "</name>" +
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
            }

            sendToDocusign(ms, "EPI - MDA Template 3-21-14.pdf", xmlBody);
        }

        public void submitNCND(UserModel model)
        {
            DateTime dt = DateTime.Now;
            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(@"C:\projects\hg\epifund\Inview.Epi.EpiFund.Web\Resources\NCNDTemplate.pdf");
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, ms);

            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField1[0]", dt.ToString("dd") + GetDaySuffix(dt.Day));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField2[0]", dt.ToString("MMM"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField3[0]", dt.ToString("yy"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField4[0]", model.CompanyName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField5[0]", model.AddressLine1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField6[0]", model.AddressLine2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField7[0]", "HEREINAFTER");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField8[0]", model.FullName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField9[0]", "HEREINAFTER");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField10[0]", model.FullName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField11[0]", model.CompanyName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField12[0]", model.CompanyName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField13[0]", model.AddressLine1);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField14[0]", model.AddressLine2);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField15[0]", model.AlternateEmail);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField16[0]", model.WorkNumber);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField17[0]", model.FaxNumber);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page7[0].TextField18[0]", model.FirstName.Substring(0, 1) + model.LastName.Substring(0, 1));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page7[0].TextField19[0]", model.FullName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page7[0].TextField20[0]", model.CompanyName);

            pdfMs.AcroFields.SetField("topmostSubform[0].Page7[0].TextField21[0]", model.FirstName.Substring(0, 1) + model.LastName.Substring(0, 1));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page7[0].TextField22[0]", model.FirstName.Substring(0, 1) + model.LastName.Substring(0, 1));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page7[0].TextField23[0]", model.FirstName.Substring(0, 1) + model.LastName.Substring(0, 1));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page7[0].TextField24[0]", model.FirstName.Substring(0, 1) + model.LastName.Substring(0, 1));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page7[0].TextField25[0]", model.FirstName.Substring(0, 1) + model.LastName.Substring(0, 1));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page7[0].TextField26[0]", model.FirstName.Substring(0, 1) + model.LastName.Substring(0, 1));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page7[0].TextField27[0]", model.FirstName.Substring(0, 1) + model.LastName.Substring(0, 1));
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
                    "<name>Confidentiality and Non-Disclosure Agreement</name>" +
                    "</document>" +
                    "</documents>" +
                // add recipient(s)
                    "<recipients>" +
                    "<signers>" +
                    "<signer>" +
                    "<recipientId>1</recipientId>" +
                    "<email>" + model.Username + "</email>" +
                    "<name>" + model.FullName + "</name>" +
                    "<tabs>" +

                    "<initialHereTabs>" +

                    "<initialHere>" +
                    "<tabLabel>i1</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>188</xPosition>" +
                    "<yPosition>728</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>1</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +

                    "<initialHere>" +
                    "<tabLabel>i2</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>170</xPosition>" +
                    "<yPosition>717</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>2</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +

                    "<initialHere>" +
                    "<tabLabel>i3</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>170</xPosition>" +
                    "<yPosition>717</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>3</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +

                    "<initialHere>" +
                    "<tabLabel>i4</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>170</xPosition>" +
                    "<yPosition>717</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>4</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +

                    "<initialHere>" +
                    "<tabLabel>i5</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>165</xPosition>" +
                    "<yPosition>717</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>5</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +

                    "<initialHere>" +
                    "<tabLabel>i6</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>170</xPosition>" +
                    "<yPosition>717</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>6</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +

                    "<initialHere>" +
                    "<tabLabel>i7</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>170</xPosition>" +
                    "<yPosition>717</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>7</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +

                    "</initialHereTabs>" +


                    "<signHereTabs>" +
                    "<signHere>" +
                    "<xPosition>145</xPosition>" + // default unit is pixels
                    "<yPosition>233</yPosition>" + // default unit is pixels
                    "<documentId>1</documentId>" +
                    "<pageNumber>7</pageNumber>" +
                    "</signHere>" +
                    "</signHereTabs>" +
                    "</tabs>" +
                    "</signer>" +
                    "</signers>" +
                    "</recipients>" +
                    "</envelopeDefinition>";
            sendToDocusign(ms, "EPI Website NCND Template 3-1-2014.pdf", xmlBody);
        }

        public void submitICAgreement(UserModel model)
        {
            DateTime dt = DateTime.Now;
            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(@"C:\projects\hg\epifund\Inview.Epi.EpiFund.Web\Resources\ICAgreementTemplate.pdf");
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, ms);

            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField1[0]", dt.ToString("dd") + GetDaySuffix(dt.Day));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField2[0]", dt.ToString("MMM"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField3[0]", dt.ToString("yy"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField4[0]", model.FirstName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField5[0]", dt.ToString("dd") + GetDaySuffix(dt.Day));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField6[0]", dt.ToString("MMM"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField7[0]", dt.ToString("yy"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField8[0]", dt.ToString("dd") + GetDaySuffix(dt.Day));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField9[0]", dt.ToString("MMM"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField10[0]", dt.ToString("yy"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField11[0]", "ADRESS LINE 1");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField12[0]", "ADRESS LINE 2");

            //sigs on every page
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField13[0]", "Corp Title");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField14[0]", "S" + "." + "H");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField15[0]", "Corp Title");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField16[0]", "S" + "." + "H");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField17[0]", "Corp Title");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField18[0]", "S" + "." + "H");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField19[0]", "Corp Title");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField20[0]", "S" + "." + "H");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField21[0]", "Corp Title");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField22[0]", "S" + "." + "H");

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
                    "<name>Independant Contractor Services Agreement</name>" +
                    "</document>" +
                    "</documents>" +
                // add recipient(s)
                    "<recipients>" +
                    "<signers>" +
                    "<signer>" +
                    "<recipientId>1</recipientId>" +
                    "<email>" + model.Username + "</email>" +
                    "<name>" + model.FirstName + " " + model.LastName + "</name>" +
                    "<tabs>" +

                    "<initialHereTabs>" +

                    "<initialHere>" +
                    "<tabLabel>i1</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>100</xPosition>" +
                    "<yPosition>717</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>1</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +

                    "<initialHere>" +
                    "<tabLabel>i2</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>100</xPosition>" +
                    "<yPosition>717</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>2</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +

                    "<initialHere>" +
                    "<tabLabel>i3</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>100</xPosition>" +
                    "<yPosition>717</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>3</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +

                    "<initialHere>" +
                    "<tabLabel>i4</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>100</xPosition>" +
                    "<yPosition>717</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>4</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +

                    "<initialHere>" +
                    "<tabLabel>i4</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>100</xPosition>" +
                    "<yPosition>717</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>5</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +

                    "</initialHereTabs>" +

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
            sendToDocusign(ms, "Template - IC Agreement - EPI USC 2B Master.pdf", xmlBody);
        }

        public void submitPersonalFinantialStatement()
        {
            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(@"C:\projects\hg\epifundTestDocs\Personal Financial Statement.pdf");
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, ms);

            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].asof[0]", "JULY");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].asofDate[0]", "1999");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].name[0]", "Skyler Hamilton");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].businessPhone[0]", "111 111-1111");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].residentialAddress[0]", "111 address drive");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].residentialPhone[0]", "222 222-2222");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].cityStateZip[0]", "City State, Zip");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].businessNameOfApplicant[0]", "InviewLabs");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].cashOnHand[0]", "300.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].savingsAccount[0]", "100.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].iraOrOtherRetirementAccount[0]", "200.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].accountsAndNotesReceivable[0]", "300.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].lifeInsurance-CashSurrenderValue[0]", "400.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].stocksAndBonds[0]", "500.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].realEstate[0]", "600.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].automobilesTotalPresentValue[0]", "700.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].otherPersonalProperty[0]", "800.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].otherAssets[0]", "900.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].totalLeft[0]", "1000.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].sourceOfIncomeSalary[0]", "1100.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].sourceOfIncomeNetInvestment[0]", "1200.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].sourceOfIncomeRealEstate[0]", "1300.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].sourceOfIncomeOther[0]", "1400.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].accountsPayable[0]", "1500.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].notesPayableToBanksAndOthers[0]", "1700.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].installmentAccountAuto[0]", "1800.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].installmentAccountAutoMontlyPayment[0]", "1900.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].installmentAccountOther[0]", "2000.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].installmentAccountOtherMonthlyPayment[0]", "2100.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].loanOnLifeInsurance[0]", "2200.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].mortgagesonRealEstate[0]", "2300.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].unpaidTaxes[0]", "2400.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].otherLiabilities[0]", "2500.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].totalLiabilities[0]", "2600.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].netWorth[0]", "2700.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].rightTotal[0]", "2800.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].liabilitiesAsEndorserOrCoMaker[0]", "2900.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].liablitiesLegalClaimsAndJudgments[0]", "3000.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].liabilitiesProvisionForFederalTaxIncome[0]", "3100.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].liabilitiesOtherSpecialDebt[0]", "3200.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].descriptionOfOtherIncome1[0]", "Super User is a question and answer site for computer enthusiasts and power users. It's 100% free, no registration required.");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].descriptionOfOtherIncome2[0]", "THIS IS A CASTED EVENT and due to the nature of the set, anyone participating must possess the physical ability to stand for extended periods, walk up and down stairs as needed, follow verbal and visual instructions without assistance, and be in general good health without any conditions that would restrict or limit any of the above activities. By presenting yourselves for check-in you and your group members certify that you meet these requirements. ");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].descriptionOfOtherIncome3[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].nameAndAddrOfNoteHolders1[0]", "Name and address of thing");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].originalBalance1[0]", "345");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].currentBalance1[0]", "1333.00");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].paymentAmount1[0]", "4");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].frequency1[0]", "monyluy");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].hosSecuredOrEndorsedTypeOfCollateral1[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].nameAndAddrOfNoteHolders2[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].nameAndAddrOfNoteHolders3[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].nameAndAddrOfNoteHolders4[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].nameAndAddrOfNoteHolders5[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].originalBalance2[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].originalBalance3[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].originalBalance4[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].originalBalance5[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].currentBalance2[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].currentBalance3[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].currentBalance4[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].currentBalance5[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].paymentAmount2[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].paymentAmount3[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].paymentAmount4[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].paymentAmount5[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].frequency2[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].frequency3[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].frequency4[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].frequency5[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].hosSecuredOrEndorsedTypeOfCollateral2[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].hosSecuredOrEndorsedTypeOfCollateral3[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].hosSecuredOrEndorsedTypeOfCollateral4[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].hosSecuredOrEndorsedTypeOfCollateral5[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].StocksAndBondsNumberOfShares1[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].StocksAndBondsNumberOfShares2[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].StocksAndBondsNumberOfShares3[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].StocksAndBondsNumberOfShares4[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].StocksAndBondsNameOfSecurities1[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].StocksAndBondsNameOfSecurities2[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].StocksAndBondsNameOfSecurities3[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].StocksAndBondsNameOfSecurities4[0]", "name of tyhi");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsCost1[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsCost2[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsCost3[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsCost4[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsMarketValueQuotation-Exchange1[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsMarketValueQuotation-Exchange2[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsMarketValueQuotation-Exchange3[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsMarketValueQuotation-Exchange4[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsDateOfQuotation-Exchange1[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsDateOfQuotation-Exchange2[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsDateOfQuotation-Exchange3[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsDateOfQuotation-Exchange4[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsTotalValue1[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsTotalValue2[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsTotalValue3[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].stocksAndBondsTotalValue4[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].properyATypeOfRealEstate[0]", "property a");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].properyBTypeOfRealEstate[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].properyCTypeOfRealEstate[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertAAddress[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertBAddress[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertCAddress[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyADatePurchased[0]", "date purchased");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyBDatePurchased[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyCDatePurchased[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyAOriginalCost[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyBOriginalCost[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyCOriginalCost[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyAPresentMarketValue[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyBPresentMarketValue[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyCPresentMarketValue[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyANameAndAddrOfMortgageHolder[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyBNameAndAddrOfMortgageHolder[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyCNameAndAddrOfMortgageHolder[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyAMortgageAccountNumber[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyBMortgageAccountNumber[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyCMortgageAccountNumber[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyAMortgageBalance[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyBMortgageBalance[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyCMortgageBalance[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyAAmountOfPaymentPerMonth-Year[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyBAmountOfPaymentPerMonth-Year[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyCAmountOfPaymentPerMonth-Year[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyAStatusOfMortgage[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyBStatusOfMortgage[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].propertyCStatusOfMortgage[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].otherPersonalPropertyAndOtherAssets[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].unpaidTaxes[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].otherLiabilities[0]", "Please do not wear clothing with logos or any white colored clothing. This taping will take place outdoors so please dress according to the weather. Cell phones and cameras are not allowed on set.");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].lifeInsuranceHeld[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].printName1[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].date1[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].ssn1[0]", "");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].printName2[0]", "Skyler Hamilton");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].date2[0]", "12/78/0000");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].ssn2[0]", "111-22-3333");

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
                    "<name>Personal Finantial Statement</name>" +
                    "</document>" +
                    "</documents>" +
                // add recipient(s)
                    "<recipients>" +
                    "<signers>" +
                    "<signer>" +
                    "<recipientId>1</recipientId>" +
                    "<email>" + "skyler.hamilton@inviewlabs.com" + "</email>" +
                    "<name>" + "Skyler" + " " + "Hamilton" + "</name>" +
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
            sendToDocusign(ms, "Personal Finantial Statement.pdf", xmlBody);
        }

        public void submitJVAgreement()
        {
            DateTime dt = DateTime.Now;
            string firstName = "Skyler";
            string lastName = "Hamilton";
            string company = "Inviewlabs LLC Company";
            string title = "Deleloper coder";
            string initials = "S.H";
            string email = "skyler.hamilton@inviewlabs.com";

            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(@"C:\projects\hg\epifund\Inview.Epi.EpiFund.Web\Resources\JVTemplate.pdf");
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, ms);

            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField1[0]", dt.ToString("dd") + GetDaySuffix(dt.Day));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField2[0]", dt.ToString("MMM"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField3[0]", dt.ToString("yy"));
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField4[0]", firstName + " " + lastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField5[0]", firstName + " " + lastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField6[0]", firstName + " " + lastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField7[0]", firstName + " " + lastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField8[0]", firstName + " " + lastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField9[0]", company);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField10[0]", firstName + " " + lastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField11[0]", firstName + " " + lastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField12[0]", firstName + " " + lastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField13[0]", firstName + " " + lastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField14[0]", firstName + " " + lastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField15[0]", firstName + " " + lastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField16[0]", company);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField17[0]", firstName + " " + lastName);
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField18[0]", title);

            //pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField19[0]", initials);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField20[0]", initials);
            //pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField21[0]", initials);

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
                    "<name>Joint Venture Marketing Partcipation Agreement</name>" +
                    "</document>" +
                    "</documents>" +
                // add recipient(s)
                    "<recipients>" +
                    "<signers>" +
                    "<signer>" +
                    "<recipientId>1</recipientId>" +
                    "<email>" + email + "</email>" +
                    "<name>" + firstName + " " + lastName + "</name>" +
                    "<tabs>" +
                    "<initialHereTabs>" +
                    "<initialHere>" +
                    "<tabLabel>i1</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>77</xPosition>" +
                    "<yPosition>718</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>1</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +
                    "<initialHere>" +
                    "<tabLabel>i2</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>77</xPosition>" +
                    "<yPosition>718</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>2</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +
                    "<initialHere>" +
                    "<tabLabel>i3</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>77</xPosition>" +
                    "<yPosition>718</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>3</pageNumber>" +
                    "<scaleValue>0.7</scaleValue>" +
                    "</initialHere>" +
                    "</initialHereTabs>" +
                    "<signHereTabs>" +
                    "<signHere>" +
                    "<xPosition>113</xPosition>" + // default unit is pixels
                    "<yPosition>432</yPosition>" + // default unit is pixels
                    "<documentId>1</documentId>" +
                    "<pageNumber>3</pageNumber>" +
                    "</signHere>" +
                    "</signHereTabs>" +
                    "</tabs>" +
                    "</signer>" +
                    "</signers>" +
                    "</recipients>" +
                    "</envelopeDefinition>";
            sendToDocusign(ms, "JVTemplate.pdf", xmlBody);
        }

        public void submitDocJSON()
        {
            _appType = "application/json";
            _docType = "application/pdf";
            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(@"C:\projects\hg\epifund\Inview.Epi.EpiFund.Web\Resources\JVTemplate.pdf");
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, ms);

            pdfMs.FormFlattening = true;
            pdfMs.Writer.CloseStream = false;

            if (pdfMs != null)
            {
                pdfMs.Close();
            }
            ms.Position = 0;
            pdfReader.Close();

            string jsonBody = "{\"emailSubject\":\"API Call for adding signature request to document and sending\"," +
                        "\"documents\":[{" +
                            "\"documentId\":\"1\"," +
                            "\"name\":\"JV Template\"}]," +
                            "\"recipients\":{" +
                                "\"signers\":[{" +
                                    "\"email\":\"" + "skyler.hamilton@inviewlabs.com" + "\"," +
                                    "\"name\":\"Name\"," +
                                    "\"recipientId\":\"1\"," +
                                    "\"tabs\":{" +
                                        "\"signHereTabs\":[{" +
                                            "\"xPosition\":\"100\"," +
                                            "\"yPosition\":\"100\"," +
                                            "\"documentId\":\"1\"," +
                        "\"pageNumber\":\"1\"" + "}]}}]}," +
                        "\"status\":\"sent\"}";

            sendToDocusign(ms, "JVTemplate.pdf", jsonBody);
        }

        public void submitBindingContingent()
        {
            DateTime dt = DateTime.Now;
            MemoryStream ms = new MemoryStream();
            PdfReader pdfReader = new PdfReader(@"C:\projects\hg\epifundTestDocs\Template-Binding Contingent LOI 7-27-14(static).pdf");
            pdfReader.RemoveUsageRights();
            PdfStamper pdfMs = new PdfStamper(pdfReader, ms);

            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField1[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField2[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField3[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField4[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField5[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField6[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField7[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField8[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField9[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField10[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField11[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField12[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField13[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField14[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField15[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField16[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField17[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField18[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField19[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField20[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField21[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField22[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField23[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField24[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField25[0]", "X");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page1[0].TextField26[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField27[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField28[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField29[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField30[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField31[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField32[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField33[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField34[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField35[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField36[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page2[0].TextField37[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField38[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField39[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField40[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField41[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField42[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page3[0].TextField43[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField44[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField45[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField46[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField47[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField48[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page4[0].TextField49[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField50[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField51[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField52[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField53[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField54[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField55[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField56[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField57[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField58[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField59[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField60[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page5[0].TextField61[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page6[0].TextField62[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page6[0].TextField63[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page6[0].TextField64[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page6[0].TextField65[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page6[0].TextField66[0]", "test");
            pdfMs.AcroFields.SetField("topmostSubform[0].Page6[0].TextField67[0]", "test");

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

                    "<signer>" +
                    "<recipientId>1</recipientId>" +
                    "<email>" + "skyler.hamilton@inviewlabs.com" + "</email>" +
                    "<name>" + "Name Name" + "</name>" +
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

                    "<initialHere>" +
                    "<tabLabel>BuyerInitial</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "<xPosition>466</xPosition>" +
                    "<yPosition>703</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>6</pageNumber>" +
                    "</initialHere>" +
                    "</initialHereTabs>" +


                    "<signHereTabs>" +
                    "<signHere>" +
                    "<xPosition>62</xPosition>" +
                    "<yPosition>384</yPosition>" +
                    "<documentId>1</documentId>" +
                    "<pageNumber>5</pageNumber>" +
                    "<tabLabel>BuyerSig</tabLabel>" +
                    "<recipientId>1</recipientId>" +
                    "</signHere>" +
                    "</signHereTabs>" +

                    "<dateSignedTabs>" +
                    "<dateSigned>" +
                    "<documentId>1</documentId>" +
                    "<xPosition>162</xPosition>" +
                    "<yPosition>384</yPosition>" +
                    "<pageNumber>5</pageNumber>" +
                    "<recipientId>1</recipientId>" +
                    "<tabLabel>BuyerDateSigned</tabLabel>" +
                    "</dateSigned>" +

                    "<dateSigned>" +
                    "<documentId>1</documentId>" +
                    "<xPosition>413</xPosition>" +
                    "<yPosition>386</yPosition>" +
                    "<pageNumber>5</pageNumber>" +
                    "<recipientId>1</recipientId>" +
                    "<tabLabel>BuyerDateSigned</tabLabel>" +
                    "</dateSigned>" +
                    "</dateSignedTabs>" +

                    "</tabs>" +
                    "</signer>" +

                    "</signers>" +
                    "</recipients>" +
                    "</envelopeDefinition>";




            sendToDocusign(ms, "BindingContingent.pdf", xmlBody);

        }

        public List<string> getDocumentsFromEnvelope(string envelopeId)
        {
            //commented version in test code
            List<string> documentNames = new List<string>();
            string baseURL = "";
            string url = "https://demo.docusign.net/restapi/v2/login_information";
            HttpWebRequest request = initializeRequest(url, "GET", null, "elizabeth.trambulo@inviewlabs.com", "7163bareef");
            string response = getResponseBody(request);
            baseURL = parseDataFromResponse(response, "baseUrl");

            url = baseURL + "/envelopes/" + envelopeId + "/documents";
            request = initializeRequest(url, "GET", null, "elizabeth.trambulo@inviewlabs.com", "7163bareef");
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
                        if (name.Contains(".pdf")) docsList.Add(name, uri);
                    }
                }
            }

            foreach (KeyValuePair<string, string> kvp in docsList)
            {
                url = baseURL + kvp.Value;
                request = initializeRequest(url, "GET", null, "elizabeth.trambulo@inviewlabs.com", "7163bareef");
                request.Accept = "application/pdf";
                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
                string path = @"C:\projects\trunk\hg\epifund\Documents\pdfs" + kvp.Key;
                using (MemoryStream ms = new MemoryStream())
                using (FileStream outfile = new FileStream(path, FileMode.Create))
                {
                    webResponse.GetResponseStream().CopyTo(ms);
                    if (ms.Length > int.MaxValue)
                    {
                        throw new NotSupportedException("Cannot write a file larger than 2GB.");
                    }
                    outfile.Write(ms.GetBuffer(), 0, (int)ms.Length);
                }
                // add key here
                documentNames.Add(kvp.Key);
            }
            return documentNames;
        }

        private string sendToDocusign(MemoryStream ms, string document, string xmlBody)
        {
            string url = "https://demo.docusign.net/restapi/v2/login_information";
            HttpWebRequest request = initializeRequest(url, "GET", null, "elizabeth.trambulo@inviewlabs.com", "7163bareef");
            string response = getResponseBody(request);
            string baseURL = parseDataFromResponse(response, "baseUrl");
            url = baseURL + "/envelopes";

            request = initializeRequest(url, "POST", null, "elizabeth.trambulo@inviewlabs.com", "7163bareef");
            configureMultiPartFormDataRequest(request, xmlBody, document, ms);
            response = getResponseBody(request);
            XDocument doc = XDocument.Load(new StringReader(response));
            var b = doc.Elements().First().Value.Replace("sent", "~").Split('~')[0];
            ms.Close();
            return b;
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
            HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(webResponse.GetResponseStream());
            string responseText = sr.ReadToEnd();
            return responseText;
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

        private string GetDaySuffix(int day)
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
            //return fieldNames;
            StringBuilder d = new StringBuilder();
            foreach (var n in fieldNames)
            {
                d.AppendLine(n + " ");
            }
            if (File.Exists(@"C:\testFile.txt"))
            {
                using (StreamWriter file = new StreamWriter(@"C:\testFile.txt", false))
                {
                    file.WriteLine(d.ToString());
                }
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
    }
}
