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
    public class POSCategoryController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private const int userId = 1;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public POSCategoryController(IUserRepository userRepository, IMemoryCache cache, IUnitOfWork unitOfWork, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _cache = cache;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("categories")]
        public async Task<IActionResult> GetCategories(string companyId)
        {
            try
            {
                string cacheKey = "categories";

                if (!_cache.TryGetValue(cacheKey, out List<POSCategory> cachedResult))
                {
                    var categories = await _unitOfWork.POSCategory.GetByCompanyIdAsync(companyId);

                    if (categories == null || !categories.Any())
                    {
                        return NotFound(new
                        {
                            StatusCode = 404,
                            message = "Category not found."
                        });
                    }

                    _cache.Set(cacheKey, categories, TimeSpan.FromMinutes(1));

                    return Ok(new
                    {
                        StatusCode = 200,
                        message = "Success",
                        data = categories
                    });
                }

                return Ok(new
                {
                    StatusCode = 200,
                    message = "Success",
                    data = cachedResult
                });
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
        [Route("category/create")]
        public async Task<IActionResult> CreateCategory([FromBody] CommonDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { StatusCode = 400, Message = "Invalid request." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var category = new POSCategory
                {
                    Name = dto.Name,
                    IsActive = dto.IsActive,
                    CreatedAt = DateTime.Now,
                    CreatedBy = userId,
                    CompanyId = "1111"
                };

                await _unitOfWork.POSCategory.AddAsync(category);
                await _unitOfWork.Save();

                _cache.Remove("categories");

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Category created successfully."
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
        [Route("category/update")]
        public async Task<IActionResult> UpdateCategory([FromBody] CommonDTO dto, long Id)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { StatusCode = 400, Message = "Invalid request." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var category = await _unitOfWork.POSCategory.GetByIdAsync(Id);

                if (category == null || category.IsDeleted)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Category not found."
                    });
                }

                category.Name = dto.Name;
                category.IsActive = dto.IsActive;
                category.UpdatedAt = DateTime.Now;
                category.UpdatedBy = userId;

                _unitOfWork.POSCategory.UpdateAsync(category);
                await _unitOfWork.Save();

                _cache.Remove("categories");

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Category updated successfully."
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
        [Route("category/delete/{id}")]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            try
            {
                await _unitOfWork.POSCategory.DeleteAsync(id);
                await _unitOfWork.Save();

                _cache.Remove("categories");
                _cache.Remove($"category{id}");

                return Ok(new
                {
                    StatusCode = 200,
                    message = "Category deleted successfully."
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    message = "An error occurred while deleting the category.",
                    error = ex.Message
                });
            }
        }
    }
}
