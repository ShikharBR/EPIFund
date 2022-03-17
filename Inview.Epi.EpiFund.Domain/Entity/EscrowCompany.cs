using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class EscrowCompany
	{
		public string EscrowCompanyAddress
		{
			get;
			set;
		}

		public Guid EscrowCompanyId
		{
			get;
			set;
		}

		public string EscrowCompanyName
		{
			get;
			set;
		}

		public EscrowCompany()
		{
		}
	}
}