using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class UserReferral
	{
		public virtual string City
		{
			get;
			set;
		}

		public virtual DateTime CreateDate
		{
			get;
			set;
		}

		public virtual string FirstName
		{
			get;
			set;
		}

		public virtual string LastName
		{
			get;
			set;
		}

		public virtual string ReferralCode
		{
			get;
			set;
		}

		public virtual string ReferralEmail
		{
			get;
			set;
		}

		public virtual bool Registered
		{
			get;
			set;
		}

		public virtual string State
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

		public virtual int UserReferralId
		{
			get;
			set;
		}

		public UserReferral()
		{
		}
	}
}