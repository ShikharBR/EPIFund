using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class AutoConfirmationMDAUpdatedMultipleAssets : Email
	{
		public string AssetNumbers
		{
			get;
			set;
		}

		public string Locations
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

		public AutoConfirmationMDAUpdatedMultipleAssets()
		{
		}
	}
}