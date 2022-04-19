using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.ViewModel;
using Inview.Epi.EpiFund.Web.ActionFilters;
using Inview.Epi.EpiFund.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Web.Controllers
{
	[LayoutActionFilter]
	public class PortfolioController : BaseController
	{
		private IAssetManager _asset;

		private IUserManager _user;

		private ISecurityManager _securityManager;

		private IEPIFundEmailService _email;

		private IPortfolioManager _portfolio;

		private IAuthProvider _auth;

		public PortfolioController(IAssetManager asset, IUserManager user, ISecurityManager securityManager, IEPIFundEmailService email, IPortfolioManager portfolio, IAuthProvider auth) : base(securityManager, email, user)
		{
			this._asset = asset;
			this._user = user;
			this._securityManager = securityManager;
			this._email = email;
			this._portfolio = portfolio;
			this._auth = auth;
		}

		[Authorize]
		public ActionResult ActivateAssetfromPortfolio(Guid id, Guid pfId)
		{
			this._portfolio.ActivateAsset(id, pfId);
			return base.RedirectToAction("UpdatePortfolio", new { id = pfId });
		}

		[Authorize]
		public ActionResult ActivatePortfolio(Guid id)
		{
			this._portfolio.ActivatePortfolio(id);
			return base.RedirectToAction("ManagePortfolios", "Portfolio");
		}

		[HttpPost]
		public List<AdminAssetQuickListModel> AssetsList(List<AdminAssetQuickListModel> assets)
		{
			this._user.RemoveUserAssetLocks(base.User.Identity.Name);
			((dynamic)base.ViewBag).AssetType = base.populateAssetTypeDDL();
			((dynamic)base.ViewBag).PortfolioList = this.populatePortfolioListDDL();
			if (assets != null)
			{
				HttpRuntime.Cache["AssetList"] = assets;
			}
			return assets;
		}

		[Authorize]
		[HttpGet]
		public ActionResult CreatePortfolio()
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			if (!base.ValidatePFUser(userByUsername))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			PortfolioViewModel portfolioViewModel = new PortfolioViewModel();
			portfolioViewModel = this.PopulateAssetList(portfolioViewModel);
			portfolioViewModel.ControllingUserType = new UserType?(userByUsername.UserType);
			portfolioViewModel.isIntial = true;
			this.SearchAssets(portfolioViewModel);
			this.GetLayout(userByUsername);
			return base.View(portfolioViewModel);
		}

		[Authorize]
		[HttpPost]
		public ActionResult CreatePortfolio(PortfolioViewModel model)
		{
			ActionResult actionResult;
			bool flag;
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			try
			{
				this.GetLayout(userByUsername);
				model.isIntial = false;
				this.SearchAssets(model);
				if (model.AssetList == null)
				{
					model = this.PopulateAssetList(model);
				}
				DateTime? lastReportedOccupancyDate = model.LastReportedOccupancyDate;
				DateTime minValue = DateTime.MinValue;
				if (lastReportedOccupancyDate.HasValue)
				{
					flag = (lastReportedOccupancyDate.HasValue ? lastReportedOccupancyDate.GetValueOrDefault() == minValue : true);
				}
				else
				{
					flag = false;
				}
				if (!flag)
				{
					lastReportedOccupancyDate = model.LastReportedOccupancyDate;
					if (lastReportedOccupancyDate.HasValue)
					{
						goto Label1;
					}
				}
				model.LastReportedOccupancyDate = new DateTime?(new DateTime(2015, 1, 1));
			Label1:
				if (HttpRuntime.Cache["AssetList"] != null)
				{
					model.Assets = (List<AdminAssetQuickListModel>)HttpRuntime.Cache["AssetList"];
				}
				if (model.PortfolioProperties.Count < 1)
				{
					model.PortfolioProperties = this._portfolio.GetPortfolioProperties(model.PortfolioId);
				}
				model.SelectedAssets = (
					from w in model.Assets
					where w.IsSelected
					select w into s
					select s.AssetId).ToList<Guid>();
				if (!base.ModelState.IsValid)
				{
					foreach (ModelError list in base.ModelState.Values.SelectMany<System.Web.Mvc.ModelState, ModelError>((System.Web.Mvc.ModelState v) => v.Errors).ToList<ModelError>())
					{
						if (!list.Exception.Message.ToLower().Contains("adminassetquicklistmodel"))
						{
							continue;
						}
						base.ModelState.Remove("Assets");
						base.ModelState.Add("Assets", new System.Web.Mvc.ModelState());
						base.ModelState.SetModelValue("Assets", new ValueProviderResult(new List<AdminAssetQuickListModel>(), "Assets", null));
						break;
					}
				}
				if (!base.ModelState.IsValid)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problems with data input fields. Please review below");
					this.GetLayout(userByUsername);
					model.Assets = new List<AdminAssetQuickListModel>();
					actionResult = base.View(model);
				}
				else if (this._portfolio.PortfolioExist(model.PortfolioName))
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "A Portfolio with this name already exists in the system.");
					this.GetLayout(userByUsername);
					model.Assets = new List<AdminAssetQuickListModel>();
					actionResult = base.View(model);
				}
				else
				{
					var portfolioId = this._portfolio.CreatePortfolio(model, userByUsername.UserId);
					base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Portfolio successfully created.");
					return base.RedirectToAction("ManagePortfolios", "Portfolio", new { portfolioNumber = portfolioId.ToString() });
                }
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, string.Concat("An issue occured during saving. error: ", exception.Message));
				this.GetLayout(userByUsername);
				model.Assets = new List<AdminAssetQuickListModel>();
				actionResult = base.View(model);
			}
			return actionResult;
		}

		[Authorize]
		public ActionResult DeactivateAssetfromPortfolio(Guid id, Guid pfId)
		{
			this._portfolio.DeactivateAsset(id, pfId);
			return base.RedirectToAction("UpdatePortfolio", new { id = pfId });
		}

		[Authorize]
		public ActionResult DeactivatePortfolio(Guid id)
		{
			this._portfolio.DeactivatePortfolio(id);
			return base.RedirectToAction("ManagePortfolios", "Portfolio");
		}

		[HttpGet]
		public ActionResult Error()
		{
			return base.View();
		}

		private void GetLayout(UserModel user)
		{
			base.ViewBag.IsSeller = (user.UserType == UserType.ListingAgent ? true : user.UserType == UserType.Investor);
			base.ViewBag.IsCorpAdmin = (this.isUserAdmin(user) || user.UserType == UserType.ListingAgent ? true : user.UserType == UserType.Investor);
			base.ViewBag.IsICAdmin = (user.UserType == UserType.ICAdmin ? true : false);
			if (((dynamic)base.ViewBag).IsSeller)
			{
				((dynamic)base.ViewBag).Layout = "~/Views/Shared/_Layout.cshtml";
				return;
			}
			((dynamic)base.ViewBag).Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
		}

		private bool isUserAdmin(UserModel userModel)
		{
			if (userModel.UserType != UserType.CorpAdmin && userModel.UserType != UserType.SiteAdmin)
			{
				return false;
			}
			return true;
		}

		[Authorize]
		public ActionResult ManagePortfolios(AdminPortfolioResultsModel model, string sortOrder, int? page, string portfolioNumber = null)
		{
            // default sort order is now name
            if(sortOrder == null)
                model.SortOrder = "name";

            int value;
            this._user.RemoveUserAssetLocks(base.User.Identity.Name);
			List<PortfolioQuickListViewModel> portfolioQuickListViewModels = new List<PortfolioQuickListViewModel>();
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			this.GetLayout(userByUsername);
			if (!base.ValidatePFUser(userByUsername))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			model.ControllingUserType = new UserType?(userByUsername.UserType);
			ManagePortfoliosModel managePortfoliosModel = new ManagePortfoliosModel()
			{
                PortfolioId = portfolioNumber == null ? Guid.Empty : new Guid(portfolioNumber),
				AddressLine1 = model.AddressLine1,
				City = model.City,
				APNNumber = model.APN,
				PortfolioName = model.PortfolioName,
				UserId = new int?(userByUsername.UserId),
				State = model.State,
				ZipCode = model.ZipCode,
				AssetNumber = model.AssetNumber,
				AssetType = model.SelectedAssetType.GetValueOrDefault(0),
				AssetName = model.AssetName,
				ControllingUserType = userByUsername.UserType,
                IsSearching = model.IsSearching
			};
			portfolioQuickListViewModels = this._portfolio.GetSearchPortfolios(managePortfoliosModel);
			((dynamic)base.ViewBag).CurrentSort = model.SortOrder;
			base.ViewBag.PortfolioIdSortParm = (model.SortOrder == "assetId" ? "assetId_desc" : "assetId");
			base.ViewBag.NameSortParm = (model.SortOrder == "name" ? "name_desc" : "name");
            base.ViewBag.NumberSortParm = (model.SortOrder == "number" ? "number_desc" : "number");
			string str = model.SortOrder;
			if (str == "name_desc")
			{
                // Trim the portfolio names eliminating any spaces that could meddle with the sort
                // Sort portfolios by PortfolioName in descending order
                portfolioQuickListViewModels = _portfolio.TrimStringProperty(portfolioQuickListViewModels);
                portfolioQuickListViewModels = _portfolio.SortPortfoliosModel(portfolioQuickListViewModels, true);
            }
            else if (str == "name")
			{
                // Trim the portfolio names eliminating any spaces that could meddle with the sort
                // Sort portfolios by PortfolioName 
                portfolioQuickListViewModels = _portfolio.TrimStringProperty(portfolioQuickListViewModels);
                portfolioQuickListViewModels = _portfolio.SortPortfoliosModel(portfolioQuickListViewModels, false);
                
			}
			else if (str == "number_desc")
			{
                
                portfolioQuickListViewModels = (
					from s in portfolioQuickListViewModels
					orderby s.PortfolioAssets.Count descending
					select s).ToList<PortfolioQuickListViewModel>();
			}
			else if (str == "number")
			{
				portfolioQuickListViewModels = (
					from w in portfolioQuickListViewModels
					orderby w.PortfolioAssets.Count
                    select w).ToList<PortfolioQuickListViewModel>();
			}
			else if (str == "assetId_desc")
			{
				portfolioQuickListViewModels = (
					from s in portfolioQuickListViewModels
					orderby s.PortfolioId descending
					select s).ToList<PortfolioQuickListViewModel>();
			}
			else
			{
                // by default sort by name ascending 
                portfolioQuickListViewModels = _portfolio.TrimStringProperty(portfolioQuickListViewModels);
                portfolioQuickListViewModels = _portfolio.SortPortfoliosModel(portfolioQuickListViewModels, false);
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
            model.Portfolios = portfolioQuickListViewModels.ToPagedList<PortfolioQuickListViewModel>(num1, num);
			return base.View(model);
		}

		[Authorize]
		public ActionResult MyUSCPage()
		{
			InsuranceCompanyUserViewModel insuranceUserByEmail = this._user.GetInsuranceUserByEmail(base.User.Identity.Name);
			InsuranceUSCViewModel insuranceUSCViewModel = new InsuranceUSCViewModel()
			{
				CompanyName = insuranceUserByEmail.Company.CompanyName,
				FullName = insuranceUserByEmail.FullName
			};
			((dynamic)base.ViewBag).Email = base.User.Identity.Name.ToLower();
			return base.View(insuranceUSCViewModel);
		}

		public ActionResult OpenExistingCalc(Guid id)
		{
			return this.PartialView("ExistingDebtFund", this._portfolio.GetPortfolio(id));
		}

		public ActionResult OpenProposedCalc(Guid id)
		{
			return this.PartialView("ProposedDebtFund", this._portfolio.GetPortfolio(id));
		}

		private PortfolioViewModel PopulateAssetList(PortfolioViewModel model)
		{
			List<Asset> assetList = this._asset.GetAssetList();
			model.AssetList = (
				from c in assetList
				select new SelectListItem()
				{
					Text = c.ProjectName,
					Value = c.AssetId.ToString()
				}).ToList<SelectListItem>();
			return model;
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
		public PartialViewResult SearchAssets(PortfolioViewModel model)
		{
			int value;
			this._user.RemoveUserAssetLocks(base.User.Identity.Name);
			((dynamic)base.ViewBag).AssetType = base.populateAssetTypeDDL();
			((dynamic)base.ViewBag).PortfolioList = this.populatePortfolioListDDL();
			List<AdminAssetQuickListModel> adminAssetQuickListModels = new List<AdminAssetQuickListModel>();
			ManageAssetsModel manageAssetsModel = new ManageAssetsModel()
			{
				AddressLine1 = model.AddressLine1,
				City = model.City,
				EndDate = model.EndDate,
				StartDate = model.StartDate,
				State = model.State,
				ZipCode = model.ZipCode,
				AssetNumber = model.AssetNumber,
				AssetType = model.SelectedAssetType.GetValueOrDefault(0),
				AssetName = model.AssetName
			};
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			manageAssetsModel.UserId = new int?(userByUsername.UserId);
			manageAssetsModel.ControllingUserType = userByUsername.UserType;
			if (this.ValidSearch(manageAssetsModel) && !model.isIntial)
			{
				adminAssetQuickListModels = this._asset.GetManageAssetsQuickList(manageAssetsModel);
			}
			adminAssetQuickListModels = this.SortAssetList(model.SortOrder, adminAssetQuickListModels);
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
			int? nullable = new int?(0);
			rowCount = model.Page;
			if (rowCount.GetValueOrDefault(0) > 0)
			{
				nullable = model.Page;
			}
			rowCount = nullable;
			if (rowCount.HasValue)
			{
				rowCount.GetValueOrDefault();
			}
			model.Assets = adminAssetQuickListModels;
			return this.PartialView("_SearchAssetListView", model.Assets);
		}

		private List<AdminAssetQuickListModel> SortAssetList(string SortOrder, List<AdminAssetQuickListModel> assets)
		{
			((dynamic)base.ViewBag).CurrentSort = SortOrder;
			base.ViewBag.ShowSortParm = (SortOrder == "show" ? "show_desc" : "show");
			base.ViewBag.TypeSortParm = (SortOrder == "type" ? "type_desc" : "type");
			base.ViewBag.CitySortParm = (SortOrder == "city" ? "city_desc" : "city");
			base.ViewBag.StateSortParm = (SortOrder == "state" ? "state_desc" : "state");
			base.ViewBag.ZipCodeParm = (SortOrder == "zip" ? "zip_desc" : "zip");
			base.ViewBag.StatusSortParm = (SortOrder == "status" ? "status_desc" : "status");
			base.ViewBag.AddressSortParm = (SortOrder == "address" ? "address_desc" : "address");
			base.ViewBag.AssetIdSortParm = (SortOrder == "assetId" ? "assetId_desc" : "assetId");
			base.ViewBag.CreatedSortParm = (SortOrder == "createdBy" ? "createdBy_desc" : "createdBy");
			base.ViewBag.AssetNameSortParm = (SortOrder == "assetName" ? "assetName_desc" : "assetName");
			switch (SortOrder)
			{
				case "assetName_desc":
				{
					assets = (
						from a in assets
						orderby a.AssetName descending
						select a).ToList<AdminAssetQuickListModel>();
					break;
				}
				case "assetName":
				{
					assets = (
						from a in assets
						orderby a.AssetName
						select a).ToList<AdminAssetQuickListModel>();
					break;
				}
				case "createdBy_desc":
				{
					assets = (
						from s in assets
						orderby s.CreatedBy descending
						select s).ToList<AdminAssetQuickListModel>();
					break;
				}
				case "createdBy":
				{
					assets = (
						from s in assets
						orderby s.CreatedBy
						select s).ToList<AdminAssetQuickListModel>();
					break;
				}
				case "type_desc":
				{
					assets = (
						from s in assets
						orderby s.Type descending
						select s).ToList<AdminAssetQuickListModel>();
					break;
				}
				case "type":
				{
					assets = (
						from s in assets
						orderby s.Type
						select s).ToList<AdminAssetQuickListModel>();
					break;
				}
				case "show_desc":
				{
					assets = (
						from s in assets
						orderby s.Show descending
						select s).ToList<AdminAssetQuickListModel>();
					break;
				}
				case "show":
				{
					assets = (
						from s in assets
						orderby s.Show
						select s).ToList<AdminAssetQuickListModel>();
					break;
				}
				case "city_desc":
				{
					assets = (
						from s in assets
						orderby s.City descending
						select s).ToList<AdminAssetQuickListModel>();
					break;
				}
				case "city":
				{
					assets = (
						from w in assets
						orderby w.City
						select w).ToList<AdminAssetQuickListModel>();
					break;
				}
				case "state_desc":
				{
					assets = (
						from s in assets
						orderby s.State descending
						select s).ToList<AdminAssetQuickListModel>();
					break;
				}
				case "state":
				{
					assets = (
						from w in assets
						orderby w.State
						select w).ToList<AdminAssetQuickListModel>();
					break;
				}
				case "address_desc":
				{
					assets = (
						from s in assets
						orderby s.AddressLine1 descending
						select s).ToList<AdminAssetQuickListModel>();
					break;
				}
				case "address":
				{
					assets = (
						from w in assets
						orderby w.AddressLine1
						select w).ToList<AdminAssetQuickListModel>();
					break;
				}
				case "assetId_desc":
				{
					assets = (
						from s in assets
						orderby s.AssetNumber descending
						select s).ToList<AdminAssetQuickListModel>();
					break;
				}
				case "assetId":
				{
					assets = (
						from w in assets
						orderby w.AssetNumber
						select w).ToList<AdminAssetQuickListModel>();
					break;
				}
				case "status_desc":
				{
					assets = (
						from s in assets
						orderby s.Status descending
						select s).ToList<AdminAssetQuickListModel>();
					break;
				}
				case "status":
				{
					assets = (
						from s in assets
						orderby s.Status
						select s).ToList<AdminAssetQuickListModel>();
					break;
				}
				case "zip_desc":
				{
					assets = (
						from s in assets
						orderby s.Zip descending
						select s).ToList<AdminAssetQuickListModel>();
					break;
				}
				case "zip":
				{
					assets = (
						from w in assets
						orderby w.Zip
						select w).ToList<AdminAssetQuickListModel>();
					break;
				}
				default:
				{
					assets = (
						from s in assets
						orderby s.AssetNumber
						select s).ToList<AdminAssetQuickListModel>();
					break;
				}
			}
			return assets;
		}

		[HttpPost]
		public PartialViewResult SortSearchAssets(string stringModel, string sortOrder)
		{
			AssetType assetType;
			List<AdminAssetQuickListModel> adminAssetQuickListModels = new List<AdminAssetQuickListModel>();
			string[] strArrays = stringModel.Split(new char[] { '&' });
			Dictionary<string, string> strs = new Dictionary<string, string>();
			string[] strArrays1 = strArrays;
			for (int i = 0; i < (int)strArrays1.Length; i++)
			{
				string[] strArrays2 = strArrays1[i].Split(new char[] { '=' });
				strs.Add(strArrays2[0], strArrays2[1]);
			}
			DateTime maxValue = DateTime.MaxValue;
			if (strs["EndDate"].Length > 1)
			{
				maxValue = DateTime.Parse(strs["EndDate"]);
			}
			DateTime minValue = DateTime.MinValue;
			if (strs["StartDate"].Length > 1)
			{
				minValue = DateTime.Parse(strs["StartDate"]);
			}
			if (strs["SelectedAssetType"].Length <= 1)
			{
				assetType = (AssetType)0;
			}
			else
			{
				assetType = (AssetType)Enum.Parse(typeof(AssetType), strs["SelectedAssetType"]);
			}
			ManageAssetsModel manageAssetsModel = new ManageAssetsModel()
			{
				AddressLine1 = strs["AddressLine1"],
				City = strs["City"],
				EndDate = new DateTime?(maxValue),
				StartDate = new DateTime?(minValue),
				State = strs["State"],
				ZipCode = strs["ZipCode"],
				AssetNumber = strs["AssetNumber"],
				AssetType = assetType,
				AssetName = strs["AssetName"]
			};
			adminAssetQuickListModels = this._asset.GetManageAssetsQuickList(manageAssetsModel);
			if (adminAssetQuickListModels.Count < 1 && HttpRuntime.Cache["AssetList"] != null)
			{
				adminAssetQuickListModels = (List<AdminAssetQuickListModel>)HttpRuntime.Cache["AssetList"];
			}
			adminAssetQuickListModels = this.SortAssetList(sortOrder, adminAssetQuickListModels);
			return this.PartialView("_SearchAssetListView", adminAssetQuickListModels);
		}

		[Authorize]
		[HttpGet]
		public ActionResult UpdatePortfolio(Guid id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			this.GetLayout(userByUsername);
			if (!base.ValidatePFUser(userByUsername))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			PortfolioViewModel portfolio = this._portfolio.GetPortfolio(id);
			portfolio.ControllingUserType = new UserType?(userByUsername.UserType);
			portfolio = this.PopulateAssetList(portfolio);
			portfolio.isIntial = true;
			this.SearchAssets(portfolio);
            if (HttpRuntime.Cache["AssetList"] != null) HttpRuntime.Cache.Remove("AssetList");
            return base.View(portfolio);
		}

		[Authorize]
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult UpdatePortfolio(PortfolioViewModel model)
		{
			ActionResult actionResult;
			bool flag;
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			try
			{
				model.isIntial = false;
				this.SearchAssets(model);
				this.GetLayout(userByUsername);
				DateTime? lastReportedOccupancyDate = model.LastReportedOccupancyDate;
				DateTime minValue = DateTime.MinValue;
				if (lastReportedOccupancyDate.HasValue)
				{
					flag = (lastReportedOccupancyDate.HasValue ? lastReportedOccupancyDate.GetValueOrDefault() == minValue : true);
				}
				else
				{
					flag = false;
				}
				if (!flag)
				{
					lastReportedOccupancyDate = model.LastReportedOccupancyDate;
					if (lastReportedOccupancyDate.HasValue)
					{
						goto Label1;
					}
				}
				model.LastReportedOccupancyDate = new DateTime?(new DateTime(2015, 1, 1));
			Label1:
				if (model.AssetList == null)
				{
					model = this.PopulateAssetList(model);
				}
				if (HttpRuntime.Cache["AssetList"] != null)
				{
					model.Assets = (List<AdminAssetQuickListModel>)HttpRuntime.Cache["AssetList"];
				}
				if (model.PortfolioProperties.Count < 1)
				{
					model.PortfolioProperties = this._portfolio.GetPortfolioProperties(model.PortfolioId);
				}
				model.SelectedAssets = (
					from w in model.Assets
					where w.IsSelected
					select w into s
					select s.AssetId).ToList<Guid>();
				if (!base.ModelState.IsValid)
				{
					foreach (ModelError list in base.ModelState.Values.SelectMany<System.Web.Mvc.ModelState, ModelError>((System.Web.Mvc.ModelState v) => v.Errors).ToList<ModelError>())
					{
						if (!list.Exception.Message.ToLower().Contains("adminassetquicklistmodel"))
						{
							continue;
						}
						base.ModelState.Remove("Assets");
						base.ModelState.Add("Assets", new System.Web.Mvc.ModelState());
						base.ModelState.SetModelValue("Assets", new ValueProviderResult(new List<AdminAssetQuickListModel>(), "Assets", null));
						break;
					}
				}
				if (!base.ModelState.IsValid)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problems with data input fields. Please review below");
					this.GetLayout(userByUsername);
					model.Assets = new List<AdminAssetQuickListModel>();
					actionResult = base.View(model);
				}
				else if (!this._portfolio.PortfolioExist(model.PortfolioId))
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Unable to save this portfolio. Please contact Site Admin.");
					this.GetLayout(userByUsername);
					model.Assets = new List<AdminAssetQuickListModel>();
					actionResult = base.View(model);
				}
				else if (this._portfolio.isPortfolioNameDuplicate(model))
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "A Portfolio with this name already exists in the system.");
					this.GetLayout(userByUsername);
					model.Assets = new List<AdminAssetQuickListModel>();
					actionResult = base.View(model);
				}
				else
				{
                    if (model.CallforOfferDate.HasValue) model.hasOffersDate = true;
                    else model.hasOffersDate = false;

                    this._portfolio.UpdatePortfolio(model, userByUsername.UserId);
					base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Portfolio successfully updated.");

                    if (HttpRuntime.Cache["AssetList"] != null) HttpRuntime.Cache.Remove("AssetList");

                    return base.RedirectToAction("ManagePortfolios", "Portfolio", new { PortfolioNumber = model.PortfolioId.ToString() });
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, string.Concat("An issue occured during saving. error: ", exception.Message));
				this.GetLayout(userByUsername);
				model.Assets = new List<AdminAssetQuickListModel>();
				actionResult = base.View(model);
			}
			return actionResult;
		}

		private bool ValidSearch(ManageAssetsModel results)
		{
			if (results.AddressLine1 != null || results.AssetName != null || results.AssetNumber != null || results.City != null || results.EndDate.HasValue || results.State != null || results.ZipCode != null || (int)results.AssetType != 0)
			{
				return true;
			}
			bool hasValue = results.StartDate.HasValue;
			return true;
		}

		[Authorize]
		[HttpGet]
		public ActionResult ViewPortfolioSummary(Guid id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			this.GetLayout(userByUsername);
			if (base.ValidatePFUser(userByUsername))
			{
				return base.View(this._portfolio.GetPortfolio(id));
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
			return base.RedirectToAction("Index", "Home");
		}
	}
}