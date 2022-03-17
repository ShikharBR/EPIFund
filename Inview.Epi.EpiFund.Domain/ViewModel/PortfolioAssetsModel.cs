using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class PortfolioAssetsModel
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

		public string City
		{
			get;
			set;
		}

		public string CreatedBy
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

		public PortfolioAssetsModel()
		{
		}
	}
}