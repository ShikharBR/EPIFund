using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class RealEstateCommercialAsset
	{
		public string AccountNumber
		{
			get;
			set;
		}

		public string AcronymForCorporateEntity
		{
			get;
			set;
		}

		public string AdditionalInformation
		{
			get;
			set;
		}

		public double? AnnualPropertyTaxes
		{
			get;
			set;
		}

		public double? AskingSalePrice
		{
			get;
			set;
		}

		public int AssetSellerId
		{
			get;
			set;
		}

		public double? AverageAdjustmentToBaseRent
		{
			get;
			set;
		}

		public double BaseRentPerSqFtMajorTenant
		{
			get;
			set;
		}

		public double BuildingsCount
		{
			get;
			set;
		}

		public string CellPhone
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public string CorporateAddress1
		{
			get;
			set;
		}

		public string CorporateAddress2
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.CorporateEntityType CorporateEntityType
		{
			get;
			set;
		}

		public string CorporateName
		{
			get;
			set;
		}

		public string CorporateTitle
		{
			get;
			set;
		}

		public int CoveredParkingSpaces
		{
			get;
			set;
		}

		public double? CurrentAnnualIncome
		{
			get;
			set;
		}

		public double? CurrentAnnualOperatingExepenses
		{
			get;
			set;
		}

		public double CurrentBpo
		{
			get;
			set;
		}

		public double? CurrentCalendarYearToDateCashFlow
		{
			get;
			set;
		}

		public double CurrentMarkerRentPerSqFt
		{
			get;
			set;
		}

		public string CurrentOperatingVacancyFactor
		{
			get;
			set;
		}

		public double? CurrentPrincipalBalance
		{
			get;
			set;
		}

		public double? CurrentVacancyFactor
		{
			get;
			set;
		}

		public MeteringMethod ElectricMeterMethod
		{
			get;
			set;
		}

		public string EmailAddress
		{
			get;
			set;
		}

		public string EnvironmentalIssues
		{
			get;
			set;
		}

		public int EstDeferredMaintenance
		{
			get;
			set;
		}

		public string Fax
		{
			get;
			set;
		}

		public string FirstMortgageCompany
		{
			get;
			set;
		}

		public string ForeclosureLender
		{
			get;
			set;
		}

		public DateTime? ForeclosureOriginalMortageDate
		{
			get;
			set;
		}

		public double? ForeclosureOriginalMortgageAmount
		{
			get;
			set;
		}

		public LenderPosition ForeclosurePosition
		{
			get;
			set;
		}

		public DateTime? ForeclosureRecordDate
		{
			get;
			set;
		}

		public string ForeclosureRecordNumber
		{
			get;
			set;
		}

		public DateTime? ForeclosureSaleDate
		{
			get;
			set;
		}

		public MeteringMethod GasMeterMethod
		{
			get;
			set;
		}

		public string GradeClassification
		{
			get;
			set;
		}

		public Guid GuidId
		{
			get;
			set;
		}

		public bool HasAAARatedMajorTenant
		{
			get;
			set;
		}

		public bool HasDeferredMaintenance
		{
			get;
			set;
		}

		public bool? HasEnvironmentalIssues
		{
			get;
			set;
		}

		public bool HasIncome
		{
			get;
			set;
		}

		public PositionMortgageType HasPositionMortgage
		{
			get;
			set;
		}

		public double? InterestRate
		{
			get;
			set;
		}

		public bool IsCertificateOfGoodStandingAvailable
		{
			get;
			set;
		}

		public string IsDebtorInAForeclosureAction
		{
			get;
			set;
		}

		public bool IsMajorTenantAAARated
		{
			get;
			set;
		}

		public bool? IsMortgageAnARM
		{
			get;
			set;
		}

		public bool isPendingForeclosure
		{
			get;
			set;
		}

		public DateTime? LastReportedDate
		{
			get;
			set;
		}

		public int LeasedSquareFootageByMajorTenant
		{
			get;
			set;
		}

		public string LenderPhone
		{
			get;
			set;
		}

		public string LenderPhoneOther
		{
			get;
			set;
		}

		public string LotNumber
		{
			get;
			set;
		}

		public string LotSize
		{
			get;
			set;
		}

		public string MajorTenant
		{
			get;
			set;
		}

		public string MFDetailsString
		{
			get;
			set;
		}

		public double? MonthlyPayment
		{
			get;
			set;
		}

		public string MortgageAdjustIfARM
		{
			get;
			set;
		}

		public string MortgageCompanyAddress
		{
			get;
			set;
		}

		public string MortgageCompanyCity
		{
			get;
			set;
		}

		public string MortgageCompanyState
		{
			get;
			set;
		}

		public string MortgageCompanyZip
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.MortgageLienAssumable? MortgageLienAssumable
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.MortgageLienType? MortgageLienType
		{
			get;
			set;
		}

		public string MotivationToLiquidate
		{
			get;
			set;
		}

		public string NameOfAAARatedMajorTenant
		{
			get;
			set;
		}

		public string NameOfCoPrincipal
		{
			get;
			set;
		}

		public string NameOfPrincipal
		{
			get;
			set;
		}

		public string NeededMaintenance
		{
			get;
			set;
		}

		public int? NumberOfMissedPayments
		{
			get;
			set;
		}

		public int NumberofSuites
		{
			get;
			set;
		}

		public int NumberOfTenants
		{
			get;
			set;
		}

		public DateTime? OccupancyDate
		{
			get;
			set;
		}

		public float OccupancyPercentage
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.OccupancyType OccupancyType
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.OperatingStatus OperatingStatus
		{
			get;
			set;
		}

		public int ParkingSpaces
		{
			get;
			set;
		}

		public string PaymentIncludes
		{
			get;
			set;
		}

		public string PreferredContactTimes
		{
			get;
			set;
		}

		public string PreferredMethods
		{
			get;
			set;
		}

		public double ProformaAnnualIncome
		{
			get;
			set;
		}

		public int ProformaAnnualNoi
		{
			get;
			set;
		}

		public double ProformaAnnualOperExpenses
		{
			get;
			set;
		}

		public double ProformaMonthlyIncome
		{
			get;
			set;
		}

		public int ProformaSgi
		{
			get;
			set;
		}

		public double ProformaVacancyFac
		{
			get;
			set;
		}

		public string ProjectName
		{
			get;
			set;
		}

		public string PropertyAddress
		{
			get;
			set;
		}

		public string PropertyCity
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.PropertyCondition PropertyCondition
		{
			get;
			set;
		}

		public string PropertyCounty
		{
			get;
			set;
		}

		public string PropertyDetailsString
		{
			get;
			set;
		}

		public string PropertyState
		{
			get;
			set;
		}

		public string PropertyZip
		{
			get;
			set;
		}

		public int RealEstateCommercialAssetId
		{
			get;
			set;
		}

		public string RecentUpgradesRenovations
		{
			get;
			set;
		}

		public int? RentableSquareFeet
		{
			get;
			set;
		}

		public string SaleDateIfForeclosing
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public string StateOfOriginCorporateEntity
		{
			get;
			set;
		}

		public string Subdivision
		{
			get;
			set;
		}

		public string TaxAssessorNumber
		{
			get;
			set;
		}

		public string TaxAssessorNumberOther
		{
			get;
			set;
		}

		public string TaxBookMap
		{
			get;
			set;
		}

		public string Terms
		{
			get;
			set;
		}

		public int? TotalRentableFeet
		{
			get;
			set;
		}

		public int? TotalRentableFeetAllApt
		{
			get;
			set;
		}

		public int TotalUnits
		{
			get;
			set;
		}

		public CommercialType Type
		{
			get;
			set;
		}

		public string TypeOfProperty
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.VacantSuites VacantSuites
		{
			get;
			set;
		}

		public string WorkPhone
		{
			get;
			set;
		}

		public int YearBuilt
		{
			get;
			set;
		}

		public string Zip
		{
			get;
			set;
		}

		public RealEstateCommercialAsset()
		{
		}
	}
}