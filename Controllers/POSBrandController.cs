using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public POSBrandController(IUserRepository userRepository, IMemoryCache cache, IUnitOfWork unitOfWork, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _cache = cache;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
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
    }
}
