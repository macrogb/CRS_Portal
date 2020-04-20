using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCV_Portal.Models
{
    interface SCVAudit
    {
    }
    public class SCVInput
    {
        public string BankName { get; set; }
        public string BankFRNNo { get; set; }
        public string BankAccessKey { get; set; }
        public string Data { get; set; }
        public string AuditSettingsFileName { get; set; }
        public string ToEmailAddress { get; set; }
        public string CCEmailAddress { get; set; }
        public string BCCEmailAddress { get; set; }
        public string ReferenceNumber { get; set; }
        public List<string> lstReferenceNumber { get; set; }
    }
    public class POLMTPF
    {
        public int ID { get; set; }
        public string PMODULE { get; set; }
        public string PTYPE { get; set; }
        public string PDESC { get; set; }
        public string POLDET { get; set; }
        public string PEDTYP { get; set; }
        public bool myonoffswitch { get; set; }
    }
  
    public class AutomationRunningStatus
    {
        public bool IsAutomationRunning { get; set; }
        public string AutomationStartDateTime { get; set; }
        public string AutomationEstimatedEndDateTime { get; set; }
        public int CompletedLevel { get; set; }
    }

    public class SCVAuditDetails
    {
        public bool IsSCVAuditRunning { get; set; }
        public int FileType { get; set; }
        public SCVAuditConfig auditConfig { get; set; }
        public SCVAuditRun auditRun { get; set; }
        public AutomationRunningStatus automationRunningStatus { get; set; }
    }

    public class SCVAuditConfig
    {
        [Required(ErrorMessage = "ReportingTOEmailAddress is required")]
        public string ReportingTOEmailAddress { get; set; }

        public string ReportingCCEmailAddress { get; set; }

        //[Required(ErrorMessage = "IsFileHeaderAvailable is required")]
        //public bool IsFileHeaderAvailable { get; set; }

        //[ReadOnly(true)]
        //public string StatusCode_SFTP { get; set; }
        //[ReadOnly(true)]
        //public string StatusCode_Exclusion { get; set; }
        //[ReadOnly(true)]
        //public string StatusCode_Ineligible { get; set; }
        //[ReadOnly(true)]
        //public string StatusCode_CareOf { get; set; }
        //[ReadOnly(true)]
        //public string StatusCode_NFTP { get; set; }
        //[ReadOnly(true)]
        //public string StatusCode_BEN { get; set; }
        //[ReadOnly(true)]
        //public string StatusCode_EXCL_BEN { get; set; }
        //[ReadOnly(true)]
        //public string StatusCode_EXCL_LEGDOR { get; set; }
        //[ReadOnly(true)]
        //public string StatusCode_EXCL_LEGDIS { get; set; }
        //[ReadOnly(true)]
        //public string StatusCode_EXCL_HMTS { get; set; }

        

        public string unusualCharInAccountTitle { get; set; }
        public string unusualCharInName { get; set; }
        public string unusualCharInAddress { get; set; }
        //[Required(ErrorMessage = "Column Delimiter is required")]
        //[MinLength(1,ErrorMessage = "Column Delimiter should be in a single character")]
        //public string ColumnDelimiter { get; set; }
    }

    public class AuditStsCodeGrid
    {
        public string MainCategory { get; set; }
        public string SubCategory { get; set; }
        public string Description { get; set; }
        public string StatusCodes { get; set; }
    }
    public class SCVAuditRun
    {
        public string SCVLicenseActivatedDate { get; set; }
        public string SCVLicenseExpiryDate { get; set; }
        public string SCVLastAuditRunDate { get; set; }
        public string RemainingDays { get; set; }
        public string TotalSCVAuditCount { get; set; }
        public string CompletedSCVAuditCount { get; set; }
        public string RemainingSCVAuditCount { get; set; }

        public int ExpectionRpt_HighRiskCount { get; set; }
        public int ExpectionRpt_MediumRiskCount { get; set; }
        public int ExpectionRpt_LowRiskCount { get; set; }

        public int FileType { get; set; }
        public int ProcSubFileType { get; set; }

        public string SCV_A_Filename { get; set; }
        public string SCV_B_Filename { get; set; }
        public string SCV_C_Filename { get; set; }
        public string SCV_D_Filename { get; set; }

        public string EXC_A_Filename { get; set; }
        public string EXC_B_Filename { get; set; }
        public string EXC_C_Filename { get; set; }
        public string EXC_D_Filename { get; set; }

        public string SCV_ABD_Filename { get; set; }
        public string EXC_ABD_Filename { get; set; }

        public string SCV_ABCD_Filename { get; set; }
        public string EXC_ABCD_Filename { get; set; }

        public string CompletedLevel { get; set; }
        public string ProcessingFileName { get; set; }
        public string AuditStartDateTime { get; set; }
        public string AudtiRunningTime { get; set; }
        public string AuditEstimatedEndDateTime { get; set; }

        public string CapImage { get; set; }
        [Required(ErrorMessage = "Captcha is required")]
        public string CaptchaCodeText { get; set; }

        public string CaptchaCode { get; set; }

    }

    public class AuditRptInfo
    {
        public string ReportName { get; set; }
        public string ReportProcessedDateTime { get; set; }
    }

    public class AuditRunningStatus
    {
        public bool IsAuditRunning { get; set; }
        public string AuditStartDateTime { get; set; }
        public string AuditEstimatedEndDateTime { get; set; }
        public string AudtiRunningTime { get; set; }
        public int CompletedLevel { get; set; }
        public string ProcessingFileName { get; set; }
        
    }

    public class ExceptionReports
    {
        public long ID { get; set; }
        public string RPTNAME { get; set; }
        public string RPTPATH { get; set; }
        public int SCVID { get; set; }
        public string EXCPRPTFLAG { get; set; }
        public int ORDERPREC { get; set; }
        public string FUNCNAME { get; set; }
        public string ENABLED { get; set; }
        public string SOURCE { get; set; }
        public string DESCRIPTION { get; set; }
        public string RiskLevel { get; set; }
    }

    public class ExceptionReportsForExport
    {
        public string ReportName { get; set; }
        public string Description { get; set; }
        public string RiskLevel { get; set; }
        public string IsActive { get; set; }
    }

    public class ExceptionRptHistory
    {
        public long ID { get; set; }
        public long ReportID { get; set; }
        public string ReportName { get; set; }
        public string FunctionName { get; set; }
        public string IsActive { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }
    }

    public class CompareExpRepots
    {
        [JsonProperty(PropertyName = "data")]
        public DynamicGrid ChildData { get; set; }
    }
    public class DynamicGrid
    {
        public List<dynamic> Data { get; set; }
    }

    public class EMContentPF
    {
        public int ID { get; set; }
        public string ReferenceNumber { get; set; }
        public string EMDATE { get; set; }
        public string EMTIME { get; set; }
        public string EMFROM { get; set; }
        public string EMTO { get; set; }
        public string EMSUB { get; set; }
        public string EMATTA { get; set; }
        public string EMBODY { get; set; }
        public string EMSTATUS { get; set; }
        public string EMCC { get; set; }
        public string EMFILENM { get; set; }
        public string EMBCC { get; set; }
    }
}

