using Inview.Epi.EpiFund.Data.Migrations;
using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.Entity;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;


namespace Inview.Epi.EpiFund.Data
{
    public class EPIRepository : DbContext, IEPIRepository
    {
        public virtual IDbSet<AssetCommission> AssetCommissions
        {
            get;
            set;
        }

        public virtual IDbSet<AssetDeferredMaintenanceItem> AssetDeferredMaintenanceItems
        {
            get;
            set;
        }

        public virtual IDbSet<AssetDocument> AssetDocuments
        {
            get;
            set;
        }

        public virtual IDbSet<AssetImage> AssetImages
        {
            get;
            set;
        }

        public virtual IDbSet<AssetListingAgent> AssetListingAgents
        {
            get;
            set;
        }

        public virtual IDbSet<AssetLock> AssetLocks
        {
            get;
            set;
        }

        public virtual IDbSet<AssetMHPSpecification> AssetMHPSpecifications
        {
            get;
            set;
        }

        public virtual IDbSet<AssetNARMember> AssetNARMembers
        {
            get;
            set;
        }

        public virtual IDbSet<AssetOrderHistory> AssetOrderHistories
        {
            get;
            set;
        }

        public virtual IDbSet<Asset> Assets
        {
            get;
            set;
        }

        public virtual IDbSet<AssetSalePayment> AssetSalePayments
        {
            get;
            set;
        }

        public virtual IDbSet<AssetSearchCriteria> AssetSearchCriterias
        {
            get;
            set;
        }

        public virtual IDbSet<AssetSeller> AssetSellers
        {
            get;
            set;
        }

        public virtual IDbSet<AssetTaxParcelNumber> AssetTaxParcelNumbers
        {
            get;
            set;
        }

        public virtual IDbSet<AssetUnitSpecification> AssetUnitSpecifications
        {
            get;
            set;
        }

        public virtual IDbSet<AssetUserDisclosure> AssetUserDisclosures
        {
            get;
            set;
        }

        public virtual IDbSet<AssetUserMDA> AssetUserMDAs
        {
            get;
            set;
        }

        public virtual IDbSet<AssetUserView> AssetUserViews
        {
            get;
            set;
        }

        public virtual IDbSet<AssetVideo> AssetVideos
        {
            get;
            set;
        }

        public virtual IDbSet<ContractFeePayout> ContractFeePayouts
        {
            get;
            set;
        }

        public virtual IDbSet<DeferredMaintenanceCost> DeferredMaintenanceCosts
        {
            get;
            set;
        }

        public virtual IDbSet<DocusignEnvelope> DocusignEnvelopes
        {
            get;
            set;
        }

        public virtual IDbSet<EmailSchedule> EmailSchedules
        {
            get;
            set;
        }

        public virtual IDbSet<EscrowCompany> EscrowCompanies
        {
            get;
            set;
        }

        public virtual IDbSet<ICAdminContractFeePayout> ICAdminContractFeePayouts
        {
            get;
            set;
        }

        public virtual IDbSet<Inquiry> Inquiries
        {
            get;
            set;
        }

        public virtual IDbSet<LOIDocument> LOIDocuments
        {
            get;
            set;
        }

        public virtual IDbSet<LOI> LOIs
        {
            get;
            set;
        }

        public virtual IDbSet<MBAUser> MbaUsers
        {
            get;
            set;
        }

        public virtual IDbSet<NARMember> NarMembers
        {
            get;
            set;
        }

        public virtual IDbSet<PaperAsset> PaperAssets
        {
            get;
            set;
        }

        public virtual IDbSet<PaperCommercialAsset> PaperCommercialAssets
        {
            get;
            set;
        }

        public virtual IDbSet<PaperResidentialAsset> PaperResidentialAssets
        {
            get;
            set;
        }

        public virtual IDbSet<PCInsuranceCompany> PCInsuranceCompanies
        {
            get;
            set;
        }

        public virtual IDbSet<PCInsuranceCompanyManager> PCInsuranceCompanyManagers
        {
            get;
            set;
        }

        public virtual IDbSet<PersonalFinancialStatement> PersonalFinancialStatements
        {
            get;
            set;
        }

        public virtual IDbSet<PortfolioAsset> PortfolioAssets
        {
            get;
            set;
        }

        public virtual IDbSet<Portfolio> Portfolios
        {
            get;
            set;
        }

        public virtual IDbSet<PrincipalInvestor> PrincipalInvestors
        {
            get;
            set;
        }

        public virtual IDbSet<RealEstateCommercialAsset> RealEstateCommercialAssets
        {
            get;
            set;
        }

        public virtual IDbSet<RealEstateResidentialAsset> RealEstateResidentialAssets
        {
            get;
            set;
        }

        public virtual IDbSet<SearchCriteriaDemographicDetail> SearchCriteriaDemographicDetails
        {
            get;
            set;
        }

        public virtual IDbSet<SearchCriteriaDueDiligenceItem> SearchCriteriaDueDiligenceItems
        {
            get;
            set;
        }

        public virtual IDbSet<SearchCriteriaGeographicParameter> SearchCriteriaGeographicParameters
        {
            get;
            set;
        }

        public virtual IDbSet<TempAssetDocument> TempAssetDocuments
        {
            get;
            set;
        }

        public virtual IDbSet<TempAssetImage> TempAssetImages
        {
            get;
            set;
        }

        public virtual IDbSet<TempAssetVideo> TempAssetVideos
        {
            get;
            set;
        }

        public virtual IDbSet<TempDeferredMaintenanceItem> TempDeferredMaintenanceItems
        {
            get;
            set;
        }

        public virtual IDbSet<TempUnitSpecification> TempUnitSpecifications
        {
            get;
            set;
        }

        public virtual IDbSet<TitleCompany> TitleCompanies
        {
            get;
            set;
        }

        public virtual IDbSet<TitleCompanyUser> TitleCompanyUsers
        {
            get;
            set;
        }

        public virtual IDbSet<TitleOrderPayment> TitleOrderPayments
        {
            get;
            set;
        }

        public virtual IDbSet<UserFile> UserFiles
        {
            get;
            set;
        }

        public virtual IDbSet<UserMachine> UserMachines
        {
            get;
            set;
        }

        public virtual IDbSet<UserNote> UserNotes
        {
            get;
            set;
        }

        public virtual IDbSet<UserRecord> UserRecords
        {
            get;
            set;
        }

        public virtual IDbSet<UserReferral> UserReferrals
        {
            get;
            set;
        }

        public virtual IDbSet<User> Users
        {
            get;
            set;
        }

        public virtual IDbSet<HoldingCompany> HoldingCompanies
        {
            get;
            set;
        }

        public virtual IDbSet<OperatingCompany> OperatingCompanies
        {
            get;
            set;
        }

        public virtual IDbSet<AssetOwnershipChange> AssetOwnershipChanges
        {
            get;
            set;
        }

        public EPIRepository()
        {
            System.Data.Entity.Database.SetInitializer<EPIRepository>(new MigrateDatabaseToLatestVersion<EPIRepository, Inview.Epi.EpiFund.Data.Migrations.Configuration>());
        }

        public void Initialize()
        {
            base.Database.Initialize(false);
        }

        DbEntityEntry Inview.Epi.EpiFund.Domain.IEPIRepository.Entry(object obj)
        {
            return base.Entry(obj);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Configurations.Add<AssetImage>(new ModelConfigurationClasses.AssetImageConfiguration());
        }

        public int Save()
        {
            return this.SaveChanges();
        }

        public void SendSqlToDebugWindow(bool enabled)
        {
            if (!enabled)
            {
                base.Database.Log = null;
            }
            else
            {
                base.Database.Log = (string s) => Debug.WriteLine(s);
            }
        }

        public virtual IDbSet<AssetHCOwnership> AssetHCOwnership
        {
            get;
            set;
        }
        public virtual IDbSet<AssetOC> AssetOC
        {
            get;
            set;
        }

        public virtual IDbSet<AssetMessage> AssetMessages { get; set; }
        public virtual IDbSet<FavoriteGroup> FavoriteGroups { get; set; }

        public virtual IDbSet<FavoriteGroupAsset> FavoriteGroupAssets { get; set; }

        public virtual IDbSet<SavedAssetSearch> SavedAssetSearches { get; set; }
    }
}