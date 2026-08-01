using POS_API.DTO;
using POS_API.Entities;

namespace POS_API.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<IEnumerable<string>> GetUserRolePermissionById(int id);
        Task<IEnumerable<UsersDTO>> GetAllUsersAsync(string companyId, bool IsAdministrator);
        Task<IEnumerable<object>> GetDynamicMenue(int userId , int DataAccessLevel);
        Task<bool> CheckUserNameIsExist(string userName);  // Changed to accept userName
        Task<bool> CheckUserNameIsExistById(string userName, int userId);  // Changed to accept userName

        Task<IEnumerable<object>> GetUserIdAndNameAsync(string companyId, int? userId, int dataAccessLevel);

    }
}
