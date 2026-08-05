using System.ComponentModel.DataAnnotations;

namespace POS_API.DTO
{
    public class CommonDTO
    {
        public long? Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public string? ShortName { get; set; }
        public bool IsActive { get; set; }

    }
}
