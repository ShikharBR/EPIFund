using System;

namespace Inview.Epi.EpiFund.Domain.Entity
{
    public class AssetOwnershipChange
    {
        public virtual Guid AssetOwnershipChangeId { get; set; }
        public virtual Guid AssetId { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual DateTime? AcquisitionDate { get; set; }
        public virtual Guid OwnerHoldingCompanyId { get; set; }
        public virtual Guid? PreviousOwnerHoldingCompanyId { get; set; }
        public virtual double ClosingPrice { get; set; }
        public virtual Enum.SellerTerms SellerTerms { get; set; }
        public virtual double ProformaCapRate { get; set; }
    }
}
