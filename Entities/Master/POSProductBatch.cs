using POS_API.Entities.Purchase;
using POS_API.Entities.Sales;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS_API.Entities.Master
{
    public class POSProductBatch
    {
        [Key]
        [Required]
        public long Id { get; set; }
        public long? CompanyId { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public long? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

        public long? DeletedBy { get; set; }
        [Required]
        public long ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public POSProduct? Product { get; set; }


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
        public ICollection<POSPurchaseDetail>? PurchaseDetails { get; set; }

        public ICollection<POSSalesDetail>? SalesDetails { get; set; }
    }
}
