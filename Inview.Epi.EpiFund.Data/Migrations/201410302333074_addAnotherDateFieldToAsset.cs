namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAnotherDateFieldToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "BalloonDateForPayoffOfNote", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "BalloonDateForPayoffOfNote");
        }
    }
}
