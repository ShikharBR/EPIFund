using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace Inview.Epi.EpiFund.Domain
{
	public interface IPDFService
	{
		void CreateNCND(byte[] doc, NCNDTemplateModel model);

		List<ExtractedImageModel> GetBitmapImagesFromPDF(string pdf, string GuidId);

        List<ExtractedImageModel> GetBitmapImagesFromPDF(byte[] pdf, string guidId);


        void GetBitmapImagesFromPDFForLocalComp(byte[] pdf, string folderName);

		string GetDaySuffix(int day);

		string GetDocumentFromDocusign(string envelopeId, int userId, DocumentType type);

		List<string> GetFieldsFromForm(string path);

		MemoryStream GetICPdf(ICAgreementTemplateModel model, byte[] doc);

		MemoryStream GetIPABuyerPdf(IPABuyerTemplateModel model, byte[] doc);

		MemoryStream GetIPANARPdf(IPANARTemplateModel model, byte[] doc);

		MemoryStream GetIPASellerPdf(IPASellerTemplateModel model, byte[] doc);

		MemoryStream GetJVAPdf(JVAgreementTemplateModel model, byte[] doc);

		MemoryStream GetMDAPdf(MDATemplateModel model, byte[] doc);

		MemoryStream GetNARIPAPdf(MDATemplateModel model, byte[] doc);

		MemoryStream GetNCNDPdf(NCNDTemplateModel model, byte[] doc);

		MemoryStream GetPFSPdf(PersonalFinancialStatementTemplateModel model, byte[] doc);

		MemoryStream GetSellerIPAPdf(MDATemplateModel model, byte[] doc);

		MemoryStream PreviewLOI(BindingContingentTemplateModel model, byte[] doc);

        MemoryStream Submit1099(MiscellaneousIncomeTemplateModel model);

		void SubmitICAgreement(ICAgreementTemplateModel model, byte[] doc, int signingUserId);

		void SubmitIPABuyer(IPABuyerTemplateModel model, byte[] doc, int signingUserId, int documentType);

		void SubmitIPANAR(IPANARTemplateModel model, byte[] doc, int signingUserId, int documentType);

		void SubmitIPASeller(IPASellerTemplateModel model, byte[] doc, int signingUserId, int documentType);

		void SubmitJVAgreement(JVAgreementTemplateModel model, byte[] doc, int signingUserId);

		void SubmitLOI(BindingContingentTemplateModel model, byte[] doc, int signingUserId);

		void SubmitPFS(PersonalFinancialStatementTemplateModel model, byte[] doc, int signingUserId);
	}
}