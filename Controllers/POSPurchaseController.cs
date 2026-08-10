using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using POS_API.DTO;
using POS_API.Entities;
using POS_API.Entities.Purchase;
using POS_API.Repository;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class POSPurchaseController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private const int userId = 1;
        private readonly IUnitOfWork _unitOfWork;

        public POSPurchaseController(IUserRepository userRepository, IMemoryCache cache, IUnitOfWork unitOfWork, ApplicationDbContext dbContext)
        {
            _userRepository = userRepository;
            _cache = cache;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("purchase/create")]
        public async Task<IActionResult> CreatePurchase([FromBody] POSPurchaseCreateDTO dto)
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
                        Message = "Purchase details are required."
                    });
                }

                // Calculate total amount
                decimal totalAmount = dto.Details.Sum(x => x.Amount);

                // Create Purchase Master
                var purchaseMaster = new POSPurchaseMaster
                {
                    PurchaseNo = dto.PurchaseNo,
                    PurchaseDate = dto.PurchaseDate,
                    SupplierId = dto.SupplierId,

                    TotalAmount = totalAmount,

                    IsActive = true,
                    IsDeleted = false,

                    CreatedAt = DateTime.Now,
                    CreatedBy = userId
                };

                // Create Purchase Details
                purchaseMaster.Details = dto.Details.Select(x => new POSPurchaseDetail
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Rate = x.Rate,
                    Amount = x.Amount
                }).ToList();

                // Save Master + Details
                await _unitOfWork.POSPurchaseMaster.AddAsync(purchaseMaster);

                await _unitOfWork.Save();

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Purchase Created Successfully.",
                    PurchaseId = purchaseMaster.Id,
                    PurchaseNo = purchaseMaster.PurchaseNo,
                    TotalAmount = purchaseMaster.TotalAmount
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
