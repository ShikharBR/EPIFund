using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class TempAssetVideo
	{
		public string Description
		{
			get;
			set;
		}

		public string FilePath
		{
			get;
			set;
		}

		public Guid GuidId
		{
			get;
			set;
		}

		public int TempAssetVideoId
		{
			get;
			set;
		}

		public TempAssetVideo()
		{
		}
	}
}