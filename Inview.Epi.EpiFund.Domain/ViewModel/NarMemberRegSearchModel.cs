using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class NarMemberRegSearchModel
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

		public string LastName
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

		public string Zip
		{
			get;
			set;
		}

		public NarMemberRegSearchModel()
		{
		}
	}
}