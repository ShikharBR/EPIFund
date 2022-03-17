using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class ConfirmationReceiptContactUsEmail : Postal.Email
	{
		public string ContactNumber
		{
			get;
			set;
		}

		public DateTime DateOfInquiry
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public string EmailOfPersonSendingInquiry
		{
			get;
			set;
		}

		public string Inquiry
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string To
		{
			get;
			set;
		}

		public string Topics
		{
			get;
			set;
		}

		public ConfirmationReceiptContactUsEmail()
		{
		}
	}
}