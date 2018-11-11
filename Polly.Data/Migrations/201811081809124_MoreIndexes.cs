namespace Polly.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreIndexes : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Product", "Url");
            CreateIndex("dbo.Product", "Title");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Product", new[] { "Title" });
            DropIndex("dbo.Product", new[] { "Url" });
        }
    }
}
