namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class loiEscrowCompanyOneLineAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LOIs", "EscrowCompanyAddress2", c => c.String());
            AddColumn("dbo.LOIs", "EscrowCompanyCity", c => c.String());
            AddColumn("dbo.LOIs", "EscrowCompanyState", c => c.String());
            AddColumn("dbo.LOIs", "EscrowCompanyZip", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LOIs", "EscrowCompanyZip");
            DropColumn("dbo.LOIs", "EscrowCompanyState");
            DropColumn("dbo.LOIs", "EscrowCompanyCity");
            DropColumn("dbo.LOIs", "EscrowCompanyAddress2");
        }
    }
}
