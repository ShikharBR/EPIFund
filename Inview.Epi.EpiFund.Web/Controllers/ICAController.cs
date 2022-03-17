using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.ViewModel;
using Inview.Epi.EpiFund.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Inview.Epi.EpiFund.Web.Controllers
{
    public class ICAController : BaseController
    {
        private IAssetManager _asset;

        private IUserManager _user;

        private IFileManager _fileManager;

        private IEPIFundEmailService _email;

        private IPortfolioManager _portfolio;
        
        private IPDFService _pdf;
        
        private static Random random = new Random();

        private UserModel AuthenticatedUser
        {
            get
            {
                if (!base.User.Identity.IsAuthenticated)
                {
                    return null;
                }
                return this._user.GetUserByUsername(base.User.Identity.Name);
            }
        }

        public ICAController(IAssetManager asset, IUserManager user, IFileManager fileManager, IEPIFundEmailService email, IPDFService pdf, ISecurityManager securityManager, IPortfolioManager portfolio) : base(securityManager, email, user)
        {
            this._pdf = pdf;
            this._email = email;
            this._user = user;
            this._asset = asset;
            this._fileManager = fileManager;
            this._portfolio = portfolio;
        }

        [Authorize]
        [HttpGet]
        public ActionResult SearchAssets()
        {
            var emptyModel = new AdminAssetSearchResultsModel();
            emptyModel.Assets = (new List<AdminAssetQuickListModel>()).ToPagedList(1, 50);
            emptyModel.Portfolios = (new List<PortfolioQuickListViewModel>()).ToPagedList(1, 50);
            base.ViewBag.IsMasterDBSearch = true;
            return base.View(emptyModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult SearchAssets(AdminAssetSearchResultsModel model, string sortOrder, int? page, string assetNumber = null)
        {
            int value;
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            if (userByUsername.UserType != Domain.Enum.UserType.ICAdmin)
            {
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
                return base.RedirectToAction("Index", "Home");
            }
            if (assetNumber != null)
            {
                model.AssetNumber = assetNumber;
            }
            List<AdminAssetQuickListModel> adminAssetQuickListModels = new List<AdminAssetQuickListModel>();
            List<PortfolioQuickListViewModel> portfolioQuickListViewModels = new List<PortfolioQuickListViewModel>();
            ManageAssetsModel manageAssetsModel = new ManageAssetsModel()
            {
                AddressLine1 = model.AddressLine1,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                AssetNumber = model.AssetNumber,
                AssetType = model.SelectedAssetType.GetValueOrDefault(0),
                AssetName = model.AssetName,
                AccListPrice = model.AccListPrice,
                AccUnits = model.AccUnits,
                MinSquareFeet = model.MinSquareFeet,
                MaxSquareFeet = model.MaxSquareFeet,
                MaxUnitsSpaces = model.MaxUnitsSpaces,
                MinUnitsSpaces = model.MinUnitsSpaces,
                UserId = userByUsername.UserId,
                ControllingUserType = Domain.Enum.UserType.CorpAdmin,
                IsPaper = model.IsPaper,
                ApnNumber = model.ApnNumber,                
            };
            adminAssetQuickListModels = this._asset.GetManageAssetsQuickList(manageAssetsModel);
            portfolioQuickListViewModels = this._asset.GetManageAssetQuickListPF(manageAssetsModel);

            ((dynamic)base.ViewBag).CurrentSort = model.SortOrder;
            base.ViewBag.ShowSortParm = (model.SortOrder == "show" ? "show_desc" : "show");
            base.ViewBag.TypeSortParm = (model.SortOrder == "type" ? "type_desc" : "type");
            base.ViewBag.CitySortParm = (model.SortOrder == "city" ? "city_desc" : "city");
            base.ViewBag.StateSortParm = (model.SortOrder == "state" ? "state_desc" : "state");
            base.ViewBag.ZipCodeParm = (model.SortOrder == "zip" ? "zip_desc" : "zip");
            base.ViewBag.StatusSortParm = (model.SortOrder == "status" ? "status_desc" : "status");
            base.ViewBag.AddressSortParm = (model.SortOrder == "address" ? "address_desc" : "address");
            base.ViewBag.AssetIdSortParm = (model.SortOrder == "assetId" ? "assetId_desc" : "assetId");
            base.ViewBag.AssetNameSortParm = (model.SortOrder == "assetName" ? "assetName_desc" : "assetName");
            base.ViewBag.UserType = userByUsername.UserType;
            base.ViewBag.IsMasterDBSearch = true;
            base.ViewBag.PostUrl = "SearchAssets";
            string str = model.SortOrder;
            switch (str)
            {
                case "assetName_desc":
                    {
                        adminAssetQuickListModels = (
                            from a in adminAssetQuickListModels
                            orderby a.AssetName descending
                            select a).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "assetName":
                    {
                        adminAssetQuickListModels = (
                            from a in adminAssetQuickListModels
                            orderby a.AssetName
                            select a).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "createdBy_desc":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.CreatedBy descending
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "type_desc":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.Type descending
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "type":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.Type
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "show_desc":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.Show descending
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "show":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.Show
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "city_desc":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.City descending
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "city":
                    {
                        adminAssetQuickListModels = (
                            from w in adminAssetQuickListModels
                            orderby w.City
                            select w).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "state_desc":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.State descending
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "state":
                    {
                        adminAssetQuickListModels = (
                            from w in adminAssetQuickListModels
                            orderby w.State
                            select w).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "address_desc":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.AddressLine1 descending
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "address":
                    {
                        adminAssetQuickListModels = (
                            from w in adminAssetQuickListModels
                            orderby w.AddressLine1
                            select w).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "assetId_desc":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.AssetNumber descending
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "assetId":
                    {
                        adminAssetQuickListModels = (
                            from w in adminAssetQuickListModels
                            orderby w.AssetNumber
                            select w).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "status_desc":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.Status descending
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "status":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.Status
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "zip_desc":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.Zip descending
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                default:
                    {
                        adminAssetQuickListModels = (str == "zip" ? (
                            from w in adminAssetQuickListModels
                            orderby w.Zip
                            select w).ToList<AdminAssetQuickListModel>() : (
                            from s in adminAssetQuickListModels
                            orderby s.AssetNumber
                            select s).ToList<AdminAssetQuickListModel>());
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
            if (rowCount.GetValueOrDefault(0) > 0)
            {
                page = model.Page;
            }
            rowCount = page;
            int num1 = (rowCount.HasValue ? rowCount.GetValueOrDefault() : 1);
            model.Assets = adminAssetQuickListModels.ToPagedList<AdminAssetQuickListModel>(num1, num);
            model.Portfolios = portfolioQuickListViewModels.ToPagedList<PortfolioQuickListViewModel>(num1, num);
            return base.View(model);
        }

        [Authorize]
        public ActionResult ICACache(AdminAssetSearchResultsModel model, string sortOrder, int? page, string assetNumber = null)
        {
            int value;
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
            if (!base.ValidateAdminUser(userByUsername))
            {
                base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
                return base.RedirectToAction("Index", "Home");
            }
            this._user.RemoveUserAssetLocks(base.User.Identity.Name);
            if (assetNumber != null)
            {
                model.AssetNumber = assetNumber;
            }
            List<AdminAssetQuickListModel> adminAssetQuickListModels = new List<AdminAssetQuickListModel>();
            List<PortfolioQuickListViewModel> portfolioQuickListViewModels = new List<PortfolioQuickListViewModel>();
            ManageAssetsModel manageAssetsModel = new ManageAssetsModel()
            {
                AddressLine1 = model.AddressLine1,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                AssetNumber = model.AssetNumber,
                AssetType = model.SelectedAssetType.GetValueOrDefault(0),
                AssetName = model.AssetName,
                AccListPrice = model.AccListPrice,
                AccUnits = model.AccUnits,
                MinSquareFeet = model.MinSquareFeet,
                MaxSquareFeet = model.MaxSquareFeet,
                MaxUnitsSpaces = model.MaxUnitsSpaces,
                MinUnitsSpaces = model.MinUnitsSpaces,
                UserId = new int?(userByUsername.UserId),
                ControllingUserType = userByUsername.UserType,
                IsPaper = model.IsPaper,
                ApnNumber = model.ApnNumber
            };
            adminAssetQuickListModels = this._asset.GetManageAssetsQuickList(manageAssetsModel);
            portfolioQuickListViewModels = this._asset.GetManageAssetQuickListPF(manageAssetsModel);

            ((dynamic)base.ViewBag).CurrentSort = model.SortOrder;
            base.ViewBag.ShowSortParm = (model.SortOrder == "show" ? "show_desc" : "show");
            base.ViewBag.TypeSortParm = (model.SortOrder == "type" ? "type_desc" : "type");
            base.ViewBag.CitySortParm = (model.SortOrder == "city" ? "city_desc" : "city");
            base.ViewBag.StateSortParm = (model.SortOrder == "state" ? "state_desc" : "state");
            base.ViewBag.ZipCodeParm = (model.SortOrder == "zip" ? "zip_desc" : "zip");
            base.ViewBag.StatusSortParm = (model.SortOrder == "status" ? "status_desc" : "status");
            base.ViewBag.AddressSortParm = (model.SortOrder == "address" ? "address_desc" : "address");
            base.ViewBag.AssetIdSortParm = (model.SortOrder == "assetId" ? "assetId_desc" : "assetId");
            base.ViewBag.CreatedSortParm = (model.SortOrder == "createdBy" ? "createdBy_desc" : "createdBy");
            base.ViewBag.AssetNameSortParm = (model.SortOrder == "assetName" ? "assetName_desc" : "assetName");
            base.ViewBag.UserType = userByUsername.UserType;
            base.ViewBag.IsMasterDBSearch = false;
            base.ViewBag.PostUrl = "ICACache";
            string str = model.SortOrder;
            switch (str)
            {
                case "assetName_desc":
                    {
                        adminAssetQuickListModels = (
                            from a in adminAssetQuickListModels
                            orderby a.AssetName descending
                            select a).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "assetName":
                    {
                        adminAssetQuickListModels = (
                            from a in adminAssetQuickListModels
                            orderby a.AssetName
                            select a).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "createdBy_desc":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.CreatedBy descending
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "createdBy":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.CreatedBy
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "type_desc":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.Type descending
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "type":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.Type
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "show_desc":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.Show descending
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "show":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.Show
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "city_desc":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.City descending
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "city":
                    {
                        adminAssetQuickListModels = (
                            from w in adminAssetQuickListModels
                            orderby w.City
                            select w).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "state_desc":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.State descending
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "state":
                    {
                        adminAssetQuickListModels = (
                            from w in adminAssetQuickListModels
                            orderby w.State
                            select w).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "address_desc":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.AddressLine1 descending
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "address":
                    {
                        adminAssetQuickListModels = (
                            from w in adminAssetQuickListModels
                            orderby w.AddressLine1
                            select w).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "assetId_desc":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.AssetNumber descending
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "assetId":
                    {
                        adminAssetQuickListModels = (
                            from w in adminAssetQuickListModels
                            orderby w.AssetNumber
                            select w).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "status_desc":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.Status descending
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "status":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.Status
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                case "zip_desc":
                    {
                        adminAssetQuickListModels = (
                            from s in adminAssetQuickListModels
                            orderby s.Zip descending
                            select s).ToList<AdminAssetQuickListModel>();
                        break;
                    }
                default:
                    {
                        adminAssetQuickListModels = (str == "zip" ? (
                            from w in adminAssetQuickListModels
                            orderby w.Zip
                            select w).ToList<AdminAssetQuickListModel>() : (
                            from s in adminAssetQuickListModels
                            orderby s.AssetNumber
                            select s).ToList<AdminAssetQuickListModel>());
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
            if (rowCount.GetValueOrDefault(0) > 0)
            {
                page = model.Page;
            }
            rowCount = page;
            int num1 = (rowCount.HasValue ? rowCount.GetValueOrDefault() : 1);
            model.Assets = adminAssetQuickListModels.ToPagedList<AdminAssetQuickListModel>(num1, num);
            model.Portfolios = portfolioQuickListViewModels.ToPagedList<PortfolioQuickListViewModel>(num1, num);
            return base.View(model);
        }


    }
}