using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class PortfolioAsset
	{
		public Inview.Epi.EpiFund.Domain.Entity.Asset Asset
		{
			get;
			set;
		}

		public Guid AssetId
		{
			get;
			set;
		}

		public bool isActive
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Entity.Portfolio Portfolio
		{
			get;
			set;
		}

		public Guid PortfolioAssetId
		{
			get;
			set;
		}

		public Guid PortfolioId
		{
			get;
			set;
		}

		public PortfolioAsset()
		{
		}
	}
}