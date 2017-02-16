using Allocat.DataModel;
using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;
using System.Data;

namespace Allocat.ApplicationService
{
    public class ProductBusinessService
    {
        IProductDataService _productDataService;

        private IProductDataService productDataService
        {
            get { return _productDataService; }
        }

        public ProductBusinessService(IProductDataService productDataService)
        {
            _productDataService = productDataService;
        }

        public IEnumerable<sp_TissueBankProductMaster_TissueBank_GetTissueBankProductMastersByTissueBankId_Result> GetTissueBankProductMastersByTissueBankId(int TissueBankId, string SearchBy, int CurrentPage, int PageSize, string SortDirection, string SortExpression, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            IEnumerable<sp_TissueBankProductMaster_TissueBank_GetTissueBankProductMastersByTissueBankId_Result> lstTbProductMasters = null;

            try
            {
                _productDataService.CreateSession();
                lstTbProductMasters = _productDataService.GetTissueBankProductMastersByTissueBankId(TissueBankId, SearchBy, CurrentPage, PageSize, SortDirection, SortExpression, out transaction);
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                _productDataService.CloseSession();
            }

            return lstTbProductMasters;
        }

        public IEnumerable<sp_TissueBankProduct_TissueBank_GetTissueBankProductsByTissueBankProductMasterId_Result> GetTissueBankProductsByTissueBankProductMasterId(int TissueBankProductMasterId,int InfoId,string InfoType, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            IEnumerable<sp_TissueBankProduct_TissueBank_GetTissueBankProductsByTissueBankProductMasterId_Result> lstTbProducts = null;

            ProductBusinessRule productBusinessRule = new ProductBusinessRule(_productDataService);

            try
            {
                _productDataService.CreateSession();

                productBusinessRule.ValidTissueBankProductMasterRequest(TissueBankProductMasterId, InfoId, InfoType);

                if (productBusinessRule.ValidationStatus == true)
                {
                    _productDataService.CreateSession();
                    lstTbProducts = _productDataService.GetTissueBankProductsByTissueBankProductMasterId(TissueBankProductMasterId, out transaction);
                }
                else
                {
                    transaction.ReturnStatus = productBusinessRule.ValidationStatus;
                    transaction.ReturnMessage = productBusinessRule.ValidationMessage;
                    transaction.ValidationErrors = productBusinessRule.ValidationErrors;
                }
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                _productDataService.CloseSession();
            }
            return lstTbProducts;
        }

        public void AddUpdateTissueBankProducts(DataTable tempTissueBankProduct_TissueBank, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            ProductBusinessRule productBusinessRule = new ProductBusinessRule(_productDataService);

            try
            {
                _productDataService.CreateSession();

                productBusinessRule.ValidateProductsDataTable(tempTissueBankProduct_TissueBank);

                if (productBusinessRule.ValidationStatus == true)
                {
                    _productDataService.AddUpdateTissueBankProducts(tempTissueBankProduct_TissueBank, out transaction);
                }
                else
                {
                    transaction.ReturnStatus = productBusinessRule.ValidationStatus;
                    transaction.ReturnMessage = productBusinessRule.ValidationMessage;
                    transaction.ValidationErrors = productBusinessRule.ValidationErrors;
                }
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                _productDataService.CloseSession();
            }
        }

        public List<string> GetPreservationTypes(out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<string> lstPreservationType = null;

            try
            {
                _productDataService.CreateSession();
                lstPreservationType = _productDataService.GetPreservationTypes(out transaction);
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                _productDataService.CloseSession();
            }
            return lstPreservationType;
        }

        public List<Source> GetSources(out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<Source> lstSource = null;

            try
            {
                _productDataService.CreateSession();
                lstSource = _productDataService.GetSources(out transaction);
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                _productDataService.CloseSession();
            }
            return lstSource;
        }

        public List<string> GetProductSizes(int TissueBankProductMasterId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<string> lstProductSize = null;

            try
            {
                _productDataService.CreateSession();
                lstProductSize = _productDataService.GetProductSizes(TissueBankProductMasterId, out transaction);
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                _productDataService.CloseSession();
            }
            return lstProductSize;
        }

        public List<string> GetProductTypes(int TissueBankProductMasterId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<string> lstProductType = null;

            try
            {
                _productDataService.CreateSession();
                lstProductType = _productDataService.GetProductTypes(TissueBankProductMasterId, out transaction);
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                _productDataService.CloseSession();
            }
            return lstProductType;
        }

        public string GetProductMasterBySearch(string SearchBY, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            string pp = "";

            try
            {
                _productDataService.CreateSession();
                pp = _productDataService.GetProductMasterBySearch(SearchBY, out transaction);
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(ex.Message);
            }
            finally
            {
                _productDataService.CloseSession();
            }

            return pp;
        }



        //HOSPITAL
        public List<usp_TissueBankProduct_GetProductSubstitutesByProductMasterName_Hospital_Result> GetProductSubstitutesByProductMasterName_Hospital(string ProductMasterName, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<usp_TissueBankProduct_GetProductSubstitutesByProductMasterName_Hospital_Result> ProductSubstitutes = null;

            try
            {
                _productDataService.CreateSession();
                ProductSubstitutes = _productDataService.GetProductSubstitutesByProductMasterName_Hospital(ProductMasterName, out transaction);
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(ex.Message);
            }
            finally
            {
                _productDataService.CloseSession();
            }

            return ProductSubstitutes;
        }

        public List<usp_TissueBankProduct_GetProductVariationsByProductMasterName_Hospital_Result> GetProductVariationsByProductMasterName_Hospital(string ProductMasterName, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<usp_TissueBankProduct_GetProductVariationsByProductMasterName_Hospital_Result> ProductVariations = null;

            try
            {
                _productDataService.CreateSession();
                ProductVariations = _productDataService.GetProductVariationsByProductMasterName_Hospital(ProductMasterName, out transaction);
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(ex.Message);
            }
            finally
            {
                _productDataService.CloseSession();
            }

            return ProductVariations;
        }

        public List<usp_TissueBankProduct_GetTbOfferingForTissueBankProduct_Hospital_Result> GetTbOfferingForTissueBankProduct_Hospital(string ProductMasterName, string ProductType, string ProductSize, string PreservationType, string SourceName, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<usp_TissueBankProduct_GetTbOfferingForTissueBankProduct_Hospital_Result> TbOfferings = null;

            try
            {
                _productDataService.CreateSession();
                TbOfferings = _productDataService.GetTbOfferingForTissueBankProduct_Hospital(ProductMasterName, ProductType, ProductSize, PreservationType, SourceName, out transaction);
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(ex.Message);
            }
            finally
            {
                _productDataService.CloseSession();
            }

            return TbOfferings;
        }

        public List<Product_Hospital> GetAllProductMasters(out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<Product_Hospital> AllProductMasters = null;

            try
            {
                _productDataService.CreateSession();
                AllProductMasters = _productDataService.GetAllProductMasters(out transaction);
            }
            catch (Exception ex)
            {
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(ex.Message);
            }
            finally
            {
                _productDataService.CloseSession();
            }

            return AllProductMasters;
        }
    }
}
