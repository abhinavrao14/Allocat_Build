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

        public void ValidateTemp_RequestForQuote_Hospital_Create(DataTable temp_RequestForQuote_Hospital_Create)
        {
            foreach (DataRow dr in temp_RequestForQuote_Hospital_Create.AsEnumerable())
            {
                //Required Validations
                ValidateRequired(dr["TissueBankProductId"], "Tissue Bank Product Id");
                ValidateRequired(dr["TissueBankId"], "Tissue Bank ID");
                ValidateRequired(dr["HospitalId"], "Hospital ID");
                ValidateRequired(dr["Quantity"], "Quantity");
                ValidateRequired(dr["UnitPrice"], "Unit Price");
                ValidateRequired(dr["LineTotal"], "Line Total");
                ValidateRequired(dr["SalesTax"], "Sales Tax");
                ValidateRequired(dr["Total"], "Total");
                ValidateRequired(dr["StatusId"], "Status Id");
                ValidateRequired(dr["RequestBody"], "Request Body");
                ValidateRequired(dr["CreatedDate"], "Created Date");
                ValidateRequired(dr["CreatedBy"], "Created By");
                ValidateRequired(dr["LastModifiedDate"], "Last Modified Date");
                ValidateRequired(dr["LastModifiedBy"], "Last Modified By");

                //Regular Expression Validations

                ValidateNumeric(dr["TissueBankProductId"], "Tissue Bank Product Id");
                ValidateNumeric(dr["TissueBankId"], "TissueBank Id");
                ValidateNumeric(dr["HospitalId"], "Hospital Id");
                ValidateNumeric(dr["StatusId"], "Status Id");
                ValidateNumeric(dr["CreatedBy"], "Created By");
                ValidateNumeric(dr["LastModifiedBy"], "Last Modified By");

                ValidateIsDate(dr["CreatedDate"], "Created Date");
                ValidateIsDate(dr["LastModifiedDate"], "Last Modified Date");
                ValidateIsDateOrNullOrEmptyDate(dr["NeedByDate"], "Need By Date");
            }
        }
    }
}
