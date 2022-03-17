using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
    public enum PropHoldType
    {
        [Description("Fee Simple Title")]
        FeeSimple = 1,
        [Description("Leasehold Title")]
        Leasehold = 2
    }
}
