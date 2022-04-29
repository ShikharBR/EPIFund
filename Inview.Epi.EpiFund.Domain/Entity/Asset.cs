using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class Asset
	{
		public AccessRoadType? AccessRoadTypeId
		{
			get;
			set;
		}

		public string AccountNumber
		{
			get;
			set;
		}

		public DateTime? ActualClosingDate
		{
			get;
			set;
		}

		public DateTime? ActualCOEDate
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

		public int? AmortizationSchedule
		{
			get;
			set;
		}

		public string AmortType
		{
			get;
			set;
		}

        public string AmortOther
        {
            get;
            set;
        }

        public double AnnualGrossIncome
		{
			get;
			set;
		}

		public double AnnualPropertyTax
		{
			get;
			set;
		}

        public string AppraisalOther
        {
            get;
            set;
        }

        public bool Approved
		{
			get;
			set;
		}

		public double AskingPrice
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.AssetCategory AssetCategory
		{
			get;
			set;
		}

		[Key]
		public Guid AssetId
		{
			get;
			set;
		}

		public virtual IList<AssetNARMember> AssetNARMembers
		{
			get;
			set;
		}

		public int AssetNumber
		{
			get;
			set;
		}

		public int? AssetSellerId
		{
			get;
			set;
		}

		public virtual List<AssetTaxParcelNumber> AssetTaxParcelNumbers
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.ViewModel.AssetType AssetType
		{
			get;
			set;
		}

		public string AssetTypeAbbreviation
		{
			get
			{
				string str;
				switch (this.AssetType)
				{
					case Inview.Epi.EpiFund.Domain.ViewModel.AssetType.Retail:
					{
						str = "Com'l - R";
						break;
					}
					case Inview.Epi.EpiFund.Domain.ViewModel.AssetType.Office:
					{
						str = "Com'l - O";
						break;
					}
					case Inview.Epi.EpiFund.Domain.ViewModel.AssetType.MultiFamily:
					{
						str = "MF";
						break;
					}
					case Inview.Epi.EpiFund.Domain.ViewModel.AssetType.Industrial:
					{
						str = "Com'l - I";
						break;
					}
					case Inview.Epi.EpiFund.Domain.ViewModel.AssetType.MHP:
					{
						str = "MHP";
						break;
					}
					case Inview.Epi.EpiFund.Domain.ViewModel.AssetType.ConvenienceStoreFuel:
					{
						str = "Com'l - S";
						break;
					}
					case Inview.Epi.EpiFund.Domain.ViewModel.AssetType.Medical:
					{
						str = "Com'l - M";
						break;
					}
					case Inview.Epi.EpiFund.Domain.ViewModel.AssetType.MixedUse:
					{
						str = "MU";
						break;
					}
					case Inview.Epi.EpiFund.Domain.ViewModel.AssetType.Retail | Inview.Epi.EpiFund.Domain.ViewModel.AssetType.MixedUse:
					{
						str = "";
						break;
					}
					case Inview.Epi.EpiFund.Domain.ViewModel.AssetType.Other:
					{
						str = "Com'l - Ot";
						break;
					}
					default:
					{
						goto case Inview.Epi.EpiFund.Domain.ViewModel.AssetType.Retail | Inview.Epi.EpiFund.Domain.ViewModel.AssetType.MixedUse;
					}
				}
				return str;
			}
		}

        public string AssetTypeAbbreviationForExport
        {
            get
            {
                switch (this.AssetType) {
                    case AssetType.MultiFamily:
                        return "MF";
                    case AssetType.FracturedCondominiumPortfolio:
                        return "FCP";
                    case AssetType.MHP:
                        return "MHP";
                    case AssetType.Retail:
                        return "Ret";
                    case AssetType.Office:
                        return "Off";
                    case AssetType.Industrial:
                        return "Indus";
                    case AssetType.Medical:
                        return "Med";
                    case AssetType.ConvenienceStoreFuel:
                        return "F/S";
                    case AssetType.Hotel:
                        return "R/H/M";
                    case AssetType.ParkingGarageProperty:
                        return "PG";
                    case AssetType.MiniStorageProperty:
                        return "MS";
                    case AssetType.MixedUse:
                        return "Mix";
                    case AssetType.Land:
                        return "Land";
                    default:
                        return "";
                }
            }
        }
        
		public DateTime? AuctionDate
		{
			get;
			set;
		}

		public bool? AuthorizeForwardPaymentHistory
		{
			get;
			set;
		}

		public double? AverageAdjustmentToBaseRentalIncomePerUnitAfterRenovations
		{
			get;
			set;
		}

		public DateTime? BalloonDateForPayoffOfNote
		{
			get;
			set;
		}

		public DateTime? BalloonDateOfNote
		{
			get;
			set;
		}

		public BathroomCount BathCount
		{
			get;
			set;
		}

		public BedroomCount BedCount
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

		public string BuyerAddress
		{
			get;
			set;
		}

		public string BuyerName
		{
			get;
			set;
		}

		public double? CalculatedPPSqFt
		{
			get;
			set;
		}

		public double? CalculatedPPU
		{
			get;
			set;
		}

		public DateTime? CallforOffersDate
		{
			get;
			set;
		}

		public float CashInvestmentApy
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public DateTime? ClosingDate
		{
			get;
			set;
		}

		public double? ClosingPrice
		{
			get;
			set;
		}

		[MaxLength]
		public string Comments
		{
			get;
			set;
		}

		public double CommissionShareToEPI
		{
			get;
			set;
		}

		public bool? CompleteRentRoll
		{
			get;
			set;
		}

		public string ContactPhoneNumber
		{
			get;
			set;
		}

		public string CorporateOwnershipAddress
		{
			get;
			set;
		}

		public string CorporateOwnershipAddress2
		{
			get;
			set;
		}

		public string CorporateOwnershipCity
		{
			get;
			set;
		}

		public string CorporateOwnershipOfficer
		{
			get;
			set;
		}

		public string CorporateOwnershipState
		{
			get;
			set;
		}

		public string CorporateOwnershipZip
		{
			get;
			set;
		}

		public string County
		{
			get;
			set;
		}

		public int CoveredParkingSpaces
		{
			get;
			set;
		}

		public DateTime? CreationDate
		{
			get;
			set;
		}

		public double CurrentBpo
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

		public bool CurrentPropertyTaxes
		{
			get;
			set;
		}

		public double CurrentVacancyFac
		{
			get;
			set;
		}

		public DateTime? DateCommissionToEpiReceived
		{
			get;
			set;
		}

		public DateTime? DateOfCsaConfirm
		{
			get;
			set;
		}

		public DateTime? DateOfOrderSubmit
		{
			get;
			set;
		}

		public DateTime? DateOfSale
		{
			get;
			set;
		}

		public virtual List<AssetDocument> Documents
		{
			get;
			set;
		}

		public string EnvironmentalIssues
		{
			get;
			set;
		}

		public string EscrowCompany
		{
			get;
			set;
		}

		public string EscrowCompanyAddress
		{
			get;
			set;
		}

		public string EscrowCompanyPhoneNumber
		{
			get;
			set;
		}

		public int? EstDeferredMaintenance
		{
			get;
			set;
		}

		public string EstDefMaintenanceDetailsString
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

		public bool? FloodPlainLocated
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

        public string FrequencyOther
        {
            get;
            set;
        }

        [MaxLength]
		public string GeneralComments
		{
			get;
			set;
		}

		[MaxLength]
		public string GeneralCommentsDtlInfo
		{
			get;
			set;
		}

		public string GradeClassification
		{
			get;
			set;
		}

		public string HasCopyOfAppraisal
		{
			get;
			set;
		}

		public bool? HasDeferredMaintenance
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

		public string HasIncomeReason
		{
			get;
			set;
		}

		public PositionMortgageType HasPositionMortgage
		{
			get;
			set;
		}

		public DateTime? HoldEndDate
		{
			get;
			set;
		}

		public int? HoldForUserId
		{
			get;
			set;
		}

		public DateTime? HoldStartDate
		{
			get;
			set;
		}

		public virtual List<AssetImage> Images
		{
			get;
			set;
		}

		public DateTime? IndicativeBidsDueDate
		{
			get;
			set;
		}

        public string InstrumentOther
        {
            get;
            set;
        }

        public double? InterestRate
		{
			get;
			set;
		}

		public InteriorRoadType? InteriorRoadTypeId
		{
			get;
			set;
		}

		public bool IsActive
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

		public bool? isOfferingMemorandum
		{
			get;
			set;
		}

		public bool IsPaper
		{
			get;
			set;
		}

		public bool? isParticipateTaxExchange
		{
			get;
			set;
		}

		public bool IsSampleAsset
		{
			get;
			set;
		}

		public bool IsSubmitted
		{
			get;
			set;
		}

		public bool IsTBDMarket
		{
			get;
			set;
		}

		public DateTime? LastPaymentRecievedOnNote
		{
			get;
			set;
		}

		public float LastReportedOccPercent
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

		public DateTime? LeaseholdMaturityDate
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

		public bool ListedByRealtor
		{
			get;
			set;
		}

		public int? ListedByUserId
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.ListingStatus ListingStatus
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

		public MHPadType? MHPadTypeId
		{
			get;
			set;
		}

		public double MonthlyGrossIncome
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

		public int? NumberNonRentableSpace
		{
			get;
			set;
		}

		public int? NumberOfMissedPayments
		{
			get;
			set;
		}

		public int? NumberParkOwnedMH
		{
			get;
			set;
		}

		public int? NumberRentableSpace
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.OccupancyType OccupancyType
		{
			get;
			set;
		}

		public string OfficerOfSeller
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.OperatingStatus OperatingStatus
		{
			get;
			set;
		}

		public DateTime? OrderDate
		{
			get;
			set;
		}

		public int? OrderedByUserId
		{
			get;
			set;
		}

		public int? OrderId
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.OrderStatus OrderStatus
		{
			get;
			set;
		}

		public double? OriginalPrincipalBalanceWRAP
		{
			get;
			set;
		}

		public string Owner
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

        public string PayHistoryOther
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

		public virtual int? PCInsuranceCompanyId
		{
			get;
			set;
		}

		public int? PCInsuranceCompanyUserId
		{
			get;
			set;
		}

		public DateTime? PCInsuranceDateOfOrderSubmit
		{
			get;
			set;
		}

		public DateTime? PCInsuranceOrderDate
		{
			get;
			set;
		}

		public virtual int? PCInsuranceOrderedByUserId
		{
			get;
			set;
		}

		public virtual Inview.Epi.EpiFund.Domain.Enum.OrderStatus PCInsuranceOrderStatus
		{
			get;
			set;
		}

		public double ProformaAnnualIncome
		{
			get;
			set;
		}

		public int ProformaAnnualOperExpenses
		{
			get;
			set;
		}

		public float ProformaAoeFactorAsPerOfSGI
		{
			get;
			set;
		}

		public double ProformaMiscIncome
		{
			get;
			set;
		}

		public double ProformaMonthlyIncome
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

		public string PropertyAddress2
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.PropertyCondition PropertyCondition
		{
			get;
			set;
		}

		public int PropertyTaxYear
		{
			get;
			set;
		}

		public PropHoldType PropHoldTypeId
		{
			get;
			set;
		}

		public DateTime? PropLastUpdated
		{
			get;
			set;
		}

        public string ProposedBuyer
		{
			get;
			set;
		}

        public string ProposedBuyerAddress
        {
            get;
            set;
        }

        public Guid? OwnerHoldingCompanyId
        {
            get;
            set;
        }

        public Guid? OwnerOperatingCompanyId
        {
            get;
            set;
        }

        public string ProposedBuyerContact
		{
			get;
			set;
		}

        public DateTime? ProposedCOEDate
		{
			get;
			set;
		}

        public string RecentUpgradesRenovations
		{
			get;
			set;
		}

		public bool? RenovatedByOwner
		{
			get;
			set;
		}

		public double? RenovationBudget
		{
			get;
			set;
		}

		public int RenovationYear
		{
			get;
			set;
		}

		[MaxLength]
		public string RetailComments
		{
			get;
			set;
		}

		public double? SalesPrice
		{
			get;
			set;
		}

        public bool SalesPriceNotProvided
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

		public double? SecuringPropertyAppraisal
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

		public virtual Inview.Epi.EpiFund.Domain.Enum.SellerTerms SellerTerms
		{
			get;
			set;
		}

        public string SellerTermsOther
        {
            get;
            set;
        }

        public bool Show
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

		public string Subdivision
		{
			get;
			set;
		}

		public string TaxBookMap
		{
			get;
			set;
		}

		public string TaxParcelNumber
		{
			get;
			set;
		}

		public string TermId
		{
			get;
			set;
		}

		public string Terms
		{
			get;
			set;
		}

        public string TermsOther
        {
            get;
            set;
        }

        public int? TitleCompanyId
		{
			get;
			set;
		}

		public int? TitleCompanyUserId
		{
			get;
			set;
		}

		public double? TotalMonthlyPaymentWRAP
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

		public virtual List<AssetVideo> Videos
		{
			get;
			set;
		}

		public bool? WasPropertyDistressed
		{
			get;
			set;
		}

		public string WasteWaterServProvider
		{
			get;
			set;
		}

		public WasteWaterType? WasteWaterTypeId
		{
			get;
			set;
		}

		public string WaterServProvider
		{
			get;
			set;
		}

		public WaterServType? WaterServTypeId
		{
			get;
			set;
		}

		public string WebsiteEmail
		{
			get;
			set;
		}

		public int YearBuilt
		{
			get;
			set;
		}

		public int YearsInArrearTaxes
		{
			get;
			set;
		}

		public string Zip
		{
			get;
			set;
		}

		public bool IsPublished
		{
			get;
			set;
		}

		public float? Longitude
		{
			get;
			set;
		}
		public float? Latitude
		{
			get;
			set;
		}

		public Asset()
		{
			this.ListingStatus = Inview.Epi.EpiFund.Domain.Enum.ListingStatus.Pending;
			this.IsSubmitted = false;
		}

	}
}