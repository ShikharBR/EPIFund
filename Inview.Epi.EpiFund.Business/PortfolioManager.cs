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
                isSubjectToAuction = model.isSubjectToAuction.HasValue ? model.isSubjectToAuction.Value : false,
                LastReportedOccupancyDate = model.LastReportedOccupancyDate,
                NumberofAssets = model.NumberofAssets,
                PortfolioName = model.PortfolioName,
                
                CapRete = model.CapRate,
                IsCallOffersDate = model.IsCallOffersDate.HasValue ? model.IsCallOffersDate.Value : false,
                
                MustPortfolioAssetsInclusive = model.MustPortfolioAssetsInclusive.HasValue ? model.MustPortfolioAssetsInclusive.Value : false,

                ListingStatusall = model.ListingStatusall,
                SalePortfolioAcceptableSeller = model.SalePortfolioAcceptableSeller,
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
                portfolioAsset.isActive = false;
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

        public List<PortfolioQuickListViewModel> GetSearchPortfolios(ManagePortfoliosModel model)
        {
            IEPIRepository ePIRepository = this._factory.Create();
            List<PortfolioQuickListViewModel> portfolioQuickListViewModels = new List<PortfolioQuickListViewModel>();
            List<User> users = ePIRepository.Users.ToList<User>();
            List<Guid> guids = new List<Guid>();
            int? nullable1 = model.UserId;
            if ((nullable1.GetValueOrDefault() <= 0 ? 0 : Convert.ToInt32(nullable1.HasValue)) != 0)
            {
                guids = (
                    from x in ePIRepository.AssetUserMDAs
                    where (int?)x.UserId == model.UserId
                    select x.AssetId).ToList<Guid>();
            }
            List<Portfolio> portfolios = new List<Portfolio>();

            if (model.PortfolioId != Guid.Empty)
            {
                portfolios = ((model.ControllingUserType == UserType.CorpAdmin || model.ControllingUserType == UserType.CorpAdmin2 ? false : model.ControllingUserType != UserType.SiteAdmin) ? (
                from w in ePIRepository.Portfolios
                where w.isActive && (int?)w.UserId == model.UserId
                && w.PortfolioId == model.PortfolioId
                select w).ToList<Portfolio>() : (
                from w in ePIRepository.Portfolios
                where w.isActive && w.PortfolioId == model.PortfolioId
                select w).ToList<Portfolio>());
            }
            else
            {
                portfolios = ((model.ControllingUserType == UserType.CorpAdmin || model.ControllingUserType == UserType.CorpAdmin2 ? false : model.ControllingUserType != UserType.SiteAdmin) ? (
                from w in ePIRepository.Portfolios
                where w.isActive && (int?)w.UserId == model.UserId
                select w).ToList<Portfolio>() : (
                from w in ePIRepository.Portfolios
                where w.isActive
                select w).ToList<Portfolio>());
            }

            List<PortfolioAsset> portfolioAssets = new List<PortfolioAsset>();
            List<Asset> assets1 = new List<Asset>();
            if (!string.IsNullOrEmpty(model.PortfolioName))
            {
                var regex = "[^A-Za-z0-9]";
                model.PortfolioName = Regex.Replace(model.PortfolioName, regex, "");
                portfolios = (
                    from a in portfolios
                    where (a.PortfolioName == null ? false : Regex.Replace(a.PortfolioName.ToLower(), regex, "").Contains(model.PortfolioName.ToLower()))
                    select a).ToList<Portfolio>();
            }


            foreach (Portfolio portfolio in portfolios)
            {
                IQueryable<PortfolioAsset> portfolioAssets1 =
                    from x in ePIRepository.PortfolioAssets
                    where x.PortfolioId == portfolio.PortfolioId
                    select x;
                portfolioAssets.AddRange(portfolioAssets1);
                List<Guid> guids1 = (
                    from x in portfolioAssets1
                    select x.AssetId).ToList<Guid>();
                foreach (Guid guid in guids1)
                {
                    if (model.ControllingUserType == UserType.ICAdmin)
                    {
                        var asset = ePIRepository.Assets.FirstOrDefault(a => a.AssetId == guid && a.ListedByUserId == model.UserId.Value && !a.IsSubmitted);
                        if (asset != null) assets1.Add(asset);
                    }
                    else assets1.Add(ePIRepository.Assets.FirstOrDefault<Asset>((Asset x) => x.AssetId == guid));
                }
            }

            bool usingAssetSearchCriteria = false;
            if ((int)model.AssetType != 0)
            {
                assets1 = (
                    from w in assets1
                    where w.AssetType == model.AssetType
                    select w).ToList<Asset>();
                usingAssetSearchCriteria = true;
            }
            if (!string.IsNullOrEmpty(model.AssetName))
            {
                assets1 = (
                    from a in assets1
                    where (a.ProjectName == null ? false : a.ProjectName.ToLower().Contains(model.AssetName.ToLower()))
                    select a).ToList<Asset>();
                usingAssetSearchCriteria = true;
            }
            if (!string.IsNullOrEmpty(model.AssetNumber))
            {
                int num = 0;
                int.TryParse(model.AssetNumber, out num);
                if (num != 0)
                {
                    assets1 = (
                        from a in assets1
                        where a.AssetNumber == num
                        select a).ToList<Asset>();
                }
                usingAssetSearchCriteria = true;
            }
            if (!string.IsNullOrEmpty(model.City))
            {
                assets1 = (
                    from x in assets1
                    where (string.IsNullOrEmpty(x.City) ? false : x.City.ToLower().Contains(model.City.ToLower()))
                    select x).ToList<Asset>();
                usingAssetSearchCriteria = true;
            }
            if (!string.IsNullOrEmpty(model.State))
            {
                assets1 = (
                    from a in assets1
                    where a.State == model.State
                    select a).ToList<Asset>();
                usingAssetSearchCriteria = true;
            }
            if (!string.IsNullOrEmpty(model.ZipCode))
            {
                assets1 = (
                    from a in assets1
                    where a.Zip == model.ZipCode
                    select a).ToList<Asset>();
                usingAssetSearchCriteria = true;
            }
            if (!string.IsNullOrEmpty(model.AddressLine1))
            {
                assets1 = (
                    from a in assets1
                    where (string.IsNullOrEmpty(a.PropertyAddress) ? false : a.PropertyAddress.ToLower().Contains(model.AddressLine1.ToLower()))
                    select a).ToList<Asset>();
                usingAssetSearchCriteria = true;
            }
            if (!string.IsNullOrEmpty(model.APNNumber))
            {
                var regex = "[^A-Za-z0-9]";
                var parcelNumber = Regex.Replace(model.APNNumber, regex, "");
                var parcelNumbers = from atpn in ePIRepository.AssetTaxParcelNumbers
                                    join a in ePIRepository.Assets on atpn.AssetId equals a.AssetId
                                    where atpn.TaxParcelNumber != null
                                    select atpn;
                var matches = parcelNumbers.ToList().Where(a => Regex.Replace(a.TaxParcelNumber, regex, "") == parcelNumber);
                assets1 = new List<Asset>();
                foreach (var apn in matches)
                {
                    assets1.Add(ePIRepository.Assets.First(a => a.AssetId == apn.AssetId));
                }
                usingAssetSearchCriteria = true;
            }
            // only show portfolios where none of the assets have been submitted to corp admin
            if (model.ControllingUserType == UserType.ICAdmin)
            {
                portfolioAssets.ForEach(f =>
                {
                    var asset = ePIRepository.Assets.First(a => a.AssetId == f.AssetId);
                    if (asset.IsSubmitted)
                    {
                        portfolios.Remove(portfolios.Find(p => p.PortfolioId == f.PortfolioId));
                    }
                });
            }
            foreach (var p in portfolios)
            {
                User user = users.Where<User>((User x) =>
                {
                    int userId = x.UserId;
                    int? nullable = model.UserId;
                    return (userId != nullable.GetValueOrDefault() ? false : nullable.HasValue);
                }).FirstOrDefault<User>();

                List<PortfolioAsset> list = (
                    from x in portfolioAssets
                    where x.PortfolioId == p.PortfolioId
                    select x).ToList<PortfolioAsset>();

                List<Asset> assets = new List<Asset>();
                foreach (PortfolioAsset portfolioAsset in list)
                {
                    assets.Add(assets1.FirstOrDefault<Asset>((Asset x) => x.AssetId == portfolioAsset.AssetId));
                }
                List<PortfolioAssetsModel> portfolioAssetsModels = new List<PortfolioAssetsModel>();
                foreach (Asset asset in assets)
                {
                    if (asset != null)
                    {
                        portfolioAssetsModels.Add(new PortfolioAssetsModel()
                        {
                            AddressLine1 = asset.PropertyAddress,
                            AssetId = asset.AssetId,
                            AssetNumber = asset.AssetNumber,
                            City = asset.City,
                            Show = (asset.Show ? "Yes" : "No"),
                            State = asset.State,
                            Zip = asset.Zip,
                            Status = EnumHelper.GetEnumDescription(asset.ListingStatus),
                            Type = EnumHelper.GetEnumDescription(asset.AssetType),
                            IsOnHold = asset.HoldForUserId.HasValue,
                            IsSampleAsset = asset.IsSampleAsset,
                            CreatedBy = (user != null ? string.Concat(user.FullName, "~", user.Username) : ""),
                            AssetName = asset.ProjectName,
                            CanViewAssetName = guids.Contains(asset.AssetId)
                        });
                    }
                }
                if (!usingAssetSearchCriteria || (usingAssetSearchCriteria && portfolioAssetsModels.Count > 0))
                {
                    portfolioQuickListViewModels.Add(new PortfolioQuickListViewModel()
                    {
                        NumberofAssets = p.NumberofAssets,
                        PortfolioId = p.PortfolioId,
                        PortfolioName = p.PortfolioName,
                        TotalItemCount = assets1.Count<Asset>(),
                        ControllingUserType = new UserType?(user.UserType),
                        HasPrivileges = (p.UserId == user.UserId ? true : false),
                        isActive = p.isActive,
                        PortfolioAssets = portfolioAssetsModels
                    });
                }
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
            }
            Portfolio entity = model.ModelToEntity();
            



            int num = (
                from x in ePIRepository.PortfolioAssets
                where x.PortfolioId == model.PortfolioId
                select x).Count<PortfolioAsset>();

            entity.NumberofAssets = num + model.SelectedAssets.Count<Guid>();
            entity.isActive = true;
            ePIRepository.Entry(entity).State = EntityState.Modified;
            ePIRepository.Save();
        }

        public List<PortfolioQuickListViewModel> TrimStringProperty(List<PortfolioQuickListViewModel> input)
        {
            // Get list of portfolios where the name starts with a space
            List<PortfolioQuickListViewModel> list = input.Where(x => x.PortfolioName.Substring(0, 1).Equals(" ")).ToList();

            foreach (PortfolioQuickListViewModel item in list)
            {
                item.PortfolioName = item.PortfolioName.Trim();
                input.Where(x => x.PortfolioId == item.PortfolioId).First().PortfolioName = item.PortfolioName;
            }

            return input;
        }

        public List<PortfolioQuickListViewModel> SortPortfoliosModel(List<PortfolioQuickListViewModel> input, bool descending)
        {
            // Sort the portfolio names
            string[] portfolioNames = input.Select(o => o.PortfolioName).ToList().ToArray();

            Array.Sort(portfolioNames, new AlphaNumericComparator());

            if (descending)
                portfolioNames = portfolioNames.OrderByDescending(d => d).ToArray();

            List<PortfolioQuickListViewModel> sortedList = new List<PortfolioQuickListViewModel>();

            foreach (string name in portfolioNames)
            {
                sortedList.Add(input.Where(x => x.PortfolioName.Equals(name)).First());
            }

            return sortedList;
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
}