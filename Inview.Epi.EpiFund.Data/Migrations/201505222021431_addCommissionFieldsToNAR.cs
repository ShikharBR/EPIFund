namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCommissionFieldsToNAR : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NARMembers", "CommissionShareAgr", c => c.Boolean(nullable: false));
            AddColumn("dbo.NARMembers", "CommissionAmount", c => c.Double());
            AddColumn("dbo.NARMembers", "DateOfCsaConfirm", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NARMembers", "DateOfCsaConfirm");
            DropColumn("dbo.NARMembers", "CommissionAmount");
            DropColumn("dbo.NARMembers", "CommissionShareAgr");
        }
    }
}
