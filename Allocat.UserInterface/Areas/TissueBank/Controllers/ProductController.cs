using Allocat.UserInterface.Models;
using Allocat.Web.Security;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Web.Security;

namespace Allocat.UserInterface.Areas.TissueBank.Controllers
{
    [CustomAuthorize(Roles = "TISSUE BANK SUPER ADMIN, TISSUE BANK INVENTORY MANAGER")]
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
        public ActionResult Manage(int Id)
        {
            ViewBag.TissueBankProductMasterId = Id;
            ViewBag.user = System.Web.HttpContext.Current.User;
            return View();
        }
    }
}