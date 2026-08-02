using POS_API.DTO.UserRoles;
using POS_API.Entities;
using POS_API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserRolesController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly IUnitOfWork _unitOfWork;
        int userId = 1;
        public UserRolesController(ApplicationDbContext dbContext, IUnitOfWork unitOfWork, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
            _unitOfWork = unitOfWork;

        }

        [HttpGet]
        [Route("userRoles")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> GetUserRolesAsync(string companyId, bool IsAdministrator)
        {
            try
            {
                var userRoles = await _unitOfWork.UserRole.GelAllUserRolesAsync(companyId, IsAdministrator);
                if (userRoles == null)
                {
                    return NotFound(new { StatusCode = 404, message = "User Roles not found !." });
                }


                return Ok(new { StatusCode = 200, message = "Success", data = userRoles });


            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { StatusCode = 404, message = "User Role not found!" });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { StatusCode = 500, message = "An error occurred", error = ex.Message });

            }
        }

        [HttpGet]
        [Route("userRolesWithGuestUser")]
        public async Task<IActionResult> GetRolesWithGuestUserValidationAsync(bool IsGuestUser, string CompanyId)
        {
            try
            {
                if (_dbContext == null)
                {
                    return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });
                }

                var userRoles = await _unitOfWork.UserRole.GetUserRolesAsync(IsGuestUser, CompanyId);


                if (userRoles == null)
                {
                    return NotFound(new { StatusCode = 404, message = "User Roles not found !." });
                }


                return Ok(new { StatusCode = 200, message = "Success", data = userRoles });


            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { StatusCode = 404, message = "User Role not found!" });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { StatusCode = 500, message = "An error occurred", error = ex.Message });

            }
        }



        [HttpGet]
        [Route("userRoles/{Id}")]
        public async Task<IActionResult> GetUserRole(int Id)
        {
            try
            {
                string cacheKey = $"userRoles{Id}";

                // Try to get the cached result
                if (!_cache.TryGetValue(cacheKey, out UserRole cachedResult))
                {
                    // Await the asynchronous call to get the module
                    var result = await _unitOfWork.UserRole.GetByIdAsync(Id);

                    // The GetByIdAsync method throws a KeyNotFoundException if the entity is not found
                    if (result == null)
                    {
                        return NotFound(new { StatusCode = 404, message = "Module Data Not Found" });
                    }

                    // Cache the result
                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(1));

                    return Ok(new { StatusCode = 200, message = "Success", data = result });
                }
                else
                {
                    return Ok(new { StatusCode = 200, message = "Success", data = cachedResult });
                }
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { StatusCode = 404, message = "User module not found!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, message = "An error occurred", error = ex.Message });
            }
        }


        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> PostUserRoles([FromBody] UserRolesDTO userRolesDTO)
        {
            try
            {

                if (userRolesDTO == null)
                {
                    return BadRequest(new { StatusCode = 400, message = "User Permission object is null." });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Create the UserRoles object
                var userRoles = new UserRole
                {
                    UserRoleName = userRolesDTO.UserRole,
                    CompanyId = "1111",
                    Permissions = userRolesDTO.Permissions,
                    DataAccessLevel = userRolesDTO.DataAccessLevel,
                    Ordering = userRolesDTO.Ordering,
                    IsActive = userRolesDTO.IsActive,
                    CreatedAt = DateTime.Now,
                    CreatedBy = userId,
                };

                await _unitOfWork.UserRole.AddAsync(userRoles);
                await _unitOfWork.Save();
                string cacheKey = $"userRoles";
                _cache.Remove(cacheKey);

                return Ok(new { StatusCode = 200, message = "User Roles Create Successfull" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut]
        [Route("update/{Id}")]
        public async Task<IActionResult> UpdateUserRoles(int Id, [FromBody] UserRolesDTO userRolesDto)
        {
            try
            {
                if (userRolesDto == null)
                {
                    return BadRequest(new { StatusCode = 400, message = "User Roles object is null." });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingUserRole = await _unitOfWork.UserRole.GetByIdAsync(Id);

                if (existingUserRole == null)
                {
                    return NotFound(new { StatusCode = 404, message = "User Roles not found." });
                }

                existingUserRole.UserRoleName = userRolesDto.UserRole;
                existingUserRole.CompanyId = "1111";
                existingUserRole.Permissions = userRolesDto.Permissions;
                existingUserRole.DataAccessLevel = userRolesDto.DataAccessLevel;
                existingUserRole.Ordering = userRolesDto.Ordering;
                existingUserRole.IsActive = userRolesDto.IsActive;
                existingUserRole.UpdatedAt = DateTime.Now;
                existingUserRole.UpdatedBy = userId;

                await _unitOfWork.UserRole.UpdateAsync(existingUserRole);
                await _unitOfWork.Save();
                string cacheKey = $"userRoles";
                _cache.Remove(cacheKey);
                string cacheKeyID = $"userRoles{Id}";
                _cache.Remove(cacheKeyID);
                return Ok(new { StatusCode = 200, message = "User Roles updated successfully." });
            }
            catch (KeyNotFoundException)
            {
                return StatusCode(404, new { StatusCode = 500, message = "User Roles Not Found!" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, message = "An error occurred while updating the user roles.", error = ex.Message });
            }
        }


        [HttpDelete]
        [Route("delete/{Id}")]
        public async Task<IActionResult> DeleteUserRole(int Id)
        {
            try
            {
                await _unitOfWork.UserRole.DeleteAsync(Id);
                await _unitOfWork.Save();
                string cacheKey = $"userRoles";
                _cache.Remove(cacheKey);
                string cacheKeyID = $"userRoles{Id}";
                _cache.Remove(cacheKeyID);

                return Ok(new { StatusCode = 200, message = "User Role delete successfully." });
            }
            catch (KeyNotFoundException)
            {
                return StatusCode(404, new { StatusCode = 404, message = "User Role Not Found!" });

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
