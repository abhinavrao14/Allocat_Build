using Allocat.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Allocat.UserInterface.Areas.TissueBank.Controllers
{
    [CustomAuthorize(Roles = "TISSUE BANK SUPER ADMIN")]
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.user = System.Web.HttpContext.Current.User;
            return View();
        }

        [HttpGet]
        public ActionResult Manage(int? UserId)
        {
            ViewBag.S_UserId = UserId;
            ViewBag.user = System.Web.HttpContext.Current.User;
            return View();
        }
    }
}