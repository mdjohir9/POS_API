using System.ComponentModel.DataAnnotations;

namespace POS_API.DTO
{
    public class POSPurchaseDetailDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public decimal Rate { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
