using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Data
{
    public class PollyDbContext : DbContext
    {
        public PollyDbContext()
            :base("DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Website> Website { get; set; }
        public DbSet<DownloadData> DownloadData { get; set; }
        public DbSet<DownloadQueue> DownloadQueue { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<PriceHistory> PriceHistory { get; set; }
        public DbSet<DataSourceType> DataSourceType { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
