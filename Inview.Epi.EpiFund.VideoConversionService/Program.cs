using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.VideoConversionService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new VideoConversionService() 
            };
            ServiceBase.Run(ServicesToRun);

            //var ss = new VideoConversionService();
            //ss.Start();
            //System.Threading.Thread.Sleep(-1);
        }
    }
}
