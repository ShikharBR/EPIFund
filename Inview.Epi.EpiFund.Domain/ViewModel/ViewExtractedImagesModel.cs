using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ViewExtractedImagesModel
	{
		public string AssetId
		{
			get;
			set;
		}

		public List<SelectListItem> Assets
		{
			get;
			set;
		}

		public List<ExtractedImageModel> ExtractedImages
		{
			get;
			set;
		}

		public string FilePath
		{
			get;
			set;
		}

		public string GuidId
		{
			get;
			set;
		}

		public int OrderTemp
		{
			get;
			set;
		}

		public ViewExtractedImagesModel()
		{
			this.ExtractedImages = new List<ExtractedImageModel>();
			this.Assets = new List<SelectListItem>();
		}
	}
}