using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class RealEstateResidentialAssetViewModel
	{
		public IEnumerable<SelectListItem> PagePropertyTypes = new List<SelectListItem>();

		[Display(Name="Acronym for Corporate Entity")]
		[Required(ErrorMessage="Acronym for Corporate Entity Required")]
		public string AcroynmForCorporateEntity
		{
			get;
			set;
		}

		[Display(Name="Agent's Name")]
		public string AgentName
		{
			get;
			set;
		}

		[Display(Name="Agent's Phone Number")]
		public string AgentNumber
		{
			get;
			set;
		}

		public double AnnualPropertyTaxes
		{
			get;
			set;
		}

		[Display(Name="Architecture")]
		public string Architecture
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> ArchitectureTypes
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

		public IEnumerable<SelectListItem> BathroomCount
		{
			get;
			set;
		}

		[Display(Name="Number of Bathrooms")]
		public string Bathrooms
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> BedroomCount
		{
			get;
			set;
		}

		[Display(Name="Number of Bedrooms")]
		public string Bedrooms
		{
			get;
			set;
		}

		[Display(Name="Building Style")]
		public string BuildingStyle
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> BuildingStyles
		{
			get;
			set;
		}

		[Display(Name="Carport")]
		public string Carport
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> CarportTypes
		{
			get;
			set;
		}

		[Display(Name="Cell Phone Number")]
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

		[Display(Name="Corporate Ownership’s Principal Officer")]
		public string CorporateOwnershipOfficer
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

		[Display(Name="Date of Birth")]
		public DateTime? DateOfBirth
		{
			get;
			set;
		}

		public List<AssetDeferredItemViewModel> DeferredMaintenanceItems
		{
			get;
			set;
		}

		[Display(Name="Define any deferred maintenance and/or updating your property may need")]
		public string DefferedMaintenance
		{
			get;
			set;
		}

		[Display(Name="Number of Dependants")]
		public int? Dependants
		{
			get;
			set;
		}

		public List<TempAssetDocument> Documents
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

		[Display(Name="Estimated Deferred Maintenance")]
		public int EstDeferredMaintenance
		{
			get;
			set;
		}

		[Display(Name="Fax")]
		public string Fax
		{
			get;
			set;
		}

		[Display(Name="Account Number")]
		public string FirstMCAccountNumber
		{
			get;
			set;
		}

		[Display(Name="Mortgage Company Address")]
		[Required(ErrorMessage="Mortgage Company Address Required")]
		public string FirstMCAddress
		{
			get;
			set;
		}

		[Display(Name="Mortgage Company City")]
		[Required(ErrorMessage="Mortgage Company City Required")]
		public string FirstMCCity
		{
			get;
			set;
		}

		[Display(Name="Current Mortgage Balance")]
		public double? FirstMCCurrentMortgageBalance
		{
			get;
			set;
		}

		[Display(Name="If yes, what is the sale date?")]
		public DateTime? FirstMCForclosureSaleDate
		{
			get;
			set;
		}

		[Display(Name="Has first mortgage company started a forclosure action?")]
		public bool? FirstMCHasForclosureStarted
		{
			get;
			set;
		}

		[Display(Name="Is mortgage an ARM?")]
		public bool? FirstMCIsMortgageAnARM
		{
			get;
			set;
		}

		[Display(Name="Date Last Payment was Made")]
		public DateTime? FirstMCLastPaymentDate
		{
			get;
			set;
		}

		[Display(Name="Lender Phone Number")]
		[Required(ErrorMessage="Lender Phone Number Required")]
		public string FirstMCLenderPhone
		{
			get;
			set;
		}

		[Display(Name="Other Lender Phone Number")]
		public string FirstMCLenderPhoneOther
		{
			get;
			set;
		}

		[Display(Name="What is your monthly payment?")]
		public double? FirstMCMonthlyPayment
		{
			get;
			set;
		}

		[Display(Name="Does mortgage have a PPP?")]
		public bool? FirstMCMortgageHasPPP
		{
			get;
			set;
		}

		[Display(Name="Number of Missed Payments")]
		public int? FirstMCNumberOfMissedPayments
		{
			get;
			set;
		}

		[Display(Name="Mortgage Payment Includes")]
		public string FirstMCPaymentIncludes
		{
			get;
			set;
		}

		[Display(Name="If yes, when does it expire?")]
		public DateTime? FirstMCPPPExpireDate
		{
			get;
			set;
		}

		[Display(Name="Mortgage Company State")]
		[Required(ErrorMessage="Mortgage Company State Required")]
		public string FirstMCState
		{
			get;
			set;
		}

		[Display(Name="Was last payment for that month?")]
		public bool? FirstMCWasLastPaymentForThatMonth
		{
			get;
			set;
		}

		[Display(Name="Mortgage Company Zip")]
		[Required(ErrorMessage="Mortgage Company Zip Required")]
		public string FirstMCZip
		{
			get;
			set;
		}

		[Display(Name="First Mortgage Required")]
		[Required(ErrorMessage="First Mortgage Required")]
		public string FirstMortgageCompany
		{
			get;
			set;
		}

		[Display(Name="Form of Residence")]
		public string FormOfResidence
		{
			get;
			set;
		}

		[Display(Name="Garage")]
		public string Garage
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> GarageTypes
		{
			get;
			set;
		}

		public Guid GuidId
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

		[Display(Name="Is there an existing 1st Position Mortgage Lien Secured by the Property")]
		[Required(ErrorMessage="Is there an existing 1st Position Mortgage Lien Secured by the Property Required")]
		public bool HasPositionMortgage
		{
			get;
			set;
		}

		[Display(Name="Home Owners Association")]
		public bool? HOA
		{
			get;
			set;
		}

		[Display(Name="HOA Dues")]
		public double? HOADues
		{
			get;
			set;
		}

		[Display(Name="per")]
		public string HOADueTime
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> HOADueTimes
		{
			get;
			set;
		}

		[Display(Name="HOA Liens")]
		public double? HOALiens
		{
			get;
			set;
		}

		[Display(Name="Are there any HOA liens against the property?")]
		public bool? HOALiensOnProperty
		{
			get;
			set;
		}

		public List<TempAssetImage> Images
		{
			get;
			set;
		}

		[Display(Name="Insurance Agent Name")]
		public string InsuranceAgentName
		{
			get;
			set;
		}

		[Display(Name="Insurance Agent Phone Number")]
		public string InsuranceAgentPhone
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

		[Display(Name="What languages do you speak?")]
		public string Language
		{
			get;
			set;
		}

		[Display(Name="Major Cross Streets")]
		public string MajorCrossStreets
		{
			get;
			set;
		}

		[Display(Name="Marital Status")]
		public string MaritalStatus
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

		[Display(Name="If Yes, is the Mortgage Lien Assumable")]
		public bool MortgageLienAssumable
		{
			get;
			set;
		}

		[Display(Name="If Yes, is the Mortgage Lien:")]
		public Inview.Epi.EpiFund.Domain.Enum.MortgageLienType MortgageLienType
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

		[Display(Name="Number of Years")]
		public int? NumberOfYears
		{
			get;
			set;
		}

		[Display(Name="Why did you fall behind in your payment(s)?")]
		public string PaymentsFallBehindReason
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> PaymentTypes
		{
			get;
			set;
		}

		[Display(Name="Policy Number")]
		public string PolicyNumber
		{
			get;
			set;
		}

		[Display(Name="Pool")]
		public string Pool
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> PoolTypes
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

		[Display(Name="Property Address")]
		[Required(ErrorMessage="Property Address Required")]
		public string PropertyAddress
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

		[Display(Name="County where Property is Located")]
		[Required(ErrorMessage="County where Property is Located Required")]
		public string PropertyCounty
		{
			get;
			set;
		}

		[Display(Name="Does Property Have an HOA?")]
		public bool? PropertyInHOA
		{
			get;
			set;
		}

		[Display(Name="Property Insurance Carrier")]
		public string PropertyInsuranceCarrier
		{
			get;
			set;
		}

		[Display(Name="Is the property listed for sale?")]
		public bool? PropertyListedForSale
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

		[Display(Name="How is Title to Property Held by Owner?")]
		public Inview.Epi.EpiFund.Domain.ViewModel.PropHoldType PropHoldType
		{
			get;
			set;
		}

		[Display(Name="Year Property was Last Updated")]
		public int? PropLastUpdated
		{
			get;
			set;
		}

		[Display(Name="Recent Upgrading and/or Remodeling")]
		public string RecentUpgradeOrRemodel
		{
			get;
			set;
		}

		[Display(Name="Account Number")]
		public string SecondMCAccountNumber
		{
			get;
			set;
		}

		[Display(Name="Mortgage Company Address")]
		public string SecondMCAddress
		{
			get;
			set;
		}

		[Display(Name="City")]
		public string SecondMCCity
		{
			get;
			set;
		}

		[Display(Name="What is your mortgage balance?")]
		public double? SecondMCCurrentMortgageBalance
		{
			get;
			set;
		}

		[Display(Name="If yes, what is the sale date")]
		public DateTime? SecondMCForclosureSaleDate
		{
			get;
			set;
		}

		[Display(Name="Has second mortgage company started foreclosure action?")]
		public bool? SecondMCHasForclosureStarted
		{
			get;
			set;
		}

		[Display(Name="Is Mortgage an ARM")]
		public bool? SecondMCIsMortgageAnARM
		{
			get;
			set;
		}

		[Display(Name="Date last payment was made")]
		public DateTime? SecondMCLastPaymentDate
		{
			get;
			set;
		}

		[Display(Name="Lender Phone Number")]
		public string SecondMCLenderPhone
		{
			get;
			set;
		}

		[Display(Name="Other Lender Phone Number")]
		public string SecondMCLenderPhoneOther
		{
			get;
			set;
		}

		[Display(Name="What is your monthly payment?")]
		public double? SecondMCMonthlyPayment
		{
			get;
			set;
		}

		[Display(Name="Does mortgage have a PPP?")]
		public bool? SecondMCMortgageHasPPP
		{
			get;
			set;
		}

		[Display(Name="Number of Missed Payments")]
		public int? SecondMCNumberOfMissedPayments
		{
			get;
			set;
		}

		[Display(Name="Mortgage Payment Includes")]
		public string SecondMCPaymentIncludes
		{
			get;
			set;
		}

		[Display(Name="If yes, when does it expire?")]
		public DateTime? SecondMCPPPExpireDate
		{
			get;
			set;
		}

		[Display(Name="State")]
		public string SecondMCState
		{
			get;
			set;
		}

		[Display(Name="Was last payment for that month?")]
		public bool? SecondMCWasLastPaymentForThatMonth
		{
			get;
			set;
		}

		[Display(Name="Zip Code")]
		public string SecondMCZip
		{
			get;
			set;
		}

		[Display(Name="Second Mortgage Company")]
		public string SecondMortgageCompany
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
		public PreferredContactTime SelectedPreferredContactTime
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

		[Display(Name="Spa")]
		public string Spa
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> SpaTypes
		{
			get;
			set;
		}

		[Display(Name="SSN or TIN")]
		public string SSN
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

		public int TotalRentableFeet
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
		public List<AssetUnitSpecification> UnitSpecifications
		{
			get;
			set;
		}

		public List<TempAssetVideo> Videos
		{
			get;
			set;
		}

		[Display(Name="Work Phone Number")]
		[Required(ErrorMessage="Work Phone Number Required")]
		public string WorkPhone
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

		public RealEstateResidentialAssetViewModel()
		{
			this.Images = new List<TempAssetImage>();
			this.Documents = new List<TempAssetDocument>();
			this.Videos = new List<TempAssetVideo>();
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			SelectListItem selectListItem = new SelectListItem()
			{
				Text = "Cell",
				Value = PreferredMethod.CellPhone.ToString()
			};
			selectListItems.Add(selectListItem);
			SelectListItem selectListItem1 = new SelectListItem()
			{
				Text = "Email",
				Value = PreferredMethod.Email.ToString()
			};
			selectListItems.Add(selectListItem1);
			SelectListItem selectListItem2 = new SelectListItem()
			{
				Text = "Fax",
				Value = PreferredMethod.Fax.ToString()
			};
			selectListItems.Add(selectListItem2);
			SelectListItem selectListItem3 = new SelectListItem()
			{
				Text = "Text",
				Value = PreferredMethod.Text.ToString()
			};
			selectListItems.Add(selectListItem3);
			SelectListItem selectListItem4 = new SelectListItem()
			{
				Text = "Mail",
				Value = PreferredMethod.Mail.ToString()
			};
			selectListItems.Add(selectListItem4);
			SelectListItem selectListItem5 = new SelectListItem()
			{
				Text = "Work",
				Value = PreferredMethod.WorkPhone.ToString()
			};
			selectListItems.Add(selectListItem5);
			this.PreferredMethods = selectListItems;
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			SelectListItem selectListItem6 = new SelectListItem()
			{
				Text = "Morning",
				Value = PreferredContactTime.Morning.ToString()
			};
			selectListItems1.Add(selectListItem6);
			SelectListItem selectListItem7 = new SelectListItem()
			{
				Text = "Afternoon",
				Value = PreferredContactTime.Afternoon.ToString()
			};
			selectListItems1.Add(selectListItem7);
			SelectListItem selectListItem8 = new SelectListItem()
			{
				Text = "Evening",
				Value = PreferredContactTime.Evening.ToString()
			};
			selectListItems1.Add(selectListItem8);
			this.PreferredContactTimes = selectListItems1;
			List<SelectListItem> selectListItems2 = new List<SelectListItem>();
			SelectListItem selectListItem9 = new SelectListItem()
			{
				Text = "Corporation",
				Value = CorporateEntityType.Corporation.ToString()
			};
			selectListItems2.Add(selectListItem9);
			SelectListItem selectListItem10 = new SelectListItem()
			{
				Text = "Limited Liability Company",
				Value = CorporateEntityType.LimitedLiabilityCompany.ToString()
			};
			selectListItems2.Add(selectListItem10);
			SelectListItem selectListItem11 = new SelectListItem()
			{
				Text = "Limited Liability Partnership",
				Value = CorporateEntityType.LimitedLiabilityPartnership.ToString()
			};
			selectListItems2.Add(selectListItem11);
			SelectListItem selectListItem12 = new SelectListItem()
			{
				Text = "Joint Venture",
				Value = CorporateEntityType.JointVenture.ToString()
			};
			selectListItems2.Add(selectListItem12);
			SelectListItem selectListItem13 = new SelectListItem()
			{
				Text = "Sole Proprietorship",
				Value = CorporateEntityType.SoleProprietorship.ToString()
			};
			selectListItems2.Add(selectListItem13);
			this.CorporateEntityTypes = selectListItems2;
			List<SelectListItem> selectListItems3 = new List<SelectListItem>();
			SelectListItem selectListItem14 = new SelectListItem()
			{
				Selected = false,
				Text = "CRE Mtg Broker",
				Value = UserType.CREBroker.ToString()
			};
			selectListItems3.Add(selectListItem14);
			SelectListItem selectListItem15 = new SelectListItem()
			{
				Selected = false,
				Text = "COO",
				Value = "COO"
			};
			selectListItems3.Add(selectListItem15);
			SelectListItem selectListItem16 = new SelectListItem()
			{
				Selected = false,
				Text = "CFO",
				Value = "CFO"
			};
			selectListItems3.Add(selectListItem16);
			SelectListItem selectListItem17 = new SelectListItem()
			{
				Selected = false,
				Text = "President",
				Value = "President"
			};
			selectListItems3.Add(selectListItem17);
			SelectListItem selectListItem18 = new SelectListItem()
			{
				Selected = false,
				Text = "VP",
				Value = "VP"
			};
			selectListItems3.Add(selectListItem18);
			SelectListItem selectListItem19 = new SelectListItem()
			{
				Selected = false,
				Text = "Secretary",
				Value = "Secretary"
			};
			selectListItems3.Add(selectListItem19);
			SelectListItem selectListItem20 = new SelectListItem()
			{
				Selected = false,
				Text = "Manager",
				Value = "Manager"
			};
			selectListItems3.Add(selectListItem20);
			SelectListItem selectListItem21 = new SelectListItem()
			{
				Selected = false,
				Text = "Trustee",
				Value = "Trustee"
			};
			selectListItems3.Add(selectListItem21);
			SelectListItem selectListItem22 = new SelectListItem()
			{
				Selected = false,
				Text = "Executor",
				Value = "Executor"
			};
			selectListItems3.Add(selectListItem22);
			this.CorporateTitles = selectListItems3;
			List<SelectListItem> selectListItems4 = new List<SelectListItem>();
			SelectListItem selectListItem23 = new SelectListItem()
			{
				Text = "Month",
				Value = "Month"
			};
			selectListItems4.Add(selectListItem23);
			SelectListItem selectListItem24 = new SelectListItem()
			{
				Text = "Quarter",
				Value = "Quarter"
			};
			selectListItems4.Add(selectListItem24);
			SelectListItem selectListItem25 = new SelectListItem()
			{
				Text = "Year",
				Value = "Year"
			};
			selectListItems4.Add(selectListItem25);
			this.HOADueTimes = selectListItems4;
			List<SelectListItem> selectListItems5 = new List<SelectListItem>();
			SelectListItem selectListItem26 = new SelectListItem()
			{
				Text = "1 Story",
				Value = "1 Story"
			};
			selectListItems5.Add(selectListItem26);
			SelectListItem selectListItem27 = new SelectListItem()
			{
				Text = "Split Level",
				Value = "Split Level"
			};
			selectListItems5.Add(selectListItem27);
			SelectListItem selectListItem28 = new SelectListItem()
			{
				Text = "2 Story",
				Value = "2 Story"
			};
			selectListItems5.Add(selectListItem28);
			SelectListItem selectListItem29 = new SelectListItem()
			{
				Text = "Tri-level Story",
				Value = "Tri-level Story"
			};
			selectListItems5.Add(selectListItem29);
			this.ArchitectureTypes = selectListItems5;
			List<SelectListItem> selectListItems6 = new List<SelectListItem>();
			SelectListItem selectListItem30 = new SelectListItem()
			{
				Text = "Single Family",
				Value = "Single Family"
			};
			selectListItems6.Add(selectListItem30);
			SelectListItem selectListItem31 = new SelectListItem()
			{
				Text = "Patio Home",
				Value = "Patio Home"
			};
			selectListItems6.Add(selectListItem31);
			SelectListItem selectListItem32 = new SelectListItem()
			{
				Text = "Condo",
				Value = "Condo"
			};
			selectListItems6.Add(selectListItem32);
			SelectListItem selectListItem33 = new SelectListItem()
			{
				Text = "TH",
				Value = "TH"
			};
			selectListItems6.Add(selectListItem33);
			SelectListItem selectListItem34 = new SelectListItem()
			{
				Text = "Duplex",
				Value = "Duplex"
			};
			selectListItems6.Add(selectListItem34);
			SelectListItem selectListItem35 = new SelectListItem()
			{
				Text = "Triplex",
				Value = "Triplex"
			};
			selectListItems6.Add(selectListItem35);
			SelectListItem selectListItem36 = new SelectListItem()
			{
				Text = "Fourplex",
				Value = "Fourplex"
			};
			selectListItems6.Add(selectListItem36);
			this.BuildingStyles = selectListItems6;
			List<SelectListItem> selectListItems7 = new List<SelectListItem>();
			SelectListItem selectListItem37 = new SelectListItem()
			{
				Text = "None",
				Value = "None"
			};
			selectListItems7.Add(selectListItem37);
			SelectListItem selectListItem38 = new SelectListItem()
			{
				Text = "In Ground",
				Value = "In Ground"
			};
			selectListItems7.Add(selectListItem38);
			SelectListItem selectListItem39 = new SelectListItem()
			{
				Text = "Above Ground",
				Value = "Above Ground"
			};
			selectListItems7.Add(selectListItem39);
			SelectListItem selectListItem40 = new SelectListItem()
			{
				Text = "Community",
				Value = "Community"
			};
			selectListItems7.Add(selectListItem40);
			this.SpaTypes = selectListItems7;
			List<SelectListItem> selectListItems8 = new List<SelectListItem>();
			SelectListItem selectListItem41 = new SelectListItem()
			{
				Text = "None",
				Value = "None"
			};
			selectListItems8.Add(selectListItem41);
			SelectListItem selectListItem42 = new SelectListItem()
			{
				Text = "Diving",
				Value = "Diving"
			};
			selectListItems8.Add(selectListItem42);
			SelectListItem selectListItem43 = new SelectListItem()
			{
				Text = "Community",
				Value = "Community"
			};
			selectListItems8.Add(selectListItem43);
			SelectListItem selectListItem44 = new SelectListItem()
			{
				Text = "Play",
				Value = "Play"
			};
			selectListItems8.Add(selectListItem44);
			this.PoolTypes = selectListItems8;
			List<SelectListItem> selectListItems9 = new List<SelectListItem>();
			SelectListItem selectListItem45 = new SelectListItem()
			{
				Text = "None",
				Value = "None"
			};
			selectListItems9.Add(selectListItem45);
			SelectListItem selectListItem46 = new SelectListItem()
			{
				Text = "1 Car ",
				Value = "1 Car "
			};
			selectListItems9.Add(selectListItem46);
			SelectListItem selectListItem47 = new SelectListItem()
			{
				Text = "2 Car",
				Value = "2 Car"
			};
			selectListItems9.Add(selectListItem47);
			SelectListItem selectListItem48 = new SelectListItem()
			{
				Text = "1+ Slab",
				Value = "1+ Slab"
			};
			selectListItems9.Add(selectListItem48);
			SelectListItem selectListItem49 = new SelectListItem()
			{
				Text = "2+ Slab",
				Value = "2+ Slab"
			};
			selectListItems9.Add(selectListItem49);
			SelectListItem selectListItem50 = new SelectListItem()
			{
				Text = "Other",
				Value = "Other"
			};
			selectListItems9.Add(selectListItem50);
			this.CarportTypes = selectListItems9;
			List<SelectListItem> selectListItems10 = new List<SelectListItem>();
			SelectListItem selectListItem51 = new SelectListItem()
			{
				Text = "None",
				Value = "None"
			};
			selectListItems10.Add(selectListItem51);
			SelectListItem selectListItem52 = new SelectListItem()
			{
				Text = "1 Car",
				Value = "1 Car"
			};
			selectListItems10.Add(selectListItem52);
			SelectListItem selectListItem53 = new SelectListItem()
			{
				Text = "2 Car",
				Value = "2 Car"
			};
			selectListItems10.Add(selectListItem53);
			SelectListItem selectListItem54 = new SelectListItem()
			{
				Text = "3",
				Value = "3"
			};
			selectListItems10.Add(selectListItem54);
			SelectListItem selectListItem55 = new SelectListItem()
			{
				Text = "4",
				Value = "4"
			};
			selectListItems10.Add(selectListItem55);
			SelectListItem selectListItem56 = new SelectListItem()
			{
				Text = "4+",
				Value = "4+"
			};
			selectListItems10.Add(selectListItem56);
			this.GarageTypes = selectListItems10;
			List<SelectListItem> selectListItems11 = new List<SelectListItem>();
			SelectListItem selectListItem57 = new SelectListItem()
			{
				Text = "1.5",
				Value = "1.5"
			};
			selectListItems11.Add(selectListItem57);
			SelectListItem selectListItem58 = new SelectListItem()
			{
				Text = "1.75",
				Value = "1.75"
			};
			selectListItems11.Add(selectListItem58);
			SelectListItem selectListItem59 = new SelectListItem()
			{
				Text = "2",
				Value = "2"
			};
			selectListItems11.Add(selectListItem59);
			SelectListItem selectListItem60 = new SelectListItem()
			{
				Text = "2.5",
				Value = "2.5"
			};
			selectListItems11.Add(selectListItem60);
			SelectListItem selectListItem61 = new SelectListItem()
			{
				Text = "2.75",
				Value = "2.75"
			};
			selectListItems11.Add(selectListItem61);
			SelectListItem selectListItem62 = new SelectListItem()
			{
				Text = "3",
				Value = "3"
			};
			selectListItems11.Add(selectListItem62);
			SelectListItem selectListItem63 = new SelectListItem()
			{
				Text = "3.5",
				Value = "3.5"
			};
			selectListItems11.Add(selectListItem63);
			SelectListItem selectListItem64 = new SelectListItem()
			{
				Text = "3.75",
				Value = "3.75"
			};
			selectListItems11.Add(selectListItem64);
			SelectListItem selectListItem65 = new SelectListItem()
			{
				Text = "4",
				Value = "4"
			};
			selectListItems11.Add(selectListItem65);
			SelectListItem selectListItem66 = new SelectListItem()
			{
				Text = "4+",
				Value = "4+"
			};
			selectListItems11.Add(selectListItem66);
			this.BathroomCount = selectListItems11;
			List<SelectListItem> selectListItems12 = new List<SelectListItem>();
			SelectListItem selectListItem67 = new SelectListItem()
			{
				Text = "1",
				Value = "1"
			};
			selectListItems12.Add(selectListItem67);
			SelectListItem selectListItem68 = new SelectListItem()
			{
				Text = "2",
				Value = "2"
			};
			selectListItems12.Add(selectListItem68);
			SelectListItem selectListItem69 = new SelectListItem()
			{
				Text = "3",
				Value = "3"
			};
			selectListItems12.Add(selectListItem69);
			SelectListItem selectListItem70 = new SelectListItem()
			{
				Text = "4",
				Value = "4"
			};
			selectListItems12.Add(selectListItem70);
			SelectListItem selectListItem71 = new SelectListItem()
			{
				Text = "5",
				Value = "5"
			};
			selectListItems12.Add(selectListItem71);
			SelectListItem selectListItem72 = new SelectListItem()
			{
				Text = "6",
				Value = "6"
			};
			selectListItems12.Add(selectListItem72);
			SelectListItem selectListItem73 = new SelectListItem()
			{
				Text = "6+",
				Value = "6+"
			};
			selectListItems12.Add(selectListItem73);
			this.BedroomCount = selectListItems12;
			List<SelectListItem> selectListItems13 = new List<SelectListItem>();
			SelectListItem selectListItem74 = new SelectListItem()
			{
				Text = "Taxes",
				Value = "Taxes"
			};
			selectListItems13.Add(selectListItem74);
			SelectListItem selectListItem75 = new SelectListItem()
			{
				Text = "Insurance",
				Value = "Insurance"
			};
			selectListItems13.Add(selectListItem75);
			SelectListItem selectListItem76 = new SelectListItem()
			{
				Text = "Both",
				Value = "Both"
			};
			selectListItems13.Add(selectListItem76);
			this.PaymentTypes = selectListItems13;
			List<SelectListItem> selectListItems14 = new List<SelectListItem>();
			SelectListItem selectListItem77 = new SelectListItem()
			{
				Text = "Commercial Retail",
				Value = AssetType.Retail.ToString()
			};
			selectListItems14.Add(selectListItem77);
			SelectListItem selectListItem78 = new SelectListItem()
			{
				Text = "Commercial Office",
				Value = AssetType.Office.ToString()
			};
			selectListItems14.Add(selectListItem78);
			SelectListItem selectListItem79 = new SelectListItem()
			{
				Text = "Multi-Family",
				Value = AssetType.MultiFamily.ToString()
			};
			selectListItems14.Add(selectListItem79);
			SelectListItem selectListItem80 = new SelectListItem()
			{
				Text = "Commercial Industrial",
				Value = AssetType.Industrial.ToString()
			};
			selectListItems14.Add(selectListItem80);
			SelectListItem selectListItem81 = new SelectListItem()
			{
				Text = "MHP",
				Value = AssetType.MHP.ToString()
			};
			selectListItems14.Add(selectListItem81);
			SelectListItem selectListItem82 = new SelectListItem()
			{
				Text = "Service/Fuel Business",
				Value = AssetType.ConvenienceStoreFuel.ToString()
			};
			selectListItems14.Add(selectListItem82);
			SelectListItem selectListItem83 = new SelectListItem()
			{
				Text = "Medical",
				Value = AssetType.Medical.ToString()
			};
			selectListItems14.Add(selectListItem83);
			SelectListItem selectListItem84 = new SelectListItem()
			{
				Text = "Mixed Use",
				Value = AssetType.MixedUse.ToString()
			};
			selectListItems14.Add(selectListItem84);
			SelectListItem selectListItem85 = new SelectListItem()
			{
				Text = "Other",
				Value = AssetType.Other.ToString()
			};
			selectListItems14.Add(selectListItem85);
			this.PropertyTypes = selectListItems14;
			List<SelectListItem> selectListItems15 = new List<SelectListItem>();
			SelectListItem selectListItem86 = new SelectListItem()
			{
				Text = "AL",
				Value = "AL"
			};
			selectListItems15.Add(selectListItem86);
			SelectListItem selectListItem87 = new SelectListItem()
			{
				Text = "AK",
				Value = "AK"
			};
			selectListItems15.Add(selectListItem87);
			SelectListItem selectListItem88 = new SelectListItem()
			{
				Text = "AZ",
				Value = "AZ"
			};
			selectListItems15.Add(selectListItem88);
			SelectListItem selectListItem89 = new SelectListItem()
			{
				Text = "AR",
				Value = "AR"
			};
			selectListItems15.Add(selectListItem89);
			SelectListItem selectListItem90 = new SelectListItem()
			{
				Text = "CA",
				Value = "CA"
			};
			selectListItems15.Add(selectListItem90);
			SelectListItem selectListItem91 = new SelectListItem()
			{
				Text = "CO",
				Value = "CO"
			};
			selectListItems15.Add(selectListItem91);
			SelectListItem selectListItem92 = new SelectListItem()
			{
				Text = "CT",
				Value = "CT"
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