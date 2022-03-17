using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.Enum
{
	public enum LOIDocumentType
	{
		[Description("Operating And Management Resume")]
		OperatingManagementResume = 1,
		[Description("Proof of Funds from the Corporate Vesting Entity")]
		ProofOfFunds = 2,
		[Description("Personal Financial Statement")]
		PersonalFinancialStatement = 3
	}
}