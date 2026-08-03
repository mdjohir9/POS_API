using System.ComponentModel.DataAnnotations;

namespace POS_API.Entities.Master
{
    public class POSCustomer:BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string? CustomerCode { get; set; }



        [Required]
        [MaxLength(150)]
        public string? CustomerName { get; set; }



        [MaxLength(20)]
        public string? Phone { get; set; }



        [MaxLength(300)]
        public string? Address { get; set; }
    }
}
