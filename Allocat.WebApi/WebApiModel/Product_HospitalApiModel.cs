using Allocat.DataModel;
using System;
using System.Collections.Generic;

namespace Allocat.WebApi.WebApiModel
{
    public class Product_HospitalApiModel : TransactionalInformation
    {
        public List<usp_TissueBankProduct_GetProductVariationsByProductMasterName_Hospital_Result> ProductVariations;
        public List<usp_TissueBankProduct_GetProductSubstitutesByProductMasterName_Hospital_Result> ProductSubstitutes;
        public List<usp_TissueBankProduct_GetTbOfferingForTissueBankProduct_Hospital_Result> TbOfferings;
        public List<Product_Hospital> AllProductMasters;
        public List<TbOfferingForRFQ_Hospital> TbOfferingsForRFQ;
        public string ProductMasterCommaSeparated;
    }

    public class Product_Hospital_DTO
    {
        public int HospitalId { get; set; }
        public string ProductMasterName { get; set; }
        public string ProductType { get; set; }
        public string ProductSize { get; set; }
        public string PreservationType { get; set; }
        public string SourceName { get; set; }
        public string OperationType { get; set; }
        public int[] TissueBankProductIds { get; set; }
    }

    public class TissueBankProductIds_Hospital
    {
        public int TissueBankProductId { get; set; }
    }
}