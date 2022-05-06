using Inview.Epi.EpiFund.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
    public class FavoriteGroupViewModel
    {
        public Guid FavoriteGroupId
        {
            get;
            set;
        }

        [Required]
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

        public int NumberOfFavoriteGroupAssets
        {
            get;
            set;
        }

        public bool FavoriteGroupNameExists
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

        public List<FavoriteGroupViewModel> FavoriteGroups
        {
            get;
            set;
        }
    }
}
