using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class AssetUnitSpecification
	{
		public Inview.Epi.EpiFund.Domain.Entity.Asset Asset
		{
			get;
			set;
		}

		public Guid AssetId
		{
			get;
			set;
		}

		public Guid AssetUnitSpecificationId
		{
			get;
			set;
		}

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

		[Display(Name="Unit Base Rent")]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        [Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		[Required(ErrorMessage="Base Rent is required")]
		public float UnitBaseRent
		{
			get;
			set;
		}

		[Display(Name="Unit Square Feet")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		[Required(ErrorMessage="Square Feet is required")]
		public int UnitSquareFeet
		{
			get;
			set;
		}

		public AssetUnitSpecification()
		{
		}
	}
}