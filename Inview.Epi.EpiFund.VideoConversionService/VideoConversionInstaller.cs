using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.DocusignService
{
    [RunInstaller(true)]
    public partial class VideoConversionInstaller : System.Configuration.Install.Installer
    {
        public VideoConversionInstaller()
        {
            InitializeComponent();
        }
    }
}
