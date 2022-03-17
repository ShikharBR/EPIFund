using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class AuthenticateComputerEmail : Email
	{
		public string Code
		{
			get;
			set;
		}

		public string IP
		{
			get;
			set;
		}

		public string To
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public AuthenticateComputerEmail()
		{
		}
	}
}