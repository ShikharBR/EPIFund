using Postal;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class NotificationForCorpAdminPaperOrRealEstateCreated : Email
	{
		public string Link
		{
			get;
			set;
		}

		public string NoteType
		{
			get;
			set;
		}

		public string To
		{
			get;
			set;
		}

		public NotificationForCorpAdminPaperOrRealEstateCreated()
		{
		}
	}
}