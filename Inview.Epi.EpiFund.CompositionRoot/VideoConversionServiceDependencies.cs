using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Business;
using Inview.Epi.EpiFund.Data;
using Ninject.Modules;
using Ninject.Syntax;
using System;

namespace Inview.Epi.EpiFund.CompositionRoot
{
	public class VideoConversionServiceDependencies : NinjectModule
	{
		public VideoConversionServiceDependencies()
		{
		}

		public override void Load()
		{
			base.Bind<IEPIContextFactory>().To<EPIContextFactory>();
			base.Bind<IAssetManager>().To<AssetManager>();
			base.Bind<IAssetVideoServiceManager>().To<AssetVideoServiceManager>();
		}
	}
}