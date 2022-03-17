using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class AssetSalePayment
	{
		public Inview.Epi.EpiFund.Domain.Entity.Asset Asset
		{
			get;
			set;
		}

		public Guid AssetId
		{
			get;
			set;
		}

		public int AssetSalePaymentId
		{
			get;
			set;
		}

		public string PaymentInformation
		{
			get;
			set;
		}

		public DateTime PaymentReceivedDate
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.PaymentType PaymentType
		{
			get;
			set;
		}

		public int RecordedByUserId
		{
			get;
			set;
		}

		public double SaleAmount
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Entity.User User
		{
			get;
			set;
		}

		public AssetSalePayment()
		{
		}
	}
}