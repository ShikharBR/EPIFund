using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class ConfirmationBindingEscrowGeneralEmail : Postal.Email
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

		public string Email
		{
			get;
			set;
		}

		public double ListingPrice
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public DateTime ProjectedClosingDate
		{
			get;
			set;
		}

		public ConfirmationBindingEscrowGeneralEmail()
		{
		}
	}
}