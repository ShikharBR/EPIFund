using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.ViewModel;
using Inview.Epi.EpiFund.Web.ActionFilters;
using Inview.Epi.EpiFund.Web.Models;
using Inview.Epi.EpiFund.Web.Models.Emails;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Web.Controllers
{
	[LayoutActionFilter]
	public class InsuranceController : BaseController
	{
		private IAssetManager _asset;

		private IUserManager _user;

		private ISecurityManager _securityManager;

		private IEPIFundEmailService _email;

		private IInsuranceManager _insurance;

		private IAuthProvider _auth;

		public InsuranceController(IAssetManager asset, IUserManager user, ISecurityManager securityManager, IEPIFundEmailService email, IInsuranceManager insurance, IAuthProvider auth) : base(securityManager, email, user)
		{
			this._asset = asset;
			this._user = user;
			this._securityManager = securityManager;
			this._email = email;
			this._insurance = insurance;
			this._auth = auth;
		}

		[Authorize]
		[HttpGet]
		public ActionResult ActivateInsuranceCompany(int? id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			base.ValidateAdmin(userByUsername);
			if (id.GetValueOrDefault(0) > 0)
			{
				this._insurance.ActivateInsuranceCompany(id.Value);
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid ID.");
			return base.RedirectToAction("ManageInsuranceCompanies");
		}

		[Authorize]
		[HttpGet]
		public ActionResult ActivateInsuranceUser(int? id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			base.ValidateAdmin(userByUsername);
			if (id.GetValueOrDefault(0) <= 0)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid ID.");
				return base.RedirectToAction("ManageInsuranceCompanies");
			}
			this._insurance.ActivateInsuranceCompanyUser(id.Value);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Insurance Company User successfully activated.");
			return base.RedirectToAction("ManageInsuranceCompanies");
		}

		[Authorize]
		[HttpGet]
		public ActionResult CreateInsuranceCompany()
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			base.ValidateAdmin(userByUsername);
			return base.View(new InsuranceCompanyViewModel());
		}

		[Authorize]
		[HttpPost]
		public ActionResult CreateInsuranceCompany(InsuranceCompanyViewModel model)
		{
			try
			{
				if (base.ModelState.IsValid)
				{
					if (this._insurance.CompanyExist(model.CompanyName))
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "An insurance Company with this name already exists in the system.");
					}
					else
					{
						this._insurance.CreateInsuranceCompany(model);
						base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Insurance Company successfully created.");
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, string.Concat("An issue occured during saving. error: ", exception.Message));
			}
			return base.RedirectToAction("ManageInsuranceCompanies");
		}

		[Authorize]
		[HttpGet]
		public ActionResult CreateInsuranceCompanyUser(int? id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			base.ValidateAdmin(userByUsername);
			if (id.GetValueOrDefault(0) > 0)
			{
				InsuranceCompanyUserViewModel insuranceCompanyUserViewModel = new InsuranceCompanyUserViewModel()
				{
					InsuranceCompanyId = id.Value
				};
				return base.View(insuranceCompanyUserViewModel);
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid ID.");
			return base.RedirectToAction("ManageInsuranceCompanies");
		}

		[Authorize]
		[HttpPost]
		public ActionResult CreateInsuranceCompanyUser(InsuranceCompanyUserViewModel model)
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
				if (!base.isEmail(model.Email))
				{
					base.ModelState.AddModelError("Email", "Valid email is Required");
				}
				if (base.ModelState.IsValid)
				{
					if (this._user.UserExists(model.Email))
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "A User with this email already exists in the system.");
					}
					else
					{
						if (string.IsNullOrEmpty(model.Password))
						{
							model.Password = this._securityManager.GenerateCode();
						}
						int num = this._insurance.CreateInsuranceCompanyUser(model);
						if (base.CanSendAutoEmail(BaseController.AutoEmailType.Insurance))
						{
							string host = base.Request.Url.Host;
							string str = base.Request.Url.Port.ToString();
							string empty = string.Empty;
							empty = (str != null ? string.Concat(new string[] { "http://", host, ":", str, "/Insurance/ValidateUser/", num.ToString() }) : string.Concat("http://", host, "/Insurance/ValidateUser/", num.ToString()));
							try
							{
								if (!model.IsActive)
								{
									this._email.Send(new CompanyUserWelcomeEmail()
									{
										Email = model.Email,
										TempPassword = model.Password,
										To = model.FullName,
										RegistrationType = "Insurance Company Manager",
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
										RegistrationType = "Insurance Company Manager",
										URL = empty
									});
								}
							}
							catch
							{
							}
						}
						base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Insurance Company User successfully created. An invite has been sent.");
						ActionResult action = base.RedirectToAction("ManageInsuranceCompanyUsers", new { id = model.InsuranceCompanyId });
						return action;
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, string.Concat("User Creation Error: ", exception.Message));
			}
			return base.View(model);
		}

		[Authorize]
		[HttpGet]
		public ActionResult DeactivateInsuranceCompany(int? id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			base.ValidateAdmin(userByUsername);
			if (id.GetValueOrDefault(0) > 0)
			{
				this._insurance.DeactivateInsuranceCompany(id.Value);
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid ID.");
			return base.RedirectToAction("ManageInsuranceCompanies");
		}

		[Authorize]
		[HttpGet]
		public ActionResult DeactivateInsuranceUser(int? id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			base.ValidateAdmin(userByUsername);
			if (id.GetValueOrDefault(0) > 0)
			{
				this._insurance.DeactivateInsuranceCompanyUser(id.Value);
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Insurance Company User successfully deactivated.");
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid ID.");
			return base.RedirectToAction("ManageInsuranceCompanies");
		}

		[Authorize]
		[HttpGet]
		public ActionResult EditInsuranceCompany(int? id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			base.ValidateAdmin(userByUsername);
			if (id.GetValueOrDefault(0) > 0)
			{
				InsuranceCompanyViewModel insuranceCompany = this._insurance.GetInsuranceCompany(id.Value);
				return base.View(insuranceCompany);
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid ID.");
			return base.View();
		}

		[Authorize]
		[HttpPost]
		public ActionResult EditInsuranceCompany(InsuranceCompanyViewModel model)
		{
			this._user.GetUserByUsername(base.User.Identity.Name);
			if (!base.ModelState.IsValid)
			{
				return base.View(model);
			}
			this._insurance.UpdateInsuranceCompany(model);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Insurance company successfully updated.");
			return base.RedirectToAction("ManageInsuranceCompanies");
		}

		[Authorize]
		[HttpGet]
		public ActionResult EditInsuranceCompanyUser(int? id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			base.ValidateAdmin(userByUsername);
			if (id.GetValueOrDefault(0) > 0)
			{
				InsuranceCompanyUserViewModel insuranceCompanyUser = this._insurance.GetInsuranceCompanyUser(id.Value);
				return base.View(insuranceCompanyUser);
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid ID.");
			return base.RedirectToAction("ManageInsuranceCompanies");
		}

		[Authorize]
		[HttpPost]
		public ActionResult EditInsuranceCompanyUser(InsuranceCompanyUserViewModel model)
		{
			if (string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.ConfirmPassword))
			{
				base.ModelState.AddModelError("Password", "Password is Required");
			}
			if (string.IsNullOrEmpty(model.ConfirmPassword) && !string.IsNullOrEmpty(model.Password))
			{
				base.ModelState.AddModelError("ConfirmPassword", "Confirm Password is Required");
			}
			if (!base.isEmail(model.Email))
			{
				base.ModelState.AddModelError("Email", "Valid email is Required");
			}
			if (!base.ModelState.IsValid)
			{
				return base.View(model);
			}
			this._insurance.UpdateInsuranceCompanyUser(model);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Insurance Company User successfully editted.");
			return base.RedirectToAction("ManageInsuranceCompanyUsers", new { id = model.InsuranceCompanyId });
		}

		[HttpGet]
		public ActionResult Error()
		{
			return base.View();
		}

		[Authorize]
		public ActionResult InsuranceCompanyPage(int? id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			base.ValidateAdmin(userByUsername);
			InsuranceCompanySearchResultsModel insuranceCompanySearchResultsModel = new InsuranceCompanySearchResultsModel();
			if (id.GetValueOrDefault(0) <= 0)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid ID.");
			}
			return base.View(insuranceCompanySearchResultsModel);
		}

		[ChildActionOnly]
		public ActionResult InsuranceUserSideNavigationMenu()
		{
			bool flag = false;
			if (base.User.Identity.IsAuthenticated)
			{
				flag = this._insurance.VerifyUserType(base.User.Identity.Name);
			}
			return this.PartialView("../Shared/_InsuranceUserNavigationMenu", flag);
		}

		[Authorize]
		[HttpGet]
		public ActionResult InviteInsuranceCompanyUser(int? id)
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			base.ValidateAdmin(userByUsername);
			if (id.GetValueOrDefault(0) <= 0)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid ID.");
				return base.RedirectToAction("ManageInsuranceCompanies");
			}
			if (base.CanSendAutoEmail(BaseController.AutoEmailType.Insurance))
			{
				InsuranceCompanyUserViewModel insuranceCompanyUser = this._insurance.GetInsuranceCompanyUser(id.Value);
				if (insuranceCompanyUser != null)
				{
					string str = string.Join("", (
						from b in (IEnumerable<byte>)insuranceCompanyUser.PasswordBytes
						select string.Format("{0:X2}", b)).ToArray<string>());
					string host = base.Request.Url.Host;
					int port = base.Request.Url.Port;
					string str1 = port.ToString();
					string empty = string.Empty;
					if (str1 != null)
					{
						string[] strArrays = new string[] { "http://", host, ":", str1, "/Insurance/ValidateUser/", null };
						port = insuranceCompanyUser.InsuranceCompanyUserId;
						strArrays[5] = port.ToString();
						empty = string.Concat(strArrays);
					}
					else
					{
						port = insuranceCompanyUser.InsuranceCompanyUserId;
						empty = string.Concat("http://", host, "/Insurance/ValidateUser/", port.ToString());
					}
					if (!insuranceCompanyUser.IsActive)
					{
						this._email.Send(new CompanyUserWelcomeEmail()
						{
							Email = insuranceCompanyUser.Email,
							TempPassword = str,
							To = insuranceCompanyUser.Email,
							RegistrationType = "Insurance Company Manager",
							URL = empty
						});
					}
					else
					{
						this._email.Send(new CompanyUserWelcomeEmailAutoActivatedEmail()
						{
							ActivatedByCorpAdmin = userByUsername.Username,
							Email = insuranceCompanyUser.Email,
							TempPassword = str,
							To = insuranceCompanyUser.Email,
							RegistrationType = "Insurance Company Manager",
							URL = empty
						});
					}
				}
			}
			return base.RedirectToAction("ManageInsuranceCompanies");
		}

		[Authorize]
		[HttpGet]
		public ActionResult ManageAssets()
		{
			return base.View(new LoginModel());
		}

		[Authorize]
		[HttpPost]
		public ActionResult ManageAssets(LoginModel model)
		{
			ActionResult action;
			InsuranceCompanyUserViewModel insuranceUserByEmail = this._user.GetInsuranceUserByEmail(base.User.Identity.Name);
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
					assetNumber = asset.PCInsuranceCompanyId;
					int insuranceCompanyId = insuranceUserByEmail.InsuranceCompanyId;
					if ((assetNumber.GetValueOrDefault() == insuranceCompanyId ? !assetNumber.HasValue : true))
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "This asset is inaccessible to your insurance company");
						action = base.View(model);
					}
					else
					{
						OrderStatus orderStatus = asset.OrderStatus;
						if (asset.OrderStatus == OrderStatus.Completed)
						{
							base.TempData["message"] = new MessageViewModel(MessageTypes.Info, "This asset has been closed.");
							action = base.View(model);
						}
						else
						{
							action = base.RedirectToAction("UpdateAsset", "Insurance", new { id = assetByAssetNumber.AssetId, fromManageAssets = false });
						}
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
		public ActionResult ManageInsuranceCompanies(InsuranceCompanySearchResultsModel model)
		{
			int value;
			if (!base.ValidateAdminUser(this._user.GetUserByUsername(base.User.Identity.Name)))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				return base.RedirectToAction("Index", "Home");
			}
			CompanySearchModel companySearchModel = new CompanySearchModel()
			{
				CompanyName = model.InsuranceCompanyName,
				CompanyURL = model.InsuranceCompanyURL
			};
			List<InsuranceCompanyViewModel> insuranceCompanies = this._insurance.GetInsuranceCompanies(companySearchModel);
			base.ViewBag.InsuranceCompanyName = (model.SortOrder == "InsuranceCompanyName" ? "InsuranceCompanyName_desc" : "InsuranceCompanyName");
			base.ViewBag.Website = (model.SortOrder == "Website" ? "Website_desc" : "Website");
			base.ViewBag.CreateDate = (model.SortOrder == "CreateDate" ? "CreateDate_desc" : "CreateDate");
			string sortOrder = model.SortOrder;
			if (sortOrder == "InsuranceCompanyName_desc")
			{
				insuranceCompanies = (
					from x in insuranceCompanies
					orderby x.CompanyName descending
					select x).ToList<InsuranceCompanyViewModel>();
			}
			else if (sortOrder == "InsuranceCompanyName")
			{
				insuranceCompanies = (
					from x in insuranceCompanies
					orderby x.CompanyName
					select x).ToList<InsuranceCompanyViewModel>();
			}
			else if (sortOrder == "Website_desc")
			{
				insuranceCompanies = (
					from x in insuranceCompanies
					orderby x.CompanyURL descending
					select x).ToList<InsuranceCompanyViewModel>();
			}
			else if (sortOrder == "Website")
			{
				insuranceCompanies = (
					from x in insuranceCompanies
					orderby x.CompanyURL
					select x).ToList<InsuranceCompanyViewModel>();
			}
			else if (sortOrder == "CreateDate_desc")
			{
				insuranceCompanies = (
					from x in insuranceCompanies
					orderby x.CreateDate descending
					select x).ToList<InsuranceCompanyViewModel>();
			}
			else if (sortOrder == "CreateDate")
			{
				insuranceCompanies = (
					from x in insuranceCompanies
					orderby x.CreateDate
					select x).ToList<InsuranceCompanyViewModel>();
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
			model.Companies = insuranceCompanies.ToPagedList<InsuranceCompanyViewModel>((rowCount.HasValue ? rowCount.GetValueOrDefault() : 1), num);
			return base.View(model);
		}

		[Authorize]
		public ActionResult ManageInsuranceCompanyUsers(int? id, InsuranceCompanyUserSearchResultsModel model)
		{
			int value;
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			base.ValidateAdmin(userByUsername);
			if (id.GetValueOrDefault(0) <= 0)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid ID.");
				return base.RedirectToAction("ManageInsuranceCompanies");
			}
			model.InsuranceCompanyId = id.Value;
			CompanyUserSearchModel companyUserSearchModel = new CompanyUserSearchModel()
			{
				FirstName = model.FirstName,
				LastName = model.LastName,
				ShowActiveOnly = model.IsActive,
				InsuranceCompanyId = id
			};
			List<InsuranceCompanyUserViewModel> insuranceCompanyUsers = this._insurance.GetInsuranceCompanyUsers(companyUserSearchModel);
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
			model.Users = insuranceCompanyUsers.ToPagedList<InsuranceCompanyUserViewModel>((rowCount.HasValue ? rowCount.GetValueOrDefault() : 1), num);
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

		[Authorize]
		[HttpGet]
		public ActionResult UpdateAsset(Guid id)
		{
			this._user.GetUserByUsername(base.User.Identity.Name);
			AssetViewModel asset = this._asset.GetAsset(id, false);
			asset.AssetDocumentTypes = new List<SelectListItem>()
			{
				new SelectListItem()
				{
					Text = "P&C Insurance Coverage Quote",
					Value = "Insurance"
				},
				new SelectListItem()
				{
					Text = "Other Insurance Coverage Related",
					Value = "InsuranceOther"
				}
			};
			asset.TaxParcelNumber = string.Join(",", (
				from x in asset.AssetTaxParcelNumbers
				select x.TaxParcelNumber).ToArray<string>());
			return base.View(asset);
		}

		[Authorize]
		[HttpPost]
		public ActionResult UpdateAsset(AssetViewModel model, string SubmitAsset)
		{
			InsuranceCompanyUserViewModel insuranceUserByEmail = this._user.GetInsuranceUserByEmail(base.User.Identity.Name);
			if (model.Documents == null)
			{
				model.Documents = new List<AssetDocument>();
			}
			int num = 0;
			for (int i = 0; i < model.Documents.Count; i++)
			{
				bool flag = false;
				do
				{
					if (i >= model.Documents.Count)
					{
						break;
					}
					if (model.Documents[i].FileName != null)
					{
						model.Documents[i].AssetDocumentId = base.generatedGuidIfNone(model.Documents[i].AssetDocumentId);
						model.Documents[i].Order = num;
						if (SubmitAsset == "Save Asset")
						{
							this._asset.MarkDocumentViewingStatus(model.Documents[i], false);
						}
						else if (SubmitAsset == "Save & Submit to Corp Admin")
						{
							this._asset.MarkDocumentViewingStatus(model.Documents[i], true);
						}
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
			bool flag1 = this._asset.UpdateAssetDocuments(model.Documents, model.AssetId);
			if (SubmitAsset == "Save Asset")
			{
				if (!flag1)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Problem saving. Please contact site admin.");
					base.RedirectToAction("UpdateAsset", "Insurance", new { id = model.AssetId });
				}
				else
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully saved.");
					this._asset.UnlockAsset(model.AssetId);
				}
			}
			else if (SubmitAsset == "Save & Submit to Corp Admin")
			{
				this._insurance.UpdateOrderStatus(model.AssetId, OrderStatus.Completed, insuranceUserByEmail.InsuranceCompanyUserId);
				if (base.CanSendAutoEmail(BaseController.AutoEmailType.Insurance))
				{
					AssetDocumentOrderRequestModel emailingModel = this._insurance.GetEmailingModel(model.AssetId, insuranceUserByEmail.InsuranceCompanyUserId);
					try
					{
						this._email.Send(new NotificationToBuyerInsQuoteDocsUploadCompleteEmail()
						{
							To = emailingModel.BuyerEmail,
							Date = DateTime.Now,
							BuyerName = emailingModel.BuyerName,
							CompanyName = emailingModel.CompanyName,
							AssetId = emailingModel.AssetId,
							AssetAddress1 = emailingModel.Address1,
							AssetAddress2 = emailingModel.Address2,
							AssetCity = emailingModel.City,
							AssetState = emailingModel.State,
							AssetZip = emailingModel.Zip,
							AssetDescription = emailingModel.AssetDescription,
							VestingEntity = emailingModel.VestingEntity,
							APN = emailingModel.APN,
							Subject = "Insurance Documents Completed"
						});
					}
					catch
					{
					}
					try
					{
						this._email.Send(new NotificationToInsCoUploadCompleteEmail()
						{
							To = insuranceUserByEmail.Email,
							Date = DateTime.Now,
							CompanyName = emailingModel.CompanyName,
							AssetId = emailingModel.AssetId,
							AssetAddress1 = emailingModel.Address1,
							AssetAddress2 = emailingModel.Address2,
							AssetCity = emailingModel.City,
							AssetState = emailingModel.State,
							AssetZip = emailingModel.Zip,
							AssetDescription = emailingModel.AssetDescription,
							VestingEntity = emailingModel.VestingEntity,
							APN = emailingModel.APN,
							BuyerName = emailingModel.BuyerName,
							Subject = "Insurance Documents Completed"
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
							this._email.Send(new NotificationToUSCInsQuoteDocsUploadCompleteEmail()
							{
								To = admin.Username,
								Date = DateTime.Now,
								CompanyName = emailingModel.CompanyName,
								AssetId = emailingModel.AssetId,
								AssetAddress1 = emailingModel.Address1,
								AssetAddress2 = emailingModel.Address2,
								AssetCity = emailingModel.City,
								AssetState = emailingModel.State,
								AssetZip = emailingModel.Zip,
								APN = emailingModel.APN,
								BuyerName = emailingModel.BuyerName,
								VestingEntity = emailingModel.VestingEntity,
								Subject = "Insurance Documents Completed",
								AssetDescription = emailingModel.AssetDescription
							});
						}
						catch
						{
						}
					}
				}
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Successfully saved.");
			}
			return base.RedirectToAction("MyUSCPage");
		}

		[Authorize]
		[HttpGet]
		public ActionResult UpdateUser()
		{
			InsuranceCompanyUserViewModel insuranceUserByEmail = this._user.GetInsuranceUserByEmail(base.User.Identity.Name);
			if (insuranceUserByEmail != null)
			{
				InsuranceCompanyUserViewModel insuranceCompanyUser = this._insurance.GetInsuranceCompanyUser(insuranceUserByEmail.InsuranceCompanyUserId);
				return base.View(insuranceCompanyUser);
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "invalid ID.");
			return base.View("../Insurance/Error");
		}

		[HttpPost]
		public ActionResult UpdateUser(InsuranceCompanyUserViewModel model)
		{
			if (string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.ConfirmPassword))
			{
				base.ModelState.AddModelError("Password", "Password is Required");
			}
			if (string.IsNullOrEmpty(model.ConfirmPassword) && !string.IsNullOrEmpty(model.Password))
			{
				base.ModelState.AddModelError("ConfirmPassword", "Confirm Password is Required");
			}
			if (!base.isEmail(model.Email))
			{
				base.ModelState.AddModelError("Email", "Valid email is Required");
			}
			if (!base.ModelState.IsValid)
			{
				if (base.TempData.ContainsKey("PCIPassword"))
				{
					base.TempData.Keep("PCIPassword");
				}
				return base.View(model);
			}
			model.IsActive = true;
			this._insurance.UpdateInsuranceCompanyUser(model);
			if (base.TempData.ContainsKey("PCIPassword"))
			{
				this._auth.Authenticate(model.Email, base.TempData["PCIPassword"].ToString());
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Insurance Company User successfully editted.");
			return base.RedirectToAction("MyUSCPage", "Insurance");
		}

		[HttpGet]
		public ActionResult UpdateUserFirstTime(int? id)
		{
			if (id.GetValueOrDefault(0) <= 0)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "invalid ID.");
				return base.View("../Insurance/Error");
			}
			InsuranceCompanyUserViewModel insuranceCompanyUser = this._insurance.GetInsuranceCompanyUser(id.Value);
			if (base.TempData.ContainsKey("PCIPassword"))
			{
				base.TempData.Keep("PCIPassword");
			}
			return base.View("../Insurance/UpdateUser", insuranceCompanyUser);
		}

		[HttpGet]
		public ActionResult ValidateUser(string id)
		{
			ValidateUserModel validateUserModel = new ValidateUserModel();
			int num = 0;
			if (!int.TryParse(id, out num))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "invalid ID.");
				return base.View("../Insurance/Error");
			}
			InsuranceCompanyUserViewModel insuranceCompanyUser = this._insurance.GetInsuranceCompanyUser(num);
			if (insuranceCompanyUser == null)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "invalid ID.");
				return base.View("../Insurance/Error");
			}
			validateUserModel.InsuranceCompanyUserId = insuranceCompanyUser.InsuranceCompanyUserId;
			string host = base.Request.Url.Host;
			string str = base.Request.Url.Port.ToString();
			if (str != null)
			{
				validateUserModel.ReturnUrl = string.Concat(new string[] { "http://", host, ":", str, "/Insurance/UpdateUser/", num.ToString() });
			}
			else
			{
				validateUserModel.ReturnUrl = string.Concat("http://", host, "/Insurance/UpdateUser/", num.ToString());
			}
			return base.View(validateUserModel);
		}

		[HttpPost]
		public ActionResult ValidateUser(ValidateUserModel model)
		{
			if (base.ModelState.IsValid)
			{
				if (this._insurance.ValidateUser(model))
				{
					base.TempData["PCIPassword"] = model.Password;
					return base.RedirectToAction("UpdateUserFirstTime", "Insurance", new { id = model.InsuranceCompanyUserId });
				}
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "The username or password is incorrect.");
			}
			return base.View(model);
		}

		[Authorize]
		public ActionResult ViewOrderHistory(OrderSearchResultsModel model)
		{
			int value;
			InsuranceCompanyUserViewModel insuranceUserByEmail = this._user.GetInsuranceUserByEmail(base.User.Identity.Name);
			OrderSearchModel orderSearchModel = new OrderSearchModel()
			{
				AssetNumber = model.AssetNumber,
				AddressLine1 = model.AddressLine1,
				City = model.City,
				State = model.State,
				AssetName = model.AssetName,
				AssetCounty = model.AssetCounty,
				APN = model.APN,
				InsuranceCompanyId = insuranceUserByEmail.InsuranceCompanyId,
				DateOrderedEndDate = model.DateOrderedEndDate,
				DatePaidEndDate = model.DatePaidEndDate,
				DateSubmittedEndDate = model.DateSubmittedEndDate,
				DateOrderedStartDate = model.DateOrderedStartDate,
				DatePaidStartDate = model.DatePaidStartDate,
				DateSubmittedStartDate = model.DateSubmittedStartDate,
				SelectedAssetType = model.SelectedAssetType
			};
			List<OrderModel> insuranceCompanyOrders = this._insurance.GetInsuranceCompanyOrders(orderSearchModel);
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
			string sortOrder = model.SortOrder;
			switch (sortOrder)
			{
				case "assetId_desc":
				{
					insuranceCompanyOrders = (
						from s in insuranceCompanyOrders
						orderby s.AssetNumber descending
						select s).ToList<OrderModel>();
					break;
				}
				case "assetId":
				{
					insuranceCompanyOrders = (
						from w in insuranceCompanyOrders
						orderby w.AssetNumber
						select w).ToList<OrderModel>();
					break;
				}
				case "type_desc":
				{
					insuranceCompanyOrders = (
						from s in insuranceCompanyOrders
						orderby s.Type descending
						select s).ToList<OrderModel>();
					break;
				}
				case "type":
				{
					insuranceCompanyOrders = (
						from s in insuranceCompanyOrders
						orderby s.Type
						select s).ToList<OrderModel>();
					break;
				}
				case "name_desc":
				{
					insuranceCompanyOrders = (
						from s in insuranceCompanyOrders
						orderby s.AssetName descending
						select s).ToList<OrderModel>();
					break;
				}
				case "name":
				{
					insuranceCompanyOrders = (
						from w in insuranceCompanyOrders
						orderby w.AssetName
						select w).ToList<OrderModel>();
					break;
				}
				case "county_desc":
				{
					insuranceCompanyOrders = (
						from s in insuranceCompanyOrders
						orderby s.County descending
						select s).ToList<OrderModel>();
					break;
				}
				case "county":
				{
					insuranceCompanyOrders = (
						from w in insuranceCompanyOrders
						orderby w.County
						select w).ToList<OrderModel>();
					break;
				}
				case "city_desc":
				{
					insuranceCompanyOrders = (
						from s in insuranceCompanyOrders
						orderby s.City descending
						select s).ToList<OrderModel>();
					break;
				}
				case "city":
				{
					insuranceCompanyOrders = (
						from w in insuranceCompanyOrders
						orderby w.City
						select w).ToList<OrderModel>();
					break;
				}
				case "state_desc":
				{
					insuranceCompanyOrders = (
						from s in insuranceCompanyOrders
						orderby s.State descending
						select s).ToList<OrderModel>();
					break;
				}
				case "state":
				{
					insuranceCompanyOrders = (
						from w in insuranceCompanyOrders
						orderby w.State
						select w).ToList<OrderModel>();
					break;
				}
				case "apn_desc":
				{
					insuranceCompanyOrders = (
						from s in insuranceCompanyOrders
						orderby s.APN descending
						select s).ToList<OrderModel>();
					break;
				}
				case "apn":
				{
					insuranceCompanyOrders = (
						from w in insuranceCompanyOrders
						orderby w.APN
						select w).ToList<OrderModel>();
					break;
				}
				case "dateOrder_desc":
				{
					insuranceCompanyOrders = (
						from s in insuranceCompanyOrders
						orderby s.DateOfOrder descending
						select s).ToList<OrderModel>();
					break;
				}
				case "dateOrder":
				{
					insuranceCompanyOrders = (
						from s in insuranceCompanyOrders
						orderby s.DateOfOrder
						select s).ToList<OrderModel>();
					break;
				}
				case "status_desc":
				{
					insuranceCompanyOrders = (
						from s in insuranceCompanyOrders
						orderby s.OrderStatus
						select s).ToList<OrderModel>();
					break;
				}
				case "status":
				{
					insuranceCompanyOrders = (
						from s in insuranceCompanyOrders
						orderby s.OrderStatus descending
						select s).ToList<OrderModel>();
					break;
				}
				case "by_desc":
				{
					insuranceCompanyOrders = (
						from x in insuranceCompanyOrders
						orderby x.FirstName descending, x.LastName descending
						select x).ToList<OrderModel>();
					break;
				}
				case "by":
				{
					insuranceCompanyOrders = (
						from x in insuranceCompanyOrders
						orderby x.FirstName, x.LastName
						select x).ToList<OrderModel>();
					break;
				}
				case "dateSubmit_desc":
				{
					insuranceCompanyOrders = (
						from s in insuranceCompanyOrders
						orderby s.DateOfSubmit descending
						select s).ToList<OrderModel>();
					break;
				}
				default:
				{
					insuranceCompanyOrders = (sortOrder == "dateSubmit" ? (
						from w in insuranceCompanyOrders
						orderby w.DateOfSubmit
						select w).ToList<OrderModel>() : (
						from x in insuranceCompanyOrders
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
			model.Orders = insuranceCompanyOrders.ToPagedList<OrderModel>((rowCount.HasValue ? rowCount.GetValueOrDefault() : 1), num);
			return base.View(model);
		}
	}
}