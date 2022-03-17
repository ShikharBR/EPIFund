using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.AssetEmailService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //var ss = new AssetEmailService();
            //ss.Start();
            //System.Threading.Thread.Sleep(Timeout.Infinite);
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new AssetEmailService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
