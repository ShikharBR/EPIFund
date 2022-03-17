namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssetMHPSpecification_Migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetMHPSpecifications",
                c => new
                    {
                        AssetMHPSpecificationId = c.Guid(nullable: false),
                        NumberSingleWide = c.Int(nullable: false),
                        CurrentSingleBaseRent = c.Single(nullable: false),
                        NumberSingleWideOwned = c.Int(nullable: false),
                        CurrentSingleOwnedBaseRent = c.Single(nullable: false),
                        NumberDoubleWide = c.Int(nullable: false),
                        CurrentDoubleBaseRent = c.Single(nullable: false),
                        NumberDoubleWideOwned = c.Int(nullable: false),
                        CurrentDoubleOwnedBaseRent = c.Single(nullable: false),
                        NumberTripleWide = c.Int(nullable: false),
                        CurrentTripleBaseRent = c.Single(nullable: false),
                        NumberTripleWideOwned = c.Int(nullable: false),
                        CurrentTripleOwnedBaseRent = c.Single(nullable: false),
                        CountOfUnits = c.Int(nullable: false),
                        AssetId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AssetMHPSpecificationId)
                .ForeignKey("dbo.Assets", t => t.AssetId)
                .Index(t => t.AssetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetMHPSpecifications", "AssetId", "dbo.Assets");
            DropIndex("dbo.AssetMHPSpecifications", new[] { "AssetId" });
            DropTable("dbo.AssetMHPSpecifications");
        }
    }
}
