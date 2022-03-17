using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class ConfirmationImportPrincipalInvestorsEmail : Email
	{
		public string EmailAddress
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public int ResultCount
		{
			get;
			set;
		}

		public string ResultString
		{
			get;
			set;
		}

		public ConfirmationImportPrincipalInvestorsEmail()
		{
		}
	}
}