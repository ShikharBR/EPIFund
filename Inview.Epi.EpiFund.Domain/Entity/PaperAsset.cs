using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class PaperAsset
	{
		public float ApyForAskingPrice
		{
			get;
			set;
		}

		public int AskingPrice
		{
			get;
			set;
		}

		public string Assignor
		{
			get;
			set;
		}

		public float Cltv
		{
			get;
			set;
		}

		public bool Current
		{
			get;
			set;
		}

		public int EquityMargin
		{
			get;
			set;
		}

		public float InvestmentYield
		{
			get;
			set;
		}

		public float Ltv
		{
			get;
			set;
		}

		public DateTime? MaturityDate
		{
			get;
			set;
		}

		public double MonthlyInterestIncome
		{
			get;
			set;
		}

		public int MonthsInArrears
		{
			get;
			set;
		}

		public DateTime? NextDueDate
		{
			get;
			set;
		}

		public float NoteRate
		{
			get;
			set;
		}

		public string OriginalInstrDocument
		{
			get;
			set;
		}

		public DateTime? OriginationDate
		{
			get;
			set;
		}

		public Guid PaperAssetId
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.PaperType PaperType
		{
			get;
			set;
		}

		public int PrincipalBalance
		{
			get;
			set;
		}

		public PaperPriority Priority
		{
			get;
			set;
		}

		public int PriorityMortgageBalance
		{
			get;
			set;
		}

		public PaperPropertyType PropertyType
		{
			get;
			set;
		}

		public string ServicingAgent
		{
			get;
			set;
		}

		public string Successor
		{
			get;
			set;
		}

		public string SuccessorAddress
		{
			get;
			set;
		}

		public string SuccessorDocDate
		{
			get;
			set;
		}

		public string SuccessorRecordedDocNumber
		{
			get;
			set;
		}

		public string SuccessorType
		{
			get;
			set;
		}

		public string Trustee
		{
			get;
			set;
		}

		public string Trustor
		{
			get;
			set;
		}

		public PaperAsset()
		{
		}
	}
}