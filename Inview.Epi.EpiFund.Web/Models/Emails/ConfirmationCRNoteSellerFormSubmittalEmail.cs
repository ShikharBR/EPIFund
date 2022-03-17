using Inview.Epi.EpiFund.Domain.ViewModel;
using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class ConfirmationCRNoteSellerFormSubmittalEmail : Postal.Email
	{
		public string APN
		{
			get;
			set;
		}

		public AssetDescriptionModel AssetDescription
		{
			get;
			set;
		}

		public string CorpOfficer
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

		public string NoteType
		{
			get;
			set;
		}

		public string VestingEntity
		{
			get;
			set;
		}

		public ConfirmationCRNoteSellerFormSubmittalEmail()
		{
		}
	}
}