namespace POS_API.DTO
{
    public class POSPurchaseCreateResultDTO
    {
        public int PurchaseId { get; set; }
        public string PurchaseNo { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }
}
