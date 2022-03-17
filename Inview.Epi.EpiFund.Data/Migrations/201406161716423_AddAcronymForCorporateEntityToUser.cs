namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAcronymForCorporateEntityToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "AcronymForCorporateEntity", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "AcronymForCorporateEntity");
        }
    }
}
