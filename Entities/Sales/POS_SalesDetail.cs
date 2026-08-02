using POS_API.Entities.Master;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS_API.Entities.Sales
{
    public class POS_SalesDetail
    {
        [Key]
        public long Id { get; set; }



        public long SalesMasterId { get; set; }



        public long ProductId { get; set; }



        [Column(TypeName = "decimal(18,3)")]
        public decimal Quantity { get; set; }



        [Column(TypeName = "decimal(18,2)")]
        public decimal Rate { get; set; }



        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }



        [ForeignKey("SalesMasterId")]
        public POS_SalesMaster? Sales { get; set; }



        [ForeignKey("ProductId")]
        public POS_Product? Product { get; set; }
    }
}
