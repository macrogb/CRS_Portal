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
        public Int64? ID { get; set; }
        [Required(ErrorMessage = "Account Branch is required.")]
        public string ActBranch { get; set; }
        [Required(ErrorMessage = "Account Number is required.")]
        public string ActNo { get; set; }
        [Required(ErrorMessage = "SortCode is required.")]
        public string SortCode { get; set; }
        [Required(ErrorMessage = "IBAN is required.")]
        public string IBAN { get; set; }
        [Required(ErrorMessage = "Currency Code is required.")]
        public string CurrCode { get; set; }
        [Required(ErrorMessage = "Account Balance is required.")]
        [RegularExpression(@"^\d+\.\d{0,2}$", ErrorMessage = "Invalid Account Balance")]
        public string ActBal { get; set; }
        [Required(ErrorMessage = "Interest Amount is required.")]
        [RegularExpression(@"^\d+\.\d{0,2}$", ErrorMessage = "Invalid Interest Amount")]
        public string IntAmt { get; set; }
        [Required(ErrorMessage = "Payment Code is required.")]
        public string PaymentCode { get; set; }
        public string ActClsInd { get; set; }
        public string ActClsDt { get; set; }
        [Required(ErrorMessage = "Account Opening Date is required.")]
        public string ActOpenDt { get; set; }
        public string UndocumentedActInd { get; set; }
        public string DorActInd { get; set; }
        [Required(ErrorMessage = "Customer Type is required.")]
        public string CustType { get; set; }
        [Required(ErrorMessage = "Account Type is required.")]
        public string ActType { get; set; }
        [Required(ErrorMessage = "Primary Customer ID is required.")]
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
        public Int64? ID { get; set; }
        public string ActBranch { get; set; }
        public string ActNo { get; set; }
        public string SortCode { get; set; }
        public string IBAN { get; set; }
        public string ActBal { get; set; }
        public string IntAmt { get; set; }
    }

    public class PaymentCodeDetail
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
