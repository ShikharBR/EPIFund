using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Domain.Enum
{
    public enum DefineSellTerm
    {
        [Description("All Cash – no PMF")]
        AllCashNoConsi = 1,
        [Description("Cash & PMF")]
        AllCashConsi = 2,
        [Description("Cash & Assumption of existing Debt Packages")]
        CashAssump = 3,
        [Description("Cash & Seller Carryback with Assumption of Existing Debt Packages")]
        CashCarry = 4,
        [Description("Cash & Seller Carryback (Property was F&C of any Debt Packages)")]
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
