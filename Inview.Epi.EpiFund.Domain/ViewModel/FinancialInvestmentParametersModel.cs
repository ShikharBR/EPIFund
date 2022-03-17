using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class FinancialInvestmentParametersModel
	{
		[Display(Name="Provide details as to specific parameters:")]
		public string BrandNewDetails
		{
			get;
			set;
		}

		public decimal InvestmentFundingRangeMax
		{
			get;
			set;
		}

		public decimal InvestmentFundingRangeMin
		{
			get;
			set;
		}

		public decimal MinimumCapitalizationRate
		{
			get;
			set;
		}

		[Display(Name="Provide details as to specific parameters:")]
		public string PartiallyBuiltDetails
		{
			get;
			set;
		}

		[Display(Name="Provide details as to pro-forma parameters:")]
		public string ProFormaParametersDetails
		{
			get;
			set;
		}

		public decimal TargetPricePerSpaceMax
		{
			get;
			set;
		}

		public decimal TargetPricePerSpaceMin
		{
			get;
			set;
		}

		public decimal TargetPricePerUnitMax
		{
			get;
			set;
		}

		public decimal TargetPricePerUnitMin
		{
			get;
			set;
		}

		[Display(Name="Will you consider a property that is brand new with few to no tenants?")]
		public bool? WillConsiderBrandNew
		{
			get;
			set;
		}

		[Display(Name="Will you consider a property that is partially built with no certificate of occupancy from the appropriate municipal authority?")]
		public bool? WillConsiderPartiallyBuilt
		{
			get;
			set;
		}

		[Display(Name="Will you consider a property that is presently [underperforming] at [future pro-forma] pricing that is supported by market comparables?")]
		public bool? WillConsiderUnperformingAtPricing
		{
			get;
			set;
		}

		public FinancialInvestmentParametersModel()
		{
		}
	}
}