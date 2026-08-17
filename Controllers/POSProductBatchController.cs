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
    public class POSProductBatchController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private const int userId = 1;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public POSProductBatchController(IUserRepository userRepository, IMemoryCache cache, IUnitOfWork unitOfWork, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _cache = cache;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("ProductBatch")]
        public async Task<IActionResult> GetProductBatch(string companyId)
        {
            try
            {
                string cacheKey = "products";

                if (!_cache.TryGetValue(cacheKey, out List<POSProductBatch> cachedResult))
                {
                    var products = await _unitOfWork.POSProductBatch.GetByCompanyIdAsync(companyId);

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
        [Route("productbatch/create")]
        public async Task<IActionResult> CreateProductBatch([FromBody] POSProductBatchDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { StatusCode = 400, Message = "Invalid Request." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var batch = new POSProductBatch
                {
                    ProductId = dto.ProductId,
                    BatchNo = dto.BatchNo,
                    LotNo = dto.LotNo,
                    ManufacturingDate = dto.ManufacturingDate,
                    ExpiryDate = dto.ExpiryDate,
                    PurchasePrice = dto.PurchasePrice,
                    SellingPrice = dto.SellingPrice,
                    ReceiveQty = dto.ReceiveQty,
                    AvailableQty = dto.AvailableQty,
                    IsActive = dto.IsActive,
                    CreatedAt = DateTime.Now,
                    CreatedBy = userId,
                    CompanyId = 1
                };

                await _unitOfWork.POSProductBatch.AddAsync(batch);
                await _unitOfWork.Save();

                _cache.Remove("productbatches");

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Product Batch Created Successfully."
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
        [Route("productbatch/update/{id}")]
        public async Task<IActionResult> UpdateProductBatch([FromBody] POSProductBatchDTO dto, int Id)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { StatusCode = 400, Message = "Invalid Request." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var batch = await _unitOfWork.POSProductBatch.GetByIdAsync(Id);

                if (batch == null || batch.IsDeleted)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Product Batch Not Found."
                    });
                }

                batch.ProductId = dto.ProductId;
                batch.BatchNo = dto.BatchNo;
                batch.LotNo = dto.LotNo;
                batch.ManufacturingDate = dto.ManufacturingDate;
                batch.ExpiryDate = dto.ExpiryDate;
                batch.PurchasePrice = dto.PurchasePrice;
                batch.SellingPrice = dto.SellingPrice;
                batch.ReceiveQty = dto.ReceiveQty;
                batch.AvailableQty = dto.AvailableQty;
                batch.IsActive = dto.IsActive;
                batch.UpdatedAt = DateTime.Now;
                batch.UpdatedBy = userId;

                _unitOfWork.POSProductBatch.UpdateAsync(batch);
                await _unitOfWork.Save();

                _cache.Remove("productbatches");

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Product Batch Updated Successfully."
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
        [Route("productbatch/delete/{id}")]
        public async Task<IActionResult> DeleteProductBatch(int id)
        {
            try
            {
                await _unitOfWork.POSProductBatch.DeleteAsync(id);
                await _unitOfWork.Save();

                _cache.Remove("productbatches");
                _cache.Remove($"productbatch{id}");

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Product Batch Deleted Successfully."
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
