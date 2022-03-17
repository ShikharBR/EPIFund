using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class ConfirmationImportNARMembersEmail : Email
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

		public string Results
		{
			get;
			set;
		}

		public ConfirmationImportNARMembersEmail()
		{
		}
	}
}