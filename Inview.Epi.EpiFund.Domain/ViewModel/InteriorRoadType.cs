using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public enum InteriorRoadType
	{
		[Description("Paved Road")]
		Paved = 1,
		[Description("Gravel Road")]
		Gravel = 2
	}
}