using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.Enum
{
	public enum PropertyCondition
	{
		Excellent = 1,
		[Description("Very Good")]
		VeryGood = 2,
		Good = 3,
		Average = 4,
		[Description("Below Average")]
		BelowAverage = 5
	}
}