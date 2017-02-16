using Allocat.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allocat.DataServiceInterface
{
    public interface IProductMasterDataService : IDataService, IDisposable
    {
        ProductMaster_TissueBank GetProductMaster_DomainFamily_ByTissueBankProductMasterId(int TissueBankProductMasterId, out TransactionalInformation transaction);
        ProductMaster_Hospital GetProductMasterByProductMasterName(string ProductMasterName, out TransactionalInformation transaction);
    }
}
