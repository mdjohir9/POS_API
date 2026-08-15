using POS_API.DTO;
using POS_API.Entities;
using POS_API.Repository;
using Microsoft.EntityFrameworkCore;
using System.IO;
using POS_API.Implementation;
using POS_API.Entities.Master;
namespace POS_API.Implementation
{
    public class POSBrandRepository : GenericRepository<POSBrand>, IPOSBrandRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public POSBrandRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
