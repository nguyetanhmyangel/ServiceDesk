using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace ServiceDesk.WebApp
{
    public class BundleConfig
    {
        // For more information on Bundling, visit https://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/css/bootstrap").Include(
                "~/assets/css/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/css/bootstrap-rtl").Include(
                "~/assets/css/bootstrap-rtl.min.css"));


            bundles.Add(new StyleBundle("~/css/beyond").Include(
                "~/assets/css/beyond.min.css",
                "~/assets/css/demo.min.css",
                "~/assets/css/font-awesome.min.css",
                "~/assets/css/typicons.min.css",
                "~/assets/css/weather-icons.min.css",
                "~/assets/css/animate.min.css"
            ));

            bundles.Add(new StyleBundle("~/css/beyond-rtl").Include(
                "~/assets/css/beyond-rtl.min.css",
                "~/assets/css/demo.min.css",
                "~/assets/css/font-awesome.min.css",
                "~/assets/css/typicons.min.css",
                "~/assets/css/weather-icons.min.css",
                "~/assets/css/animate.min.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/skin").Include(
                "~/assets/js/skins.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/assets/js/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/assets/js/bootstrap.min.js",
                "~/assets/js/slimscroll/jquery.slimscroll.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/beyond").Include(
                "~/assets/js/beyond.min.js"));

            // Use the Development version of Modernizr to develop with and learn from. Then, when you’re
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                            "~/Scripts/modernizr-*"));

            ScriptManager.ScriptResourceMapping.AddDefinition(
                "respond",
                new ScriptResourceDefinition
                {
                    Path = "~/Scripts/respond.min.js",
                    DebugPath = "~/Scripts/respond.js"
                });
        }
    }
}