using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class AssetUserView
	{
		public Inview.Epi.EpiFund.Domain.Entity.Asset Asset
		{
			get;
			set;
		}

		[ForeignKey("Asset")]
		public Guid AssetId
		{
			get;
			set;
		}

		public int AssetUserViewId
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

		public DateTime ViewDate
		{
			get;
			set;
		}

		public AssetUserView()
		{
		}
	}
}