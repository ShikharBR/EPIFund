using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class MyUSCViewModel
	{
		public List<SignedMDAQuickViewModel> AssetMDAs
		{
			get;
			set;
		}

		public List<UserFileModel> Files
		{
			get;
			set;
		}

		public string FullName
		{
			get;
			set;
		}

		public bool HasOrderHistory
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.ICStatus? ICStatus
		{
			get;
			set;
		}

		public bool IsASeller
		{
			get;
			set;
		}

		public string JVMarketerAgreementLocation
		{
			get;
			set;
		}

		public int? PersonalFinancialStatementId
		{
			get;
			set;
		}

		public List<AssetSearchCriteriaQuickViewModel> Searches
		{
			get;
			set;
		}

		public string TitleCompanyName
		{
			get;
			set;
		}

		public int UnreadLOICount
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.UserType UserType
		{
			get;
			set;
		}

        public List<SelectListItem> States
        {
            get;
            set;
        }

		public MyUSCViewModel()
		{
			this.Searches = new List<AssetSearchCriteriaQuickViewModel>();
			this.AssetMDAs = new List<SignedMDAQuickViewModel>();
			this.Files = new List<UserFileModel>();
            this.States = Common.GetSelectListItemsOfStates(false);
		}
	}
}