using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class OtherDemographicDetailModel
	{
		[Display(Name="Max Age")]
		public string AgePropertyRangeMax
		{
			get;
			set;
		}

		[Display(Name="Can Asset be substantially vacant?")]
		[Required]
		public bool? CanBeVacant
		{
			get;
			set;
		}

		[Display(Name="Can Asset have extensive renovation/updating needs?")]
		public bool? CanHaveExtensiveRenovationNeeds
		{
			get;
			set;
		}

		public bool? CanHaveExtensiveRenovationNeedsOptional
		{
			get;
			set;
		}

		[Display(Name="Do you have Parking Ratio parameters?")]
		public bool? HasParkingRatioParameters
		{
			get;
			set;
		}

		[Display(Name="If yes, what is the max vacancy a property can be at closing?")]
		public decimal MaxVacancy
		{
			get;
			set;
		}

		[Display(Name="What is the Minimum % for Accredited Tenant Profiles at a Property?")]
		[Required]
		public decimal MinimumForAccreditedTenantProfiles
		{
			get;
			set;
		}

		[Display(Name="Is Multi-Level Architecture acceptable?")]
		public bool? MultiLevelAcceptable
		{
			get;
			set;
		}

		public bool? MultiLevelAcceptableOptional
		{
			get;
			set;
		}

		[Display(Name="No Preference?")]
		public bool? NoAgePropertyPreference
		{
			get;
			set;
		}

		[Display(Name="For Office, Medical or Mixed Use, are covered parking stalls required")]
		public bool? OfficeMedicalMixedUseCoveredParkingStallsRequired
		{
			get;
			set;
		}

		public bool? OfficeMedicalMixedUseCoveredParkingStallsRequiredOptional
		{
			get;
			set;
		}

		[Display(Name="Other Demographic requirements")]
		public string OtherRequirements
		{
			get;
			set;
		}

		[Display(Name="If yes, define Ratio requisites")]
		public string ParkingRatioRatioRequisites
		{
			get;
			set;
		}

		[Display(Name="Does the property require a Major Tenant?")]
		[Required]
		public bool? PropertyRequiresMajorTenant
		{
			get;
			set;
		}

		public bool? PropertyRequiresMajorTenantOptional
		{
			get;
			set;
		}

		[Display(Name="Do you require covered parking stalls for Asset Tenants?")]
		public string RequiresCoveredParkingStalls
		{
			get;
			set;
		}

		[Display(Name="If Retail or Mixed Use, does property require \"Single Tenant Pads\" as part of site plan?")]
		public bool? RequiresSingleTenantPads
		{
			get;
			set;
		}

		[Display(Name="If yes, provide requisite details")]
		public string SingleTenantPadsRequisiteDetails
		{
			get;
			set;
		}

		[Display(Name="Max")]
		public string SquareFootageRangeMax
		{
			get;
			set;
		}

		[Display(Name="Min")]
		public string SquareFootageRangeMin
		{
			get;
			set;
		}

		[Display(Name="If yes, define Tenant requisites")]
		public string TenantRequisites
		{
			get;
			set;
		}

		[Display(Name="Will you look at CRE property that has unfinished suites?")]
		public bool? WillLookAtUnifinishedSuites
		{
			get;
			set;
		}

		public bool? WillLookAtUnifinishedSuitesOptional
		{
			get;
			set;
		}

		public OtherDemographicDetailModel()
		{
		}
	}
}