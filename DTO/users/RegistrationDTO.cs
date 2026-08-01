using System.ComponentModel.DataAnnotations;

namespace POS_API.DTO.users
{
    public class RegistrationDTO
    {
        [MaxLength(100, ErrorMessage = "Please insert First Name  value less than 100 characters.")]
        public string? FirstName { get; set; }

        [MaxLength(100, ErrorMessage = "Please insert Last Name value less than 100 characters.")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "UserName Name is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "UserName must be at least 6 characters.")]
        [MaxLength(100, ErrorMessage = "Please insert FirstName value less than 300 characters.")]
        public string? EamilOrPhone { get; set; }

        [Required(ErrorMessage = "User Password Name is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
      
    }
}
