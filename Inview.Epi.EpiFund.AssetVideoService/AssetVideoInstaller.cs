﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.AssetVideoService
{
    [RunInstaller(true)]
    public partial class AssetVideoInstaller : System.Configuration.Install.Installer
    {
        public AssetVideoInstaller()
        {
            InitializeComponent();
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
