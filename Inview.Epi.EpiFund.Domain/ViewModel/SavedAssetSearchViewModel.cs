using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
    public class SavedAssetSearchViewModel
    {
        public Guid SavedAssetSearchId { get; set; }
        public string Title { get; set; }
        public string Json { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public List<SavedAssetSearchViewModel> SavedSearches { get; set; }
    }
}
