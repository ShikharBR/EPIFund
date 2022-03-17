namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEscrowCompanies : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EscrowCompanies",
                c => new
                    {
                        EscrowCompanyId = c.Guid(nullable: false),
                        EscrowCompanyName = c.String(),
                    })
                .PrimaryKey(t => t.EscrowCompanyId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EscrowCompanies");
        }
    }
}
