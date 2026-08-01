using POS_API.Entities;
using POS_API.Repository;
using POS_API.DTO.UserRoles;
using Microsoft.EntityFrameworkCore;
namespace POS_API.Implementation
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRoleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<UserRole> GetUserRoleByIdCustom(int roleId)
        {
            return _dbContext.UserRole.Where(ur => ur.UserRoleId == roleId).ToList();
        }

        //private List<object> BuildTree(Dictionary<int, ModuleDto> moduleDict, int? parentId)
        //{
        //    var children = moduleDict.Values
        //                            .Where(m => m.ParentId == parentId && m.IsPermission == false)
        //                            .Select(m => new ModuleDto
        //                            {
        //                                ModuleID = m.ModuleID,
        //                                ParentId = m.ParentId,
        //                                Name = m.Name,
        //                                IsPermission = m.IsPermission,
        //                                Children = m.Children.Any() ? m.Children : BuildTree(moduleDict, m.ModuleID).Cast<object>().ToList()
        //                            })
        //                            .Cast<object>()
        //                            .ToList();

        //    return children;
        //}


        public async Task<IEnumerable<DdlRolesDTO>> GetUserRolesAsync(bool IsGuestUser, string CompanyId)
        {
            // Basic filter with CompanyId
            var query = _dbContext.UserRole
                .Where(m => (m.Deleted == null || m.Deleted == false) && m.CompanyId == CompanyId);

            // Apply additional DataAccessLevel filter if IsGuestUser is true
            if (IsGuestUser)
            {
                query = query.Where(m => m.DataAccessLevel == 3 || m.DataAccessLevel == 4);
            }

            // Project the data into DdlRolesDTO
            var dataDepartment = await query
                .Select(m => new DdlRolesDTO
                {
                    RolesID = m.UserRoleId,
                    RoleName = m.UserRoleName,
                })
                .ToListAsync();

            return dataDepartment;
        }

        public async Task<IEnumerable<UserRolesInfoDTO>> GelAllUserRolesAsync(string companyId, bool IsAdministrator)
        {
            var query = from role in _dbContext.UserRole
                        where role.Deleted == null || role.Deleted == false
                        select new UserRolesInfoDTO
                        {
                            UserRoleId = role.UserRoleId,
                            CompanyId = role.CompanyId,
                            UserRoleName = role.UserRoleName,
                            Permissions = role.Permissions, // Adjust field if necessary
                            DataAccessLevel = role.DataAccessLevel,
                            Ordering = role.Ordering,
                            IsActive = role.IsActive,
                            Deleted = role.Deleted,
                            CreatedAt = role.CreatedAt,
                            CreatedBy = role.CreatedBy,
                            UpdatedAt = role.UpdatedAt,
                            UpdatedBy = role.UpdatedBy,
                            DeletedAt = role.DeletedAt,
                            DeletedBy = role.DeletedBy
                        };

            // Apply filtering based on the IsAdministrator flag
            if (!IsAdministrator)
            {
                query = query.Where(role => role.CompanyId == companyId);
            }

            // Execute the query
            return await query.ToListAsync();
        }






    }
}
