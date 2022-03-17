using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class CompanyUserWelcomeEmailAutoActivatedEmail : Postal.Email
	{
		public string ActivatedByCorpAdmin
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public string RegistrationType
		{
			get;
			set;
		}

		public string TempPassword
		{
			get;
			set;
		}

		public string To
		{
			get;
			set;
		}

		public string URL
		{
			get;
			set;
		}

		public CompanyUserWelcomeEmailAutoActivatedEmail()
		{
		}
	}
}