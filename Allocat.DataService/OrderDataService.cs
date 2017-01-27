using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using Allocat.DataModel;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace Allocat.DataService
{
    public class OrderDataService : EntityFrameworkDataService, IOrderDataService
    {
        public IEnumerable<sp_Order_TissueBank_GetByTissueBankId_Result> GetOrderByTissueBankId(int TissueBankId, string SearchBy, int CurrentPage, int PageSize, string SortDirection, string SortExpression, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            IEnumerable<sp_Order_TissueBank_GetByTissueBankId_Result> lstOrder = dbConnection.sp_Order_TissueBank_GetByTissueBankId(TissueBankId, SearchBy, CurrentPage, PageSize, SortDirection, SortExpression);

            int numberOfRows = dbConnection.Order.Count();

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(numberOfRows.ToString() + " orders found.");

            return lstOrder;
        }

        public IEnumerable<sp_OrderDetail_TissueBank_GetByOrderId_Result> GetOrderDetailByOrderId(int OrderId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            IEnumerable<sp_OrderDetail_TissueBank_GetByOrderId_Result> OrderDetail = dbConnection.sp_OrderDetail_TissueBank_GetByOrderId(OrderId);

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add("Order Detail found.");

            return OrderDetail;
        }

        public int Order_Ack_Decline(int OrderId, int StatusId, string DeclineRemark, string ShippingMethod, DateTime TissueBankSendByDate, int LastModifiedBy,out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            int rowAffected = 0;
            var parameterOrderId = new SqlParameter("@OrderId", SqlDbType.Int);
            parameterOrderId.Value = OrderId;

            var parameterStatusId = new SqlParameter("@StatusId", SqlDbType.Int);
            parameterStatusId.Value = StatusId;

            var parameterDeclineRemark = new SqlParameter("@DeclineRemark", SqlDbType.NVarChar);
            if (DeclineRemark != null)
                parameterDeclineRemark.Value = DeclineRemark;
            else
                parameterDeclineRemark.Value = DBNull.Value;

            var parameterShippingMethod = new SqlParameter("@ShippingMethod", SqlDbType.NVarChar);
            if (ShippingMethod != null)
                parameterShippingMethod.Value = ShippingMethod;
            else
                parameterShippingMethod.Value = DBNull.Value;

            var parameterTissueBankSendByDate = new SqlParameter("@TissueBankSendByDate", SqlDbType.Date);
            if (TissueBankSendByDate != null)
                parameterTissueBankSendByDate.Value = TissueBankSendByDate;
            else
                parameterTissueBankSendByDate.Value = DBNull.Value;

            var parameterLastModifiedBy = new SqlParameter("@LastModifiedBy", SqlDbType.Int);
            parameterLastModifiedBy.Value = LastModifiedBy;

            rowAffected = dbConnection.Database.ExecuteSqlCommand("exec dbo.sp_Order_TissueBank_Ack_Decline @OrderId, @StatusId, @DeclineRemark, @ShippingMethod, @TissueBankSendByDate, @LastModifiedBy", parameterOrderId, parameterStatusId, parameterDeclineRemark, parameterShippingMethod, parameterTissueBankSendByDate, parameterLastModifiedBy);

            if (rowAffected > 0)
            {
                transaction.ReturnStatus = true;
                transaction.ReturnMessage.Add("Purchase Order is updated successfully.");
            }
            else
            {
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add("Database Error");
            }

            return rowAffected;
        }
    }
}
