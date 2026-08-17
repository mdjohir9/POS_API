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
    public class POSSupplierController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private const int userId = 1;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public POSSupplierController(IUserRepository userRepository, IMemoryCache cache, IUnitOfWork unitOfWork, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _cache = cache;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        [Route("suppliers")]
        public async Task<IActionResult> GetProducts(string companyId)
        {
            try
            {
                string cacheKey = "suppliers";

                if (!_cache.TryGetValue(cacheKey, out List<POSSupplier> cachedResult))
                {
                    var products = await _unitOfWork.POSSupplier.GetByCompanyIdAsync(companyId);

                    if (products == null || !products.Any())
                    {
                        return NotFound(new { StatusCode = 404, message = "suppliers not found." });
                    }

                    _cache.Set(cacheKey, products, TimeSpan.FromMinutes(1));

                    return Ok(new { StatusCode = 200, message = "Success", data = products });
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
        [Route("supplier/create")]
        public async Task<IActionResult> CreateSupplier([FromBody] POSSupplierDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { StatusCode = 400, Message = "Invalid request." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var supplier = new POSSupplier
                {
                    SupplierCode = dto.SupplierCode,
                    SupplierName = dto.SupplierName,
                    Phone = dto.Phone,
                    Address = dto.Address,
                    IsActive = dto.IsActive,
                    CreatedAt = DateTime.Now,
                    CreatedBy = userId,
                    CompanyId = 1
                };

                await _unitOfWork.POSSupplier.AddAsync(supplier);
                await _unitOfWork.Save();

                _cache.Remove("suppliers");

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Supplier created successfully."
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
        [Route("supplier/update/{id}")]
        public async Task<IActionResult> UpdateSupplier([FromBody] POSSupplierDTO dto, int Id)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { StatusCode = 400, Message = "Invalid request." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var supplier = await _unitOfWork.POSSupplier.GetByIdAsync(Id);

                if (supplier == null || supplier.IsDeleted)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Supplier not found."
                    });
                }

                supplier.SupplierCode = dto.SupplierCode;
                supplier.SupplierName = dto.SupplierName;
                supplier.Phone = dto.Phone;
                supplier.Address = dto.Address;
                supplier.IsActive = dto.IsActive;
                supplier.UpdatedAt = DateTime.Now;
                supplier.UpdatedBy = userId;

                _unitOfWork.POSSupplier.UpdateAsync(supplier);
                await _unitOfWork.Save();

                _cache.Remove("suppliers");

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Supplier updated successfully."
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
        [Route("supplier/delete/{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            try
            {
                await _unitOfWork.POSSupplier.DeleteAsync(id);
                await _unitOfWork.Save();

                _cache.Remove("suppliers");
                _cache.Remove($"supplier{id}");

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Supplier deleted successfully."
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
