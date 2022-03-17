using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.Enum
{
	public enum UserType
	{
		[Description("Nouser")]
		None =0,

		[Description("CRE Mtg Broker")]
		CREBroker = 1,

		[Description("CRE Mtg Lender")]
		CRELender = 2,

		[Description("IC Admin")]
		ICAdmin = 3,

		[Description("Corp Admin I")]
		CorpAdmin = 4,

		[Description("Principal Investor")]
		Investor = 5,

		[Description("Listing Agent")]
		ListingAgent = 6,

		[Description("Site Admin")]
		SiteAdmin = 7,

		[Description("Principal Owner")]
		CREOwner = 8,

		[Description("Corp Admin II")]
		CorpAdmin2 = 9,

		[Description("Title Comp Mgr")]
		TitleCompManager = 10,

		[Description("Title Comp Usr")]
		TitleCompUser = 11,

		[Description("CRE Mtg Note Owner")]
		CREMtgNoteOwner = 12,

		[Description("PC Insurance Manager")]
		PCInsuranceManager = 13
	}
}