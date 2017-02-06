using Allocat.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allocat.WebApi.WebApiModel
{
    public class ProductMasterApiModel : TransactionalInformation
    {
        public ProductMaster_TissueBank ProductMaster_TissueBank;

        public ProductMasterApiModel()
        {
            ProductMaster_TissueBank = new ProductMaster_TissueBank();
        }
    }

    public class ProductMasterDTO
    {
        public int ProductMasterId { get; set; }
        public string ProductMasterName { get; set; }
    }

    public class ProductMasterInquiryDTO
    {
        public int ProductMasterId { get; set; }
        public string ProductMasterName { get; set; }
        public int CurrentPageNumber { get; set; }
        public string SortExpression { get; set; }
        public string SortDirection { get; set; }
        public int PageSize { get; set; }
    }

    public class ProductMasterGetByIdDTO
    {
        public int Id { get; set; }
        public string OperationType { get; set; }
    }
}