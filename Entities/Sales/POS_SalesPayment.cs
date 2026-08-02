namespace POS_API.Entities.Sales
{
    public class POS_SalesPayment
    {
        public long Id { get; set; }

        public long SalesMasterId { get; set; }

        public long PaymentMethodId { get; set; }

        public decimal Amount { get; set; }

        public POS_SalesMaster Sales { get; set; } = null!;

        public POS_SalesPaymentMethod PaymentMethod { get; set; } = null!;
    }
}
