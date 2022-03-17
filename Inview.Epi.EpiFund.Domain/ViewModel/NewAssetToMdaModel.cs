using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class NewAssetToMdaModel
	{
		public List<Guid> AssetIdsToAdd
		{
			get;
			set;
		}

		public List<AssetDescriptionModel> AssetsToAdd
		{
			get;
			set;
		}

		public DateTime OriginalMDASignDate
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public NewAssetToMdaModel()
		{
			this.AssetsToAdd = new List<AssetDescriptionModel>();
			this.AssetIdsToAdd = new List<Guid>();
		}
	}
}