namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TitleCompanyUser_Migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TitleCompanyUsers",
                c => new
                    {
                        TitleCompanyUserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ManagingOfficerName = c.String(),
                        Email = c.String(),
                        Password = c.Binary(),
                        PhoneNumber = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsManager = c.Boolean(nullable: false),
                        TitleCompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TitleCompanyUserId)
                .ForeignKey("dbo.TitleCompanies", t => t.TitleCompanyId)
                .Index(t => t.TitleCompanyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TitleCompanyUsers", "TitleCompanyId", "dbo.TitleCompanies");
            DropIndex("dbo.TitleCompanyUsers", new[] { "TitleCompanyId" });
            DropTable("dbo.TitleCompanyUsers");
        }
    }
}
