using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class SearchCriteriaGeographicParameter
	{
		public string AdditionalRequirements
		{
			get;
			set;
		}

		public int AssetSeachCriteriaId
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Entity.AssetSearchCriteria AssetSearchCriteria
		{
			get;
			set;
		}

		public string InterestCity
		{
			get;
			set;
		}

		public string InterestState
		{
			get;
			set;
		}

		public string Restrictions
		{
			get;
			set;
		}

		public int SearchCriteriaGeographicParameterId
		{
			get;
			set;
		}

		public SearchCriteriaGeographicParameter()
		{
		}
	}
}