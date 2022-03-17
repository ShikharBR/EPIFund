using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.Helpers;
using Inview.Epi.EpiFund.Domain.ViewModel;
using Inview.Epi.EpiFund.Web.Models;
using Inview.Epi.EpiFund.Web.Models.Emails;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Inview.Epi.EpiFund.Web.Controllers
{
	public class BaseController : AsyncController
	{
		private ISecurityManager _securityManager;

		private IEPIFundEmailService _email;

		public IUserManager _user;

		public const string acceptedImageMimeTypes = "image/jpeg,image/png,image/gif";

		public BaseController(ISecurityManager securityManager, IEPIFundEmailService email, IUserManager user)
		{
			this._securityManager = securityManager;
			this._user = user;
			this._email = email;
		}

		public void AuthenticateMachine(string username)
		{
			UserModel userByUsername = this._user.GetUserByUsername(username);
			if (userByUsername != null)
			{
				this._securityManager.AuthenticateMachine(this.DetermineCompName(), userByUsername);
			}
		}

        public bool IsValidEmail(string email)
        {
            try
            {
                // if the email address survives this, it shouldnt kill the intended emailer
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<AssetType> GetAssetTypes()
        {
            return Enum.GetValues(typeof(AssetType)).Cast<AssetType>();
        }

        public IEnumerable<SelectListItem> GetAssetTypesAsSelectItemList()
        {
            return (from property in GetAssetTypes()
                         select new SelectListItem()
                         {
                             Text = property.ToString(),
                             Value = property.ToString()
                         });
        }

		public bool AuthenticateUser(string input)
		{
			if (!base.HttpContext.Request.IsAuthenticated || !(base.HttpContext.User.Identity is FormsIdentity))
			{
				return false;
			}
			string userData = ((FormsIdentity)base.HttpContext.User.Identity).Ticket.UserData;
			return false;
		}

		public bool CanSendAutoEmail(BaseController.AutoEmailType type)
		{
			bool flag = true;
			try
			{
				switch (type)
				{
					case BaseController.AutoEmailType.Admin:
					{
						flag = Convert.ToBoolean(ConfigurationManager.AppSettings["enableAutoEmailsAdmin"]);
						break;
					}
					case BaseController.AutoEmailType.Asset:
					{
						flag = Convert.ToBoolean(ConfigurationManager.AppSettings["enableAutoEmailsAsset"]);
						break;
					}
					case BaseController.AutoEmailType.Insurance:
					{
						flag = Convert.ToBoolean(ConfigurationManager.AppSettings["enableAutoEmailsInsurance"]);
						break;
					}
					case BaseController.AutoEmailType.General:
					{
						flag = Convert.ToBoolean(ConfigurationManager.AppSettings["enableAutoEmailsGeneral"]);
						break;
					}
					case BaseController.AutoEmailType.Title:
					{
						flag = Convert.ToBoolean(ConfigurationManager.AppSettings["enableAutoEmailsTitle"]);
						break;
					}
				}
			}
			catch
			{
			}
			return flag;
		}

        public List<SelectListItem> GetAllCountries()
        {
            List<SelectListItem> countries = new List<SelectListItem>();
            countries.Add(new SelectListItem { Value = "", Text = "---" });
            foreach (var cultureInfo in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                var regionInfo = new RegionInfo(cultureInfo.Name);
                if (!countries.Any(x => x.Value == regionInfo.ThreeLetterISORegionName.ToLower())) {
                    countries.Add(new SelectListItem
                    {
                        Text = regionInfo.EnglishName,
                        Value = regionInfo.ThreeLetterISORegionName.ToLower()
                    });
                }
            }
            return countries.OrderBy(x => x.Text).ToList();
        }

		public void CanUserImportMBA(string email = null)
		{
			if (base.User != null)
			{
				if (!string.IsNullOrEmpty(email))
				{
					if (base.TempData[string.Concat("CanUserUploadMBA", email)] != null)
					{
						base.TempData.Keep(string.Concat("CanUserUploadMBA", email));
						return;
					}
					base.TempData[string.Concat("CanUserUploadMBA", email)] = this._user.CanUserUploadMBA(email);
					base.TempData.Keep(string.Concat("CanUserUploadMBA", email));
					return;
				}
				if (base.User.Identity.IsAuthenticated)
				{
					if (base.TempData[string.Concat("CanUserUploadMBA", base.User.Identity.Name)] == null)
					{
						base.TempData[string.Concat("CanUserUploadMBA", base.User.Identity.Name)] = this._user.CanUserUploadMBA(base.User.Identity.Name);
						base.TempData.Keep(string.Concat("CanUserUploadMBA", base.User.Identity.Name));
						return;
					}
					base.TempData.Keep(string.Concat("CanUserUploadMBA", base.User.Identity.Name));
				}
			}
		}

		public void CanUserManagePortfolios(string email = null)
		{
			if (base.User != null)
			{
				if (!string.IsNullOrEmpty(email))
				{
					if (base.TempData[string.Concat("CanUserManagePortfolios", email)] != null)
					{
						base.TempData.Keep(string.Concat("CanUserManagePortfolios", email));
						return;
					}
					base.TempData[string.Concat("CanUserManagePortfolios", email)] = this._user.CanUserManagePortfolios(email);
					base.TempData.Keep(string.Concat("CanUserManagePortfolios", email));
					return;
				}
				if (base.User.Identity.IsAuthenticated)
				{
					if (base.TempData[string.Concat("CanUserManagePortfolios", base.User.Identity.Name)] == null)
					{
						base.TempData[string.Concat("CanUserManagePortfolios", base.User.Identity.Name)] = this._user.CanUserManagePortfolios(base.User.Identity.Name);
						base.TempData.Keep(string.Concat("CanUserManagePortfolios", base.User.Identity.Name));
						return;
					}
					base.TempData.Keep(string.Concat("CanUserManagePortfolios", base.User.Identity.Name));
				}
			}
		}

		public void CreateCookie(bool remember, UserDataModel model)
		{
			string username = model.Username;
			DateTime now = DateTime.Now;
			DateTime dateTime = DateTime.Now;
			FormsAuthenticationTicket formsAuthenticationTicket = new FormsAuthenticationTicket(1, username, now, dateTime.AddYears(1), remember, "");
			string str = FormsAuthentication.Encrypt(formsAuthenticationTicket);
			HttpCookie httpCookie = new HttpCookie(string.Concat(model.Username, model.MachineName), str)
			{
				Path = FormsAuthentication.FormsCookiePath
			};
			if (FormsAuthentication.CookieDomain != null)
			{
				httpCookie.Domain = FormsAuthentication.CookieDomain;
			}
			if (remember)
			{
				httpCookie.Expires = formsAuthenticationTicket.Expiration;
			}
			System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
			FormsAuthentication.SetAuthCookie(model.Username, false);
		}

		public string Decode(string byteArray)
		{
			byte[] numArray = MachineKey.Unprotect(Convert.FromBase64String(byteArray), new string[] { "Protect Code" });
			char[] chrArray = new char[(int)numArray.Length / 2];
			Buffer.BlockCopy(numArray, 0, chrArray, 0, (int)numArray.Length);
			return new string(chrArray);
		}

		public string DetermineCompName()
		{
			string empty = string.Empty;
			string userHostAddress = base.Request.UserHostAddress;
			IPAddress pAddress = IPAddress.Parse(userHostAddress);
			try
			{
				empty = Dns.GetHostEntry(pAddress).HostName.ToString().Split(new char[] { '.' }).ToList<string>().First<string>();
				if (string.IsNullOrEmpty(empty))
				{
					empty = userHostAddress;
				}
			}
			catch
			{
				empty = userHostAddress;
			}
			return empty;
		}

		public string Encode(string code)
		{
			byte[] numArray = new byte[code.Length * 2];
			Buffer.BlockCopy(code.ToCharArray(), 0, numArray, 0, (int)numArray.Length);
			return Convert.ToBase64String(MachineKey.Protect(numArray, new string[] { "Protect Code" }));
		}

		public Guid generatedGuidIfNone(Guid guid)
		{
			if (guid != Guid.Parse("00000000-0000-0000-0000-000000000000"))
			{
				return guid;
			}
			return Guid.NewGuid();
		}

		public string GetAccessCode(string username)
		{
			UserModel userByUsername = this._user.GetUserByUsername(username);
			string str = this.DetermineCompName();
			return this._securityManager.AddUserMachine(userByUsername, str, this._securityManager.GenerateCode(), true);
		}

		public string GetIPAddress()
		{
			return base.Request.UserHostAddress;
		}

		public CookieViewModel GetUserCookies()
		{
			CookieViewModel cookieViewModel = new CookieViewModel();
			foreach (string cooky in base.Request.Cookies)
			{
				HttpCookie item = base.Request.Cookies[cooky];
				if (item.Name.Contains("ASP.NET_SessionId") || item.Name.Contains(".ASPXAUTH"))
				{
					continue;
				}
				CookieViewModel.Cookie cookie = new CookieViewModel.Cookie()
				{
					Name = item.Name,
					Domain = item.Domain,
					Expires = item.Expires.ToLongDateString(),
					Path = item.Path,
					HttpOnly = (item.HttpOnly ? "Yes" : "No"),
					Shareable = (item.Shareable ? "Yes" : "No")
				};
				try
				{
					FormsAuthenticationTicket formsAuthenticationTicket = FormsAuthentication.Decrypt(item.Value);
					cookie.TicketPath = formsAuthenticationTicket.CookiePath;
					cookie.TicketExpiration = formsAuthenticationTicket.Expiration.ToLongDateString();
					cookie.TicketExpired = (formsAuthenticationTicket.Expired ? "Yes" : "No");
					cookie.TicketPersistent = (formsAuthenticationTicket.IsPersistent ? "Yes" : "No");
					cookie.TicketIssueDate = formsAuthenticationTicket.IssueDate.ToLongDateString();
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					cookie.TicketPath = "Could not decrypt ticket";
					cookie.TicketExpired = string.Concat("Error: ", exception.Message);
					cookie.TicketPersistent = string.Concat("Stack: ", exception.StackTrace);
				}
				cookieViewModel.Cookies.Add(cookie);
			}
			return cookieViewModel;
		}

		public bool HasValidCookie(UserDataModel model)
		{
			bool flag;
			HttpCookie item = base.Request.Cookies[string.Concat(model.Username, model.MachineName)];
			if (item == null)
			{
				this.CreateCookie(true, model);
				return false;
			}
			try
			{
				FormsAuthenticationTicket formsAuthenticationTicket = FormsAuthentication.Decrypt(item.Value);
				string cookiePath = formsAuthenticationTicket.CookiePath;
				DateTime expiration = formsAuthenticationTicket.Expiration;
				bool expired = formsAuthenticationTicket.Expired;
				bool isPersistent = formsAuthenticationTicket.IsPersistent;
				DateTime issueDate = formsAuthenticationTicket.IssueDate;
				string name = formsAuthenticationTicket.Name;
				if (expired)
				{
					FormsAuthentication.RenewTicketIfOld(formsAuthenticationTicket);
					string str = FormsAuthentication.Encrypt(formsAuthenticationTicket);
					HttpCookie httpCookie = new HttpCookie(string.Concat(model.Username, model.MachineName), str)
					{
						Path = FormsAuthentication.FormsCookiePath
					};
					if (FormsAuthentication.CookieDomain != null)
					{
						httpCookie.Domain = FormsAuthentication.CookieDomain;
					}
					System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
				FormsAuthentication.SetAuthCookie(model.Username, false);
				flag = true;
			}
			catch (Exception exception)
			{
				base.Response.Cookies.Remove(string.Concat(model.Username, model.MachineName));
				base.Request.Cookies.Remove(string.Concat(model.Username, model.MachineName));
				flag = false;
			}
			return flag;
		}

		public bool isEmail(string inputEmail)
		{
			if ((new Regex("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$")).IsMatch(inputEmail))
			{
				return true;
			}
			return false;
		}

		public IEnumerable<SelectListItem> populateAssetTypeDDL()
		{
			return 
				from AssetType type in Enum.GetValues(typeof(AssetType))
				select new SelectListItem()
				{
					Text = EnumHelper.GetEnumDescription(type),
					Value = type.ToString()
				};
		}

		public void Refresh(int? level)
		{
			base.HttpContext.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
			base.HttpContext.Response.AddHeader("Pragma", "no-cache");
			base.HttpContext.Response.AddHeader("Expires", "0");
			base.HttpContext.Response.AddHeader("refresh", string.Concat("content='0'; url=", base.Request.Url.GetLeftPart(UriPartial.Authority)));
			if (level.HasValue)
			{
				int? nullable = level;
				if (nullable.HasValue)
				{
					int valueOrDefault = nullable.GetValueOrDefault();
					if (valueOrDefault != 1)
					{
						return;
					}
					string str = this.DetermineCompName();
					foreach (string cooky in base.Request.Cookies)
					{
						if (!cooky.ToLower().Contains(str.ToLower()))
						{
							continue;
						}
						HttpCookie item = base.Request.Cookies[cooky];
						string str1 = cooky.Replace(str, "");
						this.CreateCookie(true, new UserDataModel()
						{
							MachineName = str,
							IPAddress = "",
							SecurityCode = "",
							Username = str1
						});
					}
				}
			}
		}

		public void ResendEmail(ISecurityManager securityManager, IUserManager userManager, IEPIFundEmailService email)
		{
			UserModel userByUsername = userManager.GetUserByUsername(base.User.Identity.Name);
			string codeFromPendingMachine = securityManager.GetCodeFromPendingMachine(this.DetermineCompName(), userByUsername);
			if (this.CanSendAutoEmail(BaseController.AutoEmailType.General))
			{
				email.Send(new AuthenticateComputerEmail()
				{
					To = userByUsername.Username,
					IP = base.Request.UserHostAddress,
					Code = codeFromPendingMachine,
					UserName = userByUsername.FullName
				});
			}
		}

		public void SendAccessEmail(string username)
		{
			UserModel userByUsername = this._user.GetUserByUsername(username);
			string str = this.DetermineCompName();
			string str1 = this._securityManager.AddUserMachine(userByUsername, str, this._securityManager.GenerateCode(), true);
			if (this.CanSendAutoEmail(BaseController.AutoEmailType.General))
			{
				this._email.Send(new AuthenticateComputerEmail()
				{
					To = userByUsername.Username,
					IP = base.Request.UserHostAddress,
					Code = str1,
					UserName = userByUsername.FullName
				});
			}
		}

		public void SetUpJsonImages(AssetViewModel asset, UserModel user)
		{
			AssetImagesViewModel assetImagesViewModel = new AssetImagesViewModel();
			foreach (AssetImage image in asset.Images)
			{
				AssetImageViewModel assetImageViewModel = new AssetImageViewModel();
				assetImageViewModel.ImageToModel(image);
				assetImageViewModel.DateString = asset.DateForTempImages;
				assetImageViewModel.UserId = user.UserId.ToString();
				assetImagesViewModel.Images.Add(assetImageViewModel);
			}
			asset.JsonPictures = JsonConvert.SerializeObject(assetImagesViewModel);
		}

		public void SetUpJsonImages(PaperCommercialAssetViewModel pcavm = null, RealEstateCommercialAssetViewModel pecavm = null, int? userId = null)
		{
			int value;
			string str;
			string empty;
			if (pcavm != null)
			{
				AssetImagesViewModel assetImagesViewModel = new AssetImagesViewModel();
				foreach (AssetImageModel image in pcavm.Images)
				{
					AssetImageViewModel assetImageViewModel = new AssetImageViewModel();
					assetImageViewModel.ImageToModel(image, pcavm.GuidId);
					assetImageViewModel.DateString = pcavm.DateForTempImages;
					AssetImageViewModel assetImageViewModel1 = assetImageViewModel;
					if (userId.HasValue)
					{
						value = userId.Value;
						empty = value.ToString();
					}
					else
					{
						empty = string.Empty;
					}
					assetImageViewModel1.UserId = empty;
					assetImagesViewModel.Images.Add(assetImageViewModel);
				}
				pcavm.JsonPictures = JsonConvert.SerializeObject(assetImagesViewModel);
				return;
			}
			if (pecavm != null)
			{
				AssetImagesViewModel assetImagesViewModel1 = new AssetImagesViewModel();
				foreach (AssetImageModel assetImageModel in pecavm.Images)
				{
					AssetImageViewModel dateForTempImages = new AssetImageViewModel();
					dateForTempImages.ImageToModel(assetImageModel, pecavm.GuidId);
					dateForTempImages.DateString = pecavm.DateForTempImages;
					AssetImageViewModel assetImageViewModel2 = dateForTempImages;
					if (userId.HasValue)
					{
						value = userId.Value;
						str = value.ToString();
					}
					else
					{
						str = string.Empty;
					}
					assetImageViewModel2.UserId = str;
					assetImagesViewModel1.Images.Add(dateForTempImages);
				}
				pecavm.JsonPictures = JsonConvert.SerializeObject(assetImagesViewModel1);
			}
		}

		public void ValidateAdmin(UserModel user)
		{
			if (user.UserType != UserType.CorpAdmin && user.UserType != UserType.CorpAdmin2 && user.UserType != UserType.SiteAdmin)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You do not have access to view this page.");
				base.RedirectToAction("Index", "Home");
			}
		}

		public bool ValidateAdminUser(UserModel user)
		{
			if (user.UserType != UserType.CorpAdmin && user.UserType != UserType.CorpAdmin2 && user.UserType != UserType.SiteAdmin && user.UserType != UserType.ICAdmin)
			{
				return false;
			}
			return true;
		}

		public bool ValidatePFUser(UserModel user)
		{
			if (user.UserType != UserType.CorpAdmin && user.UserType != UserType.CorpAdmin2 && user.UserType != UserType.SiteAdmin && user.UserType != UserType.Investor && user.UserType != UserType.CREBroker && user.UserType != UserType.CRELender && user.UserType != UserType.ICAdmin && user.UserType != UserType.ListingAgent)
			{
				return false;
			}
			return true;
		}

		public bool VerifyCode(ISecurityManager securityManager, string code, UserModel user)
		{
			return securityManager.VerifyCode(code, this.DetermineCompName(), user);
		}

		public void WashTempData(string email = null)
		{
			if (base.User != null)
			{
				if (string.IsNullOrEmpty(email))
				{
					if (base.TempData[string.Concat("CanUserManagePortfolios", base.User.Identity.Name)] != null)
					{
						base.TempData.Remove(string.Concat("CanUserManagePortfolios", base.User.Identity.Name));
					}
					if (base.TempData[string.Concat("CanUserUploadMBA", base.User.Identity.Name)] != null)
					{
						base.TempData.Remove(string.Concat("CanUserUploadMBA", base.User.Identity.Name));
					}
				}
				else
				{
					if (base.TempData[string.Concat("CanUserManagePortfolios", email)] != null)
					{
						base.TempData.Remove(string.Concat("CanUserManagePortfolios", email));
					}
					if (base.TempData[string.Concat("CanUserUploadMBA", email)] != null)
					{
						base.TempData.Remove(string.Concat("CanUserUploadMBA", email));
						return;
					}
				}
			}
		}

		public enum AutoEmailType
		{
			Admin,
			Asset,
			Insurance,
			General,
			Title
		}
	}
}