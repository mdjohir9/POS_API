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

        public DbSet<POS_Brand> Brands { get; set; }
        public DbSet<POS_Category> POS_Categorys { get; set; }
        public DbSet<POS_Unit> POS_Units { get; set; }
        public DbSet<POS_Product> POS_Products { get; set; }
        public DbSet<POS_ProductBatch> POS_ProductBatchs { get; set; }
        public DbSet<POS_Supplier> POS_Suppliers { get; set; }
        public DbSet<POS_Customer> POS_Customers { get; set; }
        public DbSet<POS_PurchaseMaster> POS_PurchaseMasters { get; set; }
        public DbSet<POS_PurchaseDetail> POS_PurchaseDetails { get; set; }
        public DbSet<POS_SalesMaster> POS_SalesMasters { get; set; }
        public DbSet<POS_SalesDetail> POS_SalesDetails { get; set; }
        public DbSet<POS_SalesPayment> POS_SalesPayments { get; set; }
        public DbSet<POS_SalesPaymentMethod> POS_SalesPaymentMethods { get; set; }
        public DbSet<POS_StockLedger> POS_StockLedgers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique(); 

            base.OnModelCreating(modelBuilder);


        }




    }
}
