using Inview.Epi.EpiFund.Domain;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Timers;

namespace Inview.Epi.EpiFund.Business
{
	public class DataCleanupServiceManager : IDataCleanupServiceManager
	{
		private EventLog _eventLog;

		private IEPIContextFactory _factory;

		private Timer _timer;

		public DataCleanupServiceManager(IEPIContextFactory factory)
		{
			this._factory = factory;
		}

		private void _timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this._timer.Stop();
			this._timer.Start();
		}

		public void logServiceEvent(string message, EventLogEntryType logType)
		{
			try
			{
				this._eventLog.WriteEntry(message, logType);
			}
			catch
			{
			}
		}

		public void Start(EventLog log)
		{
			this.logServiceEvent("Inside Data Cleanup Service Start Method", EventLogEntryType.Information);
			this._eventLog = log;
			double num = 60000;
			if (ConfigurationManager.AppSettings["TimerInterval"] != null)
			{
				num = Convert.ToDouble(ConfigurationManager.AppSettings["TimerInterval"]);
			}
			this._timer = new Timer(num);
			this._timer.Elapsed += new ElapsedEventHandler(this._timer_Elapsed);
			this._timer.Start();
		}

		public void Stop()
		{
		}
	}
}