using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public enum AssetType
	{
		[Description("Retail Tenant Property")]
		Retail = 1,
		[Description("Office Tenant Property")]
		Office = 2,
		[Description("Multi-Family")]
		MultiFamily = 3,
		[Description("Industrial Tenant Property")]
		Industrial = 4,
		[Description("MHP")]
		MHP = 5,
		[Description("Fuel Service Retail Property")]
		ConvenienceStoreFuel = 6,
		[Description("Medical Tenant Property")]
		Medical = 7,
		[Description("Mixed Use Commercial Property")]
		MixedUse = 8,
		[Description("Other")]
		Other = 10,
		[Description("Resort/Hotel/Motel Property")]
		Hotel = 11,
		[Description("Single Tenant Property (All Type)")]
		SingleTenantProperty = 12,
		[Description("Fractured Condominium Portfolio's")]
		FracturedCondominiumPortfolio = 13,
		[Description("Mini-Storage Property")]
		MiniStorageProperty = 14,
		[Description("Parking Garage Property")]
		ParkingGarageProperty = 15,
		[Description("Secured CRE Paper")]
		SecuredPaper = 16
	}
}