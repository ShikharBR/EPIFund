namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSearchCriteriaTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetSearchCriterias",
                c => new
                    {
                        AssetSearchCriteriaId = c.Int(nullable: false, identity: true),
                        NameOfPurchasingEntity = c.String(),
                        AddressLine1OfPurchasingEntity = c.String(),
                        AddressLine2OfPurchasingEntity = c.String(),
                        CityOfPurchasingEntity = c.String(),
                        StateOfPurchasingEntity = c.String(),
                        ZipOfPurchasingEntity = c.String(),
                        TypeOfPurchasingEntity = c.Int(nullable: false),
                        ManagingOfficerOfEntity = c.String(),
                        OfficeNumberOfEntity = c.String(),
                        CellNumberOfEntity = c.String(),
                        FaxNumberOfEntity = c.String(),
                        EmailAddressOfEntity = c.String(),
                        StateOfIncorporation = c.String(),
                        OtherCorporateOfficer = c.String(),
                        ContactNumberOfCorporateOfficer = c.String(),
                        EmailAddressOfCorporateOfficer = c.String(),
                        IsCorporateEntityInGoodStanding = c.Boolean(nullable: false),
                        NameOfOtherCorporateOfficer = c.String(),
                        ContactNumberOfOtherCorporateOfficer = c.String(),
                        EmailAddressOfOtherCorporateOfficer = c.String(),
                        HasEntityRaisedIntendedCap = c.Boolean(nullable: false),
                        AmountOfIntendedCapEquity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LeverageTarget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TermsSought = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TimelineSecuringCap = c.String(),
                        UtilizePMFunding = c.Boolean(nullable: false),
                        CREPMFBrokerLender = c.String(),
                        SecuredCLAPOF = c.Boolean(nullable: false),
                        GeneralNotesOfVestingEntity = c.String(),
                        TypeOfAssetsSought = c.Int(nullable: false),
                        InvestmentFundingRangeMin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InvestmentFundingRangeMax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinimumCapitalizationRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TargetPricePerUnitMin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TargetPricePerUnitMax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TargetPricePerSpaceMin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TargetPricePerSpaceMax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WillConsiderUnperformingAtPricing = c.Boolean(nullable: false),
                        ProFormaParametersDetails = c.String(),
                        WillConsiderBrandNew = c.Boolean(nullable: false),
                        BrandNewDetails = c.String(),
                        WillConsiderPartiallyBuilt = c.Boolean(nullable: false),
                        PartiallyBuiltDetails = c.String(),
                    })
                .PrimaryKey(t => t.AssetSearchCriteriaId);
            
            CreateTable(
                "dbo.SearchCriteriaDemographicDetails",
                c => new
                    {
                        SearchCriteriaDemographicDetailId = c.Int(nullable: false, identity: true),
                        AssetSearchCriteriaId = c.Int(nullable: false),
                        NumberOfUnitsRangeMinimum = c.Int(nullable: false),
                        NumberOfUnitsRangeMaximum = c.Int(nullable: false),
                        AgeOfPropertyMaximum = c.Int(nullable: false),
                        MultiLevelAcceptable = c.Boolean(nullable: false),
                        MultiLevelTwoLevelsAcceptable = c.Boolean(nullable: false),
                        MultiLevelThreeLevelsAcceptable = c.Boolean(nullable: false),
                        MultiLevelFourLevelsAcceptable = c.Boolean(nullable: false),
                        MultiLevelOtherLevelsAcceptable = c.Boolean(nullable: false),
                        AcceptsEFFUnits = c.Boolean(nullable: false),
                        MaxRatioOfEFfUnits = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AcceptsOneBedroomUnits = c.Boolean(nullable: false),
                        MaxRatioOfOneBedroomUnits = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RoofingFlatBuiltUp = c.Boolean(nullable: false),
                        RoofingTileOnly = c.Boolean(nullable: false),
                        RoofingShingleOnly = c.Boolean(nullable: false),
                        HasParkingRatioParameters = c.Boolean(nullable: false),
                        ParkingRatioRequisites = c.String(),
                        RequiresParkingStalls = c.Boolean(nullable: false),
                        Pools = c.Boolean(nullable: false),
                        PoolsOptional = c.Boolean(nullable: false),
                        OutdoorSpas = c.Boolean(nullable: false),
                        OutdoorSpasOptional = c.Boolean(nullable: false),
                        SecurityGates = c.Boolean(nullable: false),
                        SecurityGatesOptional = c.Boolean(nullable: false),
                        SeparateOfficeBuilding = c.Boolean(nullable: false),
                        SeparateOfficeBuildingOptional = c.Boolean(nullable: false),
                        SeparateClubhouse = c.Boolean(nullable: false),
                        SeparateClubhouseOptional = c.Boolean(nullable: false),
                        PlaygroundArea = c.Boolean(nullable: false),
                        PlaygroundAreaOptional = c.Boolean(nullable: false),
                        TennisCourts = c.Boolean(nullable: false),
                        TennisCourtsOptional = c.Boolean(nullable: false),
                        GymFacilitiesForAdults = c.Boolean(nullable: false),
                        GymFacilitiesForAdultsOptional = c.Boolean(nullable: false),
                        TurnKey = c.Boolean(nullable: false),
                        TurnKeyOptional = c.Boolean(nullable: false),
                        ExtensiveRenovationUpdatingNeeds = c.Boolean(nullable: false),
                        ExtensiveRenovationUpdatingNeedsOptional = c.Boolean(nullable: false),
                        UnderperformingProperty = c.Boolean(nullable: false),
                        UnderperformingPropertyOptional = c.Boolean(nullable: false),
                        TenantOnly = c.Boolean(nullable: false),
                        MasterMetering = c.Boolean(nullable: false),
                        GradeClassificationRequirementOfProperty = c.String(),
                        TenantProfileRestrictions = c.String(),
                        SingleWideSpaceRatioForAllSpaces = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DoubleWideSpaceRatioForAllSpaces = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OtherRequirements = c.String(),
                        SquareFootageRangeMin = c.String(),
                        SquareFootageRangeMax = c.String(),
                        AgePropertyRangeMax = c.String(),
                        PropertyRequiresMajorTenant = c.Boolean(nullable: false),
                        TenantRequisites = c.String(),
                        MinimumForAccreditedTenantProfiles = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RequiresSingleTenantPads = c.Boolean(nullable: false),
                        SingleTenantPadsRequisiteDetails = c.String(),
                        ParkingRatioRatioRequisites = c.String(),
                        RequiresCoveredParkingStalls = c.String(),
                        OfficeMedicalMixedUseCoveredParkingStallsRequired = c.Boolean(nullable: false),
                        OfficeMedicalMixedUseCoveredParkingStallsRequiredOptional = c.Boolean(nullable: false),
                        WillLookAtUnifinishedSuites = c.Boolean(nullable: false),
                        WillLookAtUnifinishedSuitesOptional = c.Boolean(nullable: false),
                        CanHaveExtensiveRenovationNeeds = c.Boolean(nullable: false),
                        CanHaveExtensiveRenovationNeedsOptional = c.Boolean(nullable: false),
                        CanBeVacant = c.Boolean(nullable: false),
                        MaxVacancy = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.SearchCriteriaDemographicDetailId)
                .ForeignKey("dbo.AssetSearchCriterias", t => t.AssetSearchCriteriaId)
                .Index(t => t.AssetSearchCriteriaId);
            
            CreateTable(
                "dbo.SearchCriteriaDueDiligenceItems",
                c => new
                    {
                        SearchCriteriaDueDiligenceItemId = c.Int(nullable: false, identity: true),
                        AssetSearchCriteriaId = c.Int(nullable: false),
                        ItemDescription = c.String(),
                        ImportanceLevel = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SearchCriteriaDueDiligenceItemId)
                .ForeignKey("dbo.AssetSearchCriterias", t => t.AssetSearchCriteriaId)
                .Index(t => t.AssetSearchCriteriaId);
            
            CreateTable(
                "dbo.SearchCriteriaGeographicParameters",
                c => new
                    {
                        SearchCriteriaGeographicParameterId = c.Int(nullable: false, identity: true),
                        AssetSeachCriteriaId = c.Int(nullable: false),
                        InterestCity = c.String(),
                        InterestState = c.String(),
                        AdditionalRequirements = c.String(),
                        Restrictions = c.String(),
                        AssetSearchCriteria_AssetSearchCriteriaId = c.Int(),
                    })
                .PrimaryKey(t => t.SearchCriteriaGeographicParameterId)
                .ForeignKey("dbo.AssetSearchCriterias", t => t.AssetSearchCriteria_AssetSearchCriteriaId)
                .Index(t => t.AssetSearchCriteria_AssetSearchCriteriaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SearchCriteriaGeographicParameters", "AssetSearchCriteria_AssetSearchCriteriaId", "dbo.AssetSearchCriterias");
            DropForeignKey("dbo.SearchCriteriaDueDiligenceItems", "AssetSearchCriteriaId", "dbo.AssetSearchCriterias");
            DropForeignKey("dbo.SearchCriteriaDemographicDetails", "AssetSearchCriteriaId", "dbo.AssetSearchCriterias");
            DropIndex("dbo.SearchCriteriaGeographicParameters", new[] { "AssetSearchCriteria_AssetSearchCriteriaId" });
            DropIndex("dbo.SearchCriteriaDueDiligenceItems", new[] { "AssetSearchCriteriaId" });
            DropIndex("dbo.SearchCriteriaDemographicDetails", new[] { "AssetSearchCriteriaId" });
            DropTable("dbo.SearchCriteriaGeographicParameters");
            DropTable("dbo.SearchCriteriaDueDiligenceItems");
            DropTable("dbo.SearchCriteriaDemographicDetails");
            DropTable("dbo.AssetSearchCriterias");
        }
    }
}
