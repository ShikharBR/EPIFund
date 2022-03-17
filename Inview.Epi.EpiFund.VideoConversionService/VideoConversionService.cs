using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Inview.Epi.EpiFund.CompositionRoot;
using Ninject;
using Inview.Epi.EpiFund.Domain;

namespace Inview.Epi.EpiFund.VideoConversionService
{
    public partial class VideoConversionService : ServiceBase
    {
        private IAssetVideoServiceManager _service;
        public VideoConversionService()
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
            IKernel kernel = new StandardKernel(new VideoConversionServiceDependencies());
            _service = kernel.Get<IAssetVideoServiceManager>();
            var factory = kernel.Get<IEPIContextFactory>();
            _service.Start(eventLog1);
            //_service.ConvertVideos();
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
