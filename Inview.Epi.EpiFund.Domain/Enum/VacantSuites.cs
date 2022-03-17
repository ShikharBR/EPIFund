using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.Enum
{
	public enum VacantSuites
	{
		[Description("All Shell Space")]
		AllShellSpace = 1,
		[Description("Some Shell Space")]
		SomeShellSpace = 2,
		[Description("Some Prior Occupied, Now Vacant Space")]
		AllPriorBuiltOutSpace = 3,
		[Description("100% Occupied and Built Out")]
		OccupiedAndBuiltOut = 4,
		[Description("Property 100% Built Out")]
		PropertyBuiltOut = 5
	}
}