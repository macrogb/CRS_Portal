﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRS_Portal.Models
{
    public class CRSMacroSettings
    {
        public static string Initial_CS { get; set; }
        public static string Canara_CS { get; set; }
        public static string BOI_CS { get; set; }
        public static string ICICI_CS { get; set; }
        
        public static string BankName { get; set; }

        public static string Domain { get; set; }
        public static string UserName { get; set; }
        public static string Password { get; set; }

        public static string CRSWebAPIURL { get; set; }

        public static string LogFilePath { get; set; }

        public static string FilePath { get; set; }
    }
}
