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
    public class RequestResponseController : ApiController
    {
        IRFQDataService rfqDataService;

        public RequestResponseController()
        {
            rfqDataService = new RFQDataService();
        }

        public HttpResponseMessage Get([FromUri] int RequestForQuoteId)
        {
            RFQ_TissueBankApiModel rfq_TissueBankApiModel = new RFQ_TissueBankApiModel();
            TransactionalInformation transaction = new TransactionalInformation();

            RFQBusinessService rfqBusinessService = new RFQBusinessService(rfqDataService);

            IEnumerable<sp_RequestResponse_TissueBank_GetByTissueBankId_Result> lstRequestResponse = rfqBusinessService.GetRequestResponseByRequestForQuoteId
                (RequestForQuoteId, out transaction);

            rfq_TissueBankApiModel.RequestResponses = lstRequestResponse;
            rfq_TissueBankApiModel.ReturnStatus = transaction.ReturnStatus;
            rfq_TissueBankApiModel.ReturnMessage = transaction.ReturnMessage;
            rfq_TissueBankApiModel.IsAuthenicated = true;

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<RFQ_TissueBankApiModel>(HttpStatusCode.OK, rfq_TissueBankApiModel);
                return response;
            }

            var badResponse = Request.CreateResponse<RFQ_TissueBankApiModel>(HttpStatusCode.BadRequest, rfq_TissueBankApiModel);
            return badResponse;
        }
    }
}
