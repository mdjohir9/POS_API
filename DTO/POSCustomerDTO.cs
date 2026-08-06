using System.ComponentModel.DataAnnotations;

namespace POS_API.DTO
{
    public class POSCustomerDTO
    {


        [Required]
        [MaxLength(50)]
        public string CustomerCode { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string CustomerName { get; set; } = null!;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(300)]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
