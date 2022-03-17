namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDatesFromPFS : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PersonalFinancialStatements", "StocksDateOfQuotation", c => c.String());
            AlterColumn("dbo.PersonalFinancialStatements", "StocksDateOfQuotation2", c => c.String());
            AlterColumn("dbo.PersonalFinancialStatements", "StocksDateOfQuotation3", c => c.String());
            AlterColumn("dbo.PersonalFinancialStatements", "StocksDateOfQuotation4", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PersonalFinancialStatements", "StocksDateOfQuotation4", c => c.DateTime());
            AlterColumn("dbo.PersonalFinancialStatements", "StocksDateOfQuotation3", c => c.DateTime());
            AlterColumn("dbo.PersonalFinancialStatements", "StocksDateOfQuotation2", c => c.DateTime());
            AlterColumn("dbo.PersonalFinancialStatements", "StocksDateOfQuotation", c => c.DateTime());
        }
    }
}
