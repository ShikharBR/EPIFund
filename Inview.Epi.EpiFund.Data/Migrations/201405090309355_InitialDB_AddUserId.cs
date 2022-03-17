namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDB_AddUserId : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.Binary(),
                        UserType = c.Int(nullable: false),
                        AlternateEmail = c.String(),
                        HomeNumber = c.String(),
                        FaxNumber = c.String(),
                        CellNumber = c.String(),
                        WorkNumber = c.String(),
                        PreferredMethod = c.Int(nullable: false),
                        PreferredContactTime = c.Int(nullable: false),
                        AddressLine1 = c.String(),
                        AddressLine2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
