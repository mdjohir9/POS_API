using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using POS_API.DTO;
using POS_API.Entities;
using POS_API.Entities.Inventory;
using POS_API.Entities.Sales;
using POS_API.Repository;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class POSSalesController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private const int userId = 1;
        private readonly IUnitOfWork _unitOfWork;

        public POSSalesController(IUserRepository userRepository, IMemoryCache cache, IUnitOfWork unitOfWork, ApplicationDbContext dbContext)
        {
            _userRepository = userRepository;
            _cache = cache;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        [Route("sales/invoice/{Id}")]
        public async Task<IActionResult> GetSalesInvoice(int Id)
        {
            try
            {
                var sales = await _unitOfWork.POSSalesMaster.GetSalesInvoiceAsync(Id);

                if (sales == null )
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Sales not found."
                    });
                }

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = sales
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("dashboard/summary")]
        public async Task<IActionResult> GetDashboardSummary(int CompanyId, DateTime Date)
        {
            try
            {

                var result = await _unitOfWork.POSSalesMaster.GetDashboardSummaryAsync(CompanyId , Date);

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("sales-purchase-summary")]
        public async Task<IActionResult> GetSalesPurchaseSummary( string companyId, int year)
        {
            if (string.IsNullOrWhiteSpace(companyId))
                return BadRequest(new { statusCode = 400, message = "CompanyId is required.", data = (object?)null });

            if (year < 2000 || year > 2100)
                return BadRequest(new { statusCode = 400, message = "Invalid year.", data = (object?)null });

            var result = await _unitOfWork.POSSalesMaster.GetSalesPurchaseSummaryAsync(companyId, year);

            return Ok(new
            {
                statusCode = 200,
                message = "Sales and purchase summary retrieved successfully.",
                data = result
            });
        }

        [HttpGet("stock-in-out-summary")]
        public async Task<IActionResult> GetStockInOutSummary([FromQuery] string companyId, [FromQuery] DateTime Date)
        {
            if (string.IsNullOrWhiteSpace(companyId))
                return BadRequest(new
                {
                    statusCode = 400,
                    message = "CompanyId is required.",
                    data = (object?)null
                });

            var result = await _unitOfWork.POSSalesMaster.GetStockInOutSummaryAsync(companyId, Date);

            return Ok(new
            {
                statusCode = 200,
                message = "Stock in and stock out summary retrieved successfully.",
                data = result
            });
        }
        [HttpGet]
        [Route("sales")]
        public async Task<IActionResult> GetSalesList()
        {
            try
            {
                var sales =
                    await _unitOfWork.POSSalesMaster.GetSalesListAsync();

                if (sales == null || !sales.Any())
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Sales not found."
                    });
                }

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = sales
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = ex.Message
                });
            }
        }
        [HttpPost]
        [Route("sales/create")]
        public async Task<IActionResult> CreateSales([FromBody] POSSalesCreateDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Invalid Request."
                    });
                }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (dto.Details == null || !dto.Details.Any())
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Sales details are required."
                    });
                }

                // ==========================================
                // 1. Calculate Gross & Net Amount
                // ==========================================

                decimal grossAmount = dto.Details.Sum(x => x.Amount);

                decimal netAmount = grossAmount - dto.DiscountAmount;

                if (netAmount < 0)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Discount amount cannot be greater than gross amount."
                    });
                }


                // ==========================================
                // 2. Create Sales Master
                // ==========================================

                var salesMaster = new POSSalesMaster
                {
                    InvoiceNo = dto.InvoiceNo,
                    SalesDate = dto.SalesDate,
                    CustomerId = dto.CustomerId,

                    GrossAmount = grossAmount,
                    DiscountAmount = dto.DiscountAmount,
                    NetAmount = netAmount,

                    IsActive = true,
                    IsDeleted = false,

                    CreatedAt = DateTime.Now,
                    CreatedBy = userId
                };


                // ==========================================
                // 3. Create Sales Details
                // ==========================================

                salesMaster.Details = dto.Details
                    .Select(x => new POSSalesDetail
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Rate = x.Rate,
                        Amount = x.Amount
                    })
                    .ToList();


                // ==========================================
                // 4. Create Payments
                // ==========================================

                if (dto.Payments != null && dto.Payments.Any())
                {
                    salesMaster.Payments = dto.Payments
                        .Select(x => new POSSalesPayment
                        {
                            PaymentMethodId = x.PaymentMethodId,
                            Amount = x.Amount
                        })
                        .ToList();
                }


                // ==========================================
                // 5. Save Sales Master
                // ==========================================

                await _unitOfWork.POSSalesMaster.AddAsync(salesMaster);

                await _unitOfWork.Save();


                // ==========================================
                // 6. Group Products
                // ==========================================

                var productGroups = dto.Details
                    .GroupBy(x => x.ProductId)
                    .Select(g => new
                    {
                        ProductId = g.Key,
                        Quantity = g.Sum(x => x.Quantity)
                    })
                    .ToList();


                // ==========================================
                // 7. Stock Ledger Entry
                // ==========================================

                foreach (var item in productGroups)
                {
                    // Get last ledger
                    var lastLedger = await _unitOfWork.POSStockLedger.GetLastLedgerByProductIdAsync(item.ProductId);

                    decimal currentStock = lastLedger?.BalanceQuantity ?? 0;

                    // Sales means stock will decrease
                    decimal newBalance = currentStock - item.Quantity;


                    // ======================================
                    // Stock Validation
                    // ======================================

                    if (newBalance < 0)
                    {
                        return BadRequest(new
                        {
                            StatusCode = 400,
                            Message = $"Insufficient stock for Product ID {item.ProductId}."
                        });
                    }


                    // ======================================
                    // Create Stock Ledger
                    // ======================================

                    var stockLedger = new POSStockLedger
                    {
                        TransactionDate = salesMaster.SalesDate,
                        ProductId = item.ProductId,

                        ReferenceType = "SALES",
                        ReferenceId = salesMaster.Id,

                        InQuantity = 0,
                        OutQuantity = item.Quantity,
                        BalanceQuantity = newBalance
                    };


                    await _unitOfWork.POSStockLedger
                        .AddAsync(stockLedger);
                }


                // ==========================================
                // 8. Save Stock Ledger
                // ==========================================

                await _unitOfWork.Save();


                // ==========================================
                // 9. Response
                // ==========================================

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Sales Created Successfully.",

                    SalesId = salesMaster.Id,
                    InvoiceNo = salesMaster.InvoiceNo,

                    GrossAmount = salesMaster.GrossAmount,
                    DiscountAmount = salesMaster.DiscountAmount,
                    NetAmount = salesMaster.NetAmount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = ex.Message
                });
            }
        }
        //[HttpPost]
        //[Route("sales/create")]
        //public async Task<IActionResult> CreateSales( [FromBody] POSSalesCreateDTO dto)
        //{
        //    try
        //    {
        //        if (dto == null)
        //        {
        //            return BadRequest(new
        //            {
        //                StatusCode = 400,
        //                Message = "Invalid Request."
        //            });
        //        }

        //        if (!ModelState.IsValid)
        //            return BadRequest(ModelState);

        //        if (dto.Details == null || !dto.Details.Any())
        //        {
        //            return BadRequest(new
        //            {
        //                StatusCode = 400,
        //                Message = "Sales details are required."
        //            });
        //        }

        //        // Calculate Gross Amount
        //        decimal grossAmount = dto.Details.Sum(x => x.Amount);

        //        // Calculate Net Amount
        //        decimal netAmount = grossAmount - dto.DiscountAmount;

        //        if (netAmount < 0)
        //        {
        //            return BadRequest(new
        //            {
        //                StatusCode = 400,
        //                Message = "Discount amount cannot be greater than gross amount."
        //            });
        //        }

        //        // Create Sales Master
        //        var salesMaster = new POSSalesMaster
        //        {
        //            InvoiceNo = dto.InvoiceNo,
        //            SalesDate = dto.SalesDate,
        //            CustomerId = dto.CustomerId,

        //            GrossAmount = grossAmount,
        //            DiscountAmount = dto.DiscountAmount,
        //            NetAmount = netAmount,

        //            IsActive = true,
        //            IsDeleted = false,

        //            CreatedAt = DateTime.Now,
        //            CreatedBy = userId
        //        };

        //        // Create Sales Details
        //        salesMaster.Details = dto.Details
        //            .Select(x => new POSSalesDetail
        //            {
        //                ProductId = x.ProductId,
        //                Quantity = x.Quantity,
        //                Rate = x.Rate,
        //                Amount = x.Amount
        //            })
        //            .ToList();

        //        if (dto.Payments != null && dto.Payments.Any())
        //        {
        //            salesMaster.Payments = dto.Payments
        //                .Select(x => new POSSalesPayment
        //                {
        //                    PaymentMethodId = x.PaymentMethodId,
        //                    Amount = x.Amount
        //                })
        //                .ToList();
        //        }

        //        // Save Master + Details + Payments
        //        await _unitOfWork.POSSalesMaster.AddAsync(salesMaster);

        //        await _unitOfWork.Save();

        //        return Ok(new
        //        {
        //            StatusCode = 200,
        //            Message = "Sales Created Successfully.",
        //            SalesId = salesMaster.Id,
        //            InvoiceNo = salesMaster.InvoiceNo,
        //            GrossAmount = salesMaster.GrossAmount,
        //            DiscountAmount = salesMaster.DiscountAmount,
        //            NetAmount = salesMaster.NetAmount

        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            StatusCode = 500,
        //            Message = ex.Message
        //        });
        //    }
        //}



    }
}
