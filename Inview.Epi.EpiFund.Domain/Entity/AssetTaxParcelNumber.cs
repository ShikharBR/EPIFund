using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class AssetTaxParcelNumber
	{
		public Guid AssetId
		{
			get;
			set;
		}

		public int AssetTaxParcelNumberId
		{
			get;
			set;
		}

		public string TaxParcelNumber
		{
			get;
			set;
		}

		public AssetTaxParcelNumber()
		{
		}
	}
}