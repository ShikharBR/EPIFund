using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.Routing;

namespace Inview.Epi.EpiFund.Web
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
	public class MultipleButtonAttribute : ActionNameSelectorAttribute
	{
		public string Argument
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public MultipleButtonAttribute()
		{
		}

		public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
		{
			bool flag = false;
			string str = string.Format("{0}:{1}", this.Name, this.Argument);
			if (controllerContext.Controller.ValueProvider.GetValue(str) != null)
			{
				controllerContext.Controller.ControllerContext.RouteData.Values[this.Name] = this.Argument;
				flag = true;
			}
			return flag;
		}
	}
}