using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.SqlServer;
using System.Threading.Tasks;

namespace Polly.Data
{
    [DbConfigurationType(typeof(MyConfiguration))]
    public class PollyDbContext : IdentityDbContext<User, Role, long, UserLogin, UserRole, UserClaim>
    {
        static PollyDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PollyDbContext, Migrations.Configuration>());
        }

        public static PollyDbContext Create()
        {
            return new PollyDbContext();
        }

        public PollyDbContext()
            :base("DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Website> Website { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<PriceHistory> PriceHistory { get; set; }
        public DbSet<DataSourceType> DataSourceType { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }

    //https://docs.microsoft.com/en-gb/ef/ef6/fundamentals/connection-resiliency/retry-logic?redirectedfrom=MSDN
    public class MyConfiguration : DbConfiguration
    {
        public MyConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}
