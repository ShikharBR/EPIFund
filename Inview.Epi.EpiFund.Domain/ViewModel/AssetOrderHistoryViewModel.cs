using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AssetOrderHistoryViewModel
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

		public bool IsSeller
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

		public AssetOrderHistoryViewModel()
		{
		}
	}
}