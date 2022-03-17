using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.Enum
{
	public enum MaintenanceDetails
	{
		[Description("Suite Build Out")]
		SuiteBuildOut = -1,
		[Description("Lease Up Commissions")]
		LeaseUpCommissions = 0,
		[Description("Roofing")]
		Roofing = 1,
		[Description("Exterior Painting")]
		ExteriorPainting = 2,
		[Description("Interior Unit Painting")]
		InteriorUnitPainting = 3,
		[Description("Master HVAC System")]
		MasterHvacSystem = 4,
		[Description("Indiviual Unit HVAC's")]
		IndividualHvac = 5,
		[Description("Indiviual Unit Kitchen Appliances")]
		IndividualAppliances = 6,
		[Description("Indiviual Unit W/D Package")]
		IndividualWasherDryer = 7,
		[Description("Indiviual Unit Flooring")]
		IndividualFlooring = 8,
		[Description("Indiviual Unit Cabinet's & Fixtures")]
		IndividualCabinetAndFixtures = 9,
		[Description("Covered Parking")]
		CoveredParking = 10,
		[Description("Exterior Fencing")]
		Fencing = 11,
		Landscaping = 12,
		[Description("Fire Damage")]
		FireDamage = 13,
		[Description("Flood Damage")]
		FloodDamage = 14,
		[Description("Other")]
		Other = 15,
		[Description("Building Exterior Renovations")]
		ExteriorRenovations = 16,
		[Description("Covered Parking Installation")]
		CoveredParkingInstall = 17,
		[Description("Covered Parking Structure")]
		CoveredParkingStructure = 18,
		[Description("Exterior Lighting")]
		Lighting = 19,
		[Description("Parking Lot")]
		ParkingLot = 20,
		[Description("Other")]
		Other2 = 21,
		[Description("Pavement Repair ")]
		PavementRepair = 22,
		[Description("Exterior Lighting ")]
		ExteriorLighting = 23,
		[Description("Park Owned Repairs ")]
		ParkOwnedRepairs = 24
	}
}