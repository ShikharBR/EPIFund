namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowMultiplePreferredMethods : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PreferredMethods", c => c.String());
            DropColumn("dbo.Users", "PreferredMethod");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "PreferredMethod", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "PreferredMethods");
        }
    }
}
