using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Domain.Entity
{
    public class SavedAssetSearch
    {
        public virtual Guid SavedAssetSearchId { get; set; }
        public virtual string Title { get; set; }
        public virtual string Json { get; set; }
        public virtual int UserId { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Updated { get; set; }
    }
}
