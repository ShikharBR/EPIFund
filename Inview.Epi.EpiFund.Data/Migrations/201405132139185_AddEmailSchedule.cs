namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailSchedule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailSchedules",
                c => new
                    {
                        EmailScheduleId = c.Int(nullable: false, identity: true),
                        EmailScheduleType = c.Int(nullable: false),
                        LastRunDate = c.DateTime(),
                        IntervalInDays = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EmailScheduleId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmailSchedules");
        }
    }
}
