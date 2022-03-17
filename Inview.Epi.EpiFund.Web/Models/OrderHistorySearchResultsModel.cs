using Inview.Epi.EpiFund.Domain.ViewModel;
using PagedList;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models
{
	public class OrderHistorySearchResultsModel
	{
		public PagedList.IPagedList<OrderModel> Completed
		{
			get;
			set;
		}

		public PagedList.IPagedList<OrderHistoryQuickListViewModel> History
		{
			get;
			set;
		}

		public PagedList.IPagedList<OrderModel> Pending
		{
			get;
			set;
		}

		public OrderHistorySearchResultsModel()
		{
		}
	}
}