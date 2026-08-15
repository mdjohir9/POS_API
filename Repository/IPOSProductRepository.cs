using POS_API.Entities.Master;

namespace POS_API.Repository
{
    public interface IPOSProductRepository:IGenericRepository<POSProduct>
    {
        Task<POSProduct?> GetByBarcodeAsync(int companyId, string barcode);
    }
}
