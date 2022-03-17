using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class AssetCommission
	{
		public Inview.Epi.EpiFund.Domain.Entity.Asset Asset
		{
			get;
			set;
		}

		public int AssetCommissionId
		{
			get;
			set;
		}

		public Guid AssetId
		{
			get;
			set;
		}

		public PaymentType ComissionPaymentType
		{
			get;
			set;
		}

		public string CommissionInformation
		{
			get;
			set;
		}

		public double CommissionPaid
		{
			get;
			set;
		}

		public DateTime CommissionPaidDate
		{
			get;
			set;
		}

		public int RecordedByUserId
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Entity.User User
		{
			get;
			set;
		}

		public AssetCommission()
		{
		}
	}
}