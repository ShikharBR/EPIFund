using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class PrincipalInvestor
	{
		public virtual string CellPhoneNumber
		{
			get;
			set;
		}

		public virtual string CompanyAddressLine1
		{
			get;
			set;
		}

		public virtual string CompanyAddressLine2
		{
			get;
			set;
		}

		public virtual string CompanyCity
		{
			get;
			set;
		}

		public virtual string CompanyName
		{
			get;
			set;
		}

		public virtual string CompanyState
		{
			get;
			set;
		}

		public virtual string CompanyZip
		{
			get;
			set;
		}

		public virtual string Email
		{
			get;
			set;
		}

		public virtual string FaxNumber
		{
			get;
			set;
		}

		public virtual string FirstName
		{
			get;
			set;
		}

		public virtual bool IsActive
		{
			get;
			set;
		}

		public virtual string LastName
		{
			get;
			set;
		}

		public virtual int PrincipalInvestorId
		{
			get;
			set;
		}

		public virtual int? ReferredByUserId
		{
			get;
			set;
		}

		public virtual bool? Registered
		{
			get;
			set;
		}

		public virtual string Website
		{
			get;
			set;
		}

		public virtual string WorkPhoneNumber
		{
			get;
			set;
		}

        public virtual string Country
        {
            get;
            set;
        }

        public PrincipalInvestor()
		{
		}
	}
}