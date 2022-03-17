namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTypeToUserFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserFiles", "Type", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserFiles", "Type");
        }
    }
}
