using System.ComponentModel.DataAnnotations;

namespace POS_API.Entities.Sales
{
    public class POSSalesPayment
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public int SalesMasterId { get; set; }

        public int PaymentMethodId { get; set; }

        public decimal Amount { get; set; }

        public POSSalesMaster Sales { get; set; } = null!;

        public POSSalesPaymentMethod PaymentMethod { get; set; } = null!;
    }
}
