using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class TempAssetImage
	{
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

		public Guid GuidId
		{
			get;
			set;
		}

		public int Order
		{
			get;
			set;
		}

		public int TempAssetImageId
		{
			get;
			set;
		}

		public TempAssetImage()
		{
		}
	}
}