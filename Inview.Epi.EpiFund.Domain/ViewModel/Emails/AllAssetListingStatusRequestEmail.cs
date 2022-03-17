using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AllAssetListingStatusRequestEmail : Email
	{
		public string AssetDescription
		{
			get;
			set;
		}

		public string AssetListingStatus
		{
			get;
			set;
		}

		public int AssetNumber
		{
			get;
			set;
		}

		public string CC
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

		public AllAssetListingStatusRequestEmail()
		{
		}
	}
}