using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class MultiFamilyAssetViewModel : AssetViewModel
	{
		[Display(Name="Calculated Price Per Unit/Space")]
		public double? CalculatedPPU
		{
			get;
			set;
		}

		[Display(Name="Electric Meter Method")]
		[Required(ErrorMessage="Electric Meter Method is required")]
		public MeteringMethod ElectricMeterMethod
		{
			get;
			set;
		}

		[Display(Name="Electric Meter Service Provider")]
		public string ElectricMeterServProvider
		{
			get;
			set;
		}

		[Display(Name="Gas Meter Method")]
		[Required(ErrorMessage="Gas Meter Method is required")]
		public MeteringMethod GasMeterMethod
		{
			get;
			set;
		}

		[Display(Name="Gas Meter Service Provider")]
		public string GasMeterServProvider
		{
			get;
			set;
		}

		[Display(Name="Last Reported Occupancy Date")]
		public DateTime? LastReportedDate
		{
			get;
			set;
		}

		[Display(Name="Asset Details")]
		public List<MultiFamilyPropertyDetails> MFDetails
		{
			get
			{
				MultiFamilyPropertyDetails multiFamilyPropertyDetail;
				List<MultiFamilyPropertyDetails> multiFamilyPropertyDetails;
				if (!string.IsNullOrWhiteSpace(this.MFDetailsString))
				{
					string[] strArrays = this.MFDetailsString.Split(new char[] { ';' });
					List<MultiFamilyPropertyDetails> multiFamilyPropertyDetails1 = new List<MultiFamilyPropertyDetails>();
					string[] strArrays1 = strArrays;
					for (int i = 0; i < (int)strArrays1.Length; i++)
					{
						if (System.Enum.TryParse<MultiFamilyPropertyDetails>(strArrays1[i], out multiFamilyPropertyDetail))
						{
							multiFamilyPropertyDetails1.Add(multiFamilyPropertyDetail);
						}
					}
					multiFamilyPropertyDetails = multiFamilyPropertyDetails1;
				}
				else
				{
					multiFamilyPropertyDetails = new List<MultiFamilyPropertyDetails>();
				}
				return multiFamilyPropertyDetails;
			}
			set
			{
				this.MFDetailsString = string.Join<MultiFamilyPropertyDetails>(";", value.ToArray());
			}
		}

		public string MFDetailsString
		{
			get;
			set;
		}

		[Display(Name="Asset Details")]
		public List<MobileHomePropertyDetails> MHPDetails
		{
			get
			{
				MobileHomePropertyDetails mobileHomePropertyDetail;
				List<MobileHomePropertyDetails> mobileHomePropertyDetails;
				if (!string.IsNullOrWhiteSpace(this.MFDetailsString))
				{
					string[] strArrays = this.MFDetailsString.Split(new char[] { ';' });
					List<MobileHomePropertyDetails> mobileHomePropertyDetails1 = new List<MobileHomePropertyDetails>();
					string[] strArrays1 = strArrays;
					for (int i = 0; i < (int)strArrays1.Length; i++)
					{
						if (System.Enum.TryParse<MobileHomePropertyDetails>(strArrays1[i], out mobileHomePropertyDetail))
						{
							mobileHomePropertyDetails1.Add(mobileHomePropertyDetail);
						}
					}
					mobileHomePropertyDetails = mobileHomePropertyDetails1;
				}
				else
				{
					mobileHomePropertyDetails = new List<MobileHomePropertyDetails>();
				}
				return mobileHomePropertyDetails;
			}
			set
			{
				this.MFDetailsString = string.Join<MobileHomePropertyDetails>(";", value.ToArray());
			}
		}

		public string MHPDetailsString
		{
			get;
			set;
		}

		[Display(Name="Specifications of MHP Units")]
		public List<AssetMHPSpecification> MHPUnitSpecifications
		{
			get;
			set;
		}

		[Display(Name="Current Occupancy Percentage")]
		public float OccupancyPercentage
		{
			get;
			set;
		}

		[Display(Name="Number of Park Owned Mobile Home Units")]
		[Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		public int ParkOwnedMHUnits
		{
			get;
			set;
		}

		[Display(Name="Total Square Feet")]
		[Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		public int TotalSquareFootage
		{
			get;
			set;
		}

		[Display(Name="Apt Units or MHP Spaces")]
		[Range(0, 2147483647, ErrorMessage="Value must be 0 or higher")]
		public int TotalUnits
		{
			get;
			set;
		}

		[Display(Name="Specifications of Units")]
		public List<AssetUnitSpecification> UnitSpecifications
		{
			get;
			set;
		}

		public MultiFamilyAssetViewModel()
		{
		}
	}
}