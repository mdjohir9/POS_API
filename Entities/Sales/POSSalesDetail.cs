using POS_API.Entities.Master;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS_API.Entities.Sales
{
    public class POSSalesDetail
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public int SalesMasterId { get; set; }

        public int ProductId { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Rate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [ForeignKey(nameof(SalesMasterId))]
        public POSSalesMaster? Sales { get; set; }

        [ForeignKey(nameof(ProductId))]
        public POSProduct? Product { get; set; }
    }
}
