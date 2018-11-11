namespace Polly.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqueIndex : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Product", new[] { "UniqueIdentifier" });
            CreateIndex("dbo.Product", "UniqueIdentifier", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Product", new[] { "UniqueIdentifier" });
            CreateIndex("dbo.Product", "UniqueIdentifier");
        }
    }
}
