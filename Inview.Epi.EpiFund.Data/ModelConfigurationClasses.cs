using Inview.Epi.EpiFund.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Data
{
    public class ModelConfigurationClasses
    {
        public class AssetImageConfiguration : EntityTypeConfiguration<AssetImage>
        {
            public AssetImageConfiguration()
            {
                //HasRequired(l => l.Asset).WithMany().HasForeignKey(l => l.AssetId);
            }
        }

        public class AssetDocumentConfiguration : EntityTypeConfiguration<AssetDocument>
        {
            public AssetDocumentConfiguration()
            {
                //HasRequired(l => l.Asset).WithMany().HasForeignKey(l => l.AssetId);
            }
        }
    }
}
