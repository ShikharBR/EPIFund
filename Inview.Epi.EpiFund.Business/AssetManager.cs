using Inview.Epi.EpiFund.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.ViewModel;
using Inview.Epi.EpiFund.Domain.Helpers;
using Inview.Epi.EpiFund.Domain.Enum;
using System.Data.Entity;
using System.Reflection;
using AutoMapper;
using System.IO;
using System.Configuration;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using OfficeOpenXml.Table.PivotTable;

namespace Inview.Epi.EpiFund.Business
{
    public class AssetManager : IAssetManager
    {
        private List<string> _dueDiligenceList = new List<string>();
        private IEPIContextFactory _factory;
        public AssetManager(IEPIContextFactory factory)
        {
            _factory = factory;
            _dueDiligenceList.Add("Site Images");
            _dueDiligenceList.Add("Property Specifications");
            _dueDiligenceList.Add("Prior Years Operating Records");
            _dueDiligenceList.Add("Current YTD Operating Reports");
            _dueDiligenceList.Add("Current Rent Rolls");
            _dueDiligenceList.Add("Tenant Leases (Commercial Asset)");
            _dueDiligenceList.Add("Old Appraisal");
            _dueDiligenceList.Add("Current Year Appraisal");
            _dueDiligenceList.Add("Property Inspection Report (Independent)");
            _dueDiligenceList.Add("Phase I Environment Report (Date Importance)");
            _dueDiligenceList.Add("Preliminary Title Report");
            _dueDiligenceList.Add("Original Title Policy Report of Current Owner");
        }

        public void RecordAssetPayment(Domain.ViewModel.RecordPaymentModel model)
        {
            var context = _factory.Create();
            var asset = context.Assets.Single(w => w.AssetNumber == model.AssetNumber && w.IsActive);
            if (asset != null)
            {
                context.AssetSalePayments.Add(new AssetSalePayment()
                {
                    AssetId = asset.AssetId,
                    PaymentInformation = model.PaymentInformation,
                    PaymentReceivedDate = DateTime.Now,
                    PaymentType = model.PaymentType,
                    RecordedByUserId = model.RecordedByUserId,
                    SaleAmount = model.PaymentAmount
                });
                context.Save();
            }
        }

        public double CalculateCommission(Guid assetId)
        {
            // probably needs to be changed
            var context = _factory.Create();
            var asset = context.Assets.Single(s => s.AssetId == assetId && s.IsActive);
            return asset.ClosingPrice.GetValueOrDefault(0) * asset.CommissionShareToEPI;
        }

        public void RecordCommissionPayment(Domain.ViewModel.RecordCommissionPaymentModel model)
        {
            var context = _factory.Create();
            var asset = context.Assets.Single(w => w.AssetNumber == model.AssetNumber && w.IsActive);
            if (asset != null)
            {
                context.AssetCommissions.Add(new AssetCommission()
                {
                    AssetId = asset.AssetId,
                    ComissionPaymentType = model.PaymentType,
                    CommissionInformation = model.PaymentInformation,
                    RecordedByUserId = model.RecordedByUserId,
                    CommissionPaidDate = DateTime.Now
                });
                context.Save();
            }
        }

        public AssetViewModel GenerateNewAsset(bool isPaperAsset, AssetType type)
        {
            AssetViewModel newAsset = new AssetViewModel()
            {
                Paper = new PaperAsset(),
            };
            if (type == AssetType.MultiFamily)
            {
                newAsset = new MultiFamilyAssetViewModel()
                {
                    Paper = new PaperAsset(),
                    UnitSpecifications = new List<AssetUnitSpecification>() { new AssetUnitSpecification() },
                    //MHPUnitSpecifications = new List<AssetMHPSpecification>() { new AssetMHPSpecification() },
                };
            }
            else if (type == AssetType.MHP)
            {
                newAsset = new MultiFamilyAssetViewModel()
                {
                    Paper = new PaperAsset(),
                    UnitSpecifications = new List<AssetUnitSpecification>() { new AssetUnitSpecification() },
                    MHPUnitSpecifications = new List<AssetMHPSpecification>() { new AssetMHPSpecification() },
                };
            }
            else if (/*type == AssetType.Commercial || */type == AssetType.Office || type == AssetType.Retail || type == AssetType.Industrial || type == AssetType.Medical || type == AssetType.Other || type == AssetType.MixedUse || type == AssetType.ConvenienceStoreFuel)
            {
                var context = _factory.Create();


                newAsset = new CommercialAssetViewModel()
                {
                    Paper = new PaperAsset(),
                };
            }
            newAsset.AssetId = Guid.NewGuid();
            newAsset.AssetType = type;
            newAsset.Images = new List<AssetImage>();
            newAsset.Documents = new List<AssetDocument>();
            newAsset.EscrowCompany = "Chicago Title Agency";
            newAsset.EscrowCompanyAddress = "9075 W. Diablo Dr., Ste 100, Las Vegas NV 89148";
            newAsset.EscrowCompanyPhoneNumber = "(702) 836-8000";

            // 5/22
            //newAsset.AssetListingAgents = new List<AssetListingAgent>() { 
            //    new AssetListingAgent() {
            //            AssetId = newAsset.AssetId,
            //            AssetListingAgentId = Guid.NewGuid()
            //}};
            var emptyNar = new NARMember();
            emptyNar.NarMemberId = 0;
            emptyNar.IsActive = false; // i should of made this nullable
            newAsset.AssetNARMembers.Add(new AssetNARMember()
            {
                AssetId = Guid.Empty,
                AssetNARMemberId = Guid.Empty,
                NarMemberId = 0,
                NARMember = emptyNar
            });

            newAsset.AssetTaxParcelNumbers = new List<AssetTaxParcelNumber>();
            newAsset.IsPaper = isPaperAsset;
            if (isPaperAsset)
            {
                newAsset.Paper = new PaperAsset();
            }
            newAsset.DateForTempImages = DateTime.Now.ToString("yyyyMMdd");
            return newAsset;
        }

        public AssetSearchCriteriaModel GetAssetSearchCriteriaById(int id)
        {
            var context = _factory.Create();
            var assetSearch = context.AssetSearchCriterias.Single(s => s.AssetSearchCriteriaId == id);

            AssetSearchCriteriaModel model = new AssetSearchCriteriaModel();
            if (assetSearch.UserId.HasValue)
            {
                model.UserId = assetSearch.UserId.Value;
            }
            model.WebsiteURLVestingCorporateEntity = assetSearch.WebsiteURLVestingCorporateEntity;
            model.NameOfOtherCorporateOfficer2 = assetSearch.NameOfOtherCorporateOfficer2;
            model.AssetSearchCriteriaId = assetSearch.AssetSearchCriteriaId;
            model.AddressLine1OfPurchasingEntity = assetSearch.AddressLine1OfPurchasingEntity;
            model.AddressLine2OfPurchasingEntity = assetSearch.AddressLine2OfPurchasingEntity;
            model.AmountOfIntendedCapEquity = assetSearch.AmountOfIntendedCapEquity;
            model.CellNumberOfEntity = assetSearch.CellNumberOfEntity;
            model.CityOfPurchasingEntity = assetSearch.CityOfPurchasingEntity;
            model.ContactNumberOfCorporateOfficer = assetSearch.ContactNumberOfCorporateOfficer;
            model.ContactNumberOfOtherCorporateOfficer = assetSearch.ContactNumberOfOtherCorporateOfficer;
            model.CREPMFBrokerLender = assetSearch.CREPMFBrokerLender;
            model.EmailAddressOfCorporateOfficer = assetSearch.EmailAddressOfCorporateOfficer;
            model.EmailAddressOfEntity = assetSearch.EmailAddressOfEntity;
            model.EmailAddressOfOtherCorporateOfficer = assetSearch.EmailAddressOfOtherCorporateOfficer;
            model.FaxNumberOfEntity = assetSearch.FaxNumberOfEntity;
            model.GeneralNotesOfVestingEntity = assetSearch.GeneralNotesOfVestingEntity;
            model.HasEntityRaisedIntendedCap = assetSearch.HasEntityRaisedIntendedCap;
            model.IsCorporateEntityInGoodStanding = assetSearch.IsCorporateEntityInGoodStanding;
            model.LeverageTarget = assetSearch.LeverageTarget;
            model.ManagingOfficerOfEntity = assetSearch.ManagingOfficerOfEntity;
            model.NameOfOtherCorporateOfficer = assetSearch.NameOfOtherCorporateOfficer;

            model.NameOfPurchasingEntity = assetSearch.NameOfPurchasingEntity;
            model.OfficeNumberOfEntity = assetSearch.OfficeNumberOfEntity;
            model.OtherCorporateOfficer = assetSearch.OtherCorporateOfficer;
            model.SecuredCLAPOF = assetSearch.SecuredCLAPOF;
            model.StateOfIncorporation = assetSearch.StateOfIncorporation;
            model.StateOfPurchasingEntity = assetSearch.StateOfPurchasingEntity;
            model.TermsSought = assetSearch.TermsSought;
            model.TimelineSecuringCap = assetSearch.TimelineSecuringCap;
            model.TypeOfAssetsSought = assetSearch.TypeOfAssetsSought;
            model.TypeOfPurchasingEntity = assetSearch.TypeOfPurchasingEntity;
            model.UtilizePMFunding = assetSearch.UtilizePMFunding;
            model.ZipOfPurchasingEntity = assetSearch.ZipOfPurchasingEntity;
            model.DateEntered = assetSearch.DateEntered;

            model.FinancialInvestmentParameters = new FinancialInvestmentParametersModel();
            model.FinancialInvestmentParameters.BrandNewDetails = assetSearch.BrandNewDetails;
            model.FinancialInvestmentParameters.InvestmentFundingRangeMax = assetSearch.InvestmentFundingRangeMax;
            model.FinancialInvestmentParameters.InvestmentFundingRangeMin = assetSearch.InvestmentFundingRangeMin;
            model.FinancialInvestmentParameters.MinimumCapitalizationRate = assetSearch.MinimumCapitalizationRate;
            model.FinancialInvestmentParameters.PartiallyBuiltDetails = assetSearch.PartiallyBuiltDetails;
            model.FinancialInvestmentParameters.ProFormaParametersDetails = assetSearch.ProFormaParametersDetails;
            model.FinancialInvestmentParameters.TargetPricePerSpaceMax = assetSearch.TargetPricePerSpaceMax;
            model.FinancialInvestmentParameters.TargetPricePerSpaceMin = assetSearch.TargetPricePerSpaceMin;
            model.FinancialInvestmentParameters.TargetPricePerUnitMax = assetSearch.TargetPricePerUnitMax;
            model.FinancialInvestmentParameters.TargetPricePerUnitMin = assetSearch.TargetPricePerUnitMin;
            model.FinancialInvestmentParameters.WillConsiderBrandNew = assetSearch.WillConsiderBrandNew;
            model.FinancialInvestmentParameters.WillConsiderPartiallyBuilt = assetSearch.WillConsiderPartiallyBuilt;
            model.FinancialInvestmentParameters.WillConsiderUnperformingAtPricing = assetSearch.WillConsiderUnperformingAtPricing;

            var multifamily = context.SearchCriteriaDemographicDetails.Single(w => w.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && w.Type == "MultiFamily");
            model.MultiFamilyDemographicDetail = new MultiFamilyHomeDemographicDetailModel();
            model.MultiFamilyDemographicDetail.AcceptsEFFUnits = multifamily.AcceptsEFFUnits;
            model.MultiFamilyDemographicDetail.AcceptsOneBedroomUnits = multifamily.AcceptsOneBedroomUnits;
            model.MultiFamilyDemographicDetail.AgeOfPropertyMaximum = multifamily.AgeOfPropertyMaximum;
            model.MultiFamilyDemographicDetail.DoubleWideSpaceRatioForAllSpaces = multifamily.DoubleWideSpaceRatioForAllSpaces;
            model.MultiFamilyDemographicDetail.ExtensiveRenovationUpdatingNeeds = multifamily.ExtensiveRenovationUpdatingNeeds;
            model.MultiFamilyDemographicDetail.ExtensiveRenovationUpdatingNeedsOptional = multifamily.ExtensiveRenovationUpdatingNeedsOptional;
            model.MultiFamilyDemographicDetail.GradeClassificationRequirementOfProperty = multifamily.GradeClassificationRequirementOfProperty;
            model.MultiFamilyDemographicDetail.GymFacilitiesForAdults = multifamily.GymFacilitiesForAdults;
            model.MultiFamilyDemographicDetail.GymFacilitiesForAdultsOptional = multifamily.GymFacilitiesForAdultsOptional;
            model.MultiFamilyDemographicDetail.HasParkingRatioParameters = multifamily.HasParkingRatioParameters;
            model.MultiFamilyDemographicDetail.MasterMetering = multifamily.MasterMetering;
            model.MultiFamilyDemographicDetail.MaxRatioOfEFfUnits = multifamily.MaxRatioOfEFfUnits;
            model.MultiFamilyDemographicDetail.MultiLevelFourLevelsAcceptable = multifamily.MultiLevelFourLevelsAcceptable;
            model.MultiFamilyDemographicDetail.MultiLevelOtherLevelsAcceptable = multifamily.MultiLevelOtherLevelsAcceptable;
            model.MultiFamilyDemographicDetail.MultiLevelThreeLevelsAcceptable = multifamily.MultiLevelThreeLevelsAcceptable;
            model.MultiFamilyDemographicDetail.MultiLevelTwoLevelsAcceptable = multifamily.MultiLevelTwoLevelsAcceptable;
            model.MultiFamilyDemographicDetail.NumberOfUnitsRangeMaximum = multifamily.NumberOfUnitsRangeMaximum;
            model.MultiFamilyDemographicDetail.NumberOfUnitsRangeMinimum = multifamily.NumberOfUnitsRangeMinimum;
            model.MultiFamilyDemographicDetail.OtherRequirements = multifamily.OtherRequirements;
            model.MultiFamilyDemographicDetail.OutdoorSpas = multifamily.OutdoorSpas;
            model.MultiFamilyDemographicDetail.OutdoorSpasOptional = multifamily.OutdoorSpasOptional;
            model.MultiFamilyDemographicDetail.ParkingRatioRequisites = multifamily.ParkingRatioRequisites;
            model.MultiFamilyDemographicDetail.PlaygroundArea = multifamily.PlaygroundArea;
            model.MultiFamilyDemographicDetail.PlaygroundAreaOptional = multifamily.PlaygroundAreaOptional;
            model.MultiFamilyDemographicDetail.Pools = multifamily.Pools;
            model.MultiFamilyDemographicDetail.PoolsOptional = multifamily.PoolsOptional;
            model.MultiFamilyDemographicDetail.RequiresParkingStalls = multifamily.RequiresParkingStalls;
            model.MultiFamilyDemographicDetail.RoofingFlatBuiltUp = multifamily.RoofingFlatBuiltUp;
            model.MultiFamilyDemographicDetail.RoofingShingleOnly = multifamily.RoofingShingleOnly;
            model.MultiFamilyDemographicDetail.RoofingTileOnly = multifamily.RoofingTileOnly;
            model.MultiFamilyDemographicDetail.SecurityGates = multifamily.SecurityGates;
            model.MultiFamilyDemographicDetail.SecurityGatesOptional = multifamily.SecurityGatesOptional;
            model.MultiFamilyDemographicDetail.SeparateClubhouse = multifamily.SeparateClubhouse;
            model.MultiFamilyDemographicDetail.SeparateClubhouseOptional = multifamily.SeparateClubhouseOptional;
            model.MultiFamilyDemographicDetail.SeparateOfficeBuilding = multifamily.SeparateOfficeBuilding;
            model.MultiFamilyDemographicDetail.SeparateOfficeBuildingOptional = multifamily.SeparateOfficeBuildingOptional;
            model.MultiFamilyDemographicDetail.SingleWideSpaceRatioForAllSpaces = multifamily.SingleWideSpaceRatioForAllSpaces;
            model.MultiFamilyDemographicDetail.TenantOnly = multifamily.TenantOnly;
            model.MultiFamilyDemographicDetail.TenantProfileRestrictions = multifamily.TenantProfileRestrictions;
            model.MultiFamilyDemographicDetail.TennisCourts = multifamily.TennisCourts;
            model.MultiFamilyDemographicDetail.TennisCourtsOptional = multifamily.TennisCourtsOptional;
            model.MultiFamilyDemographicDetail.TurnKey = multifamily.TurnKey;
            model.MultiFamilyDemographicDetail.TurnKeyOptional = multifamily.TurnKeyOptional;
            model.MultiFamilyDemographicDetail.UnderperformingProperty = multifamily.UnderperformingProperty;
            model.MultiFamilyDemographicDetail.UnderperformingPropertyOptional = multifamily.UnderperformingPropertyOptional;
            model.MultiFamilyDemographicDetail.MaxRatioOfOneBedroomUnits = multifamily.MaxRatioOfOneBedroomUnits;

            var other = context.SearchCriteriaDemographicDetails.Single(w => w.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && w.Type == "Other");
            model.OtherDemographicDetail = new OtherDemographicDetailModel();
            model.OtherDemographicDetail.SquareFootageRangeMax = other.SquareFootageRangeMax;
            model.OtherDemographicDetail.SquareFootageRangeMin = other.SquareFootageRangeMin;
            model.OtherDemographicDetail.TenantRequisites = other.TenantRequisites;
            model.OtherDemographicDetail.SingleTenantPadsRequisiteDetails = other.SingleTenantPadsRequisiteDetails;
            model.OtherDemographicDetail.RequiresSingleTenantPads = other.RequiresSingleTenantPads;
            model.OtherDemographicDetail.RequiresCoveredParkingStalls = other.RequiresCoveredParkingStalls;
            model.OtherDemographicDetail.PropertyRequiresMajorTenant = other.PropertyRequiresMajorTenant;
            model.OtherDemographicDetail.PropertyRequiresMajorTenantOptional = other.PropertyRequiresMajorTenantOptional;
            model.OtherDemographicDetail.WillLookAtUnifinishedSuites = other.WillLookAtUnifinishedSuites;
            model.OtherDemographicDetail.WillLookAtUnifinishedSuitesOptional = other.WillLookAtUnifinishedSuitesOptional;
            model.OtherDemographicDetail.CanBeVacant = other.CanBeVacant;
            model.OtherDemographicDetail.CanHaveExtensiveRenovationNeeds = other.CanHaveExtensiveRenovationNeeds;
            model.OtherDemographicDetail.CanHaveExtensiveRenovationNeedsOptional = other.CanHaveExtensiveRenovationNeedsOptional;
            model.OtherDemographicDetail.MaxVacancy = other.MaxVacancy;
            model.OtherDemographicDetail.MinimumForAccreditedTenantProfiles = other.MinimumForAccreditedTenantProfiles;
            model.OtherDemographicDetail.MultiLevelAcceptable = other.MultiLevelAcceptable;
            model.OtherDemographicDetail.MultiLevelAcceptableOptional = other.MultiLevelAcceptableOptional;
            model.OtherDemographicDetail.OfficeMedicalMixedUseCoveredParkingStallsRequired = other.OfficeMedicalMixedUseCoveredParkingStallsRequired;
            model.OtherDemographicDetail.OfficeMedicalMixedUseCoveredParkingStallsRequiredOptional = other.OfficeMedicalMixedUseCoveredParkingStallsRequiredOptional;
            model.OtherDemographicDetail.OtherRequirements = other.OtherRequirements;
            model.OtherDemographicDetail.NoAgePropertyPreference = other.NoAgePropertyPreference;

            model.GeographicParameters = new GeographicParametersModel();
            model.GeographicParameters.Interests = new List<GeographicParameterInterestModel>();
            var interests = context.SearchCriteriaGeographicParameters.Where(w => w.AssetSeachCriteriaId == assetSearch.AssetSearchCriteriaId).GroupBy(w => w.InterestState);
            foreach (var interest in interests)
            {
                var cityInt = new GeographicParameterInterestModel()
                {
                    StateOfInterest = interest.Key,
                    Cities = interest.Select(s => s.InterestCity).ToList()
                };
                for (int i = cityInt.Cities.Count; i < 5; i++)
                {
                    cityInt.Cities.Add(" ");
                }
                model.GeographicParameters.Interests.Add(cityInt);
            }
            for (int i = interests.Count(); i < 3; i++)
            {
                model.GeographicParameters.Interests.Add(new GeographicParameterInterestModel());
            }

            var due = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Site Images");
            model.DueDiligenceCheckList.SiteImages = due.ImportanceLevel;

            var due1 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Property Specifications");
            model.DueDiligenceCheckList.PropertySpecifications = due1.ImportanceLevel;

            var due2 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Prior Years Operating Records");
            model.DueDiligenceCheckList.PriorYearsOperatingReports = due2.ImportanceLevel;

            var due3 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Current YTD Operating Reports");
            model.DueDiligenceCheckList.CurrentYTDOperatingReports = due3.ImportanceLevel;

            var due4 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Current Rent Rolls");
            model.DueDiligenceCheckList.CurrentRentRolls = due4.ImportanceLevel;

            var due5 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Tenant Leases (Commercial Asset)");
            model.DueDiligenceCheckList.TenantLeases = due5.ImportanceLevel;

            var due6 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Old Appraisal");
            model.DueDiligenceCheckList.OldAppraisals = due6.ImportanceLevel;

            var due7 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Current Year Appraisal");
            model.DueDiligenceCheckList.CurrentYearAppraisal = due7.ImportanceLevel;

            var due8 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Property Inspection Report (Independent)");
            model.DueDiligenceCheckList.PropertyInspectionReport = due8.ImportanceLevel;

            var due9 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Phase I Environment Report (date importance)");
            model.DueDiligenceCheckList.Phase1EnvironmentReport = due9.ImportanceLevel;

            var due10 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Preliminary Title Report");
            model.DueDiligenceCheckList.PreliminaryTitleReport = due10.ImportanceLevel;

            var due11 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Original Title Policy Report of Current Owner");
            model.DueDiligenceCheckList.OriginalTitlePolicyReport = due11.ImportanceLevel;

            var others = context.SearchCriteriaDueDiligenceItems.Where(w => !_dueDiligenceList.Contains(w.ItemDescription));
            model.DueDiligenceCheckList.OtherInspectionItems = new List<DueDiligenceOptionalModel>();
            foreach (var o in others)
            {
                if (model.DueDiligenceCheckList.OtherInspectionItems.Count(w => w.Text == o.ItemDescription) == 0)
                {
                    model.DueDiligenceCheckList.OtherInspectionItems.Add(new DueDiligenceOptionalModel()
                    {
                        ImportanceLevel = o.ImportanceLevel,
                        Text = o.ItemDescription
                    });
                }
            }
            return model;
        }

        public void UpdateAssetSearchCriteria(Domain.ViewModel.AssetSearchCriteriaModel model)
        {
            var context = _factory.Create();

            var assetSearch = context.AssetSearchCriterias.Single(s => s.AssetSearchCriteriaId == model.AssetSearchCriteriaId);
            assetSearch.UserId = model.UserId;
            assetSearch.LastUpdated = DateTime.Now;
            assetSearch.WebsiteURLVestingCorporateEntity = model.WebsiteURLVestingCorporateEntity;
            assetSearch.NameOfOtherCorporateOfficer2 = model.NameOfOtherCorporateOfficer2;
            assetSearch.AddressLine1OfPurchasingEntity = model.AddressLine1OfPurchasingEntity;
            assetSearch.AddressLine2OfPurchasingEntity = model.AddressLine2OfPurchasingEntity;
            assetSearch.AmountOfIntendedCapEquity = model.AmountOfIntendedCapEquity;
            assetSearch.BrandNewDetails = model.FinancialInvestmentParameters.BrandNewDetails;
            assetSearch.CellNumberOfEntity = model.CellNumberOfEntity;
            assetSearch.CityOfPurchasingEntity = model.CityOfPurchasingEntity;
            assetSearch.ContactNumberOfCorporateOfficer = model.ContactNumberOfCorporateOfficer;
            assetSearch.ContactNumberOfOtherCorporateOfficer = model.ContactNumberOfOtherCorporateOfficer;
            assetSearch.CREPMFBrokerLender = model.CREPMFBrokerLender;
            assetSearch.EmailAddressOfCorporateOfficer = model.EmailAddressOfCorporateOfficer;
            assetSearch.EmailAddressOfEntity = model.EmailAddressOfEntity;
            assetSearch.EmailAddressOfOtherCorporateOfficer = model.EmailAddressOfOtherCorporateOfficer;
            assetSearch.FaxNumberOfEntity = model.FaxNumberOfEntity;
            assetSearch.GeneralNotesOfVestingEntity = model.GeneralNotesOfVestingEntity;
            if (model.HasEntityRaisedIntendedCap.HasValue)
            {
                assetSearch.HasEntityRaisedIntendedCap = model.HasEntityRaisedIntendedCap.Value;
            }
            assetSearch.InvestmentFundingRangeMax = model.FinancialInvestmentParameters.InvestmentFundingRangeMax;
            assetSearch.InvestmentFundingRangeMin = model.FinancialInvestmentParameters.InvestmentFundingRangeMin;
            assetSearch.IsCorporateEntityInGoodStanding = model.IsCorporateEntityInGoodStanding.GetValueOrDefault(false);
            assetSearch.LeverageTarget = model.LeverageTarget;
            assetSearch.ManagingOfficerOfEntity = model.ManagingOfficerOfEntity;
            assetSearch.MinimumCapitalizationRate = model.FinancialInvestmentParameters.MinimumCapitalizationRate;
            assetSearch.NameOfOtherCorporateOfficer = model.NameOfOtherCorporateOfficer;
            assetSearch.NameOfPurchasingEntity = model.NameOfPurchasingEntity;
            assetSearch.OfficeNumberOfEntity = model.OfficeNumberOfEntity;
            assetSearch.OtherCorporateOfficer = model.OtherCorporateOfficer;
            assetSearch.PartiallyBuiltDetails = model.FinancialInvestmentParameters.PartiallyBuiltDetails;
            assetSearch.ProFormaParametersDetails = model.FinancialInvestmentParameters.ProFormaParametersDetails;
            assetSearch.SecuredCLAPOF = model.SecuredCLAPOF.HasValue ? model.SecuredCLAPOF.Value : false;
            assetSearch.StateOfIncorporation = model.StateOfIncorporation;
            assetSearch.StateOfPurchasingEntity = model.StateOfPurchasingEntity;
            assetSearch.TargetPricePerSpaceMax = model.FinancialInvestmentParameters.TargetPricePerSpaceMax;
            assetSearch.TargetPricePerSpaceMin = model.FinancialInvestmentParameters.TargetPricePerSpaceMin;
            assetSearch.TargetPricePerUnitMax = model.FinancialInvestmentParameters.TargetPricePerUnitMax;
            assetSearch.TargetPricePerUnitMin = model.FinancialInvestmentParameters.TargetPricePerUnitMin;
            assetSearch.TermsSought = model.TermsSought;
            assetSearch.TimelineSecuringCap = model.TimelineSecuringCap;
            assetSearch.TypeOfAssetsSought = model.TypeOfAssetsSought;
            assetSearch.TypeOfPurchasingEntity = model.TypeOfPurchasingEntity;
            assetSearch.UtilizePMFunding = model.UtilizePMFunding.HasValue ? model.UtilizePMFunding.Value : false;
            assetSearch.WillConsiderBrandNew = model.FinancialInvestmentParameters.WillConsiderBrandNew.GetValueOrDefault(false);
            assetSearch.WillConsiderPartiallyBuilt = model.FinancialInvestmentParameters.WillConsiderPartiallyBuilt.GetValueOrDefault(false);
            assetSearch.WillConsiderUnperformingAtPricing = model.FinancialInvestmentParameters.WillConsiderUnperformingAtPricing.GetValueOrDefault(false);
            assetSearch.ZipOfPurchasingEntity = model.ZipOfPurchasingEntity;

            context.Save();

            var demo = context.SearchCriteriaDemographicDetails.Single(s => s.Type == "MultiFamily" && s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId);
            demo.AssetSearchCriteriaId = assetSearch.AssetSearchCriteriaId;
            demo.AssetSearchCriteria = assetSearch;
            demo.AcceptsEFFUnits = model.MultiFamilyDemographicDetail.AcceptsEFFUnits.HasValue ? model.MultiFamilyDemographicDetail.AcceptsEFFUnits.Value : false;
            demo.AcceptsOneBedroomUnits = model.MultiFamilyDemographicDetail.AcceptsOneBedroomUnits.HasValue ? model.MultiFamilyDemographicDetail.AcceptsOneBedroomUnits.Value : false;
            demo.AgeOfPropertyMaximum = model.MultiFamilyDemographicDetail.AgeOfPropertyMaximum;
            demo.DoubleWideSpaceRatioForAllSpaces = model.MultiFamilyDemographicDetail.DoubleWideSpaceRatioForAllSpaces;
            demo.ExtensiveRenovationUpdatingNeeds = model.MultiFamilyDemographicDetail.ExtensiveRenovationUpdatingNeeds.HasValue ? model.MultiFamilyDemographicDetail.ExtensiveRenovationUpdatingNeeds.Value : false;
            demo.ExtensiveRenovationUpdatingNeedsOptional = model.MultiFamilyDemographicDetail.ExtensiveRenovationUpdatingNeedsOptional.HasValue ? model.MultiFamilyDemographicDetail.ExtensiveRenovationUpdatingNeedsOptional.Value : false;
            demo.GradeClassificationRequirementOfProperty = model.MultiFamilyDemographicDetail.GradeClassificationRequirementOfProperty;
            demo.GymFacilitiesForAdults = model.MultiFamilyDemographicDetail.GymFacilitiesForAdults.HasValue ? model.MultiFamilyDemographicDetail.GymFacilitiesForAdults.Value : false;
            demo.GymFacilitiesForAdultsOptional = model.MultiFamilyDemographicDetail.GymFacilitiesForAdultsOptional.HasValue ? model.MultiFamilyDemographicDetail.GymFacilitiesForAdultsOptional.Value : false;
            demo.HasParkingRatioParameters = model.MultiFamilyDemographicDetail.HasParkingRatioParameters.GetValueOrDefault(false);
            demo.MasterMetering = model.MultiFamilyDemographicDetail.MasterMetering;
            demo.MaxRatioOfEFfUnits = model.MultiFamilyDemographicDetail.MaxRatioOfEFfUnits;
            demo.MultiLevelFourLevelsAcceptable = model.MultiFamilyDemographicDetail.MultiLevelFourLevelsAcceptable;
            demo.MultiLevelOtherLevelsAcceptable = model.MultiFamilyDemographicDetail.MultiLevelOtherLevelsAcceptable;
            demo.MultiLevelThreeLevelsAcceptable = model.MultiFamilyDemographicDetail.MultiLevelThreeLevelsAcceptable;
            demo.MultiLevelTwoLevelsAcceptable = model.MultiFamilyDemographicDetail.MultiLevelTwoLevelsAcceptable;
            demo.NumberOfUnitsRangeMaximum = model.MultiFamilyDemographicDetail.NumberOfUnitsRangeMaximum;
            demo.NumberOfUnitsRangeMinimum = model.MultiFamilyDemographicDetail.NumberOfUnitsRangeMinimum;
            demo.OtherRequirements = model.MultiFamilyDemographicDetail.OtherRequirements;
            demo.OutdoorSpas = model.MultiFamilyDemographicDetail.OutdoorSpas.HasValue ? model.MultiFamilyDemographicDetail.OutdoorSpas.Value : false;
            demo.OutdoorSpasOptional = model.MultiFamilyDemographicDetail.OutdoorSpasOptional.HasValue ? model.MultiFamilyDemographicDetail.OutdoorSpasOptional.Value : false;
            demo.ParkingRatioRatioRequisites = model.MultiFamilyDemographicDetail.ParkingRatioRequisites;
            demo.ParkingRatioRequisites = model.MultiFamilyDemographicDetail.ParkingRatioRequisites;
            demo.PlaygroundArea = model.MultiFamilyDemographicDetail.PlaygroundArea.HasValue ? model.MultiFamilyDemographicDetail.PlaygroundArea.Value : false;
            demo.PlaygroundAreaOptional = model.MultiFamilyDemographicDetail.PlaygroundAreaOptional.HasValue ? model.MultiFamilyDemographicDetail.PlaygroundAreaOptional.Value : false;
            demo.Pools = model.MultiFamilyDemographicDetail.Pools.HasValue ? model.MultiFamilyDemographicDetail.Pools.Value : false;
            demo.PoolsOptional = model.MultiFamilyDemographicDetail.PoolsOptional.HasValue ? model.MultiFamilyDemographicDetail.PoolsOptional.Value : false;
            demo.RequiresParkingStalls = model.MultiFamilyDemographicDetail.RequiresParkingStalls.HasValue ? model.MultiFamilyDemographicDetail.RequiresParkingStalls.Value : false;
            demo.RoofingFlatBuiltUp = model.MultiFamilyDemographicDetail.RoofingFlatBuiltUp;
            demo.RoofingShingleOnly = model.MultiFamilyDemographicDetail.RoofingShingleOnly;
            demo.RoofingTileOnly = model.MultiFamilyDemographicDetail.RoofingTileOnly;
            demo.SecurityGates = model.MultiFamilyDemographicDetail.SecurityGates.HasValue ? model.MultiFamilyDemographicDetail.SecurityGates.Value : false;
            demo.SecurityGatesOptional = model.MultiFamilyDemographicDetail.SecurityGatesOptional.HasValue ? model.MultiFamilyDemographicDetail.SecurityGatesOptional.Value : false;
            demo.SeparateClubhouse = model.MultiFamilyDemographicDetail.SeparateClubhouse.HasValue ? model.MultiFamilyDemographicDetail.SeparateClubhouse.Value : false;
            demo.SeparateClubhouseOptional = model.MultiFamilyDemographicDetail.SeparateClubhouseOptional.HasValue ? model.MultiFamilyDemographicDetail.SeparateClubhouseOptional.Value : false;
            demo.SeparateOfficeBuilding = model.MultiFamilyDemographicDetail.SeparateOfficeBuilding.HasValue ? model.MultiFamilyDemographicDetail.SeparateOfficeBuilding.Value : false;
            demo.SeparateOfficeBuildingOptional = model.MultiFamilyDemographicDetail.SeparateOfficeBuildingOptional.HasValue ? model.MultiFamilyDemographicDetail.SeparateOfficeBuildingOptional.Value : false;
            demo.SingleWideSpaceRatioForAllSpaces = model.MultiFamilyDemographicDetail.SingleWideSpaceRatioForAllSpaces;
            demo.TenantOnly = model.MultiFamilyDemographicDetail.TenantOnly;
            demo.TenantProfileRestrictions = model.MultiFamilyDemographicDetail.TenantProfileRestrictions;
            demo.TennisCourts = model.MultiFamilyDemographicDetail.TennisCourts.HasValue ? model.MultiFamilyDemographicDetail.TennisCourts.Value : false;
            demo.TennisCourtsOptional = model.MultiFamilyDemographicDetail.TennisCourtsOptional.HasValue ? model.MultiFamilyDemographicDetail.TennisCourtsOptional.Value : false;
            demo.TurnKey = model.MultiFamilyDemographicDetail.TurnKey.HasValue ? model.MultiFamilyDemographicDetail.TurnKey.Value : false;
            demo.TurnKeyOptional = model.MultiFamilyDemographicDetail.TurnKeyOptional.HasValue ? model.MultiFamilyDemographicDetail.TurnKeyOptional.Value : false;
            demo.UnderperformingProperty = model.MultiFamilyDemographicDetail.UnderperformingProperty.HasValue ? model.MultiFamilyDemographicDetail.UnderperformingProperty.Value : false;
            demo.UnderperformingPropertyOptional = model.MultiFamilyDemographicDetail.UnderperformingPropertyOptional.HasValue ? model.MultiFamilyDemographicDetail.UnderperformingPropertyOptional.Value : false;
            context.Save();

            demo = context.SearchCriteriaDemographicDetails.Single(s => s.Type == "Other" && s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId);
            demo.AssetSearchCriteriaId = assetSearch.AssetSearchCriteriaId;
            demo.AssetSearchCriteria = assetSearch;
            demo.SquareFootageRangeMax = model.OtherDemographicDetail.SquareFootageRangeMax;
            demo.SquareFootageRangeMin = model.OtherDemographicDetail.SquareFootageRangeMin;
            demo.TenantRequisites = model.OtherDemographicDetail.TenantRequisites;
            demo.SingleTenantPadsRequisiteDetails = model.OtherDemographicDetail.SingleTenantPadsRequisiteDetails;
            demo.RequiresSingleTenantPads = model.OtherDemographicDetail.RequiresSingleTenantPads.HasValue ? model.OtherDemographicDetail.RequiresSingleTenantPads.Value : false;
            demo.RequiresCoveredParkingStalls = model.OtherDemographicDetail.RequiresCoveredParkingStalls;
            demo.PropertyRequiresMajorTenant = model.OtherDemographicDetail.PropertyRequiresMajorTenant.HasValue ? model.OtherDemographicDetail.PropertyRequiresMajorTenant.Value : false;
            demo.PropertyRequiresMajorTenantOptional = model.OtherDemographicDetail.PropertyRequiresMajorTenantOptional.GetValueOrDefault(false);
            demo.WillLookAtUnifinishedSuites = model.OtherDemographicDetail.WillLookAtUnifinishedSuites.HasValue ? model.OtherDemographicDetail.WillLookAtUnifinishedSuites.Value : false;
            demo.WillLookAtUnifinishedSuitesOptional = model.OtherDemographicDetail.WillLookAtUnifinishedSuitesOptional.HasValue ? model.OtherDemographicDetail.WillLookAtUnifinishedSuitesOptional.Value : false;
            demo.CanBeVacant = model.OtherDemographicDetail.CanBeVacant.HasValue ? model.OtherDemographicDetail.CanBeVacant.Value : false;
            demo.CanHaveExtensiveRenovationNeeds = model.OtherDemographicDetail.CanHaveExtensiveRenovationNeeds.HasValue ? model.OtherDemographicDetail.CanHaveExtensiveRenovationNeeds.Value : false;
            demo.CanHaveExtensiveRenovationNeedsOptional = model.OtherDemographicDetail.CanHaveExtensiveRenovationNeedsOptional.HasValue ? model.OtherDemographicDetail.CanHaveExtensiveRenovationNeedsOptional.Value : false;
            demo.MaxRatioOfOneBedroomUnits = model.MultiFamilyDemographicDetail.MaxRatioOfOneBedroomUnits;
            demo.MaxVacancy = model.OtherDemographicDetail.MaxVacancy;
            demo.MinimumForAccreditedTenantProfiles = model.OtherDemographicDetail.MinimumForAccreditedTenantProfiles;
            demo.MultiLevelAcceptableOptional = model.OtherDemographicDetail.MultiLevelAcceptableOptional.GetValueOrDefault(false);
            demo.MultiLevelAcceptable = model.OtherDemographicDetail.MultiLevelAcceptable.HasValue ? model.OtherDemographicDetail.MultiLevelAcceptable.Value : false;
            demo.OfficeMedicalMixedUseCoveredParkingStallsRequired = model.OtherDemographicDetail.OfficeMedicalMixedUseCoveredParkingStallsRequired.HasValue ? model.OtherDemographicDetail.OfficeMedicalMixedUseCoveredParkingStallsRequired.Value : false;
            demo.OfficeMedicalMixedUseCoveredParkingStallsRequiredOptional = model.OtherDemographicDetail.OfficeMedicalMixedUseCoveredParkingStallsRequiredOptional.HasValue ? model.OtherDemographicDetail.OfficeMedicalMixedUseCoveredParkingStallsRequiredOptional.Value : false;
            demo.OtherRequirements = model.OtherDemographicDetail.OtherRequirements;
            demo.NoAgePropertyPreference = model.OtherDemographicDetail.NoAgePropertyPreference.GetValueOrDefault(false);
            context.Save();

            var geos = context.SearchCriteriaGeographicParameters.Where(w => w.AssetSeachCriteriaId == assetSearch.AssetSearchCriteriaId).ToList();
            foreach (var geo in geos)
            {
                context.SearchCriteriaGeographicParameters.Remove(geo);
                context.Save();
            }
            foreach (var item in model.GeographicParameters.Interests)
            {
                foreach (var city in item.Cities)
                {
                    if (!string.IsNullOrEmpty(city))
                    {
                        // create one
                        assetSearch.GeographicParameters.Add(new SearchCriteriaGeographicParameter()
                        {
                            AssetSearchCriteria = assetSearch,
                            AdditionalRequirements = model.GeographicParameters.AdditionalRequirements,
                            InterestCity = city,
                            InterestState = item.StateOfInterest,
                            Restrictions = model.GeographicParameters.Restrictions,
                            AssetSeachCriteriaId = assetSearch.AssetSearchCriteriaId
                        });
                    }
                }
            }
            context.Save();

            var due = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Site Images");
            due.ImportanceLevel = model.DueDiligenceCheckList.SiteImages;

            var due1 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Property Specifications");
            due1.ImportanceLevel = model.DueDiligenceCheckList.PropertySpecifications;

            var due2 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Prior Years Operating Records");
            due2.ImportanceLevel = model.DueDiligenceCheckList.PriorYearsOperatingReports;

            var due3 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Current YTD Operating Reports");
            due3.ImportanceLevel = model.DueDiligenceCheckList.CurrentYTDOperatingReports;

            var due4 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Current Rent Rolls");
            due4.ImportanceLevel = model.DueDiligenceCheckList.CurrentRentRolls;

            var due5 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Tenant Leases (Commercial Asset)");
            due5.ImportanceLevel = model.DueDiligenceCheckList.TenantLeases;

            var due6 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Old Appraisal");
            due6.ImportanceLevel = model.DueDiligenceCheckList.OldAppraisals;

            var due7 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Current Year Appraisal");
            due7.ImportanceLevel = model.DueDiligenceCheckList.CurrentYearAppraisal;

            var due8 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Property Inspection Report (Independent)");
            due8.ImportanceLevel = model.DueDiligenceCheckList.PropertyInspectionReport;

            var due9 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Phase I Environment Report (date importance)");
            due9.ImportanceLevel = model.DueDiligenceCheckList.Phase1EnvironmentReport;

            var due10 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Preliminary Title Report");
            due10.ImportanceLevel = model.DueDiligenceCheckList.PreliminaryTitleReport;

            var due11 = context.SearchCriteriaDueDiligenceItems.Single(s => s.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && s.ItemDescription == "Original Title Policy Report of Current Owner");
            due11.ImportanceLevel = model.DueDiligenceCheckList.OriginalTitlePolicyReport;


            foreach (var other in model.DueDiligenceCheckList.OtherInspectionItems)
            {
                if (!string.IsNullOrEmpty(other.Text) && other.Text != "Optional" && context.SearchCriteriaDueDiligenceItems.Count(w => w.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && w.ItemDescription == other.Text) > 0)
                {
                    var desc = context.SearchCriteriaDueDiligenceItems.Single(w => w.AssetSearchCriteriaId == assetSearch.AssetSearchCriteriaId && w.ItemDescription == other.Text);
                    desc.ImportanceLevel = other.ImportanceLevel;
                    context.Save();
                }
                else if (other.Text != "Optional" && !string.IsNullOrEmpty(other.Text))
                {
                    assetSearch.DueDiligenceItems.Add(new SearchCriteriaDueDiligenceItem()
                    {
                        AssetSearchCriteria = assetSearch,
                        AssetSearchCriteriaId = assetSearch.AssetSearchCriteriaId,
                        ImportanceLevel = other.ImportanceLevel,
                        ItemDescription = other.Text
                    });
                }
            }
            context.Save();
        }

        public int CreateAssetSearchCriteria(Domain.ViewModel.AssetSearchCriteriaModel model)
        {
            var context = _factory.Create();
            var assetSearch = new AssetSearchCriteria();
            assetSearch.DateEntered = DateTime.Now;
            assetSearch.LastUpdated = DateTime.Now;
            assetSearch.UserId = model.UserId;
            assetSearch.WebsiteURLVestingCorporateEntity = model.WebsiteURLVestingCorporateEntity;
            assetSearch.NameOfOtherCorporateOfficer2 = model.NameOfOtherCorporateOfficer2;
            assetSearch.AddressLine1OfPurchasingEntity = model.AddressLine1OfPurchasingEntity;
            assetSearch.AddressLine2OfPurchasingEntity = model.AddressLine2OfPurchasingEntity;
            assetSearch.AmountOfIntendedCapEquity = model.AmountOfIntendedCapEquity;
            assetSearch.BrandNewDetails = model.FinancialInvestmentParameters.BrandNewDetails;
            assetSearch.CellNumberOfEntity = model.CellNumberOfEntity;
            assetSearch.CityOfPurchasingEntity = model.CityOfPurchasingEntity;
            assetSearch.ContactNumberOfCorporateOfficer = model.ContactNumberOfCorporateOfficer;
            assetSearch.ContactNumberOfOtherCorporateOfficer = model.ContactNumberOfOtherCorporateOfficer;
            assetSearch.CREPMFBrokerLender = model.CREPMFBrokerLender;
            assetSearch.EmailAddressOfCorporateOfficer = model.EmailAddressOfCorporateOfficer;
            assetSearch.EmailAddressOfEntity = model.EmailAddressOfEntity;
            assetSearch.EmailAddressOfOtherCorporateOfficer = model.EmailAddressOfOtherCorporateOfficer;
            assetSearch.FaxNumberOfEntity = model.FaxNumberOfEntity;
            assetSearch.GeneralNotesOfVestingEntity = model.GeneralNotesOfVestingEntity;
            assetSearch.HasEntityRaisedIntendedCap = model.HasEntityRaisedIntendedCap.HasValue ? model.HasEntityRaisedIntendedCap.Value : false;
            assetSearch.InvestmentFundingRangeMax = model.FinancialInvestmentParameters.InvestmentFundingRangeMax;
            assetSearch.InvestmentFundingRangeMin = model.FinancialInvestmentParameters.InvestmentFundingRangeMin;
            assetSearch.IsCorporateEntityInGoodStanding = model.IsCorporateEntityInGoodStanding.GetValueOrDefault(false);
            assetSearch.LeverageTarget = model.LeverageTarget;
            assetSearch.ManagingOfficerOfEntity = model.ManagingOfficerOfEntity;
            assetSearch.MinimumCapitalizationRate = model.FinancialInvestmentParameters.MinimumCapitalizationRate;
            assetSearch.NameOfOtherCorporateOfficer = model.NameOfOtherCorporateOfficer;
            assetSearch.NameOfPurchasingEntity = model.NameOfPurchasingEntity;
            assetSearch.OfficeNumberOfEntity = model.OfficeNumberOfEntity;
            assetSearch.OtherCorporateOfficer = model.OtherCorporateOfficer;
            assetSearch.PartiallyBuiltDetails = model.FinancialInvestmentParameters.PartiallyBuiltDetails;
            assetSearch.ProFormaParametersDetails = model.FinancialInvestmentParameters.ProFormaParametersDetails;
            assetSearch.SecuredCLAPOF = model.SecuredCLAPOF.HasValue ? model.SecuredCLAPOF.Value : false;
            assetSearch.StateOfIncorporation = model.StateOfIncorporation;
            assetSearch.StateOfPurchasingEntity = model.StateOfPurchasingEntity;
            assetSearch.TargetPricePerSpaceMax = model.FinancialInvestmentParameters.TargetPricePerSpaceMax;
            assetSearch.TargetPricePerSpaceMin = model.FinancialInvestmentParameters.TargetPricePerSpaceMin;
            assetSearch.TargetPricePerUnitMax = model.FinancialInvestmentParameters.TargetPricePerUnitMax;
            assetSearch.TargetPricePerUnitMin = model.FinancialInvestmentParameters.TargetPricePerUnitMin;
            assetSearch.TermsSought = model.TermsSought;
            assetSearch.TimelineSecuringCap = model.TimelineSecuringCap;
            assetSearch.TypeOfAssetsSought = model.TypeOfAssetsSought;
            assetSearch.TypeOfPurchasingEntity = model.TypeOfPurchasingEntity;
            assetSearch.UtilizePMFunding = model.UtilizePMFunding.HasValue ? model.UtilizePMFunding.Value : false;
            assetSearch.WillConsiderBrandNew = model.FinancialInvestmentParameters.WillConsiderBrandNew.GetValueOrDefault(false);
            assetSearch.WillConsiderPartiallyBuilt = model.FinancialInvestmentParameters.WillConsiderPartiallyBuilt.GetValueOrDefault(false);
            assetSearch.WillConsiderUnperformingAtPricing = model.FinancialInvestmentParameters.WillConsiderUnperformingAtPricing.GetValueOrDefault(false);
            assetSearch.ZipOfPurchasingEntity = model.ZipOfPurchasingEntity;

            context.AssetSearchCriterias.Add(assetSearch);
            context.Save();

            var demo = new SearchCriteriaDemographicDetail();
            demo.AssetSearchCriteriaId = assetSearch.AssetSearchCriteriaId;
            demo.AssetSearchCriteria = assetSearch;
            demo.Type = "MultiFamily";
            demo.AcceptsEFFUnits = model.MultiFamilyDemographicDetail.AcceptsEFFUnits.HasValue ? model.MultiFamilyDemographicDetail.AcceptsEFFUnits.Value : false;
            demo.AcceptsOneBedroomUnits = model.MultiFamilyDemographicDetail.AcceptsOneBedroomUnits.HasValue ? model.MultiFamilyDemographicDetail.AcceptsOneBedroomUnits.Value : false;
            demo.AgeOfPropertyMaximum = model.MultiFamilyDemographicDetail.AgeOfPropertyMaximum;
            demo.DoubleWideSpaceRatioForAllSpaces = model.MultiFamilyDemographicDetail.DoubleWideSpaceRatioForAllSpaces;
            demo.ExtensiveRenovationUpdatingNeeds = model.MultiFamilyDemographicDetail.ExtensiveRenovationUpdatingNeeds.HasValue ? model.MultiFamilyDemographicDetail.ExtensiveRenovationUpdatingNeeds.Value : false;
            demo.ExtensiveRenovationUpdatingNeedsOptional = model.MultiFamilyDemographicDetail.ExtensiveRenovationUpdatingNeedsOptional.HasValue ? model.MultiFamilyDemographicDetail.ExtensiveRenovationUpdatingNeedsOptional.Value : false;
            demo.GradeClassificationRequirementOfProperty = model.MultiFamilyDemographicDetail.GradeClassificationRequirementOfProperty;
            demo.GymFacilitiesForAdults = model.MultiFamilyDemographicDetail.GymFacilitiesForAdults.HasValue ? model.MultiFamilyDemographicDetail.GymFacilitiesForAdults.Value : false;
            demo.GymFacilitiesForAdultsOptional = model.MultiFamilyDemographicDetail.GymFacilitiesForAdultsOptional.HasValue ? model.MultiFamilyDemographicDetail.GymFacilitiesForAdultsOptional.Value : false;
            demo.HasParkingRatioParameters = model.MultiFamilyDemographicDetail.HasParkingRatioParameters.GetValueOrDefault(false);
            demo.MasterMetering = model.MultiFamilyDemographicDetail.MasterMetering;
            demo.MaxRatioOfEFfUnits = model.MultiFamilyDemographicDetail.MaxRatioOfEFfUnits;
            demo.MultiLevelFourLevelsAcceptable = model.MultiFamilyDemographicDetail.MultiLevelFourLevelsAcceptable;
            demo.MultiLevelOtherLevelsAcceptable = model.MultiFamilyDemographicDetail.MultiLevelOtherLevelsAcceptable;
            demo.MultiLevelThreeLevelsAcceptable = model.MultiFamilyDemographicDetail.MultiLevelThreeLevelsAcceptable;
            demo.MultiLevelTwoLevelsAcceptable = model.MultiFamilyDemographicDetail.MultiLevelTwoLevelsAcceptable;
            demo.NumberOfUnitsRangeMaximum = model.MultiFamilyDemographicDetail.NumberOfUnitsRangeMaximum;
            demo.NumberOfUnitsRangeMinimum = model.MultiFamilyDemographicDetail.NumberOfUnitsRangeMinimum;
            demo.OtherRequirements = model.MultiFamilyDemographicDetail.OtherRequirements;
            demo.OutdoorSpas = model.MultiFamilyDemographicDetail.OutdoorSpas.HasValue ? model.MultiFamilyDemographicDetail.OutdoorSpas.Value : false;
            demo.OutdoorSpasOptional = model.MultiFamilyDemographicDetail.OutdoorSpasOptional.HasValue ? model.MultiFamilyDemographicDetail.OutdoorSpasOptional.Value : false;
            demo.ParkingRatioRatioRequisites = model.MultiFamilyDemographicDetail.ParkingRatioRequisites;
            demo.ParkingRatioRequisites = model.MultiFamilyDemographicDetail.ParkingRatioRequisites;
            demo.PlaygroundArea = model.MultiFamilyDemographicDetail.PlaygroundArea.HasValue ? model.MultiFamilyDemographicDetail.PlaygroundArea.Value : false;
            demo.PlaygroundAreaOptional = model.MultiFamilyDemographicDetail.PlaygroundAreaOptional.HasValue ? model.MultiFamilyDemographicDetail.PlaygroundAreaOptional.Value : false;
            demo.Pools = model.MultiFamilyDemographicDetail.Pools.HasValue ? model.MultiFamilyDemographicDetail.Pools.Value : false;
            demo.PoolsOptional = model.MultiFamilyDemographicDetail.PoolsOptional.HasValue ? model.MultiFamilyDemographicDetail.PoolsOptional.Value : false;
            demo.RequiresParkingStalls = model.MultiFamilyDemographicDetail.RequiresParkingStalls.HasValue ? model.MultiFamilyDemographicDetail.RequiresParkingStalls.Value : false;
            demo.RoofingFlatBuiltUp = model.MultiFamilyDemographicDetail.RoofingFlatBuiltUp;
            demo.RoofingShingleOnly = model.MultiFamilyDemographicDetail.RoofingShingleOnly;
            demo.RoofingTileOnly = model.MultiFamilyDemographicDetail.RoofingTileOnly;
            demo.SecurityGates = model.MultiFamilyDemographicDetail.SecurityGates.HasValue ? model.MultiFamilyDemographicDetail.SecurityGates.Value : false;
            demo.SecurityGatesOptional = model.MultiFamilyDemographicDetail.SecurityGatesOptional.HasValue ? model.MultiFamilyDemographicDetail.SecurityGatesOptional.Value : false;
            demo.SeparateClubhouse = model.MultiFamilyDemographicDetail.SeparateClubhouse.HasValue ? model.MultiFamilyDemographicDetail.SeparateClubhouse.Value : false;
            demo.SeparateClubhouseOptional = model.MultiFamilyDemographicDetail.SeparateClubhouseOptional.HasValue ? model.MultiFamilyDemographicDetail.SeparateClubhouseOptional.Value : false;
            demo.SeparateOfficeBuilding = model.MultiFamilyDemographicDetail.SeparateOfficeBuilding.HasValue ? model.MultiFamilyDemographicDetail.SeparateOfficeBuilding.Value : false;
            demo.SeparateOfficeBuildingOptional = model.MultiFamilyDemographicDetail.SeparateOfficeBuildingOptional.HasValue ? model.MultiFamilyDemographicDetail.SeparateOfficeBuildingOptional.Value : false;
            demo.SingleWideSpaceRatioForAllSpaces = model.MultiFamilyDemographicDetail.SingleWideSpaceRatioForAllSpaces;
            demo.TenantOnly = model.MultiFamilyDemographicDetail.TenantOnly;
            demo.TenantProfileRestrictions = model.MultiFamilyDemographicDetail.TenantProfileRestrictions;
            demo.TennisCourts = model.MultiFamilyDemographicDetail.TennisCourts.HasValue ? model.MultiFamilyDemographicDetail.TennisCourts.Value : false;
            demo.TennisCourtsOptional = model.MultiFamilyDemographicDetail.TennisCourtsOptional.HasValue ? model.MultiFamilyDemographicDetail.TennisCourtsOptional.Value : false;
            demo.TurnKey = model.MultiFamilyDemographicDetail.TurnKey.HasValue ? model.MultiFamilyDemographicDetail.TurnKey.Value : false;
            demo.TurnKeyOptional = model.MultiFamilyDemographicDetail.TurnKeyOptional.HasValue ? model.MultiFamilyDemographicDetail.TurnKeyOptional.Value : false;
            demo.UnderperformingProperty = model.MultiFamilyDemographicDetail.UnderperformingProperty.HasValue ? model.MultiFamilyDemographicDetail.UnderperformingProperty.Value : false;
            demo.UnderperformingPropertyOptional = model.MultiFamilyDemographicDetail.UnderperformingPropertyOptional.HasValue ? model.MultiFamilyDemographicDetail.UnderperformingPropertyOptional.Value : false;
            context.SearchCriteriaDemographicDetails.Add(demo);
            context.Save();

            demo = new SearchCriteriaDemographicDetail();
            demo.Type = "Other";
            demo.AssetSearchCriteriaId = assetSearch.AssetSearchCriteriaId;
            demo.NoAgePropertyPreference = model.OtherDemographicDetail.NoAgePropertyPreference.GetValueOrDefault(false);
            demo.AssetSearchCriteria = assetSearch;
            demo.SquareFootageRangeMax = model.OtherDemographicDetail.SquareFootageRangeMax;
            demo.SquareFootageRangeMin = model.OtherDemographicDetail.SquareFootageRangeMin;
            demo.TenantRequisites = model.OtherDemographicDetail.TenantRequisites;
            demo.SingleTenantPadsRequisiteDetails = model.OtherDemographicDetail.SingleTenantPadsRequisiteDetails;
            demo.RequiresSingleTenantPads = model.OtherDemographicDetail.RequiresSingleTenantPads.HasValue ? model.OtherDemographicDetail.RequiresSingleTenantPads.Value : false;
            demo.RequiresCoveredParkingStalls = model.OtherDemographicDetail.RequiresCoveredParkingStalls;
            demo.PropertyRequiresMajorTenant = model.OtherDemographicDetail.PropertyRequiresMajorTenant.HasValue ? model.OtherDemographicDetail.PropertyRequiresMajorTenant.Value : false;
            demo.PropertyRequiresMajorTenantOptional = model.OtherDemographicDetail.PropertyRequiresMajorTenantOptional.GetValueOrDefault(false);
            demo.WillLookAtUnifinishedSuites = model.OtherDemographicDetail.WillLookAtUnifinishedSuites.HasValue ? model.OtherDemographicDetail.WillLookAtUnifinishedSuites.Value : false;
            demo.WillLookAtUnifinishedSuitesOptional = model.OtherDemographicDetail.WillLookAtUnifinishedSuitesOptional.HasValue ? model.OtherDemographicDetail.WillLookAtUnifinishedSuitesOptional.Value : false;
            demo.CanBeVacant = model.OtherDemographicDetail.CanBeVacant.HasValue ? model.OtherDemographicDetail.CanBeVacant.Value : false;
            demo.CanHaveExtensiveRenovationNeeds = model.OtherDemographicDetail.CanHaveExtensiveRenovationNeeds.HasValue ? model.OtherDemographicDetail.CanHaveExtensiveRenovationNeeds.Value : false;
            demo.CanHaveExtensiveRenovationNeedsOptional = model.OtherDemographicDetail.CanHaveExtensiveRenovationNeedsOptional.HasValue ? model.OtherDemographicDetail.CanHaveExtensiveRenovationNeedsOptional.Value : false;
            demo.MaxRatioOfOneBedroomUnits = model.MultiFamilyDemographicDetail.MaxRatioOfOneBedroomUnits;
            demo.MaxVacancy = model.OtherDemographicDetail.MaxVacancy;
            demo.MinimumForAccreditedTenantProfiles = model.OtherDemographicDetail.MinimumForAccreditedTenantProfiles;
            demo.MultiLevelAcceptable = model.OtherDemographicDetail.MultiLevelAcceptable.HasValue ? model.OtherDemographicDetail.MultiLevelAcceptable.Value : false;
            demo.MultiLevelAcceptableOptional = model.OtherDemographicDetail.MultiLevelAcceptableOptional.GetValueOrDefault(false);
            demo.OfficeMedicalMixedUseCoveredParkingStallsRequired = model.OtherDemographicDetail.OfficeMedicalMixedUseCoveredParkingStallsRequired.HasValue ? model.OtherDemographicDetail.OfficeMedicalMixedUseCoveredParkingStallsRequired.Value : false;
            demo.OfficeMedicalMixedUseCoveredParkingStallsRequiredOptional = model.OtherDemographicDetail.OfficeMedicalMixedUseCoveredParkingStallsRequiredOptional.HasValue ? model.OtherDemographicDetail.OfficeMedicalMixedUseCoveredParkingStallsRequiredOptional.Value : false;
            demo.OtherRequirements = model.OtherDemographicDetail.OtherRequirements;
            context.SearchCriteriaDemographicDetails.Add(demo);
            context.Save();

            foreach (var item in model.GeographicParameters.Interests)
            {
                foreach (var city in item.Cities)
                {
                    if (!string.IsNullOrEmpty(city))
                    {
                        context.SearchCriteriaGeographicParameters.Add(new SearchCriteriaGeographicParameter()
                        {
                            AssetSearchCriteria = assetSearch,
                            AdditionalRequirements = model.GeographicParameters.AdditionalRequirements,
                            InterestCity = city,
                            InterestState = item.StateOfInterest,
                            Restrictions = model.GeographicParameters.Restrictions,
                            AssetSeachCriteriaId = assetSearch.AssetSearchCriteriaId
                        });
                    }
                }
            }
            context.Save();

            context.SearchCriteriaDueDiligenceItems.Add(new SearchCriteriaDueDiligenceItem()
            {
                AssetSearchCriteria = assetSearch,
                AssetSearchCriteriaId = assetSearch.AssetSearchCriteriaId,
                ImportanceLevel = model.DueDiligenceCheckList.SiteImages,
                ItemDescription = "Site Images"
            });

            context.SearchCriteriaDueDiligenceItems.Add(new SearchCriteriaDueDiligenceItem()
            {
                AssetSearchCriteria = assetSearch,
                AssetSearchCriteriaId = assetSearch.AssetSearchCriteriaId,
                ImportanceLevel = model.DueDiligenceCheckList.PropertySpecifications,
                ItemDescription = "Property Specifications"
            });

            context.SearchCriteriaDueDiligenceItems.Add(new SearchCriteriaDueDiligenceItem()
            {
                AssetSearchCriteria = assetSearch,
                AssetSearchCriteriaId = assetSearch.AssetSearchCriteriaId,
                ImportanceLevel = model.DueDiligenceCheckList.PriorYearsOperatingReports,
                ItemDescription = "Prior Years Operating Records"
            });

            context.SearchCriteriaDueDiligenceItems.Add(new SearchCriteriaDueDiligenceItem()
            {
                AssetSearchCriteria = assetSearch,
                AssetSearchCriteriaId = assetSearch.AssetSearchCriteriaId,
                ImportanceLevel = model.DueDiligenceCheckList.CurrentYTDOperatingReports,
                ItemDescription = "Current YTD Operating Reports"
            });

            context.SearchCriteriaDueDiligenceItems.Add(new SearchCriteriaDueDiligenceItem()
            {
                AssetSearchCriteria = assetSearch,
                AssetSearchCriteriaId = assetSearch.AssetSearchCriteriaId,
                ImportanceLevel = model.DueDiligenceCheckList.CurrentRentRolls,
                ItemDescription = "Current Rent Rolls"
            });

            context.SearchCriteriaDueDiligenceItems.Add(new SearchCriteriaDueDiligenceItem()
            {
                AssetSearchCriteria = assetSearch,
                AssetSearchCriteriaId = assetSearch.AssetSearchCriteriaId,
                ImportanceLevel = model.DueDiligenceCheckList.TenantLeases,
                ItemDescription = "Tenant Leases (Commercial Asset)"
            });

            context.SearchCriteriaDueDiligenceItems.Add(new SearchCriteriaDueDiligenceItem()
            {
                AssetSearchCriteria = assetSearch,
                AssetSearchCriteriaId = assetSearch.AssetSearchCriteriaId,
                ImportanceLevel = model.DueDiligenceCheckList.OldAppraisals,
                ItemDescription = "Old Appraisal"
            });

            context.SearchCriteriaDueDiligenceItems.Add(new SearchCriteriaDueDiligenceItem()
            {
                AssetSearchCriteria = assetSearch,
                AssetSearchCriteriaId = assetSearch.AssetSearchCriteriaId,
                ImportanceLevel = model.DueDiligenceCheckList.CurrentYearAppraisal,
                ItemDescription = "Current Year Appraisal"
            });

            context.SearchCriteriaDueDiligenceItems.Add(new SearchCriteriaDueDiligenceItem()
            {
                AssetSearchCriteria = assetSearch,
                AssetSearchCriteriaId = assetSearch.AssetSearchCriteriaId,
                ImportanceLevel = model.DueDiligenceCheckList.PropertyInspectionReport,
                ItemDescription = "Property Inspection Report (Independent)"
            });

            context.SearchCriteriaDueDiligenceItems.Add(new SearchCriteriaDueDiligenceItem()
            {
                AssetSearchCriteria = assetSearch,
                AssetSearchCriteriaId = assetSearch.AssetSearchCriteriaId,
                ImportanceLevel = model.DueDiligenceCheckList.Phase1EnvironmentReport,
                ItemDescription = "Phase I Environment Report (date importance)"
            });

            context.SearchCriteriaDueDiligenceItems.Add(new SearchCriteriaDueDiligenceItem()
            {
                AssetSearchCriteria = assetSearch,
                AssetSearchCriteriaId = assetSearch.AssetSearchCriteriaId,
                ImportanceLevel = model.DueDiligenceCheckList.PreliminaryTitleReport,
                ItemDescription = "Preliminary Title Report"
            });

            context.SearchCriteriaDueDiligenceItems.Add(new SearchCriteriaDueDiligenceItem()
            {
                AssetSearchCriteria = assetSearch,
                AssetSearchCriteriaId = assetSearch.AssetSearchCriteriaId,
                ImportanceLevel = model.DueDiligenceCheckList.OriginalTitlePolicyReport,
                ItemDescription = "Original Title Policy Report of Current Owner"
            });
            foreach (var item in model.DueDiligenceCheckList.OtherInspectionItems)
            {
                if (item.Text != "Optional" && !string.IsNullOrEmpty(item.Text))
                {
                    context.SearchCriteriaDueDiligenceItems.Add(new SearchCriteriaDueDiligenceItem()
                    {
                        AssetSearchCriteria = assetSearch,
                        AssetSearchCriteriaId = assetSearch.AssetSearchCriteriaId,
                        ImportanceLevel = item.ImportanceLevel,
                        ItemDescription = item.Text
                    });
                }
            }
            context.Save();
            return assetSearch.AssetSearchCriteriaId;
        }

        public AssetDescriptionModel GetAssetByAssetNumber(int assetNumber)
        {
            var returnAsset = new AssetDescriptionModel();
            var context = _factory.Create();
            var asset = context.Assets.SingleOrDefault(s => s.AssetNumber == assetNumber && s.IsActive);
            if (asset != null)
            {
                returnAsset.CurrentListingStatus = asset.ListingStatus;
                returnAsset.AssetId = asset.AssetId;
                returnAsset.AssetNumber = asset.AssetNumber;
                returnAsset.Show = asset.Show;
                returnAsset.HasCSAAgreement = asset.DateOfCsaConfirm.HasValue;
                returnAsset.ProjectName = asset.ProjectName;
                returnAsset.CityStateFormattedString = string.Format("{0}, {1}", asset.City, asset.State);
                returnAsset.AssetAddressOneLineFormattedString = string.Format("{0} {1}, {2} {3}", asset.PropertyAddress, asset.City, asset.State, asset.Zip);

                if (asset.AssetType == AssetType.MultiFamily || asset.AssetType == AssetType.MHP)
                {
                    var converted = asset as MultiFamilyAsset;
                    returnAsset.Description = string.Format("A {0} unit {1} property in {2}, {3}", converted.TotalUnits, EnumHelper.GetEnumDescription(asset.AssetType), asset.City, asset.State);
                }
                else
                {
                    var converted = asset as CommercialAsset;
                    returnAsset.Description = string.Format("A {0} {1} property in {2}, {3}", converted.LotSize, EnumHelper.GetEnumDescription(asset.AssetType), asset.City, asset.State);
                }
            }
            else
            {
                return null;
            }
            return returnAsset;
        }

        public void ChangeListingStatus(Guid assetId, Domain.Enum.ListingStatus newStatus)
        {
            var context = _factory.Create();
            var asset = context.Assets.Single(s => s.AssetId == assetId && s.IsActive);
            asset.ListingStatus = newStatus;
            context.Save();
        }

        public void SendChangeListingStatusEmails(ChangeListingStatusModel model)
        {
            // send emails to general population -- ? i think this means those who have viewed the asset

            // send emails to specific principals -- those that match the investor questionnaire
        }


        public List<AssetQuickListViewModel> GetAssetQuickViewList(AssetSearchModel model)
        {
            var context = _factory.Create();
            List<Asset> tempAssets = new List<Asset>();
            var assets = context.Assets.Where(w => w.Show && w.ListingStatus != ListingStatus.SoldAndClosed && w.ListingStatus != ListingStatus.SoldNotClosed && w.ListingStatus != ListingStatus.Withdrawn && w.IsActive).ToList();
            if (model.IsPaper)
            {
                // Only filtering if this is true. If we filtered when this value is false, we would exclude paper assets by default
                assets = assets.Where(a => a.IsPaper == model.IsPaper).ToList();
            }
            //var assets = context.Assets.ToList();
            if (model.MaxAgeRange.HasValue)
            {
                if (model.MaxAgeRange.Value > 0)
                {
                    var maxYear = DateTime.Now.AddYears(-model.MaxAgeRange.Value);
                    assets = assets.Where(w => w.YearBuilt >= maxYear.Year).ToList();
                }
            }
            if (model.SelectedAssetCategory.HasValue)
            {
                assets = assets.Where(w => w.AssetCategory == model.SelectedAssetCategory.Value).ToList();
            }
            if (model.SelectedAssetType.HasValue)
            {
                assets = assets.Where(w => w.AssetType == model.SelectedAssetType.Value).ToList();
            }
            if (model.SelectedListingStatus.HasValue)
            {
                assets = assets.Where(w => w.ListingStatus == model.SelectedListingStatus.Value).ToList();
            }
            if (model.AssetName != null)
            {
                assets = assets.Where(w => w.ProjectName != null && w.ProjectName.ToLower().Contains(model.AssetName.ToLower())).ToList();
            }
            if (model.AssetIds.Count > 0)
            {
                tempAssets = new List<Asset>(); // list to replace assets
                foreach (var id in model.AssetIds)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        int assetNumber = 0;
                        if (int.TryParse(id, out assetNumber))
                        {
                            var specificAsset = assets.Where(x => x.AssetNumber == assetNumber).ToList();
                            if (specificAsset.Count > 0) tempAssets.AddRange(specificAsset);
                        }
                    }
                }
                assets = tempAssets.ToList();
            }

            // Version III
            if (model.SelectedStates.Count > 0)
            {
                tempAssets = new List<Asset>(); // list to replace assets
                var oldCount = 0;
                var searchedSomething = false;
                foreach (var state in model.SelectedStates)
                {
                    if (tempAssets.Count != oldCount)
                    {
                        tempAssets = tempAssets.Distinct().ToList();
                        oldCount = tempAssets.Count;
                    }
                    if (!string.IsNullOrEmpty(state.State))
                    {
                        searchedSomething = true;
                        bool searchedSomethingInAState = false;
                        var stateTempAssets = assets.Where(x => x.State == state.State).ToList();
                        if (state.Counties.Any(x => !string.IsNullOrEmpty(x)))
                        {
                            searchedSomethingInAState = true;
                            state.Counties = state.Counties.Where(x => !string.IsNullOrEmpty(x)).ToList().ConvertAll(x => x.ToLower()).Distinct().ToList();
                            foreach (var c in state.Counties)
                            {
                                tempAssets.AddRange(stateTempAssets.Where(x => !string.IsNullOrEmpty(x.County) && x.County.ToLower().Contains(c)));
                            }
                        }
                        if (state.Cities.Any(x => !string.IsNullOrEmpty(x)))
                        {
                            searchedSomethingInAState = true;
                            state.Cities = state.Cities.Where(x => !string.IsNullOrEmpty(x)).ToList().ConvertAll(x => x.ToLower()).Distinct().ToList();
                            foreach (var c in state.Cities)
                            {
                                tempAssets.AddRange(stateTempAssets.Where(x => x.City.ToLower().Contains(c)));
                            }
                        }
                        if (!searchedSomethingInAState)
                            tempAssets.AddRange(stateTempAssets);
                    }
                    else
                    {
                        if (state.Counties.Any(x => !string.IsNullOrEmpty(x)))
                        {
                            searchedSomething = true;
                            state.Counties = state.Counties.Where(x => !string.IsNullOrEmpty(x)).ToList().ConvertAll(x => x.ToLower()).Distinct().ToList();
                            foreach (var c in state.Counties)
                            {
                                tempAssets.AddRange(assets.Where(x => !string.IsNullOrEmpty(x.County) && x.County.ToLower().Contains(c))); // county can be null
                            }
                        }
                        if (state.Cities.Any(x => !string.IsNullOrEmpty(x)))
                        {
                            searchedSomething = true;
                            state.Cities = state.Cities.Where(x => !string.IsNullOrEmpty(x)).ToList().ConvertAll(x => x.ToLower()).Distinct().ToList();
                            foreach (var c in state.Cities)
                            {
                                tempAssets.AddRange(assets.Where(x => x.City.ToLower().Contains(c)));
                            }
                        }
                    }
                }
                if (searchedSomething)
                    assets = tempAssets.Distinct().ToList();
            }

            if (!string.IsNullOrEmpty(model.State))
            {
                assets = assets.Where(a => a.State == model.State).ToList();
            }
            if (!string.IsNullOrEmpty(model.City))
            {
                assets = assets.Where(a => a.City == model.City).ToList();
            }
            // don't have turn key OR grade in assets to filter by
            // assets = assets.Where(t=>t.
            List<AssetQuickListViewModel> assetList = new List<AssetQuickListViewModel>();

            foreach (var asset in assets.ToList())
            {
                bool add = true;
                if (model.MaxPriceRange.HasValue)
                {
                    if (asset.AskingPrice > 0 && !asset.IsTBDMarket)
                    {
                        if (asset.AskingPrice > model.MaxPriceRange.Value)
                        {
                            add = false;
                        }
                    }
                    else
                    {
                        if (asset.CurrentBpo > model.MaxPriceRange.Value)
                        {
                            add = false;
                        }
                    }
                }
                if (model.MinPriceRange.HasValue)
                {
                    if (asset.AskingPrice > 0 && !asset.IsTBDMarket)
                    {
                        if (asset.AskingPrice < model.MinPriceRange.Value)
                        {
                            add = false;
                        }
                    }
                    else
                    {
                        if (asset.CurrentBpo > model.MinPriceRange.Value)
                        {
                            add = false;
                        }
                    }
                }
                if (asset.AssetType == AssetType.MultiFamily || asset.AssetType == AssetType.MHP)
                {
                    var mfAsset = (context.Assets.FirstOrDefault(s => s.AssetId == asset.AssetId && s.IsActive) as MultiFamilyAsset);
                    var proformaOccupancy = (100 - asset.CurrentVacancyFac) / 100;
                    string pricing = (asset.AskingPrice > 0 && !asset.IsTBDMarket) ? asset.AskingPrice.ToString("C0") : asset.CurrentBpo.ToString("C0");
                    int lastRecYear = 0;
                    if (mfAsset.LastReportedOccupancyDate.HasValue)
                        lastRecYear = mfAsset.LastReportedOccupancyDate.Value.Year;
                    int units = 0;
                    if (asset.AssetType == AssetType.MHP)
                    {
                        units = mfAsset.TotalUnits;
                        units += asset.NumberRentableSpace != null ? (int)asset.NumberRentableSpace : 0;
                        units += asset.NumberNonRentableSpace != null ? (int)asset.NumberNonRentableSpace : 0;
                    }
                    else
                        units = mfAsset.TotalUnits;

                    if (model.MaxUnitsSpaces.HasValue && model.MinUnitsSpaces.HasValue && (units < model.MinUnitsSpaces.Value || units > model.MaxUnitsSpaces.Value))
                    {
                        add = false;
                    }
                    if (add)
                    {
                        assetList.Add(new AssetQuickListViewModel()
                        {
                            Show = asset.Show,
                            AddressLine1 = asset.PropertyAddress,
                            AssetId = asset.AssetId,
                            AssetNumber = asset.AssetNumber,
                            AssetName = asset.ProjectName,
                            City = asset.City,
                            State = asset.State,
                            Type = getTypeAbbreviation(asset.AssetType),
                            Zip = asset.Zip,
                            Status = EnumHelper.GetEnumDescription(asset.ListingStatus),
                            CAP = (asset.CashInvestmentApy / 100).ToString("P2"),
                            NumberOfUnits = units,
                            Year = lastRecYear, // Changed from Year built (mfAsset.YearBuilt) to last recorded occupancy D - 2494 ;
                            SGI = asset.ProformaAnnualIncome.ToString("C0"),
                            Pricing = pricing,
                            Zoning = asset.AssetType == AssetType.MHP ? "MHP" : "MF",
                            OCC = proformaOccupancy.ToString("P0")
                        });
                    }

                }
                else
                {
                    var cAsset = (context.Assets.FirstOrDefault(s => s.AssetId == asset.AssetId && s.IsActive) as CommercialAsset);
                    var proformaSGI = cAsset.ProformaSgi;
                    var proformaNOI = proformaSGI - cAsset.ProformaAnnualOperExpenses;
                    string pricing = (asset.AskingPrice > 0 && !asset.IsTBDMarket) ? asset.AskingPrice.ToString("C0") : asset.CurrentBpo.ToString("C0");
                    string occ = cAsset.OccupancyPercentage.ToString() + " %";
                    if (occ.Contains('.'))
                        occ = occ.Split('.')[0] + " %";
                    if (model.MinSquareFeet.HasValue && model.MaxSquareFeet.HasValue && (cAsset.SquareFeet > model.MaxSquareFeet.Value || cAsset.SquareFeet < model.MinSquareFeet.Value))
                    {
                        add = false;
                    }
                    assetList.Add(new AssetQuickListViewModel()
                    {
                        AddressLine1 = asset.PropertyAddress,
                        AssetId = asset.AssetId,
                        AssetNumber = asset.AssetNumber,
                        AssetName = asset.ProjectName,
                        City = asset.City,
                        State = asset.State,
                        Type = getTypeAbbreviation(asset.AssetType),
                        Zip = asset.Zip,
                        Status = EnumHelper.GetEnumDescription(asset.ListingStatus),
                        CAP = (proformaNOI / cAsset.AskingPrice).ToString("P"),
                        NumberOfUnits = 0,
                        SquareFeet = cAsset.SquareFeet,
                        Year = cAsset.YearBuilt,
                        SGI = proformaSGI.ToString("C0"),
                        Pricing = pricing,
                        Zoning = EnumHelper.GetEnumDescription(cAsset.Type),
                        OCC = occ
                    });
                }
            }
            return assetList;
        }

        /// <summary>
        /// Change logic from going through Docusign process for each individual MDA but
        /// rather allow one MDA to be updated for each asset
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public bool SignedMDAWithAssetId(int userId, Guid assetId)
        {
            var context = _factory.Create();
            return context.AssetUserMDAs.Count(w => w.UserId == userId && w.AssetId == assetId) > 0;
        }

        public AssetViewModel GetAsset(Guid assetId, bool forScrollingImages)
        {
            var context = _factory.Create();
            var asset = context.Assets.FirstOrDefault(s => s.AssetId == assetId);
            if (asset == null)
            {
                return new AssetViewModel();
            }
            if (asset.AssetType == AssetType.MultiFamily || asset.AssetType == AssetType.MHP)
            {
                asset = (context.Assets.FirstOrDefault(s => s.AssetId == assetId) as MultiFamilyAsset);
            }
            else
            {
                asset = (context.Assets.FirstOrDefault(s => s.AssetId == assetId) as CommercialAsset);
                asset.ProformaAoeFactorAsPerOfSGI = (float)(asset.ProformaAnnualOperExpenses / asset.ProformaAnnualIncome) * 100;
            }


            foreach (var agent in asset.AssetNARMembers)
            {
                agent.SelectedNARMemberId = agent.NarMemberId.ToString();
            }

            var model = Mapper.Map<Asset, AssetViewModel>(asset);
            model.TypeOfPositionMortgage = EnumHelper.GetEnumDescription(asset.HasPositionMortgage);




            if (asset.AssetType == AssetType.MHP)
            {
                if ((model as MultiFamilyAssetViewModel).MHPUnitSpecifications.Count() == 0)
                {
                    context.AssetMHPSpecifications.Add(new AssetMHPSpecification()
                    {
                        AssetId = model.AssetId,
                        AssetMHPSpecificationId = Guid.NewGuid(),
                        CountOfUnits = 0,
                        CurrentDoubleBaseRent = 0,
                        CurrentSingleBaseRent = 0,
                        CurrentTripleBaseRent = 0,
                        NumberDoubleWide = 0,
                        NumberSingleWide = 0,
                        CurrentDoubleOwnedBaseRent = 0,
                        CurrentSingleOwnedBaseRent = 0,
                        CurrentTripleOwnedBaseRent = 0,
                        NumberDoubleWideOwned = 0,
                        NumberSingleWideOwned = 0,
                        NumberTripleWide = 0,
                        NumberTripleWideOwned = 0,
                    });
                    context.Save();

                }
            }

            if (asset.AssetType == AssetType.MultiFamily || asset.AssetType == AssetType.MHP)
            {
                var converted = asset as MultiFamilyAsset;
                model.Description = string.Format("A {0} unit {1} property in {2}, {3}", converted.TotalUnits, EnumHelper.GetEnumDescription(asset.AssetType), asset.City, asset.State);
            }
            else
            {
                var converted = asset as CommercialAsset;
                model.Description = string.Format("A {0} {1} property in {2}, {3}", converted.LotSize, EnumHelper.GetEnumDescription(asset.AssetType), asset.City, asset.State);
            }
            if (model.IsTBDMarket && forScrollingImages)
            {
                model.AskingPrice = model.CurrentBpo;
            }
            foreach (var doc in model.Documents)
            {
                switch (doc.Type)
                {
                    case (int)AssetDocumentType.ArialMap:
                        model.availablearialMap = true;
                        break;
                    case (int)AssetDocumentType.CurrentAppraisal:
                        model.availablecurrentAppraisal = true;
                        break;
                    case (int)AssetDocumentType.CurrentOperatingReport:
                        model.availablecurrentOperatingReport = true;
                        break;
                    case (int)AssetDocumentType.CurrentRentRoll:
                        model.availablecurrentRentRoll = true;
                        break;
                    case (int)AssetDocumentType.EnvironmentalReport:
                        model.AvailableEnvironmentalRep = true;
                        break;
                    case (int)AssetDocumentType.OriginalAppraisal:
                        model.availableoriginalAppraisal = true;
                        break;
                    case (int)AssetDocumentType.PlatMap:
                        model.availableplatMap = true;
                        break;
                    case (int)AssetDocumentType.PreliminaryTitleReport:
                        model.availablepreliminaryTitleReport = true;
                        break;
                    case (int)AssetDocumentType.PriorFiscalYearOperReport:
                        model.availablepriorFiscalYearOperReport = true;
                        break;
                    case (int)AssetDocumentType.ListingAgentMarketingBrochure:
                        model.availableListingAgentMarketingBrochure = true;
                        break;
                    case (int)AssetDocumentType.Other:
                        model.availableOtherDocument = true;
                        break;
                    case (int)AssetDocumentType.MortgageInstrumentOfRecord:
                        model.availableMortgageInstrumentRecord = true;
                        break;
                    case (int)AssetDocumentType.RecordedLiens:
                        model.availableRecordedLiens = true;
                        break;
                    case (int)AssetDocumentType.TaxLiens:
                        model.availableTaxLiens = true;
                        break;
                    case (int)AssetDocumentType.BKRelated:
                        model.availableBKRelated = true;
                        break;
                    case (int)AssetDocumentType.DOTMTG:
                        model.availableDOTMTG = true;
                        break;
                    case (int)AssetDocumentType.OtherTitle:
                        model.availableOtherTitle = true;
                        break;
                    case (int)AssetDocumentType.PreliminaryTitleReportTitle:
                        model.availablePreliminaryTitleReportTitle = true;
                        break;
                    case (int)AssetDocumentType.Insurance:
                        model.AvailableInsurance = true;
                        break;
                }
            }
            //TODO: ListingAgentInfo
            //var member = context.NarMembers.FirstOrDefault(s => asset.ListingAgentName.Contains(s.FirstName) && asset.ListingAgentName.Contains(s.LastName));
            //if (member != null)
            //{
            //    model.ListingAgentNewName = member.NarMemberId.ToString();
            //}
            if (asset.HasDeferredMaintenance.HasValue)
                model.HasDeferredMaintenance = asset.HasDeferredMaintenance.Value;
            model.DeferredMaintenanceItems = GetDefaultDeferredMaintenanceItems();
            model.DeferredMaintenanceItems = RemoveMaintainanceItems(model.AssetType, model.DeferredMaintenanceItems);
            model.DeferredMaintenanceItems = OrderMaintainanceItems(model.AssetType, model.DeferredMaintenanceItems);
            var items = context.AssetDeferredMaintenanceItems.Where(w => w.AssetId == asset.AssetId);
            var otherItemsNotIncluded = new List<AssetDeferredItemViewModel>();
            foreach (var nar in model.AssetNARMembers)
            {
                nar.SelectedNARMemberId = nar.NarMemberId.ToString();
            }
            foreach (var def in model.DeferredMaintenanceItems)
            {
                foreach (var item in items)
                {
                    if (item.MaintenanceDetail == def.MaintenanceDetail)
                    {
                        def.NumberOfUnits = item.Units;
                        def.UnitCost = item.UnitCost;
                        def.Selected = true;
                        def.ItemDescription = item.ItemDescription;
                        if (def.MaintenanceDetail == MaintenanceDetails.LeaseUpCommissions)
                            def.UnitTypeLabel = "Base Estimate Per List Agent/Ownership";
                        else if (def.MaintenanceDetail == MaintenanceDetails.SuiteBuildOut)
                            def.UnitTypeLabel = "Total Sq.Ft.";
                    }
                }
            }

            model.PropHoldTypeId = (int)asset.PropHoldTypeId;
            model.PropHoldType = EnumHelper.GetEnumDescription(asset.PropHoldTypeId); //Enum.GetName(typeof(PropHoldType), model.PropHoldTypeId);
            if (asset.PropLastUpdated.HasValue)
            {
                model.PropLastUpdatedYear = asset.PropLastUpdated.Value.Year;
                model.PropLastUpdated = asset.PropLastUpdated.Value;
            }
            if (asset.LeaseholdMaturityDate.HasValue)
                model.LeaseholdMaturityDate = asset.LeaseholdMaturityDate.Value;
            if (asset.AmortizationSchedule.HasValue)
                model.SelectedAmortSchedule = asset.AmortizationSchedule.Value.ToString();

            List<string> names = (from p in context.Portfolios
                                  join pa in context.PortfolioAssets on p.PortfolioId equals pa.PortfolioId
                                  where pa.AssetId == model.AssetId && p.isActive
                                  select p.PortfolioName).ToList();
            model.PortfolioNames = String.Join(", ", names);
            return model;
        }

        public List<AssetDeferredItemViewModel> OrderMaintainanceItems(AssetType type, List<AssetDeferredItemViewModel> maintainanceList)
        {
            List<AssetDeferredItemViewModel> reorderedList = new List<AssetDeferredItemViewModel>();
            // Re-Order items for commercial property - P:2939
            if (type == AssetType.Retail || type == AssetType.Office || type == AssetType.Industrial || type == AssetType.Medical)
            {
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.SuiteBuildOut).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.LeaseUpCommissions).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.Roofing).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.ExteriorRenovations).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.MasterHvacSystem).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.IndividualHvac).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.CoveredParkingInstall).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.CoveredParkingStructure).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.Fencing).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.Lighting).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.Landscaping).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.ParkingLot).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.Other).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.Other2).ToList());
                return reorderedList;
            }
            else if (type == AssetType.MHP)
            {
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.Roofing).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.ExteriorPainting).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.InteriorUnitPainting).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.MasterHvacSystem).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.IndividualHvac).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.IndividualAppliances).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.IndividualWasherDryer).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.IndividualFlooring).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.IndividualCabinetAndFixtures).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.CoveredParking).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.Fencing).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.Landscaping).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.PavementRepair).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.ExteriorLighting).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.ParkOwnedRepairs).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.FireDamage).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.FloodDamage).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.Other).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.Other2).ToList());
                return reorderedList;
            }
            else
            {
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.Roofing).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.ExteriorPainting).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.InteriorUnitPainting).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.MasterHvacSystem).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.IndividualHvac).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.IndividualAppliances).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.IndividualWasherDryer).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.IndividualFlooring).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.IndividualCabinetAndFixtures).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.CoveredParking).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.Fencing).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.Landscaping).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.FireDamage).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.FloodDamage).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.Other).ToList());
                reorderedList.AddRange(maintainanceList.Where(elem => elem.MaintenanceDetail == MaintenanceDetails.Other2).ToList());
                return reorderedList;
            }

        }

        public List<AssetDeferredItemViewModel> RemoveMaintainanceItems(AssetType type, List<AssetDeferredItemViewModel> maintainanceList)
        {
            // Remove items for commercial property - P:2939
            if (type == AssetType.Retail || type == AssetType.Office || type == AssetType.Industrial || type == AssetType.Medical)
            {
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.InteriorUnitPainting);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.IndividualWasherDryer);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.IndividualFlooring);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.IndividualAppliances);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.FireDamage);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.FloodDamage);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.CoveredParking);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.ExteriorPainting);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.IndividualCabinetAndFixtures);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.ParkOwnedRepairs);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.ExteriorLighting);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.PavementRepair);
            }
            else if (type == AssetType.MHP)
            {
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.SuiteBuildOut);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.LeaseUpCommissions);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.ExteriorRenovations);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.CoveredParkingInstall);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.CoveredParkingStructure);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.Lighting);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.ParkingLot);

            }
            else
            {
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.SuiteBuildOut);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.LeaseUpCommissions);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.ExteriorRenovations);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.CoveredParkingInstall);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.CoveredParkingStructure);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.Lighting);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.ParkingLot);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.ParkOwnedRepairs);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.ExteriorLighting);
                maintainanceList.RemoveAll(elem => elem.MaintenanceDetail == MaintenanceDetails.PavementRepair);
            }

            return maintainanceList;
        }

        public void DeleteAsset(Guid assetId)
        {
            var context = _factory.Create();
            var asset = context.Assets.SingleOrDefault(w => w.AssetId == assetId && w.IsActive);
            asset.IsActive = false;
            context.Save();
        }

        public void UnPublishAsset(Guid assetId)
        {
            var context = _factory.Create();
            var asset = context.Assets.SingleOrDefault(w => w.AssetId == assetId);
            asset.Show = false;
            context.Save();
        }

        public List<SignedMDAQuickViewModel> GetSignedMDAs(int userId)
        {
            var context = _factory.Create();
            var mdas = context.AssetUserMDAs.Include(w => w.Asset).Where(w => w.UserId == userId);
            var list = new List<SignedMDAQuickViewModel>();
            foreach (var mda in mdas)
            {
                var sign = new SignedMDAQuickViewModel()
                {
                    DateSigned = mda.SignMDADate,
                    AssetAddressLine1 = mda.Asset.PropertyAddress,
                    AssetCity = mda.Asset.City,
                    AssetId = mda.AssetId,
                    AssetNumber = mda.Asset.AssetNumber,
                    AssetState = mda.Asset.State,
                    AssetUserMDAId = mda.AssetUserMDAId,
                    PropertyType = EnumHelper.GetEnumDescription(mda.Asset.AssetType),
                    CurrentListingStatus = mda.Asset.ListingStatus
                };
                switch (mda.Asset.AssetType)
                {
                    case AssetType.MHP:
                        var mhp = mda.Asset as MultiFamilyAsset;
                        sign.UnitDescription = mhp.ParkOwnedMHUnits + " spaces";
                        break;
                    case AssetType.MultiFamily:
                        var mf = mda.Asset as MultiFamilyAsset;
                        sign.UnitDescription = mf.TotalUnits + " units";
                        break;
                    default:
                        var comm = mda.Asset as CommercialAsset;
                        sign.UnitDescription = comm.SquareFeet + " sq. ft.";
                        break;
                }
                if (mda.Asset.AssetType == AssetType.MultiFamily || mda.Asset.AssetType == AssetType.MHP)
                {
                    var converted = mda.Asset as MultiFamilyAsset;
                    sign.AssetDescription = string.Format("A {0} unit {1} property in {2}, {3}", converted.TotalUnits, EnumHelper.GetEnumDescription(mda.Asset.AssetType), mda.Asset.City, mda.Asset.State);
                }
                else
                {
                    var converted = mda.Asset as CommercialAsset;
                    sign.AssetDescription = string.Format("A {0} {1} property in {2}, {3}", converted.LotSize, EnumHelper.GetEnumDescription(mda.Asset.AssetType), mda.Asset.City, mda.Asset.State);
                }
                list.Add(sign);
            }
            return list;
        }

        public string GetSignedMDALocation(int assetUserMdaId)
        {
            var context = _factory.Create();
            return context.AssetUserMDAs.Single(s => s.AssetUserMDAId == assetUserMdaId).FileLocation;
        }

        public List<CarouselAssetData> GetCarouselData()
        {
            var list = new List<CarouselAssetData>();
            try
            {
                var context = _factory.Create();
                var assets = context.Assets.Where(w => w.Show && w.Images.Count > 0 && w.IsActive).OrderBy(s => Guid.NewGuid()).Take(5).ToList();
                foreach (var item in assets)
                {
                    var imgSrc = context.AssetImages.FirstOrDefault(s => s.AssetId == item.AssetId && s.IsMainImage);
                    string base64image = string.Empty;
                    if (imgSrc != null)
                    {
                        string path = Path.Combine(ConfigurationManager.AppSettings["DataRoot"], "Images", item.AssetId.ToString(), imgSrc.FileName);
                        Bitmap bitmap = ScaleImage(new Bitmap(path), 264, 264);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            bitmap.Save(ms, getFormat(imgSrc.ContentType));
                            byte[] bytes = ms.ToArray();
                            base64image = "data:" + imgSrc.ContentType + ";base64," + Convert.ToBase64String(bytes);
                        }
                    }
                    list.Add(new CarouselAssetData()
                    {
                        AskingPrice = (!item.IsTBDMarket || item.AskingPrice > 0) ? item.AskingPrice.ToString("C0") : item.CurrentBpo.ToString("C0"),
                        AssetId = item.AssetId,
                        AssetType = item.AssetType,
                        City = item.City,
                        State = item.State,
                        Image = base64image
                    });
                }
            }
            catch { }
            return list;
        }

        public AssetDescriptionModel GetAssetByAssetId(Guid assetId)
        {
            var context = _factory.Create();
            var asset = context.Assets.SingleOrDefault(s => s.AssetId == assetId && s.IsActive);
            var returnAsset = new AssetDescriptionModel();
            if (asset != null)
            {
                returnAsset.CurrentListingStatus = asset.ListingStatus;
                returnAsset.AssetId = asset.AssetId;
                returnAsset.AssetNumber = asset.AssetNumber;
                returnAsset.HasCSAAgreement = asset.DateOfCsaConfirm.HasValue;
                returnAsset.CityStateFormattedString = string.Format("{0}, {1}", asset.City, asset.State);
                returnAsset.AssetAddressOneLineFormattedString = string.Format("{0} {1}, {2} {3}", asset.PropertyAddress, asset.City, asset.State, asset.Zip);
                if (asset.AssetType == AssetType.MultiFamily || asset.AssetType == AssetType.MHP)
                {
                    var converted = asset as MultiFamilyAsset;
                    returnAsset.Description = string.Format("{0} unit {1} property in {2}, {3}", converted.TotalUnits, EnumHelper.GetEnumDescription(asset.AssetType), asset.City, asset.State);
                }
                else
                {
                    var converted = asset as CommercialAsset;
                    // remove letters from lotsize
                    string newLotSize = string.Empty;
                    if (!string.IsNullOrEmpty(converted.LotSize))
                    {
                        char[] arr = converted.LotSize.ToCharArray();
                        arr = Array.FindAll<char>(arr, (c => (char.IsDigit(c)
                            || c == '.'
                            || c == '-'
                            || char.IsWhiteSpace(c))));
                        newLotSize = new String(arr);
                    }
                    returnAsset.Description = string.Format("{0} acre {1} property in {2}, {3}", newLotSize, EnumHelper.GetEnumDescription(asset.AssetType), asset.City, asset.State);
                }
                StringBuilder sb = new StringBuilder();
                asset.AssetTaxParcelNumbers.ForEach(a =>
                {
                    sb.Append(a.TaxParcelNumber);
                    sb.Append("; ");
                });
                returnAsset.APNOneLine = sb.ToString();
                returnAsset.CorporateOwnershipOfficer = asset.CorporateOwnershipOfficer;
            }
            return returnAsset;
        }

        public bool UpdateAssetByViewModel(AssetViewModel model)
        {
            var context = _factory.Create();
            // handle assetnarmembers first, so the nar members would already be created before the next transaction?
            try
            {
                if (model.PropLastUpdatedYear != 0)
                    model.PropLastUpdated = new DateTime(model.PropLastUpdatedYear, 1, 1);

                var original = context.Assets.Find(model.AssetId);
                //foreach (var agent in model.AssetNARMembers)
                for (int i = model.AssetNARMembers.Count - 1; i >= 0; i--)
                {
                    if (model.AssetNARMembers[i].AssetNARMemberId == Guid.Empty)
                    {
                        // set AssetNARMemberId, they probably replaced the NAR Member
                        model.AssetNARMembers[i].AssetNARMemberId = Guid.NewGuid();

                        if (model.AssetNARMembers[i].NARMember.NotOnList)
                        {
                            // if they selected not on list, create nar member
                            if (model.AssetNARMembers[i].NarMemberId == 0)
                            {
                                // 6/3 if they selected not on list and its auto save, do not add the user
                                if (model.Method == "User")
                                {

                                    // 6/4 update nar member if they exist in the db
                                    string email = model.AssetNARMembers[i].NARMember.Email.ToLower();
                                    var oldNar = context.NarMembers.Where(x => x.Email != null && x.Email.Contains(email)).FirstOrDefault();
                                    if (oldNar != null)
                                    {
                                        // update nar and set the nar in the model
                                        oldNar.CellPhoneNumber = model.AssetNARMembers[i].NARMember.CellPhoneNumber;
                                        oldNar.CommissionAmount = model.AssetNARMembers[i].NARMember.CommissionAmount;
                                        oldNar.CommissionShareAgr = model.AssetNARMembers[i].NARMember.CommissionShareAgr;
                                        oldNar.CompanyAddressLine1 = model.AssetNARMembers[i].NARMember.CompanyAddressLine1;
                                        oldNar.CompanyAddressLine2 = model.AssetNARMembers[i].NARMember.CompanyAddressLine2;
                                        oldNar.CompanyCity = model.AssetNARMembers[i].NARMember.CompanyCity;
                                        oldNar.CompanyName = model.AssetNARMembers[i].NARMember.CompanyName;
                                        oldNar.CompanyState = model.AssetNARMembers[i].NARMember.CompanyState;
                                        oldNar.CompanyZip = model.AssetNARMembers[i].NARMember.CompanyZip;
                                        oldNar.DateOfCsaConfirm = model.AssetNARMembers[i].NARMember.DateOfCsaConfirm;
                                        oldNar.FaxNumber = model.AssetNARMembers[i].NARMember.FaxNumber;
                                        oldNar.FirstName = model.AssetNARMembers[i].NARMember.FirstName;
                                        oldNar.LastName = model.AssetNARMembers[i].NARMember.LastName;
                                        oldNar.NotOnList = false;
                                        oldNar.WorkPhoneNumber = model.AssetNARMembers[i].NARMember.WorkPhoneNumber;
                                        context.Entry(oldNar).State = EntityState.Modified;
                                        context.Save();
                                        model.AssetNARMembers[i].NARMember = oldNar;
                                    }
                                    else
                                    {
                                        // create the nar member now
                                        var member = model.AssetNARMembers[i].NARMember.Clone();
                                        context.NarMembers.Add(member);
                                        context.Save();
                                        model.AssetNARMembers[i].NARMember = member;
                                    }

                                }
                                else
                                {
                                    // remove this assetnar from the model until the user clicks save or submit
                                    model.AssetNARMembers.Remove(model.AssetNARMembers[i]);
                                }
                            }
                        }
                        else if (model.AssetNARMembers[i].AssetId == Guid.Empty)
                        {
                            // try removing the nar member for now, the user probably didnt set anything
                            // and this is the default
                            model.AssetNARMembers.Remove(model.AssetNARMembers[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            try
            {
                bool saved = false;
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                using (TransactionScope saveScope = new TransactionScope(TransactionScopeOption.Required, options))
                {

                    //5/27 NARMember Issues, not needed now 
                    //context.SendSqlToDebugWindow(true);
                    var original = context.Assets.Find(model.AssetId);

                    foreach (var item in model.DeferredMaintenanceItems)
                    {
                        if (item.Selected)
                        {
                            var record = context.AssetDeferredMaintenanceItems.FirstOrDefault(w => w.AssetId == model.AssetId && w.MaintenanceDetail == item.MaintenanceDetail);
                            if (record != null)
                            {
                                record.UnitCost = item.UnitCost;
                                record.Units = Convert.ToInt32(item.NumberOfUnits) == 0 ? 1 : Convert.ToInt32(item.NumberOfUnits);
                                record.ItemDescription = item.ItemDescription;
                            }
                            else
                            {
                                context.AssetDeferredMaintenanceItems.Add(new AssetDeferredMaintenanceItem()
                                {
                                    AssetId = model.AssetId,
                                    MaintenanceDetail = item.MaintenanceDetail,
                                    ItemDescription = item.ItemDescription,
                                    UnitCost = item.UnitCost,
                                    Units = Convert.ToInt32(item.NumberOfUnits) == 0 ? 1 : Convert.ToInt32(item.NumberOfUnits)
                                });
                            }
                        }
                        else
                        {
                            var record = context.AssetDeferredMaintenanceItems.FirstOrDefault(w => w.AssetId == model.AssetId && w.MaintenanceDetail == item.MaintenanceDetail);
                            if (record != null)
                            {
                                context.AssetDeferredMaintenanceItems.Remove(record);
                            }
                        }
                        context.Save();
                    }

                    var asset = Mapper.Map<AssetViewModel, Asset>(model);


                    foreach (var nar in asset.AssetNARMembers)
                    {
                        context.Entry(nar.NARMember).State = EntityState.Modified;
                    }
                    asset.IsSubmitted = model.IsSubmitted;
                    var existingAsset = context.Assets.FirstOrDefault(a => a.AssetId == asset.AssetId);
                    existingAsset.IsSubmitted = model.IsSubmitted;
                    var images = context.AssetImages.Where(w => w.AssetId == asset.AssetId).ToList();
                    foreach (var image in images)
                    {
                        context.AssetImages.Remove(image);
                    }
                    context.Save();
                    var docs = context.AssetDocuments.Where(w => w.AssetId == asset.AssetId).ToList();
                    foreach (var doc in docs)
                    {
                        context.AssetDocuments.Remove(doc);
                    }
                    var vids = context.AssetVideos.Where(w => w.AssetId == asset.AssetId).ToList();
                    foreach (var item in vids)
                    {
                        context.AssetVideos.Remove(item);
                    }
                    context.Save();
                    var units = context.AssetUnitSpecifications.Where(w => w.AssetId == asset.AssetId).ToList();
                    foreach (var unit in units)
                    {
                        context.AssetUnitSpecifications.Remove(unit);
                    }
                    var mhpUnits = context.AssetMHPSpecifications.Where(w => w.AssetId == asset.AssetId).ToList();
                    foreach (var unit in mhpUnits)
                    {
                        context.AssetMHPSpecifications.Remove(unit);
                    }
                    context.Save();
                    var agents = context.AssetNARMembers.Where(x => x.AssetId == asset.AssetId).ToList();
                    foreach (var agent in agents)
                    {
                        context.AssetNARMembers.Remove(agent);
                    }
                    context.Save();
                    foreach (var agent in asset.AssetNARMembers)
                    {
                        if (agent.AssetNARMemberId == Guid.Empty)
                        {
                            agent.AssetNARMemberId = Guid.NewGuid();
                        }
                    }

                    var taxParcelNumbers = context.AssetTaxParcelNumbers.Where(w => w.AssetId == asset.AssetId).ToList();
                    foreach (var num in taxParcelNumbers)
                    {
                        context.AssetTaxParcelNumbers.Remove(num);
                    }
                    context.Save();

                    existingAsset.PropHoldTypeId = (PropHoldType)model.PropHoldTypeId;
                    if (model.LeaseholdMaturityDate.HasValue)
                        existingAsset.LeaseholdMaturityDate = model.LeaseholdMaturityDate.Value;
                    if (model.PropLastUpdatedYear != 0)
                        existingAsset.PropLastUpdated = model.PropLastUpdated;
                    if (model.SelectedAmortSchedule != null) asset.AmortizationSchedule = Convert.ToInt32(model.SelectedAmortSchedule);
                    if (model.InterestRate != null) asset.InterestRate = model.InterestRate;
                    if (existingAsset != null)
                    {
                        if (asset.GetType() == typeof(MultiFamilyAsset))
                        {
                            var dbAsset = existingAsset as MultiFamilyAsset;
                            Mapper.Map<MultiFamilyAsset, MultiFamilyAsset>((asset as MultiFamilyAsset), (dbAsset as MultiFamilyAsset));
                        }
                        if (asset.GetType() == typeof(CommercialAsset))
                        {
                            var dbAsset = existingAsset as CommercialAsset;
                            Mapper.Map<CommercialAsset, CommercialAsset>((asset as CommercialAsset), (dbAsset as CommercialAsset));
                        }
                        context.Save();
                        saveScope.Complete();
                        saved = true;
                        return true;
                    }
                    if (!saved) saveScope.Complete();
                    return false;
                }

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public AssetViewModel PopulateDocumentOrder(AssetViewModel model)
        {
            // populate the document numbers if the conditions apply, if not remove any previously uploaded documents
            if (model.Documents.Where(x => x.Title != null && x.Title.Contains("Environmental")).Count() > 0)
            {

                int envNumber = model.Documents.FindIndex(x => x.Title.Contains("Environmental"));
                if (model.HasEnvironmentalIssues != null && model.HasEnvironmentalIssues == true)
                    model.DocumentNumberEnvi = envNumber;
                else
                    model.Documents.RemoveAt(envNumber);
            }
            if (model.Documents.Where(x => x.Title != null && x.Title.Contains("Original Appraisal")).Count() > 0)
            {
                int apprNumber = model.Documents.FindIndex(x => x.Title.Contains("Original Appraisal"));
                if (model.HasCopyOfAppraisal != null && model.HasCopyOfAppraisal.ToLower().Equals("yes"))
                    model.DocumentNumberOriAppr = apprNumber;
                else
                    model.Documents.RemoveAt(apprNumber);
            }
            return model;
        }

        public void SaveAssetVideos(List<AssetVideo> videos)
        {
            if (videos.Count() > 0)
            {
                var context = _factory.Create();

                //1.) remove all saved videos
                var foundVideos = context.AssetVideos.Where(v => v.AssetId == videos.First().AssetId).ToList();
                foundVideos.ForEach(v =>
                {
                    context.AssetVideos.Remove(v);
                });

                //2.) insert new videos
                videos.ForEach(v =>
                {
                    context.AssetVideos.Add(v);
                });
                context.Save();
            }
        }

        public int? CreateAssetByViewModel(AssetViewModel model)
        {
            try
            {
                var context = _factory.Create();
                var asset = Mapper.Map<AssetViewModel, Asset>(model);
                // remove AssetNARMembers because the NARMember will be duplicated
                asset.AssetNARMembers = new List<AssetNARMember>();
                asset.SquareFeet = model.SquareFeet;
                asset.AnnualGrossIncome = model.AnnualGrossIncome;
                // only set this value on create
                asset.ListedByUserId = model.ListedByUserId;
                asset.IsActive = true;
                var last = context.Assets.OrderByDescending(s => s.AssetNumber).FirstOrDefault();
                if (last != null)
                {
                    asset.AssetNumber = last.AssetNumber + 1;
                }
                else
                {
                    asset.AssetNumber = 1000;
                }

                asset.PropHoldTypeId = (PropHoldType)model.PropHoldTypeId;
                if (model.LeaseholdMaturityDate.HasValue)
                    asset.LeaseholdMaturityDate = model.LeaseholdMaturityDate.Value;
                if (model.PropLastUpdatedYear != 0)
                    asset.PropLastUpdated = new DateTime(model.PropLastUpdatedYear, 1, 1);
                if (model.SelectedAmortSchedule != null)
                    asset.AmortizationSchedule = Convert.ToInt32(model.SelectedAmortSchedule);

                context.Assets.Add(asset);
                context.Save();

                try
                {
                    // create portfolio if the user specified so in the Create Asset modal
                    if (model.isNewPf.HasValue && model.isNewPf.Value && !string.IsNullOrEmpty(model.NewPFName))
                    {
                        Portfolio pf = new Portfolio
                        {
                            PortfolioId = Guid.NewGuid(),
                            hasOffersDate = false,
                            isActive = true,
                            NumberofAssets = 1,
                            PortfolioName = model.NewPFName,
                            UserId = model.ListedByUserId
                        };
                        context.Portfolios.Add(pf);
                        context.Save();

                        PortfolioAsset pfa = new PortfolioAsset
                        {
                            PortfolioAssetId = Guid.NewGuid(),
                            PortfolioId = pf.PortfolioId,
                            isActive = true,
                            AssetId = asset.AssetId
                        };
                        context.PortfolioAssets.Add(pfa);
                        context.Save();
                    }
                    Guid pfId = new Guid();
                    if (!string.IsNullOrEmpty(model.ExistingPFName) && Guid.TryParse(model.ExistingPFName, out pfId))
                    {
                        var pf = context.Portfolios.FirstOrDefault(x => x.PortfolioId == pfId);
                        if (pf != null)
                        {
                            PortfolioAsset pfa = new PortfolioAsset
                            {
                                PortfolioAssetId = Guid.NewGuid(),
                                PortfolioId = pfId,
                                isActive = true,
                                AssetId = asset.AssetId
                            };
                            context.PortfolioAssets.Add(pfa);
                            context.Save();
                        }
                    }
                }
                catch { }

                try
                {
                    for (int i = model.AssetNARMembers.Count - 1; i >= 0; i--)
                    {
                        if (model.AssetNARMembers[i].AssetNARMemberId == Guid.Empty)
                        {
                            if (model.AssetNARMembers[i].NARMember.NotOnList && model.AssetNARMembers[i].NarMemberId == 0)
                            {
                                model.AssetNARMembers[i].SelectedNARMemberId = string.Empty;
                                model.AssetNARMembers[i].NARMember.NotOnList = false;

                                string email = model.AssetNARMembers[i].NARMember.Email.ToLower();
                                var oldNar = context.NarMembers.Where(x => x.Email != null && x.Email.Contains(email)).FirstOrDefault();
                                if (oldNar != null)
                                {
                                    // update nar and set the nar in the model
                                    oldNar.CellPhoneNumber = model.AssetNARMembers[i].NARMember.CellPhoneNumber;
                                    oldNar.CommissionAmount = model.AssetNARMembers[i].NARMember.CommissionAmount;
                                    oldNar.CommissionShareAgr = model.AssetNARMembers[i].NARMember.CommissionShareAgr;
                                    oldNar.CompanyAddressLine1 = model.AssetNARMembers[i].NARMember.CompanyAddressLine1;
                                    oldNar.CompanyAddressLine2 = model.AssetNARMembers[i].NARMember.CompanyAddressLine2;
                                    oldNar.CompanyCity = model.AssetNARMembers[i].NARMember.CompanyCity;
                                    oldNar.CompanyName = model.AssetNARMembers[i].NARMember.CompanyName;
                                    oldNar.CompanyState = model.AssetNARMembers[i].NARMember.CompanyState;
                                    oldNar.CompanyZip = model.AssetNARMembers[i].NARMember.CompanyZip;
                                    oldNar.DateOfCsaConfirm = model.AssetNARMembers[i].NARMember.DateOfCsaConfirm;
                                    oldNar.FaxNumber = model.AssetNARMembers[i].NARMember.FaxNumber;
                                    oldNar.FirstName = model.AssetNARMembers[i].NARMember.FirstName;
                                    oldNar.LastName = model.AssetNARMembers[i].NARMember.LastName;
                                    oldNar.NotOnList = false;
                                    oldNar.WorkPhoneNumber = model.AssetNARMembers[i].NARMember.WorkPhoneNumber;
                                    context.Entry(oldNar).State = EntityState.Modified;
                                    context.Save();

                                    var anm = new AssetNARMember();
                                    anm.AssetNARMemberId = Guid.NewGuid();
                                    anm.Asset = asset;
                                    anm.AssetId = asset.AssetId;
                                    anm.NARMember = oldNar;
                                    anm.NarMemberId = oldNar.NarMemberId;
                                    context.AssetNARMembers.Add(anm);
                                    context.Save();
                                }
                                else
                                {
                                    // create narmember first, then add to assetnarmember
                                    var member = model.AssetNARMembers[i].NARMember.Clone();
                                    context.NarMembers.Add(member);
                                    context.Save();


                                    var anm = new AssetNARMember();
                                    anm.AssetNARMemberId = Guid.NewGuid();
                                    anm.Asset = asset;
                                    anm.AssetId = asset.AssetId;
                                    anm.NARMember = member;
                                    anm.NarMemberId = member.NarMemberId;
                                    context.AssetNARMembers.Add(anm);
                                    context.Save();
                                }

                            }
                            else if (model.AssetNARMembers[i].NarMemberId > 0)
                            {
                                // update narmember just in case they changed something
                                var old = context.NarMembers.Find(model.AssetNARMembers[i].NarMemberId);
                                if (old != null)
                                {
                                    //context.Entry(old).CurrentValues.SetValues(model.AssetNARMembers[i].NARMember);

                                    old.CellPhoneNumber = model.AssetNARMembers[i].NARMember.CellPhoneNumber;
                                    old.CommissionAmount = model.AssetNARMembers[i].NARMember.CommissionAmount;
                                    old.CommissionShareAgr = model.AssetNARMembers[i].NARMember.CommissionShareAgr;
                                    old.CompanyAddressLine1 = model.AssetNARMembers[i].NARMember.CompanyAddressLine1;
                                    old.CompanyAddressLine2 = model.AssetNARMembers[i].NARMember.CompanyAddressLine2;
                                    old.CompanyCity = model.AssetNARMembers[i].NARMember.CompanyCity;
                                    old.CompanyName = model.AssetNARMembers[i].NARMember.CompanyName;
                                    old.CompanyState = model.AssetNARMembers[i].NARMember.CompanyState;
                                    old.CompanyZip = model.AssetNARMembers[i].NARMember.CompanyZip;
                                    old.DateOfCsaConfirm = model.AssetNARMembers[i].NARMember.DateOfCsaConfirm;
                                    old.FaxNumber = model.AssetNARMembers[i].NARMember.FaxNumber;
                                    old.FirstName = model.AssetNARMembers[i].NARMember.FirstName;
                                    old.LastName = model.AssetNARMembers[i].NARMember.LastName;
                                    old.NotOnList = false;
                                    old.WorkPhoneNumber = model.AssetNARMembers[i].NARMember.WorkPhoneNumber;
                                    context.Entry(old).State = EntityState.Modified;
                                    context.Save();
                                }

                                // create the assetnarmember
                                var anm = new AssetNARMember();
                                anm.AssetNARMemberId = Guid.NewGuid();
                                anm.Asset = asset;
                                anm.AssetId = asset.AssetId;
                                anm.NARMember = old;
                                anm.NarMemberId = old.NarMemberId;
                                context.AssetNARMembers.Add(anm);
                                context.Save();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //return null;
                }

                foreach (var item in model.DeferredMaintenanceItems)
                {
                    if (item.Selected)
                    {
                        var record = context.AssetDeferredMaintenanceItems.FirstOrDefault(w => w.AssetId == model.AssetId && w.MaintenanceDetail == item.MaintenanceDetail);
                        if (record != null)
                        {
                            record.UnitCost = item.UnitCost;
                            record.Units = Convert.ToInt32(item.NumberOfUnits);
                            record.ItemDescription = item.ItemDescription;
                        }
                        else
                        {
                            context.AssetDeferredMaintenanceItems.Add(new AssetDeferredMaintenanceItem()
                            {
                                AssetId = model.AssetId,
                                MaintenanceDetail = item.MaintenanceDetail,
                                UnitCost = item.UnitCost,
                                Units = Convert.ToInt32(item.NumberOfUnits),
                                ItemDescription = item.ItemDescription
                            });
                        }
                        context.Save();
                    }
                }
                return asset.AssetNumber;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool DoesAssetExist(int assetNumber)
        {
            var context = _factory.Create();
            return context.Assets.Count(w => w.AssetNumber == assetNumber) > 0;
        }

        public byte[] GetUserFile(int userfileId)
        {
            var context = _factory.Create();
            var file = context.UserFiles.SingleOrDefault(s => s.UserFileId == userfileId);
            if (file != null)
            {
                if (File.Exists(file.FileLocation))
                {
                    return File.ReadAllBytes(file.FileLocation);
                }
            }
            return null;
        }

        public void SaveUserFile(byte[] file, string description, int userId)
        {
            var context = _factory.Create();
            var path = Path.Combine(ConfigurationManager.AppSettings["pdfDirectory"], userId.ToString());
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, Guid.NewGuid().ToString() + ".pdf");
            System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create,
                                  System.IO.FileAccess.Write);
            // Writes a block of bytes to this stream using data from
            // a byte array.
            fs.Write(file, 0, file.Length);
            // close file stream
            fs.Close();
            context.UserFiles.Add(new UserFile()
            {
                DateUploaded = DateTime.Now,
                FileLocation = path,
                FileName = description,
                UserId = userId
            });
            context.Save();
        }

        public void DeleteUserFile(int userFileId)
        {
            var context = _factory.Create();
            context.UserFiles.Remove(context.UserFiles.Single(u => u.UserFileId == userFileId));
            context.Save();
        }

        public void DeleteAssetFile(string fileId, Guid assetId, FileType filetype)
        {
            var context = _factory.Create();
            context.AssetDocuments.Remove(context.AssetDocuments.Single(x => x.AssetId == assetId && x.FileName == fileId));
            context.Save();
        }

        public void DeleteUserNote(int userNoteId)
        {
            var context = _factory.Create();
            context.UserNotes.Remove(context.UserNotes.Single(s => s.UserNoteId == userNoteId));
            context.Save();
        }

        public void SaveUserNote(UserNoteModel model)
        {
            var context = _factory.Create();
            var note = context.UserNotes.Single(s => s.UserNoteId == model.UserNoteId);
            note.Notes = model.Note;
            context.Save();
        }

        public void CreateUserNote(UserNoteModel model)
        {
            var context = _factory.Create();
            context.UserNotes.Add(new UserNote()
            {
                CreateDate = DateTime.Now,
                Notes = model.Note,
                UserId = model.UserId
            });
            context.Save();
        }

        public UserNoteModel GetUserNote(int noteId)
        {
            var context = _factory.Create();
            var note = context.UserNotes.Single(s => s.UserNoteId == noteId);
            return new UserNoteModel()
            {
                Date = note.CreateDate,
                Note = note.Notes,
                UserId = note.UserId,
                UserNoteId = note.UserNoteId
            };
        }

        public void AssociateUserToSearchCriteriaForm(int userId, int formId)
        {
            var context = _factory.Create();
            var existing = context.AssetSearchCriterias.Single(s => s.AssetSearchCriteriaId == formId);
            existing.UserId = userId;
            context.Save();
        }

        public bool UserConfirmedAssetDisclosure(Guid assetId, int userid)
        {
            var context = _factory.Create();
            return context.AssetUserDisclosures.Count(s => s.UserId == userid && s.AssetId == assetId) > 0;
        }

        public void ConfirmAssetUserDisclosure(Guid assetId, int userId)
        {
            var context = _factory.Create();
            context.AssetUserDisclosures.Add(new AssetUserDisclosure()
            {
                AssetId = assetId,
                DateConfirmed = DateTime.Now,
                UserId = userId
            });
            context.Save();
        }

        public List<DeferredMaintenanceCost> GetMaintenanceCosts()
        {
            var context = _factory.Create();
            return context.DeferredMaintenanceCosts.ToList();
        }


        public void EnterBindingEscrow(Guid assetId, DateTime projectedClosingDate)
        {
            var context = _factory.Create();
            var asset = context.Assets.Single(s => s.AssetId == assetId && s.IsActive);
            asset.ClosingDate = projectedClosingDate;
            context.Save();
        }

        public void CloseEscrow(Guid assetId, DateTime actualClosingDate, double closingPrice)
        {
            var context = _factory.Create();
            var asset = context.Assets.Single(s => s.AssetId == assetId && s.IsActive);
            asset.ActualClosingDate = actualClosingDate;
            asset.ClosingPrice = closingPrice;
            context.Save();
        }

        public bool IsAssetClosing(Guid assetId)
        {
            var context = _factory.Create();
            var asset = context.Assets.Single(s => s.AssetId == assetId && s.IsActive);
            return asset.ClosingDate.HasValue;
        }

        public void PublishAsset(Guid assetId)
        {
            var context = _factory.Create();
            var asset = context.Assets.SingleOrDefault(s => s.AssetId == assetId);
            if (asset != null)
            {
                asset.CreationDate = DateTime.Now;
                asset.Show = true;
                context.Save();
            }
        }

        public List<AdminAssetQuickListModel> GetManageAssetsQuickList(ManageAssetsModel model)
        {
            var list = new List<AdminAssetQuickListModel>();
            var context = _factory.Create();
            var assets = context.Assets.Where(w => w.IsActive || w.IsActive == false);

            if (model.HasPositionMortgage.HasValue)
            {
                assets = assets.Where(x => x.HasPositionMortgage == model.HasPositionMortgage.Value);
            }

            if (model.IsPaper)
            {
                // Only filtering if this is true. If we filtered when this value is false, we would exclude paper assets by default
                assets = assets.Where(a => a.IsPaper == model.IsPaper);
            }
            var users = context.Users.ToList();
            bool isSpecificType = false;
            if (model.ControllingUserType == UserType.ICAdmin)
            {
                assets = assets.Where(a => a.ListedByUserId == model.UserId.Value && !a.IsSubmitted);
            }
            if (model.AssetType != 0)
            {
                isSpecificType = true;
                assets = assets.Where(w => w.AssetType == model.AssetType);
            }

            if (!string.IsNullOrEmpty(model.AssetNumber))
            {
                int id = 0;
                int.TryParse(model.AssetNumber, out id);
                if (id != 0)
                {
                    assets = assets.Where(a => a.AssetNumber == id);
                }
            }
            if (!string.IsNullOrEmpty(model.City))
            {
                //assets = assets.Where(a => String.Equals(a.City, model.City.ToLower(), StringComparison.CurrentCultureIgnoreCase)).ToList();
                assets = assets.Where(x => !string.IsNullOrEmpty(x.City) && x.City.ToLower().Contains(model.City.ToLower()));
            }
            if (!string.IsNullOrEmpty(model.State))
            {
                assets = assets.Where(a => a.State == model.State);
            }
            if (!string.IsNullOrEmpty(model.ZipCode))
            {
                assets = assets.Where(a => a.Zip == model.ZipCode);
            }
            if (model.StartDate.HasValue)
            {
                assets = assets.Where(a => a.CreationDate >= model.StartDate.Value);
            }
            if (model.EndDate.HasValue)
            {
                assets = assets.Where(a => a.CreationDate <= model.EndDate.Value);
            }

            var assetList = assets.ToList();

            if (!string.IsNullOrEmpty(model.AddressLine1))
            {
                //assets = assets.Where(a => String.Equals(a.PropertyAddress, model.AddressLine1.ToLower(), StringComparison.CurrentCultureIgnoreCase)).ToList();
                assetList = (from a in assetList
                             where !string.IsNullOrEmpty(a.PropertyAddress) &&
                             a.PropertyAddress.ToLower().Contains(model.AddressLine1.ToLower())
                             select a).ToList();
            }

            if (!string.IsNullOrEmpty(model.AssetName))
            {
                var regex = "[^A-Za-z0-9]";
                var assetName = Regex.Replace(model.AssetName, regex, "");
                assetList = assetList.Where(a => a.ProjectName != null && Regex.Replace(a.ProjectName.ToLower(), regex, "").Contains(assetName.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(model.ApnNumber))
            {
                var regex = "[^A-Za-z0-9]";
                List<Asset> apnAssetList = new List<Asset>();
                var apnNumber = Regex.Replace(model.ApnNumber, regex, "");
                var apnList = context.AssetTaxParcelNumbers.ToList();
                var assetIDs = apnList.Where(w => w.TaxParcelNumber != null && Regex.Replace(w.TaxParcelNumber.ToLower(), regex, "").Contains(apnNumber.ToLower())).Select(s => s.AssetId).Distinct().ToList();
                foreach (Guid id in assetIDs)
                {
                    Asset temp = assetList.Where(x => x.AssetId == id).FirstOrDefault();
                    apnAssetList.Add(temp);
                }
                assetList = apnAssetList;
            }

            if (!string.IsNullOrEmpty(model.County))
            {                
                assetList = (from a in assetList
                             where a.County != null && a.County.ToLower().Contains(model.County.ToLower())
                             select a).ToList();
            }
            if (!string.IsNullOrEmpty(model.ListAgentCompanyName))
            {
                var NARMembersList = context.NarMembers.
                                     Where(a => a.CompanyName.ToLower().Contains(model.ListAgentCompanyName.ToLower())).
                                     Select(a=>a.NarMemberId).ToList();

                var AssetNARMembersList = context.AssetNARMembers.Where(a => NARMembersList.Contains(a.NarMemberId)).Select(a => a.AssetId).ToList();
                assetList = assetList.Where(a => AssetNARMembersList.Contains(a.AssetId)).ToList();
            }
            if (!string.IsNullOrEmpty(model.ListAgentName))
            {
                var NARMembersList = context.NarMembers.
                                     Where(a => a.FirstName.ToLower().Contains(model.ListAgentName.ToLower()) 
                                     || a.LastName.ToLower().Contains(model.ListAgentName.ToLower())).
                                     Select(a => a.NarMemberId).ToList();

                var AssetNARMembersList = context.AssetNARMembers.Where(a => NARMembersList.Contains(a.NarMemberId)).Select(a => a.AssetId).ToList();
                assetList = assetList.Where(a => AssetNARMembersList.Contains(a.AssetId)).ToList();
            }

            /* */
            var assetIds = assetList.Select(al => al.AssetId).ToList();
            var portfolioAssets = context.PortfolioAssets.Where(pa => assetIds.Contains(pa.AssetId)).ToList();
            var userIds = assetList.Select(al => al.ListedByUserId).ToList();
            var uTypeList = context.Users.Where(us => userIds.Contains(us.UserId)).ToList();


            assetList.ForEach(a =>
            {
                if (a != null)
                {
                    var user = users.Where(x => x.UserId == a.ListedByUserId).FirstOrDefault();
                    bool add = true;
                    int units = 0;
                    int squareFeet = 0;
                    if (a.AssetType == AssetType.MultiFamily)
                    {
                        var mf = a as MultiFamilyAsset;
                        units = mf.TotalUnits;
                        if (units > model.MaxUnitsSpaces || units < model.MinUnitsSpaces)
                        {
                            add = false;
                        }
                    }
                    else if (a.AssetType == AssetType.MHP)
                    {
                        var mf = a as MultiFamilyAsset;
                        units = mf.TotalUnits;
                        units += a.NumberRentableSpace != null ? (int)a.NumberRentableSpace : 0;
                        units += a.NumberNonRentableSpace != null ? (int)a.NumberNonRentableSpace : 0;
                        if (units > model.MaxUnitsSpaces || units < model.MinUnitsSpaces)
                        {
                            add = false;
                        }
                    }
                    else
                    {
                        var ca = a as CommercialAsset;
                        squareFeet = ca.SquareFeet;
                        if (squareFeet > model.MaxSquareFeet || squareFeet < model.MinSquareFeet)
                        {
                            add = false;
                        }
                    }
                    if (add)
                    {
                        //-------
                        var aoe = a.ProformaAnnualOperExpenses;
                        var pagi = a.ProformaAnnualIncome;
                        var pami = a.ProformaMiscIncome;
                        var totalIncome = pagi + pami;
                        var pvf = (a.ProformaVacancyFac / 100) * totalIncome;
                        var proformaNOI = Math.Round((totalIncome - pvf) - aoe);
                        var pretax = totalIncome - pvf - aoe;
                        //-------

                        var uType = (UserType)0;
                        var uTypeListInner = uTypeList.Where(us => us.UserId == a.ListedByUserId);
                        if (uTypeListInner.Any())
                        {
                            uType = uTypeListInner.FirstOrDefault().UserType;
                        }

                        list.Add(new AdminAssetQuickListModel()
                        {
                            AddressLine1 = a.PropertyAddress,
                            AssetId = a.AssetId,
                            AssetNumber = a.AssetNumber,
                            City = a.City,
                            Show = a.Show ? "Yes" : "No",
                            State = a.State,
                            Zip = a.Zip,
                            Status = EnumHelper.GetEnumDescription(a.ListingStatus),
                            Type = EnumHelper.GetEnumDescription(a.AssetType),
                            ControllingUserType = model.ControllingUserType,
                            IsOnHold = a.HoldForUserId.HasValue,
                            IsSampleAsset = a.IsSampleAsset,
                            CreatedBy = user != null ? user.FullName + "~" + user.Username : "",
                            AssetName = a.ProjectName,
                            SquareFeet = squareFeet,
                            NumberOfUnits = units,
                            isSpecificType = isSpecificType,
                            CurrentVacancyFac = a.CurrentVacancyFac,
                            LastReportedOccupancyDate = a.LastReportedOccupancyDate,
                            OccupancyDate = a.OccupancyDate,
                            ProformaAnnualIncome = a.ProformaAnnualIncome,
                            ProformaNOI = proformaNOI,
                            CashInvestmentApy = a.CashInvestmentApy,
                            
                            capRate = ((pretax / a.CurrentBpo) * 100),

                            AskingPrice = a.AskingPrice,
                            CurrentBpo = a.CurrentBpo,
                            Portfolio = portfolioAssets.Where(x => x.AssetId == a.AssetId).Any() ? true : false,

                            /* Unknown 0,Yes 1,No */
                            AssmFin = a.HasPositionMortgage == PositionMortgageType.Yes ? "Yes" : "No",

                            /*in case of no user is */
                            UserType = uType,
                            ListingStatus = a.ListingStatus,
                            IsActive = a.IsActive,

                            BusDriver = a.Show ? "CA" : "SUS"

                        });
                    }
                }
            });
            return list;

        }

        public List<PortfolioQuickListModel> GetAssetRealetdPortfolioList(string assetId)
        {
            //----Related Portfoliyo
            /* 
             1. pass asset id and get List of PortfolioAssets 
             2. pass portfolio id and get portfolio (for name only)
             3. pass portfolio id in get List of PortfolioAssets
             4. clculate data based on the asset list
             */
            //------

            var context = _factory.Create();

            List<PortfolioQuickListModel> lstpfModel = new List<PortfolioQuickListModel>();
            Guid asId = new Guid(assetId);
            var PortfolioAssets = context.PortfolioAssets.Where(x => x.AssetId == asId).ToList();
            if (PortfolioAssets.Any())
            {
                foreach (var item in PortfolioAssets)
                {
                    PortfolioQuickListModel pfModel = new PortfolioQuickListModel();
                    var portfolio = context.Portfolios.Where(x => x.PortfolioId == item.PortfolioId).FirstOrDefault();

                    pfModel.NumberOfAssets = context.PortfolioAssets.Where(x => x.PortfolioId == portfolio.PortfolioId).ToList().Count();

                    pfModel.PortfolioId = portfolio.PortfolioId;
                    pfModel.PortfolioName = portfolio.PortfolioName;

                    var lstAssetInPortfolio = context.PortfolioAssets.Where(x => x.PortfolioId == item.PortfolioId).Select(x => x.AssetId).ToList();
                    var relatedAsstes = context.Assets.Where(la => lstAssetInPortfolio.Contains(la.AssetId));

                    int units = 0;
                    int squareFeet = 0;
                    foreach (var itemAsset in relatedAsstes)
                    {
                        if (itemAsset.AssetType == AssetType.MultiFamily)
                        {
                            var mf = itemAsset as MultiFamilyAsset;
                            units += mf.TotalUnits;
                        }
                        else if (itemAsset.AssetType == AssetType.MHP)
                        {
                            var mf = itemAsset as MultiFamilyAsset;
                            units += mf.TotalUnits;
                            units += itemAsset.NumberRentableSpace != null ? (int)itemAsset.NumberRentableSpace : 0;
                            units += itemAsset.NumberNonRentableSpace != null ? (int)itemAsset.NumberNonRentableSpace : 0;
                        }
                        else
                        {
                            var ca = itemAsset as CommercialAsset;
                            squareFeet += ca.SquareFeet;
                        }
                    }

                    pfModel.States = relatedAsstes.Select(ra => ra.State).Distinct().ToList();
                    pfModel.AssetType = relatedAsstes.Select(ra => ra.AssetType).Distinct().ToList();

                    //taken first item data
                    pfModel.UnitsSqFt = 0;

                    pfModel.NumberOfUnits = units;
                    pfModel.SquareFeet = squareFeet;

                    var calcLastOccupancyDateLst = relatedAsstes.Where(ra => ra.LastReportedOccupancyDate.HasValue);
                    var calcOccupancyDateLst = relatedAsstes.Where(ra => ra.OccupancyDate.HasValue);

                    if (calcLastOccupancyDateLst.Count() > 0)
                    {
                        pfModel.OccupancyDate = calcLastOccupancyDateLst.Max(ra => ra.LastReportedOccupancyDate).Value.ToString("MM/yyyy");
                    }
                    else if (calcOccupancyDateLst.Count() > 0)
                    {
                        pfModel.OccupancyDate = calcOccupancyDateLst.Max(ra => ra.OccupancyDate).Value.ToString("MM/yyyy");
                    }

                    //pfModel.OccupancyPercentage = Math.Round(relatedAsstes.Where(ra => ra.GetType() == typeof(MultiFamilyAssetViewModel) || 
                    //                                                                   ra.GetType() == typeof(CommercialAssetViewModel))
                    //                                            .Sum(ra => ra.LastReportedOccPercent) /
                    //                                (double)relatedAsstes.Count(), 2);

                    pfModel.OccupancyPercentage = Math.Round(relatedAsstes.Sum(ra => ra.CurrentVacancyFac) / (double)relatedAsstes.Count(), 2);



                    pfModel.CumiProformaSGI = relatedAsstes.Sum(ra => ra.ProformaAnnualIncome).ToString("C0");

                    //----
                    var proformaNOIP = 0.0D;
                    var pretaxP = 0.0D;
                    var CurrentBpoP = 0.0D;

                    foreach (var rAsset in relatedAsstes)
                    {
                        var aoeP = rAsset.ProformaAnnualOperExpenses;
                        var pagiP = rAsset.ProformaAnnualIncome;
                        var pamiP = rAsset.ProformaMiscIncome;
                        var totalIncomeP = pagiP + pamiP;
                        var pvfP = (rAsset.ProformaVacancyFac / 100) * totalIncomeP;
                        proformaNOIP += Math.Round((totalIncomeP - pvfP) - aoeP);

                        pretaxP += totalIncomeP - pvfP - aoeP;
                        CurrentBpoP += rAsset.CurrentBpo;
                    }
                    //----

                    pfModel.CumiProformaNOI = proformaNOIP.ToString("C0");


                    pfModel.AssmFin = relatedAsstes.First().HasPositionMortgage == PositionMortgageType.Yes ? "Yes" : "No";

                    pfModel.Pricing = ((relatedAsstes.Where(ra => ra.AskingPrice > 0).Any() ?
                                       relatedAsstes.Where(ra => ra.AskingPrice > 0).Sum(ra => ra.AskingPrice) : 0)
                                         + (relatedAsstes.Where(ra => ra.AskingPrice == 0).Any() ?
                                         relatedAsstes.Where(ra => ra.AskingPrice == 0).Sum(ra => ra.CurrentBpo) : 0));

                    pfModel.PricingType = relatedAsstes.Where(ra => ra.AskingPrice > 0).Any() ? "LP" : "CMV";

                    pfModel.CumiLPCapRate = ((proformaNOIP / pfModel.Pricing)).ToString("P2");


                    pfModel.UserType = context.Users.Where(us => us.UserId == relatedAsstes.FirstOrDefault().ListedByUserId).FirstOrDefault().UserType;
                    pfModel.ListingStatus = relatedAsstes.First().ListingStatus;

                    pfModel.BusDriver = relatedAsstes.Where(ra => ra.Show).Any() ? "CA" : "SUS";

                    lstpfModel.Add(pfModel);
                }
            }
            return lstpfModel;
            //-------
        }

        public List<AdminAssetQuickListModel> GetAssetByPortfolioId(string PortfolioId)
        {
            var list = new List<AdminAssetQuickListModel>();
            var context = _factory.Create();

            Guid portId = new Guid(PortfolioId);
            var AssetsidsInPortfolio = context.PortfolioAssets.Where(x => x.PortfolioId == portId).Select(a => a.AssetId).ToList();

            var assets = context.Assets.Where(la => AssetsidsInPortfolio.Contains(la.AssetId));

            var users = context.Users.ToList();
            bool isSpecificType = false;

            var assetList = assets.ToList();

            assetList.ForEach(a =>
            {

                int units = 0;
                int squareFeet = 0;
                if (a.AssetType == AssetType.MultiFamily)
                {
                    var mf = a as MultiFamilyAsset;
                    units = mf.TotalUnits;
                }
                else if (a.AssetType == AssetType.MHP)
                {
                    var mf = a as MultiFamilyAsset;
                    units = mf.TotalUnits;
                    units += a.NumberRentableSpace != null ? (int)a.NumberRentableSpace : 0;
                    units += a.NumberNonRentableSpace != null ? (int)a.NumberNonRentableSpace : 0;
                }
                else
                {
                    var ca = a as CommercialAsset;
                    squareFeet = ca.SquareFeet;
                }

                if (a != null)
                {
                    //-------
                    var aoe = a.ProformaAnnualOperExpenses;
                    var pagi = a.ProformaAnnualIncome;
                    var pami = a.ProformaMiscIncome;
                    var totalIncome = pagi + pami;
                    var pvf = (a.ProformaVacancyFac / 100) * totalIncome;
                    var proformaNOI = Math.Round((totalIncome - pvf) - aoe);
                    var pretax = totalIncome - pvf - aoe;
                    //-------

                    list.Add(new AdminAssetQuickListModel()
                    {
                        AddressLine1 = a.PropertyAddress,
                        AssetId = a.AssetId,
                        AssetNumber = a.AssetNumber,
                        City = a.City,
                        Show = a.Show ? "Yes" : "No",
                        State = a.State,
                        Zip = a.Zip,
                        Status = EnumHelper.GetEnumDescription(a.ListingStatus),
                        Type = EnumHelper.GetEnumDescription(a.AssetType),
                        //ControllingUserType = model.ControllingUserType,
                        IsOnHold = a.HoldForUserId.HasValue,
                        IsSampleAsset = a.IsSampleAsset,
                        CreatedBy = context.Users.Where(x => x.UserId == a.ListedByUserId).FirstOrDefault() != null
                                  ? context.Users.Where(x => x.UserId == a.ListedByUserId).FirstOrDefault().FullName + "~" +
                                  context.Users.Where(x => x.UserId == a.ListedByUserId).FirstOrDefault().Username : "",
                        AssetName = a.ProjectName,
                        SquareFeet = squareFeet,
                        NumberOfUnits = units,
                        isSpecificType = isSpecificType,
                        CurrentVacancyFac = a.CurrentVacancyFac,
                        LastReportedOccupancyDate = a.LastReportedOccupancyDate != null ? a.LastReportedOccupancyDate : a.OccupancyDate,
                        ProformaAnnualIncome = a.ProformaAnnualIncome,
                        ProformaNOI = proformaNOI,
                        CashInvestmentApy = a.CashInvestmentApy,
                        capRate = ((pretax / a.CurrentBpo) * 100),
                        AskingPrice = a.AskingPrice,
                        CurrentBpo = a.CurrentBpo,
                        Portfolio = context.PortfolioAssets.Where(x => x.AssetId == a.AssetId).Any() ? true : false,

                        /*Unknown 0,Yes 1,No 2	 */
                        AssmFin = a.HasPositionMortgage == PositionMortgageType.Yes ? "Yes" : "No",
                        UserType = context.Users.Where(us => us.UserId == a.ListedByUserId).FirstOrDefault().UserType,
                        ListingStatus = a.ListingStatus,
                        IsActive = a.IsActive,
                        BusDriver = a.Show ? "CA" : "SUS"

                    });

                }
            });
            return list;

        }

        public void HoldAssetForUser(int userId, int assetNumber)
        {
            var context = _factory.Create();
            var asset = context.Assets.Single(s => s.AssetNumber == assetNumber && s.IsActive);
            asset.HoldForUserId = userId;
            asset.HoldStartDate = DateTime.Now;
            context.Save();
        }

        public List<AssetDeferredItemViewModel> GetDefaultDeferredMaintenanceItems()
        {
            var list = new List<AssetDeferredItemViewModel>();
            var context = _factory.Create();
            var items = context.DeferredMaintenanceCosts.ToList();
            foreach (var item in items)
            {
                list.Add(new AssetDeferredItemViewModel()
                {
                    ItemTitle = EnumHelper.GetEnumDescription(item.MaintenanceDetail),
                    NumberOfUnits = 1,
                    Selected = false,
                    UnitCost = item.Cost,
                    UnitTypeLabel = item.InputType,
                    MaintenanceDetail = item.MaintenanceDetail
                });
            }
            list.Add(new AssetDeferredItemViewModel() { MaintenanceDetail = MaintenanceDetails.FireDamage, ItemTitle = "Fire Damage", UnitTypeLabel = null, UnitCost = 0, Selected = false, NumberOfUnits = 1 });
            list.Add(new AssetDeferredItemViewModel() { MaintenanceDetail = MaintenanceDetails.FloodDamage, ItemTitle = "Flood Damage", UnitTypeLabel = null, UnitCost = 0, Selected = false, NumberOfUnits = 1 });
            list.Add(new AssetDeferredItemViewModel() { MaintenanceDetail = MaintenanceDetails.Other, ItemTitle = "Other", UnitTypeLabel = "Units", UnitCost = 0, Selected = false, NumberOfUnits = 1 });
            return list;
        }


        public void SetSampleAsset(Guid id)
        {
            var context = _factory.Create();
            var currentSample = context.Assets.Where(w => w.IsSampleAsset && w.IsActive);
            foreach (var asset in currentSample)
            {
                asset.IsSampleAsset = false;
            }
            var newSampleAsset = context.Assets.First(s => s.AssetId == id && s.IsActive);
            newSampleAsset.IsSampleAsset = true;
            context.Save();
        }


        public AssetViewModel GetSampleAsset(bool forScrollingImages)
        {
            var context = _factory.Create();
            var asset = context.Assets.FirstOrDefault(s => s.IsSampleAsset && s.IsActive);
            var sampleId = Guid.Empty;
            if (asset != null)
            {
                sampleId = asset.AssetId;
            }
            else
            {
                sampleId = context.Assets.First(w => w.Show && w.IsActive).AssetId;
            }
            return GetAsset(sampleId, forScrollingImages);
        }

        public int SavePaperCommercial(PaperCommercialAssetViewModel model)
        {
            var context = _factory.Create();

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (TransactionScope saveScope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                var user = context.Users.Where(x => x.Username == model.Username).FirstOrDefault();
                var assetSeller = context.AssetSellers.Where(x => x.Email.ToLower() == model.EmailAddress.ToLower()).FirstOrDefault();
                int sellerId = 0;
                if (assetSeller == null)
                {
                    var newSeller = new AssetSeller();
                    newSeller.NameOfPrincipal = model.NameOfPrincipal;
                    newSeller.NameOfCoPrincipal = model.NameOfCoPrincipal;
                    //newSeller.PhoneHome = model.HomePhone;
                    //newSeller.PhoneOther = model.OtherPhone;
                    newSeller.PhoneWork = model.WorkPhone;
                    newSeller.Fax = model.Fax;
                    newSeller.Email = model.EmailAddress;
                    context.AssetSellers.Add(newSeller);
                    sellerId = newSeller.AssetSellerId;
                }
                else
                {
                    sellerId = assetSeller.AssetSellerId;
                }


                AssetType assetType = AssetType.Other; // temp value
                switch (model.TypeOfProperty)
                {
                    case "Commercial Industrial":
                        assetType = AssetType.Industrial;
                        break;
                    case "Commercial Medical":
                        assetType = AssetType.Medical;
                        break;
                    case "Commercial Office":
                        assetType = AssetType.Office;
                        break;
                    case "Commercial Retail":
                        assetType = AssetType.Retail;
                        break;
                    case "Commercial Service Station & Retail":
                        assetType = AssetType.ConvenienceStoreFuel;
                        break;
                    case "Commercial Single Tenant":
                        assetType = AssetType.Other;
                        break;
                    case "Mixed Use":
                        assetType = AssetType.MixedUse;
                        break;
                    case "Mobile Home Parks":
                        assetType = AssetType.MHP;
                        break;
                    case "Multi-Family":
                        assetType = AssetType.MultiFamily;
                        break;
                    case "Improved Acreage":
                        assetType = AssetType.Other;
                        break;
                    case "Unimproved Acreage":
                        assetType = AssetType.Other;
                        break;
                    case "Hotel":
                        assetType = AssetType.Hotel;
                        break;
                    default:
                        break;
                }

                if (assetType == AssetType.MHP || assetType == AssetType.MultiFamily)
                {
                    #region Saving MultiFamily asset
                    var asset = new MultiFamilyAsset();
                    if (user != null)
                    {
                        asset.ListedByUserId = user.UserId;
                    }
                    if (!string.IsNullOrEmpty(model.TaxAssessorNumber)) context.AssetTaxParcelNumbers.Add(new AssetTaxParcelNumber()
                    {
                        AssetId = model.GuidId,
                        TaxParcelNumber = model.TaxAssessorNumber
                    });
                    if (!string.IsNullOrEmpty(model.TaxAssessorNumberOther)) context.AssetTaxParcelNumbers.Add(new AssetTaxParcelNumber()
                    {
                        AssetId = model.GuidId,
                        TaxParcelNumber = model.TaxAssessorNumberOther
                    });
                    var last = context.Assets.OrderByDescending(s => s.AssetNumber).FirstOrDefault();
                    if (last != null)
                    {
                        asset.AssetNumber = last.AssetNumber + 1;
                    }
                    else
                    {
                        asset.AssetNumber = 1000;
                    }
                    asset.AssetSellerId = sellerId;
                    asset.IsActive = true;
                    asset.AssetId = model.GuidId;
                    asset.AssetCategory = AssetCategory.Real;
                    asset.OperatingStatus = model.OperatingStatus;
                    asset.AssetType = assetType;
                    asset.Owner = model.CorporateName;
                    asset.ProjectName = model.ProjectName;
                    asset.ContactPhoneNumber = model.WorkPhone;
                    asset.GradeClassification = model.GradeClassification;
                    var october = new DateTime(DateTime.Now.Year, 10, 1);
                    asset.PropertyTaxYear = DateTime.Now <= october ? DateTime.Now.AddYears(-1).Year : DateTime.Now.Year;
                    asset.BalloonDateForPayoffOfNote = model.BalloonDateForPayoffOfNote;

                    //MF Details
                    asset.OccupancyPercentage = model.OccupancyPercentage;
                    asset.LastReportedDate = DateTime.Now;
                    asset.ElectricMeterMethod = model.ElectricMeterMethod;
                    asset.GasMeterMethod = model.GasMeterMethod;
                    asset.TotalSquareFootage = model.PropertySquareFeet != null ? (int)model.PropertySquareFeet : 0;
                    asset.TotalUnits = model.TotalUnits;
                    asset.ParkOwnedMHUnits = 0;
                    asset.GrossRentableSquareFeet = model.PropertySquareFeet != null ? (int)model.PropertySquareFeet : 0;
                    asset.SquareFeet = model.PropertySquareFeet != null ? (int)model.PropertySquareFeet : 0;
                    //asset.UnitSpecifications = model.UnitSpecifications;
                    asset.MFDetailsString = model.MFDetailsString;
                    asset.ParkingSpaces = model.ParkingSpaces.GetValueOrDefault(0);
                    asset.CoveredParkingSpaces = model.CoveredParkingSpaces.GetValueOrDefault(0);
                    asset.LastReportedOccupancyDate = model.LastReportedDateMF;

                    asset.HasDeferredMaintenance = model.HasDeferredMaintenance;
                    asset.EstDeferredMaintenance = model.EstDeferredMaintenance;
                    //asset.EstDefMaintenanceDetailsString = model.Es;

                    //asset.ListedByRealtor = 
                    //asset.ListingAgentCompany =
                    //asset.ListingAgentName = 
                    //asset.ListingAgentEmail = 
                    //asset.ListingAgentPhoneNumber = 
                    //asset.ListingAgentCorpAddress = 
                    //asset.ListingAgentCommissionAmount = 
                    //asset.NarMemberId  = 
                    //asset.NarMember = 
                    //asset.CommissionShareAgr = 
                    //asset.DateOfCsaConfirm = 
                    asset.PropertyAddress = model.SecuringPropertyAddress;
                    asset.PropertyAddress2 = model.SecuringPropertyAddress2;
                    asset.City = model.SecuringPropertyCity;
                    asset.State = model.SecuringPropertyState;
                    asset.Zip = model.SecuringPropertyZip;
                    asset.County = model.PropertyCounty;
                    asset.LotNumber = model.LotNumber;
                    asset.Subdivision = model.Subdivision;
                    asset.TaxBookMap = model.TaxBookMap;
                    asset.TaxParcelNumber = model.TaxAssessorNumber;
                    asset.PropertyCondition = model.PropertyCondition;
                    asset.OccupancyType = model.OccupancyType;
                    asset.YearBuilt = model.YearBuilt != null ? (int)model.YearBuilt : 0;
                    asset.SquareFeet = model.PropertySquareFeet != null ? (int)model.PropertySquareFeet : 0;
                    asset.LotSize = model.LotSize;
                    asset.ParkingSpaces = model.ParkingSpaces.GetValueOrDefault(0);
                    asset.CoveredParkingSpaces = model.CoveredParkingSpaces.GetValueOrDefault(0);
                    asset.AnnualPropertyTax = model.AnnualPropertyTaxes;
                    //asset.PropertyTaxYear = 
                    //asset.CurrentPropertyTaxes = 
                    //asset.YearsInArrearTaxes = 
                    asset.CurrentBpo = model.CurrentBpo;
                    asset.AskingPrice = model.AskingSalePrice != null ? (double)model.AskingSalePrice : 0;
                    //asset.IsTBDMarket = 
                    asset.HasIncome = model.HasIncome;
                    //asset.MonthlyGrossIncome = 
                    asset.AnnualGrossIncome = model.CurrentAnnualIncome != null ? (double)model.CurrentAnnualIncome : 0;
                    //asset.HasIncomeReason = model.HasIn
                    //asset.CashInvestmentApy = 
                    //asset.ClosingDate = 
                    //asset.ActualClosingDate = 
                    //asset.ProposedBuyer = 
                    //asset.ProposedBuyerAddress = 
                    //asset.ProposedBuyerContact = 
                    asset.Approved = false;
                    asset.IsSubmitted = false;
                    asset.Show = false;
                    asset.CreationDate = DateTime.Now;
                    asset.IsPaper = true;
                    asset.CommissionShareToEPI = 0;
                    //asset.DateCommissionToEpiReceived = 
                    asset.ListingStatus = ListingStatus.Available;
                    //asset.BathCount = 
                    //asset.BedCount = 
                    asset.BuildingsCount = model.BuildingsCount;
                    //asset.ClosingPrice = 

                    asset.ProformaAnnualIncome = model.ProformaAnnualIncome;
                    asset.ProformaMonthlyIncome = model.ProformaMonthlyIncome;
                    asset.ProformaMiscIncome = model.ProformaMiscIncome;
                    asset.ProformaVacancyFac = model.ProformaVacancyFac;
                    asset.CurrentVacancyFac = model.CurrentVacancyFactor != null ? (double)model.CurrentVacancyFactor : 0;
                    //asset.LastReportedOccPercent =
                    asset.ProformaAnnualOperExpenses = model.ProformaAnnualOperExpenses;
                    //asset.ProformaAoeFactorAsPerOfSGI = 

                    asset.ForeclosureLender = model.ForeclosureLender;
                    asset.ForeclosurePosition = model.ForeclosurePosition;
                    asset.ForeclosureRecordNumber = model.ForeclosureRecordNumber;
                    asset.ForeclosureOriginalMortgageAmount = model.ForeclosureOriginalMortgageAmount;
                    asset.ForeclosureOriginalMortageDate = model.ForeclosureOriginalMortageDate;
                    asset.ForeclosureSaleDate = model.ForeclosureSaleDate;
                    asset.ForeclosureRecordDate = model.ForeclosureRecordDate;

                    //asset.PaperType =
                    //asset.PaperPropertyType =
                    //asset.PaperServicingAgent =
                    //asset.PaperAssignor =
                    //asset.PaperPrincipalBalance =
                    //asset.PaperAskingPrice =
                    //asset.PaperApyForAskingPrice =
                    //asset.PaperMonthlyInterestIncome =
                    //asset.PaperEquityMargin =
                    //asset.PaperMonthsInArrears =
                    //asset.PaperMaturityDate =
                    //asset.PaperNextDueDate =
                    //asset.PaperOriginationDate =
                    //asset.PriorityMortgageBalance =
                    //asset.PaperOriginalInstrDocument =
                    //asset.PaperCurrent =
                    //asset.PaperNoteRate =
                    //asset.PaperInvestmentYield =
                    //asset.PaperPriority =
                    //asset.PaperLtv =
                    //asset.PaperCltv =
                    //asset.PaperSuccessor =
                    //asset.PaperSuccessorAddress =
                    //asset.PaperSuccessorRecordedDocNumber =
                    //asset.PaperSuccessorDocDate =
                    //asset.PaperSuccessorType =
                    //asset.PaperTrustor =
                    //asset.PaperTrustee =
                    //asset.HoldForUserId =
                    //asset.HoldStartDate =
                    //asset.AuctionDate =
                    //asset.HoldEndDate =
                    //asset.LastReportedOccupancyDate =
                    //asset.AverageAdjustmentToBaseRentalIncomePerUnitAfterRenovations =
                    asset.IsSampleAsset = false;

                    asset.HasPositionMortgage = model.HasPositionMortgage;
                    asset.MortgageLienType = model.MortgageLienType;
                    asset.MortgageLienAssumable = model.MortgageLienAssumable;
                    asset.FirstMortgageCompany = model.FirstMortgageCompany;
                    asset.MortgageCompanyAddress = model.MortgageCompanyAddress;
                    asset.MortgageCompanyCity = model.MortgageCompanyCity;
                    asset.MortgageCompanyState = model.MortgageCompanyState;
                    asset.MortgageCompanyZip = model.MortgageCompanyZip;
                    asset.LenderPhone = model.LenderPhone;
                    asset.LenderPhoneOther = model.LenderPhoneOther;
                    asset.AccountNumber = model.AccountNumber;
                    asset.CurrentPrincipalBalance = model.CurrentPrincipalBalance;
                    asset.MonthlyPayment = model.MonthlyPayment;
                    asset.PaymentIncludes = model.PaymentIncludes;
                    asset.InterestRate = model.InterestRate;
                    asset.IsMortgageAnARM = model.IsMortgageAnARM;
                    asset.MortgageAdjustIfARM = model.MortgageAdjustIfARM;
                    asset.NumberOfMissedPayments = model.NumberOfMissedPayments;
                    //asset.AmortizationSchedule = model.;    

                    asset.NoteOrigination = model.NoteOrigination;
                    asset.NotePrincipal = model.NotePrincipal;
                    asset.BPOOfProperty = model.BPOOfProperty;
                    asset.CurrentNotePrincipal = model.CurrentNotePrincipal;
                    asset.HasCopyOfAppraisal = model.HasCopyOfAppraisal;
                    asset.MethodOfAppraisal = model.MethodOfAppraisal;
                    asset.TypeOfNote = model.TypeOfNote;
                    asset.NoteInterestRate = model.NoteInterestRate;
                    asset.PaymentAmount = model.PaymentAmount;
                    asset.PaymentFrequency = model.PaymentFrequency;
                    asset.BalloonDateOfNote = model.BalloonDateOfNote;
                    asset.TypeOfMTGInstrument = model.TypeOfMTGInstrument;
                    asset.AmortType = model.AmortType;
                    asset.PaymentsMadeOnNote = model.PaymentsMadeOnNote;
                    asset.PaymentsRemainingOnNote = model.PaymentsRemainingOnNote;
                    asset.IsNoteCurrent = model.IsNoteCurrent;
                    asset.LastPaymentRecievedOnNote = model.LastPaymentRecievedOnNote;
                    asset.NextPaymentOnNote = model.NextPaymentOnNote;
                    asset.SecuringPropertyAppraisal = model.SecuringPropertyAppraisal;
                    asset.WasPropertyDistressed = model.WasPropertyDistressed;
                    asset.PaymentHistory = model.PaymentHistory;
                    asset.SellerCarryNoteSalesDate = model.SellerCarryNoteSalesDate;
                    asset.SellerCarryNotePrice = model.SellerCarryNotePrice;
                    asset.SellerCarryNoteCashDown = model.SellerCarryNoteCashDown;

                    asset.IsNoteWRAP = model.IsNoteWRAP;
                    asset.OriginalPrincipalBalanceWRAP = model.OriginalPrincipalBalanceWRAP;
                    asset.TotalMonthlyPaymentWRAP = model.TotalMonthlyPaymentWRAP;
                    asset.FirstmortgageBalanceWRAP = model.FirstmortgageBalanceWRAP;
                    asset.FirstInterestRateWRAP = model.FirstInterestRateWRAP;
                    asset.FirstMortgagePaymentWRAP = model.FirstMortgagePaymentWRAP;
                    asset.SecondMortgageBalanceWRAP = model.SecondMortgageBalanceWRAP;
                    asset.SecondInterestRateWRAP = model.SecondInterestRateWRAP;
                    asset.SecondMortgagePaymentWRAP = model.SecondMortgagePaymentWRAP;
                    asset.NoteServicedByAgent = model.NoteServicedByAgent;
                    asset.AgentName = model.AgentName;
                    asset.AgentPhone = model.AgentPhone;
                    asset.AgentEmail = model.AgentEmail;
                    asset.AgentAccountNumber = model.AgentAccountNumber;
                    asset.AgentContactPerson = model.AgentContactPerson;
                    asset.AuthorizeForwardPaymentHistory = model.AuthorizeForwardPaymentHistory;
                    asset.AgentAddress = model.AgentAddress;
                    asset.AgentCity = model.AgentCity;
                    asset.AgentState = model.AgentState;
                    asset.AgentZip = model.AgentZip;

                    asset.HasEnvironmentalIssues = model.HasEnvironmentalIssues;
                    asset.EnvironmentalIssues = model.EnvironmentalIssues;

                    asset.CorporateOwnershipAddress = model.CorporateOwnershipAddress;
                    asset.CorporateOwnershipAddress2 = model.CorporateOwnershipAddress2;
                    asset.CorporateOwnershipCity = model.CorporateOwnershipCity;
                    asset.CorporateOwnershipState = model.CorporateOwnershipState;
                    asset.CorporateOwnershipZip = model.CorporateOwnershipZip;

                    asset.NumberRentableSpace = model.NumberRentableSpace;
                    asset.NumberNonRentableSpace = model.NumberNonRentableSpace;
                    asset.NumberParkOwnedMH = model.NumberParkOwnedMH;
                    asset.AccessRoadTypeId = (AccessRoadType)model.AccessRoadTypeId;
                    asset.InteriorRoadTypeId = (InteriorRoadType)model.InteriorRoadTypeId;
                    asset.MHPadTypeId = (MHPadType)model.MHPadTypeId;
                    asset.WasteWaterTypeId = (WasteWaterType)model.WasteWaterTypeId;
                    asset.WaterServTypeId = (WaterServType)model.WaterServTypeId;
                    asset.TermsOther = model.TermsOther;

                    if (model.OfferingPriceDeterminedByMarketBidding.HasValue && model.OfferingPriceDeterminedByMarketBidding == true)
                        asset.IndicativeBidsDueDate = model.IndicativeBidsDueDate;
                    if (model.RenovatedByOwner.HasValue && model.RenovatedByOwner == true)
                    {
                        asset.RenovatedByOwner = true;
                        if (model.RenovationYear.HasValue)
                            asset.RenovationYear = model.RenovationYear.Value;
                        asset.RecentUpgradesRenovations = model.RecentUpgradesRenovations;
                        asset.RenovationBudget = model.RenovationBudget;
                    }
                    else
                    {
                        asset.RenovatedByOwner = false;
                    }


                    context.Assets.Add(asset);
                    context.Save();

                    foreach (var image in model.Images)
                    {
                        context.AssetImages.Add(new AssetImage()
                        {
                            AssetId = model.GuidId,
                            ContentType = image.ContentType,
                            FileName = image.FileName,
                            IsFlyerImage = false,
                            IsMainImage = false,
                            Order = image.Order,
                            AssetImageId = Guid.NewGuid()
                        });
                        context.Save();
                    }
                    foreach (var doc in model.Documents)
                    {
                        context.AssetDocuments.Add(new AssetDocument()
                        {
                            AssetId = model.GuidId,
                            ContentType = doc.ContentType,
                            Description = doc.Description,
                            FileName = doc.FileName,
                            Order = doc.Order,
                            Size = doc.Size,
                            Title = doc.Title,
                            Type = doc.Type,
                            AssetDocumentId = Guid.NewGuid()
                        });
                        context.Save();
                    }
                    foreach (var vid in model.Videos)
                    {
                        context.AssetVideos.Add(new AssetVideo()
                        {
                            Asset = asset,
                            AssetId = model.GuidId,
                            Description = vid.Description,
                            FilePath = vid.FilePath
                        });
                        context.Save();
                    }
                    foreach (var spec in model.UnitSpecifications)
                    {
                        context.AssetUnitSpecifications.Add(new AssetUnitSpecification()
                        {
                            AssetId = model.GuidId,
                            AssetUnitSpecificationId = Guid.NewGuid(),
                            BedCount = spec.BedCount,
                            BathCount = spec.BathCount,
                            UnitBaseRent = spec.UnitBaseRent,
                            UnitSquareFeet = spec.UnitSquareFeet,
                            CountOfUnits = spec.CountOfUnits
                        });
                        context.Save();
                    }
                    if (model.MHPUnitSpecifications != null)
                    {
                        foreach (var spec in model.MHPUnitSpecifications)
                        {
                            context.AssetMHPSpecifications.Add(new AssetMHPSpecification()
                            {
                                AssetId = model.GuidId,
                                AssetMHPSpecificationId = Guid.NewGuid(),
                                CountOfUnits = spec.CountOfUnits,
                                CurrentDoubleBaseRent = spec.CurrentDoubleBaseRent,
                                CurrentSingleBaseRent = spec.CurrentSingleBaseRent,
                                CurrentTripleBaseRent = spec.CurrentTripleBaseRent,
                                NumberDoubleWide = spec.NumberDoubleWide,
                                NumberSingleWide = spec.NumberSingleWide,
                                CurrentDoubleOwnedBaseRent = spec.CurrentDoubleOwnedBaseRent,
                                CurrentSingleOwnedBaseRent = spec.CurrentSingleOwnedBaseRent,
                                CurrentTripleOwnedBaseRent = spec.CurrentTripleOwnedBaseRent,
                                NumberDoubleWideOwned = spec.NumberDoubleWideOwned,
                                NumberSingleWideOwned = spec.NumberSingleWideOwned,
                                NumberTripleWide = spec.NumberTripleWide,
                                NumberTripleWideOwned = spec.NumberTripleWideOwned,
                            });
                            context.Save();
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Saving Commercial asset
                    var asset = new CommercialAsset();
                    if (user != null)
                    {
                        asset.ListedByUserId = user.UserId;
                    }
                    if (!string.IsNullOrEmpty(model.TaxAssessorNumber)) context.AssetTaxParcelNumbers.Add(new AssetTaxParcelNumber()
                    {
                        AssetId = model.GuidId,
                        TaxParcelNumber = model.TaxAssessorNumber
                    });
                    if (!string.IsNullOrEmpty(model.TaxAssessorNumberOther)) context.AssetTaxParcelNumbers.Add(new AssetTaxParcelNumber()
                    {
                        AssetId = model.GuidId,
                        TaxParcelNumber = model.TaxAssessorNumberOther
                    });
                    var last = context.Assets.OrderByDescending(s => s.AssetNumber).FirstOrDefault();
                    if (last != null)
                    {
                        asset.AssetNumber = last.AssetNumber + 1;
                    }
                    else
                    {
                        asset.AssetNumber = 1000;
                    }
                    asset.AssetSellerId = sellerId;
                    asset.IsActive = true;
                    asset.AssetId = model.GuidId;
                    asset.AssetCategory = AssetCategory.Real;
                    asset.OperatingStatus = model.OperatingStatus;
                    asset.AssetType = assetType;
                    asset.Owner = model.CorporateName;
                    asset.ProjectName = model.ProjectName;
                    asset.ContactPhoneNumber = model.WorkPhone;
                    asset.GradeClassification = model.GradeClassification;
                    var october = new DateTime(DateTime.Now.Year, 10, 1);
                    asset.PropertyTaxYear = DateTime.Now <= october ? DateTime.Now.AddYears(-1).Year : DateTime.Now.Year;
                    asset.BalloonDateForPayoffOfNote = model.BalloonDateForPayoffOfNote;
                    asset.NumberOfRentableSuites = asset.NumberOfRentableSuites;
                    asset.HasDeferredMaintenance = model.HasDeferredMaintenance;
                    asset.EstDeferredMaintenance = model.EstDeferredMaintenance;

                    asset.Type = model.Type;
                    asset.RentableSquareFeet = model.RentableSquareFeet != null ? (int)model.RentableSquareFeet : 0;
                    asset.ProformaSgi = model.ProformaSgi;
                    asset.ProformaAnnualNoi = model.ProformaAnnualNoi;
                    asset.NumberOfTenants = model.NumberOfTenants;
                    asset.HasAAARatedMajorTenant = model.HasAAARatedMajorTenant;
                    asset.NameOfAAARatedMajorTenant = model.NameOfAAARatedMajorTenant;
                    asset.NumberofSuites = model.NumberofSuites;
                    asset.OccupancyPercentage = model.OccupancyPercentage;
                    asset.OccupancyDate = model.OccupancyDate;
                    asset.PropertyDetailsString = model.PropertyDetailsString;
                    asset.LeasedSquareFootageByMajorTenant = model.LeasedSquareFootageByMajorTenant;
                    asset.BaseRentPerSqFtMajorTenant = model.BaseRentPerSqFtMajorTenant;
                    asset.CurrentMarkerRentPerSqFt = model.CurrentMarkerRentPerSqFt;
                    asset.VacantSuites = model.VacantSuites;
                    asset.IsMajorTenantAAARated = model.IsMajorTenantAAARated;
                    asset.ParkingSpaces = model.ParkingSpaces.GetValueOrDefault(0);
                    asset.CoveredParkingSpaces = model.CoveredParkingSpaces.GetValueOrDefault(0);
                    asset.LastReportedOccupancyDate = model.LastReportedDateCommercial;
                    //asset.ListedByRealtor = 
                    //asset.ListingAgentCompany =
                    //asset.ListingAgentName = 
                    //asset.ListingAgentEmail = 
                    //asset.ListingAgentPhoneNumber = 
                    //asset.ListingAgentCorpAddress = 
                    //asset.ListingAgentCommissionAmount = 
                    //asset.NarMemberId  = 
                    //asset.NarMember = 
                    //asset.CommissionShareAgr = 
                    //asset.DateOfCsaConfirm = 
                    asset.PropertyAddress = model.SecuringPropertyAddress;
                    asset.PropertyAddress2 = model.SecuringPropertyAddress2;
                    asset.City = model.SecuringPropertyCity;
                    asset.State = model.SecuringPropertyState;
                    asset.Zip = model.SecuringPropertyZip;
                    asset.County = model.PropertyCounty;
                    asset.LotNumber = model.LotNumber;
                    asset.Subdivision = model.Subdivision;
                    asset.TaxBookMap = model.TaxBookMap;
                    asset.TaxParcelNumber = model.TaxAssessorNumber;
                    asset.PropertyCondition = model.PropertyCondition;
                    asset.OccupancyType = model.OccupancyType;
                    asset.YearBuilt = model.YearBuilt != null ? (int)model.YearBuilt : 0;
                    asset.SquareFeet = model.PropertySquareFeet != null ? (int)model.PropertySquareFeet : 0;
                    asset.LotSize = model.LotSize;
                    asset.ParkingSpaces = model.ParkingSpaces.GetValueOrDefault(0);
                    asset.CoveredParkingSpaces = model.CoveredParkingSpaces.GetValueOrDefault(0);
                    asset.AnnualPropertyTax = model.AnnualPropertyTaxes;
                    //asset.PropertyTaxYear = 
                    //asset.CurrentPropertyTaxes = 
                    //asset.YearsInArrearTaxes = 
                    asset.CurrentBpo = model.CurrentBpo;
                    asset.AskingPrice = model.AskingSalePrice != null ? (double)model.AskingSalePrice : 0;
                    //asset.IsTBDMarket = 
                    asset.HasIncome = model.HasIncome;
                    //asset.MonthlyGrossIncome = 
                    asset.AnnualGrossIncome = model.CurrentAnnualIncome != null ? (double)model.CurrentAnnualIncome : 0;
                    //asset.HasIncomeReason = model.HasIn
                    //asset.CashInvestmentApy = 
                    //asset.ClosingDate = 
                    //asset.ActualClosingDate = 
                    //asset.ProposedBuyer = 
                    //asset.ProposedBuyerAddress = 
                    //asset.ProposedBuyerContact = 
                    asset.Approved = false;
                    asset.IsSubmitted = false;
                    asset.Show = false;
                    asset.CreationDate = DateTime.Now;
                    asset.IsPaper = true;
                    asset.CommissionShareToEPI = 0;
                    //asset.DateCommissionToEpiReceived = 
                    asset.ListingStatus = ListingStatus.Available;
                    //asset.BathCount = 
                    //asset.BedCount = 
                    asset.BuildingsCount = model.BuildingsCount;
                    //asset.ClosingPrice = 

                    asset.ProformaAnnualIncome = model.ProformaAnnualIncome;
                    asset.ProformaMonthlyIncome = model.ProformaMonthlyIncome;
                    //asset.ProformaMiscIncome =
                    asset.ProformaVacancyFac = model.ProformaVacancyFac;
                    asset.CurrentVacancyFac = model.CurrentVacancyFactor != null ? (double)model.CurrentVacancyFactor : 0;
                    //asset.LastReportedOccPercent =
                    asset.ProformaAnnualOperExpenses = model.ProformaAnnualOperExpenses;
                    //asset.ProformaAoeFactorAsPerOfSGI = 

                    asset.ForeclosureLender = model.ForeclosureLender;
                    asset.ForeclosurePosition = model.ForeclosurePosition;
                    asset.ForeclosureRecordNumber = model.ForeclosureRecordNumber;
                    asset.ForeclosureOriginalMortgageAmount = model.ForeclosureOriginalMortgageAmount;
                    asset.ForeclosureOriginalMortageDate = model.ForeclosureOriginalMortageDate;
                    asset.ForeclosureSaleDate = model.ForeclosureSaleDate;
                    asset.ForeclosureRecordDate = model.ForeclosureRecordDate;

                    //asset.HoldForUserId =
                    //asset.HoldStartDate =
                    //asset.AuctionDate =
                    //asset.HoldEndDate =
                    //asset.AverageAdjustmentToBaseRentalIncomePerUnitAfterRenovations =
                    asset.IsSampleAsset = false;

                    asset.HasPositionMortgage = model.HasPositionMortgage;
                    asset.MortgageLienType = model.MortgageLienType;
                    asset.MortgageLienAssumable = model.MortgageLienAssumable;
                    asset.FirstMortgageCompany = model.FirstMortgageCompany;
                    asset.MortgageCompanyAddress = model.MortgageCompanyAddress;
                    asset.MortgageCompanyCity = model.MortgageCompanyCity;
                    asset.MortgageCompanyState = model.MortgageCompanyState;
                    asset.MortgageCompanyZip = model.MortgageCompanyZip;
                    asset.LenderPhone = model.LenderPhone;
                    asset.LenderPhoneOther = model.LenderPhoneOther;
                    asset.AccountNumber = model.AccountNumber;
                    asset.CurrentPrincipalBalance = model.CurrentPrincipalBalance;
                    asset.MonthlyPayment = model.MonthlyPayment;
                    asset.PaymentIncludes = model.PaymentIncludes;
                    asset.InterestRate = model.InterestRate;
                    asset.IsMortgageAnARM = model.IsMortgageAnARM;
                    asset.MortgageAdjustIfARM = model.MortgageAdjustIfARM;
                    asset.NumberOfMissedPayments = model.NumberOfMissedPayments;
                    //asset.AmortizationSchedule = model.;    

                    asset.NoteOrigination = model.NoteOrigination;
                    asset.NotePrincipal = model.NotePrincipal;
                    asset.BPOOfProperty = model.BPOOfProperty;
                    asset.CurrentNotePrincipal = model.CurrentNotePrincipal;
                    asset.HasCopyOfAppraisal = model.HasCopyOfAppraisal;
                    asset.MethodOfAppraisal = model.MethodOfAppraisal;
                    asset.TypeOfNote = model.TypeOfNote;
                    asset.NoteInterestRate = model.NoteInterestRate;
                    asset.PaymentAmount = model.PaymentAmount;
                    asset.PaymentFrequency = model.PaymentFrequency;
                    asset.BalloonDateOfNote = model.BalloonDateOfNote;
                    asset.TypeOfMTGInstrument = model.TypeOfMTGInstrument;
                    asset.AmortType = model.AmortType;
                    asset.PaymentsMadeOnNote = model.PaymentsMadeOnNote;
                    asset.PaymentsRemainingOnNote = model.PaymentsRemainingOnNote;
                    asset.IsNoteCurrent = model.IsNoteCurrent;
                    asset.LastPaymentRecievedOnNote = model.LastPaymentRecievedOnNote;
                    asset.NextPaymentOnNote = model.NextPaymentOnNote;
                    asset.SecuringPropertyAppraisal = model.SecuringPropertyAppraisal;
                    asset.WasPropertyDistressed = model.WasPropertyDistressed;
                    asset.PaymentHistory = model.PaymentHistory;
                    asset.SellerCarryNoteSalesDate = model.SellerCarryNoteSalesDate;
                    asset.SellerCarryNotePrice = model.SellerCarryNotePrice;
                    asset.SellerCarryNoteCashDown = model.SellerCarryNoteCashDown;

                    asset.IsNoteWRAP = model.IsNoteWRAP;
                    asset.OriginalPrincipalBalanceWRAP = model.OriginalPrincipalBalanceWRAP;
                    asset.TotalMonthlyPaymentWRAP = model.TotalMonthlyPaymentWRAP;
                    asset.FirstmortgageBalanceWRAP = model.FirstmortgageBalanceWRAP;
                    asset.FirstInterestRateWRAP = model.FirstInterestRateWRAP;
                    asset.FirstMortgagePaymentWRAP = model.FirstMortgagePaymentWRAP;
                    asset.SecondMortgageBalanceWRAP = model.SecondMortgageBalanceWRAP;
                    asset.SecondInterestRateWRAP = model.SecondInterestRateWRAP;
                    asset.SecondMortgagePaymentWRAP = model.SecondMortgagePaymentWRAP;
                    asset.NoteServicedByAgent = model.NoteServicedByAgent;
                    asset.AgentName = model.AgentName;
                    asset.AgentPhone = model.AgentPhone;
                    asset.AgentEmail = model.AgentEmail;
                    asset.AgentAccountNumber = model.AgentAccountNumber;
                    asset.AgentContactPerson = model.AgentContactPerson;
                    asset.AuthorizeForwardPaymentHistory = model.AuthorizeForwardPaymentHistory;
                    asset.AgentAddress = model.AgentAddress;
                    asset.AgentCity = model.AgentCity;
                    asset.AgentState = model.AgentState;
                    asset.AgentZip = model.AgentZip;

                    asset.HasEnvironmentalIssues = model.HasEnvironmentalIssues;
                    asset.EnvironmentalIssues = model.EnvironmentalIssues;

                    asset.CorporateOwnershipAddress = model.CorporateOwnershipAddress;
                    asset.CorporateOwnershipAddress2 = model.CorporateOwnershipAddress2;
                    asset.CorporateOwnershipCity = model.CorporateOwnershipCity;
                    asset.CorporateOwnershipState = model.CorporateOwnershipState;
                    asset.CorporateOwnershipZip = model.CorporateOwnershipZip;
                    asset.TermsOther = model.TermsOther;
                    if (model.OfferingPriceDeterminedByMarketBidding.HasValue && model.OfferingPriceDeterminedByMarketBidding == true)
                        asset.IndicativeBidsDueDate = model.IndicativeBidsDueDate;
                    if (model.RenovatedByOwner.HasValue && model.RenovatedByOwner == true)
                    {
                        asset.RenovatedByOwner = true;
                        if (model.RenovationYear.HasValue)
                            asset.RenovationYear = model.RenovationYear.Value;
                        asset.RecentUpgradesRenovations = model.RecentUpgradesRenovations;
                        asset.RenovationBudget = model.RenovationBudget;
                    }
                    else
                    {
                        asset.RenovatedByOwner = false;
                    }

                    context.Assets.Add(asset);
                    context.Save();

                    foreach (var image in model.Images)
                    {
                        context.AssetImages.Add(new AssetImage()
                        {
                            AssetId = model.GuidId,
                            ContentType = image.ContentType,
                            FileName = image.FileName,
                            IsFlyerImage = false,
                            IsMainImage = false,
                            Order = image.Order,
                            AssetImageId = Guid.NewGuid()
                        });
                        context.Save();
                    }
                    foreach (var doc in model.Documents)
                    {
                        context.AssetDocuments.Add(new AssetDocument()
                        {
                            AssetId = model.GuidId,
                            ContentType = doc.ContentType,
                            Description = doc.Description,
                            FileName = doc.FileName,
                            Order = doc.Order,
                            Size = doc.Size,
                            Title = doc.Title,
                            Type = doc.Type,
                            AssetDocumentId = Guid.NewGuid()
                        });
                        context.Save();
                    }
                    foreach (var vid in model.Videos)
                    {
                        context.AssetVideos.Add(new AssetVideo()
                        {
                            Asset = asset,
                            AssetId = model.GuidId,
                            Description = vid.Description,
                            FilePath = vid.FilePath
                        });
                        context.Save();
                    }
                    foreach (var spec in model.UnitSpecifications)
                    {
                        context.AssetUnitSpecifications.Add(new AssetUnitSpecification()
                        {
                            AssetId = model.GuidId,
                            AssetUnitSpecificationId = Guid.NewGuid(),
                            BedCount = spec.BedCount,
                            BathCount = spec.BathCount,
                            UnitBaseRent = spec.UnitBaseRent,
                            UnitSquareFeet = spec.UnitSquareFeet,
                            CountOfUnits = spec.CountOfUnits
                        });
                        context.Save();
                    }
                    #endregion
                }



                foreach (var item in model.DeferredMaintenanceItems)
                {
                    if (item.Selected)
                    {
                        var record = context.AssetDeferredMaintenanceItems.FirstOrDefault(w => w.AssetId == model.GuidId && w.MaintenanceDetail == item.MaintenanceDetail);
                        if (record != null)
                        {
                            record.UnitCost = item.UnitCost;
                            record.Units = Convert.ToInt32(item.NumberOfUnits);
                        }
                        else
                        {
                            context.AssetDeferredMaintenanceItems.Add(new AssetDeferredMaintenanceItem()
                            {
                                AssetId = model.GuidId,
                                MaintenanceDetail = item.MaintenanceDetail,
                                UnitCost = item.UnitCost,
                                Units = Convert.ToInt32(item.NumberOfUnits)
                            });
                        }
                        context.Save();
                    }
                }
                saveScope.Complete();
                //return entity.PaperCommercialAssetId;
                return 0;

            }

        }

        public int SavePaperResidential(PaperResidentialAssetViewModel model)
        {
            var context = _factory.Create();

            var assetSeller = context.AssetSellers.Where(x => x.Email.ToLower() == model.BeneficiaryEmail.ToLower()).FirstOrDefault();
            int sellerId = 0;
            if (assetSeller == null)
            {
                var newSeller = new AssetSeller();
                newSeller.BeneficiaryName = model.BeneficiaryName;
                newSeller.BeneficiaryFullName = model.BeneficiaryFullName;
                newSeller.BeneficiaryContactAddress = model.BeneficiaryContactAddress;
                newSeller.BeneficiaryCity = model.BeneficiaryCity;
                newSeller.BeneficiaryState = model.BeneficiaryState;
                newSeller.BeneficiaryZip = model.BeneficiaryZip;
                newSeller.BeneficiaryPhoneHome = model.BeneficiaryPhoneHome;
                newSeller.BeneficiaryPhoneCell = model.BeneficiaryPhoneCell;
                newSeller.BeneficiaryPhoneWork = model.BeneficiaryPhoneWork;
                newSeller.BeneficiaryFax = model.BeneficiaryFax;
                newSeller.BeneficiaryEmail = model.BeneficiaryEmail;
                newSeller.BeneficiaryAccountNumber = model.BeneficiaryAccountNumber;
                context.AssetSellers.Add(newSeller);
                sellerId = newSeller.AssetSellerId;
            }
            else
            {
                sellerId = assetSeller.AssetSellerId;
            }

            foreach (var image in model.Images)
            {
                var newImage = new TempAssetImage();
                newImage.GuidId = model.GuidId;
                newImage.ContentType = image.ContentType;
                newImage.FileName = image.FileName;
                newImage.Order = image.Order;
                context.TempAssetImages.Add(newImage);
            }
            foreach (var doc in model.Documents)
            {
                var newDoc = new TempAssetDocument();
                newDoc.GuidId = model.GuidId;
                newDoc.ContentType = doc.ContentType;
                newDoc.Description = doc.Description;
                newDoc.FileName = doc.FileName;
                newDoc.Order = doc.Order;
                newDoc.Size = doc.Size;
                newDoc.Title = doc.Title;
                newDoc.Type = doc.Type;
                context.TempAssetDocuments.Add(newDoc);
            }
            foreach (var vid in model.Videos)
            {
                var newVid = new TempAssetVideo();
                newVid.GuidId = model.GuidId;
                newVid.Description = vid.Description;
                newVid.FilePath = vid.FilePath;
                context.TempAssetVideos.Add(newVid);
            }

            var entity = new PaperResidentialAsset();

            entity.GuidId = model.GuidId;
            entity.AssetSellerId = sellerId;
            entity.BeneficiaryName = model.BeneficiaryName;
            entity.BeneficiaryFullName = model.BeneficiaryFullName;
            entity.BeneficiaryContactAddress = model.BeneficiaryContactAddress;
            entity.BeneficiaryCity = model.BeneficiaryCity;
            entity.BeneficiaryState = model.BeneficiaryState;
            entity.BeneficiaryZip = model.BeneficiaryZip;
            entity.BeneficiaryPhoneHome = model.BeneficiaryPhoneHome;
            entity.BeneficiaryPhoneCell = model.BeneficiaryPhoneCell;
            entity.BeneficiaryPhoneWork = model.BeneficiaryPhoneWork;
            entity.BeneficiaryFax = model.BeneficiaryFax;
            entity.BeneficiaryEmail = model.BeneficiaryEmail;
            entity.BeneficiaryAccountNumber = model.BeneficiaryAccountNumber;

            entity.NotePayerName = model.NotePayerName;
            entity.NotePayerFullName = model.NotePayerFullName;
            entity.NotePayerContactAddress = model.NotePayerContactAddress;
            entity.NotePayerCity = model.NotePayerCity;
            entity.NotePayerState = model.NotePayerState;
            entity.NotePayerZip = model.NotePayerZip;
            entity.NotePayerPhoneHome = model.NotePayerPhoneHome;
            entity.NotePayerPhoneCell = model.NotePayerPhoneCell;
            entity.NotePayerPhoneWork = model.NotePayerPhoneWork;
            entity.NotePayerFax = model.NotePayerFax;
            entity.NotePayerEmail = model.NotePayerEmail;
            entity.NotePayerSSNOrTIN = model.NotePayerSSNOrTIN;
            entity.NotePayerFICO = model.NotePayerFICO;

            entity.SecuringPropertyAddress = model.SecuringPropertyAddress;
            entity.SecuringPropertyCity = model.SecuringPropertyCity;
            entity.SecuringPropertyState = model.SecuringPropertyState;
            entity.SecuringPropertyZip = model.SecuringPropertyZip;
            entity.TypeOfProperty = model.TypeOfProperty;
            entity.Bedrooms = model.Bedrooms;
            entity.Bathrooms = model.Bathrooms;
            entity.PropertySquareFeet = model.PropertySquareFeet;
            entity.Year = model.Year;
            entity.TypeOfContruction = model.TypeOfContruction;
            entity.LotSize = model.LotSize;
            entity.Parking = model.Parking;
            entity.WaterService = model.WaterService;
            entity.SewerService = model.SewerService;
            entity.PowerService = model.PowerService;
            entity.PropertyAccess = model.PropertyAccess;
            entity.NoteOriginationDate = model.NoteOriginationDate;
            entity.OriginalNotePrincipal = model.OriginalNotePrincipal;
            entity.BPOOfProperty = model.BPOOfProperty;
            entity.CurrentNotePrincipal = model.CurrentNotePrincipal;
            entity.HasCopyOfAppraisal = model.HasCopyOfAppraisal;
            entity.MethodOfAppraisal = model.MethodOfAppraisal;
            entity.TypeOfNote = model.TypeOfNote;
            entity.NoteInterestRate = model.NoteInterestRate;
            entity.PaymentAmount = model.PaymentAmount;
            entity.PaymentFrequency = model.PaymentFrequency;
            entity.BalloonDateOfNote = model.BalloonDateOfNote;
            entity.BalloonDueAmount = model.BalloonDueAmount;
            entity.TypeOfMTGInstrument = model.TypeOfMTGInstrument;
            entity.AmortType = model.AmortType;
            entity.PaymentsMadeOnNote = model.PaymentsMadeOnNote;
            entity.PaymentsRemainingOnNote = model.PaymentsRemainingOnNote;
            entity.IsNoteCurrent = model.IsNoteCurrent;
            entity.LastPaymentRecievedOnNote = model.LastPaymentRecievedOnNote;
            entity.NextPaymentOnNote = model.NextPaymentOnNote;
            entity.SecuringPropertyAppraisal = model.SecuringPropertyAppraisal;
            entity.PaymentHistory = model.PaymentHistory;
            entity.SellerCarryNoteSalesDate = model.SellerCarryNoteSalesDate;
            entity.SellerCarryNotePrice = model.SellerCarryNotePrice;
            entity.SellerCarryNoteCashDown = model.SellerCarryNoteCashDown;
            entity.OriginalPrincipalBalanceWRAP = model.OriginalPrincipalBalanceWRAP;
            entity.TotalMonthlyPaymentWRAP = model.TotalMonthlyPaymentWRAP;
            entity.FirstmortgageBalanceWRAP = model.FirstmortgageBalanceWRAP;
            entity.FirstInterestRateWRAP = model.FirstInterestRateWRAP;
            entity.FirstMortgagePaymentWRAP = model.FirstMortgagePaymentWRAP;
            entity.SecondMortgageBalanceWRAP = model.SecondMortgageBalanceWRAP;
            entity.SecondInterestRateWRAP = model.SecondInterestRateWRAP;
            entity.SecondMortgagePaymentWRAP = model.SecondMortgagePaymentWRAP;
            entity.NoteServicedByAgent = model.NoteServicedByAgent;
            entity.AgentName = model.AgentName;
            entity.AgentPhone = model.AgentPhone;
            entity.AgentEmail = model.AgentEmail;
            entity.AgentAccountNumber = model.AgentAccountNumber;
            entity.AgentContactPerson = model.AgentContactPerson;
            entity.AuthorizeForwardPaymentHistory = model.AuthorizeForwardPaymentHistory;
            entity.AgentAddress = model.AgentAddress;
            entity.AgentCity = model.AgentCity;
            entity.AgentState = model.AgentState;
            entity.AgentZip = model.AgentZip;

            entity.HasPicturesOfProperty = model.HasPicturesOfProperty;
            entity.GeneralComments = model.GeneralComments;
            entity.SellingReason = model.SellingReason;
            entity.HasTitleInsurance = model.HasTitleInsurance;
            entity.HomeownerInsurance = model.HomeownerInsurance;
            entity.OwnerOccupied = model.OwnerOccupied;
            entity.IsPropertyRental = model.IsPropertyRental;
            entity.MonthlyRentRate = model.MonthlyRentRate;
            entity.HasEnvironmentalIssues = model.HasEnvironmentalIssues;
            entity.EnvironmentalIssues = model.EnvironmentalIssues;
            entity.AskingPrice = model.AskingPrice;

            context.PaperResidentialAssets.Add(entity);
            context.Save();
            return entity.PaperResidentialAssetId;
        }

        public int SaveRealEstateCommercial(RealEstateCommercialAssetViewModel model)
        {

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (TransactionScope saveScope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                var context = _factory.Create();
                var user = context.Users.Where(x => x.Username == model.Username).FirstOrDefault();
                // create the user if the person doesnt exist then the asset
                var assetSeller = context.AssetSellers.Where(x => x.Email.ToLower() == model.EmailAddress.ToLower()).FirstOrDefault();
                int sellerId = 0;
                if (assetSeller == null)
                {
                    var newSeller = new AssetSeller();
                    newSeller.NameOfPrincipal = model.NameOfPrincipal;
                    newSeller.NameOfCoPrincipal = model.NameOfCoPrincipal;
                    //newSeller.PhoneHome = model.HomePhone;
                    //newSeller.PhoneOther = model.OtherPhone;
                    newSeller.PhoneWork = model.WorkPhone;
                    newSeller.Fax = model.Fax;
                    newSeller.Email = model.EmailAddress;
                    context.AssetSellers.Add(newSeller);
                    sellerId = newSeller.AssetSellerId;
                }
                else
                {
                    sellerId = assetSeller.AssetSellerId;
                }

                //foreach (var spec in model.UnitSpecifications)
                //{
                //    context.TempUnitSpecifications.Add(new TempUnitSpecification()
                //    {
                //        GuidId = model.GuidId,
                //        BedCount = spec.BedCount,
                //        BathCount = spec.BathCount,
                //        UnitBaseRent = spec.UnitBaseRent,
                //        UnitSquareFeet = spec.UnitSquareFeet,
                //        CountOfUnits = spec.CountOfUnits
                //    });
                //    context.Save();
                //}
                //foreach (var image in model.Images)
                //{
                //    var newImage = new TempAssetImage();
                //    newImage.GuidId = model.GuidId;
                //    newImage.ContentType = image.ContentType;
                //    newImage.FileName = image.FileName;
                //    newImage.Order = image.Order;
                //    context.TempAssetImages.Add(newImage);
                //}
                //foreach (var doc in model.Documents)
                //{
                //    var newDoc = new TempAssetDocument();
                //    newDoc.GuidId = model.GuidId;
                //    newDoc.ContentType = doc.ContentType;
                //    newDoc.Description = doc.Description;
                //    newDoc.FileName = doc.FileName;
                //    newDoc.Order = doc.Order;
                //    newDoc.Size = doc.Size;
                //    newDoc.Title = doc.Title;
                //    newDoc.Type = doc.Type;
                //    context.TempAssetDocuments.Add(newDoc);
                //}
                //foreach (var vid in model.Videos)
                //{
                //    var newVid = new TempAssetVideo();
                //    newVid.GuidId = model.GuidId;
                //    newVid.Description = vid.Description;
                //    newVid.FilePath = vid.FilePath;
                //    context.TempAssetVideos.Add(newVid);
                //}

                //foreach (var item in model.DeferredMaintenanceItems)
                //{
                //    if (item.Selected)
                //    {
                //        var record = context.TempDeferredMaintenanceItems.FirstOrDefault(w => w.GuidId == model.GuidId && w.MaintenanceDetail == item.MaintenanceDetail);
                //        if (record != null)
                //        {
                //            record.UnitCost = item.UnitCost;
                //            record.Units = Convert.ToInt32(item.NumberOfUnits);
                //        }
                //        else
                //        {
                //            context.TempDeferredMaintenanceItems.Add(new TempDeferredMaintenanceItem()
                //            {
                //                GuidId = model.GuidId,
                //                MaintenanceDetail = item.MaintenanceDetail,
                //                UnitCost = item.UnitCost,
                //                Units = Convert.ToInt32(item.NumberOfUnits)
                //            });
                //        }
                //        context.Save();
                //    }
                //}

                //var entity = new RealEstateCommercialAsset();

                //entity.GuidId = model.GuidId;
                //entity.AssetSellerId = sellerId;
                //entity.NameOfPrincipal = model.NameOfPrincipal;
                //entity.NameOfCoPrincipal = model.NameOfCoPrincipal;
                //entity.CorporateName = model.CorporateName;
                //entity.WorkPhone = model.WorkPhone;
                //entity.CellPhone = model.CellPhone;
                //entity.Fax = model.Fax;
                //entity.EmailAddress = model.EmailAddress;

                //entity.TypeOfProperty = model.TypeOfProperty;
                //entity.PropertyAddress = model.PropertyAddress;
                //entity.PropertyCity = model.PropertyCity;
                //entity.PropertyState = model.PropertyState;
                //entity.PropertyZip = model.PropertyZip;

                //entity.FirstMortgageCompany = model.FirstMortgageCompany;
                //entity.MortgageCompanyAddress = model.MortgageCompanyAddress;
                //entity.MortgageCompanyCity = model.MortgageCompanyCity;
                //entity.MortgageCompanyState = model.MortgageCompanyState;
                //entity.MortgageCompanyZip = model.MortgageCompanyZip;
                //entity.LenderPhone = model.LenderPhone;
                //entity.LenderPhoneOther = model.LenderPhoneOther;
                //entity.AccountNumber = model.AccountNumber;
                ////entity.TypeOfMortgage = model.TypeOfMortgage;
                //entity.CurrentPrincipalBalance = model.CurrentPrincipalBalance;
                //entity.MonthlyPayment = model.MonthlyPayment;
                //entity.PaymentIncludes = model.PaymentIncludes;
                //entity.InterestRate = model.InterestRate;
                //entity.IsMortgageAnARM = model.IsMortgageAnARM;
                //entity.MortgageAdjustIfARM = model.MortgageAdjustIfARM;
                //entity.NumberOfMissedPayments = model.NumberOfMissedPayments;
                //entity.RentableSquareFeet = model.RentableSquareFeet;
                //entity.MajorTenant = model.MajorTenant;
                ////entity.NumberOfApartments = model.NumberOfApartments;
                ////entity.EfficiencyApartment = model.EfficiencyApartment;
                ////entity.EfficiencyApartmentRentRate = model.EfficiencyApartmentRentRate;
                ////entity.OneBedroom = model.OneBedroom;
                ////entity.OneBedroomRentRate = model.OneBedroomRentRate;
                ////entity.TwoBedroom = model.TwoBedroom;
                ////entity.TwoBedroomRentRate = model.TwoBedroomRentRate;
                ////entity.ThreeBedroom = model.ThreeBedroom;
                ////entity.ThreeBedroomRentRate = model.ThreeBedroomRentRate;
                //entity.CurrentVacancyFactor = model.CurrentVacancyFactor;
                //entity.CurrentAnnualIncome = model.CurrentAnnualIncome;
                //entity.CurrentAnnualOperatingExepenses = model.CurrentAnnualOperatingExepenses;
                ////entity.CurrentOperatingVacancyFactor = model.CurrentOperatingVacancyFactor;
                //entity.CurrentCalendarYearToDateCashFlow = model.CurrentCalendarYearToDateCashFlow;
                //entity.RecentUpgradesRenovations = model.RecentUpgradesRenovations;
                //entity.AdditionalInformation = model.AdditionalInformation;
                //entity.NeededMaintenance = model.NeededMaintenance;
                //entity.AskingSalePrice = model.AskingSalePrice;
                //entity.Terms = model.Terms;
                //entity.MotivationToLiquidate = model.MotivationToLiquidate;
                //entity.HasEnvironmentalIssues = model.HasEnvironmentalIssues;
                //entity.EnvironmentalIssues = model.EnvironmentalIssues;

                //entity.AcronymForCorporateEntity = model.AcronymForCorporateEntity;
                //entity.CorporateEntityType = model.SelectedCorporateEntityType;
                //entity.CorporateTitle = model.CorporateTitle;
                //entity.StateOfOriginCorporateEntity = model.StateOfOriginCorporateEntity;
                //entity.CorporateAddress1 = model.CorporateAddress1;
                //entity.CorporateAddress2 = model.CorporateAddress2;
                //entity.City = model.City;
                //entity.State = model.SelectedState;
                //entity.Zip = model.Zip;
                //entity.PropertyCounty = model.PropertyCounty;
                //entity.TaxAssessorNumber = model.TaxAssessorNumber;
                //entity.TaxAssessorNumberOther = model.TaxAssessorNumberOther;
                //entity.TotalRentableFeet = model.TotalRentableFeet;
                //entity.MortgageLienType = model.MortgageLienType;
                //entity.MortgageLienAssumable = model.MortgageLienAssumable;
                //entity.TotalRentableFeetAllApt = model.TotalRentableFeetAllApt;
                //entity.AnnualPropertyTaxes = model.AnnualPropertyTaxes;
                //entity.LotSize = model.LotSize;
                //StringBuilder sb = new StringBuilder();
                //if (model.SelectedPreferredMethods != null)
                //{
                //    model.SelectedPreferredMethods.ForEach(f => { sb.Append(f); sb.Append(";"); });
                //}
                //entity.PreferredMethods = sb.ToString();
                //sb = new StringBuilder();
                //if (model.SelectedPreferredContactTime != null)
                //{
                //    model.SelectedPreferredContactTime.ForEach(f => { sb.Append(f); sb.Append(";"); });
                //}
                //entity.PreferredContactTimes = sb.ToString();
                ////entity.MFDetailsString = model.MFDetailsString;
                //entity.AverageAdjustmentToBaseRent = model.AverageAdjustmentToBaseRentalIncomePerUnitAfterRenovations;
                //entity.EstDeferredMaintenance = model.EstDeferredMaintenance;
                //entity.HasPositionMortgage = model.HasPositionMortgage;

                //entity.OperatingStatus = model.OperatingStatus;
                //entity.ProjectName = model.ProjectName;
                //entity.LotNumber = model.LotNumber;
                //entity.Subdivision = model.Subdivision;
                //entity.TaxBookMap = model.TaxBookMap;
                //entity.PropertyCondition = model.PropertyCondition;
                //entity.GradeClassification = model.GradeClassification;
                //entity.OccupancyType = model.OccupancyType;
                //entity.YearBuilt = model.YearBuilt;
                //entity.BuildingsCount = model.BuildingsCount;
                //entity.CurrentBpo = model.CurrentBpo;
                //entity.HasIncome = model.HasIncome;
                //entity.ProformaAnnualIncome = model.ProformaAnnualIncome;
                //entity.ProformaMonthlyIncome = model.ProformaMonthlyIncome;
                //entity.ProformaVacancyFac = model.ProformaVacancyFac;
                //entity.ProformaAnnualOperExpenses = model.ProformaAnnualOperExpenses;
                //entity.isPendingForeclosure = model.isPendingForeclosure;
                //entity.ForeclosureLender = model.ForeclosureLender;
                //entity.ForeclosurePosition = model.ForeclosurePosition;
                //entity.ForeclosureRecordNumber = model.ForeclosureRecordNumber;
                //entity.ForeclosureOriginalMortgageAmount = model.ForeclosureOriginalMortgageAmount;
                //entity.ForeclosureOriginalMortageDate = model.ForeclosureOriginalMortageDate;
                //entity.ForeclosureSaleDate = model.ForeclosureSaleDate;
                //entity.ForeclosureRecordDate = model.ForeclosureRecordDate;

                ////MF Assets only
                //entity.ElectricMeterMethod = model.ElectricMeterMethod;
                //entity.GasMeterMethod = model.GasMeterMethod;
                //entity.OccupancyPercentage = model.OccupancyPercentage;
                //entity.LastReportedDate = model.LastReportedDate;
                //entity.TotalUnits = model.TotalUnits;
                //entity.CoveredParkingSpaces = model.CoveredParkingSpaces;
                //entity.ParkingSpaces = model.ParkingSpaces;

                //entity.Type = model.Type;
                //entity.NumberOfTenants = model.NumberOfTenants;
                //entity.HasAAARatedMajorTenant = model.HasAAARatedMajorTenant;
                //entity.NameOfAAARatedMajorTenant = model.NameOfAAARatedMajorTenant;
                //entity.NumberofSuites = model.NumberofSuites;
                //entity.VacantSuites = model.VacantSuites;
                //entity.ProformaSgi = model.ProformaSgi;
                //entity.ProformaAnnualNoi = model.ProformaAnnualNoi;

                //context.RealEstateCommercialAssets.Add(entity);
                //context.Save();

                // save real asset now
                var assetType = AssetType.Other;
                switch (model.TypeOfProperty)
                {
                    case "Retail":
                        assetType = AssetType.Retail;
                        break;
                    case "Office":
                        assetType = AssetType.Office;
                        break;
                    case "MultiFamily":
                        assetType = AssetType.MultiFamily;
                        break;
                    case "Industrial":
                        assetType = AssetType.Industrial;
                        break;
                    case "MHP":
                        assetType = AssetType.MHP;
                        break;
                    case "ConvenienceStoreFuel":
                        assetType = AssetType.ConvenienceStoreFuel;
                        break;
                    case "Medical":
                        assetType = AssetType.Medical;
                        break;
                    case "MixedUse":
                        assetType = AssetType.MixedUse;
                        break;
                    case "Hotel":
                        assetType = AssetType.Hotel;
                        break;
                    case "Other":
                        assetType = AssetType.Other;
                        break;
                }

                if (assetType == AssetType.MHP || assetType == AssetType.MultiFamily)
                {
                    #region Saving MultiFamily asset
                    var asset = new MultiFamilyAsset();
                    if (user != null)
                    {
                        asset.ListedByUserId = user.UserId;
                    }
                    if (!string.IsNullOrEmpty(model.TaxAssessorNumber)) context.AssetTaxParcelNumbers.Add(new AssetTaxParcelNumber()
                    {
                        AssetId = model.GuidId,
                        TaxParcelNumber = model.TaxAssessorNumber
                    });
                    if (!string.IsNullOrEmpty(model.TaxAssessorNumberOther)) context.AssetTaxParcelNumbers.Add(new AssetTaxParcelNumber()
                    {
                        AssetId = model.GuidId,
                        TaxParcelNumber = model.TaxAssessorNumberOther
                    });
                    var last = context.Assets.OrderByDescending(s => s.AssetNumber).FirstOrDefault();
                    if (last != null)
                    {
                        asset.AssetNumber = last.AssetNumber + 1;
                    }
                    else
                    {
                        asset.AssetNumber = 1000;
                    }
                    asset.AssetSellerId = sellerId;
                    asset.IsActive = true;
                    asset.AssetId = model.GuidId;
                    asset.AssetCategory = AssetCategory.Real;
                    asset.OperatingStatus = model.OperatingStatus;
                    asset.AssetType = assetType;
                    asset.Owner = model.CorporateName;
                    asset.ProjectName = model.ProjectName;
                    asset.ContactPhoneNumber = model.WorkPhone;
                    asset.GradeClassification = model.GradeClassification;
                    var october = new DateTime(DateTime.Now.Year, 10, 1);
                    asset.PropertyTaxYear = DateTime.Now <= october ? DateTime.Now.AddYears(-1).Year : DateTime.Now.Year;

                    //MF Details
                    asset.OccupancyPercentage = model.OccupancyPercentage;
                    asset.LastReportedDate = DateTime.Now;
                    asset.ElectricMeterMethod = model.ElectricMeterMethod;
                    asset.GasMeterMethod = model.GasMeterMethod;
                    asset.TotalSquareFootage = model.RentableSquareFeet != null ? (int)model.RentableSquareFeet : 0;
                    asset.TotalUnits = model.TotalUnits;
                    asset.ParkOwnedMHUnits = 0;
                    asset.GrossRentableSquareFeet = model.RentableSquareFeet != null ? (int)model.RentableSquareFeet : 0;
                    //asset.UnitSpecifications = model.UnitSpecifications;
                    asset.MFDetailsString = model.MFDetailsString;
                    asset.ParkingSpaces = model.ParkingSpaces.GetValueOrDefault(0);
                    asset.CoveredParkingSpaces = model.CoveredParkingSpaces.GetValueOrDefault(0);
                    asset.LastReportedOccupancyDate = model.LastReportedDateMF;

                    asset.HasDeferredMaintenance = model.HasDeferredMaintenance;
                    asset.EstDeferredMaintenance = model.EstDeferredMaintenance;
                    //asset.EstDefMaintenanceDetailsString = model.Es;

                    //asset.ListedByRealtor = 
                    //asset.ListingAgentCompany =
                    //asset.ListingAgentName = 
                    //asset.ListingAgentEmail = 
                    //asset.ListingAgentPhoneNumber = 
                    //asset.ListingAgentCorpAddress = 
                    //asset.ListingAgentCommissionAmount = 
                    //asset.NarMemberId  = 
                    //asset.NarMember = 
                    //asset.CommissionShareAgr = 
                    //asset.DateOfCsaConfirm = 
                    asset.PropertyAddress = model.PropertyAddress;
                    asset.PropertyAddress2 = model.PropertyAddress2;
                    asset.City = model.PropertyCity;
                    asset.State = model.PropertyState;
                    asset.Zip = model.PropertyZip;
                    asset.County = model.PropertyCounty;
                    asset.LotNumber = model.LotNumber;
                    asset.Subdivision = model.Subdivision;
                    asset.TaxBookMap = model.TaxBookMap;
                    asset.TaxParcelNumber = model.TaxAssessorNumber;
                    asset.PropertyCondition = model.PropertyCondition;
                    asset.OccupancyType = model.OccupancyType;
                    asset.YearBuilt = model.YearBuilt != null ? (int)model.YearBuilt : 0;
                    asset.SquareFeet = model.RentableSquareFeet != null ? (int)model.RentableSquareFeet : 0;
                    asset.LotSize = model.LotSize;
                    asset.ParkingSpaces = model.ParkingSpaces.GetValueOrDefault(0);
                    asset.CoveredParkingSpaces = model.CoveredParkingSpaces.GetValueOrDefault(0);
                    asset.AnnualPropertyTax = model.AnnualPropertyTaxes;
                    //asset.PropertyTaxYear = 
                    //asset.CurrentPropertyTaxes = 
                    //asset.YearsInArrearTaxes = 
                    asset.CurrentBpo = model.CurrentBpo;
                    asset.AskingPrice = model.AskingSalePrice != null ? (double)model.AskingSalePrice : 0;
                    //asset.IsTBDMarket = 
                    asset.HasIncome = model.HasIncome;
                    //asset.MonthlyGrossIncome = 
                    asset.AnnualGrossIncome = model.CurrentAnnualIncome != null ? (double)model.CurrentAnnualIncome : 0;
                    //asset.HasIncomeReason = model.HasIn
                    //asset.CashInvestmentApy = 
                    //asset.ClosingDate = 
                    //asset.ActualClosingDate = 
                    //asset.ProposedBuyer = 
                    //asset.ProposedBuyerAddress = 
                    //asset.ProposedBuyerContact = 
                    asset.Approved = false;
                    asset.IsSubmitted = false;
                    asset.Show = false;
                    asset.CreationDate = DateTime.Now;
                    asset.IsPaper = false;
                    asset.CommissionShareToEPI = 0;
                    //asset.DateCommissionToEpiReceived = 
                    //asset.ListedByUserId = model.UserId;
                    asset.ListingStatus = ListingStatus.Available;
                    //asset.BathCount = 
                    //asset.BedCount = 
                    asset.BuildingsCount = model.BuildingsCount;
                    //asset.ClosingPrice = 

                    asset.ProformaAnnualIncome = model.ProformaAnnualIncome;
                    asset.ProformaMonthlyIncome = model.ProformaMonthlyIncome;
                    asset.ProformaMiscIncome = model.ProformaMiscIncome;
                    asset.ProformaVacancyFac = model.ProformaVacancyFac;
                    asset.CurrentVacancyFac = model.CurrentVacancyFactor != null ? (double)model.CurrentVacancyFactor : 0;
                    //asset.LastReportedOccPercent =
                    asset.ProformaAnnualOperExpenses = model.ProformaAnnualOperExpenses;
                    //asset.ProformaAoeFactorAsPerOfSGI = 

                    asset.ForeclosureLender = model.ForeclosureLender;
                    asset.ForeclosurePosition = model.ForeclosurePosition;
                    asset.ForeclosureRecordNumber = model.ForeclosureRecordNumber;
                    asset.ForeclosureOriginalMortgageAmount = model.ForeclosureOriginalMortgageAmount;
                    asset.ForeclosureOriginalMortageDate = model.ForeclosureOriginalMortageDate;
                    asset.ForeclosureSaleDate = model.ForeclosureSaleDate;
                    asset.ForeclosureRecordDate = model.ForeclosureRecordDate;

                    //asset.PaperType =
                    //asset.PaperPropertyType =
                    //asset.PaperServicingAgent =
                    //asset.PaperAssignor =
                    //asset.PaperPrincipalBalance =
                    //asset.PaperAskingPrice =
                    //asset.PaperApyForAskingPrice =
                    //asset.PaperMonthlyInterestIncome =
                    //asset.PaperEquityMargin =
                    //asset.PaperMonthsInArrears =
                    //asset.PaperMaturityDate =
                    //asset.PaperNextDueDate =
                    //asset.PaperOriginationDate =
                    //asset.PriorityMortgageBalance =
                    //asset.PaperOriginalInstrDocument =
                    //asset.PaperCurrent =
                    //asset.PaperNoteRate =
                    //asset.PaperInvestmentYield =
                    //asset.PaperPriority =
                    //asset.PaperLtv =
                    //asset.PaperCltv =
                    //asset.PaperSuccessor =
                    //asset.PaperSuccessorAddress =
                    //asset.PaperSuccessorRecordedDocNumber =
                    //asset.PaperSuccessorDocDate =
                    //asset.PaperSuccessorType =
                    //asset.PaperTrustor =
                    //asset.PaperTrustee =
                    //asset.HoldForUserId =
                    //asset.HoldStartDate =
                    //asset.AuctionDate =
                    //asset.HoldEndDate =
                    //asset.LastReportedOccupancyDate =
                    //asset.AverageAdjustmentToBaseRentalIncomePerUnitAfterRenovations =
                    asset.IsSampleAsset = false;

                    asset.HasPositionMortgage = model.HasPositionMortgage;
                    asset.MortgageLienType = model.MortgageLienType;
                    asset.MortgageLienAssumable = model.MortgageLienAssumable;
                    asset.FirstMortgageCompany = model.FirstMortgageCompany;
                    asset.MortgageCompanyAddress = model.MortgageCompanyAddress;
                    asset.MortgageCompanyCity = model.MortgageCompanyCity;
                    asset.MortgageCompanyState = model.MortgageCompanyState;
                    asset.MortgageCompanyZip = model.MortgageCompanyZip;
                    asset.LenderPhone = model.LenderPhone;
                    asset.LenderPhoneOther = model.LenderPhoneOther;
                    asset.AccountNumber = model.AccountNumber;
                    asset.CurrentPrincipalBalance = model.CurrentPrincipalBalance;
                    asset.MonthlyPayment = model.MonthlyPayment;
                    asset.PaymentIncludes = model.PaymentIncludes;
                    asset.InterestRate = model.InterestRate;
                    asset.IsMortgageAnARM = model.IsMortgageAnARM;
                    asset.MortgageAdjustIfARM = model.MortgageAdjustIfARM;
                    asset.NumberOfMissedPayments = model.NumberOfMissedPayments;
                    asset.BalloonDateOfNote = model.BalloonDateOfNote;
                    //asset.AmortizationSchedule = model.;    

                    asset.CorporateOwnershipAddress = model.CorporateOwnershipAddress;
                    asset.CorporateOwnershipAddress2 = model.CorporateOwnershipAddress2;
                    asset.CorporateOwnershipCity = model.CorporateOwnershipCity;
                    asset.CorporateOwnershipState = model.CorporateOwnershipState;
                    asset.CorporateOwnershipZip = model.CorporateOwnershipZip;

                    asset.NumberRentableSpace = model.NumberRentableSpace;
                    asset.NumberNonRentableSpace = model.NumberNonRentableSpace;
                    asset.NumberParkOwnedMH = model.NumberParkOwnedMH;
                    asset.AccessRoadTypeId = (AccessRoadType)model.AccessRoadTypeId;
                    asset.InteriorRoadTypeId = (InteriorRoadType)model.InteriorRoadTypeId;
                    asset.MHPadTypeId = (MHPadType)model.MHPadTypeId;
                    asset.WasteWaterTypeId = (WasteWaterType)model.WasteWaterTypeId;
                    asset.WaterServTypeId = (WaterServType)model.WaterServTypeId;
                    asset.TermsOther = model.TermsOther;

                    if (model.OfferingPriceDeterminedByMarketBidding.HasValue && model.OfferingPriceDeterminedByMarketBidding == true)
                        asset.IndicativeBidsDueDate = model.IndicativeBidsDueDate;
                    if (model.RenovatedByOwner.HasValue && model.RenovatedByOwner == true)
                    {
                        asset.RenovatedByOwner = true;
                        if (model.RenovationYear.HasValue)
                            asset.RenovationYear = model.RenovationYear.Value;
                        asset.RecentUpgradesRenovations = model.RecentUpgradesRenovations;
                        asset.RenovationBudget = model.RenovationBudget;
                    }
                    else
                    {
                        asset.RenovatedByOwner = false;
                    }

                    context.Assets.Add(asset);
                    context.Save();

                    foreach (var image in model.Images)
                    {
                        context.AssetImages.Add(new AssetImage()
                        {
                            AssetId = model.GuidId,
                            ContentType = image.ContentType,
                            FileName = image.FileName,
                            IsFlyerImage = false,
                            IsMainImage = false,
                            Order = image.Order,
                            AssetImageId = Guid.NewGuid()
                        });
                        context.Save();
                    }
                    foreach (var doc in model.Documents)
                    {
                        context.AssetDocuments.Add(new AssetDocument()
                        {
                            AssetId = model.GuidId,
                            ContentType = doc.ContentType,
                            Description = doc.Description,
                            FileName = doc.FileName,
                            Order = doc.Order,
                            Size = doc.Size,
                            Title = doc.Title,
                            Type = doc.Type,
                            AssetDocumentId = Guid.NewGuid()
                        });
                        context.Save();
                    }
                    foreach (var vid in model.Videos)
                    {
                        context.AssetVideos.Add(new AssetVideo()
                        {
                            Asset = asset,
                            AssetId = model.GuidId,
                            Description = vid.Description,
                            FilePath = vid.FilePath
                        });
                        context.Save();
                    }
                    foreach (var spec in model.UnitSpecifications)
                    {
                        context.AssetUnitSpecifications.Add(new AssetUnitSpecification()
                        {
                            AssetId = model.GuidId,
                            AssetUnitSpecificationId = Guid.NewGuid(),
                            BedCount = spec.BedCount,
                            BathCount = spec.BathCount,
                            UnitBaseRent = spec.UnitBaseRent,
                            UnitSquareFeet = spec.UnitSquareFeet,
                            CountOfUnits = spec.CountOfUnits
                        });
                        context.Save();
                    }

                    if (model.MHPUnitSpecifications != null)
                    {
                        foreach (var spec in model.MHPUnitSpecifications)
                        {
                            context.AssetMHPSpecifications.Add(new AssetMHPSpecification()
                            {
                                AssetId = model.GuidId,
                                AssetMHPSpecificationId = Guid.NewGuid(),
                                CountOfUnits = spec.CountOfUnits,
                                CurrentDoubleBaseRent = spec.CurrentDoubleBaseRent,
                                CurrentSingleBaseRent = spec.CurrentSingleBaseRent,
                                CurrentTripleBaseRent = spec.CurrentTripleBaseRent,
                                NumberDoubleWide = spec.NumberDoubleWide,
                                NumberSingleWide = spec.NumberSingleWide,
                                CurrentDoubleOwnedBaseRent = spec.CurrentDoubleOwnedBaseRent,
                                CurrentSingleOwnedBaseRent = spec.CurrentSingleOwnedBaseRent,
                                CurrentTripleOwnedBaseRent = spec.CurrentTripleOwnedBaseRent,
                                NumberDoubleWideOwned = spec.NumberDoubleWideOwned,
                                NumberSingleWideOwned = spec.NumberSingleWideOwned,
                                NumberTripleWide = spec.NumberTripleWide,
                                NumberTripleWideOwned = spec.NumberTripleWideOwned,
                            });
                            context.Save();
                        }
                    }

                    if (user != null)
                    {
                        // if user is a listing agent, make them the listing agent of the asset
                        if (user.UserType == UserType.ListingAgent)
                        {
                            // we really need to implement to FKey changes all throughout the domain to fix this
                            var nar = context.NarMembers.First(x => x.Email == user.Username.ToLower());
                            if (nar != null)
                            {
                                // 5/28 Test
                                try
                                {
                                    AssetNARMember anm = new AssetNARMember();
                                    anm.Asset = asset;
                                    anm.AssetId = asset.AssetId;
                                    anm.AssetNARMemberId = Guid.NewGuid();
                                    anm.NARMember = nar;
                                    anm.NarMemberId = nar.NarMemberId;
                                    context.AssetNARMembers.Add(anm);
                                    context.Save();
                                }
                                catch (Exception)
                                {

                                    throw;
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Saving Commercial asset
                    var asset = new CommercialAsset();
                    if (user != null)
                    {
                        asset.ListedByUserId = user.UserId;
                    }
                    if (!string.IsNullOrEmpty(model.TaxAssessorNumber)) context.AssetTaxParcelNumbers.Add(new AssetTaxParcelNumber()
                    {
                        AssetId = model.GuidId,
                        TaxParcelNumber = model.TaxAssessorNumber
                    });
                    if (!string.IsNullOrEmpty(model.TaxAssessorNumberOther)) context.AssetTaxParcelNumbers.Add(new AssetTaxParcelNumber()
                    {
                        AssetId = model.GuidId,
                        TaxParcelNumber = model.TaxAssessorNumberOther
                    });
                    var last = context.Assets.OrderByDescending(s => s.AssetNumber).FirstOrDefault();
                    if (last != null)
                    {
                        asset.AssetNumber = last.AssetNumber + 1;
                    }
                    else
                    {
                        asset.AssetNumber = 1000;
                    }
                    asset.AssetSellerId = sellerId;
                    asset.IsActive = true;
                    asset.AssetId = model.GuidId;
                    asset.AssetCategory = AssetCategory.Real;
                    asset.OperatingStatus = model.OperatingStatus;
                    asset.AssetType = assetType;
                    asset.Owner = model.CorporateName;
                    asset.ProjectName = model.ProjectName;
                    asset.ContactPhoneNumber = model.WorkPhone;
                    asset.GradeClassification = model.GradeClassification;
                    var october = new DateTime(DateTime.Now.Year, 10, 1);
                    asset.PropertyTaxYear = DateTime.Now <= october ? DateTime.Now.AddYears(-1).Year : DateTime.Now.Year;
                    asset.NumberOfRentableSuites = asset.NumberOfRentableSuites;
                    asset.HasDeferredMaintenance = model.HasDeferredMaintenance;
                    asset.EstDeferredMaintenance = model.EstDeferredMaintenance;

                    asset.Type = model.Type;
                    asset.RentableSquareFeet = model.RentableSquareFeet != null ? (int)model.RentableSquareFeet : 0;
                    asset.ProformaSgi = model.ProformaSgi;
                    asset.ProformaAnnualNoi = model.ProformaAnnualNoi;
                    asset.NumberOfTenants = model.NumberOfTenants;
                    asset.HasAAARatedMajorTenant = model.HasAAARatedMajorTenant;
                    asset.NameOfAAARatedMajorTenant = model.NameOfAAARatedMajorTenant;
                    asset.NumberofSuites = model.NumberofSuites;
                    asset.OccupancyPercentage = model.OccupancyPercentage;
                    asset.OccupancyDate = model.OccupancyDate;
                    asset.PropertyDetailsString = model.PropertyDetailsString;
                    asset.VacantSuites = model.VacantSuites;
                    asset.LeasedSquareFootageByMajorTenant = model.LeasedSquareFootageByMajorTenant;
                    asset.BaseRentPerSqFtMajorTenant = model.BaseRentPerSqFtMajorTenant;
                    asset.CurrentMarkerRentPerSqFt = model.CurrentMarkerRentPerSqFt;
                    asset.IsMajorTenantAAARated = model.IsMajorTenantAAARated;
                    asset.ParkingSpaces = model.ParkingSpaces.GetValueOrDefault(0);
                    asset.CoveredParkingSpaces = model.CoveredParkingSpaces.GetValueOrDefault(0);
                    asset.LastReportedOccupancyDate = model.LastReportedDateCommercial;
                    //asset.ListedByRealtor = 
                    //asset.ListingAgentCompany =
                    //asset.ListingAgentName = 
                    //asset.ListingAgentEmail = 
                    //asset.ListingAgentPhoneNumber = 
                    //asset.ListingAgentCorpAddress = 
                    //asset.ListingAgentCommissionAmount = 
                    //asset.NarMemberId  = 
                    //asset.NarMember = 
                    //asset.CommissionShareAgr = 
                    //asset.DateOfCsaConfirm = 
                    asset.PropertyAddress = model.PropertyAddress;
                    asset.PropertyAddress2 = model.PropertyAddress2;
                    asset.City = model.PropertyCity;
                    asset.State = model.PropertyState;
                    asset.Zip = model.PropertyZip;
                    asset.County = model.PropertyCounty;
                    asset.LotNumber = model.LotNumber;
                    asset.Subdivision = model.Subdivision;
                    asset.TaxBookMap = model.TaxBookMap;
                    asset.TaxParcelNumber = model.TaxAssessorNumber;
                    asset.PropertyCondition = model.PropertyCondition;
                    asset.OccupancyType = model.OccupancyType;
                    asset.YearBuilt = model.YearBuilt != null ? (int)model.YearBuilt : 0;
                    asset.SquareFeet = model.RentableSquareFeet != null ? (int)model.RentableSquareFeet : 0;
                    asset.LotSize = model.LotSize;
                    asset.ParkingSpaces = model.ParkingSpaces.GetValueOrDefault(0);
                    asset.CoveredParkingSpaces = model.CoveredParkingSpaces.GetValueOrDefault(0);
                    asset.AnnualPropertyTax = model.AnnualPropertyTaxes;
                    //asset.PropertyTaxYear = 
                    //asset.CurrentPropertyTaxes = 
                    //asset.YearsInArrearTaxes = 
                    asset.CurrentBpo = model.CurrentBpo;
                    asset.AskingPrice = model.AskingSalePrice != null ? (double)model.AskingSalePrice : 0;
                    //asset.IsTBDMarket = 
                    asset.HasIncome = model.HasIncome;
                    //asset.MonthlyGrossIncome = 
                    asset.AnnualGrossIncome = model.CurrentAnnualIncome != null ? (double)model.CurrentAnnualIncome : 0;
                    //asset.HasIncomeReason = model.HasIn
                    //asset.CashInvestmentApy = 
                    //asset.ClosingDate = 
                    //asset.ActualClosingDate = 
                    //asset.ProposedBuyer = 
                    //asset.ProposedBuyerAddress = 
                    //asset.ProposedBuyerContact = 
                    asset.Approved = false;
                    asset.IsSubmitted = false;
                    asset.Show = false;
                    asset.CreationDate = DateTime.Now;
                    asset.IsPaper = false;
                    asset.CommissionShareToEPI = 0;
                    //asset.DateCommissionToEpiReceived = 
                    //asset.ListedByUserId = model.UserId;
                    asset.ListingStatus = ListingStatus.Available;
                    //asset.BathCount = 
                    //asset.BedCount = 
                    asset.BuildingsCount = model.BuildingsCount;
                    //asset.ClosingPrice = 

                    asset.ProformaAnnualIncome = model.ProformaAnnualIncome;
                    asset.ProformaMonthlyIncome = model.ProformaMonthlyIncome;
                    //asset.ProformaMiscIncome =
                    asset.ProformaVacancyFac = model.ProformaVacancyFac;
                    asset.CurrentVacancyFac = model.CurrentVacancyFactor != null ? (double)model.CurrentVacancyFactor : 0;
                    //asset.LastReportedOccPercent =
                    asset.ProformaAnnualOperExpenses = model.ProformaAnnualOperExpenses;
                    //asset.ProformaAoeFactorAsPerOfSGI = 

                    asset.ForeclosureLender = model.ForeclosureLender;
                    asset.ForeclosurePosition = model.ForeclosurePosition;
                    asset.ForeclosureRecordNumber = model.ForeclosureRecordNumber;
                    asset.ForeclosureOriginalMortgageAmount = model.ForeclosureOriginalMortgageAmount;
                    asset.ForeclosureOriginalMortageDate = model.ForeclosureOriginalMortageDate;
                    asset.ForeclosureSaleDate = model.ForeclosureSaleDate;
                    asset.ForeclosureRecordDate = model.ForeclosureRecordDate;

                    //asset.HoldForUserId =
                    //asset.HoldStartDate =
                    //asset.AuctionDate =
                    //asset.HoldEndDate =
                    //asset.AverageAdjustmentToBaseRentalIncomePerUnitAfterRenovations =
                    asset.IsSampleAsset = false;

                    asset.HasPositionMortgage = model.HasPositionMortgage;
                    asset.MortgageLienType = model.MortgageLienType;
                    asset.MortgageLienAssumable = model.MortgageLienAssumable;
                    asset.FirstMortgageCompany = model.FirstMortgageCompany;
                    asset.MortgageCompanyAddress = model.MortgageCompanyAddress;
                    asset.MortgageCompanyCity = model.MortgageCompanyCity;
                    asset.MortgageCompanyState = model.MortgageCompanyState;
                    asset.MortgageCompanyZip = model.MortgageCompanyZip;
                    asset.LenderPhone = model.LenderPhone;
                    asset.LenderPhoneOther = model.LenderPhoneOther;
                    asset.AccountNumber = model.AccountNumber;
                    asset.CurrentPrincipalBalance = model.CurrentPrincipalBalance;
                    asset.MonthlyPayment = model.MonthlyPayment;
                    asset.PaymentIncludes = model.PaymentIncludes;
                    asset.InterestRate = model.InterestRate;
                    asset.IsMortgageAnARM = model.IsMortgageAnARM;
                    asset.MortgageAdjustIfARM = model.MortgageAdjustIfARM;
                    asset.NumberOfMissedPayments = model.NumberOfMissedPayments;
                    asset.BalloonDateOfNote = model.BalloonDateOfNote;
                    // asset.AmortizationSchedule = model.;    

                    asset.CorporateOwnershipAddress = model.CorporateOwnershipAddress;
                    asset.CorporateOwnershipAddress2 = model.CorporateOwnershipAddress2;
                    asset.CorporateOwnershipCity = model.CorporateOwnershipCity;
                    asset.CorporateOwnershipState = model.CorporateOwnershipState;
                    asset.CorporateOwnershipZip = model.CorporateOwnershipZip;
                    asset.TermsOther = model.TermsOther;

                    if (model.OfferingPriceDeterminedByMarketBidding.HasValue && model.OfferingPriceDeterminedByMarketBidding == true)
                        asset.IndicativeBidsDueDate = model.IndicativeBidsDueDate;
                    if (model.RenovatedByOwner.HasValue && model.RenovatedByOwner == true)
                    {
                        asset.RenovatedByOwner = true;
                        if (model.RenovationYear.HasValue)
                            asset.RenovationYear = model.RenovationYear.Value;
                        asset.RecentUpgradesRenovations = model.RecentUpgradesRenovations;
                        asset.RenovationBudget = model.RenovationBudget;
                    }
                    else
                    {
                        asset.RenovatedByOwner = false;
                    }

                    context.Assets.Add(asset);
                    context.Save();

                    foreach (var image in model.Images)
                    {
                        context.AssetImages.Add(new AssetImage()
                        {
                            AssetId = model.GuidId,
                            ContentType = image.ContentType,
                            FileName = image.FileName,
                            IsFlyerImage = false,
                            IsMainImage = false,
                            Order = image.Order,
                            AssetImageId = Guid.NewGuid()
                        });
                        context.Save();
                    }
                    foreach (var doc in model.Documents)
                    {
                        context.AssetDocuments.Add(new AssetDocument()
                        {
                            AssetId = model.GuidId,
                            ContentType = doc.ContentType,
                            Description = doc.Description,
                            FileName = doc.FileName,
                            Order = doc.Order,
                            Size = doc.Size,
                            Title = doc.Title,
                            Type = doc.Type,
                            AssetDocumentId = Guid.NewGuid()
                        });
                        context.Save();
                    }
                    foreach (var vid in model.Videos)
                    {
                        context.AssetVideos.Add(new AssetVideo()
                        {
                            Asset = asset,
                            AssetId = model.GuidId,
                            Description = vid.Description,
                            FilePath = vid.FilePath
                        });
                        context.Save();
                    }
                    foreach (var spec in model.UnitSpecifications)
                    {
                        context.AssetUnitSpecifications.Add(new AssetUnitSpecification()
                        {
                            AssetId = model.GuidId,
                            AssetUnitSpecificationId = Guid.NewGuid(),
                            BedCount = spec.BedCount,
                            BathCount = spec.BathCount,
                            UnitBaseRent = spec.UnitBaseRent,
                            UnitSquareFeet = spec.UnitSquareFeet,
                            CountOfUnits = spec.CountOfUnits
                        });
                        context.Save();
                    }

                    if (user != null)
                    {
                        // if user is a listing agent, make them the listing agent of the asset
                        if (user.UserType == UserType.ListingAgent)
                        {
                            // we really need to implement to FKey changes all throughout the domain to fix this
                            var nar = context.NarMembers.First(x => x.Email == user.Username.ToLower());
                            if (nar != null)
                            {
                                // 5/28 Test
                                try
                                {
                                    AssetNARMember anm = new AssetNARMember();
                                    anm.Asset = asset;
                                    anm.AssetId = asset.AssetId;
                                    anm.AssetNARMemberId = Guid.NewGuid();
                                    anm.NARMember = nar;
                                    anm.NarMemberId = nar.NarMemberId;
                                    context.AssetNARMembers.Add(anm);
                                    context.Save();
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }
                        }
                    }
                    #endregion
                }

                //if (user != null)
                //{
                //    // if user is a listing agent, make them the listing agent of the asset
                //    if (user.UserType == UserType.ListingAgent)
                //    {
                //        // we really need to implement to FKey changes all throughout the domain to fix this
                //        var nar = context.NarMembers.First(x => x.Email == user.Username);
                //        if (nar != null)
                //        {
                //            // 5/22 TODO: change to AssetNARMember
                //            //AssetListingAgent agent = new AssetListingAgent();
                //            //agent.AssetId = model.GuidId;
                //            //agent.ListingAgentCellNumber = nar.CellPhoneNumber;
                //            //agent.ListingAgentCity = nar.CompanyCity;
                //            //agent.ListingAgentCompany = nar.CompanyName;
                //            //agent.ListingAgentCorpAddress = nar.CompanyAddressLine1;
                //            //agent.ListingAgentCorpAddress2 = nar.CompanyAddressLine2;
                //            //agent.ListingAgentEmail = nar.Email;
                //            //agent.ListingAgentFaxNumber = nar.FaxNumber;
                //            //agent.ListingAgentName = nar.FullName;
                //            //agent.ListingAgentNewName = nar.NarMemberId.ToString();
                //            //agent.ListingAgentPhoneNumber = nar.CellPhoneNumber;
                //            //agent.ListingAgentState = nar.CompanyState;
                //            //agent.ListingAgentWorkNumber = nar.WorkPhoneNumber;
                //            //agent.ListingAgentZip = nar.CompanyZip;
                //            //agent.AssetListingAgentId = Guid.NewGuid();
                //            //context.AssetListingAgents.Add(agent);
                //            //context.Save();
                //        }
                //    }
                //}

                foreach (var item in model.DeferredMaintenanceItems)
                {
                    if (item.Selected)
                    {
                        var record = context.AssetDeferredMaintenanceItems.FirstOrDefault(w => w.AssetId == model.GuidId && w.MaintenanceDetail == item.MaintenanceDetail);
                        if (record != null)
                        {
                            record.UnitCost = item.UnitCost;
                            record.Units = Convert.ToInt32(item.NumberOfUnits);
                        }
                        else
                        {
                            context.AssetDeferredMaintenanceItems.Add(new AssetDeferredMaintenanceItem()
                            {
                                AssetId = model.GuidId,
                                MaintenanceDetail = item.MaintenanceDetail,
                                UnitCost = item.UnitCost,
                                Units = Convert.ToInt32(item.NumberOfUnits)
                            });
                        }
                        context.Save();
                    }
                }
                saveScope.Complete();

                //return entity.RealEstateCommercialAssetId;
                return 0;
            }
        }

        public int SaveRealEstateResidential(RealEstateResidentialAssetViewModel model)
        {
            var context = _factory.Create();

            var assetSeller = context.AssetSellers.Where(x => x.Email.ToLower() == model.EmailAddress.ToLower()).FirstOrDefault();
            int sellerId = 0;
            if (assetSeller == null)
            {
                var newSeller = new AssetSeller();
                newSeller.NameOfPrincipal = model.NameOfPrincipal;
                //newSeller.PhoneHome = model.HomePhone;
                //newSeller.PhoneOther = model.OtherPhone;
                newSeller.PhoneWork = model.WorkPhone;
                newSeller.Fax = model.Fax;
                newSeller.Email = model.EmailAddress;
                context.AssetSellers.Add(newSeller);
                sellerId = newSeller.AssetSellerId;
            }
            else
            {
                sellerId = assetSeller.AssetSellerId;
            }

            foreach (var image in model.Images)
            {
                var newImage = new TempAssetImage();
                newImage.GuidId = model.GuidId;
                newImage.ContentType = image.ContentType;
                newImage.FileName = image.FileName;
                newImage.Order = image.Order;
                context.TempAssetImages.Add(newImage);
            }
            foreach (var doc in model.Documents)
            {
                var newDoc = new TempAssetDocument();
                newDoc.GuidId = model.GuidId;
                newDoc.ContentType = doc.ContentType;
                newDoc.Description = doc.Description;
                newDoc.FileName = doc.FileName;
                newDoc.Order = doc.Order;
                newDoc.Size = doc.Size;
                newDoc.Title = doc.Title;
                newDoc.Type = doc.Type;
                context.TempAssetDocuments.Add(newDoc);
            }
            foreach (var vid in model.Videos)
            {
                var newVid = new TempAssetVideo();
                newVid.GuidId = model.GuidId;
                newVid.Description = vid.Description;
                newVid.FilePath = vid.FilePath;
                context.TempAssetVideos.Add(newVid);
            }

            var entity = new RealEstateResidentialAsset();

            entity.GuidId = model.GuidId;
            entity.AssetSellerId = sellerId;
            entity.NameOfPrincipal = model.NameOfPrincipal;
            entity.NameOfCoPrincipal = model.NameOfCoPrincipal;
            //entity.HomePhone = model.HomePhone;
            entity.WorkPhone = model.WorkPhone;
            //entity.OtherPhone = model.OtherPhone;
            entity.Fax = model.Fax;
            entity.EmailAddress = model.EmailAddress;

            entity.TypeOfProperty = model.TypeOfProperty;
            entity.PropertyAddress = model.PropertyAddress;
            entity.PropertyCity = model.PropertyCity;
            entity.PropertyState = model.PropertyState;
            entity.PropertyZip = model.PropertyZip;
            entity.MajorCrossStreets = model.MajorCrossStreets;
            entity.PropertyInsuranceCarrier = model.PropertyInsuranceCarrier;
            entity.PolicyNumber = model.PolicyNumber;
            entity.InsuranceAgentName = model.InsuranceAgentName;
            entity.InsuranceAgentPhone = model.InsuranceAgentPhone;
            entity.PropertyInHOA = model.PropertyInHOA;
            entity.HOADues = model.HOADues;
            entity.HOADueTime = model.HOADueTime;
            entity.HOALiensOnProperty = model.HOALiensOnProperty;
            entity.HOALiens = model.HOALiens;
            entity.Language = model.Language;
            entity.DateOfBirth = model.DateOfBirth;
            entity.SSN = model.SSN;
            entity.PropertyListedForSale = model.PropertyListedForSale;
            entity.AgentName = model.AgentName;
            entity.AgentNumber = model.AgentNumber;

            entity.FirstMortgageCompany = model.FirstMortgageCompany;
            entity.FirstMCAddress = model.FirstMCAddress;
            entity.FirstMCCity = model.FirstMCCity;
            entity.FirstMCState = model.FirstMCState;
            entity.FirstMCZip = model.FirstMCZip;
            entity.FirstMCLenderPhone = model.FirstMCLenderPhone;
            entity.FirstMCLenderPhoneOther = model.FirstMCLenderPhoneOther;
            entity.FirstMCAccountNumber = model.FirstMCAccountNumber;
            //entity.FirstMCTypeOfMortgage = model.FirstMCTypeOfMortgage;
            //entity.FirstMCCurrentPrincipalBalance  = model.FirstMCCurrentPrincipalBalance;
            //entity.FirstMCInterestRate  = model.FirstMCInterestRate;
            entity.FirstMCIsMortgageAnARM = model.FirstMCIsMortgageAnARM;
            //entity.FirstMCMortgageAdjustIfARM  = model.FirstMCMortgageAdjustIfARM;
            entity.FirstMCMortgageHasPPP = model.FirstMCMortgageHasPPP;
            entity.FirstMCPPPExpireDate = model.FirstMCPPPExpireDate;
            entity.FirstMCMonthlyPayment = model.FirstMCMonthlyPayment;
            entity.FirstMCPaymentIncludes = model.FirstMCPaymentIncludes;
            entity.FirstMCLastPaymentDate = model.FirstMCLastPaymentDate;
            entity.FirstMCWasLastPaymentForThatMonth = model.FirstMCWasLastPaymentForThatMonth;
            entity.FirstMCNumberOfMissedPayments = model.FirstMCNumberOfMissedPayments;
            entity.FirstMCCurrentMortgageBalance = model.FirstMCCurrentMortgageBalance;
            entity.FirstMCHasForclosureStarted = model.FirstMCHasForclosureStarted;
            entity.FirstMCForclosureSaleDate = model.FirstMCForclosureSaleDate;

            entity.SecondMortgageCompany = model.SecondMortgageCompany;
            entity.SecondMCAddress = model.SecondMCAddress;
            entity.SecondMCCity = model.SecondMCCity;
            entity.SecondMCState = model.SecondMCState;
            entity.SecondMCZip = model.SecondMCZip;
            entity.SecondMCLenderPhone = model.SecondMCLenderPhone;
            entity.SecondMCLenderPhoneOther = model.SecondMCLenderPhoneOther;
            entity.SecondMCAccountNumber = model.SecondMCAccountNumber;
            //entity.SecondMCTypeOfMortgage = model.SecondMCTypeOfMortgage;
            //entity.SecondMCCurrentPrincipalBalance  = model.SecondMCCurrentPrincipalBalance;
            //entity.SecondMCInterestRate  = model.SecondMCInterestRate;
            entity.SecondMCIsMortgageAnARM = model.SecondMCIsMortgageAnARM;
            //entity.SecondMCMortgageAdjustIfARM  = model.SecondMCMortgageAdjustIfARM;
            entity.SecondMCMortgageHasPPP = model.SecondMCMortgageHasPPP;
            entity.SecondMCPPPExpireDate = model.SecondMCPPPExpireDate;
            entity.SecondMCMonthlyPayment = model.SecondMCMonthlyPayment;
            entity.SecondMCPaymentIncludes = model.SecondMCPaymentIncludes;
            entity.SecondMCLastPaymentDate = model.SecondMCLastPaymentDate;
            entity.SecondMCWasLastPaymentForThatMonth = model.SecondMCWasLastPaymentForThatMonth;
            entity.SecondMCNumberOfMissedPayments = model.SecondMCNumberOfMissedPayments;
            entity.SecondMCCurrentMortgageBalance = model.SecondMCCurrentMortgageBalance;
            entity.SecondMCHasForclosureStarted = model.SecondMCHasForclosureStarted;
            entity.SecondMCForclosureSaleDate = model.SecondMCForclosureSaleDate;

            entity.PaymentsFallBehindReason = model.PaymentsFallBehindReason;
            entity.FormOfResidence = model.FormOfResidence;
            entity.NumberOfYears = model.NumberOfYears;
            entity.MaritalStatus = model.MaritalStatus;
            entity.Dependants = model.Dependants;
            entity.Bedrooms = model.Bedrooms;
            entity.Bathrooms = model.Bathrooms;
            entity.Garage = model.Garage;
            entity.Carport = model.Carport;
            entity.Pool = model.Pool;
            entity.Spa = model.Spa;
            entity.BuildingStyle = model.BuildingStyle;
            entity.Architecture = model.Architecture;
            entity.HOA = model.HOA;
            entity.RecentUpgradeOrRemodel = model.RecentUpgradeOrRemodel;
            entity.DefferedMaintenance = model.DefferedMaintenance;

            context.RealEstateResidentialAssets.Add(entity);
            context.Save();
            return entity.RealEstateResidentialAssetId;
        }

        public PaperCommercialAssetViewModel GetPaperCommercialModel(int assetId)
        {
            var model = new PaperCommercialAssetViewModel();
            var context = _factory.Create();
            var pc = context.PaperCommercialAssets.Where(x => x.PaperCommercialAssetId == assetId).FirstOrDefault();
            if (pc != null)
            {
                var images = context.TempAssetImages.Where(x => x.GuidId == pc.GuidId).ToList();
                var docs = context.TempAssetDocuments.Where(x => x.GuidId == pc.GuidId).ToList();
                var vids = context.TempAssetVideos.Where(x => x.GuidId == pc.GuidId).ToList();
                //if (images.Count > 0) model.Images.AddRange(images);
                if (docs.Count > 0) model.Documents.AddRange(docs);
                if (vids.Count > 0) model.Videos.AddRange(vids);

                model.GuidId = pc.GuidId;
                //model.BeneficiaryName = pc.BeneficiaryName;
                //model.BeneficiaryFullName = pc.BeneficiaryFullName;
                //model.BeneficiaryContactAddress = pc.BeneficiaryContactAddress;
                //model.BeneficiaryCity = pc.BeneficiaryCity;
                //model.BeneficiaryState = pc.BeneficiaryState;
                //model.BeneficiaryZip = pc.BeneficiaryZip;
                ////model.BeneficiaryPhoneHome = pc.BeneficiaryPhoneHome;
                //model.BeneficiaryPhoneCell = pc.BeneficiaryPhoneCell;
                //model.BeneficiaryPhoneWork = pc.BeneficiaryPhoneWork;
                //model.BeneficiaryFax = pc.BeneficiaryFax;
                //model.BeneficiaryEmail = pc.BeneficiaryEmail;
                //model.BeneficiaryAccountNumber = pc.BeneficiaryAccountNumber;

                model.NotePayerName = pc.NotePayerName;
                model.NotePayerFullName = pc.NotePayerFullName;
                model.NotePayerContactAddress = pc.NotePayerContactAddress;
                model.NotePayerCity = pc.NotePayerCity;
                model.NotePayerState = pc.NotePayerState;
                model.NotePayerZip = pc.NotePayerZip;
                model.NotePayerPhoneHome = pc.NotePayerPhoneHome;
                model.NotePayerPhoneCell = pc.NotePayerPhoneCell;
                model.NotePayerPhoneWork = pc.NotePayerPhoneWork;
                model.NotePayerFax = pc.NotePayerFax;
                model.NotePayerEmail = pc.NotePayerEmail;
                model.NotePayerSSNOrTIN = pc.NotePayerSSNOrTIN;
                model.NotePayerFICO = pc.NotePayerFICO;

                model.SecuringPropertyAddress = pc.SecuringPropertyAddress;
                model.SecuringPropertyCity = pc.SecuringPropertyCity;
                model.SecuringPropertyState = pc.SecuringPropertyState;
                model.SecuringPropertyZip = pc.SecuringPropertyZip;
                model.TypeOfProperty = pc.TypeOfProperty;
                model.PropertySquareFeet = pc.PropertySquareFeet;
                model.ApartmentUnits = pc.ApartmentUnits;
                model.YearBuilt = pc.YearBuilt;
                model.LotSize = pc.LotSize;
                model.Spaces = pc.Spaces;
                model.PropertyGOI = pc.PropertyGOI;
                model.PropertyGOE = pc.PropertyGOE;
                model.PropertyVF = pc.PropertyVF;
                model.AnnualGOI = pc.AnnualGOI;
                model.AnnualGOE = pc.AnnualGOE;
                model.AnnualVF = pc.AnnualVF;
                model.CurrentVacancyFactor = pc.CurrentVacancyFactor;
                model.WaterService = pc.WaterService;
                model.SewerService = pc.SewerService;
                model.PowerService = pc.PowerService;
                model.PropertyAccess = pc.PropertyAccess;

                model.NoteOrigination = pc.NoteOrigination;
                model.NotePrincipal = pc.NotePrincipal;
                model.BPOOfProperty = pc.BPOOfProperty;
                model.CurrentNotePrincipal = pc.CurrentNotePrincipal;
                model.HasCopyOfAppraisal = pc.HasCopyOfAppraisal;
                model.MethodOfAppraisal = pc.MethodOfAppraisal;
                model.TypeOfNote = pc.TypeOfNote;
                model.NoteInterestRate = pc.NoteInterestRate;
                model.PaymentAmount = pc.PaymentAmount;
                model.PaymentFrequency = pc.PaymentFrequency;
                model.BalloonDateOfNote = pc.BalloonDateOfNote;
                model.TypeOfMTGInstrument = pc.TypeOfMTGInstrument;
                model.AmortType = pc.AmortType;
                model.PaymentsMadeOnNote = pc.PaymentsMadeOnNote;
                model.PaymentsRemainingOnNote = pc.PaymentsRemainingOnNote;
                model.IsNoteCurrent = pc.IsNoteCurrent;
                model.LastPaymentRecievedOnNote = pc.LastPaymentRecievedOnNote;
                model.NextPaymentOnNote = pc.NextPaymentOnNote;
                model.SecuringPropertyAppraisal = pc.SecuringPropertyAppraisal;
                model.WasPropertyDistressed = pc.WasPropertyDistressed;
                model.PaymentHistory = pc.PaymentHistory;
                model.SellerCarryNoteSalesDate = pc.SellerCarryNoteSalesDate;
                model.SellerCarryNotePrice = pc.SellerCarryNotePrice;
                model.SellerCarryNoteCashDown = pc.SellerCarryNoteCashDown;
                model.OriginalPrincipalBalanceWRAP = pc.OriginalPrincipalBalanceWRAP;
                model.TotalMonthlyPaymentWRAP = pc.TotalMonthlyPaymentWRAP;
                model.FirstmortgageBalanceWRAP = pc.FirstmortgageBalanceWRAP;
                model.FirstInterestRateWRAP = pc.FirstInterestRateWRAP;
                model.FirstMortgagePaymentWRAP = pc.FirstMortgagePaymentWRAP;
                model.SecondMortgageBalanceWRAP = pc.SecondMortgageBalanceWRAP;
                model.SecondInterestRateWRAP = pc.SecondInterestRateWRAP;
                model.SecondMortgagePaymentWRAP = pc.SecondMortgagePaymentWRAP;
                model.NoteServicedByAgent = pc.NoteServicedByAgent;
                model.AgentName = pc.AgentName;
                model.AgentPhone = pc.AgentPhone;
                model.AgentEmail = pc.AgentEmail;
                model.AgentAccountNumber = pc.AgentAccountNumber;
                model.AgentContactPerson = pc.AgentContactPerson;
                model.AuthorizeForwardPaymentHistory = pc.AuthorizeForwardPaymentHistory;
                model.AgentAddress = pc.AgentAddress;
                model.AgentCity = pc.AgentCity;
                model.AgentState = pc.AgentState;
                model.AgentZip = pc.AgentZip;

                //model.HasPicturesOfProperty = pc.HasPicturesOfProperty;
                //model.GeneralComments = pc.GeneralComments;
                //model.ResellingReason = pc.ResellingReason;
                model.HasEnvironmentalIssues = pc.HasEnvironmentalIssues;
                model.EnvironmentalIssues = pc.EnvironmentalIssues;
                model.AskingSalePrice = pc.AskingSalePrice;
            }
            return model;
        }

        public PaperResidentialAssetViewModel GetPaperResidentialModel(int assetId)
        {
            var model = new PaperResidentialAssetViewModel();
            var context = _factory.Create();
            var pr = context.PaperResidentialAssets.Where(x => x.PaperResidentialAssetId == assetId).FirstOrDefault();
            if (pr != null)
            {
                var images = context.TempAssetImages.Where(x => x.GuidId == pr.GuidId).ToList();
                var docs = context.TempAssetDocuments.Where(x => x.GuidId == pr.GuidId).ToList();
                var vids = context.TempAssetVideos.Where(x => x.GuidId == pr.GuidId).ToList();
                if (images.Count > 0) model.Images.AddRange(images);
                if (docs.Count > 0) model.Documents.AddRange(docs);
                if (vids.Count > 0) model.Videos.AddRange(vids);

                model.GuidId = pr.GuidId;
                model.BeneficiaryName = pr.BeneficiaryName;
                model.BeneficiaryFullName = pr.BeneficiaryFullName;
                model.BeneficiaryContactAddress = pr.BeneficiaryContactAddress;
                model.BeneficiaryCity = pr.BeneficiaryCity;
                model.BeneficiaryState = pr.BeneficiaryState;
                model.BeneficiaryZip = pr.BeneficiaryZip;
                model.BeneficiaryPhoneHome = pr.BeneficiaryPhoneHome;
                model.BeneficiaryPhoneCell = pr.BeneficiaryPhoneCell;
                model.BeneficiaryPhoneWork = pr.BeneficiaryPhoneWork;
                model.BeneficiaryFax = pr.BeneficiaryFax;
                model.BeneficiaryEmail = pr.BeneficiaryEmail;
                model.BeneficiaryAccountNumber = pr.BeneficiaryAccountNumber;

                model.NotePayerName = pr.NotePayerName;
                model.NotePayerFullName = pr.NotePayerFullName;
                model.NotePayerContactAddress = pr.NotePayerContactAddress;
                model.NotePayerCity = pr.NotePayerCity;
                model.NotePayerState = pr.NotePayerState;
                model.NotePayerZip = pr.NotePayerZip;
                model.NotePayerPhoneHome = pr.NotePayerPhoneHome;
                model.NotePayerPhoneCell = pr.NotePayerPhoneCell;
                model.NotePayerPhoneWork = pr.NotePayerPhoneWork;
                model.NotePayerFax = pr.NotePayerFax;
                model.NotePayerEmail = pr.NotePayerEmail;
                model.NotePayerSSNOrTIN = pr.NotePayerSSNOrTIN;
                model.NotePayerFICO = pr.NotePayerFICO;

                model.SecuringPropertyAddress = pr.SecuringPropertyAddress;
                model.SecuringPropertyCity = pr.SecuringPropertyCity;
                model.SecuringPropertyState = pr.SecuringPropertyState;
                model.SecuringPropertyZip = pr.SecuringPropertyZip;
                model.TypeOfProperty = pr.TypeOfProperty;
                model.Bedrooms = pr.Bedrooms;
                model.Bathrooms = pr.Bathrooms;
                model.PropertySquareFeet = pr.PropertySquareFeet;
                model.Year = pr.Year;
                model.TypeOfContruction = pr.TypeOfContruction;
                model.LotSize = pr.LotSize;
                model.Parking = pr.Parking;
                model.WaterService = pr.WaterService;
                model.SewerService = pr.SewerService;
                model.PowerService = pr.PowerService;
                model.PropertyAccess = pr.PropertyAccess;
                model.NoteOriginationDate = pr.NoteOriginationDate;
                model.OriginalNotePrincipal = pr.OriginalNotePrincipal;
                model.BPOOfProperty = pr.BPOOfProperty;
                model.CurrentNotePrincipal = pr.CurrentNotePrincipal;
                model.HasCopyOfAppraisal = pr.HasCopyOfAppraisal;
                model.MethodOfAppraisal = pr.MethodOfAppraisal;
                model.TypeOfNote = pr.TypeOfNote;
                model.NoteInterestRate = pr.NoteInterestRate;
                model.PaymentAmount = pr.PaymentAmount;
                model.PaymentFrequency = pr.PaymentFrequency;
                model.BalloonDateOfNote = pr.BalloonDateOfNote;
                model.BalloonDueAmount = pr.BalloonDueAmount;
                model.TypeOfMTGInstrument = pr.TypeOfMTGInstrument;
                model.AmortType = pr.AmortType;
                model.PaymentsMadeOnNote = pr.PaymentsMadeOnNote;
                model.PaymentsRemainingOnNote = pr.PaymentsRemainingOnNote;
                model.IsNoteCurrent = pr.IsNoteCurrent;
                model.LastPaymentRecievedOnNote = pr.LastPaymentRecievedOnNote;
                model.NextPaymentOnNote = pr.NextPaymentOnNote;
                model.SecuringPropertyAppraisal = pr.SecuringPropertyAppraisal;
                model.PaymentHistory = pr.PaymentHistory;
                model.SellerCarryNoteSalesDate = pr.SellerCarryNoteSalesDate;
                model.SellerCarryNotePrice = pr.SellerCarryNotePrice;
                model.SellerCarryNoteCashDown = pr.SellerCarryNoteCashDown;
                model.OriginalPrincipalBalanceWRAP = pr.OriginalPrincipalBalanceWRAP;
                model.TotalMonthlyPaymentWRAP = pr.TotalMonthlyPaymentWRAP;
                model.FirstmortgageBalanceWRAP = pr.FirstmortgageBalanceWRAP;
                model.FirstInterestRateWRAP = pr.FirstInterestRateWRAP;
                model.FirstMortgagePaymentWRAP = pr.FirstMortgagePaymentWRAP;
                model.SecondMortgageBalanceWRAP = pr.SecondMortgageBalanceWRAP;
                model.SecondInterestRateWRAP = pr.SecondInterestRateWRAP;
                model.SecondMortgagePaymentWRAP = pr.SecondMortgagePaymentWRAP;
                model.NoteServicedByAgent = pr.NoteServicedByAgent;
                model.AgentName = pr.AgentName;
                model.AgentPhone = pr.AgentPhone;
                model.AgentEmail = pr.AgentEmail;
                model.AgentAccountNumber = pr.AgentAccountNumber;
                model.AgentContactPerson = pr.AgentContactPerson;
                model.AuthorizeForwardPaymentHistory = pr.AuthorizeForwardPaymentHistory;
                model.AgentAddress = pr.AgentAddress;
                model.AgentCity = pr.AgentCity;
                model.AgentState = pr.AgentState;
                model.AgentZip = pr.AgentZip;

                model.HasPicturesOfProperty = pr.HasPicturesOfProperty;
                model.GeneralComments = pr.GeneralComments;
                model.SellingReason = pr.SellingReason;
                model.HasTitleInsurance = pr.HasTitleInsurance;
                model.HomeownerInsurance = pr.HomeownerInsurance;
                model.OwnerOccupied = pr.OwnerOccupied;
                model.IsPropertyRental = pr.IsPropertyRental;
                model.MonthlyRentRate = pr.MonthlyRentRate;
                model.HasEnvironmentalIssues = pr.HasEnvironmentalIssues;
                model.EnvironmentalIssues = pr.EnvironmentalIssues;
                model.AskingPrice = pr.AskingPrice;
            }
            return model;
        }

        public RealEstateCommercialAssetViewModel GetRealEstateCommercialModel(int assetId)
        {
            var model = new RealEstateCommercialAssetViewModel();
            var context = _factory.Create();
            var rec = context.RealEstateCommercialAssets.Where(x => x.RealEstateCommercialAssetId == assetId).FirstOrDefault();
            if (rec != null)
            {
                var images = context.TempAssetImages.Where(x => x.GuidId == rec.GuidId).ToList();
                var docs = context.TempAssetDocuments.Where(x => x.GuidId == rec.GuidId).ToList();
                var vids = context.TempAssetVideos.Where(x => x.GuidId == rec.GuidId).ToList();
                //if (images.Count > 0) model.Images.AddRange(images);
                if (docs.Count > 0) model.Documents.AddRange(docs);
                if (vids.Count > 0) model.Videos.AddRange(vids);

                model.GuidId = rec.GuidId;
                model.NameOfPrincipal = rec.NameOfPrincipal;
                model.NameOfCoPrincipal = rec.NameOfCoPrincipal;
                //model.HomePhone = rec.HomePhone;
                //model.WorkPhone = rec.WorkPhone;
                //model.OtherPhone = rec.OtherPhone;
                model.Fax = rec.Fax;
                model.EmailAddress = rec.EmailAddress;

                model.TypeOfProperty = rec.TypeOfProperty;
                model.PropertyAddress = rec.PropertyAddress;
                model.PropertyCity = rec.PropertyCity;
                model.PropertyState = rec.PropertyState;
                model.PropertyZip = rec.PropertyZip;

                model.FirstMortgageCompany = rec.FirstMortgageCompany;
                model.MortgageCompanyAddress = rec.MortgageCompanyAddress;
                model.MortgageCompanyCity = rec.MortgageCompanyCity;
                model.MortgageCompanyState = rec.MortgageCompanyState;
                model.MortgageCompanyZip = rec.MortgageCompanyZip;
                model.LenderPhone = rec.LenderPhone;
                model.LenderPhoneOther = rec.LenderPhoneOther;
                model.AccountNumber = rec.AccountNumber;
                //model.TypeOfMortgage = rec.TypeOfMortgage;
                model.CurrentPrincipalBalance = rec.CurrentPrincipalBalance;
                model.MonthlyPayment = rec.MonthlyPayment;
                model.PaymentIncludes = rec.PaymentIncludes;
                model.InterestRate = rec.InterestRate;
                model.IsMortgageAnARM = rec.IsMortgageAnARM;
                model.MortgageAdjustIfARM = rec.MortgageAdjustIfARM;
                model.NumberOfMissedPayments = rec.NumberOfMissedPayments;
                model.RentableSquareFeet = rec.RentableSquareFeet;
                model.MajorTenant = rec.MajorTenant;
                model.CurrentVacancyFactor = rec.CurrentVacancyFactor;
                model.CurrentAnnualIncome = rec.CurrentAnnualIncome;
                model.CurrentAnnualOperatingExepenses = rec.CurrentAnnualOperatingExepenses;
                //model.CurrentOperatingVacancyFactor = rec.CurrentOperatingVacancyFactor;
                model.CurrentCalendarYearToDateCashFlow = rec.CurrentCalendarYearToDateCashFlow;
                model.RecentUpgradesRenovations = rec.RecentUpgradesRenovations;
                model.AdditionalInformation = rec.AdditionalInformation;
                model.NeededMaintenance = rec.NeededMaintenance;
                model.AskingSalePrice = rec.AskingSalePrice;
                model.Terms = rec.Terms;
                model.MotivationToLiquidate = rec.MotivationToLiquidate;
            }
            return model;
        }

        public RealEstateResidentialAssetViewModel GetRealEstateResidentialModel(int assetId)
        {
            var model = new RealEstateResidentialAssetViewModel();
            var context = _factory.Create();
            var rer = context.RealEstateResidentialAssets.Where(x => x.RealEstateResidentialAssetId == assetId).FirstOrDefault();
            if (rer != null)
            {
                var images = context.TempAssetImages.Where(x => x.GuidId == rer.GuidId).ToList();
                var docs = context.TempAssetDocuments.Where(x => x.GuidId == rer.GuidId).ToList();
                var vids = context.TempAssetVideos.Where(x => x.GuidId == rer.GuidId).ToList();
                if (images.Count > 0) model.Images.AddRange(images);
                if (docs.Count > 0) model.Documents.AddRange(docs);
                if (vids.Count > 0) model.Videos.AddRange(vids);

                model.GuidId = rer.GuidId;
                model.NameOfPrincipal = rer.NameOfPrincipal;
                model.NameOfCoPrincipal = rer.NameOfCoPrincipal;
                //model.HomePhone = rer.HomePhone;
                model.WorkPhone = rer.WorkPhone;
                //model.OtherPhone = rer.OtherPhone;
                model.Fax = rer.Fax;
                model.EmailAddress = rer.EmailAddress;

                model.TypeOfProperty = rer.TypeOfProperty;
                model.PropertyAddress = rer.PropertyAddress;
                model.PropertyCity = rer.PropertyCity;
                model.PropertyState = rer.PropertyState;
                model.PropertyZip = rer.PropertyZip;
                model.MajorCrossStreets = rer.MajorCrossStreets;
                model.PropertyInsuranceCarrier = rer.PropertyInsuranceCarrier;
                model.PolicyNumber = rer.PolicyNumber;
                model.InsuranceAgentName = rer.InsuranceAgentName;
                model.InsuranceAgentPhone = rer.InsuranceAgentPhone;
                model.PropertyInHOA = rer.PropertyInHOA;
                model.HOADues = rer.HOADues;
                model.HOADueTime = rer.HOADueTime;
                model.HOALiensOnProperty = rer.HOALiensOnProperty;
                model.HOALiens = rer.HOALiens;
                model.Language = rer.Language;
                model.DateOfBirth = rer.DateOfBirth;
                model.SSN = rer.SSN;
                model.PropertyListedForSale = rer.PropertyListedForSale;
                model.AgentName = rer.AgentName;
                model.AgentNumber = rer.AgentNumber;

                model.FirstMortgageCompany = rer.FirstMortgageCompany;
                model.FirstMCAddress = rer.FirstMCAddress;
                model.FirstMCCity = rer.FirstMCCity;
                model.FirstMCState = rer.FirstMCState;
                model.FirstMCZip = rer.FirstMCZip;
                model.FirstMCLenderPhone = rer.FirstMCLenderPhone;
                model.FirstMCLenderPhoneOther = rer.FirstMCLenderPhoneOther;
                model.FirstMCAccountNumber = rer.FirstMCAccountNumber;
                //model.FirstMCTypeOfMortgage = rer.FirstMCTypeOfMortgage;
                //model.FirstMCCurrentPrincipalBalance  = rer.FirstMCCurrentPrincipalBalance;
                //model.FirstMCInterestRate  = rer.FirstMCInterestRate;
                model.FirstMCIsMortgageAnARM = rer.FirstMCIsMortgageAnARM;
                //model.FirstMCMortgageAdjustIfARM  = rer.FirstMCMortgageAdjustIfARM;
                model.FirstMCMortgageHasPPP = rer.FirstMCMortgageHasPPP;
                model.FirstMCPPPExpireDate = rer.FirstMCPPPExpireDate;
                model.FirstMCMonthlyPayment = rer.FirstMCMonthlyPayment;
                model.FirstMCPaymentIncludes = rer.FirstMCPaymentIncludes;
                model.FirstMCLastPaymentDate = rer.FirstMCLastPaymentDate;
                model.FirstMCWasLastPaymentForThatMonth = rer.FirstMCWasLastPaymentForThatMonth;
                model.FirstMCNumberOfMissedPayments = rer.FirstMCNumberOfMissedPayments;
                model.FirstMCCurrentMortgageBalance = rer.FirstMCCurrentMortgageBalance;
                model.FirstMCHasForclosureStarted = rer.FirstMCHasForclosureStarted;
                model.FirstMCForclosureSaleDate = rer.FirstMCForclosureSaleDate;

                model.SecondMortgageCompany = rer.SecondMortgageCompany;
                model.SecondMCAddress = rer.SecondMCAddress;
                model.SecondMCCity = rer.SecondMCCity;
                model.SecondMCState = rer.SecondMCState;
                model.SecondMCZip = rer.SecondMCZip;
                model.SecondMCLenderPhone = rer.SecondMCLenderPhone;
                model.SecondMCLenderPhoneOther = rer.SecondMCLenderPhoneOther;
                model.SecondMCAccountNumber = rer.SecondMCAccountNumber;
                //model.SecondMCTypeOfMortgage = rer.SecondMCTypeOfMortgage;
                //model.SecondMCCurrentPrincipalBalance  = rer.SecondMCCurrentPrincipalBalance;
                //model.SecondMCInterestRate  = rer.SecondMCInterestRate;
                model.SecondMCIsMortgageAnARM = rer.SecondMCIsMortgageAnARM;
                //model.SecondMCMortgageAdjustIfARM  = rer.SecondMCMortgageAdjustIfARM;
                model.SecondMCMortgageHasPPP = rer.SecondMCMortgageHasPPP;
                model.SecondMCPPPExpireDate = rer.SecondMCPPPExpireDate;
                model.SecondMCMonthlyPayment = rer.SecondMCMonthlyPayment;
                model.SecondMCPaymentIncludes = rer.SecondMCPaymentIncludes;
                model.SecondMCLastPaymentDate = rer.SecondMCLastPaymentDate;
                model.SecondMCWasLastPaymentForThatMonth = rer.SecondMCWasLastPaymentForThatMonth;
                model.SecondMCNumberOfMissedPayments = rer.SecondMCNumberOfMissedPayments;
                model.SecondMCCurrentMortgageBalance = rer.SecondMCCurrentMortgageBalance;
                model.SecondMCHasForclosureStarted = rer.SecondMCHasForclosureStarted;
                model.SecondMCForclosureSaleDate = rer.SecondMCForclosureSaleDate;

                model.PaymentsFallBehindReason = rer.PaymentsFallBehindReason;
                model.FormOfResidence = rer.FormOfResidence;
                model.NumberOfYears = rer.NumberOfYears;
                model.MaritalStatus = rer.MaritalStatus;
                model.Dependants = rer.Dependants;
                model.Bedrooms = rer.Bedrooms;
                model.Bathrooms = rer.Bathrooms;
                model.Garage = rer.Garage;
                model.Carport = rer.Carport;
                model.Pool = rer.Pool;
                model.Spa = rer.Spa;
                model.BuildingStyle = rer.BuildingStyle;
                model.Architecture = rer.Architecture;
                model.HOA = rer.HOA;
                model.RecentUpgradeOrRemodel = rer.RecentUpgradeOrRemodel;
                model.DefferedMaintenance = rer.DefferedMaintenance;
            }
            return model;
        }

        public void AddImageToAsset(byte[] image, Guid assetId)
        {
            var context = _factory.Create();
            var path = Path.Combine(ConfigurationManager.AppSettings["DataRoot"], "Images", assetId.ToString());
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var filename = Guid.NewGuid() + ".jpg";
            path = Path.Combine(path, filename);
            File.WriteAllBytes(path, image);
            context.AssetImages.Add(new AssetImage()
            {
                AssetId = assetId,
                ContentType = "image/jpeg",
                FileName = filename,
                IsFlyerImage = false,
                IsMainImage = false,
                Order = 0,
                AssetImageId = Guid.NewGuid()
            });
            context.Save();
        }

        public void AddImageToAsset(byte[] image, bool isFlyer, bool isMain, Guid assetId)
        {

        }

        public void AddImageToAsset(string image, bool isFlyer, bool isMain, Guid assetId, int order)
        {
            var context = _factory.Create();
            var path = Path.Combine(ConfigurationManager.AppSettings["DataRoot"], "Images", assetId.ToString());
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var filename = Guid.NewGuid() + ".jpg";
            path = Path.Combine(path, filename);
            //File.WriteAllBytes(path, image);
            try
            {
                // moving the file would be ideal
                File.Move(image, path);
            }
            catch
            {
                File.Copy(image, path);
            }
            if (isMain)
            {
                var mainImage = context.AssetImages.Where(x => x.AssetId == assetId && x.IsMainImage).FirstOrDefault();
                if (mainImage != null)
                {
                    mainImage.IsMainImage = false;
                    context.Save();
                }
            }
            context.AssetImages.Add(new AssetImage()
            {
                AssetId = assetId,
                ContentType = "image/jpeg",
                FileName = filename,
                IsFlyerImage = isFlyer,
                IsMainImage = isMain,
                Order = order,
                AssetImageId = Guid.NewGuid()
            });
            context.Save();
        }

        public bool IsAssetLocked(Guid assetId, int userId)
        {
            // test the minutes, the appsettings
            var context = _factory.Create();
            bool isLocked = false;
            var assetLock = context.AssetLocks.Where(x => x.AssetId == assetId).OrderByDescending(x => x.AssetLockId).FirstOrDefault();
            if (assetLock != null)
            {
                // in minutes
                int time = 60;
                if (ConfigurationManager.AppSettings.Get("AssetLockTime") != null)
                {
                    int.TryParse(ConfigurationManager.AppSettings.Get("AssetLockTime"), out time);
                }
                //first check if current user locked it
                if (assetLock.UserId == userId)
                {
                    // update time
                    assetLock.CreationTime = DateTime.Now;
                    context.Save();
                    return false;
                }
                // check timeout
                var minutes = (DateTime.Now - assetLock.CreationTime).TotalMinutes;
                if (minutes > time)
                {
                    context.AssetLocks.Remove(assetLock);
                    context.Save();
                }
                else
                {
                    isLocked = true;
                }
            }
            return isLocked;
        }

        public void LockAsset(int userId, Guid assetId)
        {
            // only one user can lock one asset at a time
            var context = _factory.Create();

            var currentUserLocks = context.AssetLocks.Where(x => x.UserId == userId).ToList();
            if (currentUserLocks.Count > 0)
            {
                foreach (var assetLock in currentUserLocks)
                {
                    context.AssetLocks.Remove(assetLock);
                }
                //context.Save();
            }

            var existingLock = context.AssetLocks.Where(x => x.AssetId == assetId).FirstOrDefault();
            if (existingLock == null)
            {
                var assetLock = new AssetLock();
                assetLock.AssetId = assetId;
                assetLock.CreationTime = DateTime.Now;
                assetLock.UserId = userId;
                context.AssetLocks.Add(assetLock);
            }
            context.Save();
        }

        public int GetAssetLockUserId(Guid assetId)
        {
            var context = _factory.Create();
            return context.AssetLocks.Where(x => x.AssetId == assetId).SingleOrDefault().UserId;
        }

        public void UnlockAsset(Guid assetId)
        {
            var context = _factory.Create();
            var assetLocks = context.AssetLocks.Where(x => x.AssetId == assetId).ToList();
            if (assetLocks.Count > 0)
            {
                foreach (var assetLock in assetLocks)
                {
                    context.AssetLocks.Remove(assetLock);
                }
                context.Save();
            }
        }

        public List<SelectListItem> GetEscrowCompanies()
        {
            var context = _factory.Create();
            var companies = new List<SelectListItem>();
            context.EscrowCompanies.ToList().ForEach(e =>
                {
                    companies.Add(new SelectListItem() { Text = e.EscrowCompanyName, Value = e.EscrowCompanyName });
                });
            return companies;
        }

        public void SaveLOI(BindingContingentTemplateModel model, int userId)
        {
            var context = _factory.Create();
            // disable older loi's requested by this user for this asset
            var oldLois = context.LOIs.Where(x => x.AssetId == model.AssetId && x.UserId == userId && x.Active).ToList();
            var existingLoi = context.LOIs.Where(x => x.LOIId == model.LOIId).ToList();
            foreach (var inactiveLoi in oldLois)
            {
                inactiveLoi.Active = false;
                context.Save();
            }

            var loi = new LOI();
            loi.ReadByListedByUser = false;
            loi.LOIId = model.LOIId;
            loi.UserId = userId;
            loi.Active = true;
            loi.AssetId = model.AssetId;
            loi.To = model.To;
            loi.From = model.From;
            loi.EmailAddress = model.EmailAddress;
            loi.Date = model.Date.ToString("MM/dd/yyyy");
            loi.FaxNumber = model.FaxNumber;
            loi.CareOf = model.CareOf;
            loi.Company = model.Company;
            loi.TotalNumberOfPagesIncludingCover = model.TotalNumberOfPagesIncludingCover;
            loi.WorkPhoneNumber = model.WorkPhoneNumber;
            loi.BusinessPhoneNumber = model.BusinessPhoneNumber;
            loi.CellPhoneNumber = model.CellPhoneNumber;
            loi.CREAquisitionLOI = model.CREAquisitionLOI;
            loi.EmailAddress2 = model.EmailAddress2;
            loi.BeneficiarySeller = model.BeneficiarySeller;
            loi.OfficePhone = model.OfficePhone;
            loi.OfficerOfSeller = model.OfficerOfSeller;
            loi.WebsiteEmail = model.WebsiteEmail;
            loi.Buyer1Name = model.Buyer1Name;
            loi.BuyerAssigneeName = model.BuyerAssigneeName;
            loi.ObjectOfPurchase = model.ObjectOfPurchase;
            loi.LegalDescription = model.LegalDescription;
            loi.AssessorNumber = model.AssessorNumber;
            loi.SecuredMortgages = model.SecuredMortgages;
            loi.Lender = model.Lender;
            loi.NoSecuredMortgages = model.NoSecuredMortgages;
            loi.OfferingPurchasePrice = model.OfferingPurchasePrice;
            loi.InitialEarnestDeposit = model.InitialEarnestDeposit;
            loi.BalanceEarnestDeposit = model.BalanceEarnestDeposit;
            loi.TermsOfPurchase = model.TermsOfPurchase;
            loi.Releasing = model.Releasing;
            loi.Terms1 = model.Terms1;
            loi.Terms2 = model.Terms2;
            loi.Terms3 = model.Terms3;
            loi.DueDiligenceDate = model.DueDiligenceDate;
            loi.DueDiligenceNumberOfDays = model.DueDiligenceNumberOfDays;
            loi.SellerDisclosureDate = model.SellerDisclosureDate;
            loi.SellerDisclosureNumberOfDays = model.SellerDisclosureNumberOfDays;
            loi.OperatingDisclosureDate = model.OperatingDisclosureDate;
            loi.OperatingDisclosureNumberOfDays = model.OperatingDisclosureNumberOfDays;
            loi.ClosingDate = model.ClosingDate;
            loi.ClosingDateNumberOfDays = model.ClosingDateNumberOfDays;

            loi.FormalDocumentationDate = model.FormalDocumentationDate;
            loi.FormalDocumentationNumberOfDays = model.FormalDocumentationNumberOfDays;

            loi.CommissionFeesName = model.CommissionFeesName;
            loi.CommissionFeesNumber = model.CommissionFeesNumber;
            loi.EscrowCompanyName = model.EscrowCompanyName;
            loi.EscrowCompanyAddress = model.EscrowCompanyAddress;
            loi.EscrowCompanyAddress2 = model.EscrowCompanyAddress2;
            loi.EscrowCompanyCity = model.EscrowCompanyCity;
            loi.EscrowCompanyState = model.EscrowCompanyState;
            loi.EscrowCompanyZip = model.EscrowCompanyZip;
            loi.EscrowCompanyPhoneNumber = model.EscrowCompanyPhoneNumber;
            loi.StateOfCountyAssessors = model.StateOfCountyAssessors;
            loi.StateOfPropertyTaxOffice = model.StateOfPropertyTaxOffice;
            loi.LOIDate = model.LOIDate;
            loi.Buyer1 = model.Buyer1;
            loi.BuyerTitle1 = model.BuyerTitle1;
            //loi.Buyer2 = 
            //loi.BuyerTitle2 = 
            loi.SellerReceiver1 = model.SellerReceiver1;
            //loi.SellerReceiver1Officer = 
            //loi.SellerReceiver1Title = 
            loi.SellerReceiver2 = model.SellerReceiver2;
            //loi.SellerReceiver2Officer = 
            //loi.SellerReceiver2Title = 
            //loi.BuyersAssignee1 = 
            //loi.BuyersAssignee1Officer = 
            //loi.BuyersAssignee1Title = 
            //loi.BuyersAssignee2 = 
            //loi.BuyersAssignee2Officer = 
            //loi.BuyersAssignee2Title = 

            // user can click the back button and hit submit again
            if (existingLoi.Count == 0)
                context.LOIs.Add(loi);
            context.Save();

            foreach (var doc in model.Documents)
            {
                var newDoc = new LOIDocument();
                newDoc.LOIDocumentId = Guid.NewGuid();
                newDoc.LOIId = model.LOIId;
                newDoc.ContentType = doc.ContentType;
                newDoc.Description = doc.Description;
                newDoc.FileName = doc.FileName;
                newDoc.Order = doc.Order;
                newDoc.Size = doc.Size;
                newDoc.Title = doc.Title;
                newDoc.Type = doc.Type;
                context.LOIDocuments.Add(newDoc);
                context.Save();
            }
        }

        public Tuple<string, string> GetUserInformationForHeldAsset(Guid assetId)
        {
            var context = _factory.Create();
            var asset = context.Assets.FirstOrDefault(s => s.AssetId == assetId && s.IsActive);
            if (asset != null)
            {
                if (asset.HoldForUserId.HasValue && (!asset.HoldEndDate.HasValue || (asset.HoldEndDate.HasValue && asset.HoldEndDate.Value > DateTime.Today)))
                {
                    var user = context.Users.FirstOrDefault(f => f.UserId == asset.HoldForUserId.Value);
                    if (user != null)
                    {
                        return new Tuple<string, string>(user.FullName, user.Username);
                    }
                    return null;
                }
            }
            return null;
        }

        public void DeleteStrandedVideosAndConvertMp4s()
        {
            // testing code here for AssetVideo service
            var context = _factory.Create();
            List<string> activeVideos = context.AssetVideos.Select(x => x.FilePath.ToLower()).ToList();
            string videoDirectory = Path.Combine(ConfigurationManager.AppSettings["DataRoot"], "Videos");
            foreach (string folder in Directory.GetDirectories(videoDirectory))
            {
                foreach (string file in Directory.GetFiles(folder))
                {
                    string webmToMp4 = Path.GetFileName(Path.ChangeExtension(file, ".mp4")).ToLower();
                    if (!activeVideos.Any(x => x == webmToMp4))
                    {
                        // Try to delete video
                        try
                        {
                            File.Delete(file);
                        }
                        catch { }
                    }
                }
            }

            KillProcesses();

            // add a converted webm from remaining mp4s
            Process[] ffmpegs = Process.GetProcessesByName("ffmpeg");
            if (!ffmpegs.Any())
            {
                foreach (var folder in Directory.GetDirectories(videoDirectory))
                {
                    var files = Directory.GetFiles(folder);
                    // check every mp4 and make sure it has a webm
                    foreach (var file in files)
                    {
                        if (file.ToLower().Contains(".mp4"))
                        {
                            string webm = Path.ChangeExtension(file, "webm");
                            if (!files.Contains(webm))
                            {
                                // create webm from mp4
                                string param = "-i " + file + " " + webm;
                                ProcessStartInfo startInfo = new ProcessStartInfo();
                                Process f = new Process();
                                startInfo.UseShellExecute = false;
                                startInfo.CreateNoWindow = true;
                                //startInfo.RedirectStandardOutput = false;
                                startInfo.FileName = ConfigurationManager.AppSettings["ffmpeg"];
                                startInfo.Arguments = param;
                                using (Process p = Process.Start(startInfo))
                                {
                                    p.WaitForExit();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void KillProcesses()
        {
            Process[] ffmpegs = Process.GetProcessesByName("ffmpeg");
            if (ffmpegs.Any())
            {
                foreach (var ffmpeg in ffmpegs)
                {
                    try
                    {
                        ffmpeg.Kill();
                    }
                    catch (Exception ex)
                    {
                        //logServiceEvent("Error stopping ffmpeg process. Error: " + ex.Message, EventLogEntryType.Error);
                    }
                }
            }
        }

        public AssetDocumentOrderRequestModel RequestDocuments(Guid assetId, int userId, int titleCompanyId, bool finalized)
        {
            // this method doesnt need to be in the AssetManager, now that I've built it
            // because i want to keep postal out of business layer since its not here yet,
            // do work then send back whatever
            var model = new AssetDocumentOrderRequestModel();
            model.Valid = false;
            var context = _factory.Create();

            try
            {
                var asset = context.Assets.Where(x => x.AssetId == assetId).FirstOrDefault();
                if (asset != null)
                {
                    model = GetAssetDocumentOrderRequest(assetId, titleCompanyId, userId, finalized);
                    if (!string.IsNullOrEmpty(model.CompanyName))
                    {
                        // TODO: confirm with liz and aliea that we have to get the manager this way
                        if (!string.IsNullOrEmpty(model.Manager.Email))
                        {
                            model.Valid = true;
                            asset.OrderStatus = OrderStatus.Pending;
                            asset.TitleCompanyId = titleCompanyId;
                            //asset.OrderedByUserId = userId;
                            //asset.OrderDate = DateTime.Now;
                            //asset.TitleCompanyUserId = titleCompanyId;
                            // delete the orderedbyuserid and orderdate on the asset and use the doc order table // edit: do not do that lol
                            asset.OrderedByUserId = userId;
                            asset.OrderDate = DateTime.Now;
                            model.State = asset.State;
                            //var oldHistory = context.AssetOrderHistories.Where(x => x.UserId == userId && x.AssetId == assetId).FirstOrDefault();
                            //var user = context.Users.Where(x => x.UserId == userId).FirstOrDefault();
                            //if (oldHistory == null)
                            //{
                            //    AssetOrderHistory history = new AssetOrderHistory();
                            //    history.AssetId = assetId;
                            //    history.UserId = userId;
                            //    history.Date = DateTime.Now;
                            //    history.UserEmail = user != null ? user.Username : string.Empty;
                            //    context.AssetOrderHistories.Add(history);
                            //    context.Save();
                            //}
                            context.Save();
                        }
                        else
                        {
                            model.Message = "Title company is missing its manager.";
                        }
                    }
                    else
                    {
                        // we dont care about this at the moment
                    }
                }
            }
            catch (Exception ex)
            {
                model.Message = "There was an error processing your request. " + ex.Message;
            }
            return model;
        }

        public bool UpdateAssetDocuments(List<AssetDocument> docs, Guid assetId)
        {
            bool success = true;
            try
            {
                var context = _factory.Create();
                var assetDocs = context.AssetDocuments.Where(x => x.AssetId == assetId).ToList();
                // delete all asset documents
                if (assetDocs.Count > 0)
                {
                    foreach (var doc in assetDocs)
                    {
                        context.AssetDocuments.Remove(doc);
                    }
                }
                // add all asset documents from list
                if (docs.Count > 0)
                {
                    foreach (var doc in docs)
                    {
                        context.AssetDocuments.Add(doc);
                    }
                }
                context.Save();
            }
            catch (Exception ex)
            {
                success = false;
                // TODO: LOG Error
            }
            return success;
        }

        public bool AddUserToOrderHistory(Guid assetId, string email, AssetOrderHistoryType type)
        {
            var context = _factory.Create();
            bool success = true;
            var user = context.Users.Where(x => x.Username == email).FirstOrDefault();
            var asset = context.Assets.Where(x => x.AssetId == assetId).FirstOrDefault();
            if (user != null && asset != null)
            {
                var oldHistory = context.AssetOrderHistories.Where(x => x.UserId == user.UserId && x.AssetId == assetId).FirstOrDefault();

                // there should be an orderedbyuserid on the asset if we are in this method
                if (asset.OrderedByUserId.GetValueOrDefault(0) > 0)
                {
                    var userThatOrderedTheDocumentsFirst = context.Users.Where(x => x.UserId == (int)asset.OrderedByUserId).FirstOrDefault();
                    if (userThatOrderedTheDocumentsFirst != null)
                    {
                        if (userThatOrderedTheDocumentsFirst.UserId != user.UserId)
                        {
                            if (oldHistory == null)
                            {
                                AssetOrderHistory history = new AssetOrderHistory();
                                history.AssetId = assetId;
                                history.UserId = user.UserId;
                                history.Date = DateTime.Now;
                                history.UserEmail = email;
                                history.HistoryType = type;
                                context.AssetOrderHistories.Add(history);
                                context.Save();
                            }
                        }
                    }
                }
            }
            return success;
        }

        public AssetDocumentOrderRequestModel GetAssetDocumentOrderRequest(Guid assetId, int titleCompanyId, int userId, bool finalized)
        {
            var context = _factory.Create();
            var model = new AssetDocumentOrderRequestModel();

            try
            {
                var user = new User();
                var asset = context.Assets.Where(x => x.AssetId == assetId).FirstOrDefault();
                if (finalized)
                {
                    user = context.Users.Where(x => x.UserId == (int)asset.OrderedByUserId).FirstOrDefault();
                    if (user != null)
                    {
                        model.OrderedByEmail = user.Username;
                        model.OrderedByName = user.FullName;
                    }
                }
                else
                    user = context.Users.Where(x => x.UserId == userId).FirstOrDefault();

                if (asset != null)
                {
                    if (asset.AssetSellerId.GetValueOrDefault(0) > 0 && asset.ListedByUserId.GetValueOrDefault(0) == 0)
                    {
                        // anonymous user submitted asset

                        // get seller from AssetSeller table
                        var seller = context.AssetSellers.Where(x => x.AssetSellerId == asset.AssetSellerId).FirstOrDefault();
                        if (seller != null)
                        {
                            // see if this seller is the same as the user that clicked the button
                            if (seller.Email == user.Username)
                            {
                                model.IsSellerAsset = true;
                                model.SellerFullName = user.FullName;
                                model.SellerEmail = user.Username;
                            }
                        }
                    }
                    // 5/28 we never implemented NAR so dont worry about this
                    //else if (asset.AssetSellerId.GetValueOrDefault(0) == 0 && asset.ListedByUserId.GetValueOrDefault(0) > 0)
                    //{
                    //    // NAR member submitted asset
                    //    model.IsNARAsset = true;
                    //    var seller = context.Users.Where(x => x.UserId == asset.ListedByUserId).FirstOrDefault();
                    //    if (seller != null)
                    //    {
                    //        model.NARMemberEmail = seller.Username;
                    //        model.NARMemberFullName = seller.FullName;
                    //    }
                    //}
                    else if (asset.AssetSellerId.GetValueOrDefault(0) > 0 && asset.ListedByUserId.GetValueOrDefault(0) > 0)
                    {
                        // We assume logged in user submitted asset through seller pages

                        if ((int)asset.ListedByUserId.GetValueOrDefault(0) == user.UserId)
                        {
                            model.IsSellerAsset = true;
                            model.SellerEmail = user.Username;
                            model.SellerFullName = user.FullName;
                        }
                    }
                    else
                    {
                        // still set these values anyways
                        model.SellerEmail = user.Username;
                        model.SellerFullName = user.FullName;
                    }

                    var titleCompany = context.TitleCompanies.Where(x => x.TitleCompanyId == titleCompanyId && x.IsActive).FirstOrDefault();
                    if (titleCompany != null)
                    {
                        var titleCompanyManager = context.TitleCompanyUsers.Where(x => x.IsManager && x.IsActive && x.TitleCompanyId == titleCompany.TitleCompanyId).FirstOrDefault();
                        model.CompanyName = titleCompany.TitleCompName;
                        model.CompanyAddress = titleCompany.TitleCompAddress;
                        model.CompanyPhone = titleCompany.TitleCompPhone;
                        if (titleCompanyManager != null)
                        {
                            model.Manager = new TitleUserQuickViewModel()
                            {
                                Email = titleCompanyManager.Email,
                                TitleCompanyUserId = titleCompanyManager.TitleCompanyUserId
                            };
                        }
                    }

                    model.History = GetAssetOrderHistory(asset, user, context);

                    if (asset.AssetType == AssetType.MultiFamily || asset.AssetType == AssetType.MHP)
                    {
                        var converted = asset as MultiFamilyAsset;
                        model.AssetDescription = string.Format("{0} unit {1}", converted.TotalUnits, EnumHelper.GetEnumDescription(asset.AssetType));
                    }
                    else
                    {
                        var converted = asset as CommercialAsset;
                        model.AssetDescription = string.Format("{0} square foot {1}", converted.SquareFeet, EnumHelper.GetEnumDescription(asset.AssetType));
                    }
                    model.AssetId = asset.AssetNumber.ToString();
                    model.Address1 = asset.PropertyAddress;
                    model.State = asset.State;
                    model.City = asset.City;
                    model.Zip = asset.Zip;
                    model.Ownership = asset.Owner;

                    // 5/22 TODO: Test
                    //var listingAgent = context.AssetListingAgents.Where(x => x.AssetId == asset.AssetId).FirstOrDefault();
                    //if (listingAgent != null)
                    //{
                    //    model.ListingAgent = listingAgent.ListingAgentName;
                    //}

                    var apns = context.AssetTaxParcelNumbers.Where(x => x.AssetId == asset.AssetId).ToList();
                    if (apns != null && apns.Count > 0)
                        model.APN = string.Join(",", apns.Select(x => x.TaxParcelNumber).ToArray());
                }
            }
            catch { }

            return model;
        }

        public int GetCurrentActiveAssetCount()
        {
            var context = _factory.Create();
            return context.Assets.Count(w => w.IsActive);
        }

        public List<AssetOrderHistoryViewModel> GetAssetOrderHistory(Asset asset, User user, IEPIRepository context)
        {
            var model = new List<AssetOrderHistoryViewModel>();
            //var context = _factory.Create();
            //var asset = context.Assets.Where(x => x.AssetId == assetId).FirstOrDefault();

            if (asset != null)
            {
                var history = context.AssetOrderHistories.Where(x => x.AssetId == asset.AssetId && x.HistoryType == AssetOrderHistoryType.Title).ToList();
                foreach (var h in history)
                {
                    //var user = context.Users.Where(x => x.UserId == h.UserId).FirstOrDefault();
                    if (user != null)
                    {
                        if (asset.AssetSellerId.GetValueOrDefault(0) > 0 && asset.ListedByUserId.GetValueOrDefault(0) == 0)
                        {
                            // anonymous user submitted asset

                            // get seller from AssetSeller table
                            var seller = context.AssetSellers.Where(x => x.AssetSellerId == asset.AssetSellerId).FirstOrDefault();
                            if (seller != null)
                            {
                                // see if this seller is the same as the user that clicked the button
                                if (seller.Email == user.Username)
                                {
                                    model.Add(new AssetOrderHistoryViewModel()
                                    {
                                        AssetOrderHistoryId = h.AssetOrderHistoryId,
                                        AssetId = asset.AssetId,
                                        Date = h.Date,
                                        UserId = h.UserId,
                                        UserEmail = h.UserEmail,
                                        IsSeller = true
                                    });
                                }
                            }
                        }
                        else if (asset.AssetSellerId.GetValueOrDefault(0) > 0 && asset.ListedByUserId.GetValueOrDefault(0) > 0)
                        {
                            // We assume logged in user submitted asset through seller pages

                            if ((int)asset.ListedByUserId.GetValueOrDefault(0) == user.UserId)
                            {
                                model.Add(new AssetOrderHistoryViewModel()
                                {
                                    AssetOrderHistoryId = h.AssetOrderHistoryId,
                                    AssetId = asset.AssetId,
                                    Date = h.Date,
                                    UserId = h.UserId,
                                    UserEmail = h.UserEmail,
                                    IsSeller = true
                                });
                            }
                        }
                        else
                        {
                            // user is not a seller
                            model.Add(new AssetOrderHistoryViewModel()
                            {
                                AssetOrderHistoryId = h.AssetOrderHistoryId,
                                AssetId = asset.AssetId,
                                Date = h.Date,
                                UserId = h.UserId,
                                UserEmail = h.UserEmail,
                                IsSeller = false
                            });
                        }
                    }
                }
            }
            return model;
        }

        public void MarkDocumentViewingStatus(AssetDocument doc, bool status)
        {
            switch (doc.Type)
            {
                case (int)AssetDocumentType.MortgageInstrumentOfRecord:
                case (int)AssetDocumentType.RecordedLiens:
                case (int)AssetDocumentType.TaxLiens:
                case (int)AssetDocumentType.BKRelated:
                case (int)AssetDocumentType.PreliminaryTitleReportTitle:
                case (int)AssetDocumentType.DOTMTG:
                case (int)AssetDocumentType.OtherTitle:
                case (int)AssetDocumentType.Insurance:
                    doc.Viewable = status;
                    break;
            }
        }

        public List<OrderModel> GetTitleCompanyOrders(OrderSearchModel model)
        {
            var context = _factory.Create();
            List<OrderModel> orders = (from a in context.Assets
                                       join apn in context.AssetTaxParcelNumbers on a.AssetId equals apn.AssetId into g
                                       join t in context.TitleCompanies on a.TitleCompanyId equals t.TitleCompanyId
                                       join u in context.TitleCompanyUsers on a.TitleCompanyUserId equals u.TitleCompanyUserId into uGroup
                                       from uItem in uGroup.DefaultIfEmpty()
                                       join o in context.TitleOrderPayments on a.AssetId equals o.AssetId into oGroup
                                       from item in oGroup.DefaultIfEmpty()
                                       where a.OrderStatus != OrderStatus.Unknown &&
                                       a.TitleCompanyId == model.TitleCompanyId &&
                                       a.IsActive
                                       select new OrderModel()
                                       {
                                           AssetNumber = a.AssetNumber,
                                           AssetId = a.AssetId,
                                           Type = a.AssetType,
                                           AssetName = a.ProjectName ?? "",
                                           County = a.County ?? "",
                                           City = a.City ?? "",
                                           State = a.State,
                                           Apns = g.Select(x => x.TaxParcelNumber),
                                           DateOfOrder = a.OrderDate,
                                           OrderStatus = a.OrderStatus,
                                           DateOfSubmit = a.DateOfOrderSubmit,
                                           Due = item == null ? t.CurrentRate : item.PaidAmount,
                                           DatePaid = item == null ? null : item.PaymentReceivedDate,
                                           TitleCompanyId = model.TitleCompanyId,
                                           FirstName = uItem != null ? uItem.FirstName : string.Empty,
                                           LastName = uItem != null ? uItem.LastName : string.Empty
                                       }).ToList();

            if (model.SelectedAssetType.HasValue)
            {
                orders = orders.Where(w => w.Type == model.SelectedAssetType.Value).ToList();
            }
            if (model.AssetNumber.GetValueOrDefault(0) > 0)
            {
                orders = orders.Where(x => x.AssetNumber == (int)model.AssetNumber).ToList();
            }
            if (!string.IsNullOrEmpty(model.AssetName))
            {
                orders = orders.Where(x => !string.IsNullOrEmpty(x.AssetName) && x.AssetName.ToLower().Contains(model.AssetName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.AssetCounty))
            {
                orders = orders.Where(x => !string.IsNullOrEmpty(x.County) && x.County.ToLower().Contains(model.AssetCounty.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.City))
            {
                orders = orders.Where(x => !string.IsNullOrEmpty(x.City) && x.City.ToLower().Contains(model.City.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.State))
            {
                orders = orders.Where(a => a.State == model.State).ToList();
            }
            if (model.DateOrderedStartDate.HasValue)
            {
                orders = orders.Where(a => a.DateOfOrder >= model.DateOrderedStartDate.Value).ToList();
            }
            if (model.DateOrderedEndDate.HasValue)
            {
                orders = orders.Where(a => a.DateOfOrder <= model.DateOrderedEndDate.Value).ToList();
            }
            if (model.DateSubmittedStartDate.HasValue)
            {
                orders = orders.Where(a => a.DateOfSubmit >= model.DateSubmittedStartDate.Value).ToList();
            }
            if (model.DateSubmittedEndDate.HasValue)
            {
                orders = orders.Where(a => a.DateOfSubmit <= model.DateSubmittedEndDate.Value).ToList();
            }
            if (model.DatePaidStartDate.HasValue)
            {
                orders = orders.Where(a => a.DatePaid >= model.DatePaidStartDate.Value).ToList();
            }
            if (model.DatePaidEndDate.HasValue)
            {
                orders = orders.Where(a => a.DatePaid <= model.DatePaidEndDate.Value).ToList();
            }

            foreach (var o in orders)
            {
                //o.APN = string.Join(",", o.Apns.ToArray());
                try
                {
                    if (o.Apns != null)
                    {
                        o.APN = o.Apns.Count() > 0 ? o.Apns.First() : "";
                    }
                }
                catch { }
            }

            if (!string.IsNullOrEmpty(model.APN))
            {
                //orders = orders.Where(x => !string.IsNullOrEmpty(x.APN) && x.APN.ToLower().Contains(model.APN.ToLower())).ToList();
                orders = orders.Where(x => !string.IsNullOrEmpty(x.APN) && string.Join("", x.Apns).ToLower().Contains(model.APN.ToLower())).ToList();
            }
            return orders;
        }

        public List<OrderModel> GetOrdersForAdmin(OrderSearchModel model)
        {
            var context = _factory.Create();
            List<OrderModel> orders = (from a in context.Assets
                                       join apn in context.AssetTaxParcelNumbers on a.AssetId equals apn.AssetId into g
                                       join t in context.TitleCompanies on a.TitleCompanyId equals t.TitleCompanyId
                                       join o in context.TitleOrderPayments on a.AssetId equals o.AssetId into oGroup
                                       from item in oGroup.DefaultIfEmpty()
                                       where a.OrderStatus != OrderStatus.Unknown &&
                                       a.TitleCompanyId == model.TitleCompanyId &&
                                       a.IsActive
                                       select new OrderModel()
                                       {
                                           AssetNumber = a.AssetNumber,
                                           AssetId = a.AssetId,
                                           Type = a.AssetType,
                                           AssetName = a.ProjectName ?? "",
                                           County = a.County ?? "",
                                           City = a.City ?? "",
                                           State = a.State,
                                           Apns = g.Select(x => x.TaxParcelNumber),
                                           DateOfOrder = a.OrderDate,
                                           OrderStatus = a.OrderStatus,
                                           DateOfSubmit = a.DateOfOrderSubmit,
                                           Due = item == null ? t.CurrentRate : item.PaidAmount,
                                           DatePaid = item == null ? null : item.PaymentReceivedDate,
                                           TitleCompanyId = model.TitleCompanyId,
                                           TitleOrderPaymentId = item == null ? 0 : item.TitleOrderPaymentId
                                       }).ToList();
            if (model.AssetNumber.GetValueOrDefault(0) > 0)
            {
                orders = orders.Where(x => x.AssetNumber == (int)model.AssetNumber).ToList();
            }
            return orders;
        }

        public void RecordTitleOrderPayment(double currentRate, DateTime datePaid, int titleCompanyId, Guid assetId, int userId)
        {
            var context = _factory.Create();
            var payment = new TitleOrderPayment();
            payment.AssetId = assetId;
            payment.PaidAmount = currentRate;
            payment.PaymentReceivedDate = datePaid;
            payment.PaymentType = PaymentType.Other;
            payment.RecordedByUserId = userId;
            payment.TitleCompanyId = titleCompanyId;
            context.TitleOrderPayments.Add(payment);
            context.Save();
        }

        public OrderModel GetTitleOrderPayment(int titleOrderPaymentId)
        {
            var context = _factory.Create();
            var top = context.TitleOrderPayments.Where(x => x.TitleOrderPaymentId == titleOrderPaymentId).FirstOrDefault();
            if (top != null)
            {
                var model = new OrderModel();
                model.AssetId = (Guid)top.AssetId;
                model.DatePaid = top.PaymentReceivedDate;
                model.Due = top.PaidAmount;
                model.TitleOrderPaymentId = top.TitleOrderPaymentId;
                // dont need anything else at the moment
                return model;
            }
            return null;
        }

        public void UpdateTitleOrderPayment(OrderModel model)
        {
            var context = _factory.Create();
            var order = context.TitleOrderPayments.Where(x => x.TitleOrderPaymentId == model.TitleOrderPaymentId).FirstOrDefault();
            if (order != null)
            {
                order.PaidAmount = model.Due;
                order.PaymentReceivedDate = model.DatePaid;
                context.Save();
            }
        }

        public void DeleteTitleOrderPayment(int id)
        {
            var context = _factory.Create();
            var order = context.TitleOrderPayments.Where(x => x.TitleOrderPaymentId == id).FirstOrDefault();
            if (order != null)
            {
                context.TitleOrderPayments.Remove(order);
                context.Save();
            }
        }

        private List<SelectListItem> GetICAdminsList()
        {
            var context = _factory.Create();
            var users = context.Users.Where(w => w.UserType == UserType.ICAdmin && w.IsActive && w.ICStatus.HasValue && w.ICStatus.Value == ICStatus.Approved);
            var userList = new List<SelectListItem>();
            foreach (var user in users)
            {
                userList.Add(new SelectListItem() { Selected = false, Text = user.FullName, Value = user.UserId.ToString() });
            }
            return userList;
        }

        private Bitmap ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            var g = Graphics.FromImage(newImage);
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, image.Width, image.Height));
            g.DrawImage(image, 0, 0, newWidth, newHeight);
            Bitmap bmp = new Bitmap(newImage);

            return bmp;
        }

        private ImageFormat getFormat(string contentType)
        {
            switch (contentType)
            {
                case "image/bmp":
                    return ImageFormat.Bmp;
                case "image/gif":
                    return ImageFormat.Gif;
                case "image/x-icon":
                    return ImageFormat.Icon;
                case "image/jpeg":
                    return ImageFormat.Jpeg;
                case "image/png":
                    return ImageFormat.Png;
                case "image/tiff":
                    return ImageFormat.Tiff;
                default:
                    return ImageFormat.Jpeg;
            }
        }


        private string getTypeAbbreviation(AssetType typeName)
        {
            switch (typeName)
            {
                case AssetType.Retail:
                    return "CR";
                case AssetType.Office:
                    return "CO";
                case AssetType.MultiFamily:
                    return "MF";
                case AssetType.Industrial:
                    return "Ind";
                case AssetType.MHP:
                    return "MHP";
                case AssetType.ConvenienceStoreFuel:
                    return "SF";
                case AssetType.Medical:
                    return "Med";
                case AssetType.MixedUse:
                    return "Mix";
                case AssetType.Other:
                    return "OT";
                case AssetType.Hotel:
                    return "Hot";
                default:
                    return EnumHelper.GetEnumDescription(typeName);
            }
        }

        public bool HasSignedMDA(int userId)
        {
            var context = _factory.Create();
            return context.AssetUserMDAs.Any(w => w.UserId == userId);
        }

        public int GetPublishedCommercialAssetCount()
        {
            var context = _factory.Create();
            return context.Assets.Count(a => a.AssetType != AssetType.Other && a.AssetType != AssetType.MHP && a.AssetType != AssetType.MultiFamily);
        }

        public double GetPublishedCommercialAssetValue()
        {
            double a = 0;
            return a;
            //var context = _factory.Create();
            //return context.Assets.Where(a => a.AssetType != AssetType.Other && a.AssetType != AssetType.MHP && a.AssetType != AssetType.MultiFamily).Sum(s => s.AskingPrice);
            
            //IEPIRepository ePIRepository = this._factory.Create();
            //double num = (
            //    from a in ePIRepository.Assets
            //    where !a.IsTBDMarket
            //    select a).Sum<Asset>((Asset s) => s.AskingPrice);
            //double num1 = (
            //    from a in ePIRepository.Assets
            //    where a.IsTBDMarket
            //    select a).Sum<Asset>((Asset s) => s.CurrentBpo);
            //return num + num1;
        }

        public int GetTotalNumberOfMultiFamilyUnits()
        {
            var context = _factory.Create();
            var assets = context.Assets.Where(a => a.AssetType == AssetType.MultiFamily);
            int units = 0;
            foreach (var asset in assets)
            {
                var mf = (MultiFamilyAsset)asset;
                units += mf.TotalUnits;
            }
            return units;
        }

        public int GetTotalSumCommercialSqFt()
        {
            var context = _factory.Create();
            var assets = context.Assets.Where(a => a.AssetType != AssetType.MultiFamily && a.AssetType != AssetType.MHP);
            int sq = 0;
            foreach (var asset in assets)
            {
                var cf = (CommercialAsset)asset;
                sq += cf.SquareFeet;
            }
            return sq;
        }

        public List<AssetDescriptionModel> GetRelatedAssets(List<Guid> assetIds)
        {
            var context = _factory.Create();
            List<Guid> ids = new List<Guid>();
            List<AssetDescriptionModel> assets = new List<AssetDescriptionModel>();
            foreach (var id in assetIds)
            {
                var asset = context.Assets.FirstOrDefault(w => w.AssetId == id);
                if (asset != null)
                {
                    ids.AddRange(context.Assets.Where(w => w.ListedByUserId == asset.ListedByUserId && w.Show && w.IsActive).Select(s => s.AssetId));
                }
            }
            ids = ids.Distinct().ToList();
            foreach (var id in ids)
            {
                assets.Add(GetAssetByAssetId(id));
            }
            return assets;
        }

        public int GetTotalNumberOfAssets()
        {
            var context = _factory.Create();
            return context.Assets.Count(w => w.IsActive);
        }

        /// <summary>
        /// Add asset to an already signed MDA.
        /// This assumes there is already an MDA.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="assetId"></param>
        public void AddAssetIdToMDA(int userId, Guid assetId)
        {
            var context = _factory.Create();
            var mda = context.AssetUserMDAs.OrderByDescending(f => f.SignMDADate).FirstOrDefault(f => f.UserId == userId);
            if (mda != null)
            {
                context.AssetUserMDAs.Add(new AssetUserMDA()
                {
                    AssetId = assetId,
                    FileLocation = mda.FileLocation,
                    SignMDADate = DateTime.Today,
                    UserId = userId
                });
                context.Save();
            }
        }

        public List<SellerAssetQuickListModel> GetSellerManageAssetsQuickList(ManageAssetsModel model)
        {
            var list = new List<SellerAssetQuickListModel>();
            var context = _factory.Create();
            var assets = context.Assets.Where(w => w.IsActive && w.ListedByUserId == model.UserId.Value).ToList();
            var users = context.Users.ToList();
            if (model.AssetType != 0)
            {
                assets = assets.Where(w => w.AssetType == model.AssetType).ToList();
            }
            if (!string.IsNullOrEmpty(model.AssetNumber))
            {
                int id = 0;
                int.TryParse(model.AssetNumber, out id);
                if (id != 0)
                {
                    assets = assets.Where(a => a.AssetNumber == id).ToList();
                }
            }
            if (!string.IsNullOrEmpty(model.City))
            {
                //assets = assets.Where(a => String.Equals(a.City, model.City.ToLower(), StringComparison.CurrentCultureIgnoreCase)).ToList();
                assets = assets.Where(x => !string.IsNullOrEmpty(x.City) && x.City.ToLower().Contains(model.City.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.State))
            {
                assets = assets.Where(a => a.State == model.State).ToList();
            }
            if (!string.IsNullOrEmpty(model.ZipCode))
            {
                assets = assets.Where(a => a.Zip == model.ZipCode).ToList();
            }
            if (model.StartDate.HasValue)
            {
                assets = assets.Where(a => a.CreationDate >= model.StartDate.Value).ToList();
            }
            if (model.EndDate.HasValue)
            {
                assets = assets.Where(a => a.CreationDate <= model.EndDate.Value).ToList();
            }
            if (model.AssetName != null)
            {
                assets = assets.Where(w => w.ProjectName != null && w.ProjectName.ToLower().Contains(model.AssetName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.AddressLine1))
            {
                //assets = assets.Where(a => String.Equals(a.PropertyAddress, model.AddressLine1.ToLower(), StringComparison.CurrentCultureIgnoreCase)).ToList();
                assets = (from a in assets
                          where !string.IsNullOrEmpty(a.PropertyAddress) &&
                          a.PropertyAddress.ToLower().Contains(model.AddressLine1.ToLower())
                          select a).ToList();
            }
            assets.ForEach(a =>
            {
                var user = users.Where(x => x.UserId == a.ListedByUserId).FirstOrDefault();
                int units = 0;
                int squareFeet = 0;
                if (a.AssetType == AssetType.MultiFamily || a.AssetType == AssetType.MHP)
                {
                    var mf = a as MultiFamilyAsset;
                    units = mf.TotalUnits;
                }
                else
                {
                    var ca = a as CommercialAsset;
                    squareFeet = ca.SquareFeet;
                }
                list.Add(new SellerAssetQuickListModel()
                {
                    AddressLine1 = a.PropertyAddress,
                    AssetId = a.AssetId,
                    AssetNumber = a.AssetNumber,
                    City = a.City,
                    Show = a.Show ? "Yes" : "No",
                    State = a.State,
                    Zip = a.Zip,
                    Status = EnumHelper.GetEnumDescription(a.ListingStatus),
                    Type = EnumHelper.GetEnumDescription(a.AssetType),
                    ControllingUserType = model.ControllingUserType,
                    IsOnHold = a.HoldForUserId.HasValue,
                    IsSampleAsset = a.IsSampleAsset,
                    CreatedBy = user != null ? user.FullName + "~" + user.Username : "",
                    AssetName = a.ProjectName,
                    SquareFeet = squareFeet,
                    NumberOfUnits = units
                });
            });
            return list;
        }

        public bool HasSellerSignedIPA(Guid assetId, int userId)
        {
            var context = _factory.Create();
            var asset = context.Assets.FirstOrDefault(a => a.AssetId == assetId && a.ListedByUserId == userId);
            if (asset != null)
            {
                // for seller/nar IPAs we'll be looking at the user files table
                // for buyer IPAs we look at the AssetUserMDAs table
                return context.UserFiles.Any(a => a.AssetId == assetId && a.UserId == userId);
            }
            return false;
        }

        // added method inmplementations

        public void AddAssetIdsToMDA(int userId, List<Guid> assetIds)
        {
            IEPIRepository ePIRepository = this._factory.Create();
            List<AssetUserMDA> list = (
                from f in ePIRepository.AssetUserMDAs
                where f.UserId == userId
                select f).ToList<AssetUserMDA>();
            if (list.Count > 0)
            {
                AssetUserMDA assetUserMDA = (
                    from f in list
                    orderby f.SignMDADate descending
                    select f).FirstOrDefault<AssetUserMDA>();
                foreach (Guid assetId in assetIds)
                {
                    if (!list.Any<AssetUserMDA>((AssetUserMDA x) => x.AssetId == assetId))
                    {
                        IDbSet<AssetUserMDA> assetUserMDAs = ePIRepository.AssetUserMDAs;
                        AssetUserMDA assetUserMDA1 = new AssetUserMDA()
                        {
                            AssetId = assetId,
                            FileLocation = assetUserMDA.FileLocation,
                            SignMDADate = DateTime.Today,
                            UserId = userId
                        };
                        assetUserMDAs.Add(assetUserMDA1);
                        ePIRepository.Save();
                    }
                }
            }
        }

        public List<Asset> GetAssetList()
        {
            return this._factory.Create().Assets.ToList<Asset>();
        }

        public List<PortfolioQuickListViewModel> GetAssetQuickViewListPF(AssetSearchModel model)
        {
            int totalUnits;
            double? minPriceRange;
            int? nullable3;
            int? nullable4;
            bool flag;
            bool flag1;
            bool flag2;
            bool hasValue;
            bool flag3;
            bool flag4;
            IEPIRepository ePIRepository = this._factory.Create();
            List<Asset> assets = new List<Asset>();
            List<Guid> guids = new List<Guid>();
            if (model.UserId > 0)
            {
                guids = (
                    from x in ePIRepository.AssetUserMDAs
                    where x.UserId == model.UserId
                    select x.AssetId).ToList<Guid>();
            }
            List<Asset> list = ePIRepository.Assets.ToList<Asset>();
            List<Portfolio> portfolios = (
                from x in ePIRepository.Portfolios
                where x.isActive
                select x).ToList<Portfolio>();
            List<Portfolio> portfolios1 = portfolios;
            List<PortfolioAsset> portfolioAssets1 = new List<PortfolioAsset>();
            List<Asset> item = new List<Asset>();
            int? nullable5 = new int?(0);
            int? nullable6 = new int?(0);
            double? nullable7 = new double?(0);
            Dictionary<Guid, List<Asset>> guids1 = new Dictionary<Guid, List<Asset>>();
            Dictionary<Guid, int?> guids2 = new Dictionary<Guid, int?>();
            Dictionary<Guid, int?> guids3 = new Dictionary<Guid, int?>();
            Dictionary<Guid, double?> guids4 = new Dictionary<Guid, double?>();
            List<Guid> guids5 = new List<Guid>();
            portfolios.ForEach((Portfolio p) =>
            {
                item = new List<Asset>();
                IQueryable<PortfolioAsset> portfolioAssets =
                    from x in ePIRepository.PortfolioAssets
                    where x.PortfolioId == p.PortfolioId
                    select x;
                portfolioAssets1.AddRange(portfolioAssets);
                foreach (PortfolioAsset portfolioAsset in portfolioAssets)
                {
                    item.Add((
                        from x in list
                        where x.AssetId == portfolioAsset.AssetId
                        select x).First<Asset>());
                }
                guids1.Add(p.PortfolioId, item);
            });
            foreach (Portfolio portfolio in portfolios)
            {
                nullable6 = new int?(0);
                nullable5 = new int?(0);
                nullable7 = new double?(0);
                item = guids1[portfolio.PortfolioId];
                nullable5 = (
                    from x in item
                    select x.NumberNonRentableSpace).Sum();
                int? minUnitsSpaces = nullable5;
                int? maxUnitsSpaces = (
                    from x in item
                    select x.NumberRentableSpace).Sum();
                if (minUnitsSpaces.HasValue & maxUnitsSpaces.HasValue)
                {
                    nullable3 = new int?(minUnitsSpaces.GetValueOrDefault() + maxUnitsSpaces.GetValueOrDefault());
                }
                else
                {
                    nullable3 = null;
                }
                nullable5 = nullable3;
                List<Asset> list1 = (
                    from x in item
                    where x.AssetType == AssetType.MultiFamily
                    select x).ToList<Asset>();
                foreach (Asset asset in list1)
                {
                    MultiFamilyAsset multiFamilyAsset = asset as MultiFamilyAsset;
                    minUnitsSpaces = nullable5;
                    totalUnits = multiFamilyAsset.TotalUnits;
                    if (minUnitsSpaces.HasValue)
                    {
                        nullable4 = new int?(minUnitsSpaces.GetValueOrDefault() + totalUnits);
                    }
                    else
                    {
                        maxUnitsSpaces = null;
                        nullable4 = maxUnitsSpaces;
                    }
                    nullable5 = nullable4;
                }
                (
                    from x in item
                    where (x.AssetType == AssetType.MHP ? false : x.AssetType != AssetType.MultiFamily)
                    select x).ToList<Asset>();
                nullable6 = new int?((
                    from x in item
                    select x.SquareFeet).Sum());
                guids2.Add(portfolio.PortfolioId, nullable5);
                guids3.Add(portfolio.PortfolioId, nullable6);
                minUnitsSpaces = model.MinUnitsSpaces;
                if (!minUnitsSpaces.HasValue)
                {
                    minUnitsSpaces = model.MaxUnitsSpaces;
                    if (minUnitsSpaces.HasValue)
                    {
                        minUnitsSpaces = nullable5;
                        maxUnitsSpaces = model.MaxUnitsSpaces;
                        totalUnits = maxUnitsSpaces.Value;
                        if ((minUnitsSpaces.GetValueOrDefault() <= totalUnits ? 0 : Convert.ToInt32(minUnitsSpaces.HasValue)) != 0 && portfolios1.Contains(portfolio))
                        {
                            guids5.Add(portfolio.PortfolioId);
                        }
                    }
                }
                else
                {
                    minUnitsSpaces = model.MaxUnitsSpaces;
                    if (!minUnitsSpaces.HasValue)
                    {
                        minUnitsSpaces = nullable5;
                        maxUnitsSpaces = model.MinUnitsSpaces;
                        totalUnits = maxUnitsSpaces.Value;
                        if ((minUnitsSpaces.GetValueOrDefault() >= totalUnits ? 0 : Convert.ToInt32(minUnitsSpaces.HasValue)) != 0 && portfolios1.Contains(portfolio))
                        {
                            guids5.Add(portfolio.PortfolioId);
                        }
                    }
                    else
                    {
                        minUnitsSpaces = nullable5;
                        maxUnitsSpaces = model.MinUnitsSpaces;
                        totalUnits = maxUnitsSpaces.Value;
                        if ((minUnitsSpaces.GetValueOrDefault() <= totalUnits ? true : !minUnitsSpaces.HasValue))
                        {
                            flag4 = true;
                        }
                        else
                        {
                            minUnitsSpaces = nullable5;
                            maxUnitsSpaces = model.MaxUnitsSpaces;
                            totalUnits = maxUnitsSpaces.Value;
                            flag4 = (minUnitsSpaces.GetValueOrDefault() >= totalUnits ? 0 : Convert.ToInt32(minUnitsSpaces.HasValue)) == 0;
                        }
                        if (flag4)
                        {
                            if (portfolios1.Contains(portfolio))
                            {
                                guids5.Add(portfolio.PortfolioId);
                            }
                        }
                    }
                }
                minUnitsSpaces = model.MinSquareFeet;
                if (!minUnitsSpaces.HasValue)
                {
                    minUnitsSpaces = model.MaxSquareFeet;
                    if (minUnitsSpaces.HasValue)
                    {
                        minUnitsSpaces = nullable6;
                        maxUnitsSpaces = model.MaxSquareFeet;
                        totalUnits = maxUnitsSpaces.Value;
                        if ((minUnitsSpaces.GetValueOrDefault() <= totalUnits ? 0 : Convert.ToInt32(minUnitsSpaces.HasValue)) != 0 && portfolios1.Contains(portfolio))
                        {
                            guids5.Add(portfolio.PortfolioId);
                        }
                    }
                }
                else
                {
                    minUnitsSpaces = model.MaxSquareFeet;
                    if (!minUnitsSpaces.HasValue)
                    {
                        minUnitsSpaces = nullable6;
                        maxUnitsSpaces = model.MinSquareFeet;
                        totalUnits = maxUnitsSpaces.Value;
                        if ((minUnitsSpaces.GetValueOrDefault() >= totalUnits ? 0 : Convert.ToInt32(minUnitsSpaces.HasValue)) != 0 && portfolios1.Contains(portfolio))
                        {
                            guids5.Add(portfolio.PortfolioId);
                        }
                    }
                    else
                    {
                        minUnitsSpaces = nullable6;
                        maxUnitsSpaces = model.MinSquareFeet;
                        totalUnits = maxUnitsSpaces.Value;
                        if ((minUnitsSpaces.GetValueOrDefault() <= totalUnits ? true : !minUnitsSpaces.HasValue))
                        {
                            flag3 = true;
                        }
                        else
                        {
                            minUnitsSpaces = nullable5;
                            maxUnitsSpaces = model.MaxSquareFeet;
                            totalUnits = maxUnitsSpaces.Value;
                            flag3 = (minUnitsSpaces.GetValueOrDefault() >= totalUnits ? 0 : Convert.ToInt32(minUnitsSpaces.HasValue)) == 0;
                        }
                        if (flag3)
                        {
                            if (portfolios1.Contains(portfolio))
                            {
                                guids5.Add(portfolio.PortfolioId);
                            }
                        }
                    }
                }
                if (model.SelectedAssetType.HasValue)
                {
                    item = item.Where<Asset>((Asset w) =>
                    {
                        AssetType assetType = w.AssetType;
                        AssetType? selectedAssetType = model.SelectedAssetType;
                        return (assetType != selectedAssetType.GetValueOrDefault() ? false : selectedAssetType.HasValue);
                    }).ToList<Asset>();
                    if (item.Count<Asset>() < 1 && portfolios1.Contains(portfolio))
                    {
                        guids5.Add(portfolio.PortfolioId);
                    }
                }
                if (!string.IsNullOrEmpty(model.AssetName))
                {
                    item = (
                        from a in item
                        where (a.ProjectName == null ? false : a.ProjectName.ToLower().Contains(model.AssetName.ToLower()))
                        select a).ToList<Asset>();
                    if (item.Count<Asset>() < 1 && portfolios1.Contains(portfolio))
                    {
                        guids5.Add(portfolio.PortfolioId);
                    }
                }
                if (model.AssetIds.Count > 0)
                {
                    assets = new List<Asset>();
                    foreach (string assetId in model.AssetIds)
                    {
                        if (!string.IsNullOrEmpty(assetId))
                        {
                            int num = 0;
                            if (int.TryParse(assetId, out num))
                            {
                                List<Asset> assets1 = (
                                    from x in item
                                    where x.AssetNumber == num
                                    select x).ToList<Asset>();
                                if (assets1.Count < 1)
                                {
                                    if (!portfolios1.Contains(portfolio))
                                    {
                                        assets.Add(assets1.First<Asset>());
                                    }
                                    else
                                    {
                                        guids5.Add(portfolio.PortfolioId);
                                    }
                                }
                            }
                        }
                    }
                    item = assets.ToList<Asset>();
                }
                minUnitsSpaces = model.MaxAgeRange;
                if (minUnitsSpaces.HasValue)
                {
                    minUnitsSpaces = model.MaxAgeRange;
                    if (minUnitsSpaces.Value > 0)
                    {
                        DateTime now = DateTime.Now;
                        minUnitsSpaces = model.MaxAgeRange;
                        DateTime dateTime = now.AddYears(-minUnitsSpaces.Value);
                        item = (
                            from w in item
                            where w.YearBuilt >= dateTime.Year
                            select w).ToList<Asset>();
                        if (item.Count<Asset>() < 1)
                        {
                            if (portfolios1.Contains(portfolio))
                            {
                                guids5.Add(portfolio.PortfolioId);
                            }
                        }
                    }
                }
                if (model.SelectedStates.Count > 0)
                {
                    assets = new List<Asset>();
                    int count = 0;
                    bool flag5 = false;
                    foreach (StateSearchCriteriaModel selectedState in model.SelectedStates)
                    {
                        if (assets.Count != count)
                        {
                            assets = assets.Distinct<Asset>().ToList<Asset>();
                            count = assets.Count;
                        }
                        if (string.IsNullOrEmpty(selectedState.State))
                        {
                            if (selectedState.Counties.Any<string>((string x) => !string.IsNullOrEmpty(x)))
                            {
                                flag5 = true;
                                selectedState.Counties = (
                                    from x in selectedState.Counties
                                    where !string.IsNullOrEmpty(x)
                                    select x).ToList<string>().ConvertAll<string>((string x) => x.ToLower()).Distinct<string>().ToList<string>();
                                foreach (string county in selectedState.Counties)
                                {
                                    assets.AddRange(
                                        from x in item
                                        where (string.IsNullOrEmpty(x.County) ? false : x.County.ToLower().Contains(county))
                                        select x);
                                }
                            }
                            if (selectedState.Cities.Any<string>((string x) => !string.IsNullOrEmpty(x)))
                            {
                                flag5 = true;
                                selectedState.Cities = (
                                    from x in selectedState.Cities
                                    where !string.IsNullOrEmpty(x)
                                    select x).ToList<string>().ConvertAll<string>((string x) => x.ToLower()).Distinct<string>().ToList<string>();
                                foreach (string city in selectedState.Cities)
                                {
                                    assets.AddRange(
                                        from x in item
                                        where x.City != null
                                        where x.City.ToLower().Contains(city)
                                        select x);
                                }
                            }
                        }
                        else
                        {
                            flag5 = true;
                            bool flag6 = false;
                            List<Asset> list2 = (
                                from x in item
                                where x.State != null
                                where x.State == selectedState.State
                                select x).ToList<Asset>();
                            if (selectedState.Counties.Any<string>((string x) => !string.IsNullOrEmpty(x)))
                            {
                                flag6 = true;
                                selectedState.Counties = (
                                    from x in selectedState.Counties
                                    where !string.IsNullOrEmpty(x)
                                    select x).ToList<string>().ConvertAll<string>((string x) => x.ToLower()).Distinct<string>().ToList<string>();
                                foreach (string str in selectedState.Counties)
                                {
                                    assets.AddRange(
                                        from x in list2
                                        where (string.IsNullOrEmpty(x.County) ? false : x.County.ToLower().Contains(str))
                                        select x);
                                }
                            }
                            if (selectedState.Cities.Any<string>((string x) => !string.IsNullOrEmpty(x)))
                            {
                                flag6 = true;
                                selectedState.Cities = (
                                    from x in selectedState.Cities
                                    where !string.IsNullOrEmpty(x)
                                    select x).ToList<string>().ConvertAll<string>((string x) => x.ToLower()).Distinct<string>().ToList<string>();
                                foreach (string city1 in selectedState.Cities)
                                {
                                    assets.AddRange(
                                        from x in list2
                                        where x.City != null
                                        where x.City.ToLower().Contains(city1)
                                        select x);
                                }
                            }
                            if (!flag6)
                            {
                                assets.AddRange(list2);
                            }
                        }
                    }
                    if (flag5)
                    {
                        item = assets.Distinct<Asset>().ToList<Asset>();
                    }
                    if (item.Count < 1 && portfolios1.Contains(portfolio))
                    {
                        guids5.Add(portfolio.PortfolioId);
                    }
                }
                nullable7 = new double?((
                    from x in item
                    select (x.AskingPrice > 0 ? x.AskingPrice : x.CurrentBpo)).Sum());
                double? maxPriceRange = model.MinPriceRange;
                if (maxPriceRange.HasValue)
                {
                    maxPriceRange = model.MinPriceRange;
                    if (maxPriceRange.Value > 0)
                    {
                        maxPriceRange = model.MaxPriceRange;
                        if (maxPriceRange.HasValue)
                        {
                            goto Label1;
                        }
                        maxPriceRange = nullable7;
                        minPriceRange = model.MinPriceRange;
                        flag = ((double)maxPriceRange.GetValueOrDefault() < (double)minPriceRange.GetValueOrDefault() ? 0 : Convert.ToInt32(maxPriceRange.HasValue & minPriceRange.HasValue)) == 0;
                        goto Label0;
                    }
                }
            Label1:
                flag = true;
            Label0:
                if (flag)
                {
                    maxPriceRange = model.MaxPriceRange;
                    if (maxPriceRange.HasValue)
                    {
                        maxPriceRange = model.MaxPriceRange;
                        if (maxPriceRange.Value > 0)
                        {
                            maxPriceRange = model.MinPriceRange;
                            if (maxPriceRange.HasValue)
                            {
                                goto Label3;
                            }
                            maxPriceRange = nullable7;
                            minPriceRange = model.MaxPriceRange;
                            flag1 = ((double)maxPriceRange.GetValueOrDefault() > (double)minPriceRange.GetValueOrDefault() ? 0 : Convert.ToInt32(maxPriceRange.HasValue & minPriceRange.HasValue)) == 0;
                            goto Label2;
                        }
                    }
                Label3:
                    flag1 = true;
                Label2:
                    if (flag1)
                    {
                        maxPriceRange = model.MaxPriceRange;
                        if (maxPriceRange.HasValue)
                        {
                            maxPriceRange = model.MinPriceRange;
                            if (maxPriceRange.HasValue)
                            {
                                maxPriceRange = nullable7;
                                minPriceRange = model.MaxPriceRange;
                                if (((double)maxPriceRange.GetValueOrDefault() > (double)minPriceRange.GetValueOrDefault() ? true : !(maxPriceRange.HasValue & minPriceRange.HasValue)))
                                {
                                    goto Label5;
                                }
                                maxPriceRange = nullable7;
                                minPriceRange = model.MinPriceRange;
                                flag2 = ((double)maxPriceRange.GetValueOrDefault() < (double)minPriceRange.GetValueOrDefault() ? 0 : Convert.ToInt32(maxPriceRange.HasValue & minPriceRange.HasValue)) == 0;
                                goto Label4;
                            }
                        }
                    Label5:
                        flag2 = true;
                    Label4:
                        if (flag2)
                        {
                            maxPriceRange = model.MinPriceRange;
                            if (maxPriceRange.HasValue)
                            {
                                hasValue = false;
                            }
                            else
                            {
                                maxPriceRange = model.MaxPriceRange;
                                hasValue = !maxPriceRange.HasValue;
                            }
                            if (hasValue)
                            {
                                guids4.Add(portfolio.PortfolioId, nullable7);
                            }
                            else
                            {
                                guids5.Add(portfolio.PortfolioId);
                            }
                        }
                        else
                        {
                            guids4.Add(portfolio.PortfolioId, nullable7);
                        }
                    }
                    else
                    {
                        guids4.Add(portfolio.PortfolioId, nullable7);
                    }
                }
                else
                {
                    guids4.Add(portfolio.PortfolioId, nullable7);
                }
            }
            foreach (Guid guid in guids5)
            {
                portfolios.Remove(ePIRepository.Portfolios.First<Portfolio>((Portfolio x) => x.PortfolioId == guid));
            }
            List<PortfolioQuickListViewModel> portfolioQuickListViewModels1 = new List<PortfolioQuickListViewModel>();
            portfolios.ForEach((Portfolio p) =>
            {
                int? holdForUserId;
                double? nullable;
                double? nullable1;
                double? nullable2;
                int userId = 0;
                User user = (
                    from x in ePIRepository.Users
                    where x.UserId == model.UserId
                    select x).FirstOrDefault<User>();
                if (user != null)
                {
                    userId = user.UserId;
                }
                item = guids1[p.PortfolioId];
                List<PortfolioAssetsModel> portfolioAssetsModels = new List<PortfolioAssetsModel>();
                foreach (Asset curAsset in item)
                {
                    if (curAsset != null)
                    {
                        PortfolioAssetsModel portfolioAssetsModel = new PortfolioAssetsModel()
                        {
                            AddressLine1 = curAsset.PropertyAddress,
                            AssetId = curAsset.AssetId,
                            AssetNumber = curAsset.AssetNumber,
                            City = curAsset.City,
                            Show = (curAsset.Show ? "Yes" : "No"),
                            State = curAsset.State,
                            Zip = curAsset.Zip,
                            Status = EnumHelper.GetEnumDescription(curAsset.ListingStatus),
                            Type = EnumHelper.GetEnumDescription(curAsset.AssetType)
                        };
                        holdForUserId = curAsset.HoldForUserId;
                        portfolioAssetsModel.IsOnHold = holdForUserId.HasValue;
                        portfolioAssetsModel.IsSampleAsset = curAsset.IsSampleAsset;
                        portfolioAssetsModel.CreatedBy = (user != null ? string.Concat(user.FullName, "~", user.Username) : "");
                        portfolioAssetsModel.AssetName = curAsset.ProjectName;
                        portfolioAssetsModel.CanViewAssetName = guids.Contains(curAsset.AssetId);
                        portfolioAssetsModels.Add(portfolioAssetsModel);
                    }
                }
                List<PortfolioQuickListViewModel> portfolioQuickListViewModels = portfolioQuickListViewModels1;
                PortfolioQuickListViewModel portfolioQuickListViewModel = new PortfolioQuickListViewModel()
                {
                    NumberofAssets = p.NumberofAssets,
                    PortfolioId = p.PortfolioId,
                    PortfolioName = p.PortfolioName,
                    TotalItemCount = list.Count<Asset>(),
                    ControllingUserType = new UserType?((user != null ? user.UserType : UserType.ListingAgent)),
                    HasPrivileges = (p.UserId == userId ? true : false),
                    isActive = p.isActive,
                    AccListPrice = guids4[p.PortfolioId]
                };
                PortfolioQuickListViewModel portfolioQuickListViewModel1 = portfolioQuickListViewModel;
                holdForUserId = guids2[p.PortfolioId];
                if (holdForUserId.HasValue)
                {
                    nullable1 = new double?((double)holdForUserId.GetValueOrDefault());
                }
                else
                {
                    nullable = null;
                    nullable1 = nullable;
                }
                portfolioQuickListViewModel1.AccUnits = nullable1;
                PortfolioQuickListViewModel portfolioQuickListViewModel2 = portfolioQuickListViewModel;
                holdForUserId = guids3[p.PortfolioId];
                if (holdForUserId.HasValue)
                {
                    nullable2 = new double?((double)holdForUserId.GetValueOrDefault());
                }
                else
                {
                    nullable = null;
                    nullable2 = nullable;
                }
                portfolioQuickListViewModel2.AccSqFt = nullable2;
                portfolioQuickListViewModel.PortfolioAssets = portfolioAssetsModels;
                portfolioQuickListViewModels.Add(portfolioQuickListViewModel);
            });
            return portfolioQuickListViewModels1;
        }

        public List<PortfolioQuickListViewModel> GetManageAssetQuickListPF(ManageAssetsModel model)
        {
            int totalUnits;
            int? nullable3;
            int? nullable4;
            bool flag;
            bool flag1;
            IEPIRepository ePIRepository = this._factory.Create();
            List<Asset> assets = new List<Asset>();
            List<Guid> guids = new List<Guid>();
            int? minUnitsSpaces = model.UserId;
            if ((minUnitsSpaces.GetValueOrDefault() <= 0 ? 0 : Convert.ToInt32(minUnitsSpaces.HasValue)) != 0)
            {
                guids = (
                    from x in ePIRepository.AssetUserMDAs
                    where (int?)x.UserId == model.UserId
                    select x.AssetId).ToList<Guid>();
            }
            List<Asset> list = ePIRepository.Assets.ToList<Asset>();
            List<Portfolio> portfolios = ((model.ControllingUserType == UserType.CorpAdmin || model.ControllingUserType == UserType.CorpAdmin2 ? false : model.ControllingUserType != UserType.SiteAdmin) ? (
                from w in ePIRepository.Portfolios
                where w.isActive && (int?)w.UserId == model.UserId
                select w).ToList<Portfolio>() : (
                from w in ePIRepository.Portfolios
                where w.isActive
                select w).ToList<Portfolio>());
            List<Portfolio> portfolios1 = portfolios;
            List<PortfolioAsset> portfolioAssets1 = new List<PortfolioAsset>();
            List<Asset> assets1 = new List<Asset>();
            int? nullable5 = new int?(0);
            int? nullable6 = new int?(0);
            double? nullable7 = new double?(0);
            Dictionary<Guid, List<Asset>> guids1 = new Dictionary<Guid, List<Asset>>();
            Dictionary<Guid, int?> guids2 = new Dictionary<Guid, int?>();
            Dictionary<Guid, int?> guids3 = new Dictionary<Guid, int?>();
            Dictionary<Guid, double?> guids4 = new Dictionary<Guid, double?>();
            List<Guid> guids5 = new List<Guid>();
            portfolios.ForEach((Portfolio p) =>
            {
                assets1 = new List<Asset>();
                IQueryable<PortfolioAsset> portfolioAssets =
                    from x in ePIRepository.PortfolioAssets
                    where x.PortfolioId == p.PortfolioId
                    select x;
                portfolioAssets1.AddRange(portfolioAssets);
                foreach (PortfolioAsset portfolioAsset in portfolioAssets)
                {
                    if (model.ControllingUserType == UserType.ICAdmin)
                    {
                        var asset = list.FirstOrDefault(a => a.AssetId == portfolioAsset.AssetId && a.ListedByUserId == model.UserId.Value && !a.IsSubmitted);
                        if (asset != null) assets1.Add(asset);
                    }
                    else assets1.Add((
                        from x in list
                        where x.AssetId == portfolioAsset.AssetId
                        select x).First<Asset>());
                }
                guids1.Add(p.PortfolioId, assets1);
            });
            // only show portfolios where none of the assets have been submitted to corp admin
            if (model.ControllingUserType == UserType.ICAdmin)
            {
                portfolioAssets1.ForEach(f =>
                {
                    var asset = ePIRepository.Assets.First(a => a.AssetId == f.AssetId);
                    if (asset.IsSubmitted)
                    {
                        portfolios.Remove(portfolios.Find(p => p.PortfolioId == f.PortfolioId));
                    }
                });
            }
            foreach (Portfolio portfolio in portfolios)
            {
                nullable6 = new int?(0);
                nullable5 = new int?(0);
                nullable7 = new double?(0);
                assets1 = guids1[portfolio.PortfolioId];
                assets1.RemoveAll((Asset item) => item == null);
                nullable5 = (
                    from x in assets1
                    select x.NumberNonRentableSpace).Sum();
                minUnitsSpaces = nullable5;
                int? maxUnitsSpaces = (
                    from x in assets1
                    select x.NumberRentableSpace).Sum();
                if (minUnitsSpaces.HasValue & maxUnitsSpaces.HasValue)
                {
                    nullable3 = new int?(minUnitsSpaces.GetValueOrDefault() + maxUnitsSpaces.GetValueOrDefault());
                }
                else
                {
                    nullable3 = null;
                }
                nullable5 = nullable3;
                List<Asset> list1 = (
                    from x in assets1
                    where x.AssetType == AssetType.MultiFamily
                    select x).ToList<Asset>();
                foreach (Asset asset in list1)
                {
                    MultiFamilyAsset multiFamilyAsset = asset as MultiFamilyAsset;
                    minUnitsSpaces = nullable5;
                    totalUnits = multiFamilyAsset.TotalUnits;
                    if (minUnitsSpaces.HasValue)
                    {
                        nullable4 = new int?(minUnitsSpaces.GetValueOrDefault() + totalUnits);
                    }
                    else
                    {
                        maxUnitsSpaces = null;
                        nullable4 = maxUnitsSpaces;
                    }
                    nullable5 = nullable4;
                }
                (
                    from x in assets1
                    where (x.AssetType == AssetType.MHP ? false : x.AssetType != AssetType.MultiFamily)
                    select x).ToList<Asset>();
                nullable6 = new int?((
                    from x in assets1
                    select x.SquareFeet).Sum());
                guids2.Add(portfolio.PortfolioId, nullable5);
                guids3.Add(portfolio.PortfolioId, nullable6);
                minUnitsSpaces = model.MinUnitsSpaces;
                if (!minUnitsSpaces.HasValue)
                {
                    minUnitsSpaces = model.MaxUnitsSpaces;
                    if (minUnitsSpaces.HasValue)
                    {
                        minUnitsSpaces = nullable5;
                        maxUnitsSpaces = model.MaxUnitsSpaces;
                        totalUnits = maxUnitsSpaces.Value;
                        if ((minUnitsSpaces.GetValueOrDefault() <= totalUnits ? 0 : Convert.ToInt32(minUnitsSpaces.HasValue)) != 0 && portfolios1.Contains(portfolio))
                        {
                            guids5.Add(portfolio.PortfolioId);
                        }
                    }
                }
                else
                {
                    minUnitsSpaces = model.MaxUnitsSpaces;
                    if (!minUnitsSpaces.HasValue)
                    {
                        minUnitsSpaces = nullable5;
                        maxUnitsSpaces = model.MinUnitsSpaces;
                        totalUnits = maxUnitsSpaces.Value;
                        if ((minUnitsSpaces.GetValueOrDefault() >= totalUnits ? 0 : Convert.ToInt32(minUnitsSpaces.HasValue)) != 0 && portfolios1.Contains(portfolio))
                        {
                            guids5.Add(portfolio.PortfolioId);
                        }
                    }
                    else
                    {
                        minUnitsSpaces = nullable5;
                        maxUnitsSpaces = model.MinUnitsSpaces;
                        totalUnits = maxUnitsSpaces.Value;
                        if ((minUnitsSpaces.GetValueOrDefault() <= totalUnits ? true : !minUnitsSpaces.HasValue))
                        {
                            flag1 = true;
                        }
                        else
                        {
                            minUnitsSpaces = nullable5;
                            maxUnitsSpaces = model.MaxUnitsSpaces;
                            totalUnits = maxUnitsSpaces.Value;
                            flag1 = (minUnitsSpaces.GetValueOrDefault() >= totalUnits ? 0 : Convert.ToInt32(minUnitsSpaces.HasValue)) == 0;
                        }
                        if (flag1)
                        {
                            if (portfolios1.Contains(portfolio))
                            {
                                guids5.Add(portfolio.PortfolioId);
                            }
                        }
                    }
                }
                minUnitsSpaces = model.MinSquareFeet;
                if (!minUnitsSpaces.HasValue)
                {
                    minUnitsSpaces = model.MaxSquareFeet;
                    if (minUnitsSpaces.HasValue)
                    {
                        minUnitsSpaces = nullable6;
                        maxUnitsSpaces = model.MaxSquareFeet;
                        totalUnits = maxUnitsSpaces.Value;
                        if ((minUnitsSpaces.GetValueOrDefault() <= totalUnits ? 0 : Convert.ToInt32(minUnitsSpaces.HasValue)) != 0 && portfolios1.Contains(portfolio))
                        {
                            guids5.Add(portfolio.PortfolioId);
                        }
                    }
                }
                else
                {
                    minUnitsSpaces = model.MaxSquareFeet;
                    if (!minUnitsSpaces.HasValue)
                    {
                        minUnitsSpaces = nullable6;
                        maxUnitsSpaces = model.MinSquareFeet;
                        totalUnits = maxUnitsSpaces.Value;
                        if ((minUnitsSpaces.GetValueOrDefault() >= totalUnits ? 0 : Convert.ToInt32(minUnitsSpaces.HasValue)) != 0 && portfolios1.Contains(portfolio))
                        {
                            guids5.Add(portfolio.PortfolioId);
                        }
                    }
                    else
                    {
                        minUnitsSpaces = nullable6;
                        maxUnitsSpaces = model.MinSquareFeet;
                        totalUnits = maxUnitsSpaces.Value;
                        if ((minUnitsSpaces.GetValueOrDefault() <= totalUnits ? true : !minUnitsSpaces.HasValue))
                        {
                            flag = true;
                        }
                        else
                        {
                            minUnitsSpaces = nullable5;
                            maxUnitsSpaces = model.MaxSquareFeet;
                            totalUnits = maxUnitsSpaces.Value;
                            flag = (minUnitsSpaces.GetValueOrDefault() >= totalUnits ? 0 : Convert.ToInt32(minUnitsSpaces.HasValue)) == 0;
                        }
                        if (flag)
                        {
                            if (portfolios1.Contains(portfolio))
                            {
                                guids5.Add(portfolio.PortfolioId);
                            }
                        }
                    }
                }
                if ((int)model.AssetType != 0)
                {
                    if ((
                        from w in assets1
                        where w.AssetType == model.AssetType
                        select w).ToList<Asset>().Count<Asset>() < 1 && portfolios1.Contains(portfolio))
                    {
                        guids5.Add(portfolio.PortfolioId);
                    }
                }
                if (!string.IsNullOrEmpty(model.AssetName))
                {
                    var regex = "[^A-Za-z0-9]";
                    var assetName = Regex.Replace(model.AssetName, regex, "");
                    if ((
                        from a in assets1
                        where (a.ProjectName == null ? false : Regex.Replace(a.ProjectName.ToLower(), regex, "").Contains(assetName.ToLower()))
                        select a).ToList<Asset>().Count<Asset>() < 1 && portfolios1.Contains(portfolio))
                    {
                        guids5.Add(portfolio.PortfolioId);
                    }
                }
                if (!string.IsNullOrEmpty(model.AssetNumber))
                {
                    int num = 0;
                    int.TryParse(model.AssetNumber, out num);
                    if (num != 0)
                    {
                        if ((
                            from a in assets1
                            where a.AssetNumber == num
                            select a).ToList<Asset>().Count<Asset>() < 1 && portfolios1.Contains(portfolio))
                        {
                            guids5.Add(portfolio.PortfolioId);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(model.AddressLine1))
                {
                    assets1 = (
                        from a in assets1
                        where (string.IsNullOrEmpty(a.PropertyAddress) ? false : a.PropertyAddress.ToLower().Contains(model.AddressLine1.ToLower()))
                        select a).ToList<Asset>();
                    if (assets1.Count<Asset>() < 1 && portfolios1.Contains(portfolio))
                    {
                        guids5.Add(portfolio.PortfolioId);
                    }
                }
                if (!string.IsNullOrEmpty(model.City))
                {
                    if ((
                        from x in assets1
                        where (string.IsNullOrEmpty(x.City) ? false : x.City.ToLower().Contains(model.City.ToLower()))
                        select x).ToList<Asset>().Count<Asset>() < 1 && portfolios1.Contains(portfolio))
                    {
                        guids5.Add(portfolio.PortfolioId);
                    }
                }
                if (!string.IsNullOrEmpty(model.State))
                {
                    if ((
                        from a in assets1
                        where a.State == model.State
                        select a).ToList<Asset>().Count<Asset>() < 1 && portfolios1.Contains(portfolio))
                    {
                        guids5.Add(portfolio.PortfolioId);
                    }
                }
                if (!string.IsNullOrEmpty(model.ZipCode))
                {
                    if ((
                        from a in assets1
                        where a.Zip == model.ZipCode
                        select a).ToList<Asset>().Count<Asset>() < 1 && portfolios1.Contains(portfolio))
                    {
                        guids5.Add(portfolio.PortfolioId);
                    }
                }
                if (model.StartDate.HasValue)
                {
                    if (assets1.Where<Asset>((Asset a) =>
                    {
                        DateTime? creationDate = a.CreationDate;
                        DateTime value = model.StartDate.Value;
                        return (creationDate.HasValue ? creationDate.GetValueOrDefault() >= value : false);
                    }).ToList<Asset>().Count<Asset>() < 1 && portfolios1.Contains(portfolio))
                    {
                        guids5.Add(portfolio.PortfolioId);
                    }
                }
                if (model.EndDate.HasValue)
                {
                    if (assets1.Where<Asset>((Asset a) =>
                    {
                        DateTime? creationDate = a.CreationDate;
                        DateTime value = model.EndDate.Value;
                        return (creationDate.HasValue ? creationDate.GetValueOrDefault() <= value : false);
                    }).ToList<Asset>().Count<Asset>() < 1 && portfolios1.Contains(portfolio))
                    {
                        guids5.Add(portfolio.PortfolioId);
                    }
                }
                if (!string.IsNullOrEmpty(model.ApnNumber))
                {
                    var regex = "[^A-Za-z0-9]";
                    var apnNumber = Regex.Replace(model.ApnNumber, regex, "");
                    var apnList = ePIRepository.AssetTaxParcelNumbers.ToList();
                    var apns = apnList.Where(w => w.TaxParcelNumber != null && Regex.Replace(w.TaxParcelNumber.ToLower(), regex, "").Contains(apnNumber.ToLower())).Select(s => s.AssetId).Distinct().ToList();
                    assets = assets1.Where(a => apns.Contains(a.AssetId)).ToList();
                    if (assets.ToList<Asset>().Count<Asset>() < 1 && portfolios1.Contains(portfolio))
                    {
                        guids5.Add(portfolio.PortfolioId);
                    }
                }

                nullable7 = new double?((
                    from x in assets1
                    select (x.AskingPrice > 0 ? x.AskingPrice : x.CurrentBpo)).Sum());
                guids4.Add(portfolio.PortfolioId, nullable7);
            }
            foreach (Guid guid in guids5)
            {
                portfolios.Remove(ePIRepository.Portfolios.First<Portfolio>((Portfolio x) => x.PortfolioId == guid));
            }
            List<PortfolioQuickListViewModel> portfolioQuickListViewModels1 = new List<PortfolioQuickListViewModel>();
            portfolios.ForEach((Portfolio p) =>
            {
                int? holdForUserId;
                double? nullable;
                double? nullable1;
                double? nullable2;
                User user = (
                    from x in ePIRepository.Users
                    where (int?)x.UserId == model.UserId
                    select x).FirstOrDefault<User>();
                int userId = user.UserId;
                assets1 = guids1[p.PortfolioId];
                List<PortfolioAssetsModel> portfolioAssetsModels = new List<PortfolioAssetsModel>();
                foreach (Asset curAsset in assets1)
                {
                    if (curAsset != null)
                    {
                        PortfolioAssetsModel portfolioAssetsModel = new PortfolioAssetsModel()
                        {
                            AddressLine1 = curAsset.PropertyAddress,
                            AssetId = curAsset.AssetId,
                            AssetNumber = curAsset.AssetNumber,
                            City = curAsset.City,
                            Show = (curAsset.Show ? "Yes" : "No"),
                            State = curAsset.State,
                            Zip = curAsset.Zip,
                            Status = EnumHelper.GetEnumDescription(curAsset.ListingStatus),
                            Type = EnumHelper.GetEnumDescription(curAsset.AssetType)
                        };
                        holdForUserId = curAsset.HoldForUserId;
                        portfolioAssetsModel.IsOnHold = holdForUserId.HasValue;
                        portfolioAssetsModel.IsSampleAsset = curAsset.IsSampleAsset;
                        portfolioAssetsModel.CreatedBy = (user != null ? string.Concat(user.FullName, "~", user.Username) : "");
                        portfolioAssetsModel.AssetName = curAsset.ProjectName;
                        portfolioAssetsModel.CanViewAssetName = guids.Contains(curAsset.AssetId);
                        portfolioAssetsModels.Add(portfolioAssetsModel);
                    }
                }
                List<PortfolioQuickListViewModel> portfolioQuickListViewModels = portfolioQuickListViewModels1;
                PortfolioQuickListViewModel portfolioQuickListViewModel = new PortfolioQuickListViewModel()
                {
                    NumberofAssets = p.NumberofAssets,
                    PortfolioId = p.PortfolioId,
                    PortfolioName = p.PortfolioName,
                    TotalItemCount = list.Count<Asset>(),
                    ControllingUserType = new UserType?(user.UserType),
                    HasPrivileges = (p.UserId == userId ? true : false),
                    isActive = p.isActive,
                    AccListPrice = guids4[p.PortfolioId]
                };
                PortfolioQuickListViewModel portfolioQuickListViewModel1 = portfolioQuickListViewModel;
                holdForUserId = guids2[p.PortfolioId];
                if (holdForUserId.HasValue)
                {
                    nullable1 = new double?((double)holdForUserId.GetValueOrDefault());
                }
                else
                {
                    nullable = null;
                    nullable1 = nullable;
                }
                portfolioQuickListViewModel1.AccUnits = nullable1;
                PortfolioQuickListViewModel portfolioQuickListViewModel2 = portfolioQuickListViewModel;
                holdForUserId = guids3[p.PortfolioId];
                if (holdForUserId.HasValue)
                {
                    nullable2 = new double?((double)holdForUserId.GetValueOrDefault());
                }
                else
                {
                    nullable = null;
                    nullable2 = nullable;
                }
                portfolioQuickListViewModel2.AccSqFt = nullable2;
                portfolioQuickListViewModel.PortfolioAssets = portfolioAssetsModels;
                portfolioQuickListViewModels.Add(portfolioQuickListViewModel);
            });
            return portfolioQuickListViewModels1;
        }

        public int GetPublishedAssetCount()
        {
            int num = this._factory.Create().Assets.Count<Asset>((Asset x) => x.IsSubmitted);
            return num;
        }

        public List<AdminAssetQuickListModel> PopulateAssetQuickList(List<AdminAssetQuickListModel> incompleteAssets)
        {
            List<Asset> assets = new List<Asset>();
            IEPIRepository ePIRepository = this._factory.Create();
            foreach (AdminAssetQuickListModel incompleteAsset in incompleteAssets)
            {
                assets.Add((
                    from x in ePIRepository.Assets
                    where x.AssetId == incompleteAsset.AssetId
                    select x).First<Asset>());
            }
            List<AdminAssetQuickListModel> adminAssetQuickListModels = new List<AdminAssetQuickListModel>();
            assets.ForEach((Asset a) =>
            {
                int totalUnits = 0;
                int squareFeet = 0;
                bool flag = true;
                if (a.AssetType == AssetType.MultiFamily)
                {
                    totalUnits = (a as MultiFamilyAsset).TotalUnits;
                }
                else if (a.AssetType != AssetType.MHP)
                {
                    squareFeet = (a as CommercialAsset).SquareFeet;
                }
                else
                {
                    totalUnits = (a as MultiFamilyAsset).TotalUnits;
                    totalUnits += (a.NumberRentableSpace.HasValue ? a.NumberRentableSpace.Value : 0);
                    totalUnits += (a.NumberNonRentableSpace.HasValue ? a.NumberNonRentableSpace.Value : 0);
                }
                if (flag)
                {
                    adminAssetQuickListModels.Add(new AdminAssetQuickListModel()
                    {
                        AddressLine1 = a.PropertyAddress,
                        AssetId = a.AssetId,
                        AssetNumber = a.AssetNumber,
                        City = a.City,
                        Show = (a.Show ? "Yes" : "No"),
                        State = a.State,
                        Zip = a.Zip,
                        Status = EnumHelper.GetEnumDescription(a.ListingStatus),
                        Type = EnumHelper.GetEnumDescription(a.AssetType),
                        IsOnHold = a.HoldForUserId.HasValue,
                        IsSampleAsset = a.IsSampleAsset,
                        AssetName = a.ProjectName,
                        SquareFeet = squareFeet,
                        NumberOfUnits = totalUnits
                    });
                }
            });
            return adminAssetQuickListModels;
        }

        public List<AssetAPNMatchModel> GetMatchingAssetsByAPNCountyState(string parcelNumber, string state, string county)
        {
            List<AssetAPNMatchModel> matchingAssets = new List<AssetAPNMatchModel>();
            var regex = "[^A-Za-z0-9]";
            IEPIRepository repo = this._factory.Create();
            parcelNumber = Regex.Replace(parcelNumber, regex, "");
            var parcelNumbers = from atpn in repo.AssetTaxParcelNumbers
                                join a in repo.Assets on atpn.AssetId equals a.AssetId
                                where a.State.ToLower() == state.ToLower() &&
                                a.County.ToLower() == county.ToLower() &&
                                atpn.TaxParcelNumber != null
                                select atpn;

            var matches = parcelNumbers.ToList().Where(a => Regex.Replace(a.TaxParcelNumber, regex, "") == parcelNumber);
            foreach (var apn in matches)
            {
                var asset = repo.Assets.First(a => a.AssetId == apn.AssetId);
                matchingAssets.Add(new AssetAPNMatchModel()
                {
                    AssetId = apn.AssetId,
                    AssetType = asset.AssetType.ToString(),
                    AddressLine1 = asset.PropertyAddress,
                    AddressLine2 = asset.PropertyAddress2,
                    City = asset.City,
                    State = asset.State,
                    County = asset.County,
                    Zip = asset.Zip,
                    Owner = asset.Owner
                });
            }

            return matchingAssets;
        }

        public bool CreateAssetOwnershipChange(AssetOwnershipChangeViewModel model)
        {
            bool success = true;
            try
            {
                var context = _factory.Create();
                AssetOwnershipChange entity = new AssetOwnershipChange();
                entity.AssetOwnershipChangeId = model.AssetOwnershipChangeId != Guid.Empty ? model.AssetOwnershipChangeId : Guid.NewGuid();
                entity.AssetId = model.AssetId;
                entity.CreateDate = DateTime.Now;
                entity.AcquisitionDate = model.AcquisitionDate;
                entity.OwnerHoldingCompanyId = model.OwnerHoldingCompanyId;
                entity.PreviousOwnerHoldingCompanyId = model.PreviousOwnerHoldingCompanyId;
                entity.ClosingPrice = model.ClosingPrice;
                entity.ProformaCapRate = model.ProformaCapRate;
                entity.SellerTerms = model.SellerTerms;
                context.AssetOwnershipChanges.Add(entity);
                context.Save();
            }
            catch (Exception ex)
            {
                success = false;
                // We really should either return the error or throw
                throw ex;
            }
            return success;
        }

        public void ActivateAsset(string assetId)
        {
            var context = _factory.Create();
            Guid asId = new Guid(assetId);
            var asset = context.Assets.SingleOrDefault(w => w.AssetId == asId);
            if (asset != null)
            {
                asset.IsActive = true;
                context.Save();
            }
        }

        public void DeActivateAsset(string assetId)
        {
            //If an asset is deactived then automatically unpublished, and actived does not again published it needs one more click

            var context = _factory.Create();
            Guid asId = new Guid(assetId);
            var asset = context.Assets.SingleOrDefault(w => w.AssetId == asId);
            if (asset != null)
            {
                asset.Show = false;                
                asset.IsActive = false;
                context.Save();

                var assetLocks = context.AssetLocks.Where(x => x.AssetId == asId).ToList();
                if (assetLocks.Count > 0)
                {
                    foreach (var assetLock in assetLocks)
                    {
                        context.AssetLocks.Remove(assetLock);
                    }
                    context.Save();
                }
            }
        }
        public void ActivateNarMember(int narMemberId)
        {
            var context = _factory.Create();
            var narMembers = context.NarMembers.SingleOrDefault(w => w.NarMemberId == narMemberId);
            if (narMembers != null)
            {
                narMembers.IsActive = true;
                context.Save();
            }
        }
        public void DeActivateNarMember(int narMemberId)
        {
            var context = _factory.Create();
            var narMembers = context.NarMembers.SingleOrDefault(w => w.NarMemberId == narMemberId);
            if (narMembers != null)
            {
                narMembers.IsActive = false;
                context.Save();
            }
        }
        public List<AdminAssetQuickListModel> GetAssetsbyHCId(string HcId)
        {
            var list = new List<AdminAssetQuickListModel>();
            var context = _factory.Create();

            Guid hcId = new Guid(HcId);
            var AssetsidsInPortfolio = context.AssetHCOwnership.Where(x => x.HoldingCompanyId == hcId).Select(a => a.AssetId).ToList();

            var assets = context.Assets.Where(la => AssetsidsInPortfolio.Contains(la.AssetId));

            var users = context.Users.ToList();
            bool isSpecificType = false;

            var assetList = assets.ToList();

            assetList.ForEach(a =>
            {

                int units = 0;
                int squareFeet = 0;
                if (a.AssetType == AssetType.MultiFamily)
                {
                    var mf = a as MultiFamilyAsset;
                    units = mf.TotalUnits;
                }
                else if (a.AssetType == AssetType.MHP)
                {
                    var mf = a as MultiFamilyAsset;
                    units = mf.TotalUnits;
                    units += a.NumberRentableSpace != null ? (int)a.NumberRentableSpace : 0;
                    units += a.NumberNonRentableSpace != null ? (int)a.NumberNonRentableSpace : 0;
                }
                else
                {
                    var ca = a as CommercialAsset;
                    squareFeet = ca.SquareFeet;
                }

                if (a != null)
                {
                    //-------
                    var aoe = a.ProformaAnnualOperExpenses;
                    var pagi = a.ProformaAnnualIncome;
                    var pami = a.ProformaMiscIncome;
                    var totalIncome = pagi + pami;
                    var pvf = (a.ProformaVacancyFac / 100) * totalIncome;
                    var proformaNOI = Math.Round((totalIncome - pvf) - aoe);
                    var pretax = totalIncome - pvf - aoe;
                    //-------

                    list.Add(new AdminAssetQuickListModel()
                    {
                        AddressLine1 = a.PropertyAddress,
                        AssetId = a.AssetId,
                        AssetNumber = a.AssetNumber,
                        City = a.City,
                        Show = a.Show ? "Yes" : "No",
                        State = a.State,
                        Zip = a.Zip,
                        Status = EnumHelper.GetEnumDescription(a.ListingStatus),
                        Type = EnumHelper.GetEnumDescription(a.AssetType),
                        //ControllingUserType = model.ControllingUserType,
                        IsOnHold = a.HoldForUserId.HasValue,
                        IsSampleAsset = a.IsSampleAsset,
                        CreatedBy = context.Users.Where(x => x.UserId == a.ListedByUserId).FirstOrDefault() != null
                                  ? context.Users.Where(x => x.UserId == a.ListedByUserId).FirstOrDefault().FullName + "~" +
                                  context.Users.Where(x => x.UserId == a.ListedByUserId).FirstOrDefault().Username : "",
                        AssetName = a.ProjectName,
                        SquareFeet = squareFeet,
                        NumberOfUnits = units,
                        isSpecificType = isSpecificType,
                        CurrentVacancyFac = a.CurrentVacancyFac,
                        LastReportedOccupancyDate = a.LastReportedOccupancyDate != null ? a.LastReportedOccupancyDate : a.OccupancyDate,
                        ProformaAnnualIncome = a.ProformaAnnualIncome,
                        ProformaNOI = proformaNOI,
                        CashInvestmentApy = a.CashInvestmentApy,
                        capRate = ((pretax / a.CurrentBpo) * 100),
                        AskingPrice = a.AskingPrice,
                        CurrentBpo = a.CurrentBpo,
                        Portfolio = context.PortfolioAssets.Where(x => x.AssetId == a.AssetId).Any() ? true : false,

                        /*Unknown 0,Yes 1,No 2	 */
                        AssmFin = a.HasPositionMortgage == PositionMortgageType.Yes ? "Yes" : "No",
                        UserType = context.Users.Where(us => us.UserId == a.ListedByUserId).FirstOrDefault().UserType,
                        ListingStatus = a.ListingStatus,
                        IsActive = a.IsActive,
                        BusDriver = a.Show ? "CA" : "SUS"

                    });

                }
            });
            return list;

        }

        public List<AdminAssetQuickListModel> GetAssetsbyOCId(string OcId)
        {
            var list = new List<AdminAssetQuickListModel>();
            var context = _factory.Create();

            Guid ocId = new Guid(OcId);
            var AssetsidsInPortfolio = context.AssetOC.Where(x => x.OperatingCompanyId == ocId).Select(a => a.AssetId).ToList();

            var assets = context.Assets.Where(la => AssetsidsInPortfolio.Contains(la.AssetId));

            var users = context.Users.ToList();
            bool isSpecificType = false;

            var assetList = assets.ToList();

            assetList.ForEach(a =>
            {

                int units = 0;
                int squareFeet = 0;
                if (a.AssetType == AssetType.MultiFamily)
                {
                    var mf = a as MultiFamilyAsset;
                    units = mf.TotalUnits;
                }
                else if (a.AssetType == AssetType.MHP)
                {
                    var mf = a as MultiFamilyAsset;
                    units = mf.TotalUnits;
                    units += a.NumberRentableSpace != null ? (int)a.NumberRentableSpace : 0;
                    units += a.NumberNonRentableSpace != null ? (int)a.NumberNonRentableSpace : 0;
                }
                else
                {
                    var ca = a as CommercialAsset;
                    squareFeet = ca.SquareFeet;
                }

                if (a != null)
                {
                    //-------
                    var aoe = a.ProformaAnnualOperExpenses;
                    var pagi = a.ProformaAnnualIncome;
                    var pami = a.ProformaMiscIncome;
                    var totalIncome = pagi + pami;
                    var pvf = (a.ProformaVacancyFac / 100) * totalIncome;
                    var proformaNOI = Math.Round((totalIncome - pvf) - aoe);
                    var pretax = totalIncome - pvf - aoe;
                    //-------

                    list.Add(new AdminAssetQuickListModel()
                    {
                        AddressLine1 = a.PropertyAddress,
                        AssetId = a.AssetId,
                        AssetNumber = a.AssetNumber,
                        City = a.City,
                        Show = a.Show ? "Yes" : "No",
                        State = a.State,
                        Zip = a.Zip,
                        Status = EnumHelper.GetEnumDescription(a.ListingStatus),
                        Type = EnumHelper.GetEnumDescription(a.AssetType),
                        //ControllingUserType = model.ControllingUserType,
                        IsOnHold = a.HoldForUserId.HasValue,
                        IsSampleAsset = a.IsSampleAsset,
                        CreatedBy = context.Users.Where(x => x.UserId == a.ListedByUserId).FirstOrDefault() != null
                                  ? context.Users.Where(x => x.UserId == a.ListedByUserId).FirstOrDefault().FullName + "~" +
                                  context.Users.Where(x => x.UserId == a.ListedByUserId).FirstOrDefault().Username : "",
                        AssetName = a.ProjectName,
                        SquareFeet = squareFeet,
                        NumberOfUnits = units,
                        isSpecificType = isSpecificType,
                        CurrentVacancyFac = a.CurrentVacancyFac,
                        LastReportedOccupancyDate = a.LastReportedOccupancyDate != null ? a.LastReportedOccupancyDate : a.OccupancyDate,
                        ProformaAnnualIncome = a.ProformaAnnualIncome,
                        ProformaNOI = proformaNOI,
                        CashInvestmentApy = a.CashInvestmentApy,
                        capRate = ((pretax / a.CurrentBpo) * 100),
                        AskingPrice = a.AskingPrice,
                        CurrentBpo = a.CurrentBpo,
                        Portfolio = context.PortfolioAssets.Where(x => x.AssetId == a.AssetId).Any() ? true : false,

                        /*Unknown 0,Yes 1,No 2	 */
                        AssmFin = a.HasPositionMortgage == PositionMortgageType.Yes ? "Yes" : "No",
                        UserType = context.Users.Where(us => us.UserId == a.ListedByUserId).FirstOrDefault().UserType,
                        ListingStatus = a.ListingStatus,
                        IsActive = a.IsActive,
                        BusDriver = a.Show ? "CA" : "SUS"

                    });

                }
            });
            return list;

        }

        public bool CheckHCDate(DateTime date, Guid assetId)
        {
            var context = _factory.Create();
            var assetHCO = context.AssetHCOwnership.Where(w => w.AssetId == assetId).OrderByDescending(w => w.CreateDate).FirstOrDefault();
            if (assetHCO != null && assetHCO.ActualClosingDate != null && assetHCO.ActualClosingDate >= date)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private class AssetDataRow
        {
            public int AssetIndex;
            public string AssetNumber;
            public string PropertyType;
            public int? MFUnits;
            public int? MHPSpaces;
            public int? SquareFeet;
            public double LPorCMV;
            public string AssetName;
            public string City;
            public string State;
            public int IsPublished;
            public string Status;
            public string MiscNotes;
            public int? HasListPrice;
            public int? HasCMV;
            public int? HasBothListPriceAndCMV;
            public int? IsMissingImages;
            public int? IsMissingOfferingMemorandum;
            public int? IsAssumableFinancing;
            public int? HasDeferredMaintenance;
            public string PortfolioName;
            public string ListedByUserInitials;
        }

        public byte[] GetAssetSpreadsheet()
        {


            using (var excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add("Assets");

                var headerRow = new List<string[]>()
                {
                    new string[] { "#", "ID #", "P-Type", "MF Units", "MHP Sp", "CRE Sq. Ft.", "LP or CMV", "Asset Name", "City", "State", "Pub", "Status",
                        "Misc Notes", "LP", "CMV", "Both", "M/I", "M/OM", "A-Fin", "DM/TI", "Portfolio Name", "ICA" }
                };

                // Determine the header range (e.g. A1:D1)
                string lastColumnLetter = Char.ConvertFromUtf32(headerRow[0].Length + 64);
                string headerRange = "A1:" + lastColumnLetter + "1";

                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Assets"];

                // Popular header row data
                worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                worksheet.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                worksheet.Cells[headerRange].Style.Font.Bold = true;

                // freeze header row
                worksheet.View.FreezePanes(2, 1);

                // set specific column widths
                //worksheet.Cells["A:A"].

                var context = _factory.Create();
                var assets = context.Assets.OrderBy(a => a.AssetNumber).ToList();

                int count = 0;
                var assetRows = new List<AssetDataRow>();
                List<Tuple<Guid, int>> portfolioRows = new List<Tuple<Guid, int>>();
                foreach (var asset in assets)
                {
                    count++;
                    var assetDataRow = new AssetDataRow()
                    {
                        AssetIndex = count,
                        AssetNumber = asset.AssetNumber.ToString(),
                        AssetName = asset.ProjectName,
                        City = asset.City,
                        State = asset.State,
                        Status = asset.ListingStatus == ListingStatus.Available ? "A" : asset.ListingStatus == ListingStatus.Pending ? "P" : "S",
                        PropertyType = asset.IsPaper ? "Note-" + asset.AssetTypeAbbreviationForExport : asset.AssetTypeAbbreviationForExport,
                        LPorCMV = asset.AskingPrice > 0 ? asset.AskingPrice : asset.CurrentBpo,
                        IsPublished = asset.Show ? 1 : 0
                    };

                    // there are some assets that won't have either units nor sq ft and there are some that will have both
                    if (asset.AssetType == AssetType.MultiFamily || asset.AssetType == AssetType.MHP || asset.AssetType == AssetType.FracturedCondominiumPortfolio
                        || asset.AssetType == AssetType.MixedUse || asset.IsPaper)
                    {
                        if (asset.AssetType == AssetType.MultiFamily)
                        {
                            var mfAsset = (MultiFamilyAsset)asset;
                            assetDataRow.MFUnits = mfAsset.TotalUnits;
                        }
                        else if (asset.AssetType == AssetType.MHP)
                        {
                            var mfAsset = (MultiFamilyAsset)asset;
                            assetDataRow.MHPSpaces = mfAsset.TotalUnits;
                        }
                        else
                        {
                            var cfAsset = (CommercialAsset)asset;
                            assetDataRow.MHPSpaces = cfAsset.NumberOfRentableSuites;
                        }
                    }
                    if (asset.AssetType == AssetType.Retail || asset.AssetType == AssetType.Office || asset.AssetType == AssetType.Industrial ||
                        asset.AssetType == AssetType.Medical || asset.AssetType == AssetType.ConvenienceStoreFuel || asset.AssetType == AssetType.MixedUse
                            || asset.IsPaper)
                    {
                        assetDataRow.SquareFeet = asset.SquareFeet;
                    }
                    var portfolio = (from pa in context.PortfolioAssets
                                     join p in context.Portfolios.DefaultIfEmpty() on pa.PortfolioId equals p.PortfolioId
                                     where pa.AssetId == asset.AssetId && pa.PortfolioId != null
                                     select p).FirstOrDefault();

                    if (context.AssetImages.Count(a => a.AssetId == asset.AssetId) == 0)
                    {
                        assetDataRow.IsMissingImages = 1;
                    }
                    if (context.AssetDocuments.Count(a => a.AssetId == asset.AssetId && a.Title == "Listing Agent Marketing Brochure") == 0)
                    {
                        assetDataRow.IsMissingOfferingMemorandum = 1;
                    }
                    if (portfolio != null)
                    {
                        var portfolioAssets = (from pa in context.PortfolioAssets
                                               join a in context.Assets on pa.AssetId equals a.AssetId
                                               where pa.PortfolioId == portfolio.PortfolioId
                                               orderby a.AssetNumber
                                               select a.AssetNumber).ToList();

                        portfolioRows.Add(new Tuple<Guid, int>(portfolio.PortfolioId, count + 1));
                        assetDataRow.PortfolioName = string.Format(portfolio.PortfolioName + " ({0} of {1})", portfolioAssets.IndexOf(asset.AssetNumber) + 1, portfolioAssets.Count);
                        assetDataRow.MiscNotes = portfolioAssets.IndexOf(asset.AssetNumber) == portfolioAssets.Count - 1 ? "Final asset in portfolio" : "Remaining assets in portfolio ";
                        for (int i = portfolioAssets.IndexOf(asset.AssetNumber) + 1; i < portfolioAssets.Count; i++)
                        {
                            assetDataRow.MiscNotes += portfolioAssets[i].ToString();
                            assetDataRow.MiscNotes += "; ";
                        }
                    }
                    var listedByUser = context.Users.FirstOrDefault(u => u.UserId == asset.ListedByUserId);
                    if (listedByUser != null)
                    {
                        assetDataRow.ListedByUserInitials = listedByUser.Initials;
                    }
                    if (asset.MortgageLienAssumable.HasValue)
                    {
                        assetDataRow.IsAssumableFinancing = 1;
                    }
                    if (asset.EstDeferredMaintenance.GetValueOrDefault(0) > 0)
                    {
                        assetDataRow.HasDeferredMaintenance = 1;
                    }
                    if (asset.CurrentPrincipalBalance.GetValueOrDefault(0) > 0)
                    {
                        assetDataRow.IsAssumableFinancing = 1;
                    }
                    if (assetDataRow.HasCMV.GetValueOrDefault(0) == assetDataRow.HasListPrice.GetValueOrDefault(0))
                    {
                        assetDataRow.HasBothListPriceAndCMV = 1;
                    }
                    else if (asset.AskingPrice > 0)
                    {
                        assetDataRow.HasListPrice = 1;
                    }
                    else if (asset.CurrentBpo > 0)
                    {
                        assetDataRow.HasCMV = 1;
                    }
                    assetRows.Add(assetDataRow);
                }

                // add borders to main grid
                var border = worksheet.Cells["A1:" + lastColumnLetter + (count + 1)].Style.Border.Top.Style
                    = worksheet.Cells["A1:" + lastColumnLetter + (count + 1)].Style.Border.Left.Style
                    = worksheet.Cells["A1:" + lastColumnLetter + (count + 1)].Style.Border.Right.Style
                    = worksheet.Cells["A1:" + lastColumnLetter + (count + 1)].Style.Border.Bottom.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                // small font for all
                worksheet.Cells["A:" + lastColumnLetter].Style.Font.Size = 8;

                // column widths
                worksheet.Column(1).Width = 6.55;
                worksheet.Column(2).Width = 6.55;
                worksheet.Column(3).Width = 6.55;
                worksheet.Column(4).Width = 6.55;
                worksheet.Column(5).Width = 6.55;
                worksheet.Column(6).Width = 10.65;
                worksheet.Column(7).Width = 13.35;
                worksheet.Column(8).Width = 36;
                worksheet.Column(9).Width = 13;
                worksheet.Column(10).Width = 5.65;
                worksheet.Column(11).Width = 5.65;
                worksheet.Column(12).Width = 5.65;
                worksheet.Column(13).Width = 10;
                worksheet.Column(14).Width = 4.35;
                worksheet.Column(15).Width = 4.35;
                worksheet.Column(16).Width = 4.35;
                worksheet.Column(17).Width = 4.35;
                worksheet.Column(18).Width = 4.35;
                worksheet.Column(19).Width = 4.35;
                worksheet.Column(20).Width = 4.35;
                worksheet.Column(21).Width = 10;
                worksheet.Column(22).Width = 4.35;


                worksheet.Cells[2, 1].LoadFromCollection(assetRows.Select(d => new
                {
                    d.AssetIndex,
                    d.AssetNumber,
                    d.PropertyType,
                    d.MFUnits,
                    d.MHPSpaces,
                    d.SquareFeet,
                    d.LPorCMV,
                    d.AssetName,
                    d.City,
                    d.State,
                    d.IsPublished,
                    d.Status,
                    d.MiscNotes,
                    d.HasListPrice,
                    d.HasCMV,
                    d.HasBothListPriceAndCMV,
                    d.IsMissingImages,
                    d.IsMissingOfferingMemorandum,
                    d.IsAssumableFinancing,
                    d.HasDeferredMaintenance,
                    d.PortfolioName,
                    d.ListedByUserInitials
                }), false); //stupid bug with LoadFromCollection makes me do this

                var newRowIndex = count + 2;
                var totalRowCount = count + 1;

                // summations and % of rows
                worksheet.Cells[newRowIndex, 4].Formula = "SUM(D1:" + "D" + totalRowCount + ")";
                worksheet.Cells[newRowIndex, 5].Formula = "SUM(E1:" + "E" + totalRowCount + ")";
                worksheet.Cells[newRowIndex, 6].Formula = "SUM(F1:" + "F" + totalRowCount + ")";
                worksheet.Cells[newRowIndex, 7].Formula = "SUM(G1:" + "G" + totalRowCount + ")";
                worksheet.Cells[newRowIndex, 4, newRowIndex, 21].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 4, newRowIndex, 7].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 4, newRowIndex, 7].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 4, newRowIndex, 7].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 4, newRowIndex, 7].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                // sum row
                worksheet.Cells[newRowIndex, 11].Formula = "SUM(K1:" + "K" + totalRowCount + ")";
                worksheet.Cells[newRowIndex, 11].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 11].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 11].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 11].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[newRowIndex, 14].Formula = "SUM(N1:" + "N" + totalRowCount + ")";
                worksheet.Cells[newRowIndex, 15].Formula = "SUM(O1:" + "O" + totalRowCount + ")";
                worksheet.Cells[newRowIndex, 16].Formula = "SUM(P1:" + "P" + totalRowCount + ")";
                worksheet.Cells[newRowIndex, 17].Formula = "SUM(Q1:" + "Q" + totalRowCount + ")";
                worksheet.Cells[newRowIndex, 18].Formula = "SUM(R1:" + "R" + totalRowCount + ")";
                worksheet.Cells[newRowIndex, 19].Formula = "SUM(S1:" + "S" + totalRowCount + ")";
                worksheet.Cells[newRowIndex, 20].Formula = "SUM(T1:" + "T" + totalRowCount + ")";
                worksheet.Cells[newRowIndex, 14, newRowIndex, 20].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 14, newRowIndex, 20].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 14, newRowIndex, 20].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 14, newRowIndex, 20].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                // averages and % of db row
                newRowIndex++;
                worksheet.Cells[newRowIndex, 7].Formula = "AVERAGE(G1:" + "G" + totalRowCount + ")";
                worksheet.Cells[newRowIndex, 7].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[newRowIndex, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);
                worksheet.Cells[newRowIndex, 7].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 7].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 7].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 7].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[newRowIndex, 8].Value = "<- Average Asset Value in db";
                worksheet.Cells[newRowIndex, 8].Style.Font.Bold = true;

                // % of
                worksheet.Cells[newRowIndex, 11].Formula = "SUM(K" + (newRowIndex - 1) + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 11].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 11].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 11].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 11].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[newRowIndex, 14].Formula = "SUM(N" + (newRowIndex - 1) + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 15].Formula = "SUM(O" + (newRowIndex - 1) + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 16].Formula = "SUM(P" + (newRowIndex - 1) + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 17].Formula = "SUM(Q" + (newRowIndex - 1) + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 18].Formula = "SUM(R" + (newRowIndex - 1) + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 19].Formula = "SUM(S" + (newRowIndex - 1) + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 20].Formula = "SUM(T" + (newRowIndex - 1) + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 14, newRowIndex, 20].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 14, newRowIndex, 20].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 14, newRowIndex, 20].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 14, newRowIndex, 20].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                // helper desc row
                newRowIndex++;
                worksheet.Cells[newRowIndex, 14, newRowIndex, 20].Merge = true;
                worksheet.Cells[newRowIndex, 14, newRowIndex, 20].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[newRowIndex, 14, newRowIndex, 20].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[newRowIndex, 14, newRowIndex, 20].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                worksheet.Cells[newRowIndex, 14, newRowIndex, 20].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 14, newRowIndex, 20].Value = "↑ As Percentage of Total Assets in Master db ↑";
                worksheet.Cells[newRowIndex, 14, newRowIndex, 20].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 14, newRowIndex, 20].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 14, newRowIndex, 20].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 14, newRowIndex, 20].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                // styling and number formats
                worksheet.Cells["A:" + lastColumnLetter].Style.Numberformat.Format = "#,##0"; // number format for number of units and sq ft
                worksheet.Cells["G:G"].Style.Numberformat.Format = "$#,##0"; // currency format for LP or CMV
                worksheet.Cells["N:T"].Style.Numberformat.Format = "0";
                worksheet.Cells["A:" + lastColumnLetter].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[newRowIndex - 1, 11].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex - 1, 14].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex - 1, 15].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex - 1, 16].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex - 1, 17].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex - 1, 18].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex - 1, 19].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex - 1, 20].Style.Numberformat.Format = "0%";

                // add colors to portfolios and bold $ and asset name
                var groupedByPortfolioRows = portfolioRows.GroupBy(g => g.Item1);
                var random = new Random();
                foreach (var group in groupedByPortfolioRows)
                {
                    // change color by group -- pick lighter colors
                    var color = Color.FromArgb(random.Next(200, 255), random.Next(150, 255), random.Next(150, 255));
                    foreach (var row in group)
                    {
                        worksheet.Cells[row.Item2, 7, row.Item2, 8].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[row.Item2, 7, row.Item2, 8].Style.Fill.BackgroundColor.SetColor(color);
                        worksheet.Cells[row.Item2, 7, row.Item2, 8].Style.Font.Bold = true;
                    }
                }

                // color formatting for A/P/S
                string availableStatusAddresses = string.Join(",", assetRows.Where(a => a.Status == "A").Select(a => "L" + (a.AssetIndex + 1)));
                string pendingStatusAddresses = string.Join(",", assetRows.Where(a => a.Status == "P").Select(a => "L" + (a.AssetIndex + 1)));
                string soldStatusAddresses = string.Join(",", assetRows.Where(a => a.Status == "S").Select(a => "L" + (a.AssetIndex + 1)));
                worksheet.Cells["L2:L" + totalRowCount + 1].Style.Font.Bold = true;
                worksheet.Cells[availableStatusAddresses].Style.Font.Color.SetColor(System.Drawing.Color.Green);
                worksheet.Cells[pendingStatusAddresses].Style.Font.Color.SetColor(System.Drawing.Color.Blue);
                worksheet.Cells[soldStatusAddresses].Style.Font.Color.SetColor(System.Drawing.Color.Red);

                // replace 0 values with -
                string mfUnitZeroAddresses = string.Join(",", assetRows.Where(a => a.MFUnits.GetValueOrDefault(0) == 0).Select(a => "D" + (a.AssetIndex + 1)));
                string mhpSpacesZeroAddresses = string.Join(",", assetRows.Where(a => a.MHPSpaces.GetValueOrDefault(0) == 0).Select(a => "E" + (a.AssetIndex + 1)));
                string squareFeetZeroAddresses = string.Join(",", assetRows.Where(a => a.SquareFeet.GetValueOrDefault(0) == 0).Select(a => "F" + (a.AssetIndex + 1)));
                string publishedZeroAddresses = string.Join(",", assetRows.Where(a => a.IsPublished == 0).Select(a => "K" + (a.AssetIndex + 1)));
                worksheet.Cells[mfUnitZeroAddresses].Value = "-";
                worksheet.Cells[mhpSpacesZeroAddresses].Value = "-";
                worksheet.Cells[squareFeetZeroAddresses].Value = "-";
                worksheet.Cells[publishedZeroAddresses].Value = "-";


                // legend rows
                newRowIndex++;
                worksheet.Cells[newRowIndex, 2].Value = "Legend:";
                worksheet.Cells[newRowIndex, 2].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 2].Style.Font.Color.SetColor(System.Drawing.Color.Red);

                newRowIndex++;
                worksheet.Cells[newRowIndex, 2].Value = "MF";
                worksheet.Cells[newRowIndex, 2].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[newRowIndex, 2].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 3].Value = "Multi-Family";
                var mfAssetCount = assets.Count(a => a.AssetType == AssetType.MultiFamily);
                worksheet.Cells[newRowIndex, 9].Value = mfAssetCount;
                worksheet.Cells[newRowIndex, 10].Value = "MF";
                worksheet.Cells[newRowIndex, 11].Formula = "SUM(I" + newRowIndex + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 11].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                // available section
                worksheet.Cells[newRowIndex, 13].Value = assets.Count(a => a.ListingStatus == ListingStatus.Available);
                worksheet.Cells[newRowIndex, 14].Value = "A";
                worksheet.Cells[newRowIndex, 14].Style.Font.Color.SetColor(System.Drawing.Color.Green);
                worksheet.Cells[newRowIndex, 15].Formula = "SUM(M" + newRowIndex + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 15, newRowIndex + 3, 15].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex, 13, newRowIndex, 15].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 13, newRowIndex, 15].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 13, newRowIndex, 15].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 13, newRowIndex, 15].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 13, newRowIndex, 15].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                newRowIndex++;
                worksheet.Cells[newRowIndex, 2].Value = "FCP";
                worksheet.Cells[newRowIndex, 2].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[newRowIndex, 2].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 3].Value = "Fractured Condo Portfolio";
                var fcpAssetCount = assets.Count(a => a.AssetType == AssetType.FracturedCondominiumPortfolio);
                worksheet.Cells[newRowIndex, 9].Value = fcpAssetCount;
                worksheet.Cells[newRowIndex, 10].Value = "FCP";
                worksheet.Cells[newRowIndex, 11].Formula = "SUM(I" + newRowIndex + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 11].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                // pending section
                worksheet.Cells[newRowIndex, 13].Value = assets.Count(a => a.ListingStatus == ListingStatus.Pending);
                worksheet.Cells[newRowIndex, 14].Value = "P";
                worksheet.Cells[newRowIndex, 14].Style.Font.Color.SetColor(System.Drawing.Color.Blue);
                worksheet.Cells[newRowIndex, 15].Formula = "SUM(M" + newRowIndex + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 13, newRowIndex, 15].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 13, newRowIndex, 15].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 13, newRowIndex, 15].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 13, newRowIndex, 15].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 13, newRowIndex, 15].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                newRowIndex++;
                worksheet.Cells[newRowIndex, 2].Value = "MHP";
                worksheet.Cells[newRowIndex, 2].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[newRowIndex, 2].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 3].Value = "Mobile Home Parks";
                var mhpAssetCount = assets.Count(a => a.AssetType == AssetType.MHP);
                worksheet.Cells[newRowIndex, 9].Value = fcpAssetCount;
                worksheet.Cells[newRowIndex, 10].Value = "MHP";
                worksheet.Cells[newRowIndex, 11].Formula = "SUM(I" + newRowIndex + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 11].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                // sold section
                worksheet.Cells[newRowIndex, 13].Value = assets.Count(a => a.ListingStatus == ListingStatus.SoldAndClosed || a.ListingStatus == ListingStatus.SoldNotClosed);
                worksheet.Cells[newRowIndex, 14].Value = "S";
                worksheet.Cells[newRowIndex, 14].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                worksheet.Cells[newRowIndex, 15].Formula = "SUM(M" + newRowIndex + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 13, newRowIndex, 15].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 13, newRowIndex, 15].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 13, newRowIndex, 15].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 13, newRowIndex, 15].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 13, newRowIndex, 15].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                newRowIndex++;
                worksheet.Cells[newRowIndex, 2].Value = "Ret";
                worksheet.Cells[newRowIndex, 2].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[newRowIndex, 2].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 3].Value = "Retail Tenant";
                var retailAssetCount = assets.Count(a => a.AssetType == AssetType.Retail);
                worksheet.Cells[newRowIndex, 9].Value = retailAssetCount;
                worksheet.Cells[newRowIndex, 10].Value = "Ret";
                worksheet.Cells[newRowIndex, 11].Formula = "SUM(I" + newRowIndex + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 11].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                newRowIndex++;
                worksheet.Cells[newRowIndex, 2].Value = "Off";
                worksheet.Cells[newRowIndex, 2].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[newRowIndex, 2].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 3].Value = "Office Tenant";
                var officeAssetCount = assets.Count(a => a.AssetType == AssetType.Office);
                worksheet.Cells[newRowIndex, 9].Value = officeAssetCount;
                worksheet.Cells[newRowIndex, 10].Value = "Off";
                worksheet.Cells[newRowIndex, 11].Formula = "SUM(I" + newRowIndex + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 11].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                newRowIndex++;
                worksheet.Cells[newRowIndex, 2].Value = "Indus";
                worksheet.Cells[newRowIndex, 2].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[newRowIndex, 2].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 3].Value = "Industrial Tenant";
                var industrialAssetCount = assets.Count(a => a.AssetType == AssetType.Industrial);
                worksheet.Cells[newRowIndex, 9].Value = industrialAssetCount;
                worksheet.Cells[newRowIndex, 10].Value = "Indus";
                worksheet.Cells[newRowIndex, 11].Formula = "SUM(I" + newRowIndex + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 11].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                newRowIndex++;
                worksheet.Cells[newRowIndex, 2].Value = "Med";
                worksheet.Cells[newRowIndex, 2].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[newRowIndex, 2].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 3].Value = "Medical Office Tenant";
                var medicalAssetCount = assets.Count(a => a.AssetType == AssetType.Medical);
                worksheet.Cells[newRowIndex, 9].Value = medicalAssetCount;
                worksheet.Cells[newRowIndex, 10].Value = "Med";
                worksheet.Cells[newRowIndex, 11].Formula = "SUM(I" + newRowIndex + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 11].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                newRowIndex++;
                worksheet.Cells[newRowIndex, 2].Value = "F/S";
                worksheet.Cells[newRowIndex, 2].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[newRowIndex, 2].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 3].Value = "Fuel Service";
                var fuelServiceAssetCount = assets.Count(a => a.AssetType == AssetType.ConvenienceStoreFuel);
                worksheet.Cells[newRowIndex, 9].Value = fuelServiceAssetCount;
                worksheet.Cells[newRowIndex, 10].Value = "F/S";
                worksheet.Cells[newRowIndex, 11].Formula = "SUM(I" + newRowIndex + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 11].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                newRowIndex++;
                worksheet.Cells[newRowIndex, 2].Value = "R/H/M";
                worksheet.Cells[newRowIndex, 2].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[newRowIndex, 2].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 3].Value = "Resort/Hotel/Motel Properties";
                var rhmAssetCount = assets.Count(a => a.AssetType == AssetType.Hotel);
                worksheet.Cells[newRowIndex, 9].Value = rhmAssetCount;
                worksheet.Cells[newRowIndex, 10].Value = "R/H/M";
                worksheet.Cells[newRowIndex, 11].Formula = "SUM(I" + newRowIndex + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 11].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                newRowIndex++;
                worksheet.Cells[newRowIndex, 2].Value = "PG";
                worksheet.Cells[newRowIndex, 2].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[newRowIndex, 2].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 3].Value = "Parking Garage Properties";
                var pgAssetCount = assets.Count(a => a.AssetType == AssetType.ParkingGarageProperty);
                worksheet.Cells[newRowIndex, 9].Value = pgAssetCount;
                worksheet.Cells[newRowIndex, 10].Value = "PG";
                worksheet.Cells[newRowIndex, 11].Formula = "SUM(I" + newRowIndex + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 11].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                newRowIndex++;
                worksheet.Cells[newRowIndex, 2].Value = "MS";
                worksheet.Cells[newRowIndex, 2].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[newRowIndex, 2].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 3].Value = "Mini-Storage Properties";
                var msAssetCount = assets.Count(a => a.AssetType == AssetType.MiniStorageProperty);
                worksheet.Cells[newRowIndex, 9].Value = msAssetCount;
                worksheet.Cells[newRowIndex, 10].Value = "MS";
                worksheet.Cells[newRowIndex, 11].Formula = "SUM(I" + newRowIndex + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 11].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                newRowIndex++;
                worksheet.Cells[newRowIndex, 2].Value = "Mix";
                worksheet.Cells[newRowIndex, 2].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[newRowIndex, 2].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 3].Value = "Mixed Use Assets (e.g., has Retail, Office, MF, FCP, other tenants)";
                var mixAssetCount = assets.Count(a => a.AssetType == AssetType.MixedUse);
                worksheet.Cells[newRowIndex, 9].Value = mixAssetCount;
                worksheet.Cells[newRowIndex, 10].Value = "Mix";
                worksheet.Cells[newRowIndex, 11].Formula = "SUM(I" + newRowIndex + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 11].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                newRowIndex++;
                worksheet.Cells[newRowIndex, 2].Value = "Land";
                worksheet.Cells[newRowIndex, 2].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[newRowIndex, 2].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 3].Value = "Land: all types";
                var landAssetCount = assets.Count(a => a.AssetType == AssetType.Land);
                worksheet.Cells[newRowIndex, 9].Value = landAssetCount;
                worksheet.Cells[newRowIndex, 10].Value = "Land";
                worksheet.Cells[newRowIndex, 11].Formula = "SUM(I" + newRowIndex + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 11].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                newRowIndex++;
                worksheet.Cells[newRowIndex, 2].Value = "Note";
                worksheet.Cells[newRowIndex, 2].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 2].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[newRowIndex, 3].Value = "Notes secured by any of the Asset Types defined (e.g., Note-MF, Note-Retail, etc.)";
                var noteAssetCount = assets.Count(a => a.IsPaper);
                worksheet.Cells[newRowIndex, 9].Value = noteAssetCount;
                worksheet.Cells[newRowIndex, 10].Value = "Note";
                worksheet.Cells[newRowIndex, 11].Formula = "SUM(I" + newRowIndex + "/A" + (count + 1) + ")";
                worksheet.Cells[newRowIndex, 11].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 9, newRowIndex, 11].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                newRowIndex++;
                worksheet.Cells[newRowIndex, 9].Formula = "SUM(I" + (newRowIndex - 14) + ":I" + (newRowIndex - 1) + ")";
                worksheet.Cells[newRowIndex, 9].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[newRowIndex, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                worksheet.Cells[newRowIndex, 11].Formula = "SUM(K" + (newRowIndex - 14) + ":K" + (newRowIndex - 1) + ")";
                worksheet.Cells[newRowIndex, 11].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 11].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[newRowIndex, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                worksheet.Cells[newRowIndex, 9].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 11].Style.Numberformat.Format = "0%";
                worksheet.Cells[newRowIndex, 9].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 9].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 9].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 9].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[newRowIndex, 11].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 11].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 11].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 11].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                newRowIndex++;
                newRowIndex++;
                worksheet.Cells[newRowIndex, 2].Value = "A";
                worksheet.Cells[newRowIndex, 3].Value = "Available";
                worksheet.Cells[newRowIndex, 2, newRowIndex, 3].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 2, newRowIndex, 3].Style.Font.Color.SetColor(System.Drawing.Color.Green);
                worksheet.Cells[newRowIndex, 2].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                newRowIndex++;
                worksheet.Cells[newRowIndex, 2].Value = "P";
                worksheet.Cells[newRowIndex, 3].Value = "Pending";

                worksheet.Cells[newRowIndex, 2, newRowIndex, 3].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 2, newRowIndex, 3].Style.Font.Color.SetColor(System.Drawing.Color.Blue);
                worksheet.Cells[newRowIndex, 2].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                newRowIndex++;
                worksheet.Cells[newRowIndex, 2].Value = "S";
                worksheet.Cells[newRowIndex, 3].Value = "Sold";

                worksheet.Cells[newRowIndex, 2, newRowIndex, 3].Style.Font.Bold = true;
                worksheet.Cells[newRowIndex, 2, newRowIndex, 3].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                worksheet.Cells[newRowIndex, 2].Style.Border.Top.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Left.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Right.Style =
                    worksheet.Cells[newRowIndex, 2].Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                worksheet.Cells[count + 4, 2, newRowIndex, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Calculate();

                return excel.GetAsByteArray();
            }
        }

        public List<AssetHCOwnershipModel> GetAssetHCByAssetId(Guid assetId)
        {
            var context = _factory.Create();

            var assetHCOwnership = context.AssetHCOwnership
                                    .Where(s => s.AssetId == assetId)
                                    .OrderByDescending(s => s.CreateDate).Select(a => new AssetHCOwnershipModel
                                    {
                                        AssetHCOwnershipId = a.AssetHCOwnershipId,
                                        AssetId = a.AssetId,
                                        HoldingCompanyId = a.HoldingCompanyId,
                                        CreateDate = a.CreateDate,
                                        Terms = a.Terms,
                                        ActualClosingDate = a.ActualClosingDate,
                                        SalesPrice = a.SalesPrice,
                                        SalesPriceNotProvided = a.SalesPriceNotProvided,
                                        CalculatedPPU = a.CalculatedPPU,
                                        CapRate = a.CapRate,
                                        CalculatedPPSqFt = a.CalculatedPPSqFt,
                                        CashInvestmentApy = a.CashInvestmentApy,
                                        TermsOther = a.TermsOther,
                                        OwnerHoldingCompany = a.HoldingCompany.CompanyName
                                    }).ToList();
            return assetHCOwnership;
        }

        public void SaveUpdateAssetHC(AssetHCOwnership assetHC)
        {
            if (assetHC != null)
            {
                var context = _factory.Create();
                if (assetHC.AssetHCOwnershipId > 0)
                {
                    var assetHCO = context.AssetHCOwnership.Where(x => x.AssetHCOwnershipId == assetHC.AssetHCOwnershipId).FirstOrDefault();
                    if (assetHCO != null)
                    {
                        //Only related data need to update 
                        assetHCO.Terms = assetHC.Terms;
                        assetHCO.TermsOther = assetHC.TermsOther;
                        assetHCO.ActualClosingDate = assetHC.ActualClosingDate;
                        assetHCO.SalesPrice = assetHC.SalesPrice;
                        assetHCO.SalesPriceNotProvided = assetHC.SalesPriceNotProvided;
                        assetHCO.CalculatedPPU = assetHC.CalculatedPPU;
                        assetHCO.CalculatedPPSqFt = assetHC.CalculatedPPSqFt;
                        assetHCO.CashInvestmentApy = assetHC.CashInvestmentApy;
                        assetHCO.HoldingCompanyId = assetHC.HoldingCompanyId;
                        assetHCO.CapRate = assetHC.CapRate;

                        context.Entry(assetHCO).State = EntityState.Modified;
                        context.Save();
                    }
                }
                else
                {
                    context.AssetHCOwnership.Add(assetHC);
                    context.Save();
                }
            }
        }
        public AssetDynamicViewModel SearchAssetsForSearch(SearchAssetModel searchModel, int? userId)
        {
            var context = _factory.Create();
            var queryBuilder = new List<string>();
            var model = new AssetDynamicViewModel();
            // search wont initiate until iteration
            #region Filtering
            var dbEntities = context.Assets.Where(a => a.IsActive/* && a.IsPublished == true*/);// && a.Latitude.HasValue && a.Longitude.HasValue);

            //check search bar input (it could be address, asset name, asset id)
            if (!string.IsNullOrWhiteSpace(searchModel.SearchBarInput))
            {
                //create a union holder
                List<IQueryable<Asset>> searchBarSearches = new List<IQueryable<Asset>>();

                //asset id 
                Guid aId = Guid.Empty;
                if (Guid.TryParse(searchModel.SearchBarInput, out aId))
                {
                    var idSearch = dbEntities.Where(d => d.AssetId == aId);
                    searchBarSearches.Add(idSearch);
                }
                else
                {
                    //name search 
                    //assume if there is no comma, that's an asset name search
                    if (searchModel.SearchBarInput.IndexOf(",") == -1)
                    {
                        searchModel.AssetName = searchModel.SearchBarInput; //<-- this will be searched later
                    }
                    else
                    {
                        //TODO: asset number search?
                    }
                }


                if (searchBarSearches.Count() > 0)
                {
                    var unionSearch = searchBarSearches[0];
                    for (int i = 1; i < searchBarSearches.Count; i++)
                    {
                        var search = searchBarSearches[i];
                        unionSearch = unionSearch.Union(search);
                    }

                    dbEntities = unionSearch.AsQueryable();
                }
            }



            // AssetTypes trump AssetType
            List<AssetType> selectedAssetTypes = new List<AssetType>();
            List<AssetSubType> selectedAssetSubTypes = new List<AssetSubType>();

            // Generate list of asset types to filter by
            if (searchModel.AssetTypes != null && searchModel.AssetTypes.Count > 0)
            {
                foreach (var atInt in searchModel.AssetTypes)
                {
                    // not sure why we need a try/catch clause here but leaving this for now
                    try
                    {
                        var assetType = (AssetType)Enum.ToObject(typeof(AssetType), atInt);
                        selectedAssetTypes.Add(assetType);
                    }
                    catch { }
                }
            }

            // Generate list of asset sub types to filer by
            if (searchModel.AssetSubTypes != null && searchModel.AssetSubTypes.Count > 0)
            {
                foreach (var atInt in searchModel.AssetSubTypes)
                {
                    // not sure why we need a try/catch clause here but leaving this for now
                    try
                    {
                        var assetSubType = (AssetSubType)Enum.ToObject(typeof(AssetSubType), atInt);
                        selectedAssetSubTypes.Add(assetSubType);
                    }
                    catch { }
                }
            }
            // perform asset filters for both asset types and asset subtypes
            if (selectedAssetTypes.Count > 0 || selectedAssetSubTypes.Count > 0)
            {
                var isPaper = selectedAssetTypes.Contains(AssetType.SecuredPaper);
                dbEntities = dbEntities.AddSearchParameter(true, a => selectedAssetTypes.Contains(a.AssetType)
                    //|| selectedAssetSubTypes.Contains(a.AssetSubType.Value) -- ToDo: Assest Sub Type need to check
                    || isPaper && a.IsPaper);
            }

            if (searchModel.GradeClassifications != null && searchModel.GradeClassifications.Count > 0)
            {
                // since an enum doesnt exist, create a list to validate against. Of course that means multiple places to update if it changes(shouldnt change much)
                var gcArr = new List<string>() { "A+", "A", "A-", "B+", "B", "B-", "C+", "C", "C-", "D+", "D" };
                var validatedGradeClassifications = new List<string>();
                foreach (var gc in searchModel.GradeClassifications) { if (gcArr.Contains(gc)) validatedGradeClassifications.Add(gc); }
                if (validatedGradeClassifications.Count == 1)
                {
                    var gc = validatedGradeClassifications.First();
                    dbEntities = dbEntities.AddSearchParameter(true, a => a.GradeClassification == gc);
                }
                else if (validatedGradeClassifications.Count > 0) { dbEntities = dbEntities.AddSearchParameter(true, a => validatedGradeClassifications.Contains(a.GradeClassification)); }
            }

            if (searchModel.OperatingStatus.HasValue && Enum.IsDefined(typeof(OperatingStatus), searchModel.OperatingStatus.Value))
            {
                try
                {
                    var operatingStatus = (OperatingStatus)Enum.ToObject(typeof(OperatingStatus), searchModel.OperatingStatus.Value);
                    dbEntities = dbEntities.AddSearchParameter(true, a => a.OperatingStatus == operatingStatus);
                }
                catch { }
            }
            if (!string.IsNullOrEmpty(searchModel.ListingType))
            {
                // Listed Price vs Auction ddl. No representation in our stack, I think
                // TODO: learn what auction means/how to determine if an asset is up for auction
                if (searchModel.ListingType == "listedPrice") { dbEntities = dbEntities.AddSearchParameter(true, a => a.AskingPrice > 0); } // might be missing auction exlusion
                else if (searchModel.ListingType == "action")
                {
                    // dont know yet
                }
            }

            // Checking the VacancyFactor parameter
            if (searchModel.VacancyFactor != null && searchModel.VacancyFactor.Count > 0)
            {
                // TODO: Should make this whole thing a function later on for multi level filtering as we will be utilizing multi level filtering numerous times
                // assign the raw db entities before filters

                dbEntities = from a in dbEntities
                             where (searchModel.VacancyFactor.Contains(0)) && (a.CurrentVacancyFac < 10)
                               || (searchModel.VacancyFactor.Contains(10)) && ((a.CurrentVacancyFac >= 10) && (a.CurrentVacancyFac < 15))
                               || (searchModel.VacancyFactor.Contains(15)) && ((a.CurrentVacancyFac >= 15) && (a.CurrentVacancyFac < 20))
                               || (searchModel.VacancyFactor.Contains(20)) && ((a.CurrentVacancyFac >= 20) && (a.CurrentVacancyFac < 30))
                               || (searchModel.VacancyFactor.Contains(30)) && ((a.CurrentVacancyFac >= 30) && (a.CurrentVacancyFac < 40))
                               || (searchModel.VacancyFactor.Contains(40)) && (a.CurrentVacancyFac >= 40)
                             select a;
            }

            // Checking the RentableSpace parameter
            if (searchModel.RentableSpace != null && searchModel.RentableSpace.Count > 0)
            {
                dbEntities = from a in dbEntities
                             where (searchModel.RentableSpace.Contains(0)) && (a.NumberRentableSpace <= 99)
                               || (searchModel.RentableSpace.Contains(100)) && ((a.NumberRentableSpace >= 100) && (a.NumberRentableSpace <= 199))
                               || (searchModel.RentableSpace.Contains(200)) && ((a.NumberRentableSpace >= 200) && (a.NumberRentableSpace <= 399))
                               || (searchModel.RentableSpace.Contains(400)) && (a.NumberRentableSpace >= 400)
                             select a;
            }

            // City, state, county
            dbEntities = dbEntities.AddSearchParameter(searchModel.City != null, x => x.City.Contains(searchModel.City.Trim()));
            dbEntities = dbEntities.AddSearchParameter(searchModel.State != null, x => x.State.Contains(searchModel.State.Trim()));
            dbEntities = dbEntities.AddSearchParameter(searchModel.County != null, x => x.County.Contains(searchModel.County.ToLower().Replace("county", "").Trim()));

            // Sam - Modified the min/max calculation to take into account whether to use BPO or Asking Price
            if (searchModel.PriceSearchType != null)
            {
                switch (searchModel.PriceSearchType)
                {
                    case 1: // listing price
                        dbEntities = dbEntities.AddSearchParameter(searchModel.Min.HasValue && searchModel.Min.Value > 0, a => (a.AskingPrice >= searchModel.Min.Value));
                        dbEntities = dbEntities.AddSearchParameter(searchModel.Max.HasValue && searchModel.Max.Value > 0, a => (a.AskingPrice <= searchModel.Max.Value));
                        break;
                    case 2: // BPO
                        dbEntities = dbEntities.AddSearchParameter(searchModel.Min.HasValue && searchModel.Min.Value > 0, a => (a.CurrentBpo >= searchModel.Min.Value));
                        dbEntities = dbEntities.AddSearchParameter(searchModel.Max.HasValue && searchModel.Max.Value > 0, a => (a.CurrentBpo <= searchModel.Max.Value));
                        break;
                    case 3: // Both
                        dbEntities = dbEntities.AddSearchParameter(searchModel.Min.HasValue && searchModel.Min.Value > 0, a => (a.CurrentBpo >= searchModel.Min.Value) || (a.AskingPrice >= searchModel.Min.Value));
                        dbEntities = dbEntities.AddSearchParameter(searchModel.Max.HasValue && searchModel.Max.Value > 0, a => (a.CurrentBpo <= searchModel.Max.Value) || (a.AskingPrice <= searchModel.Max.Value));
                        break;
                }
            }
            else
            {
                // Since there's no price search type selected, filter based on listing price if there is a min or a max value defined
                dbEntities = dbEntities.AddSearchParameter(searchModel.Min.HasValue && searchModel.Min.Value > 0, a => (a.AskingPrice >= searchModel.Min.Value));
                dbEntities = dbEntities.AddSearchParameter(searchModel.Max.HasValue && searchModel.Max.Value > 0, a => (a.AskingPrice <= searchModel.Max.Value));
            }
            dbEntities = dbEntities.AddSearchParameter(searchModel.CapRate.HasValue && searchModel.CapRate.Value > 0, a => a.CashInvestmentApy >= searchModel.CapRate.Value);
            dbEntities = dbEntities.AddSearchParameter(searchModel.YearBuilt.HasValue && searchModel.YearBuilt > 0, a => a.YearBuilt >= searchModel.YearBuilt.Value);
            dbEntities = dbEntities.AddSearchParameter(searchModel.UnitsMin.HasValue && searchModel.UnitsMin.Value > 0, a => (a as MultiFamilyAsset).TotalUnits >= searchModel.UnitsMin.Value);
            dbEntities = dbEntities.AddSearchParameter(searchModel.UnitsMax.HasValue && searchModel.UnitsMax.Value > 0, a => (a as MultiFamilyAsset).TotalUnits <= searchModel.UnitsMax.Value);

            // Sam - Addition of dbEntities
            dbEntities = dbEntities.AddSearchParameter(searchModel.SquareFeetMin.HasValue && searchModel.SquareFeetMin.Value > 0, a => a.SquareFeet >= searchModel.SquareFeetMin.Value);
            dbEntities = dbEntities.AddSearchParameter(searchModel.SquareFeetMax.HasValue && searchModel.SquareFeetMax.Value > 0, a => a.SquareFeet <= searchModel.SquareFeetMax.Value);
            dbEntities = dbEntities.AddSearchParameter(searchModel.NOI.HasValue && searchModel.NOI.Value > 0, a => (a as CommercialAsset).ProformaAnnualNoi >= searchModel.NOI.Value);
            dbEntities = dbEntities.AddSearchParameter(searchModel.SGI.HasValue && searchModel.SGI.Value > 0, a => (a as CommercialAsset).ProformaSgi >= searchModel.SGI.Value);
            dbEntities = dbEntities.AddSearchParameter(searchModel.OccPerc.HasValue && searchModel.OccPerc.Value > 0, a => (a as CommercialAsset).OccupancyPercentage >= searchModel.OccPerc.Value);


            // Min year updated (not sure if correct column)
            dbEntities = dbEntities.AddSearchParameter(searchModel.IsUpdatedChecked.HasValue && searchModel.IsUpdatedChecked.Value
                && searchModel.YearUpdated.HasValue && searchModel.YearUpdated > 0,
                a => a.PropLastUpdated.HasValue && a.PropLastUpdated.Value.Year > searchModel.YearUpdated.Value);

            // Updated/Renovated by owner
            dbEntities = dbEntities.AddSearchParameter(searchModel.IsUpdatedChecked.HasValue && searchModel.IsUpdatedChecked.Value
                && searchModel.IsUpdatedByOwner.HasValue && searchModel.IsUpdatedByOwner.Value,
                a => a.RenovatedByOwner == true);

            // Major Tenant
            dbEntities = dbEntities.AddSearchParameter(!string.IsNullOrWhiteSpace(searchModel.MajorTenant),
                a => (a as CommercialAsset).NameOfAAARatedMajorTenant.Contains(searchModel.MajorTenant));

            // Major Tenant Occ Percent
            // Assuming that major tenant occ percent is relative to total sq feet
            dbEntities = dbEntities.AddSearchParameter(searchModel.MajorTenantOccuPerc.HasValue && searchModel.MajorTenantOccuPerc > 0,
                a => a.SquareFeet > 0 && ((a as CommercialAsset).LeasedSquareFootageByMajorTenant / a.SquareFeet) * 100 > searchModel.MajorTenantOccuPerc.Value);

            // Major Tenant Lease Expiration (not sure if correct column)
            // Also not sure if it's supposed to be a dropdown of checkboxes 
            dbEntities = dbEntities.AddSearchParameter(searchModel.MajorTenantLeaseExp.HasValue && searchModel.MajorTenantLeaseExp.Value > 0,
                a => a.LeaseholdMaturityDate.HasValue && a.LeaseholdMaturityDate.Value.Year > searchModel.MajorTenantLeaseExp.Value);

            // UMR Utilities
            //InteriorRoadType
            dbEntities = dbEntities.AddSearchParameter(searchModel.InteriorRoadTypes != null && searchModel.InteriorRoadTypes.Count > 0,
                a => a.InteriorRoadTypeId.HasValue && searchModel.InteriorRoadTypes.Contains((int)a.InteriorRoadTypeId.Value));
            //AccessRoadType
            dbEntities = dbEntities.AddSearchParameter(searchModel.AccessRoadTypes != null && searchModel.AccessRoadTypes.Count > 0,
                a => a.AccessRoadTypeId.HasValue && searchModel.AccessRoadTypes.Contains((int)a.AccessRoadTypeId.Value));
            //WasteWaterType
            dbEntities = dbEntities.AddSearchParameter(searchModel.WasteWaterTypes != null && searchModel.WasteWaterTypes.Count > 0,
                a => a.WasteWaterTypeId.HasValue && searchModel.WasteWaterTypes.Contains((int)a.WasteWaterTypeId.Value));
            //WaterServType
            dbEntities = dbEntities.AddSearchParameter(searchModel.WaterServTypes != null && searchModel.WaterServTypes.Count > 0,
                a => a.WaterServTypeId.HasValue && searchModel.WaterServTypes.Contains((int)a.WaterServTypeId.Value));
            //GasMeteringMethod
            dbEntities = dbEntities.AddSearchParameter(searchModel.GasMeteringMethods != null && searchModel.GasMeteringMethods.Count > 0,
                a => searchModel.GasMeteringMethods.Contains((int)(a as MultiFamilyAsset).GasMeterMethod));
            //ElectricMeteringMethod
            dbEntities = dbEntities.AddSearchParameter(searchModel.ElectricMeteringMethods != null && searchModel.ElectricMeteringMethods.Count > 0,
                a => searchModel.ElectricMeteringMethods.Contains((int)(a as MultiFamilyAsset).ElectricMeterMethod));

            // TODO: UMR Occu Perc
            // TODO: UMR Max FC Perc

            // TODO: SMR Park Owned Units
            if (searchModel.SmrParkOwnedUnits.HasValue)
            {
                // if this field is set
                if (searchModel.SmrParkOwnedUnits == 2)
                {
                    // park owned units for space mix ratios is set to Yes
                }
            }

            // TODO: SMR Property with Undeveloped Acres
            // TODO: SMR % of undev. land to total acerage

            //Asset Name
            if (!string.IsNullOrWhiteSpace(searchModel.AssetName))
            {
                var name = (searchModel.AssetName.ToLower()).Replace(" ", "").Replace("\'", "").Replace(",", "").Replace("-", "");
                dbEntities = dbEntities.AddSearchParameter(name.Length > 0,
                    a => a.ProjectName.ToLower().Replace(" ", "").Replace("\'", "").Replace(",", "").Replace("-", "").Contains(name));
            }

            // Unit Specifications for Multifamily
            // Probably needs to be more efficient
            if (searchModel.Umr1BdPct.HasValue && searchModel.Umr1BdPct.Value > 0 ||
                searchModel.Umr2Bd1BaPct.HasValue && searchModel.Umr2Bd1BaPct.Value > 0 ||
                searchModel.Umr2Bd2BaPct.HasValue && searchModel.Umr2Bd2BaPct.Value > 0 ||
                searchModel.Umr3BdPct.HasValue && searchModel.Umr3BdPct.Value > 0 ||
                searchModel.UmrStudioPct.HasValue && searchModel.UmrStudioPct.Value > 0)
            {
                var unitSpecs = context.AssetUnitSpecifications.Where(us => us.AssetId != null).GroupBy(us => us.AssetId);

                var studioPct = searchModel.UmrStudioPct.HasValue ? (decimal)searchModel.UmrStudioPct.Value / 100 : new decimal(0);
                var oneBdPct = searchModel.Umr1BdPct.HasValue ? (decimal)searchModel.Umr1BdPct.Value / 100 : new decimal(0);
                var twoBdOneBaPct = searchModel.Umr2Bd1BaPct.HasValue ? (decimal)searchModel.Umr2Bd1BaPct.Value / 100 : new decimal(0);
                var twoBdTwoBaPct = searchModel.Umr2Bd2BaPct.HasValue ? (decimal)searchModel.Umr2Bd2BaPct.Value / 100 : new decimal(0);
                var threeBdPct = searchModel.Umr3BdPct.HasValue ? (decimal)searchModel.Umr3BdPct.Value / 100 : new decimal(0);

                dbEntities = from a in dbEntities
                             where a.AssetType == AssetType.MultiFamily
                             let unitCnt = (decimal)(a as MultiFamilyAsset).TotalUnits
                             where unitCnt > 0
                             from us in unitSpecs
                             where us.Key == a.AssetId
                             // Studio
                             where !(studioPct > 0) ||
                                    (from spec in us
                                     where spec.BedCount == BedroomCount.Zero
                                     select spec.CountOfUnits).DefaultIfEmpty(0).Sum() / unitCnt >= studioPct
                             // 1 bed percent
                             where !(oneBdPct > 0) ||
                                    (from spec in us
                                     where spec.BedCount == BedroomCount.One
                                     select spec.CountOfUnits).DefaultIfEmpty(0).Sum() / unitCnt >= oneBdPct
                             // 2 bed/1 bath percent
                             where !(twoBdOneBaPct > 0) ||
                                    (from spec in us
                                     where spec.BedCount == BedroomCount.Two && spec.BathCount == BathroomCount.One
                                     select spec.CountOfUnits).DefaultIfEmpty(0).Sum() / unitCnt >= twoBdOneBaPct
                             // 2 bed/2 bath percent
                             where !(twoBdTwoBaPct > 0) ||
                                    (from spec in us
                                     where spec.BedCount == BedroomCount.Two && spec.BathCount == BathroomCount.Two
                                     select spec.CountOfUnits).DefaultIfEmpty(0).Sum() / unitCnt >= twoBdTwoBaPct
                             // 3 bed percent
                             where !(threeBdPct > 0) ||
                                    (from spec in us
                                     where spec.BedCount == BedroomCount.Three
                                     select spec.CountOfUnits).DefaultIfEmpty(0).Sum() / unitCnt >= threeBdPct
                             select a;
            }

            // Space Specifications for MHP
            // Potential issue in database: 
            // No MHP properties have "TotalUnits"
            // No MHP specifications have "CountOfUnits"
            if (searchModel.SmrSWidePct.HasValue && searchModel.SmrSWidePct.Value > 0 ||
                searchModel.SmrDWidePct.HasValue && searchModel.SmrDWidePct.Value > 0 ||
                searchModel.SmrTWidePct.HasValue && searchModel.SmrTWidePct.Value > 0 ||
                searchModel.SmrParkOwnedMaxPct.HasValue && searchModel.SmrParkOwnedMaxPct.Value >= 0
                    && searchModel.SmrParkOwnedMaxPct.Value <= 100)
            {
                var MHPSpecs = context.AssetMHPSpecifications.Where(ms => ms.AssetId != null);
                var SWidePct = searchModel.SmrSWidePct.HasValue ? (decimal)searchModel.SmrSWidePct / 100 : new decimal(0);
                var DWidePct = searchModel.SmrDWidePct.HasValue ? (decimal)searchModel.SmrDWidePct / 100 : new decimal(0);
                var TWidePct = searchModel.SmrTWidePct.HasValue ? (decimal)searchModel.SmrTWidePct / 100 : new decimal(0);
                var POMaxPct = searchModel.SmrParkOwnedMaxPct.HasValue && searchModel.SmrParkOwnedMaxPct >= 0 &&
                    searchModel.SmrParkOwnedMaxPct <= 100 ? (decimal)searchModel.SmrParkOwnedMaxPct.Value / 100 : -1;

                dbEntities = from a in dbEntities
                             where a.AssetType == AssetType.MHP
                             from ms in MHPSpecs
                             let unitCnt = (decimal)(ms.NumberSingleWide + ms.NumberDoubleWide + ms.NumberTripleWide)
                             where unitCnt > 0
                             where ms.AssetId == a.AssetId
                             where !(SWidePct > 0) ||
                                    ms.NumberSingleWide / unitCnt >= SWidePct
                             where !(DWidePct > 0) ||
                                    ms.NumberDoubleWide / unitCnt >= DWidePct
                             where !(TWidePct > 0) ||
                                    ms.NumberTripleWide / unitCnt >= TWidePct
                             where (POMaxPct == -1) ||
                                    (ms.NumberSingleWideOwned + ms.NumberDoubleWideOwned + ms.NumberTripleWideOwned) / unitCnt <= POMaxPct
                             select a;
            }

            // Radius (in KM) around an point. approximation
            if (searchModel.Latitude.HasValue && Math.Abs(searchModel.Latitude.Value) <= 90 &&
                searchModel.Longitude.HasValue && Math.Abs(searchModel.Longitude.Value) <= 180 &&
                searchModel.SearchRadius.HasValue && searchModel.SearchRadius.Value > 0)
            {
                var lat = searchModel.Latitude.Value;
                var cosLat = Math.Cos(lat * Math.PI / 180);
                var lng = searchModel.Longitude.Value;
                // Rough conversion of KM to lat/lng distance
                var radius = Math.Pow(searchModel.SearchRadius.Value / 110.25, 2);
                //dbEntities = dbEntities.AddSearchParameter(searchModel.SearchRadius.Value > 0,
                //    a => radius > Math.Pow(a.Latitude.Value - lat, 2) + Math.Pow((a.Longitude.Value - lng) * cosLat, 2));
            }

            #endregion

            // Getting generic statistics about the search result
            var statSearch = dbEntities.GroupBy(c => 1)
               .Select(col => new
               {
                   TotalAssets = col.Count(),
                   PublishAssets = col.Where(a => a.IsPublished).Count(),
                   TotalAssetValue = col.Sum(a => (long)(a.AskingPrice > 0 ? a.AskingPrice : a.CurrentBpo)),
                   MultiFamUnits = col.Sum((a => (a.AssetType == AssetType.MultiFamily || a.AssetType == AssetType.MHP ? 1 : 0))),
                   SquareFeet = col.Count() <= 0 ? 0 : col.Sum(a => a.AssetType != AssetType.MHP ? a.SquareFeet : 0)
               }).FirstOrDefault();

            model.GlobalPartDB = 0;
            if (statSearch != null)
            {
                model.Total = statSearch.TotalAssets;
                model.PublishedAssets = statSearch.PublishAssets;
                model.TotalAssetVal = statSearch.TotalAssetValue;
                model.MultiFamUnits = statSearch.MultiFamUnits;
                model.TotalSqFt = statSearch.SquareFeet;
            }
            else
            {
                model.Total = 0;
                model.PublishedAssets = 0;
                model.TotalAssetVal = 0;
                model.MultiFamUnits = 0;
                model.TotalSqFt = 0;
            }

            // Get list of portfolios
            List<PortfolioAsset> PortfolioAssetList = new List<PortfolioAsset>();
            PortfolioAssetList = context.PortfolioAssets.ToList();

            foreach (var entity in dbEntities.OrderBy(a => a.ProjectName).Skip(searchModel.Skip).Take(searchModel.Take).ToList())
            {
                var lastReportedOccupancyYear = entity.LastReportedOccupancyDate.HasValue ? entity.LastReportedOccupancyDate.Value.ToString("yyyy") : "N/A";
                var lastReportedOccupancyDate = entity.LastReportedOccupancyDate.HasValue ? entity.LastReportedOccupancyDate.Value.ToString("MM/yyyy") : "N/A";
                bool offersDateSoon = (entity.CallforOffersDate.HasValue && entity.CallforOffersDate < DateTime.Now.AddDays(30) && entity.CallforOffersDate > DateTime.Now) ? true : false;

                PortfolioAsset assetPF = PortfolioAssetList.Where(x => x.AssetId == entity.AssetId).FirstOrDefault();
                Portfolio _portfolio = new Portfolio();
                if (assetPF != null)
                {
                    _portfolio = context.Portfolios.Where(x => x.PortfolioId == assetPF.PortfolioId && x.UserId == userId).FirstOrDefault();
                }
                else
                {
                    _portfolio = null;
                }

                var adavm = new AssetDynamicAssetViewModel
                {
                    AssetId = entity.AssetId,
                    ProjectName = entity.ProjectName,
                    YearBuilt = entity.YearBuilt,
                    AddressLine1 = entity.PropertyAddress,
                    AddressLine2 = entity.PropertyAddress2,
                    State = entity.State,
                    City = entity.City,
                    Zip = entity.Zip,
                    Longitude = entity.Longitude.GetValueOrDefault(),
                    Latitude = entity.Latitude.GetValueOrDefault(),
                    AskingPrice = entity.AskingPrice,
                    CurrentBpo = entity.CurrentBpo,
                    AssetType = (int)entity.AssetType,
                    SquareFeet = entity.SquareFeet,
                    ListingStatus = (int)entity.ListingStatus,
                    ProformaAOE = entity.ProformaAnnualOperExpenses,
                    ProformaAI = entity.ProformaAnnualIncome,
                    ProformaMI = entity.ProformaMiscIncome,
                    ProformaVF = entity.ProformaVacancyFac,
                    OccupancyRate = ((100 - entity.CurrentVacancyFac) / 100).ToString("P0"),
                    OccupancyYear = lastReportedOccupancyYear,
                    OccupancyDate = lastReportedOccupancyDate,
                    SalesPrice = entity.SalesPrice,
                    AuctionDate = entity.AuctionDate.HasValue ? entity.AuctionDate.Value.ToString("MM/dd/yy") : "N/A",
                    CallforOffersDate = entity.CallforOffersDate.HasValue ? entity.CallforOffersDate.Value.ToString("MM/dd/yy") : "N/A",
                    CallforOffersDateSoon = offersDateSoon,
                    AssmFinancing = entity.MortgageLienAssumable.ToString(),
                    IsPartOfPortfolio = (_portfolio == null) ? false : true,
                    PortfolioId = (_portfolio == null) ? Guid.Empty : _portfolio.PortfolioId,
                };
                if (entity.AssetType == AssetType.MultiFamily || entity.AssetType == AssetType.MHP)
                {
                    var converted = entity as MultiFamilyAsset;
                    adavm.TotalUnits = converted.TotalUnits;
                    adavm.Description = string.Format("{0} unit {1} property in {2}, {3}", converted.TotalUnits, EnumHelper.GetEnumDescription(entity.AssetType), entity.City, entity.State);
                }
                else
                {
                    var converted = entity as CommercialAsset;
                    // remove letters from lotsize
                    string newLotSize = string.Empty;
                    if (!string.IsNullOrEmpty(converted.LotSize))
                    {
                        char[] arr = converted.LotSize.ToCharArray();
                        arr = Array.FindAll<char>(arr, (c => (char.IsDigit(c)
                            || c == '.'
                            || c == '-'
                            || char.IsWhiteSpace(c))));
                        newLotSize = new String(arr);
                    }
                    adavm.NewLotSize = newLotSize;
                    adavm.EstDeferredMaintenance = converted.EstDeferredMaintenance;
                    adavm.Description = string.Format("{0} acre {1} property in {2}, {3}", newLotSize, EnumHelper.GetEnumDescription(entity.AssetType), entity.City, entity.State);
                }
                // literally for tonight, just get the main image in this loop. TODO: turn this method back into a one DB call asap
                var imageEntity = context.AssetImages.FirstOrDefault(x => x.IsMainImage && x.AssetId == entity.AssetId);
                if (imageEntity != null)
                {
                    adavm.Image = new AssetDynamicImageViewModel
                    {
                        FileName = imageEntity.FileName,
                        ContentType = imageEntity.ContentType
                    };
                }
                model.Assets.Add(adavm);
            }
            return model;
        }


        public AssetHCOwnershipModel GetAssetHCByAssetHCOwnershipId(int assetHCOwnershipId)
        {
            var context = _factory.Create();

            var assetHCOwnership = context.AssetHCOwnership
                                    .Where(s => s.AssetHCOwnershipId == assetHCOwnershipId)
                                    .OrderByDescending(s => s.CreateDate).Select(a => new AssetHCOwnershipModel
                                    {
                                        AssetHCOwnershipId = a.AssetHCOwnershipId,
                                        AssetId = a.AssetId,
                                        HoldingCompanyId = a.HoldingCompanyId,
                                        CreateDate = a.CreateDate,
                                        Terms = a.Terms,
                                        ActualClosingDate = a.ActualClosingDate,
                                        SalesPrice = a.SalesPrice,
                                        SalesPriceNotProvided = a.SalesPriceNotProvided,
                                        CalculatedPPU = a.CalculatedPPU,
                                        CalculatedPPSqFt = a.CalculatedPPSqFt,
                                        CashInvestmentApy = a.CashInvestmentApy,
                                        TermsOther = a.TermsOther,
                                        CapRate = a.CapRate,

                                        OwnerHoldingCompany = a.HoldingCompany.CompanyName,
                                        OwnerISRA = a.HoldingCompany.ISRA,
                                        OwnerHoldingCompanyEmail = a.HoldingCompany.Email,
                                        OwnerHoldingCompanyFirst = a.HoldingCompany.FirstName,
                                        OwnerHoldingCompanyLast = a.HoldingCompany.LastName,
                                        OwnerHoldingCompanyAddressLine1 = a.HoldingCompany.AddressLine1,
                                        OwnerHoldingCompanyAddressLine2 = a.HoldingCompany.AddressLine2,
                                        OwnerHoldingCompanyCity = a.HoldingCompany.City,
                                        OwnerHoldingCompanyState = a.HoldingCompany.State,
                                        OwnerHoldingCompanyZip = a.HoldingCompany.Zip,
                                        OwnerHoldingCompanyCountry = a.HoldingCompany.Country,
                                        OwnerHoldingCompanyWorkPhone = a.HoldingCompany.WorkNumber,
                                        OwnerHoldingCompanyCellPhone = a.HoldingCompany.CellNumber,

                                        //OwnerHoldingCompanyFax = a.HoldingCompany.FaxNumber
                                        OwnerHoldingCompanyLinkedIn = a.HoldingCompany.LinkedIn,
                                        OwnerHoldingCompanyFacebook = a.HoldingCompany.Facebook,
                                        OwnerHoldingCompanyInstagram = a.HoldingCompany.Instagram,
                                        OwnerHoldingCompanyTwitter = a.HoldingCompany.Twitter

                                    }).FirstOrDefault();
            return assetHCOwnership;
        }

        public List<AssetOCModel> GetAssetOCByAssetId(Guid assetId)
        {
            var context = _factory.Create();

            var assetHCOwnership = context.AssetOC
                                    .Where(s => s.AssetId == assetId)
                                    .OrderByDescending(s => s.CreateDate).Select(a => new AssetOCModel
                                    {
                                        AssetOCId = a.AssetOCId,
                                        AssetId = a.AssetId,
                                        OperatingCompanyId = a.OperatingCompanyId,
                                        CreateDate = a.CreateDate,

                                        OwnerOperatingCompany = a.OperatingCompany.CompanyName,
                                        FullName = a.OperatingCompany.LastName + ", " + a.OperatingCompany.FirstName,
                                        City = a.OperatingCompany.City,
                                        State = a.OperatingCompany.State,
                                        Zip = a.OperatingCompany.Zip

                                    }).ToList();
            return assetHCOwnership;
        }

        public void SaveUpdateAssetOC(HCCAssetOC assetOC)
        {
            if (assetOC != null)
            {
                var context = _factory.Create();
                if (assetOC.AssetOCId > 0)
                {

                }
                else
                {
                    AssetOC model = new AssetOC();
                    model.AssetId = assetOC.AssetId;
                    model.OperatingCompanyId = assetOC.OperatingCompanyId;
                    model.CreateDate = DateTime.Now;

                    context.AssetOC.Add(model);
                    context.Save();
                }
            }
        }

        public AssetOCModel GetAssetOCByAssetOCId(int assetOCId)
        {
            var context = _factory.Create();

            var assetOC = context.AssetOC
                                    .Where(s => s.AssetOCId == assetOCId)
                                    .OrderByDescending(s => s.CreateDate).Select(a => new AssetOCModel
                                    {
                                        AssetOCId = a.AssetOCId,
                                        AssetId = a.AssetId,
                                        OperatingCompanyId = a.OperatingCompanyId,
                                        CreateDate = a.CreateDate,
                                        OwnerOperatingCompany = a.OperatingCompany.CompanyName,
                                        FirstName = a.OperatingCompany.FirstName,
                                        LastName = a.OperatingCompany.LastName,
                                        Email = a.OperatingCompany.Email,
                                        AddressLine1 = a.OperatingCompany.AddressLine1,
                                        AddressLine2 = a.OperatingCompany.AddressLine2,
                                        City = a.OperatingCompany.City,
                                        State = a.OperatingCompany.State,
                                        Zip = a.OperatingCompany.Zip,
                                        Country = a.OperatingCompany.Country,
                                        WorkNumber = a.OperatingCompany.WorkNumber,
                                        CellNumber = a.OperatingCompany.CellNumber,
                                        //FaxNumber = a.OperatingCompany.FaxNumber

                                        LinkedIn = a.OperatingCompany.LinkedIn,
                                        Facebook = a.OperatingCompany.Facebook,
                                        Instagram = a.OperatingCompany.Instagram,
                                        Twitter = a.OperatingCompany.Twitter

                                    }).FirstOrDefault();
            return assetOC;
        }

        public AssetOCAddressModel GetHCAddressByAssetId(Guid assetId)
        {
            var context = _factory.Create();

            var assetHCAddress = context.AssetHCOwnership.Where(s => s.AssetId == assetId)
                                    .OrderByDescending(s => s.CreateDate).Select(a => new AssetOCAddressModel
                                    {
                                        AddressLine1= a.HoldingCompany.AddressLine1,
                                        AddressLine2 = a.HoldingCompany.AddressLine2,
                                        City = a.HoldingCompany.City,
                                        State = a.HoldingCompany.State,
                                        Zip = a.HoldingCompany.Zip,

                                        Country = a.HoldingCompany.Country,
                                        WorkNumber = a.HoldingCompany.WorkNumber,
                                        CellNumber = a.HoldingCompany.CellNumber,
                                        //FaxNumber = a.HoldingCompany.FaxNumber
                                        LinkedIn = a.HoldingCompany.LinkedIn,
                                        Facebook = a.HoldingCompany.Facebook,
                                        Instagram = a.HoldingCompany.Instagram,
                                        Twitter = a.HoldingCompany.Twitter

                                    }).FirstOrDefault();
            return assetHCAddress;
        }

        public List<ChainOfTitleQuickListModel> GetChainOfTitleByAssetId(string AssetId)
        {
            var list = new List<ChainOfTitleQuickListModel>();
            var context = _factory.Create();
            Guid assetId = new Guid(AssetId);

            var asset = context.Assets.Where(x => x.AssetId== assetId).FirstOrDefault();
            var assetHC = context.AssetHCOwnership.Where(x => x.AssetId == assetId).OrderByDescending(x=>x.CreateDate).ToList();
            var assetOC = context.AssetOC.Where(x => x.AssetId == assetId).OrderByDescending(x => x.CreateDate).ToList();

            var count = assetHC.Count() > assetOC.Count() ? assetHC.Count() : assetOC.Count();

            int units = 0;
            int squareFeet = 0;
            if (asset.AssetType == AssetType.MultiFamily)
            {
                var mf = asset as MultiFamilyAsset;
                units = mf.TotalUnits;
            }
            else if (asset.AssetType == AssetType.MHP)
            {
                var mf = asset as MultiFamilyAsset;
                units = mf.TotalUnits;
                units += asset.NumberRentableSpace != null ? (int)asset.NumberRentableSpace : 0;
                units += asset.NumberNonRentableSpace != null ? (int)asset.NumberNonRentableSpace : 0;
            }
            else
            {
                var ca = asset as CommercialAsset;
                squareFeet = ca.SquareFeet;
            }


            for (int i = 0; i < count; i++) 
            {
                var item = new ChainOfTitleQuickListModel();

                item.AssetId = assetId;
                item.AssetName = asset.ProjectName;
                item.AssetNumber = asset.AssetNumber;
                item.City= asset.City;
                item.State = asset.State;
                item.Type = EnumHelper.GetEnumDescription(asset.AssetType);
                item.SquareFeet = squareFeet;
                item.NumberOfUnits = units;
                item.County = asset.County;
                item.IsActive = asset.IsActive;

                item.Show = asset.Show ? "Yes" : "No";

                item.Portfolio = context.PortfolioAssets.Where(x => x.AssetId == asset.AssetId).Any() ? true : false;
                item.UserType = context.Users.Where(us => us.UserId == asset.ListedByUserId).FirstOrDefault().UserType;
                item.ListingStatus = asset.ListingStatus;                
                item.BusDriver = asset.Show ? "CA" : "SUS";


                if (i < assetHC.Count && assetHC[i] != null)
                {
                    var hcId = assetHC[i].HoldingCompanyId;
                    var hc = context.HoldingCompanies.Where(a => a.HoldingCompanyId == hcId).FirstOrDefault();

                    item.HCID = assetHC[i].HoldingCompanyId;
                    item.HCName = hc.CompanyName;
                    item.Date = assetHC[i].ActualClosingDate;
                    item.Pricing = assetHC[i].SalesPrice ?? 0;
                    item.Terms = assetHC[i].Terms;
                    item.CAP = assetHC[i].CapRate ?? 0;
                }
                if (i < assetOC.Count && assetOC[i] != null)
                {
                    var ocId = assetOC[i].OperatingCompanyId;
                    var oc = context.OperatingCompanies.Where(a => a.OperatingCompanyId == ocId).FirstOrDefault();

                    item.OCID = assetOC[i].OperatingCompanyId;
                    item.OCName = oc.CompanyName;
                }
                list.Add(item);
            }
            return list;
        }


    }
}