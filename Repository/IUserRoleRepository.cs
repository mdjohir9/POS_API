using POS_API.DTO.UserRoles;
using POS_API.Entities;

namespace POS_API.Repository
{
    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {
        IEnumerable<UserRole> GetUserRoleByIdCustom(int Id);
        Task<IEnumerable<DdlRolesDTO>> GetUserRolesAsync(bool IsGuestUser, string CompanyId);
        Task<IEnumerable<UserRolesInfoDTO>> GelAllUserRolesAsync(string companyId, bool IsAdministrator);
    }
}
