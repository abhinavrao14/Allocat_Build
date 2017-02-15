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
    public class RFQDataService : EntityFrameworkDataService, IRFQDataService
    {
        public IEnumerable<sp_RequestResponse_TissueBank_GetByTissueBankId_Result> GetRequestResponseByRequestForQuoteId(int RequestForQuoteId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            IEnumerable<sp_RequestResponse_TissueBank_GetByTissueBankId_Result> lstRequestResponse = dbConnection.sp_RequestResponse_TissueBank_GetByTissueBankId(RequestForQuoteId);

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add("Request And Responses found.");

            return lstRequestResponse;
        }

        public IEnumerable<sp_RequestForQuoteDetail_TissueBank_GetByRequestForQuoteId_Result> GetRfqDetailByRequestForQuoteId(int RequestForQuoteId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            IEnumerable<sp_RequestForQuoteDetail_TissueBank_GetByRequestForQuoteId_Result> RequestForQuoteDetail = dbConnection.sp_RequestForQuoteDetail_TissueBank_GetByRequestForQuoteId(RequestForQuoteId);

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add("Request Detail found.");

            return RequestForQuoteDetail;
        }

        public IEnumerable<sp_RequestForQuote_TissueBank_GetByTissueBankId_Result> GetRfqsByTissueBankId(int TissueBankId, string SearchBy, int CurrentPage, int PageSize, string SortDirection, string SortExpression, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            IEnumerable<sp_RequestForQuote_TissueBank_GetByTissueBankId_Result> lstRfq = dbConnection.sp_RequestForQuote_TissueBank_GetByTissueBankId(TissueBankId, SearchBy, CurrentPage, PageSize, SortDirection, SortExpression);

            int numberOfRows = (from rfq in dbConnection.RequestForQuote
                                join pe in dbConnection.ProductEntity on rfq.ProductEntityId equals pe.ProductEntityId
                                join s in dbConnection.Status on rfq.StatusId equals s.StatusId
                                join tbp in dbConnection.TissueBankProduct on pe.TissueBankProductId equals tbp.TissueBankProductId
                                join tbpm in dbConnection.TissueBankProductMaster on tbp.TissueBankProductMasterId equals tbpm.TissueBankProductMasterId
                                join tb in dbConnection.TissueBank on tbpm.TissueBankId equals tb.TissueBankId
                                join pm in dbConnection.ProductMaster on tbpm.ProductMasterId equals pm.ProductMasterId
                                where tbpm.TissueBankId == TissueBankId && (rfq.RequestForQuoteId.ToString().Contains(SearchBy)
                                || rfq.LastModifiedDate.ToString().Contains(SearchBy)
                                || tbp.ProductCode.ToString().Contains(SearchBy)
                                || pm.ProductMasterName.ToString().Contains(SearchBy)
                                || rfq.LastModifiedDate.ToString().Contains(SearchBy)
                                || rfq.CreatedDate.ToString().Contains(SearchBy)
                                )
                                select rfq).Count();

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(numberOfRows.ToString()+ " Request For Quote found.");

            return lstRfq;
        }

        public void RequestForQuote_Edit(int TissueBankId, string ResponseBody, string AttachmentName, int CreatedBy, int LastModifiedBy, int RequestForQuoteId, int StatusId, string DeclineRemark, int Quantity, decimal UnitPrice, decimal LineTotal, decimal SalesTax, decimal Total, DateTime? TissueBankSendByDate, string ShippingMethod, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            IEnumerable<string> IEmessage = dbConnection.sp_RequestForQuote_TissueBank_Edit( TissueBankId, ResponseBody, AttachmentName,  CreatedBy,  LastModifiedBy,  RequestForQuoteId,  StatusId, DeclineRemark,  Quantity,  UnitPrice,  LineTotal,  SalesTax,  Total,  TissueBankSendByDate, ShippingMethod);

            string message = IEmessage.FirstOrDefault();

            if (message.Contains("Error"))
            {
                transaction.ReturnStatus = false;
            }
            else
            {
                transaction.ReturnStatus = true;
            }
            transaction.ReturnMessage.Add(message);

            //transaction = new TransactionalInformation();
            //int rowAffected = 0;
            //var parameterTissueBankId = new SqlParameter("@TissueBankId", SqlDbType.Int);
            //parameterTissueBankId.Value = TissueBankId;
            //var parameterResponseBody = new SqlParameter("@ResponseBody", SqlDbType.NVarChar);
            //if(ResponseBody!=null)
            //    parameterResponseBody.Value = ResponseBody;
            //else
            //    parameterResponseBody.Value = DBNull.Value;

            //var parameterAttachmentName = new SqlParameter("@AttachmentName", SqlDbType.NVarChar);
            //if (AttachmentName != null)
            //    parameterAttachmentName.Value = AttachmentName;
            //else if (AttachmentName == "")
            //    parameterAttachmentName.Value = DBNull.Value;
            //else
            //    parameterAttachmentName.Value = DBNull.Value;

            //var parameterCreatedBy = new SqlParameter("@CreatedBy", SqlDbType.Int);
            //parameterCreatedBy.Value = CreatedBy;

            //var parameterLastModifiedBy = new SqlParameter("@LastModifiedBy", SqlDbType.Int);
            //parameterLastModifiedBy.Value = LastModifiedBy;

            //var parameterRequestForQuoteId = new SqlParameter("@RequestForQuoteId", SqlDbType.Int);
            //parameterRequestForQuoteId.Value = RequestForQuoteId;

            //var parameterStatusId = new SqlParameter("@StatusId", SqlDbType.Int);
            //parameterStatusId.Value = StatusId;

            //var parameterDeclineRemark = new SqlParameter("@DeclineRemark", SqlDbType.NVarChar);
            //if (DeclineRemark != null)
            //    parameterDeclineRemark.Value = DeclineRemark;
            //else
            //    parameterDeclineRemark.Value = DBNull.Value;
            //var parameterQuantity = new SqlParameter("@Quantity", SqlDbType.Int);
            //parameterQuantity.Value = Quantity;

            //var parameterUnitPrice = new SqlParameter("@UnitPrice", SqlDbType.Decimal);
            //parameterUnitPrice.Value = @UnitPrice;

            //var parameterLineTotal = new SqlParameter("@LineTotal", SqlDbType.Decimal);
            //parameterLineTotal.Value = LineTotal;

            //var parameterSalesTax = new SqlParameter("@SalesTax", SqlDbType.Decimal);
            //parameterSalesTax.Value = SalesTax;

            //var parameterTotal = new SqlParameter("@Total", SqlDbType.Decimal);
            //parameterTotal.Value = Total;

            //var parameterTissueBankSendByDate = new SqlParameter("@TissueBankSendByDate", SqlDbType.DateTime);
            //if (TissueBankSendByDate != null)
            //    parameterTissueBankSendByDate.Value = TissueBankSendByDate;
            //else
            //    parameterTissueBankSendByDate.Value = DBNull.Value;

            //var parameterShippingMethod = new SqlParameter("@ShippingMethod", SqlDbType.NVarChar);
            //if (ShippingMethod != null)
            //    parameterShippingMethod.Value = ShippingMethod;
            //else
            //    parameterShippingMethod.Value = DBNull.Value;

            //rowAffected = dbConnection.Database.ExecuteSqlCommand("exec dbo.sp_RequestForQuote_TissueBank_Edit @TissueBankId,@ResponseBody,@AttachmentName,@CreatedBy,@LastModifiedBy,@RequestForQuoteId,@StatusId,@DeclineRemark,@Quantity,@UnitPrice,@LineTotal,@SalesTax,@Total,@TissueBankSendByDate,@ShippingMethod", parameterTissueBankId, parameterResponseBody, parameterAttachmentName, parameterCreatedBy, parameterLastModifiedBy, parameterRequestForQuoteId, parameterStatusId, parameterDeclineRemark, parameterQuantity, parameterUnitPrice, parameterLineTotal, parameterSalesTax, parameterTotal, parameterTissueBankSendByDate, parameterShippingMethod);
 
            //if (rowAffected > 0)
            //{
            //    transaction.ReturnStatus = true;
            //    transaction.ReturnMessage.Add("Request for Quote is updated successfully.");
            //}
            //else
            //{
            //    transaction.ReturnStatus = false;
            //    transaction.ReturnMessage.Add("Database Error");
            //}

            //return rowAffected;
        }

        public bool ValidRFQRequest(int RFQId, int TissueBankId)
        {
            int count = (from rfq in dbConnection.RequestForQuote
                         join pe in dbConnection.ProductEntity on rfq.ProductEntityId equals pe.ProductEntityId
                         join tb in dbConnection.TissueBank on pe.TissueBankId equals tb.TissueBankId
                         where rfq.RequestForQuoteId == RFQId && pe.TissueBankId == TissueBankId
                         select rfq).Count();

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
