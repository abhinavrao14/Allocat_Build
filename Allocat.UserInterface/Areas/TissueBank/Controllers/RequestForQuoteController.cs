using Allocat.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Allocat.UserInterface.Areas.TissueBank.Controllers
{
    //[CustomAuthorize(Roles = "TISSUE BANK SUPER ADMIN, TISSUE BANK FULFILMENT MANAGER")]
    public class RequestForQuoteController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.user = System.Web.HttpContext.Current.User;
            return View();
        }

        public ActionResult Manage(int RequestForQuoteId, bool editMode)
        {
            ViewBag.RequestForQuoteId = RequestForQuoteId;
            ViewBag.user = System.Web.HttpContext.Current.User;
            ViewBag.editMode = editMode;
            return View();
        }
    }
}