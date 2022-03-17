using AutoMapper;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.ViewModel;
using Inview.Epi.EpiFund.Web.Binders;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Inview.Epi.EpiFund.Web
{
	public class MvcApplication : HttpApplication
	{
		public MvcApplication()
		{
		}

		private void Application_Error(object sender, EventArgs e)
		{
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			ModelBinders.Binders.Add(typeof(AssetViewModel), new AssetViewModelBinder());
			Mapper.CreateMap<Asset, AssetViewModel>().Include<MultiFamilyAsset, MultiFamilyAssetViewModel>().Include<CommercialAsset, CommercialAssetViewModel>();
			Mapper.CreateMap<MultiFamilyAsset, MultiFamilyAssetViewModel>();
			Mapper.CreateMap<CommercialAsset, CommercialAssetViewModel>();
			Mapper.CreateMap<AssetViewModel, Asset>().Include<MultiFamilyAssetViewModel, MultiFamilyAsset>().Include<CommercialAssetViewModel, CommercialAsset>();
			Mapper.CreateMap<MultiFamilyAssetViewModel, MultiFamilyAsset>();
			Mapper.CreateMap<CommercialAssetViewModel, CommercialAsset>();
			Mapper.CreateMap<Asset, Asset>().Include<MultiFamilyAsset, MultiFamilyAsset>().Include<CommercialAsset, CommercialAsset>();
			Mapper.CreateMap<MultiFamilyAsset, MultiFamilyAsset>();
			Mapper.CreateMap<CommercialAsset, CommercialAsset>();
			Mapper.CreateMap<AssetNARMember, AssetNARMember>();
		}
	}
}