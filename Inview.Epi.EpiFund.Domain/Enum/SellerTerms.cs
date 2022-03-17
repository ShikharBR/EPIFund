using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.Enum
{
	/*public enum SellerTerms
	{
		[Description("All Cash (Contingent Debt/Equity Funding Package LOI will not be considered)")]
		AllCashNoConsi = 1,
		[Description("All Cash (will consider contingent Debt/Equity Funding Package LOI)")]
		AllCashConsi = 2,
		[Description("Cash & Assumption of existing Debt Package")]
		CashAssump = 3,
		[Description(" Cash & Carry some equity with Assumption of existing Debt Package")]
		CashCarry = 4,
		[Description("Property is F&C of Debt; Cash & Carry some equity; Terms Negotiable")]
		FandC = 5,
		[Description(" Submit Proposal")]
		SubmitProposal = 6
	}*/
    public enum SellerTerms
    {
        [Description("All Cash – no PMF")]
        AllCashNoConsi = 1,
        [Description("Cash & PMF")]
        AllCashConsi = 2,
        [Description("Cash & Assumption of existing Debt Package")]
        CashAssump = 3,
        [Description("Cash & Seller Carryback with Assumption of Existing Debt Package")]
        CashCarry = 4,
        [Description("Cash & Seller Carryback (Property was F&C of any Debt Package)")]
        FandC = 5,
        [Description(" Submit Proposal")]
        SubmitProposal = 6,
        [Description("Cash & Property for Property 1031 Exchange")]
        CashProperty = 7,
        [Description("Property for Property 1031 Exchange – No Cash Transfer")]
        Property = 8,
        [Description("Other")]
        Other = 9
    }
}