using Allocat.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Allocat.UserInterface.Areas.Hospital.Controllers
{
    public class RequestForQuoteController : Controller
    {
        // GET: Hospital/RFQ
        public ActionResult Create([ModelBinder(typeof(IntArrayModelBinder))] int[] TBPIds)
        {
            //ViewBag.SelectedTissueBankProductIds = TBPIds;
            return View();
        }
    }
}