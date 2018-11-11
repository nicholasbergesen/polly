namespace Polly.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PollyDbContext>
    {
        public Configuration()
        {
            CommandTimeout = 6000;
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PollyDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var dataTypeValues = Enum.GetValues(typeof(DataSourceTypeEnum));

            foreach (DataSourceTypeEnum datType in dataTypeValues)
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
