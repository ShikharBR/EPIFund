using Inview.Epi.EpiFund.CompositionRoot;
using Inview.Epi.EpiFund.Domain;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.DocusignService
{
    public partial class DocusignService : ServiceBase
    {
        private IDocusignServiceManager _service;
        public DocusignService()
        {
            InitializeComponent();
            try
            {
                if (!System.Diagnostics.EventLog.SourceExists(eventLog1.Source))   // Name of the service
                {
                    System.Diagnostics.EventLog.CreateEventSource(eventLog1.Source, eventLog1.Log);
                }
            }
            catch
            {
            }
        }
        protected override void OnStart(string[] args)
        {
            // Get our business layer

            IKernel kernel = new StandardKernel(new DocusignServiceDependencies());
            _service = kernel.Get<IDocusignServiceManager>();
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
            try
            {
                logServiceEvent("Service stopped", EventLogEntryType.Information);
            }
            catch { }
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
