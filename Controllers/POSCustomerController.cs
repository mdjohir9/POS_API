using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using POS_API.DTO;
using POS_API.Entities;
using POS_API.Entities.Master;
using POS_API.Repository;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class POSCustomerController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private const int userId = 1;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public POSCustomerController(IUserRepository userRepository, IMemoryCache cache, IUnitOfWork unitOfWork, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _cache = cache;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("customers")]
        public async Task<IActionResult> GetCustomers(string companyId)
        {
            try
            {
                string cacheKey = "customers";

                if (!_cache.TryGetValue(cacheKey, out List<POSCustomer> cachedResult))
                {
                    var categories = await _unitOfWork.POSCustomer.GetByCompanyIdAsync(companyId);

                    if (categories == null || !categories.Any())
                    {
                        return NotFound(new  {  StatusCode = 404,  message = "customers not found." });
                    }

                    _cache.Set(cacheKey, categories, TimeSpan.FromMinutes(1));

                    return Ok(new  {  StatusCode = 200, message = "Success", data = categories });
                }

                return Ok(new {  StatusCode = 200, message = "Success", data = cachedResult });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {  StatusCode = 500, message = ex.Message  });
            }
        }

        [HttpPost]
        [Route("customer/create")]
        public async Task<IActionResult> CreateCustomer([FromBody] POSCustomerDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { StatusCode = 400, Message = "Invalid request." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var customer = new POSCustomer
                {
                    CustomerCode = dto.CustomerCode,
                    CustomerName = dto.CustomerName,
                    Phone = dto.Phone,
                    Address = dto.Address,
                    IsActive = dto.IsActive,
                    CreatedAt = DateTime.Now,
                    CreatedBy = userId,
                    CompanyId = 1
                };

                await _unitOfWork.POSCustomer.AddAsync(customer);
                await _unitOfWork.Save();

                _cache.Remove("customers");

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Customer created successfully."
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
        [Route("customer/update/{id}")]
        public async Task<IActionResult> UpdateCustomer([FromBody] POSCustomerDTO dto, int Id)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { StatusCode = 400, Message = "Invalid request." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var customer = await _unitOfWork.POSCustomer.GetByIdAsync(Id);

                if (customer == null || customer.IsDeleted)
                {
                    return NotFound(new {  StatusCode = 404,  Message = "Customer not found."});
                }

                customer.CustomerCode = dto.CustomerCode;
                customer.CustomerName = dto.CustomerName;
                customer.Phone = dto.Phone;
                customer.Address = dto.Address;
                customer.IsActive = dto.IsActive;
                customer.UpdatedAt = DateTime.Now;
                customer.UpdatedBy = userId;

                _unitOfWork.POSCustomer.UpdateAsync(customer);
                await _unitOfWork.Save();

                _cache.Remove("customers");
                return Ok(new {  StatusCode = 200,  Message = "Customer updated successfully."});
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
        [Route("customer/delete/{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                await _unitOfWork.POSCustomer.DeleteAsync(id);
                await _unitOfWork.Save();
                _cache.Remove("customers");
                _cache.Remove($"customer{id}");

                return Ok(new  { StatusCode = 200,  message = "Customer deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { StatusCode = 404, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new  { StatusCode = 500,  message = ex.Message });
            }
        }
    }
}
