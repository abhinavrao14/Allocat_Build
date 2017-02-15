using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Allocat.UserInterface.Models
{
    public class CustomActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var temp = System.Web.HttpContext.Current.User;

            filterContext.Controller.ViewBag.CustomActionMessage1 = temp;

            var InfoId = filterContext.Controller.ViewBag.CustomActionMessage1.InfoId;

            if (InfoId == 0)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Response" },
                                      { "Action", "AccessDenied" } ,{ "Area", "" }});
            }

            base.OnActionExecuting(filterContext);
        }
    }
}