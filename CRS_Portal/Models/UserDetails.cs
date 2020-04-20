using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCV_Portal.Models
{
    public class UserDetails
    {
        public long? UserID { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        //[MaxLength(5)]
        public string UserName { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        //[Required(ErrorMessage = "Last name required")]
        public string LastName { get; set; }


        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(18, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [StringLength(18, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 6)]
        //[StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Required(ErrorMessage = "Confirm password is required")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "User type is required")]
        public UserLevelEnum UserType { get; set; }

        public string UserTypeDesc { get; set; }

        [Required(ErrorMessage = "Bank name is required")]
        public string BankName { get; set; }
        [Required(ErrorMessage = "Bank sort name is required")]
        public string BankSortName { get; set; }
        [Required(ErrorMessage = "Bank FRN no is required")]
        public string BankFRNNo { get; set; }

        public bool IsActive { get; set; }
        public string LastLoginDate { get; set; }
        public string LastLoginTime { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedTime { get; set; }
    }

    public class UserProfile
    {
        public long? UserID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        //[Required(ErrorMessage = "Last name required")]
        public string LastName { get; set; }

        //[Required(ErrorMessage = "Bank name is required")]
        [ReadOnly(true)]
        public string BankName { get; set; }

        [Required(ErrorMessage = "Bank sort name is required")]
        public string BankSortName { get; set; }

        //[Required(ErrorMessage = "FRN Number is required")]
        [ReadOnly(true)]
        public string BankFRNNo { get; set; }
    }

    public enum UserLevelEnum
    {
        User=1,
        ClientAdmin =2,
        SuperAdmin =3
    }

    public class ChangePassword
    {
        public long? UserID { get; set; }
        public string UserName { get; set; }

        [Display(Name = "Current Password")]
        [Required(ErrorMessage = "Current Password is required")]
        [StringLength(18, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "New Password is required")]
        [StringLength(18, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm New password")]
        [StringLength(18, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 6)]
        //[StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        [Required(ErrorMessage = "Confirm New password is required")]
        public string ConfirmNewPassword { get; set; }
    }

    public class Login
    {
        public long UserID { get; set; }
        [Required(ErrorMessage = "User name is required")]
        public string Username { get; set; }
        //public string UsernameErrorMessgae { get; set; }
        //public bool UsernameIsRequired { get; set; }
        //public int UsernameMaxLength { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        //public string PasswordErrorMessgae { get; set; }
        //public bool PasswordIsRequired { get; set; }
    }
}
