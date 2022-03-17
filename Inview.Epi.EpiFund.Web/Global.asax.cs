using AutoMapper;
using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Inview.Epi.EpiFund.Web.Binders;

namespace Inview.Epi.EpiFund.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(AssetViewModel), new AssetViewModelBinder());

            Mapper.CreateMap<Asset, AssetViewModel>()
                .Include<MultiFamilyAsset, MultiFamilyAssetViewModel>()
                .Include<CommercialAsset, CommercialAssetViewModel>();
            Mapper.CreateMap<MultiFamilyAsset, MultiFamilyAssetViewModel>();
            Mapper.CreateMap<CommercialAsset, CommercialAssetViewModel>();
            Mapper.CreateMap<AssetViewModel, Asset>()
                .Include<MultiFamilyAssetViewModel, MultiFamilyAsset>()
                .Include<CommercialAssetViewModel, CommercialAsset>();
            Mapper.CreateMap<MultiFamilyAssetViewModel, MultiFamilyAsset>();
            Mapper.CreateMap<CommercialAssetViewModel, CommercialAsset>();
            Mapper.CreateMap<Asset, Asset>()
                .Include<MultiFamilyAsset, MultiFamilyAsset>()
                .Include<CommercialAsset, CommercialAsset>();
            Mapper.CreateMap<MultiFamilyAsset, MultiFamilyAsset>();
            Mapper.CreateMap<CommercialAsset, CommercialAsset>();
            Mapper.CreateMap<AssetNARMember, AssetNARMember>();


            //userd in HC and OC
            Mapper.CreateMap<HCCAssetHCOwnerships, AssetHCOwnership>();


            //Mapper.CreateMap<NARMember, NARMember>();

            //var epi = DependencyResolver.Current.GetService(typeof(IEPI)) as IEPI;
            //epi.Initialize();
        }

        void Application_Error(object sender, EventArgs e)
        {
            //var exception = Server.GetLastError();
            //if (exception == null)
            //    return;

            //// Clear the error
            //Server.ClearError();

            //// Redirect to a landing page
            //Response.Redirect("~/home/error", true);
        }
    }
}