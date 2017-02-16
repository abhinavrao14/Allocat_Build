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

        public ProductMaster_TissueBank GetProductMaster_DomainFamily_ByTissueBankProductMasterId(int TissueBankProductMasterId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            ProductMaster_TissueBank productMaster_TissueBank = new ProductMaster_TissueBank();

            try
            {
                _productMasterDataService.CreateSession();
                productMaster_TissueBank = _productMasterDataService.GetProductMaster_DomainFamily_ByTissueBankProductMasterId(TissueBankProductMasterId, out transaction);
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

        public ProductMaster_Hospital GetProductMasterByProductMasterName(string ProductMasterName, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            ProductMaster_Hospital productMaster_Hospital = new ProductMaster_Hospital();

            try
            {
                _productMasterDataService.CreateSession();
                productMaster_Hospital = _productMasterDataService.GetProductMasterByProductMasterName(ProductMasterName, out transaction);
            }
            catch (Exception ex)
            {
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(ex.Message);
            }
            finally
            {
                _productMasterDataService.CloseSession();
            }

            return productMaster_Hospital;
        }
    }
}
