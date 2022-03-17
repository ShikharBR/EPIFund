namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddICStatusToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ICStatus", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ICStatus");
        }
    }
}
