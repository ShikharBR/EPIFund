using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class ICAdminChangeStatusEmail : Email
	{
		public string Name
		{
			get;
			set;
		}

		public string To
		{
			get;
			set;
		}

		public ICAdminChangeStatusEmail()
		{
		}
	}
}