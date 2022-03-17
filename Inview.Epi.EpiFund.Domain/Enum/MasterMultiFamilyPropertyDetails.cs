using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Domain.Enum
{
    public enum MasterMultiFamilyPropertyDetails
    {
        #region MHP

        #endregion
        #region MF

        #endregion
        #region Shared
        [Description("General Features ~ Light Rail access")]
        GenFeatures_LightRailAccess,
        [Description("General Features ~ Subway access")]
        GenFeatures_SubwayAccess,
        [Description("General Features ~ Property has Water Features")]
        GenFeatures_WaterFeatures,

        [Description("Technology Features ~ 1 or more elevators")]
        TechFeatures_Elevators,
        [Description("Technology Features ~ 1 or more escalators")]
        TechFeatures_Escalators,
        [Description("Technology Features ~ Building(s) have Interior Sprinkler Systems")]
        TechFeatures_InteriorSprinklers,
        [Description("Technology Features ~ Interior Building Video Security")]
        TechFeatures_VideoSecurity,
        [Description("Technology Features ~ Individual Unit Security Alarms")]
        TechFeatures_IndividSecurityAlarms,
        #endregion

        [Description("MHP ~ Single Wide Park Owned Units")]
        MHP_SingleWideOwnedUnits,
        [Description("MHP ~ Double Wide Park Owned Units")]
        MHP_DoubleWideOwnedUnits,
        [Description("MHP ~ Triple Wide Park Owned Units")]
        MHP_TripleWideOwnedUnits,

        [Description("MF Architecture ~ Single Level Buildings")]
        Arch_SingleStory,
        [Description("MF Architecture ~ Three Story Buildings")]
        Arch_ThreeStory,
        [Description("MF Architecture ~ Townhouse Style Buildings")]
        Arch_TownhouseStyle,
        [Description("MF Architecture ~ Two Story Buildings")]
        Arch_TwoStory,
        [Description("MF Architecture ~ All units have patios or balconies")]
        Arch_AllHavePatios,
        [Description("MF Architecture ~ Some units have patios or balconies")]
        Arch_SomeHavePatios,
        [Description("MF Architecture ~ Four + Story Buildings")]
        Arch_FourPlusStoryBuildings,

        [Description("MF Unit Exteriors ~ Units Updated within 12 Months")]
        UnitExt_UpdatedIn12Months,
        [Description("MF Unit Exteriors ~ Units Updated within 24 Months")]
        UnitExt_UpdatedIn24Months,
        [Description("MF Unit Exteriors ~ Units Updated within 3-5 Years")]
        UnitExt_UpdatedIn3Years,
        [Description("MF Unit Interiors ~ All Units Updated within 12 Months")]
        UnitInt_UpdatedIn12Months,
        [Description("MF Unit Interiors ~ Some Units Updated within 12 Months")]
        UnitInt_SomeUpdatedIn12Months,
        [Description("MF Unit Interiors ~ All Units Updated within 24 Months")]
        UnitInt_UpdatedIn24Months,
        [Description("MF Unit Interiors ~ Some Units Updated within 24 Months")]
        UnitInt_SomeUpdatedIn24Months,
        [Description("MF Unit Interiors ~ All Units Updated within 3-5 Years")]
        UnitInt_UpdatedIn3Years,
        [Description("MF Unit Interiors ~ Some Units Updated within 3-5 Years")]
        UnitInt_SomeUpdatedIn3Years,
        
        [Description("MF Unit Interiors ~ All Kitchens have Microwaves")]
        UnitInt_AllHaveMicrowaves,
        [Description("MF Unit Interiors ~ Some Kitchens have Microwaves")]
        UnitInt_SomeHaveMicrowaves,
        [Description("MF Unit Interiors ~ All Kitchens have Dishwashers")]
        UnitInt_AllHaveDishwashers,
        [Description("MF Unit Interiors ~ Some Kitchens have Dishwashers")]
        UnitInt_SomeHaveDishwashers,
        [Description("MF Unit Interiors ~ All 2Bed+ Units have 2 Bathrooms")]
        UnitInt_All2BedHave2Bath,
        [Description("MF Unit Interiors ~ Some 2Bed+ Units have 2 Bathrooms")]
        UnitInt_Some2BedHave2Bath,
        [Description("MF Unit Interiors ~ Some units have walk-in closets")]
        UnitInt_SomeHaveWalkinClosets,
        [Description("MF Unit Interiors ~ All units have walk-in closets")]
        UnitInt_AllHaveWalkinClosets,

        [Description("Complex Features ~ Landscape Watering System")]
        Complex_LandscapeWateringSystem,
        [Description("Complex Features ~ Fitness Center")]
        Complex_FitnessCenter,
        [Description("Complex Features ~ 1 or more Community Laundry Facilities")]
        Complex_LaundryFacility,
        [Description("Complex Features ~ Washer/Dryer in all units")]
        Complex_AllWasherDryer,
        [Description("Complex Features ~ Washer/Dryer in some units")]
        Complex_SomeWasherDryer,
        [Description("Complex Features ~ Washer/Dryer in no units")]
        Complex_NoWasherDryer,
        [Description("Complex Features ~ Workout Facility")]
        Complex_WorkoutFacility,
        [Description("Complex Features ~ Volleyball Court")]
        Complex_VolleyballCourt,
        [Description("Complex Features ~ Tennis Courts")]
        Complex_TennisCourt,
        [Description("Complex Features ~ Tennis/Sport Court")]
        Complex_SportCourt,
        [Description("Complex Features ~ Special Golf Membership")]
        Complex_SpecialGolfMembership,
        [Description("Complex Features ~ Manager's Office")]
        Complex_ManagerOffice,
        [Description("Complex Features ~ Lake Views/Front")]
        Complex_LakeView,
        [Description("Complex Features ~ Lake Front")]
        Complex_LakeFront,
        [Description("Complex Features ~ Light Rail Access")]
        Complex_LightRailAccess,
        [Description("Complex Features ~ Golf Course Views/Front")]
        Complex_GolfView,
        [Description("Complex Features ~ Clubhouse")]
        Complex_Clubhouse,
        [Description("Complex Features ~ Child Care Facility")]
        Complex_ChildCareFacility,
        [Description("Complex Features ~ Children's Playground")]
        Complex_ChildrenPlayground,
        [Description("Complex Features ~ RV Gate")]
        Complex_RVGate,
        [Description("Complex Features ~ RV Parking Facility")]
        Complex_RVParkingFacility,
        [Description("Complex Features ~ Common Area Water Features")]
        Complex_CommonAreaWater,
        [Description("Complex Features ~ One Community Pool")]
        Pool_OneCommunity,
        [Description("Complex Features ~ Two or more Community Pools")]
        Pool_TwoCommunity,
        [Description("Complex Features ~ Three Community Pools")]
        Pool_ThreeCommunity,
        [Description("Complex Features ~ One Community Jacuzzi")]
        Jacuzzi_OneCommunity,
        [Description("Complex Features ~ Two or more Community Jacuzzi")]
        Jacuzzi_TwoCommunity,
        [Description("Complex Features ~ Community BBQ Facilities")]
        Complex_CommunityBBQFacility,

        [Description("Complex Features ~ Security Gate")]
        Security_Gate,
        [Description("Complex Features ~ Security Guard Office")]
        Security_GuardOffice,
        [Description("Complex Features ~ External Video Surveillance")]
        Security_VideoSurveillance,
        [Description("Complex Features ~ Internal Common Area Video Surveillance")]
        Security_IntVideoSurveillance,
        [Description("Complex Features ~ Card/Code Building Access")]
        Security_CodeAccess,
        [Description("Complex Features ~ Individual Unit/Suite Security System")]
        Security_IndSecuritySystem,

        [Description("Complex Features ~ One Community Spa")]
        Spa_OneCommunity,
        [Description("Complex Features ~ Three Community Spas")]
        Spa_ThreeCommunity,
        [Description("Complex Features ~ Two Community Spas")]
        Spa_TwoCommunity,
        [Description("Complex Features ~ Business Center")]
        Complex_BusinessCenter,
        [Description("Complex Features ~ Ocean Views/Front")]
        Complex_OceanViews,
        [Description("Complex Features ~ Community Pet Park")]
        Complex_PetPark,
        [Description("Complex Features ~ Perimeter Fencing")]
        Complex_PerimeterFencing,
        [Description("Complex Features ~ Washer/Dryer Hookups in each unit")]
        Complex_WasherDryerHookups,
        [Description("Complex Features ~ Washer/Dryer Hookups in some units")]
        Complex_SomeWasherDryerHookups,
        [Description("Complex Features ~ Valet Trash Removal for Tenants")]
        Complex_ValetTrashRemovalForTenants,

        [Description("Complex Parking ~ No Covered Spaces")]
        Complex_NoCoveredSpace,
        [Description("Complex Parking ~ No Complex open Spaces")]
        Complex_NoOpenSpace,
        [Description("Complex Parking ~ Dedicated Tenant Spaces")]
        Complex_DedicatedTenantSpace,
        [Description("Complex Parking ~ Metered/Fee Based")]
        Complex_MeteredFeeBased,
        [Description("Complex Parking ~ Above or Underground Garage Included")]
        Complex_GarageIncluded,
        [Description("Complex Parking ~ Neighboring Garage Agreement")]
        Complex_NeighboringGarageAgreement,
        [Description("Complex Parking ~ 1 Covered Per Unit")]
        Complex_OneCoveredPerUnit,
        [Description("Complex Parking ~ Some Garages")]
        Complex_SomeGarages,
        [Description("Complex Parking ~ 1 Individual Garage Per Unit")]
        Complex_OneGaragePerUnit,
        [Description("Complex Parking ~ Lighted Parking Areas")]
        Complex_LightedParking,
        [Description("Complex Parking ~ Some Covered Parking")]
        Complex_SomeCoveredPark,
        [Description("Complex Parking ~ Street Parking only")]
        Complex_StreetParkOnly,
        [Description("Complex Parking ~ Some Street Parking")]
        Complex_SomeStreetPark,
        [Description("Complex Parking ~ Some Carports")]
        Complex_SomeCarports,

        [Description("Construction ~ Wood Frame")]
        Const_WoodFrame,
        [Description("Construction ~ Brick")]
        Const_Brick,
        [Description("Construction ~ Steel/Metal & Wood")]
        Const_MetalAndWood,
        [Description("Construction ~ Steel/Metal & Plexiglas")]
        Const_MetalAndGlass,
        [Description("Construction ~ Cement Block")]
        Const_CementBlock,
        [Description("Construction ~ Adobe")]
        Const_Adobe,
        [Description("Construction ~ Slump Block")]
        Const_SlumpBlock,
        [Description("Construction Finish ~ Brick Veneer")]
        Const_FinishBrickVeneer,
        [Description("Construction Finish ~ Painted")]
        Const_FinishPainted,
        [Description("Construction Finish ~ Stone")]
        Const_FinishStone,
        [Description("Construction Finish ~ Siding")]
        Const_FinishSiding,
        [Description("Construction Finish ~ Stucco")]
        Const_FinishStucco,
        [Description("Construction Finish ~ Other")]
        Const_FinishOther,

        [Description("Cooling ~ Evaporative")]
        Cool_Evap,
        [Description("Cooling ~ Refrigeration")]
        Cool_Ref,
        [Description("Cooling ~ Both Ref & Evap")]
        Cool_BothRefAndEvap,
        [Description("Cooling ~ VAV System")]
        Cool_VAVSystem,
        [Description("Cooling ~ Window/Wall Units")]
        Cool_WindowWallUnits,
        [Description("Cooling ~ None")]
        Cool_None,
        [Description("Cooling ~ Ceiling Fans")]
        Cool_CeilingFans,
        [Description("Cooling ~ Heat Pump")]
        Cool_HeatPump,
        [Description("Cooling ~ Sunscreens")]
        Cool_Sunscreens,
        [Description("Cooling ~ HVAC individual climate controlled units")]
        Cool_HVACIndividual,
        [Description("Heating ~ None")]
        Heat_None,
        [Description("Heating ~ Gas")]
        Heat_Gas,
        [Description("Heating ~ Wall/Floor Unit")]
        Heat_WallFloorUnit,
        [Description("Heating ~ Electric")]
        Heat_Electric,
        [Description("Heating ~ Fireplace")]
        Heat_Fireplace,
        [Description("Heating ~ Heat Pump")]
        Heat_HeatPump,

        [Description("Kitchen Amenities ~ Trash Compactor")]
        KitchAmen_TrashCompactor,
        [Description("Kitchen Amenities ~ Breakfast Bar")]
        KitchAmen_BreakfastBar,
        [Description("Kitchen Amenities ~ Refrigerator")]
        KitchAmen_Refrig,
        [Description("Kitchen Amenities ~ Premium Wood Cabinets")]
        KitchAmen_PremiumWoodCabinets,
        [Description("Kitchen Amenities ~ All units with Dishwasher")]
        KitchAmen_Dishwasher,
        [Description("Kitchen Amenities ~ Some units with Dishwasher")]
        KitchAmen_SomeDishwasher,
        [Description("Kitchen Amenities ~ Disposal")]
        KitchAmen_Disposal,
        [Description("Kitchen Amenities ~ Pantry")]
        KitchAmen_Pantry,
        [Description("Kitchen Amenities ~ Microwave: Built-in")]
        KitchAmen_MicrowaveBuiltIn,
        [Description("Kitchen Amenities ~ Some Microwave: Built-in")]
        KitchAmen_SomeMicrowaveBuiltIn,
        [Description("Kitchen Amenities ~ Brand-new appliances")]
        KitchAmen_BrandNewAppliance,
        [Description("Kitchen Amenities ~  Dine-in Kitchen Floorplan")]
        KitchAmen_EatInKitch,
        [Description("Kitchen Amenities ~ Electric Range / Oven")]
        KitchAmen_RangeOven,
        [Description("Kitchen Amenities ~ Gas Range / Oven")]
        KitchAmen_GasRangeOven,
        [Description("Kitchen Amenities ~ Island")]
        KitchAmen_Island,
        [Description("Landscape ~ Property has Landscape Watering System")]
        LandscapeWater_NoSprinklerSys,
        [Description("Landscape ~ Property does not have Landscape Watering System")]
        LandscapeWater_SprinklerSys,
        [Description("Outdoor Features ~ Built In BBQ")]
        Outdoor_BuiltInBbq,

        [Description("Complex Building Roofing ~ All Tile")]
        Roof_AllTile,
        [Description("Complex Building Roofing ~ Partial Tile")]
        Roof_PartialTile,
        [Description("Complex Building Roofing ~ Concrete")]
        Roof_Concrete,
        [Description("Complex Building Roofing ~ Rolled")]
        Roof_Rolled,
        [Description("Complex Building Roofing ~ Shingle")]
        Roof_Shingle,
        [Description("Complex Building Roofing ~ Metal")]
        Roof_Metal,
        [Description("Complex Building Roofing ~ Built-up")]
        Roof_BuiltUp,
        [Description("Complex Building Roofing ~ Foam")]
        Roof_Foam,
        [Description("Complex Building Roofing ~ Wood Shake")]
        Roof_WoodShake,
        [Description("Complex Building Roofing ~ Other")]
        Roof_Other,

        [Description("Complete Technology Features ~ High Speed Internet Available")]
        TechFeat_InternetHighSpeedAvail,
        [Description("Complete Technology Features ~ Wi-Fi Available")]
        TechFeat_WiFiAvail,
        [Description("Complete Technology Features ~ Network Wiring - One or More R")]
        TechFeat_NetworkWireingOneOrMore,
        [Description("Complete Technology Features ~ 3+ Existing Phone Lines")]
        TechFeat_PhoneLinesExistingThreeOrMore,
        [Description("Complete Technology Features ~ Satellite Dish Pre-wire")]
        TechFeat_SatelliteDishPreWire,
        [Description("Complete Technology Features ~ Cable TV Available")]
        TechFeat_CableTvAvail,
        [Description("Complete Technology Features ~ Individual Home Alarms")]
        TechFeat_IndividualHomeAlarms,
        [Description("Complete Technology Features ~ Other")]
        TechFeat_Other,

        [Description("Property Upgrades ~ Remodeled Last 24 Months")]
        Upgrades_Remodeled,
        [Description("Property Upgrades ~ Remodeled Last 60 Months")]
        UpgradesRemodeledLast60Months,
        [Description("Property Upgrades ~ Ceramic Tile Flooring")]
        Upgrades_CeramicTile,
        [Description("Property Upgrades ~ Wood Flooring")]
        Upgrades_WoodFloor,
        [Description("Property Upgrades ~ Stone Tile")]
        Upgrades_StoneTile,
        [Description("Property Upgrades ~ New plush carpet")]
        Upgrades_NewPlushCarpet,
        [Description("Property Upgrades ~ New doors")]
        Upgrades_NewDoors,
        [Description("Property Upgrades ~ Plumbing/Electrical fixtures")]
        Upgrades_NewFixtures,
        [Description("Property Upgrades ~ New Appliances")]
        UpgradesNewAppliances,
        [Description("Property Upgrades ~ Repaint of Complex Buildings")]
        UpgradesNewPaint,
        [Description("Property Upgrades ~ Granite Counter Tops")]
        UpgradesGraniteCounters,
        [Description("Property Upgrades ~ New HVAC System")]
        UpgradesNewHVACSystem,
        [Description("Property Upgrades ~ Upgrades New Landscaping")]
        UpgradesNewLandscaping,
        [Description("Property Upgrades ~ New Roofing")]
        UpgradesNewRoofing,
        [Description("Property Upgrades ~ Kitchen Cabinets")]
        UpgradesKitchenCabinets,
        [Description("Property Upgrades ~ Bathroom Cabinets")]
        UpgradesBathroomCabinets,
        [Description("Property Upgrades ~ Window Coverings")]
        UpgradesWindowCoverings,
        [Description("Property Upgrades ~ Ceiling Fans")]
        UpgradesCeilingFans,
        [Description("Property Upgrades ~ Kitchen Appliances")]
        UpgradesKitchenAppliances,
        [Description("Property Upgrades ~ Re-sealed/surfaced Parking Lot")]
        UpgradesResealedSurfacedParkingLot,
        [Description("Property Upgrades ~ Property/Interior Units Partially Updated Last 24 Months")]
        UpgradesPropertyInteriorUnitsPartiallyUpdated,
        [Description("Property Upgrades ~ Unit/Suite Doors")]
        UpgradesUnitSuiteDoors,
        [Description("Property Upgrades ~ Some Individual HVAC Units")]
        UpgradesSomeHVACUnits,
        [Description("Property Upgrades ~ All Individual HVAC Units")]
        UpgradesAllHVACUnits,
        [Description("Property Upgrades ~ Master Boiler System")]
        UpgradesBoilerSystem,
        [Description("Property Upgrades ~ Some Buildings Re-roofed")]
        UpgradesSomeReRoofed,
        [Description("Property Upgrades ~ All Buildings Re-roofed")]
        UpgradesAllReRoofed,
        [Description("Property Upgrades ~ Landscaping")]
        UpgradesLandscaping,
        [Description("Property Upgrades ~ Other")]
        UpgradesOther,

        [Description("Interior Features ~ Washer/Dryer Hook-ups: all units")]
        IntFeatures_WasherDryerHookUpsAllUnits,
        [Description("Interior Features ~ Washer/Dryer Hook-ups: some units")]
        IntFeatures_WasherDryerHookUpsSomeUnits,
        [Description("Interior Features ~ HVAC individual climate controlled units")]
        IntFeatures_HVACIndividualCCU, // climate controlled units
        [Description("Interior Features ~ Individual Unit Alarms")]
        IntFeatures_IndividualUnitAlarms,
        [Description("Interior Features ~ Valet Trash Removal")]
        IntFeatures_ValetTrashRemoval
    }
}
