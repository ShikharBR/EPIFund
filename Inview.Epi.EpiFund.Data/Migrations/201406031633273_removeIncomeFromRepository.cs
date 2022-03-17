namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeIncomeFromRepository : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.AssetIncomes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AssetIncomes",
                c => new
                    {
                        AssetIncomeId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AssetIncomeId);
            
        }
    }
}
