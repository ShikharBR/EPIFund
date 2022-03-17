using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class UserNoteModel
	{
		public DateTime Date
		{
			get;
			set;
		}

		public string Note
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

		public UserNoteModel()
		{
		}
	}
}