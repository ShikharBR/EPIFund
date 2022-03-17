using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class UserAssetSearchCriteriaQuickViewModel
	{
		public int AssetSearchCriteriaId
		{
			get;
			set;
		}

		public DateTime DateCreated
		{
			get;
			set;
		}

		public string UserFullName
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public UserAssetSearchCriteriaQuickViewModel()
		{
		}
	}
}