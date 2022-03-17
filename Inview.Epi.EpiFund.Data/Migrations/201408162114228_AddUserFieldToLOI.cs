namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserFieldToLOI : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LOIs", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.LOIs", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LOIs", "Active");
            DropColumn("dbo.LOIs", "UserId");
        }
    }
}
