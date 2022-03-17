using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class AutoRegistrationAutoWelcomeEmail : Postal.Email
	{
		public string Email
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public string To
		{
			get;
			set;
		}

		public AutoRegistrationAutoWelcomeEmail()
		{
		}
	}
}