using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class AssetUserMDA
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

		public int AssetUserMDAId
		{
			get;
			set;
		}

		public string FileLocation
		{
			get;
			set;
		}

		public DateTime SignMDADate
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

		public AssetUserMDA()
		{
		}
	}
}