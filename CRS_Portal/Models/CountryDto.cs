using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRS_Portal.Models
{
    public class CountryDto
    {
        public int COUNTRYID { get; set; }
        public string COUNTRYCD { get; set; }
        public string COUNTRY { get; set; }
        public string STATUS { get; set; }
        public int? PRIORITYID { get; set; }
        public string POSTCODEFORMAT { get; set; }
        public string POSTCODEFORMAT1 { get; set; }
        public string CRSSTATUS { get; set; }
     
    }
}
