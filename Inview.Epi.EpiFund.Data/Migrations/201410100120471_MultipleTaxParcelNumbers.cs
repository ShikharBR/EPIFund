namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MultipleTaxParcelNumbers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetTaxParcelNumbers",
                c => new
                    {
                        AssetTaxParcelNumberId = c.Int(nullable: false, identity: true),
                        AssetId = c.Guid(nullable: false),
                        TaxParcelNumber = c.String(),
                    })
                .PrimaryKey(t => t.AssetTaxParcelNumberId)
                .ForeignKey("dbo.Assets", t => t.AssetId)
                .Index(t => t.AssetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetTaxParcelNumbers", "AssetId", "dbo.Assets");
            DropIndex("dbo.AssetTaxParcelNumbers", new[] { "AssetId" });
            DropTable("dbo.AssetTaxParcelNumbers");
        }
    }
}
