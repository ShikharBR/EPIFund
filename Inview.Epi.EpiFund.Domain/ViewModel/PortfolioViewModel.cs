using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
    public class PortfolioViewModel : BaseModel
    {
        public double AccumulatedBPO
        {
            get;
            set;
        }

        public double AccumulatedEstDef
        {
            get;
            set;
        }

        [Display(Name = "Address Line 1")]
        public string AddressLine1
        {
            get;
            set;
        }

        [Display(Name = "APN Number")]
        public string APN
        {
            get;
            set;
        }

        public List<SelectListItem> AssetList
        {
            get;
            set;
        }

        [Display(Name = "Asset Name")]
        public string AssetName
        {
            get;
            set;
        }

        [Display(Name = "Asset #")]
        public string AssetNumber
        {
            get;
            set;
        }

        public List<AdminAssetQuickListModel> Assets
        {
            get;
            set;
        }

        public List<SelectListItem> AssetTypes
        {
            get;
            set;
        }

        public double AvgInterestRate
        {
            get;
            set;
        }

        [Display(Name = "Proforma VF and Offsets (Averaged)")]
        public double AvgProformaVF
        {
            get;
            set;
        }

        public DateTime BallonDate
        {
            get;
            set;
        }

        [Display(Name = "Auction/Call for Offer Date")]
        public DateTime? CallforOfferDate
        {
            get;
            set;
        }

        [Display(Name = "City")]
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

        [Display(Name = "Current Annualized Gross Income")]
        public double CumiAnnualGrossIncome
        {
            get;
            set;
        }

        [Display(Name = "Independent Appraisal/BPO (Cumulative)")]
        public double CumiBPO
        {
            get;
            set;
        }

        [Display(Name = "Independent Appraisal/BPO CAP Rate")]
        public double CumiBPOCapRate
        {
            get;
            set;
        }

        public double CumiDefCapRate
        {
            get;
            set;
        }

        public double CumiDefProformaNOI
        {
            get;
            set;
        }

        public double CumiDefProformaSGI
        {
            get;
            set;
        }

        [Display(Name = "Listed Price CAP Rate (Cumulative)")]
        public double CumiLPCapRate
        {
            get;
            set;
        }

        public double CumiMonthBal
        {
            get;
            set;
        }

        public double CumiPrinciBal
        {
            get;
            set;
        }

        [Display(Name = "Proforma AOE (Cumulative)")]
        public double CumiProformaAOE
        {
            get;
            set;
        }

        [Display(Name = "Proforma CAM / AMI  Income")]
        public double CumiProformaCAM
        {
            get;
            set;
        }

        [Display(Name = "Proforma Pre Tax NOI (Cumulative)")]
        public double CumiProformaNOI
        {
            get;
            set;
        }

        [Display(Name = "Proforma SGI (Cumulative)")]
        public double CumiProformaSGI
        {
            get;
            set;
        }

        public double CumiSqFeet
        {
            get;
            set;
        }

        public double CumiTotalListing
        {
            get;
            set;
        }

        public double CumiUnits
        {
            get;
            set;
        }

        public List<AssetDeferredItemViewModel> DeferredMaintenanceItems
        {
            get;
            set;
        }

        [Display(Name = "Asset Entered End Date")]
        public DateTime? EndDate
        {
            get;
            set;
        }

        [Display(Name = "Estimated Deferred Maintenance")]
        public int EstDeferredMaintenance
        {
            get;
            set;
        }

        public bool ExistingDebtExists
        {
            get;
            set;
        }

        public bool HasDeferredMaintenance
        {
            get;
            set;
        }

        [Display(Name = "Does Portfolio have a CALL FOR OFFERS DATE?")]
        [Required(ErrorMessage = "Does Portfolio have a CALL FOR OFFERS DATE is Required")]
        public bool hasOffersDate
        {
            get;
            set;
        }

        public List<AssetImage> Images
        {
            get;
            set;
        }

        public bool isActive
        {
            get;
            set;
        }

        public int isARM
        {
            get;
            set;
        }

        public bool isIntial
        {
            get;
            set;
        }

        [Display(Name = "Is Portfolio Subject to Auction?")]
        [Required(ErrorMessage = "Is Portfolio Subject to Auction is Required")]
        public bool? IsTBDMarket
        {
            get;
            set;
        }

        public string MBAAgentName
        {
            get;
            set;
        }

        public string MortgageAdjustIfARM
        {
            get;
            set;
        }

        [Display(Name = "Number of Assets")]
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

        public double PercentOfPropLeased
        {
            get;
            set;
        }

        [Display(Name = "Proforma AOE as % of SGI")]
        public double PFProformaAoeFactorAsPerOfSGI
        {
            get;
            set;
        }

        public Guid PortfolioId
        {
            get;
            set;
        }

        [Display(Name = "Listed/Asking Price: ")]
        public double PortfolioListedPrice
        {
            get;
            set;
        }

        [Display(Name = "Portfolio Name")]
        [Required(ErrorMessage = "Portfolio Name is Required")]
        public string PortfolioName
        {
            get;
            set;
        }
        [Display(Name = "Must Portfolio be Sold All Assets Inclusive")]
        [Required(ErrorMessage = "Must Portfolio be Sold All Assets Inclusive")]
        public bool? MustPortfolioAssetsInclusive
        {
            get;
            set;
        }

        

        [Display(Name = "Last Reported Occupancy")]
        public DateTime? LastReportedOccupancyDate
        {
            get;
            set;
        }
        
        [Display(Name = "Cap Rate")]
        public float? CapRate
        {
            get;
            set;
        }
        [Display(Name = "Is there a Call for Offers Date?")]
        public bool? IsCallOffersDate
        {
            get;
            set;
        }

        public List<AssetViewModel> PortfolioProperties
        {
            get;
            set;
        }
        public List<AdminAssetQuickListModel> PortfolioAssetList
        {
            get;
            set;
        }

        public int? RowCount
        {
            get;
            set;
        }

        public string SelectedAmortSchedule
        {
            get;
            set;
        }

        public List<Guid> SelectedAssets
        {
            get;
            set;
        }

        [Display(Name = "Asset Type")]
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

        [Display(Name = "Asset Entered Start Date")]
        public DateTime? StartDate
        {
            get;
            set;
        }

        [Display(Name = "State")]
        public string State
        {
            get;
            set;
        }

        public new List<SelectListItem> States
        {
            get;
            set;
        }

        public int UserId
        {
            get;
            set;
        }

        public List<AssetVideo> Videos
        {
            get;
            set;
        }

        [Display(Name = "Zip")]
        public string ZipCode
        {
            get;
            set;
        }

        [Display(Name = "Pricing Display Option")]
        public Inview.Epi.EpiFund.Domain.Enum.PricingDisplay PricingDisplayOption { get; set; }

        [Display(Name = "Define Sale Terms of Portfolio Acceptable by Seller")]
        public Inview.Epi.EpiFund.Domain.Enum.SellerTerms SellerTerms { get; set; }

        [Display(Name = "Other")]
        public string SellerTermsOther { get; set; }

        [Display(Name = "Listing Status all")]
        public Inview.Epi.EpiFund.Domain.Enum.ListingStatusall ListingStatus { get; set; }


        public PortfolioViewModel()
        {
            this.Images = new List<AssetImage>();
            this.DeferredMaintenanceItems = new List<AssetDeferredItemViewModel>();
            this.Videos = new List<AssetVideo>();
            this.PortfolioProperties = new List<AssetViewModel>();
            this.SelectedAssets = new List<Guid>();
            this.LastReportedOccupancyDate = new DateTime?(DateTime.MinValue);

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            SelectListItem selectListItem = new SelectListItem()
            {
                Value = string.Empty,
                Text = "All",
                Selected = true
            };
            selectListItems.Add(selectListItem);
            SelectListItem selectListItem1 = new SelectListItem()
            {
                Value = AssetType.Office.ToString(),
                Text = EnumHelper.GetEnumDescription(AssetType.Office)
            };
            selectListItems.Add(selectListItem1);
            SelectListItem selectListItem2 = new SelectListItem()
            {
                Value = AssetType.Retail.ToString(),
                Text = EnumHelper.GetEnumDescription(AssetType.Retail)
            };
            selectListItems.Add(selectListItem2);
            SelectListItem selectListItem3 = new SelectListItem()
            {
                Value = AssetType.Industrial.ToString(),
                Text = EnumHelper.GetEnumDescription(AssetType.Industrial)
            };
            selectListItems.Add(selectListItem3);
            SelectListItem selectListItem4 = new SelectListItem()
            {
                Value = AssetType.MultiFamily.ToString(),
                Text = EnumHelper.GetEnumDescription(AssetType.MultiFamily)
            };
            selectListItems.Add(selectListItem4);
            SelectListItem selectListItem5 = new SelectListItem()
            {
                Value = AssetType.MHP.ToString(),
                Text = EnumHelper.GetEnumDescription(AssetType.MHP)
            };
            selectListItems.Add(selectListItem5);
            SelectListItem selectListItem6 = new SelectListItem()
            {
                Value = AssetType.Medical.ToString(),
                Text = EnumHelper.GetEnumDescription(AssetType.Medical)
            };
            selectListItems.Add(selectListItem6);
            SelectListItem selectListItem7 = new SelectListItem()
            {
                Value = AssetType.MixedUse.ToString(),
                Text = EnumHelper.GetEnumDescription(AssetType.MixedUse)
            };
            selectListItems.Add(selectListItem7);
            SelectListItem selectListItem8 = new SelectListItem()
            {
                Value = AssetType.Hotel.ToString(),
                Text = EnumHelper.GetEnumDescription(AssetType.Hotel)
            };
            selectListItems.Add(selectListItem8);
            SelectListItem selectListItem9 = new SelectListItem()
            {
                Value = AssetType.SingleTenantProperty.ToString(),
                Text = EnumHelper.GetEnumDescription(AssetType.SingleTenantProperty)
            };
            selectListItems.Add(selectListItem9);
            SelectListItem selectListItem10 = new SelectListItem()
            {
                Value = AssetType.FracturedCondominiumPortfolio.ToString(),
                Text = EnumHelper.GetEnumDescription(AssetType.FracturedCondominiumPortfolio)
            };
            selectListItems.Add(selectListItem10);
            SelectListItem selectListItem11 = new SelectListItem()
            {
                Value = AssetType.MiniStorageProperty.ToString(),
                Text = EnumHelper.GetEnumDescription(AssetType.MiniStorageProperty)
            };
            selectListItems.Add(selectListItem11);
            SelectListItem selectListItem12 = new SelectListItem()
            {
                Value = AssetType.ParkingGarageProperty.ToString(),
                Text = EnumHelper.GetEnumDescription(AssetType.ParkingGarageProperty)
            };
            selectListItems.Add(selectListItem12);
            SelectListItem selectListItem13 = new SelectListItem()
            {
                Value = AssetType.ConvenienceStoreFuel.ToString(),
                Text = EnumHelper.GetEnumDescription(AssetType.ConvenienceStoreFuel)
            };
            selectListItems.Add(selectListItem13);
            this.AssetTypes = selectListItems;

            List<SelectListItem> selectListItems1 = new List<SelectListItem>();
            SelectListItem selectListItem14 = new SelectListItem()
            {
                Text = "All",
                Value = "",
                Selected = true
            };
            selectListItems1.Add(selectListItem14);
            SelectListItem selectListItem15 = new SelectListItem()
            {
                Text = "AL",
                Value = "AL"
            };
            selectListItems1.Add(selectListItem15);
            SelectListItem selectListItem16 = new SelectListItem()
            {
                Text = "AK",
                Value = "AK"
            };
            selectListItems1.Add(selectListItem16);
            SelectListItem selectListItem17 = new SelectListItem()
            {
                Text = "AZ",
                Value = "AZ"
            };
            selectListItems1.Add(selectListItem17);
            SelectListItem selectListItem18 = new SelectListItem()
            {
                Text = "AR",
                Value = "AR"
            };
            selectListItems1.Add(selectListItem18);
            SelectListItem selectListItem19 = new SelectListItem()
            {
                Text = "CA",
                Value = "CA"
            };
            selectListItems1.Add(selectListItem19);
            SelectListItem selectListItem20 = new SelectListItem()
            {
                Text = "CO",
                Value = "CO"
            };
            selectListItems1.Add(selectListItem20);
            SelectListItem selectListItem21 = new SelectListItem()
            {
                Text = "CT",
                Value = "CT"
            };
            selectListItems1.Add(selectListItem21);
            SelectListItem selectListItem22 = new SelectListItem()
            {
                Text = "DC",
                Value = "DC"
            };
            selectListItems1.Add(selectListItem22);
            SelectListItem selectListItem23 = new SelectListItem()
            {
                Text = "DE",
                Value = "DE"
            };
            selectListItems1.Add(selectListItem23);
            SelectListItem selectListItem24 = new SelectListItem()
            {
                Text = "FL",
                Value = "FL"
            };
            selectListItems1.Add(selectListItem24);
            SelectListItem selectListItem25 = new SelectListItem()
            {
                Text = "GA",
                Value = "GA"
            };
            selectListItems1.Add(selectListItem25);
            SelectListItem selectListItem26 = new SelectListItem()
            {
                Text = "HI",
                Value = "HI"
            };
            selectListItems1.Add(selectListItem26);
            SelectListItem selectListItem27 = new SelectListItem()
            {
                Text = "ID",
                Value = "ID"
            };
            selectListItems1.Add(selectListItem27);
            SelectListItem selectListItem28 = new SelectListItem()
            {
                Text = "IL",
                Value = "IL"
            };
            selectListItems1.Add(selectListItem28);
            SelectListItem selectListItem29 = new SelectListItem()
            {
                Text = "IN",
                Value = "IN"
            };
            selectListItems1.Add(selectListItem29);
            SelectListItem selectListItem30 = new SelectListItem()
            {
                Text = "IA",
                Value = "IA"
            };
            selectListItems1.Add(selectListItem30);
            SelectListItem selectListItem31 = new SelectListItem()
            {
                Text = "KS",
                Value = "KS"
            };
            selectListItems1.Add(selectListItem31);
            SelectListItem selectListItem32 = new SelectListItem()
            {
                Text = "KY",
                Value = "KY"
            };
            selectListItems1.Add(selectListItem32);
            SelectListItem selectListItem33 = new SelectListItem()
            {
                Text = "LA",
                Value = "LA"
            };
            selectListItems1.Add(selectListItem33);
            SelectListItem selectListItem34 = new SelectListItem()
            {
                Text = "ME",
                Value = "ME"
            };
            selectListItems1.Add(selectListItem34);
            SelectListItem selectListItem35 = new SelectListItem()
            {
                Text = "MD",
                Value = "MD"
            };
            selectListItems1.Add(selectListItem35);
            SelectListItem selectListItem36 = new SelectListItem()
            {
                Text = "MA",
                Value = "MA"
            };
            selectListItems1.Add(selectListItem36);
            SelectListItem selectListItem37 = new SelectListItem()
            {
                Text = "MI",
                Value = "MI"
            };
            selectListItems1.Add(selectListItem37);
            SelectListItem selectListItem38 = new SelectListItem()
            {
                Text = "MN",
                Value = "MN"
            };
            selectListItems1.Add(selectListItem38);
            SelectListItem selectListItem39 = new SelectListItem()
            {
                Text = "MS",
                Value = "MS"
            };
            selectListItems1.Add(selectListItem39);
            SelectListItem selectListItem40 = new SelectListItem()
            {
                Text = "MO",
                Value = "MO"
            };
            selectListItems1.Add(selectListItem40);
            SelectListItem selectListItem41 = new SelectListItem()
            {
                Text = "MT",
                Value = "MT"
            };
            selectListItems1.Add(selectListItem41);
            SelectListItem selectListItem42 = new SelectListItem()
            {
                Text = "NE",
                Value = "NE"
            };
            selectListItems1.Add(selectListItem42);
            SelectListItem selectListItem43 = new SelectListItem()
            {
                Text = "NV",
                Value = "NV"
            };
            selectListItems1.Add(selectListItem43);
            SelectListItem selectListItem44 = new SelectListItem()
            {
                Text = "NH",
                Value = "NH"
            };
            selectListItems1.Add(selectListItem44);
            SelectListItem selectListItem45 = new SelectListItem()
            {
                Text = "NJ",
                Value = "NJ"
            };
            selectListItems1.Add(selectListItem45);
            SelectListItem selectListItem46 = new SelectListItem()
            {
                Text = "NM",
                Value = "NM"
            };
            selectListItems1.Add(selectListItem46);
            SelectListItem selectListItem47 = new SelectListItem()
            {
                Text = "NY",
                Value = "NY"
            };
            selectListItems1.Add(selectListItem47);
            SelectListItem selectListItem48 = new SelectListItem()
            {
                Text = "NC",
                Value = "NC"
            };
            selectListItems1.Add(selectListItem48);
            SelectListItem selectListItem49 = new SelectListItem()
            {
                Text = "ND",
                Value = "ND"
            };
            selectListItems1.Add(selectListItem49);
            SelectListItem selectListItem50 = new SelectListItem()
            {
                Text = "OH",
                Value = "OH"
            };
            selectListItems1.Add(selectListItem50);
            SelectListItem selectListItem51 = new SelectListItem()
            {
                Text = "OK",
                Value = "OK"
            };
            selectListItems1.Add(selectListItem51);
            SelectListItem selectListItem52 = new SelectListItem()
            {
                Text = "OR",
                Value = "OR"
            };
            selectListItems1.Add(selectListItem52);
            SelectListItem selectListItem53 = new SelectListItem()
            {
                Text = "PA",
                Value = "PA"
            };
            selectListItems1.Add(selectListItem53);
            SelectListItem selectListItem54 = new SelectListItem()
            {
                Text = "RI",
                Value = "RI"
            };
            selectListItems1.Add(selectListItem54);
            SelectListItem selectListItem55 = new SelectListItem()
            {
                Text = "SC",
                Value = "SC"
            };
            selectListItems1.Add(selectListItem55);
            SelectListItem selectListItem56 = new SelectListItem()
            {
                Text = "SD",
                Value = "SD"
            };
            selectListItems1.Add(selectListItem56);
            SelectListItem selectListItem57 = new SelectListItem()
            {
                Text = "TN",
                Value = "TN"
            };
            selectListItems1.Add(selectListItem57);
            SelectListItem selectListItem58 = new SelectListItem()
            {
                Text = "TX",
                Value = "TX"
            };
            selectListItems1.Add(selectListItem58);
            SelectListItem selectListItem59 = new SelectListItem()
            {
                Text = "UT",
                Value = "UT"
            };
            selectListItems1.Add(selectListItem59);
            SelectListItem selectListItem60 = new SelectListItem()
            {
                Text = "VT",
                Value = "VT"
            };
            selectListItems1.Add(selectListItem60);
            SelectListItem selectListItem61 = new SelectListItem()
            {
                Text = "VA",
                Value = "VA"
            };
            selectListItems1.Add(selectListItem61);
            SelectListItem selectListItem62 = new SelectListItem()
            {
                Text = "WA",
                Value = "WA"
            };
            selectListItems1.Add(selectListItem62);
            SelectListItem selectListItem63 = new SelectListItem()
            {
                Text = "WV",
                Value = "WV"
            };
            selectListItems1.Add(selectListItem63);
            SelectListItem selectListItem64 = new SelectListItem()
            {
                Text = "WI",
                Value = "WI"
            };
            selectListItems1.Add(selectListItem64);
            SelectListItem selectListItem65 = new SelectListItem()
            {
                Text = "WY",
                Value = "WY"
            };
            selectListItems1.Add(selectListItem65);
            this.States = selectListItems1;
        }

        public PortfolioViewModel EntityToModel(Portfolio entity)
        {
            this.CallforOfferDate = entity.CallforOfferDate;
            this.hasOffersDate = entity.hasOffersDate;
            this.IsTBDMarket = entity.IsTBDMarket;
            this.LastReportedOccupancyDate = entity.LastReportedOccupancyDate;
            this.NumberofAssets = entity.NumberofAssets;
            this.PortfolioName = entity.PortfolioName;
            this.PortfolioId = entity.PortfolioId;
            this.UserId = entity.UserId;
            this.MustPortfolioAssetsInclusive = entity.MustPortfolioAssetsInclusive;

            this.IsCallOffersDate = entity.IsCallOffersDate;

            this.isActive = entity.isActive;

            this.CapRate = entity.CapRete;
            this.ListingStatus = entity.ListingStatus??0;
            this.PricingDisplayOption = entity.PricingDisplayOption??0;
            this.SellerTerms = entity.SellerTerms ?? 0;
            this.SellerTermsOther = entity.SellerTermsOther;

            return this;
        }

        public Portfolio ModelToEntity()
        {
            Portfolio portfolio = new Portfolio()
            {
                MustPortfolioAssetsInclusive = this.MustPortfolioAssetsInclusive.HasValue ? this.MustPortfolioAssetsInclusive.Value : false,

                IsCallOffersDate = this.IsCallOffersDate.HasValue ? this.IsCallOffersDate.Value:false,

                CallforOfferDate = this.CallforOfferDate,
                hasOffersDate = this.hasOffersDate,
                IsTBDMarket = this.IsTBDMarket.HasValue ? this.IsTBDMarket.Value : false,
                LastReportedOccupancyDate = this.LastReportedOccupancyDate,
                NumberofAssets = this.NumberofAssets,
                PortfolioName = this.PortfolioName,
                PortfolioId = this.PortfolioId,
                UserId = this.UserId,
                isActive = this.isActive,
                CapRete = this.CapRate,
                ListingStatus = this.ListingStatus,
                PricingDisplayOption = this.PricingDisplayOption,
                SellerTerms = this.SellerTerms,
                SellerTermsOther = this.SellerTermsOther
            };
            return portfolio;
        }
                
        public string AssetIdsLpCMV { get; set; }
    }
}