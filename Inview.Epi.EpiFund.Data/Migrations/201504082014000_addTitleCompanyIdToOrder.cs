namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTitleCompanyIdToOrder : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TitleOrderPayments", new[] { "TitleCompany_TitleCompanyId" });
            RenameColumn(table: "dbo.TitleOrderPayments", name: "TitleCompany_TitleCompanyId", newName: "TitleCompanyId");
            AlterColumn("dbo.TitleOrderPayments", "TitleCompanyId", c => c.Int(nullable: false));
            CreateIndex("dbo.TitleOrderPayments", "TitleCompanyId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TitleOrderPayments", new[] { "TitleCompanyId" });
            AlterColumn("dbo.TitleOrderPayments", "TitleCompanyId", c => c.Int());
            RenameColumn(table: "dbo.TitleOrderPayments", name: "TitleCompanyId", newName: "TitleCompany_TitleCompanyId");
            CreateIndex("dbo.TitleOrderPayments", "TitleCompany_TitleCompanyId");
        }
    }
}
