using Inview.Epi.EpiFund.Domain.ViewModel;
using Postal;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class NotificationForRegistrantOfMatchingAssetsEmail : Email
	{
		public List<AssetDescriptionModel> Assets
		{
			get;
			set;
		}

		public string RegistrantEmail
		{
			get;
			set;
		}

		public string RegistrantName
		{
			get;
			set;
		}

		public DateTime SearchCriteriaDate
		{
			get;
			set;
		}

		public NotificationForRegistrantOfMatchingAssetsEmail()
		{
		}
	}
}