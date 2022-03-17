using System;
using System.Web.Optimization;

namespace Inview.Epi.EpiFund.Web
{
	public class BundleConfig
	{
		public BundleConfig()
		{
		}

		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add((new ScriptBundle("~/bundles/jquery")).Include(new string[] { "~/Scripts/jquery-{version}.js", "~/Scripts/global.js" }));
			bundles.Add((new ScriptBundle("~/bundles/jqueryui")).Include(new string[] { "~/Scripts/jquery-ui-{version}.js" }));
			bundles.Add((new ScriptBundle("~/bundles/jqueryval")).Include(new string[] { "~/Scripts/jquery.unobtrusive*", "~/Scripts/jquery.validate*" }));
			bundles.Add((new ScriptBundle("~/bundles/modernizr")).Include(new string[] { "~/Scripts/modernizr-*" }));
			bundles.Add((new StyleBundle("~/bundles/css")).Include(new string[] { "~/Content/Site.css" }));
			bundles.Add((new StyleBundle("~/bundles/themes/base/css")).Include(new string[] { "~/Content/themes/base/jquery.ui.core.css", "~/Content/themes/base/jquery.ui.resizable.css", "~/Content/themes/base/jquery.ui.selectable.css", "~/Content/themes/base/jquery.ui.accordion.css", "~/Content/themes/base/jquery.ui.autocomplete.css", "~/Content/themes/base/jquery.ui.button.css", "~/Content/themes/base/jquery.ui.dialog.css", "~/Content/themes/base/jquery.ui.slider.css", "~/Content/themes/base/jquery.ui.tabs.css", "~/Content/themes/base/jquery.ui.datepicker.css", "~/Content/themes/base/jquery.ui.progressbar.css", "~/Content/themes/base/jquery.ui.theme.css" }));
			bundles.Add((new ScriptBundle("~/bundles/dropzonescriptsa")).Include(new string[] { "~/Scripts/dropzone/dropzone.js", "~/Scripts/dropzone/app.js" }));
			bundles.Add((new ScriptBundle("~/bundles/dropzonescriptsh")).Include(new string[] { "~/Scripts/dropzone/dropzone.js", "~/Scripts/dropzone/Home.js" }));
			bundles.Add((new StyleBundle("~/Content/dropzonescss")).Include(new string[] { "~/Scripts/dropzone/basic.css", "~/Scripts/dropzone/dropzone.css", "~/Content/css/ProgressBar.css" }));
			BundleTable.EnableOptimizations = false;
		}
	}
}