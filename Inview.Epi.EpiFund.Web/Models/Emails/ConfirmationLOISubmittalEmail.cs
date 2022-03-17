using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class ConfirmationLOISubmittalEmail : Email
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

		public string MBAEmail
		{
			get;
			set;
		}

		public string MBAMember
		{
			get;
			set;
		}

		public string MBAPhone
		{
			get;
			set;
		}

		public string UserEmail
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public ConfirmationLOISubmittalEmail()
		{
		}
	}
}