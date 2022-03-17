using Inview.Epi.EpiFund.Domain.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace Inview.Epi.EpiFund.Web.Models
{
    public class OperatingCompanySearchResultsModel : BaseSearchResultsModel
    {
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }
        [Display(Name = "Company City")]
        public string City { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        public PagedList.IPagedList<OperatingCompanyViewModel> OperatingCompanies { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Show Active Only?")]
        public bool ShowActiveOnly { get; set; }
        [Display(Name = "Company State")]
        public string State { get; set; }
        [Display(Name = "Company Zip")]
        public string Zip { get; set; }

        public OperatingCompanySearchResultsModel()
        {
        }
    }
}