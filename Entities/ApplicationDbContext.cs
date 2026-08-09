using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using POS_API.Entities.Inventory;
using POS_API.Entities.Master;
using POS_API.Entities.Purchase;
using POS_API.Entities.Sales;

namespace POS_API.Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<HrdCompanyInfo> HrdCompanyInfo { get; set; }

        public DbSet<POSBrand> POS_Brands { get; set; }
        public DbSet<POSCategory> POS_Categories { get; set; }
        public DbSet<POSUnit> POS_Units { get; set; }
        public DbSet<POSProduct> POS_Products { get; set; }
        public DbSet<POSProductBatch> POS_ProductBatches { get; set; }
        public DbSet<POSSupplier> POS_Suppliers { get; set; }
        public DbSet<POSCustomer> POS_Customers { get; set; }

        public DbSet<POSPurchaseMaster> POS_PurchaseMasters { get; set; }
        public DbSet<POSPurchaseDetail> POS_PurchaseDetails { get; set; }

        public DbSet<POSSalesMaster> POS_SalesMasters { get; set; }
        public DbSet<POSSalesDetail> POS_SalesDetails { get; set; }

        public DbSet<POSSalesPayment> POS_SalesPayments { get; set; }
        public DbSet<POSSalesPaymentMethod> POS_SalesPaymentMethods { get; set; }

        public DbSet<POSStockLedger> POS_StockLedgers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("dbo");



            modelBuilder.Entity<User>()  .HasIndex(x => x.UserName).IsUnique();
            modelBuilder.Entity<POSSalesDetail>().HasOne(x => x.Sales)
           .WithMany(x => x.Details)
           .HasForeignKey(x => x.SalesMasterId)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<POSSalesDetail>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
