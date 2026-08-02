using POS_API.Entities.Inventory;
using POS_API.Entities.Purchase;
using POS_API.Entities.Sales;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS_API.Entities.Master
{
    public class POS_Product : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string? ProductCode { get; set; }



        [Required]
        [MaxLength(150)]
        public string? ProductName { get; set; }



        [Required]
        public long CategoryId { get; set; }



        [Required]
        public long BrandId { get; set; }



        [Required]
        public long UnitId { get; set; }




        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }



        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesPrice { get; set; }



        [Column(TypeName = "decimal(5,2)")]
        public decimal VATPercent { get; set; }



        [MaxLength(100)]
        public string? Barcode { get; set; }




        // Navigation Property


        [ForeignKey("CategoryId")]
        public POS_Category? Category { get; set; }



        [ForeignKey("BrandId")]
        public POS_Brand? Brand { get; set; }



        [ForeignKey("UnitId")]
        public POS_Unit? Unit { get; set; }
        public ICollection<POS_PurchaseDetail> PurchaseDetails { get; set; } = new List<POS_PurchaseDetail>();

        public ICollection<POS_SalesDetail> SalesDetails { get; set; } = new List<POS_SalesDetail>();

        public ICollection<POS_StockLedger> StockLedgers { get; set; } = new List<POS_StockLedger>();
    }
}
