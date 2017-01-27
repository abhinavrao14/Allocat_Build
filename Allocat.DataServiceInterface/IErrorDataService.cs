using Allocat.DataModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace Allocat.DataServiceInterface
{
    public interface IErrorDataService : IDataService, IDisposable
    {
        string Error_Create(int StatusId, string ErrorBody, string StackTrace,int? TransactionId,int UserId,string ErrorCode);
    }
}