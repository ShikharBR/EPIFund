using System;
using System.ComponentModel;

namespace Inview.Epi.EpiFund.Domain.Enum
{
	public enum AssetDocumentType
	{
		[Description("Current Rent Roll")]
		CurrentRentRoll,
		[Description("Current Operating Report")]
		CurrentOperatingReport,
		[Description("Prior Fiscal Year Oper Report")]
		PriorFiscalYearOperReport,
		[Description("Preliminary Title Report")]
		PreliminaryTitleReport,
		[Description("Plat Map")]
		PlatMap,
		[Description("Arial Map")]
		ArialMap,
		[Description("Original Appraisal")]
		OriginalAppraisal,
		[Description("Current Appraisal")]
		CurrentAppraisal,
		[Description("Listing Agent Marketing Brochure")]
		ListingAgentMarketingBrochure,
		[Description("Other")]
		Other,
		[Description("Mortgage Instrument of Record")]
		MortgageInstrumentOfRecord,
		[Description("Recorded Liens")]
		RecordedLiens,
		[Description("Tax Liens")]
		TaxLiens,
		[Description("BK Related Filings")]
		BKRelated,
		[Description("Title - Preliminary Title Report")]
		PreliminaryTitleReportTitle,
		[Description("Recorded DOTs & MTGs")]
		DOTMTG,
		[Description("Other Document - Title")]
		OtherTitle,
		[Description("P&C Insurance Coverage Quote")]
		Insurance,
        [Description("Other Insurance Coverage Related")]
        InsuranceOther,
        [Description("Phase 1 Environmental Report")]
        EnvironmentalReport
    }
}