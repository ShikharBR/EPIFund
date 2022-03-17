using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.Enum
{
	public enum BathroomCount
	{
		[Description("0")]
		Zero = 1,
		[Description("1.0")]
		One = 2,
		[Description("1.5")]
		OneAndHalf = 3,
		[Description("2.0")]
		Two = 4,
		[Description("2.5")]
		TwoAndHalf = 5,
		[Description("3.0")]
		Three = 6,
		[Description("3.5")]
		ThreeAndHalf = 7,
		[Description("4.0")]
		Four = 8,
		[Description("4.5")]
		FourAndHalf = 9,
		[Description("5.0")]
		Five = 10,
		[Description("5.5")]
		FiveAndHalf = 11,
		[Description("6+")]
		Six = 12
	}
}