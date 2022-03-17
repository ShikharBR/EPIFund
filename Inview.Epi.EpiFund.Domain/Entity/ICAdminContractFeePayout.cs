using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class ICAdminContractFeePayout
	{
		public virtual DateTime DatePaid
		{
			get;
			set;
		}

		public virtual double FeeAmount
		{
			get;
			set;
		}

		public virtual int ICAdminContractFeePayoutId
		{
			get;
			set;
		}

		public virtual int RecordedByUserId
		{
			get;
			set;
		}

		public virtual Inview.Epi.EpiFund.Domain.Entity.User User
		{
			get;
			set;
		}

		public virtual int UserId
		{
			get;
			set;
		}

		public ICAdminContractFeePayout()
		{
		}
	}
}