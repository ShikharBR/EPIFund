using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
    public class AssetVideo
    {
        public Inview.Epi.EpiFund.Domain.Entity.Asset Asset
        {
            get;
            set;
        }

        public Guid AssetId
        {
            get;
            set;
        }

        public int AssetVideoId
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string FilePath
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public int Index
        {
            get;
            set;
        }

        public AssetVideo()
        {
        }
    }
}