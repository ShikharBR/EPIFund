using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class TitleUserQuickViewModel
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

		public string OldEmail
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

		public int TotalItemCount
		{
			get;
			set;
		}

		public TitleUserQuickViewModel()
		{
		}
	}
}