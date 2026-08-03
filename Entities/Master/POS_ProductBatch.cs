using POS_API.Entities.Purchase;
using POS_API.Entities.Sales;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS_API.Entities.Master
{
    public class POS_ProductBatch:BaseEntity
    {
        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public POS_Product? Product { get; set; }


        [Required]
        [MaxLength(50)]
        public string BatchNo { get; set; } = string.Empty;


        [MaxLength(50)]
        public string? LotNo { get; set; }


        public DateTime? ManufacturingDate { get; set; }


        public DateTime? ExpiryDate { get; set; }


        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }


        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SellingPrice { get; set; }


        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ReceiveQty { get; set; }


        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AvailableQty { get; set; }


        // Navigation
        public ICollection<POS_PurchaseDetail>? PurchaseDetails { get; set; }

        public ICollection<POS_SalesDetail>? SalesDetails { get; set; }
    }
}
