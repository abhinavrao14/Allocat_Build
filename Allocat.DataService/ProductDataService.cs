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

        public IEnumerable<sp_TissueBankProduct_TissueBank_GetByTissueBankId_Result> GetTbProductsByTissueBankId(int TissueBankId, string SearchBy, int CurrentPage, int PageSize, string SortDirection, string SortExpression, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            IEnumerable<sp_TissueBankProduct_TissueBank_GetByTissueBankId_Result> lstTbProducts = dbConnection.sp_TissueBankProduct_TissueBank_GetByTissueBankId(TissueBankId, SearchBy, CurrentPage, PageSize, SortDirection, SortExpression);

            int numberOfRows = 0;
            if (SearchBy == "")
            {
                numberOfRows = (from tbProduct in dbConnection.TissueBankProduct
                                where tbProduct.TissueBankId == TissueBankId
                                select tbProduct).Count();
            }

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(numberOfRows.ToString()+ " Products found.");

            return lstTbProducts;
        }

        public IEnumerable<sp_TissueBankProduct_TissueBank_GetTissueBankProductsByProductMasterId_Result> GetTissueBankProductsByProductMasterId(int TissueBankId, int ProductMasterId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            IEnumerable<sp_TissueBankProduct_TissueBank_GetTissueBankProductsByProductMasterId_Result> lstTbProducts = dbConnection.sp_TissueBankProduct_TissueBank_GetTissueBankProductsByProductMasterId(TissueBankId, ProductMasterId);

            int numberOfRows = (from tbProduct in dbConnection.TissueBankProduct
                                where tbProduct.TissueBankId == TissueBankId && tbProduct.ProductMasterId == ProductMasterId
                                select tbProduct).Count();

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(numberOfRows.ToString() + " products found.");

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

        public List<string> GetProductSizes(int ProductMasterId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<string> lstProductSize = ((from tbp in dbConnection.TissueBankProduct
                                            where tbp.ProductMasterId == ProductMasterId
                                            select tbp.ProductSize).Distinct()).ToList();
            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(lstProductSize.Count.ToString() + " preservation-types found.");

            return lstProductSize;
        }

        public List<string> GetProductTypes(int ProductMasterId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<string> lstProductType = ((from tbp in dbConnection.TissueBankProduct
                                            where tbp.ProductMasterId==ProductMasterId
                                            select tbp.ProductType).Distinct()).ToList();
            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(lstProductType.Count.ToString() + " product-types found.");

            return lstProductType;
        }
    }
}
