using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.ViewModel;
using Inview.Epi.EpiFund.Web.ActionFilters;
using Inview.Epi.EpiFund.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Web.Controllers
{
	[LayoutActionFilter]
	public class RecordsController : BaseController
	{
		private IUserManager _user;

		private IAssetManager _asset;

		public RecordsController(ISecurityManager securityManager, IEPIFundEmailService email, IAssetManager asset, IUserManager user) : base(securityManager, email, user)
		{
			this._user = user;
			this._asset = asset;
		}

		[HttpGet]
		public ActionResult AdminUserSignedMDAList(int id, string sortOrder, string currentFilter, string searchString, int? page)
		{
			List<SignedMDAQuickViewModel> signedMDAQuickViewModels = new List<SignedMDAQuickViewModel>();
			signedMDAQuickViewModels = this._asset.GetSignedMDAs(id);
			if (searchString == null)
			{
				searchString = currentFilter;
			}
			else
			{
				page = new int?(1);
			}
			((dynamic)base.ViewBag).CurrentFilter = searchString;
			((dynamic)base.ViewBag).CurrentSort = sortOrder;
			base.ViewBag.DateSignedSortParm = (sortOrder == "dateSigned" ? "dateSigned_desc" : "dateSigned");
			base.ViewBag.AssetNumberSortParm = (sortOrder == "assetNumber" ? "assetNumber_desc" : "assetNumber");
			base.ViewBag.AddressSortParm = (sortOrder == "address" ? "address_desc" : "address");
			base.ViewBag.CitySortParm = (sortOrder == "city" ? "city_desc" : "city");
			base.ViewBag.StateSortParm = (sortOrder == "state" ? "state_desc" : "state");
			base.ViewBag.DescSortParm = (sortOrder == "desc" ? "desc_desc" : "desc");
			base.ViewBag.PropertyTypeParm = (sortOrder == "propertyType" ? "propertyType_desc" : "propertyType");
			switch (sortOrder)
			{
				case "desc_desc":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.UnitDescription descending
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "desc":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.UnitDescription
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "propertyType_desc":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.PropertyType descending
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "propertyType":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.PropertyType
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "dateSigned_desc":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.DateSigned descending
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "dateSigned":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.DateSigned
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "assetNumber_desc":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.AssetNumber descending
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "assetNumber":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.AssetNumber
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "address_desc":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.AssetAddressLine1 descending
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "address":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.AssetAddressLine1
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "city_desc":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.AssetCity descending
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "city":
				{
					signedMDAQuickViewModels = (
						from w in signedMDAQuickViewModels
						orderby w.AssetCity
						select w).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "state_desc":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.AssetState descending
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				default:
				{
					signedMDAQuickViewModels = (sortOrder == "state" ? (
						from w in signedMDAQuickViewModels
						orderby w.AssetState
						select w).ToList<SignedMDAQuickViewModel>() : (
						from s in signedMDAQuickViewModels
						orderby s.AssetNumber
						select s).ToList<SignedMDAQuickViewModel>());
					break;
				}
			}
			int num = 10;
			int? nullable = page;
			return base.View(signedMDAQuickViewModels.ToPagedList<SignedMDAQuickViewModel>((nullable.HasValue ? nullable.GetValueOrDefault() : 1), num));
		}

		[HttpGet]
		public FileResult DownloadNCND(int id)
		{
			FileResult fileResult;
			try
			{
				string[] strArrays = this._user.GetUserById(id).NCNDLocation.Split(new char[] { '\\' });
				string str = string.Concat(ConfigurationManager.AppSettings["pdfDirectory"], strArrays[(int)strArrays.Length - 2], "\\", strArrays[(int)strArrays.Length - 1]);
				fileResult = this.File(System.IO.File.ReadAllBytes(str), "application/octet-stream", "download.pdf");
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				exception.Message.ToString();
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, string.Concat("There was an error downloading this NCND. Error: ", exception.Message));
				fileResult = null;
			}
			return fileResult;
		}

		[HttpGet]
		public ActionResult DownloadSignedICAgreement(int id)
		{
			this._user.GetUserByUsername(base.User.Identity.Name);
			byte[] userFile = this._user.GetUserFile(id);
			if (userFile == null)
			{
				return base.RedirectToAction("ExecuteICAgreement", "DataPortal");
			}
			return this.File(userFile, "application/octet-stream", "download.pdf");
		}

		[HttpGet]
		public ActionResult DownloadSignedJVAgreement(int id)
		{
			this._user.GetUserByUsername(base.User.Identity.Name);
			byte[] userFile = this._user.GetUserFile(id);
			if (userFile == null)
			{
				return base.RedirectToAction("ExecuteJVAgreement", "DataPortal");
			}
			return this.File(userFile, "application/octet-stream", "download.pdf");
		}

		[HttpGet]
		public FileResult DownloadSignedMDA(int id)
		{
			FileResult fileResult;
			try
			{
				string[] strArrays = this._asset.GetSignedMDALocation(id).Split(new char[] { '\\' });
				string str = string.Concat(ConfigurationManager.AppSettings["pdfDirectory"], strArrays[(int)strArrays.Length - 2], "\\", strArrays[(int)strArrays.Length - 1]);
				fileResult = this.File(System.IO.File.ReadAllBytes(str), "application/octet-stream", "download.pdf");
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, string.Concat("There was an error downloading this IPA. Error: ", exception.Message));
				fileResult = null;
			}
			return fileResult;
		}

		[HttpGet]
		public ActionResult DownloadSignedNCND()
		{
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			byte[] signedNCND = this._user.GetSignedNCND(userByUsername.UserId);
			if (signedNCND == null)
			{
				return base.RedirectToAction("ExecuteNCND", "DataPortal");
			}
			return this.File(signedNCND, "application/octet-stream", "download.pdf");
		}

		[HttpGet]
		public FileResult DownloadUserFile(int id)
		{
			FileResult fileResult;
			try
			{
				byte[] userFile = this._user.GetUserFile(id);
				if (userFile == null)
				{
					base.TempData["message"] = new MessageViewModel(MessageTypes.Error, "There was an error downloading this file.");
					fileResult = null;
				}
				else
				{
					fileResult = this.File(userFile, "application/octet-stream", "download.pdf");
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.TempData["message"] = new MessageViewModel(MessageTypes.Error, string.Concat("There was an error downloading this file. Error: ", exception.Message));
				fileResult = null;
			}
			return fileResult;
		}

		[HttpGet]
		public ActionResult SignedMDAList(string sortOrder, string currentFilter, string searchString, int? page)
		{
			List<SignedMDAQuickViewModel> signedMDAQuickViewModels = new List<SignedMDAQuickViewModel>();
			UserModel userByUsername = this._user.GetUserByUsername(base.User.Identity.Name);
			signedMDAQuickViewModels = this._asset.GetSignedMDAs(userByUsername.UserId);
			if (searchString == null)
			{
				searchString = currentFilter;
			}
			else
			{
				page = new int?(1);
			}
			((dynamic)base.ViewBag).CurrentFilter = searchString;
			((dynamic)base.ViewBag).CurrentSort = sortOrder;
			base.ViewBag.DateSignedSortParm = (sortOrder == "dateSigned" ? "dateSigned_desc" : "dateSigned");
			base.ViewBag.AssetNumberSortParm = (sortOrder == "assetNumber" ? "assetNumber_desc" : "assetNumber");
			base.ViewBag.AddressSortParm = (sortOrder == "address" ? "address_desc" : "address");
			base.ViewBag.CitySortParm = (sortOrder == "city" ? "city_desc" : "city");
			base.ViewBag.StateSortParm = (sortOrder == "state" ? "state_desc" : "state");
			base.ViewBag.DescSortParm = (sortOrder == "desc" ? "desc_desc" : "desc");
			base.ViewBag.PropertyTypeParm = (sortOrder == "propertyType" ? "propertyType_desc" : "propertyType");
			switch (sortOrder)
			{
				case "desc_desc":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.UnitDescription descending
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "desc":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.UnitDescription
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "propertyType_desc":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.PropertyType descending
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "propertyType":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.PropertyType
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "dateSigned_desc":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.DateSigned descending
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "dateSigned":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.DateSigned
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "assetNumber_desc":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.AssetNumber descending
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "assetNumber":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.AssetNumber
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "address_desc":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.AssetAddressLine1 descending
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "address":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.AssetAddressLine1
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "city_desc":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.AssetCity descending
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "city":
				{
					signedMDAQuickViewModels = (
						from w in signedMDAQuickViewModels
						orderby w.AssetCity
						select w).ToList<SignedMDAQuickViewModel>();
					break;
				}
				case "state_desc":
				{
					signedMDAQuickViewModels = (
						from s in signedMDAQuickViewModels
						orderby s.AssetState descending
						select s).ToList<SignedMDAQuickViewModel>();
					break;
				}
				default:
				{
					signedMDAQuickViewModels = (sortOrder == "state" ? (
						from w in signedMDAQuickViewModels
						orderby w.AssetState
						select w).ToList<SignedMDAQuickViewModel>() : (
						from s in signedMDAQuickViewModels
						orderby s.AssetNumber
						select s).ToList<SignedMDAQuickViewModel>());
					break;
				}
			}
			int num = 10;
			int? nullable = page;
			return base.View(signedMDAQuickViewModels.ToPagedList<SignedMDAQuickViewModel>((nullable.HasValue ? nullable.GetValueOrDefault() : 1), num));
		}
	}
}