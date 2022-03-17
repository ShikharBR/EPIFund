using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AssetDescriptionModel
	{
		public string APNOneLine
		{
			get;
			set;
		}

		public string AssetAddressOneLineFormattedString
		{
			get;
			set;
		}

		public Guid AssetId
		{
			get;
			set;
		}

		public int AssetNumber
		{
			get;
			set;
		}

		public string CityStateFormattedString
		{
			get;
			set;
		}

		public string CorporateOwnershipOfficer
		{
			get;
			set;
		}

		public ListingStatus CurrentListingStatus
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public bool HasCSAAgreement
		{
			get;
			set;
		}

		public string ProjectName
		{
			get;
			set;
		}

		public bool Show
		{
			get;
			set;
		}

		public AssetDescriptionModel()
		{
		}
	}
}