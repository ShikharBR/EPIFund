using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class ConfirmationAssetInputEmail : Postal.Email
	{
		public string AssetDescription
		{
			get;
			set;
		}

		public string AssetNumber
		{
			get;
			set;
		}

		public DateTime DateOfEntry
		{
			get;
			set;
		}

		public string DidIncludeFiles
		{
			get;
			set;
		}

		public string DidIncludePhotos
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

		public string PropertyAddress
		{
			get;
			set;
		}

		public string USCIC
		{
			get;
			set;
		}

		public ConfirmationAssetInputEmail()
		{
		}
	}
}