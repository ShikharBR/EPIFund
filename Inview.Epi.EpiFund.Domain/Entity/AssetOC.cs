using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
    public class AssetOC
    {
        public Inview.Epi.EpiFund.Domain.Entity.Asset Asset
        {
            get;
            set;
        }
        public Inview.Epi.EpiFund.Domain.Entity.OperatingCompany OperatingCompany
        {
            get;
            set;
        }
               
        public int AssetOCId
        {
            get;
            set;
        }
        
        [ForeignKey("Asset")]
        public Guid? AssetId
        {
            get;
            set;
        }

        [ForeignKey("OperatingCompany")]
        public Guid? OperatingCompanyId
        {
            get;
            set;
        }

        public DateTime? CreateDate
        {
            get;
            set;
        }
       
        public AssetOC()
        {
        }
    }
}