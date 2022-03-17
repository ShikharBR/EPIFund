using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.Enum
{
	public enum DocumentType
	{
		[Description("NCND")]
		NCND = 1,
		[Description("IPA")]
		MDA = 2,
		[Description("Personal Financial Statement")]
		PersonalFinancialStatement = 3,
		[Description("IC Agreement")]
		ICAgreement = 4,
		[Description("JV Aggreement")]
		JVAgreement = 5,
		[Description("Binding Contingent")]
		BindingContingent = 6,
		[Description("LOI")]
		LOI = 7,
		[Description("Seller IPA")]
		SellerIPA = 8,
		[Description("NAR IPA")]
		NARIPA = 9
	}
}