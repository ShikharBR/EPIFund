using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.Helpers;
using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;

using AutoMapper;

namespace Inview.Epi.EpiFund.Business
{
    public class PortfolioManager : IPortfolioManager
    {
        private IEPIContextFactory _factory;

        private IAssetManager _asset;

        public PortfolioManager(IEPIContextFactory factory, IAssetManager asset)
        {
            this._factory = factory;
            this._asset = asset;
        }

        private List<AssetDeferredItemViewModel> AccumulateItems(MaintenanceDetails enumType, List<AssetDeferredItemViewModel> modelItems, List<AssetDeferredItemViewModel> propItems)
        {
            List<AssetDeferredItemViewModel> assetDeferredItemViewModels = new List<AssetDeferredItemViewModel>();
            List<AssetDeferredItemViewModel> list = new List<AssetDeferredItemViewModel>();
            assetDeferredItemViewModels = ((
                from elem in propItems
                where elem.MaintenanceDetail == enumType
                select elem).Count<AssetDeferredItemViewModel>() <= 0 ? new List<AssetDeferredItemViewModel>() : (
                from elem in propItems
                where elem.MaintenanceDetail == enumType
                select elem).ToList<AssetDeferredItemViewModel>());
            if ((
                from elem in modelItems
                where elem.MaintenanceDetail == enumType
                select elem).Count<AssetDeferredItemViewModel>() > 0)
            {
                list = (
                    from elem in modelItems
                    where elem.MaintenanceDetail == enumType
                    select elem).ToList<AssetDeferredItemViewModel>();
            }
            foreach (AssetDeferredItemViewModel assetDeferredItemViewModel in assetDeferredItemViewModels)
            {
                if (list.Count<AssetDeferredItemViewModel>() <= 0)
                {
                    list.Add(assetDeferredItemViewModel);
                }
                else
                {
                    AssetDeferredItemViewModel numberOfUnits = list.First<AssetDeferredItemViewModel>();
                    numberOfUnits.NumberOfUnits = numberOfUnits.NumberOfUnits + assetDeferredItemViewModel.NumberOfUnits;
                    AssetDeferredItemViewModel unitCost = list.First<AssetDeferredItemViewModel>();
                    unitCost.UnitCost = unitCost.UnitCost + assetDeferredItemViewModel.UnitCost;
                }
            }
            modelItems.RemoveAll((AssetDeferredItemViewModel elem) => elem.MaintenanceDetail == enumType);
            modelItems.AddRange(list);
            return modelItems;
        }

        public void ActivateAsset(Guid AssetId, Guid PfId)
        {
            IEPIRepository ePIRepository = this._factory.Create();
            PortfolioAsset portfolioAsset = ePIRepository.PortfolioAssets.FirstOrDefault<PortfolioAsset>((PortfolioAsset x) => (x.AssetId == AssetId) && (x.PortfolioId == PfId));
            if (portfolioAsset != null)
            {
                portfolioAsset.isActive = true;
                ePIRepository.Entry(portfolioAsset).State = EntityState.Modified;
                ePIRepository.Save();
            }
        }

        public void ActivatePortfolio(Guid PortfolioId)
        {
            IEPIRepository ePIRepository = this._factory.Create();
            Portfolio portfolio = ePIRepository.Portfolios.FirstOrDefault<Portfolio>((Portfolio x) => x.PortfolioId == PortfolioId);
            if (portfolio != null)
            {
                portfolio.isActive = true;
                ePIRepository.Entry(portfolio).State = EntityState.Modified;
                ePIRepository.Save();
            }
        }

        private PortfolioViewModel CalculateAccumulativeValues(PortfolioViewModel model)
        {
            AssetViewModel portfolioProperty = null;
            double proformaAnnualOperExpenses;
            double proformaAnnualIncome;
            double proformaMiscIncome;
            double num;
            double proformaVacancyFac;
            double num1;
            double proformaVacancyFac1;
            double? currentPrincipalBalance;
            bool flag;
            double valueOrDefault;
            MultiFamilyAssetViewModel multiFamilyAssetViewModel = new MultiFamilyAssetViewModel();
            CommercialAssetViewModel commercialAssetViewModel = new CommercialAssetViewModel();
            bool flag1 = true;
            int value = 0;
            string selectedAmortSchedule = null;
            string mortgageAdjustIfARM = null;
            string mBAAgentName = null;
            DateTime dateTime = new DateTime();
            double num2 = 0;
            double num3 = 0;
            double askingPrice = 0;
            double askingPrice1 = 0;
            double value1 = 0;
            double currentBpo = 0;
            double currentBpo1 = 0;
            double num4 = 0;
            double num5 = 0;
            double proformaAnnualIncome1 = 0;
            double num6 = 0;
            double num7 = 0;
            double valueOrDefault1 = 0;
            double num8 = 0;
            double squareFeet = 0;
            double squareFeet1 = 0;
            double num9 = 0;
            double annualGrossIncome = 0;
            double num10 = 0;
            double askingPrice2 = 0;
            double proformaAnnualIncome2 = 0;
            double proformaVacancyFac2 = 0;
            double proformaAnnualOperExpenses1 = 0;
            double proformaAnnualOperExpenses2 = 0;
            double num11 = 0;
            double num12 = 0;
            double num13 = 0;
            double num14 = 0;
            double num15 = 0;
            double estDeferredMaintenance = 0;
            double currentBpo2 = 0;
            double num16 = 0;
            double num17 = 0;
            double squareFeet2 = 0;
            double totalUnits = 0;
            double num18 = 0;
            double value2 = 0;
            double value3 = 0;
            double value4 = 0;
            value4 = 0;
            value3 = 0;
            value2 = 0;
            num18 = 0;
            totalUnits = 0;
            squareFeet2 = 0;
            num16 = 0;
            currentBpo2 = 0;
            estDeferredMaintenance = 0;
            num15 = 0;
            num14 = 0;
            num13 = 0;
            num12 = 0;
            num11 = 0;
            proformaAnnualOperExpenses2 = 0;
            proformaAnnualOperExpenses1 = 0;
            proformaVacancyFac2 = 0;
            proformaAnnualIncome2 = 0;
            askingPrice2 = 0;
            askingPrice = 0;
            askingPrice1 = 0;
            value1 = 0;
            currentBpo1 = 0;
            currentBpo = 0;
            num5 = 0;
            num6 = 0;
            proformaAnnualIncome1 = 0;
            valueOrDefault1 = 0;
            annualGrossIncome = 0;
            num10 = 0;
            value = 0;
            foreach (AssetViewModel portfolioProperty1 in model.PortfolioProperties)
            {
                currentPrincipalBalance = portfolioProperty1.CurrentPrincipalBalance;
                if (!currentPrincipalBalance.HasValue)
                {
                    flag = false;
                }
                else
                {
                    currentPrincipalBalance = portfolioProperty1.CurrentPrincipalBalance;
                    flag = ((double)currentPrincipalBalance.GetValueOrDefault() != 0 ? 1 : Convert.ToInt32(!currentPrincipalBalance.HasValue)) == 0;
                }
                if (!flag)
                {
                    flag1 = false;
                    break;
                }
            }
            foreach (AssetViewModel assetViewModel in model.PortfolioProperties)
            {
                annualGrossIncome += assetViewModel.AnnualGrossIncome;
                double askingPrice3 = assetViewModel.AskingPrice;
                if (assetViewModel.AskingPrice > 0)
                {
                    askingPrice2 += assetViewModel.AskingPrice;
                }
                estDeferredMaintenance += (double)assetViewModel.EstDeferredMaintenance;
                currentBpo2 += assetViewModel.CurrentBpo;
                proformaAnnualIncome2 += assetViewModel.ProformaAnnualIncome;
                proformaVacancyFac2 += assetViewModel.ProformaVacancyFac;
                proformaAnnualOperExpenses1 += assetViewModel.ProformaAnnualOperExpenses;
                proformaAnnualOperExpenses2 += assetViewModel.ProformaAnnualOperExpenses;
                squareFeet2 += (double)assetViewModel.SquareFeet;
                if (assetViewModel.SelectedAmortSchedule != null)
                {
                    currentPrincipalBalance = assetViewModel.InterestRate;
                    value4 += (double)currentPrincipalBalance.GetValueOrDefault(0);
                }
                if (assetViewModel.MortgageAdjustIfARM != null)
                {
                    mortgageAdjustIfARM = assetViewModel.MortgageAdjustIfARM;
                }
                if (assetViewModel.BalloonDateOfNote.HasValue)
                {
                    dateTime = assetViewModel.BalloonDateOfNote.Value;
                }
                if (assetViewModel.SelectedAmortSchedule != null)
                {
                    selectedAmortSchedule = assetViewModel.SelectedAmortSchedule;
                }
                if (assetViewModel.MBAAgentName != null)
                {
                    mBAAgentName = assetViewModel.MBAAgentName;
                }
                if (assetViewModel.isARM.HasValue)
                {
                    value = assetViewModel.isARM.Value;
                }
                currentPrincipalBalance = assetViewModel.CurrentPrincipalBalance;
                if (currentPrincipalBalance.HasValue)
                {
                    currentPrincipalBalance = assetViewModel.CurrentPrincipalBalance;
                    value2 += (double)currentPrincipalBalance.GetValueOrDefault(0);
                }
                currentPrincipalBalance = assetViewModel.CurrentPrincipalBalance;
                if (currentPrincipalBalance.HasValue)
                {
                    currentPrincipalBalance = assetViewModel.MonthlyPayment;
                    value3 += (double)currentPrincipalBalance.GetValueOrDefault(0);
                }
                if (assetViewModel.SquareFeet > 0)
                {
                    askingPrice = assetViewModel.AskingPrice / (double)assetViewModel.SquareFeet;
                    currentBpo1 = assetViewModel.CurrentBpo / (double)assetViewModel.SquareFeet;
                }
                if (assetViewModel.GetType() == typeof(MultiFamilyAssetViewModel))
                {
                    multiFamilyAssetViewModel = assetViewModel as MultiFamilyAssetViewModel;
                    totalUnits += (double)multiFamilyAssetViewModel.TotalUnits;
                    if (multiFamilyAssetViewModel.UnitSpecifications.Count > 0)
                    {
                    }
                    if (multiFamilyAssetViewModel.TotalUnits > 0)
                    {
                        askingPrice1 = assetViewModel.AskingPrice / (double)multiFamilyAssetViewModel.TotalUnits;
                        if ((!assetViewModel.NumberRentableSpace.HasValue ? false : assetViewModel.NumberNonRentableSpace.HasValue))
                        {
                            double askingPrice4 = assetViewModel.AskingPrice;
                            double value5 = (double)assetViewModel.NumberRentableSpace.Value;
                            int? numberNonRentableSpace = assetViewModel.NumberNonRentableSpace;
                            value1 = askingPrice4 / (value5 + (double)numberNonRentableSpace.Value);
                        }
                        currentBpo = assetViewModel.CurrentBpo / (double)multiFamilyAssetViewModel.TotalUnits;
                    }
                    proformaAnnualOperExpenses = assetViewModel.ProformaAnnualOperExpenses;
                    proformaAnnualIncome = assetViewModel.ProformaAnnualIncome;
                    proformaMiscIncome = assetViewModel.ProformaMiscIncome;
                    num2 = 0;
                    num3 = 0;
                    num7 = 0;
                    if (!assetViewModel.HasDeferredMaintenance)
                    {
                        num7 = (assetViewModel.AskingPrice == 0 ? assetViewModel.CurrentBpo : assetViewModel.AskingPrice);
                    }
                    else
                    {
                        num7 = (assetViewModel.AskingPrice == 0 ? assetViewModel.CurrentBpo + (double)multiFamilyAssetViewModel.EstDeferredMaintenance : assetViewModel.AskingPrice + (double)multiFamilyAssetViewModel.EstDeferredMaintenance);
                    }
                    num18 += num7;
                    num = proformaAnnualIncome + proformaMiscIncome;
                    if (num > 0)
                    {
                        num2 = proformaAnnualOperExpenses / num * 100;
                    }
                    proformaVacancyFac = assetViewModel.ProformaVacancyFac / 100 * num;
                    num3 = Math.Round(num - proformaVacancyFac - proformaAnnualOperExpenses);
                    num1 = num - proformaVacancyFac - proformaAnnualOperExpenses;
                    num4 = (assetViewModel.AskingPrice <= 0 ? num1 / assetViewModel.CurrentBpo : num1 / assetViewModel.AskingPrice);
                    num8 = (assetViewModel.CurrentBpo <= 0 ? num1 / assetViewModel.AskingPrice : num1 / assetViewModel.CurrentBpo);
                    currentPrincipalBalance = assetViewModel.AverageAdjustmentToBaseRentalIncomePerUnitAfterRenovations;
                    if (multiFamilyAssetViewModel.EstDeferredMaintenance == 0)
                    {
                        valueOrDefault = 0;
                    }
                    else
                    {
                        currentPrincipalBalance = assetViewModel.AverageAdjustmentToBaseRentalIncomePerUnitAfterRenovations;
                        valueOrDefault = currentPrincipalBalance.GetValueOrDefault(0) * (double)multiFamilyAssetViewModel.TotalUnits * 12;
                    }
                    valueOrDefault1 = valueOrDefault;
                    proformaAnnualIncome1 = (multiFamilyAssetViewModel.EstDeferredMaintenance == 0 ? assetViewModel.ProformaAnnualIncome : assetViewModel.ProformaAnnualIncome + valueOrDefault1 + proformaMiscIncome);
                    proformaVacancyFac1 = assetViewModel.ProformaVacancyFac / 100 * proformaAnnualIncome1;
                    num6 = (multiFamilyAssetViewModel.EstDeferredMaintenance == 0 ? num3 : Math.Round(proformaAnnualIncome1 - proformaVacancyFac1 - proformaAnnualOperExpenses));
                    num5 = (assetViewModel.AskingPrice + (double)multiFamilyAssetViewModel.EstDeferredMaintenance <= 0 ? Math.Round(Convert.ToDouble(num8.ToString("P2").Replace('%', ' ')), 2) : Math.Round(num6 / num7 * 100, 2));
                    num14 += num8;
                    num12 += num3;
                    num13 += num4;
                    num15 += proformaAnnualIncome1;
                    num16 += num6;
                    num17 += num5;
                    if (multiFamilyAssetViewModel.EstDeferredMaintenance == 0)
                    {
                        proformaAnnualIncome1 = assetViewModel.ProformaAnnualIncome;
                    }
                    num15 += proformaAnnualIncome1;
                }
                else if (model.GetType() == typeof(CommercialAssetViewModel))
                {
                    proformaAnnualOperExpenses = assetViewModel.ProformaAnnualOperExpenses;
                    proformaAnnualIncome = assetViewModel.ProformaAnnualIncome;
                    proformaMiscIncome = assetViewModel.ProformaMiscIncome;
                    num2 = 0;
                    num3 = 0;
                    squareFeet = 0;
                    squareFeet1 = 0;
                    num9 = 0;
                    num7 = 0;
                    num7 = (commercialAssetViewModel.AskingPrice == 0 ? commercialAssetViewModel.CurrentBpo + (double)commercialAssetViewModel.EstDeferredMaintenance : commercialAssetViewModel.AskingPrice + (double)commercialAssetViewModel.EstDeferredMaintenance);
                    num18 += num7;
                    totalUnits += (double)commercialAssetViewModel.NumberofSuites;
                    num = proformaAnnualIncome + proformaMiscIncome;
                    if (num > 0)
                    {
                        num2 = proformaAnnualOperExpenses / num * 100;
                    }
                    if (commercialAssetViewModel.SquareFeet > 0)
                    {
                        squareFeet = proformaMiscIncome / (double)commercialAssetViewModel.SquareFeet;
                        squareFeet1 = proformaAnnualIncome / (double)commercialAssetViewModel.SquareFeet;
                        if (commercialAssetViewModel.LeasedSquareFootageByMajorTenant > 0)
                        {
                            num9 = Math.Round(Math.Round((double)((double)commercialAssetViewModel.LeasedSquareFootageByMajorTenant / (double)commercialAssetViewModel.SquareFeet), 4) * 100, 2);
                        }
                    }
                    proformaVacancyFac = assetViewModel.ProformaVacancyFac / 100 * num;
                    num3 = Math.Round(num - proformaVacancyFac - proformaAnnualOperExpenses);
                    num1 = num - proformaVacancyFac - proformaAnnualOperExpenses;
                    num4 = (assetViewModel.AskingPrice <= 0 ? num1 / assetViewModel.CurrentBpo : num1 / assetViewModel.AskingPrice);
                    num8 = (assetViewModel.CurrentBpo <= 0 ? num1 / assetViewModel.CurrentBpo : num1 / assetViewModel.CurrentBpo);
                    currentPrincipalBalance = commercialAssetViewModel.AverageAdjustmentToBaseRentalIncomePerUnitAfterRenovations;
                    valueOrDefault1 = currentPrincipalBalance.GetValueOrDefault(0);
                    proformaAnnualIncome1 = (multiFamilyAssetViewModel.EstDeferredMaintenance == 0 ? assetViewModel.ProformaAnnualIncome : assetViewModel.ProformaAnnualIncome + valueOrDefault1 + proformaMiscIncome);
                    proformaVacancyFac1 = assetViewModel.ProformaVacancyFac / 100 * proformaAnnualIncome1;
                    num6 = (multiFamilyAssetViewModel.EstDeferredMaintenance == 0 ? num3 : Math.Round(proformaAnnualIncome1 - proformaVacancyFac1 - proformaAnnualOperExpenses));
                    num5 = (assetViewModel.AskingPrice + (double)multiFamilyAssetViewModel.EstDeferredMaintenance <= 0 ? Math.Round(Convert.ToDouble(num8.ToString("P2").Replace('%', ' ')), 2) : Math.Round(num6 / num7 * 100, 2));
                    num14 += num8;
                    num12 += num3;
                    num13 += num4;
                    num15 += proformaAnnualIncome1;
                    num16 += num6;
                    num17 += num5;
                }
            }
            value4 /= (double)model.PortfolioProperties.Count<AssetViewModel>();
            proformaVacancyFac2 /= (double)model.PortfolioProperties.Count<AssetViewModel>();
            num11 = proformaAnnualOperExpenses1 / proformaAnnualIncome2;
            num13 /= (double)model.PortfolioProperties.Count<AssetViewModel>();
            num17 /= (double)model.PortfolioProperties.Count<AssetViewModel>();
            model.PortfolioListedPrice = askingPrice2;
            model.CumiProformaNOI = num12;
            model.CumiProformaSGI = proformaAnnualIncome2;
            model.AvgProformaVF = proformaVacancyFac2 / 100;
            model.CumiProformaAOE = proformaAnnualOperExpenses1;
            model.CumiProformaCAM = proformaAnnualOperExpenses2;
            model.PFProformaAoeFactorAsPerOfSGI = num11;
            model.CumiLPCapRate = num13;
            model.CumiBPO = num14;
            model.CumiBPOCapRate = num14;
            model.CumiAnnualGrossIncome = annualGrossIncome;
            model.ExistingDebtExists = flag1;
            model.CumiSqFeet = squareFeet2;
            model.CumiUnits = totalUnits;
            model.PercentOfPropLeased = num9;
            model.AccumulatedBPO = currentBpo2;
            model.AccumulatedEstDef = estDeferredMaintenance;
            model.CumiDefProformaSGI = num15;
            model.CumiDefProformaNOI = num16;
            model.CumiDefCapRate = num17;
            model.CumiTotalListing = num18;
            model.isARM = value;
            model.SelectedAmortSchedule = selectedAmortSchedule;
            model.MortgageAdjustIfARM = mortgageAdjustIfARM;
            model.MBAAgentName = mBAAgentName;
            model.BallonDate = dateTime;
            model.CumiPrinciBal = value2;
            model.CumiMonthBal = value3;
            model.AvgInterestRate = value4;
            return model;
        }

        public Guid CreatePortfolio(PortfolioViewModel model, int UserId)
        {
            IEPIRepository ePIRepository = this._factory.Create();
            Portfolio portfolio = new Portfolio()
            {
                PortfolioId = Guid.NewGuid(),
                CallforOfferDate = model.CallforOfferDate,
                hasOffersDate = model.hasOffersDate,
                IsTBDMarket = model.IsTBDMarket.HasValue ? model.IsTBDMarket.Value : false,
                LastReportedOccupancyDate = model.LastReportedOccupancyDate,
                NumberofAssets = model.NumberofAssets,
                PortfolioName = model.PortfolioName,
                
                CapRete = model.CapRate,
                IsCallOffersDate = model.IsCallOffersDate.HasValue ? model.IsCallOffersDate.Value : false,
                
                MustPortfolioAssetsInclusive = model.MustPortfolioAssetsInclusive.HasValue ? model.MustPortfolioAssetsInclusive.Value : false,

                ListingStatus = model.ListingStatus,
                SellerTerms = model.SellerTerms,
                SellerTermsOther = model.SellerTermsOther,
                PricingDisplayOption = model.PricingDisplayOption,

            };
            portfolio.NumberofAssets = model.SelectedAssets.Count<Guid>();
            portfolio.UserId = UserId;
            portfolio.isActive = true;
            ePIRepository.Portfolios.Add(portfolio);
            ePIRepository.Save();

            foreach (Guid selectedAsset in model.SelectedAssets)
            {
                PortfolioAsset portfolioAsset = new PortfolioAsset()
                {
                    PortfolioAssetId = Guid.NewGuid(),
                    PortfolioId = portfolio.PortfolioId,
                    AssetId = selectedAsset,
                    isActive = true
                };
                ePIRepository.PortfolioAssets.Add(portfolioAsset);
                ePIRepository.Save();
            }
            

            //update related Assets as suggested at time of update porfoliyo          
            var assets = (from x in ePIRepository.PortfolioAssets where x.PortfolioId == portfolio.PortfolioId select x.Asset);

            var results = model.AssetIdsLpCMV.Split(',').ToList();
            var lstassetLPCMV = new List<assetLPCMV>();
            foreach (var assetResult in results)
            {
                var itemassetLPCMV = new assetLPCMV();
                itemassetLPCMV.Assetid = new Guid(assetResult.Split('_')[0]);
                itemassetLPCMV.Lp = Convert.ToDouble(assetResult.Split('_')[1]);
                itemassetLPCMV.Cmv = Convert.ToDouble(assetResult.Split('_')[2]);
                lstassetLPCMV.Add(itemassetLPCMV);
            }

            foreach (Asset asset in assets)
            {
                var itemassetLPCMV = lstassetLPCMV.Where(a => a.Assetid == asset.AssetId);
                asset.AskingPrice = itemassetLPCMV.FirstOrDefault().Lp;
                asset.CurrentBpo = itemassetLPCMV.FirstOrDefault().Cmv;

                asset.LastReportedOccupancyDate = model.LastReportedOccupancyDate;
                asset.OccupancyDate = model.LastReportedOccupancyDate;

                asset.SellerTerms = model.SellerTerms;
                asset.SellerTermsOther = model.SellerTermsOther;

                //as per current logic
                if (model.ListingStatus == ListingStatusall.Available)
                    asset.ListingStatus = ListingStatus.Available;
                else if (model.ListingStatus == ListingStatusall.Pending)
                    asset.ListingStatus = ListingStatus.Pending;

                asset.IsTBDMarket = model.IsTBDMarket ?? false;

                if (model.IsTBDMarket ?? false)
                {
                    asset.AuctionDate = model.CallforOfferDate;
                }
                if (model.IsCallOffersDate ?? false)
                {
                    asset.CallforOffersDate = model.CallforOfferDate;
                }
                if ((!model.IsTBDMarket ?? false) && (!model.IsCallOffersDate ?? false))
                {
                    asset.CallforOffersDate = null;
                }
                ePIRepository.Entry(asset).State = EntityState.Modified;
            }
            ePIRepository.Save();

            return portfolio.PortfolioId;
        }

        private List<AssetDeferredItemViewModel> CumulativeDefMaintainance(List<AssetDeferredItemViewModel> modelItems, List<AssetDeferredItemViewModel> propItems)
        {
            List<AssetDeferredItemViewModel> assetDeferredItemViewModels = new List<AssetDeferredItemViewModel>();
            List<AssetDeferredItemViewModel> assetDeferredItemViewModels1 = new List<AssetDeferredItemViewModel>();
            modelItems = this.AccumulateItems(MaintenanceDetails.SuiteBuildOut, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.LeaseUpCommissions, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.Roofing, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.ExteriorPainting, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.InteriorUnitPainting, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.MasterHvacSystem, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.IndividualHvac, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.IndividualAppliances, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.IndividualWasherDryer, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.IndividualFlooring, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.IndividualCabinetAndFixtures, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.CoveredParking, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.Fencing, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.Landscaping, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.FireDamage, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.FloodDamage, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.Other, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.ExteriorRenovations, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.CoveredParkingInstall, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.CoveredParkingStructure, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.Lighting, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.ParkingLot, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.Other2, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.PavementRepair, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.ExteriorLighting, modelItems, propItems);
            modelItems = this.AccumulateItems(MaintenanceDetails.ParkOwnedRepairs, modelItems, propItems);
            return modelItems;
        }

        public void DeactivateAsset(Guid AssetId, Guid PfId)
        {
            IEPIRepository ePIRepository = this._factory.Create();
            PortfolioAsset portfolioAsset = ePIRepository.PortfolioAssets.FirstOrDefault<PortfolioAsset>((PortfolioAsset x) => (x.AssetId == AssetId) && (x.PortfolioId == PfId));
            if (portfolioAsset != null)
            {
                ePIRepository.Entry(portfolioAsset).State = EntityState.Deleted;
                ePIRepository.Save();
            }
        }

        public void DeactivatePortfolio(Guid PortfolioId)
        {
            IEPIRepository ePIRepository = this._factory.Create();
            Portfolio portfolio = ePIRepository.Portfolios.FirstOrDefault<Portfolio>((Portfolio x) => x.PortfolioId == PortfolioId);
            if (portfolio != null)
            {
                portfolio.isActive = false;
                ePIRepository.Entry(portfolio).State = EntityState.Modified;
                ePIRepository.Save();
            }
        }

        public PortfolioViewModel GetPortfolio(Guid PortfolioId)
        {
            IEPIRepository ePIRepository = this._factory.Create();
            Portfolio portfolio = (
                from x in ePIRepository.Portfolios
                where x.PortfolioId == PortfolioId
                select x).First<Portfolio>();

            PortfolioViewModel portfolioViewModel = new PortfolioViewModel();

            portfolioViewModel = portfolioViewModel.EntityToModel(portfolio);

            List<PortfolioAsset> list = (
                from x in ePIRepository.PortfolioAssets
                where x.PortfolioId == PortfolioId
                select x).ToList<PortfolioAsset>();

            foreach (PortfolioAsset portfolioAsset in list)
            {
                AssetViewModel asset = this._asset.GetAsset(portfolioAsset.AssetId, false);
                asset.IsActive = portfolioAsset.isActive;
                portfolioViewModel.PortfolioProperties.Add(asset);
            }

            foreach (AssetViewModel portfolioProperty1 in portfolioViewModel.PortfolioProperties)
            {
                if ((
                    from x in portfolioProperty1.Images
                    where x.IsMainImage
                    select x).Count<AssetImage>() > 0)
                {
                    portfolioViewModel.Images.Add((
                        from x in portfolioProperty1.Images
                        where x.IsMainImage
                        select x).First<AssetImage>());
                }
            }

            foreach (AssetViewModel assetViewModel in portfolioViewModel.PortfolioProperties)
            {
                portfolioViewModel.DeferredMaintenanceItems = this.CumulativeDefMaintainance(portfolioViewModel.DeferredMaintenanceItems, assetViewModel.DeferredMaintenanceItems);
                IEnumerable<AssetDeferredItemViewModel> deferredMaintenanceItems =
                    from x in assetViewModel.DeferredMaintenanceItems
                    where x.Selected
                    select x;
                foreach (AssetDeferredItemViewModel deferredMaintenanceItem in deferredMaintenanceItems)
                {
                    PortfolioViewModel estDeferredMaintenance = portfolioViewModel;
                    estDeferredMaintenance.EstDeferredMaintenance = estDeferredMaintenance.EstDeferredMaintenance + Convert.ToInt32(deferredMaintenanceItem.UnitCost * deferredMaintenanceItem.NumberOfUnits);
                }
            }

            if (portfolioViewModel.EstDeferredMaintenance > 0)
            {
                portfolioViewModel.HasDeferredMaintenance = true;
            }

            portfolioViewModel = this.CalculateAccumulativeValues(portfolioViewModel);

            return portfolioViewModel;
        }

        public List<AssetViewModel> GetPortfolioProperties(Guid PortfolioId)
        {
            IEPIRepository ePIRepository = this._factory.Create();
            List<AssetViewModel> assetViewModels = new List<AssetViewModel>();
            List<PortfolioAsset> list = (
                from x in ePIRepository.PortfolioAssets
                where x.PortfolioId == PortfolioId
                select x).ToList<PortfolioAsset>();
            foreach (PortfolioAsset portfolioAsset in list)
            {
                assetViewModels.Add(this._asset.GetAsset(portfolioAsset.AssetId, false));
            }
            return assetViewModels;
        }

        public List<PortfolioQuickListModel> GetSearchPortfolios(ManagePortfoliosModel model)
        {
            IEPIRepository ePIRepository = this._factory.Create();
            var portfolios = ePIRepository.Portfolios.Where(a=>a.isActive).ToList();
            var portfolioQuickListViewModels = new List<PortfolioQuickListModel>();
            var users = ePIRepository.Users.ToList();
            var portfolioAssets = ePIRepository.PortfolioAssets.ToList();
            var assets = ePIRepository.Assets.ToList();


            if (!string.IsNullOrEmpty(model.PortfolioName))
            {
                var regex = "[^A-Za-z0-9]";
                var portfolioName = Regex.Replace(model.PortfolioName, regex, "");
                portfolios = portfolios.Where(a => a.PortfolioName != null && 
                                              Regex.Replace(a.PortfolioName.ToLower(), regex, "").Contains(portfolioName.ToLower())).ToList();
            }
            if (model.PortfolioId != Guid.Empty)
            {
                portfolios = portfolios.Where(a => a.PortfolioId == model.PortfolioId).ToList();
            }

            if ((int)model.AssetType != 0)
            {
               var assetsList = assets.Where(a => a.AssetType == model.AssetType).Select(a=>a.AssetId).ToList();
               var portfolioList = portfolioAssets.Where(a => assetsList.Contains(a.AssetId)).Select(a => a.PortfolioId).ToList();
                portfolios = portfolios.Where(a => portfolioList.Contains(a.PortfolioId) ).ToList();
            }
            if (!string.IsNullOrEmpty(model.AssetName))
            {
                var regex = "[^A-Za-z0-9]";
                var assetName = Regex.Replace(model.AssetName, regex, "");

                var assetsList = assets.Where(a => a.ProjectName != null &&
                                              Regex.Replace(a.ProjectName.ToLower(), regex, "").Contains(assetName.ToLower())).
                                              Select(a => a.AssetId).ToList();

                var portfolioList = portfolioAssets.Where(a => assetsList.Contains(a.AssetId)).Select(a => a.PortfolioId).ToList();
                portfolios = portfolios.Where(a => portfolioList.Contains(a.PortfolioId)).ToList();
            }
            if (!string.IsNullOrEmpty(model.AssetNumber))
            {
                var assetsList = assets.Where(a => model.AssetNumber.Contains(a.AssetNumber.ToString())).Select(a => a.AssetId).ToList();
                var portfolioList = portfolioAssets.Where(a => assetsList.Contains(a.AssetId)).Select(a => a.PortfolioId).ToList();
                portfolios = portfolios.Where(a => portfolioList.Contains(a.PortfolioId)).ToList();
            }
            if (!string.IsNullOrEmpty(model.City))
            {
                var regex = "[^A-Za-z0-9]";
                var assetCity = Regex.Replace(model.City, regex, "");

                var assetsList = assets.Where(a => a.City != null &&
                                              Regex.Replace(a.City.ToLower(), regex, "").Contains(assetCity.ToLower())).
                                              Select(a => a.AssetId).ToList();

                var portfolioList = portfolioAssets.Where(a => assetsList.Contains(a.AssetId)).Select(a => a.PortfolioId).ToList();
                portfolios = portfolios.Where(a => portfolioList.Contains(a.PortfolioId)).ToList(); 
            }
            if (!string.IsNullOrEmpty(model.State))
            {
                var assetsList = assets.Where(a => a.State != null &&
                                              a.State.ToLower().Contains(model.State.ToLower())).
                                              Select(a => a.AssetId).ToList();

                var portfolioList = portfolioAssets.Where(a => assetsList.Contains(a.AssetId)).Select(a => a.PortfolioId).ToList();
                portfolios = portfolios.Where(a => portfolioList.Contains(a.PortfolioId)).ToList();
            }
            if (!string.IsNullOrEmpty(model.ZipCode))
            {
                var assetsList = assets.Where(a => a.Zip != null &&
                                            a.Zip.ToLower().Contains(model.ZipCode.ToLower())).
                                            Select(a => a.AssetId).ToList();

                var portfolioList = portfolioAssets.Where(a => assetsList.Contains(a.AssetId)).Select(a => a.PortfolioId).ToList();
                portfolios = portfolios.Where(a => portfolioList.Contains(a.PortfolioId)).ToList();
            }
            if (!string.IsNullOrEmpty(model.AddressLine1))
            {
                var regex = "[^A-Za-z0-9]";
                var assetAddressLine1 = Regex.Replace(model.AddressLine1, regex, "");

                var assetsList = assets.Where(a => a.City != null &&
                                              Regex.Replace(a.PropertyAddress.ToLower(), regex, "").Contains(assetAddressLine1.ToLower())).
                                              Select(a => a.AssetId).ToList();

                var portfolioList = portfolioAssets.Where(a => assetsList.Contains(a.AssetId)).Select(a => a.PortfolioId).ToList();
                portfolios = portfolios.Where(a => portfolioList.Contains(a.PortfolioId)).ToList();
            }

            if (!string.IsNullOrEmpty(model.APNNumber))
            {
                var regex = "[^A-Za-z0-9]";
                List<Asset> apnAssetList = new List<Asset>();
                var apnNumber = Regex.Replace(model.APNNumber, regex, "");
                var apnList = ePIRepository.AssetTaxParcelNumbers.ToList();
                var assetsList = apnList.Where(w => w.TaxParcelNumber != null && Regex.Replace(w.TaxParcelNumber.ToLower(), regex, "").Contains(apnNumber.ToLower())).Select(s => s.AssetId).Distinct().ToList();

                var portfolioList = portfolioAssets.Where(a => assetsList.Contains(a.AssetId)).Select(a => a.PortfolioId).ToList();
                portfolios = portfolios.Where(a => portfolioList.Contains(a.PortfolioId)).ToList();
            }
            
            
            /* After filter loop on portfolio*/
            foreach (var p in portfolios)
            {
                var lstAssetInPortfolio = portfolioAssets.Where(x => x.PortfolioId == p.PortfolioId && x.isActive).Select(x => x.AssetId).ToList();
                var relatedAsstes = assets.Where(la => lstAssetInPortfolio.Contains(la.AssetId)).ToList();

                int units = 0;
                int squareFeet = 0;

                var proformaNOIP = 0.0D;
                var pretaxP = 0.0D;
                var CurrentBpoP = 0.0D;

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

                    var aoeP = itemAsset.ProformaAnnualOperExpenses;
                    var pagiP = itemAsset.ProformaAnnualIncome;
                    var pamiP = itemAsset.ProformaMiscIncome;
                    var totalIncomeP = pagiP + pamiP;
                    var pvfP = (itemAsset.ProformaVacancyFac / 100) * totalIncomeP;
                    proformaNOIP += Math.Round((totalIncomeP - pvfP) - aoeP);

                    pretaxP += totalIncomeP - pvfP - aoeP;
                    CurrentBpoP += itemAsset.CurrentBpo;
                }

                var calcLastOccupancyDateLst = relatedAsstes.Where(ra => ra.LastReportedOccupancyDate.HasValue);
                var calcOccupancyDateLst = relatedAsstes.Where(ra => ra.OccupancyDate.HasValue);

               
                var userType = users.Where(us => us.UserId == relatedAsstes.FirstOrDefault().ListedByUserId).FirstOrDefault().UserType;

                var priceing = ((relatedAsstes.Where(ra => ra.AskingPrice > 0).Any() ?
                                       relatedAsstes.Where(ra => ra.AskingPrice > 0).Sum(ra => ra.AskingPrice) : 0)
                                         + (relatedAsstes.Where(ra => ra.AskingPrice == 0).Any() ?
                                         relatedAsstes.Where(ra => ra.AskingPrice == 0).Sum(ra => ra.CurrentBpo) : 0));


                portfolioQuickListViewModels.Add(new PortfolioQuickListModel()
                {
                    NumberOfAssets = p.NumberofAssets,
                    PortfolioId = p.PortfolioId,
                    PortfolioName = p.PortfolioName,
                    States = relatedAsstes.Select(ra => ra.State).Distinct().ToList(),
                    AssetType = relatedAsstes.Select(ra => ra.AssetType).Distinct().ToList(),
                    UnitsSqFt = 0,
                    NumberOfUnits = units,
                    SquareFeet = squareFeet,
                    OccupancyDate = calcLastOccupancyDateLst.Count() > 0 ?
                                    calcLastOccupancyDateLst.Max(ra => ra.LastReportedOccupancyDate).Value.ToString("MM/yyyy") :
                                    calcOccupancyDateLst.Max(ra => ra.OccupancyDate).Value.ToString("MM/yyyy"),
                    OccupancyPercentage = Math.Round(relatedAsstes.Sum(ra => ra.CurrentVacancyFac) / (double)relatedAsstes.Count(), 2),
                    CumiProformaSGI = relatedAsstes.Sum(ra => ra.ProformaAnnualIncome).ToString("C0"),

                    CumiProformaNOI = proformaNOIP.ToString("C0"),

                    AssmFin = relatedAsstes.First().HasPositionMortgage == PositionMortgageType.Yes ? "Yes" : "No",
                    Pricing = priceing,
                    PricingType = relatedAsstes.Where(ra => ra.AskingPrice > 0).Any() ? "LP" : "CMV",
                    CumiLPCapRate = ((proformaNOIP / priceing)).ToString("P2"),
                    UserType = userType,
                    ListingStatus = relatedAsstes.First().ListingStatus,
                    BusDriver = relatedAsstes.Where(ra => ra.Show).Any() ? "CA" : "SUS",

                });

            }

            return portfolioQuickListViewModels;

        }

        public List<PortfolioQuickListViewModel> GetUserPortfolios(int UserId)
        {
            IEPIRepository ePIRepository = this._factory.Create();
            User user = (
                from x in ePIRepository.Users
                where x.UserId == UserId
                select x).FirstOrDefault<User>();
            List<PortfolioQuickListViewModel> portfolioQuickListViewModels = new List<PortfolioQuickListViewModel>();
            List<Portfolio> list = (
                (user.UserType == UserType.CorpAdmin || user.UserType == UserType.CorpAdmin2 ?
                from x in ePIRepository.Portfolios
                where x.isActive
                orderby x.PortfolioName
                select x
                :
                from x in ePIRepository.Portfolios
                where x.UserId == UserId
                && x.isActive
                orderby x.PortfolioName
                select x)).ToList<Portfolio>();
            List<Guid> portfoliosWithSubmittedAssets = new List<Guid>();
            // only send back portfolios that do not have any assets submitted to corp admin
            if (user.UserType == UserType.ICAdmin)
            {
                foreach (var portfolio in list)
                {
                    portfoliosWithSubmittedAssets.AddRange((from p in ePIRepository.PortfolioAssets
                                                            join a in ePIRepository.Assets on p.AssetId equals a.AssetId
                                                            where p.PortfolioId == portfolio.PortfolioId
                                                            && a.IsSubmitted == true
                                                            select p.PortfolioId).ToList());
                }
                foreach (var submittedAssetsPortfolios in portfoliosWithSubmittedAssets)
                {
                    list.Remove(list.Find(p => p.PortfolioId == submittedAssetsPortfolios));
                }
            }
            list.ForEach((Portfolio a) => portfolioQuickListViewModels.Add(new PortfolioQuickListViewModel()
            {
                NumberofAssets = a.NumberofAssets,
                PortfolioId = a.PortfolioId,
                PortfolioName = a.PortfolioName,
                TotalItemCount = list.Count<Portfolio>(),
                ControllingUserType = new UserType?(user.UserType),
                HasPrivileges = (a.UserId == user.UserId ? true : false),
                isActive = a.isActive
            }));
            return portfolioQuickListViewModels;
        }

        public bool isPortfolioNameDuplicate(PortfolioViewModel model)
        {
            bool flag;
            IEPIRepository ePIRepository = this._factory.Create();
            Portfolio portfolio = ePIRepository.Portfolios.FirstOrDefault<Portfolio>((Portfolio x) => x.PortfolioId == model.PortfolioId);
            Portfolio portfolio1 = ePIRepository.Portfolios.FirstOrDefault<Portfolio>((Portfolio x) => x.PortfolioName.ToLower().Equals(model.PortfolioName.ToLower()));
            if (portfolio1 == null)
            {
                flag = false;
            }
            else
            {
                flag = (!(portfolio.PortfolioId == portfolio1.PortfolioId) ? true : false);
            }
            return flag;
        }

        public bool PortfolioExist(string portfolioName)
        {
            bool flag = this._factory.Create().Portfolios.Any<Portfolio>((Portfolio x) => x.PortfolioName.ToLower() == portfolioName.ToLower() && x.isActive);
            return flag;
        }

        public bool PortfolioExist(Guid PortfolioId)
        {
            bool flag = this._factory.Create().Portfolios.Any<Portfolio>((Portfolio x) => x.PortfolioId == PortfolioId);
            return flag;
        }

        public void UpdatePortfolio(PortfolioViewModel model, int UserId)
        {
            IEPIRepository ePIRepository = this._factory.Create();
            foreach (Guid selectedAsset in model.SelectedAssets)
            {
                PortfolioAsset portfolioAsset = new PortfolioAsset()
                {
                    PortfolioAssetId = Guid.NewGuid(),
                    PortfolioId = model.PortfolioId,
                    AssetId = selectedAsset,
                    isActive = true
                };
                ePIRepository.PortfolioAssets.Add(portfolioAsset);
                ePIRepository.Save();
            }
            

            //update related Assets as suggested at time of update porfoliyo          
            var assets = (from x in ePIRepository.PortfolioAssets where x.PortfolioId == model.PortfolioId select x.Asset);
                     
            var results = model.AssetIdsLpCMV.Split(',').ToList();            
            var lstassetLPCMV = new List<assetLPCMV>();
            foreach (var assetResult in results)
            {
                var itemassetLPCMV = new assetLPCMV();
                itemassetLPCMV.Assetid = new Guid(assetResult.Split('_')[0]);
                itemassetLPCMV.Lp =  Convert.ToDouble(assetResult.Split('_')[1]);
                itemassetLPCMV.Cmv = Convert.ToDouble(assetResult.Split('_')[2]);
                lstassetLPCMV.Add(itemassetLPCMV);
            }           


            foreach (Asset asset in assets)
            {
                var itemassetLPCMV = lstassetLPCMV.Where(a => a.Assetid == asset.AssetId);
                asset.AskingPrice = itemassetLPCMV.FirstOrDefault().Lp;
                asset.CurrentBpo = itemassetLPCMV.FirstOrDefault().Cmv;

                asset.LastReportedOccupancyDate = model.LastReportedOccupancyDate;
                asset.SellerTerms = model.SellerTerms;
                asset.SellerTermsOther = model.SellerTermsOther;

                //as per current logic
                if (model.ListingStatus == ListingStatusall.Available)
                    asset.ListingStatus = ListingStatus.Available;
                else if (model.ListingStatus == ListingStatusall.Pending)
                    asset.ListingStatus = ListingStatus.Pending;

                asset.IsTBDMarket = model.IsTBDMarket ?? false;

                if (model.IsTBDMarket ?? false)
                {
                    asset.AuctionDate = model.CallforOfferDate;
                }
                if (model.IsCallOffersDate ?? false)
                {
                    asset.CallforOffersDate = model.CallforOfferDate;
                }
                if ((!model.IsTBDMarket ?? false) && (!model.IsCallOffersDate ?? false))
                {
                    asset.CallforOffersDate = null;
                }                
                ePIRepository.Entry(asset).State = EntityState.Modified;                
            }

            Portfolio entity = model.ModelToEntity();            
            //int num = (from x in ePIRepository.PortfolioAssets  where x.PortfolioId == model.PortfolioId select x).Count<PortfolioAsset>();

            entity.NumberofAssets = assets.Count() + model.SelectedAssets.Count<Guid>();
            entity.isActive = true;
            ePIRepository.Entry(entity).State = EntityState.Modified;
            ePIRepository.Save();
        }

        public List<PortfolioQuickListModel> TrimStringProperty(List<PortfolioQuickListModel> input)
        {
            // Get list of portfolios where the name starts with a space
            List<PortfolioQuickListModel> list = input.Where(x => x.PortfolioName.Substring(0, 1).Equals(" ")).ToList();
            foreach (PortfolioQuickListModel item in list)
            {
                item.PortfolioName = item.PortfolioName.Trim();
                input.Where(x => x.PortfolioId == item.PortfolioId).First().PortfolioName = item.PortfolioName;
            }

            return input;
        }

        public List<PortfolioQuickListModel> SortPortfoliosModel(List<PortfolioQuickListModel> input, bool descending)
        {
            // Sort the portfolio names
            string[] portfolioNames = input.Select(o => o.PortfolioName).ToList().ToArray();
            Array.Sort(portfolioNames, new AlphaNumericComparator());
            if (descending)
                portfolioNames = portfolioNames.OrderByDescending(d => d).ToArray();
            List<PortfolioQuickListModel> sortedList = new List<PortfolioQuickListModel>();
            foreach (string name in portfolioNames)
            {
                sortedList.Add(input.Where(x => x.PortfolioName.Equals(name)).First());
            }
            return sortedList;
        }


        public List<AdminAssetQuickListModel> GetAssetsByAssetsIds(string portfolioId, List<AdminAssetQuickListModel> assetsIds)
        {
            var list = new List<AdminAssetQuickListModel>();
            var context = _factory.Create();

            Guid portId = new Guid(portfolioId);
            var AssetsidsInPortfolio = context.PortfolioAssets.Where(x => x.PortfolioId == portId).Select(a => a.AssetId).ToList();

            var assetsIDS = assetsIds != null ? assetsIds.Where(s => s.IsSelected).Select(s => s.AssetId) : Enumerable.Range(0, 0).Select(_ => Guid.NewGuid()).ToList();
           
            var assets = context.Assets.Where(la => AssetsidsInPortfolio.Contains(la.AssetId) || assetsIDS.Contains(la.AssetId));

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
                        BusDriver = a.Show ? "CA" : "SUS",
                        IsPaper = a.IsPaper,

                        ProformaAnnualOperExpenses = a.ProformaAnnualOperExpenses,
                        ProformaMiscIncome = a.ProformaMiscIncome,
                        ProformaVacancyFac = a.ProformaVacancyFac,
                        EstDeferredMaintenance =  a.EstDeferredMaintenance??0,
                        AverageAdjustmentToBaseRentalIncomePerUnitAfterRenovations = a.AverageAdjustmentToBaseRentalIncomePerUnitAfterRenovations,

                        TotalUnits = (a.AssetType == AssetType.MultiFamily ? ((Mapper.Map<Asset, AssetViewModel>(a)) as MultiFamilyAssetViewModel).TotalUnits : 0 )

                    });;

                }
            });
            return list;
        }
    }

    public class AlphaNumericComparator : IComparer
    {
        public int Compare(object x, object y)
        {
            string s1 = x as string;
            if (s1 == null)
            {
                return 0;
            }
            string s2 = y as string;
            if (s2 == null)
            {
                return 0;
            }

            int len1 = s1.Length;
            int len2 = s2.Length;
            int marker1 = 0;
            int marker2 = 0;

            // Walk through two the strings with two markers.
            while (marker1 < len1 && marker2 < len2)
            {
                char ch1 = s1[marker1];
                char ch2 = s2[marker2];

                // Some buffers we can build up characters in for each chunk.
                char[] space1 = new char[len1];
                int loc1 = 0;
                char[] space2 = new char[len2];
                int loc2 = 0;

                // Walk through all following characters that are digits or
                // characters in BOTH strings starting at the appropriate marker.
                // Collect char arrays.
                do
                {
                    space1[loc1++] = ch1;
                    marker1++;

                    if (marker1 < len1)
                    {
                        ch1 = s1[marker1];
                    }
                    else
                    {
                        break;
                    }
                } while (char.IsDigit(ch1) == char.IsDigit(space1[0]));

                do
                {
                    space2[loc2++] = ch2;
                    marker2++;

                    if (marker2 < len2)
                    {
                        ch2 = s2[marker2];
                    }
                    else
                    {
                        break;
                    }
                } while (char.IsDigit(ch2) == char.IsDigit(space2[0]));

                // If we have collected numbers, compare them numerically.
                // Otherwise, if we have strings, compare them alphabetically.
                string str1 = new string(space1);
                string str2 = new string(space2);

                int result;

                if (char.IsDigit(space1[0]) && char.IsDigit(space2[0]))
                {
                    int thisNumericChunk = int.Parse(str1);
                    int thatNumericChunk = int.Parse(str2);
                    result = thisNumericChunk.CompareTo(thatNumericChunk);
                }
                else
                {
                    result = str1.CompareTo(str2);
                }

                if (result != 0)
                {
                    return result;
                }
            }
            return len1 - len2;
        }
    }

    public class assetLPCMV
    {
        public Guid Assetid { get; set; }
        public double Lp { get; set; }
        public double Cmv { get; set; }
    }
}