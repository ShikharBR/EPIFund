using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class AssetSearchCriteria
	{
		public string AddressLine1OfPurchasingEntity
		{
			get;
			set;
		}

		public string AddressLine2OfPurchasingEntity
		{
			get;
			set;
		}

		public decimal AmountOfIntendedCapEquity
		{
			get;
			set;
		}

		public int AssetSearchCriteriaId
		{
			get;
			set;
		}

		public string BrandNewDetails
		{
			get;
			set;
		}

		public string CellNumberOfEntity
		{
			get;
			set;
		}

		public string CityOfPurchasingEntity
		{
			get;
			set;
		}

		public string ContactNumberOfCorporateOfficer
		{
			get;
			set;
		}

		public string ContactNumberOfOtherCorporateOfficer
		{
			get;
			set;
		}

		public string CREPMFBrokerLender
		{
			get;
			set;
		}

		public DateTime DateEntered
		{
			get;
			set;
		}

		public List<SearchCriteriaDemographicDetail> DemographicDetails
		{
			get;
			set;
		}

		public List<SearchCriteriaDueDiligenceItem> DueDiligenceItems
		{
			get;
			set;
		}

		public string EmailAddressOfCorporateOfficer
		{
			get;
			set;
		}

		public string EmailAddressOfEntity
		{
			get;
			set;
		}

		public string EmailAddressOfOtherCorporateOfficer
		{
			get;
			set;
		}

		public string FaxNumberOfEntity
		{
			get;
			set;
		}

		public string GeneralNotesOfVestingEntity
		{
			get;
			set;
		}

		public List<SearchCriteriaGeographicParameter> GeographicParameters
		{
			get;
			set;
		}

		public bool HasEntityRaisedIntendedCap
		{
			get;
			set;
		}

		public decimal InvestmentFundingRangeMax
		{
			get;
			set;
		}

		public decimal InvestmentFundingRangeMin
		{
			get;
			set;
		}

		public bool IsCorporateEntityInGoodStanding
		{
			get;
			set;
		}

		public DateTime LastUpdated
		{
			get;
			set;
		}

		public decimal LeverageTarget
		{
			get;
			set;
		}

		public string ManagingOfficerOfEntity
		{
			get;
			set;
		}

		public decimal MinimumCapitalizationRate
		{
			get;
			set;
		}

		public string NameOfOtherCorporateOfficer
		{
			get;
			set;
		}

		public string NameOfOtherCorporateOfficer2
		{
			get;
			set;
		}

		public string NameOfPurchasingEntity
		{
			get;
			set;
		}

		public string OfficeNumberOfEntity
		{
			get;
			set;
		}

		public string OtherCorporateOfficer
		{
			get;
			set;
		}

		public string PartiallyBuiltDetails
		{
			get;
			set;
		}

		public string ProFormaParametersDetails
		{
			get;
			set;
		}

		public bool SecuredCLAPOF
		{
			get;
			set;
		}

		public string StateOfIncorporation
		{
			get;
			set;
		}

		public string StateOfPurchasingEntity
		{
			get;
			set;
		}

		public decimal TargetPricePerSpaceMax
		{
			get;
			set;
		}

		public decimal TargetPricePerSpaceMin
		{
			get;
			set;
		}

		public decimal TargetPricePerUnitMax
		{
			get;
			set;
		}

		public decimal TargetPricePerUnitMin
		{
			get;
			set;
		}

		public string TermsSought
		{
			get;
			set;
		}

		public string TimelineSecuringCap
		{
			get;
			set;
		}

		public AssetType TypeOfAssetsSought
		{
			get;
			set;
		}

		public CorporateEntityType TypeOfPurchasingEntity
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Entity.User User
		{
			get;
			set;
		}

		public int? UserId
		{
			get;
			set;
		}

		public bool UtilizePMFunding
		{
			get;
			set;
		}

		public string WebsiteURLVestingCorporateEntity
		{
			get;
			set;
		}

		public bool WillConsiderBrandNew
		{
			get;
			set;
		}

		public bool WillConsiderPartiallyBuilt
		{
			get;
			set;
		}

		public bool WillConsiderUnperformingAtPricing
		{
			get;
			set;
		}

		public string ZipOfPurchasingEntity
		{
			get;
			set;
		}

		public AssetSearchCriteria()
		{
		}
	}
}