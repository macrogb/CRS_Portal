using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRS_Portal.Models
{
    public class EntityLookupDetailDto
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long? ID { get; set; }
        public string CustID { get; set; }
        public string Title { get; set; }
        public string EntityName { get; set; }
        public string BuildingIdentifier { get; set; }
        public string StreetName { get; set; }
        public string DistrictName { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string CountryCode { get; set; }
        public string EmailID { get; set; }
        public string AccHoldType { get; set; }
        public string ResCountryCode { get; set; }
        public string PCountryIdentityType { get; set; }
        public string PCountryIdentityNo { get; set; }
        public string PIdentityIssuedBy { get; set; }
        public string secondResCountryCode { get; set; }
        public string SecondCountryIdentityType { get; set; }
        public string SecondCountryIdentityNo { get; set; }
        public string SecondIdentityIssuedBy { get; set; }
        public string ThirdResCountryCode { get; set; }
        public string ThirdCountryIdentityType { get; set; }
        public string ThirdCountryIdentityNo { get; set; }
        public string ThirdIdentityIssuedBy { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDT { get; set; }
        public string CreatedTM { get; set; }
        public string ModifiedBY { get; set; }
        public string ModifiedDT { get; set; }
        public string ModifiedTM { get; set; }

    }

    public class EntityLookupSummaryDto
    {
        public long? ID { get; set; }
        public string EntityID { get; set; }
        public string EntityName { get; set; }
        public string EntityType { get; set; }
    }

    public class EntityTypeDto
    {
        public int ID { get; set; }
        public string EntityType { get; set; }
    }
}
