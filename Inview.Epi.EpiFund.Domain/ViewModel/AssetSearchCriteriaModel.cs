using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AssetSearchCriteriaModel
	{
		[Display(Name="Address Line 1 of Purchasing Entity")]
		public string AddressLine1OfPurchasingEntity
		{
			get;
			set;
		}

		[Display(Name="Address Line 2 of Purchasing Entity")]
		public string AddressLine2OfPurchasingEntity
		{
			get;
			set;
		}

		[Display(Name="Amount of Intended Cap Equity")]
		public decimal AmountOfIntendedCapEquity
		{
			get;
			set;
		}

		public int AssetSearchCriteriaId
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> AssetTypes
		{
			get;
			set;
		}

		[Display(Name="Cell Number of Entity")]
		public string CellNumberOfEntity
		{
			get;
			set;
		}

		[Display(Name="City")]
		public string CityOfPurchasingEntity
		{
			get;
			set;
		}

		[Display(Name="Contact Number of Corporate Officer")]
		public string ContactNumberOfCorporateOfficer
		{
			get;
			set;
		}

		[Display(Name="Contact Number of Other Corporate Officer")]
		public string ContactNumberOfOtherCorporateOfficer
		{
			get;
			set;
		}

		public List<SelectListItem> CorporateEntityTypes
		{
			get;
			set;
		}

		[Display(Name="Identify CRE PMF Broker/Lender")]
		public string CREPMFBrokerLender
		{
			get;
			set;
		}

		[Display(Name="Date Entered")]
		public DateTime DateEntered
		{
			get;
			set;
		}

		public DueDiligenceCheckListModel DueDiligenceCheckList
		{
			get;
			set;
		}

		[Display(Name="Email Address of Corporate Officer")]
		public string EmailAddressOfCorporateOfficer
		{
			get;
			set;
		}

		[Display(Name="Email Address of Entity")]
		public string EmailAddressOfEntity
		{
			get;
			set;
		}

		[Display(Name="Email Address of Other Corporate Officer")]
		public string EmailAddressOfOtherCorporateOfficer
		{
			get;
			set;
		}

		[Display(Name="Business Fax Number of Entity")]
		public string FaxNumberOfEntity
		{
			get;
			set;
		}

		public FinancialInvestmentParametersModel FinancialInvestmentParameters
		{
			get;
			set;
		}

		[Display(Name="General Notes of Vesting Entity")]
		public string GeneralNotesOfVestingEntity
		{
			get;
			set;
		}

		public GeographicParametersModel GeographicParameters
		{
			get;
			set;
		}

		[Display(Name="Has Entity Raised Intended Purchase Money Capitalization?")]
		public bool? HasEntityRaisedIntendedCap
		{
			get;
			set;
		}

		[Display(Name="Is Corp Entity in Good Standing?")]
		public bool? IsCorporateEntityInGoodStanding
		{
			get;
			set;
		}

		[Display(Name="Last Updated")]
		public DateTime LastUpdated
		{
			get;
			set;
		}

		[Display(Name="If using PMF, define leverage target")]
		public decimal LeverageTarget
		{
			get;
			set;
		}

		[Display(Name="Managing Officer of Entity")]
		public string ManagingOfficerOfEntity
		{
			get;
			set;
		}

		public MultiFamilyHomeDemographicDetailModel MultiFamilyDemographicDetail
		{
			get;
			set;
		}

		[Display(Name="Other Corporate Officer")]
		public string NameOfOtherCorporateOfficer
		{
			get;
			set;
		}

		[Display(Name="Name of Other Corporate Entity Officer")]
		public string NameOfOtherCorporateOfficer2
		{
			get;
			set;
		}

		[Display(Name="Name of Purchasing Entity")]
		public string NameOfPurchasingEntity
		{
			get;
			set;
		}

		[Display(Name="Office Number of Entity")]
		public string OfficeNumberOfEntity
		{
			get;
			set;
		}

		[Display(Name="Other Corporate Office")]
		public string OtherCorporateOfficer
		{
			get;
			set;
		}

		public OtherDemographicDetailModel OtherDemographicDetail
		{
			get;
			set;
		}

		[Display(Name="Has Entity secured CLA/POF?")]
		public bool? SecuredCLAPOF
		{
			get;
			set;
		}

		[Display(Name="State of Incorporation")]
		public string StateOfIncorporation
		{
			get;
			set;
		}

		[Display(Name="State")]
		public string StateOfPurchasingEntity
		{
			get;
			set;
		}

		public List<SelectListItem> States
		{
			get;
			set;
		}

		[Display(Name="If using PMF, define terms sought")]
		public string TermsSought
		{
			get;
			set;
		}

		[Display(Name="If not, timeline on securing CAP:")]
		public string TimelineSecuringCap
		{
			get;
			set;
		}

		[Display(Name="Type of Assets Sought")]
		[Required]
		public AssetType TypeOfAssetsSought
		{
			get;
			set;
		}

		[Display(Name="Type of Purchasing Entity")]
		public CorporateEntityType TypeOfPurchasingEntity
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		[Display(Name="Will entity utilize PM Funding?")]
		public bool? UtilizePMFunding
		{
			get;
			set;
		}

		[Display(Name="Website URL of Vesting Corporate Entity")]
		public string WebsiteURLVestingCorporateEntity
		{
			get;
			set;
		}

		[Display(Name="Zip")]
		public string ZipOfPurchasingEntity
		{
			get;
			set;
		}

		public AssetSearchCriteriaModel()
		{
			this.MultiFamilyDemographicDetail = new MultiFamilyHomeDemographicDetailModel();
			this.OtherDemographicDetail = new OtherDemographicDetailModel();
			this.FinancialInvestmentParameters = new FinancialInvestmentParametersModel();
			this.GeographicParameters = new GeographicParametersModel()
			{
				Interests = new List<GeographicParameterInterestModel>()
			};
			this.GeographicParameters.Interests.Add(new GeographicParameterInterestModel());
			this.GeographicParameters.Interests.Add(new GeographicParameterInterestModel());
			this.GeographicParameters.Interests.Add(new GeographicParameterInterestModel());
			this.DueDiligenceCheckList = new DueDiligenceCheckListModel()
			{
				OtherInspectionItems = new List<DueDiligenceOptionalModel>()
			};
			this.DueDiligenceCheckList.OtherInspectionItems.Add(new DueDiligenceOptionalModel());
			this.DueDiligenceCheckList.OtherInspectionItems.Add(new DueDiligenceOptionalModel());
			this.DueDiligenceCheckList.OtherInspectionItems.Add(new DueDiligenceOptionalModel());
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			SelectListItem selectListItem = new SelectListItem()
			{
				Text = "Corporation",
				Value = CorporateEntityType.Corporation.ToString()
			};
			selectListItems.Add(selectListItem);
			SelectListItem selectListItem1 = new SelectListItem()
			{
				Text = "Limited Liability Corporation",
				Value = CorporateEntityType.LimitedLiabilityCompany.ToString()
			};
			selectListItems.Add(selectListItem1);
			SelectListItem selectListItem2 = new SelectListItem()
			{
				Text = "Limited Liability Partnership",
				Value = CorporateEntityType.LimitedLiabilityPartnership.ToString()
			};
			selectListItems.Add(selectListItem2);
			SelectListItem selectListItem3 = new SelectListItem()
			{
				Text = "Joint Venture",
				Value = CorporateEntityType.JointVenture.ToString()
			};
			selectListItems.Add(selectListItem3);
			SelectListItem selectListItem4 = new SelectListItem()
			{
				Text = "Sole Proprietorship",
				Value = CorporateEntityType.SoleProprietorship.ToString()
			};
			selectListItems.Add(selectListItem4);
			this.CorporateEntityTypes = selectListItems;
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			SelectListItem selectListItem5 = new SelectListItem()
			{
				Text = "AL",
				Value = "AL"
			};
			selectListItems1.Add(selectListItem5);
			SelectListItem selectListItem6 = new SelectListItem()
			{
				Text = "AK",
				Value = "AK"
			};
			selectListItems1.Add(selectListItem6);
			SelectListItem selectListItem7 = new SelectListItem()
			{
				Text = "AZ",
				Value = "AZ"
			};
			selectListItems1.Add(selectListItem7);
			SelectListItem selectListItem8 = new SelectListItem()
			{
				Text = "AR",
				Value = "AR"
			};
			selectListItems1.Add(selectListItem8);
			SelectListItem selectListItem9 = new SelectListItem()
			{
				Text = "CA",
				Value = "CA"
			};
			selectListItems1.Add(selectListItem9);
			SelectListItem selectListItem10 = new SelectListItem()
			{
				Text = "CO",
				Value = "CO"
			};
			selectListItems1.Add(selectListItem10);
			SelectListItem selectListItem11 = new SelectListItem()
			{
				Text = "CT",
				Value = "CT"
			};
			selectListItems1.Add(selectListItem11);
			SelectListItem selectListItem12 = new SelectListItem()
			{
				Text = "DC",
				Value = "DC"
			};
			selectListItems1.Add(selectListItem12);
			SelectListItem selectListItem13 = new SelectListItem()
			{
				Text = "DE",
				Value = "DE"
			};
			selectListItems1.Add(selectListItem13);
			SelectListItem selectListItem14 = new SelectListItem()
			{
				Text = "FL",
				Value = "FL"
			};
			selectListItems1.Add(selectListItem14);
			SelectListItem selectListItem15 = new SelectListItem()
			{
				Text = "GA",
				Value = "GA"
			};
			selectListItems1.Add(selectListItem15);
			SelectListItem selectListItem16 = new SelectListItem()
			{
				Text = "HI",
				Value = "HI"
			};
			selectListItems1.Add(selectListItem16);
			SelectListItem selectListItem17 = new SelectListItem()
			{
				Text = "ID",
				Value = "ID"
			};
			selectListItems1.Add(selectListItem17);
			SelectListItem selectListItem18 = new SelectListItem()
			{
				Text = "IL",
				Value = "IL"
			};
			selectListItems1.Add(selectListItem18);
			SelectListItem selectListItem19 = new SelectListItem()
			{
				Text = "IN",
				Value = "IN"
			};
			selectListItems1.Add(selectListItem19);
			SelectListItem selectListItem20 = new SelectListItem()
			{
				Text = "IA",
				Value = "IA"
			};
			selectListItems1.Add(selectListItem20);
			SelectListItem selectListItem21 = new SelectListItem()
			{
				Text = "KS",
				Value = "KS"
			};
			selectListItems1.Add(selectListItem21);
			SelectListItem selectListItem22 = new SelectListItem()
			{
				Text = "KY",
				Value = "KY"
			};
			selectListItems1.Add(selectListItem22);
			SelectListItem selectListItem23 = new SelectListItem()
			{
				Text = "LA",
				Value = "LA"
			};
			selectListItems1.Add(selectListItem23);
			SelectListItem selectListItem24 = new SelectListItem()
			{
				Text = "ME",
				Value = "ME"
			};
			selectListItems1.Add(selectListItem24);
			SelectListItem selectListItem25 = new SelectListItem()
			{
				Text = "MD",
				Value = "MD"
			};
			selectListItems1.Add(selectListItem25);
			SelectListItem selectListItem26 = new SelectListItem()
			{
				Text = "MA",
				Value = "MA"
			};
			selectListItems1.Add(selectListItem26);
			SelectListItem selectListItem27 = new SelectListItem()
			{
				Text = "MI",
				Value = "MI"
			};
			selectListItems1.Add(selectListItem27);
			SelectListItem selectListItem28 = new SelectListItem()
			{
				Text = "MN",
				Value = "MN"
			};
			selectListItems1.Add(selectListItem28);
			SelectListItem selectListItem29 = new SelectListItem()
			{
				Text = "MS",
				Value = "MS"
			};
			selectListItems1.Add(selectListItem29);
			SelectListItem selectListItem30 = new SelectListItem()
			{
				Text = "MO",
				Value = "MO"
			};
			selectListItems1.Add(selectListItem30);
			SelectListItem selectListItem31 = new SelectListItem()
			{
				Text = "MT",
				Value = "MT"
			};
			selectListItems1.Add(selectListItem31);
			SelectListItem selectListItem32 = new SelectListItem()
			{
				Text = "NE",
				Value = "NE"
			};
			selectListItems1.Add(selectListItem32);
			SelectListItem selectListItem33 = new SelectListItem()
			{
				Text = "NV",
				Value = "NV",
				Selected = true
			};
			selectListItems1.Add(selectListItem33);
			SelectListItem selectListItem34 = new SelectListItem()
			{
				Text = "NH",
				Value = "NH"
			};
			selectListItems1.Add(selectListItem34);
			SelectListItem selectListItem35 = new SelectListItem()
			{
				Text = "NJ",
				Value = "NJ"
			};
			selectListItems1.Add(selectListItem35);
			SelectListItem selectListItem36 = new SelectListItem()
			{
				Text = "NM",
				Value = "NM"
			};
			selectListItems1.Add(selectListItem36);
			SelectListItem selectListItem37 = new SelectListItem()
			{
				Text = "NY",
				Value = "NY"
			};
			selectListItems1.Add(selectListItem37);
			SelectListItem selectListItem38 = new SelectListItem()
			{
				Text = "NC",
				Value = "NC"
			};
			selectListItems1.Add(selectListItem38);
			SelectListItem selectListItem39 = new SelectListItem()
			{
				Text = "ND",
				Value = "ND"
			};
			selectListItems1.Add(selectListItem39);
			SelectListItem selectListItem40 = new SelectListItem()
			{
				Text = "OH",
				Value = "OH"
			};
			selectListItems1.Add(selectListItem40);
			SelectListItem selectListItem41 = new SelectListItem()
			{
				Text = "OK",
				Value = "OK"
			};
			selectListItems1.Add(selectListItem41);
			SelectListItem selectListItem42 = new SelectListItem()
			{
				Text = "OR",
				Value = "OR"
			};
			selectListItems1.Add(selectListItem42);
			SelectListItem selectListItem43 = new SelectListItem()
			{
				Text = "PA",
				Value = "PA"
			};
			selectListItems1.Add(selectListItem43);
			SelectListItem selectListItem44 = new SelectListItem()
			{
				Text = "RI",
				Value = "RI"
			};
			selectListItems1.Add(selectListItem44);
			SelectListItem selectListItem45 = new SelectListItem()
			{
				Text = "SC",
				Value = "SC"
			};
			selectListItems1.Add(selectListItem45);
			SelectListItem selectListItem46 = new SelectListItem()
			{
				Text = "SD",
				Value = "SD"
			};
			selectListItems1.Add(selectListItem46);
			SelectListItem selectListItem47 = new SelectListItem()
			{
				Text = "TN",
				Value = "TN"
			};
			selectListItems1.Add(selectListItem47);
			SelectListItem selectListItem48 = new SelectListItem()
			{
				Text = "TX",
				Value = "TX"
			};
			selectListItems1.Add(selectListItem48);
			SelectListItem selectListItem49 = new SelectListItem()
			{
				Text = "UT",
				Value = "UT"
			};
			selectListItems1.Add(selectListItem49);
			SelectListItem selectListItem50 = new SelectListItem()
			{
				Text = "VT",
				Value = "VT"
			};
			selectListItems1.Add(selectListItem50);
			SelectListItem selectListItem51 = new SelectListItem()
			{
				Text = "VA",
				Value = "VA"
			};
			selectListItems1.Add(selectListItem51);
			SelectListItem selectListItem52 = new SelectListItem()
			{
				Text = "WA",
				Value = "WA"
			};
			selectListItems1.Add(selectListItem52);
			SelectListItem selectListItem53 = new SelectListItem()
			{
				Text = "WV",
				Value = "WV"
			};
			selectListItems1.Add(selectListItem53);
			SelectListItem selectListItem54 = new SelectListItem()
			{
				Text = "WI",
				Value = "WI"
			};
			selectListItems1.Add(selectListItem54);
			SelectListItem selectListItem55 = new SelectListItem()
			{
				Text = "WY",
				Value = "WY"
			};
			selectListItems1.Add(selectListItem55);
			this.States = selectListItems1;
		}
	}
}