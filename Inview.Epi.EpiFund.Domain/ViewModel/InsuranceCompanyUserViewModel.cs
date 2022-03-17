using Inview.Epi.EpiFund.Domain.Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class InsuranceCompanyUserViewModel : BaseModel
	{
		public InsuranceCompanyViewModel Company
		{
			get;
			set;
		}

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

		public int InsuranceCompanyUserId
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

		[Display(Name="Is Manager?")]
		public bool IsManager
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

		public byte[] PasswordBytes
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

		public int UserId
		{
			get;
			set;
		}

		public InsuranceCompanyUserViewModel()
		{
			this.Company = new InsuranceCompanyViewModel();
		}

		public InsuranceCompanyUserViewModel(PCInsuranceCompanyManager user)
		{
			this.Company = new InsuranceCompanyViewModel();
			this.InsuranceCompanyId = user.PCInsuranceCompanyId;
			this.Email = user.User.Username;
			this.FirstName = user.User.FirstName;
			this.InsuranceCompanyUserId = user.PCInsuranceCompanyManagerId;
			this.IsActive = user.IsActive;
			this.LastName = user.User.LastName;
			this.PhoneNumber = user.User.WorkNumber;
		}

		public void EntityToModel(PCInsuranceCompanyManager user)
		{
			this.InsuranceCompanyId = user.PCInsuranceCompanyId;
			this.Email = user.User.Username;
			this.FirstName = user.User.FirstName;
			this.InsuranceCompanyUserId = user.PCInsuranceCompanyManagerId;
			this.IsActive = user.IsActive;
			this.LastName = user.User.LastName;
			this.PhoneNumber = user.User.WorkNumber;
			this.PasswordBytes = user.User.Password;
		}
	}
}