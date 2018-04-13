using System.Web.Optimization;

namespace InventoryKeeper
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/base").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootstrap-dialog.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/dataTable").Include(
                      "~/Content/jquery.dataTables.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/baseDatatable").Include(
                         "~/Scripts/System/basedatatable.js",
                         "~/Scripts/jquery.dataTables.js",
                         "~/Scripts/fnReloadAjax.js"
                         ));
        }
    }
}