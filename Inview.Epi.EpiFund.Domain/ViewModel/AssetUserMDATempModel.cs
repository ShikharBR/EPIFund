using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AssetUserMDATempModel
	{
		public Inview.Epi.EpiFund.Domain.ViewModel.AssetType AssetType
		{
			get;
			set;
		}

		public List<Inview.Epi.EpiFund.Domain.ViewModel.AssetType> AssetTypes
		{
			get;
			set;
		}

		public AssetUserMDATempModel()
		{
		}
	}
}