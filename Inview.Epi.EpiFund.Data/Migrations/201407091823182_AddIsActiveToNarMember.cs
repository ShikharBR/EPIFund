namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsActiveToNarMember : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NARMembers", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NARMembers", "IsActive");
        }
    }
}
