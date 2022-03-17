using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Domain.Enum
{
    public enum MasterCommercialPropertyDetails
    {
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

        [Description("Architecture ~ Single Level Buildings")]
        Arch_SingleStory,
        [Description("Architecture ~ Three Story Buildings")]
        Arch_ThreeStory,
        [Description("Architecture ~ Townhouse Style Buildings")]
        Arch_TownhouseStyle,
        [Description("Architecture ~ Two Story Buildings")]
        Arch_TwoStory,
        [Description("Architecture ~ Some units have patios or balconies")]
        Arch_SomeHavePatios,
        [Description("Architecture ~ Four + Story Buildings")]
        Arch_FourPlusStoryBuildings,

        [Description("Building Features ~ Fitness Center")]
        Complex_WorkoutFacility,
        [Description("Building Features ~ Lake Views")]
        Complex_LakeView,
        [Description("Building Features ~ Lake Front")]
        Complex_LakeFront,
        [Description("Building Features ~ Ocean Views")]
        Complex_OceanViews,
        [Description("Building Features ~ Ocean Front")]
        Complex_OceanFront,
        [Description("Building Features ~ Golf Course Views")]
        Complex_GolfView,
        [Description("Building Features ~ Golf Course Front")]
        Complex_GolfFront,
        [Description("Building Features ~ Special Golf Membership")]
        Complex_SpecialGolfMembership,
        [Description("Building Features ~ Child Care Facility")] 
        Complex_ChildCareFacility,  
        [Description("Building Features ~ Business Center")]
        Complex_BusinessCenter,
        [Description("Building Features ~ Landscape Watering System")]
        Complex_LandscapeWateringSystem,
        [Description("Building Features ~ Security Gate Entrance")] 
        Security_Gate,
        [Description("Building Features ~ Exterior Video Security Video Surveillance")]
        Security_VideoSurveillance, 
        [Description("Building Features ~ Internal Common Area Video Surveillance")]
        Security_IntVideoSurveillance,
        [Description("Building Features ~ Individual Unit/Suite Security System")]
        Security_IndSecuritySystem,

        [Description("Building Features ~ One or More Elevators")]
        Security_OneOrMoreElevators,
        [Description("Building Features ~ One or More Escalators")]
        Security_OneOrMoreEscalators,
        [Description("Building Features ~ Child Care Facilities")]
        Security_ChildCare,
        [Description("Building Features ~ Equipped with Sprinkler System")]
        Security_EquippedWithSprinklerSystem,
        [Description("Building Features ~ Equipped with Security Alarm System")]
        Security_EquippedWithAlarmSystem,

        [Description("Heating & Cooling Systems ~ VAV System")]
        HeatAndCool_VAV,

        [Description("Parking ~ None")]
        Park_None,
        [Description("Parking ~ Shared")]
        Park_Shared,
        [Description("Parking ~ Dedicated")]
        Park_Dedicated,
        [Description("Parking ~ Metered/Fee Based")]
        Park_MeteredFeeBased,
        [Description("Parking ~ Some Covered Stalls Provided")]
        Park_Covered,
        [Description("Parking ~ Metered/Fee")]
        Park_Fee,
        [Description("Parking ~ No Covered Spaces")]
        Park_NoCoveredSpace,
        [Description("Parking ~ Above or Underground Garage included with Property")]
        Park_AboveOrBelowPark,
        [Description("Parking ~ Neighboring Parking Garage Agreement")]
        Park_NeighboringPark,
        [Description("Parking ~ Street Parking only")]
        Park_StreetParkOnly,
        [Description("Parking ~ Some Street Parking")]
        Park_SomeStreetPark,
        [Description("Parking ~ Lighted Parking Areas")]
        Park_LightedParking,
        [Description("Parking ~ 1 Covered Per Unit")]
        Park_OneCoveredPerUnit,
        [Description("Parking ~ Above Ground Detached Parking Garage")]
        Park_AboveGroundDetached,
        [Description("Parking ~ Below Ground Parking Garage")]
        Park_BelowGroundGarage,
        [Description("Parking ~ Parking Garage is Part of Commercial Building")]
        Park_ParkingGaragePartOfCommercialBuilding,

        [Description("Construction ~ Wood Framing")]
        Const_WoodFrame,
        [Description("Construction ~ Brick")]
        Const_Brick,
        [Description("Construction ~ Steel/Metal & Wood")]
        Const_MetalAndWood,
        [Description("Construction ~ Steel/Metal & Plexiglass")]
        Const_MetalAndGlass,
        [Description("Construction ~ Cement Block")]
        Const_CementBlock,
        [Description("Construction ~ Adobe")]
        Const_Adobe,
        [Description("Construction ~ Slump Block")]
        Const_SlumpBlock,
        [Description("Construction ~ Steel/Metal Framing")]
        Const_SteelMetalFraming,
        [Description("Construction ~ Plexiglass")]
        Const_Plexiglass,

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
        [Description("Cooling ~ VAV System")]
        Cool_VAVSystem,
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
        [Description("Heating ~ VAV System")]
        Heat_VAVSystem,

        [Description("Unit Exteriors ~ Units Updated within 12 Months")]
        UnitExt_UpdatedIn12Months,
        [Description("Unit Exteriors ~ Units Updated within 24 Months")]
        UnitExt_UpdatedIn24Months,
        [Description("Unit Exteriors ~ Units Updated within 3-5 Years")]
        UnitExt_UpdatedIn3Years,
        [Description("Unit Interiors ~ All Units Updated within 12 Months")]
        UnitInt_UpdatedIn12Months,
        [Description("Unit Interiors ~ Some Units Updated within 12 Months")]
        UnitInt_SomeUpdatedIn12Months,
        [Description("Unit Interiors ~ All Units Updated within 24 Months")]
        UnitInt_UpdatedIn24Months,
        [Description("Unit Interiors ~ Some Units Updated within 24 Months")]
        UnitInt_SomeUpdatedIn24Months,
        [Description("Unit Interiors ~ All Units Updated within 3-5 Years")]
        UnitInt_UpdatedIn3Years,
        [Description("Unit Interiors ~ Some Units Updated within 3-5 Years")]
        UnitInt_SomeUpdatedIn3Years,

        [Description("Building Roofing ~ Partial Tile")]
        Roof_PartialTile,
        [Description("Building Roofing ~ Concrete")]
        Roof_Concrete,
        [Description("Building Roofing ~ Rolled")]
        Roof_Rolled,
        [Description("Building Roofing ~ Shingle")]
        Roof_Shingle,
        [Description("Building Roofing ~ Metal")]
        Roof_Metal,
        [Description("Complex Building Roofing ~ Built-up")]
        Roof_BuiltUp,
        [Description("Complex Building Roofing ~ Foam")]
        Roof_Foam,
        [Description("Building Roofing ~ Flat/Built-up Foam")]
        Roof_BuiltUpFoam,
        [Description("Building Roofing ~ Flat/Built-up Other")]
        Roof_BuiltUpOther,
        [Description("Building Roofing ~ Wood Shake")]
        Roof_WoodShake,
        [Description("Building Roofing ~ Other")]
        Roof_Other,
        [Description("Building Roofing ~ All Tile")]
        Roof_AllTile,


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
        [Description("Property Upgrades ~ Granite Counter Tops")]
        UpgradesGraniteCounters,
        [Description("Property Upgrades ~ Kitchen Cabinets")]
        UpgradesKitchenCabinets,
        [Description("Property Upgrades ~ Bathroom Cabinets")]
        UpgradesBathroomCabinets,
        [Description("Property Upgrades ~ Kitchen Appliances")]
        UpgradesKitchenAppliances,
        [Description("Property Upgrades ~ Repaint of Complex Buildings")]
        UpgradesNewPaint,
        [Description("Property Upgrades ~ Unit/Suite Doors")]
        Upgrades_UnitSuiteDoors,
        [Description("Property Upgrades ~ Some Individual HVAC Units")]
        UpgradesSomeHVACUnits,
        [Description("Property Upgrades ~ All Individual HVAC Units")]
        UpgradesAllHVACUnits,
        [Description("Property Upgrades ~ Re-sealed/surfaced Parking Lot")]
        UpgradesResealedSurfacedParkingLot,
        [Description("Property Upgrades ~ Some Buildings Re-roofed")]
        UpgradesSomeReRoofed,
        [Description("Property Upgrades ~ All Buildings Re-roofed")]
        UpgradesAllReRoofed,
        [Description("Property Upgrades ~ Landscaping")]
        UpgradesLandscaping,
        [Description("Property Upgrades ~ Other")]
        UpgradesOther,

        [Description("Complete Technology Features ~ High Speed Internet Available")]
        TechFeat_InternetHighSpeedAvail,
        [Description("Complete Technology Features ~ Wi-Fi Available")]
        TechFeat_WiFiAvail,
        [Description("Complete Technology Features ~ Network Wiring - One or More")]
        TechFeat_NetworkWireingOneOrMore,
        [Description("Complete Technology Features ~ 3+ Existing Phone Lines")]
        TechFeat_PhoneLinesExistingThreeOrMore,
        [Description("Complete Technology Features ~ Satellite Dish Pre-wire")]
        TechFeat_SatelliteDishPreWire,
        [Description("Complete Technology Features ~ Cable TV Available")]
        TechFeat_CableTvAvail,
        [Description("Complete Technology Features ~ Other")]
        TechFeat_Other,

        // instructions: (insert @ bottom of Asset Detail list)
        [Description("General Features ~ Perimeter Fencing")]
        GenFeatures_PerimeterFence,

    }
}
