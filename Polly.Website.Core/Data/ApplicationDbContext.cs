using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Polly.Website.Core.Data.Models;

namespace Polly.Website.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Website> Website { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<PriceHistory> PriceHistory { get; set; }
        public DbSet<DataSourceType> DataSourceType { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<Error> Error { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //This will singularize all table names
            foreach (IMutableEntityType entityType in builder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.DisplayName());
            }
            base.OnModelCreating(builder);
        }
    }
}