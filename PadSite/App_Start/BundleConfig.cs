using System.Web;
using System.Web.Optimization;

namespace PadSite
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jquerycontrol").Include(
               "~/Scripts/formcontrol/control-*"
               ));

            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                .Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/animate.css",
                         "~/Content/site.css",
                       "~/Content/kendohelper.css"));


            bundles.Add(new StyleBundle("~/Content/admin")
              .Include("~/Content/animate.css",
                       "~/Content/site.css",
                       "~/Content/admin.css",
                       "~/Content/kendohelper.css"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap")
                .Include("~/Content/css/bootstrap.css"));


            bundles.Add(new StyleBundle("~/Content/kendo/2012.3.1114/css")
                .Include(
                   "~/Content/kendo/2012.3.1114/kendo.common.min.css",
                   "~/Content/kendo/2012.3.1114/kendo.dataviz.min.css",
                   "~/Content/kendo/2012.3.1114/kendo.default.min.css"
                ));

            bundles.IgnoreList.Clear();
            bundles.IgnoreList.Ignore("*.intellisense.js");
            bundles.IgnoreList.Ignore("*-vsdoc.js");
            bundles.IgnoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
        }
    }
}