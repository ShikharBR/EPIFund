using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Collections.Generic;

namespace Inview.Epi.EpiFund.Domain
{
	public interface IPortfolioManager
	{
		void ActivateAsset(Guid AssetId, Guid PfId);

		void ActivatePortfolio(Guid PortfolioId);

		Guid CreatePortfolio(PortfolioViewModel model, int UserId);

		void DeactivateAsset(Guid AssetId, Guid PfId);

		void DeactivatePortfolio(Guid PortfolioId);

		PortfolioViewModel GetPortfolio(Guid PortfolioId);

		List<AssetViewModel> GetPortfolioProperties(Guid PortfolioId);

		List<PortfolioQuickListModel> GetSearchPortfolios(ManagePortfoliosModel model);

		List<PortfolioQuickListViewModel> GetUserPortfolios(int UserId);

		bool isPortfolioNameDuplicate(PortfolioViewModel model);

		bool PortfolioExist(string portfolioName);

		bool PortfolioExist(Guid PortfolioId);

        List<PortfolioQuickListModel> SortPortfoliosModel(List<PortfolioQuickListModel> input, bool descending);

        List<PortfolioQuickListModel> TrimStringProperty(List<PortfolioQuickListModel> input);

        void UpdatePortfolio(PortfolioViewModel model, int UserId);

		List<AdminAssetQuickListModel> GetAssetsByAssetsIds(string portfolioId, List<AdminAssetQuickListModel> assetsIds);
	}
}