using System;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
    public class AssetOwnershipChangeViewModel
    {
        public Guid AssetOwnershipChangeId { get; set; }
        public Guid AssetId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? AcquisitionDate { get; set; }
        public Guid OwnerHoldingCompanyId { get; set; }
        public Guid? PreviousOwnerHoldingCompanyId { get; set; }
        public double ClosingPrice { get; set; }
        public Enum.SellerTerms SellerTerms { get; set; }
        public double ProformaCapRate { get; set; }
    }
}
