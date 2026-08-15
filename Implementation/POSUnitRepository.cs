using POS_API.Entities;
using POS_API.Entities.Master;
using POS_API.Repository;

namespace POS_API.Implementation
{
    public class POSUnitRepository : GenericRepository<POSUnit>, IPOSUnitRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public POSUnitRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
