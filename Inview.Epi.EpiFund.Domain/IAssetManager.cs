using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain
{
	public interface IAssetManager
	{
		AssetDynamicViewModel SearchAssetsForSearch(SearchAssetModel searchModel, int? userId);
		void AddAssetIdsToMDA(int userId, List<Guid> assetIds);

		void AddAssetIdToMDA(int userId, Guid assetId);

		void AddImageToAsset(byte[] image, Guid assetId);

		void AddImageToAsset(byte[] image, bool isFlyer, bool isMain, Guid assetId);

		void AddImageToAsset(string image, bool isFlyer, bool isMain, Guid assetId, int order);

		bool AddUserToOrderHistory(Guid assetId, string email, AssetOrderHistoryType type);

		void AssociateUserToSearchCriteriaForm(int userId, int formId);

		double CalculateCommission(Guid assetId);

		void ChangeListingStatus(Guid assetId, ListingStatus newStatus);

		void CloseEscrow(Guid assetId, DateTime actualClosingDate, double closingPrice);

		void ConfirmAssetUserDisclosure(Guid assetId, int userId);

		int? CreateAssetByViewModel(AssetViewModel model);

		int CreateAssetSearchCriteria(AssetSearchCriteriaModel model);

		void CreateUserNote(UserNoteModel model);

		void DeleteAsset(Guid assetId);

		void DeleteAssetFile(string fileId, Guid assetId, FileType filetype);

		void DeleteStrandedVideosAndConvertMp4s();

		void DeleteTitleOrderPayment(int id);

		void DeleteUserFile(int userFileId);

		void DeleteUserNote(int userNoteId);

		bool DoesAssetExist(int assetNumber);

		void EnterBindingEscrow(Guid assetId, DateTime projectedClosingDate);

		AssetViewModel GenerateNewAsset(bool isPaperAsset, AssetType type);

		AssetViewModel GetAsset(Guid assetId, bool forScrollingImages);

		AssetDescriptionModel GetAssetByAssetId(Guid assetId);

		AssetDescriptionModel GetAssetByAssetNumber(int assetNumber);

		AssetDocumentOrderRequestModel GetAssetDocumentOrderRequest(Guid assetId, int titleCompanyId, int userId, bool finalized);

		List<Asset> GetAssetList();

		int GetAssetLockUserId(Guid assetId);

		List<AssetQuickListViewModel> GetAssetQuickViewList(AssetSearchModel model);

		List<PortfolioQuickListViewModel> GetAssetQuickViewListPF(AssetSearchModel model);

		AssetSearchCriteriaModel GetAssetSearchCriteriaById(int id);

		List<CarouselAssetData> GetCarouselData();

		int GetCurrentActiveAssetCount();

		List<AssetDeferredItemViewModel> GetDefaultDeferredMaintenanceItems();

		List<SelectListItem> GetEscrowCompanies();

		List<DeferredMaintenanceCost> GetMaintenanceCosts();

		List<PortfolioQuickListViewModel> GetManageAssetQuickListPF(ManageAssetsModel results);

		List<AdminAssetQuickListModel> GetManageAssetsQuickList(ManageAssetsModel model);

		List<PortfolioQuickListModel> GetAssetRealetdPortfolioList(string assetId);

		List<AdminAssetQuickListModel> GetAssetByPortfolioId(string PortfolioId);

		List<OrderModel> GetOrdersForAdmin(OrderSearchModel model);

		PaperCommercialAssetViewModel GetPaperCommercialModel(int assetId);

		PaperResidentialAssetViewModel GetPaperResidentialModel(int assetId);

		int GetPublishedAssetCount();

		int GetPublishedCommercialAssetCount();

		double GetPublishedCommercialAssetValue();

		RealEstateCommercialAssetViewModel GetRealEstateCommercialModel(int assetId);

		RealEstateResidentialAssetViewModel GetRealEstateResidentialModel(int assetId);

		List<AssetDescriptionModel> GetRelatedAssets(List<Guid> assetIds);

		AssetViewModel GetSampleAsset(bool forScrollingImages);

		List<SellerAssetQuickListModel> GetSellerManageAssetsQuickList(ManageAssetsModel model);

		string GetSignedMDALocation(int assetUserMdaId);

		List<SignedMDAQuickViewModel> GetSignedMDAs(int userId);

		List<OrderModel> GetTitleCompanyOrders(OrderSearchModel model);

		OrderModel GetTitleOrderPayment(int titleOrderPaymentId);

		int GetTotalNumberOfAssets();

		int GetTotalNumberOfMultiFamilyUnits();

		int GetTotalSumCommercialSqFt();

		byte[] GetUserFile(int userfileId);

		Tuple<string, string> GetUserInformationForHeldAsset(Guid assetId);

		UserNoteModel GetUserNote(int noteId);

		bool HasSellerSignedIPA(Guid assetId, int userId);

		bool HasSignedMDA(int userId);

		void HoldAssetForUser(int userId, int assetNumber);

		bool IsAssetClosing(Guid id);

		bool IsAssetLocked(Guid assetId, int userId);

		void LockAsset(int userId, Guid assetId);

		void MarkDocumentViewingStatus(AssetDocument doc, bool status);

		List<AssetDeferredItemViewModel> OrderMaintainanceItems(AssetType type, List<AssetDeferredItemViewModel> maintainanceList);

		List<AdminAssetQuickListModel> PopulateAssetQuickList(List<AdminAssetQuickListModel> incompleteAssets);

        AssetViewModel PopulateDocumentOrder(AssetViewModel model);

        void PublishAsset(Guid assetId);

		void RecordAssetPayment(RecordPaymentModel model);

		void RecordCommissionPayment(RecordCommissionPaymentModel model);

		void RecordTitleOrderPayment(double currentRate, DateTime datePaid, int titleCompanyId, Guid assetId, int userId);

		List<AssetDeferredItemViewModel> RemoveMaintainanceItems(AssetType type, List<AssetDeferredItemViewModel> maintainanceList);

		AssetDocumentOrderRequestModel RequestDocuments(Guid assetId, int userId, int titleCompanyId, bool finalized);

		void SaveLOI(BindingContingentTemplateModel model, int userId);

		int SavePaperCommercial(PaperCommercialAssetViewModel model);

		int SavePaperResidential(PaperResidentialAssetViewModel model);

		int SaveRealEstateCommercial(RealEstateCommercialAssetViewModel model);

		int SaveRealEstateResidential(RealEstateResidentialAssetViewModel model);

		void SaveUserFile(byte[] file, string desc, int userId);

		void SaveUserNote(UserNoteModel model);

		void SendChangeListingStatusEmails(ChangeListingStatusModel model);

		void SetSampleAsset(Guid id);

		bool SignedMDAWithAssetId(int userId, Guid assetId);

		void UnlockAsset(Guid assetId);

		void UnPublishAsset(Guid assetId);

		bool UpdateAssetByViewModel(AssetViewModel model);

		bool UpdateAssetDocuments(List<AssetDocument> docs, Guid assetId);

		void UpdateAssetSearchCriteria(AssetSearchCriteriaModel model);

		void UpdateTitleOrderPayment(OrderModel model);

		bool UserConfirmedAssetDisclosure(Guid assetId, int userid);

		List<AssetAPNMatchModel> GetMatchingAssetsByAPNCountyState(string parcelNumber, string state, string county);

        bool CreateAssetOwnershipChange(AssetOwnershipChangeViewModel model);

        byte[] GetAssetSpreadsheet();

        void SaveAssetVideos(List<AssetVideo> videos);

		void ActivateAsset(string assetId);
		void DeActivateAsset(string assetId);
		void ActivateNarMember(int narMemberId);
		void DeActivateNarMember(int narMemberId);
		List<AssetHCOwnershipModel> GetAssetHCByAssetId(Guid assetId);
        void SaveUpdateAssetHC(AssetHCOwnership assetHC);		
		AssetHCOwnershipModel GetAssetHCByAssetHCOwnershipId(int assetHCOwnershipId);

		List<AssetOCModel> GetAssetOCByAssetId(Guid assetId);

		void SaveUpdateAssetOC(HCCAssetOC assetOC);

		AssetOCModel GetAssetOCByAssetOCId(int assetOCId);

		AssetOCAddressModel GetHCAddressByAssetId(Guid assetId);

		List<ChainOfTitleQuickListModel> GetChainOfTitleByAssetId(string AssetId);

		List<AdminAssetQuickListModel> GetAssetsbyHCId(string HcId);

		List<AdminAssetQuickListModel> GetAssetsbyOCId(string OcId);

		bool CheckHCDate(DateTime date, Guid assetId);

	}
}