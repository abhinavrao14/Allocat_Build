using Allocat.Web.Security;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Web.Security;

namespace Allocat.UserInterface.Areas.TissueBank.Controllers
{
    public class ProductController : Controller
    {
        [CustomAuthorize(Roles = "TISSUE BANK SUPER ADMIN, TISSUE BANK INVENTORY MANAGER")]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.user = System.Web.HttpContext.Current.User;
            return View();
        }

        [HttpGet]
        public ActionResult Manage(int Id)
        {
            ViewBag.ProductMasterId = Id;
            ViewBag.user = System.Web.HttpContext.Current.User;
            return View();
        }
    }
}