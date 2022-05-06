using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
    public class GeoLocationResponse
    {
        public List<results> results { get; set; }
    }

    public class results
    {
        public geometry geometry { get; set; }

    }

    public class geometry
    {
        public location location { get; set; }
    }

    public class location
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }
}
