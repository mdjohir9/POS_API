using POS_API.Entities.Purchase;

namespace POS_API.DTO
{
    public class POSPurchaseViewDto
    {
        public long Id { get; set; }
        public string? CompanyId { get; set; }
        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string? PurchaseNo { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? ProductNames { get; set; }
    }
}
