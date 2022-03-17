using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class GeographicParameterInterestModel
	{
		[Display(Name="Cities of Interest")]
		public List<string> Cities
		{
			get;
			set;
		}

		[Display(Name="Name of State")]
		[Required]
		public string StateOfInterest
		{
			get;
			set;
		}

		public List<SelectListItem> States
		{
			get;
			set;
		}

		public GeographicParameterInterestModel()
		{
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			SelectListItem selectListItem = new SelectListItem()
			{
				Text = "AL",
				Value = "AL"
			};
			selectListItems.Add(selectListItem);
			SelectListItem selectListItem1 = new SelectListItem()
			{
				Text = "AK",
				Value = "AK"
			};
			selectListItems.Add(selectListItem1);
			SelectListItem selectListItem2 = new SelectListItem()
			{
				Text = "AZ",
				Value = "AZ"
			};
			selectListItems.Add(selectListItem2);
			SelectListItem selectListItem3 = new SelectListItem()
			{
				Text = "AR",
				Value = "AR"
			};
			selectListItems.Add(selectListItem3);
			SelectListItem selectListItem4 = new SelectListItem()
			{
				Text = "CA",
				Value = "CA"
			};
			selectListItems.Add(selectListItem4);
			SelectListItem selectListItem5 = new SelectListItem()
			{
				Text = "CO",
				Value = "CO"
			};
			selectListItems.Add(selectListItem5);
			SelectListItem selectListItem6 = new SelectListItem()
			{
				Text = "CT",
				Value = "CT"
			};
			selectListItems.Add(selectListItem6);
			SelectListItem selectListItem7 = new SelectListItem()
			{
				Text = "DC",
				Value = "DC"
			};
			selectListItems.Add(selectListItem7);
			SelectListItem selectListItem8 = new SelectListItem()
			{
				Text = "DE",
				Value = "DE"
			};
			selectListItems.Add(selectListItem8);
			SelectListItem selectListItem9 = new SelectListItem()
			{
				Text = "FL",
				Value = "FL"
			};
			selectListItems.Add(selectListItem9);
			SelectListItem selectListItem10 = new SelectListItem()
			{
				Text = "GA",
				Value = "GA"
			};
			selectListItems.Add(selectListItem10);
			SelectListItem selectListItem11 = new SelectListItem()
			{
				Text = "HI",
				Value = "HI"
			};
			selectListItems.Add(selectListItem11);
			SelectListItem selectListItem12 = new SelectListItem()
			{
				Text = "ID",
				Value = "ID"
			};
			selectListItems.Add(selectListItem12);
			SelectListItem selectListItem13 = new SelectListItem()
			{
				Text = "IL",
				Value = "IL"
			};
			selectListItems.Add(selectListItem13);
			SelectListItem selectListItem14 = new SelectListItem()
			{
				Text = "IN",
				Value = "IN"
			};
			selectListItems.Add(selectListItem14);
			SelectListItem selectListItem15 = new SelectListItem()
			{
				Text = "IA",
				Value = "IA"
			};
			selectListItems.Add(selectListItem15);
			SelectListItem selectListItem16 = new SelectListItem()
			{
				Text = "KS",
				Value = "KS"
			};
			selectListItems.Add(selectListItem16);
			SelectListItem selectListItem17 = new SelectListItem()
			{
				Text = "KY",
				Value = "KY"
			};
			selectListItems.Add(selectListItem17);
			SelectListItem selectListItem18 = new SelectListItem()
			{
				Text = "LA",
				Value = "LA"
			};
			selectListItems.Add(selectListItem18);
			SelectListItem selectListItem19 = new SelectListItem()
			{
				Text = "ME",
				Value = "ME"
			};
			selectListItems.Add(selectListItem19);
			SelectListItem selectListItem20 = new SelectListItem()
			{
				Text = "MD",
				Value = "MD"
			};
			selectListItems.Add(selectListItem20);
			SelectListItem selectListItem21 = new SelectListItem()
			{
				Text = "MA",
				Value = "MA"
			};
			selectListItems.Add(selectListItem21);
			SelectListItem selectListItem22 = new SelectListItem()
			{
				Text = "MI",
				Value = "MI"
			};
			selectListItems.Add(selectListItem22);
			SelectListItem selectListItem23 = new SelectListItem()
			{
				Text = "MN",
				Value = "MN"
			};
			selectListItems.Add(selectListItem23);
			SelectListItem selectListItem24 = new SelectListItem()
			{
				Text = "MS",
				Value = "MS"
			};
			selectListItems.Add(selectListItem24);
			SelectListItem selectListItem25 = new SelectListItem()
			{
				Text = "MO",
				Value = "MO"
			};
			selectListItems.Add(selectListItem25);
			SelectListItem selectListItem26 = new SelectListItem()
			{
				Text = "MT",
				Value = "MT"
			};
			selectListItems.Add(selectListItem26);
			SelectListItem selectListItem27 = new SelectListItem()
			{
				Text = "NE",
				Value = "NE"
			};
			selectListItems.Add(selectListItem27);
			SelectListItem selectListItem28 = new SelectListItem()
			{
				Text = "NV",
				Value = "NV",
				Selected = true
			};
			selectListItems.Add(selectListItem28);
			SelectListItem selectListItem29 = new SelectListItem()
			{
				Text = "NH",
				Value = "NH"
			};
			selectListItems.Add(selectListItem29);
			SelectListItem selectListItem30 = new SelectListItem()
			{
				Text = "NJ",
				Value = "NJ"
			};
			selectListItems.Add(selectListItem30);
			SelectListItem selectListItem31 = new SelectListItem()
			{
				Text = "NM",
				Value = "NM"
			};
			selectListItems.Add(selectListItem31);
			SelectListItem selectListItem32 = new SelectListItem()
			{
				Text = "NY",
				Value = "NY"
			};
			selectListItems.Add(selectListItem32);
			SelectListItem selectListItem33 = new SelectListItem()
			{
				Text = "NC",
				Value = "NC"
			};
			selectListItems.Add(selectListItem33);
			SelectListItem selectListItem34 = new SelectListItem()
			{
				Text = "ND",
				Value = "ND"
			};
			selectListItems.Add(selectListItem34);
			SelectListItem selectListItem35 = new SelectListItem()
			{
				Text = "OH",
				Value = "OH"
			};
			selectListItems.Add(selectListItem35);
			SelectListItem selectListItem36 = new SelectListItem()
			{
				Text = "OK",
				Value = "OK"
			};
			selectListItems.Add(selectListItem36);
			SelectListItem selectListItem37 = new SelectListItem()
			{
				Text = "OR",
				Value = "OR"
			};
			selectListItems.Add(selectListItem37);
			SelectListItem selectListItem38 = new SelectListItem()
			{
				Text = "PA",
				Value = "PA"
			};
			selectListItems.Add(selectListItem38);
			SelectListItem selectListItem39 = new SelectListItem()
			{
				Text = "RI",
				Value = "RI"
			};
			selectListItems.Add(selectListItem39);
			SelectListItem selectListItem40 = new SelectListItem()
			{
				Text = "SC",
				Value = "SC"
			};
			selectListItems.Add(selectListItem40);
			SelectListItem selectListItem41 = new SelectListItem()
			{
				Text = "SD",
				Value = "SD"
			};
			selectListItems.Add(selectListItem41);
			SelectListItem selectListItem42 = new SelectListItem()
			{
				Text = "TN",
				Value = "TN"
			};
			selectListItems.Add(selectListItem42);
			SelectListItem selectListItem43 = new SelectListItem()
			{
				Text = "TX",
				Value = "TX"
			};
			selectListItems.Add(selectListItem43);
			SelectListItem selectListItem44 = new SelectListItem()
			{
				Text = "UT",
				Value = "UT"
			};
			selectListItems.Add(selectListItem44);
			SelectListItem selectListItem45 = new SelectListItem()
			{
				Text = "VT",
				Value = "VT"
			};
			selectListItems.Add(selectListItem45);
			SelectListItem selectListItem46 = new SelectListItem()
			{
				Text = "VA",
				Value = "VA"
			};
			selectListItems.Add(selectListItem46);
			SelectListItem selectListItem47 = new SelectListItem()
			{
				Text = "WA",
				Value = "WA"
			};
			selectListItems.Add(selectListItem47);
			SelectListItem selectListItem48 = new SelectListItem()
			{
				Text = "WV",
				Value = "WV"
			};
			selectListItems.Add(selectListItem48);
			SelectListItem selectListItem49 = new SelectListItem()
			{
				Text = "WI",
				Value = "WI"
			};
			selectListItems.Add(selectListItem49);
			SelectListItem selectListItem50 = new SelectListItem()
			{
				Text = "WY",
				Value = "WY"
			};
			selectListItems.Add(selectListItem50);
			this.States = selectListItems;
		}
	}
}