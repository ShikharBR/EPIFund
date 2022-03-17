using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class CompanySearchModel
	{
		public string CompanyName
		{
			get;
			set;
		}

		public string CompanyURL
		{
			get;
			set;
		}

		public bool NeedsManager
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public CompanySearchModel()
		{
		}
	}
}