namespace Polly.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DataSourceType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Description = c.String(),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Website", t => t.WebsiteId, cascadeDelete: true)
                .Index(t => t.WebsiteId);
            
            CreateTable(
                "dbo.Website",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Domain = c.String(),
                        UserAgent = c.String(),
                        DataSourceTypeId = c.Int(nullable: false),
                        HeadingXPath = c.String(),
                        SubHeadingXPath = c.String(),
                        DescriptionXPath = c.String(),
                        PriceXPath = c.String(),
                        CategoryXPath = c.String(),
                        BreadcrumbXPath = c.String(),
                        MainImageXPath = c.String(),
                        RobotsText = c.String(),
                        Schedule = c.DateTime(nullable: false),
                        Logo = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DataSourceType", t => t.DataSourceTypeId, cascadeDelete: true)
                .Index(t => t.DataSourceTypeId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Website", t => t.WebsiteId, cascadeDelete: true)
                .Index(t => t.Priority, name: "PriorityIndex")
                .Index(t => t.WebsiteId);
            
            CreateTable(
                "dbo.PriceHistory",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OriginalPrice = c.Decimal(precision: 18, scale: 2),
                        DiscountAmount = c.Decimal(precision: 18, scale: 2),
                        DiscountPercentage = c.Decimal(precision: 18, scale: 2),
                        PriceChangeAmount = c.Decimal(precision: 18, scale: 2),
                        PriceChangePercent = c.Decimal(precision: 18, scale: 2),
                        TimeStamp = c.DateTime(nullable: false),
                        ProductId = c.Long(),
                        PreviousPriceHistoryId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PriceHistory", t => t.PreviousPriceHistoryId)
                .ForeignKey("dbo.Product", t => t.ProductId)
                .Index(t => new { t.DiscountAmount, t.PreviousPriceHistoryId }, name: "IX_PriceHistory_DiscountAmount_PreviousPriceHistoryId")
                .Index(t => t.ProductId, name: "IX_PriceHistory_ProductId")
                .Index(t => t.PreviousPriceHistoryId, name: "IX_PriceHistory_PreviousPriceHistoryId");
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UniqueIdentifier = c.String(maxLength: 80),
                        Url = c.String(maxLength: 850),
                        LastChecked = c.DateTime(nullable: false),
                        Title = c.String(maxLength: 500),
                        Description = c.String(),
                        Breadcrumb = c.String(maxLength: 500),
                        Category = c.String(maxLength: 500),
                        Image = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UniqueIdentifier, unique: true, name: "IX_Product_UniqueIdentifier")
                .Index(t => t.Url, name: "IX_Product_URL")
                .Index(t => t.LastChecked, name: "IX_Product_LastChecked")
                .Index(t => t.Title, name: "IX_Product_Title");
            
            CreateTable(
                "dbo.ProductCategory",
                c => new
                    {
                        ProductId = c.Long(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.CategoryId })
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductCategory", "ProductId", "dbo.Product");
            DropForeignKey("dbo.ProductCategory", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.PriceHistory", "ProductId", "dbo.Product");
            DropForeignKey("dbo.PriceHistory", "PreviousPriceHistoryId", "dbo.PriceHistory");
            DropForeignKey("dbo.DownloadQueue", "WebsiteId", "dbo.Website");
            DropForeignKey("dbo.DownloadData", "WebsiteId", "dbo.Website");
            DropForeignKey("dbo.Website", "DataSourceTypeId", "dbo.DataSourceType");
            DropIndex("dbo.ProductCategory", new[] { "CategoryId" });
            DropIndex("dbo.ProductCategory", new[] { "ProductId" });
            DropIndex("dbo.Product", "IX_Product_Title");
            DropIndex("dbo.Product", "IX_Product_LastChecked");
            DropIndex("dbo.Product", "IX_Product_URL");
            DropIndex("dbo.Product", "IX_Product_UniqueIdentifier");
            DropIndex("dbo.PriceHistory", "IX_PriceHistory_PreviousPriceHistoryId");
            DropIndex("dbo.PriceHistory", "IX_PriceHistory_ProductId");
            DropIndex("dbo.PriceHistory", "IX_PriceHistory_DiscountAmount_PreviousPriceHistoryId");
            DropIndex("dbo.DownloadQueue", new[] { "WebsiteId" });
            DropIndex("dbo.DownloadQueue", "PriorityIndex");
            DropIndex("dbo.Website", new[] { "DataSourceTypeId" });
            DropIndex("dbo.DownloadData", new[] { "WebsiteId" });
            DropTable("dbo.ProductCategory");
            DropTable("dbo.Product");
            DropTable("dbo.PriceHistory");
            DropTable("dbo.DownloadQueue");
            DropTable("dbo.Website");
            DropTable("dbo.DownloadData");
            DropTable("dbo.DataSourceType");
            DropTable("dbo.Category");
        }
    }
}
