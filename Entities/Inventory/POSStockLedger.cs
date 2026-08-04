using POS_API.Entities.Master;

namespace POS_API.Entities.Inventory
{
    public class POSStockLedger
    {
        public long Id { get; set; }

        public DateTime TransactionDate { get; set; }

        public long ProductId { get; set; }

        public string ReferenceType { get; set; } = string.Empty;

        public long ReferenceId { get; set; }

        public decimal InQuantity { get; set; }

        public decimal OutQuantity { get; set; }

        public decimal BalanceQuantity { get; set; }

        public POSProduct Product { get; set; } = null!;
    }
}
