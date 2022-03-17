using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class UserWelcomeEmail : Email
	{
		public string Password
		{
			get;
			set;
		}

		public string RegistrationType
		{
			get;
			set;
		}

		public string To
		{
			get;
			set;
		}

		public UserWelcomeEmail()
		{
		}
	}
}