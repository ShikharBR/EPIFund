using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.Enum
{
	public enum CommercialType
	{
		[Description("Convenience Store No Gas")]
		Convenience_NoGas = 1,

		[Description("Convenience Store With Gas")]
		Convenience_WithGas = 2,

		[Description("Gas Station")]
		GasStation = 3,

		[Description("Industrial Office")]
		IndustrialOffice = 4,

		[Description("Medical Office")]
		MedicalOffice = 5,

		[Description("Office Building")]
		OfficeBuilding = 6,

		[Description("Retail Center")]
		RetailCenter = 7,

		[Description("Single Tenant")]
		SingleTenant = 8,

		Warehouse = 9,

		Other = 10,

		[Description("Hotel/Motel")]
		Hotel = 11
	}
}