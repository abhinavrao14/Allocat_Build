using Allocat.DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Allocat.WebApi.WebApiModel
{
    public class UserApiModel : TransactionalInformation
    {
        public IEnumerable<sp_UserMngmt_TissueBank_GetByTissueBankId_Result> Users;
        public IEnumerable<sp_UserMngmt_TissueBank_GetByUserId_Result> UserDetail;
        public IEnumerable<sp_UserMngmt_GetUserRoleByUserId_Result> UserRoles;
        public IEnumerable<TissueBankRoles_TissueBank> TissueBankRoles;
    }

    public class UserMngmt_DTO
    {
        public int TissueBankId { get; set; }
        public string SearchBy { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string SortDirection { get; set; }
        public string SortExpression { get; set; }
    }

    public class UserMngmnt_User_CUD_DTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public int InfoId { get; set; }
        public DataTable TempUser_CUD { get; set; }
        public string OperationType { get; set; }
        public bool AllowLogin { get; set; }
    }
}