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
using Allocat.WebApi.CustomService;

namespace Allocat.WebApi.Controllers
{
    public class ProductHospitalController : ApiController
    {
        IProductDataService productDataService;

        public ProductHospitalController()
        {
            productDataService = new ProductDataService();
        }

        public HttpResponseMessage Get([FromUri] Product_Hospital_DTO product_Hospital_DTO)
        {
            Product_HospitalApiModel product_HospitalApiModel = new Product_HospitalApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            ProductBusinessService productBusinessService = new ProductBusinessService(productDataService);

            if (product_Hospital_DTO.OperationType == "GetAll")
            {
                List<Product_Hospital> AllProductMasters = productBusinessService.GetAllProductMasters
                    (out transaction);

                product_HospitalApiModel.AllProductMasters = AllProductMasters;
            }
            else if (product_Hospital_DTO.OperationType == "GetProductVariations")
            {
                List<usp_TissueBankProduct_GetProductVariationsByProductMasterName_Hospital_Result> ProductVariations = productBusinessService.GetProductVariationsByProductMasterName_Hospital
                    (product_Hospital_DTO.ProductMasterName, out transaction);

                product_HospitalApiModel.ProductVariations = ProductVariations;
            }
            else if (product_Hospital_DTO.OperationType == "GetProductSubstitutes")
            {
                List<usp_TissueBankProduct_GetProductSubstitutesByProductMasterName_Hospital_Result> ProductSubstitutes = productBusinessService.GetProductSubstitutesByProductMasterName_Hospital
                (product_Hospital_DTO.ProductMasterName, out transaction);

                product_HospitalApiModel.ProductSubstitutes = ProductSubstitutes;
            }
            else if (product_Hospital_DTO.OperationType == "GetTbOffering")
            {
                List<usp_TissueBankProduct_GetTbOfferingForTissueBankProduct_Hospital_Result> TbOfferings = productBusinessService.GetTbOfferingForTissueBankProduct_Hospital
                (product_Hospital_DTO.ProductMasterName, product_Hospital_DTO.ProductType, product_Hospital_DTO.ProductSize, product_Hospital_DTO.PreservationType, product_Hospital_DTO.SourceName, out transaction);

                product_HospitalApiModel.TbOfferings = TbOfferings;
            }

            product_HospitalApiModel.ReturnStatus = transaction.ReturnStatus;
            product_HospitalApiModel.ReturnMessage = transaction.ReturnMessage;

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<Product_HospitalApiModel>(HttpStatusCode.OK, product_HospitalApiModel);
                return response;
            }

            var badResponse = Request.CreateResponse<Product_HospitalApiModel>(HttpStatusCode.BadRequest, product_HospitalApiModel);
            return badResponse;
        }

        //[HttpPost]
        //public HttpResponseMessage POST(IEnumerable<ProductAddUpdate_TissueBank_DTO> Products)
        //{
        //    TransactionalInformation transaction = new TransactionalInformation();
        //    //converting ienumerable into datatable
        //    DataTable dtProducts = Utilities.ToDataTable<ProductAddUpdate_TissueBank_DTO>(Products);

        //    Product_TissueBankApiModel product_TissueBankApiModel = new Product_TissueBankApiModel();

        //    ProductBusinessService productBusinessService = new ProductBusinessService(productDataService);

        //    #region DefaultValues
        //    for (int i = 0; i < dtProducts.Rows.Count; ++i)
        //    {
        //        if (dtProducts.Rows[i]["TissueBankProductMasterId"] == null)
        //            dtProducts.Rows[i]["TissueBankProductMasterId"] = 0;
        //        else if (dtProducts.Rows[i]["TissueBankProductMasterId"].ToString() == "")
        //            dtProducts.Rows[i]["TissueBankProductMasterId"] = 0;

        //        if (dtProducts.Rows[i]["IsAvailableForSale"] != null)
        //        {
        //            if (dtProducts.Rows[i]["IsAvailableForSale"].ToString() == "Yes")
        //            {
        //                dtProducts.Rows[i]["IsAvailableForSale"] = 1;
        //            }
        //            else if (dtProducts.Rows[i]["IsAvailableForSale"].ToString() == "No")
        //            {
        //                dtProducts.Rows[i]["IsAvailableForSale"] = 0;
        //            }
        //        }
        //    }
        //    #endregion

        //    productBusinessService.AddUpdateTissueBankProducts(dtProducts, out transaction);

        //    product_TissueBankApiModel.ReturnMessage = transaction.ReturnMessage;
        //    product_TissueBankApiModel.ReturnStatus = transaction.ReturnStatus;

        //    if (transaction.ReturnStatus == false)
        //    {
        //        product_TissueBankApiModel.ValidationErrors = transaction.ValidationErrors;
        //        return Request.CreateResponse<Product_TissueBankApiModel>(HttpStatusCode.BadRequest, product_TissueBankApiModel);
        //    }
        //    else
        //    {
        //        return Request.CreateResponse<Product_TissueBankApiModel>(HttpStatusCode.OK, product_TissueBankApiModel);
        //    }
        //}
    }
}
