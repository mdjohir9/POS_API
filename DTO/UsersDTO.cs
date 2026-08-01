using System.ComponentModel.DataAnnotations;

namespace POS_API.DTO
{
    public class UsersDTO
    {

        public int UserId { get; set; }
        [MaxLength(100, ErrorMessage = "Please insert FirstName value less than 100 characters.")]
        public string? Name { get; set; }
        public string? FirstName { get; set; }


        [MaxLength(100, ErrorMessage = "Please insert Last Name value less than 100 characters.")]

        public string? LastName { get; set; }
        [Required(ErrorMessage = "UserName Name is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "UserName must be at least 6 characters.")]
        [MaxLength(100, ErrorMessage = "Please insert FirstName value less than 300 characters.")]
        public string? UserName { get; set; }
        public string? CompanyName { get; set; }
        public string? UserImage { get; set; }


        [Required(ErrorMessage = "User Password Name is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string? UserPassword { get; set; }
        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address! Please Insert Valid Email")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "UserRoleID is required")]
        public int UserRoleID { get; set; }
        public string? UserRoleName { get; set; }
        public Boolean? IsGuestUser { get; set; }
        public bool? IsApprovingAuthority { get; set; }
        public string? ReferenceID { get; set; }
        public string? AdditionalPermissions { get; set; }
        public string? RemovedPermissions { get; set; }

        public int? DataAccessLevel { get; set; }
        public string? DataAccessPermission { get; set; }
        public Boolean? IsActive { get; set; }
        public string? CompanyId { get; set; }
    }
}
