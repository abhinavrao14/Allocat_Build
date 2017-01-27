using Allocat.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Allocat.UserInterface.Areas.Hospital.Controllers
{
    public class HomeController : Controller
    {
        [CustomAuthorize(Roles = "HOSPITAL SUPER ADMIN")]
        public ActionResult Index()
        {
            return View();
        }
    }
}