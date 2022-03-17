using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class CarouselAssetData
	{
		public string AskingPrice
		{
			get;
			set;
		}

		public Guid AssetId
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.ViewModel.AssetType AssetType
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public string Image
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public CarouselAssetData()
		{
		}
	}
}