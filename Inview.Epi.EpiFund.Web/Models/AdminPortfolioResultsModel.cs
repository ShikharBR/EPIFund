using Inview.Epi.EpiFund.Domain.Enum;
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
	public class AdminPortfolioResultsModel
	{
		[Display(Name="Address Line 1")]
		public string AddressLine1
		{
			get;
			set;
		}

		[Display(Name="APN Number")]
		public string APN
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
		public string AssetNumber
		{
			get;
			set;
		}

		public List<SelectListItem> AssetTypes
		{
			get;
			set;
		}

		[Display(Name="If Yes what is the Auction/Call for Offer Date")]
		public DateTime? CallforOfferDate
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

		public UserType? ControllingUserType
		{
			get;
			set;
		}

		[Display(Name="Does Portfolio have a CALL FOR OFFERS DATE?")]
		public bool hasOffersDate
		{
			get;
			set;
		}

		[Display(Name="Is Portfolio Subject to Auction?")]
		public bool isSubjectToAuction
		{
			get;
			set;
		}

		[Display(Name="Last Reported Occupancy")]
		public DateTime? LastReportedOccupancyDate
		{
			get;
			set;
		}

		[Display(Name="Number of Assets")]
		public int NumberofAssets
		{
			get;
			set;
		}

		public int? Page
		{
			get;
			set;
		}

		public Guid PortfolioId
		{
			get;
			set;
		}

		[Display(Name="Portfolio Name")]
		[Required(ErrorMessage="Portfolio Name is Required")]
		public string PortfolioName
		{
			get;
			set;
		}

		public PagedList.IPagedList<PortfolioQuickListModel> Portfolios
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

		public UserModel SelectedUser
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

		[Display(Name="Zip")]
		public string ZipCode
		{
			get;
			set;
		}

        public bool IsSearching
        {
            get;
            set;
        }

		public AdminPortfolioResultsModel()
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
					Value = AssetType.MultiFamily.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.MultiFamily)
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
				},
				new SelectListItem()
				{
					Value = AssetType.Hotel.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.Hotel)
				},
				new SelectListItem()
				{
					Value = AssetType.SingleTenantProperty.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.SingleTenantProperty)
				},
				new SelectListItem()
				{
					Value = AssetType.FracturedCondominiumPortfolio.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.FracturedCondominiumPortfolio)
				},
				new SelectListItem()
				{
					Value = AssetType.MiniStorageProperty.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.MiniStorageProperty)
				},
				new SelectListItem()
				{
					Value = AssetType.ParkingGarageProperty.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.ParkingGarageProperty)
				},
				new SelectListItem()
				{
					Value = AssetType.ConvenienceStoreFuel.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.ConvenienceStoreFuel)
				}
			};
			this.States = new List<SelectListItem>()
			{
				new SelectListItem()
				{
					Text = "",
					Value = ""
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