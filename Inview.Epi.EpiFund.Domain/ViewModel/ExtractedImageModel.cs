using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ExtractedImageModel
	{
		public string Height
		{
			get;
			set;
		}

		public string Image
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

		public bool IsSelected
		{
			get;
			set;
		}

		public int OrderTemp
		{
			get;
			set;
		}

		public string Width
		{
			get;
			set;
		}

		public ExtractedImageModel()
		{
		}
	}
}