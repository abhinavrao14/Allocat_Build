using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allocat.DataServiceInterface
{
    public interface IDataService
    {
        void CreateSession();
        void BeginTransaction();
        void CommitTransaction(Boolean closeSession);
        void RollbackTransaction(Boolean closeSession);
        void CloseSession();
    }
}
