using System;
using System.Diagnostics;

namespace Inview.Epi.EpiFund.Domain
{
	public interface IDataCleanupServiceManager
	{
		void Start(EventLog log);

		void Stop();
	}
}