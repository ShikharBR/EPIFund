using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class PCInsuranceCompany
	{
		public virtual string CompanyAddress1
		{
			get;
			set;
		}

		public virtual string CompanyAddress2
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

		public virtual string CompanyPhoneNumber
		{
			get;
			set;
		}

		public virtual string CompanyState
		{
			get;
			set;
		}

		public virtual string CompanyURL
		{
			get;
			set;
		}

		public virtual string CompanyZip
		{
			get;
			set;
		}

		public virtual DateTime CreateDate
		{
			get;
			set;
		}

		public virtual bool IsActive
		{
			get;
			set;
		}

		public virtual int PCInsuranceCompanyId
		{
			get;
			set;
		}

		public PCInsuranceCompany()
		{
		}
	}
}