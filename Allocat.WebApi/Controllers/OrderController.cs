using Allocat.ApplicationService;
using Allocat.DataModel;
using Allocat.DataService;
using Allocat.DataServiceInterface;
using Allocat.WebApi.CustomService;
using Allocat.WebApi.WebApiModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Allocat.WebApi.Controllers
{
    public class OrderController : ApiController
    {
        IOrderDataService orderDataService;
        IErrorDataService errorDataService;
        IStatusDataService statusDataService;
        ITransactionDataService transactionDataService;

        public OrderController()
        {
            orderDataService = new OrderDataService();
            errorDataService = new ErrorDataService();
            transactionDataService = new TransactionDataService();
            statusDataService = new StatusDataService();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="order_TissueBank_DTO"></param>
        /// <returns></returns>
        public HttpResponseMessage Get([FromUri] Order_TissueBank_DTO order_TissueBank_DTO)
        {
            Order_TissueBankApiModel order_TissueBankApiModel = new Order_TissueBankApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            OrderBusinessService orderBusinessService = new OrderBusinessService(orderDataService);

            if (order_TissueBank_DTO.OperationType == "GetAll")
            {
                if (order_TissueBank_DTO.SearchBy == null) order_TissueBank_DTO.SearchBy = string.Empty;
                if (order_TissueBank_DTO.SortDirection == null) order_TissueBank_DTO.SortDirection = string.Empty;
                if (order_TissueBank_DTO.SortExpression == null) order_TissueBank_DTO.SortExpression = string.Empty;

                if (order_TissueBank_DTO.SortDirection == "") order_TissueBank_DTO.SortDirection = "ASC";
                if (order_TissueBank_DTO.SortExpression == "") order_TissueBank_DTO.SortExpression = "ProductMasterName";

                IEnumerable<sp_Order_TissueBank_GetByTissueBankId_Result> Orders = orderBusinessService.GetOrderByTissueBankId
                    (order_TissueBank_DTO.TissueBankId, order_TissueBank_DTO.SearchBy, order_TissueBank_DTO.CurrentPage, order_TissueBank_DTO.PageSize, order_TissueBank_DTO.SortDirection, order_TissueBank_DTO.SortExpression, out transaction);

                order_TissueBankApiModel.Orders = Orders;
                order_TissueBankApiModel.ReturnStatus = transaction.ReturnStatus;
                order_TissueBankApiModel.ReturnMessage = transaction.ReturnMessage;
                order_TissueBankApiModel.IsAuthenicated = true;
            }
            else
            {
                IEnumerable<sp_OrderDetail_TissueBank_GetByOrderId_Result> OrderDetail = orderBusinessService.GetOrderDetailByOrderId
                (order_TissueBank_DTO.OrderId, order_TissueBank_DTO.TissueBankId, order_TissueBank_DTO.InfoType, out transaction);

                order_TissueBankApiModel.OrderDetail = OrderDetail;
                order_TissueBankApiModel.ReturnStatus = transaction.ReturnStatus;
                order_TissueBankApiModel.ReturnMessage = transaction.ReturnMessage;
                order_TissueBankApiModel.IsAuthenicated = true;
            }

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<Order_TissueBankApiModel>(HttpStatusCode.OK, order_TissueBankApiModel);
                return response;
            }

            var badResponse = Request.CreateResponse<Order_TissueBankApiModel>(HttpStatusCode.BadRequest, order_TissueBankApiModel);
            return badResponse;
        }

        //public HttpResponseMessage Get([FromUri] int OrderId)
        //{
        //    Order_TissueBankApiModel order_TissueBank_DTO = new Order_TissueBankApiModel();
        //    TransactionalInformation transaction = new TransactionalInformation();

        //    OrderBusinessService orderBusinessService = new OrderBusinessService(orderDataService);

        //    IEnumerable<sp_OrderDetail_TissueBank_GetByOrderId_Result> OrderDetail = orderBusinessService.GetOrderDetailByOrderId
        //        (OrderId, out transaction);

        //    order_TissueBank_DTO.OrderDetail = OrderDetail;
        //    order_TissueBank_DTO.ReturnStatus = transaction.ReturnStatus;
        //    order_TissueBank_DTO.ReturnMessage = transaction.ReturnMessage;
        //    order_TissueBank_DTO.IsAuthenicated = true;

        //    if (transaction.ReturnStatus == true)
        //    {
        //        var response = Request.CreateResponse<Order_TissueBankApiModel>(HttpStatusCode.OK, order_TissueBank_DTO);
        //        return response;
        //    }

        //    var badResponse = Request.CreateResponse<Order_TissueBankApiModel>(HttpStatusCode.BadRequest, order_TissueBank_DTO);
        //    return badResponse;
        //}

        [HttpPost]
        public HttpResponseMessage Post(Order_Ack_Decline_DTO order_Ack_Decline_DTO)
        {
            Order_TissueBankApiModel order_TissueBankApiModel = new Order_TissueBankApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            TransactionBusinessService transactionBusinessService = new TransactionBusinessService(transactionDataService);
            Status status = new Status();

            ErrorBusinessService errorBusinessService = new ErrorBusinessService(errorDataService);
            StatusBusinessService statusBusinessService = new StatusBusinessService(statusDataService);
            OrderBusinessService orderBusinessService = new OrderBusinessService(orderDataService);

            //Get  OrderCommisionDetail
            OrderCommisionDetail_TissueBank orderCommisionDetail = orderBusinessService.GetOrderCommisionDetail(order_Ack_Decline_DTO.OrderId, out transaction);

            if (order_Ack_Decline_DTO.StatusId == 1008)
            {

                //static values
                orderCommisionDetail.CustomerProfileId = "1810434404";
                orderCommisionDetail.CustomerPaymentProfileIds = "1805183086";

                //Convert object to string to send as requestBody
                string objToPass = Utility.Utilities.SerializeObject<Order_Ack_Decline_DTO>(order_Ack_Decline_DTO);

                //create transaction
                int TransactionId = transactionBusinessService.Transaction_Create(DateTime.Now, (float)orderCommisionDetail.AlloCATFees, order_Ack_Decline_DTO.LastModifiedBy, objToPass, out transaction);

                //cut commision
                var response = ChargeCustomerProfile(orderCommisionDetail);

                // if response is not null then only save info in db.
                if (response.AuthCode != null)
                {
                    //converting response into xml format
                    string ResponseBody = Utility.Utilities.SerializeObject<ResTransaction>(response);

                    //get status from database for Success
                    status = statusBusinessService.GetStatusByStatusName("Success");

                    orderBusinessService.Order_Ack_Decline(order_Ack_Decline_DTO.OrderId, order_Ack_Decline_DTO.StatusId, order_Ack_Decline_DTO.DeclineRemark, order_Ack_Decline_DTO.ShippingMethod, order_Ack_Decline_DTO.TissueBankSendByDate, order_Ack_Decline_DTO.LastModifiedBy, TransactionId, response.AuthCode, ResponseBody, response.TransactionId, status.StatusId, orderCommisionDetail.TissueBankId, out transaction);

                    order_TissueBankApiModel.ReturnMessage = transaction.ReturnMessage;
                    order_TissueBankApiModel.ReturnStatus = transaction.ReturnStatus;
                }
                else
                {
                    //get status from database for Success
                    status = statusBusinessService.GetStatusByStatusName("Error");

                    if (response.ErrorCode == null)
                        response.MessageCode = "Error Code from authorize.net is null.";

                    if (response.MessageDescription == null)
                        response.MessageDescription = "Response from authorize.net is null.";

                    //if response is null then log error and update transaction too.
                    string errorMessage = errorBusinessService.Error_Create(status.StatusId, response.MessageDescription, "", TransactionId, order_Ack_Decline_DTO.LastModifiedBy, response.MessageCode);

                    order_TissueBankApiModel.ReturnStatus = transaction.ReturnStatus = false;
                    order_TissueBankApiModel.ReturnMessage.Add(response.MessageDescription);
                }
            }
            else
            {
                orderBusinessService.Order_Ack_Decline(order_Ack_Decline_DTO.OrderId, order_Ack_Decline_DTO.StatusId, order_Ack_Decline_DTO.DeclineRemark, order_Ack_Decline_DTO.ShippingMethod, order_Ack_Decline_DTO.TissueBankSendByDate, order_Ack_Decline_DTO.LastModifiedBy, 0, "", "", "", 0, orderCommisionDetail.TissueBankId, out transaction);
            }


            if (transaction.ReturnStatus == false)
            {
                order_TissueBankApiModel.ValidationErrors = transaction.ValidationErrors;
                return Request.CreateResponse<Order_TissueBankApiModel>(HttpStatusCode.BadRequest, order_TissueBankApiModel);
            }
            else
            {
                return Request.CreateResponse<Order_TissueBankApiModel>(HttpStatusCode.OK, order_TissueBankApiModel);
            }
        }

        private ResTransaction ChargeCustomerProfile(OrderCommisionDetail_TissueBank orderCommisionDetail)
        {
            CustomService.AllocatCustomServiceClient obj = new AllocatCustomServiceClient();
            ResTransaction resTransaction = new ResTransaction();
            resTransaction = obj.ChargeCustomerProfile(orderCommisionDetail.CustomerProfileId, orderCommisionDetail.CustomerPaymentProfileIds, (decimal)orderCommisionDetail.AlloCATFees);
            return resTransaction;
        }
    }
}
