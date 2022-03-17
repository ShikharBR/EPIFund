using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class ConfirmationPISellerAssetIsPublishedEmail : Email
	{
		public string APN
		{
			get;
			set;
		}

		public string AssetAddressOneLine
		{
			get;
			set;
		}

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

		public string CorpOfficer
		{
			get;
			set;
		}

		public DateTime DatePublished
		{
			get;
			set;
		}

		public string SellerName
		{
			get;
			set;
		}

		public string To
		{
			get;
			set;
		}

		public string VestingEntity
		{
			get;
			set;
		}

		public ConfirmationPISellerAssetIsPublishedEmail()
		{
		}
	}
}