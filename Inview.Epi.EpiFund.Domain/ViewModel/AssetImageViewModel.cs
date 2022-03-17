using Inview.Epi.EpiFund.Domain.Entity;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AssetImageViewModel
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

		public string DateString
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

		public int StaticOrder
		{
			get;
			set;
		}

		public string UserId
		{
			get;
			set;
		}

		public AssetImageViewModel()
		{
		}

		public void ImageToModel(AssetImage image)
		{
			this.AssetId = image.AssetId;
			this.AssetImageId = image.AssetImageId;
			this.ContentType = image.ContentType;
			this.FileName = image.FileName;
			this.IsFlyerImage = image.IsFlyerImage;
			this.IsMainImage = image.IsMainImage;
			this.Order = image.Order;
			this.OriginalFileName = image.OriginalFileName;
			this.Size = image.Size;
			this.StaticOrder = image.Order;
		}

		public void ImageToModel(AssetImageModel image, Guid assetId)
		{
			this.AssetId = assetId;
			this.AssetImageId = image.AssetImageId;
			this.ContentType = image.ContentType;
			this.FileName = image.FileName;
			this.IsFlyerImage = image.IsFlyerImage;
			this.IsMainImage = image.IsMainImage;
			this.Order = image.Order;
			this.OriginalFileName = image.OriginalFileName;
			this.Size = image.Size;
			this.StaticOrder = image.Order;
		}

		public AssetImage ModelToImage()
		{
			AssetImage assetImage = new AssetImage();
			AssetImageViewModel assetImageViewModel = this;
			assetImage.AssetId = assetImageViewModel.AssetId;
			assetImage.AssetImageId = assetImageViewModel.AssetImageId;
			assetImage.ContentType = assetImageViewModel.ContentType;
			assetImage.FileName = assetImageViewModel.FileName;
			assetImage.IsFlyerImage = assetImageViewModel.IsFlyerImage;
			assetImage.IsMainImage = assetImageViewModel.IsMainImage;
			assetImage.Order = assetImageViewModel.Order;
			assetImage.OriginalFileName = assetImageViewModel.OriginalFileName;
			assetImage.Size = assetImageViewModel.Size;
			return assetImage;
		}
	}
}