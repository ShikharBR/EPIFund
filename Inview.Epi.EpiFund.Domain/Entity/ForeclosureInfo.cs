using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class ForeclosureInfo
	{
		public Guid ForeclosureInfoId
		{
			get;
			set;
		}

		[Display(Name="FC Lender")]
		public string Lender
		{
			get;
			set;
		}

		[Display(Name="FC Original Mortgage Date")]
		public DateTime? OriginalMortageDate
		{
			get;
			set;
		}

		[Display(Name="FC Original Mortgage Amount")]
		public double? OriginalMortgageAmount
		{
			get;
			set;
		}

		public LenderPosition Position
		{
			get;
			set;
		}

		[Display(Name="FC Record Date")]
		public DateTime? RecordDate
		{
			get;
			set;
		}

		[Display(Name="FC Record Number")]
		public string RecordNumber
		{
			get;
			set;
		}

		[Display(Name="FC Sale Date")]
		public DateTime? SaleDate
		{
			get;
			set;
		}

		public ForeclosureInfo()
		{
		}
	}
}