using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Collections.Generic;

namespace Inview.Epi.EpiFund.Domain
{
	public interface ISellerManager
	{
		List<SellerIPAReceivedViewModel> GetSellerIPAsReceived(int userId);

		List<SellerLOIReceivedViewModel> GetSellerLOIsReceived(int userId);

		int GetUnreadLOICount(int userId);

		void MarkLOIAsRead(Guid loiId);
	}
}