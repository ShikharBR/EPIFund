using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.ViewModel;
using PagedList;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models
{
	public class AssetAssignmentModel
	{
		[Display(Name="Asset ID#")]
		public int AssetNumber
		{
			get;
			set;
		}

		public UserType ControllingUserType
		{
			get;
			set;
		}

		public string Status
		{
			get;
			set;
		}

		public PagedList.IPagedList<AssetAssignmentQuickViewModel> TitleAssignments
		{
			get;
			set;
		}

		public int TitleCompanyId
		{
			get;
			set;
		}

		public int TitleCompanyManagerId
		{
			get;
			set;
		}

		public AssetAssignmentModel()
		{
		}
	}
}