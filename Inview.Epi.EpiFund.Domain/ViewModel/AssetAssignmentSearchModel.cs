using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AssetAssignmentSearchModel
	{
		[Display(Name="Asset ID#")]
		public int AssetNumber
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

		public AssetAssignmentSearchModel()
		{
		}
	}
}