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


        //hospital
        List<usp_TissueBankProduct_GetProductSubstitutesByProductMasterName_Hospital_Result> GetProductSubstitutesByProductMasterName_Hospital(string ProductMasterName, out TransactionalInformation transaction);
        List<usp_TissueBankProduct_GetProductVariationsByProductMasterName_Hospital_Result> GetProductVariationsByProductMasterName_Hospital(string ProductMasterName, out TransactionalInformation transaction);
        List<usp_TissueBankProduct_GetTbOfferingForTissueBankProduct_Hospital_Result> GetTbOfferingForTissueBankProduct_Hospital(string ProductMasterName, string ProductType, string ProductSize, string PreservationType, string SourceName, out TransactionalInformation transaction);
        List<Product_Hospital> GetAllProductMasters(out TransactionalInformation transaction);
    }
}
