using Inview.Epi.EpiFund.Domain.Entity;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Inview.Epi.EpiFund.Domain
{
	public interface IEPIRepository
	{
		IDbSet<AssetCommission> AssetCommissions
		{
			get;
			set;
		}

		IDbSet<AssetDeferredMaintenanceItem> AssetDeferredMaintenanceItems
		{
			get;
			set;
		}

		IDbSet<AssetDocument> AssetDocuments
		{
			get;
			set;
		}

		IDbSet<AssetImage> AssetImages
		{
			get;
			set;
		}

		IDbSet<AssetListingAgent> AssetListingAgents
		{
			get;
			set;
		}

		IDbSet<AssetLock> AssetLocks
		{
			get;
			set;
		}

		IDbSet<AssetMHPSpecification> AssetMHPSpecifications
		{
			get;
			set;
		}

		IDbSet<AssetNARMember> AssetNARMembers
		{
			get;
			set;
		}

		IDbSet<AssetOrderHistory> AssetOrderHistories
		{
			get;
			set;
		}

		IDbSet<Asset> Assets
		{
			get;
			set;
		}

		IDbSet<AssetSalePayment> AssetSalePayments
		{
			get;
			set;
		}

		IDbSet<AssetSearchCriteria> AssetSearchCriterias
		{
			get;
			set;
		}

		IDbSet<AssetSeller> AssetSellers
		{
			get;
			set;
		}

		IDbSet<AssetTaxParcelNumber> AssetTaxParcelNumbers
		{
			get;
			set;
		}

		IDbSet<AssetUnitSpecification> AssetUnitSpecifications
		{
			get;
			set;
		}

		IDbSet<AssetUserDisclosure> AssetUserDisclosures
		{
			get;
			set;
		}

		IDbSet<AssetUserMDA> AssetUserMDAs
		{
			get;
			set;
		}

		IDbSet<AssetUserView> AssetUserViews
		{
			get;
			set;
		}

		IDbSet<AssetVideo> AssetVideos
		{
			get;
			set;
		}

		IDbSet<ContractFeePayout> ContractFeePayouts
		{
			get;
			set;
		}

		IDbSet<DeferredMaintenanceCost> DeferredMaintenanceCosts
		{
			get;
			set;
		}

		IDbSet<DocusignEnvelope> DocusignEnvelopes
		{
			get;
			set;
		}

		IDbSet<EmailSchedule> EmailSchedules
		{
			get;
			set;
		}

		IDbSet<EscrowCompany> EscrowCompanies
		{
			get;
			set;
		}

		IDbSet<ICAdminContractFeePayout> ICAdminContractFeePayouts
		{
			get;
			set;
		}

		IDbSet<Inquiry> Inquiries
		{
			get;
			set;
		}

		IDbSet<LOIDocument> LOIDocuments
		{
			get;
			set;
		}

		IDbSet<LOI> LOIs
		{
			get;
			set;
		}

		IDbSet<MBAUser> MbaUsers
		{
			get;
			set;
		}

		IDbSet<NARMember> NarMembers
		{
			get;
			set;
		}

		IDbSet<PaperAsset> PaperAssets
		{
			get;
			set;
		}

		IDbSet<PaperCommercialAsset> PaperCommercialAssets
		{
			get;
			set;
		}

		IDbSet<PaperResidentialAsset> PaperResidentialAssets
		{
			get;
			set;
		}

		IDbSet<PCInsuranceCompany> PCInsuranceCompanies
		{
			get;
			set;
		}

		IDbSet<PCInsuranceCompanyManager> PCInsuranceCompanyManagers
		{
			get;
			set;
		}

		IDbSet<PersonalFinancialStatement> PersonalFinancialStatements
		{
			get;
			set;
		}

		IDbSet<PortfolioAsset> PortfolioAssets
		{
			get;
			set;
		}

		IDbSet<Portfolio> Portfolios
		{
			get;
			set;
		}

		IDbSet<PrincipalInvestor> PrincipalInvestors
		{
			get;
			set;
		}

		IDbSet<RealEstateCommercialAsset> RealEstateCommercialAssets
		{
			get;
			set;
		}

		IDbSet<RealEstateResidentialAsset> RealEstateResidentialAssets
		{
			get;
			set;
		}

		IDbSet<SearchCriteriaDemographicDetail> SearchCriteriaDemographicDetails
		{
			get;
			set;
		}

		IDbSet<SearchCriteriaDueDiligenceItem> SearchCriteriaDueDiligenceItems
		{
			get;
			set;
		}

		IDbSet<SearchCriteriaGeographicParameter> SearchCriteriaGeographicParameters
		{
			get;
			set;
		}

		IDbSet<TempAssetDocument> TempAssetDocuments
		{
			get;
			set;
		}

		IDbSet<TempAssetImage> TempAssetImages
		{
			get;
			set;
		}

		IDbSet<TempAssetVideo> TempAssetVideos
		{
			get;
			set;
		}

		IDbSet<TempDeferredMaintenanceItem> TempDeferredMaintenanceItems
		{
			get;
			set;
		}

		IDbSet<TempUnitSpecification> TempUnitSpecifications
		{
			get;
			set;
		}

		IDbSet<TitleCompany> TitleCompanies
		{
			get;
			set;
		}

		IDbSet<TitleCompanyUser> TitleCompanyUsers
		{
			get;
			set;
		}

		IDbSet<TitleOrderPayment> TitleOrderPayments
		{
			get;
			set;
		}

		IDbSet<UserFile> UserFiles
		{
			get;
			set;
		}

		IDbSet<UserMachine> UserMachines
		{
			get;
			set;
		}

		IDbSet<UserNote> UserNotes
		{
			get;
			set;
		}

		IDbSet<UserRecord> UserRecords
		{
			get;
			set;
		}

		IDbSet<UserReferral> UserReferrals
		{
			get;
			set;
		}

		IDbSet<User> Users
		{
			get;
			set;
		}

        IDbSet<HoldingCompany> HoldingCompanies
        {
            get;
            set;
        }

        IDbSet<OperatingCompany> OperatingCompanies
        {
            get;
            set;
        }

        IDbSet<AssetOwnershipChange> AssetOwnershipChanges
        {
            get;
            set;
        }


        DbEntityEntry Entry(object entity);

		void Initialize();

		int Save();

		void SendSqlToDebugWindow(bool enabled);

		IDbSet<AssetHCOwnership> AssetHCOwnership
		{
			get;
			set;
		}

		IDbSet<AssetOC> AssetOC
		{
			get;
			set;
		}


	}
}