namespace Polly.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Index2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Product", "UniqueIdentifier");
            DropColumn("dbo.Product", "UniqueIdentifierHash");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Product", "UniqueIdentifierHash", c => c.Int(nullable: false));
            DropIndex("dbo.Product", new[] { "UniqueIdentifier" });
        }
    }
}
