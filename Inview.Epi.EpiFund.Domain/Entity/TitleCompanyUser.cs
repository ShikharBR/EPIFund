using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class TitleCompanyUser
	{
		public string AssignedStates
		{
			get;
			set;
		}

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

		public string FullName
		{
			get
			{
				return string.Concat(this.FirstName, " ", this.LastName);
			}
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

		public byte[] Password
		{
			get;
			set;
		}

		public string PhoneNumber
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Entity.TitleCompany TitleCompany
		{
			get;
			set;
		}

		public int TitleCompanyId
		{
			get;
			set;
		}

		public int TitleCompanyUserId
		{
			get;
			set;
		}

		public TitleCompanyUser()
		{
		}
	}
}