using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Domain.Enum
{
    public enum PricingDisplay
    {
		[Description("Submit Offer")]
		None = 0,

		[Description("LP")]
		Available = 1,

		[Description("CMC")]
		Pending = 2,

		[Description("If CMC")]
		SoldNotClosed = 3,

	}
}
