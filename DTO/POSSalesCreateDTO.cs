using System.ComponentModel.DataAnnotations;

namespace POS_API.DTO
{
    public class POSSalesCreateDTO
    {
        [Required]
        public string InvoiceNo { get; set; } = string.Empty;

        [Required]
        public DateTime SalesDate { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public decimal DiscountAmount { get; set; }

        [Required]
        public List<POSSalesDetailCreateDTO> Details { get; set; } = new();

        public List<POSSalesPaymentCreateDTO> Payments { get; set; } = new();
    }
}
