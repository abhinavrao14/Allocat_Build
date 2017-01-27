using System;
using System.Data;

namespace Allocat.ApplicationService
{
    public class UserBusinessRule : ValidationRules
    {
        public UserBusinessRule()
        {
            InitializeValidationRules();
        }

        public void ValidateUserRequest(int TissueBankId, string SearchBy, int CurrentPage, int PageSize, string SortDirection, string SortExpression)
        {
        }
        public void ValidateUserDetailRequest(int UserId)
        {
        }

        public void ValidateUser_CUD(int UserId, string UserName, string Password, string FullName, string MobileNumber, string EmailId, int CreatedBy, int LastModifiedBy, int InfoId, string OperationType, bool AllowLogin, DataTable TempUser_CUD)
        {

        }

        public void ValidateUserRoleRequest(int UserId)
        {

        }
    }
}