using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class GeographicParametersModel
	{
		[Display(Name="Additional Geographic requirements: (e.g., proximity to public transportation, hospitals, schools, municipal buildings, major employment, parks, other…)")]
		public string AdditionalRequirements
		{
			get;
			set;
		}

		public List<GeographicParameterInterestModel> Interests
		{
			get;
			set;
		}

		[Display(Name="Define all Geographic restrictions:\t(e.g., the opposite of the above, certain tenant profiles, other…)")]
		public string Restrictions
		{
			get;
			set;
		}

		public GeographicParametersModel()
		{
		}
	}
}