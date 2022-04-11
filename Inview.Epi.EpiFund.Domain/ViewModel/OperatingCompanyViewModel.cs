using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
    public class OperatingCompanyViewModel
    {
        public List<HoldingCompanyViewModel> HoldingCompanies { get; set; }
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
        public OperatingCompanyViewModel()
        {
            HoldingCompanies = new List<HoldingCompanyViewModel>();
            Countries = new List<SelectListItem>();
        }
    }

    public class OperatingCompanyList
    {
        public Guid OperatingCompanyId { get; set; }
        public string OperatingCompanyName { get; set; }
        public string OperatingCompanyEmail { get; set; }
        public string OperatingCompanyFirstName { get; set; }
        public string OperatingCompanyLastName { get; set; }

        public string OperatingCompanyLinkedInurl { get; set; }
        public string OperatingCompanyFacebookurl { get; set; }
        public string OperatingCompanyInstagramurl { get; set; }
        public string OperatingCompanyTwitterurl { get; set; }


        public Guid? HoldingCompanyId { get; set; }
        public string HoldingCompanyName { get; set; }

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
