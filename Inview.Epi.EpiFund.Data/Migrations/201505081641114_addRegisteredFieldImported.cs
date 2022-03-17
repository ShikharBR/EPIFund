namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRegisteredFieldImported : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MBAUsers", "Registered", c => c.Boolean());
            AddColumn("dbo.NARMembers", "Registered", c => c.Boolean());
            AddColumn("dbo.PrincipalInvestors", "Registered", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PrincipalInvestors", "Registered");
            DropColumn("dbo.NARMembers", "Registered");
            DropColumn("dbo.MBAUsers", "Registered");
        }
    }
}
