using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCV_Portal.Models
{
    public class SCVAutomation
    {
        [Required(ErrorMessage = "ReportingEmailAddress is required")]
        public string ReportingEmailAddress { get; set; }
    }

    public class SCVAutomationRun
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

        public string CompletedLevel { get; set; }
        public string ProcessingFileName { get; set; }
        public string AutomationStartDateTime { get; set; }
        public string AutomationRunningTime { get; set; }
        public string AutomationEstimatedEndDateTime { get; set; }

    }

    public class AutomationRunningDetails
    {
        public bool IsAutomationRunning { get; set; }
        public string AutomationStartDateTime { get; set; }
        public string AutomationEstimatedEndDateTime { get; set; }
        public string AutomationRunningTime { get; set; }
        public int CompletedLevel { get; set; }
        public string ProcessingFileName { get; set; }

    }

    public class AutomationRptInfo
    {
        public string ReportName { get; set; }
        public string ReportProcessedDateTime { get; set; }
    }
}
