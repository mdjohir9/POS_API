using POS_API.DTO;
using POS_API.Entities.Purchase;
using POS_API.Entities.Sales;

namespace POS_API.Repository
{
    public interface IPOSSalesMasterRepository: IGenericRepository<POSSalesMaster>
    {
        Task<IEnumerable<POSSalesListDTO>> GetSalesListAsync();
        //Task<IEnumerable<POSSalesInvoiceDTO>> GetSalesInvoiceAsync(int salesMasterId);
        Task<POSSalesInvoiceDTO?> GetSalesInvoiceAsync(int salesMasterId);
        Task<POSDashboardSummaryDTO> GetDashboardSummaryAsync(int companyId, DateTime date);
        Task<SalesPurchaseSummaryDTO> GetSalesPurchaseSummaryAsync(string companyId, int year);
        Task<StockInOutSummaryDTO> GetStockInOutSummaryAsync(string companyId, DateTime selectedDate);
    }
}
