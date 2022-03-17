namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalMHPFields_Migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "NumberRentableSpace", c => c.Int());
            AddColumn("dbo.Assets", "NumberNonRentableSpace", c => c.Int());
            AddColumn("dbo.Assets", "NumberParkOwnedMH", c => c.Int());
            AddColumn("dbo.Assets", "FloodPlainLocated", c => c.Boolean());
            AddColumn("dbo.Assets", "AccessRoadTypeId", c => c.Int());
            AddColumn("dbo.Assets", "InteriorRoadTypeId", c => c.Int());
            AddColumn("dbo.Assets", "WaterServTypeId", c => c.Int());
            AddColumn("dbo.Assets", "WasteWaterTypeId", c => c.Int());
            AddColumn("dbo.Assets", "MHPadTypeId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "MHPadTypeId");
            DropColumn("dbo.Assets", "WasteWaterTypeId");
            DropColumn("dbo.Assets", "WaterServTypeId");
            DropColumn("dbo.Assets", "InteriorRoadTypeId");
            DropColumn("dbo.Assets", "AccessRoadTypeId");
            DropColumn("dbo.Assets", "FloodPlainLocated");
            DropColumn("dbo.Assets", "NumberParkOwnedMH");
            DropColumn("dbo.Assets", "NumberNonRentableSpace");
            DropColumn("dbo.Assets", "NumberRentableSpace");
        }
    }
}
