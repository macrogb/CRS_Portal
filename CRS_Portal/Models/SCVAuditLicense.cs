using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CRS_Portal.Models
{
    [DataContract]
    public class SCVAuditLicense
    {
        [DataMember(Name = "ID")]
        public long ID { get; set; }
        [DataMember(Name = "BankName")]
        public string BankName { get; set; }
        [DataMember(Name = "BankFRNNo")]
        public string BankFRNNo { get; set; }
        [DataMember(Name = "ToEmailAddress")]
        public string ToEmailAddress { get; set; }
        [DataMember(Name = "ReportingPersonEmailID")]
        public string CCEmailAddress { get; set; }
        [DataMember(Name = "BCCEmailAddress")]
        public string BCCEmailAddress { get; set; }
        [DataMember(Name = "AccessKey")]
        public string AccessKey { get; set; }
        [DataMember(Name = "KeyRunSts")]
        public string KeyRunSts { get; set; }
        [DataMember(Name = "KeyRegDate")]
        public string KeyRegDate { get; set; }
        [DataMember(Name = "KeyRegTime")]
        public string KeyRegTime { get; set; }
        [DataMember(Name = "KeyExpDate")]
        public string KeyExpDate { get; set; }
        [DataMember(Name = "KeyExpTime")]
        public string KeyExpTime { get; set; }
        [DataMember(Name = "Data")]
        public string KeyReNewDate { get; set; }
        [DataMember(Name = "KeyReNewTime")]
        public string KeyReNewTime { get; set; }
        [DataMember(Name = "KeyTotalAccessCount")]
        public string KeyTotalAccessCount { get; set; }
        [DataMember(Name = "KeyAccessedCount")]
        public string KeyAccessedCount { get; set; }
        [DataMember(Name = "KeyAccessReminingCount")]
        public string KeyAccessReminingCount { get; set; }
        [DataMember(Name = "CreDate")]
        public string CreDate { get; set; }
        [DataMember(Name = "CreTime")]
        public string CreTime { get; set; }
        [DataMember(Name = "CreBy")]
        public string CreBy { get; set; }
        [DataMember(Name = "ModDate")]
        public string ModDate { get; set; }
        [DataMember(Name = "ModTime")]
        public string ModTime { get; set; }
        [DataMember(Name = "ModBy")]
        public string ModBy { get; set; }
    }

    public class SCVAuditHistory
    {
        public long ID { get; set; }
        public string ReferenceNumber { get; set; }
        public string BankName { get; set; }
        public string BankFRNNo { get; set; }
        public string AccessKey { get; set; }
        public string KeyRunSts { get; set; }
        public string KeyAccessNo { get; set; }
        public string AuditDate { get; set; }
        public string AuditStartDate { get; set; }
        public string AuditStartTime { get; set; }
        public string AuditEndDate { get; set; }
        public string AuditEndTime { get; set; }
        public string AuditSettingFileDetails { get; set; }
        public string AuditRptFileDetails { get; set; }
        public string AuditRptFilePath { get; set; }
        public string AuditRptFileName { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
        public string CreDate { get; set; }
        public string CreTime { get; set; }
        public string CreBy { get; set; }
        public string ModDate { get; set; }
        public string ModTime { get; set; }
        public string ModBy { get; set; }
    }

    public class ResponseAuditHistory
    {
        [JsonProperty(PropertyName = "data")]
        public List<SCVAuditHistory> MyProperty { get; set; }
    }

    //public class SCVAuditHistoryGrid
    //{
    //    public long ID { get; set; }
    //    public string ReferenceNumber { get; set; }
    //    public string BankName { get; set; }
    //    public string BankFRNNo { get; set; }
    //    public string AccessKey { get; set; }
    //    public string KeyRunSts { get; set; }
    //    public string KeyAccessNo { get; set; }
    //    public string AuditDate { get; set; }
    //    public string AuditStartDate { get; set; }
    //    public string AuditStartTime { get; set; }
    //    public string AuditEndDate { get; set; }
    //    public string AuditEndTime { get; set; }
    //    public string AuditSettingFileDetails { get; set; }
    //    public string AuditRptFilePath { get; set; }
    //    public string AuditRptFileName { get; set; }
    //    public bool IsActive { get; set; }
    //    public string Remarks { get; set; }
    //    public string CreDate { get; set; }
    //    public string CreTime { get; set; }
    //    public string CreBy { get; set; }
    //    public string ModDate { get; set; }
    //    public string ModTime { get; set; }
    //    public string ModBy { get; set; }
    //}


}
