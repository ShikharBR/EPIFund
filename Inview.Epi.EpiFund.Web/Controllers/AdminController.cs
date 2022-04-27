using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.Helpers;
using Inview.Epi.EpiFund.Domain.ViewModel;
using Inview.Epi.EpiFund.Web;
using Inview.Epi.EpiFund.Web.ActionFilters;
using Inview.Epi.EpiFund.Web.Models;
using Inview.Epi.EpiFund.Web.Models.Emails;
using Inview.Epi.EpiFund.Web.Providers;
using MVCVideo;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using AutoMapper;

namespace Inview.Epi.EpiFund.Web.Controllers
{
	[LayoutActionFilter]
	public class AdminController : BaseController
	{
		private IAssetManager _asset;

		private IUserManager _user;

		private IFileManager _fileManager;

		private IEPIFundEmailService _email;

		private IPortfolioManager _portfolio;

		private string _titleUserEmail;

		private IPDFService _pdf;

		private ProgressHelper progress = new ProgressHelper();

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

		public AdminController(IAssetManager asset, IUserManager user, IFileManager fileManager, IEPIFundEmailService email, IPDFService pdf, ISecurityManager securityManager, IPortfolioManager portfolio) : base(securityManager, email, user)
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
		public ViewResult AccessAsset()
		{
			LoginModel loginModel = new LoginModel();
			((dynamic)base.ViewBag).Email = this._user.GetTitleUserByEmail(this._user.GetUserByUsername(base.User.Identity.Name).Username).Email;
			return base.View(loginModel);
		}

		[Authorize]
		[HttpPost]
		public ActionResult AccessAsset(LoginModel model)
		{
			ActionResult action;
			TitleCompanyUserModel titleUserByEmail = this._user.GetTitleUserByEmail(this._user.GetUserByUsername(base.User.Identity.Name).Username);
			((dynamic)base.ViewBag).Email = titleUserByEmail.Email;
			try
			{
				int? assetNumber = model.AssetNumber;
				if (!assetNumber.HasValue)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Please enter the Asset Number.");
					action = base.View(model);
				}
				else
				{
					AssetDescriptionModel assetByAssetNumber = this._asset.GetAssetByAssetNumber(Convert.ToInt32(model.AssetNumber));
					AssetViewModel asset = this._asset.GetAsset(assetByAssetNumber.AssetId, false);
					OrderStatus orderStatus = asset.OrderStatus;
					assetNumber = asset.TitleCompanyId;
					int titleCompanyId = titleUserByEmail.TitleCompanyId;
					if ((assetNumber.GetValueOrDefault() == titleCompanyId ? !assetNumber.HasValue : true))
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "This asset is inaccessible to your title company");
						action = base.View(model);
					}
					else if (orderStatus == OrderStatus.Completed)
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Info, "This asset has been closed.");
						action = base.View(model);
					}
					else
					{
						action = base.RedirectToAction("CompleteAsset", "Admin", new { id = assetByAssetNumber.AssetId, fromManageAssets = false });
					}
				}
			}
			catch
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid Asset Number.");
				action = base.View(model);
			}
			return action;
		}

		[Authorize]
		[HttpGet]
		public ActionResult ActivateTitleCompany(int id)
		{
			this._user.ActivateTitleCompany(id);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Title Company successfully activated.");
			return base.RedirectToAction("ManageTitleCompanies");
		}

		[Authorize]
		[HttpGet]
		public ActionResult ActivateTitleUser(int id)
		{
			this._user.ActivateTitleUser(id);
			int titleCompanyId = this._user.GetTitleUserById(id).TitleCompanyId;
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Title Company User successfully activated.");
			return base.RedirectToAction("ManageTitleCompanyUsers", new { id = titleCompanyId });
		}

		[HttpGet]
		public ActionResult ActivateTitleUserbyManager(int id)
		{
			this._user.ActivateTitleUser(id);
			int titleCompanyId = this._user.GetTitleUserById(id).TitleCompanyId;
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Title Company User successfully activated.");
			return base.RedirectToAction("ManagerTitleUsersPage", new { id = titleCompanyId });
		}

		private string addCommas(string str)
		{
			str = str.Replace(",", "");
			string[] strArrays = (str ?? "").Split(new char[] { '.' });
			string str1 = strArrays[0];
			int length = str1.Length;
			string str2 = "";
			for (int i = length - 1; i >= 0; i--)
			{
				char chr = str1[i];
				str2 = string.Concat(chr.ToString(), str2);
				if ((length - i) % 3 == 0 && i > 0)
				{
					str2 = string.Concat(",", str2);
				}
			}
			if ((int)strArrays.Length > 1)
			{
				str2 = string.Concat(str2, ".", strArrays[1]);
			}
			return str2;
		}

		[Authorize]
		public ActionResult ApproveICAdmin(int id, string name, string email)
		{
			this._user.ApproveICAdmin(id);
			if (base.CanSendAutoEmail(BaseController.AutoEmailType.Admin))
			{
				this._email.Send(new ICAdminChangeStatusEmail()
				{
					ViewName = "ICAdminApproved",
					Name = name,
					To = email
				});
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "IC Admin approved.");
			return base.RedirectToAction("ManageICAdmins");
		}

		[Authorize]
		[HttpGet]
		public ViewResult AssetAssignment(int id)
		{
			TitleCompanyUserModel titleUserById = this._user.GetTitleUserById(id);
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			AssetAssignmentSearchModel assetAssignmentSearchModel = new AssetAssignmentSearchModel()
			{
				TitleCompanyManagerId = id,
				TitleCompanyId = titleUserById.TitleCompanyId
			};
			AssetAssignmentModel assetAssignmentModel = new AssetAssignmentModel()
			{
				TitleAssignments = this._user.GetAssetsAssigned(assetAssignmentSearchModel).ToPagedList<AssetAssignmentQuickViewModel>(1, 50),
				TitleCompanyId = titleUserById.TitleCompanyId,
				TitleCompanyManagerId = id
			};
			((dynamic)base.ViewBag).Email = userByUsername.Username;
			assetAssignmentModel.ControllingUserType = userByUsername.UserType;
			return base.View(assetAssignmentModel);
		}

		[Authorize]
		[HttpGet]
		public ViewResult AssetEscrowProcess(Guid id)
		{
			AssetViewModel asset = this._asset.GetAsset(id, false);
			return base.View(new AssetEscrowModel()
			{
				AssetId = id,
				AssetNumber = asset.AssetNumber,
				ProjectedClosingDate = asset.ClosingDate,
				ActualClosingDate = asset.ActualClosingDate,
				AssetDescription = asset.Description,
				ContractBuyer = asset.OwnerHoldingCompany,
				ContractBuyerAddress = asset.OwnerHoldingCompanyAddress,
				PrincipalContactOfContractBuyer = $"{asset.OwnerHoldingCompanyFirst} {asset.OwnerHoldingCompanyLast}",
				ListingPrice = asset.AskingPrice,
				ClosingPrice = new double?(asset.ClosingPrice)
			});
		}

		[Authorize]
		[HttpPost]
		public ActionResult AssetEscrowProcess(AssetEscrowModel model)
		{
			DateTime value;
			IEnumerable<UserQuickViewModel> users = 
				from w in this._user.GetUsers(new UserSearchModel()
				{
					ControllingUserType = UserType.SiteAdmin,
					ShowActiveOnly = true
				})
				where w.UserType != UserType.SiteAdmin
				select w;
			if (this._asset.IsAssetClosing(model.AssetId))
			{
				if (!model.ClosingPrice.HasValue || !model.ActualClosingDate.HasValue)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Both closing price and closing date are required to close escrow.");
				}
				else
				{
					this._asset.CloseEscrow(model.AssetId, model.ActualClosingDate.Value, model.ClosingPrice.Value);
					if (base.CanSendAutoEmail(BaseController.AutoEmailType.Asset))
					{
						foreach (UserQuickViewModel user in users)
						{
							try
							{
								if (!string.IsNullOrEmpty(user.Username))
								{
									IEPIFundEmailService ePIFundEmailService = this._email;
									ConfirmationClosingEscrowGeneralEmail confirmationClosingEscrowGeneralEmail = new ConfirmationClosingEscrowGeneralEmail()
									{
										AssetDescription = model.AssetDescription,
										AssetNumber = model.AssetNumber,
										ClosingPrice = model.ClosingPrice.Value,
										Name = user.FullName
									};
									if (model.ActualClosingDate.HasValue)
									{
										value = model.ActualClosingDate.Value;
									}
									else
									{
										value = (model.ProjectedClosingDate.HasValue ? model.ProjectedClosingDate.Value : DateTime.MinValue);
									}
									confirmationClosingEscrowGeneralEmail.ClosingDate = value;
									confirmationClosingEscrowGeneralEmail.Email = user.Username;
									ePIFundEmailService.Send(confirmationClosingEscrowGeneralEmail);
								}
							}
							catch
							{
							}
						}
					}
					base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Asset successfully recorded as closed escrow.");
				}
			}
			else if (!model.ProjectedClosingDate.HasValue)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Project closing date required to enter binding escrow.");
			}
			else
			{
				this._asset.EnterBindingEscrow(model.AssetId, model.ProjectedClosingDate.Value);
				if (base.CanSendAutoEmail(BaseController.AutoEmailType.Asset))
				{
					foreach (UserQuickViewModel userQuickViewModel in users)
					{
						try
						{
							this._email.Send(new ConfirmationBindingEscrowGeneralEmail()
							{
								AssetDescription = model.AssetDescription,
								AssetNumber = model.AssetNumber,
								ListingPrice = model.ListingPrice,
								ProjectedClosingDate = model.ProjectedClosingDate.Value,
								Name = string.Concat(userQuickViewModel.FirstName, " ", userQuickViewModel.LastName),
								Email = userQuickViewModel.Username
							});
						}
						catch
						{
						}
					}
				}
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Asset successfully recorded as entered binding escrow.");
			}
			return base.View(model);
		}

		[Authorize]
		[HttpGet]
		public PartialViewResult AssignmentList(int id)
		{
			TitleCompanyUserModel titleUserById = this._user.GetTitleUserById(id);
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			AssetAssignmentSearchModel assetAssignmentSearchModel = new AssetAssignmentSearchModel()
			{
				TitleCompanyManagerId = id,
				TitleCompanyId = titleUserById.TitleCompanyId
			};
			AssetAssignmentModel assetAssignmentModel = new AssetAssignmentModel()
			{
				TitleAssignments = this._user.GetAssetsAssigned(assetAssignmentSearchModel).ToPagedList<AssetAssignmentQuickViewModel>(1, 50),
				TitleCompanyId = titleUserById.TitleCompanyId,
				TitleCompanyManagerId = id
			};
			((dynamic)base.ViewBag).Email = userByUsername.Username;
			assetAssignmentModel.ControllingUserType = userByUsername.UserType;
			return this.PartialView("_AssignAssetsListView.cshtml", assetAssignmentModel.TitleAssignments);
		}

		private void BuildCommViewBags(Dictionary<string, IEnumerable<SelectListItem>> resultSet)
		{
			((dynamic)base.ViewBag).CommArchPropertyDetails = resultSet["architecture"];
			dynamic viewBag = base.ViewBag;
			IEnumerable<SelectListItem> item = resultSet["architecture"];
			viewBag.CommArchSel = (
				from x in item
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).CommArchCnt = resultSet["architecture"].Count<SelectListItem>();
			((dynamic)base.ViewBag).CommComplPropertyDetails = resultSet["complex"];
			dynamic obj = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems = resultSet["complex"];
			obj.CommComplSel = (
				from x in selectListItems
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).CommComplCnt = resultSet["complex"].Count<SelectListItem>();
			((dynamic)base.ViewBag).CommGenPropertyDetails = resultSet["general"];
			dynamic viewBag1 = base.ViewBag;
			IEnumerable<SelectListItem> item1 = resultSet["general"];
			viewBag1.CommGenSel = (
				from x in item1
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).CommGenCnt = resultSet["general"].Count<SelectListItem>();
			((dynamic)base.ViewBag).CommSecuPropertyDetails = resultSet["security"];
			dynamic obj1 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems1 = resultSet["security"];
			obj1.CommSecuSel = (
				from x in selectListItems1
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).CommSecuCnt = resultSet["security"].Count<SelectListItem>();
			((dynamic)base.ViewBag).CommParkingPropertyDetails = resultSet["parking"];
			dynamic viewBag2 = base.ViewBag;
			IEnumerable<SelectListItem> item2 = resultSet["parking"];
			viewBag2.CommParkSel = (
				from x in item2
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).CommParkCnt = resultSet["parking"].Count<SelectListItem>();
			((dynamic)base.ViewBag).CommConstPropertyDetails = resultSet["construction"];
			dynamic obj2 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems2 = resultSet["construction"];
			obj2.CommConstSel = (
				from x in selectListItems2
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).CommConstCnt = resultSet["construction"].Count<SelectListItem>();
			((dynamic)base.ViewBag).CommHVACPropertyDetails = resultSet["hvac"];
			dynamic viewBag3 = base.ViewBag;
			IEnumerable<SelectListItem> item3 = resultSet["hvac"];
			viewBag3.CommHVACSel = (
				from x in item3
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).CommHVACCnt = resultSet["hvac"].Count<SelectListItem>();
			((dynamic)base.ViewBag).CommPropPropertyDetails = resultSet["property"];
			dynamic obj3 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems3 = resultSet["property"];
			obj3.CommPropSel = (
				from x in selectListItems3
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).CommPropCnt = resultSet["property"].Count<SelectListItem>();
			((dynamic)base.ViewBag).CommRoofPropertyDetails = resultSet["roofing"];
			dynamic viewBag4 = base.ViewBag;
			IEnumerable<SelectListItem> item4 = resultSet["roofing"];
			viewBag4.CommRoofSel = (
				from x in item4
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).CommRoofCnt = resultSet["roofing"].Count<SelectListItem>();
			((dynamic)base.ViewBag).CommMFPropertyDetails = resultSet["mf"];
			dynamic obj4 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems4 = resultSet["mf"];
			obj4.CommMFlSel = (
				from x in selectListItems4
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).CommMFCnt = resultSet["mf"].Count<SelectListItem>();
			((dynamic)base.ViewBag).CommKitcPropertyDetails = resultSet["kitchen"];
			dynamic viewBag5 = base.ViewBag;
			IEnumerable<SelectListItem> item5 = resultSet["kitchen"];
			viewBag5.CommKitcSel = (
				from x in item5
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).CommKitcCnt = resultSet["kitchen"].Count<SelectListItem>();
			((dynamic)base.ViewBag).CommIntPropertyDetails = resultSet["interior"];
			dynamic obj5 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems5 = resultSet["interior"];
			obj5.CommIntSel = (
				from x in selectListItems5
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).CommIntCnt = resultSet["interior"].Count<SelectListItem>();
			((dynamic)base.ViewBag).CommExtPropertyDetails = resultSet["exterior"];
			dynamic viewBag6 = base.ViewBag;
			IEnumerable<SelectListItem> item6 = resultSet["exterior"];
			viewBag6.CommExtlSel = (
				from x in item6
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).CommExtCnt = resultSet["exterior"].Count<SelectListItem>();
			((dynamic)base.ViewBag).CommTechPropertyDetails = resultSet["tech"];
			dynamic obj6 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems6 = resultSet["tech"];
			obj6.CommTechSel = (
				from x in selectListItems6
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).CommTechCnt = resultSet["tech"].Count<SelectListItem>();
		}

		private Dictionary<string, IEnumerable<SelectListItem>> BuildDetailsDictionary(IEnumerable<SelectListItem> commPropertyDetails)
		{
			Dictionary<string, IEnumerable<SelectListItem>> strs = new Dictionary<string, IEnumerable<SelectListItem>>();
			IEnumerable<SelectListItem> list = (
				from x in commPropertyDetails
				where x.Value.ToLower().Contains("arch_")
				select x).ToList<SelectListItem>();
			strs.Add("architecture", list);
			IEnumerable<SelectListItem> selectListItems = (
				from x in commPropertyDetails
				where x.Value.ToLower().Contains("complex_")
				select x).ToList<SelectListItem>();
			strs.Add("complex", selectListItems);
			IEnumerable<SelectListItem> list1 = (
				from x in commPropertyDetails
				where x.Value.ToLower().Contains("genfeatures")
				select x).ToList<SelectListItem>();
			strs.Add("general", list1);
			IEnumerable<SelectListItem> selectListItems1 = (
				from x in commPropertyDetails
				where x.Value.ToLower().Contains("security_")
				select x).ToList<SelectListItem>();
			strs.Add("security", selectListItems1);
			IEnumerable<SelectListItem> list2 = (
				from x in commPropertyDetails
				where x.Value.ToLower().Contains("park_")
				select x).ToList<SelectListItem>();
			strs.Add("parking", list2);
			IEnumerable<SelectListItem> selectListItems2 = (
				from x in commPropertyDetails
				where x.Value.ToLower().Contains("const_")
				select x).ToList<SelectListItem>();
			strs.Add("construction", selectListItems2);
			IEnumerable<SelectListItem> list3 = commPropertyDetails.Where<SelectListItem>((SelectListItem x) => {
				if (x.Value.ToLower().Contains("cool_"))
				{
					return true;
				}
				return x.Value.ToLower().Contains("heat_");
			}).ToList<SelectListItem>();
			strs.Add("hvac", list3);
			IEnumerable<SelectListItem> selectListItems3 = commPropertyDetails.Where<SelectListItem>((SelectListItem x) => {
				if (x.Value.ToLower().Contains("unitext_"))
				{
					return true;
				}
				return x.Value.ToLower().Contains("unitint_");
			}).ToList<SelectListItem>();
			strs.Add("property", selectListItems3);
			IEnumerable<SelectListItem> list4 = (
				from x in commPropertyDetails
				where x.Value.ToLower().Contains("roof_")
				select x).ToList<SelectListItem>();
			strs.Add("roofing", list4);
			IEnumerable<SelectListItem> selectListItems4 = (
				from x in commPropertyDetails
				where x.Value.ToLower().Contains("intfeatures_")
				select x).ToList<SelectListItem>();
			strs.Add("mf", selectListItems4);
			IEnumerable<SelectListItem> list5 = (
				from x in commPropertyDetails
				where x.Value.ToLower().Contains("kitchamen")
				select x).ToList<SelectListItem>();
			strs.Add("kitchen", list5);
			IEnumerable<SelectListItem> selectListItems5 = (
				from x in commPropertyDetails
				where x.Value.ToLower().Contains("upgrades_")
				select x).ToList<SelectListItem>();
			strs.Add("interior", selectListItems5);
			IEnumerable<SelectListItem> list6 = (
				from x in commPropertyDetails
				where x.Value.ToLower().Contains("upgradesext_")
				select x).ToList<SelectListItem>();
			strs.Add("exterior", list6);
			IEnumerable<SelectListItem> selectListItems6 = (
				from x in commPropertyDetails
				where x.Value.ToLower().Contains("techfeat_")
				select x).ToList<SelectListItem>();
			strs.Add("tech", selectListItems6);
			IEnumerable<SelectListItem> list7 = (
				from x in commPropertyDetails
				where x.Value.ToLower().Contains("mfarch_")
				select x).ToList<SelectListItem>();
			strs.Add("mfArch", list7);
			IEnumerable<SelectListItem> selectListItems7 = (
				from x in commPropertyDetails
				where x.Value.ToLower().Contains("mhp_")
				select x).ToList<SelectListItem>();
			strs.Add("mhp", selectListItems7);
			return strs;
		}

		private void BuildMFViewBags(Dictionary<string, IEnumerable<SelectListItem>> resultSet)
		{
			((dynamic)base.ViewBag).MFArchPropertyDetails = resultSet["architecture"];
			dynamic viewBag = base.ViewBag;
			IEnumerable<SelectListItem> item = resultSet["architecture"];
			viewBag.MFArchSel = (
				from x in item
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MFArchCnt = resultSet["architecture"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MFComplPropertyDetails = resultSet["complex"];
			dynamic obj = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems = resultSet["complex"];
			obj.MFComplSel = (
				from x in selectListItems
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MFFComplCnt = resultSet["complex"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MFGenPropertyDetails = resultSet["general"];
			dynamic viewBag1 = base.ViewBag;
			IEnumerable<SelectListItem> item1 = resultSet["general"];
			viewBag1.MFGenSel = (
				from x in item1
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MFGenCnt = resultSet["general"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MFSecuPropertyDetails = resultSet["security"];
			dynamic obj1 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems1 = resultSet["security"];
			obj1.MFSecuSel = (
				from x in selectListItems1
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MFSecuCnt = resultSet["security"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MFParkingPropertyDetails = resultSet["parking"];
			dynamic viewBag2 = base.ViewBag;
			IEnumerable<SelectListItem> item2 = resultSet["parking"];
			viewBag2.MFParkSel = (
				from x in item2
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MFParkCnt = resultSet["parking"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MFConstPropertyDetails = resultSet["construction"];
			dynamic obj2 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems2 = resultSet["construction"];
			obj2.MFConstSel = (
				from x in selectListItems2
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MFConstCnt = resultSet["construction"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MFHVACPropertyDetails = resultSet["hvac"];
			dynamic viewBag3 = base.ViewBag;
			IEnumerable<SelectListItem> item3 = resultSet["hvac"];
			viewBag3.MFHVACSel = (
				from x in item3
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MFHVACCnt = resultSet["hvac"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MFPropPropertyDetails = resultSet["property"];
			dynamic obj3 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems3 = resultSet["property"];
			obj3.MFPropSel = (
				from x in selectListItems3
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MFPropCnt = resultSet["property"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MFRoofPropertyDetails = resultSet["roofing"];
			dynamic viewBag4 = base.ViewBag;
			IEnumerable<SelectListItem> item4 = resultSet["roofing"];
			viewBag4.MFRoofSel = (
				from x in item4
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MFRoofCnt = resultSet["roofing"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MFMFPropertyDetails = resultSet["mf"];
			dynamic obj4 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems4 = resultSet["mf"];
			obj4.MFMFlSel = (
				from x in selectListItems4
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MFMFCnt = resultSet["mf"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MFKitcPropertyDetails = resultSet["kitchen"];
			dynamic viewBag5 = base.ViewBag;
			IEnumerable<SelectListItem> item5 = resultSet["kitchen"];
			viewBag5.MFKitcSel = (
				from x in item5
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MFKitcCnt = resultSet["kitchen"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MFIntPropertyDetails = resultSet["interior"];
			dynamic obj5 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems5 = resultSet["interior"];
			obj5.MFIntSel = (
				from x in selectListItems5
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MFIntCnt = resultSet["interior"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MFExtPropertyDetails = resultSet["exterior"];
			dynamic viewBag6 = base.ViewBag;
			IEnumerable<SelectListItem> item6 = resultSet["exterior"];
			viewBag6.MFExtlSel = (
				from x in item6
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MFExtCnt = resultSet["exterior"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MFTechPropertyDetails = resultSet["tech"];
			dynamic obj6 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems6 = resultSet["tech"];
			obj6.MFTechSel = (
				from x in selectListItems6
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MFTechCnt = resultSet["tech"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MFArchPropertyDetails1 = resultSet["mfArch"];
			dynamic viewBag7 = base.ViewBag;
			IEnumerable<SelectListItem> item7 = resultSet["mfArch"];
			viewBag7.MFArchSel1 = (
				from x in item7
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MFArchCnt1 = resultSet["mfArch"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MFMHPPropertyDetails = resultSet["mhp"];
			dynamic obj7 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems7 = resultSet["mhp"];
			obj7.MFMHPSel = (
				from x in selectListItems7
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MFMHPCnt = resultSet["mhp"].Count<SelectListItem>();
		}

		[Authorize]
		public ActionResult CancelEditTitleCompany()
		{
			this._user.GetUserByUsername(base.User.Identity.Name);
			TitleSearchResultsModel titleSearchResultsModel = new TitleSearchResultsModel();
			CompanySearchModel companySearchModel = new CompanySearchModel()
			{
				CompanyName = titleSearchResultsModel.TitleCompName,
				CompanyURL = titleSearchResultsModel.TitleCompURL,
				State = titleSearchResultsModel.State
			};
			titleSearchResultsModel.Titles = this._user.GetTitleCompanies(companySearchModel).ToPagedList<TitleQuickViewModel>(1, 50);
			return base.View("ManageTitleCompanies", titleSearchResultsModel);
		}

		[Authorize]
		public ActionResult CancelEditTitleUser(int titleId)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			TitleUserSearchResultsModel titleUserSearchResultsModel = new TitleUserSearchResultsModel();
			TitleCompanyUserModel titleCompanyUserModel = new TitleCompanyUserModel();
			TitleUserSearchModel titleUserSearchModel = new TitleUserSearchModel()
			{
				Email = titleUserSearchResultsModel.Email,
				FirstName = titleUserSearchResultsModel.FirstName,
				IsActive = titleUserSearchResultsModel.IsActive,
				IsManager = titleUserSearchResultsModel.IsManager,
				LastName = titleUserSearchResultsModel.LastName,
				ManagingOfficerName = titleUserSearchResultsModel.ManagingOfficerName,
				Password = titleUserSearchResultsModel.Password,
				PhoneNumber = titleUserSearchResultsModel.PhoneNumber,
				TitleCompanyId = new int?(titleId)
			};
			titleUserSearchResultsModel.TitleUsers = this._user.GetTitleCompanyUsers(titleUserSearchModel).ToPagedList<TitleUserQuickViewModel>(1, 50);
			titleUserSearchResultsModel.ControllingUserType = userByUsername.UserType;
			((dynamic)base.ViewBag).Email = userByUsername.Username;
			titleUserSearchResultsModel.TitleCompanyId = titleId;
			return base.View("ManageTitleCompanyUsers", titleUserSearchResultsModel);
		}

		[Authorize]
		public ActionResult CancelEditTitleUserforManager()
		{
			TitleUserSearchResultsModel titleUserSearchResultsModel = new TitleUserSearchResultsModel();
			dynamic viewBag = base.ViewBag;
			List<TitleCompanyUser> titleCompanyUsers = this._user.GetTitleCompanyUsers();
			viewBag.Email = titleCompanyUsers.FirstOrDefault<TitleCompanyUser>((TitleCompanyUser x) => x.IsManager).Email;
			TitleCompanyUserModel titleUserByEmail = this._user.GetTitleUserByEmail(base.User.Identity.Name);
			TitleUserSearchModel titleUserSearchModel = new TitleUserSearchModel()
			{
				Email = titleUserSearchResultsModel.Email,
				FirstName = titleUserSearchResultsModel.FirstName,
				IsActive = titleUserSearchResultsModel.IsActive,
				IsManager = titleUserSearchResultsModel.IsManager,
				LastName = titleUserSearchResultsModel.LastName,
				ManagingOfficerName = titleUserSearchResultsModel.ManagingOfficerName,
				Password = titleUserSearchResultsModel.Password,
				PhoneNumber = titleUserSearchResultsModel.PhoneNumber,
				TitleCompanyId = new int?(titleUserByEmail.TitleCompanyId)
			};
			titleUserSearchResultsModel.TitleUsers = this._user.GetTitleCompanyUsers(titleUserSearchModel).ToPagedList<TitleUserQuickViewModel>(1, 50);
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			titleUserSearchResultsModel.ControllingUserType = userByUsername.UserType;
			((dynamic)base.ViewBag).Email = userByUsername.Username;
			titleUserSearchResultsModel.TitleCompanyId = titleUserByEmail.TitleCompanyId;
			return base.View("ManagerTitleUsersPage", titleUserSearchResultsModel);
		}

		[Authorize]
		public ActionResult CancelEditTitleUserforUser()
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			((dynamic)base.ViewBag).Email = this._user.GetTitleUserByEmail(userByUsername.Username).Email;
			MyUSCViewModel myUSCViewModel = new MyUSCViewModel()
			{
				FullName = userByUsername.FullName,
				UserId = userByUsername.UserId,
				UserType = userByUsername.UserType,
				JVMarketerAgreementLocation = userByUsername.JVMarketerAgreementLocation
			};
			TitleCompanyUserModel titleUserByEmail = this._user.GetTitleUserByEmail(userByUsername.Username);
			myUSCViewModel.TitleCompanyName = this._user.GetTitleById(titleUserByEmail.TitleCompanyId).TitleCompName;
			return base.View("MyUSCPageTitle", myUSCViewModel);
		}

		[Authorize]
		[HttpGet]
		public ActionResult ChangeListingStatus(int AssetNumber, bool fromManageAssets = false)
		{
			AssetDescriptionModel assetByAssetNumber = this._asset.GetAssetByAssetNumber(AssetNumber);
			return base.View(new ChangeListingStatusModel()
			{
				AssetId = assetByAssetNumber.AssetId,
				AssetNumber = assetByAssetNumber.AssetNumber,
				AssetDescription = assetByAssetNumber.Description,
				NewStatus = assetByAssetNumber.CurrentListingStatus,
				OldStatus = assetByAssetNumber.CurrentListingStatus
			});
		}

		[HttpPost]
		public ActionResult ChangeListingStatus(ChangeListingStatusModel model)
		{
			if (model.OldStatus == model.NewStatus)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "No change to status has been made. Select a different status from the old status to change listing status.");
			}
			else
			{
				this._asset.ChangeListingStatus(model.AssetId, model.NewStatus);
				this._asset.SendChangeListingStatusEmails(model);
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Listing status successfully changed.");
			}
			return base.View(model.AssetNumber);
		}

		[Authorize]
		[HttpGet]
		public ActionResult CompleteAsset(Guid id)
		{
			((dynamic)base.ViewBag).IsSampleAsset = false;
			List<VideoOptions> videoOptions = new List<VideoOptions>();
			if (!base.User.Identity.IsAuthenticated)
			{
				return base.RedirectToAction("Login", "Home", new { rurl = string.Concat("/DataPortal/ExecuteMDA/", id) });
			}
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			AssetViewModel asset = this._asset.GetAsset(id, false);
			this.SetUpAsset(asset, userByUsername);
			AssetDescriptionModel assetByAssetId = this._asset.GetAssetByAssetId(id);
			asset.HasCSAAgreement = assetByAssetId.HasCSAAgreement;
			asset.CurrentListingStatus = assetByAssetId.CurrentListingStatus;
			asset.Description = assetByAssetId.Description;
			asset.Show = assetByAssetId.Show;
			asset.AssetId = id;
			asset.TaxParcelNumber = string.Join(",", (
				from x in asset.AssetTaxParcelNumbers
				select x.TaxParcelNumber).ToArray<string>());
			((dynamic)base.ViewBag).Email = this._user.GetTitleUserByEmail(this._user.GetUserByUsername(base.User.Identity.Name).Username).Email;
			asset.User = this._user.GetUserEntity(base.User.Identity.Name);
			return base.View(asset);
		}

		[Authorize]
		[HttpPost]
		[MultipleButton(Name="action", Argument="Complete")]
		public ActionResult CompleteAsset(AssetViewModel model, string mainImg, string flyerImg)
		{
			AssetViewModel asset = this._asset.GetAsset(model.AssetId, false);
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			TitleCompanyUserModel titleUserByEmail = this._user.GetTitleUserByEmail(base.User.Identity.Name);
			((dynamic)base.ViewBag).IsCorpAdmin = this.isUserAdmin(userByUsername);
			base.ViewBag.IsICAdmin = (userByUsername.UserType == UserType.ICAdmin ? true : false);
			((dynamic)base.ViewBag).Email = titleUserByEmail.Email;
			IAssetManager assetManager = this._asset;
			Guid assetId = model.AssetId;
			int? titleCompanyId = model.TitleCompanyId;
			AssetDocumentOrderRequestModel assetDocumentOrderRequest = assetManager.GetAssetDocumentOrderRequest(assetId, titleCompanyId.Value, userByUsername.UserId, true);
			if (model.Documents == null)
			{
				model.Documents = new List<AssetDocument>();
			}
			int num = 0;
			for (int i = 0; i < model.Documents.Count; i++)
			{
				bool flag = false;
				model.Documents[i].AssetDocumentId = base.generatedGuidIfNone(model.Documents[i].AssetDocumentId);
				do
				{
					if (i >= model.Documents.Count)
					{
						break;
					}
					if (model.Documents[i].FileName != null)
					{
						model.Documents[i].Order = num;
						this._asset.MarkDocumentViewingStatus(model.Documents[i], true);
						num++;
						flag = false;
					}
					else
					{
						try
						{
							model.Documents.RemoveAt(i);
							flag = true;
						}
						catch
						{
						}
					}
				}
				while (flag);
			}
			bool flag1 = true;
			string empty = string.Empty;
			if (model.GetType() == typeof(MultiFamilyAssetViewModel))
			{
				MultiFamilyAssetViewModel multiFamilyAssetViewModel = asset as MultiFamilyAssetViewModel;
				empty = string.Format("{0} unit {1}", multiFamilyAssetViewModel.TotalUnits, EnumHelper.GetEnumDescription(model.AssetType));
				if (flag1)
				{
					if (!this._asset.UpdateAssetDocuments(model.Documents, model.AssetId))
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problem saving. Please contact site admin.");
						base.RedirectToAction("CompleteAsset", "admin", new { id = model.AssetId });
					}
					else
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully saved.");
						this._asset.UnlockAsset(model.AssetId);
					}
				}
			}
			if (model.GetType() == typeof(CommercialAssetViewModel))
			{
				CommercialAssetViewModel commercialAssetViewModel = asset as CommercialAssetViewModel;
				Dictionary<string, IEnumerable<SelectListItem>> strs = this.populateCommDetailsCheckBoxList((model as CommercialAssetViewModel).PropertyDetails, model.AssetType);
				this.BuildCommViewBags(strs);
				empty = string.Format("{0} square foot {1}", commercialAssetViewModel.LotSize, EnumHelper.GetEnumDescription(model.AssetType));
				if (flag1)
				{
					if (!this._asset.UpdateAssetDocuments(model.Documents, model.AssetId))
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problem saving. Please contact site admin.");
						base.RedirectToAction("CompleteAsset", "admin", new { id = model.AssetId });
					}
					else
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully saved.");
						this._asset.UnlockAsset(model.AssetId);
					}
				}
			}
			this._user.UpdateOrderStatus(model.AssetNumber, OrderStatus.Completed, userByUsername.UserId);
			StringBuilder stringBuilder = new StringBuilder();
			if (asset.AssetTaxParcelNumbers != null)
			{
				asset.AssetTaxParcelNumbers.ForEach((AssetTaxParcelNumber f) => {
					stringBuilder.Append(f.TaxParcelNumber);
					stringBuilder.Append("; ");
				});
			}
			TitleCompanyModel titleById = this._user.GetTitleById(titleUserByEmail.TitleCompanyId);
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
			if (base.CanSendAutoEmail(BaseController.AutoEmailType.Title))
			{
				if (!assetDocumentOrderRequest.IsSellerAsset)
				{
					try
					{
						this._email.Send(new PrelimConfirmationDeliverToBuyer()
						{
							Subject = "The Title Company Documents have been Uploaded",
							To = assetDocumentOrderRequest.SellerEmail,
							Date = DateTime.Now,
							TitleCompany = assetDocumentOrderRequest.CompanyName,
							AssetId = model.AssetNumber.ToString(),
							Address1 = assetDocumentOrderRequest.Address1,
							Address2 = assetDocumentOrderRequest.Address2,
							City = assetDocumentOrderRequest.City,
							State = assetDocumentOrderRequest.State,
							Zip = assetDocumentOrderRequest.Zip,
							AssetDescription = assetDocumentOrderRequest.AssetDescription,
							APN = assetDocumentOrderRequest.APN,
							Ownership = assetDocumentOrderRequest.Ownership,
							Registrant = assetDocumentOrderRequest.SellerFullName
						});
					}
					catch
					{
					}
					try
					{
						this._email.Send(new PrelimConfirmationUploadToTitleCoBuyer()
						{
							To = assetDocumentOrderRequest.Manager.Email,
							Subject = "Recorded Documents and Prelim Title Report Upload Confirmation",
							Date = DateTime.Now,
							SelectedTitleCompany = assetDocumentOrderRequest.CompanyName,
							TitleCompany = assetDocumentOrderRequest.CompanyName,
							AssetId = assetDocumentOrderRequest.AssetId,
							Address1 = assetDocumentOrderRequest.Address1,
							Address2 = assetDocumentOrderRequest.Address2,
							State = assetDocumentOrderRequest.State,
							Zip = assetDocumentOrderRequest.Zip,
							City = assetDocumentOrderRequest.City,
							AssetDescription = assetDocumentOrderRequest.AssetDescription,
							APN = assetDocumentOrderRequest.APN,
							Ownership = assetDocumentOrderRequest.Ownership,
							PIBuyer = assetDocumentOrderRequest.OrderedByName
						});
					}
					catch
					{
					}
					foreach (UserQuickViewModel user in users)
					{
						try
						{
							this._email.Send(new PrelimConfirmationDeliverToUSCBuyer()
							{
								Subject = "Recorded Documents and Prelim Title Report Upload Confirmation",
								To = user.Username,
								Date = DateTime.Now,
								TitleCompany = assetDocumentOrderRequest.CompanyName,
								AssetId = model.AssetNumber.ToString(),
								Address1 = assetDocumentOrderRequest.Address1,
								Address2 = assetDocumentOrderRequest.Address2,
								City = assetDocumentOrderRequest.City,
								State = assetDocumentOrderRequest.State,
								Zip = assetDocumentOrderRequest.Zip,
								AssetDescription = assetDocumentOrderRequest.AssetDescription,
								APN = assetDocumentOrderRequest.APN,
								Ownership = assetDocumentOrderRequest.Ownership,
								Registrant = assetDocumentOrderRequest.SellerFullName
							});
						}
						catch
						{
						}
					}
				}
				else
				{
					try
					{
						this._email.Send(new PrelimConfirmationDeliverToPISeller()
						{
							Subject = "The Title Company Documents have been Uploaded",
							To = assetDocumentOrderRequest.SellerEmail,
							Date = DateTime.Now,
							TitleCompany = assetDocumentOrderRequest.CompanyName,
							AssetId = model.AssetNumber.ToString(),
							Address1 = assetDocumentOrderRequest.Address1,
							Address2 = assetDocumentOrderRequest.Address2,
							City = assetDocumentOrderRequest.City,
							State = asset.State,
							Zip = assetDocumentOrderRequest.Zip,
							AssetDescription = assetDocumentOrderRequest.AssetDescription,
							APN = assetDocumentOrderRequest.APN,
							Ownership = assetDocumentOrderRequest.Ownership,
							Registrant = assetDocumentOrderRequest.SellerFullName
						});
					}
					catch
					{
					}
					try
					{
						this._email.Send(new PrelimConfirmationUploadToTitleCoPISeller()
						{
							To = assetDocumentOrderRequest.Manager.Email,
							Subject = "Recorded Documents & Prelim Title Report Upload Confirmation",
							Date = DateTime.Now,
							SelectedTitleCompany = assetDocumentOrderRequest.CompanyName,
							TitleCompany = assetDocumentOrderRequest.CompanyName,
							AssetId = assetDocumentOrderRequest.AssetId,
							Address1 = assetDocumentOrderRequest.Address1,
							Address2 = assetDocumentOrderRequest.Address2,
							State = assetDocumentOrderRequest.State,
							Zip = assetDocumentOrderRequest.Zip,
							City = assetDocumentOrderRequest.City,
							AssetDescription = assetDocumentOrderRequest.AssetDescription,
							APN = assetDocumentOrderRequest.APN,
							Ownership = assetDocumentOrderRequest.Ownership
						});
					}
					catch
					{
					}
					foreach (UserQuickViewModel userQuickViewModel in users)
					{
						try
						{
							this._email.Send(new PrelimConfirmationDeliverToUSCPISeller()
							{
								Subject = "Recorded Documents & Prelim Title Report Upload Confirmation",
								To = userQuickViewModel.Username,
								Date = DateTime.Now,
								TitleCompany = assetDocumentOrderRequest.CompanyName,
								AssetId = model.AssetNumber.ToString(),
								Address1 = assetDocumentOrderRequest.Address1,
								Address2 = assetDocumentOrderRequest.Address2,
								City = assetDocumentOrderRequest.City,
								State = assetDocumentOrderRequest.State,
								Zip = assetDocumentOrderRequest.Zip,
								AssetDescription = assetDocumentOrderRequest.AssetDescription,
								APN = assetDocumentOrderRequest.APN,
								Ownership = assetDocumentOrderRequest.Ownership
							});
						}
						catch
						{
						}
					}
				}
				foreach (AssetOrderHistoryViewModel history in assetDocumentOrderRequest.History)
				{
					UserModel userById = this._user.GetUserById(history.UserId);
					if (!history.IsSeller)
					{
						PrelimConfirmationDeliverToBuyer prelimConfirmationDeliverToBuyer = new PrelimConfirmationDeliverToBuyer()
						{
							Subject = "The Asset Documents have been uploaded",
							Date = DateTime.Now,
							Registrant = userById.FullName,
							TitleCompany = titleById.TitleCompName,
							AssetId = asset.AssetNumber.ToString(),
							Address1 = asset.PropertyAddress,
							City = asset.City,
							State = asset.State,
							Zip = asset.Zip,
							AssetDescription = empty,
							APN = assetDocumentOrderRequest.APN,
							Ownership = asset.Owner,
							To = userById.Username
						};
						try
						{
							this._email.Send(prelimConfirmationDeliverToBuyer);
						}
						catch
						{
						}
					}
					else
					{
						PrelimConfirmationDeliverToPISeller prelimConfirmationDeliverToPISeller = new PrelimConfirmationDeliverToPISeller()
						{
							Subject = "The Asset Documents have been uploaded",
							Date = DateTime.Now,
							Registrant = userById.FullName,
							TitleCompany = titleById.TitleCompName,
							AssetId = asset.AssetNumber.ToString(),
							Address1 = asset.PropertyAddress,
							City = asset.City,
							State = asset.State,
							Zip = asset.Zip,
							AssetDescription = empty,
							APN = assetDocumentOrderRequest.APN,
							Ownership = asset.Owner,
							To = userById.Username
						};
						try
						{
							this._email.Send(prelimConfirmationDeliverToPISeller);
						}
						catch
						{
						}
					}
				}
			}
			return base.RedirectToAction("AccessAsset", "Admin");
		}

		[Authorize]
		[HttpGet]
		public ActionResult Create(AssetType assetType, string portFolioName, bool isPaper, bool isPortfolio, bool isNewPF, string NewPfName, string taxParcelNumber = "", string state = "", string county = "")
		{
			int year;
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			if (userByUsername.UserType != UserType.CorpAdmin && userByUsername.UserType != UserType.CorpAdmin2 && userByUsername.UserType != UserType.ICAdmin && userByUsername.UserType != UserType.ListingAgent && userByUsername.UserType != UserType.Investor)
			{
				return base.RedirectToAction("Index", "Home");
			}
            if (userByUsername.UserType == UserType.ICAdmin)
            {
                if (!userByUsername.SignedICAgreement)
                {
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "We have not received your signed IC Agreement. Please check your email or execute another IC Agreement below.");
                    return base.RedirectToAction("ExecuteICAgreement", "DataPortal");
                }
                if (this._user.HasPendingICAgreement(userByUsername.UserId))
                {
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Your IC Agreement is still pending.");
                    return base.RedirectToAction("MyUSCPage", "Home");
                }
                ICStatus? cStatus = userByUsername.ICStatus;
                if ((cStatus.GetValueOrDefault() == ICStatus.Rejected ? cStatus.HasValue : false))
                {
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Your IC Agreement has been rejected.");
                    return base.RedirectToAction("MyUSCPage", "Home");
                }
                cStatus = userByUsername.ICStatus;
                if ((cStatus.GetValueOrDefault() == ICStatus.Approved ? !cStatus.HasValue : true))
                {
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Your IC Agreement is still pending approval.");
                    return base.RedirectToAction("MyUSCPage", "Home");
                }
                if (isNewPF && this._portfolio.PortfolioExist(NewPfName))
                {
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Portfolio with that name already Exists.");
                    return base.RedirectToAction("ICACache", "ICA");
                }
            }
            else
            {
                if (isNewPF && this._portfolio.PortfolioExist(NewPfName))
                {
                    base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Portfolio with that name already Exists.");
                    return base.RedirectToAction("ManageAssets", "Admin");
                }
            }
			AssetViewModel assetViewModel = new AssetViewModel();
            assetViewModel = this._asset.GenerateNewAsset(isPaper, assetType);
            assetViewModel.Countries = GetAllCountries();

            var allOperatingCompanies = _user.GetOperatingCompanies(new OperatingCompanySearchModel()).OrderBy(x => x.CompanyName).ToList();
            assetViewModel.OperatingCompanies.Add(new SelectListItem { Text = "---", Value = "" });
            assetViewModel.OperatingCompanies.Add(new SelectListItem { Text = "Unknown", Value = new Guid().ToString() });

            var unknown = allOperatingCompanies.FirstOrDefault(x => x.OperatingCompanyId == new Guid());
            // remove it from the list in order to exempt it from sorting
            if (unknown != null) allOperatingCompanies.Remove(unknown);
            foreach (var company in allOperatingCompanies)
            {
                assetViewModel.OperatingCompanies.Add(new SelectListItem
                {
                    Text = company.CompanyName,
                    Value = company.OperatingCompanyId.ToString()
                });
            }
            assetViewModel.AssetTaxParcelNumbers.Add(new AssetTaxParcelNumber()
			{
				TaxParcelNumber = taxParcelNumber
			});
            // it has to be one or the other -- either it's part of a new portfolio or part of an existing portfolio.
            // if we don't have portfolio names then we can't say it's part of a portfolio
            if ((!string.IsNullOrEmpty(portFolioName) && portFolioName != "0") || !string.IsNullOrEmpty(NewPfName))
            {
                assetViewModel.isPortfolio = new bool?(isPortfolio);
                if (portFolioName != "0" && !string.IsNullOrEmpty(portFolioName))
                {
                    assetViewModel.ExistingPFName = portFolioName;
                }
                else
                {
                    assetViewModel.isNewPf = new bool?(isNewPF);
                    assetViewModel.NewPFName = NewPfName;
                }
            }
            else
            {
                assetViewModel.isNewPf = false;
                assetViewModel.isPortfolio = false;
                assetViewModel.NewPFName = string.Empty;
                assetViewModel.ExistingPFName = string.Empty;
            }
            assetViewModel.State = state;
            assetViewModel.County = county;
			assetViewModel.User = this._user.GetUserEntity(base.User.Identity.Name);
			List<NarMemberViewModel> list = (
				from x in this._user.GetAssetNarMembers(new NarMemberSearchModel()
				{
					ShowActiveOnly = true
				})
				orderby x.LastName, x.FirstName
				select x).ToList<NarMemberViewModel>();
			assetViewModel.ListingAgents.Add(new SelectListItem()
			{
				Value = "0",
				Text = " ",
				Selected = true
			});
			foreach (NarMemberViewModel narMemberViewModel in list.OrderBy(l=>l.LastName.Trim()).ThenBy(l=>l.FirstName.Trim()))
			{
				assetViewModel.ListingAgents.Add(new SelectListItem()
				{
					Text = string.Concat(narMemberViewModel.LastName.Trim(), ", ", narMemberViewModel.FirstName.Trim()),
					Value = narMemberViewModel.NarMemberId.ToString()
				});
			}
			assetViewModel.ListingStatus = ListingStatus.Available;
			if (assetViewModel.GetType() == typeof(MultiFamilyAssetViewModel))
			{
				if (assetViewModel.AssetType != AssetType.MHP)
				{
					this.BuildMFViewBags(this.populateMFDetailsCheckBoxList((assetViewModel as MultiFamilyAssetViewModel).MFDetails));
				}
				else
				{
					this.BuildMFViewBags(this.populateMHPDetailsCheckBoxList((assetViewModel as MultiFamilyAssetViewModel).MHPDetails));
				}
			}
			if (assetViewModel.GetType() == typeof(CommercialAssetViewModel))
			{
				Dictionary<string, IEnumerable<SelectListItem>> strs = this.populateCommDetailsCheckBoxList((assetViewModel as CommercialAssetViewModel).PropertyDetails, assetViewModel.AssetType);
				this.BuildCommViewBags(strs);
			}
			DateTime now = DateTime.Now;
			DateTime dateTime = new DateTime(now.Year, 10, 1);
			AssetViewModel assetViewModel1 = assetViewModel;
			if (DateTime.Now <= dateTime)
			{
				now = DateTime.Now.AddYears(-1);
				year = now.Year;
			}
			else
			{
				year = DateTime.Now.Year;
			}
			assetViewModel1.PropertyTaxYear = year;
			base.ViewBag.IsSeller = (userByUsername.UserType == UserType.ListingAgent ? true : userByUsername.UserType == UserType.Investor);
			base.ViewBag.IsCorpAdmin = (this.isUserAdmin(userByUsername) || userByUsername.UserType == UserType.ListingAgent ? true : userByUsername.UserType == UserType.Investor);
			base.ViewBag.IsICAdmin = (userByUsername.UserType == UserType.ICAdmin ? true : false);
			if (!((dynamic)base.ViewBag).IsSeller)
			{
				((dynamic)base.ViewBag).Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
			}
			else
			{
				((dynamic)base.ViewBag).Layout = "~/Views/Shared/_Layout.cshtml";
			}
			assetViewModel.BuildingsCount = 1;
			assetViewModel.Users.Add(new SelectListItem()
			{
				Text = " ",
				Value = " "
			});
			assetViewModel.DeferredMaintenanceItems = this._asset.GetDefaultDeferredMaintenanceItems();
			assetViewModel.DeferredMaintenanceItems = this._asset.RemoveMaintainanceItems(assetViewModel.AssetType, assetViewModel.DeferredMaintenanceItems);
			assetViewModel.DeferredMaintenanceItems = this._asset.OrderMaintainanceItems(assetViewModel.AssetType, assetViewModel.DeferredMaintenanceItems);
			(
				from c in this._user.GetUsers(new UserSearchModel()
				{
					ShowActiveOnly = true
				})
				orderby c.CompanyName
				select c).ToList<UserQuickViewModel>().ForEach((UserQuickViewModel f) => assetViewModel.Users.Add(new SelectListItem()
			{
				Text = (!string.IsNullOrEmpty(f.CompanyName) ? f.CompanyName : string.Concat(f.FirstName, " ", f.LastName)),
				Value = f.UserId.ToString()
			}));
			assetViewModel.UserId = userByUsername.UserId.ToString();
			if (userByUsername.UserType == UserType.Investor)
			{
				assetViewModel.CorporateOwnershipAddress = userByUsername.AddressLine1;
				assetViewModel.CorporateOwnershipAddress2 = userByUsername.AddressLine2;
				assetViewModel.CorporateOwnershipCity = userByUsername.City;
				assetViewModel.CorporateOwnershipState = userByUsername.SelectedState;
				assetViewModel.CorporateOwnershipZip = userByUsername.Zip;
				assetViewModel.CorporateOwnershipOfficer = userByUsername.FullName;
			}
			assetViewModel.FromCreateMethod = true;

			List<AssetHCOwnershipModel> assetHCOwnershipModel = new List<AssetHCOwnershipModel>();
			List<AssetOCModel> assetOCModel = new List<AssetOCModel>();

			assetViewModel.AssetHCOwnershipLst = assetHCOwnershipModel;
			assetViewModel.AssetOCLst = assetOCModel;

			return base.View(assetViewModel);
		}

		[Authorize]
		[HttpPost]
		public ActionResult Create(AssetViewModel model)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			model.User = this._user.GetUserEntity(base.User.Identity.Name);
			if (!base.ValidateAdminUser(userByUsername))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.View("Index", "Home");
			}

            string invalid = ValidateAssetDocuments(model);
            if (invalid.Length > 0)
            {
                base.ModelState.AddModelError("", "The required documents are not present, please upload: " + invalid);
                return base.View("Create", model);
            }
            else if (model.Documents != null)
                model = _asset.PopulateDocumentOrder(model);

            model.ListedByUserId = userByUsername.UserId;
			model.IsSubmitted = false;
			((dynamic)base.ViewBag).IsCorpAdmin = this.isUserAdmin(userByUsername);
			base.ViewBag.IsICAdmin = (userByUsername.UserType == UserType.ICAdmin ? true : false);
            model.Countries = GetAllCountries();
            var allOperatingCompanies = _user.GetOperatingCompanies(new OperatingCompanySearchModel()).OrderBy(x => x.CompanyName).ToList();
            model.OperatingCompanies.Add(new SelectListItem { Text = "---", Value = "" });
            model.OperatingCompanies.Add(new SelectListItem { Text = "Unknown", Value = new Guid().ToString() });
            var unknown = allOperatingCompanies.FirstOrDefault(x => x.OperatingCompanyId == new Guid());
            // remove it from the list in order to exempt it from sorting
            if (unknown != null) allOperatingCompanies.Remove(unknown);
            foreach (var company in allOperatingCompanies)
            {
                model.OperatingCompanies.Add(new SelectListItem
                {
                    Text = company.CompanyName,
                    Value = company.OperatingCompanyId.ToString()
                });
            }
            foreach (NarMemberViewModel assetNarMember in this._user.GetAssetNarMembers(new NarMemberSearchModel()
			{
				ShowActiveOnly = true
			}).OrderBy(l=>l.LastName.Trim()).ThenBy(l=>l.FirstName.Trim()))
			{
				model.ListingAgents.Add(new SelectListItem()
				{
					Text = assetNarMember.LastName.Trim() + ", " + assetNarMember.FirstName.Trim(),
					Value = assetNarMember.NarMemberId.ToString()
				});
			}
			for (int i = 0; i < model.AssetNARMembers.Count; i++)
			{
				if (model.AssetNARMembers[i].NARMember.NotOnList)
				{
					if (string.IsNullOrEmpty(model.AssetNARMembers[i].NARMember.Email))
					{
						base.ModelState.AddModelError(string.Concat("AssetNARMembers[", i, "].NARMember.Email"), "Listing Agent Email Required");
					}
					if (string.IsNullOrEmpty(model.AssetNARMembers[i].NARMember.FirstName))
					{
						base.ModelState.AddModelError(string.Concat("AssetNARMembers[", i, "].NARMember.FirstName"), "Listing Agent First Name Required");
					}
					if (string.IsNullOrEmpty(model.AssetNARMembers[i].NARMember.LastName))
					{
						base.ModelState.AddModelError(string.Concat("AssetNARMembers[", i, "].NARMember.LastName"), "Listing Agent Last Name Required");
					}
				}
			}
			if (model.Images == null)
			{
				model.Images = new List<AssetImage>();
			}
			if (model.Documents == null)
			{
				model.Documents = new List<AssetDocument>();
			}
			if (model.Videos == null)
			{
				model.Videos = new List<AssetVideo>();
			}
			for (int j = 0; j < model.Videos.Count; j++)
			{
				bool flag = false;
				do
				{
					if (model.Videos[j].FilePath != null)
					{
						continue;
					}
					model.Videos.RemoveAt(j);
					flag = true;
				}
				while (flag);
			}
			if (!model.Images.Any<AssetImage>((AssetImage s) => s.IsMainImage) && model.Images.Count > 0)
			{
				model.Images[0].IsMainImage = true;
				model.Images[0].IsFlyerImage = true;
			}
			int num = 0;
			List<AssetImage> list = (
				from s in model.Images
				orderby s.Order
				select s).ToList<AssetImage>();
			for (int k = 0; k < list.Count; k++)
			{
				bool flag1 = false;
				do
				{
					if (k >= list.Count)
					{
						break;
					}
					if (list[k].FileName != null)
					{
						list[k].AssetImageId = base.generatedGuidIfNone(list[k].AssetImageId);
						list[k].Order = num;
						num++;
						flag1 = false;
					}
					else
					{
						try
						{
							list.RemoveAt(k);
							flag1 = true;
						}
						catch
						{
						}
					}
				}
				while (flag1);
			}
			model.Images = list;
			num = 0;
			for (int l = 0; l < model.Documents.Count; l++)
			{
				bool flag2 = false;
				do
				{
					if (model.Documents[l].FileName != null)
					{
						model.Documents[l].AssetDocumentId = base.generatedGuidIfNone(model.Documents[l].AssetDocumentId);
						model.Documents[l].Order = num;
						num++;
						flag2 = false;
					}
					else
					{
						model.Documents.RemoveAt(l);
						flag2 = true;
					}
				}
				while (flag2);
			}
			if (model.TypeOfPositionMortgage != "None")
			{
				if (!model.MortgageLienType.HasValue)
				{
					base.ModelState.AddModelError("MortgageLienType", "Mortgage Lien Type required.");
				}
				else if (!model.MortgageLienAssumable.HasValue)
				{
					base.ModelState.AddModelError("MortgageLienAssumable", "Mortgage Lien Assumable required.");
				}
			}
			if (model.AssetNumber == 0)
			{
				model.CreationDate = new DateTime?(DateTime.Now);
				model.IsActive = true;
				model.ListedByUserId = userByUsername.UserId;
			}
            // no need to set a flag in this method, SetAssetOwnership adds errors to the ModelState
            SetAssetOwnership(model);
            string str = "";
			int? nullable = null;
			if (model.GetType() == typeof(MultiFamilyAssetViewModel))
			{
				MultiFamilyAssetViewModel multiFamilyAssetViewModel = model as MultiFamilyAssetViewModel;
				((dynamic)base.ViewBag).DefMaintenanceDetails = this.populateMFDefMaintCheckBoxList((model as MultiFamilyAssetViewModel).EstDefMaintenanceDetails);
				((dynamic)base.ViewBag).MaintenanceCosts = this.setupMaintenanceCosts();
				for (int m = 0; m < multiFamilyAssetViewModel.UnitSpecifications.Count; m++)
				{
					multiFamilyAssetViewModel.UnitSpecifications[m].AssetUnitSpecificationId = base.generatedGuidIfNone(multiFamilyAssetViewModel.UnitSpecifications[m].AssetUnitSpecificationId);
				}
				for (int n = 0; n < multiFamilyAssetViewModel.MHPUnitSpecifications.Count; n++)
				{
					multiFamilyAssetViewModel.MHPUnitSpecifications[n].AssetMHPSpecificationId = base.generatedGuidIfNone(multiFamilyAssetViewModel.MHPUnitSpecifications[n].AssetMHPSpecificationId);
				}
				if (model.AssetType != AssetType.MHP)
				{
					this.BuildMFViewBags(this.populateMFDetailsCheckBoxList((model as MultiFamilyAssetViewModel).MFDetails));
				}
				else
				{
					this.BuildMFViewBags(this.populateMHPDetailsCheckBoxList((model as MultiFamilyAssetViewModel).MHPDetails));
				}
				if (!base.ModelState.IsValid)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problems with data input fields. Please review below");
					return base.View(multiFamilyAssetViewModel);
				}
				str = string.Format("A {0} unit {1} property in {2}, {3}", new object[] { multiFamilyAssetViewModel.TotalUnits, EnumHelper.GetEnumDescription(model.AssetType), model.City, model.State });
				nullable = this._asset.CreateAssetByViewModel(multiFamilyAssetViewModel);
				if (!nullable.HasValue)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problem saving. Please try again or contact site admin.");
					return base.View(multiFamilyAssetViewModel);
				}
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully saved.");
			}
			if (model.GetType() == typeof(CommercialAssetViewModel))
			{
				CommercialAssetViewModel commercialAssetViewModel = model as CommercialAssetViewModel;
				Dictionary<string, IEnumerable<SelectListItem>> strs = this.populateCommDetailsCheckBoxList((model as CommercialAssetViewModel).PropertyDetails, model.AssetType);
				this.BuildCommViewBags(strs);
				if (!base.ModelState.IsValid)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problems with data input fields. Please review below");
					return base.View(commercialAssetViewModel);
				}
				str = string.Format("A {0} sq. ft. {1} property in {2}, {3}", new object[] { commercialAssetViewModel.LotSize, EnumHelper.GetEnumDescription(model.AssetType), model.City, model.State });
				nullable = this._asset.CreateAssetByViewModel(commercialAssetViewModel);
				if (!nullable.HasValue)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problem saving. Please try again or contact site admin.");
					return base.View(commercialAssetViewModel);
				}
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully saved.");
			}
			try
			{
				this.SendICAdminEmails(userByUsername, model, nullable, str);
			}
			catch
			{
			}
            if(userByUsername.UserType == UserType.ICAdmin)
            {
                return base.RedirectToAction("ICACache", "ICA");
            }
			else if (userByUsername.UserType != UserType.CREBroker && userByUsername.UserType != UserType.CRELender && userByUsername.UserType != UserType.Investor)
			{
				return base.RedirectToAction("ManageAssets", "Admin");
			}
			return base.RedirectToAction("SellerManageAssets", "Investors");
		}

		[Authorize]
		[HttpGet]
		public ViewResult CreateMba()
		{
			return base.View(new MbaViewModel()
			{
				IsActive = true
			});
		}

		[HttpPost]
		public ActionResult CreateMba(MbaViewModel model)
		{
			try
			{
				this._user.CreateMba(model);
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "MBA successfully updated.");
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, exception.Message);
			}
			return base.View(model);
		}

		[Authorize]
		[HttpGet]
		public ViewResult CreateNARMember()
		{
			return base.View(new NarMemberViewModel()
			{
				IsActive = true
			});
		}

		[HttpPost]
		public ActionResult CreateNARMember(NarMemberViewModel model)
		{
			try
			{
				if (model.FirstName == null || model.FirstName.Length <= 0 || model.LastName == null || model.LastName.Length <= 0 || model.Email == null || model.Email.Length <= 0 || model.CompanyName == null || model.CompanyName.Length <= 0)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Please fill all the required fields.");
				}
				else if (!base.isEmail(model.Email))
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Please enter a valid email.");
				}
				else if (this._user.VerifyNarMember(model))
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "NAR member already exist in the database.");
				}
				else
				{
					this._user.CreateNarMember(model);
					base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "NAR Member successfully updated.");
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, exception.Message);
			}
			return base.View(model);
		}

		[Authorize]
		[HttpGet]
		public ViewResult CreatePrincipalInvestor()
		{
			return base.View(new PrincipalInvestorQuickViewModel()
			{
				IsActive = true
			});
		}

		[HttpPost]
		public ActionResult CreatePrincipalInvestor(PrincipalInvestorQuickViewModel model)
		{
			try
			{
				if (model.FirstName == null || model.FirstName.Length <= 0 || model.LastName == null || model.LastName.Length <= 0 || model.Email == null || model.Email.Length <= 0 || model.CompanyName == null || model.CompanyName.Length <= 0)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Please fill all the required fields.");
				}
				else if (!base.isEmail(model.Email))
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Please enter a valid email.");
				}
				else
				{
					this._user.CreatePrincipalInvestor(model);
					base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Principal Investor successfully updated.");
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, exception.Message);
			}
			return base.View(model);
		}

		[Authorize]
		[HttpGet]
		public ActionResult CreateRegisteredNarMember()
		{
			RegistrationModel registrationModel = new RegistrationModel()
			{
				HideRegistrationCategoryForListingAgent = true,
				TermsOfUse = true
			};
			return base.View(registrationModel);
		}

		[Authorize]
		[HttpPost]
		public ActionResult CreateRegisteredNarMember(RegistrationModel model)
		{
			try
			{
				if (base.ModelState.IsValid)
				{
					if (this._user.UserExists(model.Username))
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "A user with this username already exists in the system.");
					}
					else
					{
						model.SelectedUserType = UserType.ListingAgent;
						this._user.CreateUser(model);
						try
						{
							if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
							{
								this._email.Send(new UserWelcomeEmail()
								{
									Password = model.Password,
									RegistrationType = EnumHelper.GetEnumDescription(model.SelectedUserType),
									To = model.Username
								});
							}
						}
						catch
						{
						}
						base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "User successfully created.");
					}
				}
			}
			catch
			{
			}
			return base.View(model);
		}

		[Authorize]
		[HttpGet]
		public ViewResult CreateTitleCompany()
		{
			if (base.ValidateAdminUser(this._user.GetUserByUsername(base.User.Identity.Name)))
			{
				return base.View(new TitleCompanyModel());
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
			return base.View("Index", "Home");
		}

		[Authorize]
		[HttpPost]
		public ActionResult CreateTitleCompany(TitleCompanyModel model)
		{
			try
			{
				if (base.ModelState.IsValid)
				{
					if (this._user.TitleCompanyExists(model.TitleCompName))
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "A Title Company with this name already exists in the system.");
					}
					else
					{
						this._user.CreateTitle(model);
						base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Title Company successfully created.");
					}
				}
			}
			catch
			{
			}
			return base.View(model);
		}

		[Authorize]
		[HttpGet]
		public ActionResult CreateTitleUser(int id)
		{
			this._user.GetUserByUsername(base.User.Identity.Name);
			TitleCompanyUserModel titleCompanyUserModel = new TitleCompanyUserModel(this._user.GetTitleById(id))
			{
				TitleCompanyId = id
			};
			return base.View(titleCompanyUserModel);
		}

		[HttpPost]
		public ActionResult CreateTitleUser(TitleCompanyUserModel model)
		{
			int num = 0;
			try
			{
				if (string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.ConfirmPassword))
				{
					base.ModelState.AddModelError("Password", "Password is Required");
				}
				if (string.IsNullOrEmpty(model.ConfirmPassword) && !string.IsNullOrEmpty(model.Password))
				{
					base.ModelState.AddModelError("ConfirmPassword", "Confirm Password is Required");
				}
				if (base.ModelState.IsValid)
				{
					Tuple<bool, string> tuple = this._user.ValidateTitleUserStateAvailability(model);
					if (!tuple.Item1)
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, tuple.Item2);
					}
					else if (this._user.TitleCompanyUserExists(model.Email) || this._user.UserExists(model.Email))
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "A User with this email already exists in the system.");
					}
					else
					{
						if (model.IsManager)
						{
							model.SelectedStates = null;
						}
						num = this._user.CreateTitleUser(model);
						string host = base.Request.Url.Host;
						string str = base.Request.Url.Port.ToString();
						string empty = string.Empty;
						empty = (str != null ? string.Concat(new string[] { "http://", host, ":", str, "/Admin/ValidateManager/id=", num.ToString() }) : string.Concat("http://", host, "/Admin/ValidateManager/id=", num.ToString()));
						try
						{
							if (base.CanSendAutoEmail(BaseController.AutoEmailType.Asset))
							{
								if (!model.IsActive)
								{
									this._email.Send(new CompanyUserWelcomeEmail()
									{
										Email = model.Email,
										TempPassword = model.Password,
										To = model.FullName,
										RegistrationType = (model.IsManager ? "Title Company Manager" : "Title Company User"),
										URL = empty
									});
								}
								else
								{
									this._email.Send(new CompanyUserWelcomeEmailAutoActivatedEmail()
									{
										ActivatedByCorpAdmin = base.User.Identity.Name,
										Email = model.Email,
										TempPassword = model.Password,
										To = model.FullName,
										RegistrationType = (model.IsManager ? "Title Company Manager" : "Title Company User"),
										URL = empty
									});
								}
							}
						}
						catch
						{
						}
						base.TempData["message"] = new MessageViewModel(MessageTypes.Success, (model.IsManager ? "Title Company Manager successfully created." : "Title Company User successfully created."));
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, string.Concat("Title Company User Creation Error: ", exception.Message));
			}
			if (num == 0)
			{
				return base.View(model);
			}
			return base.RedirectToAction("EditTitleCompanyUser", new { id = num });
		}

		[Authorize]
		[HttpGet]
		public ActionResult CreateTitleUserbyManager(int id)
		{
			int titleCompanyId = id;
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			if (titleCompanyId == 0)
			{
				titleCompanyId = this._user.GetTitleUserByEmail(base.User.Identity.Name).TitleCompanyId;
			}
			TitleCompanyUserModel titleCompanyUserModel = new TitleCompanyUserModel(this._user.GetTitleById(titleCompanyId))
			{
				TitleCompanyId = titleCompanyId,
				ControllingUserType = userByUsername.UserType
			};
			((dynamic)base.ViewBag).Email = this._user.GetTitleCompanyUsers().FirstOrDefault<TitleCompanyUser>((TitleCompanyUser x) => {
				if (!x.IsManager)
				{
					return false;
				}
				return x.TitleCompanyId == titleCompanyId;
			}).Email;
			return base.View(titleCompanyUserModel);
		}

		[Authorize]
		[HttpPost]
		public ActionResult CreateTitleUserbyManager(TitleCompanyUserModel model)
		{
			try
			{
				if (string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.ConfirmPassword))
				{
					base.ModelState.AddModelError("Password", "Password is Required");
				}
				if (string.IsNullOrEmpty(model.ConfirmPassword) && !string.IsNullOrEmpty(model.Password))
				{
					base.ModelState.AddModelError("ConfirmPassword", "Confirm Password is Required");
				}
				if (base.ModelState.IsValid)
				{
					Tuple<bool, string> tuple = this._user.ValidateTitleUserStateAvailability(model);
					if (!tuple.Item1)
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, tuple.Item2);
					}
					else if (this._user.TitleCompanyUserExists(model.Email) || this._user.UserExists(model.Email))
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "A User with this email already exists in the system.");
					}
					else
					{
						model.IsManager = false;
						dynamic viewBag = base.ViewBag;
						List<TitleCompanyUser> titleCompanyUsers = this._user.GetTitleCompanyUsers();
						viewBag.Email = titleCompanyUsers.FirstOrDefault<TitleCompanyUser>((TitleCompanyUser x) => x.IsManager).Email;
						int num = this._user.CreateTitleUser(model);
						string host = base.Request.Url.Host;
						string str = base.Request.Url.Port.ToString();
						string empty = string.Empty;
						empty = (str != null ? string.Concat(new string[] { "http://", host, ":", str, "/Admin/ValidateUser/id=", num.ToString() }) : string.Concat("http://", host, "/Admin/ValidateUser/id=", num.ToString()));
						if (base.CanSendAutoEmail(BaseController.AutoEmailType.Title))
						{
							this._email.Send(new CompanyUserWelcomeEmail()
							{
								Email = model.Email,
								TempPassword = model.Password,
								To = model.FullName,
								RegistrationType = "Title Company User",
								URL = empty
							});
						}
						base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Title Company User successfully created.");
					}
				}
			}
			catch
			{
			}
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			model.ControllingUserType = userByUsername.UserType;
			((dynamic)base.ViewBag).Email = userByUsername.Username;
			return base.View(model);
		}

		[Authorize]
		[HttpGet]
		public ViewResult CreateUser()
		{
			RegistrationModel registrationModel = new RegistrationModel();
			registrationModel.UserTypes.Add(new SelectListItem()
			{
				Text = EnumHelper.GetEnumDescription(UserType.CorpAdmin2),
				Value = UserType.CorpAdmin2.ToString()
			});
			registrationModel.UserTypes.Add(new SelectListItem()
			{
				Text = EnumHelper.GetEnumDescription(UserType.CorpAdmin),
				Value = UserType.CorpAdmin.ToString()
			});
			registrationModel.UserTypes.Add(new SelectListItem()
			{
				Text = EnumHelper.GetEnumDescription(UserType.SiteAdmin),
				Value = UserType.SiteAdmin.ToString()
			});
			return base.View(registrationModel);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult CreateUser(RegistrationModel model)
		{
			try
			{
				if (base.ModelState.IsValid)
				{
					if (this._user.UserExists(model.Username))
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "A user with this username already exists in the system.");
					}
					else
					{
						this._user.CreateUser(model);
						try
						{
							if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
							{
								this._email.Send(new UserWelcomeEmail()
								{
									Password = model.Password,
									RegistrationType = EnumHelper.GetEnumDescription(model.SelectedUserType),
									To = model.Username
								});
							}
						}
						catch
						{
						}
						base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "User successfully created.");
					}
				}
			}
			catch
			{
			}
			return base.View(model);
		}

		[Authorize]
		[HttpGet]
		public ViewResult CreateUserNote(int id)
		{
			return base.View(new UserNoteModel()
			{
				UserId = id,
				Date = DateTime.Now
			});
		}

		[HttpPost]
		public ActionResult CreateUserNote(UserNoteModel model)
		{
			try
			{
				this._asset.CreateUserNote(model);
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "User note created.");
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, string.Concat("There was an error creating user note. Error: ", exception.Message));
			}
			return base.RedirectToAction("EditUser", "Admin", new { id = model.UserId });
		}

		public ActionResult DeactivateMba(int id)
		{
			this._user.DeactiveMba(id);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "MBA successfully deactivated.");
			return base.RedirectToAction("ManageMBAMembersImported");
		}

		[Authorize]
		public ActionResult DeactivateNARMember(int id)
		{
			this._user.DeactiveNarMember(id);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "NAR Member successfully deactivated.");
			return base.RedirectToAction("ManageNarMembersImported");
		}

		public ActionResult DeactivatePrincipalInvestor(int id)
		{
			this._user.DeactivePrincipalInvestor(id);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Principal Investor successfully deactivated.");
			return base.RedirectToAction("ManagePrincipalInvestorsImport");
		}

        public ActionResult DeactivateOperatingCompany(string id)
        {
            this._user.DeactivateOperatingCompany(Guid.Parse(id));
            base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Operating Company successfully deactivated.");
            return base.RedirectToAction("ManageOperatingCompanies");
        }

        public ActionResult DeactivateHoldingCompany(string id, string ocId)
        {
            this._user.DeactivateHoldingCompany(Guid.Parse(id));
            base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Holding Company successfully deactivated.");
            return base.RedirectToAction("UpdateOperatingCompany", new { id = ocId });
        }

        [Authorize]
		[HttpGet]
		public ActionResult DeactivateTitleCompany(int id)
		{
			this._user.DeactivateTitleCompany(id);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Title Company successfully deactivated.");
			return base.RedirectToAction("ManageTitleCompanies");
		}

		[Authorize]
		[HttpGet]
		public ActionResult DeactivateTitleUser(int id)
		{
			this._user.DeactivateTitleUser(id);
			int titleCompanyId = this._user.GetTitleUserById(id).TitleCompanyId;
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Title Company User successfully deactivated.");
			return base.RedirectToAction("ManageTitleCompanyUsers", new { id = titleCompanyId });
		}

		[HttpGet]
		public ActionResult DeactivateTitleUserbyManager(int id)
		{
			this._user.DeactivateTitleUser(id);
			int titleCompanyId = this._user.GetTitleUserById(id).TitleCompanyId;
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Title Company User successfully deactivated.");
			return base.RedirectToAction("ManagerTitleUsersPage", new { id = titleCompanyId });
		}

		[Authorize]
		[HttpGet]
		public ActionResult DeactivateUser(int id, string method)
		{
			this._user.DeleteUser(id);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "User successfully deactivated.");
			if (method == null)
			{
				return base.RedirectToAction("ManagePrincipalInvestorsReg");
			}
			return base.RedirectToAction(null);
		}

		[Authorize]
		[HttpGet]
		public ActionResult DeactivateUserMBA(int id)
		{
			this._user.DeleteUser(id);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "User successfully deactivated.");
			return base.RedirectToAction("ManageMBAMembersReg");
		}

		[HttpDelete]
		public void Delete(string fileName)
		{
		}

		[Authorize]
		public ActionResult DeleteAsset(Guid id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			this._asset.DeleteAsset(id);
			this._asset.UnlockAsset(id);
			if (userByUsername.UserType == UserType.CorpAdmin || userByUsername.UserType == UserType.ICAdmin)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Asset successfully withdrawn.");
			}
			else
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Asset successfully deactivated.");
			}
			if (userByUsername.UserType != UserType.CREBroker && userByUsername.UserType != UserType.CRELender && userByUsername.UserType != UserType.Investor)
			{
				return base.RedirectToAction("ManageAssets", "Admin");
			}
			return base.RedirectToAction("SellerManageAssets", "Investors");
		}

		[Authorize]
		[HttpGet]
		public ActionResult DeleteContractPayment(int? id, int? userId, string method)
		{
			if (!base.ValidateAdminUser(this._user.GetUserByUsername(base.User.Identity.Name)))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			if (id.GetValueOrDefault(0) <= 0)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid payment ID.");
				return base.RedirectToAction(method, new { id = userId });
			}
			this._user.DeleteContractPayment(id.Value);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully deleted payment.");
			return base.RedirectToAction(method, new { id = userId });
		}

		[Authorize]
		[HttpPost]
		public JsonResult DeleteNarMemberLA(Guid? id)
		{
			if (!id.HasValue || !(id.Value != Guid.Empty))
			{
				return new JsonResult()
				{
					Data = new { Status = "error" }
				};
			}
			this._user.DeleteAssetListingAgent(id.Value);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Listing Agent removed from asset.");
			return new JsonResult()
			{
				Data = new { Status = "success" }
			};
		}

		[Authorize]
		[HttpGet]
		public ActionResult DeleteTitleOrderPayment(int? id, int? tid)
		{
			if (!base.ValidateAdminUser(this._user.GetUserByUsername(base.User.Identity.Name)))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			if (id.GetValueOrDefault(0) > 0 && tid.GetValueOrDefault(0) > 0)
			{
				this._asset.DeleteTitleOrderPayment(id.Value);
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Order deleted.");
				return base.RedirectToAction("RecordTitleCoOrderPayment", new { TitleCoId = tid });
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid payment identification number.");
			if (tid.GetValueOrDefault(0) <= 0)
			{
				return base.RedirectToAction("ManageTitleCompanies");
			}
			return base.RedirectToAction("RecordTitleCoOrderPayment", new { TitleCoId = tid });
		}

		[Authorize]
		public ActionResult DeleteUserFile(int id, int userId)
		{
			ActionResult action;
			try
			{
				this._asset.DeleteUserFile(id);
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "User file deleted");
				action = base.RedirectToAction("EditUser", "Admin", new { id = userId });
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, string.Concat("There was an error deleting user file. Error: ", exception.Message));
				action = base.RedirectToAction("EditUser", "Admin", new { id = userId });
			}
			return action;
		}

		[Authorize]
		public ActionResult DeleteUserNote(int id, int userId)
		{
			ActionResult action;
			try
			{
				this._asset.DeleteUserNote(id);
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "User file deleted");
				action = base.RedirectToAction("EditUser", "Admin", new { id = userId });
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, string.Concat("There was an error deleting user note. Error: ", exception.Message));
				action = base.RedirectToAction("EditUser", "Admin", new { id = userId });
			}
			return action;
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
		public FileUploadJsonResult DocumentUpload(HttpPostedFileBase file, Guid assetId, string title)
		{
			if (file == null || string.IsNullOrEmpty(file.FileName))
			{
				return new FileUploadJsonResult()
				{
					Data = new { message = "FileIsNull" }
				};
			}
			string str = this._fileManager.SaveFile(file, assetId, FileType.Document);
			if (str == null)
			{
				return new FileUploadJsonResult()
				{
					Data = new { message = "false" }
				};
			}
			string str1 = string.Concat(new string[] { "/Admin/DownloadDocument?fileName=", HttpUtility.UrlEncode(str), "&assetId=", assetId.ToString(), "&contentType=", HttpUtility.UrlEncode(file.ContentType), "&title=", HttpUtility.UrlEncode(title) });
			return new FileUploadJsonResult()
			{
				Data = new { message = "true", filename = str, contentType = file.ContentType, size = this._fileManager.GetFileSize(file.ContentLength), downloadUrl = str1 }
			};
		}

		[HttpGet]
		public ActionResult DownloadDocument(string fileName, string assetId, string contentType, string title)
		{
			FilePathResult filePathResult = this._fileManager.DownloadDocument(fileName, assetId, contentType, title);
			if (filePathResult != null)
			{
				return filePathResult;
			}
			return base.Content("File not found.");
		}

		[Authorize]
		[HttpGet]
		public FileResult DownloadUserFile(int id)
		{
			byte[] userFile = this._asset.GetUserFile(id);
			if (userFile == null)
			{
				return null;
			}
			return this.File(userFile, "application/octet-stream", "download.pdf");
		}

        [AllowAnonymous]
        [HttpGet]
        public JsonResult DoesUserFileExist(int id)
        {
            byte[] userFile = this._asset.GetUserFile(id);
            if (userFile != null) return new JsonResult() { Data = new { success = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return new JsonResult() { Data = new { success = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
		public JsonResult EditAsset(Guid assetId)
		{
			int userId = this._user.GetUserByUsername(base.User.Identity.Name).UserId;
			if (!this._asset.IsAssetLocked(assetId, userId))
			{
				this._asset.LockAsset(userId, assetId);
				return new JsonResult()
				{
					Data = new { Status = "unlocked" }
				};
			}
			if (this.isUserAdmin(this._user.GetUserByUsername(base.User.Identity.Name)))
			{
				return new JsonResult()
				{
					Data = new { Status = "admin" }
				};
			}
			return new JsonResult()
			{
				Data = new { Status = "locked" }
			};
		}

		[Authorize]
		[HttpGet]
		public ActionResult EditContractPayment(int? id, int userId, string method, string name)
		{
			if (!base.ValidateAdminUser(this._user.GetUserByUsername(base.User.Identity.Name)))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			if (id.GetValueOrDefault(0) <= 0)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid payment ID.");
				return base.RedirectToAction(method);
			}
			RecordContractPaymentModel contractPayment = this._user.GetContractPayment(id.Value);
			contractPayment.Method = method;
			contractPayment.Name = name;
			contractPayment.UserId = userId;
			return base.View(contractPayment);
		}

		[Authorize]
		[HttpPost]
		public ActionResult EditContractPayment(RecordContractPaymentModel model)
		{
			this._user.UpdateContractPayment(model);
			return base.RedirectToAction(model.Method, new { id = model.UserId });
		}

		[Authorize]
		[HttpGet]
		public ActionResult EditTitleCompany(int id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			((dynamic)base.ViewBag).IsAdmin = this.isUserAdmin(userByUsername);
			return base.View(this._user.GetTitleById(id));
		}

		[Authorize]
		[HttpPost]
		public ActionResult EditTitleCompany(TitleCompanyModel model)
		{
			((dynamic)base.ViewBag).IsAdmin = this.isUserAdmin(this._user.GetUserByUsername(base.User.Identity.Name));
			if (!base.ModelState.IsValid)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid fields.");
			}
			else
			{
				this._user.UpdateTitle(model);
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Title Company successfully updated.");
			}
			return base.View(model);
		}

		[Authorize]
		[HttpGet]
		public ActionResult EditTitleCompanyUser(int id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			TitleCompanyUserModel titleUserById = this._user.GetTitleUserById(id);
			((dynamic)base.ViewBag).Email = userByUsername.Username;
			titleUserById.ControllingUserType = userByUsername.UserType;
			return base.View(titleUserById);
		}

		[Authorize]
		[HttpPost]
		public ActionResult EditTitleCompanyUser(TitleCompanyUserModel model)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			if (!base.ModelState.IsValid)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid fields.");
			}
			else
			{
				Tuple<bool, string> tuple = this._user.ValidateTitleUserStateAvailability(model);
				if (!tuple.Item1)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, tuple.Item2);
				}
				else
				{
					if (userByUsername.UserType == UserType.TitleCompUser && model.IsManager)
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You are not authorized to change the status to a manager");
						((dynamic)base.ViewBag).Email = model.Email;
						return base.View(model);
					}
					if (model.IsManager)
					{
						model.SelectedStates = null;
					}
					this._user.UpdateTitleUser(model);
					base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Title Company user successfully updated.");
				}
			}
			model.ControllingUserType = userByUsername.UserType;
			((dynamic)base.ViewBag).Email = userByUsername.Username;
			return base.View(model);
		}

		[Authorize]
		[HttpGet]
		public ActionResult EditTitleOrderPayment(int? id, int? tid)
		{
			if (!base.ValidateAdminUser(this._user.GetUserByUsername(base.User.Identity.Name)))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			if (id.GetValueOrDefault(0) > 0 && tid.GetValueOrDefault(0) > 0)
			{
				OrderModel titleOrderPayment = this._asset.GetTitleOrderPayment(id.Value);
				titleOrderPayment.TitleCompanyId = tid.Value;
				return base.View(titleOrderPayment);
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid payment identification number.");
			if (tid.GetValueOrDefault(0) <= 0)
			{
				return base.RedirectToAction("ManageTitleCompanies");
			}
			return base.RedirectToAction("RecordTitleCoOrderPayment", new { TitleCoId = tid });
		}

		[Authorize]
		[HttpPost]
		public ActionResult EditTitleOrderPayment(OrderModel model)
		{
			if (!base.ModelState.IsValid)
			{
				return base.View(model);
			}
			this._asset.UpdateTitleOrderPayment(model);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Order updated.");
			return base.RedirectToAction("RecordTitleCoOrderPayment", new { TitleCoId = model.TitleCompanyId });
		}

		[Authorize]
		[HttpGet]
		public ActionResult EditUser(int id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			UserModel userById = this._user.GetUserById(id);
			userById.ControllingUserType = userByUsername.UserType;
			return base.View(userById);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult EditUser(UserModel model)
		{
			if (!base.ModelState.IsValid)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid fields.");
			}
			else
			{
				this._user.UpdateUser(model);
				model = this._user.GetUserById(model.UserId);
				if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
				{
					this._email.Send(new ProfileUpdatedEmail()
					{
						Email = model.Username,
						NameOfRegistrant = model.FullName
					});
				}
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "User successfully updated.");
			}
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			model.ControllingUserType = userByUsername.UserType;
			return base.View(model);
		}

		[Authorize]
		[HttpGet]
		public ViewResult EditUserNote(int id)
		{
			return base.View(this._asset.GetUserNote(id));
		}

		[HttpPost]
		public ActionResult EditUserNote(UserNoteModel model)
		{
			try
			{
				this._asset.SaveUserNote(model);
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "User note saved.");
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, string.Concat("There was an error saving user note. Error: ", exception.Message));
			}
			return base.RedirectToAction("EditUser", "Admin", new { id = model.UserId });
		}

		public void Email_With_CCandBCC(string Message)
		{
			int i;
			try
			{
				string str = "rebecca.heft@gate6.com,john.vaughan@gate6.com,sushil.pandey@gate6.com";
				string str1 = "";
				string str2 = "USCREonline Image Extract Tool From PDF file";
				MailMessage mailMessage = new MailMessage()
				{
					From = new MailAddress("gate6.info@gate6.com"),
					Subject = str2,
					Body = Message,
					IsBodyHtml = true
				};
				string[] strArrays = "shashank.shukla@gate6.com".Split(new char[] { ',' });
				for (i = 0; i < (int)strArrays.Length; i++)
				{
					string str3 = strArrays[i];
					mailMessage.To.Add(new MailAddress(str3));
				}
				if (str != "")
				{
					strArrays = str.Split(new char[] { ',' });
					for (i = 0; i < (int)strArrays.Length; i++)
					{
						string str4 = strArrays[i];
						mailMessage.CC.Add(new MailAddress(str4));
					}
				}
				if (str1 != "")
				{
					strArrays = str1.Split(new char[] { ',' });
					for (i = 0; i < (int)strArrays.Length; i++)
					{
						string str5 = strArrays[i];
						mailMessage.Bcc.Add(new MailAddress(str5));
					}
				}
				SmtpClient smtpClient = new SmtpClient()
				{
					Host = "smtp.gmail.com",
					EnableSsl = true
				};
				NetworkCredential networkCredential = new NetworkCredential()
				{
					UserName = "gate6.info@gate6.com",
					Password = "Goole2010A!!"
				};
				smtpClient.UseDefaultCredentials = false;
				smtpClient.Credentials = networkCredential;
				smtpClient.Port = 587;
				smtpClient.Send(mailMessage);
				AdminController.WriteLog("Send message successfully");
			}
			catch (Exception exception)
			{
				AdminController.WriteLog(this.ExceptionDetails(exception));
			}
		}

		[Authorize]
		[HttpGet]
		public ActionResult EmailInvestmentAssetSearchCriteria(int id)
		{
			EmailInvestmentSearchCriteriaModel emailInvestmentSearchCriteriaModel = new EmailInvestmentSearchCriteriaModel();
			this._user.GetNarMembers(new NarMemberSearchModel()
			{
				ShowActiveOnly = true
			}).ForEach((NarMemberViewModel f) => emailInvestmentSearchCriteriaModel.NARMembers.Add(new SelectListItem()
			{
				Text = f.FullName,
				Value = f.NarMemberId.ToString()
			}));
			emailInvestmentSearchCriteriaModel.SearchCriteriaId = id;
			return base.View(emailInvestmentSearchCriteriaModel);
		}

		[HttpPost]
		public ActionResult EmailInvestmentAssetSearchCriteria(EmailInvestmentSearchCriteriaModel model)
		{
			string str;
			string str1;
			string str2;
			string str3;
			if (!base.ModelState.IsValid)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Missing email address");
			}
			else
			{
				AssetSearchCriteriaModel assetSearchCriteriaById = this._asset.GetAssetSearchCriteriaById(model.SearchCriteriaId);
				UserModel userById = this._user.GetUserById(assetSearchCriteriaById.UserId);
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < model.SelectNarMemberIds.Count; i++)
				{
					NarMemberViewModel narMember = this._user.GetNarMember(model.SelectNarMemberIds[i]);
					stringBuilder.Append(narMember.Email);
					if (i < model.SelectNarMemberIds.Count - 1)
					{
						stringBuilder.Append(", ");
					}
				}
				if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
				{
					SendSearchCriteriaEmail sendSearchCriteriaEmail = new SendSearchCriteriaEmail()
					{
						Email = stringBuilder.ToString(),
						AccreditedTenantProfilePercentage = assetSearchCriteriaById.OtherDemographicDetail.MinimumForAccreditedTenantProfiles,
						AssetSought = EnumHelper.GetEnumDescription(assetSearchCriteriaById.TypeOfAssetsSought),
						BudgetMax = assetSearchCriteriaById.FinancialInvestmentParameters.InvestmentFundingRangeMax,
						BudgetMin = assetSearchCriteriaById.FinancialInvestmentParameters.InvestmentFundingRangeMin
					};
					if (assetSearchCriteriaById.MultiFamilyDemographicDetail.TurnKey.HasValue)
					{
						str = (assetSearchCriteriaById.MultiFamilyDemographicDetail.TurnKey.Value ? "Yes" : "No");
					}
					else
					{
						str = "No";
					}
					sendSearchCriteriaEmail.DoesProjectHaveToBeTurnKey = str;
					if (assetSearchCriteriaById.OtherDemographicDetail.PropertyRequiresMajorTenant.HasValue)
					{
						str1 = (assetSearchCriteriaById.OtherDemographicDetail.PropertyRequiresMajorTenant.Value ? "Yes" : "No");
					}
					else
					{
						str1 = "No";
					}
					sendSearchCriteriaEmail.MajorTenant = str1;
					sendSearchCriteriaEmail.MaxRatio1BedroomUnits = assetSearchCriteriaById.MultiFamilyDemographicDetail.MaxRatioOfOneBedroomUnits;
					sendSearchCriteriaEmail.MaxRatioStudioUnits = assetSearchCriteriaById.MultiFamilyDemographicDetail.MaxRatioOfEFfUnits;
					sendSearchCriteriaEmail.MinRating = assetSearchCriteriaById.MultiFamilyDemographicDetail.GradeClassificationRequirementOfProperty;
					sendSearchCriteriaEmail.RequireRehabBudget = "No";
					if (assetSearchCriteriaById.OtherDemographicDetail.CanBeVacant.HasValue)
					{
						str2 = (assetSearchCriteriaById.OtherDemographicDetail.CanBeVacant.Value ? "Yes" : "No");
					}
					else
					{
						str2 = "No";
					}
					sendSearchCriteriaEmail.SubstantiallyVacant = str2;
					if (!assetSearchCriteriaById.MultiFamilyDemographicDetail.UnderperformingProperty.HasValue)
					{
						str3 = "No";
					}
					else if (assetSearchCriteriaById.MultiFamilyDemographicDetail.UnderperformingProperty.Value)
					{
						str3 = "Yes";
					}
					else if (assetSearchCriteriaById.MultiFamilyDemographicDetail.UnderperformingPropertyOptional.HasValue)
					{
						str3 = (assetSearchCriteriaById.MultiFamilyDemographicDetail.UnderperformingPropertyOptional.Value ? "Optional " : "No");
					}
					else
					{
						str3 = "No";
					}
					sendSearchCriteriaEmail.Underperforming = str3;
					sendSearchCriteriaEmail.UserCity = userById.City;
					sendSearchCriteriaEmail.UserState = userById.SelectedState;
					sendSearchCriteriaEmail.Name = userById.FullName;
					SendSearchCriteriaEmail strs = sendSearchCriteriaEmail;
					strs.GeographicInterests = new List<string>();
					if (assetSearchCriteriaById.GeographicParameters.Interests != null)
					{
						assetSearchCriteriaById.GeographicParameters.Interests.ForEach((GeographicParameterInterestModel f) => {
							if (f.Cities != null)
							{
								f.Cities.ForEach((string c) => {
									if (!string.IsNullOrEmpty(c.Trim()))
									{
										strs.GeographicInterests.Add(string.Format("{0}, {1}", c, f.StateOfInterest));
									}
								});
							}
						});
					}
					this._email.Send(strs);
				}
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Search criteria successfully emailed.");
			}
			this._user.GetNarMembers(new NarMemberSearchModel()
			{
				ShowActiveOnly = true
			}).ForEach((NarMemberViewModel f) => model.NARMembers.Add(new SelectListItem()
			{
				Text = f.FullName,
				Value = f.NarMemberId.ToString()
			}));
			return base.View(model);
		}

		public void EndProcess(IAsyncResult result)
		{
			string str = ((AdminController.ProcessTask)result.AsyncState).EndInvoke(result);
			this.progress.Remove(str);
		}

		public string ExceptionDetails(Exception ex)
		{
			string str;
			string empty = string.Empty;
			try
			{
				empty = string.Concat(new object[] { "Exception type ", ex.GetType(), Environment.NewLine, "Exception message: ", ex.Message, Environment.NewLine, "Stack trace: ", ex.StackTrace, Environment.NewLine });
				if (ex.InnerException != null)
				{
					empty = string.Concat(new object[] { empty, "---BEGIN InnerException--- ", Environment.NewLine, "Exception type ", ex.InnerException.GetType(), Environment.NewLine, "Exception message: ", ex.InnerException.Message, Environment.NewLine, "Stack trace: ", ex.InnerException.StackTrace, Environment.NewLine, "---END Inner Exception" });
				}
				str = empty;
			}
			catch (Exception exception)
			{
				AdminController.WriteLog(this.ExceptionDetails(exception));
				return empty;
			}
			return str;
		}

		[Authorize]
		[HttpGet]
		public ViewResult ExtractImagesFromBrochure()
		{
			if (!base.ValidateAdminUser(this._user.GetUserByUsername(base.User.Identity.Name)))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				this.Email_With_CCandBCC("You do not have access to view this page.");
				return base.View("~/Views/Home/Index.cshtml");
			}
			((dynamic)base.ViewBag).AssetType = this.populateAssetTypeDDL();
			((dynamic)base.ViewBag).PortfolioList = this.populatePortfolioListDDL();
            ((dynamic)base.ViewBag).StateList = _user.PopulateStateList();
            return base.View();
		}

		[HttpPost]
        public ActionResult ExtractImagesFromBrochure(HttpPostedFileBase file)
        {
            ViewBag.AssetType = populateAssetTypeDDL();
            ViewBag.PortfolioList = this.populatePortfolioListDDL();
            ViewBag.StateList = _user.PopulateStateList();
            if (file != null)
            {
                if (System.IO.Path.GetExtension(file.FileName).ToLower() == ".pdf")
                {
                    try
                    {
                        var guid = Guid.NewGuid().ToString();
                        var path = System.IO.Path.Combine(ConfigurationManager.AppSettings["DataRoot"], "ImportFiles", guid);
                        if (!System.IO.Directory.Exists(path))
                        {
                            System.IO.Directory.CreateDirectory(path);
                        }
                        TempData["PdfId"] = guid;
                        path = System.IO.Path.Combine(path, guid + System.IO.Path.GetExtension(file.FileName));
                        file.SaveAs(path);
                        return RedirectToAction("ViewExtractedImages", new { filepath = path });
                    }
                    catch(Exception exception) {
                        TempData["message"] = new MessageViewModel(MessageTypes.Error, string.Concat("Could not extract images. Error: ", exception.Message));
                        return View();
                    }
                }
                TempData["message"] = new MessageViewModel(MessageTypes.Error, "Uploaded file must be in PDF format.");
            }
            else
                TempData["message"] = new MessageViewModel(MessageTypes.Error, "You must select a file to upload.");
            return View();
        }

        [HttpPost]
		[MultipleButton(Name="action", Argument="Generate1099")]
		public FileResult Generate1099(AccountingRecordDisplayModel model)
		{
			UserModel userById = this._user.GetUserById(model.UserId);
			if (this._user.GetPayer(model.StartYear, userById.UserId, model.Type) == null)
			{
				return this.File(this._pdf.Submit1099(new MiscellaneousIncomeTemplateModel()
				{
					RecipientAddress = (!string.IsNullOrEmpty(userById.AddressLine2) ? string.Concat(userById.AddressLine1, " ", userById.AddressLine2) : userById.AddressLine1),
					RecipientCity = userById.City,
					RecipientState = userById.SelectedState,
					RecipientCountry = "USA",
					RecipientName = userById.FullName,
					RecipientZip = userById.Zip,
					RecipientFederalId = model.Tin,
					NonEmployeeCompensation = new double?(model.TotalPaidForFiscalYear)
				}).ToArray(), "application/octet-stream", string.Format("{0}-{1}-1099.pdf", userById.FullName, model.StartYear));
			}
			return this.File(this._pdf.Submit1099(new MiscellaneousIncomeTemplateModel()
			{
				PayerName = ConfigurationManager.AppSettings["EpiFundName"],
				PayerCity = ConfigurationManager.AppSettings["EpiFundCityStateZip"],
				PayerState = "",
				PayerZip = "",
				PayerTelephone = ConfigurationManager.AppSettings["EpiFundContactNumber"],
				PayerFederalId = ConfigurationManager.AppSettings["EPIFundFederalID"],
				RecipientAddress = (!string.IsNullOrEmpty(userById.AddressLine2) ? string.Concat(userById.AddressLine1, " ", userById.AddressLine2) : userById.AddressLine1),
				PayerAddress = ConfigurationManager.AppSettings["EpiFundAddress"],
				RecipientCity = userById.City,
				RecipientState = userById.SelectedState,
				RecipientCountry = "USA",
				RecipientName = userById.FullName,
				RecipientZip = userById.Zip,
				RecipientFederalId = userById.CorporateTIN,
				NonEmployeeCompensation = new double?(model.TotalPaidForFiscalYear)
			}).ToArray(), "application/octet-stream", string.Format("{0}-{1}-1099.pdf", userById.FullName, model.StartYear));
		}

		private byte[] getBytesFromFile(string path)
		{
			byte[] numArray;
			FileStream fileStream = null;
			try
			{
				fileStream = System.IO.File.OpenRead(path);
				byte[] numArray1 = new byte[(fileStream.Length)]; //new byte[fs.Length];
                fileStream.Read(numArray1, 0, Convert.ToInt32(fileStream.Length));
				numArray = numArray1;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
					fileStream.Dispose();
				}
			}
			return numArray;
		}

		public ContentResult GetCurrentProgress(string id)
		{
			base.ControllerContext.HttpContext.Response.AddHeader("cache-control", "no-cache");
			int status = this.progress.GetStatus(id);
			return base.Content(status.ToString());
		}

		[HttpGet]
		public ActionResult GetImageSource(string fileName, string assetId, string contentType)
		{
			byte[] viewAssetImage = this._fileManager.GetViewAssetImage(fileName, assetId, contentType, 500, 500);
			if (viewAssetImage == null)
			{
				return null;
			}
			return base.File(viewAssetImage, "image/png");
		}

		[HttpPost]
		public JsonResult GetListingUserInformation(string id)
		{
			JsonResult jsonResult;
			try
			{
				int num = 0;
				if (int.TryParse(id, out num))
				{
					NarMemberViewModel narMember = this._user.GetNarMember(num);
					if (narMember != null)
					{
						jsonResult = new JsonResult()
						{
							Data = new { Status = "true", 
										 ListingAgentWorkNumber = narMember.WorkNumber, 
										 ListingAgentFaxNumber = narMember.FaxNumber, 
										 ListingAgentCellNumber = narMember.CellPhoneNumber, 
										 ListingAgentName = narMember.FullName, ListingAgentCompany = narMember.CompanyName ?? "", 
										 ListingAgentEmail = narMember.Email, ListingAgentCorpAddress = narMember.AddressLine1 ?? "", 
										 ListingAgentCorpAddress2 = narMember.AddressLine2 ?? "", ListingAgentCity = narMember.City ?? "", 
										 ListingAgentState = narMember.State ?? "", ListingAgentZip = narMember.Zip ?? "", 
										 FirstName = narMember.FirstName ?? "", LastName = narMember.LastName ?? "", 
										 CommissionShareAgr = narMember.CommissionShareAgr, CommissionAmount = narMember.CommissionAmount, 
										 DateOfCsaConfirm = narMember.DateOfCsaConfirm, NARMemberId = narMember.NarMemberId, 
										 ReferredByUserId = narMember.ReferredByUserId, IsActive = narMember.IsActive, 
										 Website = narMember.Website, Registered = narMember.Registered }
						};
						return jsonResult;
					}
				}
				jsonResult = new JsonResult()
				{
					Data = new { Status = "false" }
				};
			}
			catch
			{
				jsonResult = new JsonResult()
				{
					Data = new { Status = "false" }
				};
			}
			return jsonResult;
		}

		[HttpGet]
		public ActionResult GetMainImageSource(Guid assetId)
		{
			return this._fileManager.GetMainImageSource(assetId);
		}

		[HttpGet]
		public string GetQryAssetImages()
		{
			return this._fileManager.GetAssetImages();
		}

		[HttpPost]
		public ActionResult GetSelectedItemsAndNotifyRegistrant(IEnumerable<AdminAssetQuickListModel> items)
		{
			NotifyRegistrantOfMatchingAssetsModel notifyRegistrantOfMatchingAssetsModel = new NotifyRegistrantOfMatchingAssetsModel();
			if (items != null)
			{
				notifyRegistrantOfMatchingAssetsModel.AssetNumbers = (
					from w in items
					where w.IsSelected
					select w into s
					select s.AssetNumber).ToList<int>();
			}
			notifyRegistrantOfMatchingAssetsModel.Searches = new List<SelectListItem>();
			this._user.GetAllSearches().ForEach((UserAssetSearchCriteriaQuickViewModel f) => notifyRegistrantOfMatchingAssetsModel.Searches.Add(new SelectListItem()
			{
				Text = string.Format("{0}'s search criteria form entered on {1}", f.UserFullName, f.DateCreated),
				Value = f.AssetSearchCriteriaId.ToString()
			}));
			return base.View("NotifyRegistrant", notifyRegistrantOfMatchingAssetsModel);
		}

		[HttpGet]
		public ActionResult GetTempImageSource(string filePath)
		{
			return new FilePathResult(Path.Combine(new string[] { filePath }), "image/jpeg");
		}

		public ActionResult GetTempImageThumbnailFromFile(string filename, Guid assetId, string userId, string dateString)
		{
			ThumbnailViewModel tempImageThumbnailByte = this._fileManager.GetTempImageThumbnailByte(filename, assetId.ToString(), dateString, userId, 100, 100);
			if (tempImageThumbnailByte == null)
			{
				return null;
			}
			return base.File(tempImageThumbnailByte.Bytes, tempImageThumbnailByte.ContentType);
		}

		public ActionResult GetThumbnailFromFile(string filename, Guid assetId)
		{
			byte[] thumbnailByte = this._fileManager.GetThumbnailByte(filename, assetId.ToString(), 100, 100);
			if (thumbnailByte == null)
			{
				return null;
			}
			return base.File(thumbnailByte, "image/png");
		}

        [HttpPost]
        public JsonResult GetPIInformation(string id)
        {
            JsonResult jsonResult;
            try
            {
                var entity = _user.GetPrincipalInvestor(Convert.ToInt32(id));
                if (entity != null)
                {
                    jsonResult = new JsonResult()
                    {
                        Data = new { Status = "true", entity.FirstName, entity.LastName, entity.AddressLine1, entity.AddressLine2, entity.City, entity.State, entity.Zip, entity.Country, WorkPhone = entity.WorkNumber, CellPhone = entity.CellPhoneNumber, Fax = entity.FaxNumber, entity.Email, entity.CompanyName, entity.IsActive }
                    };
                }
                else jsonResult = new JsonResult() { Data = new { Status = "false" } };
            }
            catch
            {
                jsonResult = new JsonResult() { Data = new { Status = "false" } };
            }
            return jsonResult;
        }

        [HttpPost]
		public JsonResult GetOperatingCompanyInformation(string id)
		{
			JsonResult jsonResult;
			try
			{
                var entity = _user.GetOperatingCompany(Guid.Parse(id));
                if (entity != null)
                {
                    jsonResult = new JsonResult()
                    {
                        Data = new { Status = "true", 
							Id = entity.OperatingCompanyId, 
							entity.FirstName, 
							entity.LastName, 
							entity.AddressLine1, 
							entity.AddressLine2, 
							entity.City, 
							entity.State, 
							entity.Zip, 
							entity.Country,
							WorkPhone = entity.WorkNumber, 
							CellPhone = entity.CellNumber,
							//Fax = entity.FaxNumber, 
							entity.LinkedIn,
							entity.Facebook,
							entity.Instagram,
							entity.Twitter,
							entity.Email, 
							entity.CompanyName, 
							entity.IsActive }
                    };
                }
                else jsonResult = new JsonResult() { Data = new { Status = "false" } };
            }
			catch
			{
                jsonResult = new JsonResult() { Data = new { Status = "false" } };
            }
			return jsonResult;
		}

        [HttpPost]
        public JsonResult GetHoldingCompanyInformation(string id)
        {
            JsonResult jsonResult;
            try
            {
                var entity = _user.GetHoldingCompany(Guid.Parse(id));
                if (entity != null)
                {
					jsonResult = new JsonResult()
					{
						Data = new
						{
							Status = "true",
							Id = entity.HoldingCompanyId,
							entity.ISRA,
							entity.FirstName,
							entity.LastName,
							entity.AddressLine1,
							entity.AddressLine2,
							entity.City,
							entity.State,
							entity.Zip,
							entity.Country,
							WorkPhone = entity.WorkNumber,
							CellPhone = entity.CellNumber,
							//Fax = entity.FaxNumber, 
							entity.LinkedIn,
							entity.Facebook,
							entity.Instagram,
							entity.Twitter,

							entity.Email,
							entity.CompanyName,
							entity.IsActive
						}
					};
                }
                else jsonResult = new JsonResult() { Data = new { Status = "false" } };
            }
            catch
            {
                jsonResult = new JsonResult() { Data = new { Status = "false" } };
            }
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetApplicablePrincipalInvestorUserOptions(string email)
        {
            JsonResult jsonResult;
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var pis = _user.GetPrincipalInvestors(new PrincipalInvestorSearchModel() { Email = email, DomainSearch = true });
                    var options = new Dictionary<string, string>();
                    foreach (var option in pis) 
						options.Add(option.PrincipalInvestorId.ToString(), option.Email);
                    jsonResult = new JsonResult() { Data = new { Status = "true", Options = options } };
                }
                else jsonResult = new JsonResult() { Data = new { Status = "false", Message = "Email is null" } };
            }
            catch
            {
                jsonResult = new JsonResult() { Data = new { Status = "false" } };
            }
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetApplicableHoldingCompanyOptions(string id)
        {
            JsonResult jsonResult;
            try
            {
                Guid ocId = new Guid();
                if (!string.IsNullOrEmpty(id) && Guid.TryParse(id, out ocId))
                {
                    var entities = _user.GetHoldingCompaniesForOperatingCompany(ocId);
                    if (entities != null)
                    {
                        entities = entities.OrderBy(x => x.CompanyName).ToList();
                        var options = new Dictionary<string, string>();
                        foreach (var option in entities) 
							options.Add(option.HoldingCompanyId.ToString(), option.CompanyName);
                        return jsonResult = new JsonResult() { Data = new { Status = "true", Options = options } };
                    }
                }
                return jsonResult = new JsonResult() { Data = new { Status = "false" } };
            }
            catch
            {
                jsonResult = new JsonResult() { Data = new { Status = "false" } };
            }
            return jsonResult;
        }

        

        public UserModel GetUserInforWithUserName(string userName)
        {
            UserModel userByName = new UserModel();
           
            if(userName != null)
                userByName = this._user.GetUserByUsername(userName);
            return userByName;
        }

        public ActionResult GetVideo(string id, string filepath)
		{
			string str = Path.Combine(ConfigurationManager.AppSettings["DataRoot"], "Videos", id.ToString(), filepath);
			if (!System.IO.File.Exists(str))
			{
				return null;
			}
			return this.File(str, "video/mp4", filepath);
		}

		[Authorize]
		public ViewResult ICAccountingReportDisplay(int? id = null)
		{
			DateTime now;
			((dynamic)base.ViewBag).AssetType = this.populateAssetTypeDDL();
			((dynamic)base.ViewBag).PortfolioList = this.populatePortfolioListDDL();
            ((dynamic)base.ViewBag).StateList = _user.PopulateStateList();
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			AccountingRecordDisplayModel cAccountingReportDisplay = null;
			if (!id.HasValue)
			{
				IUserManager userManager = this._user;
				int userId = userByUsername.UserId;
				now = DateTime.Now;
				cAccountingReportDisplay = userManager.GetICAccountingReportDisplay(userId, now.Year, userByUsername);
			}
			else
			{
				IUserManager userManager1 = this._user;
				int value = id.Value;
				now = DateTime.Now;
				cAccountingReportDisplay = userManager1.GetICAccountingReportDisplay(value, now.Year, userByUsername);
			}
			return base.View(cAccountingReportDisplay);
		}

		[HttpPost]
		[MultipleButton(Name="action", Argument="ICAccountingReportDisplay")]
		public ViewResult ICAccountingReportDisplay(AccountingRecordDisplayModel model)
		{
			((dynamic)base.ViewBag).AssetType = this.populateAssetTypeDDL();
            ((dynamic)base.ViewBag).PortfolioList = this.populatePortfolioListDDL();
            ((dynamic)base.ViewBag).StateList = _user.PopulateStateList();
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			return base.View(this._user.GetICAccountingReportDisplay(model.UserId, model.StartYear, userByUsername));
		}

		[HttpPost]
		public FileUploadJsonResult ImageDelete(string imgId, Guid assetId)
		{
			FileUploadJsonResult fileUploadJsonResult;
			FileUploadJsonResult fileUploadJsonResult1;
			if (imgId != null)
			{
				try
				{
					if (!this._fileManager.DeleteFile(imgId, assetId, FileType.Image))
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

		[Authorize]
		public ActionResult ImagesTest(int? AssetNumber)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			((dynamic)base.ViewBag).IsCorpAdmin = this.isUserAdmin(userByUsername);
			base.ViewBag.IsICAdmin = (userByUsername.UserType == UserType.ICAdmin ? true : false);
			AssetDescriptionModel assetByAssetNumber = this._asset.GetAssetByAssetNumber(AssetNumber.Value);
			AssetViewModel asset = this._asset.GetAsset(assetByAssetNumber.AssetId, false);
			this.SetUpAsset(asset, userByUsername);
			return base.View(asset);
		}

		[HttpPost]
		public FileUploadJsonResult ImageUpload(HttpPostedFileBase file, Guid assetId)
		{
			if (file == null || string.IsNullOrEmpty(file.FileName))
			{
				return new FileUploadJsonResult()
				{
					Data = new { message = "FileIsNull" }
				};
			}
			if (!(file.ContentType.ToLower() == "image/jpeg") && !(file.ContentType.ToLower() == "image/jpg") && !(file.ContentType.ToLower() == "image/gif") && !(file.ContentType.ToLower() == "image/png"))
			{
				return new FileUploadJsonResult()
				{
					Data = new { message = "InvalidImageType" }
				};
			}
			string str = this._fileManager.SaveFile(file, assetId, FileType.Image);
			try
			{
				this._fileManager.CreateThumbnail(str, assetId);
			}
			catch
			{
			}
			if (str == null)
			{
				return new FileUploadJsonResult()
				{
					Data = new { message = "false" }
				};
			}
			return new FileUploadJsonResult()
			{
				Data = new { message = "true", filename = str, contentType = file.ContentType, isCorpAdmin = this.isUserAdmin(this.AuthenticatedUser), isICAdmin = (this.AuthenticatedUser.UserType == UserType.ICAdmin ? true : false) }
			};
		}

		[Authorize]
		[HttpGet]
		public ActionResult InviteTitleCompanyUser(int id)
		{
			TitleCompanyUserModel titleUserById = this._user.GetTitleUserById(id);
			string host = base.Request.Url.Host;
			int port = base.Request.Url.Port;
			string str = port.ToString();
			string empty = string.Empty;
			if (str != null)
			{
				string[] strArrays = new string[] { "http://", host, ":", str, "/Admin/ValidateManager/id=", null };
				port = titleUserById.TitleCompanyUserId;
				strArrays[5] = port.ToString();
				empty = string.Concat(strArrays);
			}
			else
			{
				port = titleUserById.TitleCompanyUserId;
				empty = string.Concat("http://", host, "/Admin/ValidateManager/id=", port.ToString());
			}
			if (base.CanSendAutoEmail(BaseController.AutoEmailType.Title))
			{
				this._email.Send(new CompanyUserWelcomeEmail()
				{
					Email = titleUserById.Email,
					TempPassword = titleUserById.Password,
					To = titleUserById.FullName,
					RegistrationType = "Title Company User",
					URL = empty
				});
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully invited Title Company User");
			return base.RedirectToAction("ManageTitleCompanyUsers", new { id = titleUserById.TitleCompanyId });
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
		[HttpPost]
		[MultipleButton(Name="action", Argument="JVMAAccountingReportDisplay")]
		public ViewResult JVMAAccountingReportDisplay(AccountingRecordDisplayModel model)
		{
			((dynamic)base.ViewBag).AssetType = this.populateAssetTypeDDL();
			((dynamic)base.ViewBag).PortfolioList = this.populatePortfolioListDDL();
            ((dynamic)base.ViewBag).StateList = _user.PopulateStateList();
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			return base.View(this._user.GetJVMAAccountingReportDisplay(model.UserId, model.StartYear, userByUsername));
		}

		[Authorize]
		public ViewResult JVMAAccountingReportDisplay(int? id = null)
		{
			DateTime now;
			((dynamic)base.ViewBag).AssetType = this.populateAssetTypeDDL();
			((dynamic)base.ViewBag).PortfolioList = this.populatePortfolioListDDL();
            ((dynamic)base.ViewBag).StateList = _user.PopulateStateList();
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			AccountingRecordDisplayModel jVMAAccountingReportDisplay = null;
			if (!id.HasValue)
			{
				IUserManager userManager = this._user;
				int userId = userByUsername.UserId;
				now = DateTime.Now;
				jVMAAccountingReportDisplay = userManager.GetJVMAAccountingReportDisplay(userId, now.Year, userByUsername);
			}
			else
			{
				IUserManager userManager1 = this._user;
				int value = id.Value;
				now = DateTime.Now;
				jVMAAccountingReportDisplay = userManager1.GetJVMAAccountingReportDisplay(value, now.Year, userByUsername);
			}
			return base.View(jVMAAccountingReportDisplay);
		}

		[Authorize]
		public ActionResult ManageAssetNarMembers(NarMemberSearchResultsModel model)
		{
			int value;
			if (!base.ValidateAdminUser(this._user.GetUserByUsername(base.User.Identity.Name)))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			List<NarMemberViewModel> narMemberViewModels = new List<NarMemberViewModel>();
			NarMemberSearchModel narMemberSearchModel = new NarMemberSearchModel()
			{
				AddressLine1 = model.AddressLine1,
				City = model.City,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				State = model.State,
				Zip = model.Zip,
				ShowActiveOnly = model.ShowActiveOnly,
				CompanyName = model.CompanyName
			};
			narMemberViewModels = this._user.GetAssetNarMembers(narMemberSearchModel);
			((dynamic)base.ViewBag).CurrentSort = model.SortOrder;
			base.ViewBag.CitySortParm = (model.SortOrder == "city" ? "city_desc" : "city");
			base.ViewBag.StateSortParm = (model.SortOrder == "state" ? "state_desc" : "state");
			base.ViewBag.CompanyNameSortParm = (model.SortOrder == "company" ? "company_desc" : "company");
			base.ViewBag.AddressSortParm = (model.SortOrder == "address" ? "address_desc" : "address");
			base.ViewBag.EmailSortParm = (model.SortOrder == "email" ? "email_desc" : "email");
			base.ViewBag.FirstNameSortParm = (model.SortOrder == "first" ? "first_desc" : "first");
			base.ViewBag.LastNameSortParm = (model.SortOrder == "last" ? "last_desc" : "last");
			base.ViewBag.IsActiveParm = (model.SortOrder == "active" ? "active_desc" : "active");
			string sortOrder = model.SortOrder;
			switch (sortOrder)
			{
				case "active_desc":
				{
					narMemberViewModels = (
						from s in narMemberViewModels
						orderby s.IsActive descending
						select s).ToList<NarMemberViewModel>();
					break;
				}
				case "active":
				{
					narMemberViewModels = (
						from w in narMemberViewModels
						orderby w.IsActive
						select w).ToList<NarMemberViewModel>();
					break;
				}
				case "city_desc":
				{
					narMemberViewModels = (
						from s in narMemberViewModels
						orderby s.City descending
						select s).ToList<NarMemberViewModel>();
					break;
				}
				case "city":
				{
					narMemberViewModels = (
						from w in narMemberViewModels
						orderby w.City
						select w).ToList<NarMemberViewModel>();
					break;
				}
				case "state_desc":
				{
					narMemberViewModels = (
						from s in narMemberViewModels
						orderby s.State descending
						select s).ToList<NarMemberViewModel>();
					break;
				}
				case "state":
				{
					narMemberViewModels = (
						from w in narMemberViewModels
						orderby w.State
						select w).ToList<NarMemberViewModel>();
					break;
				}
				case "address_desc":
				{
					narMemberViewModels = (
						from s in narMemberViewModels
						orderby s.AddressLine1 descending
						select s).ToList<NarMemberViewModel>();
					break;
				}
				case "address":
				{
					narMemberViewModels = (
						from w in narMemberViewModels
						orderby w.AddressLine1
						select w).ToList<NarMemberViewModel>();
					break;
				}
				case "company_desc":
				{
					narMemberViewModels = (
						from s in narMemberViewModels
						orderby s.CompanyName descending
						select s).ToList<NarMemberViewModel>();
					break;
				}
				case "company":
				{
					narMemberViewModels = (
						from w in narMemberViewModels
						orderby w.CompanyName
						select w).ToList<NarMemberViewModel>();
					break;
				}
				case "email_desc":
				{
					narMemberViewModels = (
						from s in narMemberViewModels
						orderby s.Email descending
						select s).ToList<NarMemberViewModel>();
					break;
				}
				case "email":
				{
					narMemberViewModels = (
						from w in narMemberViewModels
						orderby w.Email
						select w).ToList<NarMemberViewModel>();
					break;
				}
				case "first_desc":
				{
					narMemberViewModels = (
						from s in narMemberViewModels
						orderby s.FirstName descending
						select s).ToList<NarMemberViewModel>();
					break;
				}
				case "first":
				{
					narMemberViewModels = (
						from w in narMemberViewModels
						orderby w.FirstName
						select w).ToList<NarMemberViewModel>();
					break;
				}
				case "last_desc":
				{
					narMemberViewModels = (
						from s in narMemberViewModels
						orderby s.LastName descending
						select s).ToList<NarMemberViewModel>();
					break;
				}
				default:
				{
					narMemberViewModels = (sortOrder == "last" ? (
						from w in narMemberViewModels
						orderby w.LastName
						select w).ToList<NarMemberViewModel>() : (
						from x in narMemberViewModels
						orderby x.FirstName
						select x).ToList<NarMemberViewModel>());
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
			model.Members = narMemberViewModels.ToPagedList<NarMemberViewModel>((rowCount.HasValue ? rowCount.GetValueOrDefault() : 1), num);
			return base.View(model);
		}

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetCreateAssetPartialData()
        {
            return new JsonResult()
            {
                Data = new { AssetTypeList = populateAssetTypeDDL(), PortfolioList = populatePortfolioListDDL().OrderBy(x => x.Text).ToList(), StateList = _user.PopulateStateList() }
            };
        }

		
		

		[Authorize]
		public ActionResult ManageAssetsOld(AdminAssetSearchResultsModel model, string sortOrder, int? page, string assetNumber = null)
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
			if (this.ValidSearch(model))
			{
				portfolioQuickListViewModels = this._asset.GetManageAssetQuickListPF(manageAssetsModel);
			}
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

		[Authorize]
		public ActionResult ManageICAdmins(UserSearchResultsModel model)
		{
			int value;
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			if (!base.ValidateAdminUser(userByUsername))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			List<UserType> userTypes = new List<UserType>()
			{
				UserType.ICAdmin
			};
			UserSearchModel userSearchModel = new UserSearchModel()
			{
				FirstName = model.FirstName,
				LastName = model.LastName,
				UserTypeFilter = model.SelectedUserType,
				ControllingUserType = userByUsername.UserType,
				City = model.City,
				State = model.State,
				ShowActiveOnly = false,
				UserTypeFilters = userTypes
			};
			List<UserQuickViewModel> users = this._user.GetUsers(userSearchModel);

            ((dynamic)base.ViewBag).CurrentSort = model.SortOrder;
			base.ViewBag.CitySortParm = (model.SortOrder == "city" ? "city_desc" : "city");
			base.ViewBag.StateSortParm = (model.SortOrder == "state" ? "state_desc" : "state");
			base.ViewBag.CompanyNameSortParm = (model.SortOrder == "company" ? "company_desc" : "company");
			base.ViewBag.AddressSortParm = (model.SortOrder == "address" ? "address_desc" : "address");
			base.ViewBag.EmailSortParm = (model.SortOrder == "email" ? "email_desc" : "email");
			base.ViewBag.FirstNameSortParm = (model.SortOrder == "first" ? "first_desc" : "first");
			base.ViewBag.LastNameSortParm = (model.SortOrder == "last" ? "last_desc" : "last");
			base.ViewBag.IsActiveParm = (model.SortOrder == "active" ? "active_desc" : "active");
			base.ViewBag.TypeSortParm = (model.SortOrder == "usertype" ? "usertype_desc" : "usertype");
			string sortOrder = model.SortOrder;
			switch (sortOrder)
			{
				case "firstname_desc":
				{
					users = (
						from s in users
						orderby s.FirstName descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "firstname":
				{
					users = (
						from s in users
						orderby s.FirstName
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "lastname_desc":
				{
					users = (
						from s in users
						orderby s.LastName descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "lastname":
				{
					users = (
						from s in users
						orderby s.LastName
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "usertype_desc":
				{
					users = (
						from s in users
						orderby s.UserTypeDescription descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "usertype":
				{
					users = (
						from s in users
						orderby s.UserTypeDescription
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "city_desc":
				{
					users = (
						from s in users
						orderby s.City descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "city":
				{
					users = (
						from w in users
						orderby w.City
						select w).ToList<UserQuickViewModel>();
					break;
				}
				case "state_desc":
				{
					users = (
						from s in users
						orderby s.State descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				default:
				{
					users = (sortOrder == "state" ? (
						from w in users
						orderby w.State
						select w).ToList<UserQuickViewModel>() : (
						from s in users
						orderby s.FirstName
						select s).ToList<UserQuickViewModel>());
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
			model.Users = users.ToPagedList<UserQuickViewModel>((rowCount.HasValue ? rowCount.GetValueOrDefault() : 1), num);
			return base.View(model);
		}

		[Authorize]
		public ActionResult ManageJVMAParticipants(UserSearchResultsModel model)
		{
			int value;
			if (!base.ValidateAdminUser(this._user.GetUserByUsername(base.User.Identity.Name)))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			UserSearchModel userSearchModel = new UserSearchModel()
			{
				FirstName = model.FirstName,
				LastName = model.LastName,
				CorpEntity = model.CorpEntity,
				RegistrationDateEnd = model.RegistrationDateEnd,
				RegistrationDateStart = model.RegistrationDateStart,
				ShowActiveOnly = model.ShowActiveOnly
			};
			List<UserQuickViewModel> jVMAParticipants = this._user.GetJVMAParticipants(userSearchModel);
			((dynamic)base.ViewBag).CurrentSort = model.SortOrder;
			base.ViewBag.FirstName = (model.SortOrder == "FirstName" ? "FirstName_desc" : "FirstName");
			base.ViewBag.LastName = (model.SortOrder == "LastName" ? "LastName_desc" : "LastName");
			base.ViewBag.CorpEntity = (model.SortOrder == "CorpEntity" ? "CorpEntity_desc" : "CorpEntity");
			base.ViewBag.UserType = (model.SortOrder == "UserType" ? "UserType_desc" : "UserType");
			base.ViewBag.RegisterDate = (model.SortOrder == "RegisterDate" ? "RegisterDate_desc" : "RegisterDate");
			base.ViewBag.ReferralDB = (model.SortOrder == "ReferralDB" ? "ReferralDB_desc" : "ReferralDB");
			base.ViewBag.PendingEscrows = (model.SortOrder == "PendingEscrows" ? "PendingEscrows_desc" : "PendingEscrows");
			string sortOrder = model.SortOrder;
			switch (sortOrder)
			{
				case "FirstName_desc":
				{
					jVMAParticipants = (
						from s in jVMAParticipants
						orderby s.FirstName descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "FirstName":
				{
					jVMAParticipants = (
						from s in jVMAParticipants
						orderby s.FirstName
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "LastName_desc":
				{
					jVMAParticipants = (
						from s in jVMAParticipants
						orderby s.LastName descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "LastName":
				{
					jVMAParticipants = (
						from s in jVMAParticipants
						orderby s.LastName
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "UserType_desc":
				{
					jVMAParticipants = (
						from s in jVMAParticipants
						orderby s.UserTypeString descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "UserType":
				{
					jVMAParticipants = (
						from s in jVMAParticipants
						orderby s.UserTypeString
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "CorpEntity_desc":
				{
					jVMAParticipants = (
						from s in jVMAParticipants
						orderby s.CompanyName descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "CorpEntity":
				{
					jVMAParticipants = (
						from w in jVMAParticipants
						orderby w.CompanyName
						select w).ToList<UserQuickViewModel>();
					break;
				}
				case "RegisterDate_desc":
				{
					jVMAParticipants = (
						from s in jVMAParticipants
						orderby s.RegisterDate descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "RegisterDate":
				{
					jVMAParticipants = (
						from w in jVMAParticipants
						orderby w.RegisterDate
						select w).ToList<UserQuickViewModel>();
					break;
				}
				case "ReferralDB_desc":
				{
					jVMAParticipants = (
						from s in jVMAParticipants
						orderby s.ReferralDB descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "ReferralDB":
				{
					jVMAParticipants = (
						from w in jVMAParticipants
						orderby w.ReferralDB
						select w).ToList<UserQuickViewModel>();
					break;
				}
				case "PendingEscrows_desc":
				{
					jVMAParticipants = (
						from s in jVMAParticipants
						orderby s.PendingEscrows descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				default:
				{
					jVMAParticipants = (sortOrder == "PendingEscrows" ? (
						from w in jVMAParticipants
						orderby w.PendingEscrows
						select w).ToList<UserQuickViewModel>() : (
						from s in jVMAParticipants
						orderby s.FirstName
						select s).ToList<UserQuickViewModel>());
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
			model.Users = jVMAParticipants.ToPagedList<UserQuickViewModel>((rowCount.HasValue ? rowCount.GetValueOrDefault() : 1), num);
			return base.View(model);
		}

		[Authorize]
		public ActionResult ManageMBAMembersImported(MbaSearchResultsModel model)
		{
			int value;
			if (!base.ValidateAdminUser(this._user.GetUserByUsername(base.User.Identity.Name)))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			MbaSearchModel mbaSearchModel = new MbaSearchModel()
			{
				AddressLine1 = model.AddressLine1,
				City = model.City,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				State = model.State,
				Zip = model.Zip,
				ShowActiveOnly = model.ShowActiveOnly,
				CompanyName = model.CompanyName
			};
			List<MbaViewModel> mbas = this._user.GetMbas(mbaSearchModel);
			((dynamic)base.ViewBag).CurrentSort = model.SortOrder;
			base.ViewBag.CitySortParm = (model.SortOrder == "city" ? "city_desc" : "city");
			base.ViewBag.StateSortParm = (model.SortOrder == "state" ? "state_desc" : "state");
			base.ViewBag.CompanyNameSortParm = (model.SortOrder == "company" ? "company_desc" : "company");
			base.ViewBag.AddressSortParm = (model.SortOrder == "address" ? "address_desc" : "address");
			base.ViewBag.EmailSortParm = (model.SortOrder == "email" ? "email_desc" : "email");
			base.ViewBag.FirstNameSortParm = (model.SortOrder == "first" ? "first_desc" : "first");
			base.ViewBag.LastNameSortParm = (model.SortOrder == "last" ? "last_desc" : "last");
			base.ViewBag.IsActiveParm = (model.SortOrder == "active" ? "active_desc" : "active");
			base.ViewBag.TypeSortParm = (model.SortOrder == "usertype" ? "usertype_desc" : "usertype");
			string sortOrder = model.SortOrder;
			switch (sortOrder)
			{
				case "active_desc":
				{
					mbas = (
						from s in mbas
						orderby s.IsActive descending
						select s).ToList<MbaViewModel>();
					break;
				}
				case "active":
				{
					mbas = (
						from w in mbas
						orderby w.IsActive
						select w).ToList<MbaViewModel>();
					break;
				}
				case "city_desc":
				{
					mbas = (
						from s in mbas
						orderby s.City descending
						select s).ToList<MbaViewModel>();
					break;
				}
				case "city":
				{
					mbas = (
						from w in mbas
						orderby w.City
						select w).ToList<MbaViewModel>();
					break;
				}
				case "state_desc":
				{
					mbas = (
						from s in mbas
						orderby s.State descending
						select s).ToList<MbaViewModel>();
					break;
				}
				case "state":
				{
					mbas = (
						from w in mbas
						orderby w.State
						select w).ToList<MbaViewModel>();
					break;
				}
				case "address_desc":
				{
					mbas = (
						from s in mbas
						orderby s.AddressLine1 descending
						select s).ToList<MbaViewModel>();
					break;
				}
				case "address":
				{
					mbas = (
						from w in mbas
						orderby w.AddressLine1
						select w).ToList<MbaViewModel>();
					break;
				}
				case "company_desc":
				{
					mbas = (
						from s in mbas
						orderby s.CompanyName descending
						select s).ToList<MbaViewModel>();
					break;
				}
				case "company":
				{
					mbas = (
						from w in mbas
						orderby w.CompanyName
						select w).ToList<MbaViewModel>();
					break;
				}
				case "email_desc":
				{
					mbas = (
						from s in mbas
						orderby s.Email descending
						select s).ToList<MbaViewModel>();
					break;
				}
				case "email":
				{
					mbas = (
						from w in mbas
						orderby w.Email
						select w).ToList<MbaViewModel>();
					break;
				}
				case "first_desc":
				{
					mbas = (
						from s in mbas
						orderby s.FirstName descending
						select s).ToList<MbaViewModel>();
					break;
				}
				case "first":
				{
					mbas = (
						from w in mbas
						orderby w.FirstName
						select w).ToList<MbaViewModel>();
					break;
				}
				case "last_desc":
				{
					mbas = (
						from s in mbas
						orderby s.LastName descending
						select s).ToList<MbaViewModel>();
					break;
				}
				default:
				{
					mbas = (sortOrder == "last" ? (
						from w in mbas
						orderby w.LastName
						select w).ToList<MbaViewModel>() : (
						from x in mbas
						orderby x.FirstName
						select x).ToList<MbaViewModel>());
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
			model.Members = mbas.ToPagedList<MbaViewModel>((rowCount.HasValue ? rowCount.GetValueOrDefault() : 1), num);
			return base.View(model);
		}

		[Authorize]
		public ActionResult ManageMBAMembersReg(UserSearchResultsModel model)
		{
			int value;
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			if (!base.ValidateAdminUser(userByUsername))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			List<UserType> userTypes = new List<UserType>()
			{
				UserType.CREBroker,
				UserType.CRELender
			};
			UserSearchModel userSearchModel = new UserSearchModel()
			{
				FirstName = model.FirstName,
				LastName = model.LastName,
				UserTypeFilter = model.SelectedUserType,
				ControllingUserType = userByUsername.UserType,
				City = model.City,
				State = model.State,
				ShowActiveOnly = model.ShowActiveOnly,
				Email = model.Email,
				AddressLine1 = model.AddressLine1,
				UserTypeFilters = userTypes
			};
			List<UserQuickViewModel> users = this._user.GetUsers(userSearchModel);
			((dynamic)base.ViewBag).CurrentSort = model.SortOrder;
			base.ViewBag.CitySortParm = (model.SortOrder == "city" ? "city_desc" : "city");
			base.ViewBag.StateSortParm = (model.SortOrder == "state" ? "state_desc" : "state");
			base.ViewBag.CompanyNameSortParm = (model.SortOrder == "company" ? "company_desc" : "company");
			base.ViewBag.AddressSortParm = (model.SortOrder == "address" ? "address_desc" : "address");
			base.ViewBag.EmailSortParm = (model.SortOrder == "email" ? "email_desc" : "email");
			base.ViewBag.FirstNameSortParm = (model.SortOrder == "first" ? "first_desc" : "first");
			base.ViewBag.LastNameSortParm = (model.SortOrder == "last" ? "last_desc" : "last");
			base.ViewBag.IsActiveParm = (model.SortOrder == "active" ? "active_desc" : "active");
			base.ViewBag.TypeSortParm = (model.SortOrder == "usertype" ? "usertype_desc" : "usertype");
			string sortOrder = model.SortOrder;
			switch (sortOrder)
			{
				case "firstname_desc":
				{
					users = (
						from s in users
						orderby s.FirstName descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "firstname":
				{
					users = (
						from s in users
						orderby s.FirstName
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "lastname_desc":
				{
					users = (
						from s in users
						orderby s.LastName descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "lastname":
				{
					users = (
						from s in users
						orderby s.LastName
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "company_desc":
				{
					users = (
						from s in users
						orderby s.CompanyName descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "company":
				{
					users = (
						from w in users
						orderby w.CompanyName
						select w).ToList<UserQuickViewModel>();
					break;
				}
				case "email_desc":
				{
					users = (
						from s in users
						orderby s.Email descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "email":
				{
					users = (
						from w in users
						orderby w.Email
						select w).ToList<UserQuickViewModel>();
					break;
				}
				case "address_desc":
				{
					users = (
						from s in users
						orderby s.AddressLine1 descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "address":
				{
					users = (
						from w in users
						orderby w.AddressLine1
						select w).ToList<UserQuickViewModel>();
					break;
				}
				case "city_desc":
				{
					users = (
						from s in users
						orderby s.City descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "city":
				{
					users = (
						from w in users
						orderby w.City
						select w).ToList<UserQuickViewModel>();
					break;
				}
				case "state_desc":
				{
					users = (
						from s in users
						orderby s.State descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "state":
				{
					users = (
						from w in users
						orderby w.State
						select w).ToList<UserQuickViewModel>();
					break;
				}
				case "active_desc":
				{
					users = (
						from s in users
						orderby s.IsActive descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				default:
				{
					users = (sortOrder == "active" ? (
						from w in users
						orderby w.IsActive
						select w).ToList<UserQuickViewModel>() : (
						from s in users
						orderby s.FirstName
						select s).ToList<UserQuickViewModel>());
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
			model.Users = users.ToPagedList<UserQuickViewModel>((rowCount.HasValue ? rowCount.GetValueOrDefault() : 1), num);
			return base.View(model);
		}

		[Authorize]
		public ActionResult ManageMBAUsers(UserSearchResultsModel model, string sortOrder, int? page)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			if (!base.ValidateAdminUser(userByUsername))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			model.ControllingUserType = userByUsername.UserType;
			int num = (base.TempData["RowCount"] != null ? Convert.ToInt32(base.TempData["RowCount"]) : 50);
			base.TempData.Keep("RowCount");
			int? nullable = page;
			int num1 = (nullable.HasValue ? nullable.GetValueOrDefault() : 1);
			List<MbaViewModel> mbaViewModels = new List<MbaViewModel>();
			List<UserType> userTypes = new List<UserType>()
			{
				UserType.CREBroker,
				UserType.CRELender
			};
			MbaSearchModel mbaSearchModel = new MbaSearchModel()
			{
				AddressLine1 = model.AddressLine1,
				City = model.City,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				State = model.State,
				Zip = model.Zip,
				ShowActiveOnly = model.ShowActiveOnly,
				CompanyName = model.CompanyName
			};
			UserSearchModel userSearchModel = new UserSearchModel()
			{
				FirstName = model.FirstName,
				LastName = model.LastName,
				UserTypeFilter = model.SelectedUserType,
				ControllingUserType = userByUsername.UserType,
				City = model.City,
				State = model.State,
				ShowActiveOnly = false,
				UserTypeFilters = userTypes
			};
			((dynamic)base.ViewBag).CurrentSort = sortOrder;
			base.ViewBag.CitySortParm = (sortOrder == "city" ? "city_desc" : "city");
			base.ViewBag.StateSortParm = (sortOrder == "state" ? "state_desc" : "state");
			base.ViewBag.CompanyNameSortParm = (sortOrder == "company" ? "company_desc" : "company");
			base.ViewBag.AddressSortParm = (sortOrder == "address" ? "address_desc" : "address");
			base.ViewBag.EmailSortParm = (sortOrder == "email" ? "email_desc" : "email");
			base.ViewBag.FirstNameSortParm = (sortOrder == "first" ? "first_desc" : "first");
			base.ViewBag.LastNameSortParm = (sortOrder == "last" ? "last_desc" : "last");
			base.ViewBag.IsActiveParm = (sortOrder == "active" ? "active_desc" : "active");
			base.ViewBag.TypeSortParm = (sortOrder == "usertype" ? "usertype_desc" : "usertype");
			mbaViewModels = this._user.GetMbas(mbaSearchModel);
			List<UserQuickViewModel> users = this._user.GetUsers(userSearchModel);
			switch (sortOrder)
			{
				case "active_desc":
				{
					mbaViewModels = (
						from s in mbaViewModels
						orderby s.IsActive descending
						select s).ToList<MbaViewModel>();
					break;
				}
				case "active":
				{
					mbaViewModels = (
						from w in mbaViewModels
						orderby w.IsActive
						select w).ToList<MbaViewModel>();
					break;
				}
				case "city_desc":
				{
					mbaViewModels = (
						from s in mbaViewModels
						orderby s.City descending
						select s).ToList<MbaViewModel>();
					break;
				}
				case "city":
				{
					mbaViewModels = (
						from w in mbaViewModels
						orderby w.City
						select w).ToList<MbaViewModel>();
					break;
				}
				case "state_desc":
				{
					mbaViewModels = (
						from s in mbaViewModels
						orderby s.State descending
						select s).ToList<MbaViewModel>();
					break;
				}
				case "state":
				{
					mbaViewModels = (
						from w in mbaViewModels
						orderby w.State
						select w).ToList<MbaViewModel>();
					break;
				}
				case "address_desc":
				{
					mbaViewModels = (
						from s in mbaViewModels
						orderby s.AddressLine1 descending
						select s).ToList<MbaViewModel>();
					break;
				}
				case "address":
				{
					mbaViewModels = (
						from w in mbaViewModels
						orderby w.AddressLine1
						select w).ToList<MbaViewModel>();
					break;
				}
				case "company_desc":
				{
					mbaViewModels = (
						from s in mbaViewModels
						orderby s.CompanyName descending
						select s).ToList<MbaViewModel>();
					break;
				}
				case "company":
				{
					mbaViewModels = (
						from w in mbaViewModels
						orderby w.CompanyName
						select w).ToList<MbaViewModel>();
					break;
				}
				case "email_desc":
				{
					mbaViewModels = (
						from s in mbaViewModels
						orderby s.Email descending
						select s).ToList<MbaViewModel>();
					break;
				}
				case "email":
				{
					mbaViewModels = (
						from w in mbaViewModels
						orderby w.Email
						select w).ToList<MbaViewModel>();
					break;
				}
				case "first_desc":
				{
					mbaViewModels = (
						from s in mbaViewModels
						orderby s.FirstName descending
						select s).ToList<MbaViewModel>();
					break;
				}
				case "first":
				{
					mbaViewModels = (
						from w in mbaViewModels
						orderby w.FirstName
						select w).ToList<MbaViewModel>();
					break;
				}
				case "last_desc":
				{
					mbaViewModels = (
						from s in mbaViewModels
						orderby s.LastName descending
						select s).ToList<MbaViewModel>();
					break;
				}
				default:
				{
					mbaViewModels = (sortOrder == "last" ? (
						from w in mbaViewModels
						orderby w.LastName
						select w).ToList<MbaViewModel>() : (
						from x in mbaViewModels
						orderby x.FirstName
						select x).ToList<MbaViewModel>());
					break;
				}
			}
			switch (sortOrder)
			{
				case "firstname_desc":
				{
					model.Users = (
						from s in users
						orderby s.FirstName descending
						select s).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				case "firstname":
				{
					model.Users = (
						from s in users
						orderby s.FirstName
						select s).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				case "lastname_desc":
				{
					model.Users = (
						from s in users
						orderby s.LastName descending
						select s).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				case "lastname":
				{
					model.Users = (
						from s in users
						orderby s.LastName
						select s).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				case "usertype_desc":
				{
					model.Users = (
						from s in users
						orderby s.UserTypeDescription descending
						select s).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				case "usertype":
				{
					model.Users = (
						from s in users
						orderby s.UserTypeDescription
						select s).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				case "city_desc":
				{
					model.Users = (
						from s in users
						orderby s.City descending
						select s).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				case "city":
				{
					model.Users = (
						from w in users
						orderby w.City
						select w).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				case "state_desc":
				{
					model.Users = (
						from s in users
						orderby s.State descending
						select s).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				case "state":
				{
					model.Users = (
						from w in users
						orderby w.State
						select w).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				default:
				{
					model.Users = (
						from s in users
						orderby s.FirstName
						select s).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
			}
			model.Members = mbaViewModels.ToPagedList<MbaViewModel>(num1, num);
			return base.View(model);
		}

		[Authorize]
		public ActionResult ManageNarMembersImported(NarMemberSearchResultsModel model)
		{
			int value;
			if (!base.ValidateAdminUser(this._user.GetUserByUsername(base.User.Identity.Name)))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			List<NarMemberViewModel> narMemberViewModels = new List<NarMemberViewModel>();
			NarMemberSearchModel narMemberSearchModel = new NarMemberSearchModel()
			{
				AddressLine1 = model.AddressLine1,
				City = model.City,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				State = model.State,
				Zip = model.Zip,
				ShowActiveOnly = model.ShowActiveOnly,
				CompanyName = model.CompanyName
			};
			narMemberViewModels = this._user.GetNarMembersImported(narMemberSearchModel);
			((dynamic)base.ViewBag).CurrentSort = model.SortOrder;
			base.ViewBag.CitySortParm = (model.SortOrder == "city" ? "city_desc" : "city");
			base.ViewBag.StateSortParm = (model.SortOrder == "state" ? "state_desc" : "state");
			base.ViewBag.CompanyNameSortParm = (model.SortOrder == "company" ? "company_desc" : "company");
			base.ViewBag.AddressSortParm = (model.SortOrder == "address" ? "address_desc" : "address");
			base.ViewBag.EmailSortParm = (model.SortOrder == "email" ? "email_desc" : "email");
			base.ViewBag.FirstNameSortParm = (model.SortOrder == "first" ? "first_desc" : "first");
			base.ViewBag.LastNameSortParm = (model.SortOrder == "last" ? "last_desc" : "last");
			base.ViewBag.IsActiveParm = (model.SortOrder == "active" ? "active_desc" : "active");
			string sortOrder = model.SortOrder;
			switch (sortOrder)
			{
				case "active_desc":
				{
					narMemberViewModels = (
						from s in narMemberViewModels
						orderby s.IsActive descending
						select s).ToList<NarMemberViewModel>();
					break;
				}
				case "active":
				{
					narMemberViewModels = (
						from w in narMemberViewModels
						orderby w.IsActive
						select w).ToList<NarMemberViewModel>();
					break;
				}
				case "city_desc":
				{
					narMemberViewModels = (
						from s in narMemberViewModels
						orderby s.City descending
						select s).ToList<NarMemberViewModel>();
					break;
				}
				case "city":
				{
					narMemberViewModels = (
						from w in narMemberViewModels
						orderby w.City
						select w).ToList<NarMemberViewModel>();
					break;
				}
				case "state_desc":
				{
					narMemberViewModels = (
						from s in narMemberViewModels
						orderby s.State descending
						select s).ToList<NarMemberViewModel>();
					break;
				}
				case "state":
				{
					narMemberViewModels = (
						from w in narMemberViewModels
						orderby w.State
						select w).ToList<NarMemberViewModel>();
					break;
				}
				case "address_desc":
				{
					narMemberViewModels = (
						from s in narMemberViewModels
						orderby s.AddressLine1 descending
						select s).ToList<NarMemberViewModel>();
					break;
				}
				case "address":
				{
					narMemberViewModels = (
						from w in narMemberViewModels
						orderby w.AddressLine1
						select w).ToList<NarMemberViewModel>();
					break;
				}
				case "company_desc":
				{
					narMemberViewModels = (
						from s in narMemberViewModels
						orderby s.CompanyName descending
						select s).ToList<NarMemberViewModel>();
					break;
				}
				case "company":
				{
					narMemberViewModels = (
						from w in narMemberViewModels
						orderby w.CompanyName
						select w).ToList<NarMemberViewModel>();
					break;
				}
				case "email_desc":
				{
					narMemberViewModels = (
						from s in narMemberViewModels
						orderby s.Email descending
						select s).ToList<NarMemberViewModel>();
					break;
				}
				case "email":
				{
					narMemberViewModels = (
						from w in narMemberViewModels
						orderby w.Email
						select w).ToList<NarMemberViewModel>();
					break;
				}
				case "first_desc":
				{
					narMemberViewModels = (
						from s in narMemberViewModels
						orderby s.FirstName descending
						select s).ToList<NarMemberViewModel>();
					break;
				}
				case "first":
				{
					narMemberViewModels = (
						from w in narMemberViewModels
						orderby w.FirstName
						select w).ToList<NarMemberViewModel>();
					break;
				}
				case "last_desc":
				{
					narMemberViewModels = (
						from s in narMemberViewModels
						orderby s.LastName descending
						select s).ToList<NarMemberViewModel>();
					break;
				}
				default:
				{
					narMemberViewModels = (sortOrder == "last" ? (
						from w in narMemberViewModels
						orderby w.LastName
						select w).ToList<NarMemberViewModel>() : (
						from x in narMemberViewModels
						orderby x.FirstName
						select x).ToList<NarMemberViewModel>());
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
			model.Members = narMemberViewModels.ToPagedList<NarMemberViewModel>((rowCount.HasValue ? rowCount.GetValueOrDefault() : 1), num);
			return base.View(model);
		}

		[Authorize]
		public ActionResult ManageNarMembersRegistered(UserSearchResultsModel model)
		{
			int value;
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			if (!base.ValidateAdminUser(userByUsername))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			model.ControllingUserType = userByUsername.UserType;
			List<UserType> userTypes = new List<UserType>()
			{
				UserType.ListingAgent
			};
			UserSearchModel userSearchModel = new UserSearchModel()
			{
				FirstName = model.FirstName,
				LastName = model.LastName,
				UserTypeFilter = model.SelectedUserType,
				ControllingUserType = userByUsername.UserType,
				City = model.City,
				State = model.State,
				ShowActiveOnly = false,
				UserTypeFilters = userTypes
			};
			List<UserQuickViewModel> users = this._user.GetUsers(userSearchModel);
			((dynamic)base.ViewBag).CurrentSort = model.SortOrder;
			base.ViewBag.CitySortParm = (model.SortOrder == "city" ? "city_desc" : "city");
			base.ViewBag.StateSortParm = (model.SortOrder == "state" ? "state_desc" : "state");
			base.ViewBag.CompanyNameSortParm = (model.SortOrder == "company" ? "company_desc" : "company");
			base.ViewBag.AddressSortParm = (model.SortOrder == "address" ? "address_desc" : "address");
			base.ViewBag.EmailSortParm = (model.SortOrder == "email" ? "email_desc" : "email");
			base.ViewBag.FirstNameSortParm = (model.SortOrder == "first" ? "first_desc" : "first");
			base.ViewBag.LastNameSortParm = (model.SortOrder == "last" ? "last_desc" : "last");
			base.ViewBag.IsActiveParm = (model.SortOrder == "active" ? "active_desc" : "active");
			base.ViewBag.TypeSortParm = (model.SortOrder == "usertype" ? "usertype_desc" : "usertype");
			string sortOrder = model.SortOrder;
			switch (sortOrder)
			{
				case "firstname_desc":
				{
					users = (
						from s in users
						orderby s.FirstName descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "firstname":
				{
					users = (
						from s in users
						orderby s.FirstName
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "lastname_desc":
				{
					users = (
						from s in users
						orderby s.LastName descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "lastname":
				{
					users = (
						from s in users
						orderby s.LastName
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "usertype_desc":
				{
					users = (
						from s in users
						orderby s.UserTypeDescription descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "usertype":
				{
					users = (
						from s in users
						orderby s.UserTypeDescription
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "city_desc":
				{
					users = (
						from s in users
						orderby s.City descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "city":
				{
					users = (
						from w in users
						orderby w.City
						select w).ToList<UserQuickViewModel>();
					break;
				}
				case "state_desc":
				{
					users = (
						from s in users
						orderby s.State descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				default:
				{
					users = (sortOrder == "state" ? (
						from w in users
						orderby w.State
						select w).ToList<UserQuickViewModel>() : (
						from s in users
						orderby s.FirstName
						select s).ToList<UserQuickViewModel>());
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
			model.Users = users.ToPagedList<UserQuickViewModel>((rowCount.HasValue ? rowCount.GetValueOrDefault() : 1), num);
			return base.View(model);
		}

		[Authorize]
		public ActionResult ManagePrincipalInvestorsImport(PrincipalInvestorSearchResultsModel model)
		{
			int value;
			if (!base.ValidateAdminUser(this._user.GetUserByUsername(base.User.Identity.Name)))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			PrincipalInvestorSearchModel principalInvestorSearchModel = new PrincipalInvestorSearchModel()
			{
				AddressLine1 = model.AddressLine1,
				City = model.City,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				State = model.State,
				Zip = model.Zip,
				ShowActiveOnly = model.ShowActiveOnly,
				CompanyName = model.CompanyName
			};
			((dynamic)base.ViewBag).CurrentSort = model.SortOrder;
			base.ViewBag.CitySortParm = (model.SortOrder == "city" ? "city_desc" : "city");
			base.ViewBag.StateSortParm = (model.SortOrder == "state" ? "state_desc" : "state");
			base.ViewBag.CompanyNameSortParm = (model.SortOrder == "company" ? "company_desc" : "company");
			base.ViewBag.AddressSortParm = (model.SortOrder == "address" ? "address_desc" : "address");
			base.ViewBag.EmailSortParm = (model.SortOrder == "email" ? "email_desc" : "email");
			base.ViewBag.FirstNameSortParm = (model.SortOrder == "first" ? "first_desc" : "first");
			base.ViewBag.LastNameSortParm = (model.SortOrder == "last" ? "last_desc" : "last");
			base.ViewBag.IsActiveParm = (model.SortOrder == "active" ? "active_desc" : "active");
			List<PrincipalInvestorQuickViewModel> list = this._user.GetPrincipalInvestors(principalInvestorSearchModel).ToList<PrincipalInvestorQuickViewModel>();
			string sortOrder = model.SortOrder;
			switch (sortOrder)
			{
				case "active_desc":
				{
					list = (
						from s in list
						orderby s.IsActive descending
						select s).ToList<PrincipalInvestorQuickViewModel>();
					break;
				}
				case "active":
				{
					list = (
						from w in list
						orderby w.IsActive
						select w).ToList<PrincipalInvestorQuickViewModel>();
					break;
				}
				case "city_desc":
				{
					list = (
						from s in list
						orderby s.City descending
						select s).ToList<PrincipalInvestorQuickViewModel>();
					break;
				}
				case "city":
				{
					list = (
						from w in list
						orderby w.City
						select w).ToList<PrincipalInvestorQuickViewModel>();
					break;
				}
				case "state_desc":
				{
					list = (
						from s in list
						orderby s.State descending
						select s).ToList<PrincipalInvestorQuickViewModel>();
					break;
				}
				case "state":
				{
					list = (
						from w in list
						orderby w.State
						select w).ToList<PrincipalInvestorQuickViewModel>();
					break;
				}
				case "address_desc":
				{
					list = (
						from s in list
						orderby s.AddressLine1 descending
						select s).ToList<PrincipalInvestorQuickViewModel>();
					break;
				}
				case "address":
				{
					list = (
						from w in list
						orderby w.AddressLine1
						select w).ToList<PrincipalInvestorQuickViewModel>();
					break;
				}
				case "company_desc":
				{
					list = (
						from s in list
						orderby s.CompanyName descending
						select s).ToList<PrincipalInvestorQuickViewModel>();
					break;
				}
				case "company":
				{
					list = (
						from w in list
						orderby w.CompanyName
						select w).ToList<PrincipalInvestorQuickViewModel>();
					break;
				}
				case "email_desc":
				{
					list = (
						from s in list
						orderby s.Email descending
						select s).ToList<PrincipalInvestorQuickViewModel>();
					break;
				}
				case "email":
				{
					list = (
						from w in list
						orderby w.Email
						select w).ToList<PrincipalInvestorQuickViewModel>();
					break;
				}
				case "first_desc":
				{
					list = (
						from s in list
						orderby s.FirstName descending
						select s).ToList<PrincipalInvestorQuickViewModel>();
					break;
				}
				case "first":
				{
					list = (
						from w in list
						orderby w.FirstName
						select w).ToList<PrincipalInvestorQuickViewModel>();
					break;
				}
				case "last_desc":
				{
					list = (
						from s in list
						orderby s.LastName descending
						select s).ToList<PrincipalInvestorQuickViewModel>();
					break;
				}
				default:
				{
					list = (sortOrder == "last" ? (
						from w in list
						orderby w.LastName
						select w).ToList<PrincipalInvestorQuickViewModel>() : (
						from x in list
						orderby x.FirstName
						select x).ToList<PrincipalInvestorQuickViewModel>());
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
			model.Investors = list.ToPagedList<PrincipalInvestorQuickViewModel>((rowCount.HasValue ? rowCount.GetValueOrDefault() : 1), num);
			return base.View(model);
		}

		[Authorize]
		public ActionResult ManagePrincipalInvestorsReg(UserSearchResultsModel model)
		{
			int value;
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			if (!base.ValidateAdminUser(userByUsername))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			model.ControllingUserType = userByUsername.UserType;
			List<UserType> userTypes = new List<UserType>()
			{
				UserType.Investor
			};
			UserSearchModel userSearchModel = new UserSearchModel()
			{
				FirstName = model.FirstName,
				LastName = model.LastName,
				UserTypeFilter = model.SelectedUserType,
				ControllingUserType = userByUsername.UserType,
				City = model.City,
				State = model.State,
				ShowActiveOnly = false,
				UserTypeFilters = userTypes
			};
			List<UserQuickViewModel> users = this._user.GetUsers(userSearchModel);
			((dynamic)base.ViewBag).CurrentSort = model.SortOrder;
			base.ViewBag.CitySortParm = (model.SortOrder == "city" ? "city_desc" : "city");
			base.ViewBag.StateSortParm = (model.SortOrder == "state" ? "state_desc" : "state");
			base.ViewBag.CompanyNameSortParm = (model.SortOrder == "company" ? "company_desc" : "company");
			base.ViewBag.AddressSortParm = (model.SortOrder == "address" ? "address_desc" : "address");
			base.ViewBag.EmailSortParm = (model.SortOrder == "email" ? "email_desc" : "email");
			base.ViewBag.FirstNameSortParm = (model.SortOrder == "first" ? "first_desc" : "first");
			base.ViewBag.LastNameSortParm = (model.SortOrder == "last" ? "last_desc" : "last");
			base.ViewBag.IsActiveParm = (model.SortOrder == "active" ? "active_desc" : "active");
			base.ViewBag.TypeSortParm = (model.SortOrder == "usertype" ? "usertype_desc" : "usertype");
			string sortOrder = model.SortOrder;
			switch (sortOrder)
			{
				case "firstname_desc":
				{
					users = (
						from s in users
						orderby s.FirstName descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "firstname":
				{
					users = (
						from s in users
						orderby s.FirstName
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "lastname_desc":
				{
					users = (
						from s in users
						orderby s.LastName descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "lastname":
				{
					users = (
						from s in users
						orderby s.LastName
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "usertype_desc":
				{
					users = (
						from s in users
						orderby s.UserTypeDescription descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "usertype":
				{
					users = (
						from s in users
						orderby s.UserTypeDescription
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "city_desc":
				{
					users = (
						from s in users
						orderby s.City descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				case "city":
				{
					users = (
						from w in users
						orderby w.City
						select w).ToList<UserQuickViewModel>();
					break;
				}
				case "state_desc":
				{
					users = (
						from s in users
						orderby s.State descending
						select s).ToList<UserQuickViewModel>();
					break;
				}
				default:
				{
					users = (sortOrder == "state" ? (
						from w in users
						orderby w.State
						select w).ToList<UserQuickViewModel>() : (
						from s in users
						orderby s.FirstName
						select s).ToList<UserQuickViewModel>());
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
			model.Users = users.ToPagedList<UserQuickViewModel>((rowCount.HasValue ? rowCount.GetValueOrDefault() : 1), num);
			return base.View(model);
		}


      

        [Authorize]
		public ActionResult ManagerTitleUsersPage(int id, TitleUserSearchResultsModel model, string sortOrder, int? page)
		{
			if (model == null)
			{
				model = new TitleUserSearchResultsModel();
			}
			if (id != 0)
			{
				model.TitleCompanyId = id;
			}
			int num = (base.TempData["RowCount"] != null ? Convert.ToInt32(base.TempData["RowCount"]) : 50);
			base.TempData.Keep("RowCount");
			int? nullable = page;
			int num1 = (nullable.HasValue ? nullable.GetValueOrDefault() : 1);
			TitleUserSearchModel titleUserSearchModel = new TitleUserSearchModel()
			{
				Email = model.Email,
				FirstName = model.FirstName,
				IsActive = model.IsActive,
				IsManager = model.IsManager,
				LastName = model.LastName,
				ManagingOfficerName = model.ManagingOfficerName,
				Password = model.Password,
				PhoneNumber = model.PhoneNumber,
				TitleCompanyId = new int?(id)
			};
			((dynamic)base.ViewBag).CurrentSort = sortOrder;
			base.ViewBag.FirstNameSortParm = (string.IsNullOrEmpty(sortOrder) ? "firstname_desc" : "");
			base.ViewBag.LastNameSortParm = (string.IsNullOrEmpty(sortOrder) ? "lastname_desc" : "");
			((dynamic)base.ViewBag).Email = this._user.GetTitleCompanyUsers().FirstOrDefault<TitleCompanyUser>((TitleCompanyUser x) => {
				if (!x.IsManager)
				{
					return false;
				}
				return x.TitleCompanyId == id;
			}).Email;
			List<TitleUserQuickViewModel> titleCompanyUsers = this._user.GetTitleCompanyUsers(titleUserSearchModel);
			if (sortOrder == "firstname_desc")
			{
				model.TitleUsers = (
					from s in titleCompanyUsers
					orderby s.FirstName descending
					select s).ToPagedList<TitleUserQuickViewModel>(num1, num);
			}
			else if (sortOrder == "firstname")
			{
				model.TitleUsers = (
					from s in titleCompanyUsers
					orderby s.FirstName
					select s).ToPagedList<TitleUserQuickViewModel>(num1, num);
			}
			else if (sortOrder == "lastname_desc")
			{
				model.TitleUsers = (
					from s in titleCompanyUsers
					orderby s.LastName descending
					select s).ToPagedList<TitleUserQuickViewModel>(num1, num);
			}
			else if (sortOrder == "lastname")
			{
				model.TitleUsers = (
					from w in titleCompanyUsers
					orderby w.LastName
					select w).ToPagedList<TitleUserQuickViewModel>(num1, num);
			}
			else
			{
				model.TitleUsers = (
					from x in titleCompanyUsers
					orderby x.FirstName
					select x).ToPagedList<TitleUserQuickViewModel>(num1, num);
			}
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			model.ControllingUserType = userByUsername.UserType;
			return base.View("ManagerTitleUsersPage", model);
		}

		[Authorize]
		public ActionResult ManageTitleCompanies(TitleSearchResultsModel model, string sortOrder, int? page)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			model.ControllingUserType = userByUsername.UserType;
			if (!base.ValidateAdminUser(userByUsername))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			int num = (base.TempData["RowCount"] != null ? Convert.ToInt32(base.TempData["RowCount"]) : 50);
			base.TempData.Keep("RowCount");
			int? nullable = page;
			int num1 = (nullable.HasValue ? nullable.GetValueOrDefault() : 1);
			CompanySearchModel companySearchModel = new CompanySearchModel()
			{
				CompanyName = model.TitleCompName,
				CompanyURL = model.TitleCompURL,
				State = model.State
			};
			((dynamic)base.ViewBag).CurrentSort = sortOrder;
			base.ViewBag.TitleNameSortParm = (string.IsNullOrEmpty(sortOrder) ? "titlename_desc" : "");
			base.ViewBag.StateSortParm = (sortOrder == "state" ? "state_desc" : "state");
			List<TitleQuickViewModel> titleCompanies = this._user.GetTitleCompanies(companySearchModel);
			if (sortOrder == "titlename_desc")
			{
				model.Titles = (
					from s in titleCompanies
					orderby s.TitleCompName descending
					select s).ToPagedList<TitleQuickViewModel>(num1, num);
			}
			else if (sortOrder == "titlename")
			{
				model.Titles = (
					from s in titleCompanies
					orderby s.TitleCompName
					select s).ToPagedList<TitleQuickViewModel>(num1, num);
			}
			else if (sortOrder == "state_desc")
			{
				model.Titles = (
					from s in titleCompanies
					orderby s.State descending
					select s).ToPagedList<TitleQuickViewModel>(num1, num);
			}
			else if (sortOrder == "state")
			{
				model.Titles = (
					from w in titleCompanies
					orderby w.State
					select w).ToPagedList<TitleQuickViewModel>(num1, num);
			}
			else
			{
				model.Titles = (
					from x in titleCompanies
					orderby x.TitleCompName
					select x).ToPagedList<TitleQuickViewModel>(num1, num);
			}
			return base.View(model);
		}

		[Authorize]
		public ActionResult ManageTitleCompanyUsers(int id, TitleUserSearchResultsModel model, string sortOrder, int? page)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			if (model == null)
			{
				model = new TitleUserSearchResultsModel();
			}
			if (id != 0)
			{
				model.TitleCompanyId = id;
			}
			if (userByUsername != null)
			{
				model.ControllingUserType = userByUsername.UserType;
				if (userByUsername.UserType != UserType.CorpAdmin && userByUsername.UserType != UserType.SiteAdmin)
				{
					return base.RedirectToAction("MyUSCPage", "Home");
				}
			}
			int num = (base.TempData["RowCount"] != null ? Convert.ToInt32(base.TempData["RowCount"]) : 50);
			base.TempData.Keep("RowCount");
			int? nullable = page;
			int num1 = (nullable.HasValue ? nullable.GetValueOrDefault() : 1);
			TitleUserSearchModel titleUserSearchModel = new TitleUserSearchModel()
			{
				Email = model.Email,
				FirstName = model.FirstName,
				IsActive = model.IsActive,
				IsManager = model.IsManager,
				LastName = model.LastName,
				ManagingOfficerName = model.ManagingOfficerName,
				Password = model.Password,
				PhoneNumber = model.PhoneNumber,
				TitleCompanyId = new int?(id)
			};
			((dynamic)base.ViewBag).CurrentSort = sortOrder;
			base.ViewBag.FirstNameSortParm = (string.IsNullOrEmpty(sortOrder) ? "firstname_desc" : "");
			base.ViewBag.LastNameSortParm = (string.IsNullOrEmpty(sortOrder) ? "lastname_desc" : "");
			List<TitleUserQuickViewModel> titleCompanyUsers = this._user.GetTitleCompanyUsers(titleUserSearchModel);
			if (sortOrder == "firstname_desc")
			{
				model.TitleUsers = (
					from s in titleCompanyUsers
					orderby s.FirstName descending
					select s).ToPagedList<TitleUserQuickViewModel>(num1, num);
			}
			else if (sortOrder == "firstname")
			{
				model.TitleUsers = (
					from s in titleCompanyUsers
					orderby s.FirstName
					select s).ToPagedList<TitleUserQuickViewModel>(num1, num);
			}
			else if (sortOrder == "lastname_desc")
			{
				model.TitleUsers = (
					from s in titleCompanyUsers
					orderby s.LastName descending
					select s).ToPagedList<TitleUserQuickViewModel>(num1, num);
			}
			else if (sortOrder == "lastname")
			{
				model.TitleUsers = (
					from w in titleCompanyUsers
					orderby w.LastName
					select w).ToPagedList<TitleUserQuickViewModel>(num1, num);
			}
			else
			{
				model.TitleUsers = (
					from x in titleCompanyUsers
					orderby x.FirstName
					select x).ToPagedList<TitleUserQuickViewModel>(num1, num);
			}
			return base.View("ManageTitleCompanyUsers", model);
		}

		[HttpPost]
		public ActionResult NotifyRegistrant(NotifyRegistrantOfMatchingAssetsModel model)
		{
			NotifyRegistrantOfMatchingAssetsModel notifyRegistrantOfMatchingAssetsModel = model;
			try
			{
				NotificationForRegistrantOfMatchingAssetsEmail notificationForRegistrantOfMatchingAssetsEmail = new NotificationForRegistrantOfMatchingAssetsEmail()
				{
					Assets = new List<AssetDescriptionModel>()
				};
				foreach (int assetNumber in notifyRegistrantOfMatchingAssetsModel.AssetNumbers)
				{
					notificationForRegistrantOfMatchingAssetsEmail.Assets.Add(this._asset.GetAssetByAssetNumber(assetNumber));
				}
				AssetSearchCriteriaModel assetSearchCriteriaById = this._asset.GetAssetSearchCriteriaById(Convert.ToInt32(notifyRegistrantOfMatchingAssetsModel.AssetSearchId));
				UserModel userById = this._user.GetUserById(assetSearchCriteriaById.UserId);
				notificationForRegistrantOfMatchingAssetsEmail.RegistrantEmail = userById.Username;
				notificationForRegistrantOfMatchingAssetsEmail.RegistrantName = userById.FullName;
				notificationForRegistrantOfMatchingAssetsEmail.SearchCriteriaDate = assetSearchCriteriaById.DateEntered;
				if (base.CanSendAutoEmail(BaseController.AutoEmailType.Asset))
				{
					this._email.Send(notificationForRegistrantOfMatchingAssetsEmail);
				}
				foreach (int num in notifyRegistrantOfMatchingAssetsModel.AssetNumbers)
				{
					this._asset.HoldAssetForUser(userById.UserId, num);
				}
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Asset(s) will be placed on hold for seven days. Notification has been sent to registrant.");
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, string.Concat("Could not send notification. Error: ", exception.Message));
			}
			notifyRegistrantOfMatchingAssetsModel.AssetSearchId = null;
			notifyRegistrantOfMatchingAssetsModel = new NotifyRegistrantOfMatchingAssetsModel()
			{
				Searches = new List<SelectListItem>()
			};
			this._user.GetAllSearches().ForEach((UserAssetSearchCriteriaQuickViewModel f) => notifyRegistrantOfMatchingAssetsModel.Searches.Add(new SelectListItem()
			{
				Text = string.Format("{0}'s search criteria form entered on {1}", f.UserFullName, f.DateCreated),
				Value = f.AssetSearchCriteriaId.ToString()
			}));
			return base.View();
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

		[Authorize]
		private Dictionary<string, IEnumerable<SelectListItem>> populateCommDetailsCheckBoxList(List<CommercialPropertyDetails> details, AssetType type)
		{
			IEnumerable<SelectListItem> selectListItems;
			List<CommercialPropertyDetails> list = Enum.GetValues(typeof(CommercialPropertyDetails)).Cast<CommercialPropertyDetails>().ToList<CommercialPropertyDetails>();
			if (type == AssetType.Office)
			{
				list = this.RemoveCommercialDetails(list);
				list.Remove(CommercialPropertyDetails.Complex_PetPark);
			}
			else if (type == AssetType.Retail)
			{
				list = this.RemoveCommercialDetails(list);
				list.Remove(CommercialPropertyDetails.Security_CodeAccess);
				list.Remove(CommercialPropertyDetails.Security_OnDutyGuard);
				list.Remove(CommercialPropertyDetails.UpgradesExt_BoilerSystem);
				list.Remove(CommercialPropertyDetails.Complex_PetPark);
			}
			else if (type == AssetType.Medical)
			{
				list = this.RemoveCommercialDetails(list);
				list.Remove(CommercialPropertyDetails.Complex_PetPark);
			}
			else if (type == AssetType.Industrial)
			{
				list = this.RemoveCommercialDetails(list);
				list.Remove(CommercialPropertyDetails.Complex_BusinessCenter);
				list.Remove(CommercialPropertyDetails.GenFeatures_Escalators);
				list.Remove(CommercialPropertyDetails.Complex_PetPark);
			}
			else if (type == AssetType.MixedUse)
			{
				list.Remove(CommercialPropertyDetails.Complex_ChildCareFacility);
			}
			else if (type == AssetType.ConvenienceStoreFuel)
			{
				list = this.RemoveCommercialDetails(list);
				list.Remove(CommercialPropertyDetails.Complex_ChildCareFacility);
				list.Remove(CommercialPropertyDetails.Complex_BusinessCenter);
				list.Remove(CommercialPropertyDetails.GenFeatures_Escalators);
				list.Remove(CommercialPropertyDetails.GenFeatures_Elevators);
				list.Remove(CommercialPropertyDetails.Security_Gate);
				list.Remove(CommercialPropertyDetails.Security_CodeAccess);
				list.Remove(CommercialPropertyDetails.Security_OnDutyGuard);
				list.Remove(CommercialPropertyDetails.Park_None);
				list.Remove(CommercialPropertyDetails.Park_AboveOrBelowPark);
				list.Remove(CommercialPropertyDetails.Park_StreetParkOnly);
				list.Remove(CommercialPropertyDetails.Park_NeighboringPark);
			}
			selectListItems = (details == null ? 
				from item in list
				select new SelectListItem()
				{
					Text = EnumHelper.GetEnumDescription(item),
					Value = item.ToString()
				} : 
				from item in list
				select new SelectListItem()
				{
					Text = EnumHelper.GetEnumDescription(item),
					Value = item.ToString(),
					Selected = details.Any<CommercialPropertyDetails>((CommercialPropertyDetails d) => d.ToString() == item.ToString())
				});
			return this.BuildDetailsDictionary(selectListItems);
		}

		[Authorize]
		private IEnumerable<SelectListItem> populateMFDefMaintCheckBoxList(List<MaintenanceDetails> details)
		{
			IEnumerable<SelectListItem> selectListItems;
			IEnumerable<MaintenanceDetails> maintenanceDetails = Enum.GetValues(typeof(MaintenanceDetails)).Cast<MaintenanceDetails>();
			selectListItems = (details == null ? 
				from item in maintenanceDetails
				select new SelectListItem()
				{
					Text = EnumHelper.GetEnumDescription(item),
					Value = item.ToString()
				} : 
				from item in maintenanceDetails
				select new SelectListItem()
				{
					Text = EnumHelper.GetEnumDescription(item),
					Value = item.ToString(),
					Selected = details.Any<MaintenanceDetails>((MaintenanceDetails d) => d.ToString() == item.ToString())
				});
			return selectListItems;
		}

		[Authorize]
		private Dictionary<string, IEnumerable<SelectListItem>> populateMFDetailsCheckBoxList(List<MultiFamilyPropertyDetails> details)
		{
			IEnumerable<SelectListItem> selectListItems;
			IEnumerable<MultiFamilyPropertyDetails> multiFamilyPropertyDetails = Enum.GetValues(typeof(MultiFamilyPropertyDetails)).Cast<MultiFamilyPropertyDetails>();
			selectListItems = (details == null ? 
				from mfPropDetail in multiFamilyPropertyDetails
				select new SelectListItem()
				{
					Text = EnumHelper.GetEnumDescription(mfPropDetail),
					Value = mfPropDetail.ToString()
				} : 
				from mfPropDetail in multiFamilyPropertyDetails
				select new SelectListItem()
				{
					Text = EnumHelper.GetEnumDescription(mfPropDetail),
					Value = mfPropDetail.ToString(),
					Selected = details.Any<MultiFamilyPropertyDetails>((MultiFamilyPropertyDetails d) => d.ToString() == mfPropDetail.ToString())
				});
			return this.BuildDetailsDictionary(selectListItems);
		}

		[Authorize]
		private Dictionary<string, IEnumerable<SelectListItem>> populateMHPDetailsCheckBoxList(List<MobileHomePropertyDetails> details)
		{
			IEnumerable<SelectListItem> selectListItems;
			IEnumerable<MobileHomePropertyDetails> mobileHomePropertyDetails = Enum.GetValues(typeof(MobileHomePropertyDetails)).Cast<MobileHomePropertyDetails>();
			selectListItems = (details == null ? 
				from mfPropDetail in mobileHomePropertyDetails
				select new SelectListItem()
				{
					Text = EnumHelper.GetEnumDescription(mfPropDetail),
					Value = mfPropDetail.ToString()
				} : 
				from mhpPropDetail in mobileHomePropertyDetails
				select new SelectListItem()
				{
					Text = EnumHelper.GetEnumDescription(mhpPropDetail),
					Value = mhpPropDetail.ToString(),
					Selected = details.Any<MobileHomePropertyDetails>((MobileHomePropertyDetails d) => d.ToString() == mhpPropDetail.ToString())
				});
			return this.BuildDetailsDictionary(selectListItems);
		}

		private IEnumerable<SelectListItem> populatePortfolioListDDL()
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			List<PortfolioQuickListViewModel> portfolioQuickListViewModels = new List<PortfolioQuickListViewModel>();
			if (userByUsername != null)
			{                
                portfolioQuickListViewModels = this._portfolio.GetUserPortfolios(userByUsername.UserId);                    
			}

            var portfolioSelectList = (from userPf in portfolioQuickListViewModels
                 select new SelectListItem()
                 {
                     Text = userPf.PortfolioName.Trim(),
                     Value = userPf.PortfolioId.ToString()
                 }).ToList();
            portfolioSelectList.Add(new SelectListItem()
            {
                Text = "---",
                Value = "0"
            });
            return portfolioSelectList;
		}

     

		public ActionResult ReactivateMba(int id)
		{
			this._user.ReactivateMba(id);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "MBA successfully reactivated.");
			return base.RedirectToAction("ManageMBAMembersImported");
		}

		[Authorize]
		public ActionResult ReactivateNARMember(int id)
		{
			this._user.ReactivateNarMember(id);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "NAR Member successfully reactivated.");
			return base.View("ManageNarMembersImported");
		}

		public ActionResult ReactivatePrincipalInvestor(int id)
		{
			this._user.ReactivatePrincipalInvestor(id);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Principal Investor successfully reactivated.");
			return base.RedirectToAction("ManagePrincipalInvestorsImport");
		}

        public ActionResult ReactivateOperatingCompany(string id)
        {
            this._user.ReactivateOperatingCompany(Guid.Parse(id));
            base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Operating Company successfully reactivated.");
            return base.RedirectToAction("ManageOperatingCompanies");
        }

        public ActionResult ReactivateHoldingCompany(string id, string ocId)
        {
            this._user.ReactivateHoldingCompany(Guid.Parse(id));
            base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Holding Company successfully reactivated.");
            return base.RedirectToAction("UpdateOperatingCompany", new { id = ocId });
        }

        public ActionResult ReactivateUser(int id)
		{
			this._user.ReactivateUser(id);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "User successfully reactivated.");
			return base.RedirectToAction("ManagePrincipalInvestorsReg");
		}

		public ActionResult ReactivateUserAll(int id, string method)
		{
			this._user.ReactivateUser(id);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "User successfully reactivated.");
			return base.RedirectToAction(method);
		}

		public ActionResult ReactivateUserMBA(int id)
		{
			this._user.ReactivateUser(id);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "User successfully reactivated.");
			return base.RedirectToAction("ManageMBAMembersReg");
		}

		[Authorize]
		[HttpGet]
		public ActionResult RecordCommissionPayment()
		{
			return base.View(new RecordCommissionPaymentModel());
		}

		[HttpPost]
		public ActionResult RecordCommissionPayment(RecordCommissionPaymentModel model)
		{
			if (!base.ModelState.IsValid)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid fields.");
			}
			else if (!this._asset.DoesAssetExist(model.AssetNumber))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid asset number.");
			}
			else
			{
				model.RecordedByUserId = this._user.GetUserByUsername(base.User.Identity.Name).UserId;
				this._asset.RecordCommissionPayment(model);
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Commission successfully recorded.");
			}
			return base.View(model);
		}

		[Authorize]
		[HttpGet]
		public ViewResult RecordICContractPayment(int id)
		{
			UserModel userById = this._user.GetUserById(id);
			return base.View(new RecordContractPaymentModel()
			{
				UserId = id,
				Name = userById.FullName,
				PaymentDate = DateTime.Now.Date,
				AmountPaid = 1000
			});
		}

		[HttpPost]
		public ActionResult RecordICContractPayment(RecordContractPaymentModel model)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			model.RecordedByUserId = userByUsername.UserId;
			model.Type = ContractFeePayoutType.icAdmin;
			this._user.RecordContractPayment(model);
			return base.RedirectToAction("ICAccountingReportDisplay", new { id = model.UserId });
		}

		[Authorize]
		[HttpGet]
		public ViewResult RecordJVMAContractPayment(int id)
		{
			UserModel userById = this._user.GetUserById(id);
			return base.View(new RecordContractPaymentModel()
			{
				UserId = id,
				Name = userById.FullName,
				PaymentDate = DateTime.Now.Date,
				AmountPaid = 1000
			});
		}

		[HttpPost]
		public ActionResult RecordJVMAContractPayment(RecordContractPaymentModel model)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			model.RecordedByUserId = userByUsername.UserId;
			model.Type = ContractFeePayoutType.jvma;
			this._user.RecordContractPayment(model);
			return base.RedirectToAction("JVMAAccountingReportDisplay", new { id = model.UserId });
		}

		[Authorize]
		[HttpGet]
		public ActionResult RecordPayment()
		{
			return base.View(new RecordPaymentModel());
		}

		[HttpPost]
		public ActionResult RecordPayment(RecordPaymentModel model)
		{
			if (!base.ModelState.IsValid)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid fields.");
			}
			else if (!this._asset.DoesAssetExist(model.AssetNumber))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid asset number.");
			}
			else
			{
				double num = 0;
				model.RecordedByUserId = this._user.GetUserByUsername(base.User.Identity.Name).UserId;
				this._asset.RecordAssetPayment(model);
				num = this._asset.CalculateCommission(model.AssetId);
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, string.Concat("Payment successfully record. Commission owed: ", num.ToString("C")));
			}
			return base.View(model);
		}

		[Authorize]
		[HttpPost]
		public ActionResult RecordTitleCompanyPayment(double currentRate, DateTime? datePaid, int titleCompanyId, Guid AssetId)
		{
			if (!datePaid.HasValue)
			{
				datePaid = new DateTime?(DateTime.Now);
			}
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			this._asset.RecordTitleOrderPayment(currentRate, datePaid.Value, titleCompanyId, AssetId, userByUsername.UserId);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Title company order payment recorded.");
			return base.RedirectToAction("RecordTitleCoOrderPayment", new { TitleCoId = titleCompanyId });
		}

		[Authorize]
		public ActionResult RecordTitleCoOrderPayment(OrderSearchResultsModel model, int? TitleCoId, string task = null)
		{
			if (!base.ValidateAdminUser(this._user.GetUserByUsername(base.User.Identity.Name)))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			if (TitleCoId.GetValueOrDefault(0) <= 0)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid Id.");
				return base.RedirectToAction("ManageTitleCompanies", "Admin");
			}
			if (!string.IsNullOrEmpty(task))
			{
				if (task == "pending")
				{
					model.Page = model.PendingPage;
				}
				else if (task == "completed")
				{
					model.Page = model.CompletedPage;
				}
			}
			model.TitleCompanyId = TitleCoId.Value;
			OrderSearchModel orderSearchModel = new OrderSearchModel()
			{
				TitleCompanyId = model.TitleCompanyId,
				AssetNumber = model.AssetNumber
			};
			List<OrderModel> ordersForAdmin = this._asset.GetOrdersForAdmin(orderSearchModel);
			int num = (base.TempData["RowCount"] != null ? Convert.ToInt32(base.TempData["RowCount"]) : 50);
			base.TempData.Keep("RowCount");
			int? page = model.Page;
			int num1 = (page.HasValue ? page.GetValueOrDefault() : 1);
			model.Pending = ordersForAdmin.Where<OrderModel>((OrderModel x) => {
				if (x.OrderStatus != OrderStatus.Completed)
				{
					return false;
				}
				return !x.DatePaid.HasValue;
			}).ToPagedList<OrderModel>(num1, num);
			model.Completed = ordersForAdmin.Where<OrderModel>((OrderModel x) => {
				if (x.OrderStatus != OrderStatus.Completed)
				{
					return false;
				}
				return x.DatePaid.HasValue;
			}).OrderByDescending<OrderModel, DateTime?>((OrderModel x) => x.DatePaid).ToPagedList<OrderModel>(num1, num);
			return base.View(model);
		}

		public ActionResult ReinstateSellerPrivilege(int id, string returnAction)
		{
			this._user.ReinstateSellerPrivileges(id);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "User seller privileges reinstated.");
			return base.RedirectToAction(returnAction);
		}

		[Authorize]
		public ActionResult RejectICAdmin(int id, string name, string email)
		{
			this._user.RejectICAdmin(id);
			if (base.CanSendAutoEmail(BaseController.AutoEmailType.Admin))
			{
				this._email.Send(new ICAdminChangeStatusEmail()
				{
					ViewName = "ICAdminRejected",
					Name = name,
					To = email
				});
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "IC Admin rejected.");
			return base.RedirectToAction("ManageICAdmins");
		}

		private List<CommercialPropertyDetails> RemoveCommercialDetails(List<CommercialPropertyDetails> details)
		{
			details.Remove(CommercialPropertyDetails.GenFeatures_ValetTrashRemovalForTenants);
			details.Remove(CommercialPropertyDetails.Complex_ChildCareFacility);
			details.Remove(CommercialPropertyDetails.Complex_ChildrenPlayground);
			details.Remove(CommercialPropertyDetails.Complex_ManagerOffice);
			details.Remove(CommercialPropertyDetails.Complex_Clubhouse);
			details.Remove(CommercialPropertyDetails.Complex_VolleyballCourt);
			details.Remove(CommercialPropertyDetails.Complex_SportCourt);
			details.Remove(CommercialPropertyDetails.Complex_CommunityBBQFacility);
			details.Remove(CommercialPropertyDetails.Complex_OneCommunityPool);
			details.Remove(CommercialPropertyDetails.Complex_TwoCommunityPool);
			details.Remove(CommercialPropertyDetails.Complex_OneCommunity);
			details.Remove(CommercialPropertyDetails.Complex_TwoCommunity);
			details.Remove(CommercialPropertyDetails.Complex_LaundryFacility);
			details.Remove(CommercialPropertyDetails.Complex_RVParkingFacility);
			details.Remove(CommercialPropertyDetails.IntFeatures_AllHavePatios);
			details.Remove(CommercialPropertyDetails.IntFeatures_SomeHavePatios);
			details.Remove(CommercialPropertyDetails.IntFeatures_All2BedHave2Bath);
			details.Remove(CommercialPropertyDetails.IntFeatures_SomeHaveWalkinClosets);
			details.Remove(CommercialPropertyDetails.IntFeatures_AllHaveWalkinClosets);
			details.Remove(CommercialPropertyDetails.IntFeatures_WasherDryerHookUpsAllUnits);
			details.Remove(CommercialPropertyDetails.IntFeatures_WasherDryerHookUpsSomeUnits);
			details.Remove(CommercialPropertyDetails.IntFeatures_AllWasherDryer);
			details.Remove(CommercialPropertyDetails.IntFeatures_SomeWasherDryer);
			details.Remove(CommercialPropertyDetails.KitchAmen_TrashCompactor);
			details.Remove(CommercialPropertyDetails.KitchAmen_BreakfastBar);
			details.Remove(CommercialPropertyDetails.KitchAmen_Refrig);
			details.Remove(CommercialPropertyDetails.KitchAmen_AllHaveMicrowaves);
			details.Remove(CommercialPropertyDetails.KitchAmen_SomeHaveMicrowaves);
			details.Remove(CommercialPropertyDetails.KitchAmen_AllDishwasher);
			details.Remove(CommercialPropertyDetails.KitchAmen_SomeDishwasher);
			details.Remove(CommercialPropertyDetails.KitchAmen_Disposal);
			details.Remove(CommercialPropertyDetails.KitchAmen_Pantry);
			details.Remove(CommercialPropertyDetails.KitchAmen_Island);
			details.Remove(CommercialPropertyDetails.KitchAmen_GasRangeOven);
			details.Remove(CommercialPropertyDetails.KitchAmen_ElectricRangeOven);
			details.Remove(CommercialPropertyDetails.KitchAmen_EatInKitch);
			return details;
		}

		public ActionResult RevokeSellerPrivilege(int id, string returnAction)
		{
			this._user.RevokeSellerPrivileges(id);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "User seller privileges revoked.");
			return base.RedirectToAction(returnAction);
		}

		[HttpPost]
		[MultipleButton(Name="action", Argument="Submit")]
		public ActionResult SaveAndSubmitAsset(AssetViewModel model, string mainImg, string flyerImg)
		{
			if (!base.ModelState.IsValid)
			{
				foreach (ModelError list in base.ModelState.Values.SelectMany<System.Web.Mvc.ModelState, ModelError>((System.Web.Mvc.ModelState v) => v.Errors).ToList<ModelError>())
				{
					if (!list.ErrorMessage.ToLower().Contains("asset document type"))
					{
						continue;
					}
					base.ModelState.Remove("AssetDocumentType");
                    base.ModelState.Add("AssetDocumentType", new System.Web.Mvc.ModelState());
					base.ModelState.SetModelValue("AssetDocumentType", new ValueProviderResult((object)AssetDocumentType.CurrentRentRoll, "CurrentRentRoll", null));
					goto Label0;
				}
			}
		Label0:
			bool flag = false;
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			model.User = this._user.GetUserEntity(base.User.Identity.Name);
			base.ViewBag.IsSeller = (userByUsername.UserType == UserType.ListingAgent ? true : userByUsername.UserType == UserType.Investor);
			base.ViewBag.IsCorpAdmin = (this.isUserAdmin(userByUsername) || userByUsername.UserType == UserType.ListingAgent ? true : userByUsername.UserType == UserType.Investor);
			base.ViewBag.IsICAdmin = (userByUsername.UserType == UserType.ICAdmin ? true : false);
			if (!((dynamic)base.ViewBag).IsSeller)
			{
				((dynamic)base.ViewBag).Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
			}
			else
			{
				((dynamic)base.ViewBag).Layout = "~/Views/Shared/_Layout.cshtml";
			}
			base.ViewBag.IsICAdmin = (userByUsername.UserType == UserType.ICAdmin ? true : false);
			if (!((dynamic)base.ViewBag).IsSeller)
			{
				((dynamic)base.ViewBag).Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
			}
			else
			{
				((dynamic)base.ViewBag).Layout = "~/Views/Shared/_Layout.cshtml";
			}
			List<NarMemberViewModel> assetNarMembers = this._user.GetAssetNarMembers(new NarMemberSearchModel()
			{
				ShowActiveOnly = true
			});
			model.ListingAgents.Add(new SelectListItem()
			{
				Value = "0",
				Text = " "
			});
			foreach (NarMemberViewModel assetNarMember in assetNarMembers.OrderBy(l=>l.LastName.Trim()).ThenBy(l=>l.FirstName.Trim()))
			{
				model.ListingAgents.Add(new SelectListItem()
				{
					Text = assetNarMember.LastName + ", " + assetNarMember.FirstName,
					Value = assetNarMember.NarMemberId.ToString()
				});
			}
			bool flag1 = false;
            if (model.PropHoldTypeId == 2 && !model.LeaseholdMaturityDate.HasValue)
            {
                ModelState.AddModelError("LeaseholdMaturityDate", "Leasehold maturity date required");
                flag1 = true;
            }
            foreach (AssetTaxParcelNumber assetTaxParcelNumber in model.AssetTaxParcelNumbers)
			{
                if (this._asset.GetMatchingAssetsByAPNCountyState(assetTaxParcelNumber.TaxParcelNumber, model.State, model.County).All(x => x.AssetId == model.AssetId))
                {
                    continue;
                }
                base.ModelState.AddModelError("TaxAssessorNumber", string.Concat("Tax Assessor Number ", assetTaxParcelNumber.TaxParcelNumber, " is already in use for another asset"));
				flag1 = true;
			}
			if (model.AssetNARMembers == null)
			{
				model.AssetNARMembers = new List<AssetNARMember>();
			}
			for (int i = 0; i < model.AssetNARMembers.Count; i++)
			{
				if (string.IsNullOrEmpty(model.AssetNARMembers[i].NARMember.Email))
				{
					base.ModelState.AddModelError(string.Concat("AssetNARMembers[", i, "].NARMember.Email"), "Listing Agent Email Required");
					flag1 = true;
				}
				if (string.IsNullOrEmpty(model.AssetNARMembers[i].NARMember.FirstName))
				{
					base.ModelState.AddModelError(string.Concat("AssetNARMembers[", i, "].NARMember.FirstName"), "Listing Agent First Name Required");
					flag1 = true;
				}
				if (string.IsNullOrEmpty(model.AssetNARMembers[i].NARMember.LastName))
				{
					base.ModelState.AddModelError(string.Concat("AssetNARMembers[", i, "].NARMember.LastName"), "Listing Agent Last Name Required");
					flag1 = true;
				}
			}
			List<string> strs = new List<string>();
			List<int> nums = new List<int>();
			if (!flag1)
			{
				for (int j = 0; j < model.AssetNARMembers.Count; j++)
				{
					if (model.AssetNARMembers[j].NARMember.NarMemberId != 0 && model.AssetNARMembers[j].NARMember.Email != null)
					{
						strs.Add(model.AssetNARMembers[j].NARMember.Email);
						if ((
							from temp in strs
							where temp.Equals(model.AssetNARMembers[j].NARMember.Email)
							select temp).Count<string>() > 1)
						{
							nums.Add(j);
						}
						if (nums.Count<int>() > 0)
						{
							base.ModelState.AddModelError(string.Concat("AssetNARMembers[", j, "].NARMember.Email"), string.Concat(new string[] { "Listing Agent ", model.AssetNARMembers[j].NARMember.FirstName, " ", model.AssetNARMembers[j].NARMember.LastName, " email ", model.AssetNARMembers[j].NARMember.Email, " already exist as one of the entries." }));
							flag1 = true;
						}
					}
				}
			}
			if (model.Images == null)
			{
				model.Images = new List<AssetImage>();
			}
			if (model.Documents == null)
			{
				model.Documents = new List<AssetDocument>();
			}
			if (model.Videos == null)
			{
				model.Videos = new List<AssetVideo>();
			}
			if (model.AssetTaxParcelNumbers == null)
			{
				model.AssetTaxParcelNumbers = new List<AssetTaxParcelNumber>();
			}
			int num = 0;
			List<AssetImage> fileName = (
				from s in model.Images
				orderby s.Order
				select s).ToList<AssetImage>();
			for (int l = 0; l < fileName.Count; l++)
			{
				bool flag3 = false;
				do
				{
					if (l >= fileName.Count)
					{
						break;
					}
					if (string.IsNullOrEmpty(fileName[l].ContentType))
					{
						fileName.RemoveAt(l);
						flag3 = true;
					}
					else if (!"image/jpeg,image/png,image/gif".Contains(fileName[l].ContentType.ToLower()))
					{
						fileName.RemoveAt(l);
						flag3 = true;
					}
					else
					{
						if (fileName[l].AssetImageId == Guid.Empty && base.ModelState.IsValid)
						{
							fileName[l].OriginalFileName = fileName[l].FileName;
							fileName[l].AssetImageId = base.generatedGuidIfNone(fileName[l].AssetImageId);
							IFileManager fileManager = this._fileManager;
							string str = fileName[l].FileName;
							Guid assetId = model.AssetId;
							string dateForTempImages = model.DateForTempImages;
							int userId = userByUsername.UserId;
							string str1 = fileManager.MoveTempAssetImage(str, assetId, dateForTempImages, userId.ToString());
							if (!string.IsNullOrEmpty(str1))
							{
								fileName[l].FileName = str1;
							}
						}
						if (fileName[l].FileName != null)
						{
							fileName[l].AssetId = model.AssetId;
							fileName[l].Order = num;
							num++;
							flag3 = false;
						}
						else
						{
							try
							{
								fileName.RemoveAt(l);
								flag3 = true;
							}
							catch
							{
							}
						}
					}
				}
				while (flag3);
			}
			model.Images = fileName;
			num = 0;
			for (int m = 0; m < model.Documents.Count; m++)
			{
				bool flag4 = false;
				do
				{
					if (m >= model.Documents.Count)
					{
						break;
					}
					if (model.Documents[m].FileName != null)
					{
						model.Documents[m].AssetDocumentId = base.generatedGuidIfNone(model.Documents[m].AssetDocumentId);
						model.Documents[m].Order = num;
						num++;
						flag4 = false;
					}
					else
					{
						try
						{
							model.Documents.RemoveAt(m);
							flag4 = true;
						}
						catch
						{
						}
					}
				}
				while (flag4);
			}
			for (int n = 0; n < model.AssetTaxParcelNumbers.Count; n++)
			{
				if (string.IsNullOrEmpty(model.AssetTaxParcelNumbers[n].TaxParcelNumber))
				{
					base.ModelState.AddModelError(string.Concat("AssetTaxParcelNumbers[", n, "].TaxParcelNumber"), "Tax Parcel Number required");
				}
			}
			if (model.IsPaper)
			{
				if (!model.NoteOrigination.HasValue)
				{
					base.ModelState.AddModelError("NoteOrigination", "Date of Note Origination Required");
				}
				if (!model.NotePrincipal.HasValue)
				{
					base.ModelState.AddModelError("NotePrincipal", "Original Note Principal Required");
				}
				if (!model.CurrentNotePrincipal.HasValue)
				{
					base.ModelState.AddModelError("CurrentNotePrincipal", "Current Principal Balance of Note Required");
				}
				if (string.IsNullOrEmpty(model.HasCopyOfAppraisal))
				{
					base.ModelState.AddModelError("HasCopyOfAppraisal", "Has Copy of Appraisal Required");
				}
				if (string.IsNullOrEmpty(model.TypeOfNote))
				{
					base.ModelState.AddModelError("TypeOfNote", "Type of Note Required");
				}
				if (!model.NoteInterestRate.HasValue)
				{
					base.ModelState.AddModelError("NoteInterestRate", "Note Interest Rate Required");
				}
				if (!model.PaymentAmount.HasValue)
				{
					base.ModelState.AddModelError("PaymentAmount", "Payment Amount Required");
				}
				if (model.PaymentFrequency == null)
				{
					base.ModelState.AddModelError("PaymentFrequency", "Frequency of Payment Required");
				}
				if (string.IsNullOrEmpty(model.TypeOfMTGInstrument))
				{
					base.ModelState.AddModelError("TypeOfMTGInstrument", "Type of Mortgage Instrument Required");
				}
				if (string.IsNullOrEmpty(model.AmortType))
				{
					base.ModelState.AddModelError("AmortType", "Amort Type Required");
				}
				if (!model.PaymentsMadeOnNote.HasValue)
				{
					base.ModelState.AddModelError("PaymentsMadeOnNote", "Number of Payment on Note Since its Origination Required");
				}
				if (!model.PaymentsRemainingOnNote.HasValue)
				{
					base.ModelState.AddModelError("PaymentsRemainingOnNote", "Number of Payments Remaining on Note Required");
				}
				if (!model.IsNoteCurrent.HasValue)
				{
					base.ModelState.AddModelError("IsNoteCurrent", "Is Note Current Required");
				}
				if (!model.LastPaymentRecievedOnNote.HasValue)
				{
					base.ModelState.AddModelError("LastPaymentRecievedOnNote", "Date of Last Payment Recieved on Note Required");
				}
				if (!model.NextPaymentOnNote.HasValue)
				{
					base.ModelState.AddModelError("NextPaymentOnNote", "Next Payment Due Date Required");
				}
				if (model.PaymentHistory == null)
				{
					base.ModelState.AddModelError("PaymentHistory", "Payment History of the note maker (payor) Required");
				}
			}
			if (model.HasPositionMortgage == PositionMortgageType.Yes)
			{
				bool hasValue = model.MortgageLienType.HasValue;
				if (!model.MortgageLienAssumable.HasValue)
				{
					base.ModelState.AddModelError("MortgageLienAssumable", "Mortgage Lien Assumable required.");
				}
			}
			if (!string.IsNullOrEmpty(model.TitleCompany))
			{
				int num1 = 0;
				if (int.TryParse(model.TitleCompany, out num1))
				{
					model.TitleCompanyId = new int?(num1);
				}
			}
			if (!model.Images.Any<AssetImage>((AssetImage x) => x.IsFlyerImage))
			{
				base.ModelState.AddModelError("ImageError1", "Flyer Image Required");
			}
			if (!model.Images.Any<AssetImage>((AssetImage x) => x.IsMainImage))
			{
				base.ModelState.AddModelError("ImageError2", "Main Image Required");
			}
			if (model.GetType() == typeof(MultiFamilyAssetViewModel))
			{
				MultiFamilyAssetViewModel multiFamilyAssetViewModel = model as MultiFamilyAssetViewModel;
				if (multiFamilyAssetViewModel.UnitSpecifications != null)
				{
					for (int o = 0; o < multiFamilyAssetViewModel.UnitSpecifications.Count; o++)
					{
						multiFamilyAssetViewModel.UnitSpecifications[o].AssetUnitSpecificationId = base.generatedGuidIfNone(multiFamilyAssetViewModel.UnitSpecifications[o].AssetUnitSpecificationId);
						if (multiFamilyAssetViewModel.UnitSpecifications[o].AssetId == Guid.Empty)
						{
							multiFamilyAssetViewModel.UnitSpecifications[o].AssetId = model.AssetId;
						}
					}
				}
				if (multiFamilyAssetViewModel.MHPUnitSpecifications != null)
				{
					for (int p = 0; p < multiFamilyAssetViewModel.MHPUnitSpecifications.Count; p++)
					{
						multiFamilyAssetViewModel.MHPUnitSpecifications[p].AssetMHPSpecificationId = base.generatedGuidIfNone(multiFamilyAssetViewModel.MHPUnitSpecifications[p].AssetMHPSpecificationId);
						if (multiFamilyAssetViewModel.MHPUnitSpecifications[p].AssetId == Guid.Empty)
						{
							multiFamilyAssetViewModel.MHPUnitSpecifications[p].AssetId = model.AssetId;
						}
					}
				}
				if (base.ModelState.IsValid)
				{
					multiFamilyAssetViewModel.IsSubmitted = true;
					flag = this._asset.UpdateAssetByViewModel(multiFamilyAssetViewModel);
				}
				if (model.AssetType != AssetType.MHP)
				{
					this.BuildMFViewBags(this.populateMFDetailsCheckBoxList((model as MultiFamilyAssetViewModel).MFDetails));
				}
				else
				{
					this.BuildMFViewBags(this.populateMHPDetailsCheckBoxList((model as MultiFamilyAssetViewModel).MHPDetails));
				}
				if (!base.ModelState.IsValid)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problems with data input fields. Please review below");
					base.TempData["modelState"] = base.ModelState;
					multiFamilyAssetViewModel.IsSubmitted = false;
					this.SetUpAsset(model, userByUsername);
					return base.View("UpdateAsset", model);
				}
				if (!flag)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problem saving. Please contact site admin.");
					multiFamilyAssetViewModel.IsSubmitted = false;
					return base.View("UpdateAsset", model);
				}
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully saved.");
				this._asset.UnlockAsset(model.AssetId);
			}
			if (model.GetType() == typeof(CommercialAssetViewModel))
			{
				CommercialAssetViewModel commercialAssetViewModel = model as CommercialAssetViewModel;
				Dictionary<string, IEnumerable<SelectListItem>> strs1 = this.populateCommDetailsCheckBoxList((model as CommercialAssetViewModel).PropertyDetails, model.AssetType);
				this.BuildCommViewBags(strs1);
				if (base.ModelState.IsValid)
				{
					commercialAssetViewModel.IsSubmitted = true;
					flag = this._asset.UpdateAssetByViewModel(commercialAssetViewModel);
				}
				if (!base.ModelState.IsValid)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problems with data input fields. Please review below");
					commercialAssetViewModel.IsSubmitted = false;
					this.SetUpAsset(model, userByUsername);
					return base.View("UpdateAsset", model);
				}
				if (!flag)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problem saving. Please contact site admin.");
					commercialAssetViewModel.IsSubmitted = false;
					return base.View("UpdateAsset", model);
				}
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully saved.");
				this._asset.UnlockAsset(model.AssetId);
			}
			try
			{
				AssetDescriptionModel assetByAssetId = this._asset.GetAssetByAssetId(model.AssetId);
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
						NotificationForCorpAdminAssetInputEmail notificationForCorpAdminAssetInputEmail = new NotificationForCorpAdminAssetInputEmail()
						{
							AssetDescription = assetByAssetId.Description,
							AssetNumber = assetByAssetId.AssetNumber.ToString(),
							ICAdminName = userByUsername.FullName,
							Email = user.Username,
							To = user.FullName
						};
						this._email.Send(notificationForCorpAdminAssetInputEmail);
					}
					catch
					{
					}
				}
				if (userByUsername.UserType == UserType.ICAdmin)
				{
					try
					{
						ConfirmationAssetInputEmail confirmationAssetInputEmail = new ConfirmationAssetInputEmail()
						{
							AssetNumber = assetByAssetId.AssetNumber.ToString(),
							DateOfEntry = DateTime.Now,
							DidIncludeFiles = (model.Documents.Count > 0 ? "Yes" : "No"),
							DidIncludePhotos = (model.Images.Count > 0 ? "Yes" : "No"),
							Email = base.User.Identity.Name,
							Name = userByUsername.FullName,
							USCIC = userByUsername.UserId.ToString(),
							AssetDescription = assetByAssetId.Description
						};
						this._email.Send(confirmationAssetInputEmail);
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
            if(userByUsername.UserType == UserType.ICAdmin)
            {
                return base.RedirectToAction("ICACache", "ICA");
            }
			else if (userByUsername.UserType != UserType.CREBroker && userByUsername.UserType != UserType.CRELender && userByUsername.UserType != UserType.Investor)
			{
				return base.RedirectToAction("ManageAssets", "Admin");
			}
			return base.RedirectToAction("SellerManageAssets", "Investors");
		}

		[HttpPost]
		public ActionResult SaveAssignment(int assetNumber, int selectedUserId)
		{
			this._user.UpdateAssignment(assetNumber, selectedUserId);
			TitleCompanyUserModel titleUserByEmail = this._user.GetTitleUserByEmail(base.User.Identity.Name);
			return base.RedirectToAction("AssignmentList", "Admin", new { id = titleUserByEmail.TitleCompanyUserId });
		}

		[HttpPost]
		public JsonResult SavePartialAsset(AssetViewModel model)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);

            if (model.Images == null)
			{
				model.Images = new List<AssetImage>();
			}
			if (model.Documents == null)
			{
				model.Documents = new List<AssetDocument>();
			}
			if (model.Videos == null)
			{
				model.Videos = new List<AssetVideo>();
			}
			for (int i = 0; i < model.Videos.Count; i++)
			{
				bool flag = false;
				do
				{
					if (i >= model.Videos.Count)
					{
						break;
					}
					if (model.Videos[i].FilePath == null || !System.IO.File.Exists(Path.Combine(ConfigurationManager.AppSettings["DataRoot"], "Videos", model.Videos[i].AssetId.ToString(), model.Videos[i].FilePath)))
					{
						try
						{
							model.Videos.RemoveAt(i);
							flag = true;
						}
						catch
						{
						}
					}
					else
					{
						flag = false;
					}
				}
				while (flag);
			}
			if (!model.Images.Any<AssetImage>((AssetImage s) => s.IsMainImage) && model.Images.Count > 0)
			{
				model.Images[0].IsMainImage = true;
				model.Images[0].IsFlyerImage = true;
			}
			int num = 0;
			List<AssetImage> list = (
				from s in model.Images
				orderby s.Order
				select s).ToList<AssetImage>();
			for (int j = 0; j < list.Count; j++)
			{
				bool flag1 = false;
				do
				{
					if (j >= list.Count)
					{
						break;
					}
					if (list[j].FileName != null)
					{
						list[j].AssetImageId = base.generatedGuidIfNone(list[j].AssetImageId);
						list[j].Order = num;
						num++;
						flag1 = false;
					}
					else
					{
						try
						{
							list.RemoveAt(j);
							flag1 = true;
						}
						catch
						{
						}
					}
				}
				while (flag1);
			}
			model.Images = list;
			num = 0;
			for (int k = 0; k < model.Documents.Count; k++)
			{
				if (model.Documents[k].FileName != null)
				{
					model.Documents[k].AssetDocumentId = base.generatedGuidIfNone(model.Documents[k].AssetDocumentId);
					model.Documents[k].Order = num;
					num++;
				}
				else
				{
					model.Documents.RemoveAt(k);
				}
			}
			bool flag2 = true;
			if (model.AssetNumber == 0)
			{
				model.CreationDate = new DateTime?(DateTime.Now);
				model.IsActive = true;
				model.ListedByUserId = userByUsername.UserId;
				flag2 = false;
			}
			int? nullable = null;
			if (model.GetType() == typeof(MultiFamilyAssetViewModel))
			{
				MultiFamilyAssetViewModel multiFamilyAssetViewModel = model as MultiFamilyAssetViewModel;
				for (int l = 0; l < multiFamilyAssetViewModel.UnitSpecifications.Count; l++)
				{
					multiFamilyAssetViewModel.UnitSpecifications[l].AssetUnitSpecificationId = base.generatedGuidIfNone(multiFamilyAssetViewModel.UnitSpecifications[l].AssetUnitSpecificationId);
				}
				if (multiFamilyAssetViewModel.MHPUnitSpecifications != null)
				{
					for (int m = 0; m < multiFamilyAssetViewModel.MHPUnitSpecifications.Count; m++)
					{
						multiFamilyAssetViewModel.MHPUnitSpecifications[m].AssetMHPSpecificationId = base.generatedGuidIfNone(multiFamilyAssetViewModel.MHPUnitSpecifications[m].AssetMHPSpecificationId);
					}
				}
				if (!flag2)
				{
					nullable = this._asset.CreateAssetByViewModel(multiFamilyAssetViewModel);
					if (nullable.HasValue)
					{
						string str = ((model.CreationDate.HasValue ? model.CreationDate.Value : DateTime.Now)).ToString("MM/dd/yyyy");
						return new JsonResult()
						{
							Data = new { Status = "Created", Id = nullable, CreationDate = str, ListedByUserId = userByUsername.UserId }
						};
					}
				}
				else if (this._asset.UpdateAssetByViewModel(multiFamilyAssetViewModel))
				{
					return new JsonResult()
					{
						Data = new { Status = "Updated" }
					};
				}
			}
			if (model.GetType() == typeof(CommercialAssetViewModel))
			{
				CommercialAssetViewModel commercialAssetViewModel = model as CommercialAssetViewModel;
				if (flag2)
				{
					this._asset.UpdateAssetByViewModel(commercialAssetViewModel);
					return new JsonResult()
					{
						Data = new { Status = "Updated" }
					};
				}
				nullable = this._asset.CreateAssetByViewModel(commercialAssetViewModel);
				if (nullable.HasValue)
				{
					string str1 = ((model.CreationDate.HasValue ? model.CreationDate.Value : DateTime.Now)).ToString("MM/dd/yyyy");
					return new JsonResult()
					{
						Data = new { Status = "Created", Id = nullable, CreationDate = str1, ListedByUserId = userByUsername.UserId }
					};
				}
			}
			return new JsonResult()
			{
				Data = new { Status = "Unknown" }
			};
		}

		[Authorize]
		public ActionResult SearchTitleUsersPage(int id, string searchFN, string searchLN, string searchPN, string searchEM)
		{
			TitleUserSearchResultsModel titleUserSearchResultsModel = new TitleUserSearchResultsModel();
			int num = (base.TempData["RowCount"] != null ? Convert.ToInt32(base.TempData["RowCount"]) : 50);
			base.TempData.Keep("RowCount");
			int num1 = 1;
			TitleUserSearchModel titleUserSearchModel = new TitleUserSearchModel()
			{
				Email = searchEM,
				FirstName = searchFN,
				IsActive = true,
				IsManager = true,
				LastName = searchLN,
				PhoneNumber = searchPN,
				TitleCompanyId = new int?(id)
			};
			((dynamic)base.ViewBag).Email = this._user.GetTitleCompanyUsers().FirstOrDefault<TitleCompanyUser>((TitleCompanyUser x) => {
				if (!x.IsManager)
				{
					return false;
				}
				return x.TitleCompanyId == id;
			}).Email;
			List<TitleUserQuickViewModel> titleCompanyUsers = this._user.GetTitleCompanyUsers(titleUserSearchModel);
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			titleUserSearchResultsModel.ControllingUserType = userByUsername.UserType;
			titleUserSearchResultsModel.Email = searchEM;
			titleUserSearchResultsModel.FirstName = searchFN;
			titleUserSearchResultsModel.LastName = searchLN;
			titleUserSearchResultsModel.PhoneNumber = searchPN;
			titleUserSearchResultsModel.TitleCompanyId = id;
			return this.PartialView("_ManagerTitleUsersList", (
				from x in titleCompanyUsers
				orderby x.FirstName
				select x).ToPagedList<TitleUserQuickViewModel>(num1, num));
		}

		private void SendICAdminEmails(UserModel user, AssetViewModel model, int? id, string desc)
		{
			int valueOrDefault;
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
			foreach (UserQuickViewModel userQuickViewModel in users)
			{
				NotificationForCorpAdminAssetInputEmail notificationForCorpAdminAssetInputEmail = new NotificationForCorpAdminAssetInputEmail()
				{
					AssetDescription = desc
				};
				valueOrDefault = id.GetValueOrDefault(0);
				notificationForCorpAdminAssetInputEmail.AssetNumber = valueOrDefault.ToString();
				notificationForCorpAdminAssetInputEmail.ICAdminName = user.FullName;
				notificationForCorpAdminAssetInputEmail.Email = userQuickViewModel.Username;
				this._email.Send(notificationForCorpAdminAssetInputEmail);
			}
			if (user.UserType == UserType.ICAdmin)
			{
				ConfirmationAssetInputEmail confirmationAssetInputEmail = new ConfirmationAssetInputEmail();
				valueOrDefault = id.GetValueOrDefault(0);
				confirmationAssetInputEmail.AssetNumber = valueOrDefault.ToString();
				confirmationAssetInputEmail.DateOfEntry = DateTime.Now;
				confirmationAssetInputEmail.DidIncludeFiles = (model.Documents.Count > 0 ? "Yes" : "No");
				confirmationAssetInputEmail.DidIncludePhotos = (model.Images.Count > 0 ? "Yes" : "No");
				confirmationAssetInputEmail.Email = base.User.Identity.Name;
				confirmationAssetInputEmail.Name = user.FullName;
				confirmationAssetInputEmail.USCIC = user.UserId.ToString();
				confirmationAssetInputEmail.AssetDescription = desc;
				confirmationAssetInputEmail.PropertyAddress = string.Format("{0} {1} {2} {3} {4}", new object[] { model.PropertyAddress, model.PropertyAddress2, model.City, model.State, model.Zip });
				this._email.Send(confirmationAssetInputEmail);
			}
		}

		public ActionResult SetPageNumber(string count)
		{
			if (base.TempData["RowCount"] == null)
			{
				base.TempData.Add("RowCount", count);
				base.TempData.Keep("RowCount");
			}
			else
			{
				base.TempData["RowCount"] = count;
				base.TempData.Keep("RowCount");
			}
			return null;
		}

		[Authorize]
		public ActionResult SetSampleAsset(Guid id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			this._asset.SetSampleAsset(id);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Asset successfully published.");
			if (userByUsername.UserType != UserType.CREBroker && userByUsername.UserType != UserType.CRELender && userByUsername.UserType != UserType.Investor)
			{
				return base.RedirectToAction("ManageAssets", "Admin");
			}
			return base.RedirectToAction("SellerManageAssets", "Investors");
		}

		private void SetUpAsset(AssetViewModel asset, UserModel user)
		{
			asset.ExistingListingStatus = asset.ListingStatus;
			List<NarMemberViewModel> list = (
				from x in this._user.GetAssetNarMembers(new NarMemberSearchModel()
				{
					ShowActiveOnly = true
				})
				orderby x.LastName.Trim(), x.FirstName.Trim()
				select x).ToList<NarMemberViewModel>();
			asset.ListingAgents.Add(new SelectListItem()
			{
				Value = "0",
				Text = " "
			});
			foreach (NarMemberViewModel narMemberViewModel in list.OrderBy(l=>l.LastName.Trim()).ThenBy(l=>l.FirstName.Trim()))
			{
				asset.ListingAgents.Add(new SelectListItem()
				{
					Text = string.Concat(narMemberViewModel.LastName.Trim(), ", ", narMemberViewModel.FirstName.Trim()),
					Value = narMemberViewModel.NarMemberId.ToString()
				});
			}
			base.TempData.ContainsKey("message");
			if (asset.GetType() == typeof(MultiFamilyAssetViewModel))
			{
				if (asset.AssetType != AssetType.MHP)
				{
					this.BuildMFViewBags(this.populateMFDetailsCheckBoxList((asset as MultiFamilyAssetViewModel).MFDetails));
				}
				else
				{
					this.BuildMFViewBags(this.populateMHPDetailsCheckBoxList((asset as MultiFamilyAssetViewModel).MHPDetails));
				}
				if ((asset as MultiFamilyAssetViewModel).UnitSpecifications != null)
				{
					(asset as MultiFamilyAssetViewModel).UnitSpecifications = (
						from x in (asset as MultiFamilyAssetViewModel).UnitSpecifications
						orderby x.CountOfUnits
						select x).ToList<AssetUnitSpecification>();
				}
				if ((asset as MultiFamilyAssetViewModel).MHPUnitSpecifications != null)
				{
					(asset as MultiFamilyAssetViewModel).MHPUnitSpecifications = (
						from x in (asset as MultiFamilyAssetViewModel).MHPUnitSpecifications
						orderby x.CountOfUnits
						select x).ToList<AssetMHPSpecification>();
				}
			}
			if (asset.GetType() == typeof(CommercialAssetViewModel))
			{
				Dictionary<string, IEnumerable<SelectListItem>> strs = this.populateCommDetailsCheckBoxList((asset as CommercialAssetViewModel).PropertyDetails, asset.AssetType);
				this.BuildCommViewBags(strs);
			}
			asset.Users.Add(new SelectListItem()
			{
				Text = " ",
				Value = " "
			});
			(
				from c in this._user.GetUsers(new UserSearchModel()
				{
					ShowActiveOnly = true,
                    UserTypeFilter = UserType.CREOwner
                })
				orderby c.CompanyName
				select c).ToList<UserQuickViewModel>().ForEach((UserQuickViewModel f) => asset.Users.Add(new SelectListItem()
			{
				Text =  string.Concat(f.FirstName, " ", f.LastName), //(!string.IsNullOrEmpty(f.CompanyName) ? f.CompanyName :
                    Value = f.UserId.ToString()
			}));
			if (asset.Images == null)
			{
				asset.Images = new List<AssetImage>();
			}
			asset.Images = (
				from x in asset.Images
				orderby x.Order descending
				select x).ToList<AssetImage>();
			if (asset.Documents == null)
			{
				asset.Documents = new List<AssetDocument>();
			}
			if (asset.Videos == null)
			{
				asset.Videos = new List<AssetVideo>();
			}
			if (asset.AssetNARMembers.Count == 0)
			{
				NARMember nARMember = new NARMember()
				{
					NarMemberId = 0,
					IsActive = false
				};
				asset.AssetNARMembers.Add(new AssetNARMember()
				{
					AssetId = Guid.Empty,
					AssetNARMemberId = Guid.Empty,
					NarMemberId = 0,
					NARMember = nARMember
				});
			}
			asset.isPendingForeclosure = (!string.IsNullOrEmpty(asset.ForeclosureLender) || asset.ForeclosureOriginalMortageDate.HasValue || asset.ForeclosureOriginalMortgageAmount.HasValue || asset.ForeclosureRecordDate.HasValue || asset.ForeclosureSaleDate.HasValue ? true : !string.IsNullOrEmpty(asset.ForeclosureRecordNumber));
			asset.Images = (
				from w in asset.Images
				orderby w.Order
				select w).ToList<AssetImage>();
			if (asset.AssetTaxParcelNumbers != null && asset.AssetTaxParcelNumbers.Count == 0)
			{
				asset.AssetTaxParcelNumbers.Add(new AssetTaxParcelNumber());
			}
			foreach (TitleQuickViewModel titleCompany in this._user.GetTitleCompanies(new CompanySearchModel()
			{
				CompanyName = string.Empty,
				CompanyURL = string.Empty,
				State = string.Empty
			}))
			{
				List<SelectListItem> titleCompanies = asset.TitleCompanies;
				SelectListItem selectListItem = new SelectListItem()
				{
					Text = titleCompany.TitleCompName,
					Value = titleCompany.TitleCompanyId.ToString()
				};
				int titleCompanyId = titleCompany.TitleCompanyId;
				int? nullable = asset.TitleCompanyId;
				selectListItem.Selected = titleCompanyId == nullable.GetValueOrDefault(0);
				titleCompanies.Add(selectListItem);
			}
			base.SetUpJsonImages(asset, user);
		}

		private Dictionary<string, string> setupMaintenanceCosts()
		{
			Dictionary<string, string> strs = new Dictionary<string, string>();
			foreach (DeferredMaintenanceCost maintenanceCost in this._asset.GetMaintenanceCosts())
			{
				string str = maintenanceCost.MaintenanceDetail.ToString();
				int cost = maintenanceCost.Cost;
				strs.Add(str, this.addCommas(cost.ToString()));
			}
			return strs;
		}

		public void StartProcess(string id)
		{
			this.progress.Add(id);
			AdminController.ProcessTask processTask = new AdminController.ProcessTask(this.progress.ProcessLongRunningAction);
			processTask.BeginInvoke(id, new AsyncCallback(this.EndProcess), processTask);
		}

		public ActionResult TestSend()
		{
			if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
			{
				this._email.Send(new AutoRegistrationAutoWelcomeEmail()
				{
					Email = "elizabeth.trambulo@inviewlabs.com",
					Password = "teste",
					To = "test"
				});
			}
			return null;
		}

		[Authorize]
		[HttpGet]
		public ActionResult TitleCompanyPage(int id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			TitleCompanyModel titleById = this._user.GetTitleById(id);
			TitleUserSearchResultsModel titleUserSearchResultsModel = new TitleUserSearchResultsModel()
			{
				ControllingUserType = userByUsername.UserType
			};
			TitleUserSearchModel titleUserSearchModel = new TitleUserSearchModel()
			{
				Email = titleUserSearchResultsModel.Email,
				FirstName = titleUserSearchResultsModel.FirstName,
				IsActive = true,
				IsManager = true,
				LastName = titleUserSearchResultsModel.LastName,
				ManagingOfficerName = titleUserSearchResultsModel.ManagingOfficerName,
				Password = titleUserSearchResultsModel.Password,
				TitleCompanyId = new int?(id),
				PhoneNumber = titleUserSearchResultsModel.PhoneNumber
			};
			List<TitleUserQuickViewModel> titleCompanyUsers = this._user.GetTitleCompanyUsers(titleUserSearchModel);
			titleUserSearchResultsModel.TitleUsers = (
				from x in titleCompanyUsers
				where x.IsManager
				select x).ToPagedList<TitleUserQuickViewModel>(1, 50);
			titleUserSearchResultsModel.TitleCompanyUsers = (
				from x in titleCompanyUsers
				where !x.IsManager
				select x).ToPagedList<TitleUserQuickViewModel>(1, 50);
			titleUserSearchResultsModel.FirstName = userByUsername.FirstName;
			titleUserSearchResultsModel.LastName = userByUsername.LastName;
			titleUserSearchResultsModel.ControllingUserType = userByUsername.UserType;
			titleUserSearchResultsModel.TitleCompanyName = titleById.TitleCompName;
			titleUserSearchResultsModel.TitleCompanyURL = titleById.TitleCompURL;
			return base.View(titleUserSearchResultsModel);
		}

		[Authorize]
		public ActionResult TitleManagement(TitleSearchResultsModel model, string sortOrder, int? page)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			model.ControllingUserType = userByUsername.UserType;
			if (userByUsername.UserType != UserType.CorpAdmin && userByUsername.UserType != UserType.SiteAdmin)
			{
				return base.RedirectToAction("MyUSCPage", "Home");
			}
			int num = (base.TempData["RowCount"] != null ? Convert.ToInt32(base.TempData["RowCount"]) : 50);
			base.TempData.Keep("RowCount");
			int? nullable = page;
			int num1 = (nullable.HasValue ? nullable.GetValueOrDefault() : 1);
			CompanySearchModel companySearchModel = new CompanySearchModel()
			{
				CompanyName = model.TitleCompName,
				CompanyURL = model.TitleCompURL,
				State = model.State
			};
			((dynamic)base.ViewBag).CurrentSort = sortOrder;
			base.ViewBag.TitleNameSortParm = (string.IsNullOrEmpty(sortOrder) ? "titlename_desc" : "");
			base.ViewBag.StateSortParm = (sortOrder == "state" ? "state_desc" : "state");
			List<TitleQuickViewModel> titleCompanies = this._user.GetTitleCompanies(companySearchModel);
			if (sortOrder == "titlename_desc")
			{
				model.Titles = (
					from s in titleCompanies
					orderby s.TitleCompName descending
					select s).ToPagedList<TitleQuickViewModel>(num1, num);
			}
			else if (sortOrder == "titlename")
			{
				model.Titles = (
					from s in titleCompanies
					orderby s.TitleCompName
					select s).ToPagedList<TitleQuickViewModel>(num1, num);
			}
			else if (sortOrder == "state_desc")
			{
				model.Titles = (
					from s in titleCompanies
					orderby s.State descending
					select s).ToPagedList<TitleQuickViewModel>(num1, num);
			}
			else if (sortOrder == "state")
			{
				model.Titles = (
					from w in titleCompanies
					orderby w.State
					select w).ToPagedList<TitleQuickViewModel>(num1, num);
			}
			else
			{
				model.Titles = (
					from x in titleCompanies
					orderby x.TitleCompName
					select x).ToPagedList<TitleQuickViewModel>(num1, num);
			}
			return base.View(model);
		}

		[Authorize]
		public ActionResult TitleUserManagement(TitleUserSearchResultsModel model, string sortOrder, int? page)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			model.ControllingUserType = userByUsername.UserType;
			if (userByUsername.UserType != UserType.CorpAdmin && userByUsername.UserType != UserType.SiteAdmin)
			{
				return base.RedirectToAction("MyUSCPage", "Home");
			}
			int num = (base.TempData["RowCount"] != null ? Convert.ToInt32(base.TempData["RowCount"]) : 50);
			base.TempData.Keep("RowCount");
			int? nullable = page;
			int num1 = (nullable.HasValue ? nullable.GetValueOrDefault() : 1);
			TitleUserSearchModel titleUserSearchModel = new TitleUserSearchModel()
			{
				Email = model.Email,
				FirstName = model.FirstName,
				IsActive = model.IsActive,
				IsManager = model.IsManager,
				LastName = model.LastName,
				ManagingOfficerName = model.ManagingOfficerName,
				Password = model.Password,
				PhoneNumber = model.PhoneNumber
			};
			((dynamic)base.ViewBag).CurrentSort = sortOrder;
			base.ViewBag.FirstNameSortParm = (string.IsNullOrEmpty(sortOrder) ? "firstname_desc" : "");
			base.ViewBag.LastNameSortParm = (string.IsNullOrEmpty(sortOrder) ? "lastname_desc" : "");
			List<TitleUserQuickViewModel> titleCompanyUsers = this._user.GetTitleCompanyUsers(titleUserSearchModel);
			if (sortOrder == "firstname_desc")
			{
				model.TitleUsers = (
					from s in titleCompanyUsers
					orderby s.FirstName descending
					select s).ToPagedList<TitleUserQuickViewModel>(num1, num);
			}
			else if (sortOrder == "firstname")
			{
				model.TitleUsers = (
					from s in titleCompanyUsers
					orderby s.FirstName
					select s).ToPagedList<TitleUserQuickViewModel>(num1, num);
			}
			else if (sortOrder == "lastname_desc")
			{
				model.TitleUsers = (
					from s in titleCompanyUsers
					orderby s.LastName descending
					select s).ToPagedList<TitleUserQuickViewModel>(num1, num);
			}
			else if (sortOrder == "lastname")
			{
				model.TitleUsers = (
					from w in titleCompanyUsers
					orderby w.LastName
					select w).ToPagedList<TitleUserQuickViewModel>(num1, num);
			}
			else
			{
				model.TitleUsers = (
					from x in titleCompanyUsers
					orderby x.FirstName
					select x).ToPagedList<TitleUserQuickViewModel>(num1, num);
			}
			return base.View(model);
		}

		[ChildActionOnly]
		public ActionResult TitleUserSideNavigationMenu(string Email)
		{
			return this.PartialView("../Shared/_TitleUserNavigationMenu", this._user.GetTitleUserByEmail(Email));
		}

		[Authorize]
		[HttpPost]
		[MultipleButton(Name="action", Argument="Update")]
		public ActionResult TitleUserUpdateAsset(AssetViewModel model, string mainImg, string flyerImg)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			TitleCompanyUserModel titleUserByEmail = this._user.GetTitleUserByEmail(this._user.GetUserByUsername(base.User.Identity.Name).Username);
			((dynamic)base.ViewBag).IsCorpAdmin = this.isUserAdmin(userByUsername);
			base.ViewBag.IsICAdmin = (userByUsername.UserType == UserType.ICAdmin ? true : false);
			((dynamic)base.ViewBag).Email = titleUserByEmail.Email;
			if (model.Documents == null)
			{
				model.Documents = new List<AssetDocument>();
			}
			int num = 0;
			for (int i = 0; i < model.Documents.Count; i++)
			{
				model.Documents[i].AssetDocumentId = base.generatedGuidIfNone(model.Documents[i].AssetDocumentId);
				bool flag = false;
				do
				{
					if (i >= model.Documents.Count)
					{
						break;
					}
					if (model.Documents[i].FileName != null)
					{
						model.Documents[i].Order = num;
						this._asset.MarkDocumentViewingStatus(model.Documents[i], false);
						num++;
						flag = false;
					}
					else
					{
						try
						{
							model.Documents.RemoveAt(i);
							flag = true;
						}
						catch
						{
						}
					}
				}
				while (flag);
			}
			if (!this._asset.UpdateAssetDocuments(model.Documents, model.AssetId))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problem saving. Please contact site admin.");
				base.RedirectToAction("CompleteAsset", "admin", new { id = model.AssetId });
			}
			else
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully saved.");
				this._asset.UnlockAsset(model.AssetId);
			}
			this._user.UpdateOrderStatus(model.AssetNumber, OrderStatus.Pending, 0);
			return base.RedirectToAction("AccessAsset", "Admin");
		}

		public static Guid ToGuid(int value)
		{
			byte[] numArray = new byte[16];
			BitConverter.GetBytes(value).CopyTo(numArray, 0);
			return new Guid(numArray);
		}

		[HttpPost]
		public ActionResult UnlockAsset(Guid assetId)
		{
			this._asset.UnlockAsset(assetId);
			return null;
		}

	

		[Authorize]
		[HttpGet]
		public ActionResult UpdateAsset(Guid id)
		{
            UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			if (userByUsername.UserType != UserType.CorpAdmin && userByUsername.UserType != UserType.CorpAdmin2 && userByUsername.UserType != UserType.ICAdmin && userByUsername.UserType != UserType.Investor && userByUsername.UserType != UserType.ListingAgent)
			{
				return base.RedirectToAction("Index", "Home");
			}
			if (userByUsername.UserType == UserType.ICAdmin)
			{
				if (!userByUsername.SignedICAgreement)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "We have not received your signed IC Agreement. Please check your email or execute another IC Agreement below.");
					return base.RedirectToAction("ExecuteICAgreement", "DataPortal");
				}
				if (this._user.HasPendingICAgreement(userByUsername.UserId))
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Your IC Agreement is still pending.");
					return base.RedirectToAction("MyUSCPage", "Home");
				}
				ICStatus? cStatus = userByUsername.ICStatus;
				if ((cStatus.GetValueOrDefault() == ICStatus.Rejected ? cStatus.HasValue : false))
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Your IC Agreement has been rejected.");
					return base.RedirectToAction("MyUSCPage", "Home");
				}
				cStatus = userByUsername.ICStatus;
				if ((cStatus.GetValueOrDefault() == ICStatus.Approved ? !cStatus.HasValue : true))
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Your IC Agreement is still pending approval.");
					return base.RedirectToAction("MyUSCPage", "Home");
				}
			}
			base.ViewBag.IsSeller = (userByUsername.UserType == UserType.ListingAgent ? true : userByUsername.UserType == UserType.Investor);
			base.ViewBag.IsCorpAdmin = (this.isUserAdmin(userByUsername) || userByUsername.UserType == UserType.ListingAgent ? true : userByUsername.UserType == UserType.Investor);
			base.ViewBag.IsICAdmin = (userByUsername.UserType == UserType.ICAdmin ? true : false);
			if (!((dynamic)base.ViewBag).IsSeller)
			{
				((dynamic)base.ViewBag).Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
			}
			else
			{
				((dynamic)base.ViewBag).Layout = "~/Views/Shared/_Layout.cshtml";
			}
			if (!this._asset.IsAssetLocked(id, userByUsername.UserId))
			{
				((dynamic)base.ViewBag).VideoRoot = Path.Combine(ConfigurationManager.AppSettings["DataRoot"], "Videos", id.ToString());
				AssetViewModel asset = this._asset.GetAsset(id, false);

                if (asset.Documents != null)
                    asset = _asset.PopulateDocumentOrder(asset);

                asset.FromCreateMethod = false;
				asset.DateForTempImages = DateTime.Now.ToString("yyyyMMdd");
				asset.UserId = userByUsername.UserId.ToString();
                asset.Countries = GetAllCountries();

                // current owner
                if (asset.OwnerHoldingCompanyId.HasValue)
                {
                    if (asset.OwnerHoldingCompanyId.Value == Guid.Empty)
                    {
                        asset.IsOwnerHoldingCompanyDataNotAvailable = true;
                    }
                    else
                    {
                        var ownerCo = _user.GetHoldingCompany(asset.OwnerHoldingCompanyId.Value);
                        if (ownerCo != null)
                        {
                            asset.OwnerHoldingCompanyUpdateId = ownerCo.HoldingCompanyId.ToString();
                            asset.OwnerHoldingCompanyUpdate = ownerCo.CompanyName;
                            asset.OwnerHoldingCompanyUpdateFirst = ownerCo.FirstName;
                            asset.OwnerHoldingCompanyUpdateLast = ownerCo.LastName;
                            asset.OwnerHoldingCompanyUpdateEmail = ownerCo.Email;
                            asset.OwnerHoldingCompanyUpdateAddressLine1 = ownerCo.AddressLine1;
                            asset.OwnerHoldingCompanyUpdateAddressLine2 = ownerCo.AddressLine2;
                            asset.OwnerHoldingCompanyUpdateCity = ownerCo.City;
                            asset.OwnerHoldingCompanyUpdateState = ownerCo.State;
                            asset.OwnerHoldingCompanyUpdateZip = ownerCo.Zip;
                            asset.OwnerHoldingCompanyUpdateCountry = ownerCo.Country;
                            asset.OwnerHoldingCompanyUpdateWorkPhone = ownerCo.WorkNumber;
                            asset.OwnerHoldingCompanyUpdateCellPhone = ownerCo.CellNumber;
                            asset.OwnerHoldingCompanyUpdateFax = ownerCo.FaxNumber;
                            asset.OwnerHoldingCompanyUpdateIsActive = ownerCo.IsActive;
                        }
                    }
                }
                if (asset.OwnerOperatingCompanyId.HasValue)
                {
                    if (asset.OwnerOperatingCompanyId.Value == Guid.Empty)
                    {
                        asset.IsOwnerOperatingCompanyDataNotAvailable = true;
                    }
                    else
                    {
                        // retrieve the PI for this company
                        var operatingCompany = _user.GetOperatingCompany(asset.OwnerOperatingCompanyId.Value);
                        if (operatingCompany != null)
                        {
                            asset.OwnerOperatingCompanyUpdateId = operatingCompany.OperatingCompanyId.ToString();
                            asset.OwnerOperatingCompanyUpdate = operatingCompany.CompanyName;
                            asset.OwnerOperatingCompanyUpdateFirst = operatingCompany.FirstName;
                            asset.OwnerOperatingCompanyUpdateLast = operatingCompany.LastName;
                            asset.OwnerOperatingCompanyUpdateEmail = operatingCompany.Email;
                            asset.OwnerOperatingCompanyUpdateAddressLine1 = operatingCompany.AddressLine1;
                            asset.OwnerOperatingCompanyUpdateAddressLine2 = operatingCompany.AddressLine2;
                            asset.OwnerOperatingCompanyUpdateCity = operatingCompany.City;
                            asset.OwnerOperatingCompanyUpdateState = operatingCompany.State;
                            asset.OwnerOperatingCompanyUpdateZip = operatingCompany.Zip;
                            asset.OwnerOperatingCompanyUpdateCountry = operatingCompany.Country;
                            asset.OwnerOperatingCompanyUpdateWorkPhone = operatingCompany.WorkNumber;
                            asset.OwnerOperatingCompanyUpdateCellPhone = operatingCompany.CellNumber;
                            asset.OwnerOperatingCompanyUpdateFax = operatingCompany.FaxNumber;
                            asset.OwnerOperatingCompanyUpdateIsActive = operatingCompany.IsActive;
                        }
                    }
                }
                
                this.SetUpAsset(asset, userByUsername);
				asset.User = this._user.GetUserEntity(base.User.Identity.Name);

				
				asset.AssetHCOwnershipLst = this._asset.GetAssetHCByAssetId(id);
				asset.AssetOCLst = this._asset.GetAssetOCByAssetId(id);

				return base.View(asset);
			}
			AssetViewModel assetViewModel = new AssetViewModel()
			{
				FromCreateMethod = false,
				Images = new List<AssetImage>(),
				Documents = new List<AssetDocument>(),
				Videos = new List<AssetVideo>()
			};
			base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "This asset is currently locked");
			((dynamic)base.ViewBag).VideoRoot = Path.Combine(ConfigurationManager.AppSettings["DataRoot"], "Videos", id.ToString());
			assetViewModel.User = this._user.GetUserEntity(base.User.Identity.Name);

			return base.View(assetViewModel);
		}

		[Authorize]
		[HttpPost]
		[MultipleButton(Name="action", Argument="Save")]
		public ActionResult UpdateAsset(AssetViewModel model, string mainImg, string flyerImg)
		{
           
			int userId;
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			model.User = this._user.GetUserEntity(base.User.Identity.Name);
			base.ViewBag.IsSeller = (userByUsername.UserType == UserType.ListingAgent ? true : userByUsername.UserType == UserType.Investor);
			base.ViewBag.IsCorpAdmin = (this.isUserAdmin(userByUsername) || userByUsername.UserType == UserType.ListingAgent ? true : userByUsername.UserType == UserType.Investor);
			base.ViewBag.IsICAdmin = (userByUsername.UserType == UserType.ICAdmin ? true : false);
			if (!((dynamic)base.ViewBag).IsSeller)
			{
				((dynamic)base.ViewBag).Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
			}
			else
			{
				((dynamic)base.ViewBag).Layout = "~/Views/Shared/_Layout.cshtml";
			}
			if (!((dynamic)base.ViewBag).IsSeller)
			{
				((dynamic)base.ViewBag).Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
			}
			else
			{
				((dynamic)base.ViewBag).Layout = "~/Views/Shared/_Layout.cshtml";
			}
			foreach (System.Web.Mvc.ModelState value in base.ModelState.Values)
			{
				value.Errors.Clear();
			}
			bool flag = false;

            string invalid = ValidateAssetDocuments(model);
            if (invalid.Length > 0)
            {
                base.ModelState.AddModelError("", "The required documents are not present, please upload: " + invalid);
                return base.View("UpdateAsset", model);
            }
            else if (model.Documents != null)
                model = _asset.PopulateDocumentOrder(model);

            if (model.PropHoldTypeId == 2 && !model.LeaseholdMaturityDate.HasValue) {
                ModelState.AddModelError("LeaseholdMaturityDate", "Leasehold maturity date required");
                flag = true;
            }
            foreach (AssetTaxParcelNumber assetTaxParcelNumber in model.AssetTaxParcelNumbers)
			{
                if (this._asset.GetMatchingAssetsByAPNCountyState(assetTaxParcelNumber.TaxParcelNumber, model.State, model.County).All(x => x.AssetId == model.AssetId))
				{
					continue;
				}
				base.ModelState.AddModelError("TaxAssessorNumber", string.Concat("Tax Assessor Number ", assetTaxParcelNumber.TaxParcelNumber, " is already in use for another asset"));
				flag = true;
			}
			for (int i = 0; i < model.AssetNARMembers.Count; i++)
			{
				if (model.AssetNARMembers[i].NARMember.NotOnList)
				{
					if (string.IsNullOrEmpty(model.AssetNARMembers[i].NARMember.Email))
					{
						base.ModelState.AddModelError(string.Concat("AssetNARMembers[", i, "].NARMember.Email"), "Listing Agent Email Required");
						flag = true;
					}
					if (string.IsNullOrEmpty(model.AssetNARMembers[i].NARMember.FirstName))
					{
						base.ModelState.AddModelError(string.Concat("AssetNARMembers[", i, "].NARMember.FirstName"), "Listing Agent First Name Required");
						flag = true;
					}
					if (string.IsNullOrEmpty(model.AssetNARMembers[i].NARMember.LastName))
					{
						base.ModelState.AddModelError(string.Concat("AssetNARMembers[", i, "].NARMember.LastName"), "Listing Agent Last Name Required");
						flag = true;
					}
				}
			}

            var updateOwnershipResult = SetAssetOwnership(model);
            // only update the flag in this scope *IF* setAssetOwnership returns true(an error)
            if (updateOwnershipResult.Item1) flag = updateOwnershipResult.Item1;


            List<string> strs = new List<string>();
			List<int> nums = new List<int>();
			if (!flag)
			{
				for (int j = 0; j < model.AssetNARMembers.Count; j++)
				{
					if (model.AssetNARMembers[j].NARMember.NotOnList)
					{
						if (model.AssetNARMembers[j].NARMember.NarMemberId == 0)
						{
						}
					}
					else if (model.AssetNARMembers[j].NARMember.Email != null)
					{
						strs.Add(model.AssetNARMembers[j].NARMember.Email);
						if ((
							from temp in strs
							where temp.Equals(model.AssetNARMembers[j].NARMember.Email)
							select temp).Count<string>() > 1)
						{
							nums.Add(j);
						}
						if (nums.Count<int>() > 0)
						{
							base.ModelState.AddModelError(string.Concat("AssetNARMembers[", j, "].NARMember.Email"), string.Concat(new string[] { "Listing Agent ", model.AssetNARMembers[j].NARMember.FirstName, " ", model.AssetNARMembers[j].NARMember.LastName, " email ", model.AssetNARMembers[j].NARMember.Email, " already exist as one of the entries." }));
							flag = true;
						}
					}
				}
			}
			if (model.Images == null)
			{
				model.Images = new List<AssetImage>();
			}
			if (model.Documents == null)
			{
				model.Documents = new List<AssetDocument>();
			}
			if (model.Videos == null)
			{
				model.Videos = new List<AssetVideo>();
			}
			int num = 0;
			List<AssetImage> list = (
				from s in model.Images
				orderby s.Order
				select s).ToList<AssetImage>();
			for (int l = 0; l < list.Count; l++)
			{
				bool flag2 = false;
				do
				{
					if (l >= list.Count)
					{
						break;
					}
					if (string.IsNullOrEmpty(list[l].ContentType))
					{
						list.RemoveAt(l);
						flag2 = true;
					}
					else if (!"image/jpeg,image/png,image/gif".Contains(list[l].ContentType.ToLower()))
					{
						list.RemoveAt(l);
						flag2 = true;
					}
					else
					{
						if (list[l].AssetImageId == Guid.Empty)
						{
							list[l].OriginalFileName = list[l].FileName;
							list[l].AssetImageId = base.generatedGuidIfNone(list[l].AssetImageId);
							IFileManager fileManager = this._fileManager;
							string fileName = list[l].FileName;
							Guid assetId = model.AssetId;
							string dateForTempImages = model.DateForTempImages;
							userId = userByUsername.UserId;
							string str = fileManager.MoveTempAssetImage(fileName, assetId, dateForTempImages, userId.ToString());
							if (!string.IsNullOrEmpty(str))
							{
								list[l].FileName = str;
							}
						}
						if (list[l].FileName != null)
						{
							list[l].AssetId = model.AssetId;
							list[l].Order = num;
							num++;
							flag2 = false;
						}
						else
						{
							try
							{
								list.RemoveAt(l);
								flag2 = true;
							}
							catch
							{
							}
						}
					}
				}
				while (flag2);
			}
			model.Images = list;
			num = 0;

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
                    case (int)AssetDocumentType.EnvironmentalReport:
                        model.AvailableEnvironmentalRep = true;
                        break;

                }
            }

            for (int m = 0; m < model.Documents.Count; m++)
			{
				bool flag3 = false;
				do
				{
					if (m >= model.Documents.Count)
					{
						break;
					}
					if (model.Documents[m].FileName != null)
					{
						model.Documents[m].AssetDocumentId = base.generatedGuidIfNone(model.Documents[m].AssetDocumentId);
						model.Documents[m].Order = num;
						num++;
						flag3 = false;
					}
					else
					{
						try
						{
							model.Documents.RemoveAt(m);
							flag3 = true;
						}
						catch
						{
						}
					}
				}
				while (flag3);
			}
			bool flag4 = true;
			if (model.AssetNumber == 0)
			{
				model.CreationDate = new DateTime?(DateTime.Now);
				model.IsActive = true;
				model.ListedByUserId = userByUsername.UserId;
				flag4 = false;
			}
			int? nullable = null;
			if (!string.IsNullOrEmpty(model.TitleCompany))
			{
				int num1 = 0;
				if (int.TryParse(model.TitleCompany, out num1))
				{
					model.TitleCompanyId = new int?(num1);
				}
			}
            // Create new PI if Not in List
            if (!flag && !string.IsNullOrEmpty(model.OwnerOperatingCompanyEmail) && IsValidEmail(model.OwnerOperatingCompanyEmail))
            { 
                var pi = _user.GetPrincipalInvestorByEmail(model.OwnerOperatingCompanyEmail);
                var piqvm = new PrincipalInvestorQuickViewModel
                {
                    CompanyName = model.OwnerOperatingCompany,
                    FirstName = model.OwnerOperatingCompanyFirst,
                    LastName = model.OwnerOperatingCompanyLast,
                    Email = model.OwnerOperatingCompanyEmail,
                    AddressLine1 = model.OwnerOperatingCompanyAddressLine1,
                    AddressLine2 = model.OwnerOperatingCompanyAddressLine2,
                    City = model.OwnerOperatingCompanyCity,
                    State = model.OwnerOperatingCompanyState,
                    Zip = model.OwnerOperatingCompanyZip,
                    Country = model.OwnerOperatingCompanyCountry,
                    CellPhoneNumber = model.OwnerOperatingCompanyCellPhone,
                    WorkNumber = model.OwnerOperatingCompanyWorkPhone,
                    FaxNumber = model.OwnerOperatingCompanyFax, 
                    IsActive = true,
                    IsASeller = false,
                    DoesPIHaveSellerPrivilege = false
                };
                if (pi != null)
                {
                    piqvm.PrincipalInvestorId = pi.PrincipalInvestorId;
                    piqvm.IsActive = pi.IsActive;
                    piqvm.IsASeller = pi.IsASeller;
                    piqvm.DoesPIHaveSellerPrivilege = pi.DoesPIHaveSellerPrivilege;
                    _user.UpdatePrincipalInvestor(piqvm);
                }
                else {
                    _user.CreatePrincipalInvestor(piqvm);
                }
            }
            model.NotOnOperatingList = false;

            if (model.GetType() == typeof(MultiFamilyAssetViewModel))
			{
				MultiFamilyAssetViewModel multiFamilyAssetViewModel = model as MultiFamilyAssetViewModel;
				string.Format("A {0} unit {1} property in {2}, {3}", new object[] { multiFamilyAssetViewModel.TotalUnits, EnumHelper.GetEnumDescription(model.AssetType), model.City, model.State });
				if (multiFamilyAssetViewModel.UnitSpecifications != null)
				{
					for (int n = 0; n < multiFamilyAssetViewModel.UnitSpecifications.Count; n++)
					{
						multiFamilyAssetViewModel.UnitSpecifications[n].AssetUnitSpecificationId = base.generatedGuidIfNone(multiFamilyAssetViewModel.UnitSpecifications[n].AssetUnitSpecificationId);
						if (multiFamilyAssetViewModel.UnitSpecifications[n].AssetId == Guid.Empty)
						{
							multiFamilyAssetViewModel.UnitSpecifications[n].AssetId = model.AssetId;
						}
					}
				}
				if (multiFamilyAssetViewModel.MHPUnitSpecifications != null)
				{
					for (int o = 0; o < multiFamilyAssetViewModel.MHPUnitSpecifications.Count; o++)
					{
						multiFamilyAssetViewModel.MHPUnitSpecifications[o].AssetMHPSpecificationId = base.generatedGuidIfNone(multiFamilyAssetViewModel.MHPUnitSpecifications[o].AssetMHPSpecificationId);
						if (multiFamilyAssetViewModel.MHPUnitSpecifications[o].AssetId == Guid.Empty)
						{
							multiFamilyAssetViewModel.MHPUnitSpecifications[o].AssetId = model.AssetId;
						}
					}
				}
				if (model.AssetType != AssetType.MHP)
				{
					this.BuildMFViewBags(this.populateMFDetailsCheckBoxList((model as MultiFamilyAssetViewModel).MFDetails));
				}
				else
				{
					this.BuildMFViewBags(this.populateMHPDetailsCheckBoxList((model as MultiFamilyAssetViewModel).MHPDetails));
				}
				if (flag)
				{
                    model.Countries = GetAllCountries();
                    base.SetUpJsonImages(multiFamilyAssetViewModel, userByUsername);
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problems with data input fields. Please review below");
					return base.View("UpdateAsset", multiFamilyAssetViewModel);
				}
				if (!flag4)
				{
					nullable = this._asset.CreateAssetByViewModel(multiFamilyAssetViewModel);
					if (!nullable.HasValue)
					{
						base.SetUpJsonImages(multiFamilyAssetViewModel, userByUsername);
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problem saving. Please contact site admin.");
						return base.View("UpdateAsset", multiFamilyAssetViewModel);
					}
					base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully saved.");
					this._asset.UnlockAsset(model.AssetId);
				}
				else
				{
					if (!this._asset.UpdateAssetByViewModel(multiFamilyAssetViewModel))
					{
						base.SetUpJsonImages(multiFamilyAssetViewModel, userByUsername);
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problem saving. Please contact site admin.");
						return base.View("UpdateAsset", multiFamilyAssetViewModel);
					}
                    CreateOwnershipRecordIfApplicable(model, updateOwnershipResult.Item2);

                    base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully saved.");
					this._asset.UnlockAsset(model.AssetId);
				}
			}
			if (model.GetType() == typeof(CommercialAssetViewModel))
			{
				CommercialAssetViewModel commercialAssetViewModel = model as CommercialAssetViewModel;
				string.Format("A {0} sq. ft. {1} property in {2}, {3}", new object[] { commercialAssetViewModel.LotSize, EnumHelper.GetEnumDescription(model.AssetType), model.City, model.State });
				Dictionary<string, IEnumerable<SelectListItem>> strs1 = this.populateCommDetailsCheckBoxList((model as CommercialAssetViewModel).PropertyDetails, model.AssetType);
				this.BuildCommViewBags(strs1);
				if (flag)
				{
					base.SetUpJsonImages(commercialAssetViewModel, userByUsername);
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problems with data input fields. Please review below");
					return base.View("UpdateAsset", commercialAssetViewModel);
				}
				if (!flag4)
				{
					nullable = this._asset.CreateAssetByViewModel(commercialAssetViewModel);
					if (!nullable.HasValue)
					{
						base.SetUpJsonImages(commercialAssetViewModel, userByUsername);
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problem saving. Please contact site admin.");
						return base.View("UpdateAsset", commercialAssetViewModel);
					}
					base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully saved.");
					this._asset.UnlockAsset(model.AssetId);
				}
				else
				{
					if (!this._asset.UpdateAssetByViewModel(commercialAssetViewModel))
					{
						base.SetUpJsonImages(commercialAssetViewModel, userByUsername);
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problem saving. Please contact site admin.");
						return base.View("UpdateAsset", commercialAssetViewModel);
					}
                    CreateOwnershipRecordIfApplicable(model, updateOwnershipResult.Item2);

                    base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully saved.");
					this._asset.UnlockAsset(model.AssetId);
				}
			}
			if (model.ExistingListingStatus != model.ListingStatus)
			{
				Tuple<string, string> userInformationForHeldAsset = this._asset.GetUserInformationForHeldAsset(model.AssetId);
				if (userInformationForHeldAsset != null)
				{
					try
					{
						if (base.CanSendAutoEmail(BaseController.AutoEmailType.Asset))
						{
							IEPIFundEmailService ePIFundEmailService = this._email;
							AssetStatusChangeReportEmail assetStatusChangeReportEmail = new AssetStatusChangeReportEmail()
							{
								ViewName = "AssetStatusChangeReportSpecificPrincipal",
								RecipientEmail = userInformationForHeldAsset.Item2,
								RecipientName = userInformationForHeldAsset.Item1
							};
							List<AssetChangeStatusModel> assetChangeStatusModels = new List<AssetChangeStatusModel>();
							AssetChangeStatusModel assetChangeStatusModel = new AssetChangeStatusModel()
							{
								AssetDescription = this._asset.GetAssetByAssetId(model.AssetId).Description
							};
							userId = model.AssetNumber;
							assetChangeStatusModel.AssetId = userId.ToString();
							assetChangeStatusModel.NewStatus = EnumHelper.GetEnumDescription(model.ListingStatus);
							assetChangeStatusModel.OriginalStatus = EnumHelper.GetEnumDescription(model.ExistingListingStatus);
							assetChangeStatusModels.Add(assetChangeStatusModel);
							assetStatusChangeReportEmail.AssetChanges = assetChangeStatusModels;
							ePIFundEmailService.Send(assetStatusChangeReportEmail);
						}
					}
					catch
					{
					}
				}
				else if (base.CanSendAutoEmail(BaseController.AutoEmailType.Asset))
				{
					List<UserQuickViewModel> users = this._user.GetUsers(new UserSearchModel()
					{
						UserTypeFilter = new UserType?(UserType.CREBroker),
						ShowActiveOnly = true
					});
					users.AddRange(this._user.GetUsers(new UserSearchModel()
					{
						UserTypeFilter = new UserType?(UserType.CREOwner),
						ShowActiveOnly = true
					}));
					users.AddRange(this._user.GetUsers(new UserSearchModel()
					{
						UserTypeFilter = new UserType?(UserType.CRELender),
						ShowActiveOnly = true
					}));
					users.AddRange(this._user.GetUsers(new UserSearchModel()
					{
						UserTypeFilter = new UserType?(UserType.Investor),
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
							IEPIFundEmailService ePIFundEmailService1 = this._email;
							AssetStatusChangeReportEmail assetStatusChangeReportEmail1 = new AssetStatusChangeReportEmail()
							{
								ViewName = "AssetStatusChangeReportGeneralPopulation",
								RecipientEmail = user.Username,
								RecipientName = user.FullName
							};
							List<AssetChangeStatusModel> assetChangeStatusModels1 = new List<AssetChangeStatusModel>();
							AssetChangeStatusModel enumDescription = new AssetChangeStatusModel();
							userId = model.AssetNumber;
							enumDescription.AssetId = userId.ToString();
							enumDescription.NewStatus = EnumHelper.GetEnumDescription(model.ListingStatus);
							enumDescription.OriginalStatus = EnumHelper.GetEnumDescription(model.ExistingListingStatus);
							enumDescription.AssetDescription = this._asset.GetAssetByAssetId(model.AssetId).Description;
							assetChangeStatusModels1.Add(enumDescription);
							assetStatusChangeReportEmail1.AssetChanges = assetChangeStatusModels1;
							ePIFundEmailService1.Send(assetStatusChangeReportEmail1);
						}
						catch
						{
						}
					}
				}
			}
			bool fromCreateMethod = model.FromCreateMethod;
            if(userByUsername.UserType == UserType.ICAdmin)
            {
                userId = model.AssetNumber;
                return base.RedirectToAction("ICACache", "ICA", new { assetNumber = userId.ToString() });
            }
			else if (userByUsername.UserType != UserType.CREBroker && userByUsername.UserType != UserType.CRELender && userByUsername.UserType != UserType.Investor)
			{
				userId = model.AssetNumber;
				return base.RedirectToAction("ManageAssets", "Admin", new { assetNumber = userId.ToString() });
			}
			userId = model.AssetNumber;
			return base.RedirectToAction("SellerManageAssets", "Investors", new { assetNumber = userId.ToString() });
		}

        [HttpPost]
		[MultipleButton(Name="action", Argument="UpdateImages")]
		public ActionResult UpdateImages(AssetViewModel model)
		{
			Guid assetId;
			if (base.TempData[string.Concat(model.AssetId.ToString(), "Images")] == null)
			{
				TempDataDictionary tempData = base.TempData;
				assetId = model.AssetId;
				tempData[string.Concat(assetId.ToString(), "Images")] = new List<HttpPostedFileBase>();
			}
			TempDataDictionary tempDataDictionaries = base.TempData;
			assetId = model.AssetId;
			object item = tempDataDictionaries[string.Concat(assetId.ToString(), "Images")];
			return base.Content("");
		}

		[Authorize]
		[HttpGet]
		public ViewResult UpdateMba(int id)
		{
			return base.View(this._user.GetMbaById(id));
		}

		[HttpPost]
		public ActionResult UpdateMba(MbaViewModel model)
		{
			try
			{
				this._user.UpdateMba(model);
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "MBA successfully updated.");
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, exception.Message);
			}
			return base.View(model);
		}

		[Authorize]
		[HttpGet]
		public ViewResult UpdateNARMember(int id)
		{
			return base.View(this._user.GetNarMember(id));
		}

		[HttpPost]
		public ActionResult UpdateNARMember(NarMemberViewModel model)
		{
			try
			{
				this._user.UpdateNarMember(model);
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "NAR Member successfully updated.");
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, exception.Message);
			}
			return base.View(model);
		}

		[Authorize]
		[HttpGet]
		public ViewResult UpdatePrincipalInvestor(int id)
		{
			return base.View(this._user.GetPrincipalInvestor(id));
		}

		[HttpPost]
		public ActionResult UpdatePrincipalInvestor(PrincipalInvestorQuickViewModel model)
		{
			try
			{
				this._user.UpdatePrincipalInvestor(model);
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Principal Investor successfully updated.");
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, exception.Message);
			}
			return base.View(model);
		}



       

        [HttpGet]
		public ActionResult UpdateTitleCompanyUser(int id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			TitleCompanyUserModel titleUserById = this._user.GetTitleUserById(id);
			if (userByUsername == null)
			{
				((dynamic)base.ViewBag).Email = string.Empty;
				titleUserById.IsActive = true;
			}
			else
			{
				titleUserById.ControllingUserType = userByUsername.UserType;
				((dynamic)base.ViewBag).Email = userByUsername.Username;
			}
			return base.View(titleUserById);
		}

		[HttpPost]
		public ActionResult UpdateTitleCompanyUser(TitleCompanyUserModel model)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name) ?? this._user.GetUserByUsername(model.Email);
			if (!base.ModelState.IsValid)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid fields.");
			}
			else
			{
				Tuple<bool, string> tuple = this._user.ValidateTitleUserStateAvailability(model);
				if (!tuple.Item1)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, tuple.Item2);
				}
				else
				{
					if (userByUsername.UserType == UserType.TitleCompUser && model.IsManager)
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You are not authorized to change the status to a manager");
						((dynamic)base.ViewBag).Email = model.Email;
						model.ControllingUserType = userByUsername.UserType;
						return base.View(model);
					}
					this._user.UpdateTitleUser(model);
					((dynamic)base.ViewBag).Email = userByUsername.Username;
					base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Title Company user successfully updated.");
				}
			}
			model.ControllingUserType = userByUsername.UserType;
			if (!base.User.Identity.IsAuthenticated)
			{
				((dynamic)base.ViewBag).Email = string.Empty;
			}
			else
			{
				((dynamic)base.ViewBag).Email = userByUsername.Username;
			}
			return base.View(model);
		}

		public ActionResult UploadImages(Guid? assetId, string userId, string dateString)
		{
			ActionResult fileUploadJsonResult;
			string empty = string.Empty;
			bool flag = true;
			string str = "";
			try
			{
				foreach (string file in base.Request.Files)
				{
					HttpPostedFileBase item = base.Request.Files[file];
					if (item == null || item.ContentLength <= 0)
					{
						continue;
					}
					if (item == null || string.IsNullOrEmpty(item.FileName) || !assetId.HasValue)
					{
						fileUploadJsonResult = new FileUploadJsonResult()
						{
							Data = new { message = "FileIsNull" }
						};
						return fileUploadJsonResult;
					}
					else if (item.ContentType.ToLower() != "image/jpeg" && item.ContentType.ToLower() != "image/jpg" && item.ContentType.ToLower() != "image/gif" && item.ContentType.ToLower() != "image/png")
					{
						fileUploadJsonResult = new FileUploadJsonResult()
						{
							Data = new { message = "InvalidImageType" }
						};
						return fileUploadJsonResult;
					}
					else if (!this._fileManager.SaveTempFile(item, assetId.Value, FileType.TempImage, dateString, userId))
					{
						fileUploadJsonResult = new FileUploadJsonResult()
						{
							Data = new { message = "false" }
						};
						return fileUploadJsonResult;
					}
					else
					{
						fileUploadJsonResult = new FileUploadJsonResult()
						{
							Data = new { message = "true", contentType = item.ContentType, isCorpAdmin = this.isUserAdmin(this.AuthenticatedUser), isICAdmin = (this.AuthenticatedUser.UserType == UserType.ICAdmin ? true : false) }
						};
						return fileUploadJsonResult;
					}
				}
				if (flag)
				{
					return base.Json(new { Message = str });
				}
				return base.Json(new { Message = string.Concat("Error in saving file. ", empty) });
			}
			catch (Exception exception)
			{
				flag = false;
				empty = exception.Message;
				if (flag)
				{
					return base.Json(new { Message = str });
				}
				return base.Json(new { Message = string.Concat("Error in saving file. ", empty) });
			}
			return fileUploadJsonResult;
		}

		[Authorize]
		[HttpGet]
		public ViewResult UploadUserFile(int id)
		{
			return base.View(new UploadUserFileModel()
			{
				UserId = id
			});
		}

		[HttpPost]
		public ActionResult UploadUserFile(UploadUserFileModel model)
		{
			if (string.IsNullOrEmpty(model.FileTitle) || model.UploadedDocument.ContentLength <= 0 || !(model.UploadedDocument.ContentType == "application/pdf"))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid file or name");
				return base.View(model);
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				model.UploadedDocument.InputStream.CopyTo(memoryStream);
				this._asset.SaveUserFile(memoryStream.ToArray(), model.FileTitle, model.UserId);
			}
			return base.RedirectToAction("EditUser", "Admin", new { id = model.UserId });
		}

		[Authorize]
		public ActionResult UserManagement(UserSearchResultsModel model, string sortOrder, int? page)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			model.ControllingUserType = userByUsername.UserType;
			int num = (base.TempData["RowCount"] != null ? Convert.ToInt32(base.TempData["RowCount"]) : 50);
			base.TempData.Keep("RowCount");
			int? nullable = page;
			int num1 = (nullable.HasValue ? nullable.GetValueOrDefault() : 1);
			List<UserType> userTypes = new List<UserType>()
			{
				UserType.CRELender,
				UserType.CREBroker
			};
			UserSearchModel userSearchModel = new UserSearchModel()
			{
				FirstName = model.FirstName,
				LastName = model.LastName,
				UserTypeFilter = model.SelectedUserType,
				ControllingUserType = userByUsername.UserType,
				City = model.City,
				State = model.State,
				ShowActiveOnly = false,
				ExcludedUsers = userTypes
			};
			PrincipalInvestorSearchModel principalInvestorSearchModel = new PrincipalInvestorSearchModel()
			{
				AddressLine1 = model.AddressLine1,
				City = model.City,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				State = model.State,
				Zip = model.Zip,
				ShowActiveOnly = model.ShowActiveOnly,
				CompanyName = model.CompanyName
			};
			((dynamic)base.ViewBag).CurrentSort = sortOrder;
			base.ViewBag.FirstNameSortParm = (string.IsNullOrEmpty(sortOrder) ? "firstname_desc" : "");
			base.ViewBag.LastNameSortParm = (string.IsNullOrEmpty(sortOrder) ? "lastname_desc" : "");
			base.ViewBag.TypeSortParm = (sortOrder == "usertype" ? "usertype_desc" : "usertype");
			base.ViewBag.CitySortParm = (sortOrder == "city" ? "city_desc" : "city");
			base.ViewBag.StateSortParm = (sortOrder == "state" ? "state_desc" : "state");
			base.ViewBag.CompanyNameSortParm = (sortOrder == "company" ? "company_desc" : "company");
			base.ViewBag.AddressSortParm = (sortOrder == "address" ? "address_desc" : "address");
			base.ViewBag.EmailSortParm = (sortOrder == "email" ? "email_desc" : "email");
			base.ViewBag.IsActiveParm = (sortOrder == "active" ? "active_desc" : "active");
			List<PrincipalInvestorQuickViewModel> list = this._user.GetPrincipalInvestors(principalInvestorSearchModel).ToList<PrincipalInvestorQuickViewModel>();
			List<UserQuickViewModel> users = this._user.GetUsers(userSearchModel);
			users = (
				from w in users
				where w.UserType != UserType.ICAdmin
				select w).ToList<UserQuickViewModel>();
			switch (sortOrder)
			{
				case "firstname_desc":
				{
					model.Users = (
						from s in users
						orderby s.FirstName descending
						select s).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				case "firstname":
				{
					model.Users = (
						from s in users
						orderby s.FirstName
						select s).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				case "lastname_desc":
				{
					model.Users = (
						from s in users
						orderby s.LastName descending
						select s).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				case "lastname":
				{
					model.Users = (
						from s in users
						orderby s.LastName
						select s).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				case "usertype_desc":
				{
					model.Users = (
						from s in users
						orderby s.UserTypeDescription descending
						select s).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				case "usertype":
				{
					model.Users = (
						from s in users
						orderby s.UserTypeDescription
						select s).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				case "city_desc":
				{
					model.Users = (
						from s in users
						orderby s.City descending
						select s).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				case "city":
				{
					model.Users = (
						from w in users
						orderby w.City
						select w).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				case "state_desc":
				{
					model.Users = (
						from s in users
						orderby s.State descending
						select s).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				case "state":
				{
					model.Users = (
						from w in users
						orderby w.State
						select w).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
				default:
				{
					model.Users = (
						from s in users
						orderby s.FirstName
						select s).ToPagedList<UserQuickViewModel>(num1, num);
					break;
				}
			}
			switch (sortOrder)
			{
				case "active_desc":
				{
					model.Investors = (
						from s in list
						orderby s.IsActive descending
						select s).ToPagedList<PrincipalInvestorQuickViewModel>(num1, num);
					break;
				}
				case "active":
				{
					model.Investors = (
						from w in list
						orderby w.IsActive
						select w).ToPagedList<PrincipalInvestorQuickViewModel>(num1, num);
					break;
				}
				case "city_desc":
				{
					model.Investors = (
						from s in list
						orderby s.City descending
						select s).ToPagedList<PrincipalInvestorQuickViewModel>(num1, num);
					break;
				}
				case "city":
				{
					model.Investors = (
						from w in list
						orderby w.City
						select w).ToPagedList<PrincipalInvestorQuickViewModel>(num1, num);
					break;
				}
				case "state_desc":
				{
					model.Investors = (
						from s in list
						orderby s.State descending
						select s).ToPagedList<PrincipalInvestorQuickViewModel>(num1, num);
					break;
				}
				case "state":
				{
					model.Investors = (
						from w in list
						orderby w.State
						select w).ToPagedList<PrincipalInvestorQuickViewModel>(num1, num);
					break;
				}
				case "address_desc":
				{
					model.Investors = (
						from s in list
						orderby s.AddressLine1 descending
						select s).ToPagedList<PrincipalInvestorQuickViewModel>(num1, num);
					break;
				}
				case "address":
				{
					model.Investors = (
						from w in list
						orderby w.AddressLine1
						select w).ToPagedList<PrincipalInvestorQuickViewModel>(num1, num);
					break;
				}
				case "company_desc":
				{
					model.Investors = (
						from s in list
						orderby s.CompanyName descending
						select s).ToPagedList<PrincipalInvestorQuickViewModel>(num1, num);
					break;
				}
				case "company":
				{
					model.Investors = (
						from w in list
						orderby w.CompanyName
						select w).ToPagedList<PrincipalInvestorQuickViewModel>(num1, num);
					break;
				}
				case "email_desc":
				{
					model.Investors = (
						from s in list
						orderby s.Email descending
						select s).ToPagedList<PrincipalInvestorQuickViewModel>(num1, num);
					break;
				}
				case "email":
				{
					model.Investors = (
						from w in list
						orderby w.Email
						select w).ToPagedList<PrincipalInvestorQuickViewModel>(num1, num);
					break;
				}
				case "first_desc":
				{
					model.Investors = (
						from s in list
						orderby s.FirstName descending
						select s).ToPagedList<PrincipalInvestorQuickViewModel>(num1, num);
					break;
				}
				case "first":
				{
					model.Investors = (
						from w in list
						orderby w.FirstName
						select w).ToPagedList<PrincipalInvestorQuickViewModel>(num1, num);
					break;
				}
				case "last_desc":
				{
					model.Investors = (
						from s in list
						orderby s.LastName descending
						select s).ToPagedList<PrincipalInvestorQuickViewModel>(num1, num);
					break;
				}
				case "last":
				{
					model.Investors = (
						from w in list
						orderby w.LastName
						select w).ToPagedList<PrincipalInvestorQuickViewModel>(num1, num);
					break;
				}
				default:
				{
					model.Investors = (
						from s in list
						orderby s.PrincipalInvestorId
						select s).ToPagedList<PrincipalInvestorQuickViewModel>(num1, num);
					break;
				}
			}
			return base.View(model);
		}

		[Authorize]
		[HttpGet]
		public ViewResult UserSearchList(int id)
		{
			return base.View(this._user.GetSearchesForUser(id));
		}

		public ActionResult ValidateManager(string id)
		{
			id = id.Substring(3);
			int num = 0;
			if (!int.TryParse(id, out num) || this._user.GetTitleUserById(num) == null)
			{
				return base.RedirectToAction("Index", "Home");
			}
			((dynamic)base.ViewBag).Email = string.Empty;
			ValidateUserModel validateUserModel = new ValidateUserModel()
			{
				TileCompanyUserId = num,
				DoNotShowLinks = true
			};
			string host = base.Request.Url.Host;
			base.Request.Url.Port.ToString();
			return base.View(validateUserModel);
		}

		[HttpPost]
		public ActionResult ValidateManager(ValidateUserModel model)
		{
			ActionResult action;
			string password = this._user.GetTitleUserById(model.TileCompanyUserId).Password;
			string email = this._user.GetTitleUserById(model.TileCompanyUserId).Email;
			try
			{
				if (!(email.ToLower() == model.Email.ToLower()) || !(password.ToLower() == model.Password.ToLower()))
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "The username or password is incorrect.");
					action = base.View("ValidateManager", model);
				}
				else
				{
					action = base.RedirectToAction("UpdateTitleCompanyUser", "Admin", new { id = model.TileCompanyUserId });
				}
			}
			catch (Exception exception)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "The username or password is incorrect.");
				action = base.View(model);
			}
			return action;
		}

		[HttpPost]
		public JsonResult ValidatePortFolioName(string newPF)
		{
			this._user.GetUserByUsername(base.User.Identity.Name);
			bool flag = false;
			if (!string.IsNullOrEmpty(newPF))
			{
				flag = this._portfolio.PortfolioExist(newPF);
			}
			return new JsonResult()
			{
				Data = new { Status = flag.ToString() }
			};
		}

		public ActionResult ValidateUser(string id)
		{
			id = id.Substring(3);
			ValidateUserModel validateUserModel = new ValidateUserModel()
			{
				TileCompanyUserId = Convert.ToInt32(id)
			};
			string host = base.Request.Url.Host;
			string str = base.Request.Url.Port.ToString();
			if (str != null)
			{
				validateUserModel.ReturnUrl = string.Concat(new string[] { "http://", host, ":", str, "/Admin/UpdateTitleCompanyUser/id=", id.ToString() });
			}
			else
			{
				validateUserModel.ReturnUrl = string.Concat("http://", host, "/Admin/UpdateTitleCompanyUser/id=", id.ToString());
			}
			return base.View(validateUserModel);
		}

		[HttpPost]
		public ActionResult ValidateUser(ValidateUserModel model)
		{
			ActionResult action;
			string password = this._user.GetTitleUserById(model.TileCompanyUserId).Password;
			string email = this._user.GetTitleUserById(model.TileCompanyUserId).Email;
			try
			{
				if (!(email.ToLower() == model.Email.ToLower()) || !(password.ToLower() == model.Password.ToLower()))
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "The username or password is incorrect.");
					action = base.View(model);
				}
				else
				{
					action = base.RedirectToAction("UpdateTitleCompanyUser", "Admin", new { id = model.TileCompanyUserId });
				}
			}
			catch (Exception exception)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "The username or password is incorrect.");
				action = base.View(model);
			}
			return action;
		}

		private bool ValidSearch(AdminAssetSearchResultsModel results)
		{
			if (results.AddressLine1 == null && results.AssetName == null && results.AssetNumber == null && results.City == null && results.State == null && results.ZipCode == null && !results.AccListPrice.HasValue && !results.AccUnits.HasValue)
			{
				return false;
			}
			return true;
		}

		[HttpPost]
		public FileUploadJsonResult VideoDelete(string videoId, Guid assetId)
		{
			FileUploadJsonResult fileUploadJsonResult;
			FileUploadJsonResult fileUploadJsonResult1;
			if (videoId != null)
			{
				try
				{
					if (!this._fileManager.DeleteFile(videoId, assetId, FileType.Video))
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

        [HttpGet]
        [Authorize]
        public ViewResult ViewExtractedImages(string filepath)
        {
            int orderTemp = 0;
            ViewBag.AssetType = populateAssetTypeDDL();
            ViewBag.PortfolioList = this.populatePortfolioListDDL();
            ViewBag.StateList = _user.PopulateStateList();
            var model = new ViewExtractedImagesModel();
            model.GuidId = TempData["PdfId"].ToString();

            var user = _user.GetUserByUsername(User.Identity.Name);
            var assets = _asset.GetManageAssetsQuickList(new ManageAssetsModel() { ControllingUserType = user.UserType, UserId = user.UserId });

            model.Assets = new List<SelectListItem>();
            model.FilePath = filepath;
            var list = new List<ExtractedImageModel>();
            list.AddRange(_pdf.GetBitmapImagesFromPDF(getBytesFromFile(filepath), model.GuidId));
            model.ExtractedImages = list;
            foreach (var asset in assets.OrderBy(x => x.AssetNumber))
            {
                model.Assets.Add(new SelectListItem() { Text = string.Format("ID# {0} --- {1},{2} {3}", asset.AssetNumber, asset.AddressLine1, asset.City, asset.State), Value = asset.AssetId.ToString() });
            }
            return View(model);
        }

        [HttpPost]
		public ViewResult ViewExtractedImages(ViewExtractedImagesModel model)
		{
			int num = 0;
			((dynamic)base.ViewBag).AssetType = this.populateAssetTypeDDL();
			((dynamic)base.ViewBag).PortfolioList = this.populatePortfolioListDDL();
            ((dynamic)base.ViewBag).StateList = _user.PopulateStateList();
            int count = this._asset.GetAsset(Guid.Parse(model.AssetId), false).Images.Count;
			for (int i = 0; i < model.ExtractedImages.Count; i++)
			{
				if (model.ExtractedImages[i].IsSelected)
				{
					this._asset.AddImageToAsset(model.ExtractedImages[i].Image, model.ExtractedImages[i].IsFlyerImage, model.ExtractedImages[i].IsMainImage, new Guid(model.AssetId), count + num);
					num++;
				}
			}
			try
			{
                System.IO.File.Delete(model.FilePath);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				AdminController.WriteLog(this.ExceptionDetails(exception));
				this.Email_With_CCandBCC(this.ExceptionDetails(exception));
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Images successfully extracted and added to asset.");
			return base.View("ExtractImagesFromBrochure");
		}

		[Authorize]
		public ActionResult ViewOrderHistory(OrderSearchResultsModel model)
		{
			int value;
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			((dynamic)base.ViewBag).Email = userByUsername.Username;
			TitleCompanyUserModel titleUserByEmail = this._user.GetTitleUserByEmail(base.User.Identity.Name.ToLower());
			List<OrderModel> orderModels = new List<OrderModel>();
			if (titleUserByEmail == null || titleUserByEmail.TitleCompanyId <= 0)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "The title company does not exist for this user.");
				return base.RedirectToAction("MyUscPage", "Home");
			}
			OrderSearchModel orderSearchModel = new OrderSearchModel()
			{
				AssetNumber = model.AssetNumber,
				AddressLine1 = model.AddressLine1,
				City = model.City,
				State = model.State,
				AssetName = model.AssetName,
				AssetCounty = model.AssetCounty,
				APN = model.APN,
				TitleCompanyId = titleUserByEmail.TitleCompanyId,
				DateOrderedEndDate = model.DateOrderedEndDate,
				DatePaidEndDate = model.DatePaidEndDate,
				DateSubmittedEndDate = model.DateSubmittedEndDate,
				DateOrderedStartDate = model.DateOrderedStartDate,
				DatePaidStartDate = model.DatePaidStartDate,
				DateSubmittedStartDate = model.DateSubmittedStartDate,
				SelectedAssetType = model.SelectedAssetType
			};
			orderModels = this._asset.GetTitleCompanyOrders(orderSearchModel);
			((dynamic)base.ViewBag).CurrentSort = model.SortOrder;
			base.ViewBag.AssetIdSortParm = (model.SortOrder == "assetId" ? "assetId_desc" : "assetId");
			base.ViewBag.TypeSortParm = (model.SortOrder == "type" ? "type_desc" : "type");
			base.ViewBag.NameSortParm = (model.SortOrder == "name" ? "name_desc" : "name");
			base.ViewBag.CountySortParm = (model.SortOrder == "county" ? "county_desc" : "county");
			base.ViewBag.CitySortParm = (model.SortOrder == "city" ? "city_desc" : "city");
			base.ViewBag.StateSortParm = (model.SortOrder == "state" ? "state_desc" : "state");
			base.ViewBag.ApnSortParm = (model.SortOrder == "apn" ? "apn_desc" : "apn");
			base.ViewBag.DateOrderSortParm = (model.SortOrder == "dateOrder" ? "dateOrder_desc" : "dateOrder");
			base.ViewBag.StatusSortParm = (model.SortOrder == "status" ? "status_desc" : "status");
			base.ViewBag.BySortParam = (model.SortOrder == "by" ? "by_desc" : "by");
			base.ViewBag.DateSubmitSortParm = (model.SortOrder == "dateSubmit" ? "dateSubmit_desc" : "dateSubmit");
			base.ViewBag.DueSortParm = (model.SortOrder == "due" ? "due_desc" : "due");
			base.ViewBag.DatePaidSortParm = (model.SortOrder == "datePaid" ? "datePaid_desc" : "datePaid");
			string sortOrder = model.SortOrder;
			switch (sortOrder)
			{
				case "assetId_desc":
				{
					orderModels = (
						from s in orderModels
						orderby s.AssetNumber descending
						select s).ToList<OrderModel>();
					break;
				}
				case "assetId":
				{
					orderModels = (
						from w in orderModels
						orderby w.AssetNumber
						select w).ToList<OrderModel>();
					break;
				}
				case "type_desc":
				{
					orderModels = (
						from s in orderModels
						orderby s.Type descending
						select s).ToList<OrderModel>();
					break;
				}
				case "type":
				{
					orderModels = (
						from s in orderModels
						orderby s.Type
						select s).ToList<OrderModel>();
					break;
				}
				case "name_desc":
				{
					orderModels = (
						from s in orderModels
						orderby s.AssetName descending
						select s).ToList<OrderModel>();
					break;
				}
				case "name":
				{
					orderModels = (
						from w in orderModels
						orderby w.AssetName
						select w).ToList<OrderModel>();
					break;
				}
				case "county_desc":
				{
					orderModels = (
						from s in orderModels
						orderby s.County descending
						select s).ToList<OrderModel>();
					break;
				}
				case "county":
				{
					orderModels = (
						from w in orderModels
						orderby w.County
						select w).ToList<OrderModel>();
					break;
				}
				case "city_desc":
				{
					orderModels = (
						from s in orderModels
						orderby s.City descending
						select s).ToList<OrderModel>();
					break;
				}
				case "city":
				{
					orderModels = (
						from w in orderModels
						orderby w.City
						select w).ToList<OrderModel>();
					break;
				}
				case "state_desc":
				{
					orderModels = (
						from s in orderModels
						orderby s.State descending
						select s).ToList<OrderModel>();
					break;
				}
				case "state":
				{
					orderModels = (
						from w in orderModels
						orderby w.State
						select w).ToList<OrderModel>();
					break;
				}
				case "apn_desc":
				{
					orderModels = (
						from s in orderModels
						orderby s.APN descending
						select s).ToList<OrderModel>();
					break;
				}
				case "apn":
				{
					orderModels = (
						from w in orderModels
						orderby w.APN
						select w).ToList<OrderModel>();
					break;
				}
				case "dateOrder_desc":
				{
					orderModels = (
						from s in orderModels
						orderby s.DateOfOrder descending
						select s).ToList<OrderModel>();
					break;
				}
				case "dateOrder":
				{
					orderModels = (
						from s in orderModels
						orderby s.DateOfOrder
						select s).ToList<OrderModel>();
					break;
				}
				case "status_desc":
				{
					orderModels = (
						from s in orderModels
						orderby s.OrderStatus
						select s).ToList<OrderModel>();
					break;
				}
				case "status":
				{
					orderModels = (
						from s in orderModels
						orderby s.OrderStatus descending
						select s).ToList<OrderModel>();
					break;
				}
				case "by_desc":
				{
					orderModels = (
						from x in orderModels
						orderby x.FirstName descending, x.LastName descending
						select x).ToList<OrderModel>();
					break;
				}
				case "by":
				{
					orderModels = (
						from x in orderModels
						orderby x.FirstName, x.LastName
						select x).ToList<OrderModel>();
					break;
				}
				case "dateSubmit_desc":
				{
					orderModels = (
						from s in orderModels
						orderby s.DateOfSubmit descending
						select s).ToList<OrderModel>();
					break;
				}
				case "dateSubmit":
				{
					orderModels = (
						from w in orderModels
						orderby w.DateOfSubmit
						select w).ToList<OrderModel>();
					break;
				}
				case "due_desc":
				{
					orderModels = (
						from s in orderModels
						orderby s.Due descending
						select s).ToList<OrderModel>();
					break;
				}
				case "due":
				{
					orderModels = (
						from w in orderModels
						orderby w.Due
						select w).ToList<OrderModel>();
					break;
				}
				case "datePaid_desc":
				{
					orderModels = (
						from s in orderModels
						orderby s.DatePaid descending
						select s).ToList<OrderModel>();
					break;
				}
				default:
				{
					orderModels = (sortOrder == "datePaid" ? (
						from w in orderModels
						orderby w.DatePaid
						select w).ToList<OrderModel>() : (
						from x in orderModels
						orderby x.OrderStatus, x.DateOfOrder descending
						select x).ToList<OrderModel>());
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
			model.Orders = orderModels.ToPagedList<OrderModel>((rowCount.HasValue ? rowCount.GetValueOrDefault() : 1), num);
			return base.View(model);
		}
        
        public string ValidateAssetDocuments(AssetViewModel model)
        {
            string invalid = string.Empty;
            if (model.Documents != null && model.HasEnvironmentalIssues != null && model.HasEnvironmentalIssues == true)
                if (model.Documents.Where(x=>x.Title.Contains("Environmental")).Count() < 1)
                    invalid += "Environmental Report" + " ";
            if (model.IsPaper)
            {
                if (model.Documents != null && model.HasCopyOfAppraisal != null && model.HasCopyOfAppraisal == "Yes")
                    if (model.Documents.Where(x => x.Title.Contains("Original Appraisal")).Count() < 1)
                        invalid += " Original Appraisal" + " ";
            }
            return invalid;
        }

        

        public static void WriteLog(string strLog)
		{
			FileStream fileStream = null;
			DirectoryInfo directoryInfo = null;
            string str = ConfigurationManager.AppSettings["ErrorLog"];  
			FileInfo fileInfo = new FileInfo(str);
			directoryInfo = new DirectoryInfo(fileInfo.DirectoryName);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			fileStream = (fileInfo.Exists ? new FileStream(str, FileMode.Append) : fileInfo.Create());
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.WriteLine(strLog);
			streamWriter.Close();
		}

		private delegate string ProcessTask(string id);

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [Authorize]
        [HttpGet]
        public FileResult DownloadAssetList()
        {
            if (AuthenticatedUser.UserType == UserType.CorpAdmin || AuthenticatedUser.UserType == UserType.CorpAdmin2)
            {
                return this.File(_asset.GetAssetSpreadsheet(), "application/octet-stream", string.Format("USC-Master db Inv SS of Uploaded CRE Assets {0}.xlsx", DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss")));
            }
            return null;
        }

        private Tuple<bool, bool> SetAssetOwnership(AssetViewModel model)
        {
            bool anErrorOccurred = false;
            bool ownerChanged = false;


            var canSaveOperatingCompany = true;
            // Dont even bother validating anything if data isnt available
            if (string.IsNullOrEmpty(model.OwnerOperatingCompany) &&
                string.IsNullOrEmpty(model.OwnerOperatingCompanyFirst) &&
                string.IsNullOrEmpty(model.OwnerOperatingCompanyLast) &&
                string.IsNullOrEmpty(model.OwnerOperatingCompanyEmail) &&
                string.IsNullOrEmpty(model.OwnerOperatingCompanyAddressLine1) &&
                string.IsNullOrEmpty(model.OwnerOperatingCompanyAddressLine2) &&
                string.IsNullOrEmpty(model.OwnerOperatingCompanyCity) &&
                string.IsNullOrEmpty(model.OwnerOperatingCompanyState) &&
                string.IsNullOrEmpty(model.OwnerOperatingCompanyZip) &&
                string.IsNullOrEmpty(model.OwnerOperatingCompanyCountry) &&
                string.IsNullOrEmpty(model.OwnerOperatingCompanyCellPhone) &&
                string.IsNullOrEmpty(model.OwnerOperatingCompanyFax) &&
                string.IsNullOrEmpty(model.OwnerOperatingCompanyWorkPhone)) canSaveOperatingCompany = false;

            if (model.ChangeOwnerOperatingCompany)
            {
                if (model.IsOwnerOperatingCompanyDataNotAvailable)
                {
                    model.OwnerOperatingCompanyId = Guid.Empty;
                    canSaveOperatingCompany = false;
                }
                if (canSaveOperatingCompany)
                {
                    // Operating Company Validation
                    if (model.NotOnOperatingList)
                    {
                        // We expect at least the new company name field to be populated. If they failed to populate new company name field
                        // but populated others, display an error. Same logic applies to holding
                        if (string.IsNullOrEmpty(model.OwnerOperatingCompany) &&
                        (!string.IsNullOrEmpty(model.OwnerOperatingCompanyFirst) ||
                        !string.IsNullOrEmpty(model.OwnerOperatingCompanyLast) ||
                        !string.IsNullOrEmpty(model.OwnerOperatingCompanyEmail) ||
                        !string.IsNullOrEmpty(model.OwnerOperatingCompanyAddressLine1) ||
                        !string.IsNullOrEmpty(model.OwnerOperatingCompanyAddressLine2) ||
                        !string.IsNullOrEmpty(model.OwnerOperatingCompanyCity) ||
                        !string.IsNullOrEmpty(model.OwnerOperatingCompanyState) ||
                        !string.IsNullOrEmpty(model.OwnerOperatingCompanyZip) ||
                        !string.IsNullOrEmpty(model.OwnerOperatingCompanyCountry) ||
                        !string.IsNullOrEmpty(model.OwnerOperatingCompanyCellPhone) ||
                        !string.IsNullOrEmpty(model.OwnerOperatingCompanyFax) ||
                        !string.IsNullOrEmpty(model.OwnerOperatingCompanyWorkPhone)))
                        {
                            ModelState.AddModelError($"OwnerOperatingCompany", "Contract Owner Operating Company Name is Required");
                            anErrorOccurred = true;
                            canSaveOperatingCompany = false;
                        }
                    }
                    else
                    {
                        // this covers OwnerOperatingCompanyFromPI too
                        // old logic
                        //if (!model.OwnerOperatingCompanyId.HasValue) { ModelState.AddModelError($"OwnerOperatingCompany", "Please select an operating company"); anErrorOccurred = true; }
                        //else { if (string.IsNullOrEmpty(model.OwnerOperatingCompany)) { ModelState.AddModelError($"OwnerOperatingCompany", "Contract Owner Operating Company Name is Required"); anErrorOccurred = true; } }

                        if (//string.IsNullOrEmpty(model.OwnerOperatingCompanyNew) &&
                            string.IsNullOrEmpty(model.OwnerOperatingCompany) &&
                            (!string.IsNullOrEmpty(model.OwnerOperatingCompanyFirst) ||
                            !string.IsNullOrEmpty(model.OwnerOperatingCompanyLast) ||
                            //!string.IsNullOrEmpty(model.OwnerOperatingCompanyNewFirst) ||
                            //!string.IsNullOrEmpty(model.OwnerOperatingCompanyNewLast) ||
                            !string.IsNullOrEmpty(model.OwnerOperatingCompanyEmail) ||
                            !string.IsNullOrEmpty(model.OwnerOperatingCompanyAddressLine1) ||
                            !string.IsNullOrEmpty(model.OwnerOperatingCompanyAddressLine2) ||
                            !string.IsNullOrEmpty(model.OwnerOperatingCompanyCity) ||
                            !string.IsNullOrEmpty(model.OwnerOperatingCompanyState) ||
                            !string.IsNullOrEmpty(model.OwnerOperatingCompanyZip) ||
                            !string.IsNullOrEmpty(model.OwnerOperatingCompanyCountry) ||
                            !string.IsNullOrEmpty(model.OwnerOperatingCompanyCellPhone) ||
                            !string.IsNullOrEmpty(model.OwnerOperatingCompanyFax) ||
                            !string.IsNullOrEmpty(model.OwnerOperatingCompanyWorkPhone)))
                        {
                            ModelState.AddModelError($"OwnerOperatingCompany", "Contract Owner Operating Company Name is Required");
                            anErrorOccurred = true;
                            canSaveOperatingCompany = false;
                        }
                    }
                }
                if (canSaveOperatingCompany)
                {
                    model.OwnerOperatingCompanyId = null;
                    if (model.OwnerOperatingCompanyFromPI)
                    {
                        model.NotOnOperatingList = false; // force this to false just in case the view state desynced
                                                          // Only verify that the email and company name are populated for now
                                                          // The only difference between OwnerOperatingCompanyFromPI path and NotinOwnerCompanyList path is the use of the NEW company name field
                        Guid ocId = Guid.NewGuid();
                        _user.CreateOperatingCompany(new OperatingCompanyViewModel
                        {
                            OperatingCompanyId = ocId,
                            CompanyName = model.OwnerOperatingCompany,
                            FirstName = model.OwnerOperatingCompanyFirst,
                            LastName = model.OwnerOperatingCompanyLast,
                            Email = model.OwnerOperatingCompanyEmail,
                            AddressLine1 = model.OwnerOperatingCompanyAddressLine1,
                            AddressLine2 = model.OwnerOperatingCompanyAddressLine2,
                            City = model.OwnerOperatingCompanyCity,
                            State = model.OwnerOperatingCompanyState,
                            Zip = model.OwnerOperatingCompanyZip,
                            Country = model.OwnerOperatingCompanyCountry,
                            CellNumber = model.OwnerOperatingCompanyCellPhone,
                            FaxNumber = model.OwnerOperatingCompanyFax,
                            WorkNumber = model.OwnerOperatingCompanyWorkPhone,
                            IsActive = true,
                        });
                        model.OwnerOperatingCompanyId = ocId;
                        ownerChanged = true;
                    }
                    else if (model.NotOnOperatingList)
                    {
                        Guid ocId = Guid.NewGuid();
                        _user.CreateOperatingCompany(new OperatingCompanyViewModel
                        {
                            OperatingCompanyId = ocId,
                            CompanyName = model.OwnerOperatingCompany,
                            FirstName = model.OwnerOperatingCompanyFirst,
                            LastName = model.OwnerOperatingCompanyLast,
                            Email = model.OwnerOperatingCompanyEmail,
                            AddressLine1 = model.OwnerOperatingCompanyAddressLine1,
                            AddressLine2 = model.OwnerOperatingCompanyAddressLine2,
                            City = model.OwnerOperatingCompanyCity,
                            State = model.OwnerOperatingCompanyState,
                            Zip = model.OwnerOperatingCompanyZip,
                            Country = model.OwnerOperatingCompanyCountry,
                            CellNumber = model.OwnerOperatingCompanyCellPhone,
                            FaxNumber = model.OwnerOperatingCompanyFax,
                            WorkNumber = model.OwnerOperatingCompanyWorkPhone,
                            IsActive = true,
                        });
                        model.OwnerOperatingCompanyId = ocId;
                        ownerChanged = true;
                    }
                    else
                    {
                        if (model.OwnerOperatingCompanyNewId.HasValue)
                        {
                            _user.UpdateOperatingCompany(new OperatingCompanyViewModel
                            {
                                OperatingCompanyId = model.OwnerOperatingCompanyNewId.Value,
                                CompanyName = model.OwnerOperatingCompany,
                                FirstName = model.OwnerOperatingCompanyFirst,
                                LastName = model.OwnerOperatingCompanyLast,
                                Email = model.OwnerOperatingCompanyEmail,
                                AddressLine1 = model.OwnerOperatingCompanyAddressLine1,
                                AddressLine2 = model.OwnerOperatingCompanyAddressLine2,
                                City = model.OwnerOperatingCompanyCity,
                                State = model.OwnerOperatingCompanyState,
                                Zip = model.OwnerOperatingCompanyZip,
                                Country = model.OwnerOperatingCompanyCountry,
                                CellNumber = model.OwnerOperatingCompanyCellPhone,
                                FaxNumber = model.OwnerOperatingCompanyFax,
                                WorkNumber = model.OwnerOperatingCompanyWorkPhone,
                                IsActive = model.OwnerOperatingCompanyIsActive,
                            });
                            model.OwnerOperatingCompanyId = model.OwnerOperatingCompanyNewId.Value;
                            ownerChanged = true;
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(model.OwnerOperatingCompanyUpdateId))
                {
                    if (string.IsNullOrEmpty(model.OwnerOperatingCompanyUpdate))
                    {
                        ModelState.AddModelError($"OwnerOperatingCompanyUpdate", "Contract Owner Operating Company Name is Required");
                        anErrorOccurred = true;
                    }
                    if (!anErrorOccurred)
                    {
                        _user.UpdateOperatingCompany(new OperatingCompanyViewModel
                        {
                            OperatingCompanyId = Guid.Parse(model.OwnerOperatingCompanyUpdateId),
                            CompanyName = model.OwnerOperatingCompanyUpdate,
                            FirstName = model.OwnerOperatingCompanyUpdateFirst,
                            LastName = model.OwnerOperatingCompanyUpdateLast,
                            Email = model.OwnerOperatingCompanyUpdateEmail,
                            AddressLine1 = model.OwnerOperatingCompanyUpdateAddressLine1,
                            AddressLine2 = model.OwnerOperatingCompanyUpdateAddressLine2,
                            City = model.OwnerOperatingCompanyUpdateCity,
                            State = model.OwnerOperatingCompanyUpdateState,
                            Zip = model.OwnerOperatingCompanyUpdateZip,
                            Country = model.OwnerOperatingCompanyUpdateCountry,
                            CellNumber = model.OwnerOperatingCompanyUpdateCellPhone,
                            FaxNumber = model.OwnerOperatingCompanyUpdateFax,
                            WorkNumber = model.OwnerOperatingCompanyUpdateWorkPhone,
                            IsActive = model.OwnerOperatingCompanyUpdateIsActive,
                        });
                    }
                }
            }


            var canSaveHoldingCompany = true;
            // if no fields are populated, ignore
            if (string.IsNullOrEmpty(model.OwnerHoldingCompany) &&
                string.IsNullOrEmpty(model.OwnerHoldingCompanyFirst) &&
                string.IsNullOrEmpty(model.OwnerHoldingCompanyLast) &&
                string.IsNullOrEmpty(model.OwnerHoldingCompanyEmail) &&
                string.IsNullOrEmpty(model.OwnerHoldingCompanyAddressLine1) &&
                string.IsNullOrEmpty(model.OwnerHoldingCompanyAddressLine2) &&
                string.IsNullOrEmpty(model.OwnerHoldingCompanyCity) &&
                string.IsNullOrEmpty(model.OwnerHoldingCompanyState) &&
                string.IsNullOrEmpty(model.OwnerHoldingCompanyZip) &&
                string.IsNullOrEmpty(model.OwnerHoldingCompanyCountry) &&
                string.IsNullOrEmpty(model.OwnerHoldingCompanyCellPhone) &&
                string.IsNullOrEmpty(model.OwnerHoldingCompanyFax) &&
                string.IsNullOrEmpty(model.OwnerHoldingCompanyWorkPhone)) canSaveHoldingCompany = false;

            if (model.ChangeOwnerHoldingCompany)
            {
                // set the IDs to an empty guid if data isnt available. lets hope this doesnt blow anything up
                if (model.IsOwnerHoldingCompanyDataNotAvailable)
                {
                    model.OwnerHoldingCompanyId = Guid.Empty;
                    canSaveHoldingCompany = false;
                }
                
                if (canSaveHoldingCompany)
                {
                    // Holding Company Validation
                    if (model.NotOnHoldingList)
                    {
                        if (string.IsNullOrEmpty(model.OwnerHoldingCompany) &&
                        (!string.IsNullOrEmpty(model.OwnerHoldingCompanyFirst) ||
                        !string.IsNullOrEmpty(model.OwnerHoldingCompanyLast) ||
                        !string.IsNullOrEmpty(model.OwnerHoldingCompanyEmail) ||
                        !string.IsNullOrEmpty(model.OwnerHoldingCompanyAddressLine1) ||
                        !string.IsNullOrEmpty(model.OwnerHoldingCompanyAddressLine2) ||
                        !string.IsNullOrEmpty(model.OwnerHoldingCompanyCity) ||
                        !string.IsNullOrEmpty(model.OwnerHoldingCompanyState) ||
                        !string.IsNullOrEmpty(model.OwnerHoldingCompanyZip) ||
                        !string.IsNullOrEmpty(model.OwnerHoldingCompanyCountry) ||
                        !string.IsNullOrEmpty(model.OwnerHoldingCompanyCellPhone) ||
                        !string.IsNullOrEmpty(model.OwnerHoldingCompanyFax) ||
                        !string.IsNullOrEmpty(model.OwnerHoldingCompanyWorkPhone)))
                        {
                            ModelState.AddModelError($"OwnerHoldingCompany", "Contract Owner Holding Company Name is Required");
                            anErrorOccurred = true;
                            canSaveHoldingCompany = false;
                        }
                    }
                    else
                    {
                        if (//string.IsNullOrEmpty(model.OwnerHoldingCompanyNew) &&
                            string.IsNullOrEmpty(model.OwnerHoldingCompany) &&
                            (!string.IsNullOrEmpty(model.OwnerHoldingCompanyFirst) ||
                            !string.IsNullOrEmpty(model.OwnerHoldingCompanyLast) ||
                            //!string.IsNullOrEmpty(model.OwnerHoldingCompanyNewFirst) ||
                            //!string.IsNullOrEmpty(model.OwnerHoldingCompanyNewLast) ||
                            !string.IsNullOrEmpty(model.OwnerHoldingCompanyEmail) ||
                            !string.IsNullOrEmpty(model.OwnerHoldingCompanyAddressLine1) ||
                            !string.IsNullOrEmpty(model.OwnerHoldingCompanyAddressLine2) ||
                            !string.IsNullOrEmpty(model.OwnerHoldingCompanyCity) ||
                            !string.IsNullOrEmpty(model.OwnerHoldingCompanyState) ||
                            !string.IsNullOrEmpty(model.OwnerHoldingCompanyZip) ||
                            !string.IsNullOrEmpty(model.OwnerHoldingCompanyCountry) ||
                            !string.IsNullOrEmpty(model.OwnerHoldingCompanyCellPhone) ||
                            !string.IsNullOrEmpty(model.OwnerHoldingCompanyFax) ||
                            !string.IsNullOrEmpty(model.OwnerHoldingCompanyWorkPhone)))
                        {
                            ModelState.AddModelError($"OwnerHoldingCompany", "Contract Owner Holding Company Name is Required");
                            anErrorOccurred = true;
                            canSaveHoldingCompany = false;
                        }
                    }
                }
                // process operating company first since the holding references the operating company's id still
                if (canSaveHoldingCompany)
                {
                    if (model.NotOnHoldingList)
                    {
                        Guid holdingCompanyId = Guid.NewGuid();
                        var hcvm = new HoldingCompanyViewModel
                        {
                            HoldingCompanyId = holdingCompanyId,
                            OperatingCompanyId = model.OwnerOperatingCompanyId.GetValueOrDefault(Guid.Empty),
                            CompanyName = model.OwnerHoldingCompany,
                            FirstName = model.OwnerHoldingCompanyFirst,
                            LastName = model.OwnerHoldingCompanyLast,
                            Email = model.OwnerHoldingCompanyEmail,
                            AddressLine1 = model.OwnerHoldingCompanyAddressLine1,
                            AddressLine2 = model.OwnerHoldingCompanyAddressLine2,
                            City = model.OwnerHoldingCompanyCity,
                            State = model.OwnerHoldingCompanyState,
                            Zip = model.OwnerHoldingCompanyZip,
                            Country = model.OwnerHoldingCompanyCountry,
                            CellNumber = model.OwnerHoldingCompanyCellPhone,
                            FaxNumber = model.OwnerHoldingCompanyFax,
                            WorkNumber = model.OwnerHoldingCompanyWorkPhone,
                            IsActive = true
                        };
                        _user.CreateHoldingCompany(hcvm);
                        model.OwnerHoldingCompanyId = holdingCompanyId;
                    }
                    else
                    {
                        if (model.OwnerHoldingCompanyNewId.HasValue)
                        {
                            var hcvm = new HoldingCompanyViewModel
                            {
                                HoldingCompanyId = model.OwnerHoldingCompanyNewId.Value,
                                OperatingCompanyId = model.OwnerOperatingCompanyId.GetValueOrDefault(Guid.Empty),
                                CompanyName = model.OwnerHoldingCompany,
                                FirstName = model.OwnerHoldingCompanyFirst,
                                LastName = model.OwnerHoldingCompanyLast,
                                Email = model.OwnerHoldingCompanyEmail,
                                AddressLine1 = model.OwnerHoldingCompanyAddressLine1,
                                AddressLine2 = model.OwnerHoldingCompanyAddressLine2,
                                City = model.OwnerHoldingCompanyCity,
                                State = model.OwnerHoldingCompanyState,
                                Zip = model.OwnerHoldingCompanyZip,
                                Country = model.OwnerHoldingCompanyCountry,
                                CellNumber = model.OwnerHoldingCompanyCellPhone,
                                FaxNumber = model.OwnerHoldingCompanyFax,
                                WorkNumber = model.OwnerHoldingCompanyWorkPhone,
                                IsActive = model.OwnerHoldingCompanyIsActive
                            };
                            _user.UpdateHoldingCompany(hcvm);
                            model.OwnerHoldingCompanyId = model.OwnerHoldingCompanyNewId.Value;
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(model.OwnerHoldingCompanyUpdateId))
                {
                    if (string.IsNullOrEmpty(model.OwnerHoldingCompanyUpdate))
                    {
                        ModelState.AddModelError($"OwnerHoldingCompanyUpdate", "Contract Owner Holding Company Name is Required");
                        anErrorOccurred = true;
                    }
                    if (!anErrorOccurred)
                    {
                        _user.UpdateHoldingCompany(new HoldingCompanyViewModel
                        {
                            HoldingCompanyId = Guid.Parse(model.OwnerHoldingCompanyUpdateId),
                            OperatingCompanyId = model.OwnerOperatingCompanyId.GetValueOrDefault(Guid.Empty),
                            CompanyName = model.OwnerHoldingCompanyUpdate,
                            FirstName = model.OwnerHoldingCompanyUpdateFirst,
                            LastName = model.OwnerHoldingCompanyUpdateLast,
                            Email = model.OwnerHoldingCompanyUpdateEmail,
                            AddressLine1 = model.OwnerHoldingCompanyUpdateAddressLine1,
                            AddressLine2 = model.OwnerHoldingCompanyUpdateAddressLine2,
                            City = model.OwnerHoldingCompanyUpdateCity,
                            State = model.OwnerHoldingCompanyUpdateState,
                            Zip = model.OwnerHoldingCompanyUpdateZip,
                            Country = model.OwnerHoldingCompanyUpdateCountry,
                            CellNumber = model.OwnerHoldingCompanyUpdateCellPhone,
                            FaxNumber = model.OwnerHoldingCompanyUpdateFax,
                            WorkNumber = model.OwnerHoldingCompanyUpdateWorkPhone,
                            IsActive = model.OwnerHoldingCompanyUpdateIsActive,
                        });
                    }
                }
            }

            return new Tuple<bool, bool>(anErrorOccurred, ownerChanged);
        }

        private void CreateOwnershipRecordIfApplicable(AssetViewModel model, bool operatingCompanyChanged) {
            // if asset is successfully updated and operating company changed, create an ownership change record
            if (operatingCompanyChanged && model.OwnerHoldingCompanyId.HasValue && model.OwnerOperatingCompanyId.HasValue) {
                // clear comments if a user is changing ownership
                //model.GeneralComments = string.Empty;

                // TODO: Talk to liz about when to create an ownership change
                // do we require holding, operating or both in order to create an ownership change?
                var aocModel = new AssetOwnershipChangeViewModel {
                    AssetId = model.AssetId,
                    AssetOwnershipChangeId = Guid.NewGuid(),
                    OwnerHoldingCompanyId = model.OwnerHoldingCompanyId.Value,
                    AcquisitionDate = model.ActualClosingDate,
                    ClosingPrice = model.SalesPrice.GetValueOrDefault(0),
                    SellerTerms = model.SellerTerms,
                    ProformaCapRate = model.CashInvestmentApy
                };
                if (model.ChangeOwnerHoldingCompany && !string.IsNullOrEmpty(model.OwnerHoldingCompanyUpdateId))
                    aocModel.PreviousOwnerHoldingCompanyId = Guid.Parse(model.OwnerHoldingCompanyUpdateId);
                _asset.CreateAssetOwnershipChange(aocModel);
            }
        }

		
		public ActionResult GetRelatedPortfolio(string assetId)
		{
			var portfolioList = this._asset.GetAssetRealetdPortfolioList(assetId);
			return this.PartialView("_AssetPortfolioListView", portfolioList);
		}

		public ActionResult GetAssetsbyPortfolioId(string portfolioId)
        {
			var assetsList = this._asset.GetAssetByPortfolioId(portfolioId);
			return this.PartialView("_AssetViewPortfolio", assetsList);
		}
				



		//HC & OC Logic

		
		
		[HttpPost]
		public JsonResult GetApplicableOwnerCompanyOptions(string name, string type)
		{
			JsonResult jsonResult;
			try
			{
				// user input property 'name' is trimmed at this point(we dont trim enough string fields in this project)
				if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(type))
				{
					return new JsonResult() { Data = new { Status = "true", Options = _user.SearchCompanies(name, type) } };
				}
				jsonResult = new JsonResult() { Data = new { Status = "false" } };
			}
			catch
			{
				jsonResult = new JsonResult() { Data = new { Status = "false" } };
			}
			return jsonResult;
		}

		

		[HttpPost]
		public ActionResult UpdateCreateHCnAssetHCOwnerships(HCCAssetHCOwnerships model)
		{
			JsonResult jsonResult;

			try
			{
				if (model.HoldingCompany.HoldingCompanyId != null && model.HoldingCompany.HoldingCompanyId != Guid.Empty)
				{					
					var result = this._user.UpdateHoldingCompany(model.HoldingCompany);
				}
				else
                {
					model.HoldingCompany.HoldingCompanyId = Guid.NewGuid();
					model.HoldingCompany.IsActive = true;
					var result = this._user.CreateHoldingCompany(model.HoldingCompany);
				}

				//map model
				AssetHCOwnership assetHC = new AssetHCOwnership();

				assetHC.AssetHCOwnershipId = model.AssetHCOwnershipId ?? 0;
				assetHC.AssetId = model.AssetId;
				assetHC.HoldingCompanyId = model.HoldingCompany.HoldingCompanyId;
				assetHC.CreateDate = DateTime.Now;				
				assetHC.Terms = model.Terms;
				assetHC.ActualClosingDate = model.ActualClosingDate;
				assetHC.SalesPrice = model.SalesPrice;
				assetHC.SalesPriceNotProvided = model.SalesPriceNotProvided;
				assetHC.CalculatedPPU = model.CalculatedPPU;
				assetHC.CalculatedPPSqFt = model.CalculatedPPSqFt;
				assetHC.CashInvestmentApy = model.CashInvestmentApy;
				assetHC.TermsOther = model.TermsOther;
				assetHC.CapRate = model.CapRate;

				this._asset.SaveUpdateAssetHC(assetHC);

				jsonResult = new JsonResult() { Data = new { Status = "true" } };
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, exception.Message);
				jsonResult = new JsonResult() { Data = new { Status = "false" } };
			}

			return jsonResult;
		}

		public ActionResult GetAssetHCByAssetId(Guid assetId)
		{
			var result = this._asset.GetAssetHCByAssetId(assetId);
			return Json(new { result }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetOwnerHCByAssetHCOwnershipId(int assetHCOwnershipId)
		{
			var result = this._asset.GetAssetHCByAssetHCOwnershipId(assetHCOwnershipId);
			return Json(new { result }, JsonRequestBehavior.AllowGet);			
		}


		public ActionResult UpdateCreateOC(HCCAssetOC model)
		{
			//only update OC table data
			//AssetOCId is not null
			//OperatingCompanyId is Not null

			//insert both the table AssetOC and OC
			//AssetOCId is null
			//OperatingCompanyId is null

			//insert AssetOC and update OC
			//AssetOCId is null
			//OperatingCompanyId is Not null

			JsonResult jsonResult;

			try
			{
				if (model.OperatingCompanyId != null && model.OperatingCompanyId != Guid.Empty)
				{
					OperatingCompanyViewModel OCViewModel = new OperatingCompanyViewModel();
					OCViewModel.OperatingCompanyId = model.OperatingCompanyId ?? Guid.NewGuid();
					OCViewModel.CompanyName = model.CompanyName;
					OCViewModel.FirstName = model.FirstName;
					OCViewModel.LastName = model.LastName;
					OCViewModel.Email = model.Email;
					OCViewModel.AddressLine1 = model.AddressLine1;
					OCViewModel.AddressLine2 = model.AddressLine2;
					OCViewModel.City = model.City;
					OCViewModel.State = model.State;
					OCViewModel.Zip = model.Zip;
					OCViewModel.Country = model.Country;
					OCViewModel.WorkNumber = model.WorkNumber;
					OCViewModel.CellNumber = model.CellNumber;
					OCViewModel.FaxNumber = model.FaxNumber;

					OCViewModel.LinkedIn = model.LinkedIn;
					OCViewModel.Facebook = model.Facebook;
					OCViewModel.Instagram = model.Instagram;
					OCViewModel.Twitter = model.Twitter;

					OCViewModel.IsActive = true;

					this._user.UpdateOperatingCompany(OCViewModel);
				}
				else
				{
					model.OperatingCompanyId = Guid.NewGuid();
					model.IsActive = true;

					OperatingCompanyViewModel OCViewModel = new OperatingCompanyViewModel();
					OCViewModel.OperatingCompanyId = model.OperatingCompanyId ?? Guid.NewGuid();
					OCViewModel.CompanyName = model.CompanyName;
					OCViewModel.FirstName = model.FirstName;
					OCViewModel.LastName = model.LastName;
					OCViewModel.Email = model.Email;
					OCViewModel.AddressLine1 = model.AddressLine1;
					OCViewModel.AddressLine2 = model.AddressLine2;
					OCViewModel.City = model.City;
					OCViewModel.State = model.State;
					OCViewModel.Zip = model.Zip;
					OCViewModel.Country = model.Country;
					OCViewModel.WorkNumber = model.WorkNumber;
					OCViewModel.CellNumber = model.CellNumber;
					OCViewModel.FaxNumber = model.FaxNumber;

					OCViewModel.LinkedIn = model.LinkedIn;
					OCViewModel.Facebook = model.Facebook;
					OCViewModel.Instagram = model.Instagram;
					OCViewModel.Twitter = model.Twitter;

					OCViewModel.IsActive = model.IsActive;

					var result = this._user.CreateOperatingCompany(OCViewModel);
				}

				//map model
				this._asset.SaveUpdateAssetOC(model);

				jsonResult = new JsonResult() { Data = new { Status = "true" } };
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, exception.Message);
				jsonResult = new JsonResult() { Data = new { Status = "false" } };
			}

			return jsonResult;
		}

		public ActionResult GetAssetOCByAssetId(Guid assetId)
		{
			var result = this._asset.GetAssetOCByAssetId(assetId);
			return Json(new { result }, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetOCByAssetOCId(int AssetOCId)
		{
			var result = this._asset.GetAssetOCByAssetOCId(AssetOCId);
			return Json(new { result }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetHCAddressByAssetId(Guid assetId)
		{
			var result = this._asset.GetHCAddressByAssetId(assetId);
			return Json(new { result }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetChainOfTitle(string assetId)
		{
			var assetsList = this._asset.GetChainOfTitleByAssetId(assetId);
			return this.PartialView("_AssetViewChainOfTitle", assetsList);
		}

        #region ManageAsset		      

        [Authorize]
		public ActionResult ManageAssets(AdminAssetSearchResultsModel model, string sortOrder, int? page, string assetNumber = null)
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
				ApnNumber = model.ApnNumber,
				County = model.County,
				ListAgentCompanyName = model.ListAgentCompanyName,
				ListAgentName = model.ListAgentName
			};

            if (model.AssmFin == AssmFin.Yes)
            {
				manageAssetsModel.HasPositionMortgage = PositionMortgageType.Yes;
            }
			else if (model.AssmFin == AssmFin.No)
			{
				manageAssetsModel.HasPositionMortgage = PositionMortgageType.No;
			}

			adminAssetQuickListModels = this._asset.GetManageAssetsQuickList(manageAssetsModel);
			
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
			return base.View(model);
		}

		public ActionResult ActivateAsset(string id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			this._asset.ActivateAsset(id);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Asset successfully Activated.");
			if (userByUsername.UserType != UserType.CREBroker && userByUsername.UserType != UserType.CRELender && userByUsername.UserType != UserType.Investor)
			{
				return base.RedirectToAction("ManageAssets", "Admin");
			}
			return base.RedirectToAction("SellerManageAssets", "Investors");
		}

		public ActionResult DeActivateAsset(string id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			this._asset.DeActivateAsset(id);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Asset successfully DeActivated.");
			if (userByUsername.UserType != UserType.CREBroker && userByUsername.UserType != UserType.CRELender && userByUsername.UserType != UserType.Investor)
			{
				return base.RedirectToAction("ManageAssets", "Admin");
			}
			return base.RedirectToAction("SellerManageAssets", "Investors");
		}

		public ActionResult PublishAsset(Guid id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			this._asset.PublishAsset(id);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Asset successfully published.");
			if (userByUsername.UserType != UserType.CREBroker && userByUsername.UserType != UserType.CRELender && userByUsername.UserType != UserType.Investor)
			{
				//return base.RedirectToAction("ManageAssets", "Admin");
				return base.RedirectToAction("ICACache", "ICA");
			}
			return base.RedirectToAction("SellerManageAssets", "Investors");
		}

		[Authorize]
		public ActionResult UnPublishAsset(Guid id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			this._asset.UnPublishAsset(id);
			this._asset.UnlockAsset(id);
			if (userByUsername.UserType == UserType.CorpAdmin || userByUsername.UserType == UserType.ICAdmin)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Asset successfully withdrawn.");
			}
			else
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Asset successfully deactivated.");
			}
			if (userByUsername.UserType != UserType.CREBroker && userByUsername.UserType != UserType.CRELender && userByUsername.UserType != UserType.Investor)
			{
				return base.RedirectToAction("ManageAssets", "Admin");
			}
			return base.RedirectToAction("SellerManageAssets", "Investors");
		}

		public ActionResult CheckHCDate(DateTime date, Guid assetId)
		{
			JsonResult jsonResult;
			try
			{
				var result = this._asset.CheckHCDate(date, assetId);
				jsonResult = new JsonResult() { Data = new { Status = result } };
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, exception.Message);
				jsonResult = new JsonResult() { Data = new { Status = "false" } };
			}


			return jsonResult;

		}

		#endregion

		#region ManageHC

		[Authorize]
		public ActionResult ManageHoldingCompanies(AdminHoldingCompanySearchResultsModel model, int? page)
		{
			int value;
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			if (!base.ValidateAdminUser(userByUsername))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			base.ViewBag.UserType = userByUsername.UserType;

			this._user.RemoveUserAssetLocks(base.User.Identity.Name);

			

			List<HoldingCompanyList> HCQuickListModels = new List<HoldingCompanyList>();

			ManageHoldingCompanyModel manageHCModel = new ManageHoldingCompanyModel()
			{
				HCName = model.HCName,
				ISRA = model.ISRA,
				HCEmail = model.HCEmail,
				HCFirstName = model.HCFirstName,
				HCLastName = model.HCFirstName,
				LinkedInurl = model.LinkedInurl,
				Facebookurl = model.Facebookurl,
				Instagramurl = model.Instagramurl,
				Twitterurl = model.Twitterurl,

				AssetNumber = model.AssetNumber,
				AssetName = model.AssetName,
				AddressLine1 = model.AddressLine1,
				City = model.City,
				State = model.State,
				ZipCode = model.ZipCode,
				ApnNumber = model.ApnNumber,
				IsPaper = model.IsPaper,
				County = model.County,
				ListAgentName = model.ListAgentName
			};

			//HCQuickListModels = this._user.GetHoldingCompanies(manageHCModel);
			HCQuickListModels = this._user.GetHoldingCompany(manageHCModel);

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
			model.HcList = HCQuickListModels.ToPagedList<HoldingCompanyList>(num1, num);			
			
			return base.View(model);

		}

		[Authorize]
		[HttpGet]
		public ViewResult UpdateHoldingCompany(string id)
		{
			var model = _user.GetHoldingCompany(Guid.Parse(id));
			model.Countries = GetAllCountries();
			model.OpertingCompanyList = _user.GetOperatingCompanies(new OperatingCompanySearchModel()).OrderBy(x => x.CompanyName)
				.Select(a => new SelectListItem
			{
				Text = a.CompanyName,
				Value = Convert.ToString(a.OperatingCompanyId)
			}).ToList();
			return base.View(model);
		}

		[HttpPost]
		public ActionResult UpdateHoldingCompany(HoldingCompanyViewModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var result = this._user.UpdateHoldingCompany(model);
					// Allowing a success to be sent even if !result.Success due to the holding company successfully updating(most likely)
					if (!result.Success) base.TempData["message"] = new MessageViewModel(MessageTypes.Success, result.Message);
					else base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Holding company successfully updated.");
				}
				else
				{
					model.Countries = GetAllCountries();
					model.Countries = GetAllCountries();
					return View(model);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, exception.Message);
			}
			return base.RedirectToAction("ManageHoldingCompanies");
		}


		public ActionResult GetHoldingCompanybyId(Guid id)
		{
			var result = this._user.GetHoldingCompanybyId(id);
			return this.PartialView("_HoldingComapnyDeatails", result);
			
		}
		public ActionResult GetOpertingCompanybyId(Guid id)
		{
			var result = this._user.GetOpertingCompanybyId(id);
			return this.PartialView("_OpertingComapnyDeatails", result);
		}

		public ActionResult GetAssetsbyHCId(string HcId)
		{
			var assetsList = this._asset.GetAssetsbyHCId(HcId);
			return this.PartialView("_AssetViewPortfolio", assetsList);
		}

		public ActionResult GetAssetsbyOCId(string OcId)
		{
			var assetsList = this._asset.GetAssetsbyOCId(OcId);
			return this.PartialView("_AssetViewPortfolio", assetsList);
		}

		#endregion

		#region ManageOC

		//[Authorize]
		//public ActionResult ManageOperatingCompanies(OperatingCompanySearchResultsModel model)
		//{
		//	int value;
		//	if (!base.ValidateAdminUser(this._user.GetUserByUsername(base.User.Identity.Name)))
		//	{
		//		base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
		//		return base.RedirectToAction("Index", "Home");
		//	}
		//	OperatingCompanySearchModel searchModel = new OperatingCompanySearchModel()
		//	{
		//		AddressLine1 = model.AddressLine1,
		//		City = model.City,
		//		Email = model.Email,
		//		FirstName = model.FirstName,
		//		LastName = model.LastName,
		//		State = model.State,
		//		Zip = model.Zip,
		//		ShowActiveOnly = model.ShowActiveOnly,
		//		CompanyName = model.CompanyName
		//	};
		//	((dynamic)base.ViewBag).CurrentSort = model.SortOrder;
		//	base.ViewBag.CitySortParm = (model.SortOrder == "city" ? "city_desc" : "city");
		//	base.ViewBag.StateSortParm = (model.SortOrder == "state" ? "state_desc" : "state");
		//	base.ViewBag.CountrySortParm = (model.SortOrder == "country" ? "country_desc" : "country");
		//	base.ViewBag.CompanyNameSortParm = (model.SortOrder == "company" ? "company_desc" : "company");
		//	base.ViewBag.AddressSortParm = (model.SortOrder == "address" ? "address_desc" : "address");
		//	base.ViewBag.EmailSortParm = (model.SortOrder == "email" ? "email_desc" : "email");
		//	base.ViewBag.FirstNameSortParm = (model.SortOrder == "first" ? "first_desc" : "first");
		//	base.ViewBag.LastNameSortParm = (model.SortOrder == "last" ? "last_desc" : "last");
		//	base.ViewBag.IsActiveParm = (model.SortOrder == "active" ? "active_desc" : "active");
		//	List<OperatingCompanyViewModel> list = this._user.GetOperatingCompanies(searchModel).ToList<OperatingCompanyViewModel>();
		//	string sortOrder = model.SortOrder;
		//	switch (sortOrder)
		//	{
		//		case "active_desc":
		//			{
		//				list = (
		//					from s in list
		//					orderby s.IsActive descending
		//					select s).ToList<OperatingCompanyViewModel>();
		//				break;
		//			}
		//		case "active":
		//			{
		//				list = (
		//					from w in list
		//					orderby w.IsActive
		//					select w).ToList<OperatingCompanyViewModel>();
		//				break;
		//			}
		//		case "city_desc":
		//			{
		//				list = (
		//					from s in list
		//					orderby s.City descending
		//					select s).ToList<OperatingCompanyViewModel>();
		//				break;
		//			}
		//		case "city":
		//			{
		//				list = (
		//					from w in list
		//					orderby w.City
		//					select w).ToList<OperatingCompanyViewModel>();
		//				break;
		//			}
		//		case "country":
		//			{
		//				list = (
		//					from w in list
		//					orderby w.Country
		//					select w).ToList<OperatingCompanyViewModel>();
		//				break;
		//			}
		//		case "country_desc":
		//			{
		//				list = (
		//					from w in list
		//					orderby w.Country descending
		//					select w).ToList<OperatingCompanyViewModel>();
		//				break;
		//			}
		//		case "state_desc":
		//			{
		//				list = (
		//					from s in list
		//					orderby s.State descending
		//					select s).ToList<OperatingCompanyViewModel>();
		//				break;
		//			}
		//		case "state":
		//			{
		//				list = (
		//					from w in list
		//					orderby w.State
		//					select w).ToList<OperatingCompanyViewModel>();
		//				break;
		//			}
		//		case "address_desc":
		//			{
		//				list = (
		//					from s in list
		//					orderby s.AddressLine1 descending
		//					select s).ToList<OperatingCompanyViewModel>();
		//				break;
		//			}
		//		case "address":
		//			{
		//				list = (
		//					from w in list
		//					orderby w.AddressLine1
		//					select w).ToList<OperatingCompanyViewModel>();
		//				break;
		//			}
		//		case "company_desc":
		//			{
		//				list = (
		//					from s in list
		//					orderby s.CompanyName descending
		//					select s).ToList<OperatingCompanyViewModel>();
		//				break;
		//			}
		//		case "company":
		//			{
		//				list = (
		//					from w in list
		//					orderby w.CompanyName
		//					select w).ToList<OperatingCompanyViewModel>();
		//				break;
		//			}
		//		case "email_desc":
		//			{
		//				list = (
		//					from s in list
		//					orderby s.Email descending
		//					select s).ToList<OperatingCompanyViewModel>();
		//				break;
		//			}
		//		case "email":
		//			{
		//				list = (
		//					from w in list
		//					orderby w.Email
		//					select w).ToList<OperatingCompanyViewModel>();
		//				break;
		//			}
		//		case "first_desc":
		//			{
		//				list = (
		//					from s in list
		//					orderby s.FirstName descending
		//					select s).ToList<OperatingCompanyViewModel>();
		//				break;
		//			}
		//		case "first":
		//			{
		//				list = (
		//					from w in list
		//					orderby w.FirstName
		//					select w).ToList<OperatingCompanyViewModel>();
		//				break;
		//			}
		//		case "last_desc":
		//			{
		//				list = (
		//					from s in list
		//					orderby s.LastName descending
		//					select s).ToList<OperatingCompanyViewModel>();
		//				break;
		//			}
		//		default:
		//			{
		//				list = (sortOrder == "last" ? (
		//					from w in list
		//					orderby w.LastName
		//					select w).ToList<OperatingCompanyViewModel>() : (
		//					from x in list
		//					orderby x.FirstName
		//					select x).ToList<OperatingCompanyViewModel>());
		//				break;
		//			}
		//	}
		//	int num = 0;
		//	TempDataDictionary tempData = base.TempData;
		//	int? rowCount = model.RowCount;
		//	if (rowCount.HasValue)
		//	{
		//		rowCount = model.RowCount;
		//		value = rowCount.Value;
		//	}
		//	else
		//	{
		//		value = 50;
		//	}
		//	num = value;
		//	tempData["RowCount"] = value;
		//	base.TempData.Keep("RowCount");
		//	rowCount = model.Page;
		//	model.OperatingCompanies = list.ToPagedList<OperatingCompanyViewModel>((rowCount.HasValue ? rowCount.GetValueOrDefault() : 1), num);
		//	return base.View(model);
		//}


		[Authorize]
		public ActionResult ManageOperatingCompanies(AdminOperatingCompanySearchResultsModel model, int? page)
		{
			int value;
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			if (!base.ValidateAdminUser(userByUsername))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			base.ViewBag.UserType = userByUsername.UserType;

			this._user.RemoveUserAssetLocks(base.User.Identity.Name);



			List<OperatingCompanyList> OCQuickListModels = new List<OperatingCompanyList>();

			ManageOperatingCompanyModel manageOCModel = new ManageOperatingCompanyModel()
			{
				OCName = model.OCName,
				OCEmail = model.OCEmail,
				OCFirstName = model.OCFirstName,
				OCLastName = model.OCFirstName,
				LinkedInurl = model.LinkedInurl,
				Facebookurl = model.Facebookurl,
				Instagramurl = model.Instagramurl,
				Twitterurl = model.Twitterurl,

				AssetNumber = model.AssetNumber,
				AssetName = model.AssetName,
				AddressLine1 = model.AddressLine1,
				City = model.City,
				State = model.State,
				ZipCode = model.ZipCode,
				ApnNumber = model.ApnNumber,
				IsPaper = model.IsPaper,
				County = model.County,
				ListAgentName = model.ListAgentName
			};

			//HCQuickListModels = this._user.GetHoldingCompanies(manageHCModel);
			OCQuickListModels = this._user.GetOperataingCompany(manageOCModel);

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
			model.OcList = OCQuickListModels.ToPagedList<OperatingCompanyList>(num1, num);

			return base.View(model);

		}

		[Authorize]
		[HttpGet]
		public ViewResult UpdateOperatingCompany(string id)
		{
			var model = _user.GetOPeratingCompany(Guid.Parse(id));
			model.Countries = GetAllCountries();
			return base.View(model);
		}

		[HttpPost]
		public ActionResult UpdateOperatingCompany(OperatingCompanyViewModel model)
		{
			try
			{				
				if (ModelState.IsValid)
				{
					this._user.UpdateOperatingCompany(model);
					base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Operating Company successfully updated.");
				}
				else
				{
					return View(model);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, exception.Message);
			}
			return base.View(model);
		}

		#endregion

	}
	public class JqueryDatatableParam
	{
		public string sEcho { get; set; }
		public string sSearch { get; set; }
		public int iDisplayLength { get; set; }
		public int iDisplayStart { get; set; }
		public int iColumns { get; set; }
		public int iSortCol_0 { get; set; }
		public string sSortDir_0 { get; set; }
		public int iSortingCols { get; set; }
		public string sColumns { get; set; }
	}
}