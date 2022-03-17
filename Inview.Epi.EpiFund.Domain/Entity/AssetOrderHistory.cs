using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class AssetOrderHistory
	{
		public Guid AssetId
		{
			get;
			set;
		}

		public int AssetOrderHistoryId
		{
			get;
			set;
		}

		public DateTime Date
		{
			get;
			set;
		}

		public AssetOrderHistoryType HistoryType
		{
			get;
			set;
		}

		public string UserEmail
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public AssetOrderHistory()
		{
		}
	}
}