using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class NotificationToReferringCREMtgLBRegistrantSignedUp : Email
	{
		public string AssetDescription
		{
			get;
			set;
		}

		public double AssetListPrice
		{
			get;
			set;
		}

		public string AssetNumber
		{
			get;
			set;
		}

		public string EmailAddress
		{
			get;
			set;
		}

		public string NameOfReferredBy
		{
			get;
			set;
		}

		public string NameOfRegistrant
		{
			get;
			set;
		}

		public string RegistrantEmail
		{
			get;
			set;
		}

		public string RegistrantPhone
		{
			get;
			set;
		}

		public NotificationToReferringCREMtgLBRegistrantSignedUp()
		{
		}
	}
}