using Allocat.ApplicationService;
using Allocat.DataModel;
using Allocat.DataService;
using Allocat.DataServiceInterface;
using Allocat.WebApi.WebApiModel;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Allocat.WebApi.Controllers
{
    public class OrderController : ApiController
    {
        IOrderDataService orderDataService;

        public OrderController()
        {
            orderDataService = new OrderDataService();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="order_TissueBank_DTO"></param>
        /// <returns></returns>
        public HttpResponseMessage Get([FromUri] Order_TissueBank_DTO order_TissueBank_DTO)
        {
            if (order_TissueBank_DTO.SearchBy == null) order_TissueBank_DTO.SearchBy = string.Empty;
            if (order_TissueBank_DTO.SortDirection == null) order_TissueBank_DTO.SortDirection = string.Empty;
            if (order_TissueBank_DTO.SortExpression == null) order_TissueBank_DTO.SortExpression = string.Empty;

            Order_TissueBankApiModel order_TissueBankApiModel = new Order_TissueBankApiModel();
            TransactionalInformation transaction = new TransactionalInformation();

            if (order_TissueBank_DTO.SortDirection == "") order_TissueBank_DTO.SortDirection = "ASC";
            if (order_TissueBank_DTO.SortExpression == "") order_TissueBank_DTO.SortExpression = "ProductMasterName";

            OrderBusinessService orderBusinessService = new OrderBusinessService(orderDataService);

            IEnumerable<sp_Order_TissueBank_GetByTissueBankId_Result> Orders = orderBusinessService.GetOrderByTissueBankId
                (order_TissueBank_DTO.TissueBankId, order_TissueBank_DTO.SearchBy, order_TissueBank_DTO.CurrentPage, order_TissueBank_DTO.PageSize, order_TissueBank_DTO.SortDirection, order_TissueBank_DTO.SortExpression, out transaction);

            order_TissueBankApiModel.Orders = Orders;
            order_TissueBankApiModel.ReturnStatus = transaction.ReturnStatus;
            order_TissueBankApiModel.ReturnMessage = transaction.ReturnMessage;
            order_TissueBankApiModel.IsAuthenicated = true;

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<Order_TissueBankApiModel>(HttpStatusCode.OK, order_TissueBankApiModel);
                return response;
            }

            var badResponse = Request.CreateResponse<Order_TissueBankApiModel>(HttpStatusCode.BadRequest, order_TissueBankApiModel);
            return badResponse;
        }


        public HttpResponseMessage Get([FromUri] int OrderId)
        {
            Order_TissueBankApiModel order_TissueBank_DTO = new Order_TissueBankApiModel();
            TransactionalInformation transaction = new TransactionalInformation();

            OrderBusinessService orderBusinessService = new OrderBusinessService(orderDataService);

            IEnumerable<sp_OrderDetail_TissueBank_GetByOrderId_Result> OrderDetail = orderBusinessService.GetOrderDetailByOrderId
                (OrderId, out transaction);

            order_TissueBank_DTO.OrderDetail = OrderDetail;
            order_TissueBank_DTO.ReturnStatus = transaction.ReturnStatus;
            order_TissueBank_DTO.ReturnMessage = transaction.ReturnMessage;
            order_TissueBank_DTO.IsAuthenicated = true;

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<Order_TissueBankApiModel>(HttpStatusCode.OK, order_TissueBank_DTO);
                return response;
            }

            var badResponse = Request.CreateResponse<Order_TissueBankApiModel>(HttpStatusCode.BadRequest, order_TissueBank_DTO);
            return badResponse;
        }

        [HttpPost]
        public HttpResponseMessage Post(Order_Ack_Decline_DTO order_Ack_Decline_DTO)
        {
            Order_TissueBankApiModel order_TissueBankApiModel = new Order_TissueBankApiModel();
            TransactionalInformation transaction = new TransactionalInformation();

            OrderBusinessService orderBusinessService = new OrderBusinessService(orderDataService);

            orderBusinessService.Order_Ack_Decline
               (order_Ack_Decline_DTO.OrderId, order_Ack_Decline_DTO.StatusId, order_Ack_Decline_DTO.DeclineRemark, order_Ack_Decline_DTO.ShippingMethod, order_Ack_Decline_DTO.TissueBankSendByDate, order_Ack_Decline_DTO.LastModifiedBy, out transaction);

            order_TissueBankApiModel.ReturnMessage = transaction.ReturnMessage;
            order_TissueBankApiModel.ReturnStatus = transaction.ReturnStatus;

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
    }
}
