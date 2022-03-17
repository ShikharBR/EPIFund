using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class ConfirmationClosingEscrowSpecificEmail : Email
	{
		public string AssetDescription
		{
			get;
			set;
		}

		public int AssetNumber
		{
			get;
			set;
		}

		public double ClosingPrice
		{
			get;
			set;
		}

		public DateTime EscrowCloseDate
		{
			get;
			set;
		}

		public string LenderBrokerName
		{
			get;
			set;
		}

		public string LenderEmail
		{
			get;
			set;
		}

		public string ReferredEmail
		{
			get;
			set;
		}

		public string ReferredPhoneNumber
		{
			get;
			set;
		}

		public string ReferredRegistrantName
		{
			get;
			set;
		}

		public ConfirmationClosingEscrowSpecificEmail()
		{
		}
	}
}