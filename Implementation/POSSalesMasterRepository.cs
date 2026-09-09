using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using POS_API.DTO;
using POS_API.Entities;
using POS_API.Entities.Inventory;
using POS_API.Entities.Purchase;
using POS_API.Entities.Sales;
using POS_API.Repository;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace POS_API.Implementation
{
    public class POSSalesMasterRepository : GenericRepository<POSSalesMaster>, IPOSSalesMasterRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public POSSalesMasterRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
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

        public async Task<POSSalesInvoiceDTO?> GetSalesInvoiceAsync(int salesMasterId)
        {
            var result = await _dbContext.POS_SalesMasters.AsNoTracking() .Where(x => x.Id == salesMasterId
                            && x.IsActive
                            && !x.IsDeleted)
                .Select(x => new POSSalesInvoiceDTO
                {
                    Id = x.Id,
                    InvoiceNo = x.InvoiceNo,
                    SalesDate = x.SalesDate,

                    CustomerId = x.CustomerId,
                    CustomerName = x.Customer != null
                        ? x.Customer.CustomerName
                        : "Walk-in Customer",

                    SubTotal = x.GrossAmount,
                    DiscountAmount = x.DiscountAmount,
                    GrandTotal = x.NetAmount,

                    Details = x.Details
                        .Select(d => new POSSalesDetailCreateDTO
                        {
                            SalesMasterId = d.SalesMasterId,
                            ProductId = d.ProductId,
                            ProductName = d.Product != null ? d.Product.ProductName : "",
                            Quantity = d.Quantity,
                            Rate = d.Rate,
                            Amount = d.Amount
                        }).ToList(),

                    Payments = x.Payments
                        .Select(p => new POSSalesPaymentCreateDTO
                        {

                            PaymentMethodId = p.PaymentMethodId,

                            PaymentMethodName = p.PaymentMethod != null? p.PaymentMethod.Name  : "",
                            Amount = p.Amount
                        })
                        .ToList()

                })
                .FirstOrDefaultAsync();

            return result;
        }


        public async Task<POSDashboardSummaryDTO> GetDashboardSummaryAsync(int companyId, DateTime date)
        {
            var today = date;
            var tomorrow = today.AddDays(1);
            var yesterday = today.AddDays(-1);
            var todaySales = await _dbContext.POS_SalesMasters
                .AsNoTracking()
                .Where(x =>
                    x.IsActive &&
                    !x.IsDeleted &&
                    x.SalesDate >= today &&
                    x.SalesDate < tomorrow)
                .SumAsync(x => (decimal?)x.NetAmount) ?? 0;
            var yesterdaySales = await _dbContext.POS_SalesMasters
                .AsNoTracking()
                .Where(x =>
                    x.IsActive &&
                    !x.IsDeleted &&
                    x.SalesDate >= yesterday &&
                    x.SalesDate < today)
                .SumAsync(x => (decimal?)x.NetAmount) ?? 0;


            // Sales Growth
            decimal salesGrowth = 0;

            if (yesterdaySales > 0)
            {
                salesGrowth =
                    ((todaySales - yesterdaySales) / yesterdaySales) * 100;
            }
            var todayPurchase = await _dbContext.POS_PurchaseMasters
                .AsNoTracking()
                .Where(x =>
                    x.IsActive &&
                    !x.IsDeleted &&
                    x.PurchaseDate >= today &&
                    x.PurchaseDate < tomorrow)
                .SumAsync(x => (decimal?)x.TotalAmount) ?? 0;

            var yesterdayPurchase = await _dbContext.POS_PurchaseMasters
                .AsNoTracking()
                .Where(x =>
                    x.IsActive &&
                    !x.IsDeleted &&
                    x.PurchaseDate >= yesterday &&
                    x.PurchaseDate < today)
                .SumAsync(x => (decimal?)x.TotalAmount) ?? 0;


            // Purchase Growth
            decimal purchaseGrowth = 0;

            if (yesterdayPurchase > 0)
            {
                purchaseGrowth =
                    ((todayPurchase - yesterdayPurchase) / yesterdayPurchase) * 100;
            }

            var todayProfit = await _dbContext.POS_SalesDetails
                .AsNoTracking()
                .Where(x =>
                    x.Sales != null &&
                    x.Sales.IsActive &&
                    !x.Sales.IsDeleted &&
                    x.Sales.SalesDate >= today &&
                    x.Sales.SalesDate < tomorrow)
                .SumAsync(x =>
                    (decimal?)
                    (
                        x.Amount -
                        (
                            x.Product != null
                                ? x.Product.PurchasePrice * x.Quantity
                                : 0
                        )
                    )
                ) ?? 0;

            var yesterdayProfit = await _dbContext.POS_SalesDetails
                .AsNoTracking()
                .Where(x =>
                    x.Sales != null &&
                    x.Sales.IsActive &&
                    !x.Sales.IsDeleted &&
                    x.Sales.SalesDate >= yesterday &&
                    x.Sales.SalesDate < today)
                .SumAsync(x =>
                    (decimal?)
                    (
                        x.Amount -
                        (
                            x.Product != null
                                ? x.Product.PurchasePrice * x.Quantity
                                : 0
                        )
                    )
                ) ?? 0;


            // Profit Growth
            decimal profitGrowth = 0;

            if (yesterdayProfit != 0)
            {
                profitGrowth =
                    ((todayProfit - yesterdayProfit)
                    / Math.Abs(yesterdayProfit)) * 100;
            }

            var lowStockItems = await _dbContext.POS_StockLedgers
                .AsNoTracking()
                .GroupBy(x => x.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,

                    CurrentStock =
                        g.Sum(x => x.InQuantity)
                        - g.Sum(x => x.OutQuantity)
                })
                .Join(
                    _dbContext.POS_Products
                        .AsNoTracking()
                        .Where(x =>
                            x.IsActive &&
                            !x.IsDeleted),

                    stock => stock.ProductId,

                    product => product.Id,

                    (stock, product) => new
                    {
                        stock.CurrentStock,

                        // Minimum stock level
                        MinimumStock = 10
                    })
                .CountAsync(x =>
                    x.CurrentStock <= x.MinimumStock);

            var result = new POSDashboardSummaryDTO
            {
                Sales = new POSDashboardCardDTO
                {
                    Value = Math.Round(todaySales, 2),

                    Growth = Math.Round(
                        Math.Abs(salesGrowth),
                        2),

                    IsUp = todaySales >= yesterdaySales,

                    Message = "Compared to yesterday"
                },
                Purchase = new POSDashboardCardDTO
                {
                    Value = Math.Round(todayPurchase, 2),

                    Growth = Math.Round(
                        Math.Abs(purchaseGrowth),
                        2),

                    IsUp = todayPurchase >= yesterdayPurchase,

                    Message = "Compared to yesterday"
                },

                Profit = new POSDashboardCardDTO
                {
                    Value = Math.Round(todayProfit, 2),

                    Growth = Math.Round(
                        Math.Abs(profitGrowth),
                        2),

                    IsUp = todayProfit >= yesterdayProfit,

                    Message = "Compared to yesterday"
                },
                LowStockItems = lowStockItems
            };

            return result;
        }


        public async Task<SalesPurchaseSummaryDTO> GetSalesPurchaseSummaryAsync(string companyId, int year)
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = startDate.AddYears(1);

            var sales = await _dbContext.POS_SalesMasters
                .Where(x => 
                            x.SalesDate >= startDate &&
                            x.SalesDate < endDate &&
                            !x.IsDeleted)
                .Select(x => new
                {
                    x.SalesDate,
                    x.NetAmount
                })
                .ToListAsync();

            var purchases = await _dbContext.POS_PurchaseMasters
                .Where(x => 
                            x.PurchaseDate >= startDate &&
                            x.PurchaseDate < endDate &&
                            !x.IsDeleted)
                .Select(x => new
                {
                    x.PurchaseDate,
                    x.TotalAmount
                })
                .ToListAsync();

            var result = new SalesPurchaseSummaryDTO
            {
                SalesAmount = sales.Sum(x => x.NetAmount),
                PurchaseAmount = purchases.Sum(x => x.TotalAmount),

                MonthlySalesAmounts = Enumerable.Range(1, 12)
                    .Select(month => sales
                        .Where(x => x.SalesDate.Month == month)
                        .Sum(x => x.NetAmount))
                    .ToArray(),

                MonthlyPurchaseAmounts = Enumerable.Range(1, 12)
                    .Select(month => purchases
                        .Where(x => x.PurchaseDate.Month == month)
                        .Sum(x => x.TotalAmount))
                    .ToArray()
            };

            return result;
        }



        public async Task<StockInOutSummaryDTO> GetStockInOutSummaryAsync(string companyId, DateTime selectedDate)
        {
            var endDate = selectedDate.Date.AddDays(1);
            var startDate = selectedDate.Date.AddDays(-6);

            var ledgerData = await _dbContext.POS_StockLedgers
                .Where(x => 
                            x.TransactionDate >= startDate &&
                            x.TransactionDate < endDate)
                .Select(x => new
                {
                    x.TransactionDate,
                    x.InQuantity,
                    x.OutQuantity
                })
                .ToListAsync();

            var result = new StockInOutSummaryDTO();

            for (int i = 0; i < 7; i++)
            {
                var date = startDate.AddDays(i);

                result.Labels.Add(date.ToString("ddd"));

                result.StockInData.Add(
                    ledgerData
                        .Where(x => x.TransactionDate.Date == date.Date)
                        .Sum(x => x.InQuantity)
                );

                result.StockOutData.Add(
                    ledgerData
                        .Where(x => x.TransactionDate.Date == date.Date)
                        .Sum(x => x.OutQuantity)
                );
            }

            result.StockInTotal = result.StockInData.Sum();
            result.StockOutTotal = result.StockOutData.Sum();

            return result;
        }
    } 


        
}
