using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SCV_Portal.Models
{
    public class NotDuplicateCustomer
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int? ID { get; set; }
        [Required(ErrorMessage = "Customer Number is required")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Customer number must be 6 characters")]
        public string CustNo { get; set; }
        [Required(ErrorMessage = "Duplicate Customer Number is required")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Duplicate Customer number must be 6 characters")]
        public string NDUPCustNo { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }
        public long ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedTime { get; set; }
    }

    public class NotDuplicateCustomerExport
    {
        public string CustomerNo { get; set; }
        public string DuplicateCustomerNumber { get; set; }
    }
}
