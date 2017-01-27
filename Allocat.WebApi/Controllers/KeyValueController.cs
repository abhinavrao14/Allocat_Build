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
    public class KeyValueController : ApiController
    {
        IKeyValueDataService keyValueDataService;

        public KeyValueController()
        {
            keyValueDataService = new KeyValueDataService();
        }

        public HttpResponseMessage Get(string Type)
        {
            CommonApiModel commonApiModel = new CommonApiModel();
            TransactionalInformation transaction = new TransactionalInformation();

            KeyValueBusinessService keyValueBusinessService = new KeyValueBusinessService(keyValueDataService);

            IEnumerable<KeyValue> KeyValues = keyValueBusinessService.Get
                (Type, out transaction);
            commonApiModel.KeyValues = KeyValues;

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
