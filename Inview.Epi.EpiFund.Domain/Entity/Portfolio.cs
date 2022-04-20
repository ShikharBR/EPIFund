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
		public int ListingStatus
		{
			get;
			set;
		}
		public bool MustPortfolioAssetsInclusive
		{
			get;
			set;
		}

		public string SalePortfolioAcceptableSeller
		{
			get;
			set;
		}

		public string PricingDisplayOption
		{
			get;
			set;
		}
		public bool IsCallOffersDate
		{
			get;
			set;
		}


		public Portfolio()
		{
		}
	}
}