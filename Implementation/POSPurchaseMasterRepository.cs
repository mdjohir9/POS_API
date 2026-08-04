using POS_API.Entities;
using POS_API.Entities.Purchase;
using POS_API.Repository;

namespace POS_API.Implementation
{
    public class POSPurchaseMasterRepository: GenericRepository<POSPurchaseMaster>, IPOSPurchaseMasterRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public POSPurchaseMasterRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
