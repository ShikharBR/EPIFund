using Inview.Epi.EpiFund.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class BindingContingentTemplateModel : BaseModel
	{
		[Display(Name="Property Address")]
		[Required(ErrorMessage="Property Address is required")]
		public string Address1
		{
			get;
			set;
		}

		[Display(Name="Property Address 2")]
		public string Address2
		{
			get;
			set;
		}

		[Display(Name="Assessor Number")]
		[Required(ErrorMessage="Assessor Number is required")]
		public string AssessorNumber
		{
			get;
			set;
		}

		public Guid AssetId
		{
			get;
			set;
		}

		public List<AssetNARMember> AssetNARMembers
		{
			get;
			set;
		}

		public int AssetNumber
		{
			get;
			set;
		}

		[Display(Name="Balance Earnest Deposit")]
		public double? BalanceEarnestDeposit
		{
			get;
			set;
		}

		[Display(Name="Beneficiary/Seller")]
		[Required(ErrorMessage="Beneficiary/Seller is required")]
		public string BeneficiarySeller
		{
			get;
			set;
		}

		[Display(Name="Business Phone Number")]
		[Required(ErrorMessage="Business Phone Number is required")]
		public string BusinessPhoneNumber
		{
			get;
			set;
		}

		[Display(Name="First Buyer")]
		public string Buyer1
		{
			get;
			set;
		}

		[Display(Name="Name of Vesting Entity")]
		[Required(ErrorMessage="Name of Vesting Entity is required")]
		public string Buyer1Name
		{
			get;
			set;
		}

		[Display(Name="Second Buyer")]
		public string Buyer2
		{
			get;
			set;
		}

		[Display(Name="Type of Vesting Entity")]
		[Required(ErrorMessage="Type of Vesting Entity is required")]
		public string BuyerAssigneeName
		{
			get;
			set;
		}

		[Display(Name="First Buyer's Assignee")]
		public string BuyersAssignee1
		{
			get;
			set;
		}

		[Display(Name="First Buyer's Assignee Officer")]
		public string BuyersAssignee1Officer
		{
			get;
			set;
		}

		[Display(Name="First Buyer's Assignee Title")]
		public string BuyersAssignee1Title
		{
			get;
			set;
		}

		[Display(Name="Second Buyer's Assignee")]
		public string BuyersAssignee2
		{
			get;
			set;
		}

		[Display(Name="Second Buyer's Assignee Officer")]
		public string BuyersAssignee2Officer
		{
			get;
			set;
		}

		[Display(Name="Second Buyer's Assignee Title")]
		public string BuyersAssignee2Title
		{
			get;
			set;
		}

		[Display(Name="First Buyer Title")]
		public string BuyerTitle1
		{
			get;
			set;
		}

		[Display(Name="Second Buyer Title")]
		public string BuyerTitle2
		{
			get;
			set;
		}

		[Display(Name="Care Of")]
		[Required(ErrorMessage="Care Of is required")]
		public string CareOf
		{
			get;
			set;
		}

		[Display(Name="Cell Phone Number")]
		[Required(ErrorMessage="Cell Phone Number is required")]
		public string CellPhoneNumber
		{
			get;
			set;
		}

		[Display(Name="Property City")]
		[Required(ErrorMessage="Property City is required")]
		public string City
		{
			get;
			set;
		}

		[Display(Name="Closing Date")]
		[Required(ErrorMessage="Closing Date is required")]
		public string ClosingDate
		{
			get;
			set;
		}

		public List<SelectListItem> ClosingDateNames
		{
			get;
			set;
		}

		[Required(ErrorMessage="Closing Date is required")]
		public string ClosingDateNumberOfDays
		{
			get;
			set;
		}

		public List<SelectListItem> ClosingDateNumbers
		{
			get;
			set;
		}

		[Display(Name="Commission Fees")]
		[Required(ErrorMessage="Commission Fees is required")]
		public string CommissionFeesName
		{
			get;
			set;
		}

		[Required(ErrorMessage="Commission Fees Number is required")]
		public string CommissionFeesNumber
		{
			get;
			set;
		}

		[Display(Name="Company")]
		[Required(ErrorMessage="Company is required")]
		public string Company
		{
			get;
			set;
		}

		[Display(Name="CRE Aquisition LOI")]
		[Required(ErrorMessage="CRE Aquisition LOI is required")]
		public string CREAquisitionLOI
		{
			get;
			set;
		}

		[Display(Name="Date")]
		[Required(ErrorMessage="Date is required")]
		public DateTime Date
		{
			get;
			set;
		}

		[Display(Name="Execution Day")]
		public int Day
		{
			get;
			set;
		}

		public List<LOIDocument> Documents
		{
			get;
			set;
		}

		[Display(Name="Due Diligence Period")]
		[Required(ErrorMessage="Due Diligence Period is required")]
		public string DueDiligenceDate
		{
			get;
			set;
		}

		public List<SelectListItem> DueDiligenceNames
		{
			get;
			set;
		}

		[Required(ErrorMessage="Due Diligence Period is required")]
		public string DueDiligenceNumberOfDays
		{
			get;
			set;
		}

		public List<SelectListItem> DueDiligenceNumbers
		{
			get;
			set;
		}

		[Display(Name="Email Address")]
		[RegularExpression("^((([a-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+(\\.([a-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+)*)|((\\x22)((((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(([\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x7f]|\\x21|[\\x23-\\x5b]|[\\x5d-\\x7e]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(\\\\([\\x01-\\x09\\x0b\\x0c\\x0d-\\x7f]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF]))))*(((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(\\x22)))@((([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.)+(([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.?$", ErrorMessage="Invalid email address")]
		[Required(ErrorMessage="Email Address is required")]
		public string EmailAddress
		{
			get;
			set;
		}

		[Display(Name="Email Address")]
		[RegularExpression("^((([a-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+(\\.([a-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+)*)|((\\x22)((((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(([\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x7f]|\\x21|[\\x23-\\x5b]|[\\x5d-\\x7e]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(\\\\([\\x01-\\x09\\x0b\\x0c\\x0d-\\x7f]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF]))))*(((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(\\x22)))@((([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.)+(([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.?$", ErrorMessage="Invalid second email address")]
		[Required(ErrorMessage="Email Address is required")]
		public string EmailAddress2
		{
			get;
			set;
		}

		public List<SelectListItem> EscrowCompanies
		{
			get;
			set;
		}

		[Display(Name="Escrow Company Address")]
		[Required(ErrorMessage="Escrow Company Address is required")]
		public string EscrowCompanyAddress
		{
			get;
			set;
		}

		[Display(Name="Escrow Company Address 2")]
		public string EscrowCompanyAddress2
		{
			get;
			set;
		}

		[Display(Name="Escrow Company City")]
		public string EscrowCompanyCity
		{
			get;
			set;
		}

		[Display(Name="Escrow Company")]
		[Required(ErrorMessage="Escrow Company is required")]
		public string EscrowCompanyName
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

		[Display(Name="Escrow Company State")]
		public string EscrowCompanyState
		{
			get;
			set;
		}

		[Display(Name="Escrow Company Zip")]
		public string EscrowCompanyZip
		{
			get;
			set;
		}

		public List<SelectListItem> EscrowStates
		{
			get;
			set;
		}

		[Display(Name="Fax Number")]
		public string FaxNumber
		{
			get;
			set;
		}

		[Display(Name="Formal Documentation")]
		[Required(ErrorMessage="Formal Documentation is required")]
		public string FormalDocumentationDate
		{
			get;
			set;
		}

		public string FormalDocumentationNumberOfDays
		{
			get;
			set;
		}

		[Display(Name="From")]
		[Required(ErrorMessage="From is required")]
		public string From
		{
			get;
			set;
		}

		public bool HasDocuments
		{
			get;
			set;
		}

		[Display(Name="Initial Earnest Deposit")]
		public double? InitialEarnestDeposit
		{
			get;
			set;
		}

		[Display(Name="Property Address")]
		public string LegalDescription
		{
			get;
			set;
		}

		[Display(Name="Lender")]
		public string Lender
		{
			get;
			set;
		}

		public int ListingAgentCount
		{
			get;
			set;
		}

		public List<SelectListItem> ListingAgents
		{
			get;
			set;
		}

		[Display(Name="Seller’s Acceptance Deadline Date of LOI")]
		public DateTime LOIDate
		{
			get;
			set;
		}

		public Guid LOIId
		{
			get;
			set;
		}

		[Display(Name="Execution Month")]
		public string Month
		{
			get;
			set;
		}

		[Display(Name="Are there any secured mortgages of record purported against subject property?")]
		public bool NoSecuredMortgages
		{
			get;
			set;
		}

		[Display(Name="Object of Purchase")]
		[Required(ErrorMessage="Object of Purchase is required")]
		public string ObjectOfPurchase
		{
			get;
			set;
		}

		[Display(Name="Offering Purchase Price")]
		public double OfferingPurchasePrice
		{
			get;
			set;
		}

		[Display(Name="Office Phone")]
		[Required(ErrorMessage="Office Phone is required")]
		public string OfficePhone
		{
			get;
			set;
		}

		[Display(Name="Officer of Seller")]
		[Required(ErrorMessage="Officer of Seller is required")]
		public string OfficerOfSeller
		{
			get;
			set;
		}

		[Display(Name="Operating Disclosures")]
		[Required(ErrorMessage="Operating Disclosures is required")]
		public string OperatingDisclosureDate
		{
			get;
			set;
		}

		[Required(ErrorMessage="Operating Disclosures is required")]
		public string OperatingDisclosureNumberOfDays
		{
			get;
			set;
		}

		public List<SelectListItem> OperationDisclosureNames
		{
			get;
			set;
		}

		public List<SelectListItem> OperationDisclosureNumbers
		{
			get;
			set;
		}

		[Display(Name="Entity to be released from LOI if LOI is assigned")]
		public string Releasing
		{
			get;
			set;
		}

		[Display(Name="Secured Mortgages")]
		public string SecuredMortgages
		{
			get;
			set;
		}

		public string SelectedListingAgent
		{
			get;
			set;
		}

		[Display(Name="Seller Disclosures")]
		[Required(ErrorMessage="Seller Disclosures is required")]
		public string SellerDisclosureDate
		{
			get;
			set;
		}

		public List<SelectListItem> SellerDisclosureNames
		{
			get;
			set;
		}

		[Required(ErrorMessage="Seller Disclosures is required")]
		public string SellerDisclosureNumberOfDays
		{
			get;
			set;
		}

		public List<SelectListItem> SellerDisclosureNumbers
		{
			get;
			set;
		}

		[Display(Name="First Seller/Receiver")]
		public string SellerReceiver1
		{
			get;
			set;
		}

		[Display(Name="First Seller/Receiver Officer")]
		public string SellerReceiver1Officer
		{
			get;
			set;
		}

		[Display(Name="First Seller/Reciever Title")]
		public string SellerReceiver1Title
		{
			get;
			set;
		}

		[Display(Name="Second Seller/Receiver")]
		public string SellerReceiver2
		{
			get;
			set;
		}

		[Display(Name="Second Seller/Receiver Officer")]
		public string SellerReceiver2Officer
		{
			get;
			set;
		}

		[Display(Name="Second Seller/Receiver Title")]
		public string SellerReceiver2Title
		{
			get;
			set;
		}

		[Display(Name="Property State")]
		[Required(ErrorMessage="Property State is required")]
		public string State
		{
			get;
			set;
		}

		[Display(Name="County of State for Subject Property")]
		public string StateOfCountyAssessors
		{
			get;
			set;
		}

		[Display(Name="State of County Assessor for Subject Property")]
		public string StateOfPropertyTaxOffice
		{
			get;
			set;
		}

		[Display(Name="Terms of Purchase: Additional Term #1")]
		public string Terms1
		{
			get;
			set;
		}

		[Display(Name="Terms of Purchase: Additional Term #2")]
		public string Terms2
		{
			get;
			set;
		}

		[Display(Name="Terms of Purchase: Additional Term #3")]
		public string Terms3
		{
			get;
			set;
		}

		[Display(Name="Terms of Purchase")]
		public double? TermsOfPurchase
		{
			get;
			set;
		}

		[Display(Name="To")]
		[Required(ErrorMessage="To is required")]
		public string To
		{
			get;
			set;
		}

		[Display(Name="Total number of pages for LOI, including Cover")]
		[Required(ErrorMessage="Total number of pages for LOI, including Cover required")]
		public int TotalNumberOfPagesIncludingCover
		{
			get;
			set;
		}

		[Display(Name="Website/Email")]
		[Required(ErrorMessage="Website/Email is required")]
		public string WebsiteEmail
		{
			get;
			set;
		}

		[Display(Name="Work Phone Number")]
		[Required(ErrorMessage="Work Phone Number is required")]
		public string WorkPhoneNumber
		{
			get;
			set;
		}

		[Display(Name="Execution Year")]
		public int Year
		{
			get;
			set;
		}

		[Display(Name="Property Zip")]
		[Required(ErrorMessage="Property Zip is required")]
		public string Zip
		{
			get;
			set;
		}

		public BindingContingentTemplateModel()
		{
			this.EscrowStates = new List<SelectListItem>();
			this.ListingAgents = new List<SelectListItem>();
			this.AssetNARMembers = new List<AssetNARMember>();
			this.Documents = new List<LOIDocument>();
			this.ListingAgents = new List<SelectListItem>();
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			SelectListItem selectListItem = new SelectListItem()
			{
				Text = "Fourteen",
				Value = "Fourteen"
			};
			selectListItems.Add(selectListItem);
			SelectListItem selectListItem1 = new SelectListItem()
			{
				Text = "Twenty One",
				Value = "Twenty One"
			};
			selectListItems.Add(selectListItem1);
			SelectListItem selectListItem2 = new SelectListItem()
			{
				Text = "Thirty",
				Value = "Thirty"
			};
			selectListItems.Add(selectListItem2);
			SelectListItem selectListItem3 = new SelectListItem()
			{
				Text = "Forty Five",
				Value = "Forty Five"
			};
			selectListItems.Add(selectListItem3);
			SelectListItem selectListItem4 = new SelectListItem()
			{
				Text = "Sixty",
				Value = "Sixty"
			};
			selectListItems.Add(selectListItem4);
			this.DueDiligenceNames = selectListItems;
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			SelectListItem selectListItem5 = new SelectListItem()
			{
				Text = "14",
				Value = "14"
			};
			selectListItems1.Add(selectListItem5);
			SelectListItem selectListItem6 = new SelectListItem()
			{
				Text = "21",
				Value = "21"
			};
			selectListItems1.Add(selectListItem6);
			SelectListItem selectListItem7 = new SelectListItem()
			{
				Text = "30",
				Value = "30"
			};
			selectListItems1.Add(selectListItem7);
			SelectListItem selectListItem8 = new SelectListItem()
			{
				Text = "45",
				Value = "45"
			};
			selectListItems1.Add(selectListItem8);
			SelectListItem selectListItem9 = new SelectListItem()
			{
				Text = "60",
				Value = "60"
			};
			selectListItems1.Add(selectListItem9);
			this.DueDiligenceNumbers = selectListItems1;
			List<SelectListItem> selectListItems2 = new List<SelectListItem>();
			SelectListItem selectListItem10 = new SelectListItem()
			{
				Text = "Five",
				Value = "Five"
			};
			selectListItems2.Add(selectListItem10);
			SelectListItem selectListItem11 = new SelectListItem()
			{
				Text = "Six",
				Value = "Six"
			};
			selectListItems2.Add(selectListItem11);
			SelectListItem selectListItem12 = new SelectListItem()
			{
				Text = "Seven",
				Value = "Seven"
			};
			selectListItems2.Add(selectListItem12);
			SelectListItem selectListItem13 = new SelectListItem()
			{
				Text = "Fourteen",
				Value = "Fourteen"
			};
			selectListItems2.Add(selectListItem13);
			SelectListItem selectListItem14 = new SelectListItem()
			{
				Text = "Thirty",
				Value = "Thirty"
			};
			selectListItems2.Add(selectListItem14);
			this.SellerDisclosureNames = selectListItems2;
			List<SelectListItem> selectListItems3 = new List<SelectListItem>();
			SelectListItem selectListItem15 = new SelectListItem()
			{
				Text = "5",
				Value = "5"
			};
			selectListItems3.Add(selectListItem15);
			SelectListItem selectListItem16 = new SelectListItem()
			{
				Text = "6",
				Value = "6"
			};
			selectListItems3.Add(selectListItem16);
			SelectListItem selectListItem17 = new SelectListItem()
			{
				Text = "7",
				Value = "7"
			};
			selectListItems3.Add(selectListItem17);
			SelectListItem selectListItem18 = new SelectListItem()
			{
				Text = "14",
				Value = "14"
			};
			selectListItems3.Add(selectListItem18);
			SelectListItem selectListItem19 = new SelectListItem()
			{
				Text = "30",
				Value = "30"
			};
			selectListItems3.Add(selectListItem19);
			this.SellerDisclosureNumbers = selectListItems3;
			List<SelectListItem> selectListItems4 = new List<SelectListItem>();
			SelectListItem selectListItem20 = new SelectListItem()
			{
				Text = "Five",
				Value = "Five"
			};
			selectListItems4.Add(selectListItem20);
			SelectListItem selectListItem21 = new SelectListItem()
			{
				Text = "Six",
				Value = "Six"
			};
			selectListItems4.Add(selectListItem21);
			SelectListItem selectListItem22 = new SelectListItem()
			{
				Text = "Seven",
				Value = "Seven"
			};
			selectListItems4.Add(selectListItem22);
			SelectListItem selectListItem23 = new SelectListItem()
			{
				Text = "Fourteen",
				Value = "Fourteen"
			};
			selectListItems4.Add(selectListItem23);
			SelectListItem selectListItem24 = new SelectListItem()
			{
				Text = "Thirty",
				Value = "Thirty"
			};
			selectListItems4.Add(selectListItem24);
			this.OperationDisclosureNames = selectListItems4;
			List<SelectListItem> selectListItems5 = new List<SelectListItem>();
			SelectListItem selectListItem25 = new SelectListItem()
			{
				Text = "5",
				Value = "5"
			};
			selectListItems5.Add(selectListItem25);
			SelectListItem selectListItem26 = new SelectListItem()
			{
				Text = "6",
				Value = "6"
			};
			selectListItems5.Add(selectListItem26);
			SelectListItem selectListItem27 = new SelectListItem()
			{
				Text = "7",
				Value = "7"
			};
			selectListItems5.Add(selectListItem27);
			SelectListItem selectListItem28 = new SelectListItem()
			{
				Text = "14",
				Value = "14"
			};
			selectListItems5.Add(selectListItem28);
			SelectListItem selectListItem29 = new SelectListItem()
			{
				Text = "30",
				Value = "30"
			};
			selectListItems5.Add(selectListItem29);
			this.OperationDisclosureNumbers = selectListItems5;
			List<SelectListItem> selectListItems6 = new List<SelectListItem>();
			SelectListItem selectListItem30 = new SelectListItem()
			{
				Text = "Fourteen",
				Value = "Fourteen"
			};
			selectListItems6.Add(selectListItem30);
			SelectListItem selectListItem31 = new SelectListItem()
			{
				Text = "Twenty One",
				Value = "Twenty One"
			};
			selectListItems6.Add(selectListItem31);
			SelectListItem selectListItem32 = new SelectListItem()
			{
				Text = "Thirty",
				Value = "Thirty"
			};
			selectListItems6.Add(selectListItem32);
			SelectListItem selectListItem33 = new SelectListItem()
			{
				Text = "Forty Five",
				Value = "Forty Five"
			};
			selectListItems6.Add(selectListItem33);
			SelectListItem selectListItem34 = new SelectListItem()
			{
				Text = "Sixty",
				Value = "Sixty"
			};
			selectListItems6.Add(selectListItem34);
			this.ClosingDateNames = selectListItems6;
			List<SelectListItem> selectListItems7 = new List<SelectListItem>();
			SelectListItem selectListItem35 = new SelectListItem()
			{
				Text = "14",
				Value = "14"
			};
			selectListItems7.Add(selectListItem35);
			SelectListItem selectListItem36 = new SelectListItem()
			{
				Text = "21",
				Value = "21"
			};
			selectListItems7.Add(selectListItem36);
			SelectListItem selectListItem37 = new SelectListItem()
			{
				Text = "30",
				Value = "30"
			};
			selectListItems7.Add(selectListItem37);
			SelectListItem selectListItem38 = new SelectListItem()
			{
				Text = "45",
				Value = "45"
			};
			selectListItems7.Add(selectListItem38);
			SelectListItem selectListItem39 = new SelectListItem()
			{
				Text = "60",
				Value = "60"
			};
			selectListItems7.Add(selectListItem39);
			this.ClosingDateNumbers = selectListItems7;
			List<SelectListItem> selectListItems8 = new List<SelectListItem>();
			SelectListItem selectListItem40 = new SelectListItem()
			{
				Text = "AL",
				Value = "AL"
			};
			selectListItems8.Add(selectListItem40);
			SelectListItem selectListItem41 = new SelectListItem()
			{
				Text = "AK",
				Value = "AK"
			};
			selectListItems8.Add(selectListItem41);
			SelectListItem selectListItem42 = new SelectListItem()
			{
				Text = "AZ",
				Value = "AZ"
			};
			selectListItems8.Add(selectListItem42);
			SelectListItem selectListItem43 = new SelectListItem()
			{
				Text = "AR",
				Value = "AR"
			};
			selectListItems8.Add(selectListItem43);
			SelectListItem selectListItem44 = new SelectListItem()
			{
				Text = "CA",
				Value = "CA"
			};
			selectListItems8.Add(selectListItem44);
			SelectListItem selectListItem45 = new SelectListItem()
			{
				Text = "CO",
				Value = "CO"
			};
			selectListItems8.Add(selectListItem45);
			SelectListItem selectListItem46 = new SelectListItem()
			{
				Text = "CT",
				Value = "CT"
			};
			selectListItems8.Add(selectListItem46);
			SelectListItem selectListItem47 = new SelectListItem()
			{
				Text = "DC",
				Value = "DC"
			};
			selectListItems8.Add(selectListItem47);
			SelectListItem selectListItem48 = new SelectListItem()
			{
				Text = "DE",
				Value = "DE"
			};
			selectListItems8.Add(selectListItem48);
			SelectListItem selectListItem49 = new SelectListItem()
			{
				Text = "FL",
				Value = "FL"
			};
			selectListItems8.Add(selectListItem49);
			SelectListItem selectListItem50 = new SelectListItem()
			{
				Text = "GA",
				Value = "GA"
			};
			selectListItems8.Add(selectListItem50);
			SelectListItem selectListItem51 = new SelectListItem()
			{
				Text = "HI",
				Value = "HI"
			};
			selectListItems8.Add(selectListItem51);
			SelectListItem selectListItem52 = new SelectListItem()
			{
				Text = "ID",
				Value = "ID"
			};
			selectListItems8.Add(selectListItem52);
			SelectListItem selectListItem53 = new SelectListItem()
			{
				Text = "IL",
				Value = "IL"
			};
			selectListItems8.Add(selectListItem53);
			SelectListItem selectListItem54 = new SelectListItem()
			{
				Text = "IN",
				Value = "IN"
			};
			selectListItems8.Add(selectListItem54);
			SelectListItem selectListItem55 = new SelectListItem()
			{
				Text = "IA",
				Value = "IA"
			};
			selectListItems8.Add(selectListItem55);
			SelectListItem selectListItem56 = new SelectListItem()
			{
				Text = "KS",
				Value = "KS"
			};
			selectListItems8.Add(selectListItem56);
			SelectListItem selectListItem57 = new SelectListItem()
			{
				Text = "KY",
				Value = "KY"
			};
			selectListItems8.Add(selectListItem57);
			SelectListItem selectListItem58 = new SelectListItem()
			{
				Text = "LA",
				Value = "LA"
			};
			selectListItems8.Add(selectListItem58);
			SelectListItem selectListItem59 = new SelectListItem()
			{
				Text = "ME",
				Value = "ME"
			};
			selectListItems8.Add(selectListItem59);
			SelectListItem selectListItem60 = new SelectListItem()
			{
				Text = "MD",
				Value = "MD"
			};
			selectListItems8.Add(selectListItem60);
			SelectListItem selectListItem61 = new SelectListItem()
			{
				Text = "MA",
				Value = "MA"
			};
			selectListItems8.Add(selectListItem61);
			SelectListItem selectListItem62 = new SelectListItem()
			{
				Text = "MI",
				Value = "MI"
			};
			selectListItems8.Add(selectListItem62);
			SelectListItem selectListItem63 = new SelectListItem()
			{
				Text = "MN",
				Value = "MN"
			};
			selectListItems8.Add(selectListItem63);
			SelectListItem selectListItem64 = new SelectListItem()
			{
				Text = "MS",
				Value = "MS"
			};
			selectListItems8.Add(selectListItem64);
			SelectListItem selectListItem65 = new SelectListItem()
			{
				Text = "MO",
				Value = "MO"
			};
			selectListItems8.Add(selectListItem65);
			SelectListItem selectListItem66 = new SelectListItem()
			{
				Text = "MT",
				Value = "MT"
			};
			selectListItems8.Add(selectListItem66);
			SelectListItem selectListItem67 = new SelectListItem()
			{
				Text = "NE",
				Value = "NE"
			};
			selectListItems8.Add(selectListItem67);
			SelectListItem selectListItem68 = new SelectListItem()
			{
				Text = "NV",
				Value = "NV"
			};
			selectListItems8.Add(selectListItem68);
			SelectListItem selectListItem69 = new SelectListItem()
			{
				Text = "NH",
				Value = "NH"
			};
			selectListItems8.Add(selectListItem69);
			SelectListItem selectListItem70 = new SelectListItem()
			{
				Text = "NJ",
				Value = "NJ"
			};
			selectListItems8.Add(selectListItem70);
			SelectListItem selectListItem71 = new SelectListItem()
			{
				Text = "NM",
				Value = "NM"
			};
			selectListItems8.Add(selectListItem71);
			SelectListItem selectListItem72 = new SelectListItem()
			{
				Text = "NY",
				Value = "NY"
			};
			selectListItems8.Add(selectListItem72);
			SelectListItem selectListItem73 = new SelectListItem()
			{
				Text = "NC",
				Value = "NC"
			};
			selectListItems8.Add(selectListItem73);
			SelectListItem selectListItem74 = new SelectListItem()
			{
				Text = "ND",
				Value = "ND"
			};
			selectListItems8.Add(selectListItem74);
			SelectListItem selectListItem75 = new SelectListItem()
			{
				Text = "OH",
				Value = "OH"
			};
			selectListItems8.Add(selectListItem75);
			SelectListItem selectListItem76 = new SelectListItem()
			{
				Text = "OK",
				Value = "OK"
			};
			selectListItems8.Add(selectListItem76);
			SelectListItem selectListItem77 = new SelectListItem()
			{
				Text = "OR",
				Value = "OR"
			};
			selectListItems8.Add(selectListItem77);
			SelectListItem selectListItem78 = new SelectListItem()
			{
				Text = "PA",
				Value = "PA"
			};
			selectListItems8.Add(selectListItem78);
			SelectListItem selectListItem79 = new SelectListItem()
			{
				Text = "RI",
				Value = "RI"
			};
			selectListItems8.Add(selectListItem79);
			SelectListItem selectListItem80 = new SelectListItem()
			{
				Text = "SC",
				Value = "SC"
			};
			selectListItems8.Add(selectListItem80);
			SelectListItem selectListItem81 = new SelectListItem()
			{
				Text = "SD",
				Value = "SD"
			};
			selectListItems8.Add(selectListItem81);
			SelectListItem selectListItem82 = new SelectListItem()
			{
				Text = "TN",
				Value = "TN"
			};
			selectListItems8.Add(selectListItem82);
			SelectListItem selectListItem83 = new SelectListItem()
			{
				Text = "TX",
				Value = "TX"
			};
			selectListItems8.Add(selectListItem83);
			SelectListItem selectListItem84 = new SelectListItem()
			{
				Text = "UT",
				Value = "UT"
			};
			selectListItems8.Add(selectListItem84);
			SelectListItem selectListItem85 = new SelectListItem()
			{
				Text = "VT",
				Value = "VT"
			};
			selectListItems8.Add(selectListItem85);
			SelectListItem selectListItem86 = new SelectListItem()
			{
				Text = "VA",
				Value = "VA"
			};
			selectListItems8.Add(selectListItem86);
			SelectListItem selectListItem87 = new SelectListItem()
			{
				Text = "WA",
				Value = "WA"
			};
			selectListItems8.Add(selectListItem87);
			SelectListItem selectListItem88 = new SelectListItem()
			{
				Text = "WV",
				Value = "WV"
			};
			selectListItems8.Add(selectListItem88);
			SelectListItem selectListItem89 = new SelectListItem()
			{
				Text = "WI",
				Value = "WI"
			};
			selectListItems8.Add(selectListItem89);
			SelectListItem selectListItem90 = new SelectListItem()
			{
				Text = "WY",
				Value = "WY"
			};
			selectListItems8.Add(selectListItem90);
			this.EscrowStates = selectListItems8;
		}
	}
}