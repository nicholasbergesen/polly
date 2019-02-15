namespace Polly.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManualIndexes1 : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE NONCLUSTERED INDEX [IX_PriceHistory_DiscountPercentage] ON [dbo].[PriceHistory]
                    (
                        [DiscountPercentage] ASC
                    )
                    INCLUDE([Id],
                        [Price],
	                    [TimeStamp],
	                    [ProductId],
	                    [OriginalPrice],
	                    [DiscountAmount],
	                    [PreviousPriceHistoryId],
	                    [PriceChangeAmount],
	                    [PriceChangePercent])");

            Sql(@"CREATE NONCLUSTERED INDEX [IX_PriceHistory_OriginalPrice] ON [dbo].[PriceHistory]
                    (
	                    [OriginalPrice] ASC
                    )
                    INCLUDE ([Id],
	                    [Price],
	                    [TimeStamp],
	                    [ProductId],
	                    [DiscountAmount],
	                    [DiscountPercentage],
	                    [PreviousPriceHistoryId],
	                    [PriceChangeAmount],
	                    [PriceChangePercent])");

            Sql(@"CREATE NONCLUSTERED INDEX [IX_PriceHistory_TimeStamp] ON [dbo].[PriceHistory]
                (
	                [TimeStamp] ASC
                )
                INCLUDE ([Id],
	                [ProductId])");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PriceHistory", "IX_PriceHistory_DiscountPercentage");
            DropIndex("dbo.PriceHistory", "IX_PriceHistory_OriginalPrice");
            DropIndex("dbo.PriceHistory", "IX_PriceHistory_TimeStamp");
        }
    }
}
