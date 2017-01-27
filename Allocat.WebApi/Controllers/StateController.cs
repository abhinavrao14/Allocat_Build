using Allocat.ApplicationService;
using Allocat.DataModel;
using Allocat.DataService;
using Allocat.DataServiceInterface;
using Allocat.WebApi.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Allocat.WebApi.Controllers
{
    public class StateController : ApiController
    {
        IStateDataService stateDataService;

        public StateController()
        {
            stateDataService = new StateDataService();
        }

        public HttpResponseMessage Get()
        {
            CommonApiModel commonApiModel = new CommonApiModel();
            TransactionalInformation transaction = new TransactionalInformation();

            StateBusinessService stateBusinessService = new StateBusinessService(stateDataService);

            IEnumerable<State> States = stateBusinessService.GetState
                (out transaction);
            commonApiModel.States = States;

            commonApiModel.ReturnStatus = transaction.ReturnStatus;
            commonApiModel.ReturnMessage = transaction.ReturnMessage;
            commonApiModel.IsAuthenicated = true;

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<CommonApiModel>(HttpStatusCode.OK, commonApiModel);
                return response;
            }

            var badResponse = Request.CreateResponse<CommonApiModel>(HttpStatusCode.BadRequest, commonApiModel);
            return badResponse;
        }
    }
}
