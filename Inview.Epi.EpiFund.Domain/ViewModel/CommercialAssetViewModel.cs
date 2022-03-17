using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class CommercialAssetViewModel : AssetViewModel
	{
		[Display(Name="Major Tenant's Base Rent per Square Feet")]
		public double BaseRentPerSqFtMajorTenant
		{
			get;
			set;
		}

		[Display(Name="Calculated Price Per Square Foot")]
		public double? CalculatedPPSqFt
		{
			get;
			set;
		}

		[Display(Name="Current Market Rents per Square Feet")]
		public double CurrentMarkerRentPerSqFt
		{
			get;
			set;
		}

		[Display(Name="Electric Meter Method")]
		public MeteringMethod ElectricMeterMethod
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

		[Display(Name="Does Property Have a Major Tenant?")]
		public bool HasAAARatedMajorTenant
		{
			get;
			set;
		}

		[Display(Name="Is the Major Tenant a Publically Traded Company")]
		public bool IsMajorTenantAAARated
		{
			get;
			set;
		}

		[Display(Name="Leased Square Footage of property by Major Tenant")]
		public int LeasedSquareFootageByMajorTenant
		{
			get;
			set;
		}

		[Display(Name="Name of Major Tenant (If Applicable)")]
		public string NameOfAAARatedMajorTenant
		{
			get;
			set;
		}

		[Display(Name="Current Number of Vacant Suites for Property")]
		public int NumberOfRentableSuites
		{
			get;
			set;
		}

		[Display(Name="Current Number of Retail/Office Suites")]
		public int NumberofSuites
		{
			get;
			set;
		}

		[Display(Name="Number of Tenants")]
		public int NumberOfTenants
		{
			get;
			set;
		}

		[Display(Name="Last Reported Occupancy Date")]
		public DateTime? OccupancyDate
		{
			get;
			set;
		}

		[Display(Name="Occupancy Percentage")]
		public float OccupancyPercentage
		{
			get;
			set;
		}

		[Display(Name="Percentage of Property Leased by Major Tenant")]
		public double PercentOfPropertyLeasedByMajorTenant
		{
			get;
			set;
		}

		[Display(Name="Proforma Annual NOI")]
		public int ProformaAnnualNoi
		{
			get;
			set;
		}

		[Display(Name="Proforma SGI")]
		[Required(ErrorMessage="Proforma SGI is required")]
		public int ProformaSgi
		{
			get;
			set;
		}

		[Display(Name="Property Details")]
		public List<CommercialPropertyDetails> PropertyDetails
		{
			get
			{
				CommercialPropertyDetails commercialPropertyDetail;
				List<CommercialPropertyDetails> commercialPropertyDetails;
				if (!string.IsNullOrWhiteSpace(this.PropertyDetailsString))
				{
					string[] strArrays = this.PropertyDetailsString.Split(new char[] { ';' });
					List<CommercialPropertyDetails> commercialPropertyDetails1 = new List<CommercialPropertyDetails>();
					string[] strArrays1 = strArrays;
					for (int i = 0; i < (int)strArrays1.Length; i++)
					{
						if (System.Enum.TryParse<CommercialPropertyDetails>(strArrays1[i], out commercialPropertyDetail))
						{
							commercialPropertyDetails1.Add(commercialPropertyDetail);
						}
					}
					commercialPropertyDetails = commercialPropertyDetails1;
				}
				else
				{
					commercialPropertyDetails = new List<CommercialPropertyDetails>();
				}
				return commercialPropertyDetails;
			}
			set
			{
				this.PropertyDetailsString = string.Join<CommercialPropertyDetails>(";", value.ToArray());
			}
		}

		public string PropertyDetailsString
		{
			get;
			set;
		}

		[Display(Name="Rentable Square Feet")]
		[Required(ErrorMessage="Rentable Square Feet is required")]
		public int RentableSquareFeet
		{
			get;
			set;
		}

		[Display(Name="Type of Commercial Asset")]
		[Required(ErrorMessage="Type of Asset is required")]
		public CommercialType Type
		{
			get;
			set;
		}

		[Display(Name="Define Vacant Suites")]
		[Required(ErrorMessage="Vacant Suites is required")]
		public Inview.Epi.EpiFund.Domain.Enum.VacantSuites VacantSuites
		{
			get;
			set;
		}

		public CommercialAssetViewModel()
		{
		}
	}
}