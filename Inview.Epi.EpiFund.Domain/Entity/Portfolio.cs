using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class Portfolio
	{
		public DateTime? CallforOfferDate
		{
			get;
			set;
		}

		public bool hasOffersDate
		{
			get;
			set;
		}

		public bool isActive
		{
			get;
			set;
		}

		public bool isSubjectToAuction
		{
			get;
			set;
		}

		public DateTime? LastReportedOccupancyDate
		{
			get;
			set;
		}

		public int NumberofAssets
		{
			get;
			set;
		}

		public Guid PortfolioId
		{
			get;
			set;
		}

		public string PortfolioName
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

		public bool IsCallOffersDate
		{
			get;
			set;
		}
		public float? CapRete
		{
			get;
			set;
		}


		public bool MustPortfolioAssetsInclusive { get; set; }

		public Inview.Epi.EpiFund.Domain.Enum.ListingStatusall? ListingStatusall { get; set; }
		public Inview.Epi.EpiFund.Domain.Enum.SellerTerms? SalePortfolioAcceptableSeller { get; set; }
		public Inview.Epi.EpiFund.Domain.Enum.PricingDisplay? PricingDisplayOption { get; set; }


		public Portfolio()
		{
		}
	}
}