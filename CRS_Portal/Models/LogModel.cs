using System;

namespace CRS_Portal.Models
{
    //public class ErrorViewModel
    //{
    //    public string RequestId { get; set; }
    //    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    //}

    public class LogDetails
    {
        public string UserID { get; set; }
        public string MethodName { get; set; }
        public string ParameterValue { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }

    public class ErrorLogDetails
    {
        public string UserID { get; set; }
        public string MethodName { get; set; }
        public string ParameterValue { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }

    public class SCV_AuditLog
    {
        public long ID { get; set; }
        public string LogType { get; set; }
        public string LogMessage { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }
    }

}