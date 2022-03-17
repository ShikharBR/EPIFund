using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AssetListingStatusRequestEmail : Email
	{
		public string AssetDescription
		{
			get;
			set;
		}

		public int AssetNumber
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

		public string UserViews
		{
			get;
			set;
		}

		public AssetListingStatusRequestEmail()
		{
		}
	}
}