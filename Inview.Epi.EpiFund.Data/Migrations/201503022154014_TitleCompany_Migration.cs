namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TitleCompany_Migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TitleCompanies",
                c => new
                    {
                        TitleCompanyId = c.Int(nullable: false, identity: true),
                        TitleCompName = c.String(),
                        TitleCompURL = c.String(),
                        IncludedStates = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TitleCompanyId);
            
            DropColumn("dbo.NARMembers", "Website");
            DropColumn("dbo.PrincipalInvestors", "Website");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PrincipalInvestors", "Website", c => c.String());
            AddColumn("dbo.NARMembers", "Website", c => c.String());
            DropTable("dbo.TitleCompanies");
        }
    }
}
