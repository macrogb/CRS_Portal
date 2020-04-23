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
        [Required(ErrorMessage = "Entity ID is required.")]
        public string CustID { get; set; }
        public string Title { get; set; }
        [Required(ErrorMessage = "Entity Name is required.")]
        [StringLength(105, MinimumLength = 1, ErrorMessage = "Entity Name must have minimum 1 and maximum 105 characters")]
        public string EntityName { get; set; }
        [Required(ErrorMessage = "Building Identifier is required.")]
        [StringLength(70, MinimumLength = 1, ErrorMessage = "Building Identifier must have minimum 1 and maximum 70 characters")]
        public string BuildingIdentifier { get; set; }
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Street Name must have minimum 1 and maximum 100 characters")]
        public string StreetName { get; set; }
        [Required(ErrorMessage = "District Name is required.")]
        [StringLength(70, MinimumLength = 1, ErrorMessage = "District Name must have minimum 1 and maximum 70 characters")]
        public string DistrictName { get; set; }
        [StringLength(70, MinimumLength = 1, ErrorMessage = "City must have minimum 1 and maximum 70 characters")]
        public string City { get; set; }
        [StringLength(35, MinimumLength = 1, ErrorMessage = "Post Code must have minimum 1 and maximum 35 characters")]
        public string PostCode { get; set; }
        public string CountryCode { get; set; }
        public string EmailID { get; set; }
        [Required(ErrorMessage = "Entity Type is required.")]
        public string AccHoldType { get; set; }
        [Required(ErrorMessage = "Primary country Tax Details required.")]
        public string ResCountryCode { get; set; }
        [Required(ErrorMessage = "Primary country Tax Details required.")]
        public string PCountryIdentityType { get; set; }
        [Required(ErrorMessage = "Primary country Tax Details required.")]
        public string PCountryIdentityNo { get; set; }
        [Required(ErrorMessage = "Primary country Tax Details required.")]
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
