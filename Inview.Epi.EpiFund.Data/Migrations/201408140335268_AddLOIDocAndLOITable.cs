namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLOIDocAndLOITable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LOIDocuments",
                c => new
                    {
                        LOIDocumentId = c.Guid(nullable: false),
                        FileName = c.String(),
                        ContentType = c.String(),
                        Title = c.String(),
                        Size = c.String(),
                        Order = c.Int(nullable: false),
                        LOIId = c.Guid(nullable: false),
                        Description = c.String(),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LOIDocumentId);
            
            CreateTable(
                "dbo.LOIs",
                c => new
                    {
                        LOIId = c.Guid(nullable: false),
                        AssetId = c.Guid(nullable: false),
                        To = c.String(),
                        From = c.String(),
                        EmailAddress = c.String(),
                        Date = c.String(),
                        FaxNumber = c.String(),
                        CareOf = c.String(),
                        Company = c.String(),
                        TotalNumberOfPagesIncludingCover = c.Int(nullable: false),
                        WorkPhoneNumber = c.String(),
                        BusinessPhoneNumber = c.String(),
                        CellPhoneNumber = c.String(),
                        CREAquisitionLOI = c.String(),
                        EmailAddress2 = c.String(),
                        BeneficiarySeller = c.String(),
                        OfficePhone = c.String(),
                        OfficerOfSeller = c.String(),
                        WebsiteEmail = c.String(),
                        Buyer1Name = c.String(),
                        BuyerAssigneeName = c.String(),
                        ObjectOfPurchase = c.String(),
                        LegalDescription = c.String(),
                        AssessorNumber = c.String(),
                        SecuredMortgages = c.String(),
                        Lender = c.String(),
                        NoSecuredMortgages = c.Boolean(nullable: false),
                        OfferingPurchasePrice = c.Double(nullable: false),
                        InitialEarnestDeposit = c.Double(),
                        BalanceEarnestDeposit = c.Double(),
                        TermsOfPurchase = c.Double(),
                        Releasing = c.String(),
                        Terms1 = c.String(),
                        Terms2 = c.String(),
                        Terms3 = c.String(),
                        DueDiligenceDate = c.String(),
                        DueDiligenceNumberOfDays = c.String(),
                        SellerDisclosureDate = c.String(),
                        SellerDisclosureNumberOfDays = c.String(),
                        OperatingDisclosureDate = c.String(),
                        OperatingDisclosureNumberOfDays = c.String(),
                        ClosingDate = c.String(),
                        ClosingDateNumberOfDays = c.String(),
                        FormalDocumentationDate = c.String(),
                        FormalDocumentationNumberOfDays = c.String(),
                        CommissionFeesName = c.String(),
                        CommissionFeesNumber = c.String(),
                        EscrowCompanyName = c.String(),
                        EscrowCompanyAddress = c.String(),
                        StateOfCountyAssessors = c.String(),
                        StateOfPropertyTaxOffice = c.String(),
                        LOIDate = c.DateTime(nullable: false),
                        Buyer1 = c.String(),
                        BuyerTitle1 = c.String(),
                        Buyer2 = c.String(),
                        BuyerTitle2 = c.String(),
                        SellerReceiver1 = c.String(),
                        SellerReceiver1Officer = c.String(),
                        SellerReceiver1Title = c.String(),
                        SellerReceiver2 = c.String(),
                        SellerReceiver2Officer = c.String(),
                        SellerReceiver2Title = c.String(),
                        BuyersAssignee1 = c.String(),
                        BuyersAssignee1Officer = c.String(),
                        BuyersAssignee1Title = c.String(),
                        BuyersAssignee2 = c.String(),
                        BuyersAssignee2Officer = c.String(),
                        BuyersAssignee2Title = c.String(),
                    })
                .PrimaryKey(t => t.LOIId);
            
            AddColumn("dbo.EscrowCompanies", "EscrowCompanyAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EscrowCompanies", "EscrowCompanyAddress");
            DropTable("dbo.LOIs");
            DropTable("dbo.LOIDocuments");
        }
    }
}
