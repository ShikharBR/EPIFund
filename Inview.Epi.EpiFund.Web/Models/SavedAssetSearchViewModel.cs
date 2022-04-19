using System.Collections.Generic;
using System;

namespace Inview.Epi.EpiFund.Web.Models
{
    public class SavedAssetSearchViewModel {
        public Guid SavedAssetSearchId { get; set; }
        public string Title { get; set; }
        public string Json { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public List<SavedAssetSearchViewModel> SavedSearches { get; set; }
    }
}
