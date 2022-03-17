using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class ContractFeePayout
	{
		public int ContractFeePayoutId
		{
			get;
			set;
		}

		public DateTime DatePaid
		{
			get;
			set;
		}

		public double FeeAmount
		{
			get;
			set;
		}

		public int RecordedByUserId
		{
			get;
			set;
		}

		public ContractFeePayoutType Type
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Entity.User User
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public ContractFeePayout()
		{
		}
	}
}