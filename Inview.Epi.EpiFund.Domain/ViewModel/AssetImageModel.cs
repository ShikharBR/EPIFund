using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AssetImageModel
	{
		public Guid AssetId
		{
			get;
			set;
		}

		public Guid AssetImageId
		{
			get;
			set;
		}

		public string ContentType
		{
			get;
			set;
		}

		public string FileName
		{
			get;
			set;
		}

		public bool IsFlyerImage
		{
			get;
			set;
		}

		public bool IsMainImage
		{
			get;
			set;
		}

		public int Order
		{
			get;
			set;
		}

		public string OriginalFileName
		{
			get;
			set;
		}

		public string Size
		{
			get;
			set;
		}

		public AssetImageModel()
		{
		}
	}
}