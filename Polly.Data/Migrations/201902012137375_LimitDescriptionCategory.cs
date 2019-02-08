namespace Polly.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LimitDescriptionCategory : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Category", "Description", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Category", "Description", c => c.String());
        }
    }
}
