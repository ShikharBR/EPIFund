using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.DocusignService
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
                new DocusignService() 
            };
            ServiceBase.Run(ServicesToRun);
            //var ss = new DocusignService();
            //ss.Start();
            //System.Threading.Thread.Sleep(-1);
        }
    }
}
