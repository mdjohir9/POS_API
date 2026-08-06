using System.ComponentModel.DataAnnotations;

namespace POS_API.DTO
{
    public class POSProductDTO
    {
        //public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string ProductCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        public long CategoryId { get; set; }

        [Required]
        public long BrandId { get; set; }

        [Required]
        public long UnitId { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SalesPrice { get; set; }

        public decimal VATPercent { get; set; }

        public string? Barcode { get; set; }

        public bool IsBatchRequired { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
