using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
    public enum WasteWaterType
    {
        [Description("Public")]
        Public = 1,
        [Description("Septic")]
        Septic = 2
    }
}
