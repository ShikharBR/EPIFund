using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.ViewModel;
using Inview.Epi.EpiFund.Web.Models.Emails;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Web.Controllers
{
    public class AssetController : BaseController
    {
        private IAssetManager _asset;
        private IUserManager _user;
        private IEPIFundEmailService _email;

        public AssetController(IAssetManager assetManager, IUserManager userManager, ISecurityManager securityManager, IEPIFundEmailService emailManager) : base(securityManager, emailManager, userManager)
        {
            _asset = assetManager;
            _user = userManager;
            _email = emailManager;
        }

        public ActionResult ClaimAsset()
        {
            ((dynamic)base.ViewBag).AssetTypes = GetAssetTypesAsSelectItemList();
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult SaveMessage(string AssetId, string AssetNumber, string Message)
        {
            // I dont know if user authentication is required to use this method and am assuming not at the moment
            try
            {
                // message validation handled in view
                string corpAdminEmailString = ConfigurationManager.AppSettings["CorpAdminEmails"];
                if (!string.IsNullOrEmpty(corpAdminEmailString))
                {
                    var corpAdminEmails = corpAdminEmailString.Split(';');
                    foreach (var emailAddress in corpAdminEmails)
                    {
                        if (IsValidEmail(emailAddress))
                        {
                            _email.Send(new ClaimAssetNotifyCorpAdminEmail()
                            {
                                Message = Message,
                                To = emailAddress,
                                UserEmail = User.Identity.IsAuthenticated ? User.Identity.Name : "N/A"
                            });   
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult() { Data = new {}, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult CheckPropertyExists(string assessorParcelNumber, string state, string county)
        {
            if (string.IsNullOrEmpty(assessorParcelNumber))
            {
                return new JsonResult() { Data = new { Assetdata = new List<AssetAPNMatchModel>() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var apns = assessorParcelNumber.Split(new char[] {',' }, StringSplitOptions.RemoveEmptyEntries);
                List<AssetAPNMatchModel> matchingAssets = new List<AssetAPNMatchModel>();
                foreach(var apn in apns)
                {
                    matchingAssets.AddRange(_asset.GetMatchingAssetsByAPNCountyState(apn, state, county));
                }
                if (matchingAssets.Count > 0)
                {
                    // Hackily get the first asset for now
                    var asset = matchingAssets.FirstOrDefault();
                    return new JsonResult()
                    {
                        Data = new { Assetdata = asset, IsClaimed = true, asset.AssetId },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                return new JsonResult()
                {
                    Data = new { Assetdata = matchingAssets, IsClaimed = false, AssetId = "" },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
    }
}