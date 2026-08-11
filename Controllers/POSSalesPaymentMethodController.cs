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
    public class POSSalesPaymentMethodController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private const int userId = 1;
        private readonly IUnitOfWork _unitOfWork;

        public POSSalesPaymentMethodController(IUserRepository userRepository, IMemoryCache cache, IUnitOfWork unitOfWork, ApplicationDbContext dbContext)
        {
            _userRepository = userRepository;
            _cache = cache;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("payment-methods")]
        public async Task<IActionResult> GetPayments(string companyId)
        {
            try
            {
                string cacheKey = "paymentMethods";
                if (!_cache.TryGetValue(cacheKey, out List<POSSalesPaymentMethod> cachedResult))
                {
                    var users = await _unitOfWork.POSSalesPayment.GetByCompanyIdAsync(companyId);
                    if (users == null || !users.Any())
                    {
                        return NotFound(new { StatusCode = 404, message = "Users not found!." });
                    }

                    _cache.Set(cacheKey, users, TimeSpan.FromMinutes(1));
                    return Ok(new { StatusCode = 200, message = "Success", data = users });
                }
                else
                {
                    return Ok(new { StatusCode = 200, message = "Success", data = cachedResult });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, message = "An error occurred", error = ex.Message });
            }
        }

        [HttpPost]
        [Route("payment-method/create")]
        public async Task<IActionResult> CreatePaymentMethod([FromBody] CommonDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { StatusCode = 400, Message = "Invalid request." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var paymentMethod = new POSSalesPaymentMethod
                {
                    Name = dto.Name,
                    IsActive = dto.IsActive,
                    CreatedAt = DateTime.Now,
                    CreatedBy = userId,
                    CompanyId= 1, 
                };

                await _unitOfWork.POSSalesPaymentMethod.AddAsync(paymentMethod);
                await _unitOfWork.Save();

                _cache.Remove("paymentMethods");

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Payment method created successfully."
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
        [Route("payment-method/update")]
        public async Task<IActionResult> UpdatePaymentMethod([FromBody] CommonDTO dto, int Id)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { StatusCode = 400, Message = "Invalid request." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var paymentMethod = await _unitOfWork.POSSalesPaymentMethod.GetByIdAsync(Id);

                if (paymentMethod == null || paymentMethod.IsDeleted)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Payment method not found."
                    });
                }

                paymentMethod.Name = dto.Name;
                paymentMethod.IsActive = dto.IsActive;
                paymentMethod.UpdatedAt = DateTime.Now;
                paymentMethod.UpdatedBy = userId;

                _unitOfWork.POSSalesPaymentMethod.UpdateAsync(paymentMethod);
                await _unitOfWork.Save();

                _cache.Remove("paymentMethods");

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Payment method updated successfully."
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
        [Route("payment-method/delete/{id}")]
        public async Task<IActionResult> DeletePaymentMethod(int id)
        {
            try
            {
                await _unitOfWork.POSSalesPaymentMethod.DeleteAsync(id);
                await _unitOfWork.Save();

                string cacheKey = "paymentMethods";
                string cacheKeyID = $"paymentMethod{id}";

                _cache.Remove(cacheKeyID);
                _cache.Remove(cacheKey);

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Payment method deleted successfully."
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
                    Message = "An error occurred while deleting the payment method.",
                    Error = ex.Message
                });
            }
        }
    }
}
