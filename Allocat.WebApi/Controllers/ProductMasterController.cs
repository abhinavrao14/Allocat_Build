using Allocat.ApplicationService;
using Allocat.DataModel;
using Allocat.DataService;
using Allocat.WebApi.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Allocat.WebApi.Controllers
{
    //[RoutePrefix("api/ProductMaster")]
    public class ProductMasterController : ApiController
    {
        private ProductMasterDataService productMasterDataService;

        public ProductMasterController()
        {
            productMasterDataService = new ProductMasterDataService();
        }

        public HttpResponseMessage GetById([FromUri] ProductMasterGetByIdDTO productMasterGetByIdDTO)
        {
            ProductMasterApiModel productMasterApiModel = new ProductMasterApiModel();
            TransactionalInformation transaction = new TransactionalInformation();

            ProductMasterBusinessService productMasterBusinessService = new ProductMasterBusinessService(productMasterDataService);
            ProductMaster_TissueBank productMaster_TissueBank = new ProductMaster_TissueBank();

            if (productMasterGetByIdDTO.OperationType == "GetByTissueBankProductMasterId")
            {
                productMaster_TissueBank = productMasterBusinessService.GetProductMaster_DomainFamily_ByTissueBankProductMasterId
                    (productMasterGetByIdDTO.Id, out transaction);
            }

            productMasterApiModel.ProductMaster_TissueBank = productMaster_TissueBank;
            productMasterApiModel.IsAuthenicated = true;
            productMasterApiModel.ReturnStatus = transaction.ReturnStatus;
            productMasterApiModel.ReturnMessage = transaction.ReturnMessage;

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<ProductMasterApiModel>(HttpStatusCode.OK, productMasterApiModel);
                return response;
            }

            var badResponse = Request.CreateResponse<ProductMasterApiModel>(HttpStatusCode.BadRequest, productMasterApiModel);
            return badResponse;
        }

    }
}
