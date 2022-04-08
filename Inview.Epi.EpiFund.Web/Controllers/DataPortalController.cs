using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.Helpers;
using Inview.Epi.EpiFund.Domain.ViewModel;
using Inview.Epi.EpiFund.Web;
using Inview.Epi.EpiFund.Web.ActionFilters;
using Inview.Epi.EpiFund.Web.Infrastructure;
using Inview.Epi.EpiFund.Web.Models;
using Inview.Epi.EpiFund.Web.Models.Emails;
using Inview.Epi.EpiFund.Web.Properties;
using Inview.Epi.EpiFund.Web.Providers;
using MVCVideo;
using PagedList;
using Postal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Web.Controllers
{
    [LayoutActionFilter]
    public class DataPortalController : BaseController
    {
        private IUserManager _user;

        private IAssetManager _asset;

        private IPDFService _pdf;

        private IEPIFundEmailService _email;

        private IFileManager _fileManager;

        private IDocusignServiceManager _docs;

        private ITitleCompanyManager _title;

        private IInsuranceManager _insurance;

        private IPortfolioManager _portfolio;

        private List<string> _commercialRequirements
        {
            get;
            set;
        }

        private List<string> _multifamilyRequirements
        {
            get;
            set;
        }

        public DataPortalController(IUserManager user, IAssetManager asset, IPDFService pdf, IEPIFundEmailService email, IFileManager file, ISecurityManager security, IDocusignServiceManager docs, ITitleCompanyManager title, IInsuranceManager insurance, IPortfolioManager portfolio) : base(security, email, user)
        {
            this._docs = docs;
            this._fileManager = file;
            this._pdf = pdf;
            this._title = title;
            this._asset = asset;
            this._user = user;
            this._email = email;
            this._insurance = insurance;
            this._portfolio = portfolio;
            this._commercialRequirements = new List<string>()
            {
                "OtherDemographicDetail.PropertyRequiresMajorTenant",
                "OtherDemographicDetail.MinimumForAccreditedTenantProfiles",
                "OtherDemographicDetail.CanBeVacant"
            };
            this._multifamilyRequirements = new List<string>()
            {
                "MultiFamilyDemographicDetail.AcceptsOneBedroomUnits",
                "MultiFamilyDemographicDetail.UnderperformingProperty",
                "MultiFamilyDemographicDetail.TurnKey",
                "MultiFamilyDemographicDetail.ExtensiveRenovationUpdatingNeeds",
                "MultiFamilyDemographicDetail.MaxRatioOfEFfUnits",
                "MultiFamilyDemographicDetail.GradeClassificationRequirementOfProperty"
            };
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "AcceptNCND")]
        public ActionResult AcceptNCND(NCNDTemplateModel model)
        {
            if (model.AgreeToTerms)
            {
                this._pdf.CreateNCND(Properties.Resources.NCNDTemplate, model);
                return base.RedirectToAction("MyUSCPage", "Home");
            }
            base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You must agree to the NCND terms in order to continue.");
            return base.View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddAssetsToMda(NewAssetToMdaModel model)
        {
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            this._asset.AddAssetIdsToMDA(userByUsername.UserId, model.AssetIdsToAdd);
            AutoConfirmationMDAUpdatedMultipleAssets autoConfirmationMDAUpdatedMultipleAsset = new AutoConfirmationMDAUpdatedMultipleAssets()
            {
                RecipientEmail = userByUsername.Username,
                RecipientName = userByUsername.FullName,
                Locations = string.Empty
            };
            StringBuilder stringBuilder = new StringBuilder();
            StringBuilder stringBuilder1 = new StringBuilder();
            for (int i = 0; i < model.AssetIdsToAdd.Count; i++)
            {
                AssetDescriptionModel assetByAssetId = this._asset.GetAssetByAssetId(model.AssetIdsToAdd[i]);
                if (!autoConfirmationMDAUpdatedMultipleAsset.Locations.Contains(assetByAssetId.CityStateFormattedString))
                {
                    stringBuilder.Append(assetByAssetId.CityStateFormattedString);
                    if (i == model.AssetIdsToAdd.Count - 2)
                    {
                        stringBuilder.Append(", and ");
                    }
                    else if (i < model.AssetIdsToAdd.Count - 1)
                    {
                        stringBuilder.Append(", ");
                    }
                }
                stringBuilder1.Append(assetByAssetId.AssetNumber);
                if (i == model.AssetIdsToAdd.Count - 2)
                {
                    stringBuilder1.Append(", and ");
                }
                else if (i < model.AssetIdsToAdd.Count - 1)
                {
                    stringBuilder1.Append(", ");
                }
            }
            autoConfirmationMDAUpdatedMultipleAsset.AssetNumbers = stringBuilder1.ToString();
            autoConfirmationMDAUpdatedMultipleAsset.Locations = stringBuilder.ToString();
            if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
            {
                this._email.Send(autoConfirmationMDAUpdatedMultipleAsset);
            }
            return base.RedirectToAction("MyUSCPage", "Home");
        }

        [HttpPost]
        public ActionResult AddAssetToMda(IEnumerable<AssetQuickListViewModel> items)
        {
            List<Guid> guids = new List<Guid>();
            items.ToList<AssetQuickListViewModel>().ForEach((AssetQuickListViewModel item) =>
            {
                if (item.IsSelected)
                {
                    guids.Add(item.AssetId);
                }
            });
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            NewAssetToMdaModel newAssetToMdaModel = new NewAssetToMdaModel();
            DateTime? mDASignedDate = this._user.GetMDASignedDate(userByUsername.UserId);
            newAssetToMdaModel.OriginalMDASignDate = mDASignedDate.GetValueOrDefault(DateTime.Now);
            newAssetToMdaModel.UserId = userByUsername.UserId;
            NewAssetToMdaModel newAssetToMdaModel1 = newAssetToMdaModel;
            List<Guid> list = (
                from s in this._asset.GetSignedMDAs(userByUsername.UserId)
                select s.AssetId).ToList<Guid>();
            foreach (Guid guid in guids)
            {
                if (list.Contains(guid))
                {
                    continue;
                }
                newAssetToMdaModel1.AssetIdsToAdd.Add(guid);
                newAssetToMdaModel1.AssetsToAdd.Add(this._asset.GetAssetByAssetId(guid));
            }
            return base.View("AddAssetToMDA", newAssetToMdaModel1);
        }

        [Authorize]
        [HttpGet]
        public ActionResult AddAssetToMda(Guid id)
        {
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            List<AssetDescriptionModel> relatedAssets = this._asset.GetRelatedAssets(new List<Guid>()
            {
                id
            });
            relatedAssets.Add(this._asset.GetAssetByAssetId(id));
            NewAssetToMdaModel newAssetToMdaModel = new NewAssetToMdaModel();
            DateTime? mDASignedDate = this._user.GetMDASignedDate(userByUsername.UserId);
            newAssetToMdaModel.OriginalMDASignDate = mDASignedDate.GetValueOrDefault(DateTime.Now);
            newAssetToMdaModel.UserId = userByUsername.UserId;
            newAssetToMdaModel.AssetIdsToAdd = (
                from s in relatedAssets
                select s.AssetId).ToList<Guid>();
            newAssetToMdaModel.AssetsToAdd = relatedAssets;
            return base.View(newAssetToMdaModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddUserOrder(Guid assetId)
        {
            if (assetId == Guid.Empty)
            {
                return base.Json(new { result = false }, JsonRequestBehavior.DenyGet);
            }
            bool orderHistory = this._asset.AddUserToOrderHistory(assetId, base.User.Identity.Name, AssetOrderHistoryType.Title);
            return base.Json(new { result = orderHistory }, JsonRequestBehavior.DenyGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddUserOrderInsurance(Guid assetId)
        {
            if (assetId == Guid.Empty)
            {
                return base.Json(new { result = false }, JsonRequestBehavior.DenyGet);
            }
            bool orderHistory = this._asset.AddUserToOrderHistory(assetId, base.User.Identity.Name, AssetOrderHistoryType.Insurance);
            return base.Json(new { result = orderHistory }, JsonRequestBehavior.DenyGet);
        }

        public ActionResult AssetList(AssetSearchResultsModel model, string sortOrder, int? page, string filter = null, int? restrictRes = null)
        {
            string city;
            int value;
            model.CallingUserType = null;
            ((dynamic)base.ViewBag).AssetType = this.populateAssetTypeDDL();
            ((dynamic)base.ViewBag).PortfolioList = this.populatePortfolioListDDL();
            List<AssetQuickListViewModel> assetQuickListViewModels = new List<AssetQuickListViewModel>();
            List<PortfolioQuickListViewModel> portfolioQuickListViewModels = new List<PortfolioQuickListViewModel>();
            AssetSearchModel assetSearchModel = new AssetSearchModel()
            {
                MaxAgeRange = new int?((!string.IsNullOrEmpty(model.MaxAgeRange) ? Convert.ToInt32(model.MaxAgeRange) : 0)),
                MaxPriceRange = model.MaxPriceRange,
                MinPriceRange = model.MinPriceRange,
                SelectedAssetCategory = model.SelectedAssetCategory,
                SelectedAssetType = model.SelectedAssetType,
                SelectedListingStatus = model.SelectedListingStatus,
                UserType = model.CallingUserType,
                IsPaper = model.IsPaper
            };
            if (!string.IsNullOrEmpty(model.City))
            {
                city = model.City;
            }
            else
            {
                city = null;
            }
            assetSearchModel.City = city;
            assetSearchModel.Grade = model.MinGrade;
            assetSearchModel.State = model.State;
            assetSearchModel.TurnKey = (!string.IsNullOrEmpty(model.TurnKey) ? Convert.ToBoolean(model.TurnKey) : false);
            assetSearchModel.SelectedStates = model.SelectedStates;
            assetSearchModel.AssetIds = model.AssetIds;
            assetSearchModel.AssetName = model.AssetName;
            assetSearchModel.AccListPrice = model.AccListPrice;
            assetSearchModel.AccUnits = model.AccUnits;
            assetSearchModel.MaxSquareFeet = model.MaxSquareFeet;
            assetSearchModel.MinSquareFeet = model.MinSquareFeet;
            assetSearchModel.MaxUnitsSpaces = model.MaxUnitsSpaces;
            assetSearchModel.MinUnitsSpaces = model.MinUnitsSpaces;
            AssetSearchModel userId = assetSearchModel;
            if (base.User.Identity.IsAuthenticated)
            {
                UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
                model.CallingUserType = new UserType?(userByUsername.UserType);
                userId.UserId = userByUsername.UserId;
                userId.UserType = new UserType?(userByUsername.UserType);
            }
            if (!string.IsNullOrEmpty(filter))
            {
                switch (filter)
                {
                    case "Indus":
                        {
                            userId.SelectedAssetType = new AssetType?(AssetType.Industrial);
                            model.AssetTypes.Single<SelectListItem>((SelectListItem w) => w.Value == AssetType.Industrial.ToString()).Selected = true;
                            break;
                        }
                    case "CRERetail":
                        {
                            userId.SelectedAssetType = new AssetType?(AssetType.Retail);
                            model.AssetTypes.Single<SelectListItem>((SelectListItem w) => w.Value == AssetType.Retail.ToString()).Selected = true;
                            break;
                        }
                    case "CREOffice":
                        {
                            userId.SelectedAssetType = new AssetType?(AssetType.Office);
                            model.AssetTypes.Single<SelectListItem>((SelectListItem w) => w.Value == AssetType.Office.ToString()).Selected = true;
                            break;
                        }
                    case "Medical":
                        {
                            userId.SelectedAssetType = new AssetType?(AssetType.Medical);
                            model.AssetTypes.Single<SelectListItem>((SelectListItem w) => w.Value == AssetType.Medical.ToString()).Selected = true;
                            break;
                        }
                    case "SecuredPaper":
                        {
                            //userId.SelectedAssetType = new AssetType?(AssetType.SecuredPaper);
                            //model.AssetTypes.Single<SelectListItem>((SelectListItem w) => w.Value == AssetType.SecuredPaper.ToString()).Selected = true;
                            model.IsPaper = true;
                            break;
                        }
                    case "MultiFamily":
                        {
                            userId.SelectedAssetType = new AssetType?(AssetType.MultiFamily);
                            model.AssetTypes.Single<SelectListItem>((SelectListItem w) => w.Value == AssetType.MultiFamily.ToString()).Selected = true;
                            break;
                        }
                    case "MHP":
                        {
                            userId.SelectedAssetType = new AssetType?(AssetType.MHP);
                            model.AssetTypes.Single<SelectListItem>((SelectListItem s) => s.Value == AssetType.MHP.ToString()).Selected = true;
                            break;
                        }
                    case "Hospitality":
                        {
                            userId.SelectedAssetType = new AssetType?(AssetType.Hotel);
                            model.AssetTypes.Single<SelectListItem>((SelectListItem s) => s.Value == AssetType.Hotel.ToString()).Selected = true;
                            break;
                        }
                    case "Fuel":
                        {
                            userId.SelectedAssetType = new AssetType?(AssetType.ConvenienceStoreFuel);
                            model.AssetTypes.Single<SelectListItem>((SelectListItem s) => s.Value == AssetType.ConvenienceStoreFuel.ToString()).Selected = true;
                            break;
                        }
                }
            }
            if (!string.IsNullOrEmpty(filter))
            {
                assetQuickListViewModels = new List<AssetQuickListViewModel>();
                model.ShowSearchFormLink = false;
            }
            else
            {
                if (model.AssetIds.Count <= 0)
                {
                    userId.AssetIds = new List<string>();
                }
                else
                {
                    bool flag = false;
                    foreach (string assetId in model.AssetIds)
                    {
                        if (string.IsNullOrEmpty(assetId))
                        {
                            continue;
                        }
                        flag = true;
                    }
                    if (!flag)
                    {
                        userId.AssetIds = new List<string>();
                    }
                }
                if (!restrictRes.HasValue)
                {
                    assetQuickListViewModels = this._asset.GetAssetQuickViewList(userId);
                    portfolioQuickListViewModels = this._asset.GetAssetQuickViewListPF(userId);
                }
                ((dynamic)base.ViewBag).CurrentSort = sortOrder;
                base.ViewBag.TypeSortParm = (sortOrder == "type" ? "type_desc" : "type");
                base.ViewBag.TypeSortParm = (sortOrder == "status" ? "status_desc" : "status");
                base.ViewBag.CitySortParm = (sortOrder == "city" ? "city_desc" : "city");
                base.ViewBag.StateSortParm = (sortOrder == "state" ? "state_desc" : "state");
                base.ViewBag.AddressSortParm = (sortOrder == "address" ? "address_desc" : "address");
                base.ViewBag.AssetIdSortParm = (sortOrder == "assetId" ? "assetId_desc" : "assetId");
                switch (sortOrder)
                {
                    case "type_desc":
                        {
                            assetQuickListViewModels = (
                                from s in assetQuickListViewModels
                                orderby s.Type descending
                                select s).ToList<AssetQuickListViewModel>();
                            break;
                        }
                    case "type":
                        {
                            assetQuickListViewModels = (
                                from s in assetQuickListViewModels
                                orderby s.Type
                                select s).ToList<AssetQuickListViewModel>();
                            break;
                        }
                    case "status_desc":
                        {
                            assetQuickListViewModels = (
                                from s in assetQuickListViewModels
                                orderby s.Status descending
                                select s).ToList<AssetQuickListViewModel>();
                            break;
                        }
                    case "status":
                        {
                            assetQuickListViewModels = (
                                from s in assetQuickListViewModels
                                orderby s.Status
                                select s).ToList<AssetQuickListViewModel>();
                            break;
                        }
                    case "city_desc":
                        {
                            assetQuickListViewModels = (
                                from s in assetQuickListViewModels
                                orderby s.City descending
                                select s).ToList<AssetQuickListViewModel>();
                            break;
                        }
                    case "city":
                        {
                            assetQuickListViewModels = (
                                from w in assetQuickListViewModels
                                orderby w.City
                                select w).ToList<AssetQuickListViewModel>();
                            break;
                        }
                    case "state_desc":
                        {
                            assetQuickListViewModels = (
                                from s in assetQuickListViewModels
                                orderby s.State descending
                                select s).ToList<AssetQuickListViewModel>();
                            break;
                        }
                    case "state":
                        {
                            assetQuickListViewModels = (
                                from w in assetQuickListViewModels
                                orderby w.State
                                select w).ToList<AssetQuickListViewModel>();
                            break;
                        }
                    case "address_desc":
                        {
                            assetQuickListViewModels = (
                                from s in assetQuickListViewModels
                                orderby s.AddressLine1 descending
                                select s).ToList<AssetQuickListViewModel>();
                            break;
                        }
                    case "address":
                        {
                            assetQuickListViewModels = (
                                from w in assetQuickListViewModels
                                orderby w.AddressLine1
                                select w).ToList<AssetQuickListViewModel>();
                            break;
                        }
                    case "assetId_desc":
                        {
                            assetQuickListViewModels = (
                                from s in assetQuickListViewModels
                                orderby s.AssetNumber descending
                                select s).ToList<AssetQuickListViewModel>();
                            break;
                        }
                    default:
                        {
                            assetQuickListViewModels = (sortOrder == "assetId" ? (
                                from w in assetQuickListViewModels
                                orderby w.AssetNumber
                                select w).ToList<AssetQuickListViewModel>() : (
                                from s in assetQuickListViewModels
                                orderby s.AssetNumber
                                select s).ToList<AssetQuickListViewModel>());
                            break;
                        }
                }
                if (!restrictRes.HasValue)
                {
                    model.ShowSearchFormLink = assetQuickListViewModels.Count == 0;
                }
            }
            int num = 0;
            TempDataDictionary tempData = base.TempData;
            int? rowCount = model.RowCount;
            if (rowCount.HasValue)
            {
                rowCount = model.RowCount;
                value = rowCount.Value;
            }
            else
            {
                value = 50;
            }
            num = value;
            tempData["RowCount"] = value;
            base.TempData.Keep("RowCount");
            rowCount = model.Page;
            if (rowCount.GetValueOrDefault(0) > 0)
            {
                page = model.Page;
            }
            rowCount = page;
            int num1 = (rowCount.HasValue ? rowCount.GetValueOrDefault() : 1);
            model.Assets = assetQuickListViewModels.ToPagedList<AssetQuickListViewModel>(num1, num);
            model.Portfolios = portfolioQuickListViewModels.ToPagedList<PortfolioQuickListViewModel>(num1, num);
            return base.View(model);
        }

        [HttpGet]
        public ActionResult AssetView(Guid id)
        {
            if (!base.User.Identity.IsAuthenticated)
            {
                return base.RedirectToAction("Login", "Home");
            }
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            if (this._asset.SignedMDAWithAssetId(userByUsername.UserId, id) || userByUsername.UserType == UserType.SiteAdmin || userByUsername.UserType == UserType.CorpAdmin)
            {
                return base.View(this._asset.GetAsset(id, false));
            }
            if (this._asset.HasSignedMDA(userByUsername.UserId))
            {
                return base.RedirectToAction("AddAssetToMda", "DataPortal", new { id = id });
            }
            return base.RedirectToAction("ExecuteMDA", "DataPortal", new { id = id });
        }

        private AssetViewModel BuildModel(Guid id)
        {
            ((dynamic)base.ViewBag).IsSampleAsset = false;
            List<VideoOptions> videoOptions = new List<VideoOptions>();
            AssetViewModel asset = this._asset.GetAsset(id, false);
            foreach (TitleQuickViewModel titleQuickViewModel in
                from x in this._user.GetTitleCompanies(new CompanySearchModel()
                {
                    CompanyName = string.Empty,
                    CompanyURL = string.Empty,
                    State = asset.State,
                    NeedsManager = true
                })
                orderby x.TitleCompName
                select x)
            {
                asset.TitleCompanies.Add(new SelectListItem()
                {
                    Text = titleQuickViewModel.TitleCompName,
                    Value = titleQuickViewModel.TitleCompanyId.ToString(),
                    Selected = false
                });
            }
            ((dynamic)base.ViewBag).Videos = (dynamic)null;
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            if (userByUsername != null)
            {
                if (!userByUsername.MBAUserId.HasValue)
                {
                    asset.MBAAgentName = string.Empty;
                }
                else
                {
                    IUserManager userManager = this._user;
                    int? mBAUserId = userByUsername.MBAUserId;
                    asset.MBAAgentName = userManager.GetMbaById(mBAUserId.Value).FullName;
                }
                bool? isMortgageAnARM = asset.IsMortgageAnARM;
                if (isMortgageAnARM.HasValue)
                {
                    isMortgageAnARM = asset.IsMortgageAnARM;
                    if ((isMortgageAnARM.GetValueOrDefault() ? !isMortgageAnARM.HasValue : true))
                    {
                        goto Label1;
                    }
                    asset.isARM = new int?(1);
                    goto Label0;
                }
                Label1:
                isMortgageAnARM = asset.IsMortgageAnARM;
                if (isMortgageAnARM.HasValue)
                {
                    isMortgageAnARM = asset.IsMortgageAnARM;
                    if ((!isMortgageAnARM.GetValueOrDefault() ? isMortgageAnARM.HasValue : false))
                    {
                        asset.isARM = new int?(0);
                    }
                }
                Label0:
                if (userByUsername.UserType == UserType.CorpAdmin)
                {
                    UserType userType = userByUsername.UserType;
                }
                bool flag = (asset.ListedByUserId == userByUsername.UserId || this._asset.SignedMDAWithAssetId(userByUsername.UserId, id) || userByUsername.UserType == UserType.CorpAdmin ? true : userByUsername.UserType == UserType.SiteAdmin);
                if (!flag && this._asset.HasSignedMDA(userByUsername.UserId))
                {
                    this._asset.AddAssetIdToMDA(userByUsername.UserId, id);
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Info, "This asset has been added to your previously signed IPA.");
                }
                else if (!flag)
                {
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You must sign an IPA to view this asset.");
                    return null;
                }
            }
            if (asset.Images == null)
            {
                asset.Images = new List<AssetImage>();
            }
            else
            {
                asset.Images = (
                    from w in asset.Images
                    orderby w.IsMainImage descending, w.Order
                    select w).ToList<AssetImage>();
            }
            bool isPaper = asset.IsPaper;
            return asset;
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CreateNewSearchCriteria")]
        public ActionResult CreateNewFromExistingSearchCriteria(AssetSearchCriteriaModel model)
        {
            model.AssetTypes = this.populateAssetTypeDDL();
            if (model.TypeOfAssetsSought == AssetType.MHP || model.TypeOfAssetsSought == AssetType.MultiFamily)
            {
                foreach (string _commercialRequirement in this._commercialRequirements)
                {
                    if (!base.ModelState.ContainsKey(_commercialRequirement))
                    {
                        continue;
                    }
                    base.ModelState[_commercialRequirement].Errors.Clear();
                }
            }
            else
            {
                foreach (string _multifamilyRequirement in this._multifamilyRequirements)
                {
                    if (!base.ModelState.ContainsKey(_multifamilyRequirement))
                    {
                        continue;
                    }
                    base.ModelState[_multifamilyRequirement].Errors.Clear();
                }
            }
            if (base.ModelState.IsValid)
            {
                if (model.GeographicParameters.Interests.Count<GeographicParameterInterestModel>((GeographicParameterInterestModel s) => s.Cities.Any<string>((string c) => c.Length > 0)) <= 0)
                {
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "At least one geographic city and state is required.");
                }
                else
                {
                    int num = this._asset.CreateAssetSearchCriteria(model);
                    if (base.User.Identity.IsAuthenticated)
                    {
                        UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
                        ((dynamic)base.ViewBag).CurrentActiveAssetCount = this._asset.GetCurrentActiveAssetCount();
                        if (base.CanSendAutoEmail(BaseController.AutoEmailType.Asset))
                        {
                            try
                            {
                                this._email.Send(new ConfirmationSearchCriteriaSubmittedEmail()
                                {
                                    Email = userByUsername.Username,
                                    To = string.Concat(userByUsername.FirstName, " ", userByUsername.LastName)
                                });
                            }
                            catch
                            {
                            }
                            List<UserQuickViewModel> users = this._user.GetUsers(new UserSearchModel()
                            {
                                UserTypeFilter = new UserType?(UserType.CorpAdmin),
                                ShowActiveOnly = true
                            });
                            users.AddRange(this._user.GetUsers(new UserSearchModel()
                            {
                                UserTypeFilter = new UserType?(UserType.CorpAdmin2),
                                ShowActiveOnly = true
                            }));
                            users.AddRange(this._user.GetUsers(new UserSearchModel()
                            {
                                UserTypeFilter = new UserType?(UserType.SiteAdmin),
                                ShowActiveOnly = true
                            }));
                            users.AddRange(this._user.GetUsers(new UserSearchModel()
                            {
                                UserTypeFilter = new UserType?(UserType.Investor),
                                ShowActiveOnly = true
                            }));
                            users.AddRange(this._user.GetUsers(new UserSearchModel()
                            {
                                UserTypeFilter = new UserType?(UserType.CREBroker),
                                ShowActiveOnly = true
                            }));
                            users.AddRange(this._user.GetUsers(new UserSearchModel()
                            {
                                UserTypeFilter = new UserType?(UserType.CRELender),
                                ShowActiveOnly = true
                            }));
                            users.AddRange(this._user.GetUsers(new UserSearchModel()
                            {
                                UserTypeFilter = new UserType?(UserType.CREOwner),
                                ShowActiveOnly = true
                            }));
                            users.AddRange(this._user.GetUsers(new UserSearchModel()
                            {
                                UserTypeFilter = new UserType?(UserType.ICAdmin),
                                ShowActiveOnly = true
                            }));
                            users.AddRange(this._user.GetUsers(new UserSearchModel()
                            {
                                UserTypeFilter = new UserType?(UserType.ListingAgent),
                                ShowActiveOnly = true
                            }));
                            foreach (UserQuickViewModel user in users)
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(user.Username))
                                    {
                                        this._email.Send(new ConfirmationToAdminSearchCriteriaSubmitted()
                                        {
                                            Email = user.Username,
                                            SearchCriteraId = num,
                                            SearchCriteriaDate = DateTime.Now,
                                            To = string.Concat(user.FirstName, " ", user.LastName),
                                            SearchCriteriaName = userByUsername.FullName
                                        });
                                    }
                                }
                                catch (Exception exception)
                                {
                                }
                            }
                        }
                        base.TempData["message"] = new MessageViewModel(MessageTypes.Success, string.Concat("Successfully saved. This search form can be referenced with ID#", num, " from your My USC Page."));
                    }
                    else if (!this._user.UserExists(model.EmailAddressOfEntity))
                    {
                        string str = this._user.GeneratePassword();
                        this._user.CreateUser(model.EmailAddressOfEntity, this._user.HashPassword(str), UserType.Investor);
                        base.AuthenticateMachine(model.EmailAddressOfEntity);
                        base.CreateCookie(false, new UserDataModel()
                        {
                            Username = model.EmailAddressOfEntity,
                            IPAddress = base.GetIPAddress(),
                            MachineName = base.DetermineCompName(),
                            SecurityCode = ""
                        });
                        if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
                        {
                            this._email.Send(new AutoRegistrationAutoWelcomeEmail()
                            {
                                Email = model.EmailAddressOfEntity,
                                Password = str,
                                To = model.NameOfPurchasingEntity
                            });
                        }
                        base.TempData["message"] = new MessageViewModel(MessageTypes.Success, string.Concat("Successfully saved. This search form can be referenced with ID#", num, " from your My USC Page. You've also been automatically registered as an Investor for this site. You will receive an email with your username and password."));
                    }
                    else
                    {
                        base.RedirectToAction("Login", "Home", new { searchCriteriaFormId = num });
                    }
                }
            }
            return base.View("EditInvestmentAssetSearchCriteria", model);
        }

        [HttpGet]
        public ViewResult DataPortal()
        {
            return base.View();
        }

        [HttpPost]
        public FileUploadJsonResult DocumentDelete(string docId, Guid assetId)
        {
            FileUploadJsonResult fileUploadJsonResult;
            FileUploadJsonResult fileUploadJsonResult1;
            if (docId != null)
            {
                try
                {
                    bool flag = this._fileManager.DeleteFile(docId, assetId, FileType.Document);
                    this._asset.DeleteAssetFile(docId, assetId, FileType.Document);
                    if (!flag)
                    {
                        fileUploadJsonResult1 = new FileUploadJsonResult()
                        {
                            Data = new { message = "Could Not Find File" }
                        };
                        return fileUploadJsonResult1;
                    }
                    else
                    {
                        fileUploadJsonResult = new FileUploadJsonResult()
                        {
                            Data = new { message = "true" }
                        };
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    fileUploadJsonResult = new FileUploadJsonResult()
                    {
                        Data = new { message = exception.Message }
                    };
                }
                return fileUploadJsonResult;
            }
            fileUploadJsonResult1 = new FileUploadJsonResult()
            {
                Data = new { message = "Could Not Find File" }
            };
            return fileUploadJsonResult1;
        }

        [HttpPost]
        public FileUploadJsonResult DocumentUpload(HttpPostedFileBase file, Guid loiId, string type, string title)
        {
            if (file == null || string.IsNullOrEmpty(file.FileName))
            {
                return new FileUploadJsonResult()
                {
                    Data = new { message = "FileIsNull" }
                };
            }
            string str = this._fileManager.SaveFile(file, loiId, FileType.Document);
            int num = Convert.ToInt32(type);
            string empty = string.Empty;
            if (num != 1)
            {
                empty = (num != 3 ? EnumHelper.GetEnumDescription(LOIDocumentType.ProofOfFunds) : EnumHelper.GetEnumDescription(LOIDocumentType.PersonalFinancialStatement));
            }
            else
            {
                empty = EnumHelper.GetEnumDescription(LOIDocumentType.OperatingManagementResume);
            }
            if (str == null)
            {
                return new FileUploadJsonResult()
                {
                    Data = new { message = "false" }
                };
            }
            string str1 = string.Concat(new string[] { "/Admin/DownloadDocument?fileName=", HttpUtility.UrlEncode(str), "&assetId=", loiId.ToString(), "&contentType=", HttpUtility.UrlEncode(file.ContentType), "&title=", HttpUtility.UrlEncode(title) });
            return new FileUploadJsonResult()
            {
                Data = new { message = "true", filename = str, contentType = file.ContentType, size = this._fileManager.GetFileSize(file.ContentLength), typeDesc = empty, downloadUrl = str1 }
            };
        }

        [Authorize]
        public ActionResult DocusignProcess()
        {
            List<AssetDescriptionModel>.Enumerator enumerator;
            Guid assetId;
            ActionResult action;
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            List<DocumentType> documentTypes = this._docs.ProcessSignedDocuments(new int?(userByUsername.UserId));
            if (documentTypes != null && documentTypes.Count<DocumentType>() > 0)
            {
                DocumentType documentType = documentTypes.First<DocumentType>();
                if (documentType != DocumentType.MDA)
                {
                    if (documentType == DocumentType.JVAgreement)
                    {
                        return base.RedirectToAction("MyUSCPage", "Home");
                    }
                }
                else if (base.TempData["MDAAssets"] != null)
                {
                    List<AssetDescriptionModel> item = base.TempData["MDAAssets"] as List<AssetDescriptionModel>;
                    if (item.Count > 0)
                    {
                        enumerator = item.GetEnumerator();
                        try
                        {
                            while (enumerator.MoveNext())
                            {
                                AssetDescriptionModel current = enumerator.Current;
                                if (!this._asset.UserConfirmedAssetDisclosure(current.AssetId, userByUsername.UserId) || !this._asset.SignedMDAWithAssetId(userByUsername.UserId, current.AssetId))
                                {
                                    continue;
                                }
                                assetId = current.AssetId;
                                action = base.RedirectToAction("ViewAsset", new { id = assetId.ToString() });
                                return action;
                            }
                            goto Label0;
                        }
                        finally
                        {
                            ((IDisposable)enumerator).Dispose();
                        }
                        return action;
                    }
                    Label0:
                    base.TempData["MDAAssets"] = base.TempData["MDAAssets"];
                }
                else if (base.TempData[string.Concat(userByUsername.UserId.ToString(), "Assets")] != null)
                {
                    TempDataDictionary tempData = base.TempData;
                    int userId = userByUsername.UserId;
                    List<AssetDescriptionModel> assetDescriptionModels = tempData[string.Concat(userId.ToString(), "Assets")] as List<AssetDescriptionModel>;
                    if (assetDescriptionModels.Count != 0)
                    {
                        if (assetDescriptionModels.Count == 1)
                        {
                            if (this._asset.SignedMDAWithAssetId(userByUsername.UserId, assetDescriptionModels.First<AssetDescriptionModel>().AssetId))
                            {
                                if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
                                {
                                    this._email.Send(new AutoConfirmationMDAUpdatedEmail()
                                    {
                                        AssetDescription = this._asset.GetAssetByAssetId(assetDescriptionModels.First<AssetDescriptionModel>().AssetId),
                                        RecipientEmail = userByUsername.Username,
                                        RecipientName = userByUsername.FullName
                                    });
                                }
                                assetId = assetDescriptionModels.First<AssetDescriptionModel>().AssetId;
                                return base.RedirectToAction("ViewAsset", new { id = assetId.ToString() });
                            }
                            TempDataDictionary tempDataDictionaries = base.TempData;
                            userId = userByUsername.UserId;
                            string str = string.Concat(userId.ToString(), "Assets");
                            TempDataDictionary tempData1 = base.TempData;
                            userId = userByUsername.UserId;
                            tempDataDictionaries[str] = tempData1[string.Concat(userId.ToString(), "Assets")];
                        }
                        else if (assetDescriptionModels.Count > 1)
                        {
                            bool flag = true;
                            foreach (AssetDescriptionModel assetDescriptionModel in assetDescriptionModels)
                            {
                                if (!(!this._asset.SignedMDAWithAssetId(userByUsername.UserId, assetDescriptionModels.First<AssetDescriptionModel>().AssetId) & flag))
                                {
                                    continue;
                                }
                                flag = false;
                            }
                            if (flag)
                            {
                                AutoConfirmationMDAUpdatedMultipleAssets autoConfirmationMDAUpdatedMultipleAsset = new AutoConfirmationMDAUpdatedMultipleAssets()
                                {
                                    RecipientEmail = userByUsername.Username,
                                    RecipientName = userByUsername.FullName,
                                    Locations = string.Empty
                                };
                                StringBuilder stringBuilder = new StringBuilder();
                                StringBuilder stringBuilder1 = new StringBuilder();
                                for (int i = 0; i < assetDescriptionModels.Count; i++)
                                {
                                    AssetDescriptionModel assetByAssetId = this._asset.GetAssetByAssetId(assetDescriptionModels[i].AssetId);
                                    if (!autoConfirmationMDAUpdatedMultipleAsset.Locations.Contains(assetByAssetId.CityStateFormattedString))
                                    {
                                        stringBuilder.Append(assetByAssetId.CityStateFormattedString);
                                        if (i == assetDescriptionModels.Count - 2)
                                        {
                                            stringBuilder.Append(", and ");
                                        }
                                        else if (i < assetDescriptionModels.Count - 1)
                                        {
                                            stringBuilder.Append(", ");
                                        }
                                    }
                                    stringBuilder1.Append(assetByAssetId.AssetNumber);
                                    if (i == assetDescriptionModels.Count - 2)
                                    {
                                        stringBuilder1.Append(", and ");
                                    }
                                    else if (i < assetDescriptionModels.Count - 1)
                                    {
                                        stringBuilder1.Append(", ");
                                    }
                                }
                                autoConfirmationMDAUpdatedMultipleAsset.AssetNumbers = stringBuilder1.ToString();
                                autoConfirmationMDAUpdatedMultipleAsset.Locations = stringBuilder.ToString();
                                if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
                                {
                                    this._email.Send(autoConfirmationMDAUpdatedMultipleAsset);
                                }
                                return base.RedirectToAction("MyUSCPage", "Home");
                            }
                            TempDataDictionary item1 = base.TempData;
                            userId = userByUsername.UserId;
                            string str1 = string.Concat(userId.ToString(), "Assets");
                            TempDataDictionary tempDataDictionaries1 = base.TempData;
                            userId = userByUsername.UserId;
                            item1[str1] = tempDataDictionaries1[string.Concat(userId.ToString(), "Assets")];
                        }
                    }
                }
            }
            return base.View();
        }

        [HttpGet]
        public FileResult DownloadICAgreementTemplate()
        {
            return this.File(Properties.Resources.ICAgreementTemplateWOSig, "application/octet-stream", "download.pdf");
        }

        [HttpGet]
        public FileResult DownloadIPABuyerTemplate()
        {
            return this.File(Properties.Resources.IPABuyerWOSig, "application/octet-stream", "download.pdf");
        }

        [HttpGet]
        public FileResult DownloadIPANARTemplate()
        {
            return this.File(Properties.Resources.IPANARWOSig, "application/octet-stream", "download.pdf");
        }

        [HttpGet]
        public FileResult DownloadIPASellerTemplate()
        {
            return this.File(Properties.Resources.IPASellerWOSig, "application/octet-stream", "download.pdf");
        }

        [HttpGet]
        public FileResult DownloadJVTemplate()
        {
            return this.File(Properties.Resources.JVTemplateWOSig, "application/octet-stream", "download.pdf");
        }

        [HttpGet]
        public FileResult DownloadLOITemplate()
        {
            return this.File(Properties.Resources.LOITemplate, "application/octet-stream", "download.pdf");
        }

        [HttpGet]
        public FileResult DownloadNCNDTemplate()
        {
            return this.File(Properties.Resources.NCNDTemplateWOSig, "application/octet-stream", "download.pdf");
        }

        [HttpGet]
        public FileResult DownloadPFSTemplate()
        {
            return this.File(Properties.Resources.PersonalFinancialStatementTemplate, "application/octet-stream", "download.pdf");
        }

        [HttpGet]
        public ActionResult EditAsset(Guid id)
        {
            return base.RedirectToAction("EditAssetTemplate", new { id = id });
        }

        [HttpGet]
        public ActionResult EditInvestmentAssetSearchCriteria(int id)
        {
            AssetSearchCriteriaModel assetSearchCriteriaById = this._asset.GetAssetSearchCriteriaById(id);
            assetSearchCriteriaById.AssetTypes = this.populateAssetTypeDDL();
            return base.View(assetSearchCriteriaById);
        }

        [Authorize]
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "SaveSearchCriteria")]
        public ActionResult EditInvestmentAssetSearchCriteria(AssetSearchCriteriaModel model)
        {
            model.AssetTypes = this.populateAssetTypeDDL();
            if (model.TypeOfAssetsSought == AssetType.MHP || model.TypeOfAssetsSought == AssetType.MultiFamily)
            {
                foreach (string _commercialRequirement in this._commercialRequirements)
                {
                    if (!base.ModelState.ContainsKey(_commercialRequirement))
                    {
                        continue;
                    }
                    base.ModelState[_commercialRequirement].Errors.Clear();
                }
            }
            else
            {
                foreach (string _multifamilyRequirement in this._multifamilyRequirements)
                {
                    if (!base.ModelState.ContainsKey(_multifamilyRequirement))
                    {
                        continue;
                    }
                    base.ModelState[_multifamilyRequirement].Errors.Clear();
                }
            }
            if (base.ModelState.IsValid)
            {
                this._asset.UpdateAssetSearchCriteria(model);
                if (base.CanSendAutoEmail(BaseController.AutoEmailType.Asset))
                {
                    try
                    {
                        this._email.Send(new ConfirmationSearchCriteriaSubmittedEmail()
                        {
                            Email = base.User.Identity.Name,
                            To = model.NameOfPurchasingEntity
                        });
                    }
                    catch
                    {
                    }
                    List<UserQuickViewModel> users = this._user.GetUsers(new UserSearchModel()
                    {
                        UserTypeFilter = new UserType?(UserType.CorpAdmin),
                        ShowActiveOnly = true
                    });
                    users.AddRange(this._user.GetUsers(new UserSearchModel()
                    {
                        UserTypeFilter = new UserType?(UserType.CorpAdmin2),
                        ShowActiveOnly = true
                    }));
                    users.AddRange(this._user.GetUsers(new UserSearchModel()
                    {
                        UserTypeFilter = new UserType?(UserType.SiteAdmin),
                        ShowActiveOnly = true
                    }));
                    users.AddRange(this._user.GetUsers(new UserSearchModel()
                    {
                        UserTypeFilter = new UserType?(UserType.Investor),
                        ShowActiveOnly = true
                    }));
                    users.AddRange(this._user.GetUsers(new UserSearchModel()
                    {
                        UserTypeFilter = new UserType?(UserType.CREBroker),
                        ShowActiveOnly = true
                    }));
                    users.AddRange(this._user.GetUsers(new UserSearchModel()
                    {
                        UserTypeFilter = new UserType?(UserType.CRELender),
                        ShowActiveOnly = true
                    }));
                    users.AddRange(this._user.GetUsers(new UserSearchModel()
                    {
                        UserTypeFilter = new UserType?(UserType.CREOwner),
                        ShowActiveOnly = true
                    }));
                    users.AddRange(this._user.GetUsers(new UserSearchModel()
                    {
                        UserTypeFilter = new UserType?(UserType.ICAdmin),
                        ShowActiveOnly = true
                    }));
                    users.AddRange(this._user.GetUsers(new UserSearchModel()
                    {
                        UserTypeFilter = new UserType?(UserType.ListingAgent),
                        ShowActiveOnly = true
                    }));
                    UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
                    foreach (UserQuickViewModel user in users)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(user.Username))
                            {
                                ((dynamic)base.ViewBag).CurrentActiveAssetCount = this._asset.GetCurrentActiveAssetCount();
                                this._email.Send(new ConfirmationToAdminSearchCriteriaSubmitted()
                                {
                                    Email = user.Username,
                                    SearchCriteriaDate = DateTime.Now,
                                    To = string.Concat(user.FirstName, " ", user.LastName),
                                    SearchCriteriaName = userByUsername.FullName
                                });
                            }
                        }
                        catch (Exception exception)
                        {
                        }
                    }
                }
                base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully updated.");
            }
            return base.View("EditInvestmentAssetSearchCriteria", model);
        }

        [HttpGet]
        public ViewResult EditPersonalFinancialStatement(int id)
        {
            return base.View(this._user.GetPersonalFinancialStatementByUserId(id));
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Update")]
        [ValidateInput(false)]
        public ActionResult EditPersonalFinancialStatement(PersonalFinancialStatementTemplateModel model, string EditPFS)
        {
            if (!base.ModelState.IsValid)
            {
                return base.View("EditPersonalFinancialStatement", model);
            }
            this._user.UpdatePersonalFinancialStatement(model);
            base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully saved.");
            return base.RedirectToAction("MyUSCPage", "Home");
        }

        [HttpGet]
        public ActionResult Execute1099()
        {
            return base.View(new MiscellaneousIncomeTemplateModel());
        }

        [HttpPost]
        public ActionResult Execute1099(MiscellaneousIncomeTemplateModel model)
        {
            MemoryStream memoryStream = this._pdf.Submit1099(model);
            DateTime now = DateTime.Now;
            return new PDFResult(memoryStream, true, string.Concat("f1099form", now.ToString("yyyyMMddhhmmss"), ".pdf"));
        }

        [HttpGet]
        public ActionResult ExecuteICAgreement(bool bypass = false)
        {
            ICAgreementTemplateModel cAgreementTemplateModel = new ICAgreementTemplateModel();
            if (base.User.Identity.IsAuthenticated)
            {
                UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
                if (!bypass && this._user.HasPendingICAgreement(userByUsername.UserId))
                {
                    return base.View("WaitingForDocument", new WaitingForDocumentModel()
                    {
                        Action = "ExecuteICAgreement",
                        DocumentTitle = "IC Agreement"
                    });
                }
                cAgreementTemplateModel.City = userByUsername.City;
                cAgreementTemplateModel.UserAddressLine1 = userByUsername.AddressLine1;
                cAgreementTemplateModel.UserAddressLine2 = userByUsername.AddressLine2;
                cAgreementTemplateModel.State = userByUsername.SelectedState;
                cAgreementTemplateModel.UserFirstName = userByUsername.FirstName;
                cAgreementTemplateModel.UserLastName = userByUsername.LastName;
                cAgreementTemplateModel.Email = userByUsername.Username;
                cAgreementTemplateModel.Zip = userByUsername.Zip;
                cAgreementTemplateModel.CorpTitle = userByUsername.CorporateTitle;
                cAgreementTemplateModel.AcronymOfCorporateEntity = userByUsername.AcroynmForCorporateEntity;
                cAgreementTemplateModel.CorporateTIN = userByUsername.CorporateTIN;
                cAgreementTemplateModel.LicenseDesc = userByUsername.LicenseDesc;
                cAgreementTemplateModel.LicenseNumber = userByUsername.LicenseNumber;
            }
            return base.View(cAgreementTemplateModel);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ExecuteICAgreement")]
        public ActionResult ExecuteICAgreement(ICAgreementTemplateModel model)
        {
            if (base.ModelState.IsValid)
            {
                int? nullable = null;
                string str = "";
                if (!this._user.UserExists(model.Email))
                {
                    try
                    {
                        string str1 = this._user.GeneratePassword();
                        nullable = new int?(this._user.CreateUser(model.Email, this._user.HashPassword(str1), UserType.ICAdmin));
                        base.AuthenticateMachine(model.Email);
                        base.CreateCookie(false, new UserDataModel()
                        {
                            Username = model.Email,
                            IPAddress = base.GetIPAddress(),
                            MachineName = base.DetermineCompName(),
                            SecurityCode = ""
                        });
                        if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
                        {
                            this._email.Send(new AutoRegistrationAutoWelcomeEmail()
                            {
                                To = string.Concat(model.UserFirstName, " ", model.UserLastName),
                                Email = model.Email,
                                Password = str1
                            });
                        }
                        str = "You've been automatically registered. You will receive an email with your password.";
                    }
                    catch (Exception exception1)
                    {
                        Exception exception = exception1;
                        base.TempData["message"] = new MessageViewModel(MessageTypes.Error, string.Concat("Invalid email. Error: ", exception.Message));
                    }
                }
                else if (!base.User.Identity.IsAuthenticated)
                {
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "There is an account with this email already registered in our system. Please login and resubmit this form.");
                }
                else
                {
                    UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
                    nullable = new int?(userByUsername.UserId);
                }
                if (nullable.HasValue)
                {
                    this._pdf.SubmitICAgreement(model, Properties.Resources.ICAgreementTemplate, nullable.Value);
                    this._user.SetICInformation(model, nullable.Value);
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Success, string.Concat("Successfully executed. Please wait for an email from DocuSign to sign document.", str));
                    return base.RedirectToAction("DocusignProcess");
                }
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Please login and resubmit this form.");
            }
            return base.View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult ExecuteIPA()
        {
            IPABuyerTemplateModel pABuyerTemplateModel = new IPABuyerTemplateModel();
            if (base.User.Identity.IsAuthenticated)
            {
                UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
                if (base.TempData[string.Concat(userByUsername.UserId.ToString(), "Assets")] == null)
                {
                    return base.RedirectToAction("InvestorOpportunities", "Home");
                }
                TempDataDictionary tempData = base.TempData;
                int userId = userByUsername.UserId;
                List<Guid> item = tempData[string.Concat(userId.ToString(), "Assets")] as List<Guid>;
                if (item == null)
                {
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "There were no assets specified.");
                    return base.RedirectToAction("InvestorOpportunities", "Home");
                }
                foreach (Guid guid in item)
                {
                    AssetDescriptionModel assetByAssetId = this._asset.GetAssetByAssetId(guid);
                    pABuyerTemplateModel.Assets.Add(assetByAssetId);
                }
                if (userByUsername.UserType != UserType.CorpAdmin && userByUsername.UserType != UserType.CorpAdmin2)
                {
                    UserType userType = userByUsername.UserType;
                }
                pABuyerTemplateModel.City = userByUsername.City;
                pABuyerTemplateModel.UserAddressLine1 = userByUsername.AddressLine1;
                pABuyerTemplateModel.UserAddressLine2 = userByUsername.AddressLine2;
                pABuyerTemplateModel.State = userByUsername.SelectedState;
                pABuyerTemplateModel.UserFirstName = userByUsername.FirstName;
                pABuyerTemplateModel.UserLastName = userByUsername.LastName;
                pABuyerTemplateModel.Fax = userByUsername.FaxNumber;
                pABuyerTemplateModel.Phone = userByUsername.CellNumber;
                pABuyerTemplateModel.Email = userByUsername.Username;
                pABuyerTemplateModel.CompanyName = userByUsername.CompanyName;
                pABuyerTemplateModel.CorpTitle = userByUsername.CorporateTitle;
                pABuyerTemplateModel.FirstNameOfCorporateEntity = userByUsername.AcroynmForCorporateEntity;
                TempDataDictionary tempDataDictionaries = base.TempData;
                userId = userByUsername.UserId;
                string str = string.Concat(userId.ToString(), "Assets");
                TempDataDictionary tempData1 = base.TempData;
                userId = userByUsername.UserId;
                tempDataDictionaries[str] = tempData1[string.Concat(userId.ToString(), "Assets")];
            }
            return base.View(pABuyerTemplateModel);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ExecuteIPA")]
        public ActionResult ExecuteIPA(IPABuyerTemplateModel model)
        {
            if (!base.ModelState.IsValid)
            {
                return base.View(model);
            }
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            this._pdf.SubmitIPABuyer(model, Properties.Resources.IPABuyerWSig, userByUsername.UserId, 1);
            TempDataDictionary tempData = base.TempData;
            int userId = userByUsername.UserId;
            tempData[string.Concat(userId.ToString(), "Assets")] = model.Assets;
            base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully executed. Please wait for an email from DocuSign to sign document.");
            return base.RedirectToAction("DocusignProcess");
        }

        [HttpGet]
        public ActionResult ExecuteJVAgreement(bool bypass = false)
        {
            JVAgreementTemplateModel jVAgreementTemplateModel = new JVAgreementTemplateModel();
            if (!base.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/Home/Login?rurl=/DataPortal/ExecuteJVAgreement");
            }
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            if (!bypass && this._user.HasPendingJVAgreement(userByUsername.UserId))
            {
                return base.View("WaitingForDocument", new WaitingForDocumentModel()
                {
                    Action = "ExecuteJVAgreement",
                    DocumentTitle = "JV Agreement"
                });
            }
            jVAgreementTemplateModel.Email = userByUsername.Username;
            jVAgreementTemplateModel.UserFirstName = userByUsername.FirstName;
            jVAgreementTemplateModel.UserLastName = userByUsername.LastName;
            jVAgreementTemplateModel.CorpTitle = userByUsername.CorporateTitle;
            jVAgreementTemplateModel.StateOfOriginOfCorporateEntity = userByUsername.StateOfOriginCorporateEntity;
            jVAgreementTemplateModel.CompanyName = userByUsername.CompanyName;
            jVAgreementTemplateModel.AcronymOfCorporateEntity = userByUsername.AcroynmForCorporateEntity;
            DateTime now = DateTime.Now;
            jVAgreementTemplateModel.Year = now.Year;
            jVAgreementTemplateModel.Month = now.ToString("MMMM");
            jVAgreementTemplateModel.Day = now.Day;
            return base.View(jVAgreementTemplateModel);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ExecuteJVAgreement")]
        public ActionResult ExecuteJVAgreement(JVAgreementTemplateModel model)
        {
            DateTime now = DateTime.Now;
            model.Year = now.Year;
            model.Day = now.Day;
            model.Month = now.ToString("MMMM");
            if (!base.ModelState.IsValid)
            {
                return base.View("ExecuteJVAgreement", model);
            }
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            this._pdf.SubmitJVAgreement(model, Properties.Resources.JVTemplate, userByUsername.UserId);
            base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully executed. Please wait for an email from DocuSign to sign document.");
            return base.RedirectToAction("DocusignProcess");
        }

        [Authorize]
        [HttpGet]
        public ActionResult ExecuteLOI(Guid id)
        {
            ActionResult actionResult;
            try
            {
                AssetViewModel asset = this._asset.GetAsset(id, false);
                UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
                BindingContingentTemplateModel bindingContingentTemplateModel = new BindingContingentTemplateModel()
                {
                    AssetNumber = asset.AssetNumber
                };
                foreach (AssetNARMember assetNARMember in asset.AssetNARMembers)
                {
                    bindingContingentTemplateModel.ListingAgents.Add(new SelectListItem()
                    {
                        Text = assetNARMember.NARMember.FullName,
                        Value = assetNARMember.NarMemberId.ToString()
                    });
                    bindingContingentTemplateModel.AssetNARMembers.Add(assetNARMember);
                }
                bindingContingentTemplateModel.ListingAgentCount = bindingContingentTemplateModel.AssetNARMembers.Count;
                bindingContingentTemplateModel.TotalNumberOfPagesIncludingCover = 6;
                for (int i = 0; i < asset.AssetTaxParcelNumbers.Count; i++)
                {
                    if (asset.AssetTaxParcelNumbers.Count - 1 != i)
                    {
                        BindingContingentTemplateModel bindingContingentTemplateModel1 = bindingContingentTemplateModel;
                        bindingContingentTemplateModel1.AssessorNumber = string.Concat(bindingContingentTemplateModel1.AssessorNumber, asset.AssetTaxParcelNumbers[i].TaxParcelNumber, ", ");
                    }
                    else
                    {
                        BindingContingentTemplateModel bindingContingentTemplateModel2 = bindingContingentTemplateModel;
                        bindingContingentTemplateModel2.AssessorNumber = string.Concat(bindingContingentTemplateModel2.AssessorNumber, asset.AssetTaxParcelNumbers[i].TaxParcelNumber);
                    }
                }
                DateTime now = DateTime.Now;
                bindingContingentTemplateModel.LOIId = Guid.NewGuid();
                bindingContingentTemplateModel.LOIDate = now.AddDays(14);
                bindingContingentTemplateModel.AssetId = asset.AssetId;
                bindingContingentTemplateModel.From = string.Concat(userByUsername.FirstName, " ", userByUsername.LastName);
                bindingContingentTemplateModel.Date = DateTime.Now;
                bindingContingentTemplateModel.CareOf = userByUsername.CompanyName;
                bindingContingentTemplateModel.BusinessPhoneNumber = userByUsername.WorkNumber;
                bindingContingentTemplateModel.CREAquisitionLOI = asset.ProjectName;
                bindingContingentTemplateModel.EmailAddress2 = userByUsername.Username;
                if (!asset.IsTBDMarket)
                {
                    bindingContingentTemplateModel.OfferingPurchasePrice = asset.AskingPrice;
                }
                else
                {
                    bindingContingentTemplateModel.OfferingPurchasePrice = asset.CurrentBpo;
                }
                bindingContingentTemplateModel.BeneficiarySeller = userByUsername.CompanyName;
                bindingContingentTemplateModel.OfficePhone = (asset.ListedByRealtor ? "---" : userByUsername.WorkNumber);
                bindingContingentTemplateModel.OfficerOfSeller = (asset.ListedByRealtor ? "---" : string.Concat(userByUsername.FirstName, " ", userByUsername.LastName));
                bindingContingentTemplateModel.WebsiteEmail = (asset.ListedByRealtor ? "---" : userByUsername.Username);
                bindingContingentTemplateModel.BuyerAssigneeName = EnumHelper.GetEnumDescription(userByUsername.SelectedCorporateEntityType);
                bindingContingentTemplateModel.Buyer1Name = userByUsername.CompanyName;
                bindingContingentTemplateModel.ObjectOfPurchase = EnumHelper.GetEnumDescription(asset.AssetType);
                bindingContingentTemplateModel.SecuredMortgages = "N/A";
                bindingContingentTemplateModel.Lender = "N/A";
                bindingContingentTemplateModel.NoSecuredMortgages = false;
                bindingContingentTemplateModel.Releasing = userByUsername.CompanyName;
                bindingContingentTemplateModel.FormalDocumentationDate = "Seven";
                bindingContingentTemplateModel.FormalDocumentationNumberOfDays = "7";
                bindingContingentTemplateModel.WebsiteEmail = asset.WebsiteEmail;
                bindingContingentTemplateModel.OfficerOfSeller = asset.OfficerOfSeller;
                bindingContingentTemplateModel.StateOfPropertyTaxOffice = asset.State;
                bindingContingentTemplateModel.StateOfCountyAssessors = asset.County;
                bindingContingentTemplateModel.Address1 = asset.PropertyAddress;
                bindingContingentTemplateModel.Address2 = asset.PropertyAddress2;
                bindingContingentTemplateModel.City = asset.City;
                bindingContingentTemplateModel.State = asset.State;
                bindingContingentTemplateModel.Zip = asset.Zip;
                bindingContingentTemplateModel.EscrowCompanies = this._asset.GetEscrowCompanies();
                bindingContingentTemplateModel.EscrowCompanyName = asset.EscrowCompany;
                bindingContingentTemplateModel.EscrowCompanyAddress = asset.EscrowCompanyAddress;
                bindingContingentTemplateModel.EscrowCompanyPhoneNumber = asset.EscrowCompanyPhoneNumber;
                bindingContingentTemplateModel.Buyer1 = string.Concat(userByUsername.FirstName, " ", userByUsername.LastName);
                bindingContingentTemplateModel.BuyerTitle1 = userByUsername.CorporateTitle;
                actionResult = base.View(bindingContingentTemplateModel);
            }
            catch (Exception exception)
            {
                base.TempData["message"] = (!base.User.Identity.IsAuthenticated ? new MessageViewModel(MessageTypes.Error, "You need to login to continue.") : new MessageViewModel(MessageTypes.Error, "Invalid asset ID."));
                BindingContingentTemplateModel bindingContingentTemplateModel3 = new BindingContingentTemplateModel();
                DateTime dateTime = DateTime.Now;
                actionResult = base.View(bindingContingentTemplateModel3);
            }
            return actionResult;
        }

        [Authorize]
        [HttpPost]
        public ActionResult ExecuteLOI(BindingContingentTemplateModel model, string SubmitLOI)
        {
            if (!base.ModelState.IsValid)
            {
                return base.View(model);
            }
            if (SubmitLOI == "Preview")
            {
                return this.File(this._pdf.PreviewLOI(model, Properties.Resources.LOITemplate), "application/octet-stream", "preview.pdf");
            }
            bool flag = false;
            bool flag1 = false;
            int num = 0;
            if (model.Documents == null)
            {
                model.Documents = new List<LOIDocument>();
            }
            foreach (LOIDocument document in model.Documents)
            {
                int type = (int)document.Type;
                if (type == 1)
                {
                    flag1 = true;
                }
                else if (type == 2)
                {
                    flag = true;
                }
            }
            if (!(flag & flag1))
            {
                string empty = string.Empty;
                if (!flag1)
                {
                    empty = string.Concat(empty, "Operating & Management Resumes document required. ");
                }
                if (!flag)
                {
                    empty = string.Concat(empty, "Proof of Funds document required.");
                }
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, empty);
                return base.View(model);
            }
            for (int i = 0; i < model.Documents.Count; i++)
            {
                bool flag2 = false;
                do
                {
                    if (i >= model.Documents.Count)
                    {
                        break;
                    }
                    if (model.Documents[i].FileName != null)
                    {
                        model.Documents[i].Order = num;
                        num++;
                        flag2 = false;
                    }
                    else
                    {
                        model.Documents.RemoveAt(i);
                        flag2 = true;
                    }
                }
                while (flag2);
            }
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            this._asset.SaveLOI(model, userByUsername.UserId);
            try
            {
                this._pdf.SubmitLOI(model, Properties.Resources.LOITemplate, userByUsername.UserId);
            }
            catch (Exception exception)
            {
            }
            if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
            {
                this._email.Send(new ConfirmationLOISubmittalEmail()
                {
                    UserEmail = model.EmailAddress2,
                    UserName = model.Buyer1Name,
                    MBAEmail = model.EmailAddress,
                    MBAMember = model.To,
                    MBACorporateName = model.Company,
                    MBAPhone = model.BusinessPhoneNumber,
                    MBACorporateAddress = model.EscrowCompanyAddress
                });
            }
            this._user.LogUserRecord(UserRecordType.LOISubmittal, userByUsername.UserId);
            base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully executed. Please wait for an email from DocuSign to sign document.");
            Guid assetId = model.AssetId;
            return base.RedirectToAction("ExecuteLOI", new { id = assetId.ToString() });
        }

        [HttpGet]
        public ActionResult ExecuteMDA(Guid id, bool bypass = false)
        {
            IPABuyerTemplateModel pABuyerTemplateModel = new IPABuyerTemplateModel();
            AssetDescriptionModel assetByAssetId = this._asset.GetAssetByAssetId(id);
            pABuyerTemplateModel.Assets.Add(assetByAssetId);
            if (base.User.Identity.IsAuthenticated)
            {
                UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
                if (userByUsername.UserType == UserType.CorpAdmin || userByUsername.UserType == UserType.CorpAdmin2 || userByUsername.UserType == UserType.SiteAdmin)
                {
                    return base.RedirectToAction("ViewAsset", new { id = id.ToString() });
                }
                if (!bypass && this._user.HasPendingMDA(userByUsername.UserId, assetByAssetId.AssetNumber))
                {
                    return base.View("WaitingForDocument", new WaitingForDocumentModel()
                    {
                        Action = "ExecuteMDA",
                        DocumentTitle = "MDA Agreement"
                    });
                }
                pABuyerTemplateModel.City = userByUsername.City;
                pABuyerTemplateModel.UserAddressLine1 = userByUsername.AddressLine1;
                pABuyerTemplateModel.UserAddressLine2 = userByUsername.AddressLine2;
                pABuyerTemplateModel.State = userByUsername.SelectedState;
                pABuyerTemplateModel.UserFirstName = userByUsername.FirstName;
                pABuyerTemplateModel.UserLastName = userByUsername.LastName;
                pABuyerTemplateModel.Fax = userByUsername.FaxNumber;
                pABuyerTemplateModel.Phone = userByUsername.CellNumber;
                pABuyerTemplateModel.Email = userByUsername.Username;
                pABuyerTemplateModel.CompanyName = userByUsername.CompanyName;
                pABuyerTemplateModel.CorpTitle = userByUsername.CorporateTitle;
                pABuyerTemplateModel.FirstNameOfCorporateEntity = userByUsername.AcroynmForCorporateEntity;
            }
            return base.View(pABuyerTemplateModel);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ExecuteMDA")]
        public ActionResult ExecuteMDA(IPABuyerTemplateModel model)
        {
            if (!base.ModelState.IsValid)
            {
                return base.View(model);
            }
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            this._pdf.SubmitIPABuyer(model, Properties.Resources.IPABuyerWSig, userByUsername.UserId, 1);
            base.TempData["MDAAssets"] = model.Assets;
            base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully executed. Please wait for an email from DocuSign to sign document.");
            return base.RedirectToAction("DocusignProcess");
        }

        [HttpGet]
        public ActionResult ExecuteMDAs()
        {
            if (!base.User.Identity.IsAuthenticated)
            {
                return base.RedirectToAction("Login", "Home");
            }
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            TempDataDictionary tempData = base.TempData;
            int userId = userByUsername.UserId;
            List<Guid> item = tempData[string.Concat(userId.ToString(), "Assets")] as List<Guid>;
            if (item == null)
            {
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "There was an error retrieving your selected assets.");
                return base.RedirectToAction("MyUSCPage", "Home");
            }
            DateTime? mDASignedDate = this._user.GetMDASignedDate(userByUsername.UserId);
            if (!mDASignedDate.HasValue)
            {
                if (item.Count <= 0)
                {
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Please select at least one asset to proceed.");
                    return base.RedirectToAction("InvestorOpportunities", "Home");
                }
                TempDataDictionary tempDataDictionaries = base.TempData;
                userId = userByUsername.UserId;
                tempDataDictionaries[string.Concat(userId.ToString(), "Assets")] = item;
                return base.RedirectToAction("ExecuteIPA", "DataPortal");
            }
            NewAssetToMdaModel newAssetToMdaModel = new NewAssetToMdaModel()
            {
                OriginalMDASignDate = mDASignedDate.Value,
                UserId = userByUsername.UserId
            };
            List<Guid> list = (
                from s in this._asset.GetSignedMDAs(userByUsername.UserId)
                select s.AssetId).ToList<Guid>();
            foreach (Guid guid in item)
            {
                if (list.Contains(guid))
                {
                    continue;
                }
                newAssetToMdaModel.AssetIdsToAdd.Add(guid);
                newAssetToMdaModel.AssetsToAdd.Add(this._asset.GetAssetByAssetId(guid));
            }
            return base.View("AddAssetToMDA", newAssetToMdaModel);
        }

        [HttpPost]
        public ActionResult ExecuteMDAs(IEnumerable<AssetQuickListViewModel> items)
        {
            List<Guid> guids = new List<Guid>();
            items.ToList<AssetQuickListViewModel>().ForEach((AssetQuickListViewModel item) =>
            {
                if (item.IsSelected)
                {
                    guids.Add(item.AssetId);
                }
            });
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            DateTime? mDASignedDate = this._user.GetMDASignedDate(userByUsername.UserId);
            if (!mDASignedDate.HasValue)
            {
                if (guids.Count <= 0)
                {
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Please select at least one asset to proceed.");
                    return base.RedirectToAction("InvestorOpportunities", "Home");
                }
                TempDataDictionary tempData = base.TempData;
                int userId = userByUsername.UserId;
                tempData[string.Concat(userId.ToString(), "Assets")] = guids;
                return base.RedirectToAction("ExecuteIPA", "DataPortal");
            }
            NewAssetToMdaModel newAssetToMdaModel = new NewAssetToMdaModel()
            {
                OriginalMDASignDate = mDASignedDate.Value,
                UserId = userByUsername.UserId
            };
            List<Guid> list = (
                from s in this._asset.GetSignedMDAs(userByUsername.UserId)
                select s.AssetId).ToList<Guid>();
            foreach (Guid guid in guids)
            {
                if (list.Contains(guid))
                {
                    continue;
                }
                newAssetToMdaModel.AssetIdsToAdd.Add(guid);
                newAssetToMdaModel.AssetsToAdd.Add(this._asset.GetAssetByAssetId(guid));
            }
            return base.View("AddAssetToMDA", newAssetToMdaModel);
        }

        [HttpGet]
        public ActionResult ExecuteMDATemplate()
        {
            IPABuyerTemplateModel pABuyerTemplateModel = new IPABuyerTemplateModel();
            if (base.User.Identity.IsAuthenticated)
            {
                UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
                if (userByUsername.UserType != UserType.CorpAdmin && userByUsername.UserType != UserType.CorpAdmin2)
                {
                    UserType userType = userByUsername.UserType;
                }
                pABuyerTemplateModel.City = userByUsername.City;
                pABuyerTemplateModel.UserAddressLine1 = userByUsername.AddressLine1;
                pABuyerTemplateModel.UserAddressLine2 = userByUsername.AddressLine2;
                pABuyerTemplateModel.State = userByUsername.SelectedState;
                pABuyerTemplateModel.UserFirstName = userByUsername.FirstName;
                pABuyerTemplateModel.UserLastName = userByUsername.LastName;
                pABuyerTemplateModel.Fax = userByUsername.FaxNumber;
                pABuyerTemplateModel.Phone = userByUsername.CellNumber;
                pABuyerTemplateModel.Email = userByUsername.Username;
                pABuyerTemplateModel.CompanyName = userByUsername.CompanyName;
                pABuyerTemplateModel.CorpTitle = userByUsername.CorporateTitle;
                pABuyerTemplateModel.FirstNameOfCorporateEntity = userByUsername.AcroynmForCorporateEntity;
            }
            return base.View("ExecuteMDA", pABuyerTemplateModel);
        }

        [HttpGet]
        public ActionResult ExecuteNCND(bool bypass = false)
        {
            NCNDTemplateModel nCNDTemplateModel = new NCNDTemplateModel();
            if (base.User.Identity.IsAuthenticated)
            {
                UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
                nCNDTemplateModel.City = userByUsername.City;
                nCNDTemplateModel.UserAddressLine1 = userByUsername.AddressLine1;
                nCNDTemplateModel.UserAddressLine2 = userByUsername.AddressLine2;
                nCNDTemplateModel.Zip = userByUsername.Zip;
                nCNDTemplateModel.State = userByUsername.SelectedState;
                nCNDTemplateModel.UserFirstName = userByUsername.FirstName;
                nCNDTemplateModel.UserLastName = userByUsername.LastName;
                nCNDTemplateModel.Fax = userByUsername.FaxNumber;
                nCNDTemplateModel.Phone = userByUsername.CellNumber;
                nCNDTemplateModel.Email = userByUsername.Username;
                nCNDTemplateModel.CompanyName = userByUsername.CompanyName;
                nCNDTemplateModel.CorpTitle = userByUsername.CorporateTitle;
                nCNDTemplateModel.AcronymOfCorporateEntity = userByUsername.AcroynmForCorporateEntity;
            }
            return base.View(nCNDTemplateModel);
        }

        [HttpPost]
        public ActionResult ExecuteNCND(NCNDTemplateModel model)
        {
            if (!base.ModelState.IsValid)
            {
                return base.View(model);
            }
            model.CompanyTypeDescription = EnumHelper.GetEnumDescription(model.TypeOfCorporateEntity);
            model.DayWithSuffix = this._pdf.GetDaySuffix(model.Day);
            return base.View("AcceptNCND", model);
        }

        [HttpGet]
        public ActionResult ExecutePersonalFinancialStatement()
        {
            if (!base.User.Identity.IsAuthenticated)
            {
                return base.RedirectToAction("Login", "Home");
            }
            PersonalFinancialStatementTemplateModel personalFinancialStatementTemplateModel = new PersonalFinancialStatementTemplateModel();
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            PersonalFinancialStatementTemplateModel personalFinancialStatementByUserId = this._user.GetPersonalFinancialStatementByUserId(userByUsername.UserId);
            if (personalFinancialStatementByUserId != null)
            {
                personalFinancialStatementTemplateModel = personalFinancialStatementByUserId;
            }
            else
            {
                personalFinancialStatementTemplateModel.City = userByUsername.City;
                personalFinancialStatementTemplateModel.ResidentialAddress = userByUsername.AddressLine1;
                personalFinancialStatementTemplateModel.State = userByUsername.SelectedState;
                personalFinancialStatementTemplateModel.BusinessName = userByUsername.CompanyName;
                personalFinancialStatementTemplateModel.BusinessPhone = userByUsername.WorkNumber;
                personalFinancialStatementTemplateModel.UserFirstName = userByUsername.FirstName;
                personalFinancialStatementTemplateModel.UserLastName = userByUsername.LastName;
                personalFinancialStatementTemplateModel.Email = userByUsername.Username;
                personalFinancialStatementTemplateModel.ResidentialPhone = userByUsername.CellNumber;
                personalFinancialStatementTemplateModel.Zip = userByUsername.Zip;
            }
            DateTime now = DateTime.Now;
            personalFinancialStatementTemplateModel.Year = now.Year;
            personalFinancialStatementTemplateModel.Day = now.Day;
            personalFinancialStatementTemplateModel.Month = now.ToString("MMMM");
            return base.View(personalFinancialStatementTemplateModel);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Submit")]
        [ValidateInput(false)]
        public ActionResult ExecutePersonalFinancialStatement(PersonalFinancialStatementTemplateModel model, string SubmitPFS)
        {
            DateTime now = DateTime.Now;
            model.Year = now.Year;
            model.Day = now.Day;
            model.Month = now.ToString("MMMM");
            if (!base.ModelState.IsValid)
            {
                return base.View("ExecutePersonalFinancialStatement", model);
            }
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            model.UserId = userByUsername.UserId;
            userByUsername.WorkNumber = model.BusinessPhone;
            userByUsername.CellNumber = model.ResidentialPhone;
            this._user.UpdateUser(userByUsername);
            this._user.CreatePersonalFinancialStatement(model);
            this._pdf.SubmitPFS(model, Properties.Resources.PersonalFinancialStatementTemplate, userByUsername.UserId);
            base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully executed. Please wait for an email from DocuSign to sign document.");
            return base.View("DocusignProcess");
        }

        [HttpGet]
        public ViewResult ImportMBAUsers()
        {
            ImportMbaModel importMbaModel = new ImportMbaModel();
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            if (!base.ValidateAdminUser(userByUsername))
            {
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
                return base.View("~/Views/Home/Index.cshtml");
            }
            if (userByUsername.UserType == UserType.CorpAdmin || userByUsername.UserType == UserType.CorpAdmin2 || userByUsername.UserType == UserType.SiteAdmin)
            {
                ((dynamic)base.ViewBag).Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
                importMbaModel.ShowCheckbox = true;
            }
            else
            {
                ((dynamic)base.ViewBag).Layout = "~/Views/Shared/_Layout.cshtml";
            }
            return base.View(importMbaModel);
        }

        [HttpPost]
        public ActionResult ImportMBAUsers(ImportMbaModel model)
        {
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            if (!base.ValidateAdminUser(userByUsername))
            {
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
                return base.RedirectToAction("Index", "Home");
            }
            if (model.File == null)
            {
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "No file found. Please select a file.");
            }
            else
            {
                string extension = Path.GetExtension(model.File.FileName);
                if (extension == ".xls" || extension == ".xlsx")
                {
                    try
                    {
                        string str = Path.Combine(ConfigurationManager.AppSettings["DataRoot"], "ImportFiles");
                        if (!Directory.Exists(str))
                        {
                            Directory.CreateDirectory(str);
                        }
                        string str1 = Path.Combine(str, string.Concat(Guid.NewGuid(), extension));
                        model.File.SaveAs(str1);
                        string str2 = this._user.ImportMBAUsers(str1, userByUsername.UserId, model.AreReferredUsers, model.CheckAgainstNARMembers, model.CheckAgainstPrincipalInvestors);
                        this._email.Send(new ConfirmationImportMBAsEmail()
                        {
                            EmailAddress = userByUsername.Username,
                            Name = userByUsername.FullName,
                            Results = str2
                        });
                        base.TempData["message"] = new MessageViewModel(MessageTypes.Success, str2);
                    }
                    catch (OleDbException oleDbException)
                    {
                        base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "There was an error importing these MBA Members. You must use the template file provided in this website to complete this process.");
                    }
                    catch (Exception exception1)
                    {
                        Exception exception = exception1;
                        base.TempData["message"] = new MessageViewModel(MessageTypes.Error, string.Concat("There was an error importing these MBA Members. Please check file to make sure the format matches with the import template. Error: ", exception.Message));
                    }
                }
                else
                {
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid file type. Only excel spreadsheets are supported.");
                }
            }
            if (userByUsername.UserType == UserType.CorpAdmin || userByUsername.UserType == UserType.CorpAdmin2 || userByUsername.UserType == UserType.SiteAdmin)
            {
                ((dynamic)base.ViewBag).Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
                model.ShowCheckbox = true;
            }
            else
            {
                ((dynamic)base.ViewBag).Layout = "~/Views/Shared/_Layout.cshtml";
            }
            return base.View(model);
        }

        [HttpGet]
        public ViewResult ImportNarMembers()
        {
            ImportNarMemberModel importNarMemberModel = new ImportNarMemberModel();
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            if (!base.ValidateAdminUser(userByUsername))
            {
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
                return base.View("~/Views/Home/Index.cshtml");
            }
            if (userByUsername.UserType == UserType.CorpAdmin || userByUsername.UserType == UserType.CorpAdmin2 || userByUsername.UserType == UserType.SiteAdmin)
            {
                ((dynamic)base.ViewBag).Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
                importNarMemberModel.ShowCheckbox = true;
            }
            else
            {
                ((dynamic)base.ViewBag).Layout = "~/Views/Shared/_Layout.cshtml";
            }
            return base.View(importNarMemberModel);
        }

        [HttpPost]
        public ActionResult ImportNarMembers(ImportNarMemberModel model)
        {
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            if (!base.ValidateAdminUser(userByUsername))
            {
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
                return base.RedirectToAction("Index", "Home");
            }
            if (model.File == null)
            {
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Missing file. Please select a file.");
            }
            else
            {
                string extension = Path.GetExtension(model.File.FileName);
                if (extension == ".xls" || extension == ".xlsx")
                {
                    try
                    {
                        string str = Path.Combine(ConfigurationManager.AppSettings["DataRoot"], "ImportFiles");
                        if (!Directory.Exists(str))
                        {
                            Directory.CreateDirectory(str);
                        }
                        string str1 = Path.Combine(str, string.Concat(Guid.NewGuid(), extension));
                        model.File.SaveAs(str1);
                        string str2 = this._user.ImportNARMembers(str1, userByUsername.UserId, model.AreReferredUsers, model.CheckAgainstMBAs, model.CheckAgainstPrincipalInvestors);
                        this._email.Send(new ConfirmationImportNARMembersEmail()
                        {
                            EmailAddress = userByUsername.Username,
                            Name = userByUsername.FullName,
                            Results = str2
                        });
                        base.TempData["message"] = new MessageViewModel(MessageTypes.Success, str2);
                    }
                    catch (OleDbException oleDbException)
                    {
                        base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "There was an error importing these NAR members. You must use the template file provided in this website to complete this process.");
                    }
                    catch (Exception exception1)
                    {
                        Exception exception = exception1;
                        base.TempData["message"] = new MessageViewModel(MessageTypes.Error, string.Concat("There was an error importing these NAR members. Please check file to make sure the format matches with the import template. Error: ", exception.Message));
                    }
                }
                else
                {
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid file type. Only excel spreadsheets are supported.");
                }
            }
            if (userByUsername.UserType == UserType.CorpAdmin || userByUsername.UserType == UserType.CorpAdmin2 || userByUsername.UserType == UserType.SiteAdmin)
            {
                ((dynamic)base.ViewBag).Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
                model.ShowCheckbox = true;
            }
            else
            {
                ((dynamic)base.ViewBag).Layout = "~/Views/Shared/_Layout.cshtml";
            }
            return base.View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult ImportPrincipalInvestors()
        {
            ImportPrincipalInvestorModel importPrincipalInvestorModel = new ImportPrincipalInvestorModel();
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            if (userByUsername.UserType != UserType.CorpAdmin && userByUsername.UserType != UserType.CorpAdmin2 && userByUsername.UserType != UserType.SiteAdmin && userByUsername.UserType != UserType.CREBroker && userByUsername.UserType != UserType.CRELender)
            {
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
                return base.RedirectToAction("Index", "Home");
            }
            if (userByUsername.UserType == UserType.CorpAdmin || userByUsername.UserType == UserType.CorpAdmin2 || userByUsername.UserType == UserType.SiteAdmin)
            {
                ((dynamic)base.ViewBag).Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
                importPrincipalInvestorModel.ShowCheckbox = true;
            }
            else
            {
                ((dynamic)base.ViewBag).Layout = "~/Views/Shared/_Layout.cshtml";
            }
            return base.View(importPrincipalInvestorModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ImportPrincipalInvestors(ImportPrincipalInvestorModel model)
        {
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            if (userByUsername.UserType != UserType.CorpAdmin && userByUsername.UserType != UserType.CorpAdmin2 && userByUsername.UserType != UserType.SiteAdmin && userByUsername.UserType != UserType.CREBroker && userByUsername.UserType != UserType.CRELender)
            {
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
                return base.RedirectToAction("Index", "Home");
            }
            if (model.File == null)
            {
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "No file found. Please select a file.");
            }
            else
            {
                string extension = Path.GetExtension(model.File.FileName);
                if (extension == ".xls" || extension == ".xlsx")
                {
                    try
                    {
                        string str = Path.Combine(ConfigurationManager.AppSettings["DataRoot"], "ImportFiles");
                        if (!Directory.Exists(str))
                        {
                            Directory.CreateDirectory(str);
                        }
                        string str1 = Path.Combine(str, string.Concat(Guid.NewGuid(), extension));
                        model.File.SaveAs(str1);
                        ImportUsersModel importUsersModel = this._user.ImportPrincipalInvestors(str1, userByUsername.UserId, model.AreReferredUsers, model.CheckAgainstMBAs, model.CheckAgainstNARs);
                        this._email.Send(new ConfirmationImportPrincipalInvestorsEmail()
                        {
                            ViewName = "ConfirmationImportPrincipalInvestors",
                            EmailAddress = userByUsername.Username,
                            Name = userByUsername.FullName,
                            ResultString = importUsersModel.Message
                        });
                        if (model.AreReferredUsers)
                        {
                            this._email.Send(new ConfirmationImportPrincipalInvestorsEmail()
                            {
                                ViewName = "ConfirmationImportReferredPrincipalInvestors",
                                EmailAddress = userByUsername.Username,
                                Name = userByUsername.FullName,
                                ResultCount = importUsersModel.ReferredCount
                            });
                        }
                        base.TempData["message"] = new MessageViewModel(MessageTypes.Success, importUsersModel.Message);
                    }
                    catch (Exception exception1)
                    {
                        Exception exception = exception1;
                        base.TempData["message"] = new MessageViewModel(MessageTypes.Error, string.Concat("There was an error importing these Principal Investors. Please check file to make sure the format matches with the import template. Error: ", exception.Message));
                    }
                }
                else
                {
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid file type. Only excel spreadsheets are supported.");
                }
            }
            if (userByUsername.UserType == UserType.CorpAdmin || userByUsername.UserType == UserType.CorpAdmin2 || userByUsername.UserType == UserType.SiteAdmin)
            {
                ((dynamic)base.ViewBag).Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
                model.ShowCheckbox = true;
            }
            else
            {
                ((dynamic)base.ViewBag).Layout = "~/Views/Shared/_Layout.cshtml";
            }
            return base.View(model);
        }

        [HttpGet]
        public ActionResult InvestmentAssetSearchCriteria(string MinAgeRange, string MaxAgeRange, string MinPriceRange, string MaxPriceRange, string AssetCategory, string AssetType, string ListingStatus)
        {
            AssetSearchCriteriaModel assetSearchCriteriaModel = new AssetSearchCriteriaModel()
            {
                AssetTypes = this.populateAssetTypeDDL()
            };
            if (base.User.Identity.IsAuthenticated)
            {
                UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
                assetSearchCriteriaModel.UserId = userByUsername.UserId;
                assetSearchCriteriaModel.NameOfPurchasingEntity = userByUsername.CompanyName;
                assetSearchCriteriaModel.AddressLine1OfPurchasingEntity = userByUsername.AddressLine1;
                assetSearchCriteriaModel.ZipOfPurchasingEntity = userByUsername.Zip;
                assetSearchCriteriaModel.AddressLine2OfPurchasingEntity = userByUsername.AddressLine2;
                assetSearchCriteriaModel.CityOfPurchasingEntity = userByUsername.City;
                assetSearchCriteriaModel.StateOfPurchasingEntity = userByUsername.SelectedState;
                assetSearchCriteriaModel.TypeOfPurchasingEntity = userByUsername.SelectedCorporateEntityType;
                assetSearchCriteriaModel.ManagingOfficerOfEntity = userByUsername.FullName;
                assetSearchCriteriaModel.OfficeNumberOfEntity = userByUsername.WorkNumber;
                assetSearchCriteriaModel.CellNumberOfEntity = userByUsername.CellNumber;
                assetSearchCriteriaModel.FaxNumberOfEntity = userByUsername.FaxNumber;
                assetSearchCriteriaModel.EmailAddressOfEntity = userByUsername.Username;
                assetSearchCriteriaModel.StateOfIncorporation = userByUsername.StateOfOriginCorporateEntity;
                assetSearchCriteriaModel.IsCorporateEntityInGoodStanding = new bool?(userByUsername.IsCertificateOfGoodStandingAvailable);
            }
            if (!string.IsNullOrEmpty(MaxAgeRange))
            {
                int num = 0;
                if (int.TryParse(MaxAgeRange, out num) && num != 0)
                {
                    assetSearchCriteriaModel.MultiFamilyDemographicDetail.AgeOfPropertyMaximum = num;
                }
            }
            if (!string.IsNullOrEmpty(MinPriceRange))
            {
                int num1 = 0;
                if (int.TryParse(MinPriceRange, out num1) && num1 != 0)
                {
                    assetSearchCriteriaModel.FinancialInvestmentParameters.InvestmentFundingRangeMin = num1;
                }
            }
            if (!string.IsNullOrEmpty(MaxPriceRange))
            {
                int num2 = 0;
                if (int.TryParse(MaxPriceRange, out num2) && num2 != 0)
                {
                    assetSearchCriteriaModel.FinancialInvestmentParameters.InvestmentFundingRangeMax = num2;
                }
            }
            string.IsNullOrEmpty(AssetCategory);
            if (!string.IsNullOrEmpty(AssetType))
            {
                int num3 = 0;
                if (int.TryParse(AssetType, out num3) && num3 != 0)
                {
                    assetSearchCriteriaModel.TypeOfAssetsSought = (Inview.Epi.EpiFund.Domain.ViewModel.AssetType)num3;
                    assetSearchCriteriaModel.AssetTypes.Single<SelectListItem>((SelectListItem s) => s.Value == assetSearchCriteriaModel.TypeOfAssetsSought.ToString()).Selected = true;
                }
            }
            string.IsNullOrEmpty(ListingStatus);
            return base.View(assetSearchCriteriaModel);
        }

        [HttpPost]
        public ActionResult InvestmentAssetSearchCriteria(AssetSearchCriteriaModel model)
        {
            object num;
            object obj;
            model.AssetTypes = this.populateAssetTypeDDL();
            if (model.TypeOfAssetsSought == AssetType.MHP || model.TypeOfAssetsSought == AssetType.MultiFamily)
            {
                foreach (string _commercialRequirement in this._commercialRequirements)
                {
                    if (!base.ModelState.ContainsKey(_commercialRequirement))
                    {
                        continue;
                    }
                    base.ModelState[_commercialRequirement].Errors.Clear();
                }
            }
            else
            {
                foreach (string _multifamilyRequirement in this._multifamilyRequirements)
                {
                    if (!base.ModelState.ContainsKey(_multifamilyRequirement))
                    {
                        continue;
                    }
                    base.ModelState[_multifamilyRequirement].Errors.Clear();
                }
            }
            if (model.FinancialInvestmentParameters.InvestmentFundingRangeMax < model.FinancialInvestmentParameters.InvestmentFundingRangeMin)
            {
                base.ModelState.AddModelError(string.Empty, "Max Investment Funding Range should be greater than the Min Range");
            }
            if (model.TypeOfAssetsSought != AssetType.MultiFamily && model.TypeOfAssetsSought != AssetType.MHP)
            {
                if (model.OtherDemographicDetail.SquareFootageRangeMin != null)
                {
                    num = Convert.ToInt32(model.OtherDemographicDetail.SquareFootageRangeMin);
                }
                else
                {
                    num = null;
                }
                double num1 = (double)num;
                if (model.OtherDemographicDetail.SquareFootageRangeMax != null)
                {
                    obj = Convert.ToInt32(model.OtherDemographicDetail.SquareFootageRangeMax);
                }
                else
                {
                    obj = null;
                }
                if ((double)obj < num1)
                {
                    base.ModelState.AddModelError(string.Empty, "Max Square Footage Range should be greater than the Min Range");
                }
            }
            else if (model.MultiFamilyDemographicDetail.NumberOfUnitsRangeMaximum < model.MultiFamilyDemographicDetail.NumberOfUnitsRangeMinimum)
            {
                base.ModelState.AddModelError(string.Empty, "Max Units Range should be greater than the Min Range");
            }
            if (base.ModelState.IsValid)
            {
                if (model.GeographicParameters.Interests.Count<GeographicParameterInterestModel>((GeographicParameterInterestModel s) => s.StateOfInterest.Length > 0) <= 0)
                {
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "At least one geographic state is required.");
                }
                else if (!base.User.Identity.IsAuthenticated)
                {
                    if (!string.IsNullOrEmpty(model.EmailAddressOfEntity) && this._user.UserExists(model.EmailAddressOfEntity))
                    {
                        return base.RedirectToAction("Login", "Home");
                    }
                    if (string.IsNullOrEmpty(model.EmailAddressOfEntity))
                    {
                        base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "If you are not logged/registered, you must enter an email address to complete this form.");
                    }
                    else
                    {
                        string str = this._user.GeneratePassword();
                        int num2 = this._user.CreateUser(model.EmailAddressOfEntity, this._user.HashPassword(str), UserType.Investor);
                        base.AuthenticateMachine(model.EmailAddressOfEntity);
                        base.CreateCookie(false, new UserDataModel()
                        {
                            Username = model.EmailAddressOfEntity,
                            IPAddress = base.GetIPAddress(),
                            MachineName = base.DetermineCompName(),
                            SecurityCode = ""
                        });
                        if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
                        {
                            this._email.Send(new AutoRegistrationAutoWelcomeEmail()
                            {
                                Email = model.EmailAddressOfEntity,
                                Password = str,
                                To = model.NameOfPurchasingEntity
                            });
                        }
                        model.UserId = num2;
                        int num3 = this._asset.CreateAssetSearchCriteria(model);
                        ((dynamic)base.ViewBag).CurrentActiveAssetCount = this._asset.GetCurrentActiveAssetCount();
                        if (base.CanSendAutoEmail(BaseController.AutoEmailType.Asset))
                        {
                            this._email.Send(new ConfirmationSearchCriteriaSubmittedEmail()
                            {
                                Email = model.EmailAddressOfEntity,
                                To = model.NameOfPurchasingEntity
                            });
                        }
                        base.TempData["message"] = new MessageViewModel(MessageTypes.Success, string.Concat("Successfully saved. This search form can be referenced with ID#", num3, " from your My USC Page. You've also been automatically registered as an Investor for this site. You will receive an email with your username and password."));
                    }
                }
                else
                {
                    int num4 = this._asset.CreateAssetSearchCriteria(model);
                    if (base.CanSendAutoEmail(BaseController.AutoEmailType.Asset))
                    {
                        UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
                        try
                        {
                            ((dynamic)base.ViewBag).CurrentActiveAssetCount = this._asset.GetCurrentActiveAssetCount();
                            this._email.Send(new ConfirmationSearchCriteriaSubmittedEmail()
                            {
                                Email = userByUsername.Username,
                                To = string.Concat(userByUsername.FirstName, " ", userByUsername.LastName)
                            });
                        }
                        catch
                        {
                        }
                        List<UserType> userTypes = new List<UserType>()
                        {
                            UserType.CorpAdmin,
                            UserType.CorpAdmin2,
                            UserType.SiteAdmin
                        };
                        foreach (UserQuickViewModel user in this._user.GetUsers(new UserSearchModel()
                        {
                            UserTypeFilters = userTypes,
                            ShowActiveOnly = true
                        }))
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(user.Username))
                                {
                                    this._email.Send(new ConfirmationToAdminSearchCriteriaSubmitted()
                                    {
                                        Email = user.Username,
                                        SearchCriteraId = num4,
                                        SearchCriteriaDate = DateTime.Now,
                                        To = string.Concat(user.FirstName, " ", user.LastName),
                                        SearchCriteriaName = userByUsername.FullName
                                    });
                                }
                            }
                            catch (Exception exception)
                            {
                            }
                        }
                    }
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Success, string.Concat("Successfully saved. This search form can be referenced with ID#", num4, " from your My USC Page."));
                }
            }
            return base.View(model);
        }

        private string NumberToWords(int number)
        {
            if (number == 0)
            {
                return "zero";
            }
            if (number < 0)
            {
                return string.Concat("minus ", this.NumberToWords(Math.Abs(number)));
            }
            string str = "";
            if (number / 1000000 > 0)
            {
                str = string.Concat(str, this.NumberToWords(number / 1000000), " million ");
                number %= 1000000;
            }
            if (number / 1000 > 0)
            {
                str = string.Concat(str, this.NumberToWords(number / 1000), " thousand ");
                number %= 1000;
            }
            if (number / 100 > 0)
            {
                str = string.Concat(str, this.NumberToWords(number / 100), " hundred ");
                number %= 100;
            }
            if (number > 0)
            {
                if (str != "")
                {
                    str = string.Concat(str, "and ");
                }
                string[] strArrays = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                string[] strArrays1 = new string[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
                if (number >= 20)
                {
                    str = string.Concat(str, strArrays1[number / 10]);
                    if (number % 10 > 0)
                    {
                        str = string.Concat(str, "-", strArrays[number % 10]);
                    }
                }
                else
                {
                    str = string.Concat(str, strArrays[number]);
                }
            }
            return str;
        }

        public ActionResult OpenExistingCalc(Guid id)
        {
            return this.PartialView("ExistingDebtFund", this.BuildModel(id));
        }

        public ActionResult OpenProposedCalc(Guid id)
        {
            return this.PartialView("ProposedDebtFund", this.BuildModel(id));
        }

        [Authorize]
        [HttpPost]
        public ActionResult OrderDocuments(Guid AssetId, string TitleCompany)
        {
            if (string.IsNullOrEmpty(TitleCompany))
            {
                return base.RedirectToAction("MyUSCPage", "Home");
            }
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            int num = 0;
            if (!int.TryParse(TitleCompany, out num))
            {
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "The Title Company is missing its Identification Number");
                if (userByUsername.UserType != UserType.CorpAdmin2 && userByUsername.UserType != UserType.CorpAdmin && userByUsername.UserType != UserType.SiteAdmin)
                {
                    return base.RedirectToAction("ViewAsset", "DataPortal", new { id = AssetId });
                }
                return base.RedirectToAction("ViewAsset", "DataPortal", new { id = AssetId, fromManageAssets = true });
            }
            AssetDocumentOrderRequestModel assetDocumentOrderRequestModel = this._asset.RequestDocuments(AssetId, userByUsername.UserId, num, false);
            if (assetDocumentOrderRequestModel.Valid)
            {
                if (assetDocumentOrderRequestModel.IsSellerAsset)
                {
                    if (base.CanSendAutoEmail(BaseController.AutoEmailType.Title))
                    {
                        try
                        {
                            this._email.Send(new PrelimConfirmationOrderToPISeller()
                            {
                                Subject = "Your Order has been Submitted",
                                Owner = assetDocumentOrderRequestModel.SellerFullName,
                                Date = DateTime.Now,
                                TitleCompany = assetDocumentOrderRequestModel.CompanyName,
                                AssetId = assetDocumentOrderRequestModel.AssetId,
                                Address1 = assetDocumentOrderRequestModel.Address1,
                                Address2 = assetDocumentOrderRequestModel.Address2,
                                City = assetDocumentOrderRequestModel.City,
                                State = assetDocumentOrderRequestModel.State,
                                Zip = assetDocumentOrderRequestModel.Zip,
                                AssetDescription = assetDocumentOrderRequestModel.AssetDescription,
                                APN = assetDocumentOrderRequestModel.APN,
                                Ownership = assetDocumentOrderRequestModel.Ownership,
                                To = assetDocumentOrderRequestModel.SellerEmail
                            });
                        }
                        catch
                        {
                        }
                        try
                        {
                            string titleCompanyUserEmailBasedOnState = this._title.GetTitleCompanyUserEmailBasedOnState(num, assetDocumentOrderRequestModel.State);
                            this._email.Send(new PrelimConfirmationOrderToTitleCoPISeller()
                            {
                                To = assetDocumentOrderRequestModel.Manager.Email,
                                Subject = string.Concat("Requested Documents for ", assetDocumentOrderRequestModel.Address1),
                                Date = DateTime.Now,
                                SelectedTitleCompany = assetDocumentOrderRequestModel.CompanyName,
                                TitleCompany = assetDocumentOrderRequestModel.CompanyName,
                                AssetId = assetDocumentOrderRequestModel.AssetId,
                                Address1 = assetDocumentOrderRequestModel.Address1,
                                Address2 = assetDocumentOrderRequestModel.Address2,
                                State = assetDocumentOrderRequestModel.State,
                                Zip = assetDocumentOrderRequestModel.Zip,
                                City = assetDocumentOrderRequestModel.City,
                                AssetDescription = assetDocumentOrderRequestModel.AssetDescription,
                                APN = assetDocumentOrderRequestModel.APN,
                                Ownership = assetDocumentOrderRequestModel.Ownership,
                                PIBuyer = userByUsername.FullName
                            });
                            if (titleCompanyUserEmailBasedOnState != assetDocumentOrderRequestModel.Manager.Email)
                            {
                                this._email.Send(new PrelimConfirmationOrderToTitleCoPISeller()
                                {
                                    To = titleCompanyUserEmailBasedOnState,
                                    Subject = string.Concat("Requested Documents for ", assetDocumentOrderRequestModel.Address1),
                                    Date = DateTime.Now,
                                    SelectedTitleCompany = assetDocumentOrderRequestModel.CompanyName,
                                    TitleCompany = assetDocumentOrderRequestModel.CompanyName,
                                    AssetId = assetDocumentOrderRequestModel.AssetId,
                                    Address1 = assetDocumentOrderRequestModel.Address1,
                                    Address2 = assetDocumentOrderRequestModel.Address2,
                                    State = assetDocumentOrderRequestModel.State,
                                    Zip = assetDocumentOrderRequestModel.Zip,
                                    City = assetDocumentOrderRequestModel.City,
                                    AssetDescription = assetDocumentOrderRequestModel.AssetDescription,
                                    APN = assetDocumentOrderRequestModel.APN,
                                    Ownership = assetDocumentOrderRequestModel.Ownership,
                                    PIBuyer = userByUsername.FullName
                                });
                            }
                        }
                        catch
                        {
                        }
                        List<UserQuickViewModel> users = this._user.GetUsers(new UserSearchModel()
                        {
                            UserTypeFilter = new UserType?(UserType.CorpAdmin),
                            ShowActiveOnly = true
                        });
                        users.AddRange(this._user.GetUsers(new UserSearchModel()
                        {
                            UserTypeFilter = new UserType?(UserType.SiteAdmin),
                            ShowActiveOnly = true
                        }));
                        foreach (UserQuickViewModel user in users)
                        {
                            try
                            {
                                this._email.Send(new PrelimConfirmationOrderToUSCPISeller()
                                {
                                    Subject = string.Concat("Documents for Asset ", assetDocumentOrderRequestModel.AssetId, " have been ordered"),
                                    Date = DateTime.Now,
                                    TitleCompany = assetDocumentOrderRequestModel.CompanyName,
                                    AssetId = assetDocumentOrderRequestModel.AssetId,
                                    Address1 = assetDocumentOrderRequestModel.Address1,
                                    Address2 = assetDocumentOrderRequestModel.Address2,
                                    State = assetDocumentOrderRequestModel.State,
                                    Zip = assetDocumentOrderRequestModel.Zip,
                                    City = assetDocumentOrderRequestModel.City,
                                    AssetDescription = assetDocumentOrderRequestModel.AssetDescription,
                                    APN = assetDocumentOrderRequestModel.APN,
                                    Ownership = assetDocumentOrderRequestModel.Ownership,
                                    To = user.Username
                                });
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                else if (base.CanSendAutoEmail(BaseController.AutoEmailType.Title))
                {
                    try
                    {
                        this._email.Send(new PrelimConfirmationOrderToBuyer()
                        {
                            Subject = "Your Order has been Submitted",
                            Registrant = userByUsername.FullName,
                            Date = DateTime.Now,
                            TitleCompany = assetDocumentOrderRequestModel.CompanyName,
                            TitleCompanyAddress = assetDocumentOrderRequestModel.CompanyAddress,
                            TitleCompanyPhone = assetDocumentOrderRequestModel.CompanyPhone,
                            AssetId = assetDocumentOrderRequestModel.AssetId,
                            Address1 = assetDocumentOrderRequestModel.Address1,
                            Address2 = assetDocumentOrderRequestModel.Address2,
                            City = assetDocumentOrderRequestModel.City,
                            State = assetDocumentOrderRequestModel.State,
                            Zip = assetDocumentOrderRequestModel.Zip,
                            AssetDescription = assetDocumentOrderRequestModel.AssetDescription,
                            APN = assetDocumentOrderRequestModel.APN,
                            Ownership = assetDocumentOrderRequestModel.Ownership,
                            To = userByUsername.Username
                        });
                    }
                    catch
                    {
                    }
                    try
                    {
                        string str = this._title.GetTitleCompanyUserEmailBasedOnState(num, assetDocumentOrderRequestModel.State);
                        this._email.Send(new PrelimConfirmationOrderToTitleCoBuyer()
                        {
                            To = assetDocumentOrderRequestModel.Manager.Email,
                            Subject = string.Concat("Requested Documents for ", assetDocumentOrderRequestModel.Address1),
                            Date = DateTime.Now,
                            SelectedTitleCompany = assetDocumentOrderRequestModel.CompanyName,
                            TitleCompany = assetDocumentOrderRequestModel.CompanyName,
                            AssetId = assetDocumentOrderRequestModel.AssetId,
                            Address1 = assetDocumentOrderRequestModel.Address1,
                            Address2 = assetDocumentOrderRequestModel.Address2,
                            State = assetDocumentOrderRequestModel.State,
                            Zip = assetDocumentOrderRequestModel.Zip,
                            City = assetDocumentOrderRequestModel.City,
                            AssetDescription = assetDocumentOrderRequestModel.AssetDescription,
                            APN = assetDocumentOrderRequestModel.APN,
                            Ownership = assetDocumentOrderRequestModel.Ownership,
                            PIBuyer = userByUsername.FullName
                        });
                        if (str != assetDocumentOrderRequestModel.Manager.Email)
                        {
                            this._email.Send(new PrelimConfirmationOrderToTitleCoBuyer()
                            {
                                To = str,
                                Subject = string.Concat("Requested Documents for ", assetDocumentOrderRequestModel.Address1),
                                Date = DateTime.Now,
                                SelectedTitleCompany = assetDocumentOrderRequestModel.CompanyName,
                                TitleCompany = assetDocumentOrderRequestModel.CompanyName,
                                AssetId = assetDocumentOrderRequestModel.AssetId,
                                Address1 = assetDocumentOrderRequestModel.Address1,
                                Address2 = assetDocumentOrderRequestModel.Address2,
                                State = assetDocumentOrderRequestModel.State,
                                Zip = assetDocumentOrderRequestModel.Zip,
                                City = assetDocumentOrderRequestModel.City,
                                AssetDescription = assetDocumentOrderRequestModel.AssetDescription,
                                APN = assetDocumentOrderRequestModel.APN,
                                Ownership = assetDocumentOrderRequestModel.Ownership,
                                PIBuyer = userByUsername.FullName
                            });
                        }
                    }
                    catch
                    {
                    }
                    List<UserQuickViewModel> userQuickViewModels = this._user.GetUsers(new UserSearchModel()
                    {
                        UserTypeFilter = new UserType?(UserType.CorpAdmin),
                        ShowActiveOnly = true
                    });
                    userQuickViewModels.AddRange(this._user.GetUsers(new UserSearchModel()
                    {
                        UserTypeFilter = new UserType?(UserType.SiteAdmin),
                        ShowActiveOnly = true
                    }));
                    foreach (UserQuickViewModel userQuickViewModel in userQuickViewModels)
                    {
                        try
                        {
                            this._email.Send(new PrelimConfirmationOrderToUSCBuyer()
                            {
                                Subject = string.Concat("Documents for Asset ", assetDocumentOrderRequestModel.AssetId, " have been ordered"),
                                Date = DateTime.Now,
                                TitleCompany = assetDocumentOrderRequestModel.CompanyName,
                                AssetId = assetDocumentOrderRequestModel.AssetId,
                                Address1 = assetDocumentOrderRequestModel.Address1,
                                Address2 = assetDocumentOrderRequestModel.Address2,
                                State = assetDocumentOrderRequestModel.State,
                                Zip = assetDocumentOrderRequestModel.Zip,
                                City = assetDocumentOrderRequestModel.City,
                                AssetDescription = assetDocumentOrderRequestModel.AssetDescription,
                                APN = assetDocumentOrderRequestModel.APN,
                                Ownership = assetDocumentOrderRequestModel.Ownership,
                                To = userQuickViewModel.Username,
                                Registrant = userByUsername.FullName
                            });
                        }
                        catch
                        {
                        }
                    }
                }
                base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Recorded documents have successfully been requested. You will be notified via email when the documents are uploaded.");
            }
            else if (!assetDocumentOrderRequestModel.Valid && !string.IsNullOrEmpty(assetDocumentOrderRequestModel.Message))
            {
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, assetDocumentOrderRequestModel.Message);
            }
            if (userByUsername.UserType != UserType.CorpAdmin2 && userByUsername.UserType != UserType.CorpAdmin && userByUsername.UserType != UserType.SiteAdmin)
            {
                return base.RedirectToAction("ViewAsset", "DataPortal", new { id = AssetId });
            }
            return base.RedirectToAction("ViewAsset", "DataPortal", new { id = AssetId, fromManageAssets = true });
        }

        [HttpPost]
        public FileUploadJsonResult PFSUserFileDelete(string id)
        {
            FileUploadJsonResult fileUploadJsonResult;
            try
            {
                int num = 0;
                if (!int.TryParse(id, out num))
                {
                    fileUploadJsonResult = new FileUploadJsonResult()
                    {
                        Data = new { message = "false" }
                    };
                }
                else
                {
                    this._user.DeleteUserFile(num);
                    this._fileManager.DeleteUserFile(num);
                    fileUploadJsonResult = new FileUploadJsonResult()
                    {
                        Data = new { message = "true" }
                    };
                }
            }
            catch
            {
                fileUploadJsonResult = new FileUploadJsonResult()
                {
                    Data = new { message = "false" }
                };
            }
            return fileUploadJsonResult;
        }

        [HttpPost]
        public FileUploadJsonResult PFSUserFileUpload(HttpPostedFileBase file, UploadUserFileType type)
        {
            FileUploadJsonResult fileUploadJsonResult;
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            if (file == null || string.IsNullOrEmpty(file.FileName))
            {
                return new FileUploadJsonResult()
                {
                    Data = new { message = "FileIsNull" }
                };
            }
            try
            {
                int num = this._fileManager.CreateUserFile(file, userByUsername.UserId, new UploadUserFileType?(type));
                fileUploadJsonResult = new FileUploadJsonResult()
                {
                    Data = new { message = "true", filename = file.FileName, contentType = file.ContentType, size = this._fileManager.GetFileSize(file.ContentLength), id = num }
                };
            }
            catch
            {
                fileUploadJsonResult = new FileUploadJsonResult()
                {
                    Data = new { message = "false" }
                };
            }
            return fileUploadJsonResult;
        }

        private new IEnumerable<SelectListItem> populateAssetTypeDDL()
        {
            return
                from AssetType type in Enum.GetValues(typeof(AssetType))
                select new SelectListItem()
                {
                    Text = EnumHelper.GetEnumDescription(type),
                    Value = type.ToString()
                };
        }

        private IEnumerable<SelectListItem> populatePortfolioListDDL()
        {
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            List<PortfolioQuickListViewModel> portfolioQuickListViewModels = new List<PortfolioQuickListViewModel>();
            if (userByUsername != null)
            {
                portfolioQuickListViewModels = this._portfolio.GetUserPortfolios(userByUsername.UserId);
            }
            return
                from userPf in portfolioQuickListViewModels
                select new SelectListItem()
                {
                    Text = userPf.PortfolioName,
                    Value = userPf.PortfolioId.ToString()
                };
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "PreviewICAgreementPdf")]
        public FileResult PreviewICAgreementPdf(ICAgreementTemplateModel model)
        {
            return this.File(this._pdf.GetICPdf(model, Properties.Resources.ICAgreementTemplate), "application/octet-stream", "download.pdf");
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "PreviewJVAgreementPdf")]
        public FileResult PreviewJVAgreementPdf(JVAgreementTemplateModel model)
        {
            DateTime now = DateTime.Now;
            model.Year = now.Year;
            model.Day = now.Day;
            model.Month = now.ToString("MMMM");
            return this.File(this._pdf.GetJVAPdf(model, Properties.Resources.JVTemplate), "application/octet-stream", "download.pdf");
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "PreviewNCNDAgreementPdf")]
        public FileResult PreviewJVAgreementPdf(NCNDTemplateModel model)
        {
            return this.File(this._pdf.GetNCNDPdf(model, Properties.Resources.NCNDTemplate), "application/octet-stream", "download.pdf");
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "PreviewMDAPdf")]
        public FileResult PreviewMDAPdf(IPABuyerTemplateModel model)
        {
            return this.File(this._pdf.GetIPABuyerPdf(model, Properties.Resources.IPABuyerWSig), "application/octet-stream", "download.pdf");
        }

        [Authorize]
        [HttpPost]
        public ActionResult RequestInsuranceDocuments(Guid AssetId, string InsuranceCompany)
        {
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            if (string.IsNullOrEmpty(InsuranceCompany))
            {
                return base.RedirectToAction("MyUSCPage", "Home");
            }
            int num = 0;
            if (!int.TryParse(InsuranceCompany, out num))
            {
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "The Insurance Company is missing its Identification Number");
            }
            else
            {
                AssetDocumentOrderRequestModel assetDocumentOrderRequestModel = this._insurance.RequestDocuments(AssetId, userByUsername.UserId, num, false);
                if (assetDocumentOrderRequestModel.Valid)
                {
                    if (base.CanSendAutoEmail(BaseController.AutoEmailType.Insurance))
                    {
                        try
                        {
                            this._email.Send(new ConfirmationToBuyerInsQuoteOrderedEmail()
                            {
                                To = userByUsername.Username,
                                Date = DateTime.Now,
                                CompanyName = assetDocumentOrderRequestModel.CompanyName,
                                AssetId = assetDocumentOrderRequestModel.AssetId,
                                AssetAddress1 = assetDocumentOrderRequestModel.Address1,
                                AssetAddress2 = assetDocumentOrderRequestModel.Address2,
                                AssetCity = assetDocumentOrderRequestModel.City,
                                AssetState = assetDocumentOrderRequestModel.State,
                                AssetZip = assetDocumentOrderRequestModel.Zip,
                                AssetDescription = assetDocumentOrderRequestModel.AssetDescription,
                                VestingEntity = assetDocumentOrderRequestModel.VestingEntity,
                                APN = assetDocumentOrderRequestModel.APN,
                                Subject = "Insurance Documents Requested",
                                BuyerName = userByUsername.FullName
                            });
                        }
                        catch
                        {
                        }
                        try
                        {
                            this._email.Send(new NotificationToInsCoDocsOrderedEmail()
                            {
                                To = assetDocumentOrderRequestModel.InsuranceManagerEmail,
                                Date = DateTime.Now,
                                CompanyName = assetDocumentOrderRequestModel.CompanyName,
                                AssetId = assetDocumentOrderRequestModel.AssetId,
                                AssetAddress1 = assetDocumentOrderRequestModel.Address1,
                                AssetAddress2 = assetDocumentOrderRequestModel.Address2,
                                AssetCity = assetDocumentOrderRequestModel.City,
                                AssetState = assetDocumentOrderRequestModel.State,
                                AssetZip = assetDocumentOrderRequestModel.Zip,
                                AssetSpecs = assetDocumentOrderRequestModel.AssetSpecs,
                                APN = assetDocumentOrderRequestModel.APN,
                                BuyerName = assetDocumentOrderRequestModel.BuyerName,
                                VestingEntity = assetDocumentOrderRequestModel.VestingEntity,
                                BuyerEIN = assetDocumentOrderRequestModel.BuyerEIN,
                                BuyerContactInfo = assetDocumentOrderRequestModel.BuyerContactInfo,
                                BuyerAddress1 = assetDocumentOrderRequestModel.BuyerAddress1,
                                BuyerAddress2 = assetDocumentOrderRequestModel.BuyerAddress2,
                                BuyerCity = assetDocumentOrderRequestModel.BuyerCity,
                                BuyerState = assetDocumentOrderRequestModel.BuyerState,
                                BuyerZip = assetDocumentOrderRequestModel.BuyerZip,
                                Subject = "Insurance Documents Requested",
                                AssetDescription = assetDocumentOrderRequestModel.AssetDescription
                            });
                        }
                        catch
                        {
                        }
                        foreach (UserModel admin in this._user.GetAdmins())
                        {
                            if (!base.isEmail(admin.Username))
                            {
                                continue;
                            }
                            try
                            {
                                this._email.Send(new ConfirmationToUSCInsQuoteOrderedEmail()
                                {
                                    To = admin.Username,
                                    Date = DateTime.Now,
                                    CompanyName = assetDocumentOrderRequestModel.CompanyName,
                                    AssetId = assetDocumentOrderRequestModel.AssetId,
                                    AssetAddress1 = assetDocumentOrderRequestModel.Address1,
                                    AssetAddress2 = assetDocumentOrderRequestModel.Address2,
                                    AssetCity = assetDocumentOrderRequestModel.City,
                                    AssetState = assetDocumentOrderRequestModel.State,
                                    AssetZip = assetDocumentOrderRequestModel.Zip,
                                    APN = assetDocumentOrderRequestModel.APN,
                                    BuyerName = assetDocumentOrderRequestModel.BuyerName,
                                    VestingEntity = assetDocumentOrderRequestModel.VestingEntity,
                                    Subject = "Insurance Documents Requested",
                                    AssetDescription = assetDocumentOrderRequestModel.AssetDescription
                                });
                            }
                            catch
                            {
                            }
                        }
                    }
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Insurance documents have successfully been requested. You will be notified via email when the documents are uploaded.");
                }
                else if (!assetDocumentOrderRequestModel.Valid && !string.IsNullOrEmpty(assetDocumentOrderRequestModel.Message))
                {
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Error, assetDocumentOrderRequestModel.Message);
                }
            }
            if (userByUsername.UserType != UserType.CorpAdmin2 && userByUsername.UserType != UserType.CorpAdmin && userByUsername.UserType != UserType.SiteAdmin)
            {
                return base.RedirectToAction("ViewAsset", "DataPortal", new { id = AssetId });
            }
            return base.RedirectToAction("ViewAsset", "DataPortal", new { id = AssetId, fromManageAssets = true });
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Save")]
        [ValidateInput(false)]
        public ActionResult SavePersonalFinancialStatement(PersonalFinancialStatementTemplateModel model, string SubmitPFS)
        {
            DateTime now = DateTime.Now;
            model.Year = now.Year;
            model.Day = now.Day;
            model.Month = now.ToString("MMMM");
            if (!base.ModelState.IsValid)
            {
                return base.View("ExecutePersonalFinancialStatement", model);
            }
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            model.UserId = userByUsername.UserId;
            this._user.CreatePersonalFinancialStatement(model);
            userByUsername.CellNumber = model.ResidentialPhone;
            userByUsername.WorkNumber = model.BusinessPhone;
            this._user.UpdateUser(userByUsername);
            base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully saved.");
            return base.RedirectToAction("MyUSCPage", "Home");
        }

        [HttpGet]
        public ViewResult SendPFSToMortgageLenderBroker()
        {
            EmailPersonalFinancialStatementModel emailPersonalFinancialStatementModel = new EmailPersonalFinancialStatementModel();
            UserModel referredByEmail = this._user.GetReferredByEmail(base.User.Identity.Name);
            if (referredByEmail != null)
            {
                emailPersonalFinancialStatementModel.CompanyName = referredByEmail.CompanyName;
                emailPersonalFinancialStatementModel.OfficerName = referredByEmail.ManagingOfficerName;
                emailPersonalFinancialStatementModel.ForwardEmailAddress = referredByEmail.Username;
            }
            return base.View(emailPersonalFinancialStatementModel);
        }

        [HttpPost]
        public ActionResult SendPFSToMortgageLenderBroker(EmailPersonalFinancialStatementModel model)
        {
            try
            {
                if (base.ModelState.IsValid)
                {
                    UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
                    ForwardPFSEmail forwardPFSEmail = new ForwardPFSEmail()
                    {
                        CompanyName = model.CompanyName,
                        ForwardingEmailAddress = model.ForwardEmailAddress,
                        OfficerName = model.OfficerName,
                        OwningUserName = userByUsername.FullName
                    };
                    foreach (HttpPostedFileBase document in model.Documents)
                    {
                        UploadUserFileType? nullable = null;
                        int num = this._fileManager.CreateUserFile(document, userByUsername.UserId, nullable);
                        UserFileModel userFileById = this._user.GetUserFileById(num);
                        Attachment attachment = new Attachment(userFileById.Location)
                        {
                            Name = userFileById.Description
                        };
                        forwardPFSEmail.Attach(attachment);
                    }
                    if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
                    {
                        this._email.Send(forwardPFSEmail);
                    }
                    this._user.LogUserRecord(UserRecordType.DFSubmittal, userByUsername.UserId);
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Email successfully sent.");
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, exception.Message);
            }
            return base.View(model);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "UpdateSubmit")]
        [ValidateInput(false)]
        public ActionResult SubmitPersonalFinancialStatement(PersonalFinancialStatementTemplateModel model, string EditPFS)
        {
            if (!base.ModelState.IsValid)
            {
                return base.View("EditPersonalFinancialStatement", model);
            }
            this._user.UpdatePersonalFinancialStatement(model);
            this._pdf.SubmitPFS(model, Properties.Resources.PersonalFinancialStatementTemplate, model.UserId);
            base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully executed. Please wait for an email from DocuSign to sign document.");
            return base.View("DocusignProcess");
        }

        [HttpGet]
        public ActionResult ViewAsset(Guid id, bool fromManageAssets = false)
        {
            List<AssetImage> images;
            IOrderedEnumerable<AssetImage> isMainImage;
            AssetViewModel list;
            ((dynamic)base.ViewBag).IsSampleAsset = false;
            List<VideoOptions> videoOptions = new List<VideoOptions>();
            if (!base.User.Identity.IsAuthenticated)
            {
                return base.RedirectToAction("Login", "Home", new { rurl = string.Concat("/DataPortal/ExecuteMDA/", id) });
            }
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            AssetViewModel asset = this._asset.GetAsset(id, false);
            asset = _asset.PopulateDocumentOrder(asset);

            ((dynamic)base.ViewBag).Layout = "~/Views/Shared/_Layout.cshtml";
            if (userByUsername.UserType == UserType.CorpAdmin && userByUsername.UserType == UserType.CorpAdmin2)
            {
                fromManageAssets = true;
            }
            if (fromManageAssets)
            {
                ((dynamic)base.ViewBag).Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
            }
            else if (userByUsername.UserType != UserType.CorpAdmin && userByUsername.UserType != UserType.CorpAdmin2 && asset.ListedByUserId != userByUsername.UserId)
            {
                if ((asset.ListedByUserId == userByUsername.UserId ? false : !this._asset.UserConfirmedAssetDisclosure(id, userByUsername.UserId)))
                {
                    return base.RedirectToAction("ViewAssetDisclosure", "DataPortal", new { id = id, userId = userByUsername.UserId });
                }
            }
            bool flag = (asset.ListedByUserId == userByUsername.UserId || this._asset.SignedMDAWithAssetId(userByUsername.UserId, id) || userByUsername.UserType == UserType.CorpAdmin || userByUsername.UserType == UserType.SiteAdmin ? true : userByUsername.UserType == UserType.Investor);
            if (!flag && this._asset.HasSignedMDA(userByUsername.UserId))
            {
                return base.RedirectToAction("AddAssetToMda", "DataPortal", new { id = id });
            }
            if (!flag)
            {
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You must sign an IPA to view this asset.");
                return base.RedirectToAction("ExecuteMDA", "DataPortal", new { id = id });
            }
            foreach (TitleQuickViewModel titleQuickViewModel in
                from x in this._user.GetTitleCompanies(new CompanySearchModel()
                {
                    CompanyName = string.Empty,
                    CompanyURL = string.Empty,
                    State = asset.State,
                    NeedsManager = true
                })
                orderby x.TitleCompName
                select x)
            {
                asset.TitleCompanies.Add(new SelectListItem()
                {
                    Text = titleQuickViewModel.TitleCompName,
                    Value = titleQuickViewModel.TitleCompanyId.ToString(),
                    Selected = false
                });
            }
            foreach (InsuranceCompanyViewModel insuranceCompany in this._insurance.GetInsuranceCompanies(new CompanySearchModel()
            {
                NeedsManager = true
            }))
            {
                asset.InsuranceCompanies.Add(new SelectListItem()
                {
                    Text = insuranceCompany.CompanyName,
                    Value = insuranceCompany.InsuranceCompanyId.ToString()
                });
            }

            ((dynamic)base.ViewBag).Videos = videoOptions;
            if (!userByUsername.MBAUserId.HasValue)
            {
                asset.MBAAgentName = string.Empty;
            }
            else
            {
                IUserManager userManager = this._user;
                int? mBAUserId = userByUsername.MBAUserId;
                asset.MBAAgentName = userManager.GetMbaById(mBAUserId.Value).FullName;
            }
            bool? isMortgageAnARM = asset.IsMortgageAnARM;
            if (isMortgageAnARM.HasValue)
            {
                isMortgageAnARM = asset.IsMortgageAnARM;
                if ((isMortgageAnARM.GetValueOrDefault() ? !isMortgageAnARM.HasValue : true))
                {
                    goto Label1;
                }
                asset.isARM = new int?(1);
                if (asset.Images == null)
                {
                    asset.Images = new List<AssetImage>();
                }
                else
                {
                    list = asset;
                    images = asset.Images;
                    isMainImage =
                        from w in images
                        orderby w.IsMainImage descending
                        select w;
                    list.Images = isMainImage.ThenBy<AssetImage, int>((AssetImage w) => w.Order).ToList<AssetImage>();
                }
                if (!asset.IsPaper)
                {
                    return base.View(asset);
                }
                return base.View("ViewPaperAsset", asset);
            }
            Label1:
            isMortgageAnARM = asset.IsMortgageAnARM;
            if (isMortgageAnARM.HasValue)
            {
                isMortgageAnARM = asset.IsMortgageAnARM;
                if ((!isMortgageAnARM.GetValueOrDefault() ? isMortgageAnARM.HasValue : false))
                {
                    asset.isARM = new int?(0);
                }
            }
            if (asset.Images == null)
            {
                asset.Images = new List<AssetImage>();
            }
            else
            {
                list = asset;
                images = asset.Images;
                isMainImage =
                    from w in images
                    orderby w.IsMainImage descending
                    select w;
                list.Images = isMainImage.ThenBy<AssetImage, int>((AssetImage w) => w.Order).ToList<AssetImage>();
            }
            if (!asset.IsPaper)
            {
                return base.View(asset);
            }
            return base.View("ViewPaperAsset", asset);
        }

        [HttpGet]
        public ViewResult ViewAssetDisclosure(Guid id, int userId)
        {
            return base.View(new ViewAssetDisclosureModel()
            {
                AssetId = id,
                UserId = userId
            });
        }

        [HttpPost]
        public ActionResult ViewAssetDisclosure(ViewAssetDisclosureModel model)
        {
            this._asset.ConfirmAssetUserDisclosure(model.AssetId, model.UserId);
            return base.RedirectToAction("ViewAsset", "DataPortal", new { id = model.AssetId });
        }

        public ViewResult ViewInvestmentAssetSearchCriteria(int id)
        {
            return base.View(this._asset.GetAssetSearchCriteriaById(id));
        }

        [Authorize]
        public ActionResult ViewNetworkUploadInventory(JVMANetUpSearchResultsModel model)
        {
            int value;
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            model.FullName = userByUsername.FullName;
            JVMANetUpSearchModel jVMANetUpSearchModel = new JVMANetUpSearchModel()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                City = model.City,
                State = model.State,
                DateOfUploadStart = model.DateOfUploadStart,
                DateOfUploadEnd = model.DateOfUploadEnd,
                DateRefRegisteredStart = model.DateRefRegisteredStart,
                DateRefRegisteredEnd = model.DateRefRegisteredEnd,
                UserType = model.UserType,
                AssetType = model.SelectedAssetType,
                DateOfDFSubmittalStart = model.DateOfDFSubmittalStart,
                DateOfDFSubmittalEnd = model.DateOfDFSubmittalEnd,
                DateOfLOISubmittalStart = model.DateOfLOISubmittalStart,
                DateOfLOISubmittalEnd = model.DateOfLOISubmittalEnd
            };
            List<JVMANetUpViewModel> jVMAUserNetworkUploads = this._user.GetJVMAUserNetworkUploads(jVMANetUpSearchModel, userByUsername.UserId);
            base.ViewBag.FirstName = (model.SortOrder == "firstName" ? "firstName_desc" : "firstName");
            base.ViewBag.LastName = (model.SortOrder == "lastName" ? "lastName_desc" : "lastName");
            base.ViewBag.City = (model.SortOrder == "city" ? "city_desc" : "city");
            base.ViewBag.State = (model.SortOrder == "state" ? "state_desc" : "state");
            base.ViewBag.DateOfUpload = (model.SortOrder == "dateOfUpload" ? "dateOfUpload_desc" : "dateOfUpload");
            base.ViewBag.DateRefRegistered = (model.SortOrder == "dateRefRegistered" ? "dateRefRegistered_desc" : "dateRefRegistered");
            base.ViewBag.UserType = (model.SortOrder == "userType" ? "userType_desc" : "userType");
            base.ViewBag.MDAs = (model.SortOrder == "mda" ? "mda_desc" : "mda");
            base.ViewBag.AssetType = (model.SortOrder == "assetTypee" ? "assetType_desc" : "assetType");
            base.ViewBag.DateOfDFSubmittal = (model.SortOrder == "dateOfDFSubmittal" ? "dateOfDFSubmittal_desc" : "dateOfDFSubmittal");
            base.ViewBag.DateOfLOISubmittal = (model.SortOrder == "dateOfLOISubmittal" ? "dateOfLOISubmittal_desc" : "dateOfLOISubmittal");
            string sortOrder = model.SortOrder;
            switch (sortOrder)
            {
                case "firstName_desc":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.FirstName descending
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "firstName":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.FirstName
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "lastName_desc":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.LastName descending
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "lastName":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.LastName
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "city_desc":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.City descending
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "city":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.City
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "state_desc":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.State descending
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "state":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.State
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "dateOfUpload_desc":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.DateOfUpload descending
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "dateOfUpload":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.DateOfUpload
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "dateRefRegistered_desc":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.DateRefRegistered descending
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "dateRefRegistered":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.DateRefRegistered
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "userType_desc":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.UserTypeString descending
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "userType":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.UserTypeString
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "mda_desc":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.MDAs descending
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "mda":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.MDAs
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "assetType_desc":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.AssetTypes descending
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "assetType":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.AssetTypes
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "dateOfDFSubmittal_desc":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.DateOfDFSubmittal descending
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "dateOfDFSubmittal":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.DateOfDFSubmittal
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                case "dateOfLOISubmittal_desc":
                    {
                        jVMAUserNetworkUploads = (
                            from x in jVMAUserNetworkUploads
                            orderby x.DateOfLOISubmittal descending
                            select x).ToList<JVMANetUpViewModel>();
                        break;
                    }
                default:
                    {
                        jVMAUserNetworkUploads = (sortOrder == "dateOfLOISubmittal" ? (
                            from x in jVMAUserNetworkUploads
                            orderby x.DateOfLOISubmittal
                            select x).ToList<JVMANetUpViewModel>() : (
                            from x in jVMAUserNetworkUploads
                            orderby x.FirstName
                            select x).ToList<JVMANetUpViewModel>());
                        break;
                    }
            }
            int num = 0;
            TempDataDictionary tempData = base.TempData;
            int? rowCount = model.RowCount;
            if (rowCount.HasValue)
            {
                rowCount = model.RowCount;
                value = rowCount.Value;
            }
            else
            {
                value = 50;
            }
            num = value;
            tempData["RowCount"] = value;
            base.TempData.Keep("RowCount");
            rowCount = model.Page;
            model.Uploads = jVMAUserNetworkUploads.ToPagedList<JVMANetUpViewModel>((rowCount.HasValue ? rowCount.GetValueOrDefault() : 1), num);
            return base.View(model);
        }

        [Authorize]
        public ActionResult ViewOrderHistory(OrderHistorySearchResultsModel model, string sortOrder, int? page, string filter = null)
        {
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            if (!this._user.HasOrderHistory(userByUsername.UserId))
            {
                return base.RedirectToAction("MyUSCPage", "Home");
            }
            List<OrderHistoryQuickListViewModel> ordersForUser = this._user.GetOrdersForUser(userByUsername.UserId);
            int num = (base.TempData["RowCount"] != null ? Convert.ToInt32(base.TempData["RowCount"]) : 50);
            int? nullable = page;
            model.History = ordersForUser.ToPagedList<OrderHistoryQuickListViewModel>((nullable.HasValue ? nullable.GetValueOrDefault() : 1), num);
            return base.View(model);
        }

        [HttpGet]
        public ActionResult ViewSampleAsset()
        {
            ((dynamic)base.ViewBag).IsSampleAsset = true;
            ((dynamic)base.ViewBag).Layout = "~/Views/Shared/_Layout.cshtml";
            return base.View("ViewAsset", this._asset.GetSampleAsset(false));
        }

        [Authorize]
        [HttpGet]
        public ActionResult ViewUserMDAHistory(int? id)
        {
            if (id.GetValueOrDefault(0) <= 0)
            {
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid URL.");
                return base.RedirectToAction("MyUSCPage", "Home");
            }
            JVMAUserMDASearchResultsModel jVMAUserMDASearchResultsModel = new JVMAUserMDASearchResultsModel();
            JVMAUserMDAViewModels jVMAUserMDAHistory = this._user.GetJVMAUserMDAHistory(new JVMAUserMDASearchModel()
            {
                Username = base.User.Identity.Name,
                ReferralUserId = id.Value
            });
            jVMAUserMDASearchResultsModel.Assets = jVMAUserMDAHistory.Assets.ToPagedList<JVMAUserMDAViewModel>(1, 50);
            jVMAUserMDASearchResultsModel.ParticipantFullName = jVMAUserMDAHistory.ParticipantFullName;
            jVMAUserMDASearchResultsModel.ReferredUserFullName = jVMAUserMDAHistory.ReferredUserFullName;
            jVMAUserMDASearchResultsModel.UserId = jVMAUserMDAHistory.UserId;
            return base.View(jVMAUserMDASearchResultsModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ViewUserMDAHistory(JVMAUserMDASearchResultsModel model)
        {
            int value;
            this._user.GetUserByUsername(base.User.Identity.Name);
            JVMAUserMDASearchModel jVMAUserMDASearchModel = new JVMAUserMDASearchModel()
            {
                UserId = model.UserId,
                AssetNumber = model.AssetNumber,
                AssetType = model.SelectedAssetType,
                State = model.State,
                DateOfMDAStart = model.DateOfMDAStart,
                DateOfMDAEnd = model.DateOfMDAEnd,
                DateOfDFSubmittalStart = model.DateOfDFSubmittalStart,
                DateOfDFSubmittalEnd = model.DateOfDFSubmittalEnd,
                DateOfLOISubmittalStart = model.DateOfLOISubmittalStart,
                DateOfLOISubmittalEnd = model.DateOfLOISubmittalEnd,
                ProposedCOEStart = model.ProposedCOEStart,
                ProposedCOEEnd = model.ProposedCOEEnd,
                ActualCOEStart = model.ActualCOEStart,
                ActualCOEEnd = model.ActualCOEEnd,
                DateRefFeePaidStart = model.DateRefFeePaidStart,
                DateRefFeePaidEnd = model.DateRefFeePaidEnd
            };
            JVMAUserMDAViewModels jVMAUserMDAHistory = this._user.GetJVMAUserMDAHistory(jVMAUserMDASearchModel);
            base.ViewBag.AssetNumber = (model.SortOrder == "AssetNumber" ? "AssetNumber_desc" : "AssetNumber");
            base.ViewBag.AssetType = (model.SortOrder == "AssetType" ? "AssetType_desc" : "AssetType");
            base.ViewBag.State = (model.SortOrder == "State" ? "State_desc" : "State");
            base.ViewBag.DateOfMDA = (model.SortOrder == "DateOfMDA" ? "DateOfMDA_desc" : "DateOfMDA");
            base.ViewBag.DateOfDFSubmittal = (model.SortOrder == "DateOfDFSubmittal" ? "DateOfDFSubmittal_desc" : "DateOfDFSubmittal");
            base.ViewBag.DateOfLOISubmittal = (model.SortOrder == "DateOfLOISubmittal" ? "DateOfLOISubmittal_desc" : "DateOfLOISubmittal");
            base.ViewBag.ProposedCOE = (model.SortOrder == "ProposedCOE" ? "ProposedCOE_desc" : "ProposedCOE");
            base.ViewBag.ActualCOE = (model.SortOrder == "ActualCOE" ? "ActualCOE_desc" : "ActualCOE");
            base.ViewBag.DateRefFeePaid = (model.SortOrder == "DateRefFeePaid" ? "DateRefFeePaid_desc" : "DateRefFeePaid");
            string sortOrder = model.SortOrder;
            switch (sortOrder)
            {
                case "AssetNumber_desc":
                    {
                        jVMAUserMDAHistory.Assets = (
                            from x in jVMAUserMDAHistory.Assets
                            orderby x.AssetNumber descending
                            select x).ToList<JVMAUserMDAViewModel>();
                        break;
                    }
                case "AssetNumber":
                    {
                        jVMAUserMDAHistory.Assets = (
                            from x in jVMAUserMDAHistory.Assets
                            orderby x.AssetNumber
                            select x).ToList<JVMAUserMDAViewModel>();
                        break;
                    }
                case "AssetType_desc":
                    {
                        jVMAUserMDAHistory.Assets = (
                            from x in jVMAUserMDAHistory.Assets
                            orderby x.AssetTypeString descending
                            select x).ToList<JVMAUserMDAViewModel>();
                        break;
                    }
                case "AssetType":
                    {
                        jVMAUserMDAHistory.Assets = (
                            from x in jVMAUserMDAHistory.Assets
                            orderby x.AssetTypeString
                            select x).ToList<JVMAUserMDAViewModel>();
                        break;
                    }
                case "State_desc":
                    {
                        jVMAUserMDAHistory.Assets = (
                            from x in jVMAUserMDAHistory.Assets
                            orderby x.State descending
                            select x).ToList<JVMAUserMDAViewModel>();
                        break;
                    }
                case "State":
                    {
                        jVMAUserMDAHistory.Assets = (
                            from x in jVMAUserMDAHistory.Assets
                            orderby x.State
                            select x).ToList<JVMAUserMDAViewModel>();
                        break;
                    }
                case "DateOfMDA_desc":
                    {
                        jVMAUserMDAHistory.Assets = (
                            from x in jVMAUserMDAHistory.Assets
                            orderby x.DateOfMDA descending
                            select x).ToList<JVMAUserMDAViewModel>();
                        break;
                    }
                case "DateOfMDA":
                    {
                        jVMAUserMDAHistory.Assets = (
                            from x in jVMAUserMDAHistory.Assets
                            orderby x.DateOfMDA
                            select x).ToList<JVMAUserMDAViewModel>();
                        break;
                    }
                case "DateOfDFSubmittal_desc":
                    {
                        jVMAUserMDAHistory.Assets = (
                            from x in jVMAUserMDAHistory.Assets
                            orderby x.DateOfDFSubmittal descending
                            select x).ToList<JVMAUserMDAViewModel>();
                        break;
                    }
                case "DateOfDFSubmittal":
                    {
                        jVMAUserMDAHistory.Assets = (
                            from x in jVMAUserMDAHistory.Assets
                            orderby x.DateOfDFSubmittal
                            select x).ToList<JVMAUserMDAViewModel>();
                        break;
                    }
                case "DateOfLOISubmittal_desc":
                    {
                        jVMAUserMDAHistory.Assets = (
                            from x in jVMAUserMDAHistory.Assets
                            orderby x.DateOfLOISubmittal descending
                            select x).ToList<JVMAUserMDAViewModel>();
                        break;
                    }
                case "DateOfLOISubmittal":
                    {
                        jVMAUserMDAHistory.Assets = (
                            from x in jVMAUserMDAHistory.Assets
                            orderby x.DateOfLOISubmittal
                            select x).ToList<JVMAUserMDAViewModel>();
                        break;
                    }
                case "ProposedCOE_desc":
                    {
                        jVMAUserMDAHistory.Assets = (
                            from x in jVMAUserMDAHistory.Assets
                            orderby x.ProposedCOE descending
                            select x).ToList<JVMAUserMDAViewModel>();
                        break;
                    }
                case "ProposedCOE":
                    {
                        jVMAUserMDAHistory.Assets = (
                            from x in jVMAUserMDAHistory.Assets
                            orderby x.ProposedCOE
                            select x).ToList<JVMAUserMDAViewModel>();
                        break;
                    }
                case "ActualCOE_desc":
                    {
                        jVMAUserMDAHistory.Assets = (
                            from x in jVMAUserMDAHistory.Assets
                            orderby x.ActualCOE descending
                            select x).ToList<JVMAUserMDAViewModel>();
                        break;
                    }
                case "ActualCOE":
                    {
                        jVMAUserMDAHistory.Assets = (
                            from x in jVMAUserMDAHistory.Assets
                            orderby x.ActualCOE
                            select x).ToList<JVMAUserMDAViewModel>();
                        break;
                    }
                case "DateRefFeePaid_desc":
                    {
                        jVMAUserMDAHistory.Assets = (
                            from x in jVMAUserMDAHistory.Assets
                            orderby x.DateRefFeePaid descending
                            select x).ToList<JVMAUserMDAViewModel>();
                        break;
                    }
                case "DateRefFeePaid":
                    {
                        jVMAUserMDAHistory.Assets = (
                            from x in jVMAUserMDAHistory.Assets
                            orderby x.DateRefFeePaid
                            select x).ToList<JVMAUserMDAViewModel>();
                        break;
                    }
                default:
                    {
                        jVMAUserMDAHistory.Assets = (
                            from x in jVMAUserMDAHistory.Assets
                            orderby x.AssetNumber
                            select x).ToList<JVMAUserMDAViewModel>();
                        break;
                    }
            }
            int num = 0;
            TempDataDictionary tempData = base.TempData;
            int? rowCount = model.RowCount;
            if (rowCount.HasValue)
            {
                rowCount = model.RowCount;
                value = rowCount.Value;
            }
            else
            {
                value = 50;
            }
            num = value;
            tempData["RowCount"] = value;
            base.TempData.Keep("RowCount");
            rowCount = model.Page;
            int num1 = (rowCount.HasValue ? rowCount.GetValueOrDefault() : 1);
            model.Assets = jVMAUserMDAHistory.Assets.ToPagedList<JVMAUserMDAViewModel>(num1, num);
            return base.View(model);
        }

        #region Assests Search 
        [HttpGet]
        public ActionResult AssetSearchView()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AssetSearch()
        {
            // Create a new SavedAssetSearchViewModel object to hold information that will be displayed to the user
            SavedAssetSearchViewModel model = new SavedAssetSearchViewModel();

            if (User.Identity.IsAuthenticated)
            {
                //ToDo
                //// Get user information
                //UserModel userByUsername = _userManager.GetUserByUsername(base.User.Identity.Name);

                //// Get a list of the user's saved searches
                //model.SavedSearches = _assetManager.GetSavedSearchesForUser(userByUsername.UserId);
            }

            // Return this information to the view
            return View(model);
        }





        [AllowAnonymous]
        [HttpPost]
        public JsonResult SearchAssets(SearchAssetModel model)
        {
            UserModel userByUsername = _user.GetUserByUsername(base.User.Identity.Name);
            int? userId = (userByUsername == null) ? null : (int?)userByUsername.UserId;
            var data = _asset.SearchAssetsForSearch(model, userId);
            data.SearchId = Guid.NewGuid();

            return new JsonResult() { Data = data };
        }
        #endregion
    }
}