using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRS_Portal.Models
{
    public class TransactionDetails
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Int64 ID { get; set; }
        public string ActBranch { get; set; }
        public string ActNo { get; set; }
        public string SortCode { get; set; }
        public string IBAN { get; set; }
        public string CurrCode { get; set; }
        public string ActBal { get; set; }
        public string IntAmt { get; set; }
        public string PaymentCode { get; set; }
        public string ActClsInd { get; set; }
        public string ActClsDt { get; set; }
        public string ActOpenDt { get; set; }
        public string UndocumentedActInd { get; set; }
        public string DorActInd { get; set; }
        public string CustType { get; set; }
        public string ActType { get; set; }
        public string PCUSTID { get; set; }
        public string J1CUSTID { get; set; }
        public string J2CUSTID { get; set; }
        public string J3CUSTID { get; set; }
        public string J4CUSTID { get; set; }
        public string J5CUSTID { get; set; }
        public string J6CUSTID { get; set; }
        public string J7CUSTID { get; set; }
        public string J8CUSTID { get; set; }
    }

    public class TransactionDetailsSummary
    {
        public Int64 ID { get; set; }
        public string ActBranch { get; set; }
        public string ActNo { get; set; }
        public string SortCode { get; set; }
        public string IBAN { get; set; }
        public string ActBal { get; set; }
        public string IntAmt { get; set; }
    }
}
