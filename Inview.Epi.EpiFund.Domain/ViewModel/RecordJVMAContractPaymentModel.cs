using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class RecordJVMAContractPaymentModel
	{
		[Display(Name="Amount Paid")]
		public double AmountPaid
		{
			get;
			set;
		}

		[Display(Name="IC Admin Name")]
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

		public int UserId
		{
			get;
			set;
		}

		public RecordJVMAContractPaymentModel()
		{
		}
	}
}