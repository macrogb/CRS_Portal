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
        public int? ID { get; set; }
        public string CUSTID { get; set; }
        public string TITLE { get; set; }
        public string ENTITYNAME { get; set; }
        public string BUILDINGIDENTIFIER { get; set; }
        public string STREETNAME { get; set; }
        public string DISTRICTNAME { get; set; }
        public string CITY { get; set; }
        public string POSTCODE { get; set; }
        public string COUNTRYCODE { get; set; }
        public string EMAILID { get; set; }
        public string ACCHOLDTYPE { get; set; }
        public string RESCOUNTRYCODE { get; set; }
        public string PCOUNTRYIDENTITYTYPE { get; set; }
        public string PCOUNTRYIDENTITYNO { get; set; }
        public string PIDENITYISSUEDBY { get; set; }
        public string SECONDRESCOUNTRYCODE { get; set; }
        public string SECONDCOUNTRYIDENTITYTYPE { get; set; }
        public string SECONDCOUNTRYIDENTITYNO { get; set; }
        public string SECONDIDENITYISSUEDBY { get; set; }
        public string THIRDRESCOUNTRYCODE { get; set; }
        public string THIRDCOUNTRYIDENTITYTYPE { get; set; }
        public string THIRDCOUNTRYIDENTITYNO { get; set; }
        public string THIRDIDENITYISSUEDBY { get; set; }
        public string CREATEDBY { get; set; }
        public string CREATEDDT { get; set; }
        public string CREATEDTM { get; set; }
        public string MODIFIEDBY { get; set; }
        public string MODIFIEDDT { get; set; }
        public string MODIFIEDTM { get; set; }

    }

    public class EntityLookupSummaryDto
    {
        public string EntityID { get; set; }
        public string EntityName { get; set; }
        public string EntityType { get; set; }
    }
}
