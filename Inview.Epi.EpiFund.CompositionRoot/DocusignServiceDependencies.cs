using Inview.Epi.EpiFund.Business;
using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Data;
using Ninject.Modules;
using Ninject.Syntax;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Inview.Epi.EpiFund.CompositionRoot
{
	public class DocusignServiceDependencies : NinjectModule
	{
		public DocusignServiceDependencies()
		{
		}

		public override void Load()
		{
			base.Bind<IEPIContextFactory>().To<EPIContextFactory>();
			base.Bind<IPDFService>().To<PDFService>();
			base.Bind<IDocusignServiceManager>().To<DocusignServiceManager>();
			base.Bind<IEPIFundEmailService>().To<EPIFundEmailService>().WithConstructorArgument("mandrillApiKey", ConfigurationManager.AppSettings["MandrillApiKey"]);
			base.Bind<IAssetManager>().To<AssetManager>();
			base.Bind<IUserManager>().To<UserManager>();
		}
	}
}