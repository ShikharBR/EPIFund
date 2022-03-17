using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AssetSearchCriteriaQuickViewModel
	{
		public DateTime DateCreated
		{
			get;
			set;
		}

		public DateTime LastUpdated
		{
			get;
			set;
		}

		public int SearchCriteriaId
		{
			get;
			set;
		}

		public AssetSearchCriteriaQuickViewModel()
		{
		}
	}
}