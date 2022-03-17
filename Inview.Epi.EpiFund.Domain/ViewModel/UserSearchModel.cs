using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class UserSearchModel
	{
		public string AddressLine1
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public string CompanyName
		{
			get;
			set;
		}

		public UserType ControllingUserType
		{
			get;
			set;
		}

		public int? CorpAdminId
		{
			get;
			set;
		}

		public string CorpEntity
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public List<UserType> ExcludedUsers
		{
			get;
			set;
		}

		public string FirstName
		{
			get;
			set;
		}

		public bool HasJVMA
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

		public int? PendingEscrows
		{
			get;
			set;
		}

		public double? ReferralDB
		{
			get;
			set;
		}

		public DateTime? RegistrationDateEnd
		{
			get;
			set;
		}

		public DateTime? RegistrationDateStart
		{
			get;
			set;
		}

		public bool ShowActiveOnly
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public UserType? UserTypeFilter
		{
			get;
			set;
		}

		public List<UserType> UserTypeFilters
		{
			get;
			set;
		}

		public string Zip
		{
			get;
			set;
		}

		public UserSearchModel()
		{
		}
	}
}