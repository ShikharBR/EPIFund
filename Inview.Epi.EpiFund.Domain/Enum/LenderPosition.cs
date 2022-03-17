using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.Enum
{
	public enum LenderPosition
	{
		[Description("1st")]
		First = 1,
		[Description("2nd")]
		Second = 2,
		[Description("3rd")]
		Third = 3,
		[Description("Other")]
		Other = 4
	}
}