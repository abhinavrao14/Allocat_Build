using Allocat.DataModel;
using System;
using System.Collections.Generic;

namespace Allocat.WebApi.WebApiModel
{
    public class Order_TissueBankApiModel : TransactionalInformation
    {
        public IEnumerable<sp_Order_TissueBank_GetByTissueBankId_Result> Orders;
        public IEnumerable<sp_OrderDetail_TissueBank_GetByOrderId_Result> OrderDetail;
    }

    public class Order_TissueBank_DTO
    {
        public int TissueBankId { get; set; }
        public string SearchBy { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string SortDirection { get; set; }
        public string SortExpression { get; set; }
    }

    public class Order_Ack_Decline_DTO
    {
        public int OrderId { get; set; }
        public int StatusId { get; set; }
        public string DeclineRemark { get; set; }
        public string ShippingMethod { get; set; }
        public DateTime TissueBankSendByDate { get; set; }
        public int LastModifiedBy { get; set; }
    }


    //public class ProductAddUpdate_TissueBank_DTO
    //{
    //    public int TissueBankProductId { get; set; }
    //    public Nullable<int> TissueBankId { get; set; }
    //    public Nullable<int> ProductMasterId { get; set; }
    //    public string ProductType { get; set; }
    //    public string ProductCode { get; set; }
    //    public string ProductSize { get; set; }
    //    public string PreservationType { get; set; }
    //    public string ProductDescription { get; set; }
    //    public string Remark { get; set; }
    //    public Nullable<decimal> UnitPrice { get; set; }
    //    public Nullable<int> SourceId { get; set; }
    //    public string IsAvailableForSale { get; set; }
    //    public Nullable<System.DateTime> AvailabilityStartDate { get; set; }
    //    public Nullable<System.DateTime> AvailabilityEndDate { get; set; }
    //    public Nullable<int> CreatedBy { get; set; }
    //    public Nullable<int> LastModifiedBy { get; set; }
    //}
}