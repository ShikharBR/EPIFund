using Inview.Epi.EpiFund.Domain.ViewModel;
using PagedList;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models
{
	public class InsuranceCompanyUserSearchResultsModel : BaseSearchResultsModel
	{
		[Compare("Password", ErrorMessage="Password and confirm password do not match.")]
		[DataType(DataType.Password)]
		[Display(Name="Confirm Password")]
		public string ConfirmPassword
		{
			get;
			set;
		}

		[DataType(DataType.EmailAddress)]
		[Display(Name="Email")]
		[Required]
		public string Email
		{
			get;
			set;
		}

		[Display(Name="First Name")]
		[RegularExpression("([a-zA-Z\\d]+[\\w\\d]*|)[a-zA-Z]+[\\w\\d.]*", ErrorMessage="Invalid first name")]
		[Required]
		public string FirstName
		{
			get;
			set;
		}

		public string FullName
		{
			get
			{
				return string.Concat(this.FirstName, " ", this.LastName);
			}
		}

		public int InsuranceCompanyId
		{
			get;
			set;
		}

		[Display(Name="Is Active?")]
		public bool IsActive
		{
			get;
			set;
		}

		[Display(Name="Last Name")]
		[RegularExpression("([a-zA-Z\\d]+[\\w\\d]*|)[a-zA-Z]+[\\w\\d.]*", ErrorMessage="Invalid last name")]
		[Required]
		public string LastName
		{
			get;
			set;
		}

		public string ManagingOfficerName
		{
			get;
			set;
		}

		public string OldEmail
		{
			get;
			set;
		}

		[Display(Name="Password")]
		public string Password
		{
			get;
			set;
		}

		[Display(Name="Phone Number")]
		public string PhoneNumber
		{
			get;
			set;
		}

		public PagedList.IPagedList<InsuranceCompanyUserViewModel> Users
		{
			get;
			set;
		}

		public InsuranceCompanyUserSearchResultsModel()
		{
		}
	}
}