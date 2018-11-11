namespace Polly.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Indexes : DbMigration
    {
        public override void Up()
        {
            Sql("CREATE INDEX [IX_Product_UniqueIdentifierHash] ON [PollyDb].[dbo].[Product] ([UniqueIdentifierHash])");
            Sql("CREATE INDEX [IX_DownloadQueue_WebsiteId] ON [PollyDb].[dbo].[DownloadQueue] ([WebsiteId]) INCLUDE ([Id], [Priority])");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Product", new[] { "IX_Product_UniqueIdentifierHash" });
            DropIndex("dbo.DownloadQueue", new[] { "IX_DownloadQueue_WebsiteId" });
        }
    }
}
