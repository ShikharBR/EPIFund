using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class PaperCommercialAsset
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

		public string AgentAccountNumber
		{
			get;
			set;
		}

		public string AgentAddress
		{
			get;
			set;
		}

		public string AgentCity
		{
			get;
			set;
		}

		public string AgentContactPerson
		{
			get;
			set;
		}

		public string AgentEmail
		{
			get;
			set;
		}

		public string AgentName
		{
			get;
			set;
		}

		public string AgentPhone
		{
			get;
			set;
		}

		public string AgentState
		{
			get;
			set;
		}

		public string AgentZip
		{
			get;
			set;
		}

		public string AmortType
		{
			get;
			set;
		}

		public double? AnnualGOE
		{
			get;
			set;
		}

		public double? AnnualGOI
		{
			get;
			set;
		}

		public double? AnnualVF
		{
			get;
			set;
		}

		public int? ApartmentUnits
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

		public bool? AuthorizeForwardPaymentHistory
		{
			get;
			set;
		}

		public DateTime? BalloonDateOfNote
		{
			get;
			set;
		}

		public double BaseRentPerSqFtMajorTenant
		{
			get;
			set;
		}

		public double? BPOOfProperty
		{
			get;
			set;
		}

		public int BuildingsCount
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

		public double CurrentBpo
		{
			get;
			set;
		}

		public double CurrentMarkerRentPerSqFt
		{
			get;
			set;
		}

		public double? CurrentNotePrincipal
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

		public string Fax
		{
			get;
			set;
		}

		public double? FirstInterestRateWRAP
		{
			get;
			set;
		}

		public double? FirstmortgageBalanceWRAP
		{
			get;
			set;
		}

		public string FirstMortgageCompany
		{
			get;
			set;
		}

		public double? FirstMortgagePaymentWRAP
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

		public string HasCopyOfAppraisal
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

		public bool? HasProformaInformation
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

		public bool? IsNoteCurrent
		{
			get;
			set;
		}

		public bool? IsNoteWRAP
		{
			get;
			set;
		}

		public bool isPendingForeclosure
		{
			get;
			set;
		}

		public DateTime? LastPaymentRecievedOnNote
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

		public string MethodOfAppraisal
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

		public DateTime? NextPaymentOnNote
		{
			get;
			set;
		}

		public double? NoteInterestRate
		{
			get;
			set;
		}

		public DateTime? NoteOrigination
		{
			get;
			set;
		}

		public string NotePayerCity
		{
			get;
			set;
		}

		public string NotePayerContactAddress
		{
			get;
			set;
		}

		public string NotePayerEmail
		{
			get;
			set;
		}

		public string NotePayerFax
		{
			get;
			set;
		}

		public string NotePayerFICO
		{
			get;
			set;
		}

		public string NotePayerFullName
		{
			get;
			set;
		}

		public string NotePayerName
		{
			get;
			set;
		}

		public string NotePayerPhoneCell
		{
			get;
			set;
		}

		public string NotePayerPhoneHome
		{
			get;
			set;
		}

		public string NotePayerPhoneWork
		{
			get;
			set;
		}

		public string NotePayerSSNOrTIN
		{
			get;
			set;
		}

		public string NotePayerState
		{
			get;
			set;
		}

		public string NotePayerZip
		{
			get;
			set;
		}

		public double? NotePrincipal
		{
			get;
			set;
		}

		public bool? NoteServicedByAgent
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

		public double? OriginalPrincipalBalanceWRAP
		{
			get;
			set;
		}

		public int PaperCommercialAssetId
		{
			get;
			set;
		}

		public int ParkingSpaces
		{
			get;
			set;
		}

		public double? PaymentAmount
		{
			get;
			set;
		}

		public string PaymentFrequency
		{
			get;
			set;
		}

		public string PaymentHistory
		{
			get;
			set;
		}

		public string PaymentIncludes
		{
			get;
			set;
		}

		public int? PaymentsMadeOnNote
		{
			get;
			set;
		}

		public int? PaymentsRemainingOnNote
		{
			get;
			set;
		}

		public string PowerService
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

		public string PropertyAccess
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

		public double? PropertyGOE
		{
			get;
			set;
		}

		public double? PropertyGOI
		{
			get;
			set;
		}

		public int? PropertySquareFeet
		{
			get;
			set;
		}

		public string PropertyState
		{
			get;
			set;
		}

		public double? PropertyVF
		{
			get;
			set;
		}

		public string PropertyZip
		{
			get;
			set;
		}

		public string RecentUpgradesRenovations
		{
			get;
			set;
		}

		public int RentableSquareFeet
		{
			get;
			set;
		}

		public double? SecondInterestRateWRAP
		{
			get;
			set;
		}

		public double? SecondMortgageBalanceWRAP
		{
			get;
			set;
		}

		public double? SecondMortgagePaymentWRAP
		{
			get;
			set;
		}

		public string SecuringPropertyAddress
		{
			get;
			set;
		}

		public double? SecuringPropertyAppraisal
		{
			get;
			set;
		}

		public string SecuringPropertyCity
		{
			get;
			set;
		}

		public string SecuringPropertyCounty
		{
			get;
			set;
		}

		public string SecuringPropertyState
		{
			get;
			set;
		}

		public string SecuringPropertyZip
		{
			get;
			set;
		}

		public double? SellerCarryNoteCashDown
		{
			get;
			set;
		}

		public double? SellerCarryNotePrice
		{
			get;
			set;
		}

		public DateTime? SellerCarryNoteSalesDate
		{
			get;
			set;
		}

		public string SewerService
		{
			get;
			set;
		}

		public int? Spaces
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

		public double? TotalMonthlyPaymentWRAP
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

		public string TypeOfMTGInstrument
		{
			get;
			set;
		}

		public string TypeOfNote
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

		public bool? WasPropertyDistressed
		{
			get;
			set;
		}

		public string WaterService
		{
			get;
			set;
		}

		public string WorkPhone
		{
			get;
			set;
		}

		public int? YearBuilt
		{
			get;
			set;
		}

		public string Zip
		{
			get;
			set;
		}

		public PaperCommercialAsset()
		{
		}
	}
}