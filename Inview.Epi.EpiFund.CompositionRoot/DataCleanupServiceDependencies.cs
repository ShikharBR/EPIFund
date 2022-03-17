using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Data;
using Inview.Epi.EpiFund.Business;

using Ninject.Modules;
using Ninject.Syntax;
using System;

namespace Inview.Epi.EpiFund.CompositionRoot
{
	public class DataCleanupServiceDependencies : NinjectModule
	{
		public DataCleanupServiceDependencies()
		{
		}

		public override void Load()
		{
			base.Bind<IEPIContextFactory>().To<EPIContextFactory>();
			base.Bind<IDataCleanupServiceManager>().To<DataCleanupServiceManager>();
		}
	}
}