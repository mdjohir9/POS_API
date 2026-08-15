using POS_API.Entities;
using POS_API.Entities.Inventory;
using POS_API.Repository;

namespace POS_API.Implementation
{
    public class POSStockLedgerRepository: GenericRepository<POSStockLedger>, IPOSStockLedgerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public POSStockLedgerRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
