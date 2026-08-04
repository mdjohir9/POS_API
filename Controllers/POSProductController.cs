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
    public class POSProductController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private const int userId = 1;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public POSProductController(IUserRepository userRepository, IMemoryCache cache, IUnitOfWork unitOfWork, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _cache = cache;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }



        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> GetProducts(string companyId)
        {
            try
            {
                string cacheKey = "products";

                if (!_cache.TryGetValue(cacheKey, out List<POSProduct> cachedResult))
                {
                    var products = await _unitOfWork.POSCategory.GetByCompanyIdAsync(companyId);

                    if (products == null || !products.Any())
                    {
                        return NotFound(new { StatusCode = 404, message = "customers not found." });
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



    }
}
