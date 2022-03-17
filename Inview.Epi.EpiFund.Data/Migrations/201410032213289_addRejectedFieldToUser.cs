namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRejectedFieldToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsRejectedICAdmin", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IsRejectedICAdmin");
        }
    }
}
