using Allocat.DataModel;
using System;
using System.Collections.Generic;

namespace Allocat.WebApi.WebApiModel
{
    public class Product_TissueBankApiModel : TransactionalInformation
    {
        public IEnumerable<sp_TissueBankProductMaster_TissueBank_GetTissueBankProductMastersByTissueBankId_Result> TbProductMasters;
        public IEnumerable<sp_TissueBankProduct_TissueBank_GetTissueBankProductsByTissueBankProductMasterId_Result> TbProducts;
        public string ProductMasterCommaSeparated;

        public List<string> PreservationTypes;
        public List<Source> Sources;
        public List<string> ProductSizes;
        public List<string> ProductTypes;
    }

    public class ProductList_TissueBank_DTO
    {
        public int TissueBankId { get; set; }
        public string SearchBy { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string SortDirection { get; set; }
        public string SortExpression { get; set; }


        public int TissueBankProductMasterId { get; set; }
        public string InfoType { get; set; }
        public string OperationType { get; set; }

    }

    public class ProductAddUpdate_TissueBank_DTO
    {
        public int TissueBankProductId { get; set; }
        public Nullable<int> TissueBankId { get; set; }
        public Nullable<int> TissueBankProductMasterId { get; set; }
        public string ProductType { get; set; }
        public string ProductCode { get; set; }
        public string ProductSize { get; set; }
        public string PreservationType { get; set; }
        public string ProductDescription { get; set; }
        public string Remark { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<int> SourceId { get; set; }
        public string IsAvailableForSale { get; set; }
        public Nullable<System.DateTime> AvailabilityStartDate { get; set; }
        public Nullable<System.DateTime> AvailabilityEndDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
    }
}