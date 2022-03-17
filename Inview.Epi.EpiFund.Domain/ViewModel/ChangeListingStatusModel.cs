using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ChangeListingStatusModel
	{
		[Display(Name="Description")]
		public string AssetDescription
		{
			get;
			set;
		}

		public Guid AssetId
		{
			get;
			set;
		}

		[Display(Name="Asset #")]
		public int AssetNumber
		{
			get;
			set;
		}

		[Display(Name="Listing Status")]
		public ListingStatus NewStatus
		{
			get;
			set;
		}

		public ListingStatus OldStatus
		{
			get;
			set;
		}

		public List<SelectListItem> StatusList
		{
			get;
			set;
		}

		public ChangeListingStatusModel()
		{
		}
	}
}