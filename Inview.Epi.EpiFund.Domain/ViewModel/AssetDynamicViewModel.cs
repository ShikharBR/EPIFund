using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;



namespace Inview.Epi.EpiFund.Domain.ViewModel
{
    public class AssetDynamicViewModel
    {
        public int Total { get; set; }
        public int PublishedAssets { get; set; }
        public int GlobalPartDB { get; set; }
        public long TotalAssetVal { get; set; }
        public int MultiFamUnits { get; set; }
        public int TotalSqFt { get; set; }
        public Guid SearchId { get; set; }

        public List<AssetDynamicAssetViewModel> Assets { get; set; }

        public AssetDynamicViewModel()
        {
            Assets = new List<AssetDynamicAssetViewModel>();
        }
    }
    public class AssetDynamicAssetViewModel
    {
        public Guid AssetId { get; set; }
        public string ProjectName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public double? AskingPrice { get; set; }
        public double? CurrentBpo { get; set; }
        public int AssetType { get; set; }
        public int SquareFeet { get; set; }
        public int? TotalUnits { get; set; }
        public string Description { get; set; }
        public string NewLotSize { get; set; }
        public AssetDynamicImageViewModel Image { get; set; }
        public int ListingStatus { get; set; }
        public int ProformaAOE { get; set; }
        public double ProformaAI { get; set; }
        public double ProformaMI { get; set; }
        public double ProformaVF { get; set; }
        public double ProformaNOI { get; set; }
        public string OccupancyRate { get; set; }
        public string OccupancyDate { get; set; }
        public string OccupancyYear { get; set; }
        public double? SalesPrice { get; set; }
        public int? EstDeferredMaintenance { get; set; }
        public float? CapRate { get; set; }
        public string AuctionDate { get; set; }
        public string CallforOffersDate { get; set; }
        public bool CallforOffersDateSoon { get; set; }
        public string AssmFinancing { get; set; }
        public bool IsPartOfPortfolio { get; set; }
        public Guid PortfolioId { get; set; }
        public string PriceChange { get; set; }
        public bool DisableLOIFPA { get; set; }
        public string AssetTypeDescription
        {
            get
            {
                return EnumHelper.GetEnumDescription((AssetType)AssetType);
            }
        }
        public string AssetTypeAbbreviation
        {
            get
            {
                return EnumHelper.GetAbbreviation((AssetType)AssetType);
            }
        }
        public string AssetTypeShorthand
        {
            get
            {
                return EnumHelper.GetAssetTypeShorthand((AssetType)AssetType);
            }
        }
        public string ListingStatusDescription
        {
            get
            {
                return EnumHelper.GetEnumDescription((ListingStatus)ListingStatus);
            }
        }
        public int YearBuilt { get; set; }
    }
    public class AssetDynamicImageViewModel
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
