using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ContractFeeDetail
	{
		public int ContractFeeDetailId
		{
			get;
			set;
		}

		public double ContractPayment
		{
			get;
			set;
		}

		public DateTime DateFeePaid
		{
			get;
			set;
		}

		public ContractFeeDetail()
		{
		}
	}
}