using Allocat.ApplicationService;
using Allocat.DataModel;
using Allocat.DataService;
using Allocat.DataServiceInterface;
using Allocat.Utility;
using Allocat.WebApi.WebApiModel;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace Allocat.WebApi.Controllers
{
    public class RFQHospitalController : ApiController
    {
        IRFQDataService rfqDataService;

        public RFQHospitalController()
        {
            rfqDataService = new RFQDataService();
        }

        //public HttpResponseMessage Get([FromUri] RFQ_TissueBank_DTO rfq_TissueBank_DTO)
        //{
        //    RFQ_TissueBankApiModel rfq_TissueBankApiModel = new RFQ_TissueBankApiModel();
        //    TransactionalInformation transaction = new TransactionalInformation();
        //    RFQBusinessService rfqBusinessService = new RFQBusinessService(rfqDataService);

        //    if (rfq_TissueBank_DTO.OperationType == "GetAll")
        //    {
        //        if (rfq_TissueBank_DTO.SearchBy == null) rfq_TissueBank_DTO.SearchBy = string.Empty;
        //        if (rfq_TissueBank_DTO.SortDirection == null) rfq_TissueBank_DTO.SortDirection = string.Empty;
        //        if (rfq_TissueBank_DTO.SortExpression == null) rfq_TissueBank_DTO.SortExpression = string.Empty;

        //        if (rfq_TissueBank_DTO.SortDirection == "") rfq_TissueBank_DTO.SortDirection = "DESC";
        //        if (rfq_TissueBank_DTO.SortExpression == "") rfq_TissueBank_DTO.SortExpression = "RequestForQuoteId";


        //        IEnumerable<sp_RequestForQuote_TissueBank_GetByTissueBankId_Result> RequestForQuotes = rfqBusinessService.GetRfqsByTissueBankId
        //            (rfq_TissueBank_DTO.TissueBankId, rfq_TissueBank_DTO.SearchBy, rfq_TissueBank_DTO.CurrentPage, rfq_TissueBank_DTO.PageSize, rfq_TissueBank_DTO.SortDirection, rfq_TissueBank_DTO.SortExpression, out transaction);

        //        rfq_TissueBankApiModel.RequestForQuotes = RequestForQuotes;
        //        rfq_TissueBankApiModel.ReturnStatus = transaction.ReturnStatus;
        //        rfq_TissueBankApiModel.ReturnMessage = transaction.ReturnMessage;
        //        rfq_TissueBankApiModel.IsAuthenicated = true;
        //    }
        //    else if (rfq_TissueBank_DTO.OperationType == "GetById")
        //    {
        //        IEnumerable<sp_RequestForQuoteDetail_TissueBank_GetByRequestForQuoteId_Result> RequestForQuoteDetail = rfqBusinessService.GetRfqDetailByRequestForQuoteId
        //       (rfq_TissueBank_DTO.RequestForQuoteId, rfq_TissueBank_DTO.TissueBankId, rfq_TissueBank_DTO.InfoType, out transaction);

        //        rfq_TissueBankApiModel.RequestForQuoteDetail = RequestForQuoteDetail;
        //        rfq_TissueBankApiModel.ReturnStatus = transaction.ReturnStatus;
        //        rfq_TissueBankApiModel.ReturnMessage = transaction.ReturnMessage;
        //        rfq_TissueBankApiModel.IsAuthenicated = true;
        //    }

        //    if (transaction.ReturnStatus == true)
        //    {
        //        var response = Request.CreateResponse<RFQ_TissueBankApiModel>(HttpStatusCode.OK, rfq_TissueBankApiModel);
        //        return response;
        //    }

        //    var badResponse = Request.CreateResponse<RFQ_TissueBankApiModel>(HttpStatusCode.BadRequest, rfq_TissueBankApiModel);
        //    return badResponse;
        //}


        [HttpPost]
        public HttpResponseMessage Post(IEnumerable<RFQ_Hospital_Create_DTO> rFQ_Hospital_Create_DTO)
        {
            RFQ_HospitalApiModel rFQ_HospitalApiModel = new RFQ_HospitalApiModel();
            TransactionalInformation transaction = new TransactionalInformation();

            RFQBusinessService rfqBusinessService = new RFQBusinessService(rfqDataService);

            DataTable temp_RequestForQuote_Hospital_Create = Utilities.ToDataTable<RFQ_Hospital_Create_DTO>(rFQ_Hospital_Create_DTO);

            rfqBusinessService.RequestForQuote_Hospital_Create
               (temp_RequestForQuote_Hospital_Create, out transaction);

            rFQ_HospitalApiModel.ReturnMessage = transaction.ReturnMessage;
            rFQ_HospitalApiModel.ReturnStatus = transaction.ReturnStatus;

            if (transaction.ReturnStatus == false)
            {
                rFQ_HospitalApiModel.ValidationErrors = transaction.ValidationErrors;
                return Request.CreateResponse<RFQ_HospitalApiModel>(HttpStatusCode.BadRequest, rFQ_HospitalApiModel);
            }
            else
            {
                return Request.CreateResponse<RFQ_HospitalApiModel>(HttpStatusCode.OK, rFQ_HospitalApiModel);
            }
        }
    }
}
