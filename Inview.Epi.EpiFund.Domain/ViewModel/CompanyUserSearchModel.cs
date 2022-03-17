using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class CompanyUserSearchModel
	{
		public string Email
		{
			get;
			set;
		}

		public string FirstName
		{
			get;
			set;
		}

		public int? InsuranceCompanyId
		{
			get;
			set;
		}

		public bool IsActive
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public string PhoneNumber
		{
			get;
			set;
		}

		public bool ShowActiveOnly
		{
			get;
			set;
		}

		public CompanyUserSearchModel()
		{
		}
	}
}