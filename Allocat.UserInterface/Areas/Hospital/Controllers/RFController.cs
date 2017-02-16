using Allocat.UserInterface.Models;
using Allocat.Web.Security;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Web.Security;

namespace Allocat.UserInterface.Areas.Hospital.Controllers
{
    [CustomAuthorize(Roles = "HOSPITAL SUPER ADMIN, HOSPITAL PURCHASING AGENT , HOSPITAL CLINICAL USER")]
    [CustomActionFilter]
    public class RFController : Controller
    {
        [HttpGet]
        public ActionResult Index([ModelBinder(typeof(IntArrayModelBinder))] int[] TBPIds)
        {
            ViewBag.SelectedTissueBankProductIds = TBPIds;
            ViewBag.user = System.Web.HttpContext.Current.User;
            return View();
        }
    }
}