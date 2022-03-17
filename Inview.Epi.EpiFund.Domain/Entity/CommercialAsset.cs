using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class CommercialAsset : Asset
	{
		public double BaseRentPerSqFtMajorTenant
		{
			get;
			set;
		}

		public double CurrentMarkerRentPerSqFt
		{
			get;
			set;
		}

		public bool HasAAARatedMajorTenant
		{
			get;
			set;
		}

		public bool IsMajorTenantAAARated
		{
			get;
			set;
		}

		public int LeasedSquareFootageByMajorTenant
		{
			get;
			set;
		}

		public string NameOfAAARatedMajorTenant
		{
			get;
			set;
		}

		public int NumberOfRentableSuites
		{
			get;
			set;
		}

		public int NumberofSuites
		{
			get;
			set;
		}

		public int NumberOfTenants
		{
			get;
			set;
		}

		public DateTime? OccupancyDate
		{
			get;
			set;
		}

		public float OccupancyPercentage
		{
			get;
			set;
		}

		public int ProformaAnnualNoi
		{
			get;
			set;
		}

		public int ProformaSgi
		{
			get;
			set;
		}

		public string PropertyDetailsString
		{
			get;
			set;
		}

		public int RentableSquareFeet
		{
			get;
			set;
		}

		public CommercialType Type
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.VacantSuites VacantSuites
		{
			get;
			set;
		}

		public CommercialAsset()
		{
		}
	}
}