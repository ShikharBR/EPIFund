using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class ConfirmationFundingPackageSubmittalMBAEmail : Email
	{
		public string MBACorporateAddress
		{
			get;
			set;
		}

		public string MBACorporateName
		{
			get;
			set;
		}

		public string MBAEmailAddress
		{
			get;
			set;
		}

		public string MBAMemberName
		{
			get;
			set;
		}

		public string MBAPhoneNumber
		{
			get;
			set;
		}

		public ConfirmationFundingPackageSubmittalMBAEmail()
		{
		}
	}
}