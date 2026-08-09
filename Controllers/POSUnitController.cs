using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using POS_API.DTO;
using POS_API.Entities;
using POS_API.Entities.Master;
using POS_API.Repository;
using System.ComponentModel.Design;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class POSUnitController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private const int userId = 1;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public POSUnitController(IUserRepository userRepository, IMemoryCache cache, IUnitOfWork unitOfWork, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _cache = cache;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet]
        [Route("units")]
        public async Task<IActionResult> GetUnits(string companyId)
        {
            try
            {
                string cacheKey = "units";
                if (!_cache.TryGetValue(cacheKey, out List<POSBrand> cachedResult))
                {
                    var users = await _unitOfWork.POSUnit.GetByCompanyIdAsync(companyId);
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
        [Route("unit/create")]
        public async Task<IActionResult> CreateBrand([FromBody] CommonDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { StatusCode = 400, Message = "Invalid request." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var unit = new POSUnit
                {
                    Name = dto.Name,
                    IsActive = dto.IsActive,
                    CreatedAt = DateTime.Now,
                    CreatedBy = userId,
                    CompanyId = 1
                };

                await _unitOfWork.POSUnit.AddAsync(unit);
                await _unitOfWork.Save();

                _cache.Remove("unit");

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "unit created successfully."
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
        [Route("unit/update")]
        public async Task<IActionResult> UpdateUnit([FromBody] CommonDTO dto,long Id)
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
                        Message = "unit not found."
                    });
                }

                brand.Name = dto.Name;
                brand.IsActive = dto.IsActive;
                brand.UpdatedAt = DateTime.Now;
                brand.UpdatedBy = userId;

                _unitOfWork.POSBrand.UpdateAsync(brand);
                await _unitOfWork.Save();

                _cache.Remove("unit");

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "unit updated successfully."
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
        [Route("unit/delete/{id}")]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            try
            {
                // Assuming the user ID of the person performing the delete is stored in the claims
                await _unitOfWork.User.DeleteAsync(id);
                await _unitOfWork.Save();
                string cacheKey = $"unit";
                string cacheKeyID = $"unit{id}";
                _cache.Remove(cacheKeyID);
                _cache.Remove(cacheKey);
                return Ok(new { StatusCode = 200, message = "unit deleted successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { StatusCode = 404, message = ex.Message });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, message = "An error occurred while deleting the unit.", error = ex.Message });
            }

        }

    }
}
