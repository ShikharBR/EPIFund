using Inview.Epi.EpiFund.Domain.ViewModel;
using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class AutoConfirmationMDAUpdatedEmail : Email
	{
		public AssetDescriptionModel AssetDescription
		{
			get;
			set;
		}

		public string RecipientEmail
		{
			get;
			set;
		}

		public string RecipientName
		{
			get;
			set;
		}

		public AutoConfirmationMDAUpdatedEmail()
		{
		}
	}
}