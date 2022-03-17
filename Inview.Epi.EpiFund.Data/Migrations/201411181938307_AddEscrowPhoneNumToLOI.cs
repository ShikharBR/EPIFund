namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEscrowPhoneNumToLOI : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LOIs", "EscrowCompanyPhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LOIs", "EscrowCompanyPhoneNumber");
        }
    }
}
