namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addYetAnotherFieldToNARMember : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NARMembers", "NotOnList", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NARMembers", "NotOnList");
        }
    }
}
