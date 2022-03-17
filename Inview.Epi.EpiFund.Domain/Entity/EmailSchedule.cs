using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class EmailSchedule
	{
		public int EmailScheduleId
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.EmailScheduleType EmailScheduleType
		{
			get;
			set;
		}

		public int IntervalInDays
		{
			get;
			set;
		}

		public DateTime? LastRunDate
		{
			get;
			set;
		}

		public DateTime StartDate
		{
			get;
			set;
		}

		public EmailSchedule()
		{
		}
	}
}