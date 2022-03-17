namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddListedByUserIdToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "ListedByUserId", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "User_UserId", c => c.Int());
            CreateIndex("dbo.Assets", "User_UserId");
            AddForeignKey("dbo.Assets", "User_UserId", "dbo.Users", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assets", "User_UserId", "dbo.Users");
            DropIndex("dbo.Assets", new[] { "User_UserId" });
            DropColumn("dbo.Assets", "User_UserId");
            DropColumn("dbo.Assets", "ListedByUserId");
        }
    }
}
