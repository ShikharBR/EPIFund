using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class TempDeferredMaintenanceItem
	{
		public Guid GuidId
		{
			get;
			set;
		}

		public MaintenanceDetails MaintenanceDetail
		{
			get;
			set;
		}

		public int TempDeferredMaintenanceItemId
		{
			get;
			set;
		}

		public double UnitCost
		{
			get;
			set;
		}

		public int Units
		{
			get;
			set;
		}

		public TempDeferredMaintenanceItem()
		{
		}
	}
}