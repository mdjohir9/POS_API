using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique(); 

            base.OnModelCreating(modelBuilder);


        }




    }
}
