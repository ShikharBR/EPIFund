using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class ConfirmationToAdminSearchCriteriaSubmitted : Postal.Email
	{
		public string Email
		{
			get;
			set;
		}

		public int SearchCriteraId
		{
			get;
			set;
		}

		public DateTime SearchCriteriaDate
		{
			get;
			set;
		}

		public string SearchCriteriaName
		{
			get;
			set;
		}

		public string To
		{
			get;
			set;
		}

		public ConfirmationToAdminSearchCriteriaSubmitted()
		{
		}
	}
}