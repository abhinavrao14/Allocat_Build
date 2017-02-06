using Allocat.ApplicationService;
using Allocat.DataModel;
using Allocat.DataService;
using Allocat.DataServiceInterface;
using Allocat.Utility;
using Allocat.WebApi.WebApiModel;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;


namespace Allocat.WebApi.Controllers
{
    public class ProductApiController : ApiController
    {
        protected string toEmail, EmailSubj, EmailMsg;

        IProductDataService productDataService;

        public ProductApiController()
        {
            productDataService = new ProductDataService();
        }

        public HttpResponseMessage Get([FromUri] ProductList_TissueBank_DTO productList_TissueBank_DTO)
        {
            if (productList_TissueBank_DTO.SearchBy == null) productList_TissueBank_DTO.SearchBy = string.Empty;
            if (productList_TissueBank_DTO.SortDirection == null) productList_TissueBank_DTO.SortDirection = string.Empty;
            if (productList_TissueBank_DTO.SortExpression == null) productList_TissueBank_DTO.SortExpression = string.Empty;

            Product_TissueBankApiModel product_TissueBankModel = new Product_TissueBankApiModel();
            TransactionalInformation transaction = new TransactionalInformation();

            if (productList_TissueBank_DTO.SortDirection == "") productList_TissueBank_DTO.SortDirection = "ASC";
            if (productList_TissueBank_DTO.SortExpression == "") productList_TissueBank_DTO.SortExpression = "ProductMasterName";

            ProductBusinessService productBusinessService = new ProductBusinessService(productDataService);

            IEnumerable<sp_TissueBankProductMaster_TissueBank_GetTissueBankProductMastersByTissueBankId_Result> TbProductMasters = productBusinessService.GetTissueBankProductMastersByTissueBankId
                (productList_TissueBank_DTO.TissueBankId, productList_TissueBank_DTO.SearchBy, productList_TissueBank_DTO.CurrentPage, productList_TissueBank_DTO.PageSize, productList_TissueBank_DTO.SortDirection, productList_TissueBank_DTO.SortExpression, out transaction);

            product_TissueBankModel.TbProductMasters = TbProductMasters;
            product_TissueBankModel.ReturnStatus = transaction.ReturnStatus;
            product_TissueBankModel.ReturnMessage = transaction.ReturnMessage;
            product_TissueBankModel.IsAuthenicated = true;

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<Product_TissueBankApiModel>(HttpStatusCode.OK, product_TissueBankModel);
                return response;
            }

            var badResponse = Request.CreateResponse<Product_TissueBankApiModel>(HttpStatusCode.BadRequest, product_TissueBankModel);
            return badResponse;
        }

        public HttpResponseMessage Get([FromUri] int TissueBankProductMasterId)
        {
            Product_TissueBankApiModel product_TissueBankModel = new Product_TissueBankApiModel();
            TransactionalInformation transaction = new TransactionalInformation();

            ProductBusinessService productBusinessService = new ProductBusinessService(productDataService);

            IEnumerable<sp_TissueBankProduct_TissueBank_GetTissueBankProductsByTissueBankProductMasterId_Result> TbProducts = productBusinessService.GetTissueBankProductsByTissueBankProductMasterId
                (TissueBankProductMasterId, out transaction);

            product_TissueBankModel.TbProducts = TbProducts;
            product_TissueBankModel.ReturnStatus = transaction.ReturnStatus;
            product_TissueBankModel.ReturnMessage = transaction.ReturnMessage;
            product_TissueBankModel.IsAuthenicated = true;

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<Product_TissueBankApiModel>(HttpStatusCode.OK, product_TissueBankModel);
                return response;
            }

            var badResponse = Request.CreateResponse<Product_TissueBankApiModel>(HttpStatusCode.BadRequest, product_TissueBankModel);
            return badResponse;
        }

        [HttpPost]
        public HttpResponseMessage POST(IEnumerable<ProductAddUpdate_TissueBank_DTO> Products)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            //converting ienumerable into datatable
            DataTable dtProducts = Utilities.ToDataTable<ProductAddUpdate_TissueBank_DTO>(Products);

            Product_TissueBankApiModel product_TissueBankApiModel = new Product_TissueBankApiModel();

            ProductBusinessService productBusinessService = new ProductBusinessService(productDataService);

            #region DefaultValues
            for (int i = 0; i < dtProducts.Rows.Count; ++i)
            {
                if (dtProducts.Rows[i]["TissueBankProductMasterId"] == null)
                    dtProducts.Rows[i]["TissueBankProductMasterId"] = 0;
                else if (dtProducts.Rows[i]["TissueBankProductMasterId"].ToString() == "")
                    dtProducts.Rows[i]["TissueBankProductMasterId"] = 0;

                if (dtProducts.Rows[i]["IsAvailableForSale"] != null)
                {
                    if (dtProducts.Rows[i]["IsAvailableForSale"].ToString() == "Yes")
                    {
                        dtProducts.Rows[i]["IsAvailableForSale"] = 1;
                    }
                    else if (dtProducts.Rows[i]["IsAvailableForSale"].ToString() == "No")
                    {
                        dtProducts.Rows[i]["IsAvailableForSale"] = 0;
                    }
                }
            }
            #endregion

            productBusinessService.AddUpdateTissueBankProducts(dtProducts, out transaction);

            product_TissueBankApiModel.ReturnMessage = transaction.ReturnMessage;
            product_TissueBankApiModel.ReturnStatus = transaction.ReturnStatus;

            if (transaction.ReturnStatus == false)
            {
                product_TissueBankApiModel.ValidationErrors = transaction.ValidationErrors;
                return Request.CreateResponse<Product_TissueBankApiModel>(HttpStatusCode.BadRequest, product_TissueBankApiModel);
            }
            else
            {
                return Request.CreateResponse<Product_TissueBankApiModel>(HttpStatusCode.OK, product_TissueBankApiModel);
            }
        }

        //public HttpResponseMessage Get([FromUri] ProductList_TissueBank_DTO productList_TissueBank_DTO)
        //{
        //    if (productList_TissueBank_DTO.SearchBy == null) productList_TissueBank_DTO.SearchBy = string.Empty;
        //    if (productList_TissueBank_DTO.SortDirection == null) productList_TissueBank_DTO.SortDirection = string.Empty;
        //    if (productList_TissueBank_DTO.SortExpression == null) productList_TissueBank_DTO.SortExpression = string.Empty;

        //    Product_TissueBankApiModel product_TissueBankModel = new Product_TissueBankApiModel();
        //    TransactionalInformation transaction = new TransactionalInformation();

        //    if (productList_TissueBank_DTO.SortDirection == "") productList_TissueBank_DTO.SortDirection = "ASC";
        //    if (productList_TissueBank_DTO.SortExpression == "") productList_TissueBank_DTO.SortExpression = "ProductMasterName";

        //    ProductBusinessService productBusinessService = new ProductBusinessService(productDataService);

        //    IEnumerable<sp_TissueBankProduct_TissueBank_GetByTissueBankId_Result> products = productBusinessService.GetProducts
        //        (productList_TissueBank_DTO.TissueBankId, productList_TissueBank_DTO.SearchBy, productList_TissueBank_DTO.CurrentPage, productList_TissueBank_DTO.PageSize, productList_TissueBank_DTO.SortDirection, productList_TissueBank_DTO.SortExpression, out transaction);

        //    product_TissueBankModel.Products = products;
        //    product_TissueBankModel.ReturnStatus = transaction.ReturnStatus;
        //    product_TissueBankModel.ReturnMessage = transaction.ReturnMessage;
        //    product_TissueBankModel.IsAuthenicated = true;

        //    if (transaction.ReturnStatus == true)
        //    {
        //        var response = Request.CreateResponse<Product_TissueBankApiModel>(HttpStatusCode.OK, product_TissueBankModel);
        //        return response;
        //    }

        //    var badResponse = Request.CreateResponse<Product_TissueBankApiModel>(HttpStatusCode.BadRequest, product_TissueBankModel);
        //    return badResponse;
        //}
    }
}
