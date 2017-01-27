using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allocat.DataModel;
using Allocat.Utility;
using System.Linq.Dynamic;

namespace Allocat.DataService
{
    public class ProductMasterDataService : EntityFrameworkDataService, IProductMasterDataService
    {
        public ProductMaster_TissueBank GetProductMaster_DomainFamily_ById(int ProductMasterId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            ProductMaster_TissueBank productMaster_TissueBank = (from pm in dbConnection.ProductMaster
                                                                 join ds in dbConnection.DomainScope on pm.DomainScopeId equals ds.DomainScopeId into dsSet
                                                                 from ds in dsSet.DefaultIfEmpty()
                                                                 join pmFamily in dbConnection.ProductMaster on pm.FamilyId equals pmFamily.ProductMasterId into t1
                                                                 from rt1 in t1.DefaultIfEmpty()
                                                                 where pm.ProductMasterId == ProductMasterId
                                                                 select new ProductMaster_TissueBank
                                                                 {
                                                                     ProductMasterId = pm.ProductMasterId,
                                                                     ProductMasterName = pm.ProductMasterName,
                                                                     ProductMasterDescription = pm.ProductMasterDescription,
                                                                     ProductMasterFeatures = pm.ProductMasterFeatures,
                                                                     ProductMasterImageName = pm.ProductMasterImageName,
                                                                     ProductMasterThumbnailImageName = pm.ProductMasterThumbnailImageName,
                                                                     DomainScopeId = pm.DomainScopeId,
                                                                     FamilyId = pm.FamilyId,
                                                                     IsActive = pm.IsActive,
                                                                     CreatedDate = pm.CreatedDate,
                                                                     CreatedBy = pm.CreatedBy,
                                                                     LastModifiedDate = pm.LastModifiedDate,
                                                                     LastModifiedBy = pm.LastModifiedBy,
                                                                     DomainName = ds.DomainName,
                                                                     FamilyProductMasterName = rt1.ProductMasterName

                                                                 }).First(); 

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add("1 Product Master found.");
            return productMaster_TissueBank;
        }
    }
}
