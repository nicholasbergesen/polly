namespace Polly.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shortedul : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Product", "Url", c => c.String(maxLength: 850));
            CreateIndex("dbo.Product", "Url");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Product", "Url", c => c.String(maxLength: 850));
            CreateIndex("dbo.Product", "Url");
        }
    }
}
