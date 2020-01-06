namespace Polly.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RemoveDownloadQueueTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DownloadData", "WebsiteId", "dbo.Website");
            DropForeignKey("dbo.DownloadQueue", "WebsiteId", "dbo.Website");
            DropIndex("dbo.DownloadData", new[] { "WebsiteId" });
            DropIndex("dbo.DownloadQueue", "PriorityIndex");
            DropIndex("dbo.DownloadQueue", new[] { "WebsiteId" });
            DropTable("dbo.DownloadData");
            DropTable("dbo.DownloadQueue");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.DownloadQueue",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    AddedDate = c.DateTime(nullable: false),
                    DownloadUrl = c.String(maxLength: 2000),
                    Priority = c.Int(nullable: false),
                    WebsiteId = c.Long(nullable: false),
                    UrlHash = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.DownloadData",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    TimeStamp = c.DateTime(nullable: false),
                    Url = c.String(),
                    RawHtml = c.String(),
                    WebsiteId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateIndex("dbo.DownloadQueue", "WebsiteId");
            CreateIndex("dbo.DownloadQueue", "Priority", name: "PriorityIndex");
            CreateIndex("dbo.DownloadData", "WebsiteId");
            AddForeignKey("dbo.DownloadQueue", "WebsiteId", "dbo.Website", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DownloadData", "WebsiteId", "dbo.Website", "Id", cascadeDelete: true);
        }
    }
}
