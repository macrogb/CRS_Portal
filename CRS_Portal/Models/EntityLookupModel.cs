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
        public List<IdentityDto> lstIdentityType { get; set; }

        public EntityLookupModel()
        {
            PopulateEntityType();
            PopulateIdentityType();
        }

        private void PopulateIdentityType()
        {
            lstIdentityType = new List<IdentityDto>();
            lstIdentityType.Add(new IdentityDto { ID = 1, IdentityType = "GIIN" });
            lstIdentityType.Add(new IdentityDto { ID = 2, IdentityType = "EIN" });
            lstIdentityType.Add(new IdentityDto { ID = 3, IdentityType = "TIN" });
            lstIdentityType.Add(new IdentityDto { ID = 4, IdentityType = "CRN" });
            lstIdentityType.Add(new IdentityDto { ID = 5, IdentityType = "OTHER" });

        }

        private void PopulateEntityType()
        {
            lstEntityType = new List<EntityTypeDto>();
            lstEntityType.Add(new EntityTypeDto { ID = 1, EntityType = "REPORTABLE ENTITY (ORGANISATION)" });
            lstEntityType.Add(new EntityTypeDto { ID = 2, EntityType = "PASSIVE NON FINANCIAL ENTITY" });
            lstEntityType.Add(new EntityTypeDto { ID = 3, EntityType = "PASSIVE NON FINANCIAL ENTITY WITH CONTROLLING PERSON(S)" });
            lstEntityType.Add(new EntityTypeDto { ID = 4, EntityType = "OWNER DOCUMENTED FINANCIAL INSTITUTION (FATCA ONLY)" });
            lstEntityType.Add(new EntityTypeDto { ID = 5, EntityType = "ACTIVE NON FINANCIAL ENTITY" });

        }
    }

    
}
