namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTitleCoOrderColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "TitleCompanyId", c => c.Int());
            AddColumn("dbo.Assets", "TitleCompanyUserId", c => c.Int());
            AddColumn("dbo.Assets", "OrderId", c => c.Int());
            AddColumn("dbo.Assets", "OrderDate", c => c.DateTime());
            AddColumn("dbo.Assets", "OrderStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "OrderedByUserId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "OrderedByUserId");
            DropColumn("dbo.Assets", "OrderStatus");
            DropColumn("dbo.Assets", "OrderDate");
            DropColumn("dbo.Assets", "OrderId");
            DropColumn("dbo.Assets", "TitleCompanyUserId");
            DropColumn("dbo.Assets", "TitleCompanyId");
        }
    }
}
