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
    public class ProductBusinessRule : ValidationRules
    {
        //IProductDataService productDataService;
        public ProductBusinessRule()
        {
            InitializeValidationRules();
        }
        public void ValidateProductsDataTable(DataTable tempTissueBankProduct_TissueBank)
        {
            foreach (DataRow dr in tempTissueBankProduct_TissueBank.AsEnumerable())
            {
                //Required Validations
                ValidateRequired(dr["TissueBankProductId"], "Tissue Bank Product Id");
                ValidateRequired(dr["TissueBankId"], "Tissue Bank ID");
                ValidateRequired(dr["ProductMasterId"], "Product Master Id");
                ValidateRequired(dr["ProductType"], "Product Type");
                ValidateRequired(dr["ProductCode"], "Product Code");
                ValidateRequired(dr["ProductSize"], "Product Size");
                ValidateRequired(dr["PreservationType"], "Preservation Type");
                ValidateRequired(dr["IsAvailableForSale"], "Is Available For Sale : Status");
                ValidateRequired(dr["SourceId"], "Source-Id");
                ValidateRequired(dr["LastModifiedBy"], "Last Modified By");

                //Regular Expression Validations

                ValidateNumeric(dr["LastModifiedBy"], "Last Modified By");
                ValidateNumeric(dr["SourceId"], "Source-Id");
                //ValidateIsDateOrNullOrEmptyDateForTable(dr["AvailabilityStartDate"], "Availability Start Date");
                //ValidateIsDateOrNullOrEmptyDateForTable(dr["AvailabilityEndDate"], "Availability End Date");

                if (ValidateNumeric(dr["TissueBankProductId"], "Tissue Bank Product Id"))
                {
                    if (Convert.ToInt16(dr["TissueBankProductId"]) > 0)
                    {
                        ValidateRequired(dr["CreatedBy"], "Created By");

                        ValidateNumeric(dr["CreatedBy"], "Created By");
                    }
                }
            }
        }
    }
}
