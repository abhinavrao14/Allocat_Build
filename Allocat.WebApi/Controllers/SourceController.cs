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
    public class SourceController : ApiController
    {
        IProductDataService productDataService;

        public SourceController()
        {
            productDataService = new ProductDataService();
        }
        public HttpResponseMessage Get()
        {
            Product_TissueBankApiModel rfq_TissueBankApiModel = new Product_TissueBankApiModel();
            TransactionalInformation transaction = new TransactionalInformation();

            ProductBusinessService productBusinessService = new ProductBusinessService(productDataService);

            List<Source> Sources = productBusinessService.GetSources(out transaction);

            rfq_TissueBankApiModel.Sources = Sources;
            rfq_TissueBankApiModel.IsAuthenicated = true;
            rfq_TissueBankApiModel.ReturnStatus = transaction.ReturnStatus;
            rfq_TissueBankApiModel.ReturnMessage = transaction.ReturnMessage;
            rfq_TissueBankApiModel.IsAuthenicated = true;

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<Product_TissueBankApiModel>(HttpStatusCode.OK, rfq_TissueBankApiModel);
                return response;
            }

            var badResponse = Request.CreateResponse<Product_TissueBankApiModel>(HttpStatusCode.BadRequest, rfq_TissueBankApiModel);
            return badResponse;
        }
    }
}
