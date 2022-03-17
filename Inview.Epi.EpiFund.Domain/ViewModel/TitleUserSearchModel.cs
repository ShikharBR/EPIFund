using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class TitleUserSearchModel
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

		public bool IsActive
		{
			get;
			set;
		}

		public bool IsManager
		{
			get;
			set;
		}

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

		public int? TitleCompanyId
		{
			get;
			set;
		}

		public TitleUserSearchModel()
		{
		}
	}
}