using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.Helpers;
using Inview.Epi.EpiFund.Domain.ViewModel;
using Inview.Epi.EpiFund.Web;
using Inview.Epi.EpiFund.Web.ActionFilters;
using Inview.Epi.EpiFund.Web.Models;
using Inview.Epi.EpiFund.Web.Models.Emails;
using Inview.Epi.EpiFund.Web.Properties;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Web.Controllers
{
	[LayoutActionFilter]
	public class InvestorsController : BaseController
	{
		private IUserManager _user;

		private IAssetManager _asset;

		private IPDFService _pdf;

		private ISellerManager _seller;

		private IEPIFundEmailService _email;

		private IPortfolioManager _portfolio;

		public InvestorsController(ISecurityManager securityManager, IEPIFundEmailService email, IUserManager user, IAssetManager asset, IPDFService pdf, ISellerManager seller, IPortfolioManager portfolio) : base(securityManager, email, user)
		{
			this._email = email;
			this._pdf = pdf;
			this._user = user;
			this._asset = asset;
			this._seller = seller;
			this._portfolio = portfolio;
		}

		[HttpGet]
		public FileResult DownloadLOI(int userFileId, Guid loiId)
		{
			this._seller.MarkLOIAsRead(loiId);
			return this.File(this._user.GetUserFile(userFileId), "application/octet-stream", "download.pdf");
		}

		[HttpGet]
		public ActionResult ExecuteNARMDA(Guid id, bool bypass = false)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			IPANARTemplateModel pANARTemplateModel = new IPANARTemplateModel();
			AssetDescriptionModel assetByAssetId = this._asset.GetAssetByAssetId(id);
			pANARTemplateModel.Assets.Add(assetByAssetId);
			if (base.User.Identity.IsAuthenticated)
			{
				pANARTemplateModel.City = userByUsername.City;
				pANARTemplateModel.UserAddressLine1 = userByUsername.AddressLine1;
				pANARTemplateModel.UserAddressLine2 = userByUsername.AddressLine2;
				pANARTemplateModel.State = userByUsername.SelectedState;
				pANARTemplateModel.UserFirstName = userByUsername.FirstName;
				pANARTemplateModel.UserLastName = userByUsername.LastName;
				pANARTemplateModel.Fax = userByUsername.FaxNumber;
				pANARTemplateModel.Phone = userByUsername.CellNumber;
				pANARTemplateModel.Email = userByUsername.Username;
				pANARTemplateModel.CompanyName = userByUsername.CompanyName;
				pANARTemplateModel.CorpTitle = userByUsername.CorporateTitle;
				pANARTemplateModel.FirstNameOfCorporateEntity = userByUsername.AcroynmForCorporateEntity;
				AssetViewModel asset = this._asset.GetAsset(pANARTemplateModel.Assets.First<AssetDescriptionModel>().AssetId, false);
				pANARTemplateModel.DateAccepted = DateTime.Now.ToString("MM/dd/yyyy");
				pANARTemplateModel.PropertyAddress1 = asset.PropertyAddress;
				pANARTemplateModel.PropertyAddress2 = asset.PropertyAddress2;
				pANARTemplateModel.PropertyCity = asset.City;
				pANARTemplateModel.PropertyState = asset.State;
				pANARTemplateModel.PropertyZip = asset.Zip;
				pANARTemplateModel.PropertyCounty = asset.County;
				pANARTemplateModel.PropertyType = EnumHelper.GetEnumDescription(asset.AssetType);
				pANARTemplateModel.PropertyName = asset.ProjectName;
				StringBuilder stringBuilder = new StringBuilder();
				foreach (AssetTaxParcelNumber assetTaxParcelNumber in asset.AssetTaxParcelNumbers)
				{
					stringBuilder.Append(assetTaxParcelNumber.TaxParcelNumber);
					stringBuilder.Append("; ");
				}
				pANARTemplateModel.PropertyApns = stringBuilder.ToString();
			}
			return base.View(pANARTemplateModel);
		}

		[HttpPost]
		[MultipleButton(Name="action", Argument="ExecuteNARMDA")]
		public ActionResult ExecuteNARMDA(IPANARTemplateModel model)
		{
			if (!base.ModelState.IsValid)
			{
				return base.View(model);
			}
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			this._pdf.SubmitIPANAR(model, Properties.Resources.IPANARWOSig, userByUsername.UserId, 1);
			base.TempData["MDAAssets"] = model.Assets;
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully executed. Please wait for an email from DocuSign to sign document.");
			return base.RedirectToAction("DocusignProcess", "DataPortal");
		}

		[HttpGet]
		public ActionResult ExecuteSellerMDA(Guid id, bool bypass = false)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			IPASellerTemplateModel pASellerTemplateModel = new IPASellerTemplateModel();
			AssetViewModel asset = this._asset.GetAsset(id, false);
			AssetDescriptionModel assetByAssetId = this._asset.GetAssetByAssetId(id);
			pASellerTemplateModel.Assets.Add(assetByAssetId);
			if (base.User.Identity.IsAuthenticated)
			{
				pASellerTemplateModel.City = userByUsername.City;
				pASellerTemplateModel.UserAddressLine1 = userByUsername.AddressLine1;
				pASellerTemplateModel.UserAddressLine2 = userByUsername.AddressLine2;
				pASellerTemplateModel.State = userByUsername.SelectedState;
				pASellerTemplateModel.UserFirstName = userByUsername.FirstName;
				pASellerTemplateModel.UserLastName = userByUsername.LastName;
				pASellerTemplateModel.Fax = userByUsername.FaxNumber;
				pASellerTemplateModel.Phone = userByUsername.CellNumber;
				pASellerTemplateModel.Email = userByUsername.Username;
				pASellerTemplateModel.CompanyName = userByUsername.CompanyName;
				pASellerTemplateModel.CorpTitle = userByUsername.CorporateTitle;
				pASellerTemplateModel.FirstNameOfCorporateEntity = userByUsername.AcroynmForCorporateEntity;
				pASellerTemplateModel.PropertyAddress1 = asset.PropertyAddress;
				pASellerTemplateModel.PropertyAddress2 = asset.PropertyAddress2;
				pASellerTemplateModel.PropertyCity = asset.City;
				pASellerTemplateModel.PropertyState = asset.State;
				pASellerTemplateModel.PropertyZip = asset.Zip;
				pASellerTemplateModel.PropertyCounty = asset.County;
				pASellerTemplateModel.PropertyType = EnumHelper.GetEnumDescription(asset.AssetType);
				pASellerTemplateModel.PropertyName = asset.ProjectName;
				StringBuilder stringBuilder = new StringBuilder();
				foreach (AssetTaxParcelNumber assetTaxParcelNumber in asset.AssetTaxParcelNumbers)
				{
					stringBuilder.Append(assetTaxParcelNumber.TaxParcelNumber);
					stringBuilder.Append("; ");
				}
				pASellerTemplateModel.PropertyApns = stringBuilder.ToString();
				pASellerTemplateModel.DateAccepted = DateTime.Now.ToString("MM/dd/yyyy");
			}
			return base.View(pASellerTemplateModel);
		}

		[HttpPost]
		[MultipleButton(Name="action", Argument="ExecuteSellerMDA")]
		public ActionResult ExecuteSellerMDA(IPASellerTemplateModel model)
		{
			if (!base.ModelState.IsValid)
			{
				return base.View(model);
			}
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			this._pdf.SubmitIPASeller(model, Properties.Resources.IPASellerWOSig, userByUsername.UserId, 1);
			base.TempData["MDAAssets"] = model.Assets;
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully executed. Please wait for an email from DocuSign to sign document.");
			return base.RedirectToAction("DocusignProcess", "DataPortal");
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
		[MultipleButton(Name="action", Argument="PreviewNARMDAPdf")]
		public FileResult PreviewNARMDAPdf(IPANARTemplateModel model)
		{
			return this.File(this._pdf.GetIPANARPdf(model, Properties.Resources.IPANARWSig), "application/octet-stream", "download.pdf");
		}

		[HttpPost]
		[MultipleButton(Name="action", Argument="PreviewSellerMDAPdf")]
		public FileResult PreviewSellerMDAPdf(IPASellerTemplateModel model)
		{
			return this.File(this._pdf.GetIPASellerPdf(model, Properties.Resources.IPASellerWSig), "application/octet-stream", "download.pdf");
		}

		public ActionResult PublishAsset(Guid id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			if (!this._asset.HasSellerSignedIPA(id, userByUsername.UserId))
			{
				if (userByUsername.UserType == UserType.ListingAgent)
				{
					return base.RedirectToAction("ExecuteNARMDA", new { id = id });
				}
				return base.RedirectToAction("ExecuteSellerMDA", new { id = id });
			}
			this._asset.PublishAsset(id);
			if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
			{
				AssetViewModel asset = this._asset.GetAsset(id, false);
				AssetDescriptionModel assetByAssetId = this._asset.GetAssetByAssetId(id);
				UserModel userById = this._user.GetUserById(asset.ListedByUserId.Value);
				string empty = string.Empty;
				if (asset.AssetTaxParcelNumbers != null)
				{
					empty = string.Join(",", (
						from x in asset.AssetTaxParcelNumbers
						select x.TaxParcelNumber).ToArray<string>());
				}
				ConfirmationPISellerAssetIsPublishedEmail confirmationPISellerAssetIsPublishedEmail = new ConfirmationPISellerAssetIsPublishedEmail()
				{
					DatePublished = DateTime.Now,
					To = userByUsername.Username,
					SellerName = userByUsername.FullName,
					AssetNumber = asset.AssetNumber.ToString(),
					AssetAddressOneLine = string.Format("{0} {1}, {2}, {3} {4}", new object[] { asset.PropertyAddress, asset.PropertyAddress2, asset.City, asset.State, asset.Zip }),
					AssetDescription = assetByAssetId.Description,
					CorpOfficer = asset.CorporateOwnershipOfficer,
					VestingEntity = userById.FullName,
					APN = empty
				};
				ConfirmationPISellerAssetIsPublishedEmail str = confirmationPISellerAssetIsPublishedEmail;
				StringBuilder stringBuilder = new StringBuilder();
				asset.AssetTaxParcelNumbers.ForEach((AssetTaxParcelNumber a) => {
					stringBuilder.Append(a.TaxParcelNumber);
					stringBuilder.Append("; ");
				});
				str.APN = stringBuilder.ToString();
				this._email.Send(str);
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Asset successfully published.");
			return base.RedirectToAction("SellerManageAssets", "Investors");
		}

		public ViewResult SecureDebtEquityPartner()
		{
			return base.View();
		}

		[Authorize]
		public ActionResult SellerManageAssets(SellerAssetSearchResultsModel model, string sortOrder, int? page, string assetNumber = null)
		{
			int value;
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			if (assetNumber != null)
			{
				model.AssetNumber = assetNumber;
			}
			((dynamic)base.ViewBag).AssetType = this.populateAssetTypeDDL();
			((dynamic)base.ViewBag).PortfolioList = this.populatePortfolioListDDL();
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
				UserId = new int?(userByUsername.UserId),
				AssetName = model.AssetName,
				MaxSquareFeet = model.MaxSquareFeet,
				MinSquareFeet = model.MinSquareFeet,
				MaxUnitsSpaces = model.MaxUnitsSpaces,
				MinUnitsSpaces = model.MinUnitsSpaces
			};
			manageAssetsModel.UserId = new int?(userByUsername.UserId);
			manageAssetsModel.ControllingUserType = userByUsername.UserType;
			List<SellerAssetQuickListModel> sellerManageAssetsQuickList = this._asset.GetSellerManageAssetsQuickList(manageAssetsModel);
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
			((dynamic)base.ViewBag).HasSellerPrivilege = userByUsername.HasSellerPrivilege;
			string str = model.SortOrder;
			switch (str)
			{
				case "assetName_desc":
				{
					sellerManageAssetsQuickList = (
						from a in sellerManageAssetsQuickList
						orderby a.AssetName descending
						select a).ToList<SellerAssetQuickListModel>();
					break;
				}
				case "assetName":
				{
					sellerManageAssetsQuickList = (
						from a in sellerManageAssetsQuickList
						orderby a.AssetName
						select a).ToList<SellerAssetQuickListModel>();
					break;
				}
				case "createdBy_desc":
				{
					sellerManageAssetsQuickList = (
						from s in sellerManageAssetsQuickList
						orderby s.CreatedBy descending
						select s).ToList<SellerAssetQuickListModel>();
					break;
				}
				case "createdBy":
				{
					sellerManageAssetsQuickList = (
						from s in sellerManageAssetsQuickList
						orderby s.CreatedBy
						select s).ToList<SellerAssetQuickListModel>();
					break;
				}
				case "type_desc":
				{
					sellerManageAssetsQuickList = (
						from s in sellerManageAssetsQuickList
						orderby s.Type descending
						select s).ToList<SellerAssetQuickListModel>();
					break;
				}
				case "type":
				{
					sellerManageAssetsQuickList = (
						from s in sellerManageAssetsQuickList
						orderby s.Type
						select s).ToList<SellerAssetQuickListModel>();
					break;
				}
				case "show_desc":
				{
					sellerManageAssetsQuickList = (
						from s in sellerManageAssetsQuickList
						orderby s.Show descending
						select s).ToList<SellerAssetQuickListModel>();
					break;
				}
				case "show":
				{
					sellerManageAssetsQuickList = (
						from s in sellerManageAssetsQuickList
						orderby s.Show
						select s).ToList<SellerAssetQuickListModel>();
					break;
				}
				case "city_desc":
				{
					sellerManageAssetsQuickList = (
						from s in sellerManageAssetsQuickList
						orderby s.City descending
						select s).ToList<SellerAssetQuickListModel>();
					break;
				}
				case "city":
				{
					sellerManageAssetsQuickList = (
						from w in sellerManageAssetsQuickList
						orderby w.City
						select w).ToList<SellerAssetQuickListModel>();
					break;
				}
				case "state_desc":
				{
					sellerManageAssetsQuickList = (
						from s in sellerManageAssetsQuickList
						orderby s.State descending
						select s).ToList<SellerAssetQuickListModel>();
					break;
				}
				case "state":
				{
					sellerManageAssetsQuickList = (
						from w in sellerManageAssetsQuickList
						orderby w.State
						select w).ToList<SellerAssetQuickListModel>();
					break;
				}
				case "address_desc":
				{
					sellerManageAssetsQuickList = (
						from s in sellerManageAssetsQuickList
						orderby s.AddressLine1 descending
						select s).ToList<SellerAssetQuickListModel>();
					break;
				}
				case "address":
				{
					sellerManageAssetsQuickList = (
						from w in sellerManageAssetsQuickList
						orderby w.AddressLine1
						select w).ToList<SellerAssetQuickListModel>();
					break;
				}
				case "assetId_desc":
				{
					sellerManageAssetsQuickList = (
						from s in sellerManageAssetsQuickList
						orderby s.AssetNumber descending
						select s).ToList<SellerAssetQuickListModel>();
					break;
				}
				case "assetId":
				{
					sellerManageAssetsQuickList = (
						from w in sellerManageAssetsQuickList
						orderby w.AssetNumber
						select w).ToList<SellerAssetQuickListModel>();
					break;
				}
				case "status_desc":
				{
					sellerManageAssetsQuickList = (
						from s in sellerManageAssetsQuickList
						orderby s.Status descending
						select s).ToList<SellerAssetQuickListModel>();
					break;
				}
				case "status":
				{
					sellerManageAssetsQuickList = (
						from s in sellerManageAssetsQuickList
						orderby s.Status
						select s).ToList<SellerAssetQuickListModel>();
					break;
				}
				case "zip_desc":
				{
					sellerManageAssetsQuickList = (
						from s in sellerManageAssetsQuickList
						orderby s.Zip descending
						select s).ToList<SellerAssetQuickListModel>();
					break;
				}
				default:
				{
					sellerManageAssetsQuickList = (str == "zip" ? (
						from w in sellerManageAssetsQuickList
						orderby w.Zip
						select w).ToList<SellerAssetQuickListModel>() : (
						from s in sellerManageAssetsQuickList
						orderby s.AssetNumber
						select s).ToList<SellerAssetQuickListModel>());
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
			model.Assets = sellerManageAssetsQuickList.ToPagedList<SellerAssetQuickListModel>((rowCount.HasValue ? rowCount.GetValueOrDefault() : 1), num);
			return base.View(model);
		}

		public ViewResult SummaryReportOfIPAs()
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			return base.View(this._seller.GetSellerIPAsReceived(userByUsername.UserId));
		}

		public ViewResult SummaryReportOfLOIs()
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			return base.View(this._seller.GetSellerLOIsReceived(userByUsername.UserId));
		}

		public ActionResult UnpublishAsset(Guid id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			this._asset.UnPublishAsset(id);
			if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
			{
				AssetViewModel asset = this._asset.GetAsset(id, false);
				AssetDescriptionModel assetByAssetId = this._asset.GetAssetByAssetId(id);
				UserModel userById = this._user.GetUserById(asset.ListedByUserId.Value);
				ConfirmationPISellerAssetIsUnPublishedEmail confirmationPISellerAssetIsUnPublishedEmail = new ConfirmationPISellerAssetIsUnPublishedEmail()
				{
					DatePublished = DateTime.Now,
					To = userByUsername.Username,
					SellerName = userByUsername.FullName,
					AssetNumber = asset.AssetNumber.ToString(),
					AssetAddressOneLine = string.Format("{0} {1}, {2}, {3} {4}", new object[] { asset.PropertyAddress, asset.PropertyAddress2, asset.City, asset.State, asset.Zip }),
					AssetDescription = assetByAssetId.Description,
					CorpOfficer = asset.CorporateOwnershipOfficer,
					VestingEntity = userById.FullName
				};
				ConfirmationPISellerAssetIsUnPublishedEmail str = confirmationPISellerAssetIsUnPublishedEmail;
				StringBuilder stringBuilder = new StringBuilder();
				asset.AssetTaxParcelNumbers.ForEach((AssetTaxParcelNumber a) => {
					stringBuilder.Append(a.TaxParcelNumber);
					stringBuilder.Append("; ");
				});
				str.APN = stringBuilder.ToString();
				this._email.Send(str);
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Asset successfully unpublished.");
			return base.RedirectToAction("SellerManageAssets", "Investors");
		}
	}
}