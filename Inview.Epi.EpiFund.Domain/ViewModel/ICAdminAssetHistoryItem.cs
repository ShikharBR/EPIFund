using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ICAdminAssetHistoryItem
	{
		public Guid AssetId
		{
			get;
			set;
		}

		public string AssetInventoryAddress
		{
			get;
			set;
		}

		public int AssetNumber
		{
			get;
			set;
		}

		public string AssetType
		{
			get;
			set;
		}

		public DateTime DatePublished
		{
			get;
			set;
		}

		public ICAdminAssetHistoryItem()
		{
		}
	}
}