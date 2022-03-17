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
	public class WebDependencies : NinjectModule
	{
		public WebDependencies()
		{
		}

		public override void Load()
		{
			base.Bind<IUserManager>().To<UserManager>();
			base.Bind<IEPIContextFactory>().To<EPIContextFactory>();
			base.Bind<IContactService>().To<ContactService>();
			base.Bind<IEPI>().To<EPI>();
			base.Bind<IAssetManager>().To<AssetManager>();
			base.Bind<IFileManager>().To<FileManager>();
			base.Bind<IPDFService>().To<PDFService>();
			base.Bind<ISecurityManager>().To<SecurityManager>();
			base.Bind<IDocusignServiceManager>().To<DocusignServiceManager>();
			base.Bind<IPortfolioManager>().To<PortfolioManager>();
			base.Bind<ITitleCompanyManager>().To<TitleCompanyManager>();
			base.Bind<IEPIFundEmailService>().To<EPIFundEmailService>().WithConstructorArgument("mandrillApiKey", ConfigurationManager.AppSettings["MandrillApiKey"]);
			base.Bind<ISellerManager>().To<SellerManager>();
			base.Bind<IInsuranceManager>().To<InsuranceManager>();
		}
	}
}