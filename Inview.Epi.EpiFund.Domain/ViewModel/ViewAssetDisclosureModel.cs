using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ViewAssetDisclosureModel
	{
		public Guid AssetId
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public ViewAssetDisclosureModel()
		{
		}
	}
}