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
    public class POSBrandController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private const int userId = 1;
        private readonly IUnitOfWork _unitOfWork;

        public POSBrandController(IUserRepository userRepository, IMemoryCache cache, IUnitOfWork unitOfWork, ApplicationDbContext dbContext)
        {
            _userRepository = userRepository;
            _cache = cache;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("brand")]
        public async Task<IActionResult> GetBrand(int Id)
        {
            try
            {
                string cacheKey = "users";
                if (!_cache.TryGetValue(cacheKey, out List<POSBrand> cachedResult))
                {
                    var users = await _unitOfWork.POSBrand.GetByIdAsync(Id);
                    if (users == null)
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

        [HttpGet]
        [Route("brands")]
        public async Task<IActionResult> GetBrands(string companyId)
        {
            try
            {
                string cacheKey = "users";
                if (!_cache.TryGetValue(cacheKey, out List<POSBrand> cachedResult))
                {
                    var users = await _unitOfWork.POSBrand.GetByCompanyIdAsync(companyId);
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
        [Route("brand/create")]
        public async Task<IActionResult> CreateBrand([FromBody] POSBrandDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { StatusCode = 400, Message = "Invalid request." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var brand = new POSBrand
                {
                    Name = dto.Name,
                    IsActive = dto.IsActive,
                    CreatedAt = DateTime.Now,
                    CreatedBy = userId,
                    CompanyId = 1
                };

                await _unitOfWork.POSBrand.AddAsync(brand);
                await _unitOfWork.Save();

                _cache.Remove("brands");

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Brand created successfully."
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
        [Route("brand/update")]
        public async Task<IActionResult> UpdateBrand([FromBody] POSBrandDTO dto, int Id)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { StatusCode = 400, Message = "Invalid request." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var brand = await _unitOfWork.POSBrand.GetByIdAsync(Id);

                if (brand == null || brand.IsDeleted)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Brand not found."
                    });
                }

                brand.Name = dto.Name;
                brand.IsActive = dto.IsActive;
                brand.UpdatedAt = DateTime.Now;
                brand.UpdatedBy = userId;

                _unitOfWork.POSBrand.UpdateAsync(brand);
                await _unitOfWork.Save();

                _cache.Remove("brands");

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Brand updated successfully."
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
        [Route("brand/delete/{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            try
            {
                // Assuming the user ID of the person performing the delete is stored in the claims
                await _unitOfWork.POSBrand.DeleteAsync(id);
                await _unitOfWork.Save();
                string cacheKey = $"users";
                string cacheKeyID = $"user{id}";
                _cache.Remove(cacheKeyID);
                _cache.Remove(cacheKey);
                return Ok(new { StatusCode = 200, message = "User deleted successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { StatusCode = 404, message = ex.Message });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, message = "An error occurred while deleting the user.", error = ex.Message });
            }

        }

    }
}
