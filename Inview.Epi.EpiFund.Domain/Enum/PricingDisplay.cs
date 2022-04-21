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
		SubmitOffer = 1,

		[Description("LP")]
		Lp = 2,

		[Description("CMV")]
		Cmv = 3,

	}
}
