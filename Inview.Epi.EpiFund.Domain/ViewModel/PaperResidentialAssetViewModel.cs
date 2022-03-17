using Inview.Epi.EpiFund.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class PaperResidentialAssetViewModel
	{
		public IEnumerable<SelectListItem> PagePropertyTypes = new List<SelectListItem>();

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

		[Display(Name="Name of Servicing Agent")]
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

		public IEnumerable<SelectListItem> AppraisalMethods
		{
			get;
			set;
		}

		[Display(Name="Asking Price for Note")]
		public double? AskingPrice
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

		[Display(Name="Balloon Date of Note")]
		public DateTime? BalloonDateOfNote
		{
			get;
			set;
		}

		[Display(Name="Amount of Balloon Due on Balloon Date")]
		public double? BalloonDueAmount
		{
			get;
			set;
		}

		[Display(Name="Number of Bathrooms")]
		public int? Bathrooms
		{
			get;
			set;
		}

		[Display(Name="Number of Bedrooms")]
		public int? Bedrooms
		{
			get;
			set;
		}

		[Display(Name="Account Number")]
		public string BeneficiaryAccountNumber
		{
			get;
			set;
		}

		[Display(Name="City")]
		public string BeneficiaryCity
		{
			get;
			set;
		}

		[Display(Name="Beneficiary Contact Address")]
		public string BeneficiaryContactAddress
		{
			get;
			set;
		}

		[Display(Name="Email")]
		[Required(ErrorMessage="Email Required")]
		public string BeneficiaryEmail
		{
			get;
			set;
		}

		[Display(Name="Fax Number")]
		public string BeneficiaryFax
		{
			get;
			set;
		}

		[Display(Name="Contact Full Name")]
		[Required(ErrorMessage="Contact Full Name Required")]
		public string BeneficiaryFullName
		{
			get;
			set;
		}

		[Display(Name="Your Name/Beneficiary (Entity)")]
		[Required(ErrorMessage="Beneficiary Name Required")]
		public string BeneficiaryName
		{
			get;
			set;
		}

		[Display(Name="Home Phone Number")]
		public string BeneficiaryPhoneCell
		{
			get;
			set;
		}

		[Display(Name="Home Phone Number")]
		[Required(ErrorMessage="Home Phone Number Required")]
		public string BeneficiaryPhoneHome
		{
			get;
			set;
		}

		[Display(Name="Work Phone Number")]
		[Required(ErrorMessage="Work Phone Number Required")]
		public string BeneficiaryPhoneWork
		{
			get;
			set;
		}

		[Display(Name="State")]
		public string BeneficiaryState
		{
			get;
			set;
		}

		[Display(Name="Zip")]
		public string BeneficiaryZip
		{
			get;
			set;
		}

		[Display(Name="BPO if Property at Note Origination")]
		public double? BPOOfProperty
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> ConstructionTypes
		{
			get;
			set;
		}

		[Display(Name="Current Note Principal")]
		public double? CurrentNotePrincipal
		{
			get;
			set;
		}

		public List<AssetDeferredItemViewModel> DeferredMaintenanceItems
		{
			get;
			set;
		}

		public List<TempAssetDocument> Documents
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

		[Display(Name="Interest Rate")]
		public double? FirstInterestRateWRAP
		{
			get;
			set;
		}

		[Display(Name="1st mortgage Balance")]
		public double? FirstmortgageBalanceWRAP
		{
			get;
			set;
		}

		[Display(Name="1st Mortgage Payment")]
		public double? FirstMortgagePaymentWRAP
		{
			get;
			set;
		}

		[Display(Name="General Comments")]
		public string GeneralComments
		{
			get;
			set;
		}

		public Guid GuidId
		{
			get;
			set;
		}

		[Display(Name="Do you have a copy of the property appraisal at the time of note origination?")]
		public string HasCopyOfAppraisal
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

		[Display(Name="Do you have electronic pictures of the securing property?")]
		public bool? HasPicturesOfProperty
		{
			get;
			set;
		}

		[Display(Name="is there title insurance on the note?")]
		public bool? HasTitleInsurance
		{
			get;
			set;
		}

		[Display(Name="Is there homeowners insurance on the property?")]
		public string HomeownerInsurance
		{
			get;
			set;
		}

		public List<TempAssetImage> Images
		{
			get;
			set;
		}

		[Display(Name="Is Note Current")]
		public bool? IsNoteCurrent
		{
			get;
			set;
		}

		[Display(Name="Is property presently a rental?")]
		public bool? IsPropertyRental
		{
			get;
			set;
		}

		[Display(Name="Date of Last Payment Recieved on Note")]
		public DateTime? LastPaymentRecievedOnNote
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

		public IEnumerable<SelectListItem> LotSizes
		{
			get;
			set;
		}

		[Display(Name="Method of Appraisal")]
		public string MethodOfAppraisal
		{
			get;
			set;
		}

		[Display(Name="If property is a rental, what is the monthly rate?")]
		public double? MonthlyRentRate
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> MortgageInstruments
		{
			get;
			set;
		}

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

		[Display(Name="Date of Note Origination")]
		public DateTime? NoteOriginationDate
		{
			get;
			set;
		}

		[Display(Name="City")]
		public string NotePayerCity
		{
			get;
			set;
		}

		[Display(Name="Note Payor Contact Address")]
		public string NotePayerContactAddress
		{
			get;
			set;
		}

		[Display(Name="Email")]
		public string NotePayerEmail
		{
			get;
			set;
		}

		[Display(Name="Fax Number")]
		public string NotePayerFax
		{
			get;
			set;
		}

		[Display(Name="Payor's FICO")]
		public string NotePayerFICO
		{
			get;
			set;
		}

		[Display(Name="Contact Full Name")]
		public string NotePayerFullName
		{
			get;
			set;
		}

		[Display(Name="Name of Note Payor (Entity)")]
		public string NotePayerName
		{
			get;
			set;
		}

		[Display(Name="Cell Phone Number")]
		public string NotePayerPhoneCell
		{
			get;
			set;
		}

		[Display(Name="Home Phone Number")]
		public string NotePayerPhoneHome
		{
			get;
			set;
		}

		[Display(Name="Work Phone Number")]
		public string NotePayerPhoneWork
		{
			get;
			set;
		}

		[Display(Name="SSN or TIN")]
		public string NotePayerSSNOrTIN
		{
			get;
			set;
		}

		[Display(Name="State")]
		public string NotePayerState
		{
			get;
			set;
		}

		[Display(Name="Zip")]
		public string NotePayerZip
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

		[Display(Name="Original Note Principal")]
		public double? OriginalNotePrincipal
		{
			get;
			set;
		}

		[Display(Name="Define the original principal balance of the wrap around note")]
		public double? OriginalPrincipalBalanceWRAP
		{
			get;
			set;
		}

		[Display(Name="Is the property Owner-Occupied at this time?")]
		public bool? OwnerOccupied
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> OwnerOccupiedTypes
		{
			get;
			set;
		}

		[Display(Name="Parking")]
		public string Parking
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> ParkingTypes
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

		[Display(Name="Define the payment history if the note maker (payor)")]
		public string PaymentHistory
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

		[Display(Name="Number if Payments Remaining on Note")]
		public int? PaymentsRemainingOnNote
		{
			get;
			set;
		}

		[Display(Name="Power Service")]
		public string PowerService
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> PowerServices
		{
			get;
			set;
		}

		[Display(Name="Property Access")]
		public string PropertyAccess
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> PropertyAccessTypes
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> PropertyAppraisals
		{
			get;
			set;
		}

		[Display(Name="Property Square Feet")]
		public int? PropertySquareFeet
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> PropertyTypes
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

		[Display(Name="Description of updates/renovation undertaken:")]
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
		public int RenovationYear
		{
			get;
			set;
		}

		public List<SelectListItem> RenovationYears
		{
			get;
			set;
		}

		[Display(Name="Interest Rate")]
		public double? SecondInterestRateWRAP
		{
			get;
			set;
		}

		[Display(Name="2nd Mortgage Balance")]
		public double? SecondMortgageBalanceWRAP
		{
			get;
			set;
		}

		[Display(Name="2nd Mortgage Payment")]
		public double? SecondMortgagePaymentWRAP
		{
			get;
			set;
		}

		[Display(Name="Full Address of Securing Property")]
		[Required(ErrorMessage="Securing Property Address Required")]
		public string SecuringPropertyAddress
		{
			get;
			set;
		}

		[Display(Name="If note is more that 6 months old, what will the securing property appraise for today?")]
		public double? SecuringPropertyAppraisal
		{
			get;
			set;
		}

		[Display(Name="City")]
		[Required(ErrorMessage="Securing Property City Required")]
		public string SecuringPropertyCity
		{
			get;
			set;
		}

		[Display(Name="State")]
		[Required(ErrorMessage="Securing Property State Required")]
		public string SecuringPropertyState
		{
			get;
			set;
		}

		[Display(Name="Zip")]
		[Required(ErrorMessage="Securing Property Zip Required")]
		public string SecuringPropertyZip
		{
			get;
			set;
		}

		[Display(Name="Cash Down Paid by Payor")]
		public double? SellerCarryNoteCashDown
		{
			get;
			set;
		}

		[Display(Name="Property Sales Price")]
		public double? SellerCarryNotePrice
		{
			get;
			set;
		}

		[Display(Name="Sales Date")]
		public DateTime? SellerCarryNoteSalesDate
		{
			get;
			set;
		}

		[Display(Name="Reason for Selling")]
		public string SellingReason
		{
			get;
			set;
		}

		[Display(Name="Sewer Service")]
		public string SewerService
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> SewerServices
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> States
		{
			get;
			set;
		}

		[Display(Name="Define the total monthly payment due on the wrap around note")]
		public double? TotalMonthlyPaymentWRAP
		{
			get;
			set;
		}

		[Display(Name="Type of Construction")]
		public string TypeOfContruction
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

		[Display(Name="Type of Property")]
		[Required(ErrorMessage="Type of Property Required")]
		public string TypeOfProperty
		{
			get;
			set;
		}

		public List<TempAssetVideo> Videos
		{
			get;
			set;
		}

		[Display(Name="Water Service")]
		public string WaterService
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> WaterServices
		{
			get;
			set;
		}

		[Display(Name="Year Built")]
		public int? Year
		{
			get;
			set;
		}

		public PaperResidentialAssetViewModel()
		{
			this.Images = new List<TempAssetImage>();
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
				Text = "Always by the Due Date",
				Value = "Always by the Due Date"
			};
			selectListItems.Add(selectListItem1);
			SelectListItem selectListItem2 = new SelectListItem()
			{
				Text = "Always before the Grace Period Expired",
				Value = "Always before the Grace Period Expired"
			};
			selectListItems.Add(selectListItem2);
			SelectListItem selectListItem3 = new SelectListItem()
			{
				Text = "Sometimes after the Grace Period but before 30 days late",
				Value = "Sometimes after the Grace Period but before 30 days late"
			};
			selectListItems.Add(selectListItem3);
			SelectListItem selectListItem4 = new SelectListItem()
			{
				Text = "Some 30 day late payment history",
				Value = "Some 30 day late payment history"
			};
			selectListItems.Add(selectListItem4);
			SelectListItem selectListItem5 = new SelectListItem()
			{
				Text = "Some 60 day late payment history",
				Value = "Some 60 day late payment history"
			};
			selectListItems.Add(selectListItem5);
			SelectListItem selectListItem6 = new SelectListItem()
			{
				Text = "Note has been in default proceeding before",
				Value = "Note has been in default proceeding before"
			};
			selectListItems.Add(selectListItem6);
			SelectListItem selectListItem7 = new SelectListItem()
			{
				Text = "Note is presently in default and subject to foreclosure",
				Value = "Note is presently in default and subject to foreclosure"
			};
			selectListItems.Add(selectListItem7);
			SelectListItem selectListItem8 = new SelectListItem()
			{
				Text = "Other",
				Value = "Other"
			};
			selectListItems.Add(selectListItem8);
			this.PaymentHistories = selectListItems;
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			SelectListItem selectListItem9 = new SelectListItem()
			{
				Text = "Interest Only",
				Value = "Interest Only"
			};
			selectListItems1.Add(selectListItem9);
			SelectListItem selectListItem10 = new SelectListItem()
			{
				Text = "15 Year Amortization",
				Value = "15 Year Amortization"
			};
			selectListItems1.Add(selectListItem10);
			SelectListItem selectListItem11 = new SelectListItem()
			{
				Text = "30 Year Amortization",
				Value = "30 Year Amortization"
			};
			selectListItems1.Add(selectListItem11);
			SelectListItem selectListItem12 = new SelectListItem()
			{
				Text = "Other",
				Value = "Other"
			};
			selectListItems1.Add(selectListItem12);
			this.AmortTypes = selectListItems1;
			List<SelectListItem> selectListItems2 = new List<SelectListItem>();
			SelectListItem selectListItem13 = new SelectListItem()
			{
				Text = "Deed of Trust",
				Value = "Deed of Trust"
			};
			selectListItems2.Add(selectListItem13);
			SelectListItem selectListItem14 = new SelectListItem()
			{
				Text = "Mortgage",
				Value = "Mortgage"
			};
			selectListItems2.Add(selectListItem14);
			SelectListItem selectListItem15 = new SelectListItem()
			{
				Text = "Land Contract",
				Value = "Land Contract"
			};
			selectListItems2.Add(selectListItem15);
			SelectListItem selectListItem16 = new SelectListItem()
			{
				Text = "Contract For Deed",
				Value = "Contract For Deed"
			};
			selectListItems2.Add(selectListItem16);
			SelectListItem selectListItem17 = new SelectListItem()
			{
				Text = "Other",
				Value = "Other"
			};
			selectListItems2.Add(selectListItem17);
			this.MortgageInstruments = selectListItems2;
			List<SelectListItem> selectListItems3 = new List<SelectListItem>();
			SelectListItem selectListItem18 = new SelectListItem()
			{
				Text = "Annual",
				Value = "Annual"
			};
			selectListItems3.Add(selectListItem18);
			SelectListItem selectListItem19 = new SelectListItem()
			{
				Text = "Monthly",
				Value = "Monthly"
			};
			selectListItems3.Add(selectListItem19);
			SelectListItem selectListItem20 = new SelectListItem()
			{
				Text = "Quarterly",
				Value = "Quarterly"
			};
			selectListItems3.Add(selectListItem20);
			SelectListItem selectListItem21 = new SelectListItem()
			{
				Text = "Semi-annual",
				Value = "Semi-annual"
			};
			selectListItems3.Add(selectListItem21);
			SelectListItem selectListItem22 = new SelectListItem()
			{
				Text = "Other",
				Value = "Other"
			};
			selectListItems3.Add(selectListItem22);
			this.PaymentFrequencies = selectListItems3;
			List<SelectListItem> selectListItems4 = new List<SelectListItem>();
			SelectListItem selectListItem23 = new SelectListItem()
			{
				Text = "1st Position, Institutional",
				Value = "1st Position, Institutional"
			};
			selectListItems4.Add(selectListItem23);
			SelectListItem selectListItem24 = new SelectListItem()
			{
				Text = "1st Position, Private Seller Carryback",
				Value = "1st Position, Private Seller Carryback"
			};
			selectListItems4.Add(selectListItem24);
			SelectListItem selectListItem25 = new SelectListItem()
			{
				Text = "2nd Position, Institutional",
				Value = "2nd Position, Institutional"
			};
			selectListItems4.Add(selectListItem25);
			SelectListItem selectListItem26 = new SelectListItem()
			{
				Text = "2nd Position, Private Seller Carryback",
				Value = "2nd Position, Private Seller Carryback"
			};
			selectListItems4.Add(selectListItem26);
			SelectListItem selectListItem27 = new SelectListItem()
			{
				Text = "3rd Position, Private Seller Carryback",
				Value = "3rd Position, Private Seller Carryback"
			};
			selectListItems4.Add(selectListItem27);
			SelectListItem selectListItem28 = new SelectListItem()
			{
				Text = "Wrap Around, Encompassing 1 Mortgage",
				Value = "Wrap Around, Encompassing 1 Mortgage"
			};
			selectListItems4.Add(selectListItem28);
			SelectListItem selectListItem29 = new SelectListItem()
			{
				Text = "Wrap Around, Encompassing 2 Mortgages",
				Value = "Wrap Around, Encompassing 2 Mortgages"
			};
			selectListItems4.Add(selectListItem29);
			this.NoteTypes = selectListItems4;
			List<SelectListItem> selectListItems5 = new List<SelectListItem>();
			SelectListItem selectListItem30 = new SelectListItem()
			{
				Text = "Independent Appraiser",
				Value = "Independent Appraiser"
			};
			selectListItems5.Add(selectListItem30);
			SelectListItem selectListItem31 = new SelectListItem()
			{
				Text = "Broker Price Opinion",
				Value = "Broker Price Opinion"
			};
			selectListItems5.Add(selectListItem31);
			SelectListItem selectListItem32 = new SelectListItem()
			{
				Text = "Other",
				Value = "Other"
			};
			selectListItems5.Add(selectListItem32);
			this.AppraisalMethods = selectListItems5;
			List<SelectListItem> selectListItems6 = new List<SelectListItem>();
			SelectListItem selectListItem33 = new SelectListItem()
			{
				Text = "Yes, in paper form only",
				Value = "Yes, in paper form only"
			};
			selectListItems6.Add(selectListItem33);
			SelectListItem selectListItem34 = new SelectListItem()
			{
				Text = "Yes, in electronic format",
				Value = "Yes, in electronic format"
			};
			selectListItems6.Add(selectListItem34);
			SelectListItem selectListItem35 = new SelectListItem()
			{
				Text = "No",
				Value = "No"
			};
			selectListItems6.Add(selectListItem35);
			this.PropertyAppraisals = selectListItems6;
			List<SelectListItem> selectListItems7 = new List<SelectListItem>();
			SelectListItem selectListItem36 = new SelectListItem()
			{
				Text = "Yes",
				Value = "Yes"
			};
			selectListItems7.Add(selectListItem36);
			SelectListItem selectListItem37 = new SelectListItem()
			{
				Text = "No, Vacant",
				Value = "No, Vacant"
			};
			selectListItems7.Add(selectListItem37);
			SelectListItem selectListItem38 = new SelectListItem()
			{
				Text = "No, Renter Occupied",
				Value = "No, Renter Occupied"
			};
			selectListItems7.Add(selectListItem38);
			SelectListItem selectListItem39 = new SelectListItem()
			{
				Text = "No, Family Occupied",
				Value = "No, Family Occupied"
			};
			selectListItems7.Add(selectListItem39);
			this.OwnerOccupiedTypes = selectListItems7;
			List<SelectListItem> selectListItems8 = new List<SelectListItem>();
			SelectListItem selectListItem40 = new SelectListItem()
			{
				Text = "Easement",
				Value = "Easement"
			};
			selectListItems8.Add(selectListItem40);
			SelectListItem selectListItem41 = new SelectListItem()
			{
				Text = "Private Road",
				Value = "Private Road"
			};
			selectListItems8.Add(selectListItem41);
			SelectListItem selectListItem42 = new SelectListItem()
			{
				Text = "Public Roads",
				Value = "Public Roads"
			};
			selectListItems8.Add(selectListItem42);
			SelectListItem selectListItem43 = new SelectListItem()
			{
				Text = "Other",
				Value = "Other"
			};
			selectListItems8.Add(selectListItem43);
			this.PropertyAccessTypes = selectListItems8;
			List<SelectListItem> selectListItems9 = new List<SelectListItem>();
			SelectListItem selectListItem44 = new SelectListItem()
			{
				Text = "Public",
				Value = "Public"
			};
			selectListItems9.Add(selectListItem44);
			SelectListItem selectListItem45 = new SelectListItem()
			{
				Text = "Well",
				Value = "Well"
			};
			selectListItems9.Add(selectListItem45);
			SelectListItem selectListItem46 = new SelectListItem()
			{
				Text = "Other",
				Value = "Other"
			};
			selectListItems9.Add(selectListItem46);
			this.WaterServices = selectListItems9;
			List<SelectListItem> selectListItems10 = new List<SelectListItem>();
			SelectListItem selectListItem47 = new SelectListItem()
			{
				Text = "Public",
				Value = "Public"
			};
			selectListItems10.Add(selectListItem47);
			SelectListItem selectListItem48 = new SelectListItem()
			{
				Text = "Septic",
				Value = "Septic"
			};
			selectListItems10.Add(selectListItem48);
			SelectListItem selectListItem49 = new SelectListItem()
			{
				Text = "Other",
				Value = "Other"
			};
			selectListItems10.Add(selectListItem49);
			this.SewerServices = selectListItems10;
			List<SelectListItem> selectListItems11 = new List<SelectListItem>();
			SelectListItem selectListItem50 = new SelectListItem()
			{
				Text = "Gas",
				Value = "Gas"
			};
			selectListItems11.Add(selectListItem50);
			SelectListItem selectListItem51 = new SelectListItem()
			{
				Text = "Electric",
				Value = "Electric"
			};
			selectListItems11.Add(selectListItem51);
			SelectListItem selectListItem52 = new SelectListItem()
			{
				Text = "Both",
				Value = "Both"
			};
			selectListItems11.Add(selectListItem52);
			this.PowerServices = selectListItems11;
			List<SelectListItem> selectListItems12 = new List<SelectListItem>();
			SelectListItem selectListItem53 = new SelectListItem()
			{
				Text = "None",
				Value = "None"
			};
			selectListItems12.Add(selectListItem53);
			SelectListItem selectListItem54 = new SelectListItem()
			{
				Text = "1 Car",
				Value = "1 Car"
			};
			selectListItems12.Add(selectListItem54);
			SelectListItem selectListItem55 = new SelectListItem()
			{
				Text = "2 Car",
				Value = "2 Car"
			};
			selectListItems12.Add(selectListItem55);
			SelectListItem selectListItem56 = new SelectListItem()
			{
				Text = "3",
				Value = "3"
			};
			selectListItems12.Add(selectListItem56);
			SelectListItem selectListItem57 = new SelectListItem()
			{
				Text = "4",
				Value = "4"
			};
			selectListItems12.Add(selectListItem57);
			SelectListItem selectListItem58 = new SelectListItem()
			{
				Text = "4+",
				Value = "4+"
			};
			selectListItems12.Add(selectListItem58);
			this.ParkingTypes = selectListItems12;
			List<SelectListItem> selectListItems13 = new List<SelectListItem>();
			SelectListItem selectListItem59 = new SelectListItem()
			{
				Text = "0 - 7500 ft",
				Value = "0 - 7500 ft"
			};
			selectListItems13.Add(selectListItem59);
			SelectListItem selectListItem60 = new SelectListItem()
			{
				Text = "7501 - 10000 ft",
				Value = "7501 - 10000 ft"
			};
			selectListItems13.Add(selectListItem60);
			SelectListItem selectListItem61 = new SelectListItem()
			{
				Text = "1/4 - 1/3 acre",
				Value = "1/4 - 1/3 acre"
			};
			selectListItems13.Add(selectListItem61);
			SelectListItem selectListItem62 = new SelectListItem()
			{
				Text = "1/3 - 1/2 acre",
				Value = "1/3 - 1/2 acre"
			};
			selectListItems13.Add(selectListItem62);
			SelectListItem selectListItem63 = new SelectListItem()
			{
				Text = "1/2 - 1 acre",
				Value = "1/2 - 1 acre"
			};
			selectListItems13.Add(selectListItem63);
			SelectListItem selectListItem64 = new SelectListItem()
			{
				Text = "1 - 2.5 acre",
				Value = "1 - 2.5 acre"
			};
			selectListItems13.Add(selectListItem64);
			SelectListItem selectListItem65 = new SelectListItem()
			{
				Text = "More than 2.5 acre",
				Value = "More than 2.5 acre"
			};
			selectListItems13.Add(selectListItem65);
			SelectListItem selectListItem66 = new SelectListItem()
			{
				Text = "Other",
				Value = "Other"
			};
			selectListItems13.Add(selectListItem66);
			this.LotSizes = selectListItems13;
			List<SelectListItem> selectListItems14 = new List<SelectListItem>();
			SelectListItem selectListItem67 = new SelectListItem()
			{
				Text = "Block",
				Value = "Block"
			};
			selectListItems14.Add(selectListItem67);
			SelectListItem selectListItem68 = new SelectListItem()
			{
				Text = "Brick",
				Value = "Brick"
			};
			selectListItems14.Add(selectListItem68);
			SelectListItem selectListItem69 = new SelectListItem()
			{
				Text = "Frame",
				Value = "Frame"
			};
			selectListItems14.Add(selectListItem69);
			SelectListItem selectListItem70 = new SelectListItem()
			{
				Text = "Stone",
				Value = "Stone"
			};
			selectListItems14.Add(selectListItem70);
			SelectListItem selectListItem71 = new SelectListItem()
			{
				Text = "Stucco",
				Value = "Stucco"
			};
			selectListItems14.Add(selectListItem71);
			SelectListItem selectListItem72 = new SelectListItem()
			{
				Text = "Other",
				Value = "Other"
			};
			selectListItems14.Add(selectListItem72);
			this.ConstructionTypes = selectListItems14;
			List<SelectListItem> selectListItems15 = new List<SelectListItem>();
			SelectListItem selectListItem73 = new SelectListItem()
			{
				Text = "Improved Lot or Acreage",
				Value = "Improved Lot or Acreage"
			};
			selectListItems15.Add(selectListItem73);
			SelectListItem selectListItem74 = new SelectListItem()
			{
				Text = "Industrial Building",
				Value = "Industrial Building"
			};
			selectListItems15.Add(selectListItem74);
			SelectListItem selectListItem75 = new SelectListItem()
			{
				Text = "Mobile Home Parking",
				Value = "Mobile Home Parking"
			};
			selectListItems15.Add(selectListItem75);
			SelectListItem selectListItem76 = new SelectListItem()
			{
				Text = "Multi-Family",
				Value = "Multi-Family"
			};
			selectListItems15.Add(selectListItem76);
			SelectListItem selectListItem77 = new SelectListItem()
			{
				Text = "Office Building",
				Value = "Office Building"
			};
			selectListItems15.Add(selectListItem77);
			SelectListItem selectListItem78 = new SelectListItem()
			{
				Text = "Raw Acreage",
				Value = "Raw Acreage"
			};
			selectListItems15.Add(selectListItem78);
			SelectListItem selectListItem79 = new SelectListItem()
			{
				Text = "Retail Property",
				Value = "Retail Property"
			};
			selectListItems15.Add(selectListItem79);
			SelectListItem selectListItem80 = new SelectListItem()
			{
				Text = "Unimproved Lot",
				Value = "Unimproved Lot"
			};
			selectListItems15.Add(selectListItem80);
			SelectListItem selectListItem81 = new SelectListItem()
			{
				Text = "Other",
				Value = "Other"
			};
			selectListItems15.Add(selectListItem81);
			this.PropertyTypes = selectListItems15;
			List<SelectListItem> selectListItems16 = new List<SelectListItem>();
			SelectListItem selectListItem82 = new SelectListItem()
			{
				Text = "AL",
				Value = "AL"
			};
			selectListItems16.Add(selectListItem82);
			SelectListItem selectListItem83 = new SelectListItem()
			{
				Text = "AK",
				Value = "AK"
			};
			selectListItems16.Add(selectListItem83);
			SelectListItem selectListItem84 = new SelectListItem()
			{
				Text = "AZ",
				Value = "AZ"
			};
			selectListItems16.Add(selectListItem84);
			SelectListItem selectListItem85 = new SelectListItem()
			{
				Text = "AR",
				Value = "AR"
			};
			selectListItems16.Add(selectListItem85);
			SelectListItem selectListItem86 = new SelectListItem()
			{
				Text = "CA",
				Value = "CA"
			};
			selectListItems16.Add(selectListItem86);
			SelectListItem selectListItem87 = new SelectListItem()
			{
				Text = "CO",
				Value = "CO"
			};
			selectListItems16.Add(selectListItem87);
			SelectListItem selectListItem88 = new SelectListItem()
			{
				Text = "CT",
				Value = "CT"
			};
			selectListItems16.Add(selectListItem88);
			SelectListItem selectListItem89 = new SelectListItem()
			{
				Text = "DC",
				Value = "DC"
			};
			selectListItems16.Add(selectListItem89);
			SelectListItem selectListItem90 = new SelectListItem()
			{
				Text = "DE",
				Value = "DE"
			};
			selectListItems16.Add(selectListItem90);
			SelectListItem selectListItem91 = new SelectListItem()
			{
				Text = "FL",
				Value = "FL"
			};
			selectListItems16.Add(selectListItem91);
			SelectListItem selectListItem92 = new SelectListItem()
			{
				Text = "GA",
				Value = "GA"
			};
			selectListItems16.Add(selectListItem92);
			SelectListItem selectListItem93 = new SelectListItem()
			{
				Text = "HI",
				Value = "HI"
			};
			selectListItems16.Add(selectListItem93);
			SelectListItem selectListItem94 = new SelectListItem()
			{
				Text = "ID",
				Value = "ID"
			};
			selectListItems16.Add(selectListItem94);
			SelectListItem selectListItem95 = new SelectListItem()
			{
				Text = "IL",
				Value = "IL"
			};
			selectListItems16.Add(selectListItem95);
			SelectListItem selectListItem96 = new SelectListItem()
			{
				Text = "IN",
				Value = "IN"
			};
			selectListItems16.Add(selectListItem96);
			SelectListItem selectListItem97 = new SelectListItem()
			{
				Text = "IA",
				Value = "IA"
			};
			selectListItems16.Add(selectListItem97);
			SelectListItem selectListItem98 = new SelectListItem()
			{
				Text = "KS",
				Value = "KS"
			};
			selectListItems16.Add(selectListItem98);
			SelectListItem selectListItem99 = new SelectListItem()
			{
				Text = "KY",
				Value = "KY"
			};
			selectListItems16.Add(selectListItem99);
			SelectListItem selectListItem100 = new SelectListItem()
			{
				Text = "LA",
				Value = "LA"
			};
			selectListItems16.Add(selectListItem100);
			SelectListItem selectListItem101 = new SelectListItem()
			{
				Text = "ME",
				Value = "ME"
			};
			selectListItems16.Add(selectListItem101);
			SelectListItem selectListItem102 = new SelectListItem()
			{
				Text = "MD",
				Value = "MD"
			};
			selectListItems16.Add(selectListItem102);
			SelectListItem selectListItem103 = new SelectListItem()
			{
				Text = "MA",
				Value = "MA"
			};
			selectListItems16.Add(selectListItem103);
			SelectListItem selectListItem104 = new SelectListItem()
			{
				Text = "MI",
				Value = "MI"
			};
			selectListItems16.Add(selectListItem104);
			SelectListItem selectListItem105 = new SelectListItem()
			{
				Text = "MN",
				Value = "MN"
			};
			selectListItems16.Add(selectListItem105);
			SelectListItem selectListItem106 = new SelectListItem()
			{
				Text = "MS",
				Value = "MS"
			};
			selectListItems16.Add(selectListItem106);
			SelectListItem selectListItem107 = new SelectListItem()
			{
				Text = "MO",
				Value = "MO"
			};
			selectListItems16.Add(selectListItem107);
			SelectListItem selectListItem108 = new SelectListItem()
			{
				Text = "MT",
				Value = "MT"
			};
			selectListItems16.Add(selectListItem108);
			SelectListItem selectListItem109 = new SelectListItem()
			{
				Text = "NE",
				Value = "NE"
			};
			selectListItems16.Add(selectListItem109);
			SelectListItem selectListItem110 = new SelectListItem()
			{
				Text = "NV",
				Value = "NV"
			};
			selectListItems16.Add(selectListItem110);
			SelectListItem selectListItem111 = new SelectListItem()
			{
				Text = "NH",
				Value = "NH"
			};
			selectListItems16.Add(selectListItem111);
			SelectListItem selectListItem112 = new SelectListItem()
			{
				Text = "NJ",
				Value = "NJ"
			};
			selectListItems16.Add(selectListItem112);
			SelectListItem selectListItem113 = new SelectListItem()
			{
				Text = "NM",
				Value = "NM"
			};
			selectListItems16.Add(selectListItem113);
			SelectListItem selectListItem114 = new SelectListItem()
			{
				Text = "NY",
				Value = "NY"
			};
			selectListItems16.Add(selectListItem114);
			SelectListItem selectListItem115 = new SelectListItem()
			{
				Text = "NC",
				Value = "NC"
			};
			selectListItems16.Add(selectListItem115);
			SelectListItem selectListItem116 = new SelectListItem()
			{
				Text = "ND",
				Value = "ND"
			};
			selectListItems16.Add(selectListItem116);
			SelectListItem selectListItem117 = new SelectListItem()
			{
				Text = "OH",
				Value = "OH"
			};
			selectListItems16.Add(selectListItem117);
			SelectListItem selectListItem118 = new SelectListItem()
			{
				Text = "OK",
				Value = "OK"
			};
			selectListItems16.Add(selectListItem118);
			SelectListItem selectListItem119 = new SelectListItem()
			{
				Text = "OR",
				Value = "OR"
			};
			selectListItems16.Add(selectListItem119);
			SelectListItem selectListItem120 = new SelectListItem()
			{
				Text = "PA",
				Value = "PA"
			};
			selectListItems16.Add(selectListItem120);
			SelectListItem selectListItem121 = new SelectListItem()
			{
				Text = "RI",
				Value = "RI"
			};
			selectListItems16.Add(selectListItem121);
			SelectListItem selectListItem122 = new SelectListItem()
			{
				Text = "SC",
				Value = "SC"
			};
			selectListItems16.Add(selectListItem122);
			SelectListItem selectListItem123 = new SelectListItem()
			{
				Text = "SD",
				Value = "SD"
			};
			selectListItems16.Add(selectListItem123);
			SelectListItem selectListItem124 = new SelectListItem()
			{
				Text = "TN",
				Value = "TN"
			};
			selectListItems16.Add(selectListItem124);
			SelectListItem selectListItem125 = new SelectListItem()
			{
				Text = "TX",
				Value = "TX"
			};
			selectListItems16.Add(selectListItem125);
			SelectListItem selectListItem126 = new SelectListItem()
			{
				Text = "UT",
				Value = "UT"
			};
			selectListItems16.Add(selectListItem126);
			SelectListItem selectListItem127 = new SelectListItem()
			{
				Text = "VT",
				Value = "VT"
			};
			selectListItems16.Add(selectListItem127);
			SelectListItem selectListItem128 = new SelectListItem()
			{
				Text = "VA",
				Value = "VA"
			};
			selectListItems16.Add(selectListItem128);
			SelectListItem selectListItem129 = new SelectListItem()
			{
				Text = "WA",
				Value = "WA"
			};
			selectListItems16.Add(selectListItem129);
			SelectListItem selectListItem130 = new SelectListItem()
			{
				Text = "WV",
				Value = "WV"
			};
			selectListItems16.Add(selectListItem130);
			SelectListItem selectListItem131 = new SelectListItem()
			{
				Text = "WI",
				Value = "WI"
			};
			selectListItems16.Add(selectListItem131);
			SelectListItem selectListItem132 = new SelectListItem()
			{
				Text = "WY",
				Value = "WY"
			};
			selectListItems16.Add(selectListItem132);
			this.States = selectListItems16;
		}
	}
}