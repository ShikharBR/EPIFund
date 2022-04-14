using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.Enum
{
	public enum ListingStatus
	{
		[Description("NoStatus")]
		None = 0,

		[Description("Available")]
		Available = 1,

		[Description("Pending")]
		Pending = 2,

		[Description("Sold Not Closed")]
		SoldNotClosed = 3,

		[Description("Sold And Closed")]
		SoldAndClosed = 4,

		[Description("Withdrawn")]
		Withdrawn = 5
	}
}