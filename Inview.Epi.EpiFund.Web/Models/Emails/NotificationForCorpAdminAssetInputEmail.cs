using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class NotificationForCorpAdminAssetInputEmail : Postal.Email
	{
		public string AssetDescription
		{
			get;
			set;
		}

		public string AssetNumber
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public string ICAdminName
		{
			get;
			set;
		}

		public string To
		{
			get;
			set;
		}

		public NotificationForCorpAdminAssetInputEmail()
		{
		}
	}
}