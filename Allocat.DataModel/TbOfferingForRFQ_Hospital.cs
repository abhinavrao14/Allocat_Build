using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allocat.DataModel
{
    public class TbOfferingForRFQ_Hospital
    {
        public int TissueBankProductId { get; set; }
        public int TissueBankId { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string Remark { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
