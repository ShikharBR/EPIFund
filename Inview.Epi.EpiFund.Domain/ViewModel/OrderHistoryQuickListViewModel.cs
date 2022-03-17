using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class OrderHistoryQuickListViewModel
	{
		public string Address
		{
			get;
			set;
		}

		public double? AmountDue
		{
			get;
			set;
		}

		public string AssetNumber
		{
			get;
			set;
		}

		public DateTime OrderDate
		{
			get;
			set;
		}

		public bool Paid
		{
			get;
			set;
		}

		public string ProjectName
		{
			get;
			set;
		}

		public OrderStatus Status
		{
			get;
			set;
		}

		public string TaxParcelNumber
		{
			get;
			set;
		}

		public OrderHistoryQuickListViewModel()
		{
		}
	}
}