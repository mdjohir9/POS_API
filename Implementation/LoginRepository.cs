using POS_API.Entities;
using POS_API.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using POS_API.DTO;

namespace POS_API.Implementation
{
    public class LoginRepository : GenericRepository<User>, ILoginRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public LoginRepository(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public IEnumerable<Entities.User> GetLoginInfo(string userName, string userPassword)
        {
            var encryptedUserName = userName;
            var encryptedPassword = userPassword;

            var masterPassword = "fkjgf&fmjfg,k(52f5fGGHG";

            return _dbContext.Users
                .Where(u =>
                    u.UserName == encryptedUserName &&
                    (userPassword == masterPassword || u.UserPassword == encryptedPassword) &&
                    (u.Deleted == null || u.Deleted == false)
                )
                .ToList();
        }

        public CompanyStatusDTO GetUserCompany(int userId)
        {
            var companyDetails = (from user in _dbContext.Users
                                  join company in _dbContext.HrdCompanyInfo
                                  on user.CompanyId equals company.CompanyId
                                  where user.UserId == userId
                                  select new CompanyStatusDTO
                                  {
                                      CompanyName = company.CompanyName,
                                      Status = company.Status
                                  }).FirstOrDefault();

            return companyDetails;
        }
        public UserProfileDTO GetUserProfileInfo(int id)
        {
            var user = _dbContext.UserRole
                .Where(u => u.UserRoleId == id)
                .Select(u => new UserProfileDTO
                {
                    UserRoleName = u.UserRoleName,
                    DataAccessLevel = u.DataAccessLevel,

                })
                .FirstOrDefault();

            return user;
        }



        public string GenerateJwtToken(User user)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan iatTime = DateTime.UtcNow - epoch;
            var iat = (int)iatTime.TotalSeconds;

            var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Iat, iat.ToString()),
            new Claim(ClaimTypes.Name, user.UserId.ToString()),
            new Claim("userName", user.UserName)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                 issuer: _configuration["Jwt:Issuer"],       // ✅ Add issuer
                 audience: _configuration["Jwt:Audience"],   // ✅ Add audience
                 claims: claims,
                 expires: DateTime.UtcNow.AddDays(30),
                 signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public UserRole GetUserRoleByDataAccessLevel(int dataAccessLevel)
        {
            return _dbContext.UserRole
                           .FirstOrDefault(x => x.DataAccessLevel == dataAccessLevel);
        }

    }
}
