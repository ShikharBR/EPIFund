using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
     public class SearchAssetModel
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsLoggedIn { get; set; }

        // Lat/Lng and search radius  (in kilometers)
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? SearchRadius { get; set; }

        // ddls
        public int? AssetType { get; set; } // TESTED
        public int? AssetSubType { get; set; }
        public int? OperatingStatus { get; set; }
        public int? ListingStatus { get; set; }
        public string ListingType { get; set; }
        public int? PriceSearchType { get; set; }

        // checkbox

        // lists
        public List<int> AssetTypes { get; set; }
        public List<int> AssetSubTypes { get; set; }
        public List<string> GradeClassifications { get; set; }

        public List<int> VacancyFactor { get; set; }
        public List<int> RentableSpace { get; set; }

        // textboxes
        public string AssetName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public string SearchBarInput { get; set; }
        public float? CapRate { get; set; }
        public int? YearBuilt { get; set; } // val >= 1800 && val <= current year? I guess we only need validation in the UI
        public bool? IsUpdatedChecked { get; set; }
        public bool? IsUpdatedByOwner { get; set; }
        public int? YearUpdated { get; set; }

        public int? UnitsMin { get; set; }
        public int? UnitsMax { get; set; }

        public int? SquareFeetMin { get; set; }
        public int? SquareFeetMax { get; set; }
        public double? SGI { get; set; }
        public double? NOI { get; set; }
        public int? OccPerc { get; set; }

        public double? Min { get; set; }
        public double? Max { get; set; }

        // rando enums added
        public int? AssetStatus { get; set; }
        // public int? MeteringMethod { get; set; }
        public int? MortageLienType { get; set; }
        public int? OccupancyType { get; set; }
        public int? PropertyType { get; set; }
        public int? SoldStatus { get; set; }

        // UNKNOWNS
        public string PropertyAgeRange { get; set; }

        // Utilites


        // Major Tenant
        public string MajorTenant { get; set; }
        public int? MajorTenantOccuPerc { get; set; }
        public int? MajorTenantLeaseExp { get; set; }
        //public List<int> MajorTenantLeaseExp { get; set; }

        // Unit Mix Ratio
        public int? UmrStudioPct { get; set; }
        public int? Umr1BdPct { get; set; }
        public int? Umr2Bd1BaPct { get; set; }
        public int? Umr2Bd2BaPct { get; set; }
        public int? Umr3BdPct { get; set; }
        public int? UmrOccuPerc { get; set; }
        public int? UmrMaxFCPerc { get; set; }

        public List<int> InteriorRoadTypes { get; set; }
        public List<int> AccessRoadTypes { get; set; }
        public List<int> WasteWaterTypes { get; set; }
        public List<int> WaterServTypes { get; set; }
        public List<int> ElectricMeteringMethods { get; set; }
        public List<int> GasMeteringMethods { get; set; }

        // Space Mix Ratio
        public int? SmrSWidePct { get; set; }
        public int? SmrDWidePct { get; set; }
        public int? SmrTWidePct { get; set; }
        public int? SmrParkOwnedUnits { get; set; }
        public int? SmrParkOwnedMaxPct { get; set; }
        public List<int> SmrPropWithUndevAcres { get; set; }
        public int? SmrUndevToTotalAcreRatioPct { get; set; }


        public int? BuildingsCount { get; set; }
        public string AmortizationSchedule { get; set; }
        public DateTime? LeaseholdMaturityDate { get; set; }
        public List<string> PositionMortgage { get; set; }
        public List<string> MortgageInstruments { get; set; }
        public List<string> LoanBalanceofNote { get; set; }
    }
}
