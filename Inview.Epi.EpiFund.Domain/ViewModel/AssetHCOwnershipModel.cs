using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AssetHCOwnershipModel
	{
        public int AssetHCOwnershipId
        {
            get;
            set;
        }
        public Guid? AssetId
        {
            get;
            set;
        }       
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

        public string HCName {
            get;
            set;
        }

        public AssetHCOwnershipModel()
        {
        }

	}
}