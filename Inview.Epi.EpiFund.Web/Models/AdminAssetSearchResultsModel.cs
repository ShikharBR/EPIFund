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
	public class AdminAssetSearchResultsModel
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

		[Display(Name="Address Line 1")]
		public string AddressLine1
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


		[Display(Name = "AssmFin")]
		[Required(ErrorMessage = "AssmFin is required")]
		public Inview.Epi.EpiFund.Domain.Enum.AssmFin AssmFin
		{
			get;
			set;
		}

		public PagedList.IPagedList<AdminAssetQuickListModel> Assets
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

		[Display(Name="Zip")]
		public string ZipCode
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

		[Display(Name = "Yes")]
		public bool AssmFinYes
		{
			get;
			set;
		}

		[Display(Name = "No")]
		public bool AssmFinNo
		{
			get;
			set;
		}


		public List<SelectListItem> States
        {
            get;
            set;
        }

        public List<SelectListItem> DisabledStates
        {
            get { return new List<SelectListItem>() { new SelectListItem() { Text = "All" } }; }
        }

        [Display(Name = "APN Number")]
        public string ApnNumber
        {
            get;
            set;
        }

		[Display(Name = "County")]
		public string County
		{
			get;
			set;
		}

		[Display(Name = "List Agent Company Name")]
		public string ListAgentCompanyName
		{
			get;
			set;
		}

		[Display(Name = "List Agent Name")]
		public string ListAgentName
		{
			get;
			set;
		}

		public AdminAssetSearchResultsModel()
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
					Value = AssetType.MultiFamily.ToString(),
					Text = EnumHelper.GetEnumDescription(AssetType.MultiFamily)
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
				},
                /*new SelectListItem()
                {
                    Value = AssetType.SecuredPaper.ToString(),
                    Text = EnumHelper.GetEnumDescription(AssetType.SecuredPaper)
                },*/
                new SelectListItem()
                {
                    Value = AssetType.Other.ToString(),
                    Text = EnumHelper.GetEnumDescription(AssetType.Other)
                }
            };
            this.States = Common.GetSelectListItemsOfStates(true);



		}
	}
}