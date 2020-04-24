using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRS_Portal.Models
{
    public class ClosedAccountDetails
    {
        public Int64?  ID { get; set; }
        public string DealNo { get; set; }
        public string DealDt { get; set; }
        public string ValueDt { get; set; }
        public string MaturityDt { get; set; }
        public string CustNo { get; set; }
        public string CustName { get; set; }
        public string PrincipalAmt { get; set; }
        public string Currency { get; set; }
        public string InterestAmtPaid { get; set; }
        public string ProdType { get; set; }
        public string PaymentCode { get; set; }
    }
}
