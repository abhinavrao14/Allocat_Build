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
        IEnumerable<sp_TissueBankProduct_TissueBank_GetByTissueBankId_Result> GetTbProductsByTissueBankId(int TissueBankId, string SearchBy, int CurrentPage, int PageSize, string SortDirection, string SortExpression, out TransactionalInformation transaction);
        IEnumerable<sp_TissueBankProduct_TissueBank_GetTissueBankProductsByProductMasterId_Result> GetTissueBankProductsByProductMasterId(int TissueBankId, int ProductMasterId, out TransactionalInformation transaction);
        int AddUpdateTissueBankProducts(DataTable tempTissueBankProduct_TissueBank, out TransactionalInformation transaction);
        List<string> GetPreservationTypes(out TransactionalInformation transaction);
        List<Source> GetSources(out TransactionalInformation transaction);
        List<string> GetProductSizes(int ProductMasterId, out TransactionalInformation transaction);
        List<string> GetProductTypes(int ProductMasterId, out TransactionalInformation transaction);
    }
}
