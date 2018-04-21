namespace Polly.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Polly.Data.PollyDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Polly.Data.PollyDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var datTypeValues = Enum.GetValues(typeof(DataSourceTypeEnum));

            foreach (DataSourceTypeEnum datType in datTypeValues)
            {
                context.DataSourceType.AddOrUpdate(new DataSourceType()
                {
                    Id = datType,
                    Description = Enum.GetName(typeof(DataSourceTypeEnum), datType)
                });
            }
        }
    }
}
