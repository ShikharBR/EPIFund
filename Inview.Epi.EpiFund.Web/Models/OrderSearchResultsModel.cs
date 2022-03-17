using Inview.Epi.EpiFund.Domain.Helpers;
using Inview.Epi.EpiFund.Domain.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Web.Models
{
	public class OrderSearchResultsModel
	{
		[Display(Name="Address Line 1")]
		public string AddressLine1
		{
			get;
			set;
		}

		[Display(Name="APN")]
		public string APN
		{
			get;
			set;
		}

		[Display(Name="County")]
		public string AssetCounty
		{
			get;
			set;
		}

		[Display(Name="Asset Name")]
		public string AssetName
		{
			get;
			set;
		}

		[Display(Name="Asset #")]
		public int? AssetNumber
		{
			get;
			set;
		}

		public List<SelectListItem> AssetTypes
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

		public PagedList.IPagedList<OrderModel> Completed
		{
			get;
			set;
		}

		public int? CompletedPage
		{
			get;
			set;
		}

		[Display(Name="Date Ordered End Date")]
		public DateTime? DateOrderedEndDate
		{
			get;
			set;
		}

		[Display(Name="Date Ordered Start Date")]
		public DateTime? DateOrderedStartDate
		{
			get;
			set;
		}

		[Display(Name="Date Paid End Date")]
		public DateTime? DatePaidEndDate
		{
			get;
			set;
		}

		[Display(Name="Date Paid Start Date")]
		public DateTime? DatePaidStartDate
		{
			get;
			set;
		}

		[Display(Name="Date Submitted End Date")]
		public DateTime? DateSubmittedEndDate
		{
			get;
			set;
		}

		[Display(Name="Date Submitted Start Date")]
		public DateTime? DateSubmittedStartDate
		{
			get;
			set;
		}

		public int InsuranceCompanyId
		{
			get;
			set;
		}

		public PagedList.IPagedList<OrderModel> Orders
		{
			get;
			set;
		}

		public int? Page
		{
			get;
			set;
		}

		public PagedList.IPagedList<OrderModel> Pending
		{
			get;
			set;
		}

		public int? PendingPage
		{
			get;
			set;
		}

		public int? RowCount
		{
			get;
			set;
		}

		[Display(Name="Asset Type")]
		public AssetType? SelectedAssetType
		{
			get;
			set;
		}

		public string SortOrder
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

		public int TitleCompanyId
		{
			get;
			set;
		}

		public OrderSearchResultsModel()
		{
			this.AssetTypes = new List<SelectListItem>()
			{
				new SelectListItem()
				{
					Value = null,
					Text = "All",
					Selected = true
				},
				new SelectListItem()
				{
					Value = AssetType.MultiFamily.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.MultiFamily)
				},
				new SelectListItem()
				{
					Value = AssetType.Office.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.Office)
				},
				new SelectListItem()
				{
					Value = AssetType.Retail.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.Retail)
				},
				new SelectListItem()
				{
					Value = AssetType.Industrial.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.Industrial)
				},
				new SelectListItem()
				{
					Value = AssetType.MHP.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.MHP)
				},
				new SelectListItem()
				{
					Value = AssetType.Medical.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.Medical)
				},
				new SelectListItem()
				{
					Value = AssetType.MixedUse.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.MixedUse)
				}
			};
			this.States = new List<SelectListItem>()
			{
				new SelectListItem()
				{
					Text = "All",
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