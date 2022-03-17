using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class MultiFamilyAsset : Asset
	{
		public MeteringMethod ElectricMeterMethod
		{
			get;
			set;
		}

		public string ElectricMeterServProvider
		{
			get;
			set;
		}

		public MeteringMethod GasMeterMethod
		{
			get;
			set;
		}

		public string GasMeterServProvider
		{
			get;
			set;
		}

		public int GrossRentableSquareFeet
		{
			get;
			set;
		}

		public DateTime? LastReportedDate
		{
			get;
			set;
		}

		public string MFDetailsString
		{
			get;
			set;
		}

		public virtual List<AssetMHPSpecification> MHPUnitSpecifications
		{
			get;
			set;
		}

		public float OccupancyPercentage
		{
			get;
			set;
		}

		public int ParkOwnedMHUnits
		{
			get;
			set;
		}

		public int TotalSquareFootage
		{
			get;
			set;
		}

		public int TotalUnits
		{
			get;
			set;
		}

		public virtual List<AssetUnitSpecification> UnitSpecifications
		{
			get;
			set;
		}

		public MultiFamilyAsset()
		{
		}
	}
}