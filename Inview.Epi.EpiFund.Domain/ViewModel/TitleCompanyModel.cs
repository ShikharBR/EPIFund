using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class TitleCompanyModel
	{
		[Display(Name="Title Company City")]
		public string City
		{
			get;
			set;
		}

		[Display(Name="Created On")]
		public DateTime CreatedOn
		{
			get;
			set;
		}

		[Display(Name="Current Rate")]
		[Required]
		public double? CurrentRate
		{
			get;
			set;
		}

		public List<SelectListItem> IncludedStates
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

		[Display(Name="Included States")]
		[Required]
		public List<StatesOfUS> SelectedIncludedStates
		{
			get;
			set;
		}

		[Display(Name="Title Company State")]
		public string State
		{
			get;
			set;
		}

		[Display(Name="Title Company Address")]
		public string TitleCompAddress
		{
			get;
			set;
		}

		[Display(Name="Title Company Address 2")]
		public string TitleCompAddress2
		{
			get;
			set;
		}

		public int TitleCompanyId
		{
			get;
			set;
		}

		[Display(Name="Title Company Name")]
		[Required]
		public string TitleCompName
		{
			get;
			set;
		}

		[Display(Name="Title Company Phone Number")]
		public string TitleCompPhone
		{
			get;
			set;
		}

		[Display(Name="Website")]
		public string TitleCompURL
		{
			get;
			set;
		}

		[Display(Name="Title Company Zip")]
		public string Zip
		{
			get;
			set;
		}

		public TitleCompanyModel()
		{
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			SelectListItem selectListItem = new SelectListItem()
			{
				Text = "All",
				Value = "ALL"
			};
			selectListItems.Add(selectListItem);
			SelectListItem selectListItem1 = new SelectListItem()
			{
				Text = "Alabama",
				Value = "AL"
			};
			selectListItems.Add(selectListItem1);
			SelectListItem selectListItem2 = new SelectListItem()
			{
				Text = "Alaska",
				Value = "AK"
			};
			selectListItems.Add(selectListItem2);
			SelectListItem selectListItem3 = new SelectListItem()
			{
				Text = "Arizona",
				Value = "AZ"
			};
			selectListItems.Add(selectListItem3);
			SelectListItem selectListItem4 = new SelectListItem()
			{
				Text = "Arkansas",
				Value = "AR"
			};
			selectListItems.Add(selectListItem4);
			SelectListItem selectListItem5 = new SelectListItem()
			{
				Text = "California",
				Value = "CA"
			};
			selectListItems.Add(selectListItem5);
			SelectListItem selectListItem6 = new SelectListItem()
			{
				Text = "Colorado",
				Value = "CO"
			};
			selectListItems.Add(selectListItem6);
			SelectListItem selectListItem7 = new SelectListItem()
			{
				Text = "Connecticut",
				Value = "CT"
			};
			selectListItems.Add(selectListItem7);
			SelectListItem selectListItem8 = new SelectListItem()
			{
				Text = "District of Columbia",
				Value = "DC"
			};
			selectListItems.Add(selectListItem8);
			SelectListItem selectListItem9 = new SelectListItem()
			{
				Text = "Delaware",
				Value = "DE"
			};
			selectListItems.Add(selectListItem9);
			SelectListItem selectListItem10 = new SelectListItem()
			{
				Text = "Florida",
				Value = "FL"
			};
			selectListItems.Add(selectListItem10);
			SelectListItem selectListItem11 = new SelectListItem()
			{
				Text = "Georgia",
				Value = "GA"
			};
			selectListItems.Add(selectListItem11);
			SelectListItem selectListItem12 = new SelectListItem()
			{
				Text = "Hawaii",
				Value = "HI"
			};
			selectListItems.Add(selectListItem12);
			SelectListItem selectListItem13 = new SelectListItem()
			{
				Text = "Idaho",
				Value = "ID"
			};
			selectListItems.Add(selectListItem13);
			SelectListItem selectListItem14 = new SelectListItem()
			{
				Text = "Illinois",
				Value = "IL"
			};
			selectListItems.Add(selectListItem14);
			SelectListItem selectListItem15 = new SelectListItem()
			{
				Text = "Indiana",
				Value = "IN"
			};
			selectListItems.Add(selectListItem15);
			SelectListItem selectListItem16 = new SelectListItem()
			{
				Text = "Iowa",
				Value = "IA"
			};
			selectListItems.Add(selectListItem16);
			SelectListItem selectListItem17 = new SelectListItem()
			{
				Text = "Kansas",
				Value = "KS"
			};
			selectListItems.Add(selectListItem17);
			SelectListItem selectListItem18 = new SelectListItem()
			{
				Text = "Kentucky",
				Value = "KY"
			};
			selectListItems.Add(selectListItem18);
			SelectListItem selectListItem19 = new SelectListItem()
			{
				Text = "Louisiana",
				Value = "LA"
			};
			selectListItems.Add(selectListItem19);
			SelectListItem selectListItem20 = new SelectListItem()
			{
				Text = "Maine",
				Value = "ME"
			};
			selectListItems.Add(selectListItem20);
			SelectListItem selectListItem21 = new SelectListItem()
			{
				Text = "Maryland",
				Value = "MD"
			};
			selectListItems.Add(selectListItem21);
			SelectListItem selectListItem22 = new SelectListItem()
			{
				Text = "Massachusetts",
				Value = "MA"
			};
			selectListItems.Add(selectListItem22);
			SelectListItem selectListItem23 = new SelectListItem()
			{
				Text = "Michigan",
				Value = "MI"
			};
			selectListItems.Add(selectListItem23);
			SelectListItem selectListItem24 = new SelectListItem()
			{
				Text = "Minnesota",
				Value = "MN"
			};
			selectListItems.Add(selectListItem24);
			SelectListItem selectListItem25 = new SelectListItem()
			{
				Text = "Mississippi",
				Value = "MS"
			};
			selectListItems.Add(selectListItem25);
			SelectListItem selectListItem26 = new SelectListItem()
			{
				Text = "Missouri",
				Value = "MO"
			};
			selectListItems.Add(selectListItem26);
			SelectListItem selectListItem27 = new SelectListItem()
			{
				Text = "Montana",
				Value = "MT"
			};
			selectListItems.Add(selectListItem27);
			SelectListItem selectListItem28 = new SelectListItem()
			{
				Text = "Nebraska",
				Value = "NE"
			};
			selectListItems.Add(selectListItem28);
			SelectListItem selectListItem29 = new SelectListItem()
			{
				Text = "Nevada",
				Value = "NV"
			};
			selectListItems.Add(selectListItem29);
			SelectListItem selectListItem30 = new SelectListItem()
			{
				Text = "New Hampshire",
				Value = "NH"
			};
			selectListItems.Add(selectListItem30);
			SelectListItem selectListItem31 = new SelectListItem()
			{
				Text = "New Jersey",
				Value = "NJ"
			};
			selectListItems.Add(selectListItem31);
			SelectListItem selectListItem32 = new SelectListItem()
			{
				Text = "New Mexico",
				Value = "NM"
			};
			selectListItems.Add(selectListItem32);
			SelectListItem selectListItem33 = new SelectListItem()
			{
				Text = "New York",
				Value = "NY"
			};
			selectListItems.Add(selectListItem33);
			SelectListItem selectListItem34 = new SelectListItem()
			{
				Text = "North Carolina",
				Value = "NC"
			};
			selectListItems.Add(selectListItem34);
			SelectListItem selectListItem35 = new SelectListItem()
			{
				Text = "North Dakota",
				Value = "ND"
			};
			selectListItems.Add(selectListItem35);
			SelectListItem selectListItem36 = new SelectListItem()
			{
				Text = "Ohio",
				Value = "OH"
			};
			selectListItems.Add(selectListItem36);
			SelectListItem selectListItem37 = new SelectListItem()
			{
				Text = "Oklahoma",
				Value = "OK"
			};
			selectListItems.Add(selectListItem37);
			SelectListItem selectListItem38 = new SelectListItem()
			{
				Text = "Oregon",
				Value = "OR"
			};
			selectListItems.Add(selectListItem38);
			SelectListItem selectListItem39 = new SelectListItem()
			{
				Text = "Pennsylvania",
				Value = "PA"
			};
			selectListItems.Add(selectListItem39);
			SelectListItem selectListItem40 = new SelectListItem()
			{
				Text = "Rhode Island",
				Value = "RI"
			};
			selectListItems.Add(selectListItem40);
			SelectListItem selectListItem41 = new SelectListItem()
			{
				Text = "South Carolina",
				Value = "SC"
			};
			selectListItems.Add(selectListItem41);
			SelectListItem selectListItem42 = new SelectListItem()
			{
				Text = "South Dakota",
				Value = "SD"
			};
			selectListItems.Add(selectListItem42);
			SelectListItem selectListItem43 = new SelectListItem()
			{
				Text = "Tennessee",
				Value = "TN"
			};
			selectListItems.Add(selectListItem43);
			SelectListItem selectListItem44 = new SelectListItem()
			{
				Text = "Texas",
				Value = "TX"
			};
			selectListItems.Add(selectListItem44);
			SelectListItem selectListItem45 = new SelectListItem()
			{
				Text = "Utah",
				Value = "UT"
			};
			selectListItems.Add(selectListItem45);
			SelectListItem selectListItem46 = new SelectListItem()
			{
				Text = "Vermont",
				Value = "VT"
			};
			selectListItems.Add(selectListItem46);
			SelectListItem selectListItem47 = new SelectListItem()
			{
				Text = "Virginia",
				Value = "VA"
			};
			selectListItems.Add(selectListItem47);
			SelectListItem selectListItem48 = new SelectListItem()
			{
				Text = "Washington",
				Value = "WA"
			};
			selectListItems.Add(selectListItem48);
			SelectListItem selectListItem49 = new SelectListItem()
			{
				Text = "West Virginia",
				Value = "WV"
			};
			selectListItems.Add(selectListItem49);
			SelectListItem selectListItem50 = new SelectListItem()
			{
				Text = "Wisconsin",
				Value = "WI"
			};
			selectListItems.Add(selectListItem50);
			SelectListItem selectListItem51 = new SelectListItem()
			{
				Text = "Wyoming",
				Value = "WY"
			};
			selectListItems.Add(selectListItem51);
			this.IncludedStates = selectListItems;
		}
	}
}