using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRS_Portal.Models
{
    public enum ProductTypeEnum
    {
        //IAA, ISA, NA, FD1, FD2, FD4, FP4P, OTHER
        IAA = 1,
        ISA =2,
        NA =3,
        FD1 =4,
        FD2 = 5,
        FD4 = 6,
        FP4P = 7,
        OTHER = 8
    }

    public class ProductDetails 
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int? ID { get; set; }
        [Required(ErrorMessage = "Product type is required")]
        public ProductTypeEnum ProductType { get; set; }
        [Required(ErrorMessage = "Product name is required")]
        //[RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Product name allowed only Alphabets and Numbers")]
        public string ProductName { get; set; }
        public string ProductNameWithOutSpace { get; set; }
        public string ProductDescription { get; set; }
        public string ProductTypeDesc { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }
        public long ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedTime { get; set; }
    }

    public class ProductDetailsForExport
    {
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string ProductDescription { get; set; }
    }

    public class BICDetails
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int? ID { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        [Required(ErrorMessage = "BIC code is required")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "BIC Code allowed only Alphabets and Numbers")]
        public string BICCode { get; set; }
        public string CountryCode { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }
        public long ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedTime { get; set; }
    }

    public class BICDetailsForExport
    {
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string BICCode { get; set; }
        public string CountryCode { get; set; }
    }

    public class CorpAccounts
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int? ID { get; set; }
        [Required(ErrorMessage = "Account number is required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Account number allowed only Numbers")]
        public string AccountNumber { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }
        public long ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedTime { get; set; }
    }

    public class CorpAccountsForExport
    {
        public string AccountNumber { get; set; }       
    }

    public class SortCodes
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int? ID { get; set; }
        [Required(ErrorMessage = "Sortcode is required")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Sort code allowed only Alphabets and Numbers")]
        public string SortCode { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }
        public long ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedTime { get; set; }
    }

    public class SortCodesExport
    {
        public string SortCode { get; set; }
    }

    public class POAAccounts
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int? ID { get; set; }
        [Required(ErrorMessage = "Account number is required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Account number allowed only Numbers")]
        public string AccountNumber { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }
        public long ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedTime { get; set; }
    }

    public class POAAccountsForExport
    {
        public string AccountNumber { get; set; }
    }

    public class AccountStatusCode
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int? ID { get; set; }
        [Required(ErrorMessage = "Status code is required")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Status code allowed only Alphabets and Numbers")]
        public string StatusCode { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public AccountStatusMainCategoryEnum CategoryType { get; set; }
        public string SubCategory { get; set; }
        public AccountStatusSubCategoryEnum SubCategoryType { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }
        public long ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedTime { get; set; }
    }

    public class AccountStatusCodeForExport
    {
        public string StatusCode { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ExclusionSubCategory { get; set; }
    }

    public enum AccountStatusMainCategoryEnum
    {
        FFSTP = 1,
        NFFSTP = 2,
        EXCLUSION = 3,
        INELIGIBLE = 4,
    }

    public enum AccountStatusSubCategoryEnum
    {
        Select = -1,
        BEN = 5,     
        LDM = 6, //legally dormant 
        LDS = 7, //legal dispute
        HMTS = 8, //HM Treasury
        //CAREOF = 5,
        //BLANK = 5,
    }

    public class CorporateAccounts
    {
        public List<string> AccountNumber { get; set; }
    }
    public class BICDet
    {
        public List<string> BIC { get; set; }
    }

    public class ExceptionRptExcludeStsCodes
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long? ID { get; set; }
        public long ReportID { get; set; }
        public string ReportName { get; set; }
        public string ReportNameWithOutSpace { get; set; }
        public string StatusCodes { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }
        public long ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedTime { get; set; }
    }

    public class ExceptionRptExcludeStsCodesExport
    {
        public string ReportName { get; set; }
        public string StatusCodes { get; set; }
    }
}
