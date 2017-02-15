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

        public int Order_Ack_Decline(int OrderId, int StatusId, string DeclineRemark, string ShippingMethod, DateTime TissueBankSendByDate, int LastModifiedBy, int TransactionId, string AuthCode, string ResponseBody, string AuthTransactionId, int TransactionStatusId, int TissueBankId, out TransactionalInformation transaction)
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

            var parameteTransactionId = new SqlParameter("@TransactionId", SqlDbType.VarChar);
            if (TransactionId != 0)
                parameteTransactionId.Value = TransactionId;
            else
                parameteTransactionId.Value = DBNull.Value;

            parameteTransactionId.Value = TransactionId;

            var parameterAuthCode = new SqlParameter("@AuthCode", SqlDbType.VarChar);
            parameterAuthCode.Value = AuthCode;

            var parameterResponseBody = new SqlParameter("@ResponseBody", SqlDbType.VarChar);
            parameterResponseBody.Value = ResponseBody;

            var parameterAuthTransactionId = new SqlParameter("@AuthTransactionId", SqlDbType.VarChar);
            parameterAuthTransactionId.Value = AuthTransactionId;

            var parameterTransactionStatusId = new SqlParameter("@TransactionStatusId", SqlDbType.VarChar);
            if (TransactionStatusId != 0)
                parameterTransactionStatusId.Value = TransactionStatusId;
            else
                parameterTransactionStatusId.Value = DBNull.Value;

            var parameterTissueBankId = new SqlParameter("@TissueBankId", SqlDbType.VarChar);
            parameterTissueBankId.Value = TissueBankId;


            rowAffected = dbConnection.Database.ExecuteSqlCommand("exec dbo.sp_Order_TissueBank_Ack_Decline @OrderId, @StatusId, @DeclineRemark, @ShippingMethod, @TissueBankSendByDate, @LastModifiedBy, @TransactionId , @AuthCode , @ResponseBody, @AuthTransactionId, @TransactionStatusId, @TissueBankId", parameterOrderId, parameterStatusId, parameterDeclineRemark, parameterShippingMethod, parameterTissueBankSendByDate, parameterLastModifiedBy,parameteTransactionId,parameterAuthCode, parameterResponseBody, parameterAuthTransactionId, parameterTransactionStatusId, parameterTissueBankId);

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

        public OrderCommisionDetail_TissueBank GetOrderCommisionDetail(int OrderId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            OrderCommisionDetail_TissueBank orderCommisionDetail = (from o in dbConnection.Order
                                                                    join od in dbConnection.OrderDetail on o.OrderId equals od.OrderId
                                                                    join pe in dbConnection.ProductEntity on od.ProductEntityId equals pe.ProductEntityId
                                                                    join tb in dbConnection.TissueBank on pe.TissueBankId equals tb.TissueBankId
                                                                    where o.OrderId == OrderId
                                                                    select new OrderCommisionDetail_TissueBank
                                                                    {
                                                                        AlloCATFees = o.AlloCATFees,
                                                                        CustomerProfileId = tb.CustomerProfileId,
                                                                        CustomerPaymentProfileIds = tb.CustomerPaymentProfileIds,
                                                                        TissueBankId=tb.TissueBankId
                                                                    }).FirstOrDefault();

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add("orderCommisionDetail found.");

            return orderCommisionDetail;
        }

        public bool ValidateOrderDetailRequest(int OrderId, int TissueBankId)
        {
            int count = (from o in dbConnection.Order
                         join od in dbConnection.OrderDetail on o.OrderId equals od.OrderId
                         join pe in dbConnection.ProductEntity on od.ProductEntityId equals pe.ProductEntityId
                         join tb in dbConnection.TissueBank on pe.TissueBankId equals tb.TissueBankId
                         where o.OrderId == OrderId && pe.TissueBankId == TissueBankId
                         select o).Count();

            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

     
    }
}
