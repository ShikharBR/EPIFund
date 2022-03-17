using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.Enum
{
	public enum RetailPropertyType
	{
		Condo,
		Patio,
		SFR,
		[Description("Town Home")]
		TownHome
	}
}