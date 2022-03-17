using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class RegistrationModel
	{
		[Display(Name="Acronym for Corporate Entity")]
		public string AcroynmForCorporateEntity
		{
			get;
			set;
		}

		[Display(Name="Corporate Entity Address Line 1")]
		public string AddressLine1
		{
			get;
			set;
		}

		[Display(Name="Corporate Entity Address Line 2")]
		public string AddressLine2
		{
			get;
			set;
		}

		[Required]
		public bool AgreesToMDA
		{
			get;
			set;
		}

		[Required]
		public bool AgreesToNCND
		{
			get;
			set;
		}

		public string AlternateEmail
		{
			get;
			set;
		}

		public int? AssetId
		{
			get;
			set;
		}

		public List<Guid> AssetIds
		{
			get;
			set;
		}

		[Display(Name="Cell Phone Number")]
		[Required]
		public string CellNumber
		{
			get;
			set;
		}

		[Display(Name="City")]
		public string City
		{
			get;
			set;
		}

		[Display(Name="Commercial Office")]
		public bool CommercialOfficeInterest
		{
			get;
			set;
		}

		[Display(Name="Commercial Other")]
		public bool CommercialOtherInterest
		{
			get;
			set;
		}

		[Display(Name="Commercial Retail")]
		public bool CommercialRetailInterest
		{
			get;
			set;
		}

		[Display(Name="Company Name")]
		public string CompanyName
		{
			get;
			set;
		}

		[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage="Password and confirm password do not match.")]
		[DataType(DataType.Password)]
		[Display(Name="Confirm Password")]
		[Required]
		public string ConfirmPassword
		{
			get;
			set;
		}

		public List<SelectListItem> CorporateEntityTypes
		{
			get;
			set;
		}

		[Display(Name="Corporate TIN or Sole Proprietorship SSN")]
		public string CorporateTIN
		{
			get;
			set;
		}

		[Display(Name="Corporate Title")]
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

		[Display(Name="Execution Day")]
		public int Day
		{
			get
			{
				return DateTime.Now.Day;
			}
		}

		[Display(Name="Corporate Entity EIN")]
		public string EIN
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

		[Display(Name="First Name")]
		[RegularExpression("([a-zA-Z\\d]+[\\w\\d]*|)[a-zA-Z]+[\\w\\d.]*", ErrorMessage="Invalid first name")]
		[Required]
		public string FirstName
		{
			get;
			set;
		}

		[Display(Name="Fractured Condo Portfolio's")]
		public bool FracturedCondoPortfoliosInterest
		{
			get;
			set;
		}

		[Display(Name="Fuel Service Retail Property")]
		public bool FuelServicePropertyInterest
		{
			get;
			set;
		}

		public string FullName
		{
			get
			{
				return string.Concat(this.FirstName, " ", this.LastName);
			}
		}

		[Display(Name="Government Tenant Property")]
		public bool GorvernmentTenantPropertyInterest
		{
			get;
			set;
		}

		public bool HideRegistrationCategoryForListingAgent
		{
			get;
			set;
		}

		[Display(Name="Home Phone Number")]
		public string HomeNumber
		{
			get;
			set;
		}

		[Display(Name="Industry Tenant Property")]
		public bool IndustryTenantPropertyInterest
		{
			get;
			set;
		}

		[Display(Name="Is Active?")]
		public bool IsActive
		{
			get;
			set;
		}

		[Display(Name="Is \"Certificate of Good Standing\" available for your Corporate Entity from its residing Secretary of State?")]
		public bool? IsCertificateOfGoodStandingAvailable
		{
			get;
			set;
		}

		[Display(Name="Last Name")]
		[RegularExpression("([a-zA-Z\\d]+[\\w\\d]*|)[a-zA-Z]+[\\w\\d.]*", ErrorMessage="Invalid last name")]
		[Required]
		public string LastName
		{
			get;
			set;
		}

		[Display(Name="Operating License Description")]
		public string LicenseDesc
		{
			get;
			set;
		}

		[Display(Name="Operating License Number")]
		public string LicenseNumber
		{
			get;
			set;
		}

		public string machineName
		{
			get;
			set;
		}

		public string ManagingOfficerName
		{
			get;
			set;
		}

		[Display(Name="Medical Tenant Property")]
		public bool MedicalTenantPropertyInterest
		{
			get;
			set;
		}

		[Display(Name="Mobile Home Park")]
		public bool MHPInterest
		{
			get;
			set;
		}

		[Display(Name="Mini-Storage Property")]
		public bool MiniStoragePropertyInterest
		{
			get;
			set;
		}

		[Display(Name="Mixed Use Commercial Property")]
		public bool MixedUseCommercialPropertyInterest
		{
			get;
			set;
		}

		[Display(Name="Execution Month")]
		public string Month
		{
			get
			{
				return DateTime.Now.ToString("MMMM");
			}
		}

		[Display(Name="Multi-Family")]
		public bool MultiFamilyInterest
		{
			get;
			set;
		}

		public string NewICAdmin
		{
			get;
			set;
		}

		[Display(Name="Non Completed Developments")]
		public bool NonCompletedDevelopmentsInterest
		{
			get;
			set;
		}

		[Display(Name="Office Tenant Property")]
		public bool OfficeTenantPropertyInterest
		{
			get;
			set;
		}

		[Display(Name="Parking Garage Property")]
		public bool ParkingGaragePropertyInterest
		{
			get;
			set;
		}

		[Display(Name="Password")]
		[Required]
		public string Password
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

		public string ReferralId
		{
			get;
			set;
		}

		[Display(Name="Resort/Hotel/Motel Property")]
		public bool ResortHotelMotelPropertyInterest
		{
			get;
			set;
		}

		[Display(Name="Retail Tenant Property")]
		public bool RetailTenantPropertyInterest
		{
			get;
			set;
		}

		public string ReturnUrl
		{
			get;
			set;
		}

		[Display(Name="CRE Mortgage Secured Paper")]
		public bool SecuredPaperInterest
		{
			get;
			set;
		}

		[Display(Name="Type of Corporate Entity")]
		public CorporateEntityType SelectedCorporateEntityType
		{
			get;
			set;
		}

		[Display(Name="Preferred Contact Time")]
		[Required]
		public List<PreferredContactTime> SelectedPreferredContactTime
		{
			get;
			set;
		}

		[Display(Name="Preferred Method of Communications")]
		[Required]
		public List<PreferredMethod> SelectedPreferredMethods
		{
			get;
			set;
		}

		[Display(Name="State")]
		public string SelectedState
		{
			get;
			set;
		}

		[Display(Name="Registration Category")]
		[Required]
		public UserType SelectedUserType
		{
			get;
			set;
		}

		[Display(Name="Single Tenant Retail Portfolio's")]
		public bool SingleTenantRetailPortfoliosInterest
		{
			get;
			set;
		}

		[Display(Name="State Where License Is Held")]
		public string StateLicenseHeld
		{
			get;
			set;
		}

		[Display(Name="State of Origin of Corporate Entity")]
		public string StateOfOriginCorporateEntity
		{
			get;
			set;
		}

		public List<SelectListItem> States
		{
			get;
			set;
		}

		[Required]
		public bool TermsOfUse
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		[Display(Name="Email")]
		[RegularExpression("^((([a-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+(\\.([a-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+)*)|((\\x22)((((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(([\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x7f]|\\x21|[\\x23-\\x5b]|[\\x5d-\\x7e]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(\\\\([\\x01-\\x09\\x0b\\x0c\\x0d-\\x7f]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF]))))*(((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(\\x22)))@((([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.)+(([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.?$", ErrorMessage="Invalid email address")]
		[Required]
		public string Username
		{
			get;
			set;
		}

		public List<SelectListItem> UserTypes
		{
			get;
			set;
		}

		[Display(Name="Work Number")]
		public string WorkNumber
		{
			get;
			set;
		}

		[Display(Name="Execution Year")]
		public int Year
		{
			get
			{
				return DateTime.Now.Year;
			}
		}

		[Display(Name="Zip")]
		public string Zip
		{
			get;
			set;
		}

		public RegistrationModel()
		{
			this.AssetIds = new List<Guid>();
			this.HideRegistrationCategoryForListingAgent = false;
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			SelectListItem selectListItem = new SelectListItem()
			{
				Selected = false,
				Text = "CRE Mtg Broker",
				Value = UserType.CREBroker.ToString()
			};
			selectListItems.Add(selectListItem);
			SelectListItem selectListItem1 = new SelectListItem()
			{
				Selected = false,
				Text = "COO",
				Value = "COO"
			};
			selectListItems.Add(selectListItem1);
			SelectListItem selectListItem2 = new SelectListItem()
			{
				Selected = false,
				Text = "CFO",
				Value = "CFO"
			};
			selectListItems.Add(selectListItem2);
			SelectListItem selectListItem3 = new SelectListItem()
			{
				Selected = false,
				Text = "President",
				Value = "President"
			};
			selectListItems.Add(selectListItem3);
			SelectListItem selectListItem4 = new SelectListItem()
			{
				Selected = false,
				Text = "VP",
				Value = "VP"
			};
			selectListItems.Add(selectListItem4);
			SelectListItem selectListItem5 = new SelectListItem()
			{
				Selected = false,
				Text = "Secretary",
				Value = "Secretary"
			};
			selectListItems.Add(selectListItem5);
			SelectListItem selectListItem6 = new SelectListItem()
			{
				Selected = false,
				Text = "Manager",
				Value = "Manager"
			};
			selectListItems.Add(selectListItem6);
			SelectListItem selectListItem7 = new SelectListItem()
			{
				Selected = false,
				Text = "Trustee",
				Value = "Trustee"
			};
			selectListItems.Add(selectListItem7);
			SelectListItem selectListItem8 = new SelectListItem()
			{
				Selected = false,
				Text = "Executor",
				Value = "Executor"
			};
			selectListItems.Add(selectListItem8);
			this.CorporateTitles = selectListItems;
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			SelectListItem selectListItem9 = new SelectListItem()
			{
				Selected = false,
				Text = "CRE Mtg Broker",
				Value = UserType.CREBroker.ToString()
			};
			selectListItems1.Add(selectListItem9);
			SelectListItem selectListItem10 = new SelectListItem()
			{
				Selected = false,
				Text = "CRE Mtg Lender",
				Value = UserType.CRELender.ToString()
			};
			selectListItems1.Add(selectListItem10);
			SelectListItem selectListItem11 = new SelectListItem()
			{
				Selected = false,
				Text = "Principal Investor",
				Value = UserType.Investor.ToString()
			};
			selectListItems1.Add(selectListItem11);
			SelectListItem selectListItem12 = new SelectListItem()
			{
				Selected = false,
				Text = "CRE Property Owner",
				Value = UserType.CREOwner.ToString()
			};
			selectListItems1.Add(selectListItem12);
			SelectListItem selectListItem13 = new SelectListItem()
			{
				Selected = false,
				Text = "CRE Mtg Note Owner",
				Value = UserType.CREMtgNoteOwner.ToString()
			};
			selectListItems1.Add(selectListItem13);
			SelectListItem selectListItem14 = new SelectListItem()
			{
				Selected = false,
				Text = "IC Participant",
				Value = UserType.ICAdmin.ToString()
			};
			selectListItems1.Add(selectListItem14);
			SelectListItem selectListItem15 = new SelectListItem()
			{
				Value = UserType.ListingAgent.ToString(),
				Text = "NAR Member"
			};
			selectListItems1.Add(selectListItem15);
			this.UserTypes = selectListItems1;
			List<SelectListItem> selectListItems2 = new List<SelectListItem>();
			SelectListItem selectListItem16 = new SelectListItem()
			{
				Text = "Corporation",
				Value = CorporateEntityType.Corporation.ToString()
			};
			selectListItems2.Add(selectListItem16);
			SelectListItem selectListItem17 = new SelectListItem()
			{
				Text = "Limited Liability Company",
				Value = CorporateEntityType.LimitedLiabilityCompany.ToString()
			};
			selectListItems2.Add(selectListItem17);
			SelectListItem selectListItem18 = new SelectListItem()
			{
				Text = "Limited Liability Partnership",
				Value = CorporateEntityType.LimitedLiabilityPartnership.ToString()
			};
			selectListItems2.Add(selectListItem18);
			SelectListItem selectListItem19 = new SelectListItem()
			{
				Text = "Joint Venture",
				Value = CorporateEntityType.JointVenture.ToString()
			};
			selectListItems2.Add(selectListItem19);
			SelectListItem selectListItem20 = new SelectListItem()
			{
				Text = "Sole Proprietorship",
				Value = CorporateEntityType.SoleProprietorship.ToString()
			};
			selectListItems2.Add(selectListItem20);
			this.CorporateEntityTypes = selectListItems2;
			List<SelectListItem> selectListItems3 = new List<SelectListItem>();
			SelectListItem selectListItem21 = new SelectListItem()
			{
				Text = "AL",
				Value = "AL"
			};
			selectListItems3.Add(selectListItem21);
			SelectListItem selectListItem22 = new SelectListItem()
			{
				Text = "AK",
				Value = "AK"
			};
			selectListItems3.Add(selectListItem22);
			SelectListItem selectListItem23 = new SelectListItem()
			{
				Text = "AZ",
				Value = "AZ"
			};
			selectListItems3.Add(selectListItem23);
			SelectListItem selectListItem24 = new SelectListItem()
			{
				Text = "AR",
				Value = "AR"
			};
			selectListItems3.Add(selectListItem24);
			SelectListItem selectListItem25 = new SelectListItem()
			{
				Text = "CA",
				Value = "CA"
			};
			selectListItems3.Add(selectListItem25);
			SelectListItem selectListItem26 = new SelectListItem()
			{
				Text = "CO",
				Value = "CO"
			};
			selectListItems3.Add(selectListItem26);
			SelectListItem selectListItem27 = new SelectListItem()
			{
				Text = "CT",
				Value = "CT"
			};
			selectListItems3.Add(selectListItem27);
			SelectListItem selectListItem28 = new SelectListItem()
			{
				Text = "DE",
				Value = "DE"
			};
			selectListItems3.Add(selectListItem28);
			SelectListItem selectListItem29 = new SelectListItem()
			{
				Text = "FL",
				Value = "FL"
			};
			selectListItems3.Add(selectListItem29);
			SelectListItem selectListItem30 = new SelectListItem()
			{
				Text = "GA",
				Value = "GA"
			};
			selectListItems3.Add(selectListItem30);
			SelectListItem selectListItem31 = new SelectListItem()
			{
				Text = "HI",
				Value = "HI"
			};
			selectListItems3.Add(selectListItem31);
			SelectListItem selectListItem32 = new SelectListItem()
			{
				Text = "ID",
				Value = "ID"
			};
			selectListItems3.Add(selectListItem32);
			SelectListItem selectListItem33 = new SelectListItem()
			{
				Text = "IL",
				Value = "IL"
			};
			selectListItems3.Add(selectListItem33);
			SelectListItem selectListItem34 = new SelectListItem()
			{
				Text = "IN",
				Value = "IN"
			};
			selectListItems3.Add(selectListItem34);
			SelectListItem selectListItem35 = new SelectListItem()
			{
				Text = "IA",
				Value = "IA"
			};
			selectListItems3.Add(selectListItem35);
			SelectListItem selectListItem36 = new SelectListItem()
			{
				Text = "KS",
				Value = "KS"
			};
			selectListItems3.Add(selectListItem36);
			SelectListItem selectListItem37 = new SelectListItem()
			{
				Text = "KY",
				Value = "KY"
			};
			selectListItems3.Add(selectListItem37);
			SelectListItem selectListItem38 = new SelectListItem()
			{
				Text = "LA",
				Value = "LA"
			};
			selectListItems3.Add(selectListItem38);
			SelectListItem selectListItem39 = new SelectListItem()
			{
				Text = "ME",
				Value = "ME"
			};
			selectListItems3.Add(selectListItem39);
			SelectListItem selectListItem40 = new SelectListItem()
			{
				Text = "MD",
				Value = "MD"
			};
			selectListItems3.Add(selectListItem40);
			SelectListItem selectListItem41 = new SelectListItem()
			{
				Text = "MA",
				Value = "MA"
			};
			selectListItems3.Add(selectListItem41);
			SelectListItem selectListItem42 = new SelectListItem()
			{
				Text = "MI",
				Value = "MI"
			};
			selectListItems3.Add(selectListItem42);
			SelectListItem selectListItem43 = new SelectListItem()
			{
				Text = "MN",
				Value = "MN"
			};
			selectListItems3.Add(selectListItem43);
			SelectListItem selectListItem44 = new SelectListItem()
			{
				Text = "MS",
				Value = "MS"
			};
			selectListItems3.Add(selectListItem44);
			SelectListItem selectListItem45 = new SelectListItem()
			{
				Text = "MO",
				Value = "MO"
			};
			selectListItems3.Add(selectListItem45);
			SelectListItem selectListItem46 = new SelectListItem()
			{
				Text = "MT",
				Value = "MT"
			};
			selectListItems3.Add(selectListItem46);
			SelectListItem selectListItem47 = new SelectListItem()
			{
				Text = "NE",
				Value = "NE"
			};
			selectListItems3.Add(selectListItem47);
			SelectListItem selectListItem48 = new SelectListItem()
			{
				Text = "NV",
				Value = "NV",
				Selected = true
			};
			selectListItems3.Add(selectListItem48);
			SelectListItem selectListItem49 = new SelectListItem()
			{
				Text = "NH",
				Value = "NH"
			};
			selectListItems3.Add(selectListItem49);
			SelectListItem selectListItem50 = new SelectListItem()
			{
				Text = "NJ",
				Value = "NJ"
			};
			selectListItems3.Add(selectListItem50);
			SelectListItem selectListItem51 = new SelectListItem()
			{
				Text = "NM",
				Value = "NM"
			};
			selectListItems3.Add(selectListItem51);
			SelectListItem selectListItem52 = new SelectListItem()
			{
				Text = "NY",
				Value = "NY"
			};
			selectListItems3.Add(selectListItem52);
			SelectListItem selectListItem53 = new SelectListItem()
			{
				Text = "NC",
				Value = "NC"
			};
			selectListItems3.Add(selectListItem53);
			SelectListItem selectListItem54 = new SelectListItem()
			{
				Text = "ND",
				Value = "ND"
			};
			selectListItems3.Add(selectListItem54);
			SelectListItem selectListItem55 = new SelectListItem()
			{
				Text = "OH",
				Value = "OH"
			};
			selectListItems3.Add(selectListItem55);
			SelectListItem selectListItem56 = new SelectListItem()
			{
				Text = "OK",
				Value = "OK"
			};
			selectListItems3.Add(selectListItem56);
			SelectListItem selectListItem57 = new SelectListItem()
			{
				Text = "OR",
				Value = "OR"
			};
			selectListItems3.Add(selectListItem57);
			SelectListItem selectListItem58 = new SelectListItem()
			{
				Text = "PA",
				Value = "PA"
			};
			selectListItems3.Add(selectListItem58);
			SelectListItem selectListItem59 = new SelectListItem()
			{
				Text = "RI",
				Value = "RI"
			};
			selectListItems3.Add(selectListItem59);
			SelectListItem selectListItem60 = new SelectListItem()
			{
				Text = "SC",
				Value = "SC"
			};
			selectListItems3.Add(selectListItem60);
			SelectListItem selectListItem61 = new SelectListItem()
			{
				Text = "SD",
				Value = "SD"
			};
			selectListItems3.Add(selectListItem61);
			SelectListItem selectListItem62 = new SelectListItem()
			{
				Text = "TN",
				Value = "TN"
			};
			selectListItems3.Add(selectListItem62);
			SelectListItem selectListItem63 = new SelectListItem()
			{
				Text = "TX",
				Value = "TX"
			};
			selectListItems3.Add(selectListItem63);
			SelectListItem selectListItem64 = new SelectListItem()
			{
				Text = "UT",
				Value = "UT"
			};
			selectListItems3.Add(selectListItem64);
			SelectListItem selectListItem65 = new SelectListItem()
			{
				Text = "VT",
				Value = "VT"
			};
			selectListItems3.Add(selectListItem65);
			SelectListItem selectListItem66 = new SelectListItem()
			{
				Text = "VA",
				Value = "VA"
			};
			selectListItems3.Add(selectListItem66);
			SelectListItem selectListItem67 = new SelectListItem()
			{
				Text = "WA",
				Value = "WA"
			};
			selectListItems3.Add(selectListItem67);
			SelectListItem selectListItem68 = new SelectListItem()
			{
				Text = "WV",
				Value = "WV"
			};
			selectListItems3.Add(selectListItem68);
			SelectListItem selectListItem69 = new SelectListItem()
			{
				Text = "WI",
				Value = "WI"
			};
			selectListItems3.Add(selectListItem69);
			SelectListItem selectListItem70 = new SelectListItem()
			{
				Text = "WY",
				Value = "WY"
			};
			selectListItems3.Add(selectListItem70);
			this.States = selectListItems3;
			List<SelectListItem> selectListItems4 = new List<SelectListItem>();
			SelectListItem selectListItem71 = new SelectListItem()
			{
				Text = "Cell",
				Value = PreferredMethod.CellPhone.ToString()
			};
			selectListItems4.Add(selectListItem71);
			SelectListItem selectListItem72 = new SelectListItem()
			{
				Text = "Email",
				Value = PreferredMethod.Email.ToString()
			};
			selectListItems4.Add(selectListItem72);
			SelectListItem selectListItem73 = new SelectListItem()
			{
				Text = "Fax",
				Value = PreferredMethod.Fax.ToString()
			};
			selectListItems4.Add(selectListItem73);
			SelectListItem selectListItem74 = new SelectListItem()
			{
				Text = "Text",
				Value = PreferredMethod.Text.ToString()
			};
			selectListItems4.Add(selectListItem74);
			SelectListItem selectListItem75 = new SelectListItem()
			{
				Text = "Mail",
				Value = PreferredMethod.Mail.ToString()
			};
			selectListItems4.Add(selectListItem75);
			SelectListItem selectListItem76 = new SelectListItem()
			{
				Text = "Work",
				Value = PreferredMethod.WorkPhone.ToString()
			};
			selectListItems4.Add(selectListItem76);
			this.PreferredMethods = selectListItems4;
			List<SelectListItem> selectListItems5 = new List<SelectListItem>();
			SelectListItem selectListItem77 = new SelectListItem()
			{
				Text = "Morning",
				Value = PreferredContactTime.Morning.ToString()
			};
			selectListItems5.Add(selectListItem77);
			SelectListItem selectListItem78 = new SelectListItem()
			{
				Text = "Afternoon",
				Value = PreferredContactTime.Afternoon.ToString()
			};
			selectListItems5.Add(selectListItem78);
			SelectListItem selectListItem79 = new SelectListItem()
			{
				Text = "Evening",
				Value = PreferredContactTime.Evening.ToString()
			};
			selectListItems5.Add(selectListItem79);
			this.PreferredContactTimes = selectListItems5;
		}
	}
}