using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class ConfirmationClosingEscrowGeneralEmail : Postal.Email
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

		public DateTime ClosingDate
		{
			get;
			set;
		}

		public double ClosingPrice
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public ConfirmationClosingEscrowGeneralEmail()
		{
		}
	}
}