using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.Enum
{
	public enum OperatingStatus
	{
		FDIC = 1,
		[Description("In Default - FDIC Control")]
		DefaultFDIC = 2,
		[Description("Pending Foreclosure")]
		PendingForeclosure = 3,
		[Description("Private - In Default")]
		PrivateDefault = 4,
		[Description("Private - Not In Default")]
		PrivateNotDefault = 5,
		REO = 6
	}
}