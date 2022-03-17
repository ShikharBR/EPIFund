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
using Inview.Epi.EpiFund.Web.Providers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Inview.Epi.EpiFund.Web.Controllers
{
	[LayoutActionFilter]
	public class HomeController : BaseController
	{
		private IUserManager _userManager;

		private IAuthProvider _auth;

		private IEPIFundEmailService _email;

		private IContactService _contact;

		private IFileManager _fileManager;

		private IAssetManager _asset;

		private ISecurityManager _securityManager;

		private IDocusignServiceManager _docs;

		private IPDFService _pdf;

		private ISellerManager _seller;

		private IPortfolioManager _portfolio;

		protected string WindowStatusText = "";

		public HomeController(IUserManager manager, IAuthProvider auth, IEPIFundEmailService email, IContactService contact, IAssetManager asset, IFileManager fileManager, ISecurityManager securityManager, IDocusignServiceManager docs, IPDFService pdfService, ISellerManager seller, IPortfolioManager portfolio) : base(securityManager, email, manager)
		{
			this._seller = seller;
			this._docs = docs;
			this._asset = asset;
			this._contact = contact;
			this._email = email;
			this._auth = auth;
			this._userManager = manager;
			this._fileManager = fileManager;
			this._securityManager = securityManager;
			this._pdf = pdfService;
			this._portfolio = portfolio;
		}

		public ViewResult AboutUs()
		{
			return base.View();
		}

		[ChildActionOnly]
		public ActionResult AdminSideNavigationMenu()
		{
			UserModel userByUsername = this._userManager.GetUserByUsername(base.User.Identity.Name);
			return this.PartialView("../Shared/_AdminNavigationMenu", userByUsername);
		}

		public ViewResult Affiliations()
		{
			return base.View();
		}

		[HttpGet]
		public bool AuthSubmit(string Username, string Password)
		{
			if ((Username == "uscreonline") & (Password == "USR#16nline"))
			{
				base.Session["RootAuth"] = true;
				return true;
			}
			base.Session["RootAuth"] = false;
			return false;
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

		private void BuildMHPViewBags(Dictionary<string, IEnumerable<SelectListItem>> resultSet)
		{
			((dynamic)base.ViewBag).MHPArchPropertyDetails = resultSet["architecture"];
			dynamic viewBag = base.ViewBag;
			IEnumerable<SelectListItem> item = resultSet["architecture"];
			viewBag.MHPArchSel = (
				from x in item
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPArchCnt = resultSet["architecture"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPComplPropertyDetails = resultSet["complex"];
			dynamic obj = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems = resultSet["complex"];
			obj.MHPComplSel = (
				from x in selectListItems
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPFComplCnt = resultSet["complex"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPGenPropertyDetails = resultSet["general"];
			dynamic viewBag1 = base.ViewBag;
			IEnumerable<SelectListItem> item1 = resultSet["general"];
			viewBag1.MHPGenSel = (
				from x in item1
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPGenCnt = resultSet["general"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPSecuPropertyDetails = resultSet["security"];
			dynamic obj1 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems1 = resultSet["security"];
			obj1.MHPSecuSel = (
				from x in selectListItems1
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPSecuCnt = resultSet["security"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPParkingPropertyDetails = resultSet["parking"];
			dynamic viewBag2 = base.ViewBag;
			IEnumerable<SelectListItem> item2 = resultSet["parking"];
			viewBag2.MHPParkSel = (
				from x in item2
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPParkCnt = resultSet["parking"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPConstPropertyDetails = resultSet["construction"];
			dynamic obj2 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems2 = resultSet["construction"];
			obj2.MHPConstSel = (
				from x in selectListItems2
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPConstCnt = resultSet["construction"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPHVACPropertyDetails = resultSet["hvac"];
			dynamic viewBag3 = base.ViewBag;
			IEnumerable<SelectListItem> item3 = resultSet["hvac"];
			viewBag3.MHPHVACSel = (
				from x in item3
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPHVACCnt = resultSet["hvac"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPPropPropertyDetails = resultSet["property"];
			dynamic obj3 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems3 = resultSet["property"];
			obj3.MHPPropSel = (
				from x in selectListItems3
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPPropCnt = resultSet["property"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPRoofPropertyDetails = resultSet["roofing"];
			dynamic viewBag4 = base.ViewBag;
			IEnumerable<SelectListItem> item4 = resultSet["roofing"];
			viewBag4.MHPRoofSel = (
				from x in item4
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPRoofCnt = resultSet["roofing"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPMFPropertyDetails = resultSet["mf"];
			dynamic obj4 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems4 = resultSet["mf"];
			obj4.MHPMFlSel = (
				from x in selectListItems4
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPMFCnt = resultSet["mf"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPKitcPropertyDetails = resultSet["kitchen"];
			dynamic viewBag5 = base.ViewBag;
			IEnumerable<SelectListItem> item5 = resultSet["kitchen"];
			viewBag5.MHPKitcSel = (
				from x in item5
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPKitcCnt = resultSet["kitchen"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPIntPropertyDetails = resultSet["interior"];
			dynamic obj5 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems5 = resultSet["interior"];
			obj5.MHPIntSel = (
				from x in selectListItems5
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPIntCnt = resultSet["interior"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPExtPropertyDetails = resultSet["exterior"];
			dynamic viewBag6 = base.ViewBag;
			IEnumerable<SelectListItem> item6 = resultSet["exterior"];
			viewBag6.MHPExtlSel = (
				from x in item6
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPExtCnt = resultSet["exterior"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPTechPropertyDetails = resultSet["tech"];
			dynamic obj6 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems6 = resultSet["tech"];
			obj6.MHPTechSel = (
				from x in selectListItems6
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPTechCnt = resultSet["tech"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPArchPropertyDetails1 = resultSet["mfArch"];
			dynamic viewBag7 = base.ViewBag;
			IEnumerable<SelectListItem> item7 = resultSet["mfArch"];
			viewBag7.MHPArchSel1 = (
				from x in item7
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPArchCnt1 = resultSet["mfArch"].Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPMHPPropertyDetails = resultSet["mhp"];
			dynamic obj7 = base.ViewBag;
			IEnumerable<SelectListItem> selectListItems7 = resultSet["mhp"];
			obj7.MHPMHPSel = (
				from x in selectListItems7
				where x.Selected
				select x).Count<SelectListItem>();
			((dynamic)base.ViewBag).MHPMHPCnt = resultSet["mhp"].Count<SelectListItem>();
		}

		[ChildActionOnly]
		public ActionResult CarouselPartialView(bool IsAdmin = false)
		{
			ActionResult actionResult;
			try
			{
				UrlHelper urlHelper = new UrlHelper(base.HttpContext.Request.RequestContext);
				string str = urlHelper.RequestContext.RouteData.Values["action"].ToString();
				((dynamic)base.ViewBag).IsAdmin = IsAdmin;
				if (str != "ViewAsset")
				{
					actionResult = (str != "ViewSampleAsset" ? this.PartialView("../Shared/_CarouselPartialView", this._asset.GetCarouselData()) : this.PartialView("../Shared/_ViewAssetCarouselPartialView", this._asset.GetSampleAsset(true)));
				}
				else
				{
					actionResult = this.PartialView("../Shared/_ViewAssetCarouselPartialView", this._asset.GetAsset(new Guid(urlHelper.RequestContext.RouteData.Values["id"].ToString()), true));
				}
			}
			catch
			{
				actionResult = this.PartialView("../Shared/_CarouselPartialView", this._asset.GetCarouselData());
			}
			return actionResult;
		}

		[HttpGet]
		public ViewResult ChangePassword()
		{
			UserModel userByUsername = this._userManager.GetUserByUsername(base.User.Identity.Name);
			if (userByUsername.UserType == UserType.CorpAdmin || userByUsername.UserType == UserType.CorpAdmin2 || userByUsername.UserType == UserType.ICAdmin || userByUsername.UserType == UserType.SiteAdmin)
			{
				((dynamic)base.ViewBag).Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
			}
			else
			{
				((dynamic)base.ViewBag).Layout = "~/Views/Shared/_Layout.cshtml";
			}
			return base.View(new ChangePasswordModel());
		}

		[HttpPost]
		public ActionResult ChangePassword(ChangePasswordModel model)
		{
			UserModel userByUsername = this._userManager.GetUserByUsername(base.User.Identity.Name);
			if (!base.ModelState.IsValid)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "One or more fields are invalid.");
			}
			else
			{
				this._userManager.ChangePassword(userByUsername.UserId, model.Password);
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Password successfully changed.");
			}
			if (userByUsername.UserType == UserType.CorpAdmin || userByUsername.UserType == UserType.CorpAdmin2 || userByUsername.UserType == UserType.ICAdmin || userByUsername.UserType == UserType.SiteAdmin)
			{
				((dynamic)base.ViewBag).Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
			}
			else
			{
				((dynamic)base.ViewBag).Layout = "~/Views/Shared/_Layout.cshtml";
			}
			return base.View(model);
		}

		[HttpGet]
		public ViewResult ContactUs()
		{
			ContactUsModel contactUsModel = new ContactUsModel();
			if (base.User.Identity.IsAuthenticated)
			{
				UserModel userByUsername = this._userManager.GetUserByUsername(base.User.Identity.Name);
				contactUsModel.ContactNumber = (!string.IsNullOrEmpty(userByUsername.CellNumber) ? userByUsername.CellNumber : userByUsername.WorkNumber);
				contactUsModel.EmailAddress = userByUsername.Username;
				contactUsModel.Name = userByUsername.FullName;
			}
			return base.View(contactUsModel);
		}

		[HttpPost]
		public ActionResult ContactUs(ContactUsModel model)
		{
			if (!base.ModelState.IsValid)
			{
				return base.View(model);
			}
			if (!base.isEmail(model.EmailAddress) || model.SelectedTopics == null)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "The enter a valid email and select at least one topic.");
				return base.View("ContactUs", model);
			}
			model.DateOfInquiry = DateTime.Now;
			int num = this._contact.SaveInquiry(model);
			StringBuilder stringBuilder = new StringBuilder();
			model.SelectedTopics.ForEach((string f) => {
				stringBuilder.Append(f);
				stringBuilder.Append("; ");
			});
			if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
			{
				ConfirmationReceiptContactUsEmail confirmationReceiptContactUsEmail = new ConfirmationReceiptContactUsEmail()
				{
					ContactNumber = model.ContactNumber,
					DateOfInquiry = model.DateOfInquiry,
					Email = model.EmailAddress,
					Inquiry = model.QuestionsComments,
					Name = model.Name,
					To = model.Name,
					EmailOfPersonSendingInquiry = model.EmailAddress,
					Topics = stringBuilder.ToString()
				};
				this._email.Send(confirmationReceiptContactUsEmail);
				ContactUsForwardEmail contactUsForwardEmail = new ContactUsForwardEmail()
				{
					DateOfInquiry = model.DateOfInquiry,
					Email = ConfigurationManager.AppSettings["AdminEmail"].ToString(),
					Inquiry = model.QuestionsComments,
					Name = model.Name,
					To = "Admin User",
					EmailOfPersonSendingInquiry = model.EmailAddress,
					ContactNumber = model.ContactNumber,
					Topics = stringBuilder.ToString()
				};
				this._email.Send(contactUsForwardEmail);
			}
			return base.RedirectToAction("ContactUsReceipt", new { contactId = num });
		}

		[HttpGet]
		public ViewResult ContactUsReceipt(int contactId)
		{
			return base.View(this._contact.GetReceipt(contactId));
		}

		[ChildActionOnly]
		public ActionResult CounterPartialView()
		{
			((dynamic)base.ViewBag).TotalAssetValue = Math.Round(this._asset.GetPublishedCommercialAssetValue());
			((dynamic)base.ViewBag).TotalAssets = this._asset.GetTotalNumberOfAssets();
			((dynamic)base.ViewBag).TotalPublishedAssets = this._asset.GetPublishedAssetCount();
			((dynamic)base.ViewBag).TotalMultiFamilyUnits = this._asset.GetTotalNumberOfMultiFamilyUnits();
			((dynamic)base.ViewBag).TotalSumCommercialPropertySqFt = this._asset.GetTotalSumCommercialSqFt();
			((dynamic)base.ViewBag).TotalIndustryParticipants = this._userManager.GetTotalIndustryParticipants();
			((dynamic)base.ViewBag).TotalMBACount = this._userManager.GetImportedMBACount();
			((dynamic)base.ViewBag).TotalNARCount = this._userManager.GetImportedNARCount();
			return base.PartialView("../Shared/_CounterPartialView");
		}

		[HttpGet]
		public ViewResult DataPortal()
		{
			return base.View();
		}

		[HttpPost]
		public FileUploadJsonResult DocumentDelete(string docId, Guid guidId)
		{
			if (docId != null && this._fileManager.DeleteFile(docId, guidId, FileType.Document))
			{
				return new FileUploadJsonResult()
				{
					Data = new { message = "true" }
				};
			}
			return new FileUploadJsonResult()
			{
				Data = new { message = "Could Not Find File" }
			};
		}

		[HttpPost]
		public FileUploadJsonResult DocumentUpload(HttpPostedFileBase file, Guid guidId, string title)
		{
			if (file == null || string.IsNullOrEmpty(file.FileName))
			{
				return new FileUploadJsonResult()
				{
					Data = new { message = "FileIsNull" }
				};
			}
			string str = this._fileManager.SaveFile(file, guidId, FileType.Document);
			if (str == null)
			{
				return new FileUploadJsonResult()
				{
					Data = new { message = "false" }
				};
			}
			string str1 = string.Concat(new string[] { "/Admin/DownloadDocument?fileName=", HttpUtility.UrlEncode(str), "&assetId=", guidId.ToString(), "&contentType=", HttpUtility.UrlEncode(file.ContentType), "&title=", HttpUtility.UrlEncode(title) });
			return new FileUploadJsonResult()
			{
				Data = new { message = "true", filename = str, contentType = file.ContentType, size = this._fileManager.GetFileSize(file.ContentLength), downloadUrl = str1 }
			};
		}

		[Authorize]
		[HttpGet]
		public ActionResult EditProfile()
		{
			UserModel userByUsername = this._userManager.GetUserByUsername(base.User.Identity.Name);
			ICStatus? cStatus = userByUsername.ICStatus;
			if ((cStatus.GetValueOrDefault() == ICStatus.Rejected ? cStatus.HasValue : false))
			{
				base.TempData["UserTypes"] = new List<SelectListItem>()
				{
					new SelectListItem()
					{
						Selected = false,
						Text = "CRE Mtg Broker",
						Value = UserType.CREBroker.ToString()
					},
					new SelectListItem()
					{
						Selected = false,
						Text = "CRE Mtg Lender",
						Value = UserType.CRELender.ToString()
					},
					new SelectListItem()
					{
						Selected = false,
						Text = "Principal Investor",
						Value = UserType.Investor.ToString()
					},
					new SelectListItem()
					{
						Selected = false,
						Text = "CRE Owner",
						Value = UserType.CREOwner.ToString()
					}
				};
			}
			if (userByUsername.UserType == UserType.CorpAdmin || userByUsername.UserType == UserType.CorpAdmin2 || userByUsername.UserType == UserType.ICAdmin || userByUsername.UserType == UserType.SiteAdmin)
			{
				cStatus = userByUsername.ICStatus;
				if ((cStatus.GetValueOrDefault() == ICStatus.Rejected ? cStatus.HasValue : false))
				{
					//goto Label1; 
				}
				((dynamic)base.ViewBag).Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
				return base.View(userByUsername);
			}
			((dynamic)base.ViewBag).Layout = "~/Views/Shared/_Layout.cshtml";
			return base.View(userByUsername);
		}

		[Authorize]
		[HttpPost]
		public ActionResult EditProfile(UserModel model)
		{
			ICStatus? cStatus;
			UserModel userByUsername;
			if (model.UserType == UserType.ICAdmin)
			{
				cStatus = model.ICStatus;
				if ((cStatus.GetValueOrDefault() == ICStatus.Rejected ? !cStatus.HasValue : true))
				{
					if (string.IsNullOrEmpty(model.CorporateTIN))
					{
						base.ModelState.AddModelError("CorporateTIN", "Corporate TIN or Sole Proprietorship SSN required.");
					}
					if (string.IsNullOrEmpty(model.LicenseDesc))
					{
						base.ModelState.AddModelError("LicenseDesc", "Operating License Description required.");
					}
					if (string.IsNullOrEmpty(model.LicenseNumber))
					{
						base.ModelState.AddModelError("LicenseNumber", "Operating License Number required.");
					}
				}
			}
			if (model.UserType == UserType.CorpAdmin || model.UserType == UserType.CorpAdmin2 || model.UserType == UserType.ICAdmin || model.UserType == UserType.SiteAdmin)
			{
				cStatus = model.ICStatus;
				if ((cStatus.GetValueOrDefault() == ICStatus.Rejected ? cStatus.HasValue : false))
				{
					//goto Label1;
				}
				((dynamic)base.ViewBag).Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
				if (!base.ModelState.IsValid)
				{
					return base.View(model);
				}
				this._userManager.UpdateUser(model);
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Profile updated.");
				userByUsername = this._userManager.GetUserByUsername(base.User.Identity.Name);
				return base.View(userByUsername);
			}
			((dynamic)base.ViewBag).Layout = "~/Views/Shared/_Layout.cshtml";
			if (!base.ModelState.IsValid)
			{
				return base.View(model);
			}
			this._userManager.UpdateUser(model);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Profile updated.");
			userByUsername = this._userManager.GetUserByUsername(base.User.Identity.Name);
			return base.View(userByUsername);
		}

		private void EmailJVMAParticipantPIRegistered(UserModel user)
		{
			this._userManager.GetJVMAParticipantByUser(user.Username);
		}

		private void EmailPIFromFeeSimple(UserModel user)
		{
			if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
			{
				UserModel jVMAParticipantByUser = this._userManager.GetJVMAParticipantByUser(user.Username);
				if (jVMAParticipantByUser != null)
				{
					HtmlString htmlString = new HtmlString(this._userManager.GetPIAssetHistoryDescriptionsHTML(user.Username));
					try
					{
						this._email.Send(new NotificationToReferredPIUploadedAssetEmail()
						{
							PrincipalInvestorName = user.FullName,
							Date = DateTime.Now,
							ReferrerName = jVMAParticipantByUser.FullName,
							UploadedAssets = htmlString.ToString(),
							To = user.Username
						});
					}
					catch (Exception exception)
					{
						throw;
					}
				}
			}
		}

		[HttpGet]
		public ActionResult EmploymentOpportunities()
		{
			return base.View();
		}

		[HttpGet]
		public ActionResult EpiFundBuysPaper(string propertyType)
		{
			string str = propertyType;
			((dynamic)base.ViewBag).PortfolioList = this.populatePortfolioListDDL();
			IEnumerable<PropertyType> propertyTypes = Enum.GetValues(typeof(PropertyType)).Cast<PropertyType>();
			if (string.IsNullOrWhiteSpace(str))
			{
				str = PropertyType.Commercial.ToString();
			}
			PropertyType propertyType1 = propertyTypes.FirstOrDefault<PropertyType>((PropertyType pt) => pt.ToString() == str);
			IEnumerable<SelectListItem> selectListItem = 
				from property in propertyTypes
				select new SelectListItem()
				{
					Text = property.ToString(),
					Value = property.ToString(),
					Selected = property == propertyType1
				};
			((dynamic)base.ViewBag).PropertyTypes = selectListItem;
			if (propertyType1 != PropertyType.Residential)
			{
				PaperCommercialAssetViewModel paperCommercialAsset = this.GetPaperCommercialAsset();
				paperCommercialAsset.PagePropertyTypes = selectListItem;
				paperCommercialAsset.GuidId = Guid.NewGuid();
				paperCommercialAsset.DeferredMaintenanceItems = this._asset.GetDefaultDeferredMaintenanceItems();
				paperCommercialAsset.DeferredMaintenanceItems = this._asset.RemoveMaintainanceItems(AssetType.Office, paperCommercialAsset.DeferredMaintenanceItems);
				paperCommercialAsset.DeferredMaintenanceItems = this._asset.OrderMaintainanceItems(AssetType.Office, paperCommercialAsset.DeferredMaintenanceItems);
				paperCommercialAsset.DeferredMaintenanceItemsMF = this._asset.GetDefaultDeferredMaintenanceItems();
				paperCommercialAsset.DeferredMaintenanceItemsMF = this._asset.RemoveMaintainanceItems(AssetType.MultiFamily, paperCommercialAsset.DeferredMaintenanceItemsMF);
				paperCommercialAsset.DeferredMaintenanceItemsMF = this._asset.OrderMaintainanceItems(AssetType.MultiFamily, paperCommercialAsset.DeferredMaintenanceItemsMF);
				this.BuildMFViewBags(this.populateMFDetailsCheckBoxList(paperCommercialAsset.MFDetails));
				this.BuildMHPViewBags(this.populateMHPDetailsCheckBoxList(paperCommercialAsset.MHPDetails));
				this.BuildCommViewBags(this.populateCommDetailsCheckBoxList(paperCommercialAsset.PropertyDetails, AssetType.Office));
				return base.View("EpiFundBuysCommercialPaper", paperCommercialAsset);
			}
			PaperResidentialAssetViewModel paperResidentialAssetViewModel = new PaperResidentialAssetViewModel()
			{
				PagePropertyTypes = selectListItem,
				GuidId = Guid.NewGuid(),
				DeferredMaintenanceItems = this._asset.GetDefaultDeferredMaintenanceItems()
			};
			paperResidentialAssetViewModel.DeferredMaintenanceItems = this._asset.RemoveMaintainanceItems(AssetType.MultiFamily, paperResidentialAssetViewModel.DeferredMaintenanceItems);
			paperResidentialAssetViewModel.DeferredMaintenanceItems = this._asset.OrderMaintainanceItems(AssetType.MultiFamily, paperResidentialAssetViewModel.DeferredMaintenanceItems);
			if (base.User.Identity.IsAuthenticated)
			{
				UserModel userByUsername = this._userManager.GetUserByUsername(base.User.Identity.Name);
				paperResidentialAssetViewModel.BeneficiaryName = string.Concat(userByUsername.FirstName, " ", userByUsername.LastName);
				paperResidentialAssetViewModel.BeneficiaryFullName = string.Concat(userByUsername.FirstName, " ", userByUsername.LastName);
				paperResidentialAssetViewModel.BeneficiaryContactAddress = string.Concat(userByUsername.AddressLine1, " ", userByUsername.AddressLine2);
				paperResidentialAssetViewModel.BeneficiaryCity = userByUsername.City;
				paperResidentialAssetViewModel.BeneficiaryState = userByUsername.SelectedState;
				paperResidentialAssetViewModel.BeneficiaryZip = userByUsername.Zip;
				paperResidentialAssetViewModel.BeneficiaryPhoneHome = userByUsername.HomeNumber;
				paperResidentialAssetViewModel.BeneficiaryPhoneWork = userByUsername.WorkNumber;
				paperResidentialAssetViewModel.BeneficiaryPhoneCell = userByUsername.CellNumber;
				paperResidentialAssetViewModel.BeneficiaryFax = userByUsername.FaxNumber;
				paperResidentialAssetViewModel.BeneficiaryEmail = userByUsername.Username;
			}
			return base.View("EpiFundBuysResidentialPaper", paperResidentialAssetViewModel);
		}

		[HttpPost]
		public ActionResult EpiFundBuysPaper(PaperCommercialAssetViewModel model, string SubmitPaper)
		{
			((dynamic)base.ViewBag).PortfolioList = this.populatePortfolioListDDL();
			UserModel userModel = new UserModel();
			if (base.User.Identity.IsAuthenticated)
			{
				userModel = this._userManager.GetUserByUsername(base.User.Identity.Name);
			}
			IAssetManager assetManager = this._asset;
			Guid guidId = new Guid();
			if (assetManager.GetMatchingAssetsByAPNCountyState(model.TaxAssessorNumber, "", "").Any())
			{
				base.ModelState.AddModelError("TaxAssessorNumber", "Tax Assessor Number is already in use for another asset");
			}
			if (model.TaxAssessorNumberOther != null)
			{
				IAssetManager assetManager1 = this._asset;
				guidId = new Guid();
				if (assetManager1.GetMatchingAssetsByAPNCountyState(model.TaxAssessorNumberOther, "" , "").Any())
				{
					base.ModelState.AddModelError("TaxAssessorNumberOther", "Other Tax Assessor Number is already in use for another asset");
				}
			}
			if (!base.ModelState.IsValid)
			{
				foreach (ModelError list in base.ModelState.Values.SelectMany<System.Web.Mvc.ModelState, ModelError>((System.Web.Mvc.ModelState v) => v.Errors).ToList<ModelError>())
				{
					if (list.ErrorMessage.ToLower().Contains("asset document type"))
					{
						base.ModelState.Remove("AssetDocumentType");
						base.ModelState.Add("AssetDocumentType", new System.Web.Mvc.ModelState());
						base.ModelState.SetModelValue("AssetDocumentType", new ValueProviderResult((object)AssetDocumentType.CurrentRentRoll, "CurrentRentRoll", null));
					}
					if (!list.ErrorMessage.Contains("Property Updated Year"))
					{
						continue;
					}
					base.ModelState.Remove("PropLastUpdated");
					base.ModelState.Add("PropLastUpdated", new System.Web.Mvc.ModelState());
					base.ModelState.SetModelValue("PropLastUpdated", new ValueProviderResult((object)0, "0", null));
					model.isPropUpdateUnknown = true;
				}
			}
			if (model.Images == null)
			{
				model.Images = new List<AssetImageModel>();
			}
			if (model.Documents == null)
			{
				model.Documents = new List<TempAssetDocument>();
			}
			if (model.Videos == null)
			{
				model.Videos = new List<TempAssetVideo>();
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
					if (model.Videos[i].FilePath != null)
					{
						continue;
					}
					model.Videos.RemoveAt(i);
					flag = true;
				}
				while (flag);
			}
			int num = 0;
			List<AssetImageModel> fileName = (
				from s in model.Images
				orderby s.Order
				select s).ToList<AssetImageModel>();
			for (int j = 0; j < fileName.Count; j++)
			{
				bool flag1 = false;
				do
				{
					if (j >= fileName.Count)
					{
						break;
					}
					if (fileName[j].AssetImageId == Guid.Empty && base.ModelState.IsValid)
					{
						fileName[j].OriginalFileName = fileName[j].FileName;
						fileName[j].AssetImageId = this.generatedGuidIfNone(fileName[j].AssetImageId);
						IFileManager fileManager = this._fileManager;
						string str = fileName[j].FileName;
						Guid guid = model.GuidId;
						string dateForTempImages = model.DateForTempImages;
						int userId = userModel.UserId;
						string str1 = fileManager.MoveTempAssetImage(str, guid, dateForTempImages, userId.ToString());
						if (!string.IsNullOrEmpty(str1))
						{
							fileName[j].FileName = str1;
						}
					}
					if (fileName[j].FileName != null)
					{
						fileName[j].AssetId = model.GuidId;
						fileName[j].Order = num;
						num++;
						flag1 = false;
					}
					else
					{
						try
						{
							fileName.RemoveAt(j);
							flag1 = true;
						}
						catch
						{
						}
					}
				}
				while (flag1);
			}
			model.Images = fileName;
			num = 0;
			for (int k = 0; k < model.Documents.Count; k++)
			{
				bool flag2 = false;
				do
				{
					if (k >= model.Documents.Count)
					{
						break;
					}
					if (model.Documents[k].FileName != null)
					{
						model.Documents[k].Order = num;
						num++;
						flag2 = false;
					}
					else
					{
						model.Documents.RemoveAt(k);
						flag2 = true;
					}
				}
				while (flag2);
			}
			List<SelectListItem> selectListItems = new List<SelectListItem>()
			{
				new SelectListItem()
				{
					Text = "Commercial",
					Value = "Commercial",
					Selected = true
				},
				new SelectListItem()
				{
					Text = "Residential",
					Value = "Residential"
				}
			};
			((dynamic)base.ViewBag).PropertyTypes = selectListItems;
			if (!string.IsNullOrEmpty(model.TypeOfProperty))
			{
				if (!(model.TypeOfProperty != "Multi-Family") || !(model.TypeOfProperty != "Mobile Home Parks"))
				{
					if ((int)model.ElectricMeterMethod == 0)
					{
						base.ModelState.AddModelError("ElectricMeterMethod", "Electric Meter Method is required");
					}
					if ((int)model.GasMeterMethod == 0)
					{
						base.ModelState.AddModelError("GasMeterMethod", "Gas Meter Method is required");
					}
				}
				else
				{
					CommercialType type = model.Type;
					if ((int)model.VacantSuites == 0)
					{
						base.ModelState.AddModelError("VacantSuites", "Vacant Suites is required");
					}
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
			if (model.NoteServicedByAgent.HasValue && model.NoteServicedByAgent.Value)
			{
				if (string.IsNullOrEmpty(model.AgentName))
				{
					base.ModelState.AddModelError("AgentName", "Company name of Servicing Agent required.");
				}
				if (string.IsNullOrEmpty(model.AgentPhone))
				{
					base.ModelState.AddModelError("AgentPhone", "Phone Number of Servicing Agent required.");
				}
				if (string.IsNullOrEmpty(model.AgentEmail))
				{
					base.ModelState.AddModelError("AgentEmail", "Email Address of Servicing Agent required.");
				}
				if (string.IsNullOrEmpty(model.AgentAccountNumber))
				{
					base.ModelState.AddModelError("AgentAccountNumber", "Account Number of Servicing Agent required.");
				}
				if (string.IsNullOrEmpty(model.AgentContactPerson))
				{
					base.ModelState.AddModelError("AgentContactPerson", "Servicing Contact Person required.");
				}
				if (!model.AuthorizeForwardPaymentHistory.HasValue)
				{
					base.ModelState.AddModelError("AuthorizeForwardPaymentHistory", "Do you authorize servicing agent to send information to US CRE Online Fund required.");
				}
				if (string.IsNullOrEmpty(model.AgentAddress))
				{
					base.ModelState.AddModelError("AgentAddress", "Address of Servicing Agent required.");
				}
				if (string.IsNullOrEmpty(model.AgentCity))
				{
					base.ModelState.AddModelError("AgentCity", "City of Servicing Agent required.");
				}
				if (string.IsNullOrEmpty(model.AgentState))
				{
					base.ModelState.AddModelError("AgentState", "State of Servicing Agent required.");
				}
				if (string.IsNullOrEmpty(model.AgentZip))
				{
					base.ModelState.AddModelError("AgentZip", "Zip of Servicing Agent required.");
				}
			}
			this.BuildMFViewBags(this.populateMFDetailsCheckBoxList(model.MFDetails));
			this.BuildMHPViewBags(this.populateMHPDetailsCheckBoxList(model.MHPDetails));
			this.BuildCommViewBags(this.populateCommDetailsCheckBoxList(model.PropertyDetails, AssetType.Office));
			if (!base.ModelState.IsValid)
			{
				base.SetUpJsonImages(model, null, new int?(userModel.UserId));
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Please fill in all the reqd. fields before submitting.");
				return base.View("EpiFundBuysCommercialPaper", model);
			}
			if (base.User.Identity.IsAuthenticated)
			{
				model.Username = base.User.Identity.Name.ToLower();
			}
			int num1 = this._asset.SavePaperCommercial(model);
			if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
			{
				AssetDescriptionModel assetByAssetNumber = this._asset.GetAssetByAssetNumber(num1);
				this._email.Send(new ConfirmationCRNoteSellerFormSubmittalEmail()
				{
					Email = model.EmailAddress,
					Name = string.Concat(model.NameOfPrincipal, " ", model.NameOfCoPrincipal),
					NoteType = "Paper",
					APN = string.Concat(model.TaxAssessorNumber, "; ", model.TaxAssessorNumberOther),
					CorpOfficer = model.CorporateOwnershipOfficer,
					VestingEntity = model.CorporateName,
					AssetDescription = assetByAssetNumber
				});
				List<UserQuickViewModel> users = this._userManager.GetUsers(new UserSearchModel()
				{
					UserTypeFilter = new UserType?(UserType.CorpAdmin2),
					ShowActiveOnly = true
				});
				users.AddRange(this._userManager.GetUsers(new UserSearchModel()
				{
					UserTypeFilter = new UserType?(UserType.CorpAdmin),
					ShowActiveOnly = true
				}));
				foreach (UserQuickViewModel user in users)
				{
					IEPIFundEmailService ePIFundEmailService = this._email;
					NotificationForCorpAdminPaperOrRealEstateCreated notificationForCorpAdminPaperOrRealEstateCreated = new NotificationForCorpAdminPaperOrRealEstateCreated()
					{
						To = user.Username
					};
					string leftPart = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
					guidId = model.GuidId;
					notificationForCorpAdminPaperOrRealEstateCreated.Link = string.Format("{0}/DataPortal/ViewAsset/{1}", leftPart, guidId.ToString());
					notificationForCorpAdminPaperOrRealEstateCreated.NoteType = "Paper";
					ePIFundEmailService.Send(notificationForCorpAdminPaperOrRealEstateCreated);
				}
			}
			if (base.User.Identity.IsAuthenticated)
			{
				this.EmailPIFromFeeSimple(this._userManager.GetUserByUsername(base.User.Identity.Name));
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Form successfully submitted.");
			TempDataDictionary tempData = base.TempData;
			guidId = model.GuidId;
			tempData["AssetUrl"] = string.Concat("/DataPortal/ViewAsset/", guidId.ToString());
			if (SubmitPaper != "Submit Form")
			{
				return base.RedirectToAction("EpiFundBuysPaper");
			}
			return base.View("EpiFundBuysCommercialPaper", model);
		}

		[HttpGet]
		public ActionResult EpiFundBuysRealEstate(string propertyType)
		{
			string str = propertyType;
			((dynamic)base.ViewBag).PortfolioList = this.populatePortfolioListDDL();
			IEnumerable<PropertyType> propertyTypes = Enum.GetValues(typeof(PropertyType)).Cast<PropertyType>();
			if (string.IsNullOrWhiteSpace(str))
			{
				str = PropertyType.Commercial.ToString();
			}
			PropertyType propertyType1 = propertyTypes.FirstOrDefault<PropertyType>((PropertyType pt) => pt.ToString() == str);
			IEnumerable<SelectListItem> selectListItem = 
				from property in propertyTypes
				select new SelectListItem()
				{
					Text = property.ToString(),
					Value = property.ToString(),
					Selected = property == propertyType1
				};
			((dynamic)base.ViewBag).PropertyTypes = selectListItem;
			if (propertyType1 != PropertyType.Residential)
			{
				RealEstateCommercialAssetViewModel realEstateCommercialAsset = this.GetRealEstateCommercialAsset();
				realEstateCommercialAsset.PagePropertyTypes = selectListItem;
				realEstateCommercialAsset.GuidId = Guid.NewGuid();
				this.BuildMFViewBags(this.populateMFDetailsCheckBoxList(realEstateCommercialAsset.MFDetails));
				this.BuildMHPViewBags(this.populateMHPDetailsCheckBoxList(realEstateCommercialAsset.MHPDetails));
				this.BuildCommViewBags(this.populateCommDetailsCheckBoxList(realEstateCommercialAsset.PropertyDetails, AssetType.Office));
				realEstateCommercialAsset.DeferredMaintenanceItems = this._asset.GetDefaultDeferredMaintenanceItems();
				realEstateCommercialAsset.DeferredMaintenanceItems = this._asset.RemoveMaintainanceItems(AssetType.Office, realEstateCommercialAsset.DeferredMaintenanceItems);
				realEstateCommercialAsset.DeferredMaintenanceItems = this._asset.OrderMaintainanceItems(AssetType.Office, realEstateCommercialAsset.DeferredMaintenanceItems);
				realEstateCommercialAsset.DeferredMaintenanceItemsMF = this._asset.GetDefaultDeferredMaintenanceItems();
				realEstateCommercialAsset.DeferredMaintenanceItemsMF = this._asset.RemoveMaintainanceItems(AssetType.MultiFamily, realEstateCommercialAsset.DeferredMaintenanceItemsMF);
				realEstateCommercialAsset.DeferredMaintenanceItemsMF = this._asset.OrderMaintainanceItems(AssetType.MultiFamily, realEstateCommercialAsset.DeferredMaintenanceItemsMF);
				return base.View("EpiFundBuysCommercialRealEstate", realEstateCommercialAsset);
			}
			RealEstateResidentialAssetViewModel realEstateResidentialAssetViewModel = new RealEstateResidentialAssetViewModel()
			{
				PagePropertyTypes = selectListItem,
				GuidId = Guid.NewGuid(),
				DeferredMaintenanceItems = this._asset.GetDefaultDeferredMaintenanceItems()
			};
			realEstateResidentialAssetViewModel.DeferredMaintenanceItems = this._asset.RemoveMaintainanceItems(AssetType.MultiFamily, realEstateResidentialAssetViewModel.DeferredMaintenanceItems);
			realEstateResidentialAssetViewModel.DeferredMaintenanceItems = this._asset.OrderMaintainanceItems(AssetType.MultiFamily, realEstateResidentialAssetViewModel.DeferredMaintenanceItems);
			if (base.User.Identity.IsAuthenticated)
			{
				UserModel userByUsername = this._userManager.GetUserByUsername(base.User.Identity.Name);
				realEstateResidentialAssetViewModel.WorkPhone = userByUsername.WorkNumber;
				realEstateResidentialAssetViewModel.Fax = userByUsername.FaxNumber;
				realEstateResidentialAssetViewModel.EmailAddress = userByUsername.Username;
				realEstateResidentialAssetViewModel.CorporateOwnershipAddress = string.Concat(new string[] { userByUsername.AddressLine1, " ", userByUsername.AddressLine2, " ", userByUsername.City, "  ", userByUsername.StateOfOriginCorporateEntity, " ", userByUsername.Zip });
			}
			return base.View("EpiFundBuysResidentialRealEstate", realEstateResidentialAssetViewModel);
		}

		[HttpPost]
		public ActionResult EpiFundBuysRealEstate(RealEstateCommercialAssetViewModel model, string SubmitREC)
		{
			((dynamic)base.ViewBag).PortfolioList = this.populatePortfolioListDDL();
			UserModel userModel = new UserModel();
			if (base.User.Identity.IsAuthenticated)
			{
				userModel = this._userManager.GetUserByUsername(base.User.Identity.Name);
			}
			if (!base.ModelState.IsValid)
			{
				foreach (ModelError list in base.ModelState.Values.SelectMany<System.Web.Mvc.ModelState, ModelError>((System.Web.Mvc.ModelState v) => v.Errors).ToList<ModelError>())
				{
					if (list.ErrorMessage.ToLower().Contains("asset document type"))
					{
						base.ModelState.Remove("AssetDocumentType");
						base.ModelState.Add("AssetDocumentType", new System.Web.Mvc.ModelState());
						base.ModelState.SetModelValue("AssetDocumentType", new ValueProviderResult((object)AssetDocumentType.CurrentRentRoll, "CurrentRentRoll", null));
					}
					if (!list.ErrorMessage.Contains("Property Updated Year"))
					{
						continue;
					}
					base.ModelState.Remove("PropLastUpdated");
					base.ModelState.Add("PropLastUpdated", new System.Web.Mvc.ModelState());
					base.ModelState.SetModelValue("PropLastUpdated", new ValueProviderResult((object)0, "0", null));
					model.isPropUpdateUnknown = true;
				}
			}
			this.BuildMFViewBags(this.populateMFDetailsCheckBoxList(model.MFDetails));
			this.BuildMHPViewBags(this.populateMHPDetailsCheckBoxList(model.MHPDetails));
			this.BuildCommViewBags(this.populateCommDetailsCheckBoxList(model.PropertyDetails, AssetType.Office));
			if (model.Images == null)
			{
				model.Images = new List<AssetImageModel>();
			}
			if (model.Documents == null)
			{
				model.Documents = new List<TempAssetDocument>();
			}
			if (model.Videos == null)
			{
				model.Videos = new List<TempAssetVideo>();
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
					if (model.Videos[i].FilePath != null)
					{
						continue;
					}
					model.Videos.RemoveAt(i);
					flag = true;
				}
				while (flag);
			}
			int num = 0;
			List<AssetImageModel> fileName = (
				from s in model.Images
				orderby s.Order
				select s).ToList<AssetImageModel>();
			for (int j = 0; j < fileName.Count; j++)
			{
				bool flag1 = false;
				do
				{
					if (j >= fileName.Count)
					{
						break;
					}
					if (fileName[j].AssetImageId == Guid.Empty && base.ModelState.IsValid)
					{
						fileName[j].OriginalFileName = fileName[j].FileName;
						fileName[j].AssetImageId = this.generatedGuidIfNone(fileName[j].AssetImageId);
						IFileManager fileManager = this._fileManager;
						string str = fileName[j].FileName;
						Guid guidId = model.GuidId;
						string dateForTempImages = model.DateForTempImages;
						int userId = userModel.UserId;
						string str1 = fileManager.MoveTempAssetImage(str, guidId, dateForTempImages, userId.ToString());
						if (!string.IsNullOrEmpty(str1))
						{
							fileName[j].FileName = str1;
						}
					}
					if (fileName[j].FileName != null)
					{
						fileName[j].AssetId = model.GuidId;
						fileName[j].Order = num;
						num++;
						flag1 = false;
					}
					else
					{
						try
						{
							fileName.RemoveAt(j);
							flag1 = true;
						}
						catch
						{
						}
					}
				}
				while (flag1);
			}
			model.Images = fileName;
			num = 0;
			for (int k = 0; k < model.Documents.Count; k++)
			{
				bool flag2 = false;
				do
				{
					if (k >= model.Documents.Count)
					{
						break;
					}
					if (model.Documents[k].FileName != null)
					{
						model.Documents[k].Order = num;
						num++;
						flag2 = false;
					}
					else
					{
						model.Documents.RemoveAt(k);
						flag2 = true;
					}
				}
				while (flag2);
			}
			if (!string.IsNullOrEmpty(model.TypeOfProperty))
			{
				if (!(model.TypeOfProperty != AssetType.MultiFamily.ToString()) || !(model.TypeOfProperty != AssetType.MHP.ToString()))
				{
					if ((int)model.ElectricMeterMethod == 0)
					{
						base.ModelState.AddModelError("ElectricMeterMethod", "Electric Meter Method is required");
					}
					if ((int)model.GasMeterMethod == 0)
					{
						base.ModelState.AddModelError("GasMeterMethod", "Gas Meter Method is required");
					}
				}
				else
				{
					if ((int)model.Type == 0)
					{
						base.ModelState.AddModelError("Type", "Type of Commercial Asset is required");
					}
					if ((int)model.VacantSuites == 0)
					{
						base.ModelState.AddModelError("VacantSuites", "Vacant Suites is required");
					}
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
			IAssetManager assetManager = this._asset;
			Guid guid = new Guid();
			if (assetManager.GetMatchingAssetsByAPNCountyState(model.TaxAssessorNumber, "", "").Any())
			{
				base.ModelState.AddModelError("TaxAssessorNumber", "Tax Assessor Number is already in use for another asset");
			}
			if (model.TaxAssessorNumberOther != null)
			{
				IAssetManager assetManager1 = this._asset;
				guid = new Guid();
				if (assetManager1.GetMatchingAssetsByAPNCountyState(model.TaxAssessorNumberOther, "", "").Any())
				{
					base.ModelState.AddModelError("TaxAssessorNumberOther", "Other Tax Assessor Number is already in use for another asset");
				}
			}
			if (!base.ModelState.IsValid)
			{
				base.SetUpJsonImages(null, model, new int?(userModel.UserId));
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Please fill in all the reqd. fields before submitting");
				return base.View("EpiFundBuysCommercialRealEstate", model);
			}
			if (base.User.Identity.IsAuthenticated)
			{
				model.Username = base.User.Identity.Name.ToLower();
			}
			int num1 = this._asset.SaveRealEstateCommercial(model);
			AssetDescriptionModel assetByAssetNumber = this._asset.GetAssetByAssetNumber(num1);
			if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
			{
				List<UserQuickViewModel> users = this._userManager.GetUsers(new UserSearchModel()
				{
					UserTypeFilter = new UserType?(UserType.CorpAdmin2),
					ShowActiveOnly = true
				});
				users.AddRange(this._userManager.GetUsers(new UserSearchModel()
				{
					UserTypeFilter = new UserType?(UserType.CorpAdmin),
					ShowActiveOnly = true
				}));
				foreach (UserQuickViewModel user in users)
				{
					IEPIFundEmailService ePIFundEmailService = this._email;
					NotificationForCorpAdminPaperOrRealEstateCreated notificationForCorpAdminPaperOrRealEstateCreated = new NotificationForCorpAdminPaperOrRealEstateCreated()
					{
						To = user.Username
					};
					string leftPart = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
					guid = model.GuidId;
					notificationForCorpAdminPaperOrRealEstateCreated.Link = string.Format("{0}/DataPortal/ViewAsset/{1}", leftPart, guid.ToString());
					notificationForCorpAdminPaperOrRealEstateCreated.NoteType = "Real Estate";
					ePIFundEmailService.Send(notificationForCorpAdminPaperOrRealEstateCreated);
				}
				string taxAssessorNumber = model.TaxAssessorNumber;
				if (model.TaxAssessorNumberOther != null)
				{
					taxAssessorNumber = string.Concat(model.TaxAssessorNumber, "; ", model.TaxAssessorNumberOther);
				}
				string empty = string.Empty;
				if (model.CorporateOwnershipOfficer != null)
				{
					empty = model.CorporateOwnershipOfficer;
				}
				this._email.Send(new ConfirmationCRSellerFormSubmittalEmail()
				{
					Email = model.EmailAddress,
					Name = model.NameOfPrincipal,
					APN = taxAssessorNumber,
					CorpOfficer = empty,
					VestingEntity = model.CorporateName,
					AssetDescription = assetByAssetNumber,
					AssetType = model.TypeOfProperty
				});
			}
			if (base.User.Identity.IsAuthenticated)
			{
				this.EmailPIFromFeeSimple(this._userManager.GetUserByUsername(base.User.Identity.Name));
			}
			TempDataDictionary tempData = base.TempData;
			guid = model.GuidId;
			tempData["AssetUrl"] = string.Concat("/DataPortal/ViewAsset/", guid.ToString());
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Form successfully submitted.");
			if (SubmitREC != "Submit Form")
			{
				return base.RedirectToAction("EpiFundBuysRealEstate");
			}
			return base.View("EpiFundBuysCommercialRealEstate", model);
		}

		public ViewResult Error()
		{
			return base.View();
		}

		[AllowAnonymous]
		[HttpGet]
		public ViewResult ForgotPassword()
		{
			return base.View(new ResetPasswordModel());
		}

		[AllowAnonymous]
		[HttpPost]
		public ViewResult ForgotPassword(ResetPasswordModel model)
		{
			ViewResult viewResult;
			if (this._userManager.IsUserDisabled(model.Username))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "This username does not exist or your account is disabled. Inactive users cannot reset passwords.");
				return base.View();
			}
			string str = this._userManager.ResetPassword(model.Username);
			if (str == null)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Invalid username.");
				return base.View();
			}
			try
			{
				ResetPasswordEmail resetPasswordEmail = new ResetPasswordEmail()
					{
						Email = model.Username,
						To = model.Username,
						Password = str
					};
			    this._email.Send(resetPasswordEmail);
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Your password has been reset successfully. Please check your email for further instructions.");
				viewResult = base.View();
			}
			catch (Exception exception)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "There was a problem sending your reset password email. The problem has been logged and our support team has been notified. We are sorry for the inconvenience.");
				return base.View();
			}
			return viewResult;
		}

		private new Guid generatedGuidIfNone(Guid guid)
		{
			if (guid != Guid.Parse("00000000-0000-0000-0000-000000000000"))
			{
				return guid;
			}
			return Guid.NewGuid();
		}

		private PaperCommercialAssetViewModel GetPaperCommercialAsset()
		{
			string[] strArrays;
			int i;
			PaperCommercialAssetViewModel paperCommercialAssetViewModel = new PaperCommercialAssetViewModel();
			if (base.User.Identity.IsAuthenticated)
			{
				UserModel userByUsername = this._userManager.GetUserByUsername(base.User.Identity.Name);
				paperCommercialAssetViewModel.NameOfPrincipal = userByUsername.FirstName;
				paperCommercialAssetViewModel.NameOfCoPrincipal = userByUsername.LastName;
				if (paperCommercialAssetViewModel.CorporateOwnershipOfficer == null || paperCommercialAssetViewModel.CorporateOwnershipOfficer.Length < 1)
				{
					paperCommercialAssetViewModel.CorporateOwnershipOfficer = string.Concat(userByUsername.FirstName, " ", userByUsername.LastName);
				}
				paperCommercialAssetViewModel.CorporateName = userByUsername.CompanyName;
				paperCommercialAssetViewModel.AcronymForCorporateEntity = userByUsername.AcroynmForCorporateEntity;
				paperCommercialAssetViewModel.CorporateAddress1 = userByUsername.AddressLine1;
				paperCommercialAssetViewModel.CorporateAddress2 = userByUsername.AddressLine2;
				paperCommercialAssetViewModel.City = userByUsername.City;
				paperCommercialAssetViewModel.Zip = userByUsername.Zip;
				paperCommercialAssetViewModel.WorkPhone = userByUsername.WorkNumber;
				paperCommercialAssetViewModel.Fax = userByUsername.FaxNumber;
				paperCommercialAssetViewModel.EmailAddress = userByUsername.Username;
				paperCommercialAssetViewModel.SelectedCorporateEntityType = userByUsername.SelectedCorporateEntityType;
				paperCommercialAssetViewModel.CorporateTitle = userByUsername.CorporateTitle;
				paperCommercialAssetViewModel.StateOfOriginCorporateEntity = userByUsername.StateOfOriginCorporateEntity;
				paperCommercialAssetViewModel.CellPhone = userByUsername.CellNumber;
				paperCommercialAssetViewModel.Fax = userByUsername.FaxNumber;
				paperCommercialAssetViewModel.CorporateOwnershipAddress = userByUsername.AddressLine1;
				paperCommercialAssetViewModel.CorporateOwnershipAddress2 = userByUsername.AddressLine2;
				paperCommercialAssetViewModel.CorporateOwnershipCity = userByUsername.City;
				paperCommercialAssetViewModel.CorporateOwnershipState = userByUsername.StateOfOriginCorporateEntity;
				paperCommercialAssetViewModel.CorporateOwnershipZip = userByUsername.Zip;
				paperCommercialAssetViewModel.SelectedPreferredMethods = new List<PreferredMethod>();
				if (userByUsername.PreferredMethods != null && userByUsername.SelectedPreferredMethodsString != null)
				{
					strArrays = userByUsername.SelectedPreferredMethodsString.Split(new char[] { ';' });
					for (i = 0; i < (int)strArrays.Length; i++)
					{
						string str = strArrays[i];
						if (!string.IsNullOrEmpty(str))
						{
							paperCommercialAssetViewModel.SelectedPreferredMethods.Add((PreferredMethod)Enum.Parse(typeof(PreferredMethod), str));
						}
					}
				}
				paperCommercialAssetViewModel.SelectedPreferredContactTime = new List<PreferredContactTime>();
				if (userByUsername.PreferredContactTimes != null && userByUsername.SelectedPreferredContactTimesString != null)
				{
					strArrays = userByUsername.SelectedPreferredContactTimesString.Split(new char[] { ';' });
					for (i = 0; i < (int)strArrays.Length; i++)
					{
						string str1 = strArrays[i];
						if (!string.IsNullOrEmpty(str1))
						{
							paperCommercialAssetViewModel.SelectedPreferredContactTime.Add((PreferredContactTime)Enum.Parse(typeof(PreferredContactTime), str1));
						}
					}
				}
				paperCommercialAssetViewModel.UserId = userByUsername.UserId;
			}
			return paperCommercialAssetViewModel;
		}

		private RealEstateCommercialAssetViewModel GetRealEstateCommercialAsset()
		{
			string[] strArrays;
			int i;
			RealEstateCommercialAssetViewModel realEstateCommercialAssetViewModel = new RealEstateCommercialAssetViewModel();
			if (base.User.Identity.IsAuthenticated)
			{
				UserModel userByUsername = this._userManager.GetUserByUsername(base.User.Identity.Name);
				realEstateCommercialAssetViewModel.NameOfPrincipal = userByUsername.FirstName;
				realEstateCommercialAssetViewModel.NameOfCoPrincipal = userByUsername.LastName;
				if (realEstateCommercialAssetViewModel.CorporateOwnershipOfficer == null || realEstateCommercialAssetViewModel.CorporateOwnershipOfficer.Length < 1)
				{
					realEstateCommercialAssetViewModel.CorporateOwnershipOfficer = string.Concat(userByUsername.FirstName, " ", userByUsername.LastName);
				}
				realEstateCommercialAssetViewModel.CorporateName = userByUsername.CompanyName;
				realEstateCommercialAssetViewModel.AcronymForCorporateEntity = userByUsername.AcroynmForCorporateEntity;
				realEstateCommercialAssetViewModel.CorporateAddress1 = userByUsername.AddressLine1;
				realEstateCommercialAssetViewModel.CorporateAddress2 = userByUsername.AddressLine2;
				realEstateCommercialAssetViewModel.City = userByUsername.City;
				realEstateCommercialAssetViewModel.Zip = userByUsername.Zip;
				realEstateCommercialAssetViewModel.WorkPhone = userByUsername.WorkNumber;
				realEstateCommercialAssetViewModel.Fax = userByUsername.FaxNumber;
				realEstateCommercialAssetViewModel.EmailAddress = userByUsername.Username;
				realEstateCommercialAssetViewModel.CorporateOwnershipAddress = userByUsername.AddressLine1;
				realEstateCommercialAssetViewModel.CorporateOwnershipAddress2 = userByUsername.AddressLine2;
				realEstateCommercialAssetViewModel.CorporateOwnershipCity = userByUsername.City;
				realEstateCommercialAssetViewModel.CorporateOwnershipState = userByUsername.StateOfOriginCorporateEntity;
				realEstateCommercialAssetViewModel.CorporateOwnershipZip = userByUsername.Zip;
				realEstateCommercialAssetViewModel.SelectedCorporateEntityType = userByUsername.SelectedCorporateEntityType;
				realEstateCommercialAssetViewModel.CorporateTitle = userByUsername.CorporateTitle;
				realEstateCommercialAssetViewModel.StateOfOriginCorporateEntity = userByUsername.StateOfOriginCorporateEntity;
				realEstateCommercialAssetViewModel.CellPhone = userByUsername.CellNumber;
				realEstateCommercialAssetViewModel.Fax = userByUsername.FaxNumber;
				realEstateCommercialAssetViewModel.SelectedPreferredMethods = new List<PreferredMethod>();
				if (userByUsername.PreferredMethods != null && userByUsername.SelectedPreferredMethodsString != null)
				{
					strArrays = userByUsername.SelectedPreferredMethodsString.Split(new char[] { ';' });
					for (i = 0; i < (int)strArrays.Length; i++)
					{
						string str = strArrays[i];
						if (!string.IsNullOrEmpty(str))
						{
							realEstateCommercialAssetViewModel.SelectedPreferredMethods.Add((PreferredMethod)Enum.Parse(typeof(PreferredMethod), str));
						}
					}
				}
				realEstateCommercialAssetViewModel.SelectedPreferredContactTime = new List<PreferredContactTime>();
				if (userByUsername.PreferredContactTimes != null && userByUsername.SelectedPreferredContactTimesString != null)
				{
					strArrays = userByUsername.SelectedPreferredContactTimesString.Split(new char[] { ';' });
					for (i = 0; i < (int)strArrays.Length; i++)
					{
						string str1 = strArrays[i];
						if (!string.IsNullOrEmpty(str1))
						{
							realEstateCommercialAssetViewModel.SelectedPreferredContactTime.Add((PreferredContactTime)Enum.Parse(typeof(PreferredContactTime), str1));
						}
					}
				}
				realEstateCommercialAssetViewModel.UserId = userByUsername.UserId;
			}
			return realEstateCommercialAssetViewModel;
		}

		public ActionResult GetVideo(string id, string filepath)
		{
			string str = Path.Combine(ConfigurationManager.AppSettings["DataRoot"], "Videos", id.ToString(), filepath);
			if (!System.IO.File.Exists(str))
			{
				return null;
			}
			return base.File(str, "video/mp4");
		}

		[HttpPost]
		public FileUploadJsonResult ImageDelete(string imgId, Guid guidId)
		{
			if (imgId != null && this._fileManager.DeleteFile(imgId, guidId, FileType.Image))
			{
				return new FileUploadJsonResult()
				{
					Data = new { message = "true" }
				};
			}
			return new FileUploadJsonResult()
			{
				Data = new { message = "Could Not Find File" }
			};
		}

		[HttpPost]
		public FileUploadJsonResult ImageUpload(HttpPostedFileBase file, Guid guidId)
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
			string str = this._fileManager.SaveFile(file, guidId, FileType.Image);
			try
			{
				this._fileManager.CreateThumbnail(str, guidId);
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
				Data = new { message = "true", filename = str, contentType = file.ContentType }
			};
		}

		public ActionResult Index(string ReturnUrl = null)
		{
            if (base.HttpContext.Request.Url.Host.ToLower() == "localhost")
			{
				base.Session["RootAuth"] = true;
			}
			if (!string.IsNullOrEmpty(ReturnUrl))
			{
				base.TempData["rurl"] = ReturnUrl;
			}
			return base.View();
		}

		[HttpGet]
		public ActionResult InvestorOpportunities()
		{
			return base.View();
		}

		[HttpPost]
		public JsonResult IsAuthenticated()
		{
			if (!base.HasValidCookie(new UserDataModel()
			{
				Username = base.User.Identity.Name,
				IPAddress = base.GetIPAddress(),
				MachineName = base.DetermineCompName()
			}))
			{
				return new JsonResult()
				{
					Data = new { success = "false" }
				};
			}
			return new JsonResult()
			{
				Data = new { success = "true" }
			};
		}

		[HttpPost]
		public bool IsLoggedIn()
		{
			return base.User.Identity.IsAuthenticated;
		}

		[HttpGet]
		public ViewResult JointVentureMarketing()
		{
			return base.View();
		}

		[HttpGet]
		public ActionResult Login(int? searchCriteriaFormId = null, string rurl = null)
		{
			string item = ConfigurationManager.AppSettings["SystemAdminUser"];
			string str = ConfigurationManager.AppSettings["SystemAdminPassword"];
			string item1 = ConfigurationManager.AppSettings["ReliantAdminUser"];
			string str1 = ConfigurationManager.AppSettings["ReliantAdminPassword"];
			if (!this._userManager.UserExists(item))
			{
				this._userManager.CreateUser(item, this._userManager.HashPassword(str), UserType.SiteAdmin);
			}
			if (!this._userManager.UserExists(item1))
			{
				this._userManager.CreateUser(item1, this._userManager.HashPassword(str1), UserType.SiteAdmin);
			}
			if (base.User.Identity != null && this._userManager.UserExists(base.User.Identity.Name))
			{
				return base.RedirectToAction("MyUSCPage", "Home");
			}
			LoginModel loginModel = new LoginModel();
			if (base.TempData["rurl"] != null)
			{
				loginModel.ReturnUrl = base.TempData["rurl"].ToString();
			}
			if (rurl != null)
			{
				loginModel.ReturnUrl = rurl;
			}
			if (searchCriteriaFormId.HasValue)
			{
				loginModel.AssetSearchCriteriaId = new int?(searchCriteriaFormId.Value);
			}
			return base.View(loginModel);
		}

		[HttpPost]
		public ActionResult Login(LoginModel model)
		{
			int? assetSearchCriteriaId;
			if (model == null || model.Password == null || model.Username == null)
			{
				model.Password = string.Empty;
				return base.View(model);
			}
			if (!this._auth.Authenticate(model.Username, model.Password))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Incorrect username or password");
				model.Password = string.Empty;
				return base.View(model);
			}
			UserModel userByUsername = this._userManager.GetUserByUsername(model.Username);
			if (!userByUsername.IsActive)
			{
				this._auth.Logout();
				base.TempData["UserDeactivated"] = "true";
				return base.View();
			}
			base.WashTempData(model.Username);
			ICStatus? cStatus = userByUsername.ICStatus;
			if ((cStatus.GetValueOrDefault() == ICStatus.Rejected ? cStatus.HasValue : false))
			{
				return base.RedirectToAction("MyUSCPage");
			}
			if (userByUsername.UserType == UserType.ICAdmin)
			{
				cStatus = userByUsername.ICStatus;
				if (cStatus.HasValue)
				{
					if (!userByUsername.SignedICAgreement)
					{
						return base.RedirectToAction("MyUSCPage");
					}
					cStatus = userByUsername.ICStatus;
					if (cStatus.Value != ICStatus.Approved)
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Your IC Admin application has not been approved. You cannot enter this site unless you have been approved by USC.");
						return base.View();
					}
				}
			}
			if (model.AssetSearchCriteriaId.HasValue)
			{
				IAssetManager assetManager = this._asset;
				int userId = userByUsername.UserId;
				assetSearchCriteriaId = model.AssetSearchCriteriaId;
				assetManager.AssociateUserToSearchCriteriaForm(userId, assetSearchCriteriaId.Value);
			}
			if (userByUsername.UserType != UserType.TitleCompManager && userByUsername.UserType != UserType.TitleCompUser && model.AssetNumber.HasValue)
			{
				IAssetManager assetManager1 = this._asset;
				assetSearchCriteriaId = model.AssetNumber;
				AssetDescriptionModel assetByAssetNumber = assetManager1.GetAssetByAssetNumber(assetSearchCriteriaId.Value);
				if (assetByAssetNumber == null)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You have entered an Asset ID # that is not in the USC database. Please enter an alternative Asset ID # or, please continue your Asset Search through the normal protocols of the website.  Thank you.");
					return base.RedirectToAction("MyUSCPage", "Home");
				}
				if (assetByAssetNumber.AssetId == Guid.Empty)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You have entered an Asset ID # that is not in the USC database. Please enter an alternative Asset ID # or, please continue your Asset Search through the normal protocols of the website.  Thank you.");
					return base.RedirectToAction("MyUSCPage", "Home");
				}
				if (userByUsername.UserType == UserType.CorpAdmin2 || userByUsername.UserType == UserType.CorpAdmin || userByUsername.UserType == UserType.SiteAdmin)
				{
					return base.RedirectToAction("ViewAsset", "DataPortal", new { id = assetByAssetNumber.AssetId, fromManageAssets = true });
				}
				if (!userByUsername.SignedNCND)
				{
					return base.RedirectToAction("SignDocsInformation", "Home");
				}
				if (!assetByAssetNumber.Show)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You have entered an Asset ID # that is not in the USC database.  Please enter an alternative Asset ID # or, please continue your Asset Search through the normal protocols of the website.  Thank you.");
					return base.View();
				}
				if (!string.IsNullOrEmpty(model.ReturnUrl))
				{
					return this.Redirect(model.ReturnUrl);
				}
				if (userByUsername.UserType != UserType.CorpAdmin2 && userByUsername.UserType != UserType.CorpAdmin && userByUsername.UserType != UserType.SiteAdmin)
				{
					return base.RedirectToAction("ViewAsset", "DataPortal", new { id = assetByAssetNumber.AssetId });
				}
				return base.RedirectToAction("ViewAsset", "DataPortal", new { id = assetByAssetNumber.AssetId, fromManageAssets = true });
			}
			if (userByUsername.UserType == UserType.TitleCompManager || userByUsername.UserType == UserType.TitleCompUser)
			{
				TitleCompanyUserModel titleUserByEmail = this._userManager.GetTitleUserByEmail(userByUsername.Username);
				if (titleUserByEmail == null)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Incorrect title company username or password");
					return base.View();
				}
				if (!titleUserByEmail.IsActive)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Your login has not been validated.");
					this._auth.Logout();
					return base.View();
				}
				if (!model.AssetNumber.HasValue)
				{
					return base.RedirectToAction("MyUSCPage", "Home");
				}
				IAssetManager assetManager2 = this._asset;
				assetSearchCriteriaId = model.AssetNumber;
				AssetDescriptionModel assetDescriptionModel = assetManager2.GetAssetByAssetNumber(assetSearchCriteriaId.Value);
				if (assetDescriptionModel.AssetId != Guid.Empty)
				{
					return base.RedirectToAction("CompleteAsset", "Admin", new { id = assetDescriptionModel.AssetId, fromManageAssets = false });
				}
			}
			else if (userByUsername.UserType == UserType.PCInsuranceManager)
			{
				InsuranceCompanyUserViewModel insuranceUserByEmail = this._userManager.GetInsuranceUserByEmail(userByUsername.Username);
				if (insuranceUserByEmail != null && insuranceUserByEmail.IsActive)
				{
					if (!model.AssetNumber.HasValue)
					{
						return base.RedirectToAction("MyUSCPage", "Home");
					}
					IAssetManager assetManager3 = this._asset;
					assetSearchCriteriaId = model.AssetNumber;
					AssetDescriptionModel assetByAssetNumber1 = assetManager3.GetAssetByAssetNumber(assetSearchCriteriaId.Value);
					if (assetByAssetNumber1.AssetId != Guid.Empty)
					{
						return base.RedirectToAction("CompleteAsset", "Insurance", new { id = assetByAssetNumber1.AssetId, fromManageAssets = false });
					}
				}
			}
			if (!userByUsername.SignedNCND)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "We have not received your signed NCND Agreement. Please execute the Agreement below.");
				return base.RedirectToAction("ExecuteNCND", "DataPortal");
			}
			if (model.AssetIds.Count > 0)
			{
				TempDataDictionary tempData = base.TempData;
				int assetIds = userByUsername.UserId;
				tempData[string.Concat(assetIds.ToString(), "Assets")] = model.AssetIds;
				return base.RedirectToAction("ExecuteMDAs", "DataPortal");
			}
			if (!string.IsNullOrEmpty(model.ReturnUrl) && !model.ReturnUrl.Contains("MyUSCPage"))
			{
				return this.Redirect(model.ReturnUrl);
			}
			return base.RedirectToAction("MyUSCPage", "Home");
		}

		[HttpPost]
		public ActionResult LoginAlt(IEnumerable<AssetQuickListViewModel> items)
		{
			LoginModel loginModel = new LoginModel();
			List<Guid> guids = new List<Guid>();
			items.ToList<AssetQuickListViewModel>().ForEach((AssetQuickListViewModel item) => {
				if (item.IsSelected)
				{
					guids.Add(item.AssetId);
				}
			});
			loginModel.AssetIds = guids;
			return base.View("Login", loginModel);
		}

		public RedirectToRouteResult Logout()
		{
			base.WashTempData(null);
			this._auth.Logout();
			return base.RedirectToAction("Login");
		}

		public ActionResult MyUSCPage()
		{
			ICStatus? cStatus;
			((dynamic)base.ViewBag).AssetType = base.populateAssetTypeDDL();
			((dynamic)base.ViewBag).PortfolioList = this.populatePortfolioListDDL();
			UserModel userByUsername = this._userManager.GetUserByUsername(base.User.Identity.Name);
			if (userByUsername == null)
			{
				return base.View("Login");
			}
			this._docs.ProcessSignedDocuments(new int?(userByUsername.UserId));
			if (!userByUsername.SignedICAgreement && userByUsername.UserType == UserType.ICAdmin)
			{
				cStatus = userByUsername.ICStatus;
				if ((cStatus.GetValueOrDefault() == ICStatus.Rejected ? !cStatus.HasValue : true))
				{
					if (!this._userManager.HasPendingICAgreement(userByUsername.UserId))
					{
						return base.View("ICAdminFillOutAgreement");
					}
					userByUsername = this._userManager.GetUserByUsername(base.User.Identity.Name);
					if (!userByUsername.SignedICAgreement && userByUsername.UserType == UserType.ICAdmin)
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "We have not received your signed IC Agreement. Please check your email your execute another IC Agreement below.");
						return base.RedirectToAction("ExecuteICAgreement", "DataPortal");
					}
				}
			}
			List<AssetSearchCriteriaQuickViewModel> searchesForUser = this._userManager.GetSearchesForUser(userByUsername.UserId);
			List<UserFileModel> filesByUserId = this._userManager.GetFilesByUserId(userByUsername.UserId);
			PersonalFinancialStatementTemplateModel personalFinancialStatementByUserId = this._userManager.GetPersonalFinancialStatementByUserId(userByUsername.UserId);
			int? nullable = null;
			bool flag = this._userManager.HasOrderHistory(userByUsername.UserId);
			MyUSCViewModel myUSCViewModel = new MyUSCViewModel()
			{
				FullName = userByUsername.FullName,
				UserId = userByUsername.UserId,
				UserType = userByUsername.UserType,
				Searches = searchesForUser,
				PersonalFinancialStatementId = (personalFinancialStatementByUserId != null ? new int?(personalFinancialStatementByUserId.PersonalFinancialStatementId) : nullable),
				Files = filesByUserId,
				JVMarketerAgreementLocation = userByUsername.JVMarketerAgreementLocation,
				HasOrderHistory = flag,
				IsASeller = userByUsername.IsASeller,
				AssetMDAs = this._asset.GetSignedMDAs(userByUsername.UserId).Where<SignedMDAQuickViewModel>((SignedMDAQuickViewModel w) => {
					if (w.CurrentListingStatus == ListingStatus.SoldAndClosed)
					{
						return false;
					}
					return w.CurrentListingStatus != ListingStatus.SoldNotClosed;
				}).OrderByDescending<SignedMDAQuickViewModel, DateTime>((SignedMDAQuickViewModel w) => w.DateSigned).ToList<SignedMDAQuickViewModel>(),
				UnreadLOICount = this._seller.GetUnreadLOICount(userByUsername.UserId)
			};
			cStatus = userByUsername.ICStatus;
			if ((cStatus.GetValueOrDefault() == ICStatus.Rejected ? cStatus.HasValue : false))
			{
				base.TempData["rejected"] = "true";
				return base.View(myUSCViewModel);
			}
			if (userByUsername.UserType == UserType.CorpAdmin || userByUsername.UserType == UserType.ICAdmin || userByUsername.UserType == UserType.SiteAdmin)
			{
				return base.View("../Admin/MyUSCPageAdmin", myUSCViewModel);
			}
			if (userByUsername.UserType != UserType.TitleCompManager && userByUsername.UserType != UserType.TitleCompUser)
			{
				if (userByUsername.UserType != UserType.PCInsuranceManager)
				{
					return base.View(myUSCViewModel);
				}
				return base.RedirectToAction("MyUSCPage", "Insurance");
			}
			TitleCompanyUserModel titleUserByEmail = this._userManager.GetTitleUserByEmail(userByUsername.Username);
			string titleCompName = this._userManager.GetTitleById(titleUserByEmail.TitleCompanyId).TitleCompName;
			myUSCViewModel.TitleCompanyName = titleCompName;
			((dynamic)base.ViewBag).Email = this._userManager.GetTitleUserById(titleUserByEmail.TitleCompanyUserId).Email;
			return base.View("../Admin/MyUSCPageTitle", myUSCViewModel);
		}

		[HttpGet]
		public ActionResult NewMachine(string rurl = null)
		{
			UserMachineModel userMachineModel = new UserMachineModel();
			if (rurl != null)
			{
				userMachineModel.ReturnUrl = rurl;
			}
			if (base.TempData["TempUserEmail"] != null)
			{
				userMachineModel.Username = base.TempData["TempUserEmail"].ToString();
			}
			if (base.TempData["TempAssetNumber"] != null)
			{
				userMachineModel.AssetNumber = new int?((int)base.TempData["TempAssetNumber"]);
			}
			return base.View(userMachineModel);
		}

		[HttpPost]
		[MultipleButton(Name="action", Argument="NewMachine")]
		public ActionResult NewMachine(UserMachineModel model)
		{
			if (!base.ModelState.IsValid)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Please enter both username and password.");
			}
			else if (!this._auth.Authenticate(model.Username, model.Password))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Incorrect username and/or password.");
			}
			else
			{
				UserModel userByUsername = this._userManager.GetUserByUsername(model.Username);
				if (base.VerifyCode(this._securityManager, model.Code, userByUsername))
				{
					base.AuthenticateMachine(model.Username);
					base.CreateCookie(true, new UserDataModel()
					{
						Username = userByUsername.Username,
						IPAddress = base.GetIPAddress(),
						MachineName = base.DetermineCompName(),
						SecurityCode = model.Code
					});
					if (userByUsername.UserType == UserType.ICAdmin && userByUsername.ICStatus.HasValue)
					{
						if (!userByUsername.SignedICAgreement)
						{
							base.TempData["NewICAdmin"] = "true";
							base.RedirectToAction("NewMachine", "Home");
						}
						if (userByUsername.ICStatus.Value != ICStatus.Approved)
						{
							base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Your IC Admin application has not been approved. You cannot enter this site unless you have been approved by USC.");
							base.RedirectToAction("NewMachine", "Home");
						}
					}
					if (!base.HasValidCookie(new UserDataModel()
					{
						Username = model.Username,
						IPAddress = base.GetIPAddress(),
						MachineName = base.DetermineCompName()
					}) && userByUsername.UserType != UserType.SiteAdmin)
					{
						base.SendAccessEmail(model.Username);
						return base.RedirectToAction("NewMachine", "Home");
					}
					if (!model.AssetNumber.HasValue)
					{
						return base.RedirectToAction("MyUSCPage", "Home");
					}
					AssetDescriptionModel assetByAssetNumber = this._asset.GetAssetByAssetNumber(model.AssetNumber.Value);
					if (assetByAssetNumber.AssetId == Guid.Empty)
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You have entered an Asset ID # that is not in the USC database.  Please enter an alternative Asset ID # or, please continue your Asset Search through the normal protocols of the website.  Thank you.");
						return base.RedirectToAction("MyUSCPage", "Home");
					}
					if (userByUsername.UserType == UserType.CorpAdmin2 || userByUsername.UserType == UserType.CorpAdmin || userByUsername.UserType == UserType.SiteAdmin)
					{
						return base.RedirectToAction("ViewAsset", "DataPortal", new { id = assetByAssetNumber.AssetId, fromManageAssets = true });
					}
					if (!userByUsername.SignedNCND)
					{
						return base.RedirectToAction("SignDocsInformation", "Home");
					}
					if (!assetByAssetNumber.Show)
					{
						base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You have entered an Asset ID # that is not in the USC database.  Please enter an alternative Asset ID # or, please continue your Asset Search through the normal protocols of the website.  Thank you.");
						return base.RedirectToAction("MyUSCPage", "Home");
					}
					if (!string.IsNullOrEmpty(model.ReturnUrl))
					{
						return this.Redirect(model.ReturnUrl);
					}
					if (userByUsername.UserType != UserType.CorpAdmin2 && userByUsername.UserType != UserType.CorpAdmin && userByUsername.UserType != UserType.SiteAdmin)
					{
						return base.RedirectToAction("ViewAsset", "DataPortal", new { id = assetByAssetNumber.AssetId });
					}
					return base.RedirectToAction("ViewAsset", "DataPortal", new { id = assetByAssetNumber.AssetId, fromManageAssets = true });
				}
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "The code you entered is incorrect.");
			}
			if (!string.IsNullOrEmpty(model.ReturnUrl))
			{
				return this.Redirect(model.ReturnUrl);
			}
			base.TempData["TempUserEmail"] = model.Username;
			return base.RedirectToAction("NewMachine", "Home");
		}

		[Authorize]
		[HttpGet]
		public ActionResult PerformTediousTaskThatWouldTakeTooLongWithoutCode()
		{
			ActionResult actionResult;
			UserModel userByUsername = this._userManager.GetUserByUsername(base.User.Identity.Name);
			if (userByUsername.UserType != UserType.CorpAdmin && userByUsername.UserType != UserType.CorpAdmin2)
			{
				return base.RedirectToAction("MyUSCPage");
			}
			try
			{
				this._userManager.PerformTediousTaskThatWouldTakeTooLongWithoutCode();
				return base.Content("Completed");
			}
			catch (Exception exception)
			{
				actionResult = base.Content(exception.Message);
			}
			return actionResult;
		}

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
			return 
				from userPf in portfolioQuickListViewModels
				select new SelectListItem()
				{
					Text = userPf.PortfolioName,
					Value = userPf.PortfolioId.ToString()
				};
		}

		[HttpGet]
		public ViewResult PrivacyPolicy()
		{
			return base.View();
		}

		[Authorize]
		[HttpGet]
		public ActionResult ReferUser()
		{
			return base.View(new UserReferralModel());
		}

		[Authorize]
		[HttpPost]
		public ActionResult ReferUser(UserReferralModel model)
		{
			if (!base.ModelState.IsValid)
			{
				return base.View(model);
			}
			Tuple<bool, bool> tuple = this._userManager.CanUserBeReferred(model.Email);
			if (tuple.Item1)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "User already registered.");
				return base.View(model);
			}
			if (tuple.Item2)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "This email is already referred.");
				return base.View(model);
			}
			string str = this._userManager.ReferUser(model.Email, base.User.Identity.Name);
			if (string.IsNullOrEmpty(str))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "There was an error processing your request.");
				return base.View(model);
			}
			if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
			{
				string host = base.Request.Url.Host;
				string str1 = base.Request.Url.Port.ToString();
				string empty = string.Empty;
				empty = (str1 != null ? string.Concat(new string[] { "http://", host, ":", str1, "/Home/Registration?rId=", str }) : string.Concat("http://", host, "/Home/Registration?rId=", str));
				UserReferralSubmittedEmail userReferralSubmittedEmail = new UserReferralSubmittedEmail()
				{
					To = model.Email,
					ReferrerEmail = base.User.Identity.Name.ToLower(),
					Url = empty
				};
				try
				{
					this._email.Send(userReferralSubmittedEmail);
				}
				catch (Exception exception)
				{
				}
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "User successfully referred.");
			return base.RedirectToAction("MyUscPage");
		}

		[HttpGet]
		public ActionResult Registration(int? id = null, string rurl = null, string rId = null)
		{
			RegistrationModel registrationModel = new RegistrationModel();
			if (rurl != null)
			{
				registrationModel.ReturnUrl = rurl;
			}
			if (id.HasValue)
			{
				registrationModel.AssetId = id;
			}
			if (!string.IsNullOrEmpty(rId))
			{
				registrationModel.ReferralId = rId;
			}
			return base.View(registrationModel);
		}

		[HttpPost]
		public ActionResult Registration(RegistrationModel model, string SubmitRegistration)
		{
			if (model.SelectedUserType != UserType.ICAdmin && !model.CommercialOfficeInterest && !model.CommercialOtherInterest && !model.CommercialRetailInterest && !model.MHPInterest && !model.MultiFamilyInterest && !model.SecuredPaperInterest && !model.FracturedCondoPortfoliosInterest && !model.FuelServicePropertyInterest && !model.GorvernmentTenantPropertyInterest && !model.IndustryTenantPropertyInterest && !model.MedicalTenantPropertyInterest && !model.MiniStoragePropertyInterest && !model.MixedUseCommercialPropertyInterest && !model.NonCompletedDevelopmentsInterest && !model.OfficeTenantPropertyInterest && !model.ParkingGaragePropertyInterest && !model.ResortHotelMotelPropertyInterest && !model.RetailTenantPropertyInterest && !model.SingleTenantRetailPortfoliosInterest)
			{
				base.ModelState.AddModelError("CommercialOfficeInterest", "You must select at least one asset type.");
			}
			if (!base.ModelState.IsValid)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "One or more fields are invalid.");
				return base.View(model);
			}
			if (!model.TermsOfUse || !model.AgreesToNCND)
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "You must agree to the Terms of Use and the NCND in order to continue.");
				return base.View(model);
			}
			NCNDTemplateModel nCNDTemplateModel = new NCNDTemplateModel()
			{
				AcronymOfCorporateEntity = model.AcroynmForCorporateEntity,
				AgreeToTerms = model.TermsOfUse,
				City = model.City,
				CompanyName = model.CompanyName,
				CorpTitle = model.CorporateTitle,
				Email = model.Username,
				Fax = model.FaxNumber,
				Phone = model.WorkNumber,
				State = model.SelectedState,
				StateOfOriginOfCorporateEntity = model.StateOfOriginCorporateEntity,
				TypeOfCorporateEntity = model.SelectedCorporateEntityType,
				UserAddressLine1 = model.AddressLine1,
				UserAddressLine2 = model.AddressLine2,
				UserFirstName = model.FirstName,
				UserLastName = model.LastName,
				Zip = model.Zip
			};
			if (SubmitRegistration == "Preview Completed NCND")
			{
				return this.File(this._pdf.GetNCNDPdf(nCNDTemplateModel, Properties.Resources.NCNDTemplate), "application/octet-stream", "download.pdf");
			}
			if (this._userManager.UserExists(model.Username))
			{
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Username already exists.");
				return base.View(model);
			}
			int assetIds = this._userManager.CreateUser(model);
			this._pdf.CreateNCND(Properties.Resources.NCNDTemplate, nCNDTemplateModel);
			base.AuthenticateMachine(model.Username);
			base.CreateCookie(true, new UserDataModel()
			{
				Username = model.Username,
				IPAddress = base.GetIPAddress(),
				MachineName = base.DetermineCompName(),
				SecurityCode = ""
			});
			string accessCode = base.GetAccessCode(model.Username);
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Registration successfully submitted.");
			if (base.CanSendAutoEmail(BaseController.AutoEmailType.General))
			{
				UserModel jVMAParticipantByUser = this._userManager.GetJVMAParticipantByUser(model.Username);
				if (jVMAParticipantByUser != null)
				{
					List<SellerAssetQuickListModel> sellerManageAssetsQuickList = this._asset.GetSellerManageAssetsQuickList(new ManageAssetsModel()
					{
						Email = model.Username
					});
					string empty = string.Empty;
					string str = string.Empty;
					double showablePrice = 0;
					if (sellerManageAssetsQuickList != null && sellerManageAssetsQuickList.Count > 0)
					{
						SellerAssetQuickListModel sellerAssetQuickListModel = (
							from x in sellerManageAssetsQuickList
							orderby x.AssetNumber descending
							select x).FirstOrDefault<SellerAssetQuickListModel>();
						if (sellerAssetQuickListModel != null)
						{
							empty = sellerAssetQuickListModel.Description;
							str = sellerAssetQuickListModel.AssetNumber.ToString();
							showablePrice = sellerAssetQuickListModel.ShowablePrice;
						}
					}
					this._email.Send(new NotificationToReferringCREMtgLBRegistrantSignedUp()
					{
						AssetDescription = empty,
						AssetNumber = str,
						AssetListPrice = showablePrice,
						EmailAddress = jVMAParticipantByUser.Username,
						NameOfReferredBy = jVMAParticipantByUser.FullName,
						NameOfRegistrant = model.FullName,
						RegistrantEmail = model.Username,
						RegistrantPhone = model.WorkNumber
					});
				}
				if (model.SelectedUserType != UserType.ICAdmin)
				{
					this._email.Send(new RegistrationAutoWelcomeEmail()
					{
						AssetId = (model.AssetId.HasValue ? model.AssetId.Value.ToString() : ""),
						City = model.City,
						Email = model.Username,
						RegistrationType = EnumHelper.GetEnumDescription(model.SelectedUserType),
						State = model.SelectedState,
						To = model.FullName,
						Code = accessCode
					});
				}
				else
				{
					this._email.Send(new ICAdminWelcome()
					{
						Email = model.Username,
						To = model.FullName
					});
					foreach (UserModel admin in this._userManager.GetAdmins())
					{
						this._email.Send(new ICAdminWelcome()
						{
							Email = admin.Username,
							To = model.FullName
						});
					}
				}
			}
			this._auth.Authenticate(model.Username, model.Password);
			if (!base.HasValidCookie(new UserDataModel()
			{
				Username = model.Username,
				MachineName = base.DetermineCompName(),
				IPAddress = base.GetIPAddress()
			}))
			{
				base.TempData["TempUserEmail"] = model.Username;
				return base.RedirectToAction("NewMachine", "Home");
			}
			if (model.SelectedUserType == UserType.ICAdmin)
			{
				base.TempData["NewICAdmin"] = "true";
				return base.RedirectToAction("Registration");
			}
			if (model.AssetIds.Count <= 0)
			{
				if (string.IsNullOrEmpty(model.ReturnUrl))
				{
					return base.RedirectToAction("MyUscPage");
				}
				return this.Redirect(model.ReturnUrl);
			}
			base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Registration successfully submitted. Please sign an IPA to view your selected assets.");
			base.TempData[string.Concat(assetIds, "Assets")] = model.AssetIds;
			return base.RedirectToAction("ExecuteMDAs", "DataPortal");
		}

		[HttpPost]
		public ActionResult RegistrationAlt(IEnumerable<AssetQuickListViewModel> items)
		{
			RegistrationModel registrationModel = new RegistrationModel();
			List<Guid> guids = new List<Guid>();
			items.ToList<AssetQuickListViewModel>().ForEach((AssetQuickListViewModel item) => {
				if (item.IsSelected)
				{
					guids.Add(item.AssetId);
				}
			});
			registrationModel.AssetIds = guids;
			return base.View("Registration", registrationModel);
		}

		[HttpGet]
		public ViewResult RegistrationIntro()
		{
			return base.View();
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

		[HttpPost]
		[MultipleButton(Name="action", Argument="ResendEmail")]
		public ActionResult ResendEmail(UserMachineModel model)
		{
			try
			{
				if (base.User.Identity.IsAuthenticated)
				{
					base.ResendEmail(this._securityManager, this._userManager, this._email);
				}
				else if (string.IsNullOrEmpty(model.Username))
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Please enter username in order to resend code.");
				}
				else if (string.IsNullOrEmpty(model.Password))
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "Username & Password both need to be entered.");
				}
				else
				{
					base.SendAccessEmail(model.Username);
					base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Code sent. Please check your email.");
				}
			}
			catch
			{
			}
			return base.RedirectToAction("NewMachine");
		}

		[HttpGet]
		public ActionResult RetrieveImagesFromPdf()
		{
			FileStream fileStream = null;
			try
			{
				fileStream = System.IO.File.OpenRead("C:\\Users\\shamilton\\Documents\\EPI 808\\Villager Marketing Package.pdf");
				byte[] numArray = new byte[(fileStream.Length)];
				fileStream.Read(numArray, 0, Convert.ToInt32(fileStream.Length));
				this._pdf.GetBitmapImagesFromPDFForLocalComp(numArray, "VillageMarketingPackage");
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
					fileStream.Dispose();
				}
			}
			return base.View();
		}

		public ActionResult SaveUploadedFile()
		{
			bool flag = true;
			string fileName = "";
			try
			{
				foreach (string file in base.Request.Files)
				{
					HttpPostedFileBase item = base.Request.Files[file];
					fileName = item.FileName;
					if (item == null || item.ContentLength <= 0)
					{
						continue;
					}
					string str = Path.Combine((new DirectoryInfo(string.Format("{0}Images\\WallImages", base.Server.MapPath("\\")))).ToString(), "imagepath");
					Path.GetFileName(item.FileName);
					if (!Directory.Exists(str))
					{
						Directory.CreateDirectory(str);
					}
					string str1 = string.Format("{0}\\{1}", str, item.FileName);
					item.SaveAs(str1);
				}
			}
			catch (Exception exception)
			{
				flag = false;
			}
			if (flag)
			{
				return base.Json(new { Message = fileName });
			}
			return base.Json(new { Message = "Error in saving file" });
		}

		[HttpGet]
		public ViewResult SignDocsInformation()
		{
			return base.View();
		}

		[HttpGet]
		public ActionResult TestSecurity()
		{
			if (base.User.Identity.IsAuthenticated)
			{
				if (!base.HasValidCookie(new UserDataModel()
				{
					Username = base.User.Identity.Name,
					IPAddress = base.GetIPAddress(),
					MachineName = base.DetermineCompName()
				}))
				{
					base.SendAccessEmail(base.User.Identity.Name);
					return base.RedirectToAction("NewMachine", "Home");
				}
				base.TempData["message"] = new MessageViewModel(MessageTypes.Success, "Machine Authenticated!");
			}
			return base.View();
		}

		[HttpGet]
		public ActionResult TestWrite()
		{
			this._asset.DeleteStrandedVideosAndConvertMp4s();
			return base.View();
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
							Data = new { message = "true", contentType = item.ContentType, isCorpAdmin = false, isICAdmin = false }
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

		[HttpGet]
		public bool ValidateSiteAuth()
		{
			if (!Convert.ToBoolean(base.Session["RootAuth"]))
			{
				return false;
			}
			return true;
		}

		[HttpPost]
		public JsonResult ValidateUser()
		{
			bool flag = false;
			if (!base.User.Identity.IsAuthenticated)
			{
				return new JsonResult()
				{
					Data = new { Status = flag.ToString() }
				};
			}
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			if (userByUsername.UserType == UserType.Investor || userByUsername.UserType == UserType.CorpAdmin || userByUsername.UserType == UserType.CorpAdmin2 || userByUsername.UserType == UserType.SiteAdmin)
			{
				flag = true;
			}
			return new JsonResult()
			{
				Data = new { Status = flag.ToString() }
			};
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

		[HttpPost]
		public FileUploadJsonResult VideoUpload(HttpPostedFileBase file, Guid guidId)
		{
			if (file == null || string.IsNullOrEmpty(file.FileName))
			{
				return new FileUploadJsonResult()
				{
					Data = new { message = "FileIsNull" }
				};
			}
			if (Path.GetExtension(file.FileName).ToLower() != ".mp4")
			{
				return new FileUploadJsonResult()
				{
					Data = new { message = "Unsupported format." }
				};
			}
			string str = this._fileManager.SaveFile(file, guidId, FileType.Video);
			if (str == null)
			{
				return new FileUploadJsonResult()
				{
					Data = new { message = "false" }
				};
			}
			return new FileUploadJsonResult()
			{
				Data = new { message = "true", filename = str, size = this._fileManager.GetFileSize(file.ContentLength), id = guidId.ToString() }
			};
		}

		[HttpGet]
		public ActionResult ViewAsset(string id, string type)
		{
			int num = 0;
			if (type == "pr")
			{
				if (!int.TryParse(id, out num))
				{
					return base.View("Null");
				}
				PaperResidentialAssetViewModel paperResidentialModel = this._asset.GetPaperResidentialModel(num);
				if (string.IsNullOrEmpty(paperResidentialModel.BeneficiaryEmail))
				{
					return base.View("Null");
				}
				return base.View("ShowResidentialPaper", paperResidentialModel);
			}
			if (type == "pc")
			{
				if (!int.TryParse(id, out num))
				{
					return base.View("Null");
				}
				PaperCommercialAssetViewModel paperCommercialModel = this._asset.GetPaperCommercialModel(num);
				if (string.IsNullOrEmpty(paperCommercialModel.EmailAddress))
				{
					return base.View("Null");
				}
				return base.View("ShowCommercialPaper", paperCommercialModel);
			}
			if (type == "rer")
			{
				if (!int.TryParse(id, out num))
				{
					return base.View("Null");
				}
				RealEstateResidentialAssetViewModel realEstateResidentialModel = this._asset.GetRealEstateResidentialModel(num);
				if (string.IsNullOrEmpty(realEstateResidentialModel.EmailAddress))
				{
					return base.View("Null");
				}
				return base.View("ShowResidentialRealEstate", realEstateResidentialModel);
			}
			if (type != "rec")
			{
				return base.View("Null");
			}
			if (!int.TryParse(id, out num))
			{
				return base.View("Null");
			}
			RealEstateCommercialAssetViewModel realEstateCommercialModel = this._asset.GetRealEstateCommercialModel(num);
			if (string.IsNullOrEmpty(realEstateCommercialModel.EmailAddress))
			{
				return base.View("Null");
			}
			return base.View("ShowCommercialRealEstate", realEstateCommercialModel);
		}
	}
}