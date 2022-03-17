using Inview.Epi.EpiFund.Domain.ViewModel;
using PagedList;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models
{
	public class InsuranceCompanySearchResultsModel : BaseSearchResultsModel
	{
		public PagedList.IPagedList<InsuranceCompanyViewModel> Companies
		{
			get;
			set;
		}

		[Display(Name="Insurance Company Name")]
		public string InsuranceCompanyName
		{
			get;
			set;
		}

		[Display(Name="Website")]
		public string InsuranceCompanyURL
		{
			get;
			set;
		}

		[Display(Name="State")]
		public string State
		{
			get;
			set;
		}

		public PagedList.IPagedList<InsuranceCompanyUserViewModel> Users
		{
			get;
			set;
		}

		public InsuranceCompanySearchResultsModel()
		{
		}
	}
}