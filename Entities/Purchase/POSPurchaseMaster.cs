using POS_API.Entities.Master;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS_API.Entities.Purchase
{
    public class POSPurchaseMaster 
    {
        [Key]
        [Required]
        public long Id { get; set; }
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
        [MaxLength(50)]
        public string? PurchaseNo { get; set; }



        [Required]
        public DateTime PurchaseDate { get; set; }



        [Required]
        public long SupplierId { get; set; }



        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }



        [ForeignKey("SupplierId")]
        public POSSupplier? Supplier { get; set; }



        public ICollection<POSPurchaseDetail>? Details { get; set; }

    }
}
