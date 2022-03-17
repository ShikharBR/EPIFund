using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.Enum
{
	public enum CommercialPropertyDetails
	{
		[Description("Light Rail access")]
		GenFeatures_LightRailAccess,
		[Description("Subway access")]
		GenFeatures_SubwayAccess,
		[Description("1 or more elevators")]
		GenFeatures_Elevators,
		[Description("1 or more escalators")]
		GenFeatures_Escalators,
		[Description("Building(s) have Interior Sprinkler Systems")]
		GenFeatures_InteriorSprinklers,
		[Description("Valet Trash Removal for Tenants")]
		GenFeatures_ValetTrashRemovalForTenants,
		[Description("Single Level Buildings")]
		Arch_SingleStory,
		[Description("Three Story Buildings")]
		Arch_ThreeStory,
		[Description("Two Story Buildings")]
		Arch_TwoStory,
		[Description("Four + Story Buildings")]
		Arch_FourPlusStoryBuildings,
		[Description("Fitness Center")]
		Complex_WorkoutFacility,
		[Description("Lake Views/Front")]
		Complex_LakeView,
		[Description("Ocean Views/Front")]
		Complex_OceanViews,
		[Description("Golf Course Views/Front")]
		Complex_GolfView,
		[Description("Special Golf Membership")]
		Complex_SpecialGolfMembership,
		[Description("Business Center")]
		Complex_BusinessCenter,
		[Description("Landscape Watering System")]
		Complex_LandscapeWateringSystem,
		[Description("RV Parking Facility")]
		Complex_RVParkingFacility,
		[Description("Common Area Water Features")]
		Complex_CommonAreaWater,
		[Description("Perimeter Fencing")]
		Complex_PerimeterFence,
		[Description("Child Care Facility")]
		Complex_ChildCareFacility,
		[Description("Children's Playground")]
		Complex_ChildrenPlayground,
		[Description("Community Pet Park")]
		Complex_PetPark,
		[Description("Manager's Office")]
		Complex_ManagerOffice,
		[Description("Clubhouse")]
		Complex_Clubhouse,
		[Description("Volleyball Court")]
		Complex_VolleyballCourt,
		[Description("Tennis/Sport Court")]
		Complex_SportCourt,
		[Description("Community BBQ Facilities")]
		Complex_CommunityBBQFacility,
		[Description("One Community Pool")]
		Complex_OneCommunityPool,
		[Description("Two or more Community Pools")]
		Complex_TwoCommunityPool,
		[Description("One Community Jacuzzi")]
		Complex_OneCommunity,
		[Description("Two or more Community Jacuzzi")]
		Complex_TwoCommunity,
		[Description("1 or more Community Laundry Facilities")]
		Complex_LaundryFacility,
		[Description("Child Care Facilities")]
		Complex_ChildCare,
		[Description("Security Gate Entrance")]
		Security_Gate,
		[Description("Exterior Video Surveillance")]
		Security_VideoSurveillance,
		[Description("Internal Common Area Video Surveillance")]
		Security_IntVideoSurveillance,
		[Description("Individual Unit/Suite Security System")]
		Security_IndSecuritySystem,
		[Description("On Duty Security Guard")]
		Security_OnDutyGuard,
		[Description("Card/Code Building Access")]
		Security_CodeAccess,
		[Description("No Complex Open Spaces")]
		Park_None,
		[Description("Shared w/Adjacent Property")]
		Park_Shared,
		[Description("Dedicated Tenant Spaces")]
		Park_Dedicated,
		[Description("Metered/Fee Based")]
		Park_MeteredFeeBased,
		[Description("Some Covered Stalls Provided")]
		Park_Covered,
		[Description("No Covered Spaces")]
		Park_NoCoveredSpace,
		[Description("Above or Underground Garage included with Property")]
		Park_AboveOrBelowPark,
		[Description("Neighboring Parking Garage Agreement")]
		Park_NeighboringPark,
		[Description("Street Parking only")]
		Park_StreetParkOnly,
		[Description("Some Street Parking")]
		Park_SomeStreetPark,
		[Description("Lighted Parking Areas")]
		Park_LightedParking,
		[Description("1 Covered Per Unit")]
		Park_OneCoveredPerUnit,
		[Description("Wood Framing")]
		Const_WoodFrame,
		[Description("Brick")]
		Const_Brick,
		[Description("Steel/Metal & Wood")]
		Const_MetalAndWood,
		[Description("Steel/Metal & Plexiglass")]
		Const_MetalAndGlass,
		[Description("Cement Block")]
		Const_CementBlock,
		[Description("Adobe")]
		Const_Adobe,
		[Description("Slump Block")]
		Const_SlumpBlock,
		[Description("Brick Veneer")]
		Const_FinishBrickVeneer,
		[Description("Painted")]
		Const_FinishPainted,
		[Description("Stone")]
		Const_FinishStone,
		[Description("Siding")]
		Const_FinishSiding,
		[Description("Stucco")]
		Const_FinishStucco,
		[Description("Other")]
		Const_FinishOther,
		[Description("Evaporative")]
		Cool_Evap,
		[Description("Refrigeration")]
		Cool_Ref,
		[Description("Both Ref & Evap")]
		Cool_BothRefAndEvap,
		[Description("Window/Wall Units")]
		Cool_WindowWallUnits,
		[Description("No Cooling Facilities")]
		Cool_None,
		[Description("Ceiling Fans")]
		Cool_CeilingFans,
		[Description("Sunscreens")]
		Cool_Sunscreens,
		[Description("HVAC individual climate controlled units")]
		Cool_HVACIndividual,
		[Description("No Heating Facilities")]
		Heat_None,
		[Description("Gas")]
		Heat_Gas,
		[Description("Fireplace")]
		Heat_Fireplace,
		[Description("Heat Pump")]
		Heat_HeatPump,
		[Description("VAV System")]
		Heat_VAVSystem,
		[Description("Units Updated within 12 Months")]
		UnitExt_UpdatedIn12Months,
		[Description("Units Updated within 24 Months")]
		UnitExt_UpdatedIn24Months,
		[Description("Units Updated within 3-5 Years")]
		UnitExt_UpdatedIn3Years,
		[Description("All Units Updated within 12 Months")]
		UnitInt_UpdatedIn12Months,
		[Description("Some Units Updated within 12 Months")]
		UnitInt_SomeUpdatedIn12Months,
		[Description("All Units Updated within 24 Months")]
		UnitInt_UpdatedIn24Months,
		[Description("Some Units Updated within 24 Months")]
		UnitInt_SomeUpdatedIn24Months,
		[Description("All Units Updated within 3-5 Years")]
		UnitInt_UpdatedIn3Years,
		[Description("Some Units Updated within 3-5 Years")]
		UnitInt_SomeUpdatedIn3Years,
		[Description("Partial Tile")]
		Roof_PartialTile,
		[Description("Concrete")]
		Roof_Concrete,
		[Description("Shingle")]
		Roof_Shingle,
		[Description("Metal")]
		Roof_Metal,
		[Description("Flat/Built-up Foam")]
		Roof_BuiltUpFoam,
		[Description("Flat/Built-up Other")]
		Roof_BuiltUpOther,
		[Description("Wood Shake")]
		Roof_WoodShake,
		[Description("Other")]
		Roof_Other,
		[Description("All Tile")]
		Roof_AllTile,
		[Description("All units have patios or balconies")]
		IntFeatures_AllHavePatios,
		[Description("Some units have patios or balconies")]
		IntFeatures_SomeHavePatios,
		[Description("All 2Bed+ Units have 2 Bathrooms")]
		IntFeatures_All2BedHave2Bath,
		[Description("Some units have walk-in closets")]
		IntFeatures_SomeHaveWalkinClosets,
		[Description("All units have walk-in closets")]
		IntFeatures_AllHaveWalkinClosets,
		[Description("Washer/Dryer Hook-ups: all units")]
		IntFeatures_WasherDryerHookUpsAllUnits,
		[Description("Washer/Dryer Hook-ups: some units")]
		IntFeatures_WasherDryerHookUpsSomeUnits,
		[Description("Washer/Dryer in all units")]
		IntFeatures_AllWasherDryer,
		[Description("Washer/Dryer in some units")]
		IntFeatures_SomeWasherDryer,
		[Description("Trash Compactor")]
		KitchAmen_TrashCompactor,
		[Description("Breakfast Bar")]
		KitchAmen_BreakfastBar,
		[Description("Refrigerator")]
		KitchAmen_Refrig,
		[Description("All with Built-in Microwaves")]
		KitchAmen_AllHaveMicrowaves,
		[Description("Some Built-in Microwaves")]
		KitchAmen_SomeHaveMicrowaves,
		[Description("All units with Dishwasher")]
		KitchAmen_AllDishwasher,
		[Description("Some units with Dishwasher")]
		KitchAmen_SomeDishwasher,
		[Description("Disposal")]
		KitchAmen_Disposal,
		[Description("Storage Pantry")]
		KitchAmen_Pantry,
		[Description(" Dine-in Kitchen Floorplan")]
		KitchAmen_EatInKitch,
		[Description("Electric Range / Oven")]
		KitchAmen_ElectricRangeOven,
		[Description("Gas Range / Oven")]
		KitchAmen_GasRangeOven,
		[Description("Island")]
		KitchAmen_Island,
		[Description("Ceramic Tile Flooring")]
		Upgrades_CeramicTile,
		[Description("Wood Flooring")]
		Upgrades_WoodFloor,
		[Description("Stone Tile")]
		Upgrades_StoneTile,
		[Description("New plush carpet")]
		Upgrades_NewPlushCarpet,
		[Description("New doors")]
		Upgrades_NewDoors,
		[Description("Plumbing/Electrical fixtures")]
		Upgrades_NewFixtures,
		[Description("Granite Counter Tops")]
		Upgrades_GraniteCounters,
		[Description("Kitchen/Bathroom Cabinets")]
		Upgrades_KitchenCabinets,
		[Description("Kitchen Appliances")]
		Upgrades_KitchenAppliances,
		[Description("Repaint of Complex Buildings")]
		UpgradesExt_NewPaint,
		[Description("Unit/Suite Doors")]
		UpgradesExt_UnitSuiteDoors,
		[Description("Master Boiler System")]
		UpgradesExt_BoilerSystem,
		[Description("Some Individual HVAC Units")]
		UpgradesExt_SomeHVACUnits,
		[Description("All Individual HVAC Units")]
		UpgradesExt_AllHVACUnits,
		[Description("Re-sealed/surfaced Parking Lot")]
		UpgradesExt_ResealedSurfacedParkingLot,
		[Description("Some Buildings Re-roofed")]
		UpgradesExt_SomeReRoofed,
		[Description("All Buildings Re-roofed")]
		UpgradesExt_AllReRoofed,
		[Description("Landscaping")]
		UpgradesExt_Landscaping,
		[Description("Other")]
		UpgradesExt_Other,
		[Description("High Speed Internet Available")]
		TechFeat_InternetHighSpeedAvail,
		[Description("Wi-Fi Available")]
		TechFeat_WiFiAvail,
		[Description("Satellite Dish Pre-wire")]
		TechFeat_SatelliteDishPreWire,
		[Description("Cable TV Available")]
		TechFeat_CableTvAvail,
		[Description("Other")]
		TechFeat_Other
	}
}