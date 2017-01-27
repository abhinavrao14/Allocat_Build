using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allocat.DataModel
{
    public class ProductMaster_TissueBank
    {
        public int ProductMasterId { get; set; }
        public string ProductMasterName { get; set; }
        public string ProductMasterDescription { get; set; }
        public string ProductMasterFeatures { get; set; }
        public string ProductMasterImageName { get; set; }
        public Nullable<int> DomainScopeId { get; set; }
        public Nullable<int> FamilyId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public string ProductMasterThumbnailImageName { get; set; }
        public string DomainName { get; set; }
        public string FamilyProductMasterName { get; set; }
    }
}
