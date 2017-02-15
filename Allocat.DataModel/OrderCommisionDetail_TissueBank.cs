using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allocat.DataModel
{
    public class OrderCommisionDetail_TissueBank
    {
        public decimal? AlloCATFees { get; set; }
        public string CustomerProfileId { get; set; }
        public string CustomerPaymentProfileIds { get; set; }
        public int TissueBankId { get; set; }
    }
}
