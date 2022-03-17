using Inview.Epi.EpiFund.Domain.ViewModel;
using System;

namespace Inview.Epi.EpiFund.Domain
{
	public interface IContactService
	{
		ContactUsReceiptModel GetReceipt(int InquiryId);

		void MarkAsResponded(int InquiryId);

		int SaveInquiry(ContactUsModel model);
	}
}