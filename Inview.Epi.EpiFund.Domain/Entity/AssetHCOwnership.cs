using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
    public class AssetHCOwnership
    {
        public Inview.Epi.EpiFund.Domain.Entity.Asset Asset
        {
            get;
            set;
        }
        public Inview.Epi.EpiFund.Domain.Entity.HoldingCompany HoldingCompany
        {
            get;
            set;
        }

        //[AssetHCOwnershipId], [AssetId], [OwnerHoldingCompanyId], [CreateDate],
        //[Terms], [ActualClosingDate], [SalesPrice], [SalesPriceNotProvided],
        //[CalculatedPPU], [CalculatedPPSqFt], [CashInvestmentApy], [TermsOther]
               
        public int AssetHCOwnershipId
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

        [ForeignKey("HoldingCompany")]
        public Guid? HoldingCompanyId
        {
            get;
            set;
        }
        public DateTime? CreateDate
        {
            get;
            set;
        }
        public int? Terms
        {
            get;
            set;
        }
        public DateTime? ActualClosingDate
        {
            get;
            set;
        }
        public double? SalesPrice
        {
            get;
            set;
        }
        public bool? SalesPriceNotProvided
        {
            get;
            set;
        }
        public double? CalculatedPPU
        {
            get;
            set;
        }
        public double? CalculatedPPSqFt
        {
            get;
            set;
        }
        public double? CashInvestmentApy
        {
            get;
            set;
        }
        public string TermsOther
        {
            get;
            set;
        }

        public AssetHCOwnership()
        {
        }
    }
}