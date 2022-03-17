namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsSubmittedToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "IsSubmitted", c => c.Boolean(nullable: false));
            DropColumn("dbo.Assets", "Active");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assets", "Active", c => c.Boolean(nullable: false));
            DropColumn("dbo.Assets", "IsSubmitted");
        }
    }
}
