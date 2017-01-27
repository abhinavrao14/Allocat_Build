using Allocat.DataModel;
using Allocat.DataService;
using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allocat.ApplicationService
{
    public class ProductMasterBusinessService
    {
        private IProductMasterDataService _productMasterDataService;

        public ProductMasterBusinessService(IProductMasterDataService dataService)
        {
            _productMasterDataService = dataService;
        }

        //public List<ProductMaster> GetProductMasters(string ProductMasterName, DataGridPagingInformation paging, out TransactionalInformation transaction)
        //{
        //    transaction = new TransactionalInformation();

        //    List<ProductMaster> productMasters = new List<ProductMaster>();

        //    try
        //    {
        //        _productMasterDataService.CreateSession();
        //        productMasters = _productMasterDataService.GetProductMasters(ProductMasterName, paging, out transaction);
        //    }
        //    catch (Exception ex)
        //    {
        //        transaction.ReturnMessage = new List<string>();
        //        string errorMessage = ex.Message;
        //        transaction.ReturnStatus = false;
        //        transaction.ReturnMessage.Add(errorMessage);
        //    }
        //    finally
        //    {
        //        _productMasterDataService.CloseSession();
        //    }

        //    return productMasters;

        //}

        public ProductMaster_TissueBank GetProductMaster_DomainFamily_ById(int ProductMasterId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            ProductMaster_TissueBank productMaster_TissueBank = new ProductMaster_TissueBank();

            try
            {
                _productMasterDataService.CreateSession();
                productMaster_TissueBank = _productMasterDataService.GetProductMaster_DomainFamily_ById(ProductMasterId, out transaction);
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
                _productMasterDataService.CloseSession();
            }

            return productMaster_TissueBank;
        }
    }
}
