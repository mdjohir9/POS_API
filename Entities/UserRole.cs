using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace POS_API.Entities
{
    [SoftDelete]
    public class UserRole
    {
        [Required]
        [Key]
        public int UserRoleId { get; set; }
        public string? CompanyId { get; set; }

        [Required(ErrorMessage = "UserRole Name is required")]
        [Column(TypeName = "nvarchar(100)")]
        [MaxLength(100, ErrorMessage = "Please insert a value less than 100 characters.")]
        public string? UserRoleName { get; set; }

        [Required(ErrorMessage = "Permissions is required")]
        [Column(TypeName = "nvarchar(max)")]
        public string? Permissions { get; set; }

        public int? DataAccessLevel { get; set; }

        [Required(ErrorMessage = "Ordering is required")]
        public int Ordering { get; set; }
        public Boolean? IsActive { get; set; }
        public Boolean? Deleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }


    }
}
