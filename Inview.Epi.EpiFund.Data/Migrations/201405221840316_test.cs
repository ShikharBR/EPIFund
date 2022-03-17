namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CorpAdminId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "CorpAdminId");
        }
    }
}
