using System.ComponentModel.DataAnnotations;

namespace POS_API.DTO
{
    public class POSSupplierDTO
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string SupplierCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string SupplierName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(300)]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
