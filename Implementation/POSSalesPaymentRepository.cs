using POS_API.Entities;
using POS_API.Entities.Sales;
using POS_API.Repository;

namespace POS_API.Implementation
{
    public class POSSalesPaymentRepository: GenericRepository<POSSalesPayment>, IPOSSalesPaymentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public POSSalesPaymentRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
