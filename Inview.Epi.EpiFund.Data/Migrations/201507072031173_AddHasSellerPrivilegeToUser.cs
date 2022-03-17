namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHasSellerPrivilegeToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "HasSellerPrivilege", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "HasSellerPrivilege");
        }
    }
}
