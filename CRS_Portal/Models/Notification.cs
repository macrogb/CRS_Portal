using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCV_Portal.Models
{
    public class Notification
    {
        public long ID { get; set; }
        public NotificationCategory Category { get; set; }
        public string CategoryDesc { get; set; }
        public string Source { get; set; }
        public string DestinationBankname { get; set; }
        public string DestinationBankFRN { get; set; }
        public string Message { get; set; }
        public bool UserSeen { get; set; }
        public bool IsActive { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedTime { get; set; }
    }

    public enum NotificationCategory
    {
        AuditProcess = 1,
        LicenseCheckProcess = 2,
        AutomationProcess = 3,
        OtherProcess = 4
    }
}


