using Allocat.DataModel;
using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;

namespace Allocat.ApplicationService
{
    public class OrderBusinessService
    {
        private IOrderDataService orderDataService;

        public OrderBusinessService(IOrderDataService _orderDataService)
        {
            orderDataService = _orderDataService;
        }

        public IEnumerable<sp_Order_TissueBank_GetByTissueBankId_Result> GetOrderByTissueBankId(int TissueBankId, string SearchBy, int CurrentPage, int PageSize, string SortDirection, string SortExpression, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            OrderBusinessRule orderBusinessRule = new OrderBusinessRule();

            IEnumerable<sp_Order_TissueBank_GetByTissueBankId_Result> lstOrder = null;

            try
            {
                orderDataService.CreateSession();

                orderBusinessRule.ValidateOrderRequest(TissueBankId, SearchBy, CurrentPage, PageSize, SortDirection, SortExpression);

                if (orderBusinessRule.ValidationStatus == true)
                {
                    lstOrder = orderDataService.GetOrderByTissueBankId(TissueBankId, SearchBy, CurrentPage, PageSize, SortDirection, SortExpression, out transaction);
                }
                else
                {
                    transaction.ReturnStatus = orderBusinessRule.ValidationStatus;
                    transaction.ReturnMessage = orderBusinessRule.ValidationMessage;
                    transaction.ValidationErrors = orderBusinessRule.ValidationErrors;
                }
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                orderDataService.CloseSession();
            }

            return lstOrder;
        }

        public IEnumerable<sp_OrderDetail_TissueBank_GetByOrderId_Result> GetOrderDetailByOrderId(int OrderId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            IEnumerable<sp_OrderDetail_TissueBank_GetByOrderId_Result> OrderDetail = null;
            OrderBusinessRule orderBusinessRule = new OrderBusinessRule();

            try
            {
                orderDataService.CreateSession();

                orderBusinessRule.ValidateOrderDetailRequest(OrderId);

                if (orderBusinessRule.ValidationStatus == true)
                {
                    orderDataService.CreateSession();
                    OrderDetail = orderDataService.GetOrderDetailByOrderId(OrderId, out transaction);
                }
                else
                {
                    transaction.ReturnStatus = orderBusinessRule.ValidationStatus;
                    transaction.ReturnMessage = orderBusinessRule.ValidationMessage;
                    transaction.ValidationErrors = orderBusinessRule.ValidationErrors;
                }

            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                orderDataService.CloseSession();
            }

            return OrderDetail;

        }

        public void Order_Ack_Decline(int OrderId, int StatusId, string DeclineRemark, string ShippingMethod, DateTime TissueBankSendByDate,int LastModifiedBy, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            OrderBusinessRule orderBusinessRule = new OrderBusinessRule();

            try
            {
                orderDataService.CreateSession();

                orderBusinessRule.ValidateOrder_Ack_Decline(OrderId, StatusId, DeclineRemark, ShippingMethod, TissueBankSendByDate, LastModifiedBy);

                if (orderBusinessRule.ValidationStatus == true)
                {
                    orderDataService.Order_Ack_Decline(OrderId, StatusId, DeclineRemark, ShippingMethod, TissueBankSendByDate, LastModifiedBy,out transaction);
                }
                else
                {
                    transaction.ReturnStatus = orderBusinessRule.ValidationStatus;
                    transaction.ReturnMessage = orderBusinessRule.ValidationMessage;
                    transaction.ValidationErrors = orderBusinessRule.ValidationErrors;
                }

            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                orderDataService.CloseSession();
            }
        }
    }
}
