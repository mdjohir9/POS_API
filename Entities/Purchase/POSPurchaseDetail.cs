using POS_API.Entities.Master;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS_API.Entities.Purchase
{
    public class POSPurchaseDetail
    {
        [Key]
        public long Id { get; set; }



        [Required]
        public long PurchaseMasterId { get; set; }



        [Required]
        public long ProductId { get; set; }



        [Column(TypeName = "decimal(18,3)")]
        public decimal Quantity { get; set; }



        [Column(TypeName = "decimal(18,2)")]
        public decimal Rate { get; set; }



        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }



        [ForeignKey("PurchaseMasterId")]
        public POSPurchaseMaster? Purchase { get; set; }



        [ForeignKey("ProductId")]
        public POSProduct? Product { get; set; }
    }
}
