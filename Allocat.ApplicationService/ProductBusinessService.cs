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

        public IEnumerable<sp_TissueBankProduct_TissueBank_GetByTissueBankId_Result> GetProducts(int TissueBankId, string SearchBy, int CurrentPage, int PageSize, string SortDirection, string SortExpression, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            IEnumerable<sp_TissueBankProduct_TissueBank_GetByTissueBankId_Result> lstTbProducts = null;

            try
            {
                _productDataService.CreateSession();
                lstTbProducts = _productDataService.GetTbProductsByTissueBankId(TissueBankId, SearchBy, CurrentPage, PageSize, SortDirection, SortExpression, out transaction);
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

        public IEnumerable<sp_TissueBankProduct_TissueBank_GetTissueBankProductsByProductMasterId_Result> GetTissueBankProductsByProductMasterId(int TissueBankId, int ProductMasterId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            IEnumerable<sp_TissueBankProduct_TissueBank_GetTissueBankProductsByProductMasterId_Result> lstTbProducts = null;

            try
            {
                _productDataService.CreateSession();
                lstTbProducts = _productDataService.GetTissueBankProductsByProductMasterId(TissueBankId, ProductMasterId, out transaction);
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

            ProductBusinessRule productBusinessRule = new ProductBusinessRule();

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

        public List<string> GetProductSizes(int ProductMasterId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<string> lstProductSize = null;

            try
            {
                _productDataService.CreateSession();
                lstProductSize = _productDataService.GetProductSizes(ProductMasterId, out transaction);
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

        public List<string> GetProductTypes(int ProductMasterId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<string> lstProductType = null;

            try
            {
                _productDataService.CreateSession();
                lstProductType = _productDataService.GetProductTypes(ProductMasterId, out transaction);
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
    }
}
