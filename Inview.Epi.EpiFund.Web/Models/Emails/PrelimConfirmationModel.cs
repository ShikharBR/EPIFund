using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class PrelimConfirmationModel : Email
	{
		public string Address1
		{
			get;
			set;
		}

		public string Address2
		{
			get;
			set;
		}

		public string APN
		{
			get;
			set;
		}

		public string AssetDescription
		{
			get;
			set;
		}

		public string AssetId
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public DateTime Date
		{
			get;
			set;
		}

		public string ListingAgent
		{
			get;
			set;
		}

		public string NARUploaderName
		{
			get;
			set;
		}

		public string Owner
		{
			get;
			set;
		}

		public string Ownership
		{
			get;
			set;
		}

		public string SelectedTitleCompany
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public string Subject
		{
			get;
			set;
		}

		public string TitleCompany
		{
			get;
			set;
		}

		public string To
		{
			get;
			set;
		}

		public string Zip
		{
			get;
			set;
		}

		public PrelimConfirmationModel()
		{
		}
	}
}