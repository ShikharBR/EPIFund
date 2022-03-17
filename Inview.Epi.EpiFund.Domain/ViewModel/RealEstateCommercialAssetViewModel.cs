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
	public class RealEstateCommercialAssetViewModel
	{
		public IEnumerable<SelectListItem> PagePropertyTypes = new List<SelectListItem>();

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

		[Display(Name="Acronym for Corporate Entity")]
		[Required(ErrorMessage="Acronym for Corporate Entity Required")]
		public string AcronymForCorporateEntity
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

		public IEnumerable<SelectListItem> AmortizationScheduleList
		{
			get;
			set;
		}

		[Display(Name="Current Scheduled Annual Property Taxes")]
		[Required(ErrorMessage="Annual Property Taxes Required")]
		public double AnnualPropertyTaxes
		{
			get;
			set;
		}

		[Display(Name="If yes, is there an “Independent Phase I/II Environmental Report” provided in the Documents Section?")]
		public bool? AreEnviDocsProvided
		{
			get;
			set;
		}

		[Display(Name="Asking Sales Price")]
		public double? AskingSalePrice
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

		public string AssetUrl
		{
			get;
			set;
		}

		public bool availablearialMap
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

		public bool availableListingAgentMarketingBrochure
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

		public bool availablepriorFiscalYearOperReport
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

		[DataType(DataType.DateTime)]
		[Display(Name="Balloon Date of Note (if applicable)")]
		public DateTime? BalloonDateOfNote
		{
			get;
			set;
		}

		[Display(Name="Major Tenant's Base Rent per Square Feet")]
		public double BaseRentPerSqFtMajorTenant
		{
			get;
			set;
		}

		[Display(Name="Number of Buildings")]
		public int BuildingsCount
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

		[Display(Name="Cell Phone Number")]
		[Required(ErrorMessage="Cell Phone Number Required")]
		public string CellPhone
		{
			get;
			set;
		}

		[Display(Name="City")]
		[Required(ErrorMessage="City Required")]
		public string City
		{
			get;
			set;
		}

		[Display(Name="Corporate Entity Address Line 1")]
		[Required(ErrorMessage="Corporate Entity Address Required")]
		public string CorporateAddress1
		{
			get;
			set;
		}

		[Display(Name="Corporate Entity Address Line 2")]
		public string CorporateAddress2
		{
			get;
			set;
		}

		public List<SelectListItem> CorporateEntityTypes
		{
			get;
			set;
		}

		[Display(Name="Corporate Name that Property is Vested in")]
		[Required(ErrorMessage="Corporate Name Required")]
		public string CorporateName
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

		[Display(Name="Corporate Title")]
		[Required(ErrorMessage="Corporate Title Required")]
		public string CorporateTitle
		{
			get;
			set;
		}

		public List<SelectListItem> CorporateTitles
		{
			get;
			set;
		}

		[Display(Name="Covered Parking Spaces")]
		[Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		public int? CoveredParkingSpaces
		{
			get;
			set;
		}

		[Display(Name="Current Scheduled Gross Operating Income")]
		public double? CurrentAnnualIncome
		{
			get;
			set;
		}

		[Display(Name="Current Annual Operating Expenses")]
		public double? CurrentAnnualOperatingExepenses
		{
			get;
			set;
		}

		[Display(Name="Current Market Value/CMV")]
		public double CurrentBpo
		{
			get;
			set;
		}

		[Display(Name="Current Scheduled Annual Pre Tax Net Operating Income")]
		public double? CurrentCalendarYearToDateCashFlow
		{
			get;
			set;
		}

		[Display(Name="Current Market Rents per Square Feet")]
		public double CurrentMarkerRentPerSqFt
		{
			get;
			set;
		}

		[Display(Name="Current Monthly Income")]
		public double CurrentMI
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

		[Display(Name="Current Annualized Vacancy Factor as a Percentage of Rentable Sq.Ft. or Number of Units")]
		public double? CurrentVacancyFactor
		{
			get;
			set;
		}

		public string DateForTempImages
		{
			get;
			set;
		}

		public List<AssetDeferredItemViewModel> DeferredMaintenanceItems
		{
			get;
			set;
		}

		public List<AssetDeferredItemViewModel> DeferredMaintenanceItemsMF
		{
			get;
			set;
		}

		public List<TempAssetDocument> Documents
		{
			get;
			set;
		}

		[Display(Name="Electric Meter Method")]
		[Required(ErrorMessage="Electric Meter Method is required")]
		public MeteringMethod ElectricMeterMethod
		{
			get;
			set;
		}

		[Display(Name="Email")]
		[Required(ErrorMessage="Email Address Required")]
		public string EmailAddress
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

		[Display(Name="Estimated Deferred Maintenance")]
		public int EstDeferredMaintenance
		{
			get;
			set;
		}

		public string ExistingPFName
		{
			get;
			set;
		}

		[Display(Name="Fax Number")]
		public string Fax
		{
			get;
			set;
		}

		[Display(Name="Name of 1st Mortgage Lender")]
		public string FirstMortgageCompany
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

		[DataType(DataType.DateTime)]
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

		[DataType(DataType.DateTime)]
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

		[DataType(DataType.DateTime)]
		[Display(Name="FC Sale Date")]
		public DateTime? ForeclosureSaleDate
		{
			get;
			set;
		}

		[Display(Name="Gas Meter Method")]
		[Required(ErrorMessage="Gas Meter Method is required")]
		public MeteringMethod GasMeterMethod
		{
			get;
			set;
		}

		[Display(Name="Grade Classification")]
		[Required(ErrorMessage="Grade Classification is required")]
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

		public Guid GuidId
		{
			get;
			set;
		}

		[Display(Name="Does Property Have a Major Tenant?")]
		public bool HasAAARatedMajorTenant
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

		[Display(Name="Is there an existing 1st Position Mortgage Lien Secured by the Property")]
		[Required(ErrorMessage="Is there an existing 1st Position Mortgage Lien Secured by the Property Required")]
		public PositionMortgageType HasPositionMortgage
		{
			get;
			set;
		}

		public List<AssetImageModel> Images
		{
			get;
			set;
		}

		[DataType(DataType.DateTime)]
		[Display(Name="Date when indicative Bids are due from qualified PI’s:")]
		public DateTime? IndicativeBidsDueDate
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

		[Display(Name="Is \"Certificate of Good Standing\" available for your Corporate Entity from its residing Secretary of State?")]
		[Required(ErrorMessage="\"Certificate of Good Standing\" Required")]
		public bool IsCertificateOfGoodStandingAvailable
		{
			get;
			set;
		}

		[Display(Name="Is the Major Tenant a Publically Traded Company")]
		public bool IsMajorTenantAAARated
		{
			get;
			set;
		}

		[Display(Name="is Mortgage an ARM")]
		public bool? IsMortgageAnARM
		{
			get;
			set;
		}

		public bool? isNewPF
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

		[Display(Name="Is this CRE Asset part of a Portfolio of CRE Asset (vested/controlled by the same individual/entity)?")]
		public bool? isPartofPort
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

		public bool isProjNameUnknown
		{
			get;
			set;
		}

		public bool isPropUpdateUnknown
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
		[Display(Name="Last Reported Occupancy Date")]
		public DateTime? LastReportedDateCommercial
		{
			get;
			set;
		}

		[DataType(DataType.DateTime)]
		[Display(Name="Last Reported Occupancy Date")]
		public DateTime? LastReportedDateMF
		{
			get;
			set;
		}

		[Display(Name="Leased Square Footage of property by Major Tenant")]
		public int LeasedSquareFootageByMajorTenant
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

		[Display(Name="Lot Number")]
		public string LotNumber
		{
			get;
			set;
		}

		[Display(Name="Lot Size")]
		[Required(ErrorMessage="Lot Size Required")]
		public string LotSize
		{
			get;
			set;
		}

		[Display(Name="If Property is: Retail, Office, Industrial, Medical, Service/Fuel, or Mixed Use, provide name of its Major Tenant")]
		public string MajorTenant
		{
			get;
			set;
		}

		[Display(Name="Asset Details")]
		public List<MultiFamilyPropertyDetails> MFDetails
		{
			get
			{
				List<MultiFamilyPropertyDetails> list;
				if (!string.IsNullOrWhiteSpace(this.MFDetailsString))
				{
					string mFDetailsString = this.MFDetailsString;
					char[] chrArray = new char[] { ';' };
					list = (
						from e in mFDetailsString.Split(chrArray)
						select (MultiFamilyPropertyDetails)System.Enum.Parse(typeof(MultiFamilyPropertyDetails), e)).ToList<MultiFamilyPropertyDetails>();
				}
				else
				{
					list = new List<MultiFamilyPropertyDetails>();
				}
				return list;
			}
			set
			{
				this.MFDetailsString = string.Join<MultiFamilyPropertyDetails>(";", value.ToArray());
			}
		}

		public string MFDetailsString
		{
			get;
			set;
		}

		public string MHPadType
		{
			get;
			set;
		}

		[Display(Name="Mobile Home Pads ")]
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

		[Display(Name="Asset Details")]
		public List<MobileHomePropertyDetails> MHPDetails
		{
			get
			{
				MobileHomePropertyDetails mobileHomePropertyDetail;
				List<MobileHomePropertyDetails> mobileHomePropertyDetails;
				if (!string.IsNullOrWhiteSpace(this.MFDetailsString))
				{
					string[] strArrays = this.MFDetailsString.Split(new char[] { ';' });
					List<MobileHomePropertyDetails> mobileHomePropertyDetails1 = new List<MobileHomePropertyDetails>();
					string[] strArrays1 = strArrays;
					for (int i = 0; i < (int)strArrays1.Length; i++)
					{
						if (System.Enum.TryParse<MobileHomePropertyDetails>(strArrays1[i], out mobileHomePropertyDetail))
						{
							mobileHomePropertyDetails1.Add(mobileHomePropertyDetail);
						}
					}
					mobileHomePropertyDetails = mobileHomePropertyDetails1;
				}
				else
				{
					mobileHomePropertyDetails = new List<MobileHomePropertyDetails>();
				}
				return mobileHomePropertyDetails;
			}
			set
			{
				this.MFDetailsString = string.Join<MobileHomePropertyDetails>(";", value.ToArray());
			}
		}

		public string MHPDetailsString
		{
			get;
			set;
		}

		[Display(Name="Specifications of MHP Units")]
		public List<AssetMHPSpecification> MHPUnitSpecifications
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

		[Display(Name="If yes, when does mortgage adjust?")]
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

		[Display(Name="If Yes, is the Mortgage Lien Assumable")]
		public Inview.Epi.EpiFund.Domain.Enum.MortgageLienAssumable? MortgageLienAssumable
		{
			get;
			set;
		}

		[Display(Name="If Yes, is the Mortgage Lien:")]
		public Inview.Epi.EpiFund.Domain.Enum.MortgageLienType? MortgageLienType
		{
			get;
			set;
		}

		[Display(Name="Please explain your motivation to liquidate your interest in the above defined property")]
		public string MotivationToLiquidate
		{
			get;
			set;
		}

		[Display(Name="Name of Major Tenant (If Applicable)?")]
		public string NameOfAAARatedMajorTenant
		{
			get;
			set;
		}

		[Display(Name="Last Name of Principal")]
		public string NameOfCoPrincipal
		{
			get;
			set;
		}

		[Display(Name="First Name of Principal")]
		[Required(ErrorMessage="Name of Principal Required")]
		public string NameOfPrincipal
		{
			get;
			set;
		}

		[Display(Name="Define any deferred maintenance and/or updating that the property may need")]
		public string NeededMaintenance
		{
			get;
			set;
		}

		public string NewPfName
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

		[Display(Name="Current Number of Vacant Suites for Property")]
		public int NumberOfRentableSuites
		{
			get;
			set;
		}

		[Display(Name="Current Number of Retail/Office/Commercial Rentable Suites?")]
		public int NumberofSuites
		{
			get;
			set;
		}

		[Display(Name="Number of Tenants")]
		public int NumberOfTenants
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

		[Display(Name="Last Reported Occupancy Date for Property?")]
		public DateTime? OccupancyDate
		{
			get;
			set;
		}

		[Display(Name="Proforma Occupancy Percentage")]
		public float OccupancyPercentage
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

		[Display(Name="Offering Price to be determined by Market Bidding?")]
		public bool? OfferingPriceDeterminedByMarketBidding
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

		[Display(Name="Parking Spaces")]
		[Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		public int? ParkingSpaces
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

		public IEnumerable<SelectListItem> PaymentTypes
		{
			get;
			set;
		}

		public List<SelectListItem> PreferredContactTimes
		{
			get;
			set;
		}

		public List<SelectListItem> PreferredMethods
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

		[Display(Name="Proforma Annual NOI")]
		public int ProformaAnnualNoi
		{
			get;
			set;
		}

		[Display(Name="Proforma Annual Operating Expenses")]
		public int ProformaAnnualOperExpenses
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

		[DataType(DataType.DateTime)]
		[Display(Name="Proforma SGI")]
		[Required(ErrorMessage="Proforma SGI is required")]
		public int ProformaSgi
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
		[Required(ErrorMessage="Name of Project is required")]
		public string ProjectName
		{
			get;
			set;
		}

		[Display(Name="Property Address 1")]
		[Required(ErrorMessage="Property Address 1 Required")]
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

		[Display(Name="Property City")]
		[Required(ErrorMessage="Property City Required")]
		public string PropertyCity
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

		[Display(Name="County where Property is Located")]
		[Required(ErrorMessage="County where Property is Located Required")]
		public string PropertyCounty
		{
			get;
			set;
		}

		[Display(Name="Property Details")]
		public List<CommercialPropertyDetails> PropertyDetails
		{
			get
			{
				List<CommercialPropertyDetails> list;
				if (!string.IsNullOrWhiteSpace(this.PropertyDetailsString))
				{
					string propertyDetailsString = this.PropertyDetailsString;
					char[] chrArray = new char[] { ';' };
					list = (
						from e in propertyDetailsString.Split(chrArray)
						select (CommercialPropertyDetails)System.Enum.Parse(typeof(CommercialPropertyDetails), e)).ToList<CommercialPropertyDetails>();
				}
				else
				{
					list = new List<CommercialPropertyDetails>();
				}
				return list;
			}
			set
			{
				this.PropertyDetailsString = string.Join<CommercialPropertyDetails>(";", value.ToArray());
			}
		}

		public string PropertyDetailsString
		{
			get;
			set;
		}

		[Display(Name="Property State")]
		[Required(ErrorMessage="Property State Required")]
		public string PropertyState
		{
			get;
			set;
		}

		public int PropertyTaxYear
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> PropertyTypes
		{
			get;
			set;
		}

		[Display(Name="Property Zip")]
		[Required(ErrorMessage="Property Zip Required")]
		public string PropertyZip
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

		[Display(Name="Year Property was Last Updated")]
		[Required(ErrorMessage="Property Updated Year  is required")]
		public int? PropLastUpdated
		{
			get;
			set;
		}

		[Display(Name="Define recent upgrades and/or renovations")]
		public string RecentUpgradesRenovations
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

		[Display(Name="Approximate budget allocated toward updates/renovation:")]
		[Range(0, double.MaxValue, ErrorMessage="Value must be 0 or higher")]
		public double? RenovationBudget
		{
			get;
			set;
		}

		[Display(Name="Year when updates/renovation occurred:")]
		public int? RenovationYear
		{
			get;
			set;
		}

		public List<SelectListItem> RenovationYears
		{
			get;
			set;
		}

		[Display(Name="Total Rentable Square Footage of Property: Retail, Office, Industrial, Medical Office, Multi-Family, Service/Fuel, or Mixed Use")]
		public int? RentableSquareFeet
		{
			get;
			set;
		}

		[Display(Name="Amortization Schedule")]
		public string SelectedAmortSchedule
		{
			get;
			set;
		}

		[Display(Name="Type of Corporate Entity")]
		[Required(ErrorMessage="Type of Corporate Entity Required")]
		public CorporateEntityType SelectedCorporateEntityType
		{
			get;
			set;
		}

		[Display(Name="Preferred Contact Time")]
		public List<PreferredContactTime> SelectedPreferredContactTime
		{
			get;
			set;
		}

		[Display(Name="Preferred Method of Communications")]
		public List<PreferredMethod> SelectedPreferredMethods
		{
			get;
			set;
		}

		[Display(Name="State")]
		[Required(ErrorMessage="State Required")]
		public string SelectedState
		{
			get;
			set;
		}

		[Display(Name="State of Origin of Corporate Entity")]
		[Required(ErrorMessage="State of Origin of Corporate Entity Required")]
		public string StateOfOriginCorporateEntity
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> States
		{
			get;
			set;
		}

		public string Subdivision
		{
			get;
			set;
		}

		[Display(Name="Tax Assessor Number for Property")]
		[Required(ErrorMessage="Tax Assessor Number for Property Required")]
		public string TaxAssessorNumber
		{
			get;
			set;
		}

		[Display(Name="Other Tax Assessor Number of Property")]
		public string TaxAssessorNumberOther
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

		[Display(Name="Please define any terms you may accept")]
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

        [Display(Name = "Other")]
        public string TermsOther
        {
            get;
            set;
        }

        public List<SelectListItem> TermTypes
		{
			get;
			set;
		}

		public int TotalRentableFeet
		{
			get;
			set;
		}

		[Display(Name="Total Rentable Square Feet for all Apartment Units")]
		public int TotalRentableFeetAllApt
		{
			get;
			set;
		}

		[Display(Name="Apt Units or MHP Spaces")]
		public int TotalUnits
		{
			get;
			set;
		}

		[Display(Name="Type of Commercial Asset")]
		public CommercialType Type
		{
			get;
			set;
		}

		[Display(Name="Type of Property")]
		[Required(ErrorMessage="Type of Property Required")]
		public string TypeOfProperty
		{
			get;
			set;
		}

		[Display(Name="Specifications of Units")]
		public List<TempUnitSpecification> UnitSpecifications
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public string Username
		{
			get;
			set;
		}

		[Display(Name="Define Vacant Rentable Suite Build Out status at the Property?")]
		public Inview.Epi.EpiFund.Domain.Enum.VacantSuites VacantSuites
		{
			get;
			set;
		}

		public List<TempAssetVideo> Videos
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

		[Display(Name="Work Phone Number")]
		public string WorkPhone
		{
			get;
			set;
		}

		[Display(Name="Year Originally Built")]
		[Range(1700, 2147483647, ErrorMessage="Year must be between 1700 and higher")]
		[Required(ErrorMessage="Year Originally Built is required")]
		public int YearBuilt
		{
			get;
			set;
		}

		[Display(Name="Zip")]
		[Required(ErrorMessage="Zip Required")]
		public string Zip
		{
			get;
			set;
		}

		public RealEstateCommercialAssetViewModel()
		{
			this.DateForTempImages = DateTime.Now.ToString("yyyyMMdd");
			this.UnitSpecifications = new List<TempUnitSpecification>()
			{
				new TempUnitSpecification()
			};
			this.MHPUnitSpecifications = new List<AssetMHPSpecification>()
			{
				new AssetMHPSpecification()
			};
			this.Images = new List<AssetImageModel>();
			this.Images = new List<AssetImageModel>();
			this.Documents = new List<TempAssetDocument>();
			this.Videos = new List<TempAssetVideo>();
			this.RenovationYears = new List<SelectListItem>();
			for (int i = DateTime.Now.Year; i >= 1970; i--)
			{
				List<SelectListItem> renovationYears = this.RenovationYears;
				SelectListItem selectListItem = new SelectListItem()
				{
					Value = i.ToString(),
					Text = i.ToString(),
					Selected = false
				};
				renovationYears.Add(selectListItem);
			}
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			SelectListItem selectListItem1 = new SelectListItem()
			{
				Value = "",
				Text = "---",
				Selected = true
			};
			selectListItems.Add(selectListItem1);
			SelectListItem selectListItem2 = new SelectListItem()
			{
				Value = "A+",
				Text = "A+"
			};
			selectListItems.Add(selectListItem2);
			SelectListItem selectListItem3 = new SelectListItem()
			{
				Value = "A",
				Text = "A"
			};
			selectListItems.Add(selectListItem3);
			SelectListItem selectListItem4 = new SelectListItem()
			{
				Value = "A-",
				Text = "A-"
			};
			selectListItems.Add(selectListItem4);
			SelectListItem selectListItem5 = new SelectListItem()
			{
				Value = "B+",
				Text = "B+"
			};
			selectListItems.Add(selectListItem5);
			SelectListItem selectListItem6 = new SelectListItem()
			{
				Value = "B",
				Text = "B"
			};
			selectListItems.Add(selectListItem6);
			SelectListItem selectListItem7 = new SelectListItem()
			{
				Value = "B-",
				Text = "B-"
			};
			selectListItems.Add(selectListItem7);
			SelectListItem selectListItem8 = new SelectListItem()
			{
				Value = "C+",
				Text = "C+"
			};
			selectListItems.Add(selectListItem8);
			SelectListItem selectListItem9 = new SelectListItem()
			{
				Value = "C",
				Text = "C"
			};
			selectListItems.Add(selectListItem9);
			SelectListItem selectListItem10 = new SelectListItem()
			{
				Value = "C-",
				Text = "C-"
			};
			selectListItems.Add(selectListItem10);
			SelectListItem selectListItem11 = new SelectListItem()
			{
				Value = "D+",
				Text = "D+"
			};
			selectListItems.Add(selectListItem11);
			SelectListItem selectListItem12 = new SelectListItem()
			{
				Value = "D",
				Text = "D"
			};
			selectListItems.Add(selectListItem12);
			this.Grades = selectListItems;
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			SelectListItem selectListItem13 = new SelectListItem()
			{
				Text = "Cell",
				Value = PreferredMethod.CellPhone.ToString()
			};
			selectListItems1.Add(selectListItem13);
			SelectListItem selectListItem14 = new SelectListItem()
			{
				Text = "Email",
				Value = PreferredMethod.Email.ToString()
			};
			selectListItems1.Add(selectListItem14);
			SelectListItem selectListItem15 = new SelectListItem()
			{
				Text = "Fax",
				Value = PreferredMethod.Fax.ToString()
			};
			selectListItems1.Add(selectListItem15);
			SelectListItem selectListItem16 = new SelectListItem()
			{
				Text = "Text",
				Value = PreferredMethod.Text.ToString()
			};
			selectListItems1.Add(selectListItem16);
			SelectListItem selectListItem17 = new SelectListItem()
			{
				Text = "Mail",
				Value = PreferredMethod.Mail.ToString()
			};
			selectListItems1.Add(selectListItem17);
			SelectListItem selectListItem18 = new SelectListItem()
			{
				Text = "Work",
				Value = PreferredMethod.WorkPhone.ToString()
			};
			selectListItems1.Add(selectListItem18);
			this.PreferredMethods = selectListItems1;
			List<SelectListItem> selectListItems2 = new List<SelectListItem>();
			SelectListItem selectListItem19 = new SelectListItem()
			{
				Text = "Morning",
				Value = PreferredContactTime.Morning.ToString()
			};
			selectListItems2.Add(selectListItem19);
			SelectListItem selectListItem20 = new SelectListItem()
			{
				Text = "Afternoon",
				Value = PreferredContactTime.Afternoon.ToString()
			};
			selectListItems2.Add(selectListItem20);
			SelectListItem selectListItem21 = new SelectListItem()
			{
				Text = "Evening",
				Value = PreferredContactTime.Evening.ToString()
			};
			selectListItems2.Add(selectListItem21);
			this.PreferredContactTimes = selectListItems2;
			List<SelectListItem> selectListItems3 = new List<SelectListItem>();
			SelectListItem selectListItem22 = new SelectListItem()
			{
				Value = "0",
				Text = "---",
				Selected = true
			};
			selectListItems3.Add(selectListItem22);
			SelectListItem selectListItem23 = new SelectListItem()
			{
				Value = "1",
				Text = "Fee Simple Title"
			};
			selectListItems3.Add(selectListItem23);
			SelectListItem selectListItem24 = new SelectListItem()
			{
				Value = "2",
				Text = "Leasehold Title"
			};
			selectListItems3.Add(selectListItem24);
			this.PropHoldTypes = selectListItems3;
			List<SelectListItem> selectListItems4 = new List<SelectListItem>();
			SelectListItem selectListItem25 = new SelectListItem()
			{
				Value = "0",
				Text = "---",
				Selected = true
			};
			selectListItems4.Add(selectListItem25);
			SelectListItem selectListItem26 = new SelectListItem()
			{
				Value = SellerTerms.AllCashNoConsi.ToString(),
				Text = EnumHelper.GetEnumDescription(SellerTerms.AllCashNoConsi)
			};
			selectListItems4.Add(selectListItem26);
			SelectListItem selectListItem27 = new SelectListItem()
			{
				Value = SellerTerms.AllCashConsi.ToString(),
				Text = EnumHelper.GetEnumDescription(SellerTerms.AllCashConsi)
			};
			selectListItems4.Add(selectListItem27);
			SelectListItem selectListItem28 = new SelectListItem()
			{
				Value = SellerTerms.CashAssump.ToString(),
				Text = EnumHelper.GetEnumDescription(SellerTerms.CashAssump)
			};
			selectListItems4.Add(selectListItem28);
			SelectListItem selectListItem29 = new SelectListItem()
			{
				Value = SellerTerms.CashCarry.ToString(),
				Text = EnumHelper.GetEnumDescription(SellerTerms.CashCarry)
			};
			selectListItems4.Add(selectListItem29);
			SelectListItem selectListItem30 = new SelectListItem()
			{
				Value = SellerTerms.FandC.ToString(),
				Text = EnumHelper.GetEnumDescription(SellerTerms.FandC)
			};
			selectListItems4.Add(selectListItem30);
			SelectListItem selectListItem31 = new SelectListItem()
			{
				Value = SellerTerms.CashProperty.ToString(),
				Text = EnumHelper.GetEnumDescription(SellerTerms.CashProperty)
			};
			selectListItems4.Add(selectListItem31);
            SelectListItem selectListItem31_1 = new SelectListItem()
            {
                Value = SellerTerms.Property.ToString(),
                Text = EnumHelper.GetEnumDescription(SellerTerms.Property)
            };
            selectListItems4.Add(selectListItem31_1);
            SelectListItem selectListItem31_2 = new SelectListItem()
            {
                Value = SellerTerms.Other.ToString(),
                Text = EnumHelper.GetEnumDescription(SellerTerms.Other)
            };
            selectListItems4.Add(selectListItem31_2);
            this.TermTypes = selectListItems4;
			List<SelectListItem> selectListItems5 = new List<SelectListItem>();
			SelectListItem selectListItem32 = new SelectListItem()
			{
				Text = "Corporation",
				Value = CorporateEntityType.Corporation.ToString()
			};
			selectListItems5.Add(selectListItem32);
			SelectListItem selectListItem33 = new SelectListItem()
			{
				Text = "Limited Liability Company",
				Value = CorporateEntityType.LimitedLiabilityCompany.ToString()
			};
			selectListItems5.Add(selectListItem33);
			SelectListItem selectListItem34 = new SelectListItem()
			{
				Text = "Limited Liability Partnership",
				Value = CorporateEntityType.LimitedLiabilityPartnership.ToString()
			};
			selectListItems5.Add(selectListItem34);
			SelectListItem selectListItem35 = new SelectListItem()
			{
				Text = "Joint Venture",
				Value = CorporateEntityType.JointVenture.ToString()
			};
			selectListItems5.Add(selectListItem35);
			SelectListItem selectListItem36 = new SelectListItem()
			{
				Text = "Sole Proprietorship",
				Value = CorporateEntityType.SoleProprietorship.ToString()
			};
			selectListItems5.Add(selectListItem36);
			this.CorporateEntityTypes = selectListItems5;
			List<SelectListItem> selectListItems6 = new List<SelectListItem>();
			SelectListItem selectListItem37 = new SelectListItem()
			{
				Selected = false,
				Text = "CRE Mtg Broker",
				Value = UserType.CREBroker.ToString()
			};
			selectListItems6.Add(selectListItem37);
			SelectListItem selectListItem38 = new SelectListItem()
			{
				Selected = false,
				Text = "COO",
				Value = "COO"
			};
			selectListItems6.Add(selectListItem38);
			SelectListItem selectListItem39 = new SelectListItem()
			{
				Selected = false,
				Text = "CFO",
				Value = "CFO"
			};
			selectListItems6.Add(selectListItem39);
			SelectListItem selectListItem40 = new SelectListItem()
			{
				Selected = false,
				Text = "President",
				Value = "President"
			};
			selectListItems6.Add(selectListItem40);
			SelectListItem selectListItem41 = new SelectListItem()
			{
				Selected = false,
				Text = "VP",
				Value = "VP"
			};
			selectListItems6.Add(selectListItem41);
			SelectListItem selectListItem42 = new SelectListItem()
			{
				Selected = false,
				Text = "Secretary",
				Value = "Secretary"
			};
			selectListItems6.Add(selectListItem42);
			SelectListItem selectListItem43 = new SelectListItem()
			{
				Selected = false,
				Text = "Manager",
				Value = "Manager"
			};
			selectListItems6.Add(selectListItem43);
			SelectListItem selectListItem44 = new SelectListItem()
			{
				Selected = false,
				Text = "Trustee",
				Value = "Trustee"
			};
			selectListItems6.Add(selectListItem44);
			SelectListItem selectListItem45 = new SelectListItem()
			{
				Selected = false,
				Text = "Executor",
				Value = "Executor"
			};
			selectListItems6.Add(selectListItem45);
			this.CorporateTitles = selectListItems6;
			List<SelectListItem> selectListItems7 = new List<SelectListItem>();
			SelectListItem selectListItem46 = new SelectListItem()
			{
				Text = "Interest Only",
				Value = "Interest Only"
			};
			selectListItems7.Add(selectListItem46);
			SelectListItem selectListItem47 = new SelectListItem()
			{
				Text = "Principal & Interest",
				Value = "Principal & Interest"
			};
			selectListItems7.Add(selectListItem47);
			SelectListItem selectListItem48 = new SelectListItem()
			{
				Text = "Property Taxes",
				Value = "Property Taxes"
			};
			selectListItems7.Add(selectListItem48);
			SelectListItem selectListItem49 = new SelectListItem()
			{
				Text = "Property Insurance",
				Value = "Property Insurance"
			};
			selectListItems7.Add(selectListItem49);
			SelectListItem selectListItem50 = new SelectListItem()
			{
				Text = "Both Taxes & Insurance",
				Value = "Both Taxes & Insurance"
			};
			selectListItems7.Add(selectListItem50);
			this.PaymentTypes = selectListItems7;
			List<SelectListItem> selectListItems8 = new List<SelectListItem>();
			SelectListItem selectListItem51 = new SelectListItem()
			{
				Value = "0",
				Text = "---",
				Selected = true
			};
			selectListItems8.Add(selectListItem51);
			SelectListItem selectListItem52 = new SelectListItem()
			{
				Value = "1",
				Text = "Private"
			};
			selectListItems8.Add(selectListItem52);
			SelectListItem selectListItem53 = new SelectListItem()
			{
				Value = "2",
				Text = "Public"
			};
			selectListItems8.Add(selectListItem53);
			SelectListItem selectListItem54 = new SelectListItem()
			{
				Value = "2",
				Text = "On-Site"
			};
			selectListItems8.Add(selectListItem54);
			this.WaterServTypes = selectListItems8;
			List<SelectListItem> selectListItems9 = new List<SelectListItem>();
			SelectListItem selectListItem55 = new SelectListItem()
			{
				Value = "0",
				Text = "---",
				Selected = true
			};
			selectListItems9.Add(selectListItem55);
			SelectListItem selectListItem56 = new SelectListItem()
			{
				Value = "1",
				Text = "Public"
			};
			selectListItems9.Add(selectListItem56);
			SelectListItem selectListItem57 = new SelectListItem()
			{
				Value = "2",
				Text = "Septic"
			};
			selectListItems9.Add(selectListItem57);
			this.WasteWaterTypes = selectListItems9;
			List<SelectListItem> selectListItems10 = new List<SelectListItem>();
			SelectListItem selectListItem58 = new SelectListItem()
			{
				Value = "0",
				Text = "---",
				Selected = true
			};
			selectListItems10.Add(selectListItem58);
			SelectListItem selectListItem59 = new SelectListItem()
			{
				Value = "1",
				Text = "Private"
			};
			selectListItems10.Add(selectListItem59);
			SelectListItem selectListItem60 = new SelectListItem()
			{
				Value = "2",
				Text = "Public"
			};
			selectListItems10.Add(selectListItem60);
			this.AccessRoadTypes = selectListItems10;
			List<SelectListItem> selectListItems11 = new List<SelectListItem>();
			SelectListItem selectListItem61 = new SelectListItem()
			{
				Value = "0",
				Text = "---",
				Selected = true
			};
			selectListItems11.Add(selectListItem61);
			SelectListItem selectListItem62 = new SelectListItem()
			{
				Value = "1",
				Text = "Paved"
			};
			selectListItems11.Add(selectListItem62);
			SelectListItem selectListItem63 = new SelectListItem()
			{
				Value = "2",
				Text = "Gravel"
			};
			selectListItems11.Add(selectListItem63);
			this.InteriorRoadTypes = selectListItems11;
			List<SelectListItem> selectListItems12 = new List<SelectListItem>();
			SelectListItem selectListItem64 = new SelectListItem()
			{
				Value = "0",
				Text = "---",
				Selected = true
			};
			selectListItems12.Add(selectListItem64);
			SelectListItem selectListItem65 = new SelectListItem()
			{
				Value = "1",
				Text = "Above Ground Units"
			};
			selectListItems12.Add(selectListItem65);
			SelectListItem selectListItem66 = new SelectListItem()
			{
				Value = "2",
				Text = "Affixed Units"
			};
			selectListItems12.Add(selectListItem66);
			this.MHPadTypes = selectListItems12;
			List<SelectListItem> selectListItems13 = new List<SelectListItem>();
			SelectListItem selectListItem67 = new SelectListItem()
			{
				Value = "10",
				Text = "10"
			};
			selectListItems13.Add(selectListItem67);
			SelectListItem selectListItem68 = new SelectListItem()
			{
				Value = "15",
				Text = "15"
			};
			selectListItems13.Add(selectListItem68);
			SelectListItem selectListItem69 = new SelectListItem()
			{
				Value = "20",
				Text = "20"
			};
			selectListItems13.Add(selectListItem69);
			SelectListItem selectListItem70 = new SelectListItem()
			{
				Value = "25",
				Text = "25"
			};
			selectListItems13.Add(selectListItem70);
			SelectListItem selectListItem71 = new SelectListItem()
			{
				Value = "30",
				Text = "30"
			};
			selectListItems13.Add(selectListItem71);
			this.AmortizationScheduleList = selectListItems13;
			List<SelectListItem> selectListItems14 = new List<SelectListItem>();
			SelectListItem selectListItem72 = new SelectListItem()
			{
				Value = AssetType.FracturedCondominiumPortfolio.ToString(),
				Text = EnumHelper.GetEnumDescription(AssetType.FracturedCondominiumPortfolio)
			};
			selectListItems14.Add(selectListItem72);
			SelectListItem selectListItem73 = new SelectListItem()
			{
				Value = AssetType.ConvenienceStoreFuel.ToString(),
				Text = EnumHelper.GetEnumDescription(AssetType.ConvenienceStoreFuel)
			};
			selectListItems14.Add(selectListItem73);
			SelectListItem selectListItem74 = new SelectListItem()
			{
				Value = AssetType.Industrial.ToString(),
				Text = EnumHelper.GetEnumDescription(AssetType.Industrial)
			};
			selectListItems14.Add(selectListItem74);
			SelectListItem selectListItem75 = new SelectListItem()
			{
				Value = AssetType.Medical.ToString(),
				Text = EnumHelper.GetEnumDescription(AssetType.Medical)
			};
			selectListItems14.Add(selectListItem75);
			SelectListItem selectListItem76 = new SelectListItem()
			{
				Value = AssetType.MHP.ToString(),
				Text = EnumHelper.GetEnumDescription(AssetType.MHP)
			};
			selectListItems14.Add(selectListItem76);
			SelectListItem selectListItem77 = new SelectListItem()
			{
				Value = AssetType.MiniStorageProperty.ToString(),
				Text = EnumHelper.GetEnumDescription(AssetType.MiniStorageProperty)
			};
			selectListItems14.Add(selectListItem77);
			SelectListItem selectListItem78 = new SelectListItem()
			{
				Value = AssetType.MixedUse.ToString(),
				Text = EnumHelper.GetEnumDescription(AssetType.MixedUse)
			};
			selectListItems14.Add(selectListItem78);
			SelectListItem selectListItem79 = new SelectListItem()
			{
				Value = AssetType.MultiFamily.ToString(),
				Text = EnumHelper.GetEnumDescription(AssetType.MultiFamily)
			};
			selectListItems14.Add(selectListItem79);
			SelectListItem selectListItem80 = new SelectListItem()
			{
				Value = AssetType.Office.ToString(),
				Text = EnumHelper.GetEnumDescription(AssetType.Office)
			};
			selectListItems14.Add(selectListItem80);
			SelectListItem selectListItem81 = new SelectListItem()
			{
				Value = AssetType.ParkingGarageProperty.ToString(),
				Text = EnumHelper.GetEnumDescription(AssetType.ParkingGarageProperty)
			};
			selectListItems14.Add(selectListItem81);
			SelectListItem selectListItem82 = new SelectListItem()
			{
				Value = AssetType.Hotel.ToString(),
				Text = EnumHelper.GetEnumDescription(AssetType.Hotel)
			};
			selectListItems14.Add(selectListItem82);
			SelectListItem selectListItem83 = new SelectListItem()
			{
				Value = AssetType.Retail.ToString(),
				Text = EnumHelper.GetEnumDescription(AssetType.Retail)
			};
			selectListItems14.Add(selectListItem83);
			SelectListItem selectListItem84 = new SelectListItem()
			{
				Value = AssetType.SingleTenantProperty.ToString(),
				Text = EnumHelper.GetEnumDescription(AssetType.SingleTenantProperty)
			};
			selectListItems14.Add(selectListItem84);
			this.PropertyTypes = selectListItems14;
			List<SelectListItem> selectListItems15 = new List<SelectListItem>();
			SelectListItem selectListItem85 = new SelectListItem()
			{
				Text = "AL",
				Value = "AL"
			};
			selectListItems15.Add(selectListItem85);
			SelectListItem selectListItem86 = new SelectListItem()
			{
				Text = "AK",
				Value = "AK"
			};
			selectListItems15.Add(selectListItem86);
			SelectListItem selectListItem87 = new SelectListItem()
			{
				Text = "AZ",
				Value = "AZ"
			};
			selectListItems15.Add(selectListItem87);
			SelectListItem selectListItem88 = new SelectListItem()
			{
				Text = "AR",
				Value = "AR"
			};
			selectListItems15.Add(selectListItem88);
			SelectListItem selectListItem89 = new SelectListItem()
			{
				Text = "CA",
				Value = "CA"
			};
			selectListItems15.Add(selectListItem89);
			SelectListItem selectListItem90 = new SelectListItem()
			{
				Text = "CO",
				Value = "CO"
			};
			selectListItems15.Add(selectListItem90);
			SelectListItem selectListItem91 = new SelectListItem()
			{
				Text = "CT",
				Value = "CT"
			};
			selectListItems15.Add(selectListItem91);
			SelectListItem selectListItem92 = new SelectListItem()
			{
				Text = "DC",
				Value = "DC"
			};
			selectListItems15.Add(selectListItem92);
			SelectListItem selectListItem93 = new SelectListItem()
			{
				Text = "DE",
				Value = "DE"
			};
			selectListItems15.Add(selectListItem93);
			SelectListItem selectListItem94 = new SelectListItem()
			{
				Text = "FL",
				Value = "FL"
			};
			selectListItems15.Add(selectListItem94);
			SelectListItem selectListItem95 = new SelectListItem()
			{
				Text = "GA",
				Value = "GA"
			};
			selectListItems15.Add(selectListItem95);
			SelectListItem selectListItem96 = new SelectListItem()
			{
				Text = "HI",
				Value = "HI"
			};
			selectListItems15.Add(selectListItem96);
			SelectListItem selectListItem97 = new SelectListItem()
			{
				Text = "ID",
				Value = "ID"
			};
			selectListItems15.Add(selectListItem97);
			SelectListItem selectListItem98 = new SelectListItem()
			{
				Text = "IL",
				Value = "IL"
			};
			selectListItems15.Add(selectListItem98);
			SelectListItem selectListItem99 = new SelectListItem()
			{
				Text = "IN",
				Value = "IN"
			};
			selectListItems15.Add(selectListItem99);
			SelectListItem selectListItem100 = new SelectListItem()
			{
				Text = "IA",
				Value = "IA"
			};
			selectListItems15.Add(selectListItem100);
			SelectListItem selectListItem101 = new SelectListItem()
			{
				Text = "KS",
				Value = "KS"
			};
			selectListItems15.Add(selectListItem101);
			SelectListItem selectListItem102 = new SelectListItem()
			{
				Text = "KY",
				Value = "KY"
			};
			selectListItems15.Add(selectListItem102);
			SelectListItem selectListItem103 = new SelectListItem()
			{
				Text = "LA",
				Value = "LA"
			};
			selectListItems15.Add(selectListItem103);
			SelectListItem selectListItem104 = new SelectListItem()
			{
				Text = "ME",
				Value = "ME"
			};
			selectListItems15.Add(selectListItem104);
			SelectListItem selectListItem105 = new SelectListItem()
			{
				Text = "MD",
				Value = "MD"
			};
			selectListItems15.Add(selectListItem105);
			SelectListItem selectListItem106 = new SelectListItem()
			{
				Text = "MA",
				Value = "MA"
			};
			selectListItems15.Add(selectListItem106);
			SelectListItem selectListItem107 = new SelectListItem()
			{
				Text = "MI",
				Value = "MI"
			};
			selectListItems15.Add(selectListItem107);
			SelectListItem selectListItem108 = new SelectListItem()
			{
				Text = "MN",
				Value = "MN"
			};
			selectListItems15.Add(selectListItem108);
			SelectListItem selectListItem109 = new SelectListItem()
			{
				Text = "MS",
				Value = "MS"
			};
			selectListItems15.Add(selectListItem109);
			SelectListItem selectListItem110 = new SelectListItem()
			{
				Text = "MO",
				Value = "MO"
			};
			selectListItems15.Add(selectListItem110);
			SelectListItem selectListItem111 = new SelectListItem()
			{
				Text = "MT",
				Value = "MT"
			};
			selectListItems15.Add(selectListItem111);
			SelectListItem selectListItem112 = new SelectListItem()
			{
				Text = "NE",
				Value = "NE"
			};
			selectListItems15.Add(selectListItem112);
			SelectListItem selectListItem113 = new SelectListItem()
			{
				Text = "NV",
				Value = "NV"
			};
			selectListItems15.Add(selectListItem113);
			SelectListItem selectListItem114 = new SelectListItem()
			{
				Text = "NH",
				Value = "NH"
			};
			selectListItems15.Add(selectListItem114);
			SelectListItem selectListItem115 = new SelectListItem()
			{
				Text = "NJ",
				Value = "NJ"
			};
			selectListItems15.Add(selectListItem115);
			SelectListItem selectListItem116 = new SelectListItem()
			{
				Text = "NM",
				Value = "NM"
			};
			selectListItems15.Add(selectListItem116);
			SelectListItem selectListItem117 = new SelectListItem()
			{
				Text = "NY",
				Value = "NY"
			};
			selectListItems15.Add(selectListItem117);
			SelectListItem selectListItem118 = new SelectListItem()
			{
				Text = "NC",
				Value = "NC"
			};
			selectListItems15.Add(selectListItem118);
			SelectListItem selectListItem119 = new SelectListItem()
			{
				Text = "ND",
				Value = "ND"
			};
			selectListItems15.Add(selectListItem119);
			SelectListItem selectListItem120 = new SelectListItem()
			{
				Text = "OH",
				Value = "OH"
			};
			selectListItems15.Add(selectListItem120);
			SelectListItem selectListItem121 = new SelectListItem()
			{
				Text = "OK",
				Value = "OK"
			};
			selectListItems15.Add(selectListItem121);
			SelectListItem selectListItem122 = new SelectListItem()
			{
				Text = "OR",
				Value = "OR"
			};
			selectListItems15.Add(selectListItem122);
			SelectListItem selectListItem123 = new SelectListItem()
			{
				Text = "PA",
				Value = "PA"
			};
			selectListItems15.Add(selectListItem123);
			SelectListItem selectListItem124 = new SelectListItem()
			{
				Text = "RI",
				Value = "RI"
			};
			selectListItems15.Add(selectListItem124);
			SelectListItem selectListItem125 = new SelectListItem()
			{
				Text = "SC",
				Value = "SC"
			};
			selectListItems15.Add(selectListItem125);
			SelectListItem selectListItem126 = new SelectListItem()
			{
				Text = "SD",
				Value = "SD"
			};
			selectListItems15.Add(selectListItem126);
			SelectListItem selectListItem127 = new SelectListItem()
			{
				Text = "TN",
				Value = "TN"
			};
			selectListItems15.Add(selectListItem127);
			SelectListItem selectListItem128 = new SelectListItem()
			{
				Text = "TX",
				Value = "TX"
			};
			selectListItems15.Add(selectListItem128);
			SelectListItem selectListItem129 = new SelectListItem()
			{
				Text = "UT",
				Value = "UT"
			};
			selectListItems15.Add(selectListItem129);
			SelectListItem selectListItem130 = new SelectListItem()
			{
				Text = "VT",
				Value = "VT"
			};
			selectListItems15.Add(selectListItem130);
			SelectListItem selectListItem131 = new SelectListItem()
			{
				Text = "VA",
				Value = "VA"
			};
			selectListItems15.Add(selectListItem131);
			SelectListItem selectListItem132 = new SelectListItem()
			{
				Text = "WA",
				Value = "WA"
			};
			selectListItems15.Add(selectListItem132);
			SelectListItem selectListItem133 = new SelectListItem()
			{
				Text = "WV",
				Value = "WV"
			};
			selectListItems15.Add(selectListItem133);
			SelectListItem selectListItem134 = new SelectListItem()
			{
				Text = "WI",
				Value = "WI"
			};
			selectListItems15.Add(selectListItem134);
			SelectListItem selectListItem135 = new SelectListItem()
			{
				Text = "WY",
				Value = "WY"
			};
			selectListItems15.Add(selectListItem135);
			this.States = selectListItems15;
		}
	}
}