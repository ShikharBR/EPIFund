using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
    public enum WaterServType
    {
        [Description("Private")]
        Private = 1,
        [Description("Public")]
        Public = 2,
        [Description("On-Site")]
        OnSite = 3
    }
}
