using System.ComponentModel.DataAnnotations;

namespace POS_API.DTO
{
    public class POSSalesPaymentCreateDTO
    {
        [Required]
        public int PaymentMethodId { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
