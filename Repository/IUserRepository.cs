using POS_API.DTO;
using POS_API.Entities;

namespace POS_API.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<IEnumerable<string>> GetUserRolePermissionById(int id);
        Task<IEnumerable<UsersDTO>> GetAllUsersAsync(long companyId, bool IsAdministrator);
        Task<IEnumerable<object>> GetDynamicMenue(int userId , int DataAccessLevel);
        Task<bool> CheckUserNameIsExist(string userName);  // Changed to accept userName
        Task<bool> CheckUserNameIsExistById(string userName, int userId);  // Changed to accept userName

        Task<IEnumerable<object>> GetUserIdAndNameAsync(long companyId, int? userId, int dataAccessLevel);

    }
}
