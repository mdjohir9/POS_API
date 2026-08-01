using System.ComponentModel.DataAnnotations;

namespace POS_API.DTO.UserRoles
{
    public class UserRolesDTO
    {
        [Required(ErrorMessage = "UserRole Name is required")]
        [MaxLength(100, ErrorMessage = "Please insert a value less than 100 characters.")]
        public string? UserRole { get; set; }
        public string? CompanyId { get; set; }

        [Required(ErrorMessage = "Permissions is required")]
        public string? Permissions { get; set; }

        [Required(ErrorMessage = "Ordering is required")]
        public int Ordering { get; set; }
        public int? DataAccessLevel { get; set; }

        public Boolean? IsActive { get; set; }
    }
}
