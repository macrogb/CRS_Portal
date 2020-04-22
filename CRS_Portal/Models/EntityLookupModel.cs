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
        public List<EntityTypeDto> lstEntityType { get; set; }

        public EntityLookupModel()
        {
            lstEntityType = new List<EntityTypeDto>();
            lstEntityType.Add(new EntityTypeDto { ID = 1, EntityType = "REPORTABLE ENTITY (ORGANISATION)" });
            lstEntityType.Add(new EntityTypeDto { ID = 2, EntityType = "PASSIVE NON FINANCIAL ENTITY" });
            lstEntityType.Add(new EntityTypeDto { ID = 1, EntityType = "PASSIVE NON FINANCIAL ENTITY WITH CONTROLLING PERSON(S)" });
            lstEntityType.Add(new EntityTypeDto { ID = 1, EntityType = "OWNER DOCUMENTED FINANCIAL INSTITUTION (FATCA ONLY)" });
            lstEntityType.Add(new EntityTypeDto { ID = 1, EntityType = "ACTIVE NON FINANCIAL ENTITY" });
           
        }
    }

    
}
