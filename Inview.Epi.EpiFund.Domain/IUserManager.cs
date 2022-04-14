using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain
{
	public interface IUserManager
	{
		void ActivateTitleCompany(int titleId);

		void ActivateTitleUser(int userId);

		void ApproveICAdmin(int id);

		Tuple<bool, bool> CanUserBeReferred(string email);

		string CanUserManagePortfolios(string userName);

		string CanUserUploadMBA(string userName);

		void ChangePassword(int userId, string newPassword);

		void CreateMba(MbaViewModel model);

		int CreateNarMember(NarMemberViewModel model);

		void CreatePersonalFinancialStatement(PersonalFinancialStatementTemplateModel model);

		void CreatePrincipalInvestor(PrincipalInvestorQuickViewModel model);

		void CreateTitle(TitleCompanyModel model);

		int CreateTitleUser(TitleCompanyUserModel model);

		int CreateUser(RegistrationModel model);

		int CreateUser(string username, byte[] password, UserType userType);

		void DeactivateTitleCompany(int titleId);

		void DeactivateTitleUser(int userId);

		void DeactiveMba(int id);

		void DeactiveNarMember(int id);

		void DeactivePrincipalInvestor(int id);

		void DeleteAssetListingAgent(Guid id);

		void DeleteContractPayment(int id);

		void DeleteUser(int UserId);

		void DeleteUserFile(int userFileId);

		string GeneratePassword();

		List<UserModel> GetAdmins();

		List<UserAssetSearchCriteriaQuickViewModel> GetAllSearches();

		NarMemberViewModel GetAssetNarMember(Guid id);

		List<NarMemberViewModel> GetAssetNarMembers(NarMemberSearchModel model);

		List<AssetAssignmentQuickViewModel> GetAssetsAssigned(AssetAssignmentSearchModel searchModel);

		RecordContractPaymentModel GetContractPayment(int id);

		List<UserFileModel> GetFilesByUserId(int userId);

		AccountingRecordDisplayModel GetICAccountingReportDisplay(int userId, int fiscalYear, UserModel user);

		List<UserQuickViewModel> GetICAdmins(UserSearchModel searchModel);

		int GetImportedMBACount();

		int GetImportedNARCount();

		InsuranceCompanyUserViewModel GetInsuranceUserByEmail(string email);

		AccountingRecordDisplayModel GetJVMAAccountingReportDisplay(int userId, int fiscalYear, UserModel user);

		UserModel GetJVMAParticipantByUser(string username);

		List<UserQuickViewModel> GetJVMAParticipants(UserSearchModel model);

		JVMAUserMDAViewModels GetJVMAUserMDAHistory(JVMAUserMDASearchModel model);

		List<JVMANetUpViewModel> GetJVMAUserNetworkUploads(JVMANetUpSearchModel model, int userId);

		MbaViewModel GetMbaById(int id);

		List<MbaViewModel> GetMbas(MbaSearchModel model);

		DateTime? GetMDASignedDate(int userId);

		NarMemberViewModel GetNarMember(int id);

		List<NarMemberViewModel> GetNarMembers(NarMemberSearchModel model);

		List<NarMemberViewModel> GetNarMembersImported(NarMemberSearchModel model);

		List<OrderHistoryQuickListViewModel> GetOrdersForUser(int userId);

		User GetPayer(int year, int userId, ContractFeePayoutType type);

		PersonalFinancialStatementTemplateModel GetPersonalFinancialStatementByUserId(int userId);

		string GetPIAssetHistoryDescriptionsHTML(string username);

		PrincipalInvestorQuickViewModel GetPrincipalInvestor(int id);

        OperatingCompanyViewModel GetOperatingCompanyForAssetContract(Guid id);

        PrincipalInvestorQuickViewModel GetPrincipalInvestorByEmail(string email);

        List<PrincipalInvestorQuickViewModel> GetPrincipalInvestors(PrincipalInvestorSearchModel model);

        List<OperatingCompanyViewModel> GetOperatingCompanies(OperatingCompanySearchModel model);

        UserModel GetReferredByEmail(string username);

		int GetRegisteredMBACount();

		int GetRegisteredPICount();

		List<AssetSearchCriteriaQuickViewModel> GetSearchesForUser(int userId);

		byte[] GetSignedICAgreement(int userId);

		byte[] GetSignedJVAgreement(int userId);

		byte[] GetSignedNCND(int userId);

		TitleCompanyModel GetTitleById(int titleId);

		List<TitleQuickViewModel> GetTitleCompanies(CompanySearchModel searchModel);

		List<TitleUserQuickViewModel> GetTitleCompanyUsers(TitleUserSearchModel searchModel);

		List<TitleCompanyUser> GetTitleCompanyUsers();

		List<TitleCompanyUser> GetTitleCompUsersTitleId(int TitleId);

		TitleCompanyUserModel GetTitleUserByEmail(string email);

		TitleCompanyUserModel GetTitleUserById(int titleUserId);

		int GetTotalIndustryParticipants();

		int GetTotalNARCount();

		UserModel GetUserById(int userId);

		UserModel GetUserByUsername(string username);

		User GetUserEntity(string username);

		byte[] GetUserFile(int userFileId);

		UserFileModel GetUserFileById(int id);

		List<UserQuickViewModel> GetUsers(UserSearchModel model);

		byte[] HashPassword(string password);

		bool HasOrderHistory(int userId);

		bool HasPendingICAgreement(int userId);

		bool HasPendingJVAgreement(int userId);

		bool HasPendingMDA(int userId, int assetNumber);

		bool HasPendingNCND(int userId);

		string ImportMBAUsers(string path, int referringUserId, bool areReferredUsers, bool checkAgainstNARMembers = true, bool checkAgainstPrincipalInvestors = true);

		string ImportNARMembers(string path, int referringUserId, bool areReferredUsers, bool checkAgainstMBAs = true, bool checkAgainstPrincipalInvestors = true);

		ImportUsersModel ImportPrincipalInvestors(string path, int referringUserId, bool areReferredUsers, bool checkAgainstMBAs = true, bool CheckAgainstNARs = true);

		bool IsUserDisabled(string username);

		bool Login(string username, string password);

		void LogUserRecord(UserRecordType type, int userId);

		bool NARExist(string email);

		void PerformTediousTaskThatWouldTakeTooLongWithoutCode();

		void ReactivateMba(int id);

		void ReactivateNarMember(int id);

		void ReactivatePrincipalInvestor(int id);

		void ReactivateUser(int id);

		void RecordContractPayment(RecordContractPaymentModel model);

		string ReferUser(string email, string username);

		void ReinstateSellerPrivileges(int userId);

		void RejectICAdmin(int id);

		void RemoveUserAssetLocks(string Username);

		string ResetPassword(string username);

		void RevokeSellerPrivileges(int userId);

		void SetICInformation(ICAgreementTemplateModel model, int userId);

		bool TitleCompanyExists(string TitleCompName);

		bool TitleCompanyUserExists(string UserEmail);

		void UpdateAssignment(int assetNumber, int selectedUserId);

		void UpdateContractPayment(RecordContractPaymentModel model);

		void UpdateMba(MbaViewModel model);

		void UpdateNarMember(NarMemberViewModel model);

		void UpdateOrderStatus(int assetNumber, OrderStatus orderstatus, int titleCompanyUserId);

		void UpdatePersonalFinancialStatement(PersonalFinancialStatementTemplateModel model);

		void UpdatePrincipalInvestor(PrincipalInvestorQuickViewModel model);

        void UpdateTitle(TitleCompanyModel model);

		void UpdateTitleUser(TitleCompanyUserModel model);

		void UpdateUser(UserModel model);

		bool UserExists(string username);

		Tuple<bool, string> ValidateTitleUserStateAvailability(TitleCompanyUserModel model);

		bool VerifyNarMember(NarMemberViewModel model);
        


        bool CreateHoldingCompany(HoldingCompanyViewModel model);

        void DeleteHoldingCompany(Guid holdingCompanyId);

        HoldingCompanyUpdateResult UpdateHoldingCompany(HoldingCompanyViewModel model);

        void DeactivateHoldingCompany(Guid id);

        void UpdateOperatingCompany(OperatingCompanyViewModel model);

        OperatingCompanyViewModel GetOperatingCompany(Guid id);

        bool CreateOperatingCompany(OperatingCompanyViewModel model);

        void ReactivateOperatingCompany(Guid id);

        void DeactivateOperatingCompany(Guid id);

        void ReactivateHoldingCompany(Guid id);

        Dictionary<string, string> SearchCompanies(string term, string type);

        IEnumerable<SelectListItem> PopulateStateList();


		HoldingCompanyViewModel GetHoldingCompany(Guid holdingCompanyId);

		List<HoldingCompanyViewModel> GetHoldingCompaniesNew();

		List<HoldingCompanyViewModel> GetHoldingCompaniesForOperatingCompany(Guid id);

		List<HoldingCompanyList> GetHoldingCompanies(ManageHoldingCompanyModel model);

		List<HoldingCompanyList> GetHoldingCompany(ManageHoldingCompanyModel model);

		HoldingCompany GetHoldingCompanybyId(Guid id);

		OperatingCompany GetOpertingCompanybyId(Guid id);

		List<OperatingCompanyList> GetOperataingCompany(ManageOperatingCompanyModel model);

		OperatingCompanyViewModel GetOPeratingCompany(Guid operatingCompanyId);


	}
}