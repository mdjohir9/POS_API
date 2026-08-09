using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace POS_API.Entities
{
    [SoftDelete]
    public class User
    {
        [Required]
        [Key]
        public int UserId { get; set; }
        public long? CompanyId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [MaxLength(100, ErrorMessage = "Please insert FirstName value less than 100 characters.")]
        public string? FirstName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [MaxLength(100, ErrorMessage = "Please insert Last Name value less than 100 characters.")]

        public string? LastName { get; set; }

        [Required(ErrorMessage = "UserName Name is required")]
        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "UserName must be at least 6 characters.")]
        [MaxLength(100, ErrorMessage = "Please insert FirstName value less than 300 characters.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9._-]*$", ErrorMessage = "Username must start with an alphabetic character and can only contain letters, numbers, '-', '.', and '_'.")]
        public string? UserName { get; set; }
        public string? UserImage { get; set; }


        [Column(TypeName = "nvarchar(150)")]
        [Required(ErrorMessage = "User Password Name is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string? UserPassword { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Email Address is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "UserRoleID is required")]
        public int UserRoleID { get; set; }
        public bool? IsGuestUser { get; set; }
        public bool? IsApprovingAuthority { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? ReferenceID { get; set; }//Refarence Id ==CustommerID
        [Column(TypeName = "nvarchar(max)")]
        public string? AdditionalPermissions { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? RemovedPermissions { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string? DataAccessPermission { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
        public bool? IsAdministrator { get; set; }

        

    }
}
