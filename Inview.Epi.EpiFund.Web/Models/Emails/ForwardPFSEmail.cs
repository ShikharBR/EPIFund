using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class ForwardPFSEmail : Email
	{
		public string CompanyName
		{
			get;
			set;
		}

		public string ForwardingEmailAddress
		{
			get;
			set;
		}

		public string OfficerName
		{
			get;
			set;
		}

		public string OwningUserName
		{
			get;
			set;
		}

		public ForwardPFSEmail()
		{
		}
	}
}