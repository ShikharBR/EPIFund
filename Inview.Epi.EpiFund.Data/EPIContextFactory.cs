using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inview.Epi.EpiFund.Domain;

namespace Inview.Epi.EpiFund.Data
{
    public class EPIContextFactory : IEPIContextFactory
    {
        public IEPIRepository Create()
        {
            var context = new EPIRepository();
            return context;
        }
    }
}
