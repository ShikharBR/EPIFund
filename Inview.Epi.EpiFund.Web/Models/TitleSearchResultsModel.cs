using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Web.Models
{
	public class TitleSearchResultsModel
	{
		public UserType ControllingUserType
		{
			get;
			set;
		}

		[Display(Name="State")]
		public string State
		{
			get;
			set;
		}

		public List<SelectListItem> States
		{
			get;
			set;
		}

		[Display(Name="Title Company Name")]
		public string TitleCompName
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

		public PagedList.IPagedList<TitleQuickViewModel> Titles
		{
			get;
			set;
		}

		public TitleSearchResultsModel()
		{
			this.States = new List<SelectListItem>()
			{
				new SelectListItem()
				{
					Text = "--",
					Value = "",
					Selected = true
				},
				new SelectListItem()
				{
					Text = "AL",
					Value = "AL"
				},
				new SelectListItem()
				{
					Text = "AK",
					Value = "AK"
				},
				new SelectListItem()
				{
					Text = "AZ",
					Value = "AZ"
				},
				new SelectListItem()
				{
					Text = "AR",
					Value = "AR"
				},
				new SelectListItem()
				{
					Text = "CA",
					Value = "CA"
				},
				new SelectListItem()
				{
					Text = "CO",
					Value = "CO"
				},
				new SelectListItem()
				{
					Text = "CT",
					Value = "CT"
				},
				new SelectListItem()
				{
					Text = "DC",
					Value = "DC"
				},
				new SelectListItem()
				{
					Text = "DE",
					Value = "DE"
				},
				new SelectListItem()
				{
					Text = "FL",
					Value = "FL"
				},
				new SelectListItem()
				{
					Text = "GA",
					Value = "GA"
				},
				new SelectListItem()
				{
					Text = "HI",
					Value = "HI"
				},
				new SelectListItem()
				{
					Text = "ID",
					Value = "ID"
				},
				new SelectListItem()
				{
					Text = "IL",
					Value = "IL"
				},
				new SelectListItem()
				{
					Text = "IN",
					Value = "IN"
				},
				new SelectListItem()
				{
					Text = "IA",
					Value = "IA"
				},
				new SelectListItem()
				{
					Text = "KS",
					Value = "KS"
				},
				new SelectListItem()
				{
					Text = "KY",
					Value = "KY"
				},
				new SelectListItem()
				{
					Text = "LA",
					Value = "LA"
				},
				new SelectListItem()
				{
					Text = "ME",
					Value = "ME"
				},
				new SelectListItem()
				{
					Text = "MD",
					Value = "MD"
				},
				new SelectListItem()
				{
					Text = "MA",
					Value = "MA"
				},
				new SelectListItem()
				{
					Text = "MI",
					Value = "MI"
				},
				new SelectListItem()
				{
					Text = "MN",
					Value = "MN"
				},
				new SelectListItem()
				{
					Text = "MS",
					Value = "MS"
				},
				new SelectListItem()
				{
					Text = "MO",
					Value = "MO"
				},
				new SelectListItem()
				{
					Text = "MT",
					Value = "MT"
				},
				new SelectListItem()
				{
					Text = "NE",
					Value = "NE"
				},
				new SelectListItem()
				{
					Text = "NV",
					Value = "NV"
				},
				new SelectListItem()
				{
					Text = "NH",
					Value = "NH"
				},
				new SelectListItem()
				{
					Text = "NJ",
					Value = "NJ"
				},
				new SelectListItem()
				{
					Text = "NM",
					Value = "NM"
				},
				new SelectListItem()
				{
					Text = "NY",
					Value = "NY"
				},
				new SelectListItem()
				{
					Text = "NC",
					Value = "NC"
				},
				new SelectListItem()
				{
					Text = "ND",
					Value = "ND"
				},
				new SelectListItem()
				{
					Text = "OH",
					Value = "OH"
				},
				new SelectListItem()
				{
					Text = "OK",
					Value = "OK"
				},
				new SelectListItem()
				{
					Text = "OR",
					Value = "OR"
				},
				new SelectListItem()
				{
					Text = "PA",
					Value = "PA"
				},
				new SelectListItem()
				{
					Text = "RI",
					Value = "RI"
				},
				new SelectListItem()
				{
					Text = "SC",
					Value = "SC"
				},
				new SelectListItem()
				{
					Text = "SD",
					Value = "SD"
				},
				new SelectListItem()
				{
					Text = "TN",
					Value = "TN"
				},
				new SelectListItem()
				{
					Text = "TX",
					Value = "TX"
				},
				new SelectListItem()
				{
					Text = "UT",
					Value = "UT"
				},
				new SelectListItem()
				{
					Text = "VT",
					Value = "VT"
				},
				new SelectListItem()
				{
					Text = "VA",
					Value = "VA"
				},
				new SelectListItem()
				{
					Text = "WA",
					Value = "WA"
				},
				new SelectListItem()
				{
					Text = "WV",
					Value = "WV"
				},
				new SelectListItem()
				{
					Text = "WI",
					Value = "WI"
				},
				new SelectListItem()
				{
					Text = "WY",
					Value = "WY"
				}
			};
		}
	}
}