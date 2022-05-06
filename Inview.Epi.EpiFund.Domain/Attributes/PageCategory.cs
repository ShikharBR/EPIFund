using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Domain.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property)
]
    public class PageCategory : System.Attribute
    {
        public readonly Domain.ViewModel.PageCategory Category;

        public PageCategory(Domain.ViewModel.PageCategory category)
        {
            Category = category;
        }
    }
}
