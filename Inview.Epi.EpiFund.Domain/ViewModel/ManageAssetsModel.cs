using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ManageAssetsModel
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

		public string AddressLine1
		{
			get;
			set;
		}

		public string AssetName
		{
			get;
			set;
		}

		public string AssetNumber
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.ViewModel.AssetType AssetType
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public UserType ControllingUserType
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public DateTime? EndDate
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

		public DateTime? StartDate
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public int? UserId
		{
			get;
			set;
		}

		public string ZipCode
		{
			get;
			set;
		}

        public bool IsPaper
        {
            get;
            set;
        }

        public string ApnNumber
        {
            get;
            set;
        }

		public ManageAssetsModel()
		{
		}
	}
}