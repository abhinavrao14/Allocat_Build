using Allocat.DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allocat.DataServiceInterface
{
    public interface IRFQDataService : IDataService,IDisposable
    {
        IEnumerable<sp_RequestForQuote_TissueBank_GetByTissueBankId_Result> GetRfqsByTissueBankId(int TissueBankId, string SearchBy, int CurrentPage, int PageSize, string SortDirection, string SortExpression, out TransactionalInformation transaction);
        IEnumerable<sp_RequestForQuoteDetail_TissueBank_GetByRequestForQuoteId_Result> GetRfqDetailByRequestForQuoteId(int RequestForQuoteId, out TransactionalInformation transaction);
        IEnumerable<sp_RequestResponse_TissueBank_GetByTissueBankId_Result> GetRequestResponseByRequestForQuoteId(int RequestForQuoteId, out TransactionalInformation transaction);
        int RequestForQuote_Edit(int TissueBankId, string ResponseBody, string AttachmentName, int CreatedBy, int LastModifiedBy, int RequestForQuoteId, int StatusId, string DeclineRemark, int Quantity, decimal UnitPrice, decimal LineTotal, decimal SalesTax, decimal Total, DateTime? TissueBankSendByDate, string ShippingMethod, out TransactionalInformation transaction);
    }
}
