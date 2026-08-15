using POS_API.Entities;
using POS_API.Entities.Master;
using POS_API.Repository;

namespace POS_API.Implementation
{
    public class POSCategoryRepository : GenericRepository<POSCategory>, IPOSCategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        //private static List<PermissionRouteDTO> PermissionList = new List<PermissionRouteDTO>();
        //public List<PermissionRouteDTO> PermissionList { get; set; }
        public POSCategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
