using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class Inquiry
	{
		public string Comments
		{
			get;
			set;
		}

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

		public string EmailAddress
		{
			get;
			set;
		}

		public int InquiryId
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public bool Responded
		{
			get;
			set;
		}

		public string Topics
		{
			get;
			set;
		}

		public Inquiry()
		{
		}
	}
}