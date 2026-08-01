using POS_API.DTO;
using POS_API.Entities;
using POS_API.Repository;

namespace POS_API.Repository
{
    public interface ILoginRepository : IGenericRepository<User>
    {
        IEnumerable<User> GetLoginInfo(string userName, string userPassword);
        //string GetUserDepartment(string EmpId);
        //string GetUserDesignation(string EmpId);
        //IEnumerable<string> GetUserPermission(string userId);
        UserProfileDTO GetUserProfileInfo(int Id);
        CompanyStatusDTO GetUserCompany(int userId);
        UserRole GetUserRoleByDataAccessLevel(int dataAccessLevel);

        string GenerateJwtToken(User user);
    }
}
