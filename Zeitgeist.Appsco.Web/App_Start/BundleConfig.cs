using System.Web;
using System.Web.Optimization;

namespace Zeitgeist.Appsco.Web
{
    public class BundleConfig
    {
        // Para obtener más información acerca de Bundling, consulte http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/all").Include(
                "~/Scripts/jquery-1.9.1.js",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Content/template/assets/js/ace-extra.min.js",
                "~/Scripts/jquery-ui-1.8.24.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/knockout-3.1.0.js",
                "~/Scripts/underscore-min.js",
                "~/Content/template/assets/js/bootstrap.min.js",
                "~/Content/template/assets/js/date-time/bootstrap-datepicker.min.js",
                "~/Content/template/assets/js/jquery-ui.custom.min.js",
                "~/Content/template/assets/js/jquery-ui.min.js",
                "~/Content/template/assets/js/jquery.ui.touch-punch.min.js",
                "~/Content/template/assets/js/jquery.easypiechart.min.js",
                "~/Content/template/assets/js/ace-elements.min.js",
                "~/Content/template/assets/js/jquery.sparkline.min.js",
                "~/Content/template/assets/js/ace.min.js",
                "~/Scripts/moment.js",
                "~/Scripts/countdown.js",
                "~/Scripts/moment-countdown.js",
                "~/Scripts/site.js",
                "~/Scripts/numeral/numeral.js",
                "~/Scripts/knockout.mapping-latest.debug.js",
                "~/Scripts/Model/KnockoutHelpers.js"
                ));

    
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información sobre los formularios. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de creación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
            //BundleTable.EnableOptimizations = true;
        }
    }
}