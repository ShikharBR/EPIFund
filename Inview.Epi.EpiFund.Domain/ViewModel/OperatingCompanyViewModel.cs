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
}
