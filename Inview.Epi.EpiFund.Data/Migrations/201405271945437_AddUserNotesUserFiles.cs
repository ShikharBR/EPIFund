namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserNotesUserFiles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserFiles",
                c => new
                    {
                        UserFileId = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        DateUploaded = c.DateTime(nullable: false),
                        FileLocation = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserFileId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserNotes",
                c => new
                    {
                        UserNoteId = c.Int(nullable: false, identity: true),
                        Notes = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserNoteId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.AssetUserMDAs", "FileLocation", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserNotes", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserFiles", "UserId", "dbo.Users");
            DropIndex("dbo.UserNotes", new[] { "UserId" });
            DropIndex("dbo.UserFiles", new[] { "UserId" });
            DropColumn("dbo.AssetUserMDAs", "FileLocation");
            DropTable("dbo.UserNotes");
            DropTable("dbo.UserFiles");
        }
    }
}
