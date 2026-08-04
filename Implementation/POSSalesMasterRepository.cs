using POS_API.Entities;
using POS_API.Entities.Sales;
using POS_API.Repository;

namespace POS_API.Implementation
{
    public class POSSalesMasterRepository: GenericRepository<POSSalesMaster>, IPOSSalesMasterRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public POSSalesMasterRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
