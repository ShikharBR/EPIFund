using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Inview.Epi.EpiFund.Web
{
	public class RouteConfig
	{
		public RouteConfig()
		{
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
		}
	}
}