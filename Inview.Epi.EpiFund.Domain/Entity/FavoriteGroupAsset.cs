using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Domain.Entity
{
    public class FavoriteGroupAsset
    {
        public Asset Asset
        {
            get;
            set;
        }

        public Guid AssetId
        {
            get;
            set;
        }

        public FavoriteGroup FavoriteGroup
        {
            get;
            set;
        }

        public Guid FavoriteGroupAssetId
        {
            get;
            set;
        }

        public Guid FavoriteGroupId
        {
            get;
            set;
        }

        public double PriceWhenAdded
        {
            get;
            set;
        }

        public FavoriteGroupAsset()
        {
        }
    }
}
