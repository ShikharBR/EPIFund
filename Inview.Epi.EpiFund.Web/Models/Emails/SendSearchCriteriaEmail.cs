using Postal;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class SendSearchCriteriaEmail : Postal.Email
	{
		public decimal AccreditedTenantProfilePercentage
		{
			get;
			set;
		}

		public string AssetSought
		{
			get;
			set;
		}

		public decimal BudgetMax
		{
			get;
			set;
		}

		public decimal BudgetMin
		{
			get;
			set;
		}

		public string DoesProjectHaveToBeTurnKey
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public List<string> GeographicInterests
		{
			get;
			set;
		}

		public string MajorTenant
		{
			get;
			set;
		}

		public decimal MaxRatio1BedroomUnits
		{
			get;
			set;
		}

		public decimal MaxRatioStudioUnits
		{
			get;
			set;
		}

		public string MinRating
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string RequireRehabBudget
		{
			get;
			set;
		}

		public string SubstantiallyVacant
		{
			get;
			set;
		}

		public string Underperforming
		{
			get;
			set;
		}

		public string UserCity
		{
			get;
			set;
		}

		public string UserState
		{
			get;
			set;
		}

		public SendSearchCriteriaEmail()
		{
		}
	}
}