using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public enum WaterServType
	{
		[Description("Private")]
		Private = 1,
		[Description("Public")]
		Public = 2,
		[Description("On-Site")]
		OnSite = 3
	}
}