namespace POS_API.DTO
{
    public class POSSalesListDTO
    {
        public long SalesId { get; set; }

        public string? InvoiceNo { get; set; }

        public DateTime SalesDate { get; set; }

        public long CustomerId { get; set; }

        public string? CustomerName { get; set; }

        public string? ProductName { get; set; }

        public string? PaymentMethod { get; set; }

        public decimal GrossAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal NetAmount { get; set; }
    }
}
