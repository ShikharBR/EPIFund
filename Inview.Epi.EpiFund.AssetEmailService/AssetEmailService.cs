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
using Ninject;
using Inview.Epi.EpiFund.CompositionRoot;

namespace Inview.Epi.EpiFund.AssetEmailService
{
    public partial class AssetEmailService : ServiceBase
    {
        private IAssetEmailServiceManager _service = null;
        public AssetEmailService()
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
            // Get our business layer

            IKernel kernel = new StandardKernel(new AssetEmailServiceDependencies());
            _service = kernel.Get<IAssetEmailServiceManager>();
            var factory = kernel.Get<IEPIContextFactory>();
            _service.Start(eventLog1);
            logServiceEvent("Service started", EventLogEntryType.Information);

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
