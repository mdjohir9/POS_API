using POS_API.DTO;
using POS_API.DTO.users;
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
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private const int userId = 1;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserRepository userRepository, IMemoryCache cache, IUnitOfWork unitOfWork, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _cache = cache;
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet]
        [Route("dynamicMenu")]
        public async Task<IActionResult> GetDynamicMenue(int userId, int DataAccessLevel)
        {
            try
            {

                var users = await _unitOfWork.User.GetDynamicMenue(userId, DataAccessLevel);
                return Ok(new { StatusCode = 200, message = "Success", data = users });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, message = "An error occurred", error = ex.Message });
            }
        }

        [HttpGet]
        [Route("UserNameAndId")]
        public async Task<IActionResult> GetUserIdAndName(long companyId, int? userId, int dataAccessLevel)
        {
            try
            {
                var users = await _unitOfWork.User.GetUserIdAndNameAsync(companyId, userId, dataAccessLevel);

                if (users == null || !users.Any())
                {
                    return NotFound(new { StatusCode = 404, message = "Users not found!" });
                }

                return Ok(new { StatusCode = 200, message = "Success", data = users });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, message = "An error occurred", error = ex.Message });
            }
        }



        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> GetUsers(long companyId, bool IsAdministrator)
        {
            try
            {
                string cacheKey = "users";
                if (!_cache.TryGetValue(cacheKey, out List<User> cachedResult))
                {
                    var users = await _unitOfWork.User.GetAllUsersAsync(companyId, IsAdministrator);
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




        //[HttpGet]
        //[Route("userProfile/{id}")]
        //public async Task<IActionResult> GetProfile(int id, string CompanyId)
        //{
        //    try
        //    {
        //        string cacheKey = "users";
        //        if (!_cache.TryGetValue(cacheKey, out List<User> cachedResult))
        //        {
        //            var users = await _unitOfWork.User.GetUserProfileAsync(id, CompanyId);
        //            if (users == null || !users.Any())
        //            {
        //                return NotFound(new { StatusCode = 404, message = "Users not found!." });
        //            }

        //            _cache.Set(cacheKey, users, TimeSpan.FromMinutes(1));
        //            return Ok(new { StatusCode = 200, message = "Success", data = users });
        //        }
        //        else
        //        {
        //            return Ok(new { StatusCode = 200, message = "Success", data = cachedResult });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { StatusCode = 500, message = "An error occurred", error = ex.Message });
        //    }
        //}


        [HttpGet]
        [Route("users/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                string cacheKey = $"user{id}";
                if (!_cache.TryGetValue(cacheKey, out UsersDTO cachedResult))
                {
                    var user = await _unitOfWork.User.GetByIdAsync(id);
                    if (user == null)
                    {
                        return NotFound(new { StatusCode = 404, message = "User not found." });
                    }

                    // Get the base URL
                    var request = _httpContextAccessor.HttpContext.Request;
                    var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";

                    // Determine ImageUrl
                    var imageUrl = string.IsNullOrEmpty(user.UserImage) ? "" : $"{baseUrl}/{user.UserImage}";

                    // Map user entity to UsersDTO
                    var userDto = new UsersDTO
                    {
                        UserId = user.UserId,
                        Name = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        UserImage = imageUrl, // Full URL for UserImage
                        UserPassword =user.UserPassword,
                        Email = user.Email,
                        UserRoleID = user.UserRoleID,
                        IsGuestUser = user.IsGuestUser,
                        IsApprovingAuthority = user.IsApprovingAuthority,
                        ReferenceID = user.ReferenceID,
                        AdditionalPermissions = user.AdditionalPermissions,
                        RemovedPermissions = user.RemovedPermissions,
                        DataAccessPermission = user.DataAccessPermission,
                        IsActive = user.IsActive,
                        CompanyId = user.CompanyId,
                    };

                    // Cache the result for 1 minute
                    _cache.Set(cacheKey, userDto, TimeSpan.FromMinutes(1));

                    return Ok(new { StatusCode = 200, message = "Success", data = userDto });
                }
                else
                {
                    return Ok(new { StatusCode = 200, message = "Success", data = cachedResult });
                }
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { StatusCode = 404, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, message = "An error occurred", error = ex.Message });
            }
        }




        [HttpPost]
        [Route("users/create")]
        public async Task<IActionResult> PostUsers([FromBody] UserCreateDTO usersDTO)
        {
            try
            {
                // Check if the incoming user object is null
                if (usersDTO == null)
                {
                    return BadRequest(new { StatusCode = 400, message = "User object is null." });
                }

                // Ensure that the username does not already exist in the database
                bool userNameExists = await _unitOfWork.User.CheckUserNameIsExist(usersDTO.UserName);
                if (userNameExists)
                {
                    return Ok(new { StatusCode = 400, message = "An account with this email or phone number already exists. Would you like to sign in instead?" });
                }

                // Validate the model state
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Proceed to create a new User
                var user = new User
                {
                    FirstName = usersDTO.FirstName,
                    LastName = usersDTO.LastName,
                    UserName = usersDTO.UserName,
                    UserPassword = usersDTO.UserPassword,
                    Email = usersDTO.UserName,
                    UserRoleID = usersDTO.UserRoleID,
                    IsGuestUser = false,
                    IsApprovingAuthority = null,
                    ReferenceID = null,
                    AdditionalPermissions =null,
                    RemovedPermissions = null,
                    DataAccessPermission = null,
                    IsActive = usersDTO.IsActive,
                    CreatedAt = DateTime.Now,
                    CreatedBy = userId,  // Ensure you have a way to set the userId
                    CompanyId = 1
                };

                // Add the new user and save changes
                await _unitOfWork.User.AddAsync(user);
                await _unitOfWork.Save();

                // Clear the cache for user-related data if necessary
                string cacheKey = $"users";
                _cache.Remove(cacheKey);

                // Return success response
                return Ok(new { StatusCode = 200, message = "User created successfully" });
            }
            catch (Exception ex)
            {
                // Return error response in case of any exceptions
                return StatusCode(500, new { StatusCode = 500, message = "An error occurred", error = ex.Message });
            }
        }


        [HttpPut]
        [Route("users/password/update/{id}")]
        public async Task<IActionResult> UpdateUsersPassword(int id, [FromBody] UserPasswordUpdateDTO usersDTO)
        {
            try
            {
                // Validate input
                if (usersDTO == null)
                {
                    return BadRequest(new { StatusCode = 400, message = "User password object is null." });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Fetch the existing user
                var existingUser = await _unitOfWork.User.GetByIdAsync(id);

                if (existingUser == null)
                {
                    return NotFound(new { StatusCode = 404, message = "User not found." });
                }

                // Decrypt the stored password and compare with the provided old password
                string oldUserPasswordDecrypted = existingUser.UserPassword;

                if (oldUserPasswordDecrypted != usersDTO.OldUserPassword)
                {
                    return Ok(new { StatusCode = 400, message = "Old Password Doesn't Match" });

                }

                // Update the password and save the changes
                existingUser.UserPassword =usersDTO.NewUserPassword;
                existingUser.UpdatedAt = DateTime.Now;
                existingUser.UpdatedBy = id;

                await _unitOfWork.User.UpdateAsync(existingUser);
                await _unitOfWork.Save();

                // Clear cache
                string cacheKey = $"users";
                string cacheKeyID = $"user{id}";
                _cache.Remove(cacheKeyID);
                _cache.Remove(cacheKey);

                return Ok(new { StatusCode = 200, message = "User password updated successfully." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { StatusCode = 404, message = "User not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, message = "An error occurred while updating the user password.", error = ex.Message });
            }
        }


        [HttpPut]
        [Route("users/update/{id}")]
        public async Task<IActionResult> UpdateUsers(int id, [FromBody] UsersDTO usersDTO)
        {
            try
            {
                if (usersDTO == null)
                {
                    return BadRequest(new { StatusCode = 400, message = "User Roles object is null." });
                }
                bool userNameExists = await _unitOfWork.User.CheckUserNameIsExistById(usersDTO.UserName, id);
                if (userNameExists)
                {
                    return Ok(new { StatusCode = 400, message = "Username already exists. Please choose a different username." });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingUser = await _unitOfWork.User.GetByIdAsync(id);


                if (existingUser == null)
                {
                    return NotFound(new { StatusCode = 404, message = "User Roles not found." });
                }
                existingUser.FirstName = usersDTO.FirstName;
                existingUser.LastName = usersDTO.LastName;
                existingUser.UserName = usersDTO.UserName;
                existingUser.UserImage = usersDTO.UserImage;
                existingUser.UserPassword =usersDTO.UserPassword;
                existingUser.Email = usersDTO.Email;
                existingUser.UserRoleID = usersDTO.UserRoleID;
                existingUser.IsGuestUser = usersDTO.IsGuestUser;
                existingUser.IsApprovingAuthority = usersDTO.IsApprovingAuthority;
                existingUser.ReferenceID = usersDTO.ReferenceID;
                existingUser.AdditionalPermissions = usersDTO.AdditionalPermissions;
                existingUser.RemovedPermissions = usersDTO.RemovedPermissions;
                existingUser.DataAccessPermission = usersDTO.DataAccessPermission;
                existingUser.IsActive = usersDTO.IsActive;
                existingUser.UpdatedAt = DateTime.Now;
                existingUser.UpdatedBy = id;
                existingUser.CompanyId = 1;

                await _unitOfWork.User.UpdateAsync(existingUser);
                await _unitOfWork.Save();
                string cacheKey = $"users";
                string cacheKeyID = $"user{id}";
                _cache.Remove(cacheKeyID);
                _cache.Remove(cacheKey);
                return Ok(new { StatusCode = 200, message = "User Roles updated successfully." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { StatusCode = 404, message = "User not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, message = "An error occurred while updating the user.", error = ex.Message });
            }
        }
        [HttpDelete]
        [Route("users/delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                // Assuming the user ID of the person performing the delete is stored in the claims
                await _unitOfWork.User.DeleteAsync(id);
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



        [HttpPost]
        [Route("users/ImageUpdate")]
        public async Task<IActionResult> UploadImage([FromBody] ImageUpdateDTO imageUpdateDTO)
        {

            string DocumentType = "UserImage";
            var result = await _unitOfWork.User.SaveDocumentsListsAsync(imageUpdateDTO.ImageBase64, imageUpdateDTO.ID, imageUpdateDTO.CompanyId, DocumentType);

            var userId = new User { UserId = int.Parse(imageUpdateDTO.ID) };
            await _unitOfWork.User.UpdateAsync(userId, "UserImage", result);

            await _unitOfWork.Save();
            string cacheKey = $"users";
            string cacheKeyID = $"user{imageUpdateDTO.ID}";
            return Ok(new { statusCode = 200, message = "Image Update Success" });
        }

        //[HttpPost]
        //[Route("users/ImageUpdate/{id}")]
        //public async Task<IActionResult> UploadImage(IFormFile file, int id)
        //{
        //    if (file == null || file.Length == 0)
        //        return BadRequest("No file uploaded.");
        //    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        //    var fileExtension = Path.GetExtension(file.FileName).ToLower();
        //    if (!allowedExtensions.Contains(fileExtension))
        //        return Ok(new { StatusCode = 400, message = "Invalid file type. Only .jpg, .jpeg, and .png are allowed." });

        //    var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userImage");
        //    if (!Directory.Exists(uploadsFolderPath))
        //    {
        //        Directory.CreateDirectory(uploadsFolderPath);
        //    }

        //    var fileName = $"{id}{fileExtension}";
        //    var filePath = Path.Combine(uploadsFolderPath, fileName);

        //    var user = await _unitOfWork.User.GetByIdAsync(id);
        //    if (user == null)
        //        return NotFound("User not found");

        //    if (!string.IsNullOrEmpty(user.UserImage))
        //    {
        //        var existingImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.UserImage.TrimStart('/'));
        //        if (System.IO.File.Exists(existingImagePath))
        //        {
        //            System.IO.File.Delete(existingImagePath);
        //        }
        //    }

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }
        //    var relativePath = "/userImage/" + fileName;

        //    user.UserImage = relativePath;

        //    await _unitOfWork.User.UpdateAsync(user);
        //    await _unitOfWork.Save();

        //    return Ok(new { filePath = relativePath });
        //}



    }
}
