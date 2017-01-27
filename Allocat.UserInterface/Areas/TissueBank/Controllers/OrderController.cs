using System.Web.Mvc;

namespace Allocat.UserInterface.Areas.TissueBank.Controllers
{
    //[CustomAuthorize(Roles = "TISSUE BANK SUPER ADMIN, TISSUE BANK FULFILMENT MANAGER")]
    public class OrderController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.user = System.Web.HttpContext.Current.User;
            return View();
        }

        public ActionResult Manage(int OrderId)
        {
            ViewBag.OrderId = OrderId;
            ViewBag.user = System.Web.HttpContext.Current.User;
            return View();
        }
    }
}