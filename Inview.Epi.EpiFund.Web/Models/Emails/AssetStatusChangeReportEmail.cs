using Postal;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class AssetStatusChangeReportEmail : Email
	{
		public List<AssetChangeStatusModel> AssetChanges
		{
			get;
			set;
		}

		public string RecipientEmail
		{
			get;
			set;
		}

		public string RecipientName
		{
			get;
			set;
		}

		public AssetStatusChangeReportEmail()
		{
		}
	}
}