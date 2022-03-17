using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.Enum
{
	public enum CorporateEntityType
	{
		[Description("Corporation")]
		Corporation = 1,
		[Description("Limited Liability Company")]
		LimitedLiabilityCompany = 2,
		[Description("Limited Liability Partnership")]
		LimitedLiabilityPartnership = 3,
		[Description("Joint Venture")]
		JointVenture = 4,
		[Description("Sole Proprietorship")]
		SoleProprietorship = 5
	}
}