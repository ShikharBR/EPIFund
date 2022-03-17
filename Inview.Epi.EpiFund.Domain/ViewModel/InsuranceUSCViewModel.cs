using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class InsuranceUSCViewModel
	{
		public string CompanyName
		{
			get;
			set;
		}

		public string FullName
		{
			get;
			set;
		}

		public bool HasOrderHistory
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public InsuranceUSCViewModel()
		{
		}
	}
}