using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class UserMachine
	{
		public string Code
		{
			get;
			set;
		}

		public string MachineName
		{
			get;
			set;
		}

		public MachineStatus Status
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

		public int UserMachineId
		{
			get;
			set;
		}

		public UserMachine()
		{
		}
	}
}