namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FKchanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Assets", "CreationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Assets", "CreationDate", c => c.DateTime(nullable: false));
        }
    }
}
