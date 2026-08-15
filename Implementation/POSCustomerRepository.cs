using POS_API.Entities;
using POS_API.Entities.Master;
using POS_API.Repository;

namespace POS_API.Implementation
{
    public class POSCustomerRepository : GenericRepository<POSCustomer>, IPOSCustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private static List<PermissionRouteDTO> PermissionList = new List<PermissionRouteDTO>();
        //public List<PermissionRouteDTO> PermissionList { get; set; }
        public POSCustomerRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
