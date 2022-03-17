using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class DeferredMaintenanceCost
	{
		public int Cost
		{
			get;
			set;
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int DeferredMaintananceCostId
		{
			get;
			set;
		}

		public string InputType
		{
			get;
			set;
		}

		public MaintenanceDetails MaintenanceDetail
		{
			get;
			set;
		}

		public DeferredMaintenanceCost()
		{
		}
	}
}