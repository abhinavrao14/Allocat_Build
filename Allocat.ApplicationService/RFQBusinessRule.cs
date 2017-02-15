using Allocat.DataModel;
using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allocat.ApplicationService
{
    public class RFQBusinessRule : ValidationRules
    {
        IRFQDataService rFQDataService;

        public RFQBusinessRule(IRFQDataService _rFQDataService)
        {
            rFQDataService = _rFQDataService;
        }


        public void ValidateResponse(int TissueBankId, string ResponseBody, string AttachmentName, int CreatedBy, int LastModifiedBy, int RequestForQuoteId, int StatusId, string DeclineRemark, int Quantity, decimal UnitPrice, decimal LineTotal, decimal SalesTax, decimal Total, DateTime? TissueBankSendByDate, string ShippingMethod)
        {

        }

        public void ValidRFQRequest(int RequestForQuoteId, int InfoId, string InfoType)
        {
            if (ValidateRequired(RequestForQuoteId, "Request For Quote Id"))
            {
                //Regular Expression Validations
                if (ValidateNumeric(RequestForQuoteId, "Request For Quote Id"))
                {
                    if (InfoType == "TISSUEBANK")
                    {
                        ValidRFQRequest(RequestForQuoteId, InfoId);
                    }
                }
            }
        }

        private void ValidRFQRequest(int RequestForQuoteId, int TissueBankId)
        {
            Boolean valid = rFQDataService.ValidRFQRequest(RequestForQuoteId, TissueBankId);
            if (valid == false)
            {
                AddValidationError("RequestForQuoteId", "Access Denied");
            }
        }
    }
}
