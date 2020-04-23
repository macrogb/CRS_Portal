using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRS_Portal.Models
{
    public class TransactionDetailsModel
    {
        public TransactionDetails TranDetails { get; set; }
        public List<Currency> lstCurrency { get; set; }
        public List<PaymentCodeDetail> lstPaymentCode { get; set; }

        public TransactionDetailsModel()
        {
            PopulatePaymentCodeDetails();
        }
        public void PopulatePaymentCodeDetails()
        {
            lstPaymentCode = new List<PaymentCodeDetail>();
            lstPaymentCode.Add(new PaymentCodeDetail { ID = 1, Code = "00", Description = "Custodial Account:Gross Interest Paid/Credited" });
            lstPaymentCode.Add(new PaymentCodeDetail { ID = 1, Code = "01", Description = "Custodial Account:Gross divident Paid/Credited" });
            lstPaymentCode.Add(new PaymentCodeDetail { ID = 1, Code = "02", Description = "Custodial Account:Gross Income Paid/Credited" });
            lstPaymentCode.Add(new PaymentCodeDetail { ID = 1, Code = "03", Description = "Custodial Account:Gross from Property Paid/Credited" });
            lstPaymentCode.Add(new PaymentCodeDetail { ID = 1, Code = "10", Description = "Depository Account" });
            lstPaymentCode.Add(new PaymentCodeDetail { ID = 1, Code = "20", Description = "Other Accounts" });
            lstPaymentCode.Add(new PaymentCodeDetail { ID = 1, Code = "90", Description = "Aggregate Reports" });


        }
    }
}
