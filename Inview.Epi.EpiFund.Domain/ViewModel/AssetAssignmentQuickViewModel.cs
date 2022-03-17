using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AssetAssignmentQuickViewModel
	{
		[Display(Name="Asset ID#")]
		public int AssetNumber
		{
			get;
			set;
		}

		[Display(Name="State")]
		public List<SelectListItem> CompanyUsers
		{
			get;
			set;
		}

		[Required]
		public string SelectedCompanyUserId
		{
			get;
			set;
		}

		public string Status
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

		public int TotalItemCount
		{
			get;
			set;
		}

		public AssetAssignmentQuickViewModel()
		{
		}
	}
}