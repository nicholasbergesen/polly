namespace Polly.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Renme : DbMigration
    {
        public override void Up()
        {
            RenameColumn("PriceHistory", "SpecialPrice", "OriginalPrice");
        }
        
        public override void Down()
        {

        }
    }
}
