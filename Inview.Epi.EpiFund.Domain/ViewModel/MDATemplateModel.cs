using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class MDATemplateModel
	{
		[Display(Name="Assets")]
		public List<AssetDescriptionModel> Assets
		{
			get;
			set;
		}

		[Display(Name="City")]
		[Required]
		public string City
		{
			get;
			set;
		}

		[Display(Name="Name of Corporate Entity")]
		[Required]
		public string CompanyName
		{
			get;
			set;
		}

		[Display(Name="Title")]
		[Required]
		public string CorpTitle
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

		[Display(Name="Email")]
		[Required]
		public string Email
		{
			get;
			set;
		}

		[Display(Name="Fax")]
		[Required]
		public string Fax
		{
			get;
			set;
		}

		[Display(Name="Acronym of Corporate Entity")]
		[Required]
		public string FirstNameOfCorporateEntity
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

		[Display(Name="Phone")]
		[Required]
		public string Phone
		{
			get;
			set;
		}

		[Display(Name="State")]
		[Required]
		public string State
		{
			get;
			set;
		}

		[Display(Name="State of Origin of the Corporate Entity")]
		[Required]
		public string StateOfOriginOfCorporateEntity
		{
			get;
			set;
		}

		public List<SelectListItem> States
		{
			get;
			set;
		}

		[Display(Name="Type of Corporate Entity")]
		[Required]
		public CorporateEntityType TypeOfCorporateEntity
		{
			get;
			set;
		}

		public List<SelectListItem> TypesOfCorporateEntity
		{
			get;
			set;
		}

		[Display(Name="Address Line 1")]
		[Required]
		public string UserAddressLine1
		{
			get;
			set;
		}

		[Display(Name="Address Line 2")]
		public string UserAddressLine2
		{
			get;
			set;
		}

		[Display(Name="First Name of Registrant")]
		[Required]
		public string UserFirstName
		{
			get;
			set;
		}

		[Display(Name="Last Name of Registrant")]
		[Required]
		public string UserLastName
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

		public MDATemplateModel()
		{
			this.Assets = new List<AssetDescriptionModel>();
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
			this.TypesOfCorporateEntity = selectListItems;
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