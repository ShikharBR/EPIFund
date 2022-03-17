using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public enum WasteWaterType
	{
		[Description("Public")]
		Public = 1,
		[Description("Septic")]
		Septic = 2,
		[Description("Private")]
		Private = 3
	}
}