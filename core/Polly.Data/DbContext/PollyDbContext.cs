using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Polly.Data
{
    public class PollyDbContext : IdentityDbContext<User, Role, long>
    {
        public PollyDbContext(DbContextOptions<PollyDbContext> options)
           : base(options)
        {
        }

        public DbSet<Website> Website { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<PriceHistory> PriceHistory { get; set; }
        public DbSet<DataSourceType> DataSourceType { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<Error> Error { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
