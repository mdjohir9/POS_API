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
            POSBrand = new POSBrandRepository(_dbContext, _httpContextAccessor);
            POSCategory = new POSCategoryRepository(_dbContext);
            POSCustomer = new POSCustomerRepository(_dbContext, _httpContextAccessor);
            POSSupplier = new POSSupplierRepository(_dbContext, _httpContextAccessor);
            POSProduct = new POSProductRepository(_dbContext, _httpContextAccessor);
            POSProductBatch = new POSProductBatchRepository(_dbContext, _httpContextAccessor);
            POSPurchaseMaster = new POSPurchaseMasterRepository(_dbContext,_httpContextAccessor);
            POSPurchaseDetail = new POSPurchaseDetailRepository(_dbContext,_httpContextAccessor);
            POSSalesMaster = new POSSalesMasterRepository(_dbContext,_httpContextAccessor);
            POSSalesDetail = new POSSalesDetailRepository(_dbContext ,_httpContextAccessor);
            POSSalesPayment = new POSSalesPaymentRepository(_dbContext, _httpContextAccessor);
            POSSalesPaymentMethod = new POSSalesPaymentMethodRepository(_dbContext, _httpContextAccessor);
            POSStockLedger = new POSStockLedgerRepository(_dbContext, _httpContextAccessor);
            POSUnit = new POSUnitRepository(_dbContext, _httpContextAccessor);

        }


        public IUserRepository User { get; private set; }
        public IUserRoleRepository UserRole { get; private set; }
   
        public ILoginRepository Login { get; private set; }
        public IPOSBrandRepository POSBrand { get; private set; }
        public IPOSCategoryRepository POSCategory { get; private set; }
        public IPOSCustomerRepository POSCustomer { get; private set; }
        public IPOSProductRepository POSProduct { get; private set; }
        public IPOSProductBatchRepository POSProductBatch { get; private set; }
        public IPOSUnitRepository POSUnit { get; private set; }
        public IPOSSupplierRepository POSSupplier { get; private set; }
        public IPOSPurchaseMasterRepository POSPurchaseMaster { get; private set; }
        public IPOSPurchaseDetailRepository POSPurchaseDetail { get; private set; }
        public IPOSSalesMasterRepository POSSalesMaster { get; private set; }
        public IPOSSalesDetailRepository POSSalesDetail { get; private set; }
        public IPOSSalesPaymentRepository POSSalesPayment { get; private set; }
        public IPOSSalesPaymentMethodRepository POSSalesPaymentMethod { get; private set; }
        public IPOSStockLedgerRepository POSStockLedger { get; private set; }


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
