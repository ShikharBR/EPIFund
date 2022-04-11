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
        public string Terms
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
        public double? CapRate { get;set;}

        public string OwnerHoldingCompany
        {
            get;
            set;
        }

        public bool OwnerISRA
        {
            get;
            set;
        }
        public string OwnerHoldingCompanyEmail
        {
            get;
            set;
        }
        public string OwnerHoldingCompanyFirst
        {
            get;
            set;
        }
        public string OwnerHoldingCompanyLast
        {
            get;
            set;
        }
        public string OwnerHoldingCompanyAddressLine1
        {
            get;
            set;
        }
        public string OwnerHoldingCompanyAddressLine2
        {
            get;
            set;
        }
        public string OwnerHoldingCompanyCity
        {
            get;
            set;
        }
        public string OwnerHoldingCompanyState
        {
            get;
            set;
        }
        public string OwnerHoldingCompanyZip
        {
            get;
            set;
        }
        public string OwnerHoldingCompanyCountry
        {
            get;
            set;
        }
        public string OwnerHoldingCompanyWorkPhone
        {
            get;
            set;
        }
        public string OwnerHoldingCompanyCellPhone
        {
            get;
            set;
        }

        //public string OwnerHoldingCompanyFax{get;set;}
        public string OwnerHoldingCompanyLinkedIn { get; set; }
        public string OwnerHoldingCompanyFacebook { get; set; }
        public string OwnerHoldingCompanyInstagram { get; set; }
        public string OwnerHoldingCompanyTwitter { get; set; }


        public AssetHCOwnershipModel()
        {
        }

    }

    public class AssetOCModel
    {
        public string OwnerOperatingCompany
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }
        public string LastName
        {
            get;
            set;
        }

        public string FullName
        {
            get;
            set;
        }
        public string Email
        {
            get;
            set;
        }

        public string AddressLine1
        {
            get;
            set;
        }
        public string AddressLine2
        {
            get;
            set;
        }

        public string City
        {
            get;
            set;
        }
        public string State
        {
            get;
            set;
        }
        public string Zip
        {
            get;
            set;
        }

        public string Country
        {
            get;
            set;
        }
        public string WorkNumber
        {
            get;
            set;
        }
        public string CellNumber
        {
            get;
            set;
        }

        //public string FaxNumber{get;set;}

        public string LinkedIn { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }

        public int AssetOCId
        {
            get;
            set;
        }
        public Guid? AssetId
        {
            get;
            set;
        }
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
        public AssetOCModel()
        {
        }

    }

    public class AssetOCAddressModel
    {
        public string AddressLine1
        {
            get;
            set;
        }
        public string AddressLine2
        {
            get;
            set;
        }

        public string City
        {
            get;
            set;
        }
        public string State
        {
            get;
            set;
        }
        public string Zip
        {
            get;
            set;
        }

        public string Country
        {
            get;
            set;
        }
        public string WorkNumber
        {
            get;
            set;
        }
        public string CellNumber
        {
            get;
            set;
        }

        //public string FaxNumber{get;set;}
        public string LinkedIn   { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }

        public AssetOCAddressModel()
        {
        }

    }
}