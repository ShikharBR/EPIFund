using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class AssetChangeStatusModel
	{
		public string AssetDescription
		{
			get;
			set;
		}

		public string AssetId
		{
			get;
			set;
		}

		public string NewStatus
		{
			get;
			set;
		}

		public string OriginalStatus
		{
			get;
			set;
		}

		public AssetChangeStatusModel()
		{
		}
	}
}