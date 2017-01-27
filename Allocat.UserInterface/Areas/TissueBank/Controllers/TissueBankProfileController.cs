using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Allocat.UserInterface.Areas.TissueBank.Controllers
{
    public class TissueBankProfileController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.user = System.Web.HttpContext.Current.User;
            return View();
        }
    }
}