namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNarMember : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NARMembers",
                c => new
                    {
                        NarMemberId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        CompanyName = c.String(),
                        CompanyAddressLine1 = c.String(),
                        CompanyAddressLine2 = c.String(),
                        CompanyCity = c.String(),
                        CompanyState = c.String(),
                        CompanyZip = c.String(),
                        PhoneNumber = c.String(),
                        ReferredByUserId = c.Int(),
                    })
                .PrimaryKey(t => t.NarMemberId);
            
            AddColumn("dbo.Assets", "NarMemberId", c => c.Int());
            CreateIndex("dbo.Assets", "NarMemberId");
            AddForeignKey("dbo.Assets", "NarMemberId", "dbo.NARMembers", "NarMemberId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assets", "NarMemberId", "dbo.NARMembers");
            DropIndex("dbo.Assets", new[] { "NarMemberId" });
            DropColumn("dbo.Assets", "NarMemberId");
            DropTable("dbo.NARMembers");
        }
    }
}
