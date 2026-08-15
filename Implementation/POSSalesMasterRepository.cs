using Microsoft.EntityFrameworkCore;
using POS_API.DTO;
using POS_API.Entities;
using POS_API.Entities.Inventory;
using POS_API.Entities.Purchase;
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
        public async Task<IEnumerable<POSSalesListDTO>> GetSalesListAsync()
        {
            var query =
                from sm in _dbContext.POS_SalesMasters

                join c in _dbContext.POS_Customers
                    on sm.CustomerId equals c.Id

                where !sm.IsDeleted

                select new POSSalesListDTO
                {
                    SalesId = sm.Id,

                    InvoiceNo = sm.InvoiceNo,

                    SalesDate = sm.SalesDate,

                    CustomerId = sm.CustomerId,

                    CustomerName = c.CustomerName,

                    ProductName =
                        string.Join(", ",
                            _dbContext.POS_SalesDetails
                                .Where(d =>
                                    d.SalesMasterId == sm.Id)
                                .Join(
                                    _dbContext.POS_Products,
                                    d => d.ProductId,
                                    p => p.Id,
                                    (d, p) => p.ProductName
                                )
                                .Distinct()
                        ),

                    PaymentMethod =
                        string.Join(", ",
                            _dbContext.POS_SalesPayments
                                .Where(p =>
                                    p.SalesMasterId == sm.Id)
                                .Join(
                                    _dbContext.POS_SalesPaymentMethods,
                                    p => p.PaymentMethodId,
                                    pm => pm.Id,
                                    (p, pm) => pm.Name
                                )
                                .Distinct()
                        ),

                    GrossAmount = sm.GrossAmount,

                    DiscountAmount = sm.DiscountAmount,

                    NetAmount = sm.NetAmount
                };

            return await query
                .OrderByDescending(x => x.SalesId)
                .ToListAsync();
        }


    }
}
