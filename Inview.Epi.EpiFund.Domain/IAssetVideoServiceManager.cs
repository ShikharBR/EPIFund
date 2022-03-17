using System;
using System.Diagnostics;

namespace Inview.Epi.EpiFund.Domain
{
	public interface IAssetVideoServiceManager
	{
		void ConvertVideos();

		void Start(EventLog log);

		void Stop();
	}
}