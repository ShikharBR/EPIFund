using System;

namespace Inview.Epi.EpiFund.Domain.Entity
{
    public class HoldingCompany
    {
        public virtual Guid HoldingCompanyId { get; set; }
        public virtual Guid OperatingCompanyId { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string Zip { get; set; }
        public virtual string Country { get; set; }
        public virtual string WorkNumber { get; set; }
        public virtual string CellNumber { get; set; }
        public virtual string FaxNumber { get; set; }
        public virtual bool IsActive { get; set; }
    }
}
