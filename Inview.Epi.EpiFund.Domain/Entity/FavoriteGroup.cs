using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Domain.Entity
{
    public class FavoriteGroup
    {
        public Guid FavoriteGroupId
        {
            get;
            set;
        }

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

        public AssetImage FavoriteGroupImage
        {
            get;
            set;
        }

        public Guid FavoriteGroupImageAssetId
        {
            get;
            set;
        }

        public int NumberOfFavoriteGroupAssets
        {
            get;
            set;
        }

        public int UserId
        {
            get;
            set;
        }

        public FavoriteGroup()
        {
        }
    }
}
