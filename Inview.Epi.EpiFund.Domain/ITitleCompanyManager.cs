using System;

namespace Inview.Epi.EpiFund.Domain
{
	public interface ITitleCompanyManager
	{
		string GetTitleCompanyUserEmailBasedOnState(int titleCompanyId, string state);
	}
}