using Allocat.ApplicationService;
using Allocat.DataModel;
using Allocat.DataService;
using Allocat.DataServiceInterface;
using System.Web.Mvc;
using System.Web.Security;

namespace Allocat.UserInterface.Controllers
{
    public class ResponseController : Controller
    {
        IUserDataService userDataService;

        public ResponseController()
        {
            userDataService = new UserDataService();
        }

        public ActionResult TissueBankUser_SignUp_Successful()
        {
            return View();
        }

        public ActionResult TissueBank_Registration_Successful()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Account", new { area = "", Status = "TissueBank_Registration_Successful" });
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult TissueBank_Verification_Successful(bool response, int UserId)
        {
            if (response)
            {
                TransactionalInformation transaction = new TransactionalInformation();
                UserBusinessService userBusinessService = new UserBusinessService(userDataService);
                userBusinessService.UserEmailVerified(UserId, out transaction);

                if (transaction.ReturnStatus == false)
                {
                    ViewBag.message = transaction.ReturnMessage;
                }
            }
            return View();
        }
    }
}