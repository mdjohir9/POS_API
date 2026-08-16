using POS_API.DTO;
using POS_API.Entities.Purchase;

namespace POS_API.Repository
{
    public interface IPOSPurchaseMasterRepository:IGenericRepository<POSPurchaseMaster>
    {
        Task<POSPurchaseCreateResultDTO> CreatePurchaseAsync(POSPurchaseMaster purchaseMaster, List<POSPurchaseDetail> details);
        Task<IEnumerable<POSPurchaseViewDto>> GetPurchasesFromViewAsync(string companyId);

    }
}
