using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.Enum
{
	public enum BedroomCount
	{
		[Description("Studio")]
		Zero = 1,
		[Description("1")]
		One = 2,
		[Description("2")]
		Two = 3,
		[Description("3")]
		Three = 4,
		[Description("4")]
		Four = 5,
		[Description("5")]
		Five = 6,
		[Description("6")]
		Six = 7,
		[Description("7+")]
		SevenPlus = 8
	}
}