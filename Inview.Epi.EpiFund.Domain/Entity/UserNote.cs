using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class UserNote
	{
		public DateTime CreateDate
		{
			get;
			set;
		}

		public string Notes
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Entity.User User
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public int UserNoteId
		{
			get;
			set;
		}

		public UserNote()
		{
		}
	}
}