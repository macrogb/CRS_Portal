using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRS_Portal.Models
{
    public class TemplateDownloadModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TemplateFilePath { get; set; }
        public string LastYearLookupData { get; set; }
        public bool IsActive { get; set; }
    }
}
