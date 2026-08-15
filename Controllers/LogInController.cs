using POS_API.DTO;
using POS_API.DTO.users;
using POS_API.Entities;
using POS_API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class LogInController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly IUnitOfWork _unitOfWork;
        int userId = 1;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LogInController(ApplicationDbContext dbContext, IUnitOfWork unitOfWork, IMemoryCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _cache = cache;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }
        //test

        [HttpPost]
        [Route("Login")]
        public IActionResult PostUsers([FromBody] LoginDTO loginDTO)
        {
            try
            {

                if (loginDTO == null)
                {
                    return BadRequest(new { StatusCode = 400, message = "User object is null." });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new { StatusCode = 400, message = "Invalid model state.", data = ModelState });
                }

                var users = _unitOfWork.Login.GetLoginInfo(loginDTO.UserName, loginDTO.UserPassword);
                var _user = users.FirstOrDefault();

                if (_user == null)
                {
                    return NotFound(new { StatusCode = 404, message = "User not found or invalid credentials." });
                }

                var userRole = _unitOfWork.Login.GetUserProfileInfo(_user.UserRoleID);
                var _userRoles = users.FirstOrDefault();
                var Company = _unitOfWork.Login.GetUserCompany(_user.UserId);

         
                if (Company != null)
                {
                    switch (Company.Status)
                    {
                        case 0:
                            return BadRequest(new { StatusCode = 400, message = "Your company is inactive." });
                        case 2:
                            return BadRequest(new { StatusCode = 400, message = "Your company is expired." });
                        case 3:
                            return BadRequest(new { StatusCode = 400, message = "Your company is suspended." });
                    }
                }

                var accessToken = _unitOfWork.Login.GenerateJwtToken(_user);

            

                if (_user.IsAdministrator == null)
                {
                    _user.IsAdministrator = false;
                }

                //var request = _httpContextAccessor.HttpContext.Request;
                //var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";

                //var imageUrl = string.IsNullOrEmpty(_user.UserImage) ? "" : $"{baseUrl}/{_user.UserImage}";

                var userinfo = users
                    .Select(u => new LoginInfoDTO
                    {
                        UserId = u.UserId,
                        CompanyId = u.CompanyId,
                        CompanyName = Company?.CompanyName,
                        Status = Company?.Status,
                        UserName = u.UserName,
                        UserPassword = u.UserPassword,
                        UserImage = u.UserImage,
                        Name = (u.FirstName + " " + u.LastName),
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        UserRoleID = u.UserRoleID,  
                        RoleName = userRole.UserRoleName,
                        Email = u.Email,
                        IsGuestUser = u.IsGuestUser,
                        CustomerID = u.ReferenceID,
                        AdditionalPermissions = u.AdditionalPermissions,
                        RemovedPermissions = u.RemovedPermissions,
                        IsAdministrator = u.IsAdministrator,
                        dataAccessLevel=userRole.DataAccessLevel.ToString(),
                    })
                    .FirstOrDefault();

                if (userinfo == null)
                {
                    return NotFound(new { StatusCode = 404, message = "User information not found." });
                }

                HttpContext.Session.SetString("UserId", _user.UserId.ToString());
                HttpContext.Session.SetString("UserName", _user.UserName);

                return Ok(new
                {
                    StatusCode = 200,
                    message = "Login successful.",
                    data = userinfo,
                    AccessToken = accessToken,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, message = "An error occurred.", error = ex.Message });
            }
        }


        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> PostUsers([FromBody] RegistrationDTO usersDTO)
        {
            try
            {
                // Validate the DTO
                if (usersDTO == null)
                {
                    return BadRequest(new { StatusCode = 400, message = "User object is null." });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Ensure passwords match
                if (usersDTO.NewPassword != usersDTO.ConfirmPassword)
                {
                    return BadRequest(new { StatusCode = 400, message = "New Password and Confirm Password do not match." });
                }

                // Check if the username or email already exists
                //bool userNameExists = await _unitOfWork.User.CheckUserNameIsExist(usersDTO.EamilOrPhone);
                //if (userNameExists)
                //{
                //    return Ok(new
                //    {
                //        StatusCode = 400,
                //        message = "An account with this email or phone number already exists. Would you like to sign in instead?."
                //    });
                //}

                // Get user role by data access level
                var userRole = _unitOfWork.Login.GetUserRoleByDataAccessLevel(1);
                if (userRole == null || userRole.UserRoleId == 0)
                {
                    return BadRequest(new { StatusCode = 400, message = "Please set up UserRole." });
                }

                // Create the user object
                var user = new User
                {
                    FirstName = usersDTO.FirstName,
                    LastName = usersDTO.LastName,
                    UserName = usersDTO.EamilOrPhone,
                    UserPassword = usersDTO.ConfirmPassword,
                    Email = usersDTO.EamilOrPhone,
                    UserRoleID = userRole.UserRoleId,
                    IsGuestUser = true,
                    IsApprovingAuthority = false,
                    ReferenceID = null,
                    AdditionalPermissions = null,
                    RemovedPermissions = null,
                    DataAccessPermission = null,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    CompanyId = 1
                };

                // Save user with exception handling for unique constraint violation
                try
                {
                    await _unitOfWork.User.AddAsync(user);
                    await _unitOfWork.Save();

                    // Clear related cache
                    string cacheKey = $"users";
                    _cache.Remove(cacheKey);

                    return Ok(new { StatusCode = 200, message = "User created successfully" });
                }
                catch (DbUpdateException dbEx)
                {
                    if (dbEx.InnerException is SqlException sqlEx &&
                        sqlEx.Message.Contains("Cannot insert duplicate key row") &&
                        sqlEx.Message.Contains("IX_Users_UserName"))
                    {
                        return Ok(new
                        {
                            StatusCode = 400,
                            message = "An account with this phone number already exists. Would you like to sign in instead?."
                        });
                    }

                    return StatusCode(500, new { StatusCode = 500, message = "A database error occurred", error = dbEx.Message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, message = "An error occurred", error = ex.Message });
            }
        }
    }
}
