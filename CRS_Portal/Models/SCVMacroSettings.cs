using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRS_Portal.Models
{
    public class SCVMacroSettings
    {
        public static string Initial_CS { get; set; }
        public static string Canara_Audit_CS { get; set; }
        public static string BOI_Audit_CS { get; set; }
        public static string ICICI_Audit_CS { get; set; }
        
        public static string BankName { get; set; }

        public static string Domain { get; set; }
        public static string UserName { get; set; }
        public static string Password { get; set; }

        public static string SCVWebAPIURL { get; set; }

        public static string LogFilePath { get; set; }

        public static string FilePath { get; set; }
    }
}
