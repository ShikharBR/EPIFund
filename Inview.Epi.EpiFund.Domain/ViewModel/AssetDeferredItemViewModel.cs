using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AssetDeferredItemViewModel
	{
		public string ItemDescription
		{
			get;
			set;
		}

		public string ItemTitle
		{
			get;
			set;
		}

		public MaintenanceDetails MaintenanceDetail
		{
			get;
			set;
		}

		public double NumberOfUnits
		{
			get;
			set;
		}

		public bool Selected
		{
			get;
			set;
		}

		public double UnitCost
		{
			get;
			set;
		}

		public string UnitTypeLabel
		{
			get;
			set;
		}

		public AssetDeferredItemViewModel()
		{
		}
	}
}