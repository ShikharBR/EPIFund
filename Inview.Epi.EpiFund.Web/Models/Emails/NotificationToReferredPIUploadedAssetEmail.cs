using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class NotificationToReferredPIUploadedAssetEmail : Email
	{
		public DateTime Date
		{
			get;
			set;
		}

		public string PrincipalInvestorName
		{
			get;
			set;
		}

		public string ReferrerName
		{
			get;
			set;
		}

		public string To
		{
			get;
			set;
		}

		public string UploadedAssets
		{
			get;
			set;
		}

		public NotificationToReferredPIUploadedAssetEmail()
		{
		}
	}
}