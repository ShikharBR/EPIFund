using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AssetImagesViewModel
	{
		public List<AssetImageViewModel> Images
		{
			get;
			set;
		}

		public AssetImagesViewModel()
		{
			this.Images = new List<AssetImageViewModel>();
		}
	}
}