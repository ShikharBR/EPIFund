using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AssetViewModel : BaseModel
	{
		public string AccessRoadType
		{
			get;
			set;
		}

		[Display(Name="Access Roads to Property ")]
		public int? AccessRoadTypeId
		{
			get;
			set;
		}

		public List<SelectListItem> AccessRoadTypes
		{
			get;
			set;
		}

		[Display(Name="Account Number")]
		public string AccountNumber
		{
			get;
			set;
		}

		[DataType(DataType.DateTime)]
		[Display(Name=" Actual Closing Date")]
		public DateTime? ActualClosingDate
		{
			get;
			set;
		}

		[Display(Name="Actual COE Date")]
		public DateTime? ActualCOEDate
		{
			get;
			set;
		}

		[Display(Name="In the interest of Full Disclosure, please enter any additional information on the Property:")]
		public string AdditionalInformation
		{
			get;
			set;
		}

		[Display(Name="Account Number of Servicing Agent")]
		public string AgentAccountNumber
		{
			get;
			set;
		}

		[Display(Name="Address of Servicing Agent")]
		public string AgentAddress
		{
			get;
			set;
		}

		[Display(Name="City")]
		public string AgentCity
		{
			get;
			set;
		}

		[Display(Name="Servicing Contact Person")]
		public string AgentContactPerson
		{
			get;
			set;
		}

		[Display(Name="Email Address of Servicing Agent")]
		public string AgentEmail
		{
			get;
			set;
		}

		[Display(Name="Company name of Servicing Agent")]
		public string AgentName
		{
			get;
			set;
		}

		[Display(Name="Phone Number of Servicing Agent")]
		public string AgentPhone
		{
			get;
			set;
		}

		[Display(Name="State")]
		public string AgentState
		{
			get;
			set;
		}

		[Display(Name="Zip")]
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

		public IEnumerable<SelectListItem> AmortizationScheduleList
		{
			get;
			set;
		}

		[Display(Name="Amort Type")]
		public string AmortType
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> AmortTypes
		{
			get;
			set;
		}

        [Display(Name = "Other")]
        public string AmortOther
        {
            get;
            set;
        }

        [Display(Name="Current Annual Gross Income")]
		[Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		public double AnnualGrossIncome
		{
			get;
			set;
		}

		[Display(Name="Annual Property Tx")]
		public double AnnualPropertyTax
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> AppraisalMethods
		{
			get;
			set;
		}

		public bool Approved
		{
			get;
			set;
		}

		[Display(Name="List Price")]
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

		[Display(Name="Asset Document Type")]
		public Inview.Epi.EpiFund.Domain.Enum.AssetDocumentType AssetDocumentType
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> AssetDocumentTypes
		{
			get;
			set;
		}

		public Guid AssetId
		{
			get;
			set;
		}

		public IList<AssetNARMember> AssetNARMembers
		{
			get;
			set;
		}

        [Display(Name="Asset Number")]
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

		[Display(Name="Tax Parcel Numbers")]
		public List<AssetTaxParcelNumber> AssetTaxParcelNumbers
		{
			get;
			set;
		}

		[Display(Name="Asset Type")]
		[Required(ErrorMessage="Asset Type is required")]
		public Inview.Epi.EpiFund.Domain.ViewModel.AssetType AssetType
		{
			get;
			set;
		}

		[Display(Name="Auction/LOI Submittal Date")]
		public DateTime? AuctionDate
		{
			get;
			set;
		}

		[Display(Name="Do you authorize servicing agent to send US CRE Online Fund a payment history?")]
		public bool? AuthorizeForwardPaymentHistory
		{
			get;
			set;
		}

		public bool availablearialMap
		{
			get;
			set;
		}

		public bool availableBKRelated
		{
			get;
			set;
		}

		public bool availablecurrentAppraisal
		{
			get;
			set;
		}

		public bool availablecurrentOperatingReport
		{
			get;
			set;
		}

		public bool availablecurrentRentRoll
		{
			get;
			set;
		}

		public bool availableDOTMTG
		{
			get;
			set;
		}

        public bool AvailableEnvironmentalRep
        {
            get;
            set;
        }

        public bool AvailableInsurance
		{
			get;
			set;
		}

		public bool availableInsuranceOther
		{
			get;
			set;
		}

		public bool availableListingAgentMarketingBrochure
		{
			get;
			set;
		}

		public bool availableMortgageInstrumentRecord
		{
			get;
			set;
		}

		public bool availableoriginalAppraisal
		{
			get;
			set;
		}

		public bool availableOtherDocument
		{
			get;
			set;
		}

		public bool availableOtherTitle
		{
			get;
			set;
		}

		public bool availableplatMap
		{
			get;
			set;
		}

		public bool availablepreliminaryTitleReport
		{
			get;
			set;
		}

		public bool availablePreliminaryTitleReportTitle
		{
			get;
			set;
		}

		public bool availablepriorFiscalYearOperReport
		{
			get;
			set;
		}

		public bool availableRecordedLiens
		{
			get;
			set;
		}

		public bool availableTaxLiens
		{
			get;
			set;
		}

		[Display(Name="Average Adjustment to Base Rental Income Per Unit after Renovations")]
		public double? AverageAdjustmentToBaseRentalIncomePerUnitAfterRenovations
		{
			get;
			set;
		}

        [Display(Name = "Other")]
        public string AppraisalOther
        {
            get;
            set;
        }

        [DataType(DataType.DateTime)]
		[Display(Name="Balloon Date for Payoff of Note")]
		public DateTime? BalloonDateForPayoffOfNote
		{
			get;
			set;
		}

		[Display(Name="Balloon Date of Note (if applicable)")]
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

		[Display(Name="BPO of Property at Note Origination (if available)")]
		public double? BPOOfProperty
		{
			get;
			set;
		}

		[Display(Name="Number of Buildings")]
		[Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		[Required(ErrorMessage="Number of Buildings is required")]
		public double BuildingsCount
		{
			get;
			set;
		}

		[Display(Name="Buyer Address")]
		public string BuyerAddress
		{
			get;
			set;
		}

		[Display(Name="Buyer Name")]
		public string BuyerName
		{
			get;
			set;
		}

		[Display(Name="Call for Offers Date")]
		public DateTime? CallforOffersDate
		{
			get;
			set;
		}

		[Display(Name="Proforma Cap Rate (%)")]
		[DisplayFormat(DataFormatString="{0:F2}")]
		public float CashInvestmentApy
		{
			get;
			set;
		}

		[Required(ErrorMessage="City is required")]
		public string City
		{
			get;
			set;
		}

		[DataType(DataType.DateTime)]
		[Display(Name="Contract Proposed Closing Date")]
		public DateTime? ClosingDate
		{
			get;
			set;
		}

		public double ClosingPrice
		{
			get;
			set;
		}

		[Display(Name="Commission Share Agr")]
		public bool CommissionShareAgr
		{
			get;
			set;
		}

		[Display(Name="Commission Share to EPI")]
		public double CommissionShareToEPI
		{
			get;
			set;
		}

		[Display(Name="Is there a Complete Rent Roll / Tenant Lease Summary for this Asset?")]
		public bool? CompleteRentRoll
		{
			get;
			set;
		}

		[Display(Name="Phone Number")]
		public string ContactPhoneNumber
		{
			get;
			set;
		}

		[Display(Name="Corporate Ownership Address")]
		public string CorporateOwnershipAddress
		{
			get;
			set;
		}

		[Display(Name="Corporate Ownership Address 2")]
		public string CorporateOwnershipAddress2
		{
			get;
			set;
		}

		[Display(Name="Corporate Ownership City")]
		public string CorporateOwnershipCity
		{
			get;
			set;
		}

		[Display(Name="Corporate Ownership’s Principal Officer")]
		public string CorporateOwnershipOfficer
		{
			get;
			set;
		}

		[Display(Name="Corporate Ownership State")]
		public string CorporateOwnershipState
		{
			get;
			set;
		}

		[Display(Name="Corporate Ownership Zip")]
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

		[Display(Name="Covered Parking Spaces")]
		[Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		public int CoveredParkingSpaces
		{
			get;
			set;
		}

		[DataType(DataType.DateTime)]
		[Display(Name="Original")]
		public DateTime? CreationDate
		{
			get;
			set;
		}

		[Display(Name="Calculated Market Value/CMV")]
		public double CurrentBpo
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.ListingStatus CurrentListingStatus
		{
			get;
			set;
		}

		[Display(Name="Current Principal Balance of Note")]
		public double? CurrentNotePrincipal
		{
			get;
			set;
		}

		[Display(Name="Current Principal Balance")]
		public double? CurrentPrincipalBalance
		{
			get;
			set;
		}

		[Display(Name="Current Property Taxes")]
		public bool CurrentPropertyTaxes
		{
			get;
			set;
		}

		[Display(Name="Current VF, Concessions, & Misc Offsets")]
		public double CurrentVacancyFac
		{
			get;
			set;
		}

		[Display(Name="Date Commission to EPI Received")]
		public DateTime? DateCommissionToEpiReceived
		{
			get;
			set;
		}

		public string DateForTempImages
		{
			get;
			set;
		}

		[Display(Name="Date Of CSA Confirm")]
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

		[Display(Name="Date of Contract")]
		public DateTime? DateOfSale
		{
			get;
			set;
		}

		[Display(Name="Days Before Listing")]
		[Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		public int DaysBeforeListing
		{
			get;
			set;
		}

		public List<AssetDeferredItemViewModel> DeferredMaintenanceItems
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public List<AssetDocument> Documents
		{
			get;
			set;
		}

		[Display(Name="If yes, please define")]
		public string EnvironmentalIssues
		{
			get;
			set;
		}

        public int DocumentNumberEnvi
        {
            get;
            set;
        }

        public int DocumentNumberOriAppr
        {
            get;
            set;
        }
        
        [Display(Name="Escrow Company")]
		public string EscrowCompany
		{
			get;
			set;
		}

		[Display(Name="Escrow Company Address")]
		public string EscrowCompanyAddress
		{
			get;
			set;
		}

		[Display(Name="Escrow Company Phone Number")]
		public string EscrowCompanyPhoneNumber
		{
			get;
			set;
		}

		[Display(Name="Estimated Deferred Maintenance")]
		[Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		public int EstDeferredMaintenance
		{
			get;
			set;
		}

		[Display(Name="Estimated Deferred Maintenance")]
		public List<MaintenanceDetails> EstDefMaintenanceDetails
		{
			get
			{
				List<MaintenanceDetails> list;
				if (!string.IsNullOrWhiteSpace(this.EstDefMaintenanceDetailsString))
				{
					string estDefMaintenanceDetailsString = this.EstDefMaintenanceDetailsString;
					char[] chrArray = new char[] { ';' };
					list = (
						from e in estDefMaintenanceDetailsString.Split(chrArray)
						select (MaintenanceDetails)System.Enum.Parse(typeof(MaintenanceDetails), e)).ToList<MaintenanceDetails>();
				}
				else
				{
					list = new List<MaintenanceDetails>();
				}
				return list;
			}
			set
			{
				this.EstDefMaintenanceDetailsString = string.Join<MaintenanceDetails>(";", value.ToArray());
			}
		}

		public string EstDefMaintenanceDetailsString
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.ListingStatus ExistingListingStatus
		{
			get;
			set;
		}

		public string ExistingPFName
		{
			get;
			set;
		}

		[Display(Name="Define the Interest Rate of the 1st Mortgage of the WRAP or AITD")]
		public double? FirstInterestRateWRAP
		{
			get;
			set;
		}

		[Display(Name="Define the 1st Mortgage Balance of the WRAP or AITD")]
		public double? FirstmortgageBalanceWRAP
		{
			get;
			set;
		}

		[Display(Name="Name of Mortgage Lender")]
		public string FirstMortgageCompany
		{
			get;
			set;
		}

		[Display(Name="Define the 1st Mortgage Payment of the WRAP or AITD")]
		public double? FirstMortgagePaymentWRAP
		{
			get;
			set;
		}

		[Display(Name="Is Property Located in a Flood Plain?")]
		public bool? FloodPlainLocated
		{
			get;
			set;
		}

		[Display(Name="FC Lender")]
		public string ForeclosureLender
		{
			get;
			set;
		}

		[Display(Name="FC Original Mortgage Date")]
		public DateTime? ForeclosureOriginalMortageDate
		{
			get;
			set;
		}

		[Display(Name="FC Original Mortgage Amount")]
		public double? ForeclosureOriginalMortgageAmount
		{
			get;
			set;
		}

		[Display(Name="Foreclosure Position")]
		public LenderPosition ForeclosurePosition
		{
			get;
			set;
		}

		[Display(Name="FC Record Date")]
		public DateTime? ForeclosureRecordDate
		{
			get;
			set;
		}

		[Display(Name="FC Record Number")]
		public string ForeclosureRecordNumber
		{
			get;
			set;
		}

		[Display(Name="FC Sale Date")]
		public DateTime? ForeclosureSaleDate
		{
			get;
			set;
		}

        [Display(Name = "Other")]
        public string FrequencyOther
        {
            get;
            set;
        }

        public bool FromCreateMethod
		{
			get;
			set;
		}

		[Display(Name="Miscellaneous Notes")]
		public string GeneralComments
		{
			get;
			set;
		}

		[Display(Name="Miscellaneous Notes")]
		public string GeneralCommentsDtlInfo
		{
			get;
			set;
		}

		[Display(Name="Grade Classification")]
		public string GradeClassification
		{
			get;
			set;
		}

		public List<SelectListItem> Grades
		{
			get;
			set;
		}

		[Display(Name= "Property Appraisal at time of note origination?")]
		public string HasCopyOfAppraisal
		{
			get;
			set;
		}

		public bool HasCSAAgreement
		{
			get;
			set;
		}

		[Display(Name="Does the Property Have Deferred Maintenance?")]
		public bool HasDeferredMaintenance
		{
			get;
			set;
		}

		[Display(Name="Are there any environmental issues with the property?")]
		public bool? HasEnvironmentalIssues
		{
			get;
			set;
		}

		[Display(Name="Does Property Produce Income?")]
		public bool HasIncome
		{
			get;
			set;
		}

		[Display(Name="If no, please explain why.")]
		public string HasIncomeReason
		{
			get;
			set;
		}

        [Display(Name = "Is there an existing 1st Position Mortgage Lien Secured by the Property")]
        //[Required(ErrorMessage = "Is there an existing 1st Position Mortgage Lien Secured by the Property Required")]
        public PositionMortgageType HasPositionMortgage
        {
            get;
            set;
        }

        [Display(Name= "Position of Additional Mortgage Lien secured by the Property")]
		public string TypeOfPositionMortgage
        {
            get;
            set;
        }

        public IEnumerable<SelectListItem>  PositionMortgage
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

		public List<AssetImage> Images
		{
			get;
			set;
		}

        [Display(Name = "Other")]
        public string InstrumentOther
        {
            get;
            set;
        }

        public List<SelectListItem> InsuranceCompanies
		{
			get;
			set;
		}

		[Display(Name="Interest Rate")]
		public double? InterestRate
		{
			get;
			set;
		}

		public string InteriorRoadType
		{
			get;
			set;
		}

		[Display(Name="Interior Road for Property")]
		public int? InteriorRoadTypeId
		{
			get;
			set;
		}

		public List<SelectListItem> InteriorRoadTypes
		{
			get;
			set;
		}

		public bool IsActive
		{
			get;
			set;
		}

		public bool isAdditionalInfo
		{
			get;
			set;
		}

		public int? isARM
		{
			get;
			set;
		}

		[Display(Name="Is Mortgage an ARM")]
		public bool? IsMortgageAnARM
		{
			get;
			set;
		}

		public bool? isNewPf
		{
			get;
			set;
		}

		[Display(Name="Is Note Current?")]
		public bool? IsNoteCurrent
		{
			get;
			set;
		}

		[Display(Name="Is your Note part of an original [WRAP AROUND] (“WRAP”) or [ALL INCLUSIVE TRUST DEED] (“AITD”)?")]
		public bool? IsNoteWRAP
		{
			get;
			set;
		}

		[Display(Name="Offering Memorandum Proforma Nets Out VF Offsets")]
		public bool isOfferingMemorandum
		{
			get;
			set;
		}

		public bool isOfferingProformaVF
		{
			get;
			set;
		}

		[Display(Name="Is This a Paper Asset?")]
		public bool IsPaper
		{
			get;
			set;
		}

		[Display(Name="Will Ownership consider participation in a 1031 Tax Deferred Exchange Transaction with the Property?")]
		public bool? isParticipateTaxExchange
		{
			get;
			set;
		}

		[Display(Name="Is Property in Pending Foreclosure?")]
		public bool isPendingForeclosure
		{
			get;
			set;
		}

		public bool? isPortfolio
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

		[Display(Name="Will Market Value be “TBD by Auction Protocol”?")]
		public bool IsTBDMarket
		{
			get;
			set;
		}

		public string JsonPictures
		{
			get;
			set;
		}

		[DataType(DataType.DateTime)]
		[Display(Name="Date of Last Payment Recieved on Note")]
		public DateTime? LastPaymentRecievedOnNote
		{
			get;
			set;
		}

		[Display(Name="Last Reported Occupancy %")]
		[DisplayFormat(DataFormatString="{0:F2}")]
		public float LastReportedOccPercent
		{
			get;
			set;
		}

		[Display(Name="Last Reported Occupancy Date")]
		public DateTime? LastReportedOccupancyDate
		{
			get;
			set;
		}

		[Display(Name="Date when Leasehold Agreement Matures:")]
		public DateTime? LeaseholdMaturityDate
		{
			get;
			set;
		}

		[Display(Name="Lender Phone Number")]
		public string LenderPhone
		{
			get;
			set;
		}

		[Display(Name="Other Lender Phone Number")]
		public string LenderPhoneOther
		{
			get;
			set;
		}

		[Display(Name="Listed By Realtor?")]
		public bool ListedByRealtor
		{
			get;
			set;
		}

		[Display(Name="Listed By User")]
		public int ListedByUserId
		{
			get;
			set;
		}

		public List<SelectListItem> ListingAgents
		{
			get;
			set;
		}

		[Display(Name="Listing Status")]
		public Inview.Epi.EpiFund.Domain.Enum.ListingStatus ListingStatus
		{
			get;
			set;
		}

		[Display(Name="Lot Number")]
		public string LotNumber
		{
			get;
			set;
		}

		[Display(Name="Lot Size")]
		public string LotSize
		{
			get;
			set;
		}

		public string MBAAgentName
		{
			get;
			set;
		}

		public string Method
		{
			get;
			set;
		}

		[Display(Name="Method of Appraisal (if available)")]
		public string MethodOfAppraisal
		{
			get;
			set;
		}

		public string MHPadType
		{
			get;
			set;
		}

		[Display(Name="Mobile Home Pads")]
		public int? MHPadTypeId
		{
			get;
			set;
		}

		public List<SelectListItem> MHPadTypes
		{
			get;
			set;
		}

		[Display(Name="Current Monthly Gross Income")]
		[Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		public int MonthlyGrossIncome
		{
			get;
			set;
		}

		[Display(Name="Monthly Payment")]
		public double? MonthlyPayment
		{
			get;
			set;
		}

		[Display(Name="If Yes, when does mortgage adjust?")]
		public string MortgageAdjustIfARM
		{
			get;
			set;
		}

		[Display(Name="Mortgage Company Address")]
		public string MortgageCompanyAddress
		{
			get;
			set;
		}

		[Display(Name="Mortgage Company City")]
		public string MortgageCompanyCity
		{
			get;
			set;
		}

		[Display(Name="Mortgage Company State")]
		public string MortgageCompanyState
		{
			get;
			set;
		}

		[Display(Name="Mortgage Company Zip")]
		public string MortgageCompanyZip
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> MortgageInstruments
		{
			get;
			set;
		}

		[Display(Name="Is the Mortgage Lien Assumable")]
		public Inview.Epi.EpiFund.Domain.Enum.MortgageLienAssumable? MortgageLienAssumable
		{
			get;
			set;
		}

		[Display(Name="Is the Mortgage Lien:")]
		public Inview.Epi.EpiFund.Domain.Enum.MortgageLienType? MortgageLienType
		{
			get;
			set;
		}

		public string NewPFName
		{
			get;
			set;
		}

		[DataType(DataType.DateTime)]
		[Display(Name="Next Payment Due Date")]
		public DateTime? NextPaymentOnNote
		{
			get;
			set;
		}

		[Display(Name="Note Interest Rate")]
		public double? NoteInterestRate
		{
			get;
			set;
		}

		[DataType(DataType.DateTime)]
		[Display(Name="Date of Note Origination")]
		public DateTime? NoteOrigination
		{
			get;
			set;
		}

		[Display(Name="Original Note Principal")]
		public double? NotePrincipal
		{
			get;
			set;
		}

		[Display(Name="Is your note professionally serviced by an independant agent?")]
		public bool? NoteServicedByAgent
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> NoteTypes
		{
			get;
			set;
		}

		[Display(Name="Number of Non-Rentable Spaces")]
		public int? NumberNonRentableSpace
		{
			get;
			set;
		}

		[Display(Name="Number of Missed Payments")]
		public int? NumberOfMissedPayments
		{
			get;
			set;
		}

		[Display(Name="Number of Park Owned Mobile Homes")]
		public int? NumberParkOwnedMH
		{
			get;
			set;
		}

		[Display(Name="Number of Rentable Spaces")]
		public int? NumberRentableSpace
		{
			get;
			set;
		}

		[Display(Name="Occupancy Type")]
		public Inview.Epi.EpiFund.Domain.Enum.OccupancyType OccupancyType
		{
			get;
			set;
		}

		[Display(Name="Officer of Seller")]
		public string OfficerOfSeller
		{
			get;
			set;
		}

		[Display(Name="Operating Status")]
		[Required(ErrorMessage="Operating Status is required")]
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

		[Display(Name="Define the original principal balance of the WRAP or AITD")]
		public double? OriginalPrincipalBalanceWRAP
		{
			get;
			set;
		}

		[StringLength(100, ErrorMessage="Owner Name length should be less than 100")]
		public string Owner
		{
			get;
			set;
		}

		public PaperAsset Paper
		{
			get;
			set;
		}

		[Display(Name="Parking Spaces")]
		[Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		public int ParkingSpaces
		{
			get;
			set;
		}

		[Display(Name="Payment Amount")]
		public double? PaymentAmount
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> PaymentFrequencies
		{
			get;
			set;
		}

		[Display(Name="Frequency of Payment")]
		public string PaymentFrequency
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> PaymentHistories
		{
			get;
			set;
		}

		[Display(Name="Define the payment history of the note maker (payor)")]
		public string PaymentHistory
		{
			get;
			set;
		}

        [Display(Name = "Other")]
        public string PayHistoryOther
        {
            get;
            set;
        }

        [Display(Name="Payment Includes")]
		public string PaymentIncludes
		{
			get;
			set;
		}

		[Display(Name="Number of Payments Made on Note Since its Origination")]
		public int? PaymentsMadeOnNote
		{
			get;
			set;
		}

		[Display(Name="Number of Payments Remaining on Note")]
		public int? PaymentsRemainingOnNote
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> PaymentTypes
		{
			get;
			set;
		}

		public int? PCInsuranceCompanyId
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

		public Inview.Epi.EpiFund.Domain.Enum.OrderStatus PCInsuranceOrderStatus
		{
			get;
			set;
		}

		public string PortfolioNames
		{
			get;
			set;
		}

		[Display(Name="Proforma Annual Gross Income")]
		[Range(0, double.MaxValue, ErrorMessage="Value must be 0 or higher")]
		public double ProformaAnnualIncome
		{
			get;
			set;
		}

		[Display(Name="Proforma Annual Operating Expenses")]
		[Range(0, double.MaxValue, ErrorMessage="Value must be 0 or higher")]
		public double ProformaAnnualOperExpenses
		{
			get;
			set;
		}

		[Display(Name="Proforma AOE Factor as % of Proforma SGI")]
		[DisplayFormat(DataFormatString="{0:F2}")]
		public float ProformaAoeFactorAsPerOfSGI
		{
			get;
			set;
		}

		[Display(Name="Proforma Annual Misc Income")]
		[Range(0, double.MaxValue, ErrorMessage="Value must be 0 or higher")]
		public double ProformaMiscIncome
		{
			get;
			set;
		}

		[Display(Name="Proforma Monthly Gross Income")]
		[Range(0, double.MaxValue, ErrorMessage="Value must be 0 or higher")]
		public double ProformaMonthlyIncome
		{
			get;
			set;
		}

		[Display(Name="Proforma VF, Concessions, & Misc Offsets")]
		public double ProformaVacancyFac
		{
			get;
			set;
		}

		[Display(Name="Name of Project")]
		public string ProjectName
		{
			get;
			set;
		}

		[Display(Name="Property Address 1")]
		[Required(ErrorMessage="Address is required")]
		public string PropertyAddress
		{
			get;
			set;
		}

		[Display(Name="Property Address 2")]
		public string PropertyAddress2
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> PropertyAppraisals
		{
			get;
			set;
		}

		[Display(Name="Property Condition")]
		[Required(ErrorMessage="Property Condition is required")]
		public Inview.Epi.EpiFund.Domain.Enum.PropertyCondition PropertyCondition
		{
			get;
			set;
		}

		[Display(Name="Property Tax Year")]
		[Range(1700, 5000, ErrorMessage="Year must be 1700 or higher")]
		public int PropertyTaxYear
		{
			get;
			set;
		}

		public string PropHoldType
		{
			get;
			set;
		}

		[Display(Name="How is Title to Property Held by Owner?")]
		public int PropHoldTypeId
		{
			get;
			set;
		}

		public List<SelectListItem> PropHoldTypes
		{
			get;
			set;
		}

		public DateTime? PropLastUpdated
		{
			get;
			set;
		}

		[Display(Name="Year Property was Last Updated")]
		public int PropLastUpdatedYear
		{
			get;
			set;
		}

        public List<SelectListItem> OperatingCompanies { get; set; }
        public List<SelectListItem> Countries { get; set; }
        #region Asset Owner Create Fields
        public bool CopyOperatingCompanyCreate { get; set; }
        public bool OwnerOperatingCompanyFromPI { get; set; }

        public bool IsOwnerOperatingCompanyDataNotAvailable { get; set; }
        public bool ChangeOwnerOperatingCompany { get; set; }
        public Guid? OwnerOperatingCompanyId { get; set; }
        public Guid? OwnerOperatingCompanyNewId { get; set; }
        public string OwnerOperatingCompanyCompanyId { get; set; }
        public bool OwnerOperatingCompanyIsActive { get; set; }
        public bool OwnerOperatingCompanyIsASeller { get; set; }
        public bool OwnerOperatingCompanyHaveSellerPrivilege { get; set; }
        [Display(Name = "Contract Owner Operating Company")]
        public string OwnerOperatingCompany { get; set; }
        [Display(Name = "First Name of Officer of the Operating Company")]
        public string OwnerOperatingCompanyFirst { get; set; }
        [Display(Name = "Last Name of Officer of the Operating Company")]
        public string OwnerOperatingCompanyLast { get; set; }
        [Display(Name = "Contract Owner Operating Company Email")]
        public string OwnerOperatingCompanyEmail { get; set; }
        [Display(Name = "Contract Owner Operating Company Address Line 1")]
        public string OwnerOperatingCompanyAddressLine1 { get; set; }
        [Display(Name = "Contract Owner Operating Company Address Line 2")]
        public string OwnerOperatingCompanyAddressLine2 { get; set; }
        [Display(Name = "Contract Owner Operating Company City")]
        public string OwnerOperatingCompanyCity { get; set; }
        [Display(Name = "State/Province/Region")]
        public string OwnerOperatingCompanyState { get; set; }
        [Display(Name = "ZIP/Postal Code")]
        public string OwnerOperatingCompanyZip { get; set; }
        [Display(Name = "Contract Owner Operating Company Country")]
        public string OwnerOperatingCompanyCountry { get; set; }
        [Display(Name = "Contract Owner Operating Company Work #")]
        public string OwnerOperatingCompanyWorkPhone { get; set; }
        [Display(Name = "Contract Owner Operating Company Cell #")]
        public string OwnerOperatingCompanyCellPhone { get; set; }
        [Display(Name = "Contract Owner Operating Company Fax #")]
        public string OwnerOperatingCompanyFax { get; set; }
        [Display(Name = "Contract Owner Operating Company User")]
        public string OwnerOperatingCompanyUser { get; set; }

        [Display(Name = "Not on PI List?")]
        public bool NotOnHoldingList { get; set; }
        [Display(Name = "Not on Company List?")]
        public bool NotOnOperatingList { get; set; }
        [Display(Name = "Contract Owner(Holding) the same as Operating Company?")]
        public bool CopyOwnerOperatingCompany{ get; set; }
        public List<SelectListItem> PrincipalInvestorOwnerUsers { get; set; }
        public List<SelectListItem> OwnerHoldingCompanies { get; set; }

        public bool IsOwnerHoldingCompanyDataNotAvailable { get; set; }
        public bool ChangeOwnerHoldingCompany { get; set; }
        public Guid? OwnerHoldingCompanyId { get; set; }
        public Guid? OwnerHoldingCompanyNewId { get; set; }
        public bool OwnerHoldingCompanyIsActive { get; set; }

        [Display(Name = "Contract Owner of Asset(Holding Company)")]
        public string OwnerHoldingCompany { get; set; }

		[Display(Name = "Is RA")]
		public bool OwnerISRA { get; set; }

		[Display(Name = "Contract Owner Holding Company Address Line 1")]
        public string OwnerHoldingCompanyAddressLine1 { get; set; }
        [Display(Name = "Contract Owner Holding Company Address Line 2")]
        public string OwnerHoldingCompanyAddressLine2 { get; set; }
        [Display(Name = "Contract Owner Holding Company City")]
        public string OwnerHoldingCompanyCity { get; set; }
        [Display(Name = "First Name of Officer of the Corporate Owner")]
        public string OwnerHoldingCompanyFirst { get; set; }
        [Display(Name = "Last Name of Officer of the Corporate Owner")]
        public string OwnerHoldingCompanyLast { get; set; }
        [Display(Name = "State/Province/Region")]
        public string OwnerHoldingCompanyState { get; set; }
        [Display(Name = "ZIP/Postal Code")]
        public string OwnerHoldingCompanyZip { get; set; }
        [Display(Name = "Contract Owner Holding Company Country")]
        public string OwnerHoldingCompanyCountry { get; set; }
        [Display(Name = "Contract Owner Holding Company Work #")]
        public string OwnerHoldingCompanyWorkPhone { get; set; }
        [Display(Name = "Contract Owner Holding Company Cell #")]
        public string OwnerHoldingCompanyCellPhone { get; set; }
        [Display(Name = "Contract Owner Holding Company Fax #")]
        public string OwnerHoldingCompanyFax { get; set; }
        [Display(Name = "Contract Owner Holding Company Email")]
        public string OwnerHoldingCompanyEmail { get; set; }
        public string OwnerHoldingCompanyAddress
        {
            get
            {
                string[] OwnerHoldingCompanyAddressLine1 = new string[] { this.OwnerHoldingCompanyAddressLine1, " ", this.OwnerHoldingCompanyAddressLine2, " ", this.OwnerHoldingCompanyCity, ", ", this.OwnerHoldingCompanyState, " ", this.OwnerHoldingCompanyZip };
                return string.Concat(OwnerHoldingCompanyAddressLine1);
            }
        }
        #endregion

        #region Asset Owner Update Fields
        public bool CopyOperatingCompanyUpdate { get; set; }
        public string OwnerHoldingCompanyUpdateId { get; set; }
        public string OwnerHoldingCompanyUpdate { get; set; }
        public string OwnerHoldingCompanyUpdateFirst { get; set; }
        public string OwnerHoldingCompanyUpdateLast { get; set; }
        public string OwnerHoldingCompanyUpdateEmail { get; set; }
        public string OwnerHoldingCompanyUpdateAddressLine1 { get; set; }
        public string OwnerHoldingCompanyUpdateAddressLine2 { get; set; }
        public string OwnerHoldingCompanyUpdateCity { get; set; }
        public string OwnerHoldingCompanyUpdateState { get; set; }
        public string OwnerHoldingCompanyUpdateZip { get; set; }
        public string OwnerHoldingCompanyUpdateCountry { get; set; }
        public string OwnerHoldingCompanyUpdateWorkPhone { get; set; }
        public string OwnerHoldingCompanyUpdateCellPhone { get; set; }
        public string OwnerHoldingCompanyUpdateFax { get; set; }
        public bool OwnerHoldingCompanyUpdateIsActive { get; set; }

        public string OwnerOperatingCompanyUpdateId { get; set; }
        public string OwnerOperatingCompanyUpdate { get; set; }
        public string OwnerOperatingCompanyUpdateFirst { get; set; }
        public string OwnerOperatingCompanyUpdateLast { get; set; }
        public string OwnerOperatingCompanyUpdateEmail { get; set; }
        public string OwnerOperatingCompanyUpdateAddressLine1 { get; set; }
        public string OwnerOperatingCompanyUpdateAddressLine2 { get; set; }
        public string OwnerOperatingCompanyUpdateCity { get; set; }
        public string OwnerOperatingCompanyUpdateState { get; set; }
        public string OwnerOperatingCompanyUpdateZip { get; set; }
        public string OwnerOperatingCompanyUpdateCountry { get; set; }
        public string OwnerOperatingCompanyUpdateWorkPhone { get; set; }
        public string OwnerOperatingCompanyUpdateCellPhone { get; set; }
        public string OwnerOperatingCompanyUpdateFax { get; set; }
        public bool OwnerOperatingCompanyUpdateIsActive { get; set; }
        #endregion

        [Display(Name="Proposed COE Date")]
		public DateTime? ProposedCOEDate
		{
			get;
			set;
		}

		[Display(Name="Has the Property been updated/renovated by Current Owner?")]
		public bool? RenovatedByOwner
		{
			get;
			set;
		}

		[Display(Name="Contract Sales Price")]
		public double? SalesPrice
		{
			get;
			set;
		}

        [Display(Name = "No SP Provided")]
        public bool SalesPriceNotProvided
        {
            get;
            set;
        }

        [Display(Name="Define the Interest Rate of the 2nd Mortgage of the WRAP or AITD (if applicable)")]
		public double? SecondInterestRateWRAP
		{
			get;
			set;
		}

		[Display(Name="Define the 2nd Mortgage Balance of the WRAP or AITD (if applicable)")]
		public double? SecondMortgageBalanceWRAP
		{
			get;
			set;
		}

		[Display(Name="Define the 2nd Mortgage Payment of the WRAP or AITD (if applicable)")]
		public double? SecondMortgagePaymentWRAP
		{
			get;
			set;
		}

		[Display(Name="Enter your Opinion of Current Property Value")]
		public double? SecuringPropertyAppraisal
		{
			get;
			set;
		}

		public int? SelAssetDoc
		{
			get;
			set;
		}

		[Display(Name="Original Amortization Schedule")]
		public string SelectedAmortSchedule
		{
			get;
			set;
		}

		[Display(Name="Asset Document Type")]
		public string SelectedDocumentType
		{
			get;
			set;
		}

		[Display(Name="Original Cash Down Paid by Payor")]
		public double? SellerCarryNoteCashDown
		{
			get;
			set;
		}

		[Display(Name="Original Purchase Price of Property by Payor")]
		public double? SellerCarryNotePrice
		{
			get;
			set;
		}

		[DataType(DataType.DateTime)]
		[Display(Name="Date Payor Originally Acquired Property")]
		public DateTime? SellerCarryNoteSalesDate
		{
			get;
			set;
		}

		[Display(Name="Please define any terms you may accept")]
		public Inview.Epi.EpiFund.Domain.Enum.SellerTerms SellerTerms
		{
			get;
			set;
		}

        [Display(Name = "Other")]
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

		[Display(Name="Total Rentable Square Feet")]
		[Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		public int SquareFeet
		{
			get;
			set;
		}

		[Required(ErrorMessage="State is required")]
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

		[Display(Name="Tax Book Map")]
		public string TaxBookMap
		{
			get;
			set;
		}

		[Display(Name="Tax Parcel Number")]
		public string TaxParcelNumber
		{
			get;
			set;
		}

		[Display(Name="Please define any terms you may accept")]
		public string TermId
		{
			get;
			set;
		}

		[Display(Name="Terms")]
		public string Terms
		{
			get;
			set;
		}

        [Display(Name = "Other")]
        public string TermsOther
        {
            get;
            set;
        }

        public List<SelectListItem> TermsOptions
		{
			get;
			set;
		}

		public List<SelectListItem> TermTypes
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> TitleAssetDocumentTypes
		{
			get;
			set;
		}

		public List<SelectListItem> TitleCompanies
		{
			get;
			set;
		}

		public string TitleCompany
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

		[Display(Name="Define the total monthly payment due on the WRAP or AITD")]
		public double? TotalMonthlyPaymentWRAP
		{
			get;
			set;
		}

		[Display(Name="Type of Mortgage Instrument")]
		public string TypeOfMTGInstrument
		{
			get;
			set;
		}

		[Display(Name="Type of Note")]
		public string TypeOfNote
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Entity.User User
		{
			get;
			set;
		}

		public string UserId
		{
			get;
			set;
		}

		public List<SelectListItem> Users
		{
			get;
			set;
		}

		public List<AssetVideo> Videos
		{
			get;
			set;
		}

		[Display(Name="Was Property Distressed at Note Origination")]
		public bool? WasPropertyDistressed
		{
			get;
			set;
		}

		[Display(Name="Waste Water Service Provider")]
		public string WasteWaterServProvider
		{
			get;
			set;
		}

		public string WasteWaterType
		{
			get;
			set;
		}

		[Display(Name="Waste Water Service for Property")]
		public int? WasteWaterTypeId
		{
			get;
			set;
		}

		public List<SelectListItem> WasteWaterTypes
		{
			get;
			set;
		}

		[Display(Name="Water Service Provider")]
		public string WaterServProvider
		{
			get;
			set;
		}

		public string WaterServType
		{
			get;
			set;
		}

		[Display(Name="Water Service to Property ")]
		public int? WaterServTypeId
		{
			get;
			set;
		}

		public List<SelectListItem> WaterServTypes
		{
			get;
			set;
		}

		[Display(Name="Website/Email")]
		public string WebsiteEmail
		{
			get;
			set;
		}

		[Display(Name="Year Originally Built")]
		[Range(1700, 5000, ErrorMessage="Year must be 1700 or higher")]
		public int YearBuilt
		{
			get;
			set;
		}

		[Display(Name="Years Taxes are in Arrear")]
		public int YearsInArrearTaxes
		{
			get;
			set;
		}

		[Display(Name="Zip Code")]
		[RegularExpression("^[0-9]{5}$", ErrorMessage="Zip Code is Invalid. Please enter 5 digits")]
		[Required(ErrorMessage="Zip Code is required")]
		public string Zip
		{
			get;
			set;
		}

		public AssetViewModel()
		{
			this.InsuranceCompanies = new List<SelectListItem>();
			this.TitleCompanies = new List<SelectListItem>();
			this.IsSubmitted = false;
			this.Videos = new List<AssetVideo>();
			this.ListingAgents = new List<SelectListItem>();
			this.AccessRoadTypeId = new int?(0);
			this.InteriorRoadTypeId = new int?(0);
			this.WaterServTypeId = new int?(0);
			this.WasteWaterTypeId = new int?(0);
			this.MHPadTypeId = new int?(0);
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			SelectListItem selectListItem = new SelectListItem()
			{
				Text = "Always by the Due Date",
				Value = "Always by the Due Date"
			};
			selectListItems.Add(selectListItem);
			SelectListItem selectListItem1 = new SelectListItem()
			{
				Text = "Always before the Grace Period Expired",
				Value = "Always before the Grace Period Expired"
			};
			selectListItems.Add(selectListItem1);
			SelectListItem selectListItem2 = new SelectListItem()
			{
				Text = "Sometimes after the Grace Period but before 30 days late",
				Value = "Sometimes after the Grace Period but before 30 days late"
			};
			selectListItems.Add(selectListItem2);
			SelectListItem selectListItem3 = new SelectListItem()
			{
				Text = "Some 30 day late payment history",
				Value = "Some 30 day late payment history"
			};
			selectListItems.Add(selectListItem3);
			SelectListItem selectListItem4 = new SelectListItem()
			{
				Text = "Some 60 day late payment history",
				Value = "Some 60 day late payment history"
			};
			selectListItems.Add(selectListItem4);
			SelectListItem selectListItem5 = new SelectListItem()
			{
				Text = "Note has been in default proceeding before",
				Value = "Note has been in default proceeding before"
			};
			selectListItems.Add(selectListItem5);
			SelectListItem selectListItem6 = new SelectListItem()
			{
				Text = "Note is presently in default and subject to foreclosure",
				Value = "Note is presently in default and subject to foreclosure"
			};
			selectListItems.Add(selectListItem6);
			SelectListItem selectListItem7 = new SelectListItem()
			{
				Text = "Other",
				Value = "Other"
			};
			selectListItems.Add(selectListItem7);
			this.PaymentHistories = selectListItems;
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			SelectListItem selectListItem8 = new SelectListItem()
			{
				Text = "Interest Only",
				Value = "Interest Only"
			};
			selectListItems1.Add(selectListItem8);
			SelectListItem selectListItem9 = new SelectListItem()
			{
				Text = "15 Year Amortization",
				Value = "15 Year Amortization"
			};
			selectListItems1.Add(selectListItem9);
			SelectListItem selectListItem10 = new SelectListItem()
			{
				Text = "20 Year Amortization",
				Value = "20 Year Amortization"
			};
			selectListItems1.Add(selectListItem10);
			SelectListItem selectListItem11 = new SelectListItem()
			{
				Text = "25 Year Amortization",
				Value = "25 Year Amortization"
			};
			selectListItems1.Add(selectListItem11);
			SelectListItem selectListItem12 = new SelectListItem()
			{
				Text = "30 Year Amortization",
				Value = "30 Year Amortization"
			};
			selectListItems1.Add(selectListItem12);
			SelectListItem selectListItem13 = new SelectListItem()
			{
				Text = "Other",
				Value = "Other"
			};
			selectListItems1.Add(selectListItem13);
			this.AmortTypes = selectListItems1;
			List<SelectListItem> selectListItems2 = new List<SelectListItem>();
			SelectListItem selectListItem14 = new SelectListItem()
			{
				Text = "Deed of Trust",
				Value = "Deed of Trust"
			};
			selectListItems2.Add(selectListItem14);
			SelectListItem selectListItem15 = new SelectListItem()
			{
				Text = "Mortgage",
				Value = "Mortgage"
			};
			selectListItems2.Add(selectListItem15);
			SelectListItem selectListItem16 = new SelectListItem()
			{
				Text = "Land Contract",
				Value = "Land Contract"
			};
			selectListItems2.Add(selectListItem16);
			SelectListItem selectListItem17 = new SelectListItem()
			{
				Text = "Contract For Deed",
				Value = "Contract For Deed"
			};
			selectListItems2.Add(selectListItem17);
			SelectListItem selectListItem18 = new SelectListItem()
			{
				Text = "Other",
				Value = "Other"
			};
			selectListItems2.Add(selectListItem18);
			this.MortgageInstruments = selectListItems2;
			List<SelectListItem> selectListItems3 = new List<SelectListItem>();
			SelectListItem selectListItem19 = new SelectListItem()
			{
				Text = "---",
				Value = "",
				Selected = true
			};
			selectListItems3.Add(selectListItem19);
			SelectListItem selectListItem20 = new SelectListItem()
			{
				Text = "All Cash",
				Value = "All Cash"
			};
			selectListItems3.Add(selectListItem20);
			SelectListItem selectListItem21 = new SelectListItem()
			{
				Text = "Debt Funding Participation",
				Value = "Debt Funding Participation"
			};
			selectListItems3.Add(selectListItem21);
			SelectListItem selectListItem22 = new SelectListItem()
			{
				Text = "Cash & Seller Carry",
				Value = "Cash & Seller Carry"
			};
			selectListItems3.Add(selectListItem22);
			SelectListItem selectListItem23 = new SelectListItem()
			{
				Text = "Other",
				Value = "Other"
			};
			selectListItems3.Add(selectListItem23);
			this.TermsOptions = selectListItems3;
			List<SelectListItem> selectListItems4 = new List<SelectListItem>();
			SelectListItem selectListItem24 = new SelectListItem()
			{
				Value = "0",
				Text = "---",
				Selected = true
			};
			selectListItems4.Add(selectListItem24);
			SelectListItem selectListItem25 = new SelectListItem()
			{
				Value = Inview.Epi.EpiFund.Domain.Enum.SellerTerms.AllCashNoConsi.ToString(),
				Text = EnumHelper.GetEnumDescription(Inview.Epi.EpiFund.Domain.Enum.SellerTerms.AllCashNoConsi)
			};
			selectListItems4.Add(selectListItem25);
			SelectListItem selectListItem26 = new SelectListItem()
			{
				Value = Inview.Epi.EpiFund.Domain.Enum.SellerTerms.AllCashConsi.ToString(),
				Text = EnumHelper.GetEnumDescription(Inview.Epi.EpiFund.Domain.Enum.SellerTerms.AllCashConsi)
			};
			selectListItems4.Add(selectListItem26);
			SelectListItem selectListItem27 = new SelectListItem()
			{
				Value = Inview.Epi.EpiFund.Domain.Enum.SellerTerms.CashAssump.ToString(),
				Text = EnumHelper.GetEnumDescription(Inview.Epi.EpiFund.Domain.Enum.SellerTerms.CashAssump)
			};
			selectListItems4.Add(selectListItem27);
			SelectListItem selectListItem28 = new SelectListItem()
			{
				Value = Inview.Epi.EpiFund.Domain.Enum.SellerTerms.CashCarry.ToString(),
				Text = EnumHelper.GetEnumDescription(Inview.Epi.EpiFund.Domain.Enum.SellerTerms.CashCarry)
			};
			selectListItems4.Add(selectListItem28);
			SelectListItem selectListItem29 = new SelectListItem()
			{
				Value = Inview.Epi.EpiFund.Domain.Enum.SellerTerms.FandC.ToString(),
				Text = EnumHelper.GetEnumDescription(Inview.Epi.EpiFund.Domain.Enum.SellerTerms.FandC)
			};
			selectListItems4.Add(selectListItem29);
			SelectListItem selectListItem30 = new SelectListItem()
			{
				Value = Inview.Epi.EpiFund.Domain.Enum.SellerTerms.CashProperty.ToString(),
				Text = EnumHelper.GetEnumDescription(Inview.Epi.EpiFund.Domain.Enum.SellerTerms.CashProperty)
			};
			selectListItems4.Add(selectListItem30);
            SelectListItem selectListItem30_1 = new SelectListItem()
            {
                Value = SellerTerms.Property.ToString(),
                Text = EnumHelper.GetEnumDescription(SellerTerms.Property)
            };
            selectListItems4.Add(selectListItem30_1);
            SelectListItem selectListItem30_2 = new SelectListItem()
            {
                Value = SellerTerms.Other.ToString(),
                Text = EnumHelper.GetEnumDescription(SellerTerms.Other)
            };
            selectListItems4.Add(selectListItem30_2);
            this.TermTypes = selectListItems4;
			List<SelectListItem> selectListItems5 = new List<SelectListItem>();
			SelectListItem selectListItem31 = new SelectListItem()
			{
				Text = "Annual",
				Value = "Annual"
			};
			selectListItems5.Add(selectListItem31);
			SelectListItem selectListItem32 = new SelectListItem()
			{
				Text = "Monthly",
				Value = "Monthly"
			};
			selectListItems5.Add(selectListItem32);
			SelectListItem selectListItem33 = new SelectListItem()
			{
				Text = "Quarterly",
				Value = "Quarterly"
			};
			selectListItems5.Add(selectListItem33);
			SelectListItem selectListItem34 = new SelectListItem()
			{
				Text = "Semi-annual",
				Value = "Semi-annual"
			};
			selectListItems5.Add(selectListItem34);
			SelectListItem selectListItem35 = new SelectListItem()
			{
				Text = "Other",
				Value = "Other"
			};
			selectListItems5.Add(selectListItem35);
			this.PaymentFrequencies = selectListItems5;
			List<SelectListItem> selectListItems6 = new List<SelectListItem>();
            SelectListItem selectListItemNONE = new SelectListItem()
            {
                Text = "None",
                Value = "None"
            };
            selectListItems6.Add(selectListItemNONE);
            SelectListItem selectListItem36 = new SelectListItem()
			{
				Text = "1st Position, Institutional",
				Value = "1st Position, Institutional"
			};
			selectListItems6.Add(selectListItem36);
            SelectListItem selectListItemPI1 = new SelectListItem()
            {
                Text = "1st Position, Principal Investor",
                Value = "1st Position, Principal Investor"
            };
            selectListItems6.Add(selectListItemPI1);
            // Not changing the value, so that existing records in DB still function
            SelectListItem selectListItem37 = new SelectListItem()
			{
				Text = "1st Position, Seller Carryback",
				Value = "1st Position, Seller Carryback"
            };
			selectListItems6.Add(selectListItem37);
			SelectListItem selectListItem38 = new SelectListItem()
			{
				Text = "2nd Position, Institutional",
				Value = "2nd Position, Institutional"
			};
			selectListItems6.Add(selectListItem38);
            SelectListItem selectListItemPI2 = new SelectListItem()
            {
                Text = "2nd Position, Principal Investor",
                Value = "2nd Position, Principal Investor"
            };
            selectListItems6.Add(selectListItemPI2);
            SelectListItem selectListItem39 = new SelectListItem()
			{
				Text = "2nd Position, Seller Carryback",
				Value = "2nd Position, Seller Carryback"
            };
			selectListItems6.Add(selectListItem39);
			SelectListItem selectListItem40 = new SelectListItem()
			{
				Text = "3rd Position, Seller Carryback",
				Value = "3rd Position, Seller Carryback"
            };
			selectListItems6.Add(selectListItem40);
			SelectListItem selectListItem41 = new SelectListItem()
			{
				Text = "Wrap Around, Encompassing 1 Mortgage",
				Value = "Wrap Around, Encompassing 1 Mortgage"
			};
			selectListItems6.Add(selectListItem41);
			SelectListItem selectListItem42 = new SelectListItem()
			{
				Text = "Wrap Around, Encompassing 2 Mortgages",
				Value = "Wrap Around, Encompassing 2 Mortgages"
			};
			selectListItems6.Add(selectListItem42);
			this.NoteTypes = selectListItems6;
            this.PositionMortgage = selectListItems6;

            List<SelectListItem> selectListItems7 = new List<SelectListItem>();
			SelectListItem selectListItem43 = new SelectListItem()
			{
				Text = "Independent Appraiser",
				Value = "Independent Appraiser"
			};
			selectListItems7.Add(selectListItem43);
			SelectListItem selectListItem44 = new SelectListItem()
			{
				Text = "Broker Price Opinion",
				Value = "Broker Price Opinion"
			};
			selectListItems7.Add(selectListItem44);
			SelectListItem selectListItem45 = new SelectListItem()
			{
				Text = "Other",
				Value = "Other"
			};
			selectListItems7.Add(selectListItem45);
			this.AppraisalMethods = selectListItems7;
			List<SelectListItem> selectListItems8 = new List<SelectListItem>();
			SelectListItem selectListItem46 = new SelectListItem()
			{
				Text = "Yes",
				Value = "Yes"
			};
			selectListItems8.Add(selectListItem46);
			SelectListItem selectListItem48 = new SelectListItem()
			{
				Text = "No",
				Value = "No"
			};
			selectListItems8.Add(selectListItem48);
			this.PropertyAppraisals = selectListItems8;
			List<SelectListItem> selectListItems9 = new List<SelectListItem>();
			SelectListItem selectListItem49 = new SelectListItem()
			{
				Value = "",
				Text = "---",
				Selected = true
			};
			selectListItems9.Add(selectListItem49);
			SelectListItem selectListItem50 = new SelectListItem()
			{
				Value = "A+",
				Text = "A+"
			};
			selectListItems9.Add(selectListItem50);
			SelectListItem selectListItem51 = new SelectListItem()
			{
				Value = "A",
				Text = "A"
			};
			selectListItems9.Add(selectListItem51);
			SelectListItem selectListItem52 = new SelectListItem()
			{
				Value = "A-",
				Text = "A-"
			};
			selectListItems9.Add(selectListItem52);
			SelectListItem selectListItem53 = new SelectListItem()
			{
				Value = "B+",
				Text = "B+"
			};
			selectListItems9.Add(selectListItem53);
			SelectListItem selectListItem54 = new SelectListItem()
			{
				Value = "B",
				Text = "B"
			};
			selectListItems9.Add(selectListItem54);
			SelectListItem selectListItem55 = new SelectListItem()
			{
				Value = "B-",
				Text = "B-"
			};
			selectListItems9.Add(selectListItem55);
			SelectListItem selectListItem56 = new SelectListItem()
			{
				Value = "C+",
				Text = "C+"
			};
			selectListItems9.Add(selectListItem56);
			SelectListItem selectListItem57 = new SelectListItem()
			{
				Value = "C",
				Text = "C"
			};
			selectListItems9.Add(selectListItem57);
			SelectListItem selectListItem58 = new SelectListItem()
			{
				Value = "C-",
				Text = "C-"
			};
			selectListItems9.Add(selectListItem58);
			SelectListItem selectListItem59 = new SelectListItem()
			{
				Value = "D+",
				Text = "D+"
			};
			selectListItems9.Add(selectListItem59);
			SelectListItem selectListItem60 = new SelectListItem()
			{
				Value = "D",
				Text = "D"
			};
			selectListItems9.Add(selectListItem60);
			this.Grades = selectListItems9;
			List<SelectListItem> selectListItems10 = new List<SelectListItem>();
			SelectListItem selectListItem61 = new SelectListItem()
			{
				Value = "BKRelated",
				Text = "BK Related Filings"
			};
			selectListItems10.Add(selectListItem61);
			SelectListItem selectListItem62 = new SelectListItem()
			{
				Value = "MortgageInstrumentOfRecord",
				Text = "Mortgage Instrument of Record"
			};
			selectListItems10.Add(selectListItem62);
			SelectListItem selectListItem63 = new SelectListItem()
			{
				Value = "OtherTitle",
				Text = "Other"
			};
			selectListItems10.Add(selectListItem63);
			SelectListItem selectListItem64 = new SelectListItem()
			{
				Value = "PreliminaryTitleReportTitle",
				Text = "Preliminary Title Report"
			};
			selectListItems10.Add(selectListItem64);
			SelectListItem selectListItem65 = new SelectListItem()
			{
				Value = "DOTMTG",
				Text = "Recorded DOTs & MTGs"
			};
			selectListItems10.Add(selectListItem65);
			SelectListItem selectListItem66 = new SelectListItem()
			{
				Value = "RecordedLiens",
				Text = "Recorded Liens"
			};
			selectListItems10.Add(selectListItem66);
			SelectListItem selectListItem67 = new SelectListItem()
			{
				Value = "TaxLiens",
				Text = "Assessor's Annual Tax Billing Statement"
            };
			selectListItems10.Add(selectListItem67);
			this.TitleAssetDocumentTypes = selectListItems10;
			List<SelectListItem> selectListItems11 = new List<SelectListItem>();
			SelectListItem selectListItem68 = new SelectListItem()
			{
				Value = "",
				Text = "---",
				Selected = true
			};
			selectListItems11.Add(selectListItem68);
			SelectListItem selectListItem69 = new SelectListItem()
			{
				Value = "ArialMap",
				Text = "Arial Map"
			};
			selectListItems11.Add(selectListItem69);
			SelectListItem selectListItem70 = new SelectListItem()
			{
				Value = "BKRelated",
				Text = "BK Related Filings"
			};
			selectListItems11.Add(selectListItem70);
			SelectListItem selectListItem71 = new SelectListItem()
			{
				Value = "CurrentAppraisal",
				Text = "Current Appraisal"
			};
			selectListItems11.Add(selectListItem71);
			SelectListItem selectListItem72 = new SelectListItem()
			{
				Value = "CurrentOperatingReport",
				Text = "Current Operating Report"
			};
			selectListItems11.Add(selectListItem72);
			SelectListItem selectListItem73 = new SelectListItem()
			{
				Value = "CurrentRentRoll",
				Text = "Current Rent Roll"
			};
			selectListItems11.Add(selectListItem73);
            SelectListItem selectListItemEnv = new SelectListItem()
            {
                Value = "EnvironmentalReport",
                Text = "Phase 1 Environmental Report"
            };
            selectListItems11.Add(selectListItemEnv);
            SelectListItem selectListItem74 = new SelectListItem()
			{
				Value = "Insurance",
				Text = "Insurance"
			};
			selectListItems11.Add(selectListItem74);
			SelectListItem selectListItem75 = new SelectListItem()
			{
				Value = "ListingAgentMarketingBrochure",
				Text = "Listing Agent Marketing Brochure"
			};
			selectListItems11.Add(selectListItem75);
			SelectListItem selectListItem76 = new SelectListItem()
			{
				Value = "MortgageInstrumentOfRecord",
				Text = "Mortgage Instrument of Record"
			};
			selectListItems11.Add(selectListItem76);
			SelectListItem selectListItem77 = new SelectListItem()
			{
				Value = "OriginalAppraisal",
				Text = "Original Appraisal"
			};
			selectListItems11.Add(selectListItem77);
			SelectListItem selectListItem78 = new SelectListItem()
			{
				Value = "Other",
				Text = "Other"
			};
			selectListItems11.Add(selectListItem78);
			SelectListItem selectListItem79 = new SelectListItem()
			{
				Value = "OtherTitle",
				Text = "Title - Other Document"
			};
			selectListItems11.Add(selectListItem79);
			SelectListItem selectListItem80 = new SelectListItem()
			{
				Text = "P&C Insurance Coverage Quote",
				Value = "Insurance"
			};
			selectListItems11.Add(selectListItem80);
			SelectListItem selectListItem81 = new SelectListItem()
			{
				Value = "PlatMap",
				Text = "Plat Map"
			};
			selectListItems11.Add(selectListItem81);
			SelectListItem selectListItem82 = new SelectListItem()
			{
				Value = "PreliminaryTitleReport",
				Text = "Preliminary Title Report"
			};
			selectListItems11.Add(selectListItem82);
			SelectListItem selectListItem83 = new SelectListItem()
			{
				Value = "PriorFiscalYearOperReport",
				Text = "Prior Fiscal Year Oper Report"
			};
			selectListItems11.Add(selectListItem83);
			SelectListItem selectListItem84 = new SelectListItem()
			{
				Value = "DOTMTG",
				Text = "Recorded DOTs & MTGs"
			};
			selectListItems11.Add(selectListItem84);
			SelectListItem selectListItem85 = new SelectListItem()
			{
				Value = "RecordedLiens",
				Text = "Recorded Liens"
			};
			selectListItems11.Add(selectListItem85);
			SelectListItem selectListItem86 = new SelectListItem()
			{
				Value = "TaxLiens",
				Text = "Assessor's Annual Tax Billing Statement"
			};
			selectListItems11.Add(selectListItem86);
			SelectListItem selectListItem87 = new SelectListItem()
			{
				Value = "PreliminaryTitleReportTitle",
				Text = "Title - Preliminary Title Report"
			};
			selectListItems11.Add(selectListItem87);
			this.AssetDocumentTypes = selectListItems11;
			List<SelectListItem> selectListItems12 = new List<SelectListItem>();
			SelectListItem selectListItem88 = new SelectListItem()
			{
				Value = "0",
				Text = "0"
			};
			selectListItems12.Add(selectListItem88);
			SelectListItem selectListItem89 = new SelectListItem()
			{
				Value = "10",
				Text = "10"
			};
			selectListItems12.Add(selectListItem89);
			SelectListItem selectListItem90 = new SelectListItem()
			{
				Value = "15",
				Text = "15"
			};
			selectListItems12.Add(selectListItem90);
			SelectListItem selectListItem91 = new SelectListItem()
			{
				Value = "20",
				Text = "20"
			};
			selectListItems12.Add(selectListItem91);
			SelectListItem selectListItem92 = new SelectListItem()
			{
				Value = "25",
				Text = "25"
			};
			selectListItems12.Add(selectListItem92);
			SelectListItem selectListItem93 = new SelectListItem()
			{
				Value = "30",
				Text = "30"
			};
			selectListItems12.Add(selectListItem93);
			this.AmortizationScheduleList = selectListItems12;
			List<SelectListItem> selectListItems13 = new List<SelectListItem>();
			SelectListItem selectListItem94 = new SelectListItem()
			{
				Value = "0",
				Text = "---",
				Selected = true
			};
			selectListItems13.Add(selectListItem94);
			SelectListItem selectListItem95 = new SelectListItem()
			{
				Value = "1",
				Text = "Fee Simple Title"
			};
			selectListItems13.Add(selectListItem95);
			SelectListItem selectListItem96 = new SelectListItem()
			{
				Value = "2",
				Text = "Leasehold Title"
			};
			selectListItems13.Add(selectListItem96);
			this.PropHoldTypes = selectListItems13;
			List<SelectListItem> selectListItems14 = new List<SelectListItem>();
            /* // Removing PropHoldTypes default option at request
             SelectListItem selectListItem96 = new SelectListItem()
			{
				Value = "0",
				Text = "---",
				Selected = true
			};
			selectListItems14.Add(selectListItem96);*/
            SelectListItem selectListItem97 = new SelectListItem()
			{
				Value = "1",
				Text = "Private"
			};
			selectListItems14.Add(selectListItem97);
			SelectListItem selectListItem98 = new SelectListItem()
			{
				Value = "2",
				Text = "Public"
			};
			selectListItems14.Add(selectListItem98);
			this.AccessRoadTypes = selectListItems14;
			List<SelectListItem> selectListItems15 = new List<SelectListItem>();
			SelectListItem selectListItem99 = new SelectListItem()
			{
				Value = "0",
				Text = "---",
				Selected = true
			};
			selectListItems15.Add(selectListItem99);
			SelectListItem selectListItem100 = new SelectListItem()
			{
				Value = "1",
				Text = "Paved"
			};
			selectListItems15.Add(selectListItem100);
			SelectListItem selectListItem101 = new SelectListItem()
			{
				Value = "2",
				Text = "Gravel"
			};
			selectListItems15.Add(selectListItem101);
			this.InteriorRoadTypes = selectListItems15;
			List<SelectListItem> selectListItems16 = new List<SelectListItem>();
			SelectListItem selectListItem102 = new SelectListItem()
			{
				Value = "0",
				Text = "---",
				Selected = true
			};
			selectListItems16.Add(selectListItem102);
			SelectListItem selectListItem103 = new SelectListItem()
			{
				Value = "1",
				Text = "Private"
			};
			selectListItems16.Add(selectListItem103);
			SelectListItem selectListItem104 = new SelectListItem()
			{
				Value = "2",
				Text = "Public"
			};
			selectListItems16.Add(selectListItem104);
			SelectListItem selectListItem105 = new SelectListItem()
			{
				Value = "3",
				Text = "On-Site"
			};
			selectListItems16.Add(selectListItem105);
			this.WaterServTypes = selectListItems16;
			List<SelectListItem> selectListItems17 = new List<SelectListItem>();
			SelectListItem selectListItem106 = new SelectListItem()
			{
				Value = "0",
				Text = "---",
				Selected = true
			};
			selectListItems17.Add(selectListItem106);
			SelectListItem selectListItem107 = new SelectListItem()
			{
				Value = "1",
				Text = "Public"
			};
			selectListItems17.Add(selectListItem107);
			SelectListItem selectListItem108 = new SelectListItem()
			{
				Value = "2",
				Text = "Septic"
			};
			selectListItems17.Add(selectListItem108);
			SelectListItem selectListItem109 = new SelectListItem()
			{
				Value = "3",
				Text = "Private"
			};
			selectListItems17.Add(selectListItem109);
			this.WasteWaterTypes = selectListItems17;
			List<SelectListItem> selectListItems18 = new List<SelectListItem>();
			SelectListItem selectListItem110 = new SelectListItem()
			{
				Value = "0",
				Text = "---",
				Selected = true
			};
			selectListItems18.Add(selectListItem110);
			SelectListItem selectListItem111 = new SelectListItem()
			{
				Value = "1",
				Text = "Above Ground Units"
			};
			selectListItems18.Add(selectListItem111);
			SelectListItem selectListItem112 = new SelectListItem()
			{
				Value = "2",
				Text = "Affixed Units"
			};
			selectListItems18.Add(selectListItem112);
			this.MHPadTypes = selectListItems18;
			this.availablecurrentRentRoll = false;
            this.AvailableEnvironmentalRep = false;
            this.availablecurrentOperatingReport = false;
			this.availablepriorFiscalYearOperReport = false;
			this.availablepreliminaryTitleReport = false;
			this.availableplatMap = false;
			this.availablearialMap = false;
			this.availableoriginalAppraisal = false;
			this.availablecurrentAppraisal = false;
			this.availableListingAgentMarketingBrochure = false;
			this.availableOtherDocument = false;
			this.availableMortgageInstrumentRecord = false;
			this.availableRecordedLiens = false;
			this.availableTaxLiens = false;
			this.availableBKRelated = false;
			this.availablePreliminaryTitleReportTitle = false;
			this.availableDOTMTG = false;
			this.availableOtherTitle = false;
			this.AvailableInsurance = false;
			this.availableInsuranceOther = false;
			this.HasIncome = true;
			this.IsActive = true;
			this.Show = false;
			this.IsTBDMarket = false;
			this.Users = new List<SelectListItem>();
			List<SelectListItem> selectListItems19 = new List<SelectListItem>();
			SelectListItem selectListItem113 = new SelectListItem()
			{
				Text = "Interest Only",
				Value = "Interest Only"
			};
			selectListItems19.Add(selectListItem113);
			SelectListItem selectListItem114 = new SelectListItem()
			{
				Text = "Principal & Interest",
				Value = "Principal & Interest"
			};
			selectListItems19.Add(selectListItem114);
			SelectListItem selectListItem115 = new SelectListItem()
			{
				Text = "Property Taxes",
				Value = "Property Taxes"
			};
			selectListItems19.Add(selectListItem115);
			SelectListItem selectListItem116 = new SelectListItem()
			{
				Text = "Property Insurance",
				Value = "Property Insurance"
			};
			selectListItems19.Add(selectListItem116);
			SelectListItem selectListItem117 = new SelectListItem()
			{
				Text = "Both Taxes & Insurance",
				Value = "Both Taxes & Insurance"
			};
			selectListItems19.Add(selectListItem117);
			this.PaymentTypes = selectListItems19;
			this.DeferredMaintenanceItems = new List<AssetDeferredItemViewModel>();
			this.AssetNARMembers = new List<AssetNARMember>();
            this.OperatingCompanies = new List<SelectListItem>();
            this.PrincipalInvestorOwnerUsers = new List<SelectListItem>();
            this.OwnerHoldingCompanies = new List<SelectListItem>();
            Countries = new List<SelectListItem>();
            this.DocumentNumberEnvi = -1;
            this.DocumentNumberOriAppr = -1;
        }


		//added new logic of Chain of title START

		public List<AssetHCOwnershipModel> AssetHCOwnershipLst
		{
			get;
			set;
		}

		public List<AssetOCModel> AssetOCLst
		{
			get;
			set;
		}

		//added new logic of Chain of title END
	}
}