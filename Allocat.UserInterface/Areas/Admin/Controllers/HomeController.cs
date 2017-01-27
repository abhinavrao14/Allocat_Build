using Allocat.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Allocat.UserInterface.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [CustomAuthorize(Roles = "ALLOCAT ADMIN")]
        public ActionResult Index()
        {
            return View();
        }
    }
}