namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAssetSeller : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetSellers",
                c => new
                    {
                        AssetSellerId = c.Int(nullable: false, identity: true),
                        NameOfPrincipal = c.String(),
                        PhoneHome = c.String(),
                        PhoneWork = c.String(),
                        PhoneOther = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.AssetSellerId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AssetSellers");
        }
    }
}
