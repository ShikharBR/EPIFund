using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class RecordContractPaymentModel
	{
		[Display(Name="Amount Paid")]
		public double AmountPaid
		{
			get;
			set;
		}

		public int ContractPaymentId
		{
			get;
			set;
		}

		public string Method
		{
			get;
			set;
		}

		[Display(Name="User Name")]
		public string Name
		{
			get;
			set;
		}

		[Display(Name="Date of Payment")]
		public DateTime PaymentDate
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

		public int UserId
		{
			get;
			set;
		}

		public RecordContractPaymentModel()
		{
		}
	}
}