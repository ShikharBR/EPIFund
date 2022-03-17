namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAssetUserAssignments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetUserAssignments",
                c => new
                    {
                        AssetUserAssignmentId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        AssetId = c.Guid(nullable: false),
                        ServiceOrderDate = c.DateTime(nullable: false),
                        ContractFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateFeePaid = c.DateTime(),
                        ServiceOrderCompleted = c.DateTime(),
                        MiscellaneousNotes = c.String(),
                    })
                .PrimaryKey(t => t.AssetUserAssignmentId)
                .ForeignKey("dbo.Assets", t => t.AssetId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.AssetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetUserAssignments", "UserId", "dbo.Users");
            DropForeignKey("dbo.AssetUserAssignments", "AssetId", "dbo.Assets");
            DropIndex("dbo.AssetUserAssignments", new[] { "AssetId" });
            DropIndex("dbo.AssetUserAssignments", new[] { "UserId" });
            DropTable("dbo.AssetUserAssignments");
        }
    }
}
