using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRS_Portal.Models
{
    public class AuditSettings
    {
        public string AuditStartDateTime { get; set; }
        public string AuditTotalCount { get; set; }
        public string AuditRemainingCount { get; set; }
        public string AuditAccessedCount { get; set; }

        public string AuditReportingToMailAddress { get; set; }
        public string AuditReportingCCMailAddress { get; set; }
        public string RmvSplCharInCustomerName { get; set; }
        public string RmvSplCharInCustomerAddress { get; set; }
        public string RmvSplCharInCustomerTitle { get; set; }

        public string FileType { get; set; }
        public string ProcessingFileType { get; set; }
        public string FileHeader { get; set; }
        public string UploadedFilePath { get; set; }
        public List<string> UploadedFileNames { get; set; }

        public List<AccountStatusCode> AccountStatusCode { get; set; }
        public List<ExceptionRptExcludeStsCodes> ExceptionRptExcludeStsCodes { get; set; }
        public List<BICDetails> BICDetails { get; set; }
        public List<CorpAccounts> CorpAccounts { get; set; }
        public List<POAAccounts> POAAccounts { get; set; }
        public List<ProductDetails> ProductDetails { get; set; }
        public List<SortCodes> SortCodes { get; set; }
        public List<ExceptionReports> ExceptionReports { get; set; }

    }
}
