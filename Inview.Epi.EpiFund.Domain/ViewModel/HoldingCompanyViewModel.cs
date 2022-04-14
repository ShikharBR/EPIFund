using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Inview.Epi.EpiFund.Domain.Enum;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
    public class HoldingCompanyViewModel
    {
        public Guid HoldingCompanyId { get; set; }
        
        public List<HoldingCompanyAssetSimpleViewModel> Assets { get; set; }
        
        public List<HoldingCompanyOperatingCompanySimpleViewModel> OperatingCompanies { get; set; }
        
        public List<SelectListItem> Countries { get; set; }

        public List<SelectListItem> OpertingCompanyList { get; set; }

        [Display(Name = "Operating Company")]
        public Guid OperatingCompanyId { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Company Name is required")]
        public string CompanyName { get; set; }

        [Display(Name = "Is RA")]
        public bool ISRA { get; set; }

                
        [Display(Name = "First Name of Officer")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name of Officer")]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        //[Required(ErrorMessage = "Email Address is required")]
        public string Email { get; set; }
        [Display(Name = "Company Address Line 1")]
        public string AddressLine1 { get; set; }
        [Display(Name = "Company Address Line 2")]
        public string AddressLine2 { get; set; }
        [Display(Name = "Company City")]
        public string City { get; set; }
        [Display(Name = "State/Province/Region")]
        public string State { get; set; }
        [Display(Name = "Company Country")]
        public string Country { get; set; }
        [Display(Name = "Zip/Postal Code")]
        public string Zip { get; set; }
        [Display(Name = "Main Office Number")]
        public string WorkNumber { get; set; }
        [Display(Name = "Cell Phone Number")]
        public string CellNumber { get; set; }
        
        [Display(Name = "Fax Number")]
        public string FaxNumber { get; set; }

        [Display(Name = "Linked In")]
        public string LinkedIn { get; set; }
        [Display(Name = "Facebook")]
        public string Facebook { get; set; }
        [Display(Name = "Instagram")]
        public string Instagram { get; set; }
        [Display(Name = "Twitter")]
        public string Twitter { get; set; }

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }
        [Display(Name = "Assign Holding Company to Asset by Asset Number")]
        public string AssetAssignment { get; set; }
        public bool OverwriteAsset { get; set; }

        public string FullName
        {
            get
            {
                return string.Concat(this.FirstName, " ", this.LastName);
            }
        }

        public HoldingCompanyViewModel() {
            Assets = new List<HoldingCompanyAssetSimpleViewModel>();
            Countries = new List<SelectListItem>();
            OperatingCompanies = new List<HoldingCompanyOperatingCompanySimpleViewModel>();
            OpertingCompanyList = new List<SelectListItem>();
        }

        [Display(Name = "PI Name")]
        public string PIName { get; set; }

        [Display(Name = "Assets Count")]
        public int AssetsCount { get; set; }

        [Display(Name = "Assets Published Count")]
        public int AssetsPublishedCount { get; set; }

        [Display(Name = "Operating Comapny Name")]
        public string OperatingComapnyName { get; set; }

    }

    public class HoldingCompanyAssetSimpleViewModel
    {
        public Guid AssetId { get; set; }
        public Guid? OperatingCompanyId { get; set; }
        public int AssetNumber { get; set; }
        public bool Published { get; set; }
    }
    public class HoldingCompanyOperatingCompanySimpleViewModel
    {
        public Guid OperatingCompanyId { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? ReferredByJvmp { get; set; }
        public string DateOfRegistration { get; set; }
        public bool? RegisteredDocs { get; set; }
    }


    //Model HC and AssetHCOwnershipss
    public class HCCAssetHCOwnerships
    {
        public HoldingCompanyViewModel HoldingCompany { get; set; }
        public int? AssetHCOwnershipId { get; set; }
        public Guid? AssetId { get; set; }
        public string Terms { get; set; }
        public DateTime? ActualClosingDate { get; set; }
        public double? SalesPrice { get; set; }
        public bool? SalesPriceNotProvided { get; set; }
        public double? CalculatedPPU { get; set; }
        public double? CalculatedPPSqFt { get; set; }
        public double? CashInvestmentApy { get; set; }
        public string TermsOther { get; set; }
        public double? CapRate { get; set; }

    }

    public class HCCAssetOC
    {       
        public int? AssetOCId { get; set; }
        public Guid? OperatingCompanyId { get; set; }
        public Guid? AssetId { get; set; }

        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public string Zip { get; set; }
        public string Country { get; set; }
        public string WorkNumber { get; set; }
        public string CellNumber { get; set; }
        
        public string FaxNumber { get; set; }
               
        public string LinkedIn { get; set; }       
        public string Facebook { get; set; }       
        public string Instagram { get; set; }        
        public string Twitter { get; set; }

        public bool IsActive { get; set; }

    }

    public class HoldingCompanyList
    {    

        public Guid HoldingCompanyId { get; set; }
        public string HoldingCompanyName { get; set; }
        public string HoldingCompanyEmail { get; set; }
        public string HoldingCompanyFirstName { get; set; }
        public string HoldingCompanyLastName { get; set; }

        public string HoldingCompanyLinkedInurl { get; set; }
        public string HoldingCompanyFacebookurl { get; set; }
        public string HoldingCompanyInstagramurl { get; set; }
        public string HoldingCompanyTwitterurl { get; set; }
        public bool ISRA { get; set; }



        public Guid? OperatingCompanyId { get; set; }
        public string OperatingCompanyName { get; set; }

        public Guid? AssetId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public AssetType Type { get; set; }

        public int SquareFeet { get; set; }
        public int NumberOfUnits { get; set; }

        public string AssetName { get; set; }
        public int AssetNumber { get; set; }

        public bool Show { get; set; }
        public bool IsPaper { get; set; }

        public string Address1 { get; set; }
        public string ZipCode { get; set; }
        public string County { get; set; }


        public int AssetCount { get; set; }
        
        public UserType UserType { get; set; }
        public ListingStatus ListingStatus { get; set; }

        public string BusDriver { get; set; }

        public bool Portfolio { get; set; }
        public int PortfolioCount { get; set; }

    }
}