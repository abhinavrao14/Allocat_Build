using Allocat.Web.Security;
using System.Web.Mvc;

namespace Allocat.UserInterface.Areas.TissueBank.Controllers
{
    public class HomeController : Controller
    {
        [CustomAuthorize(Roles = "TISSUE BANK SUPER ADMIN, TISSUE BANK INVENTORY MANAGER")]
        public ActionResult Index()
        {
            ViewBag.user = System.Web.HttpContext.Current.User;

            return View();
        }
    }
}