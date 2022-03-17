namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMbaToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "MbaUserId", c => c.Int());
            CreateIndex("dbo.Users", "MbaUserId");
            AddForeignKey("dbo.Users", "MbaUserId", "dbo.MBAUsers", "MBAUserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "MbaUserId", "dbo.MBAUsers");
            DropIndex("dbo.Users", new[] { "MbaUserId" });
            DropColumn("dbo.Users", "MbaUserId");
        }
    }
}
