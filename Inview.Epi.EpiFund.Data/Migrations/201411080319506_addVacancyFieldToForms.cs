namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVacancyFieldToForms : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "NumberOfRentableSuites", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "NumberOfRentableSuites");
        }
    }
}
