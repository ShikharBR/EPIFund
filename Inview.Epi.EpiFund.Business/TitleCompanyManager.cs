using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.Entity;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Business
{
	public class TitleCompanyManager : ITitleCompanyManager
	{
		private IEPIContextFactory _factory;

		public TitleCompanyManager(IEPIContextFactory factory)
		{
			this._factory = factory;
		}

		public string GetTitleCompanyUserEmailBasedOnState(int titleCompanyId, string state)
		{
			string str;
			IEPIRepository ePIRepository = this._factory.Create();
			TitleCompanyUser titleCompanyUser = ePIRepository.TitleCompanyUsers.FirstOrDefault<TitleCompanyUser>((TitleCompanyUser w) => w.TitleCompanyId == titleCompanyId && w.AssignedStates.Contains(state));
			str = (titleCompanyUser == null ? ePIRepository.TitleCompanyUsers.FirstOrDefault<TitleCompanyUser>((TitleCompanyUser s) => s.TitleCompanyId == titleCompanyId && s.IsManager).Email : titleCompanyUser.Email);
			return str;
		}
	}
}