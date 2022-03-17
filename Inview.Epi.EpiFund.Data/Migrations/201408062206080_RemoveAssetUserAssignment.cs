namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAssetUserAssignment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssetUserAssignments", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.AssetUserAssignments", "UserId", "dbo.Users");
            DropIndex("dbo.AssetUserAssignments", new[] { "UserId" });
            DropIndex("dbo.AssetUserAssignments", new[] { "AssetId" });
            DropTable("dbo.AssetUserAssignments");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.AssetUserAssignmentId);
            
            CreateIndex("dbo.AssetUserAssignments", "AssetId");
            CreateIndex("dbo.AssetUserAssignments", "UserId");
            AddForeignKey("dbo.AssetUserAssignments", "UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.AssetUserAssignments", "AssetId", "dbo.Assets", "AssetId");
        }
    }
}
