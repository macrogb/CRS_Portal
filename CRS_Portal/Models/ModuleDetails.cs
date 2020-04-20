using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRS_Portal.Models
{
    public class ModuleDetails
    {
        public int ID { get; set; }
        public int ModuleType { get; set; }
        public int ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public bool IsActive { get; set; }        
    }

    public class ModuleChild
    {
        public int ID { get; set; }
        public int ModuleParentId { get; set; }
        public string ModuleName { get; set; }        
        public bool IsChecked_C { get; set; }
        public string PageID { get; set; }
    }

    public class ModuleParent
    {
        public int ID { get; set; }
        public string ModuleTitle { get; set; }
        public bool IsChecked_P { get; set; }
        public IList<ModuleChild> ModuleChild { get; set; }
        //public IEnumerable<SelectListItem> UserDetails { get; set; }       
    }



    public class UserAccessLevel
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public int MenuID { get; set; }
        public bool IsAccess { get; set; }
    }

}
