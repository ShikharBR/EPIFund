using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
    public class FavoriteGroupAssetsViewModel
    {
        public string FavoriteGroupName
        {
            get;
            set;
        }

        public string FavoriteGroupDescription
        {
            get;
            set;
        }

        public Guid FavoriteGroupId
        {
            get;
            set;
        }

        public List<AssetViewModel> FavoriteGroupAssets
        {
            get;
            set;
        }

        public List<FavoriteGroupAssetsQuickListModel> FavoriteGroupAssetsQuickList
        {
            get;
            set;
        }

        public List<AssetDynamicAssetViewModel> FavoriteGroupAssetsDynamicViewModel
        {
            get; set;
        }

        public int Total
        {
            get;
            set;
        }

        public int PublishedAssets
        {
            get;
            set;
        }

        public long TotalAssetVal
        {
            get;
            set;
        }

        public int MultiFamUnits
        {
            get;
            set;
        }

        public int TotalSqFt
        {
            get;
            set;
        }

    }
}
