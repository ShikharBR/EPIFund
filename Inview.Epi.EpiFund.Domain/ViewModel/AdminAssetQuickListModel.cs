using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AdminAssetQuickListModel
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

		public bool isSpecificType
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

		public AdminAssetQuickListModel()
		{
		}

		public double CurrentVacancyFac
		{
			get;
			set;
		}
		public DateTime? LastReportedOccupancyDate
		{
			get;
			set;
		}
		public DateTime? OccupancyDate
		{
			get;
			set;
		}
		public double ProformaAnnualIncome
		{
			get;
			set;
		}
		public double ProformaNOI
		{
			get;
			set;
		}
		public float CashInvestmentApy
		{
			get;
			set;
		}
		public double capRate
		{
			get;
			set;
		}
		public double AskingPrice
		{
			get;
			set;
		}
		public double CurrentBpo
		{
			get;
			set;
		}
		public bool Portfolio
		{
			get;
			set;
		}

		public string AssmFin
		{
			get;
			set;
		}

		public UserType UserType
		{
			get;
			set;
		}
		public ListingStatus ListingStatus
		{
			get;
			set;
		}
		public bool IsActive
		{
			get;
			set;
		}

		public string BusDriver
		{
			get;
			set;
		}

	}
	public class PortfolioQuickListModel
	{
		public Guid PortfolioId
		{
			get;
			set;
		}
		public List<string> States
		{
			get;
			set;
		}
		public List<AssetType> AssetType
		{
			get;
			set;
		}

		public int UnitsSqFt
		{
			get;
			set;
		}
		public string PortfolioName
		{
			get;
			set;
		}

		public double OccupancyPercentage
		{
			get;
			set;
		}
		public string OccupancyDate
		{
			get;
			set;
		}

		public string CumiProformaSGI
		{
			get;
			set;
		}
		public string CumiProformaNOI
		{
			get;
			set;
		}
		public string CumiLPCapRate
		{
			get;
			set;
		}
		public double Pricing
		{
			get;
			set;
		}
		public string PricingType
		{
			get;
			set;
		}

		public string AssmFin
		{
			get;
			set;
		}

		public int? NumberOfUnits
		{
			get;
			set;
		}
		public double SquareFeet
		{
			get;
			set;
		}
		public UserType UserType
		{
			get;
			set;
		}
		public ListingStatus ListingStatus
		{
			get;
			set;
		}
		public string BusDriver
		{
			get;
			set;
		}

		public int? NumberOfAssets
		{
			get;
			set;
		}
	}
	public class ChainOfTitleQuickListModel
	{
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
		public string State
		{
			get;
			set;
		}

		public string Type
		{
			get;
			set;
		}
		public int SquareFeet
		{
			get;
			set;
		}
		public int NumberOfUnits
		{
			get;
			set;
		}
		public string County
		{
			get;
			set;
		}
		public string Show
		{
			get;
			set;
		}

		public DateTime? Date
		{
			get;
			set;
		}
		public double Pricing
		{
			get;
			set;
		}
		public string Terms
		{
			get;
			set;
		}
		public double CAP
		{
			get;
			set;
		}

		public Guid? HCID
		{
			get;
			set;
		}
		public Guid? OCID
		{
			get;
			set;
		}

		public string HCName
		{
			get;
			set;
		}
		public string OCName
		{
			get;
			set;
		}

		public bool Portfolio
		{
			get;
			set;
		}

		public UserType UserType
		{
			get;
			set;
		}
		public ListingStatus ListingStatus
		{
			get;
			set;
		}
		public bool IsActive
		{
			get;
			set;
		}
		public string BusDriver
		{
			get;
			set;
		}

	}
}