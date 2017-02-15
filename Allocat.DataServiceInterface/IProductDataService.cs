using Allocat.DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allocat.DataServiceInterface
{
    public interface IProductDataService : IDataService, IDisposable
    {
        IEnumerable<sp_TissueBankProductMaster_TissueBank_GetTissueBankProductMastersByTissueBankId_Result> GetTissueBankProductMastersByTissueBankId(int TissueBankId, string SearchBy, int CurrentPage, int PageSize, string SortDirection, string SortExpression, out TransactionalInformation transaction);
        IEnumerable<sp_TissueBankProduct_TissueBank_GetTissueBankProductsByTissueBankProductMasterId_Result> GetTissueBankProductsByTissueBankProductMasterId(int TissueBankProductMasterId, out TransactionalInformation transaction);

        int AddUpdateTissueBankProducts(DataTable tempTissueBankProduct_TissueBank, out TransactionalInformation transaction);
        
        List<string> GetPreservationTypes(out TransactionalInformation transaction);
        List<Source> GetSources(out TransactionalInformation transaction);
        List<string> GetProductSizes(int TissueBankProductMasterId, out TransactionalInformation transaction);
        List<string> GetProductTypes(int TissueBankProductMasterId, out TransactionalInformation transaction);

        string GetProductMasterBySearch(string SearchBY, out TransactionalInformation transaction);
        //bool ValidateUniqueProductCodeInTissueBank(string ProductCode, int TissueBankId);

        bool ValidTissueBankProductMasterRequest(int TissueBankProductMasterId, int TissueBankId);
    }
}
