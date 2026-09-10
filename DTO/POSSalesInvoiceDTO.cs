namespace POS_API.DTO
{
    public class POSSalesInvoiceDTO
    {
        public int Id { get; set; }

        public string? InvoiceNo { get; set; }

        public DateTime SalesDate { get; set; }

        public long? CustomerId { get; set; }

        public string? CustomerName { get; set; }

        public decimal SubTotal { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal GrandTotal { get; set; }



        public List<POSSalesDetailCreateDTO> Details { get; set; } = new List<POSSalesDetailCreateDTO>();

        public List<POSSalesPaymentCreateDTO> Payments { get; set; }  = new List<POSSalesPaymentCreateDTO>();
    }
}
