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
	public class AssetSearchResultsModel
	{
		[Display(Name="Cumulative Listed Price ")]
		public double? AccListPrice
		{
			get;
			set;
		}

		[Display(Name="Cumulative Units")]
		public double? AccUnits
		{
			get;
			set;
		}

		public List<SelectListItem> AssetCategories
		{
			get;
			set;
		}

		[Display(Name="Location of Asset By ID")]
		public List<string> AssetIds
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

		public PagedList.IPagedList<AssetQuickListViewModel> Assets
		{
			get;
			set;
		}

		public List<SelectListItem> AssetTypes
		{
			get;
			set;
		}

		public UserType? CallingUserType
		{
			get;
			set;
		}

		[Display(Name="Location of Asset By City")]
		public string City
		{
			get;
			set;
		}

		public List<SelectListItem> GradeClassifications
		{
			get;
			set;
		}

		public List<SelectListItem> ListingStatuses
		{
			get;
			set;
		}

		[Display(Name="Max Age Range")]
		public string MaxAgeRange
		{
			get;
			set;
		}

		public List<SelectListItem> MaxAges
		{
			get;
			set;
		}

		[Display(Name="Max Price Range")]
		public double? MaxPriceRange
		{
			get;
			set;
		}

		[Display(Name="Max Square Feet")]
		public int? MaxSquareFeet
		{
			get;
			set;
		}

		[Display(Name="Max Units/Spaces")]
		public int? MaxUnitsSpaces
		{
			get;
			set;
		}

		[Display(Name="Min Grade Classification of Asset")]
		public string MinGrade
		{
			get;
			set;
		}

		[Display(Name="Min Price Range")]
		public double? MinPriceRange
		{
			get;
			set;
		}

		[Display(Name="Min Square Feet")]
		public int? MinSquareFeet
		{
			get;
			set;
		}

		[Display(Name="Min Units/Spaces")]
		public int? MinUnitsSpaces
		{
			get;
			set;
		}

		public int? Page
		{
			get;
			set;
		}

		public PagedList.IPagedList<PortfolioQuickListViewModel> Portfolios
		{
			get;
			set;
		}

		public int? RowCount
		{
			get;
			set;
		}

		[Display(Name="Asset Category")]
		public AssetCategory? SelectedAssetCategory
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

		public string SelectedAssetTypeName
		{
			get;
			set;
		}

		[Display(Name="Listing Status")]
		public ListingStatus? SelectedListingStatus
		{
			get;
			set;
		}

		[Display(Name="Location of Asset By State")]
		public List<StateSearchCriteriaModel> SelectedStates
		{
			get;
			set;
		}

		public bool ShowSearchFormLink
		{
			get;
			set;
		}

		[Display(Name="Location of Asset By State")]
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

		[Display(Name="Is Property required to be \"Turn Key\"?")]
		public string TurnKey
		{
			get;
			set;
		}

		public List<SelectListItem> TurnKeyOptions
		{
			get;
			set;
		}

        [Display(Name = "Is Paper")]
        public bool IsPaper
        {
            get;
            set;
        }

        public AssetSearchResultsModel()
		{
			this.AssetIds = new List<string>()
			{
				""
			};
			this.SelectedStates = new List<StateSearchCriteriaModel>()
			{
				new StateSearchCriteriaModel()
				{
					State = ""
				}
			};
			this.ShowSearchFormLink = false;
			this.TurnKeyOptions = new List<SelectListItem>()
			{
				new SelectListItem()
				{
					Value = "true",
					Text = "Yes"
				},
				new SelectListItem()
				{
					Value = "false",
					Text = "No",
					Selected = true
				}
			};
			this.MaxAges = new List<SelectListItem>()
			{
				new SelectListItem()
				{
					Value = "5",
					Text = "<= 5 years"
				},
				new SelectListItem()
				{
					Value = "10",
					Text = "<= 10 years"
				},
				new SelectListItem()
				{
					Value = "15",
					Text = "<= 15 years"
				},
				new SelectListItem()
				{
					Value = "20",
					Text = "<= 20"
				},
				new SelectListItem()
				{
					Value = "30",
					Text = "<= 30"
				},
				new SelectListItem()
				{
					Value = "0",
					Text = "N/A",
					Selected = true
				}
			};
			this.GradeClassifications = new List<SelectListItem>()
			{
				new SelectListItem()
				{
					Value = "A+",
					Text = "A+"
				},
				new SelectListItem()
				{
					Value = "A",
					Text = "A"
				},
				new SelectListItem()
				{
					Value = "A-",
					Text = "A-"
				},
				new SelectListItem()
				{
					Value = "B+",
					Text = "B+"
				},
				new SelectListItem()
				{
					Value = "B",
					Text = "B"
				},
				new SelectListItem()
				{
					Value = "B-",
					Text = "B-"
				},
				new SelectListItem()
				{
					Value = "C+",
					Text = "C+"
				},
				new SelectListItem()
				{
					Value = "C",
					Text = "C"
				},
				new SelectListItem()
				{
					Value = "C-",
					Text = "C-"
				},
				new SelectListItem()
				{
					Value = "D+",
					Text = "D+"
				},
				new SelectListItem()
				{
					Value = "D",
					Text = "D"
				},
				new SelectListItem()
				{
					Value = "All",
					Text = "All",
					Selected = true
				}
			};
			this.AssetCategories = new List<SelectListItem>()
			{
				new SelectListItem()
				{
					Value = null,
					Text = "All",
					Selected = true
				},
				new SelectListItem()
				{
					Value = AssetCategory.Paper.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetCategory.Paper)
				},
				new SelectListItem()
				{
					Value = AssetCategory.Real.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetCategory.Real)
				}
			};
			this.AssetTypes = new List<SelectListItem>()
			{
				new SelectListItem()
				{
					Value = "",
					Text = "All"
				},
				new SelectListItem()
				{
					Value = AssetType.ConvenienceStoreFuel.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.ConvenienceStoreFuel)
				},
				new SelectListItem()
				{
					Value = AssetType.FracturedCondominiumPortfolio.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.FracturedCondominiumPortfolio)
				},
				new SelectListItem()
				{
					Value = AssetType.Industrial.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.Industrial)
				},
				new SelectListItem()
				{
					Value = AssetType.Medical.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.Medical)
				},
				new SelectListItem()
				{
					Value = AssetType.MHP.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.MHP)
				},
				new SelectListItem()
				{
					Value = AssetType.MiniStorageProperty.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.MiniStorageProperty)
				},
				new SelectListItem()
				{
					Value = AssetType.MixedUse.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.MixedUse)
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
					Value = AssetType.ParkingGarageProperty.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.ParkingGarageProperty)
				},
				new SelectListItem()
				{
					Value = AssetType.Hotel.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.Hotel)
				},
				new SelectListItem()
				{
					Value = AssetType.Retail.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.Retail)
				},
				new SelectListItem()
				{
					Value = AssetType.SingleTenantProperty.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.SingleTenantProperty)
				}
				/*new SelectListItem()
				{
					Value = AssetType.SecuredPaper.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.SecuredPaper)
				}*/
			};
			this.ListingStatuses = new List<SelectListItem>()
			{
				new SelectListItem()
				{
					Value = null,
					Text = "All",
					Selected = true
				},
				new SelectListItem()
				{
					Value = ListingStatus.Available.ToString(),
					Text = EnumHelper.GetEnumDescription(ListingStatus.Available)
				},
				new SelectListItem()
				{
					Value = ListingStatus.Pending.ToString(),
					Text = EnumHelper.GetEnumDescription(ListingStatus.Pending)
				},
				new SelectListItem()
				{
					Value = ListingStatus.SoldAndClosed.ToString(),
					Text = EnumHelper.GetEnumDescription(ListingStatus.SoldAndClosed)
				},
				new SelectListItem()
				{
					Value = ListingStatus.SoldNotClosed.ToString(),
					Text = EnumHelper.GetEnumDescription(ListingStatus.SoldNotClosed)
				},
				new SelectListItem()
				{
					Value = ListingStatus.Withdrawn.ToString(),
					Text = EnumHelper.GetEnumDescription(ListingStatus.Withdrawn)
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