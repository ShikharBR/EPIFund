using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inview.Epi.EpiFund.Web.Models
{
    public class HoldingCompanySearchResultsModel
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

        public List<HoldingCompanyViewModel> HoldingCompanies { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Show Active Only?")]
        public bool ShowActiveOnly { get; set; }
        [Display(Name = "Company State")]
        public string State { get; set; }
        [Display(Name = "Company Zip")]
        public string Zip { get; set; }

        public HoldingCompanySearchResultsModel()
        {
        }
    }
}