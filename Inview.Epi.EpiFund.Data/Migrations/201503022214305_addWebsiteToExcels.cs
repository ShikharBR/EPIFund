namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addWebsiteToExcels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NARMembers", "Website", c => c.String());
            AddColumn("dbo.PrincipalInvestors", "Website", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PrincipalInvestors", "Website");
            DropColumn("dbo.NARMembers", "Website");
        }
    }
}
