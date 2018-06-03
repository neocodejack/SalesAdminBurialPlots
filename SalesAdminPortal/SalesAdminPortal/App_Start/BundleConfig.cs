using System.Web;
using System.Web.Optimization;

namespace SalesAdminPortal
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"
                      ));

            //Login Bundle
            bundles.Add(new StyleBundle("~/Content/login").Include(
                      "~/Content/Login/main.css",
                      "~/Content/Login/util.css",
                      "~/fonts/font-awesome-4.7.0/css/font-awesome.min.css",
                      "~/fonts/iconic/css/material-design-iconic-font.min.css",
                      "~/Content/vendor/animate/animate.css",
                      "~/Content/vendor/css-hamburgers/hamburgers.min.css",
                      "~/Content/vendor/animsition/css/animsition.min.css",
                      "~/Content/vendor/select2/select2.min.css",
                      "~/Content/vendor/daterangepicker/daterangepicker.css"
                    ));
            bundles.Add(new ScriptBundle("~/Scripts/login").Include(
                      "~/Content/vendor/jquery/jquery-3.2.1.min.js",
                      "~/Content/vendor/animsition/js/animsition.min.js",
                      "~/Content/vendor/bootstrap/js/popper.js",
                      "~/Content/vendor/bootstrap/js/bootstrap.min.js",
                      "~/Content/vendor/select2/select2.min.js",
                      "~/Content/vendor/daterangepicker/moment.min.js",
                      "~/Content/vendor/daterangepicker/daterangepicker.js",
                      "~/Content/vendor/countdowntime/countdowntime.js",
                      "~/Scripts/Login/main.js"
                    ));

            //Login Bundle
            bundles.Add(new StyleBundle("~/Relase/Content/login").Include(
                      "~/SalesAdminPortal_deploy/Content/bootstrap.css",
                      "~/SalesAdminPortal_deploy/Content/Login/main.css",
                      "~/SalesAdminPortal_deploy/Content/Login/util.css",
                      "~/SalesAdminPortal_deploy/fonts/font-awesome-4.7.0/css/font-awesome.min.css",
                      "~/SalesAdminPortal_deploy/fonts/iconic/css/material-design-iconic-font.min.css",
                      "~/SalesAdminPortal_deploy/Content/vendor/animate/animate.css",
                      "~/SalesAdminPortal_deploy/Content/vendor/css-hamburgers/hamburgers.min.css",
                      "~/SalesAdminPortal_deploy/Content/vendor/animsition/css/animsition.min.css",
                      "~/SalesAdminPortal_deploy/Content/vendor/select2/select2.min.css",
                      "~/SalesAdminPortal_deploy/Content/vendor/daterangepicker/daterangepicker.css"
                    ));
            bundles.Add(new ScriptBundle("~/Release/Scripts/login").Include(
                      "~/SalesAdminPortal_deploy/Content/vendor/jquery/jquery-3.2.1.min.js",
                      "~/SalesAdminPortal_deploy/Content/vendor/animsition/js/animsition.min.js",
                      "~/SalesAdminPortal_deploy/Content/vendor/bootstrap/js/popper.js",
                      "~/SalesAdminPortal_deploy/Content/vendor/bootstrap/js/bootstrap.min.js",
                      "~/SalesAdminPortal_deploy/Content/vendor/select2/select2.min.js",
                      "~/SalesAdminPortal_deploy/Content/vendor/daterangepicker/moment.min.js",
                      "~/SalesAdminPortal_deploy/Content/vendor/daterangepicker/daterangepicker.js",
                      "~/SalesAdminPortal_deploy/Content/vendor/countdowntime/countdowntime.js",
                      "~/SalesAdminPortal_deploy/Scripts/Login/main.js"
                    ));
        }
    }
}
