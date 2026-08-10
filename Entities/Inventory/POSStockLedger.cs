using POS_API.Entities.Master;
using System.ComponentModel.DataAnnotations;

namespace POS_API.Entities.Inventory
{
    public class POSStockLedger
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public DateTime TransactionDate { get; set; }

        public int ProductId { get; set; }

        public string ReferenceType { get; set; } = string.Empty;

        public int ReferenceId { get; set; }

        public decimal InQuantity { get; set; }

        public decimal OutQuantity { get; set; }

        public decimal BalanceQuantity { get; set; }

        public POSProduct Product { get; set; } = null!;
    }
}
