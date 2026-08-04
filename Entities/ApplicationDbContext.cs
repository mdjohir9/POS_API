using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using POS_API.Entities.Inventory;
using POS_API.Entities.Master;
using POS_API.Entities.Purchase;
using POS_API.Entities.Sales;

namespace POS_API.Entities
{
    public class ApplicationDbContext:DbContext
    {
       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<HrdCompanyInfo> HrdCompanyInfo { get; set; }

        public DbSet<POSBrand> POS_Brands { get; set; }
        public DbSet<POSCategory> POS_Categorys { get; set; }
        public DbSet<POSCustomer> POS_Units { get; set; }
        public DbSet<POSProduct> POS_Products { get; set; }
        public DbSet<POSProductBatch> POS_ProductBatchs { get; set; }
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
            modelBuilder.HasDefaultSchema("dbo");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique(); 

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<POSSalesDetail>()
                .HasOne(x => x.Product)
                .WithMany(x => x.SalesDetails)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HrdCompanyInfo>().HasKey(x => x.CompanyId);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                     .Where(x => typeof(BaseEntity).IsAssignableFrom(x.ClrType)))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasOne(typeof(HrdCompanyInfo))
                    .WithMany()
                    .HasForeignKey("CompanyId")
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }

    }
}
