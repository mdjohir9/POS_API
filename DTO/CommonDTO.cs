using System.ComponentModel.DataAnnotations;

namespace POS_API.DTO
{
    public class CommonDTO
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public bool IsActive { get; set; }

    }
}
