using POS_API.Entities;
using POS_API.Entities.Purchase;
using POS_API.Repository;

namespace POS_API.Implementation
{
    public class POSPurchaseDetailRepository: GenericRepository<POSPurchaseDetail>, IPOSPurchaseDetailRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public POSPurchaseDetailRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
