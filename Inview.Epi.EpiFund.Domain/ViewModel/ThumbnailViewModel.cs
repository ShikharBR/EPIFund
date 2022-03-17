using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ThumbnailViewModel
	{
		public virtual byte[] Bytes
		{
			get;
			set;
		}

		public virtual string ContentType
		{
			get;
			set;
		}

		public ThumbnailViewModel()
		{
		}
	}
}