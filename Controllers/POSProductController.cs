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
                    var products = await _unitOfWork.POSProduct.GetByCompanyIdAsync(companyId);

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

        [HttpPost]
        [Route("product/create")]
        public async Task<IActionResult> CreateProduct([FromBody] POSProductDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { StatusCode = 400, Message = "Invalid Request." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var product = new POSProduct
                {
                    ProductCode = dto.ProductCode,
                    ProductName = dto.ProductName,
                    CategoryId = dto.CategoryId,
                    BrandId = dto.BrandId,
                    UnitId = dto.UnitId,
                    PurchasePrice = dto.PurchasePrice,
                    SalesPrice = dto.SalesPrice,
                    VATPercent = dto.VATPercent,
                    Barcode = dto.Barcode,
                    IsBatchRequired = dto.IsBatchRequired,
                    IsActive = dto.IsActive,
                    CreatedAt = DateTime.Now,
                    CreatedBy = userId,
                    CompanyId = "1111"
                };

                await _unitOfWork.POSProduct.AddAsync(product);
                await _unitOfWork.Save();

                _cache.Remove("products");

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Product Created Successfully."
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
        [Route("product/update")]
        public async Task<IActionResult> UpdateProduct([FromBody] POSProductDTO dto, long Id)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { StatusCode = 400, Message = "Invalid Request." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var product = await _unitOfWork.POSProduct.GetByIdAsync(Id);

                if (product == null || product.IsDeleted)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Product Not Found."
                    });
                }

                product.ProductCode = dto.ProductCode;
                product.ProductName = dto.ProductName;
                product.CategoryId = dto.CategoryId;
                product.BrandId = dto.BrandId;
                product.UnitId = dto.UnitId;
                product.PurchasePrice = dto.PurchasePrice;
                product.SalesPrice = dto.SalesPrice;
                product.VATPercent = dto.VATPercent;
                product.Barcode = dto.Barcode;
                product.IsBatchRequired = dto.IsBatchRequired;
                product.IsActive = dto.IsActive;
                product.UpdatedAt = DateTime.Now;
                product.UpdatedBy = userId;

                _unitOfWork.POSProduct.UpdateAsync(product);
                await _unitOfWork.Save();

                _cache.Remove("products");

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Product Updated Successfully."
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
        [Route("product/delete/{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            try
            {
                await _unitOfWork.POSProduct.DeleteAsync(id);
                await _unitOfWork.Save();

                _cache.Remove("products");
                _cache.Remove($"product{id}");

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Product Deleted Successfully."
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
