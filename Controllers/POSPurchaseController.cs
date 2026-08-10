using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using POS_API.DTO;
using POS_API.Entities;
using POS_API.Entities.Master;
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

        [HttpGet]
        [Route("Purchases")]
        public async Task<IActionResult> GetPurchases(string companyId)
        {
            try
            {
                string cacheKey = "Purchases";

                if (!_cache.TryGetValue(cacheKey, out List<POSPurchaseMaster> cachedResult))
                {
                    var Purchases = await _unitOfWork.POSPurchaseMaster.GetByCompanyIdAsync(companyId);

                    if (Purchases == null || !Purchases.Any())
                    {
                        return NotFound(new { StatusCode = 404, message = "customers not found." });
                    }

                    _cache.Set(cacheKey, Purchases, TimeSpan.FromMinutes(1));

                    return Ok(new { StatusCode = 200, message = "Success", data = Purchases });
                }
                return Ok(new { StatusCode = 200, message = "Success", data = cachedResult });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    message = ex.Message
                });
            }
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


        [HttpPut]
        [Route("purchase/update/{id}")]
        public async Task<IActionResult> UpdatePurchase(int id, [FromBody] POSPurchaseCreateDTO dto)
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

                var purchaseMaster = await _unitOfWork.POSPurchaseMaster.GetByIdAsync(id);

                if (purchaseMaster == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Purchase not found."
                    });
                }

                decimal totalAmount = dto.Details.Sum(x => x.Amount);

                purchaseMaster.PurchaseNo = dto.PurchaseNo;
                purchaseMaster.PurchaseDate = dto.PurchaseDate;
                purchaseMaster.SupplierId = dto.SupplierId;
                purchaseMaster.TotalAmount = totalAmount;

                purchaseMaster.UpdatedAt = DateTime.Now;
                purchaseMaster.UpdatedBy = userId;

                _unitOfWork.POSPurchaseMaster.UpdateAsync(purchaseMaster);

                await _unitOfWork.Save();

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Purchase Updated Successfully.",
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

        [HttpDelete]
        [Route("purchase/delete/{id}")]
        public async Task<IActionResult> DeletePurchase(int id)
        {
            try
            {
                await _unitOfWork.POSPurchaseMaster.DeleteAsync(id);
                await _unitOfWork.Save();

                _cache.Remove("Purchase");
                _cache.Remove($"Purchase{id}");

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Purchase Deleted Successfully."
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = ex.Message
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
