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

		List<PortfolioQuickListViewModel> GetSearchPortfolios(ManagePortfoliosModel model);

		List<PortfolioQuickListViewModel> GetUserPortfolios(int UserId);

		bool isPortfolioNameDuplicate(PortfolioViewModel model);

		bool PortfolioExist(string portfolioName);

		bool PortfolioExist(Guid PortfolioId);

        List<PortfolioQuickListViewModel> SortPortfoliosModel(List<PortfolioQuickListViewModel> input, bool descending);

        List<PortfolioQuickListViewModel> TrimStringProperty(List<PortfolioQuickListViewModel> input);

        void UpdatePortfolio(PortfolioViewModel model, int UserId);
	}
}