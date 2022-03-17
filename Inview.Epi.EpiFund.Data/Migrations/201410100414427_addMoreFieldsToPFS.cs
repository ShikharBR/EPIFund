namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMoreFieldsToPFS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PersonalFinancialStatements", "NotesPayablePaymentAmount", c => c.String());
            AddColumn("dbo.PersonalFinancialStatements", "NotesPayablePaymentAmount2", c => c.String());
            AddColumn("dbo.PersonalFinancialStatements", "NotesPayablePaymentAmount3", c => c.String());
            AddColumn("dbo.PersonalFinancialStatements", "NotesPayablePaymentAmount4", c => c.String());
            AddColumn("dbo.PersonalFinancialStatements", "NotesPayablePaymentAmount5", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PersonalFinancialStatements", "NotesPayablePaymentAmount5");
            DropColumn("dbo.PersonalFinancialStatements", "NotesPayablePaymentAmount4");
            DropColumn("dbo.PersonalFinancialStatements", "NotesPayablePaymentAmount3");
            DropColumn("dbo.PersonalFinancialStatements", "NotesPayablePaymentAmount2");
            DropColumn("dbo.PersonalFinancialStatements", "NotesPayablePaymentAmount");
        }
    }
}
