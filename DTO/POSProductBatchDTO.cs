using System.ComponentModel.DataAnnotations;

namespace POS_API.DTO
{
    public class POSProductBatchDTO
    {

        [Required]
        public int ProductId { get; set; }
        public int? CompanyId { get; set; }
        [Required]
        [MaxLength(50)]
        public string BatchNo { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? LotNo { get; set; }

        public DateTime? ManufacturingDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [Required]
        public decimal PurchasePrice { get; set; }

        [Required]
        public decimal SellingPrice { get; set; }

        [Required]
        public decimal ReceiveQty { get; set; }

        [Required]
        public decimal AvailableQty { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
