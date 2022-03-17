using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class AssetDeferredMaintenanceItem
	{
		public Inview.Epi.EpiFund.Domain.Entity.Asset Asset
		{
			get;
			set;
		}

		public int AssetDeferredMaintenanceItemId
		{
			get;
			set;
		}

		public Guid AssetId
		{
			get;
			set;
		}

		public string ItemDescription
		{
			get;
			set;
		}

		public MaintenanceDetails MaintenanceDetail
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

		public AssetDeferredMaintenanceItem()
		{
		}
	}
}