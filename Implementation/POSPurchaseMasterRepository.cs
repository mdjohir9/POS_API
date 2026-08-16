using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using POS_API.DTO;
using POS_API.Entities;
using POS_API.Entities.Inventory;
using POS_API.Entities.Purchase;
using POS_API.Repository;
using System.Data;

namespace POS_API.Implementation
{
    public class POSPurchaseMasterRepository: GenericRepository<POSPurchaseMaster>, IPOSPurchaseMasterRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public POSPurchaseMasterRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

        }

        public async Task<POSPurchaseCreateResultDTO> CreatePurchaseAsync(POSPurchaseMaster purchaseMaster, List<POSPurchaseDetail> details)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                purchaseMaster.Details = details;
                await AddAsync(purchaseMaster);
                await _dbContext.SaveChangesAsync();
                var productGroups = details.GroupBy(x => x.ProductId).Select(g => new { ProductId = g.Key, Quantity = g.Sum(x => x.Quantity) }).ToList();

                foreach (var item in productGroups)
                {
                    var lastLedger = await _dbContext.POS_StockLedgers.Where(x => x.ProductId == item.ProductId).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    decimal currentStock = lastLedger?.BalanceQuantity ?? 0;
                    decimal newBalance = currentStock + item.Quantity;
                    var stockLedger = new POSStockLedger
                    {
                        TransactionDate = purchaseMaster.PurchaseDate,
                        ProductId = item.ProductId,
                        ReferenceType = "PURCHASE",
                        ReferenceId = purchaseMaster.Id,
                        InQuantity = item.Quantity,
                        OutQuantity = 0,
                        BalanceQuantity = newBalance
                    };
                    await _dbContext.POS_StockLedgers.AddAsync(stockLedger);
                }
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new POSPurchaseCreateResultDTO
                {
                    PurchaseId = purchaseMaster.Id,
                    PurchaseNo = purchaseMaster.PurchaseNo,
                    TotalAmount = purchaseMaster.TotalAmount
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<POSPurchaseViewDto>> GetPurchasesFromViewAsync( string companyId)
        {
            var connectionString = _configuration.GetConnectionString(
                "DbConnection"
            );

            using IDbConnection connection =
                new SqlConnection(connectionString);

            const string sql = @"
            SELECT *
            FROM dbo.vw_POSPurchaseDetails
            WHERE CompanyId = @CompanyId
            ORDER BY Id DESC";

            return await connection.QueryAsync<POSPurchaseViewDto>(
                sql,
                new
                {
                    CompanyId = companyId
                });
        }
    }
}
