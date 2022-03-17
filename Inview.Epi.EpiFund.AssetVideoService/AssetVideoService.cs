using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.CompositionRoot;
using Ninject;

namespace Inview.Epi.EpiFund.AssetVideoService
{
    public partial class AssetVideoService : ServiceBase
    {
        public IAssetVideoServiceManager _service = null;
        public AssetVideoService()
        {
            InitializeComponent();
            try
            {
                if (!System.Diagnostics.EventLog.SourceExists(eventLog1.Source))   // Name of the service
                {
                    System.Diagnostics.EventLog.CreateEventSource(eventLog1.Source, eventLog1.Log);
                }
            }
            catch { }
        }

        protected override void OnStart(string[] args)
        {
            logServiceEvent("Service started", EventLogEntryType.Information);
            try
            {
                IKernel kernel = new StandardKernel(new AssetVideoServiceDependencies());
                _service = kernel.Get<IAssetVideoServiceManager>();
                var factory = kernel.Get<IEPIContextFactory>();
                try
                {
                    _service.Start(eventLog1);
                }
                catch (Exception ex)
                {
                    logServiceEvent("Error running code", EventLogEntryType.Error);
                }
            }
            catch (Exception ex)
            {
                logServiceEvent("Error retrieving dependencies", EventLogEntryType.Error);
            }
        }

        public void Start()
        {
            OnStart(null);
        }

        protected override void OnStop()
        {
            logServiceEvent("Service stopped", EventLogEntryType.Information);
        }

        #region Private Methods

        public void logServiceEvent(string message, EventLogEntryType logType)
        {
            try
            {
                eventLog1.WriteEntry(message, logType);
            }
            catch { }
        }

        #endregion
    }
}
