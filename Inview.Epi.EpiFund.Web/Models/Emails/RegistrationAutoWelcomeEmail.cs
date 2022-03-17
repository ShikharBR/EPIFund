using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class RegistrationAutoWelcomeEmail : Postal.Email
	{
		public string AssetId
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public string Code
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

		public string State
		{
			get;
			set;
		}

		public string To
		{
			get;
			set;
		}

		public RegistrationAutoWelcomeEmail()
		{
		}
	}
}