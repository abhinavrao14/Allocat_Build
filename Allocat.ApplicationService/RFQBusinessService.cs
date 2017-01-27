using Allocat.DataModel;
using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;


namespace Allocat.ApplicationService
{
    public class RFQBusinessService
    {
        private IRFQDataService rfqDataService;

        public RFQBusinessService(IRFQDataService _rfqDataService)
        {
            rfqDataService = _rfqDataService;
        }

        public IEnumerable<sp_RequestForQuote_TissueBank_GetByTissueBankId_Result> GetRfqsByTissueBankId(int TissueBankId, string SearchBy, int CurrentPage, int PageSize, string SortDirection, string SortExpression, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            IEnumerable<sp_RequestForQuote_TissueBank_GetByTissueBankId_Result> lstRfq = null;

            try
            {
                rfqDataService.CreateSession();
                lstRfq = rfqDataService.GetRfqsByTissueBankId(TissueBankId, SearchBy, CurrentPage, PageSize, SortDirection, SortExpression, out transaction);
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
                rfqDataService.CloseSession();
            }

            return lstRfq;

        }


        public IEnumerable<sp_RequestResponse_TissueBank_GetByTissueBankId_Result> GetRequestResponseByRequestForQuoteId(int RequestForQuoteId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            IEnumerable<sp_RequestResponse_TissueBank_GetByTissueBankId_Result> lstRequestResponse = null;

            try
            {
                rfqDataService.CreateSession();
                lstRequestResponse = rfqDataService.GetRequestResponseByRequestForQuoteId(RequestForQuoteId, out transaction);
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
                rfqDataService.CloseSession();
            }

            return lstRequestResponse;

        }


        public IEnumerable<sp_RequestForQuoteDetail_TissueBank_GetByRequestForQuoteId_Result> GetRfqDetailByRequestForQuoteId(int RequestForQuoteId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            IEnumerable<sp_RequestForQuoteDetail_TissueBank_GetByRequestForQuoteId_Result> RequestForQuoteDetail = null;

            try
            {
                rfqDataService.CreateSession();
                RequestForQuoteDetail = rfqDataService.GetRfqDetailByRequestForQuoteId(RequestForQuoteId, out transaction);
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
                rfqDataService.CloseSession();
            }

            return RequestForQuoteDetail;

        }

        public void RequestForQuote_Edit(int TissueBankId, string ResponseBody, string AttachmentName, int CreatedBy, int LastModifiedBy, int RequestForQuoteId, int StatusId, string DeclineRemark, int Quantity, decimal UnitPrice, decimal LineTotal, decimal SalesTax, decimal Total, DateTime? TissueBankSendByDate, string ShippingMethod, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            RFQBusinessRule rFQBusinessRule = new RFQBusinessRule();

            try
            {
                rfqDataService.CreateSession();

                // rFQBusinessRule.ValidateResponse(TissueBankId, ResponseBody, AttachmentName, CreatedBy, LastModifiedBy, RequestForQuoteId, StatusId, DeclineRemark, Quantity, UnitPrice, LineTotal, SalesTax, Total, TissueBankSendByDate, ShippingMethod);

                //  if (rFQBusinessRule.ValidationStatus == true)
                // {
                rfqDataService.RequestForQuote_Edit(TissueBankId, ResponseBody, AttachmentName, CreatedBy, LastModifiedBy, RequestForQuoteId, StatusId, DeclineRemark, Quantity, UnitPrice, LineTotal, SalesTax, Total, TissueBankSendByDate, ShippingMethod, out transaction);

                //}
                //else
                //{
                //    transaction.ReturnStatus = productBusinessRule.ValidationStatus;
                //    transaction.ReturnMessage = productBusinessRule.ValidationMessage;
                //    transaction.ValidationErrors = productBusinessRule.ValidationErrors;
                //}

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
                rfqDataService.CloseSession();
            }
        }
    }
}
