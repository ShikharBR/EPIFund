using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Domain.Enum
{
    public enum AssetSubType
    {
        [Description("Single Tenant")]
        IND_SingleTenant = 1,
        [Description("Multiple Tenant")]
        IND_MultipleTenant = 2,
        [Description("Publically Traded Single Tenant")]
        IND_PTSingleTenant = 3,
        [Description("Publically Traded Multiple Tenant")]
        IND_PTMultipleTenant = 4,
        [Description("Warehouse - Single Tenant")]
        IND_WSingleTenant = 5,
        [Description("Warehouse - Multiple Tenant")]
        IND_WMultipleTenant = 6,
        [Description("Light Manufacturing")]
        IND_LightManu = 7,
        [Description("Heavy Manufacturing")]
        IND_HeavyManu = 8,
        [Description("Office - Warehouse")]
        IND_Warehouse = 9,

        [Description("Other")]
        MED_Hospital = 10,
        [Description("Resort/Hotel/Motel Property")]
        MED_HOSCENTER = 11,
        [Description("Franchised Tenant")]
        MED_FranchisedTenant = 12,
        [Description("Publically Traded Franchised Tenant")]
        MED_PublicallyTradedFranchisedTenant = 13,
        [Description(" Publically Traded Major Tenant")]
        MED_PublicallyTradedMajorTenant = 14,
        [Description(" Single Tenant")]
        MED_SingleTenant = 15,
        [Description("Multiple Tenant")]
        MED_MultipleTenant = 16,
        [Description("Research Facility")]
        MED_ResearchFacility = 17,
        [Description("Rehabilitation Facility")]
        MED_RehabilitationFacility = 18,
        [Description("Assisted Living Facility")]
        MED_AssistedLivingFacility = 19,

        [Description("Single Tenant")]
        OFF_SingleTenant = 20,
        [Description("Publically Traded Single Tenant")]
        OFF_PublicallyTradedSingleTenant = 21,
        [Description("Multiple Tenant")]
        OFF_MultipleTenant = 22,
        [Description("Publically Traded Major Tenant")]
        OFF_PublicallyTradedMajorTenant = 23,
        [Description("High Rise")]
        OFF_HighRise = 24,
        [Description("Mid Rise")]
        OFF_MidRise = 25,
        [Description("Garden Style")]
        OFF_GardenStyle = 26,
        [Description("Office Condominium Development")]
        OFF_OfficeCondominiumDevelopment = 27,
        [Description("Government Tenant")]
        OFF_GovernmentTenant = 28,

        [Description("Single Tenant")]
        RET_SingleTenant = 29,
        [Description("Multiple Tenant with Anchor")]
        RET_MultipleTenantwithAnchor = 30,
        [Description("Multiple Tenant without Anchor")]
        RET_MultipleTenantwithoutAnchor = 31,
        [Description("Publically Traded Single Tenant")]
        RET_PublicallyTradedSingleTenant = 32,
        [Description("Publically Traded Anchor Tenant")]
        RET_PublicallyTradedAnchorTenant = 33,
        [Description("Restaurant Franchise")]
        RET_RestaurantFranchise = 34,
        [Description("Grocery Franchise")]
        RET_GroceryFranchise = 35,
        [Description("Retail Franchise")]
        RET_RetailFranchise = 36,
        [Description("Factory Outlet Center")]
        RET_FactoryOutletCenter = 37,
        [Description("Government Tenant")]
        RET_GovernmentTenant = 38,
        [Description("Regional Mall")]
        RET_RegionalMall = 39,

        [Description("Convenience Store Franchise – No Fuel")]
        FUL_ConvenienceStoreFranchiseNoFuel = 40,
        [Description("Convenience Store Franchise – With Fuel")]
        FUL_ConvenienceStoreFranchiseWithFuel = 41,
        [Description("Convenience Store – No Fuel")]
        FUL_ConvenienceStoreNoFuel = 42,
        [Description("Convenience Store – With Fuel")]
        FUL_ConvenienceStoreWithFuel = 43,
        [Description("Multi-Purpose Trucking Service Facility")]
        FUL_MultiPurposeTruckingServiceFacility = 44,
        [Description("Auto Care Facility – With Fuel")]
        FUL_AutoCareFacilityWithFuel = 45,
        [Description("Auto Care Facility – No Fuel")]
        FUL_AutoCareFacilityNoFuel = 46,
        [Description("Franchised Auto Care Facility")]
        FUL_FranchisedAutoCareFacility = 47,


        [Description("Student Housing Development")]
        MUL_StudentHousingDevelopment = 48,
        [Description("Retirement Housing Development")]
        MUL_RetirementHousingDevelopment = 49,
        [Description("Garden Style")]
        MUL_GardenStyle = 50,
        [Description("High Rise")]
        MUL_HighRise = 51,
        [Description("Mid Rise")]
        MUL_MidRise = 52,
        [Description("Section 8 Housing – Government Assisted")]
        MUL_SectionHousingGovernmentAssisted = 53,
        [Description("LIHTC")]
        MUL_LIHTC = 54,
        [Description(" Property with 1+ Community Pool/Spa")]
        MUL_PropertywithCommunityPoolSpa = 55,


        [Description("MHP Properties with or without RV Facilities")]
        MHP_PropertieswithwithoutRVFacilities = 56,
        [Description("MHP Properties without RV Facilities")]
        MHP_PropertieswithoutRVFacilities = 57,
        [Description("MHP Properties with Only Affixed Units")]
        MHP_PropertieswithOnlyAffixedUnits = 58,
        [Description("MHP Properties with Affixed or Above Ground Units")]
        MHP_PropertieswithAffixedAboveGroundUnits = 59,
        [Description("MHP Properties with HOA’s")]
        MHP_PropertieswithHOA = 60,
        [Description("RV Parks and Campsites without MHP Spaces")]
        MHP_RVParksCampsiteswithoutMHPSpaces = 61
    }
}
