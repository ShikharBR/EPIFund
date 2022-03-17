namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterTitleCompanyWorkflow : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "EIN", c => c.String());
            AddColumn("dbo.AssetOrderHistories", "UserEmail", c => c.String());
            AddColumn("dbo.TitleCompanies", "TitleCompAddress", c => c.String());
            AddColumn("dbo.TitleCompanies", "TitleCompPhone", c => c.String());
            AddColumn("dbo.TitleCompanyUsers", "AssignedStates", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TitleCompanyUsers", "AssignedStates");
            DropColumn("dbo.TitleCompanies", "TitleCompPhone");
            DropColumn("dbo.TitleCompanies", "TitleCompAddress");
            DropColumn("dbo.AssetOrderHistories", "UserEmail");
            DropColumn("dbo.Users", "EIN");
        }
    }
}
