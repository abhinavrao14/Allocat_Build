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

        //[Route("GetProductMasters")]
        [HttpGet]
        //public HttpResponseMessage GetProductMaster([FromUri] ProductMasterInquiryDTO productMasterInquiryDTO)
        //{
        //    if (productMasterInquiryDTO.ProductMasterName == null) productMasterInquiryDTO.ProductMasterName = string.Empty;
        //    if (productMasterInquiryDTO.SortDirection == null) productMasterInquiryDTO.SortDirection = string.Empty;
        //    if (productMasterInquiryDTO.SortExpression == null) productMasterInquiryDTO.SortExpression = string.Empty;

        //    ProductMasterApiModel productMasterApiModel = new ProductMasterApiModel();
        //    TransactionalInformation transaction = new TransactionalInformation();
        //    ProductMasterBusinessService productMasterBusinessService;

        //    DataGridPagingInformation paging = new DataGridPagingInformation();
        //    paging.CurrentPageNumber = productMasterInquiryDTO.CurrentPageNumber;
        //    paging.PageSize = productMasterInquiryDTO.PageSize;
        //    paging.SortExpression = productMasterInquiryDTO.SortExpression;
        //    paging.SortDirection = productMasterInquiryDTO.SortDirection;

        //    if (paging.SortDirection == "") paging.SortDirection = "ASC";
        //    if (paging.SortExpression == "") paging.SortExpression = "ProductMasterName";

        //    productMasterBusinessService = new ProductMasterBusinessService(productMasterDataService);

        //    List<ProductMaster> productMasters = productMasterBusinessService.GetProductMasters(productMasterInquiryDTO.ProductMasterName, paging, out transaction);

        //    productMasterApiModel.ProductMasters = productMasters;
        //    productMasterApiModel.IsAuthenicated = true;
        //    productMasterApiModel.ReturnStatus = transaction.ReturnStatus;
        //    productMasterApiModel.ReturnMessage = transaction.ReturnMessage;
        //    productMasterApiModel.IsAuthenicated = true;

        //    if (transaction.ReturnStatus == true)
        //    {
        //        var response = Request.CreateResponse<ProductMasterApiModel>(HttpStatusCode.OK, productMasterApiModel);
        //        return response;
        //    }

        //    var badResponse = Request.CreateResponse<ProductMasterApiModel>(HttpStatusCode.BadRequest, productMasterApiModel);
        //    return badResponse;
        //}

        public HttpResponseMessage GetById(int ProductMasterId)
        {
            ProductMasterApiModel productMasterApiModel = new ProductMasterApiModel();
            TransactionalInformation transaction = new TransactionalInformation();

            ProductMasterBusinessService productMasterBusinessService = new ProductMasterBusinessService(productMasterDataService);

            ProductMaster_TissueBank productMaster_TissueBank = productMasterBusinessService.GetProductMaster_DomainFamily_ById
                (ProductMasterId, out transaction);

            productMasterApiModel.ProductMaster_TissueBank = productMaster_TissueBank;
            productMasterApiModel.IsAuthenicated = true;
            productMasterApiModel.ReturnStatus = transaction.ReturnStatus;
            productMasterApiModel.ReturnMessage = transaction.ReturnMessage;
            productMasterApiModel.IsAuthenicated = true;

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
