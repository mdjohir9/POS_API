using System.ComponentModel.DataAnnotations;

namespace POS_API.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "User Password Name is required")]
        public string? UserPassword { get; set; }
    }
}
