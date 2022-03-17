using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Business;
using Inview.Epi.EpiFund.Data;
using Ninject.Modules;
using Ninject.Syntax;
using System;

namespace Inview.Epi.EpiFund.CompositionRoot
{
	public class AssetVideoServiceDependencies : NinjectModule
	{
		public AssetVideoServiceDependencies()
		{
		}

		public override void Load()
		{
			base.Bind<IEPIContextFactory>().To<EPIContextFactory>();
			base.Bind<IAssetVideoServiceManager>().To<AssetVideoServiceManager>();
		}
	}
}