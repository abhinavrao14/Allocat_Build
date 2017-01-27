using System.Web.Mvc;

namespace Allocat.UserInterface.Areas.TissueBank
{
    public class TissueBankAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TissueBank";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TissueBank_default",
                "TissueBank/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}