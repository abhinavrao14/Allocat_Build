using Allocat.ApplicationService;
using Allocat.DataModel;
using Allocat.DataService;
using Allocat.DataServiceInterface;
using Allocat.WebApi.WebApiModel;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Allocat.WebApi.Controllers
{
    public class RoleController : ApiController
    {
        IUserDataService userDataService;

        public RoleController()
        {
            userDataService = new UserDataService();
        }

        public HttpResponseMessage Get(string type)
        {
            UserApiModel userApiModel = new UserApiModel();
            TransactionalInformation transaction = new TransactionalInformation();

            UserBusinessService userBusinessService = new UserBusinessService(userDataService);

            IEnumerable<TissueBankRoles_TissueBank> TissueBankRoles = userBusinessService.GetTissueBankRoles
                (type,out transaction);
            userApiModel.TissueBankRoles = TissueBankRoles;

            userApiModel.ReturnStatus = transaction.ReturnStatus;
            userApiModel.ReturnMessage = transaction.ReturnMessage;
            userApiModel.IsAuthenicated = true;

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<UserApiModel>(HttpStatusCode.OK, userApiModel);
                return response;
            }

            var badResponse = Request.CreateResponse<UserApiModel>(HttpStatusCode.BadRequest, userApiModel);
            return badResponse;
        }
    }
}
