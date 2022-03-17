using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class SellerAssetQuickListModel
	{
		public string AddressLine1
		{
			get;
			set;
		}

		public Guid AssetId
		{
			get;
			set;
		}

		public string AssetName
		{
			get;
			set;
		}

		public int AssetNumber
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

		public string CreatedBy
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public bool IsOnHold
		{
			get;
			set;
		}

		public bool IsSampleAsset
		{
			get;
			set;
		}

		public bool IsSelected
		{
			get;
			set;
		}

		public int NumberOfUnits
		{
			get;
			set;
		}

		public string Show
		{
			get;
			set;
		}

		public double ShowablePrice
		{
			get;
			set;
		}

		public int SquareFeet
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public string Status
		{
			get;
			set;
		}

		public string Type
		{
			get;
			set;
		}

		public string Zip
		{
			get;
			set;
		}

		public SellerAssetQuickListModel()
		{
		}
	}
}