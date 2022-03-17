namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJVMarketerAgreementToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "JVMarketerAgreementLocation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "JVMarketerAgreementLocation");
        }
    }
}
