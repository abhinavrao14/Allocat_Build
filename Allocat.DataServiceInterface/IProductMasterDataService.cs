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
        ProductMaster_TissueBank GetProductMaster_DomainFamily_ById(int id, out TransactionalInformation transaction);
    }
}
