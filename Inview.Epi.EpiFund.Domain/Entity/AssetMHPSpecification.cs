using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class AssetMHPSpecification
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

		public Guid AssetMHPSpecificationId
		{
			get;
			set;
		}

		public int CountOfUnits
		{
			get;
			set;
		}

		[Display(Name="Current Double wide space base rent  ")]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
		[Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		[Required(ErrorMessage="Double wide space base rent is required")]
		public float CurrentDoubleBaseRent
		{
			get;
			set;
		}

		[Display(Name="Current Double Wide Park Owned Unit Base Rent ")]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        [Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		[Required(ErrorMessage="Double Wide Park Owned Unit Base Rent is required")]
		public float CurrentDoubleOwnedBaseRent
		{
			get;
			set;
		}

		[Display(Name="Current Single wide space base rent ")]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        [Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		[Required(ErrorMessage="Single wide space base rent is required")]
		public float CurrentSingleBaseRent
		{
			get;
			set;
		}

		[Display(Name="Current Single Wide Park Owned Unit Base Rent ")]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        [Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		[Required(ErrorMessage="Single Wide Park Owned Unit Base Rent is required")]
		public float CurrentSingleOwnedBaseRent
		{
			get;
			set;
		}

		[Display(Name="Current Triple wide space base rent  ")]
		[Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		[Required(ErrorMessage="Triple wide space base rent is required")]
		public float CurrentTripleBaseRent
		{
			get;
			set;
		}

		[Display(Name="Current Triple Wide Park Owned Unit Base Rent ")]
		[Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		[Required(ErrorMessage="Triple Wide Park Owned Unit Base Rent is required")]
		public float CurrentTripleOwnedBaseRent
		{
			get;
			set;
		}

		[Display(Name="Number of Double Wide Rental Spaces")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int NumberDoubleWide
		{
			get;
			set;
		}

		[Display(Name="Number of Double Wide park Owned Units")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int NumberDoubleWideOwned
		{
			get;
			set;
		}

		[Display(Name="Number of Single Wide Rental Spaces")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int NumberSingleWide
		{
			get;
			set;
		}

		[Display(Name="Number of Single Wide park Owned Units")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int NumberSingleWideOwned
		{
			get;
			set;
		}

		[Display(Name="Number of Triple Wide Rental Spaces")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int NumberTripleWide
		{
			get;
			set;
		}

		[Display(Name="Number of Triple Wide park Owned Units")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int NumberTripleWideOwned
		{
			get;
			set;
		}

		public AssetMHPSpecification()
		{
		}
	}
}