using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public enum PropHoldType
	{
		[Description("Fee Simple Title")]
		FeeSimple = 1,
		[Description("Leasehold Title")]
		Leasehold = 2
	}
}