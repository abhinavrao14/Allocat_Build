using System.Web.Mvc;
using System.Web.Security;

namespace Allocat.UserInterface.Controllers
{
    public class ResponseController : Controller
    {
        public ActionResult TissueBankUser_SignUp_Successful()
        {
            return View();
        }

        public ActionResult TissueBank_Registration_Successful()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Account", new { area = "" ,Status= "TissueBank_Registration_Successful" });
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

    }
}