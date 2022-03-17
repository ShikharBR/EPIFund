using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class UserReferralSubmittedEmail : Email
	{
		public string ReferrerEmail
		{
			get;
			set;
		}

		public string To
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public UserReferralSubmittedEmail()
		{
		}
	}
}