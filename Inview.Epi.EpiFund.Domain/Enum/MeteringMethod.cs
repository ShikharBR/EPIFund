using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.Enum
{
	public enum MeteringMethod
	{
		[Description("N/A")]
		NotAvailable = 1,
		[Description("Individual Meters")]
		Individual = 2,
		[Description("Master Meter")]
		Master = 3
	}
}