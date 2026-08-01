using Microsoft.EntityFrameworkCore.Storage;
using POS_API.Repository;

namespace POS_API.Repository
{
    public interface IUnitOfWork: IDisposable
    {

        IUserRepository User { get; }
        IUserRoleRepository UserRole { get; }
        ILoginRepository Login { get; }

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> Save();
    }
}
