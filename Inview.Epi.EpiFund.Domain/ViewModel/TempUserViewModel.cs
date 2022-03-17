using Inview.Epi.EpiFund.Domain.Entity;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class TempUserViewModel
	{
		public int PICount
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Entity.User User
		{
			get;
			set;
		}

		public TempUserViewModel()
		{
		}
	}
}