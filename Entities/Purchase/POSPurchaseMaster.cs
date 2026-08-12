using POS_API.Entities.Master;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS_API.Entities.Purchase
{
    [SoftDelete]
    public class POSPurchaseMaster 
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }


        public int? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }


        public int? DeletedBy { get; set; }
        [Required]
        [MaxLength(50)]
        public string? PurchaseNo { get; set; }



        [Required]
        public DateTime PurchaseDate { get; set; }



        [Required]
        public int SupplierId { get; set; }


        public int? CompanyId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }



        [ForeignKey("SupplierId")]
        public POSSupplier? Supplier { get; set; }



        public ICollection<POSPurchaseDetail>? Details { get; set; }

    }
}
