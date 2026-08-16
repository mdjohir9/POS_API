using System.ComponentModel.DataAnnotations;

namespace POS_API.DTO
{
    public class POSBrandDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
