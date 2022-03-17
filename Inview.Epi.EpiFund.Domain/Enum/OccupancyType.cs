using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.Enum
{
	public enum OccupancyType
	{
		[Description("Commercial - Single Tenant")]
		CommercialSingleTenant = 1,
		[Description("Commercial - Multiple Tenants")]
		CommercialMultipleTenants = 2,
		[Description("Multi-Family")]
		MultiFamily = 3,
		Vacant = 4,
		Abandoned = 5,
		[Description("MHP – Multiple Park Tenants")]
		MHPMultiPark = 6,
		[Description("MHP – Mixed Use Tenant Profile")]
		MHPMixedTenant = 7
	}
}