namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGradeAndProjectNameToAsset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "GradeClassification", c => c.String());
            AddColumn("dbo.Assets", "ProjectName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "ProjectName");
            DropColumn("dbo.Assets", "GradeClassification");
        }
    }
}
