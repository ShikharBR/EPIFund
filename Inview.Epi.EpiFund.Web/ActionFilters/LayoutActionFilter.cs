using Inview.Epi.EpiFund.Web.Controllers;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Inview.Epi.EpiFund.Web.ActionFilters
{
	public class LayoutActionFilter : ActionFilterAttribute
	{
		public LayoutActionFilter()
		{
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			try
			{
				BaseController controller = filterContext.Controller as BaseController;
				controller.CanUserImportMBA(null);
				controller.CanUserManagePortfolios(null);
			}
			catch
			{
			}
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			string lower = filterContext.ActionDescriptor.ActionName.ToLower();
			string str = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
			bool isChildAction = filterContext.IsChildAction;
			if ((!(str == "home") || !(lower == "index") && !(lower == "authsubmit") && !(lower == "validateuser")) && !isChildAction && !Convert.ToBoolean(filterContext.HttpContext.Session["RootAuth"]))
			{
				filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary()
				{
					{ "controller", "Home" },
					{ "action", "index" }
				});
				base.OnActionExecuting(filterContext);
			}
		}
	}
}