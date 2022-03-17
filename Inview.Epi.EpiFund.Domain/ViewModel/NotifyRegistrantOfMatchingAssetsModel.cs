using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class NotifyRegistrantOfMatchingAssetsModel
	{
		public List<int> AssetNumbers
		{
			get;
			set;
		}

		public string AssetSearchId
		{
			get;
			set;
		}

		public List<SelectListItem> Searches
		{
			get;
			set;
		}

		public NotifyRegistrantOfMatchingAssetsModel()
		{
		}
	}
}