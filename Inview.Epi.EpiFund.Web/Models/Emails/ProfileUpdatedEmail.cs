using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class ProfileUpdatedEmail : Postal.Email
	{
		public string Email
		{
			get;
			set;
		}

		public string NameOfRegistrant
		{
			get;
			set;
		}

		public ProfileUpdatedEmail()
		{
		}
	}
}