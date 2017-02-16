using Allocat.UserInterface.Models;
using Allocat.Web.Security;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Web.Security;

namespace Allocat.UserInterface.Areas.Hospital.Controllers
{
    [CustomAuthorize(Roles = "HOSPITAL SUPER ADMIN, HOSPITAL PURCHASING AGENT , HOSPITAL CLINICAL USER")]
    [CustomActionFilter]
    public class ProductController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.user = System.Web.HttpContext.Current.User;
            return View();
        }

        [HttpGet]
        public ActionResult Manage(string ProductMasterName, string ProductType, string ProductSize, string PreservationType, string SourceName)
        {
            ViewBag.ProductMasterName = ProductMasterName;
            ViewBag.ProductType = ProductType;
            ViewBag.ProductSize = ProductSize;
            ViewBag.PreservationType = PreservationType;
            ViewBag.SourceName = SourceName;

            ViewBag.user = System.Web.HttpContext.Current.User;
            return View();
        }
    }
}