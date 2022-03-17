using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class PCInsuranceCompanyManager
	{
		public virtual bool IsActive
		{
			get;
			set;
		}

		public virtual Inview.Epi.EpiFund.Domain.Entity.PCInsuranceCompany PCInsuranceCompany
		{
			get;
			set;
		}

		public virtual int PCInsuranceCompanyId
		{
			get;
			set;
		}

		public virtual int PCInsuranceCompanyManagerId
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

		public PCInsuranceCompanyManager()
		{
		}
	}
}