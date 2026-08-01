using POS_API.Entities;
using POS_API.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using POS_API.Implementation;

namespace POS_API.Implementation
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

      
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public UnitOfWork(ApplicationDbContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _connectionString = _configuration.GetConnectionString("DbConnection");


            User = new UserRepository(_dbContext, _httpContextAccessor);
            UserRole = new UserRoleRepository(_dbContext);
            Login = new LoginRepository(_dbContext, _configuration);

        }


        public IUserRepository User { get; private set; }
        public IUserRoleRepository UserRole { get; private set; }
   
        public ILoginRepository Login { get; private set; }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public Task<int> Save()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
