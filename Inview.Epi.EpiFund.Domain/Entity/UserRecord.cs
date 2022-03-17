using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class UserRecord
	{
		public virtual string Data
		{
			get;
			set;
		}

		public virtual DateTime Date
		{
			get;
			set;
		}

		public virtual Inview.Epi.EpiFund.Domain.Entity.User User
		{
			get;
			set;
		}

		public virtual int UserId
		{
			get;
			set;
		}

		public virtual int UserRecordId
		{
			get;
			set;
		}

		public virtual Inview.Epi.EpiFund.Domain.Enum.UserRecordType UserRecordType
		{
			get;
			set;
		}

		public UserRecord()
		{
		}
	}
}