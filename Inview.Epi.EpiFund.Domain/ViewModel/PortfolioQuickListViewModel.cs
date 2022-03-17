using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class PortfolioQuickListViewModel
	{
		public double? AccListPrice
		{
			get;
			set;
		}

		public double? AccSqFt
		{
			get;
			set;
		}

		public double? AccUnits
		{
			get;
			set;
		}

		public UserType? ControllingUserType
		{
			get;
			set;
		}

		public bool HasOffersDate
		{
			get;
			set;
		}

		public bool HasPrivileges
		{
			get;
			set;
		}

		public bool isActive
		{
			get;
			set;
		}

		public bool IsSelected
		{
			get;
			set;
		}

		public int NumberofAssets
		{
			get;
			set;
		}

		public List<PortfolioAssetsModel> PortfolioAssets
		{
			get;
			set;
		}

		public Guid PortfolioId
		{
			get;
			set;
		}

		public string PortfolioName
		{
			get;
			set;
		}

		public int TotalItemCount
		{
			get;
			set;
		}

		public PortfolioQuickListViewModel()
		{
		}
	}
}