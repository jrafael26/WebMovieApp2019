using System.Web;
using System.Web.Optimization;

namespace WebApplicationApp
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
            "~/Scripts/jquery-{version}.js",
            "~/Scripts/bootstrap.js",
            "~/scripts/bootbox.js",
            "~/Scripts/respond.js",
            "~/scripts/DataTables/jquery.datatables.js",
            "~/scripts/DataTables/datatables.bootstrap.js",
            "~/scripts/typeahead.bundle.js",
            "~/scripts/toastr.js"
          ));


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //CSS
            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/bootstrap_superhero.css",
                     "~/Content/bootstrap-theme.css",
                     "~/content/DataTables/css/datatables.bootstrap.css",
                     "~/content/typeahead.css",
                     "~/content/toastr.css",
                     "~/Content/site.css"));

        }
    }
}
