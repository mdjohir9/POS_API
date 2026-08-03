namespace POS_API.Entities.Sales
{
    public class POSSalesPayment
    {
        public long Id { get; set; }

        public long SalesMasterId { get; set; }

        public long PaymentMethodId { get; set; }

        public decimal Amount { get; set; }

        public POSSalesMaster Sales { get; set; } = null!;

        public POSSalesPaymentMethod PaymentMethod { get; set; } = null!;
    }
}
