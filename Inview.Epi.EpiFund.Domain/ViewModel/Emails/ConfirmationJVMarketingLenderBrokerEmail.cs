using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ConfirmationJVMarketingLenderBrokerEmail : Email
	{
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

		public ConfirmationJVMarketingLenderBrokerEmail()
		{
		}
	}
}