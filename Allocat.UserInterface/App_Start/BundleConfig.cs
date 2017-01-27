using System.Web;
using System.Web.Optimization;

namespace Allocat.UserInterface
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Assets/js/lib/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                      "~/Assets/js/lib/bootstrap.js",
                      "~/Assets/js/lib/respond.js",
                      "~/Assets/js/lib/jquery-{version}.js",
                      "~/Assets/js/customJs/photoModule/appInfo.js",
                      "~/Assets/js/customJs/photoModule/egAppStatus.js",
                      "~/Assets/js/customJs/photoModule/photo.module.js",
                      "~/Assets/js/customJs/photoModule/photoManager.js",
                      "~/Assets/js/customJs/photoModule/egAddPhoto.js",
                      "~/Assets/js/customJs/photoModule/egFiles.js",
                      "~/Assets/js/customJs/photoModule/egUpload.js",
                      "~/Assets/js/customJs/photoModule/egPhotoUploader.js",
                      "~/Assets/js/customJs/photoModule/photoManagerClient.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Assets/js/lib/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Assets/js/lib/bootstrap.js",
                      "~/Assets/js/lib/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Assets/css/lib/bootstrap.css",
                      "~/Assets/css/lib/Site.css"));
        }
    }
}
