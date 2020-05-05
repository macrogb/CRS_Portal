using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRS_Portal.Models
{
    public class EntityLookupDto
    {
        [Required(ErrorMessage = "Entity ID is required.")]
        public string CustID { get; set; }
        [Required(ErrorMessage = "Entity Name is required.")]
        [StringLength(105, MinimumLength = 1, ErrorMessage = "Entity Name must have minimum 1 and maximum 105 characters")]
        public string EntityName { get; set; }
        [StringLength(70, MinimumLength = 1, ErrorMessage = "Building Identifier must have minimum 1 and maximum 70 characters")]
        public string BuildingIdentifier { get; set; }
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Street Name must have minimum 1 and maximum 100 characters")]
        public string StreetName { get; set; }
        [StringLength(70, MinimumLength = 1, ErrorMessage = "District Name must have minimum 1 and maximum 70 characters")]
        public string DistrictName { get; set; }
        [StringLength(70, MinimumLength = 1, ErrorMessage = "City must have minimum 1 and maximum 70 characters")]
        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }
        [StringLength(70, MinimumLength = 1, ErrorMessage = "Post Code must have minimum 1 and maximum 70 characters")]
        public string PostCode { get; set; }
        [Required(ErrorMessage = "Country Code is required.")]
        public string CountryCode { get; set; }
        public string EmailID { get; set; }
        [Required(ErrorMessage = "Entity Type is required.")]
        public string AccHoldType { get; set; }
        [Required(ErrorMessage = "Primary country Tax Details required.")]
        public string ResCountryCode { get; set; }
        public string PCountryIdentityType { get; set; }
        [StringLength(80, MinimumLength = 1, ErrorMessage = "Identity number must have minimum 1 and maximum 80 characters")]
        public string PCountryIdentityNo { get; set; }
        public string PIdentityIssuedBy { get; set; }
        public string secondResCountryCode { get; set; }
        public string SecondCountryIdentityType { get; set; }
        [StringLength(80, MinimumLength = 1, ErrorMessage = "Identity number must have minimum 1 and maximum 80 characters")]
        public string SecondCountryIdentityNo { get; set; }
        public string SecondIdentityIssuedBy { get; set; }
        public string ThirdResCountryCode { get; set; }
        public string ThirdCountryIdentityType { get; set; }
        [StringLength(80, MinimumLength = 1, ErrorMessage = "Identity number must have minimum 1 and maximum 80 characters")]
        public string ThirdCountryIdentityNo { get; set; }
        public string ThirdIdentityIssuedBy { get; set; }

    }
    public class EntityLookupDetailDto :EntityLookupDto
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long? ID { get; set; }
        public string Title { get; set; }
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
