using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AssetQuickListViewModel
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

		public bool CanViewAssetName
		{
			get;
			set;
		}

		public string CAP
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public UserType? ControllingUserType
		{
			get;
			set;
		}

		public string ImageContentType
		{
			get;
			set;
		}

		public string ImageFileName
		{
			get;
			set;
		}

		public bool IsPartofPF
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

		public string OCC
		{
			get;
			set;
		}

		public Guid PortfolioId
		{
			get;
			set;
		}

		public string PortfolioName
		{
			get;
			set;
		}

		public string Pricing
		{
			get;
			set;
		}

		public string SGI
		{
			get;
			set;
		}

		public bool Show
		{
			get;
			set;
		}

		public bool ShowDeleteAsset
		{
			get;
			set;
		}

		public bool ShowEditAsset
		{
			get;
			set;
		}

		public bool ShowEscrowAsset
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

		public int Year
		{
			get;
			set;
		}

		public string Zip
		{
			get;
			set;
		}

		public string Zoning
		{
			get;
			set;
		}

		public AssetQuickListViewModel()
		{
		}
	}
}