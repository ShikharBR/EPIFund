using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class TempUnitSpecification
	{
		[Display(Name="Bathrooms")]
		public BathroomCount BathCount
		{
			get;
			set;
		}

		[Display(Name="Bedrooms")]
		public BedroomCount BedCount
		{
			get;
			set;
		}

		[Display(Name="Number of Unit Types")]
		[Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		[Required(ErrorMessage="# of Units is required")]
		public int CountOfUnits
		{
			get;
			set;
		}

		public Guid GuidId
		{
			get;
			set;
		}

		public int TempUnitSpecificationId
		{
			get;
			set;
		}

		[Display(Name="Unit Base Rent")]
		[Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		[Required(ErrorMessage="Base Rent is required")]
		public float UnitBaseRent
		{
			get;
			set;
		}

		[Display(Name="Unit Square Feet")]
		[Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		[Required(ErrorMessage="Square Feet is required")]
		public int UnitSquareFeet
		{
			get;
			set;
		}

		public TempUnitSpecification()
		{
		}
	}
}