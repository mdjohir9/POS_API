using POS_API.DTO;
using POS_API.Entities.Purchase;
using POS_API.Entities.Sales;

namespace POS_API.Repository
{
    public interface IPOSSalesMasterRepository: IGenericRepository<POSSalesMaster>
    {
        Task<IEnumerable<POSSalesListDTO>> GetSalesListAsync();

    }
}
