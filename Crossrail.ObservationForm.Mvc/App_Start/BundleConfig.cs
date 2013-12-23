using System.Web;
using System.Web.Optimization;

namespace Crossrail.ObservationForm.Mvc
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/jquery-ui.timespinner.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/globalize.js",
                "~/Scripts/common.js",
                "~/Scripts/jquery.validate*",
                "~/Scripts/forms.js",
                "~/Scripts/jquery.unobtrusive*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/reset.css",
                "~/Content/css.css"));

            bundles.Add(new StyleBundle("~/Content/themes/crossrail/css").Include(
                "~/Content/themes/crossrail/jquery-ui-1.10.3.custom.css"));
        }
    }
}