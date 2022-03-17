using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace Inview.Epi.EpiFund.Business
{
	public class ContactService : IContactService
	{
		private IEPIContextFactory _factory;

		public ContactService(IEPIContextFactory factory)
		{
			this._factory = factory;
		}

		public ContactUsReceiptModel GetReceipt(int InquiryId)
		{
			Inquiry inquiry = this._factory.Create().Inquiries.Single<Inquiry>((Inquiry s) => s.InquiryId == InquiryId);
			ContactUsReceiptModel contactUsReceiptModel = new ContactUsReceiptModel()
			{
				DateOfInquiry = inquiry.DateOfInquiry,
				Inquiry = inquiry.Comments,
				Name = inquiry.Name,
				Email = inquiry.EmailAddress,
				Topics = inquiry.Topics,
				PhoneNumber = inquiry.ContactNumber
			};
			return contactUsReceiptModel;
		}

		public void MarkAsResponded(int InquiryId)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			Inquiry inquiry = ePIRepository.Inquiries.Single<Inquiry>((Inquiry s) => s.InquiryId == InquiryId);
			inquiry.Responded = true;
			ePIRepository.Save();
		}

		public int SaveInquiry(ContactUsModel model)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			Inquiry inquiry = new Inquiry()
			{
				ContactNumber = model.ContactNumber,
				EmailAddress = model.EmailAddress,
				Comments = model.QuestionsComments,
				Name = model.Name,
				DateOfInquiry = model.DateOfInquiry,
				Responded = false
			};
			Inquiry str = inquiry;
			StringBuilder stringBuilder = new StringBuilder();
			model.SelectedTopics.ForEach((string f) => {
				stringBuilder.Append(f);
				stringBuilder.Append("; ");
			});
			str.Topics = stringBuilder.ToString();
			ePIRepository.Inquiries.Add(str);
			ePIRepository.Save();
			return str.InquiryId;
		}
	}
}