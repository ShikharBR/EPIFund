using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
    public class FavoriteGroupAssetsQuickListModel
    {
        public Guid AssetId
        {
            get;
            set;
        }

        public string AssetName
        {
            get;
            set;
        }

        public int AssetNumber
        {
            get;
            set;
        }

        public string City
        {
            get;
            set;
        }

        public string State
        {
            get;
            set;
        }

        public int NumberOfUnits
        {
            get;
            set;
        }

        public string SquareFeet
        {
            get;
            set;
        }

        public string PricingCMV
        {
            get;
            set;
        }

        public string AskingPrice
        {
            get;
            set;
        }

        public string CurrentBpo
        {
            get;
            set;
        }

        public string PriceChange
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public string ProformaSGI
        {
            get;
            set;
        }

        public string ProformaNOI
        {
            get;
            set;
        }

        public string CapRate
        {
            get;
            set;
        }

        public string OccupancyPercentage
        {
            get;
            set;
        }

        public string OccupancyDate
        {
            get;
            set;
        }

        public string CallforOffersDate
        {
            get;
            set;
        }

        public string AuctionDate
        {
            get;
            set;
        }

        public bool CallforOffersDateSoon
        {
            get;
            set;
        }

        public bool DisableLOIFPA
        {
            get;
            set;
        }

        public MortgageLienAssumable? AssmFinancing
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public bool IsPartOfPortfolio
        {
            get;
            set;
        }

        public Guid? PortFolioID
        {
            get;
            set;
        }
    }
}
