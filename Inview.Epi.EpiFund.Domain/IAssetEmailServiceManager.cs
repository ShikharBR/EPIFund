using System;
using System.Diagnostics;

namespace Inview.Epi.EpiFund.Domain
{
	public interface IAssetEmailServiceManager
	{
		void Start(EventLog log);

		void Stop();
	}
}