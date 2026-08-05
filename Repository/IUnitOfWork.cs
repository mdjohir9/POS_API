using Microsoft.EntityFrameworkCore.Storage;
using POS_API.Repository;

namespace POS_API.Repository
{
    public interface IUnitOfWork: IDisposable
    {

        IUserRepository User { get; }
        IUserRoleRepository UserRole { get; }
        ILoginRepository Login { get; }
        IPOSBrandRepository POSBrand { get; }
        IPOSCategoryRepository POSCategory { get; }
        IPOSCustomerRepository POSCustomer { get; }
        IPOSProductRepository POSProduct { get; }
        IPOSProductBatchRepository POSProductBatch { get; }
        IPOSSupplierRepository POSSupplier { get; }
        IPOSUnitRepository POSUnit { get; }
        IPOSPurchaseMasterRepository POSPurchaseMaster { get; }
        IPOSPurchaseDetailRepository POSPurchaseDetail { get; }
        IPOSSalesMasterRepository POSSalesMaster { get; }
        IPOSSalesDetailRepository POSSalesDetail { get; }
        IPOSSalesPaymentRepository POSSalesPayment { get; }
        IPOSSalesPaymentMethodRepository POSSalesPaymentMethod { get; }
        IPOSStockLedgerRepository POSStockLedger { get; }
      

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> Save();
    }
}
