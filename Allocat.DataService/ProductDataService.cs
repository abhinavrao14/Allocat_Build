using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using Allocat.DataModel;
using System.Data;
using System.Data.SqlClient;

namespace Allocat.DataService
{
    public class ProductDataService : EntityFrameworkDataService, IProductDataService
    {
        public int AddUpdateTissueBankProducts(DataTable tempTissueBankProduct_TissueBank, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            int rowAffected = 0;
            var parameter = new SqlParameter("@temp", SqlDbType.Structured);
            parameter.Value = tempTissueBankProduct_TissueBank;
            parameter.TypeName = "dbo.tempTissueBankProduct_TissueBank";

            rowAffected = dbConnection.Database.ExecuteSqlCommand("exec dbo.sp_TissueBankProduct_TissueBank_AddUpdate @temp", parameter);

            if (rowAffected > 0)
            {
                transaction.ReturnStatus = true;
                transaction.ReturnMessage.Add("Products are updated/added successfully.");
            }
            else
            {
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add("Database Error");
            }

            return rowAffected;
        }

        public IEnumerable<sp_TissueBankProductMaster_TissueBank_GetTissueBankProductMastersByTissueBankId_Result> GetTissueBankProductMastersByTissueBankId(int TissueBankId, string SearchBy, int CurrentPage, int PageSize, string SortDirection, string SortExpression, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            IEnumerable<sp_TissueBankProductMaster_TissueBank_GetTissueBankProductMastersByTissueBankId_Result> lstTbProductMasters = dbConnection.sp_TissueBankProductMaster_TissueBank_GetTissueBankProductMastersByTissueBankId(TissueBankId, SearchBy, CurrentPage, PageSize, SortDirection, SortExpression);

            int Count = ((from tbpm in dbConnection.TissueBankProductMaster
                          join tb in dbConnection.TissueBank on tbpm.TissueBankId equals tb.TissueBankId
                          join tbp in dbConnection.TissueBankProduct on tbpm.TissueBankProductMasterId equals tbp.TissueBankProductMasterId
                          join pm in dbConnection.ProductMaster on tbpm.ProductMasterId equals pm.ProductMasterId
                          where tbpm.TissueBankId == TissueBankId && tbpm.IsActive == true && tb.IsActive == true && pm.IsActive == true && tbp.IsActive == true && pm.ProductMasterName.Contains(SearchBy)
                          select tbpm).Distinct().Count());

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(Count + " Product Masters found.");

            return lstTbProductMasters;
        }

        public IEnumerable<sp_TissueBankProduct_TissueBank_GetTissueBankProductsByTissueBankProductMasterId_Result> GetTissueBankProductsByTissueBankProductMasterId(int TissueBankProductMasterId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            IEnumerable<sp_TissueBankProduct_TissueBank_GetTissueBankProductsByTissueBankProductMasterId_Result> lstTbProducts = dbConnection.sp_TissueBankProduct_TissueBank_GetTissueBankProductsByTissueBankProductMasterId(TissueBankProductMasterId);

            int Count = ((from tbp in dbConnection.TissueBankProduct
                          join tb in dbConnection.TissueBank on tbp.TissueBankId equals tb.TissueBankId
                          join tbpm in dbConnection.TissueBankProductMaster on tbp.TissueBankProductMasterId equals tbpm.TissueBankProductMasterId
                          join pm in dbConnection.ProductMaster on tbpm.ProductMasterId equals pm.ProductMasterId
                          where tbpm.TissueBankProductMasterId == TissueBankProductMasterId && tbpm.IsActive == true && tb.IsActive == true && pm.IsActive == true && tbp.IsActive == true
                          select tbp).Count());

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(Count + " TB Products found.");

            return lstTbProducts;
        }

        public List<string> GetPreservationTypes(out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<string> lstPreservationType = (from tbp in dbConnection.TissueBankProduct
                                                select tbp.PreservationType).Distinct().ToList();

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(lstPreservationType.Count.ToString() + " preservation-types found.");

            return lstPreservationType;
        }

        public List<Source> GetSources(out TransactionalInformation transaction)
        {

            dbConnection.Configuration.ProxyCreationEnabled = false;

            transaction = new TransactionalInformation();

            List<Source> lstSource = (from Source in dbConnection.Source
                                      select Source).ToList();
            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(lstSource.Count.ToString() + " sources found.");

            return lstSource;
        }

        public List<string> GetProductSizes(int TissueBankProductMasterId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<string> lstProductSize = ((from tbp in dbConnection.TissueBankProduct
                                            join tbpm in dbConnection.TissueBankProductMaster on tbp.TissueBankProductMasterId equals tbpm.TissueBankProductMasterId
                                            join pm in dbConnection.ProductMaster on tbpm.ProductMasterId equals pm.ProductMasterId
                                            where tbpm.TissueBankProductMasterId == TissueBankProductMasterId
                                            select tbp.ProductSize).Distinct()).ToList();
            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(lstProductSize.Count.ToString() + " preservation-types found.");

            return lstProductSize;
        }

        public List<string> GetProductTypes(int TissueBankProductMasterId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<string> lstProductType = ((from tbp in dbConnection.TissueBankProduct
                                            join tbpm in dbConnection.TissueBankProductMaster on tbp.TissueBankProductMasterId equals tbpm.TissueBankProductMasterId
                                            join pm in dbConnection.ProductMaster on tbpm.ProductMasterId equals pm.ProductMasterId
                                            where tbpm.TissueBankProductMasterId == TissueBankProductMasterId
                                            select tbp.ProductType).Distinct()).ToList();
            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(lstProductType.Count.ToString() + " product-types found.");

            return lstProductType;
        }

        //public bool ValidateUniqueProductCodeInTissueBank(string ProductCode, int TissueBankId)
        //{
        //    TissueBankProduct tbp = dbConnection.TissueBankProduct.FirstOrDefault(t => t.ProductCode == ProductCode && t.TissueBankId!= TissueBankId);
        //    if (tbp == null)
        //        return true;

        //    return false;
        //}
    }
}
