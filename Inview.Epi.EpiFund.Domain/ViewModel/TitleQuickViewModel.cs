using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class TitleQuickViewModel
	{
		public DateTime CreatedOn
		{
			get;
			set;
		}

		public bool IsActive
		{
			get;
			set;
		}

		public bool showInActive
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public int TitleCompanyId
		{
			get;
			set;
		}

		public string TitleCompName
		{
			get;
			set;
		}

		public string TitleCompURL
		{
			get;
			set;
		}

		public int TotalItemCount
		{
			get;
			set;
		}

		public TitleQuickViewModel()
		{
		}
	}
}