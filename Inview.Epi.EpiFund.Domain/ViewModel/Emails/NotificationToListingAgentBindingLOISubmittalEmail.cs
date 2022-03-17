using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class NotificationToListingAgentBindingLOISubmittalEmail : Email
	{
		public string Description
		{
			get;
			set;
		}

		public string ListingAgent
		{
			get;
			set;
		}

		public string Phone
		{
			get;
			set;
		}

		public string Principal
		{
			get;
			set;
		}

		public string ProjectName
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

		public string Vesting
		{
			get;
			set;
		}

		public NotificationToListingAgentBindingLOISubmittalEmail()
		{
		}
	}
}