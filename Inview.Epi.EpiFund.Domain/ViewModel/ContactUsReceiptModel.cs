using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ContactUsReceiptModel
	{
		[Display(Name="Date and Time of Inquiry")]
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

		[Display(Name="Inquiry")]
		public string Inquiry
		{
			get;
			set;
		}

		[Display(Name="Name of Person sending Inquiry")]
		public string Name
		{
			get;
			set;
		}

		[Display(Name="Phone Number of Person sending Inquiry")]
		public string PhoneNumber
		{
			get;
			set;
		}

		[Display(Name="Questions/Comments pertaining to")]
		public string Topics
		{
			get;
			set;
		}

		public ContactUsReceiptModel()
		{
		}
	}
}