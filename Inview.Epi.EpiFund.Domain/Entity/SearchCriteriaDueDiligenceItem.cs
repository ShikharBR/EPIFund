using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class SearchCriteriaDueDiligenceItem
	{
		public Inview.Epi.EpiFund.Domain.Entity.AssetSearchCriteria AssetSearchCriteria
		{
			get;
			set;
		}

		public int AssetSearchCriteriaId
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.ImportanceLevel ImportanceLevel
		{
			get;
			set;
		}

		public string ItemDescription
		{
			get;
			set;
		}

		public int SearchCriteriaDueDiligenceItemId
		{
			get;
			set;
		}

		public SearchCriteriaDueDiligenceItem()
		{
		}
	}
}