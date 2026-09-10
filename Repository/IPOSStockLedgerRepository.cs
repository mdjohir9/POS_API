using POS_API.Entities.Inventory;

namespace POS_API.Repository
{
    public interface IPOSStockLedgerRepository: IGenericRepository<POSStockLedger>
    {
        Task<POSStockLedger?> GetLastLedgerByProductIdAsync(long productId);
    }
}
