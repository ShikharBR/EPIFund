using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.Helpers;
using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Inview.Epi.EpiFund.Business
{
	public class InsuranceManager : IInsuranceManager
	{
		private IEPIContextFactory _factory;

		public InsuranceManager(IEPIContextFactory factory)
		{
			this._factory = factory;
		}

		public void ActivateInsuranceCompany(int id)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			IDbSet<PCInsuranceCompany> pCInsuranceCompanies = ePIRepository.PCInsuranceCompanies;
			object[] objArray = new object[] { id };
			PCInsuranceCompany pCInsuranceCompany = pCInsuranceCompanies.Find(objArray);
			if (pCInsuranceCompany != null)
			{
				pCInsuranceCompany.IsActive = true;
				ePIRepository.Save();
			}
		}

		public void ActivateInsuranceCompanyUser(int id)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			PCInsuranceCompanyManager pCInsuranceCompanyManager = QueryableExtensions.Include<PCInsuranceCompanyManager, User>(ePIRepository.PCInsuranceCompanyManagers, (PCInsuranceCompanyManager x) => x.User).SingleOrDefault<PCInsuranceCompanyManager>((PCInsuranceCompanyManager x) => x.PCInsuranceCompanyManagerId == id);
			if (pCInsuranceCompanyManager != null)
			{
				this.DeactivateCurrentActiveManager(ePIRepository, pCInsuranceCompanyManager.PCInsuranceCompanyId, pCInsuranceCompanyManager.User.Username);
				pCInsuranceCompanyManager.IsActive = true;
				pCInsuranceCompanyManager.User.IsActive = true;
				ePIRepository.Entry(pCInsuranceCompanyManager).State = EntityState.Modified;
				ePIRepository.Entry(pCInsuranceCompanyManager.User).State = EntityState.Modified;
				ePIRepository.Save();
			}
		}

		public bool CompanyExist(string companyName)
		{
			bool flag = this._factory.Create().PCInsuranceCompanies.Any<PCInsuranceCompany>((PCInsuranceCompany x) => x.CompanyName.ToLower() == companyName.ToLower());
			return flag;
		}

		public void CreateInsuranceCompany(InsuranceCompanyViewModel model)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			PCInsuranceCompany pCInsuranceCompany = new PCInsuranceCompany()
			{
				CompanyAddress1 = model.CompanyAddress,
				CompanyAddress2 = model.CompanyAddress2,
				CompanyCity = model.CompanyCity,
				CompanyName = model.CompanyName,
				CompanyState = model.CompanyState,
				CompanyURL = model.CompanyURL,
				CompanyZip = model.CompanyZip,
				CreateDate = DateTime.Now,
				IsActive = true,
				CompanyPhoneNumber = model.CompanyPhone
			};
			ePIRepository.PCInsuranceCompanies.Add(pCInsuranceCompany);
			ePIRepository.Save();
		}

		public int CreateInsuranceCompanyUser(InsuranceCompanyUserViewModel model)
		{
			int pCInsuranceCompanyManagerId;
			IEPIRepository ePIRepository = this._factory.Create();
			try
			{
				IDbSet<PCInsuranceCompany> pCInsuranceCompanies = ePIRepository.PCInsuranceCompanies;
				object[] insuranceCompanyId = new object[] { model.InsuranceCompanyId };
				PCInsuranceCompany pCInsuranceCompany = pCInsuranceCompanies.Find(insuranceCompanyId);
				MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
				User user = new User()
				{
					CellNumber = model.PhoneNumber,
					FirstName = model.FirstName,
					IsActive = model.IsActive,
					LastName = model.LastName,
					Username = model.Email.ToLower(),
					Password = mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(model.Password)),
					SignupDate = new DateTime?(DateTime.Now),
					UserType = UserType.PCInsuranceManager,
					AcronymForCorporateEntity = null,
					CorporateTitle = null,
					CommercialOfficeInterest = false,
					CommercialOtherInterest = false,
					CommercialRetailInterest = false,
					MultiFamilyInterest = false,
					MHPInterest = false,
					SecuredPaperInterest = false,
					IsCertificateOfGoodStandingAvailable = false
				};
				ePIRepository.Users.Add(user);
				ePIRepository.Save();
				if (model.IsActive)
				{
					this.DeactivateCurrentActiveManager(ePIRepository, model.InsuranceCompanyId, model.Email);
				}
				PCInsuranceCompanyManager pCInsuranceCompanyManager = new PCInsuranceCompanyManager()
				{
					IsActive = model.IsActive,
					PCInsuranceCompany = pCInsuranceCompany,
					PCInsuranceCompanyId = pCInsuranceCompany.PCInsuranceCompanyId,
					User = user,
					UserId = user.UserId
				};
				ePIRepository.PCInsuranceCompanyManagers.Add(pCInsuranceCompanyManager);
				ePIRepository.Save();
				pCInsuranceCompanyManagerId = pCInsuranceCompanyManager.PCInsuranceCompanyManagerId;
			}
			catch (Exception exception)
			{
				pCInsuranceCompanyManagerId = 0;
			}
			return pCInsuranceCompanyManagerId;
		}

		private void DeactivateCurrentActiveManager(IEPIRepository context, int companyId, string email)
		{
			List<PCInsuranceCompanyManager> list = (
				from x in QueryableExtensions.Include<PCInsuranceCompanyManager, User>(context.PCInsuranceCompanyManagers, (PCInsuranceCompanyManager x) => x.User)
				where x.PCInsuranceCompanyId == companyId && (x.User.Username != email.ToLower())
				select x).ToList<PCInsuranceCompanyManager>();
			if (list.Count > 0)
			{
				list.ForEach((PCInsuranceCompanyManager pc) => {
					pc.User.IsActive = false;
					pc.IsActive = false;
				});
				context.Save();
			}
		}

		public void DeactivateInsuranceCompany(int id)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			IDbSet<PCInsuranceCompany> pCInsuranceCompanies = ePIRepository.PCInsuranceCompanies;
			object[] objArray = new object[] { id };
			PCInsuranceCompany pCInsuranceCompany = pCInsuranceCompanies.Find(objArray);
			if (pCInsuranceCompany != null)
			{
				pCInsuranceCompany.IsActive = false;
				ePIRepository.Save();
			}
		}

		public void DeactivateInsuranceCompanyUser(int id)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			PCInsuranceCompanyManager pCInsuranceCompanyManager = QueryableExtensions.Include<PCInsuranceCompanyManager, User>(ePIRepository.PCInsuranceCompanyManagers, (PCInsuranceCompanyManager x) => x.User).SingleOrDefault<PCInsuranceCompanyManager>((PCInsuranceCompanyManager x) => x.PCInsuranceCompanyManagerId == id);
			if (pCInsuranceCompanyManager != null)
			{
				pCInsuranceCompanyManager.IsActive = false;
				pCInsuranceCompanyManager.User.IsActive = false;
				ePIRepository.Entry(pCInsuranceCompanyManager).State = EntityState.Modified;
				ePIRepository.Entry(pCInsuranceCompanyManager.User).State = EntityState.Modified;
				ePIRepository.Save();
			}
		}

		public AssetDocumentOrderRequestModel GetEmailingModel(Guid assetId, int managerId)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			AssetDocumentOrderRequestModel assetDocumentOrderRequestModel = new AssetDocumentOrderRequestModel();
			IDbSet<Asset> assets = ePIRepository.Assets;
			object[] objArray = new object[] { assetId };
			Asset asset = assets.Find(objArray);
			PCInsuranceCompanyManager pCInsuranceCompanyManager = QueryableExtensions.Include<PCInsuranceCompanyManager, User>(QueryableExtensions.Include<PCInsuranceCompanyManager, PCInsuranceCompany>(ePIRepository.PCInsuranceCompanyManagers, (PCInsuranceCompanyManager x) => x.PCInsuranceCompany), (PCInsuranceCompanyManager x) => x.User).FirstOrDefault<PCInsuranceCompanyManager>((PCInsuranceCompanyManager x) => x.PCInsuranceCompanyManagerId == managerId);
			int? pCInsuranceOrderedByUserId = asset.PCInsuranceOrderedByUserId;
			this.SetUpEmailingCampaign(ePIRepository, assetDocumentOrderRequestModel, asset, pCInsuranceCompanyManager, pCInsuranceOrderedByUserId.Value);
			return assetDocumentOrderRequestModel;
		}

		public List<InsuranceCompanyViewModel> GetInsuranceCompanies(CompanySearchModel model)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			List<PCInsuranceCompany> list = ePIRepository.PCInsuranceCompanies.ToList<PCInsuranceCompany>();
			if (!string.IsNullOrEmpty(model.CompanyName))
			{
				list = (
					from w in list
					where (w.CompanyName == null ? false : w.CompanyName.ToLower().Contains(model.CompanyName.ToLower()))
					select w).ToList<PCInsuranceCompany>();
			}
			if (!string.IsNullOrEmpty(model.CompanyURL))
			{
				list = (
					from w in list
					where (w.CompanyURL == null ? false : w.CompanyURL.ToLower().Contains(model.CompanyURL.ToLower()))
					select w).ToList<PCInsuranceCompany>();
			}
			if (model.NeedsManager)
			{
				foreach (PCInsuranceCompany pCInsuranceCompany in ePIRepository.PCInsuranceCompanies.ToList<PCInsuranceCompany>())
				{
					if ((
						from x in ePIRepository.PCInsuranceCompanyManagers
						where x.IsActive
						select x).FirstOrDefault<PCInsuranceCompanyManager>() == null)
					{
						try
						{
							list.Remove(pCInsuranceCompany);
						}
						catch
						{
						}
					}
				}
			}
			List<InsuranceCompanyViewModel> insuranceCompanyViewModels = new List<InsuranceCompanyViewModel>();
			list.ForEach((PCInsuranceCompany f) => {
				InsuranceCompanyViewModel insuranceCompanyViewModel = new InsuranceCompanyViewModel()
				{
					CompanyAddress = f.CompanyAddress1,
					CompanyAddress2 = f.CompanyAddress2,
					CompanyCity = f.CompanyCity,
					CompanyName = f.CompanyName,
					CompanyState = f.CompanyState,
					CompanyURL = f.CompanyURL,
					CompanyZip = f.CompanyZip,
					CreateDate = f.CreateDate,
					IsActive = f.IsActive,
					InsuranceCompanyId = f.PCInsuranceCompanyId,
					CompanyPhone = f.CompanyPhoneNumber
				};
				insuranceCompanyViewModels.Add(insuranceCompanyViewModel);
			});
			return insuranceCompanyViewModels;
		}

		public InsuranceCompanyViewModel GetInsuranceCompany(int id)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			InsuranceCompanyViewModel insuranceCompanyViewModel = new InsuranceCompanyViewModel();
			IDbSet<PCInsuranceCompany> pCInsuranceCompanies = ePIRepository.PCInsuranceCompanies;
			object[] objArray = new object[] { id };
			PCInsuranceCompany pCInsuranceCompany = pCInsuranceCompanies.Find(objArray);
			if (pCInsuranceCompany != null)
			{
				insuranceCompanyViewModel.EntityToModel(pCInsuranceCompany);
			}
			return insuranceCompanyViewModel;
		}

		public List<OrderModel> GetInsuranceCompanyOrders(OrderSearchModel model)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			List<OrderModel> list = (
				from a in ePIRepository.Assets
				join apn in ePIRepository.AssetTaxParcelNumbers on a.AssetId equals apn.AssetId into g
				join c in ePIRepository.PCInsuranceCompanies on a.PCInsuranceCompanyId equals (int?)c.PCInsuranceCompanyId
				join cm in ePIRepository.PCInsuranceCompanyManagers on c.PCInsuranceCompanyId equals cm.PCInsuranceCompanyId
				join u in ePIRepository.Users on cm.UserId equals u.UserId
				where (int)a.PCInsuranceOrderStatus != 0 && a.PCInsuranceCompanyId == (int?)model.InsuranceCompanyId && a.IsActive && u.IsActive
				select new OrderModel()
				{
					AssetNumber = a.AssetNumber,
					AssetId = a.AssetId,
					Type = (AssetType?)a.AssetType,
					AssetName = a.ProjectName ?? "",
					County = a.County ?? "",
					City = a.City ?? "",
					State = a.State,
					Apns = g.Select<AssetTaxParcelNumber, string>((AssetTaxParcelNumber x) => x.TaxParcelNumber),
					DateOfOrder = a.PCInsuranceOrderDate,
					OrderStatus = a.PCInsuranceOrderStatus,
					DateOfSubmit = a.PCInsuranceDateOfOrderSubmit,
					InsuranceCompanyId = model.InsuranceCompanyId,
					FirstName = u.FirstName,
					LastName = u.LastName
				}).ToList<OrderModel>();
			if (model.SelectedAssetType.HasValue)
			{
				list = list.Where<OrderModel>((OrderModel w) => {
					AssetType? type = w.Type;
					AssetType value = model.SelectedAssetType.Value;
					return (type.GetValueOrDefault() != value ? false : type.HasValue);
				}).ToList<OrderModel>();
			}
			if (model.AssetNumber.GetValueOrDefault(0) > 0)
			{
				list = (
					from x in list
					where x.AssetNumber == model.AssetNumber.Value
					select x).ToList<OrderModel>();
			}
			if (!string.IsNullOrEmpty(model.AssetName))
			{
				list = (
					from x in list
					where (string.IsNullOrEmpty(x.AssetName) ? false : x.AssetName.ToLower().Contains(model.AssetName.ToLower()))
					select x).ToList<OrderModel>();
			}
			if (!string.IsNullOrEmpty(model.AssetCounty))
			{
				list = (
					from x in list
					where (string.IsNullOrEmpty(x.County) ? false : x.County.ToLower().Contains(model.AssetCounty.ToLower()))
					select x).ToList<OrderModel>();
			}
			if (!string.IsNullOrEmpty(model.City))
			{
				list = (
					from x in list
					where (string.IsNullOrEmpty(x.City) ? false : x.City.ToLower().Contains(model.City.ToLower()))
					select x).ToList<OrderModel>();
			}
			if (!string.IsNullOrEmpty(model.State))
			{
				list = (
					from a in list
					where a.State == model.State
					select a).ToList<OrderModel>();
			}
			if (model.DateOrderedStartDate.HasValue)
			{
				list = list.Where<OrderModel>((OrderModel a) => {
					DateTime? dateOfOrder = a.DateOfOrder;
					DateTime value = model.DateOrderedStartDate.Value;
					return (dateOfOrder.HasValue ? dateOfOrder.GetValueOrDefault() >= value : false);
				}).ToList<OrderModel>();
			}
			if (model.DateOrderedEndDate.HasValue)
			{
				list = list.Where<OrderModel>((OrderModel a) => {
					DateTime? dateOfOrder = a.DateOfOrder;
					DateTime value = model.DateOrderedEndDate.Value;
					return (dateOfOrder.HasValue ? dateOfOrder.GetValueOrDefault() <= value : false);
				}).ToList<OrderModel>();
			}
			if (model.DateSubmittedStartDate.HasValue)
			{
				list = list.Where<OrderModel>((OrderModel a) => {
					DateTime? dateOfSubmit = a.DateOfSubmit;
					DateTime value = model.DateSubmittedStartDate.Value;
					return (dateOfSubmit.HasValue ? dateOfSubmit.GetValueOrDefault() >= value : false);
				}).ToList<OrderModel>();
			}
			if (model.DateSubmittedEndDate.HasValue)
			{
				list = list.Where<OrderModel>((OrderModel a) => {
					DateTime? dateOfSubmit = a.DateOfSubmit;
					DateTime value = model.DateSubmittedEndDate.Value;
					return (dateOfSubmit.HasValue ? dateOfSubmit.GetValueOrDefault() <= value : false);
				}).ToList<OrderModel>();
			}
			foreach (OrderModel orderModel in list)
			{
				try
				{
					if (orderModel.Apns != null)
					{
						orderModel.APN = (orderModel.Apns.Count<string>() > 0 ? orderModel.Apns.First<string>() : "");
					}
				}
				catch
				{
				}
			}
			if (!string.IsNullOrEmpty(model.APN))
			{
				list = (
					from x in list
					where (string.IsNullOrEmpty(x.APN) ? false : string.Join("", x.Apns).ToLower().Contains(model.APN.ToLower()))
					select x).ToList<OrderModel>();
			}
			return list;
		}

		public InsuranceCompanyUserViewModel GetInsuranceCompanyUser(int id)
		{
			InsuranceCompanyUserViewModel insuranceCompanyUserViewModel;
			IEPIRepository ePIRepository = this._factory.Create();
			InsuranceCompanyUserViewModel insuranceCompanyUserViewModel1 = new InsuranceCompanyUserViewModel();
			PCInsuranceCompanyManager pCInsuranceCompanyManager = QueryableExtensions.Include<PCInsuranceCompanyManager, User>(QueryableExtensions.Include<PCInsuranceCompanyManager, PCInsuranceCompany>(ePIRepository.PCInsuranceCompanyManagers, (PCInsuranceCompanyManager x) => x.PCInsuranceCompany), (PCInsuranceCompanyManager x) => x.User).FirstOrDefault<PCInsuranceCompanyManager>((PCInsuranceCompanyManager x) => x.PCInsuranceCompanyManagerId == id);
			if (pCInsuranceCompanyManager == null)
			{
				insuranceCompanyUserViewModel = null;
			}
			else
			{
				insuranceCompanyUserViewModel1.EntityToModel(pCInsuranceCompanyManager);
				insuranceCompanyUserViewModel1.Company.EntityToModel(pCInsuranceCompanyManager.PCInsuranceCompany);
				insuranceCompanyUserViewModel = insuranceCompanyUserViewModel1;
			}
			return insuranceCompanyUserViewModel;
		}

		public List<InsuranceCompanyUserViewModel> GetInsuranceCompanyUsers(CompanyUserSearchModel model)
		{
			List<PCInsuranceCompanyManager> list = QueryableExtensions.Include<PCInsuranceCompanyManager, User>(
				from x in this._factory.Create().PCInsuranceCompanyManagers
				where (int?)x.PCInsuranceCompanyId == model.InsuranceCompanyId
				select x, (PCInsuranceCompanyManager x) => x.User).ToList<PCInsuranceCompanyManager>();
			if (!string.IsNullOrEmpty(model.FirstName))
			{
				list = (
					from w in list
					where (w.User.FirstName == null ? false : w.User.FirstName.ToLower().Contains(model.FirstName.ToLower()))
					select w).ToList<PCInsuranceCompanyManager>();
			}
			if (!string.IsNullOrEmpty(model.LastName))
			{
				list = (
					from w in list
					where (w.User.LastName == null ? false : w.User.LastName.ToLower().Contains(model.LastName.ToLower()))
					select w).ToList<PCInsuranceCompanyManager>();
			}
			List<InsuranceCompanyUserViewModel> insuranceCompanyUserViewModels = new List<InsuranceCompanyUserViewModel>();
			list.ForEach((PCInsuranceCompanyManager f) => {
				InsuranceCompanyUserViewModel insuranceCompanyUserViewModel = new InsuranceCompanyUserViewModel()
				{
					Email = f.User.Username,
					FirstName = f.User.FirstName,
					InsuranceCompanyUserId = f.PCInsuranceCompanyManagerId,
					IsActive = f.IsActive,
					LastName = f.User.LastName
				};
				insuranceCompanyUserViewModels.Add(insuranceCompanyUserViewModel);
			});
			return insuranceCompanyUserViewModels;
		}

		public AssetDocumentOrderRequestModel RequestDocuments(Guid assetId, int userId, int insuranceCompanyId, bool finalized)
		{
			AssetDocumentOrderRequestModel assetDocumentOrderRequestModel = new AssetDocumentOrderRequestModel()
			{
				Valid = false
			};
			IEPIRepository ePIRepository = this._factory.Create();
			try
			{
				Asset nullable = (
					from x in ePIRepository.Assets
					where x.AssetId == assetId
					select x).FirstOrDefault<Asset>();
				if (nullable != null)
				{
					PCInsuranceCompanyManager pCInsuranceCompanyManager = QueryableExtensions.Include<PCInsuranceCompanyManager, PCInsuranceCompany>(ePIRepository.PCInsuranceCompanyManagers, (PCInsuranceCompanyManager x) => x.PCInsuranceCompany).FirstOrDefault<PCInsuranceCompanyManager>((PCInsuranceCompanyManager x) => x.PCInsuranceCompanyId == insuranceCompanyId && x.IsActive);
					if (pCInsuranceCompanyManager != null)
					{
						assetDocumentOrderRequestModel.Valid = true;
						nullable.PCInsuranceOrderStatus = OrderStatus.Pending;
						nullable.PCInsuranceCompanyId = new int?(insuranceCompanyId);
						nullable.PCInsuranceOrderedByUserId = new int?(userId);
						nullable.PCInsuranceOrderDate = new DateTime?(DateTime.Now);
						ePIRepository.Save();
						this.SetUpEmailingCampaign(ePIRepository, assetDocumentOrderRequestModel, nullable, pCInsuranceCompanyManager, userId);
					}
				}
			}
			catch (Exception exception)
			{
				assetDocumentOrderRequestModel.Message = string.Concat("There was an error processing your request. ", exception.Message);
			}
			return assetDocumentOrderRequestModel;
		}

		private void SetUpEmailingCampaign(IEPIRepository context, AssetDocumentOrderRequestModel model, Asset asset, PCInsuranceCompanyManager manager, int userId)
		{
			IDbSet<User> users = context.Users;
			object[] objArray = new object[] { userId };
			User user = users.Find(objArray);
			if ((asset.AssetType == AssetType.MultiFamily ? false : asset.AssetType != AssetType.MHP))
			{
				CommercialAsset commercialAsset = asset as CommercialAsset;
				model.AssetDescription = string.Format("{0} square foot {1}", commercialAsset.SquareFeet, EnumHelper.GetEnumDescription(asset.AssetType));
				model.AssetSpecs = string.Format("Year Build: {0} Total Rentable Sq.Ft: {1}", commercialAsset.YearBuilt, commercialAsset.SquareFeet);
			}
			else
			{
				MultiFamilyAsset multiFamilyAsset = asset as MultiFamilyAsset;
				model.AssetDescription = string.Format("{0} unit {1}", multiFamilyAsset.TotalUnits, EnumHelper.GetEnumDescription(asset.AssetType));
				model.AssetSpecs = string.Format("Year Build: {0} Total Units: {1}", multiFamilyAsset.YearBuilt, multiFamilyAsset.TotalUnits);
			}
			model.AssetId = asset.AssetNumber.ToString();
			model.Address1 = asset.PropertyAddress;
			model.State = asset.State;
			model.City = asset.City;
			model.Zip = asset.Zip;
			model.Ownership = asset.Owner;
			List<AssetTaxParcelNumber> list = (
				from x in context.AssetTaxParcelNumbers
				where x.AssetId == asset.AssetId
				select x).ToList<AssetTaxParcelNumber>();
			if ((list == null ? false : list.Count > 0))
			{
				model.APN = string.Join(",", (
					from x in list
					select x.TaxParcelNumber).ToArray<string>());
			}
			model.CompanyName = manager.PCInsuranceCompany.CompanyName;
			model.VestingEntity = string.Format("{0} a {1} {2}", user.CompanyName, user.StateOfOriginCorporateEntity, EnumHelper.GetEnumDescription(user.CorporateEntityType));
			model.BuyerEIN = user.EIN;
			model.BuyerName = user.FullName;
			List<string> strs = new List<string>();
			if (!string.IsNullOrEmpty(user.CellNumber))
			{
				strs.Add(string.Concat("Cell #: ", user.CellNumber));
			}
			if (!string.IsNullOrEmpty(user.WorkNumber))
			{
				strs.Add(string.Concat(" Work #: ", user.WorkNumber));
			}
			strs.Add(string.Concat(" Email: ", user.Username));
			model.BuyerContactInfo = string.Join(",", strs.ToArray());
			model.BuyerAddress1 = user.AddressLine1;
			model.BuyerAddress2 = user.AddressLine2;
			model.BuyerCity = user.City;
			model.BuyerState = user.State;
			model.BuyerZip = user.Zip;
			model.BuyerEmail = user.Username;
			model.InsuranceManagerEmail = manager.User.Username;
		}

		public void UpdateInsuranceCompany(InsuranceCompanyViewModel model)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			ePIRepository.Entry(model.ModelToEntity()).State = EntityState.Modified;
			ePIRepository.Save();
		}

		public void UpdateInsuranceCompanyUser(InsuranceCompanyUserViewModel model)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			PCInsuranceCompanyManager isActive = QueryableExtensions.Include<PCInsuranceCompanyManager, User>(ePIRepository.PCInsuranceCompanyManagers, (PCInsuranceCompanyManager x) => x.User).SingleOrDefault<PCInsuranceCompanyManager>((PCInsuranceCompanyManager x) => x.PCInsuranceCompanyManagerId == model.InsuranceCompanyUserId);
			if (isActive != null)
			{
				if (model.IsActive)
				{
					this.DeactivateCurrentActiveManager(ePIRepository, model.InsuranceCompanyId, model.Email);
				}
				isActive.IsActive = model.IsActive;
				isActive.User.IsActive = model.IsActive;
				isActive.User.Username = model.Email;
				isActive.User.FirstName = model.FirstName;
				isActive.User.LastName = model.LastName;
				isActive.User.WorkNumber = model.PhoneNumber;
				if (!string.IsNullOrEmpty(model.Password))
				{
					MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
					isActive.User.Password = mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
				}
				ePIRepository.Entry(isActive).State = EntityState.Modified;
				ePIRepository.Entry(isActive.User).State = EntityState.Modified;
				ePIRepository.Save();
			}
		}

		public void UpdateOrderStatus(Guid assetId, OrderStatus orderStatus, int managerId)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			IDbSet<Asset> assets = ePIRepository.Assets;
			object[] objArray = new object[] { assetId };
			Asset nullable = assets.Find(objArray);
			nullable.PCInsuranceOrderStatus = orderStatus;
			if (orderStatus == OrderStatus.Completed)
			{
				nullable.PCInsuranceDateOfOrderSubmit = new DateTime?(DateTime.Now);
				nullable.PCInsuranceCompanyUserId = new int?(managerId);
			}
			ePIRepository.Save();
		}

		public bool ValidateUser(ValidateUserModel model)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			PCInsuranceCompanyManager pCInsuranceCompanyManager = QueryableExtensions.Include<PCInsuranceCompanyManager, User>(QueryableExtensions.Include<PCInsuranceCompanyManager, PCInsuranceCompany>(ePIRepository.PCInsuranceCompanyManagers, (PCInsuranceCompanyManager x) => x.PCInsuranceCompany), (PCInsuranceCompanyManager x) => x.User).FirstOrDefault<PCInsuranceCompanyManager>((PCInsuranceCompanyManager x) => x.PCInsuranceCompanyManagerId == model.InsuranceCompanyUserId);
			byte[] numArray = mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
			return ((pCInsuranceCompanyManager.User.Username != model.Email.ToLower() ? true : !numArray.SequenceEqual<byte>(pCInsuranceCompanyManager.User.Password)) ? false : true);
		}

		public bool VerifyUserType(string email)
		{
			bool flag = false;
			if (QueryableExtensions.Include<PCInsuranceCompanyManager, User>(this._factory.Create().PCInsuranceCompanyManagers, (PCInsuranceCompanyManager x) => x.User).FirstOrDefault<PCInsuranceCompanyManager>((PCInsuranceCompanyManager x) => (int)x.User.UserType == 13 && x.IsActive) != null)
			{
				flag = true;
			}
			return flag;
		}
	}
}