using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Collections.Generic;

namespace Inview.Epi.EpiFund.Domain
{
	public interface IInsuranceManager
	{
		void ActivateInsuranceCompany(int id);

		void ActivateInsuranceCompanyUser(int id);

		bool CompanyExist(string companyName);

		void CreateInsuranceCompany(InsuranceCompanyViewModel model);

		int CreateInsuranceCompanyUser(InsuranceCompanyUserViewModel model);

		void DeactivateInsuranceCompany(int id);

		void DeactivateInsuranceCompanyUser(int id);

		AssetDocumentOrderRequestModel GetEmailingModel(Guid assetId, int managerId);

		List<InsuranceCompanyViewModel> GetInsuranceCompanies(CompanySearchModel model);

		InsuranceCompanyViewModel GetInsuranceCompany(int id);

		List<OrderModel> GetInsuranceCompanyOrders(OrderSearchModel search);

		InsuranceCompanyUserViewModel GetInsuranceCompanyUser(int id);

		List<InsuranceCompanyUserViewModel> GetInsuranceCompanyUsers(CompanyUserSearchModel model);

		AssetDocumentOrderRequestModel RequestDocuments(Guid assetId, int userId, int titleCompanyId, bool finalized);

		void UpdateInsuranceCompany(InsuranceCompanyViewModel model);

		void UpdateInsuranceCompanyUser(InsuranceCompanyUserViewModel model);

		void UpdateOrderStatus(Guid assetId, OrderStatus orderStatus, int managerId);

		bool ValidateUser(ValidateUserModel model);

		bool VerifyUserType(string email);
	}
}