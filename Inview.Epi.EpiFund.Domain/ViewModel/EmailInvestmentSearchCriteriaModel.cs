using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class EmailInvestmentSearchCriteriaModel
	{
		public List<SelectListItem> NARMembers
		{
			get;
			set;
		}

		public int SearchCriteriaId
		{
			get;
			set;
		}

		public List<int> SelectNarMemberIds
		{
			get;
			set;
		}

		public EmailInvestmentSearchCriteriaModel()
		{
			this.SelectNarMemberIds = new List<int>();
			this.NARMembers = new List<SelectListItem>();
		}
	}
}