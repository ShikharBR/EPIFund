using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class ReportedOccupancy
	{
		public DateTime LastReportedDate
		{
			get;
			set;
		}

		public float Percentage
		{
			get;
			set;
		}

		public Guid ReportedOccupancyId
		{
			get;
			set;
		}

		public ReportedOccupancy()
		{
		}
	}
}