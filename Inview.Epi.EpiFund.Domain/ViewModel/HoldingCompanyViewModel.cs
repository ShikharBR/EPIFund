using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
    public class HoldingCompanyViewModel
    {
        public Guid HoldingCompanyId { get; set; }
        public List<HoldingCompanyAssetSimpleViewModel> Assets { get; set; }
        public List<HoldingCompanyOperatingCompanySimpleViewModel> OperatingCompanies { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public Guid OperatingCompanyId { get; set; }
        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Company Name is required")]
        public string CompanyName { get; set; }
                
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
        public Guid? AssetId { get; set; }
        public int? Terms { get; set; }
        public DateTime? ActualClosingDate { get; set; }
        public double? SalesPrice { get; set; }
        public bool? SalesPriceNotProvided { get; set; }
        public double? CalculatedPPU { get; set; }
        public double? CalculatedPPSqFt { get; set; }
        public double? CashInvestmentApy { get; set; }
        public string TermsOther { get; set; }

    }
}