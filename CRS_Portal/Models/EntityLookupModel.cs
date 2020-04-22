using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRS_Portal.Models
{
    public class EntityLookupModel
    {
        public EntityLookupDetailDto entity { get; set; }
        public List<CountryDto> lstcountry { get; set; }
    }
}
