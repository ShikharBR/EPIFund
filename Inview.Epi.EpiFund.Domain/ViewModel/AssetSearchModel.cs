using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AssetSearchModel
	{
		public double? AccListPrice
		{
			get;
			set;
		}

		public double? AccUnits
		{
			get;
			set;
		}

		public List<string> AssetIds
		{
			get;
			set;
		}

		public string AssetName
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public string Grade
		{
			get;
			set;
		}

		public int? MaxAgeRange
		{
			get;
			set;
		}

		public double? MaxPriceRange
		{
			get;
			set;
		}

		public int? MaxSquareFeet
		{
			get;
			set;
		}

		public int? MaxUnitsSpaces
		{
			get;
			set;
		}

		public double? MinPriceRange
		{
			get;
			set;
		}

		public int? MinSquareFeet
		{
			get;
			set;
		}

		public int? MinUnitsSpaces
		{
			get;
			set;
		}

		public AssetCategory? SelectedAssetCategory
		{
			get;
			set;
		}

		public AssetType? SelectedAssetType
		{
			get;
			set;
		}

		public ListingStatus? SelectedListingStatus
		{
			get;
			set;
		}

		public List<StateSearchCriteriaModel> SelectedStates
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public bool TurnKey
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.UserType? UserType
		{
			get;
			set;
		}

        public bool IsPaper
        {
            get;
            set;
        }

		public AssetSearchModel()
		{
		}
	}
}