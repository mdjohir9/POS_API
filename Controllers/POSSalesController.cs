using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using POS_API.DTO;
using POS_API.Entities;
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
        public async Task<IActionResult> CreateSales( [FromBody] POSSalesCreateDTO dto)
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

                // Calculate Gross Amount
                decimal grossAmount = dto.Details.Sum(x => x.Amount);

                // Calculate Net Amount
                decimal netAmount = grossAmount - dto.DiscountAmount;

                if (netAmount < 0)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Discount amount cannot be greater than gross amount."
                    });
                }

                // Create Sales Master
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

                // Create Sales Details
                salesMaster.Details = dto.Details
                    .Select(x => new POSSalesDetail
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Rate = x.Rate,
                        Amount = x.Amount
                    })
                    .ToList();

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

                // Save Master + Details + Payments
                await _unitOfWork.POSSalesMaster.AddAsync(salesMaster);

                await _unitOfWork.Save();

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
    }
}
