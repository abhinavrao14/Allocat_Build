using Allocat.ApplicationService;
using Allocat.DataModel;
using Allocat.DataService;
using Allocat.DataServiceInterface;
using Allocat.WebApi.WebApiModel;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Allocat.WebApi.Controllers
{
    public class RFQController : ApiController
    {
        IRFQDataService rfqDataService;

        public RFQController()
        {
            rfqDataService = new RFQDataService();
        }

        public HttpResponseMessage Get([FromUri] RFQ_TissueBank_DTO rfq_TissueBank_DTO)
        {
            if (rfq_TissueBank_DTO.SearchBy == null) rfq_TissueBank_DTO.SearchBy = string.Empty;
            if (rfq_TissueBank_DTO.SortDirection == null) rfq_TissueBank_DTO.SortDirection = string.Empty;
            if (rfq_TissueBank_DTO.SortExpression == null) rfq_TissueBank_DTO.SortExpression = string.Empty;

            RFQ_TissueBankApiModel rfq_TissueBankApiModel = new RFQ_TissueBankApiModel();
            TransactionalInformation transaction = new TransactionalInformation();

            if (rfq_TissueBank_DTO.SortDirection == "") rfq_TissueBank_DTO.SortDirection = "ASC";
            if (rfq_TissueBank_DTO.SortExpression == "") rfq_TissueBank_DTO.SortExpression = "ProductMasterName";

            RFQBusinessService rfqBusinessService = new RFQBusinessService(rfqDataService);

            IEnumerable<sp_RequestForQuote_TissueBank_GetByTissueBankId_Result> RequestForQuotes = rfqBusinessService.GetRfqsByTissueBankId
                (rfq_TissueBank_DTO.TissueBankId, rfq_TissueBank_DTO.SearchBy, rfq_TissueBank_DTO.CurrentPage, rfq_TissueBank_DTO.PageSize, rfq_TissueBank_DTO.SortDirection, rfq_TissueBank_DTO.SortExpression, out transaction);

            rfq_TissueBankApiModel.RequestForQuotes = RequestForQuotes;
            rfq_TissueBankApiModel.ReturnStatus = transaction.ReturnStatus;
            rfq_TissueBankApiModel.ReturnMessage = transaction.ReturnMessage;
            rfq_TissueBankApiModel.IsAuthenicated = true;

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<RFQ_TissueBankApiModel>(HttpStatusCode.OK, rfq_TissueBankApiModel);
                return response;
            }

            var badResponse = Request.CreateResponse<RFQ_TissueBankApiModel>(HttpStatusCode.BadRequest, rfq_TissueBankApiModel);
            return badResponse;
        }


        public HttpResponseMessage Get([FromUri] int RequestForQuoteId)
        {
            RFQ_TissueBankApiModel rfq_TissueBankApiModel = new RFQ_TissueBankApiModel();
            TransactionalInformation transaction = new TransactionalInformation();

            RFQBusinessService rfqBusinessService = new RFQBusinessService(rfqDataService);

            IEnumerable<sp_RequestForQuoteDetail_TissueBank_GetByRequestForQuoteId_Result> RequestForQuoteDetail = rfqBusinessService.GetRfqDetailByRequestForQuoteId
                (RequestForQuoteId, out transaction);

            rfq_TissueBankApiModel.RequestForQuoteDetail = RequestForQuoteDetail;
            rfq_TissueBankApiModel.ReturnStatus = transaction.ReturnStatus;
            rfq_TissueBankApiModel.ReturnMessage = transaction.ReturnMessage;
            rfq_TissueBankApiModel.IsAuthenicated = true;

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<RFQ_TissueBankApiModel>(HttpStatusCode.OK, rfq_TissueBankApiModel);
                return response;
            }

            var badResponse = Request.CreateResponse<RFQ_TissueBankApiModel>(HttpStatusCode.BadRequest, rfq_TissueBankApiModel);
            return badResponse;
        }

        [HttpPost]
        public HttpResponseMessage Post(RFQ_TissueBank_Edit_DTO rfq_TissueBank_Edit_DTO)
        {
            RFQ_TissueBankApiModel rfq_TissueBankApiModel = new RFQ_TissueBankApiModel();
            TransactionalInformation transaction = new TransactionalInformation();

            RFQBusinessService rfqBusinessService = new RFQBusinessService(rfqDataService);

            rfqBusinessService.RequestForQuote_Edit
               (rfq_TissueBank_Edit_DTO.TissueBankId, rfq_TissueBank_Edit_DTO.ResponseBody, rfq_TissueBank_Edit_DTO.AttachmentName, rfq_TissueBank_Edit_DTO.CreatedBy, rfq_TissueBank_Edit_DTO.LastModifiedBy, rfq_TissueBank_Edit_DTO.RequestForQuoteId, rfq_TissueBank_Edit_DTO.StatusId, rfq_TissueBank_Edit_DTO.DeclineRemark, rfq_TissueBank_Edit_DTO.Quantity, rfq_TissueBank_Edit_DTO.UnitPrice, rfq_TissueBank_Edit_DTO.LineTotal, rfq_TissueBank_Edit_DTO.SalesTax, rfq_TissueBank_Edit_DTO.Total, rfq_TissueBank_Edit_DTO.TissueBankSendByDate, rfq_TissueBank_Edit_DTO.ShippingMethod, out transaction);

            rfq_TissueBankApiModel.ReturnMessage = transaction.ReturnMessage;
            rfq_TissueBankApiModel.ReturnStatus = transaction.ReturnStatus;

            if (transaction.ReturnStatus == false)
            {
                rfq_TissueBankApiModel.ValidationErrors = transaction.ValidationErrors;
                return Request.CreateResponse<RFQ_TissueBankApiModel>(HttpStatusCode.BadRequest, rfq_TissueBankApiModel);
            }
            else
            {
                return  Request.CreateResponse<RFQ_TissueBankApiModel>(HttpStatusCode.OK, rfq_TissueBankApiModel);
            }
        }
    }
}
