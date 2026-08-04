using POS_API.Entities.Master;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS_API.Entities.Sales
{
    public class POSSalesMaster : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string? InvoiceNo { get; set; }



        [Required]
        public DateTime SalesDate { get; set; }



        public long CustomerId { get; set; }



        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossAmount { get; set; }



        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }



        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmount { get; set; }




        [ForeignKey("CustomerId")]
        public POSCustomer? Customer { get; set; }



        public ICollection<POSSalesDetail>? Details { get; set; }

    }
}
