using POS_API.Entities.Master;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS_API.Entities.Sales
{
    public class POSSalesMaster
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
        public string? InvoiceNo { get; set; }

        [Required]
        public DateTime SalesDate { get; set; }

        public int CustomerId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmount { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public POSCustomer? Customer { get; set; }

        public ICollection<POSSalesDetail> Details { get; set; }
            = new List<POSSalesDetail>();
        public ICollection<POSSalesPayment> Payments { get; set; } = new List<POSSalesPayment>();
    }
}
