using System;

namespace Allocat.ApplicationService
{
    public class OrderBusinessRule : ValidationRules
    {
        public OrderBusinessRule()
        {
            InitializeValidationRules();
        }

        public void ValidateOrder_Ack_Decline(int OrderId, int StatusId, string DeclineRemark, string ShippingMethod, DateTime TissueBankSendByDate, int LastModifiedBy)
        {
            //Required Validations
            ValidateRequired(OrderId, "Order-Id");
            if (ValidateRequired(StatusId, "Status-Id"))
            {
                if (StatusId == 5)
                {
                    ValidateRequired(DeclineRemark, "Decline Remark");
                }
                else
                {
                    ValidateRequired(ShippingMethod, "Shipping Method");
                    ValidateRequired(TissueBankSendByDate, "TissueBank Send By Date");
                }
            }
            ValidateRequired(LastModifiedBy, "Last Modified By");

            //Regular Expression Validations
            ValidateNumeric(OrderId, "Order-Id");
            ValidateNumeric(StatusId, "Status-Id");
            ValidateIsDate(TissueBankSendByDate, "TissueBank Send By Date");
            ValidateNumeric(LastModifiedBy, "Last Modified By");
        }

        public void ValidateOrderRequest(int TissueBankId, string SearchBy, int CurrentPage, int PageSize, string SortDirection, string SortExpression)
        {
            //Required Validations
            ValidateRequired(TissueBankId, "TissueBank Id");
            ValidateRequired(CurrentPage, "Current Page");
            ValidateRequired(PageSize, "Page Size");
            ValidateRequired(SortDirection, "Sort Direction");
            ValidateRequired(SortExpression, "Sort Expression");

            //Regular Expression Validations
            ValidateNumeric(TissueBankId, "TissueBank Id");
        }

        public void ValidateOrderDetailRequest(int OrderId)
        {
            //Required Validations
            ValidateRequired(OrderId, "Order Id");

            //Regular Expression Validations
            ValidateNumeric(OrderId, "Order Id");
        }
    }
}
