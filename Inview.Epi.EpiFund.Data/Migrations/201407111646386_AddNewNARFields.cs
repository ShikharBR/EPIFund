namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewNARFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NARMembers", "CellPhoneNumber", c => c.String());
            AddColumn("dbo.NARMembers", "WorkPhoneNumber", c => c.String());
            AddColumn("dbo.NARMembers", "FaxNumber", c => c.String());
            DropColumn("dbo.NARMembers", "PhoneNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NARMembers", "PhoneNumber", c => c.String());
            DropColumn("dbo.NARMembers", "FaxNumber");
            DropColumn("dbo.NARMembers", "WorkPhoneNumber");
            DropColumn("dbo.NARMembers", "CellPhoneNumber");
        }
    }
}
