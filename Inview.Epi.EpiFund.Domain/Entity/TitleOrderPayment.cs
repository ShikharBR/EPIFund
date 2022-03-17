using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class TitleOrderPayment
	{
		public Inview.Epi.EpiFund.Domain.Entity.Asset Asset
		{
			get;
			set;
		}

		public Guid? AssetId
		{
			get;
			set;
		}

		public double? PaidAmount
		{
			get;
			set;
		}

		public string PaymentInformation
		{
			get;
			set;
		}

		public DateTime? PaymentReceivedDate
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.PaymentType? PaymentType
		{
			get;
			set;
		}

		public int? RecordedByUserId
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Entity.TitleCompany TitleCompany
		{
			get;
			set;
		}

		public int? TitleCompanyId
		{
			get;
			set;
		}

		public int TitleOrderPaymentId
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Entity.User User
		{
			get;
			set;
		}

		public TitleOrderPayment()
		{
		}
	}
}