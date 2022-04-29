using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.ViewModel;
using System.Data.Entity;
using Inview.Epi.EpiFund.Domain.Entity;
using System.Text.RegularExpressions;
using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.Helpers;
using System.IO;
using System.Data.OleDb;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace Inview.Epi.EpiFund.Business
{
    public class UserManager : IUserManager
    {
        private IEPIContextFactory _factory;

        public UserManager(IEPIContextFactory factory)
        {
            _factory = factory;
        }

        public void ChangePassword(int userId, string newPassword)
        {
            var context = _factory.Create();
            context.Users.Find(userId).Password = HashPassword(newPassword);
            context.Save();
        }

        public int GetUserIdByUsername(string username)
        {
            var context = _factory.Create();
            return context.Users.First(s => s.Username == username).UserId;
        }

        public byte[] HashPassword(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            return md5.ComputeHash(Encoding.UTF8.GetBytes(password));

        }

        public bool LoginWithHash(string username, string password)
        {
            var context = _factory.Create();
            var mdas = context.AssetUserMDAs.Include(w => w.Asset);
            var user = context.Users.FirstOrDefault(s => s.Username == username);
            //var user = context.Users.FirstOrDefault(s => s.Username == username && s.IsActive);
            if (user != null)
            {
                string hash = string.Join("", user.Password.Select(b => string.Format("{0:X2}", b)).ToArray());
                if (string.Compare(hash, password, true) == 0)
                    return true;
            }
            return false;
        }

        public bool Login(string username, byte[] password)
        {
            var context = _factory.Create();
            //return context.Users.FirstOrDefault(s => s.Username.ToLower() == username.ToLower() && s.IsActive && s.Password == password) != null;
            return context.Users.FirstOrDefault(s => s.Username.ToLower() == username.ToLower() && s.Password == password) != null;
        }

        public bool Login(string username, string password)
        {
            return Login(username, HashPassword(password));
        }

        public void DeleteUser(int UserId)
        {
            var context = _factory.Create();
            var user = context.Users.First(s => s.UserId == UserId);
            user.IsActive = false;
            context.Save();
        }

        public void UpdateUser(UserModel model)
        {
            var context = _factory.Create();
            var user = context.Users.First(s => s.UserId == model.UserId);
            user.AcronymForCorporateEntity = model.AcroynmForCorporateEntity;
            user.CorporateTitle = model.CorporateTitle;
            user.AddressLine1 = model.AddressLine1;
            user.AddressLine2 = model.AddressLine2;
            user.AlternateEmail = model.AlternateEmail;
            user.CellNumber = model.CellNumber;
            user.City = model.City;
            user.FaxNumber = model.FaxNumber;
            user.FirstName = model.FirstName;
            user.HomeNumber = model.HomeNumber;
            user.IsActive = model.IsActive;
            user.LastName = model.LastName;
            user.ManagingOfficerName = model.ManagingOfficerName;
            user.EIN = model.EIN;
            StringBuilder sb = new StringBuilder();
            if (model.SelectedPreferredMethods != null)
            {
                model.SelectedPreferredMethods.ForEach(f => { sb.Append(f); sb.Append(";"); });
            }
            user.PreferredMethods = sb.ToString();
            sb = new StringBuilder();
            if (model.SelectedPreferredContactTime != null)
            {
                model.SelectedPreferredContactTime.ForEach(f => { sb.Append(f); sb.Append(";"); });
            }
            user.PreferredContactTimes = sb.ToString();

            user.State = model.SelectedState;
            user.Username = model.Username;
            user.WorkNumber = model.WorkNumber;
            user.Zip = model.Zip;
            user.CommercialOfficeInterest = model.CommercialOfficeInterest;
            user.CommercialOtherInterest = model.CommercialOtherInterest;
            user.CommercialRetailInterest = model.CommercialRetailInterest;
            user.MultiFamilyInterest = model.MultiFamilyInterest;
            user.MHPInterest = model.MHPInterest;
            user.SecuredPaperInterest = model.SecuredPaperInterest;
            user.CompanyName = model.CompanyName;
            user.CorporateTIN = model.CorporateTIN;
            user.StateLicenseDesc = model.LicenseDesc;
            user.StateLicenseNumber = model.LicenseNumber;
            user.HasSellerPrivilege = model.HasSellerPrivilege;
            user.LicenseStateIsHeld = model.StateLicenseHeld;


            if (model.ICStatus == ICStatus.Rejected)
            {
                //change user's type
                user.ICStatus = null;
                user.UserType = model.SelectedUserType;
                user.IsRejectedICAdmin = true;
            }
            context.Save();
        }

        /*public void CreateUser(RegistrationModel model)
        {
            var context = _factory.Create();
            var user = new User();
            user.AcronymForCorporateEntity = model.AcroynmForCorporateEntity;
            user.CorporateTitle = model.CorporateTitle;
            user.CommercialOfficeInterest = model.CommercialOfficeInterest;
            user.CommercialOtherInterest = model.CommercialOtherInterest;
            user.CommercialRetailInterest = model.CommercialRetailInterest;
            user.MultiFamilyInterest = model.MultiFamilyInterest;
            user.MHPInterest = model.MHPInterest;
            user.SecuredPaperInterest = model.SecuredPaperInterest;
            user.AddressLine1 = model.AddressLine1;
            user.AddressLine2 = model.AddressLine2;
            user.AlternateEmail = model.AlternateEmail;
            user.CellNumber = model.CellNumber;
            user.City = model.City;
            user.FaxNumber = model.FaxNumber;
            user.FirstName = model.FirstName;
            user.HomeNumber = model.HomeNumber;
            user.IsActive = true;
            user.LastName = model.LastName;
            user.ManagingOfficerName = model.ManagingOfficerName;
            user.State = model.SelectedState;
            user.Username = model.Username.ToLower();
            user.WorkNumber = model.WorkNumber;
            user.Zip = model.Zip;
            user.Password = HashPassword(model.Password);
            user.UserType = model.SelectedUserType;
            user.CorporateTitle = model.CorporateTitle;
            user.StateOfOriginCorporateEntity = model.StateOfOriginCorporateEntity;
            user.IsCertificateOfGoodStandingAvailable = model.IsCertificateOfGoodStandingAvailable.GetValueOrDefault(false);
            user.CorporateEntityType = model.SelectedCorporateEntityType;
            user.CompanyName = model.CompanyName;
            user.SignupDate = DateTime.Now;
            user.EIN = model.EIN;
            user.CorporateTIN = model.CorporateTIN;
            user.LicenseStateIsHeld = model.StateLicenseHeld;
            // i am using State[LD/LN]
            user.StateLicenseDesc = model.LicenseDesc;
            user.StateLicenseNumber = model.LicenseNumber;
            StringBuilder sb = new StringBuilder();
            if (model.SelectedPreferredMethods != null)
            {
                model.SelectedPreferredMethods.ForEach(f => { sb.Append(f); sb.Append(";"); });
            }
            user.PreferredMethods = sb.ToString();
            sb = new StringBuilder();
            if (model.SelectedPreferredContactTime != null)
            {
                model.SelectedPreferredContactTime.ForEach(f => { sb.Append(f); sb.Append(";"); });
            }
            user.PreferredContactTimes = sb.ToString();

            var mba = context.MbaUsers.FirstOrDefault(s => s.Email == user.Username);
            if (mba != null)
            {
                user.MbaUserId = mba.MBAUserId;
            }
            if (user.UserType == UserType.ICAdmin)
            {
                user.ICStatus = ICStatus.Pending;
            }
            else
            {
                user.ICStatus = null;
            }
            if (!string.IsNullOrEmpty(model.ReferralId))
            {
                // user supposedly registered through referral link
                var referral = context.UserReferrals.Where(x => x.ReferralEmail == model.Username.ToLower()).FirstOrDefault();
                if (referral != null && referral.ReferralCode == model.ReferralId)
                {
                    user.ReferredByUserId = referral.UserId;
                    user.ReferralStatus = UserRegisteredReferralStatus.ReferralLink;
                    referral.Registered = true;
                    context.Entry(referral).State = EntityState.Modified;
                }
                else if (referral != null)
                {
                    // user used link but link was tampered/incorrect
                    user.ReferredByUserId = referral.UserId;
                    user.ReferralStatus = UserRegisteredReferralStatus.LinkButTableAfter;
                    referral.Registered = true;
                    context.Entry(referral).State = EntityState.Modified;
                }
            }
            else
            {
                // no referral link, check table anyway
                var referral = context.UserReferrals.Where(x => x.ReferralEmail == model.Username.ToLower()).FirstOrDefault();
                if (referral != null)
                {
                    user.ReferredByUserId = referral.UserId;
                    user.ReferralStatus = UserRegisteredReferralStatus.ThroughUserReferralTable;
                    referral.Registered = true;
                    context.Entry(referral).State = EntityState.Modified;
                }
            }

            // default is true -- if they register, they should be able to sell
            user.HasSellerPrivilege = true;

            context.Users.Add(user);
            context.Save();

            if (user.UserType == UserType.ListingAgent)
            {
                // see if the user is an imported nar member and flag if so
                var nar = context.NarMembers.Where(x => x.Email == user.Username.ToLower()).FirstOrDefault();
                if (nar != null)
                {
                    //TODO: we should probably do any referred by work here
                    nar.Registered = true;
                }
                else
                {
                    // create the nar member and set them to registered
                    var member = new NARMember();
                    member.CellPhoneNumber = user.CellNumber;
                    member.CompanyAddressLine1 = user.AddressLine1;
                    member.CompanyAddressLine2 = user.AddressLine2;
                    member.CompanyCity = user.City;
                    member.CompanyName = user.CompanyName;
                    member.CompanyState = user.State;
                    member.CompanyZip = user.Zip;
                    member.Email = user.Username;
                    member.FaxNumber = user.FaxNumber;
                    member.FirstName = user.FirstName;
                    member.IsActive = true;
                    member.LastName = user.LastName;
                    member.Registered = true;
                    member.WorkPhoneNumber = user.WorkNumber;
                    member.ReferredByUserId = user.UserId; // this field isnt really used at the moment
                    context.NarMembers.Add(member);
                }
                context.Save();
            }
        }
        */

        public UserModel GetUserByUsername(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                var context = _factory.Create();
                var user = context.Users.First(s => s.Username == username.ToLower());
                return GetUserById(user.UserId);
            }
            return null;
        }

        public bool UserExists(string username)
        {
            var context = _factory.Create();
            return context.Users.Count(s => s.Username == username) > 0;
        }

        public int CreateUser(string username, byte[] password, UserType userType)
        {
            var context = _factory.Create();
            var user = new User()
            {
                Username = username,
                Password = password,
                UserType = userType,
                IsActive = true
            };
            context.Users.Add(user);
            context.Save();
            return user.UserId;
        }

        public bool IsUserDisabled(string username)
        {
            var context = _factory.Create();
            var user = context.Users.FirstOrDefault(w => w.Username == username);
            if (user != null)
            {
                return !user.IsActive;
            }
            return true;
        }

        public string ResetPassword(string username)
        {
            var context = _factory.Create();
            var user = context.Users.First(s => s.Username == username);
            var pw = randomPassword();
            user.Password = HashPassword(pw);
            context.Save();
            return pw;
        }

        private string randomPassword(int length = 8)
        {
            var g = Guid.NewGuid();
            var s = Convert.ToBase64String(g.ToByteArray());
            s = Regex.Replace(s, "[^a-zA-Z0-9]", string.Empty);
            return s.Substring(0, length);
        }

        public List<UserQuickViewModel> GetUsers(UserSearchModel model)
        {
            var context = _factory.Create();
            var list = context.Users.ToList();
            try
            {
                // this method is called everywhere. we need some unit testing to make sure other calls work when changes like this happens
                if (model.ExcludedUsers != null)
                {
                    foreach (var exclude in model.ExcludedUsers)
                    {
                        list = list.Where(x => x.UserType != exclude).ToList();
                    }
                }
            }
            catch { }
            try
            {
                if (model.UserTypeFilters != null)
                {
                    var tempList = new List<User>();
                    foreach (var include in model.UserTypeFilters)
                    {
                        //list = list.Where(x => x.UserType == include).ToList();
                        tempList.AddRange(list.Where(x => x.UserType == include).ToList());
                    }
                    list = tempList.Distinct().ToList();
                }
            }
            catch { }
            if (model.ShowActiveOnly)
            {
                list = list.Where(w => w.IsActive).ToList();
            }
            if (!string.IsNullOrEmpty(model.FirstName))
            {
                list = list.Where(w => w.FirstName != null && w.FirstName.ToLower().Contains(model.FirstName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.LastName))
            {
                list = list.Where(w => w.LastName != null && w.LastName.ToLower().Contains(model.LastName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.Email))
            {
                list = list.Where(w => w.Username != null && w.Username.ToLower().Contains(model.Email.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.AddressLine1))
            {
                list = list.Where(w => w.AddressLine1 != null && w.AddressLine1.ToLower().Contains(model.AddressLine1.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.City))
            {
                list = list.Where(w => w.City != null && w.City.ToLower().Contains(model.City.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.State))
            {
                list = list.Where(w => w.State != null && w.State.ToLower().Contains(model.State.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.CompanyName))
            {
                list = list.Where(w => w.CompanyName == model.CompanyName).ToList();
            }
            if (model.UserTypeFilter.HasValue)
            {
                list = list.Where(w => w.UserType == model.UserTypeFilter.Value).ToList();
            }
            if (model.CorpAdminId.HasValue)
            {
                list = list.Where(w => w.CorpAdminId.HasValue && w.CorpAdminId.Value == model.CorpAdminId.Value).ToList();
            }

            var returnlist = new List<UserQuickViewModel>();
            list.ForEach(f =>
            {
                var newModel = new UserQuickViewModel();
                newModel.City = f.City;
                newModel.FirstName = f.FirstName;
                newModel.LastName = f.LastName;
                newModel.Email = f.Username;
                newModel.AddressLine1 = f.AddressLine1;
                newModel.State = f.State;
                newModel.UserId = f.UserId;
                newModel.UserType = f.UserType;
                newModel.UserTypeDescription = EnumHelper.GetEnumDescription(f.UserType);
                newModel.Username = f.Username;
                newModel.CompanyName = f.CompanyName;
                newModel.NumberOfSearchCriteria = context.AssetSearchCriterias.Count(s => s.UserId.HasValue && s.UserId.Value == f.UserId);
                newModel.HasSearchCriteria = newModel.NumberOfSearchCriteria > 0;
                newModel.NumberOfSignedMDAs = context.AssetUserMDAs.Count(w => w.UserId == f.UserId);
                newModel.ReferredByJVMA = f.MbaUserId.HasValue;
                newModel.TIN = f.CorporateTIN;
                newModel.LicenseType = f.StateLicenseDesc;
                newModel.OperatingLicenseNumber = f.StateLicenseNumber;
                newModel.IsActive = f.IsActive;
                if (f.MbaUserId.HasValue)
                {
                    newModel.ReferredByUserId = context.MbaUsers.First(s => s.MBAUserId == f.MbaUserId.Value).ReferredByUserId;
                }
                newModel.RegisterDate = f.SignupDate != null ? f.SignupDate.GetValueOrDefault(DateTime.Now) : f.NCNDSignDate.GetValueOrDefault(DateTime.Now);
                newModel.InEscrow = context.Assets.Any(w => w.ProposedBuyerContact == f.FullName);
                newModel.HasPOFCLA = false;
                newModel.HasNCND = f.NCNDSignDate.HasValue;
                newModel.ICStatus = f.ICStatus;
                StringBuilder sb = new StringBuilder();
                if (f.CommercialOfficeInterest)
                {
                    sb.Append("CRE: O<br/>");
                }
                if (f.CommercialOtherInterest)
                {
                    sb.Append("CRE: M<br/>");
                }
                if (f.CommercialRetailInterest)
                {
                    sb.Append("CRE: R<br/>");
                }
                if (f.MHPInterest)
                {
                    sb.Append("MHP<br/>");
                }
                if (f.MultiFamilyInterest)
                {
                    sb.Append("MF<br/>");
                }
                if (f.SecuredPaperInterest)
                {
                    if (sb.ToString() == "CRE: O<br/>CRE: M<br/>CRE: R<br/>MHP<br/>MF<br/>")
                    {
                        sb = new StringBuilder("All");
                    }
                    else
                    {
                        sb.Append("Paper");
                    }
                }
                newModel.UserTypeString = GetUserTypeAcronym(f.UserType);
                newModel.AssetsOfInterest = sb.ToString();
                newModel.IsASeller = context.Assets.Count(w => w.ListedByUserId == f.UserId) > 0;
                newModel.StateLicenseIsHeld = f.LicenseStateIsHeld;
                newModel.DoesPIHaveSellerPrivilege = f.HasSellerPrivilege;
                if (f.UserType == UserType.ICAdmin)
                {
                    newModel.PendingAssets = getPendingAssetCountForICAdmin(f.UserId);
                    newModel.TotalNewAssets = getTotalAssetCountForICAdmin(f.UserId);
                    newModel.Last30Assets = getLast30DayAssetCountForICAdmin(f.UserId);
                }
                returnlist.Add(newModel);
            });
            return returnlist;
        }

        public UserModel GetUserById(int userId)
        {
            var context = _factory.Create();
            var user = context.Users.Include(s => s.UserNotes).Include(s => s.UserFiles).FirstOrDefault(s => s.UserId == userId);
            if (user != null)
            {
                var model = new UserModel()
                {
                    AcroynmForCorporateEntity = user.AcronymForCorporateEntity,
                    CorporateTitle = user.CorporateTitle,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ManagingOfficerName = user.ManagingOfficerName,
                    AddressLine1 = user.AddressLine1,
                    AddressLine2 = user.AddressLine2,
                    AlternateEmail = user.AlternateEmail,
                    CellNumber = user.CellNumber,
                    City = user.City,
                    FaxNumber = user.FaxNumber,
                    HomeNumber = user.HomeNumber,
                    IsActive = user.IsActive,
                    SelectedState = user.State,
                    UserId = user.UserId,
                    Username = user.Username,
                    UserType = user.UserType,
                    WorkNumber = user.WorkNumber,
                    CommercialOfficeInterest = user.CommercialOfficeInterest,
                    CommercialOtherInterest = user.CommercialOtherInterest,
                    CommercialRetailInterest = user.CommercialRetailInterest,
                    MultiFamilyInterest = user.MultiFamilyInterest,
                    MHPInterest = user.MHPInterest,
                    SecuredPaperInterest = user.SecuredPaperInterest,
                    IsCertificateOfGoodStandingAvailable = user.IsCertificateOfGoodStandingAvailable,
                    StateOfOriginCorporateEntity = user.StateOfOriginCorporateEntity,
                    SelectedCorporateEntityType = user.CorporateEntityType,
                    CompanyName = user.CompanyName,
                    Zip = user.Zip,
                    SignedNCND = user.NCNDSignDate.HasValue,
                    ICStatus = user.ICStatus,
                    SignedICAgreement = !string.IsNullOrEmpty(user.ICFileLocation),
                    NCNDLocation = user.NCNDFileLocation,
                    // i am using State[LD/LN]
                    LicenseDesc = user.StateLicenseDesc,
                    LicenseNumber = user.StateLicenseNumber,
                    CorporateTIN = user.CorporateTIN,
                    SelectedPreferredContactTimesString = user.PreferredContactTimes,
                    SelectedPreferredMethodsString = user.PreferredMethods,
                    JVMarketerAgreementLocation = user.JVMarketerAgreementLocation,
                    EIN = user.EIN,
                    MBAUserId = user.MbaUserId,
                    HasSellerPrivilege = user.HasSellerPrivilege,
                    StateLicenseHeld = user.LicenseStateIsHeld,
                    IsASeller = context.Assets.Any(a => a.ListedByUserId == user.UserId)
                };
                model.SelectedPreferredMethods = new List<PreferredMethod>();
                if (user.PreferredMethods != null)
                {
                    var splits = user.PreferredMethods.Split(';');
                    foreach (var split in splits)
                    {
                        if (!string.IsNullOrEmpty(split))
                        {
                            model.SelectedPreferredMethods.Add((PreferredMethod)Enum.Parse(typeof(PreferredMethod), split));
                        }
                    }
                }
                model.SelectedPreferredContactTime = new List<PreferredContactTime>();
                if (user.PreferredContactTimes != null)
                {
                    var splits = user.PreferredContactTimes.Split(';');
                    foreach (var split in splits)
                    {
                        if (!string.IsNullOrEmpty(split))
                        {
                            model.SelectedPreferredContactTime.Add((PreferredContactTime)Enum.Parse(typeof(PreferredContactTime), split));
                        }
                    }
                }
                model.UserFiles = new List<UserFileModel>();
                model.UserNotes = new List<UserNoteModel>();
                user.UserFiles.ForEach(f =>
                {
                    model.UserFiles.Add(new UserFileModel()
                    {
                        Description = f.FileName,
                        UserFileId = f.UserFileId,
                        Location = f.FileLocation,
                        DateUploaded = f.DateUploaded.ToShortDateString()
                    });
                });
                user.UserNotes.ForEach(f =>
                {
                    model.UserNotes.Add(new UserNoteModel()
                    {
                        Date = f.CreateDate,
                        Note = f.Notes,
                        UserNoteId = f.UserNoteId,

                    });
                });
                return model;
            }
            return null;
        }


        public List<TitleQuickViewModel> GetTitles(CompanySearchModel model)
        {
            var context = _factory.Create();
            var list = context.TitleCompanies.ToList();
            //if (model.ShowActiveOnly)
            //{
            //    list = list.Where(w => w.IsActive).ToList();
            //}
            if (!string.IsNullOrEmpty(model.CompanyName))
            {
                list = list.Where(w => w.TitleCompName != null && w.TitleCompName.ToLower().Contains(model.CompanyName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.CompanyURL))
            {
                list = list.Where(w => w.TitleCompURL != null && w.TitleCompURL.ToLower().Contains(model.CompanyURL.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.State))
            {
                list = list.Where(w => w.IncludedStates.ToLower().Contains(model.State.ToLower())).ToList();
            }
            if (model.NeedsManager)
            {
                foreach (var tc in context.TitleCompanies.ToList())
                {
                    var titleCompanyManager = context.TitleCompanyUsers.Where(x => x.IsManager && x.IsActive && x.TitleCompanyId == tc.TitleCompanyId).FirstOrDefault();
                    if (titleCompanyManager == null)
                    {
                        try
                        {
                            list.Remove(tc);
                        }
                        catch { }
                    }
                }
            }
            var returnlist = new List<TitleQuickViewModel>();
            list.ForEach(f =>
            {
                var newModel = new TitleQuickViewModel();
                if (f.IncludedStates.Contains("ALL"))
                    f.IncludedStates = "All;";
                newModel.TitleCompanyId = f.TitleCompanyId;
                newModel.TitleCompName = f.TitleCompName;
                newModel.TitleCompURL = f.TitleCompURL;
                newModel.IsActive = f.IsActive;
                newModel.State = f.IncludedStates;
                newModel.CreatedOn = f.CreatedOn;
                newModel.showInActive = false;
                returnlist.Add(newModel);
            });
            return returnlist;
        }

        public List<TitleUserQuickViewModel> GetTitleUsers(TitleUserSearchModel model)
        {
            var context = _factory.Create();
            var list = context.TitleCompanyUsers.ToList();
            //if (model.ShowActiveOnly)
            //{
            //    list = list.Where(w => w.IsActive).ToList();
            //}
            if (model.TitleCompanyId != null)
            {
                list = list.Where(w => w.TitleCompanyId == Convert.ToInt32(model.TitleCompanyId)).ToList();
            }
            if (!string.IsNullOrEmpty(model.FirstName))
            {
                list = list.Where(w => w.FirstName != null && w.FirstName.ToLower().Contains(model.FirstName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.LastName))
            {
                list = list.Where(w => w.LastName != null && w.LastName.ToLower().Contains(model.LastName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.Email))
            {
                list = list.Where(w => w.Email.ToLower().Contains(model.Email.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.PhoneNumber))
            {
                list = list.Where(w => w.PhoneNumber.ToLower().Contains(model.PhoneNumber.ToLower())).ToList();
            }
            var returnlist = new List<TitleUserQuickViewModel>();
            list.ForEach(f =>
            {
                var newModel = new TitleUserQuickViewModel();

                newModel.TitleCompanyId = f.TitleCompanyId;
                newModel.Email = f.Email;
                newModel.FirstName = f.FirstName;
                newModel.IsManager = f.IsManager;
                newModel.IsActive = f.IsActive;
                newModel.LastName = f.LastName;
                newModel.ManagingOfficerName = f.ManagingOfficerName;
                newModel.Password = f.Password;
                newModel.PhoneNumber = f.PhoneNumber;
                newModel.TitleCompanyUserId = f.TitleCompanyUserId;
                newModel.OldEmail = f.Email;
                returnlist.Add(newModel);
            });
            return returnlist;
        }


        public List<AssetAssignmentQuickViewModel> GetAssignments(AssetAssignmentSearchModel model)
        {
            var context = _factory.Create();
            var list = context.Assets.ToList();

            if (model.TitleCompanyId != null)
            {
                list = list.Where(w => w.TitleCompanyId != null && w.TitleCompanyId == model.TitleCompanyId && w.OrderStatus != OrderStatus.Completed).ToList();
            }
            //if (model.TitleCompanyManagerId != null)
            //{
            //    list = list.Where(w => w.TitleCompanyUserId != null && w.TitleCompanyUserId == model.TitleCompanyManagerId).ToList();
            //}

            var returnlist = new List<AssetAssignmentQuickViewModel>();

            List<TitleCompanyUser> titleCompanyUsers = GetTitleCompUsersTitleId(model.TitleCompanyId).Where(x => x.IsActive == true).ToList();



            list.ForEach(f =>
            {
                var newModel = new AssetAssignmentQuickViewModel();
                newModel.TitleCompanyId = Convert.ToInt32(f.TitleCompanyId);  // 1;
                newModel.AssetNumber = f.AssetNumber; // 1006;
                newModel.Status = f.OrderStatus.ToString();//OrderStatus.Pending.ToString();
                newModel.SelectedCompanyUserId = f.TitleCompanyUserId.ToString(); // "2";
                List<SelectListItem> companyUsers = GenerateCompanyUsers(titleCompanyUsers);
                SelectListItem current = companyUsers.FirstOrDefault(x => x.Value == newModel.SelectedCompanyUserId);
                companyUsers.Remove(current); current.Selected = true; companyUsers.Add(current);
                newModel.CompanyUsers = companyUsers;
                newModel.TitleCompanyManagerId = titleCompanyUsers.FirstOrDefault(x => x.IsManager == true).TitleCompanyUserId;//1;
                returnlist.Add(newModel);
            });
            return returnlist;
        }

        private List<SelectListItem> GenerateCompanyUsers(List<TitleCompanyUser> titleCompanyUsers)
        {


            List<SelectListItem> CompanyUsers = new List<SelectListItem>();
            foreach (TitleCompanyUser user in titleCompanyUsers)
            {
                CompanyUsers.Add(new SelectListItem() { Text = user.FullName, Value = user.TitleCompanyUserId.ToString() });
            }
            return CompanyUsers;
        }

        public bool TitleCompanyExists(string TitleCompName)
        {
            var context = _factory.Create();
            return context.TitleCompanies.Count(s => s.TitleCompName.ToLower() == TitleCompName) > 0;
        }

        public void CreateTitle(TitleCompanyModel model)
        {
            var context = _factory.Create();
            var titleCompany = new TitleCompany();
            titleCompany.CreatedOn = DateTime.Now;
            titleCompany.IsActive = true;
            titleCompany.TitleCompName = model.TitleCompName;
            titleCompany.TitleCompURL = model.TitleCompURL;
            titleCompany.TitleCompAddress = model.TitleCompAddress;
            titleCompany.TitleCompPhone = model.TitleCompPhone;
            titleCompany.CurrentRate = model.CurrentRate;
            titleCompany.TitleCompAddress2 = model.TitleCompAddress2;
            titleCompany.City = model.City;
            titleCompany.State = model.State;
            titleCompany.Zip = model.Zip;

            StringBuilder sb = new StringBuilder();
            if (model.SelectedIncludedStates != null)
            {
                if (model.SelectedIncludedStates.Contains(StatesOfUS.ALL))
                {
                    model.IncludedStates.ForEach(f => { if (f.Value.ToString().ToUpper() != "ALL") { sb.Append(f.Value); sb.Append(";"); } });
                }
                else
                {
                    model.SelectedIncludedStates.ForEach(f => { sb.Append(f); sb.Append(";"); });
                }
            }
            titleCompany.IncludedStates = sb.ToString();
            context.TitleCompanies.Add(titleCompany);
            context.Save();
        }


        public TitleCompanyModel GetTitleById(int titleId)
        {
            var context = _factory.Create();
            var titleCompany = context.TitleCompanies.FirstOrDefault(s => s.TitleCompanyId == titleId);
            if (titleCompany != null)
            {
                var model = new TitleCompanyModel()
                {
                    TitleCompanyId = titleCompany.TitleCompanyId,
                    TitleCompName = titleCompany.TitleCompName,
                    TitleCompURL = titleCompany.TitleCompURL,
                    TitleCompAddress = titleCompany.TitleCompAddress,
                    TitleCompPhone = titleCompany.TitleCompPhone,
                    CurrentRate = titleCompany.CurrentRate,
                    TitleCompAddress2 = titleCompany.TitleCompAddress2,
                    City = titleCompany.City,
                    State = titleCompany.State,
                    Zip = titleCompany.Zip
                };
                model.SelectedIncludedStates = new List<StatesOfUS>();
                if (titleCompany.IncludedStates != null)
                {
                    var splits = titleCompany.IncludedStates.Split(';');
                    foreach (var split in splits)
                    {
                        if (!string.IsNullOrEmpty(split))
                        {
                            model.SelectedIncludedStates.Add((StatesOfUS)Enum.Parse(typeof(StatesOfUS), split));
                        }
                    }
                }
                model.IsActive = titleCompany.IsActive;
                model.CreatedOn = titleCompany.CreatedOn;

                return model;
            }
            return null;
        }

        public TitleCompanyUserModel GetTitleUserById(int titleUserId)
        {
            var context = _factory.Create();
            var titleUser = context.TitleCompanyUsers.FirstOrDefault(s => s.TitleCompanyUserId == titleUserId);
            var titleCompany = GetTitleById(titleUser.TitleCompanyId);
            if (titleUser != null)
            {
                var model = new TitleCompanyUserModel(titleCompany)
                {
                    TitleCompanyId = titleUser.TitleCompanyId,
                    Email = titleUser.Email,
                    OldEmail = titleUser.Email,
                    FirstName = titleUser.FirstName,
                    IsActive = titleUser.IsActive,
                    IsManager = titleUser.IsManager,
                    LastName = titleUser.LastName,
                    ManagingOfficerName = titleUser.ManagingOfficerName,
                    Password = GetString(titleUser.Password),
                    PhoneNumber = titleUser.PhoneNumber,
                    TitleCompanyUserId = titleUser.TitleCompanyUserId
                };

                if (!string.IsNullOrEmpty(titleUser.AssignedStates))
                {
                    var splits = titleUser.AssignedStates.Split(';');
                    foreach (var split in splits)
                    {
                        if (!string.IsNullOrEmpty(split))
                        {
                            model.SelectedStates.Add((StatesOfUS)Enum.Parse(typeof(StatesOfUS), split));
                        }
                    }
                }

                return model;
            }
            return null;
        }

        public InsuranceCompanyUserViewModel GetInsuranceUserByEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var context = _factory.Create();
                var iUser = context.PCInsuranceCompanyManagers.Include(x => x.PCInsuranceCompany).Include(x => x.User).FirstOrDefault(x => x.User.Username == email.ToLower());
                if (iUser != null)
                {
                    var model = new InsuranceCompanyUserViewModel(iUser);
                    model.Company.EntityToModel(iUser.PCInsuranceCompany);
                    return model;
                }
            }
            return null;
        }

        public TitleCompanyUserModel GetTitleUserByEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var context = _factory.Create();
                var titleUser = context.TitleCompanyUsers.FirstOrDefault(s => s.Email == email);
                var titleCompany = GetTitleById(titleUser.TitleCompanyId);
                if (titleUser != null)
                {
                    var model = new TitleCompanyUserModel(titleCompany)
                    {
                        TitleCompanyId = titleUser.TitleCompanyId,
                        Email = titleUser.Email,
                        OldEmail = titleUser.Email,
                        FirstName = titleUser.FirstName,
                        IsActive = titleUser.IsActive,
                        IsManager = titleUser.IsManager,
                        LastName = titleUser.LastName,
                        ManagingOfficerName = titleUser.ManagingOfficerName,
                        Password = GetString(titleUser.Password),
                        PhoneNumber = titleUser.PhoneNumber,
                        TitleCompanyUserId = titleUser.TitleCompanyUserId
                    };

                    if (!string.IsNullOrEmpty(titleUser.AssignedStates))
                    {
                        var splits = titleUser.AssignedStates.Split(';');
                        foreach (var split in splits)
                        {
                            if (!string.IsNullOrEmpty(split))
                            {
                                model.SelectedStates.Add((StatesOfUS)Enum.Parse(typeof(StatesOfUS), split));
                            }
                        }
                    }

                    return model;
                }
            }
            return null;
        }


        public void UpdateTitle(TitleCompanyModel model)
        {
            var context = _factory.Create();
            var titleCompany = context.TitleCompanies.First(s => s.TitleCompanyId == model.TitleCompanyId);
            titleCompany.TitleCompName = model.TitleCompName;
            titleCompany.TitleCompURL = model.TitleCompURL;
            //titleCompany.IsActive = model.IsActive;
            titleCompany.TitleCompPhone = model.TitleCompPhone;
            titleCompany.TitleCompAddress = model.TitleCompAddress;
            titleCompany.CurrentRate = model.CurrentRate;
            titleCompany.TitleCompAddress2 = model.TitleCompAddress2;
            titleCompany.City = model.City;
            titleCompany.State = model.State;
            titleCompany.Zip = model.Zip;
            StringBuilder sb = new StringBuilder();
            if (model.SelectedIncludedStates != null)
            {
                if (model.SelectedIncludedStates.Contains(StatesOfUS.ALL))
                {
                    model.IncludedStates.ForEach(f => { sb.Append(f.Value); sb.Append(";"); });
                }
                else
                {
                    model.SelectedIncludedStates.ForEach(f => { sb.Append(f); sb.Append(";"); });
                }
            }
            titleCompany.IncludedStates = sb.ToString();

            context.Save();
        }

        public void DeactivateTitleCompany(int titleId)
        {
            var context = _factory.Create();
            var titleCompany = context.TitleCompanies.First(s => s.TitleCompanyId == titleId);
            titleCompany.IsActive = false;
            context.Save();
        }

        public void ActivateTitleCompany(int titleId)
        {
            var context = _factory.Create();
            var titleCompany = context.TitleCompanies.First(s => s.TitleCompanyId == titleId);
            titleCompany.IsActive = true;
            context.Save();
        }


        public List<TitleUserQuickViewModel> GetTitleCompanyUsers(TitleUserSearchModel searchModel)
        {
            var context = _factory.Create();
            var titleCompanyUsers = GetTitleUsers(searchModel);
            var userFiles = context.UserFiles;
            var assets = context.Assets;
            var pastDate = DateTime.Now.AddDays(-30);
            titleCompanyUsers.ToList().ForEach(u =>
            {
                u.TotalItemCount = titleCompanyUsers.Count();
                // u.showInActive = false;
            });

            return titleCompanyUsers;
        }

        public List<TitleCompanyUser> GetTitleCompanyUsers()
        {
            var context = _factory.Create();
            var titleCompanyUsers = context.TitleCompanyUsers.ToList();

            return titleCompanyUsers;
        }

        public List<TitleCompanyUser> GetTitleCompUsersTitleId(int TitleId)
        {
            var context = _factory.Create();
            var titleCompanyUsers = context.TitleCompanyUsers.Where(x => x.TitleCompanyId == TitleId).ToList();

            return titleCompanyUsers;
        }

        public bool TitleCompanyUserExists(string UserEmail)
        {
            var context = _factory.Create();
            return context.TitleCompanyUsers.Count(s => s.Email.ToLower() == UserEmail) > 0;
        }


        public int CreateTitleUser(TitleCompanyUserModel model)
        {
            if (string.IsNullOrEmpty(model.Password))
            {
                // if no password provided, generate one automatically
                model.Password = GeneratePassword();
            }
            var context = _factory.Create();
            var titleUser = new TitleCompanyUser();
            titleUser.Email = model.Email;
            titleUser.FirstName = model.FirstName;
            titleUser.IsActive = model.IsActive;
            titleUser.IsManager = model.IsManager;
            titleUser.LastName = model.LastName;
            titleUser.ManagingOfficerName = model.ManagingOfficerName;
            titleUser.Password = GetBytes(model.Password);
            titleUser.PhoneNumber = model.PhoneNumber;
            titleUser.TitleCompanyId = model.TitleCompanyId;

            StringBuilder sb = new StringBuilder();

            if (model.SelectedStates != null)
            {
                if (model.SelectedStates.Contains(StatesOfUS.ALL))
                {
                    model.States.ToList().ForEach(f => { sb.Append(f.Value); sb.Append(";"); });
                }
                else
                {
                    model.SelectedStates.ForEach(f => { sb.Append(f); sb.Append(";"); });
                }
            }
            titleUser.AssignedStates = sb.ToString();

            context.TitleCompanyUsers.Add(titleUser);
            context.Save();


            var user = new User();
            user.CellNumber = model.PhoneNumber;
            user.FirstName = model.FirstName;
            user.ManagingOfficerName = model.ManagingOfficerName;
            user.IsActive = true;
            user.LastName = model.LastName;
            user.Username = model.Email.ToLower();
            user.Password = HashPassword(model.Password);
            user.SignupDate = DateTime.Now;
            if (model.IsManager)
                user.UserType = UserType.TitleCompManager;
            else
                user.UserType = UserType.TitleCompUser;


            user.AcronymForCorporateEntity = null;
            user.CorporateTitle = null;
            user.CommercialOfficeInterest = false;
            user.CommercialOtherInterest = false;
            user.CommercialRetailInterest = false;
            user.MultiFamilyInterest = false;
            user.MHPInterest = false;
            user.SecuredPaperInterest = false;
            user.IsCertificateOfGoodStandingAvailable = false;
            //user.CorporateEntityType = null;
            context.Users.Add(user);
            context.Save();


            return context.TitleCompanyUsers.Where(x => x.Email == model.Email).Select(x => x.TitleCompanyUserId).ToList().Last();
        }

        public void UpdateTitleUser(TitleCompanyUserModel model)
        {
            var context = _factory.Create();
            var titleUser = context.TitleCompanyUsers.First(s => s.TitleCompanyUserId == model.TitleCompanyUserId);
            titleUser.Email = model.Email;
            titleUser.FirstName = model.FirstName;
            titleUser.IsActive = model.IsActive;
            titleUser.IsManager = model.IsManager;
            titleUser.LastName = model.LastName;
            titleUser.ManagingOfficerName = model.ManagingOfficerName;
            if (model.Password != null)
                titleUser.Password = GetBytes(model.Password);
            titleUser.PhoneNumber = model.PhoneNumber;
            titleUser.TitleCompanyId = model.TitleCompanyId;
            StringBuilder sb = new StringBuilder();

            if (model.SelectedStates != null)
            {
                if (model.SelectedStates.Contains(StatesOfUS.ALL))
                {
                    model.States.ToList().ForEach(f => { sb.Append(f.Value); sb.Append(";"); });
                }
                else
                {
                    model.SelectedStates.ForEach(f => { sb.Append(f); sb.Append(";"); });
                }
            }
            titleUser.AssignedStates = sb.ToString();

            // update user record too
            var User = context.Users.First(s => s.Username == model.OldEmail);
            User.FirstName = model.FirstName;
            User.LastName = model.LastName;
            User.Username = model.Email;
            User.ManagingOfficerName = model.ManagingOfficerName;
            if (model.Password != null)
                User.Password = HashPassword(model.Password);
            User.HomeNumber = model.PhoneNumber;
            if (model.IsManager)
                User.UserType = UserType.TitleCompManager;
            else
                User.UserType = UserType.TitleCompUser;

            context.Save();
        }

        public void DeactivateTitleUser(int userId)
        {
            var context = _factory.Create();
            var titleUser = context.TitleCompanyUsers.First(s => s.TitleCompanyUserId == userId);
            titleUser.IsActive = false;

            //var titleUserUser = context.Users.Where(x => x.Username == titleUser.Email).FirstOrDefault();
            //if (titleUserUser != null)
            //{
            //    titleUserUser.IsActive = false;
            //}

            context.Save();
        }

        public void ActivateTitleUser(int userId)
        {
            var context = _factory.Create();
            var titleUser = context.TitleCompanyUsers.First(s => s.TitleCompanyUserId == userId);
            titleUser.IsActive = true;

            //var titleUserUser = context.Users.Where(x => x.Username == titleUser.Email).FirstOrDefault();
            //if (titleUserUser != null)
            //{
            //    titleUserUser.IsActive = true;
            //}

            context.Save();
        }


        public List<AssetAssignmentQuickViewModel> GetAssetsAssigned(AssetAssignmentSearchModel searchModel)
        {
            var context = _factory.Create();
            var titleAssignments = GetAssignments(searchModel);
            var userFiles = context.UserFiles;
            var assets = context.Assets;
            var pastDate = DateTime.Now.AddDays(-30);
            titleAssignments.ToList().ForEach(u =>
            {
                u.TotalItemCount = titleAssignments.Count();

            });

            return titleAssignments;
        }

        public void UpdateAssignment(int assetNumber, int selectedUserId)
        {
            var context = _factory.Create();
            var asset = context.Assets.First(s => s.AssetNumber == assetNumber);
            asset.TitleCompanyUserId = selectedUserId;
            asset.OrderStatus = OrderStatus.Pending;
            context.Save();
        }

        public void UpdateOrderStatus(int assetNumber, OrderStatus orderstatus, int titleCompanyUserId)
        {
            var context = _factory.Create();
            var asset = context.Assets.First(s => s.AssetNumber == assetNumber);
            asset.OrderStatus = orderstatus;
            if (orderstatus == OrderStatus.Completed)
            {
                asset.DateOfOrderSubmit = DateTime.Now;
                asset.TitleCompanyUserId = titleCompanyUserId;
            }
            context.Save();
        }

        public Tuple<bool, string> ValidateTitleUserStateAvailability(TitleCompanyUserModel model)
        {
            var context = _factory.Create();
            string selectedStates = string.Empty;
            if (model.SelectedStates != null)
            {
                model.SelectedStates.ForEach(f => { selectedStates += f; });
            }
            bool clear = true;
            StringBuilder errors = new StringBuilder();
            var titleCoUsers = context.TitleCompanyUsers.Where(x => x.TitleCompanyId == model.TitleCompanyId &&
                x.IsActive &&
                !x.IsManager &&
                x.TitleCompanyUserId != model.TitleCompanyUserId).ToList();
            foreach (var user in titleCoUsers)
            {
                if (!string.IsNullOrEmpty(user.AssignedStates))
                {
                    var splits = user.AssignedStates.Split(';');
                    foreach (var split in splits)
                    {
                        if (!string.IsNullOrEmpty(split))
                        {
                            if (selectedStates.Contains(split))
                            {
                                errors.Append(string.Format("The user {0} has already been assigned to {1}. ", user.FullName, split));
                                clear = false;
                            }
                        }
                    }
                }
            }
            if (clear)
            {
                return new Tuple<bool, string>(true, "");
            }
            else
            {
                return new Tuple<bool, string>(false, errors.ToString());
            }
        }

        public byte[] GetSignedNCND(int userId)
        {
            var context = _factory.Create();
            var user = context.Users.Single(w => w.UserId == userId);
            if (user.NCNDSignDate.HasValue)
            {
                if (File.Exists(user.NCNDFileLocation))
                {
                    return File.ReadAllBytes(user.NCNDFileLocation);
                }
            }
            return null;
        }

        public byte[] GetSignedICAgreement(int userId)
        {
            var context = _factory.Create();
            var user = context.Users.Single(w => w.UserId == userId);
            if (!string.IsNullOrEmpty(user.ICFileLocation))
            {
                if (File.Exists(user.ICFileLocation))
                {
                    return File.ReadAllBytes(user.ICFileLocation);
                }
            }
            return null;
        }

        public string GeneratePassword()
        {
            return randomPassword();
        }

        public List<AssetSearchCriteriaQuickViewModel> GetSearchesForUser(int userId)
        {
            var context = _factory.Create();
            var searches = context.AssetSearchCriterias.Where(w => w.UserId == userId).OrderByDescending(s => s.DateEntered).ToList();
            var list = new List<AssetSearchCriteriaQuickViewModel>();
            searches.ForEach(f =>
            {
                list.Add(new AssetSearchCriteriaQuickViewModel()
                {
                    DateCreated = f.DateEntered,
                    SearchCriteriaId = f.AssetSearchCriteriaId
                });
            });
            return list;
        }

        public bool HasPendingNCND(int userId)
        {
            var context = _factory.Create();
            return context.DocusignEnvelopes.Count(w => w.UserId == userId && w.DocumentType == DocumentType.NCND && w.DateReceived == null) > 0;
        }

        public bool HasPendingICAgreement(int userId)
        {
            var context = _factory.Create();
            return context.DocusignEnvelopes.Count(w => w.UserId == userId && w.DocumentType == DocumentType.ICAgreement && w.DateReceived == null) > 0;
        }

        public bool HasPendingJVAgreement(int userId)
        {
            var context = _factory.Create();
            return context.DocusignEnvelopes.Count(w => w.UserId == userId && w.DocumentType == DocumentType.JVAgreement && w.DateReceived == null) > 0;
        }

        public bool HasPendingMDA(int userId, int assetNumber)
        {
            var context = _factory.Create();
            var mdas = context.DocusignEnvelopes.Where(w => w.UserId == userId && w.DocumentType == DocumentType.MDA && w.DateReceived == null).ToList();
            foreach (var mda in mdas)
            {
                var list = mda.AssetNumbers.Split('&');
                foreach (var item in list)
                {
                    if (item != "")
                    {
                        if (item == assetNumber.ToString())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void CreatePersonalFinancialStatement(PersonalFinancialStatementTemplateModel model)
        {
            var context = _factory.Create();
            var record = new PersonalFinancialStatement()
            {
                ExecutionDate = DateTime.Now,
                NetInvestmentIncome = model.NetInvestmentIncome,
                AssetAccountsReceivable = model.AccountsRecievable,
                AssetAutomobiles = model.Automobiles,
                AssetCashOnHand = model.CashOnHand,
                AssetIRA = model.OtherRetirementOrIraAccount,
                AssetLifeInsurance = model.LifeInsurance,
                AssetOtherAssets = model.OtherAssets,
                AssetOtherPersonalProperty = model.OtherPropertyAssets,
                AssetRealEstate = model.RealEstate,
                AssetSavingsAccount = model.SavingsAccount,
                AssetStocks = model.StocksAndBonds,
                ContingentAsEndorser = model.EndorserOrCoMaker,
                ContingentLegalClaims = model.LegalClaimsOrJudgments,
                ContingentOtherSpecialDebt = model.OtherSpecialDebt,
                DescriptionOfOtherIncome1 = model.OtherIncomeDescription1,
                DescriptionOfOtherIncome2 = model.OtherIncomeDescription2,
                DescriptionOfOtherIncome3 = model.OtherIncomeDescription3,
                LiabilityAccountsPayable = model.AccountsPayable,
                LiabilityInstallmentAccountAuto = model.InstallmentAccountAuto,
                LiabilityInstallmentAccountOther = model.InstallmentAccountOther,
                LiabilityLoanOnLifeInsurance = model.LifeInsuranceLoan,
                LiabilityMonthlyPayment = model.InstallmentAccountAutoMonthlyPayment,
                LiabilityMonthlyPayments = model.InstallmentAccountOtherMonthlyPayment,
                LiabilityMortgagesOnRealEstate = model.RealEstateMortgage,
                LiabilityNetWorth = model.NetWorth,
                LiabilityNotesPayable = model.NotesPayableToOthers,
                LiabilityOtherLiabilities = model.LiabilitiesOtherLiabilities,
                LiabilityTotalLiabilities = model.LiabilitiesTotalLiabilities,
                LiabilityUnpaidTaxes = model.LiabilitiesUnpaidTaxes,
                LifeInsuranceHeld = model.LifeInsuranceHeld,
                NotesPayableCurrentBalance = model.CurrentBalance1,
                NotesPayableCurrentBalance2 = model.CurrentBalance2,
                NotesPayableCurrentBalance3 = model.CurrentBalance3,
                NotesPayableCurrentBalance4 = model.CurrentBalance4,
                NotesPayableCurrentBalance5 = model.CurrentBalance5,
                NotesPayableFrequency = model.Frequency1,
                NotesPayableFrequency2 = model.Frequency2,
                NotesPayableFrequency3 = model.Frequency3,
                NotesPayableFrequency4 = model.Frequency4,
                NotesPayableFrequency5 = model.Frequency5,
                NotesPayableName = model.NameAndAddressOfNoteholders1,
                NotesPayableName2 = model.NameAndAddressOfNoteholders2,
                NotesPayableName3 = model.NameAndAddressOfNoteholders3,
                NotesPayableName4 = model.NameAndAddressOfNoteholders4,
                NotesPayableName5 = model.NameAndAddressOfNoteholders5,
                NotesPayableOriginalBalance = model.OriginalBalance1,
                NotesPayableOriginalBalance2 = model.OriginalBalance2,
                NotesPayableOriginalBalance3 = model.OriginalBalance3,
                NotesPayableOriginalBalance4 = model.OriginalBalance4,
                NotesPayableOriginalBalance5 = model.OriginalBalance5,
                OtherIncome = model.OtherIncome,
                OtherLiabilities = model.OtherLiabilities,
                OtherPersonalPropertyAndOtherAssets = model.OtherProperty,
                RealEstateIncome = model.RealEstateIncome,
                UnpaidTaxes = model.UnpaidTaxes,
                StocksTotalValue4 = model.StocksAndBondsTotalValue4,
                StocksTotalValue3 = model.StocksAndBondsTotalValue3,
                StocksTotalValue2 = model.StocksAndBondsTotalValue2,
                StocksTotalValue = model.StocksAndBondsTotalValue1,
                StocksNumberOfShares4 = model.StocksAndBondsNumberOfShares4,
                StocksNumberOfShares3 = model.StocksAndBondsNumberOfShares3,
                StocksNumberOfShares2 = model.StocksAndBondsNumberOfShares2,
                StocksNumberOfShares = model.StocksAndBondsNumberOfShares1,
                StocksNameOfSecurities4 = model.StocksAndBondsNameOfSecurities4,
                StocksNameOfSecurities3 = model.StocksAndBondsNameOfSecurities3,
                StocksNameOfSecurities2 = model.StocksAndBondsNameOfSecurities2,
                StocksNameOfSecurities = model.StocksAndBondsNameOfSecurities1,
                StocksMarketValue4 = model.StocksAndBondsMarketValue4,
                StocksMarketValue3 = model.StocksAndBondsMarketValue3,
                StocksMarketValue2 = model.StocksAndBondsMarketValue2,
                StocksMarketValue = model.StocksAndBondsMarketValue1,
                StocksDateOfQuotation4 = model.StocksAndBondsDateOfExchange4,
                StocksDateOfQuotation3 = model.StocksAndBondsDateOfExchange3,
                StocksDateOfQuotation2 = model.StocksAndBondsDateOfExchange2,
                StocksDateOfQuotation = model.StocksAndBondsDateOfExchange1,
                StocksCost4 = model.StocksAndBondsCost4,
                StocksCost3 = model.StocksAndBondsCost3,
                StocksCost2 = model.StocksAndBondsCost2,
                StocksCost = model.StocksAndBondsCost1,
                SocialSecurityNumber = model.SocialSecurityNumber,
                Salary = model.Salary,
                RealEstateOwnedTypeOfRealEstatePropertyC = model.PropertyCTypeOfRealEstate,
                RealEstateOwnedAddressPropertyC = model.PropertyCAddress,
                RealEstateOwnedAmountOfPaymentPerMonthPropertyC = model.PropertyCAmountOfPaymentRecurring,
                RealEstateOwnedDatePurchasedPropertyC = model.PropertyCDatePurchased,
                RealEstateOwnedMortgageAccountNumberPropertyC = model.PropertyCMortgageAccountNumber,
                RealEstateOwnedMortgageBalancePropertyC = model.PropertyCMortgageBalance,
                RealEstateOwnedNameAndAddressPropertyC = model.PropertyCNameAndAddressOfMortgageHolder,
                RealEstateOwnedPresentMarketValuePropertyC = model.PropertyCPresentMarketValue,
                RealEstateOwnedStatusOfMortgagePropertyC = model.PropertyCMortgageStatus,
                RealEstateOwnedTypeOfRealEstatePropertyB = model.PropertyBTypeOfRealEstate,
                RealEstateOwnedAddressPropertyB = model.PropertyBAddress,
                RealEstateOwnedAmountOfPaymentPerMonthPropertyB = model.PropertyBAmountOfPaymentRecurring,
                RealEstateOwnedDatePurchasedPropertyB = model.PropertyBDatePurchased,
                RealEstateOwnedMortgageAccountNumberPropertyB = model.PropertyBMortgageAccountNumber,
                RealEstateOwnedMortgageBalancePropertyB = model.PropertyBMortgageBalance,
                RealEstateOwnedNameAndAddressPropertyB = model.PropertyBNameAndAddressOfMortgageHolder,
                RealEstateOwnedPresentMarketValuePropertyB = model.PropertyBPresentMarketValue,
                RealEstateOwnedStatusOfMortgagePropertyB = model.PropertyBMortgageStatus,
                RealEstateOwnedTypeOfRealEstatePropertyA = model.PropertyATypeOfRealEstate,
                RealEstateOwnedAddressPropertyA = model.PropertyAAddress,
                RealEstateOwnedAmountOfPaymentPerMonthPropertyA = model.PropertyAAmountOfPaymentRecurring,
                RealEstateOwnedDatePurchasedPropertyA = model.PropertyADatePurchased,
                RealEstateOwnedMortgageAccountNumberPropertyA = model.PropertyAMortgageAccountNumber,
                RealEstateOwnedMortgageBalancePropertyA = model.PropertyAMortgageBalance,
                RealEstateOwnedNameAndAddressPropertyA = model.PropertyANameAndAddressOfMortgageHolder,
                RealEstateOwnedPresentMarketValuePropertyA = model.PropertyAPresentMarketValue,
                RealEstateOwnedStatusOfMortgagePropertyA = model.PropertyAMortgageStatus,
                ContingentProvisions = model.FederalIncomeTax,
                NotesPayableSecuredOrEndorsed = model.TypeOfCollateral1,
                NotesPayableSecuredOrEndorsed2 = model.TypeOfCollateral2,
                NotesPayableSecuredOrEndorsed3 = model.TypeOfCollateral3,
                NotesPayableSecuredOrEndorsed4 = model.TypeOfCollateral4,
                NotesPayableSecuredOrEndorsed5 = model.TypeOfCollateral5,
                UserId = model.UserId
            };
            context.PersonalFinancialStatements.Add(record);
            context.Save();
        }

        public PersonalFinancialStatementTemplateModel GetPersonalFinancialStatementByUserId(int userId)
        {
            var context = _factory.Create();
            var record = context.PersonalFinancialStatements.OrderByDescending(w => w.ExecutionDate).FirstOrDefault(s => s.UserId == userId);
            var user = context.Users.Single(s => s.UserId == userId);
            if (record != null)
            {
                string date1 = string.Empty;
                string date2 = string.Empty;
                string date3 = string.Empty;
                DateTime date = new DateTime();
                if (!string.IsNullOrEmpty(record.RealEstateOwnedDatePurchasedPropertyA))
                {
                    if (DateTime.TryParse(record.RealEstateOwnedDatePurchasedPropertyA, out date)) date1 = Convert.ToDateTime(record.RealEstateOwnedDatePurchasedPropertyA).ToString("MM/dd/yyyy");
                }
                if (!string.IsNullOrEmpty(record.RealEstateOwnedDatePurchasedPropertyB))
                {
                    if (DateTime.TryParse(record.RealEstateOwnedDatePurchasedPropertyB, out date)) date2 = Convert.ToDateTime(record.RealEstateOwnedDatePurchasedPropertyB).ToString("MM/dd/yyyy");
                }
                if (!string.IsNullOrEmpty(record.RealEstateOwnedDatePurchasedPropertyC))
                {
                    if (DateTime.TryParse(record.RealEstateOwnedDatePurchasedPropertyC, out date)) date3 = Convert.ToDateTime(record.RealEstateOwnedDatePurchasedPropertyC).ToString("MM/dd/yyyy");
                }

                return new PersonalFinancialStatementTemplateModel()
                {
                    AccountsPayable = record.LiabilityAccountsPayable,
                    AccountsRecievable = record.AssetAccountsReceivable,
                    Automobiles = record.AssetAutomobiles,
                    BusinessName = user.CompanyName,
                    BusinessPhone = user.WorkNumber,
                    CashOnHand = record.AssetCashOnHand,
                    City = user.City,
                    CurrentBalance1 = record.NotesPayableCurrentBalance,
                    CurrentBalance2 = record.NotesPayableCurrentBalance2,
                    CurrentBalance3 = record.NotesPayableCurrentBalance3,
                    CurrentBalance4 = record.NotesPayableCurrentBalance4,
                    CurrentBalance5 = record.NotesPayableCurrentBalance5,
                    Day = record.ExecutionDate.Day,
                    Email = user.Username,
                    EndorserOrCoMaker = record.ContingentAsEndorser,
                    FederalIncomeTax = record.ContingentProvisions,
                    Frequency1 = record.NotesPayableFrequency,
                    Frequency2 = record.NotesPayableFrequency2,
                    Frequency3 = record.NotesPayableFrequency3,
                    Frequency4 = record.NotesPayableFrequency4,
                    Frequency5 = record.NotesPayableFrequency5,
                    InstallmentAccountAuto = record.LiabilityInstallmentAccountAuto,
                    InstallmentAccountAutoMonthlyPayment = record.LiabilityMonthlyPayment,
                    InstallmentAccountOther = record.LiabilityInstallmentAccountOther,
                    InstallmentAccountOtherMonthlyPayment = record.LiabilityMonthlyPayments,
                    LegalClaimsOrJudgments = record.ContingentLegalClaims,
                    LiabilitiesOtherLiabilities = record.LiabilityOtherLiabilities,
                    LiabilitiesTotal = record.LiabilityTotalLiabilities,
                    OtherRetirementOrIraAccount = record.AssetIRA,
                    LiabilitiesTotalLiabilities = record.LiabilityTotalLiabilities,
                    LiabilitiesUnpaidTaxes = record.LiabilityUnpaidTaxes,
                    LifeInsurance = record.AssetLifeInsurance,
                    RealEstate = record.AssetRealEstate,//these fields
                    OtherAssets = record.AssetOtherAssets,
                    RealEstateIncome = record.RealEstateIncome,//are immensely confusing
                    RealEstateMortgage = record.LiabilityMortgagesOnRealEstate,
                    PropertyAAmountOfPaymentRecurring = record.RealEstateOwnedAmountOfPaymentPerMonthPropertyA,
                    PropertyBAmountOfPaymentRecurring = record.RealEstateOwnedAmountOfPaymentPerMonthPropertyB,
                    PropertyCAmountOfPaymentRecurring = record.RealEstateOwnedAmountOfPaymentPerMonthPropertyC,
                    LifeInsuranceHeld = record.LifeInsuranceHeld,
                    LifeInsuranceLoan = record.LiabilityLoanOnLifeInsurance,
                    Month = (new DateTime(2000, record.ExecutionDate.Month, 1)).ToString("MMMM"),
                    NameAndAddressOfNoteholders1 = record.NotesPayableName,
                    NameAndAddressOfNoteholders2 = record.NotesPayableName2,
                    NameAndAddressOfNoteholders3 = record.NotesPayableName3,
                    NameAndAddressOfNoteholders4 = record.NotesPayableName4,
                    NameAndAddressOfNoteholders5 = record.NotesPayableName5,
                    NetWorth = record.LiabilityNetWorth,
                    NotesPayableToOthers = record.LiabilityNotesPayable,
                    OtherIncomeDescription1 = record.DescriptionOfOtherIncome1,
                    OtherIncomeDescription2 = record.DescriptionOfOtherIncome2,
                    OtherIncomeDescription3 = record.DescriptionOfOtherIncome3,
                    OriginalBalance1 = record.NotesPayableOriginalBalance,
                    OriginalBalance2 = record.NotesPayableOriginalBalance2,
                    OriginalBalance3 = record.NotesPayableOriginalBalance3,
                    OriginalBalance4 = record.NotesPayableOriginalBalance4,
                    OriginalBalance5 = record.NotesPayableOriginalBalance5,
                    OtherIncome = record.OtherIncome,
                    OtherLiabilities = record.OtherLiabilities,
                    OtherProperty = record.OtherPersonalPropertyAndOtherAssets,
                    OtherPropertyAssets = record.AssetOtherPersonalProperty,
                    OtherSpecialDebt = record.ContingentOtherSpecialDebt,
                    PersonalFinancialStatementId = record.PersonalFinancialStatementId,
                    PropertyAAddress = record.RealEstateOwnedAddressPropertyA,
                    PropertyBAddress = record.RealEstateOwnedAddressPropertyB,
                    PropertyCAddress = record.RealEstateOwnedAddressPropertyB,
                    PropertyADatePurchased = date1,
                    PropertyBDatePurchased = date2,
                    PropertyCDatePurchased = date3,
                    PropertyAMortgageAccountNumber = record.RealEstateOwnedMortgageAccountNumberPropertyA,
                    PropertyBMortgageAccountNumber = record.RealEstateOwnedMortgageAccountNumberPropertyB,
                    PropertyCMortgageAccountNumber = record.RealEstateOwnedMortgageAccountNumberPropertyC,
                    PropertyAMortgageBalance = record.RealEstateOwnedMortgageBalancePropertyA,
                    PropertyBMortgageBalance = record.RealEstateOwnedMortgageBalancePropertyB,
                    PropertyCMortgageBalance = record.RealEstateOwnedMortgageBalancePropertyC,
                    PropertyAMortgageStatus = record.RealEstateOwnedStatusOfMortgagePropertyA,
                    PropertyBMortgageStatus = record.RealEstateOwnedStatusOfMortgagePropertyB,
                    PropertyCMortgageStatus = record.RealEstateOwnedStatusOfMortgagePropertyC,
                    PropertyANameAndAddressOfMortgageHolder = record.RealEstateOwnedNameAndAddressPropertyA,
                    PropertyBNameAndAddressOfMortgageHolder = record.RealEstateOwnedNameAndAddressPropertyB,
                    PropertyCNameAndAddressOfMortgageHolder = record.RealEstateOwnedNameAndAddressPropertyC,
                    PropertyAPresentMarketValue = record.RealEstateOwnedPresentMarketValuePropertyA,
                    PropertyBPresentMarketValue = record.RealEstateOwnedPresentMarketValuePropertyB,
                    PropertyCPresentMarketValue = record.RealEstateOwnedPresentMarketValuePropertyC,
                    PropertyATypeOfRealEstate = record.RealEstateOwnedTypeOfRealEstatePropertyA,
                    PropertyBTypeOfRealEstate = record.RealEstateOwnedTypeOfRealEstatePropertyB,
                    PropertyCTypeOfRealEstate = record.RealEstateOwnedTypeOfRealEstatePropertyC,
                    ResidentialAddress = user.AddressLine1,
                    StocksAndBondsCost1 = record.StocksCost,
                    StocksAndBondsCost2 = record.StocksCost2,
                    StocksAndBondsCost3 = record.StocksCost3,
                    StocksAndBondsCost4 = record.StocksCost4,
                    StocksAndBondsDateOfExchange1 = record.StocksDateOfQuotation,
                    StocksAndBondsDateOfExchange2 = record.StocksDateOfQuotation2,
                    StocksAndBondsDateOfExchange3 = record.StocksDateOfQuotation3,
                    StocksAndBondsDateOfExchange4 = record.StocksDateOfQuotation4,
                    ResidentialPhone = user.CellNumber,
                    Salary = record.Salary,
                    SavingsAccount = record.AssetSavingsAccount,
                    Zip = user.Zip,
                    SocialSecurityNumber = record.SocialSecurityNumber,
                    UserLastName = user.LastName,
                    UserFirstName = user.FirstName,
                    UserId = userId,
                    UnpaidTaxes = record.UnpaidTaxes,
                    TypeOfCollateral5 = record.NotesPayableSecuredOrEndorsed5,
                    TypeOfCollateral4 = record.NotesPayableSecuredOrEndorsed4,
                    TypeOfCollateral3 = record.NotesPayableSecuredOrEndorsed3,
                    TypeOfCollateral2 = record.NotesPayableSecuredOrEndorsed2,
                    TypeOfCollateral1 = record.NotesPayableSecuredOrEndorsed,
                    StocksAndBondsTotalValue4 = record.StocksTotalValue4,
                    StocksAndBondsTotalValue3 = record.StocksTotalValue3,
                    StocksAndBondsTotalValue2 = record.StocksTotalValue2,
                    StocksAndBondsTotalValue1 = record.StocksTotalValue,
                    StocksAndBondsNumberOfShares4 = record.StocksNumberOfShares4,
                    StocksAndBondsNumberOfShares3 = record.StocksNumberOfShares3,
                    StocksAndBondsNumberOfShares2 = record.StocksNumberOfShares2,
                    StocksAndBondsNumberOfShares1 = record.StocksNumberOfShares,
                    StocksAndBondsNameOfSecurities4 = record.StocksNameOfSecurities4,
                    StocksAndBondsNameOfSecurities2 = record.StocksNameOfSecurities2,
                    StocksAndBondsNameOfSecurities3 = record.StocksNameOfSecurities3,
                    StocksAndBondsNameOfSecurities1 = record.StocksNameOfSecurities,
                    StocksAndBondsMarketValue4 = record.StocksMarketValue4,
                    StocksAndBondsMarketValue3 = record.StocksMarketValue3,
                    StocksAndBondsMarketValue2 = record.StocksMarketValue2,
                    StocksAndBondsMarketValue1 = record.StocksMarketValue,
                    Year = record.ExecutionDate.Year,
                    StocksAndBonds = record.AssetStocks,
                    PaymentAmount1 = record.NotesPayablePaymentAmount,
                    PaymentAmount2 = record.NotesPayablePaymentAmount2,
                    PaymentAmount3 = record.NotesPayablePaymentAmount3,
                    PaymentAmount4 = record.NotesPayablePaymentAmount4,
                    PaymentAmount5 = record.NotesPayablePaymentAmount5,
                    PropertyAOriginalCost = record.RealEstateOwnedOriginalCostA,
                    PropertyBOriginalCost = record.RealEstateOwnedOriginalCostB,
                    PropertyCOriginalCost = record.RealEstateOwnedOriginalCostC,
                    NetInvestmentIncome = record.NetInvestmentIncome,
                    State = user.State
                };
            }
            return null;
        }


        public void UpdatePersonalFinancialStatement(PersonalFinancialStatementTemplateModel model)
        {
            var context = _factory.Create();
            var record = context.PersonalFinancialStatements.Single(s => s.PersonalFinancialStatementId == model.PersonalFinancialStatementId);
            record.AssetAccountsReceivable = model.AccountsRecievable;
            record.AssetAutomobiles = model.Automobiles;
            record.AssetCashOnHand = model.CashOnHand;
            record.AssetIRA = model.OtherRetirementOrIraAccount;
            record.AssetLifeInsurance = model.LifeInsurance;
            record.AssetOtherAssets = model.OtherAssets;
            record.AssetOtherPersonalProperty = model.OtherPropertyAssets;
            record.AssetRealEstate = model.RealEstate;
            record.AssetSavingsAccount = model.SavingsAccount;
            record.AssetStocks = model.StocksAndBonds;
            record.ContingentAsEndorser = model.EndorserOrCoMaker;
            record.ContingentLegalClaims = model.LegalClaimsOrJudgments;
            record.ContingentOtherSpecialDebt = model.OtherSpecialDebt;
            record.DescriptionOfOtherIncome1 = model.OtherIncomeDescription1;
            record.DescriptionOfOtherIncome2 = model.OtherIncomeDescription2;
            record.DescriptionOfOtherIncome3 = model.OtherIncomeDescription3;
            record.LiabilityAccountsPayable = model.AccountsPayable;
            record.LiabilityInstallmentAccountAuto = model.InstallmentAccountAuto;
            record.LiabilityInstallmentAccountOther = model.InstallmentAccountOther;
            record.LiabilityLoanOnLifeInsurance = model.LifeInsuranceLoan;
            record.LiabilityMonthlyPayment = model.InstallmentAccountAutoMonthlyPayment;
            record.LiabilityMonthlyPayments = model.InstallmentAccountOtherMonthlyPayment;
            record.LiabilityMortgagesOnRealEstate = model.RealEstateMortgage;
            record.LiabilityNetWorth = model.NetWorth;
            record.LiabilityNotesPayable = model.NotesPayableToOthers;
            record.LiabilityOtherLiabilities = model.LiabilitiesOtherLiabilities;
            record.LiabilityTotalLiabilities = model.LiabilitiesTotalLiabilities;
            record.LiabilityUnpaidTaxes = model.LiabilitiesUnpaidTaxes;
            record.LifeInsuranceHeld = model.LifeInsuranceHeld;
            record.NotesPayableCurrentBalance = model.CurrentBalance1;
            record.NotesPayableCurrentBalance2 = model.CurrentBalance2;
            record.NotesPayableCurrentBalance3 = model.CurrentBalance3;
            record.NotesPayableCurrentBalance4 = model.CurrentBalance4;
            record.NotesPayableCurrentBalance5 = model.CurrentBalance5;
            record.NotesPayablePaymentAmount = model.PaymentAmount1;
            record.NotesPayablePaymentAmount2 = model.PaymentAmount1;
            record.NotesPayablePaymentAmount3 = model.PaymentAmount1;
            record.NotesPayablePaymentAmount4 = model.PaymentAmount1;
            record.NotesPayablePaymentAmount5 = model.PaymentAmount1;
            record.NotesPayableFrequency = model.Frequency1;
            record.NotesPayableFrequency2 = model.Frequency2;
            record.NotesPayableFrequency3 = model.Frequency3;
            record.NotesPayableFrequency4 = model.Frequency4;
            record.NotesPayableFrequency5 = model.Frequency5;
            record.NotesPayableName = model.NameAndAddressOfNoteholders1;
            record.NotesPayableName2 = model.NameAndAddressOfNoteholders2;
            record.NotesPayableName3 = model.NameAndAddressOfNoteholders3;
            record.NotesPayableName4 = model.NameAndAddressOfNoteholders4;
            record.NotesPayableName5 = model.NameAndAddressOfNoteholders5;
            record.NotesPayableOriginalBalance = model.OriginalBalance1;
            record.NotesPayableOriginalBalance2 = model.OriginalBalance2;
            record.NotesPayableOriginalBalance3 = model.OriginalBalance3;
            record.NotesPayableOriginalBalance4 = model.OriginalBalance4;
            record.NotesPayableOriginalBalance5 = model.OriginalBalance5;
            record.OtherIncome = model.OtherIncome;
            record.OtherLiabilities = model.OtherLiabilities;
            record.OtherPersonalPropertyAndOtherAssets = model.OtherProperty;
            record.RealEstateIncome = model.RealEstateIncome;
            record.UnpaidTaxes = model.UnpaidTaxes;
            record.StocksTotalValue4 = model.StocksAndBondsTotalValue4;
            record.StocksTotalValue3 = model.StocksAndBondsTotalValue3;
            record.StocksTotalValue2 = model.StocksAndBondsTotalValue2;
            record.StocksTotalValue = model.StocksAndBondsTotalValue1;
            record.StocksNumberOfShares4 = model.StocksAndBondsNumberOfShares4;
            record.StocksNumberOfShares3 = model.StocksAndBondsNumberOfShares3;
            record.StocksNumberOfShares2 = model.StocksAndBondsNumberOfShares2;
            record.StocksNumberOfShares = model.StocksAndBondsNumberOfShares1;
            record.StocksNameOfSecurities4 = model.StocksAndBondsNameOfSecurities4;
            record.StocksNameOfSecurities3 = model.StocksAndBondsNameOfSecurities3;
            record.StocksNameOfSecurities2 = model.StocksAndBondsNameOfSecurities2;
            record.StocksNameOfSecurities = model.StocksAndBondsNameOfSecurities1;
            record.StocksMarketValue4 = model.StocksAndBondsMarketValue4;
            record.StocksMarketValue3 = model.StocksAndBondsMarketValue3;
            record.StocksMarketValue2 = model.StocksAndBondsMarketValue2;
            record.StocksMarketValue = model.StocksAndBondsMarketValue1;
            record.StocksDateOfQuotation4 = model.StocksAndBondsDateOfExchange4;
            record.StocksDateOfQuotation3 = model.StocksAndBondsDateOfExchange3;
            record.StocksDateOfQuotation2 = model.StocksAndBondsDateOfExchange2;
            record.StocksDateOfQuotation = model.StocksAndBondsDateOfExchange1;
            record.StocksCost4 = model.StocksAndBondsCost4;
            record.StocksCost3 = model.StocksAndBondsCost3;
            record.StocksCost2 = model.StocksAndBondsCost2;
            record.StocksCost = model.StocksAndBondsCost1;
            record.SocialSecurityNumber = model.SocialSecurityNumber;
            record.Salary = model.Salary;
            record.RealEstateOwnedTypeOfRealEstatePropertyC = model.PropertyCTypeOfRealEstate;
            record.RealEstateOwnedAddressPropertyC = model.PropertyCAddress;
            record.RealEstateOwnedAmountOfPaymentPerMonthPropertyC = model.PropertyCAmountOfPaymentRecurring;
            record.RealEstateOwnedDatePurchasedPropertyC = model.PropertyCDatePurchased;
            record.RealEstateOwnedMortgageAccountNumberPropertyC = model.PropertyCMortgageAccountNumber;
            record.RealEstateOwnedMortgageBalancePropertyC = model.PropertyCMortgageBalance;
            record.RealEstateOwnedNameAndAddressPropertyC = model.PropertyCNameAndAddressOfMortgageHolder;
            record.RealEstateOwnedPresentMarketValuePropertyC = model.PropertyCPresentMarketValue;
            record.RealEstateOwnedStatusOfMortgagePropertyC = model.PropertyCMortgageStatus;
            record.RealEstateOwnedTypeOfRealEstatePropertyB = model.PropertyBTypeOfRealEstate;
            record.RealEstateOwnedAddressPropertyB = model.PropertyBAddress;
            record.RealEstateOwnedAmountOfPaymentPerMonthPropertyB = model.PropertyBAmountOfPaymentRecurring;
            record.RealEstateOwnedDatePurchasedPropertyB = model.PropertyBDatePurchased;
            record.RealEstateOwnedMortgageAccountNumberPropertyB = model.PropertyBMortgageAccountNumber;
            record.RealEstateOwnedMortgageBalancePropertyB = model.PropertyBMortgageBalance;
            record.RealEstateOwnedNameAndAddressPropertyB = model.PropertyBNameAndAddressOfMortgageHolder;
            record.RealEstateOwnedPresentMarketValuePropertyB = model.PropertyBPresentMarketValue;
            record.RealEstateOwnedStatusOfMortgagePropertyB = model.PropertyBMortgageStatus;
            record.RealEstateOwnedTypeOfRealEstatePropertyA = model.PropertyATypeOfRealEstate;
            record.RealEstateOwnedAddressPropertyA = model.PropertyAAddress;
            record.RealEstateOwnedAmountOfPaymentPerMonthPropertyA = model.PropertyAAmountOfPaymentRecurring;
            record.RealEstateOwnedDatePurchasedPropertyA = model.PropertyADatePurchased;
            record.RealEstateOwnedMortgageAccountNumberPropertyA = model.PropertyAMortgageAccountNumber;
            record.RealEstateOwnedMortgageBalancePropertyA = model.PropertyAMortgageBalance;
            record.RealEstateOwnedNameAndAddressPropertyA = model.PropertyANameAndAddressOfMortgageHolder;
            record.RealEstateOwnedPresentMarketValuePropertyA = model.PropertyAPresentMarketValue;
            record.RealEstateOwnedStatusOfMortgagePropertyA = model.PropertyAMortgageStatus;
            record.ContingentProvisions = model.FederalIncomeTax;
            record.NotesPayableSecuredOrEndorsed = model.TypeOfCollateral1;
            record.NotesPayableSecuredOrEndorsed2 = model.TypeOfCollateral2;
            record.NotesPayableSecuredOrEndorsed3 = model.TypeOfCollateral3;
            record.NotesPayableSecuredOrEndorsed4 = model.TypeOfCollateral4;
            record.NotesPayableSecuredOrEndorsed5 = model.TypeOfCollateral5;

            record.RealEstateOwnedOriginalCostA = model.PropertyAOriginalCost;
            record.RealEstateOwnedOriginalCostB = model.PropertyBOriginalCost;
            record.RealEstateOwnedOriginalCostC = model.PropertyCOriginalCost;
            record.NetInvestmentIncome = model.NetInvestmentIncome;
            record.UserId = model.UserId;


            context.Save();
        }
        public List<UserAssetSearchCriteriaQuickViewModel> GetAllSearches()
        {
            var context = _factory.Create();
            var list = new List<UserAssetSearchCriteriaQuickViewModel>();
            context.AssetSearchCriterias.Include(s => s.User).ToList().ForEach(f =>
            {
                if (f.UserId.HasValue)
                {
                    list.Add(new UserAssetSearchCriteriaQuickViewModel()
                    {
                        DateCreated = f.DateEntered,
                        UserFullName = f.User.FirstName + " " + f.User.LastName,
                        UserId = f.UserId.Value,
                        AssetSearchCriteriaId = f.AssetSearchCriteriaId
                    });
                }
            });

            return list;

        }

        public List<UserFileModel> GetFilesByUserId(int userId)
        {
            var context = _factory.Create();
            var files = context.UserFiles.Where(w => w.UserId == userId);
            var list = new List<UserFileModel>();
            files.ToList().ForEach(f =>
            {
                list.Add(new UserFileModel()
                {
                    Description = f.FileName,
                    Location = f.FileLocation,
                    UserFileId = f.UserFileId,
                    Type = f.Type,
                    DateUploaded = f.DateUploaded.ToString("MM-dd-yyyy")
                });
            });
            return list;
        }

        public byte[] GetUserFile(int userFileId)
        {
            var context = _factory.Create();
            var file = context.UserFiles.FirstOrDefault(w => w.UserFileId == userFileId);
            if (file != null)
            {
                if (File.Exists(file.FileLocation))
                {
                    return File.ReadAllBytes(file.FileLocation);
                }
            }
            return null;
        }

        public byte[] GetSignedJVAgreement(int userId)
        {
            var context = _factory.Create();
            var user = context.Users.Single(w => w.UserId == userId);
            if (!string.IsNullOrEmpty(user.JVMarketerAgreementLocation))
            {
                if (File.Exists(user.JVMarketerAgreementLocation))
                {
                    return File.ReadAllBytes(user.JVMarketerAgreementLocation);
                }
            }
            return null;
        }


        public void DeleteUserFile(int userFileId)
        {
            var context = _factory.Create();
            var file = context.UserFiles.FirstOrDefault(w => w.UserFileId == userFileId);
            if (file != null)
            {
                context.UserFiles.Remove(file);
                context.Save();
            }
        }


        public UserFileModel GetUserFileById(int id)
        {
            var context = _factory.Create();
            var file = context.UserFiles.FirstOrDefault(w => w.UserFileId == id);
            if (file != null)
            {
                return new UserFileModel()
                {
                    Description = file.FileName,
                    Location = file.FileLocation,
                    Type = file.Type,
                    UserFileId = file.UserFileId
                };
            }
            return null;
        }

        public string ImportNARMembers(string path, int referringUserId, bool areReferredUsers, bool checkAgainstMBAs = true, bool checkAgainstPrincipalInvestors = true)
        {
            int insertedRows = 0;
            int errorRows = 0;
            var connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + @";Extended Properties=""Excel 12.0 Xml;HDR=YES;IMEX=1""";
            var connection = new OleDbConnection(connectionString);
            connection.Open();
            int rowIndex = 1;

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM [NARMembers$]";

            var dataReader = command.ExecuteReader();
            StringBuilder sb = new StringBuilder();
            var context = _factory.Create();
            var user = context.Users.Find(referringUserId);
            while (dataReader.Read())
            {
                try
                {
                    string firstName = null;
                    string lastName = null;
                    string email = null;
                    string companyName = null;
                    string companyAddressLine1 = null;
                    string companyAddressLine2 = null;
                    string companyCity = null;
                    string companyState = null;
                    string companyZip = null;
                    string cellNumber = null;
                    string faxNumber = null;
                    string workNumber = null;
                    string website = null;
                    if (dataReader["First Name"].ToString().Length > 0)
                    {
                        firstName = dataReader["First Name"].ToString();
                    }
                    if (dataReader["Last Name"].ToString().Length > 0)
                    {
                        lastName = dataReader["Last Name"].ToString();
                    }
                    if (dataReader["Email"].ToString().Length > 0)
                    {
                        email = dataReader["Email"].ToString();
                    }
                    if (dataReader["Company Name"].ToString().Length > 0)
                    {
                        companyName = dataReader["Company Name"].ToString();
                    }
                    if (dataReader["Company Web Site"].ToString().Length > 0)
                    {
                        website = dataReader["Company Web Site"].ToString();
                    }
                    if (dataReader["Company Address Line 1"].ToString().Length > 0)
                    {
                        companyAddressLine1 = dataReader["Company Address Line 1"].ToString();
                    }
                    if (dataReader["Company Address Line 2"].ToString().Length > 0)
                    {
                        companyAddressLine2 = dataReader["Company Address Line 2"].ToString();
                    }
                    if (dataReader["Company City"].ToString().Length > 0)
                    {
                        companyCity = dataReader["Company City"].ToString();
                    }
                    if (dataReader["Company State"].ToString().Length > 0)
                    {
                        companyState = dataReader["Company State"].ToString();
                    }
                    if (dataReader["Company Zip"].ToString().Length > 0)
                    {
                        companyZip = dataReader["Company Zip"].ToString();
                    }
                    if (dataReader["Principal Direct Contact Phone Number"].ToString().Length > 0)
                    {
                        cellNumber = dataReader["Principal Direct Contact Phone Number"].ToString();
                    }
                    if (dataReader["Company General Fax Number"].ToString().Length > 0)
                    {
                        faxNumber = dataReader["Company General Fax Number"].ToString();
                    }
                    if (dataReader["Company General Phone Number"].ToString().Length > 0)
                    {
                        workNumber = dataReader["Company General Phone Number"].ToString();
                    }
                    if (!string.IsNullOrEmpty(email))
                    {
                        bool error = false;
                        var narMember = context.NarMembers.FirstOrDefault(s => s.Email.ToLower() == email.ToLower());
                        if (narMember != null)
                        {
                            error = true;
                            errorRows++;
                            sb.Append(string.Format("Row {0}, {1} {2} - NAR Member already in system.<br/>", rowIndex, firstName, lastName));
                        }
                        if (checkAgainstMBAs)
                        {
                            var mbaMember = context.MbaUsers.FirstOrDefault(s => s.Email.ToLower() == email.ToLower());
                            if (mbaMember != null)
                            {
                                error = true;
                                errorRows++;
                                sb.Append(string.Format("Row {0}, {1} {2} - User is already a MBA in system.<br/>", rowIndex, firstName, lastName));
                            }
                        }
                        if (checkAgainstPrincipalInvestors)
                        {
                            var investors = context.PrincipalInvestors.FirstOrDefault(s => s.Email.ToLower() == email.ToLower());
                            if (investors != null)
                            {
                                error = true;
                                errorRows++;
                                sb.Append(string.Format("Row {0}, {1} {2} - User is already a Principal Investor in system.<br/>", rowIndex, firstName, lastName));
                            }
                        }
                        if (!error)
                        {
                            context.NarMembers.Add(new NARMember()
                            {
                                CompanyAddressLine1 = companyAddressLine1,
                                CompanyAddressLine2 = companyAddressLine2,
                                CompanyCity = companyCity,
                                CompanyName = companyName,
                                CompanyState = companyState,
                                CompanyZip = companyZip,
                                Email = email.ToLower(),
                                FirstName = firstName,
                                LastName = lastName,
                                CellPhoneNumber = cellNumber,
                                WorkPhoneNumber = workNumber,
                                FaxNumber = faxNumber,
                                ReferredByUserId = referringUserId,
                                IsActive = true,
                                Website = website,
                                Registered = false
                            });
                            if (areReferredUsers)
                            {
                                try
                                {
                                    ReferUser(context, firstName, lastName, companyCity, companyState, email, user);
                                }
                                catch { }
                            }
                            context.Save();
                            insertedRows++;
                        }
                    }
                    else
                    {
                        errorRows++;
                        sb.Append(string.Format("Row {0} - Missing required email address.<br/>", rowIndex));
                    }
                }
                catch (Exception ex)
                {
                    errorRows++;
                    sb.Append(ex.Message + Environment.NewLine);
                }
                rowIndex++;
            }
            dataReader.Close();
            connection.Close();
            //context.Save();
            return string.Format("File successfully processed. {0} imported; {1} errors<br/>{2}", insertedRows, errorRows, sb.ToString());
        }

        public NarMemberViewModel GetAssetNarMember(Guid id)
        {
            var context = _factory.Create();
            NarMemberViewModel model = null;
            var member = context.AssetNARMembers.Where(x => x.AssetNARMemberId == id)
                .Include(x => x.Asset)
                .Include(x => x.NARMember)
                .ToList();
            if (member.Count > 0)
            {
                var anmGroup = member.GroupBy(x => x.NarMemberId).First();
                var anm = anmGroup.First();
                model = new NarMemberViewModel()
                {
                    AssetNumbers = string.Join(" ", anmGroup.OrderBy(x => x.Asset.AssetNumber).Select(x => x.Asset.AssetNumber)),
                    CellPhoneNumber = anm.NARMember.CellPhoneNumber,
                    FaxNumber = anm.NARMember.FaxNumber,
                    WorkNumber = anm.NARMember.WorkPhoneNumber,
                    AddressLine1 = anm.NARMember.CompanyAddressLine1,
                    AddressLine2 = anm.NARMember.CompanyAddressLine2,
                    City = anm.NARMember.CompanyCity,
                    CompanyName = anm.NARMember.CompanyName,
                    Email = anm.NARMember.Email,
                    FirstName = anm.NARMember.FirstName,
                    IsActive = anm.NARMember.IsActive,
                    LastName = anm.NARMember.LastName,
                    NarMemberId = anm.NARMember.NarMemberId,
                    State = anm.NARMember.CompanyState,
                    Zip = anm.NARMember.CompanyZip,
                    CommissionShareAgr = anm.NARMember.CommissionShareAgr,
                    CommissionAmount = anm.NARMember.CommissionAmount,
                    DateOfCsaConfirm = anm.NARMember.DateOfCsaConfirm,
                    AssetId = anm.AssetId,
                    AssetNARMemberId = anm.AssetNARMemberId,
                    ReferredByUserId = anm.NARMember.ReferredByUserId,
                    Website = anm.NARMember.Website,
                    Registered = anm.NARMember.Registered
                };
            }
            return model;
        }

        public List<NarMemberViewModel> GetAssetNarMembers(NarMemberSearchModel model)
        {
            var list = new List<NarMemberViewModel>();
            var context = _factory.Create();
            var members = new List<NarMemberViewModel>();

            // using this method for grouping now. its a HACK because we dont have the appropriate FKeys and ListingAgentName should be
            // a proper NARMemberId
            //var tempMembers1 = context.AssetListingAgents
            //    .Where(x => x.ListingAgentNewName != null && x.ListingAgentNewName != "0")
            //    .Join(context.Assets,
            //    la => la.AssetId,
            //    asset => asset.AssetId,
            //    (la, asset) => new NarMemberLAViewModel()
            //    {
            //        AssetNumber = asset.AssetNumber,
            //        ListingAgentCellNumber = la.ListingAgentCellNumber,
            //        ListingAgentFaxNumber = la.ListingAgentFaxNumber,
            //        ListingAgentWorkNumber = la.ListingAgentWorkNumber,
            //        ListingAgentCorpAddress = la.ListingAgentCorpAddress,
            //        ListingAgentCorpAddress2 = la.ListingAgentCorpAddress2,
            //        ListingAgentCity = la.ListingAgentCity,
            //        ListingAgentEmail = la.ListingAgentEmail,
            //        ListingAgentName = la.ListingAgentName,
            //        AssetListingAgentId = la.AssetListingAgentId,
            //        ListingAgentState = la.ListingAgentState,
            //        ListingAgentZip = la.ListingAgentZip,
            //        ListingAgentCompany = la.ListingAgentCompany,
            //        ListingAgentPhoneNumber = la.ListingAgentPhoneNumber,
            //        AssetId = la.AssetId,
            //        ListingAgentNewName = la.ListingAgentNewName
            //    }).ToList();

            var tempMembers = context.AssetNARMembers
                .Include(x => x.Asset)
                .Include(x => x.NARMember).ToList();


            var groups = tempMembers.GroupBy(x => x.NarMemberId);
            foreach (var anmGroup in groups)
            {
                var anm = anmGroup.First();
                int totalPublished = anmGroup.Where(x => x.Asset.IsPublished).Count();
                members.Add(new NarMemberViewModel()
                {
                    AssetNumbers = string.Join(" ", anmGroup.OrderBy(x => x.Asset.AssetNumber).Select(x => x.Asset.AssetNumber)),
                    CellPhoneNumber = anm.NARMember.CellPhoneNumber,
                    FaxNumber = anm.NARMember.FaxNumber,
                    WorkNumber = anm.NARMember.WorkPhoneNumber,
                    AddressLine1 = anm.NARMember.CompanyAddressLine1,
                    AddressLine2 = anm.NARMember.CompanyAddressLine2,
                    City = anm.NARMember.CompanyCity,
                    CompanyName = anm.NARMember.CompanyName,
                    Email = anm.NARMember.Email,
                    FirstName = anm.NARMember.FirstName,
                    IsActive = anm.NARMember.IsActive,
                    LastName = anm.NARMember.LastName,
                    NarMemberId = anm.NARMember.NarMemberId,
                    State = anm.NARMember.CompanyState,
                    Zip = anm.NARMember.CompanyZip,
                    CommissionShareAgr = anm.NARMember.CommissionShareAgr,
                    CommissionAmount = anm.NARMember.CommissionAmount,
                    DateOfCsaConfirm = anm.NARMember.DateOfCsaConfirm,
                    AssetId = anm.AssetId,
                    AssetNARMemberId = anm.AssetNARMemberId,
                    TotalAssests = anmGroup.Count(),
                    TotalPublished = totalPublished
                });
            }

            if (model.ShowActiveOnly)
            {
                members = members.Where(w => w.IsActive).ToList();
            }
            if (!string.IsNullOrEmpty(model.AddressLine1))
            {
                members = members.Where(w => !string.IsNullOrEmpty(w.AddressLine1) && w.AddressLine1.ToLower().Contains(model.AddressLine1.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.City))
            {
                members = members.Where(w => !string.IsNullOrEmpty(w.City) && w.City.ToLower().Contains(model.City.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.FirstName))
            {
                members = members.Where(w => !string.IsNullOrEmpty(w.FirstName) && w.FirstName.ToLower().Contains(model.FirstName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.LastName))
            {
                members = members.Where(w => !string.IsNullOrEmpty(w.LastName) && w.LastName.ToLower().Contains(model.LastName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.State))
            {
                members = members.Where(w => !string.IsNullOrEmpty(w.State) && w.State.ToLower().Contains(model.State.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.Email))
            {
                members = members.Where(w => !string.IsNullOrEmpty(w.Email) && w.Email.ToLower().Contains(model.Email.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.CompanyName))
            {
                members = members.Where(w => !string.IsNullOrEmpty(w.CompanyName) && w.CompanyName.ToLower().Contains(model.CompanyName.ToLower())).ToList();
            }

            return members;
        }

        public List<NarMemberViewModel> GetNarMembersImported(NarMemberSearchModel model)
        {
            var list = new List<NarMemberViewModel>();
            var context = _factory.Create();
            var members = context.NarMembers.Where(x => x.Registered == null || x.Registered == false).ToList();
            if (model.ShowActiveOnly)
            {
                members = context.NarMembers.Where(w => w.IsActive).ToList();
            }
            if (!string.IsNullOrEmpty(model.AddressLine1))
            {
                members = members.Where(w => !string.IsNullOrEmpty(w.CompanyAddressLine1) && w.CompanyAddressLine1.ToLower().Contains(model.AddressLine1.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.City))
            {
                members = members.Where(w => !string.IsNullOrEmpty(w.CompanyCity) && w.CompanyCity.ToLower().Contains(model.City.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.FirstName))
            {
                members = members.Where(w => !string.IsNullOrEmpty(w.FirstName) && w.FirstName.ToLower().Contains(model.FirstName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.LastName))
            {
                members = members.Where(w => !string.IsNullOrEmpty(w.LastName) && w.LastName.ToLower().Contains(model.LastName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.State))
            {
                members = members.Where(w => !string.IsNullOrEmpty(w.CompanyState) && w.CompanyState.ToLower().Contains(model.State.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.Email))
            {
                members = members.Where(w => !string.IsNullOrEmpty(w.Email) && w.Email.ToLower().Contains(model.Email.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.CompanyName))
            {
                members = members.Where(w => !string.IsNullOrEmpty(w.CompanyName) && w.CompanyName.ToLower().Contains(model.CompanyName.ToLower())).ToList();
            }
            members.ToList().ForEach(s =>
            {
                list.Add(new NarMemberViewModel()
                {
                    CellPhoneNumber = s.CellPhoneNumber,
                    FaxNumber = s.FaxNumber,
                    WorkNumber = s.WorkPhoneNumber,
                    AddressLine1 = s.CompanyAddressLine1,
                    AddressLine2 = s.CompanyAddressLine2,
                    City = s.CompanyCity,
                    CompanyName = s.CompanyName,
                    Email = s.Email,
                    FirstName = s.FirstName,
                    IsActive = s.IsActive,
                    LastName = s.LastName,
                    NarMemberId = s.NarMemberId,
                    State = s.CompanyState,
                    Zip = s.CompanyZip
                });
            });
            return list.OrderBy(x => x.FirstName).ToList();
        }

        public List<NarMemberViewModel> GetNarMembers(NarMemberSearchModel model)
        {
            var list = new List<NarMemberViewModel>();
            var context = _factory.Create();
            IQueryable<NARMember> members = context.NarMembers;
            if (model.ShowActiveOnly)
            {
                members = context.NarMembers.Where(w => w.IsActive);
            }
            if (!string.IsNullOrEmpty(model.AddressLine1))
            {
                members = members.Where(w => w.CompanyAddressLine1 == model.AddressLine1);
            }
            if (!string.IsNullOrEmpty(model.City))
            {
                members = members.Where(w => w.CompanyCity == model.City);
            }
            if (!string.IsNullOrEmpty(model.FirstName))
            {
                members = members.Where(w => w.FirstName == model.FirstName);
            }
            if (!string.IsNullOrEmpty(model.LastName))
            {
                members = members.Where(w => w.LastName == model.LastName);
            }
            if (!string.IsNullOrEmpty(model.State))
            {
                members = members.Where(w => w.CompanyState == model.State);
            }
            if (!string.IsNullOrEmpty(model.Zip))
            {
                members = members.Where(w => w.CompanyZip == model.Zip);
            }
            members.ToList().ForEach(s =>
            {
                list.Add(new NarMemberViewModel()
                {
                    CellPhoneNumber = s.CellPhoneNumber,
                    FaxNumber = s.FaxNumber,
                    WorkNumber = s.WorkPhoneNumber,
                    AddressLine1 = s.CompanyAddressLine1,
                    AddressLine2 = s.CompanyAddressLine2,
                    City = s.CompanyCity,
                    CompanyName = s.CompanyName,
                    Email = s.Email,
                    FirstName = s.FirstName,
                    IsActive = s.IsActive,
                    LastName = s.LastName,
                    NarMemberId = s.NarMemberId,
                    State = s.CompanyState,
                    Zip = s.CompanyZip
                });
            });
            return list.OrderBy(x => x.FirstName).ToList();
        }

        public NarMemberViewModel GetNarMember(int id)
        {
            var context = _factory.Create();
            var member = context.NarMembers.FirstOrDefault(s => s.NarMemberId == id);
            if (member != null)
            {
                return new NarMemberViewModel()
                {
                    CellPhoneNumber = member.CellPhoneNumber,
                    FaxNumber = member.FaxNumber,
                    WorkNumber = member.WorkPhoneNumber,
                    AddressLine1 = member.CompanyAddressLine1,
                    AddressLine2 = member.CompanyAddressLine2,
                    City = member.CompanyCity,
                    CompanyName = member.CompanyName,
                    Email = member.Email,
                    FirstName = member.FirstName,
                    IsActive = member.IsActive,
                    LastName = member.LastName,
                    NarMemberId = member.NarMemberId,
                    State = member.CompanyState,
                    Zip = member.CompanyZip
                };
            }
            return null;
        }

        public void DeactiveNarMember(int id)
        {
            var context = _factory.Create();
            var member = context.NarMembers.FirstOrDefault(w => w.NarMemberId == id);
            if (member != null)
            {
                member.IsActive = false;
                context.Save();
            }
        }

        public void ReactivateNarMember(int id)
        {
            var context = _factory.Create();
            var member = context.NarMembers.FirstOrDefault(w => w.NarMemberId == id);
            if (member != null)
            {
                member.IsActive = true;
                context.Save();
            }
        }

        public void UpdateNarMember(NarMemberViewModel model)
        {
            var context = _factory.Create();
            var member = context.NarMembers.FirstOrDefault(w => w.NarMemberId == model.NarMemberId);
            if (member != null)
            {
                member.CellPhoneNumber = model.CellPhoneNumber;
                member.WorkPhoneNumber = model.WorkNumber;
                member.FaxNumber = model.FaxNumber;
                member.CompanyAddressLine1 = model.AddressLine1;
                member.CompanyAddressLine2 = model.AddressLine2;
                member.CompanyCity = model.City;
                member.CompanyName = model.CompanyName;
                member.CompanyState = model.State;
                member.CompanyZip = model.Zip;
                member.Email = model.Email;
                member.FirstName = model.FirstName;
                member.IsActive = model.IsActive;
                member.LastName = model.LastName;
                member.NarMemberId = model.NarMemberId;
                context.Save();
            }
        }

        //public void CreateNarMember(NarMemberQuickViewModel model)
        //{
        //    var context = _factory.Create();
        //    context.NarMembers.Add(new NARMember()
        //    {
        //        FaxNumber = model.FaxNumber,
        //        CellPhoneNumber = model.CellPhoneNumber,
        //        WorkPhoneNumber = model.WorkNumber,
        //        CompanyAddressLine1 = model.AddressLine1,
        //        CompanyAddressLine2 = model.AddressLine2,
        //        CompanyCity = model.City,
        //        CompanyName = model.CompanyName,
        //        CompanyState = model.State,
        //        CompanyZip = model.Zip,
        //        Email = model.Email,
        //        FirstName = model.FirstName,
        //        IsActive = model.IsActive,
        //        LastName = model.LastName,
        //        NarMemberId = model.NarMemberId,
        //    });
        //    context.Save();
        //}

        public int CreateNarMember(NarMemberViewModel model)
        {
            var context = _factory.Create();
            var narMember = new NARMember();

            narMember.FaxNumber = model.FaxNumber;
            narMember.CellPhoneNumber = model.CellPhoneNumber;
            narMember.WorkPhoneNumber = model.WorkNumber;
            narMember.CompanyAddressLine1 = model.AddressLine1;
            narMember.CompanyAddressLine2 = model.AddressLine2;
            narMember.CompanyCity = model.City;
            narMember.CompanyName = model.CompanyName;
            narMember.CompanyState = model.State;
            narMember.CompanyZip = model.Zip;
            narMember.Email = model.Email.ToLower();
            narMember.FirstName = model.FirstName;
            narMember.IsActive = model.IsActive;
            narMember.LastName = model.LastName;
            //narMember.NarMemberId = model.NarMemberId;
            narMember.Registered = false;
            context.NarMembers.Add(narMember);
            context.Save();
            return narMember.NarMemberId;
        }

        public string ImportMBAUsers(string path, int referringUserId, bool areReferredUsers, bool checkAgainstNARMembers = true, bool checkAgainstPrincipalInvestors = true)
        {
            int insertedRows = 0;
            int errorRows = 0;
            var connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + @";Extended Properties=""Excel 12.0 Xml;HDR=YES;IMEX=1""";
            var connection = new OleDbConnection(connectionString);
            connection.Open();
            int rowIndex = 1;

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM [MBA Members$]";

            var dataReader = command.ExecuteReader();
            StringBuilder sb = new StringBuilder();
            var context = _factory.Create();
            var user = context.Users.Find(referringUserId);
            while (dataReader.Read())
            {
                try
                {
                    string firstName = null;
                    string lastName = null;
                    string email = null;
                    string companyName = null;
                    string companyAddressLine1 = null;
                    string companyAddressLine2 = null;
                    string companyCity = null;
                    string companyState = null;
                    string companyZip = null;
                    string cellNumber = null;
                    string faxNumber = null;
                    string workNumber = null;
                    string website = null;

                    if (dataReader["First Name"].ToString().Length > 0)
                    {
                        firstName = dataReader["First Name"].ToString();
                    }
                    if (dataReader["Last Name"].ToString().Length > 0)
                    {
                        lastName = dataReader["Last Name"].ToString();
                    }
                    if (dataReader["Email"].ToString().Length > 0)
                    {
                        email = dataReader["Email"].ToString();
                    }
                    if (dataReader["Company Name"].ToString().Length > 0)
                    {
                        companyName = dataReader["Company Name"].ToString();
                    }
                    if (dataReader["Company Web Site"].ToString().Length > 0)
                    {
                        website = dataReader["Company Web Site"].ToString();
                    }
                    if (dataReader["Company Address Line 1"].ToString().Length > 0)
                    {
                        companyAddressLine1 = dataReader["Company Address Line 1"].ToString();
                    }
                    if (dataReader["Company Address Line 2"].ToString().Length > 0)
                    {
                        companyAddressLine2 = dataReader["Company Address Line 2"].ToString();
                    }
                    if (dataReader["Company City"].ToString().Length > 0)
                    {
                        companyCity = dataReader["Company City"].ToString();
                    }
                    if (dataReader["Company State"].ToString().Length > 0)
                    {
                        companyState = dataReader["Company State"].ToString();
                    }
                    if (dataReader["Company Zip"].ToString().Length > 0)
                    {
                        companyZip = dataReader["Company Zip"].ToString();
                    }
                    if (dataReader["Principal Direct Contact Phone Number"].ToString().Length > 0)
                    {
                        cellNumber = dataReader["Principal Direct Contact Phone Number"].ToString();
                    }
                    if (dataReader["Company General Fax Number"].ToString().Length > 0)
                    {
                        faxNumber = dataReader["Company General Fax Number"].ToString();
                    }
                    if (dataReader["Company General Phone Number"].ToString().Length > 0)
                    {
                        workNumber = dataReader["Company General Phone Number"].ToString();
                    }
                    if (!string.IsNullOrEmpty(email))
                    {
                        var error = false;
                        var mbaUsers = context.MbaUsers.FirstOrDefault(s => s.Email.ToLower() == email.ToLower());
                        if (checkAgainstNARMembers)
                        {
                            var narMembers = context.NarMembers.FirstOrDefault(s => s.Email.ToLower() == email.ToLower());
                            if (narMembers != null)
                            {
                                error = true;
                                errorRows++;
                                sb.Append(string.Format("Row {0}, {1} {2} - Already listed as an NAR member in system.<br/>", rowIndex, firstName, lastName));
                            }
                        }
                        if (checkAgainstPrincipalInvestors)
                        {
                            var investors = context.PrincipalInvestors.FirstOrDefault(s => s.Email.ToLower() == email.ToLower());
                            if (investors != null)
                            {
                                error = true;
                                errorRows++;
                                sb.Append(string.Format("Row {0}, {1} {2} - User is already a Principal Investor in system.<br/>", rowIndex, firstName, lastName));
                            }
                        }
                        if (mbaUsers != null)
                        {
                            error = true;
                            errorRows++;
                            sb.Append(string.Format("Row {0}, {1} {2} - MBA already in system.<br/>", rowIndex, firstName, lastName));
                        }
                        if (!error)
                        {
                            context.MbaUsers.Add(new MBAUser()
                            {
                                CompanyAddressLine1 = companyAddressLine1,
                                CompanyAddressLine2 = companyAddressLine2,
                                CompanyCity = companyCity,
                                CompanyName = companyName,
                                CompanyState = companyState,
                                CompanyZip = companyZip,
                                Email = email.ToLower(),
                                FirstName = firstName,
                                LastName = lastName,
                                CellPhoneNumber = cellNumber,
                                WorkPhoneNumber = workNumber,
                                FaxNumber = faxNumber,
                                ReferredByUserId = referringUserId,
                                CompanyWebsite = website,
                                IsActive = true,
                                Registered = false
                            });
                            if (areReferredUsers)
                            {
                                try
                                {
                                    string message = ReferUser(context, firstName, lastName, companyCity, companyState, email, user);
                                    if (!string.IsNullOrEmpty(message))
                                    {
                                        sb.AppendLine(message);
                                        errorRows++;
                                    }
                                }
                                catch { }
                            }
                            insertedRows++;
                        }
                    }
                    else
                    {
                        errorRows++;
                        sb.Append(string.Format("Row {0} - Missing required email address.<br/>", rowIndex));
                    }
                }
                catch (Exception ex)
                {
                    errorRows++;
                    sb.Append(ex.Message + Environment.NewLine);
                }
                rowIndex++;
            }
            dataReader.Close();
            connection.Close();
            context.Save();
            return string.Format("File successfully processed. {0} imported; {1} errors<br/>{2}", insertedRows, errorRows, sb.ToString());
        }

        public List<MbaViewModel> GetMbas(MbaSearchModel model)
        {
            var list = new List<MbaViewModel>();
            var context = _factory.Create();
            IQueryable<MBAUser> members = context.MbaUsers.Where(x => x.Registered == null || x.Registered == false);
            if (model.ShowActiveOnly)
            {
                members = context.MbaUsers.Where(w => w.IsActive);
            }
            if (!string.IsNullOrEmpty(model.AddressLine1))
            {
                members = members.Where(w => w.CompanyAddressLine1 == model.AddressLine1);
            }
            if (!string.IsNullOrEmpty(model.City))
            {
                members = members.Where(w => w.CompanyCity == model.City);
            }
            if (!string.IsNullOrEmpty(model.FirstName))
            {
                members = members.Where(w => w.FirstName.ToLower().Contains(model.FirstName.ToLower()));
            }
            if (!string.IsNullOrEmpty(model.LastName))
            {
                members = members.Where(w => w.LastName.ToLower().Contains(model.LastName.ToLower()));
            }
            if (!string.IsNullOrEmpty(model.Email))
            {
                members = members.Where(w => w.Email.ToLower().Contains(model.Email.ToLower()));
            }
            if (!string.IsNullOrEmpty(model.CompanyName))
            {
                members = members.Where(w => w.CompanyName.ToLower().Contains(model.CompanyName.ToLower()));
            }
            if (!string.IsNullOrEmpty(model.Email))
            {
                members = members.Where(w => w.Email == model.Email);
            }
            if (!string.IsNullOrEmpty(model.CompanyName))
            {
                members = members.Where(w => w.CompanyName == model.CompanyName);
            }
            if (!string.IsNullOrEmpty(model.State))
            {
                members = members.Where(w => w.CompanyState == model.State);
            }
            if (!string.IsNullOrEmpty(model.Zip))
            {
                members = members.Where(w => w.CompanyZip == model.Zip);
            }
            members.ToList().ForEach(s =>
            {
                list.Add(new MbaViewModel()
                {
                    CellPhoneNumber = s.CellPhoneNumber,
                    FaxNumber = s.FaxNumber,
                    WorkNumber = s.WorkPhoneNumber,
                    AddressLine1 = s.CompanyAddressLine1,
                    AddressLine2 = s.CompanyAddressLine2,
                    City = s.CompanyCity,
                    CompanyName = s.CompanyName,
                    Email = s.Email,
                    FirstName = s.FirstName,
                    IsActive = s.IsActive,
                    LastName = s.LastName,
                    MbaUserId = s.MBAUserId,
                    State = s.CompanyState,
                    Zip = s.CompanyZip,
                    Website = s.CompanyWebsite
                });
            });
            return list;
        }

        public MbaViewModel GetMbaById(int id)
        {
            var context = _factory.Create();
            var member = context.MbaUsers.FirstOrDefault(s => s.MBAUserId == id);
            if (member != null)
            {
                return new MbaViewModel()
                {
                    CellPhoneNumber = member.CellPhoneNumber,
                    FaxNumber = member.FaxNumber,
                    WorkNumber = member.WorkPhoneNumber,
                    AddressLine1 = member.CompanyAddressLine1,
                    AddressLine2 = member.CompanyAddressLine2,
                    City = member.CompanyCity,
                    CompanyName = member.CompanyName,
                    Email = member.Email,
                    FirstName = member.FirstName,
                    IsActive = member.IsActive,
                    LastName = member.LastName,
                    MbaUserId = member.MBAUserId,
                    State = member.CompanyState,
                    Zip = member.CompanyZip,
                    Website = member.CompanyWebsite
                };
            }
            return null;
        }

        public void DeactiveMba(int id)
        {
            var context = _factory.Create();
            var member = context.MbaUsers.FirstOrDefault(w => w.MBAUserId == id);
            if (member != null)
            {
                member.IsActive = false;
                context.Save();
            }
        }

        public void UpdateMba(MbaViewModel model)
        {
            var context = _factory.Create();
            var member = context.MbaUsers.FirstOrDefault(w => w.MBAUserId == model.MbaUserId);
            if (member != null)
            {
                member.CellPhoneNumber = model.CellPhoneNumber;
                member.WorkPhoneNumber = model.WorkNumber;
                member.FaxNumber = model.FaxNumber;
                member.CompanyAddressLine1 = model.AddressLine1;
                member.CompanyAddressLine2 = model.AddressLine2;
                member.CompanyCity = model.City;
                member.CompanyName = model.CompanyName;
                member.CompanyState = model.State;
                member.CompanyZip = model.Zip;
                member.Email = model.Email;
                member.FirstName = model.FirstName;
                member.IsActive = model.IsActive;
                member.LastName = model.LastName;
                member.CompanyWebsite = model.Website;
                context.Save();
            }
        }

        public void ReactivateMba(int id)
        {
            var context = _factory.Create();
            var member = context.MbaUsers.FirstOrDefault(w => w.MBAUserId == id);
            if (member != null)
            {
                member.IsActive = true;
                context.Save();
            }
        }

        public void CreateMba(MbaViewModel model)
        {
            var context = _factory.Create();
            context.MbaUsers.Add(new MBAUser()
            {
                FaxNumber = model.FaxNumber,
                CellPhoneNumber = model.CellPhoneNumber,
                WorkPhoneNumber = model.WorkNumber,
                CompanyAddressLine1 = model.AddressLine1,
                CompanyAddressLine2 = model.AddressLine2,
                CompanyCity = model.City,
                CompanyName = model.CompanyName,
                CompanyState = model.State,
                CompanyZip = model.Zip,
                Email = model.Email.ToLower(),
                FirstName = model.FirstName,
                IsActive = model.IsActive,
                LastName = model.LastName,
                CompanyWebsite = model.Website,
                Registered = false
            });
            context.Save();
        }


        public void ApproveICAdmin(int id)
        {
            var context = _factory.Create();
            var user = context.Users.FirstOrDefault(f => f.UserId == id);
            if (user != null)
            {
                user.ICStatus = ICStatus.Approved;
                context.Save();
            }
        }

        public void RejectICAdmin(int id)
        {
            var context = _factory.Create();
            var user = context.Users.FirstOrDefault(f => f.UserId == id);
            if (user != null)
            {
                user.ICStatus = ICStatus.Rejected;
                context.Save();
            }
        }

        private int getTotalAssetCountForICAdmin(int userId)
        {
            var context = _factory.Create();
            return context.Assets.Count(w => w.ListedByUserId == userId && w.IsSubmitted);
        }

        private int getLast30DayAssetCountForICAdmin(int userId)
        {
            var context = _factory.Create();
            var dateFilter = DateTime.Now.AddDays(-30);
            return context.Assets.Count(w => w.ListedByUserId == userId && w.IsSubmitted && w.CreationDate.HasValue && w.CreationDate.Value >= dateFilter);
        }

        private int getPendingAssetCountForICAdmin(int userId)
        {
            var context = _factory.Create();
            return context.Assets.Count(w => w.ListedByUserId == userId && !w.IsSubmitted);
        }

        public AccountingRecordDisplayModel GetICAccountingReportDisplay(int userId, int fiscalYear, UserModel controllingUser)
        {
            var context = _factory.Create();
            var user = context.Users.Include(s => s.UserNotes).FirstOrDefault(f => f.UserId == userId);
            var assets = context.Assets.Where(w => w.ListedByUserId == user.UserId && w.IsSubmitted && w.Show && w.CreationDate.HasValue && w.CreationDate.Value.Year == fiscalYear);
            var model = new AccountingRecordDisplayModel();
            model.Type = ContractFeePayoutType.icAdmin;
            model.StartYear = fiscalYear;
            model.UserId = userId;
            model.Name = user.FullName;
            model.AddressLine1 = user.AddressLine1 + " " + user.AddressLine2;
            model.City = user.City;
            model.State = user.State;
            model.Zip = user.Zip;
            model.Tin = user.CorporateTIN;
            model.ControllingUserType = controllingUser.UserType;
            var ic = context.UserFiles.Where(w => w.UserId == user.UserId && w.FileName == "Executed IC Agreement").OrderByDescending(w => w.DateUploaded).ToList();
            if (ic.Count() > 0)
            {
                model.DateOfCurrentSignedICAgreement = ic.First().DateUploaded;
            }
            else
            {
                model.DateOfCurrentSignedICAgreement = DateTime.Now;
            }
            model.MiscellaneousNotes = user.UserNotes.Where(w => w.CreateDate.Year == fiscalYear).OrderBy(w => w.CreateDate).Select(s => s.Notes).ToList();
            foreach (var asset in assets)
            {
                model.AssetHistoryItems.Add(new ICAdminAssetHistoryItem()
                {
                    AssetId = asset.AssetId,
                    AssetNumber = asset.AssetNumber,
                    DatePublished = asset.CreationDate.GetValueOrDefault(DateTime.Now),
                    AssetType = asset.AssetTypeAbbreviation,
                    AssetInventoryAddress = string.Format("{0} - {1} - {2} - {3}", asset.PropertyAddress, asset.City, asset.State, asset.Zip)
                });
            }
            model.ContractDates = ic.Select(s => s.DateUploaded).OrderBy(w => w.Date).ToList();
            var payouts = context.ContractFeePayouts.Where(w => w.UserId == user.UserId && w.DatePaid.Year == fiscalYear && w.Type == ContractFeePayoutType.icAdmin).OrderBy(w => w.DatePaid).ToList();
            foreach (var payout in payouts)
            {
                model.ContractFeeDetails.Add(new ContractFeeDetail()
                {
                    ContractPayment = payout.FeeAmount,
                    DateFeePaid = payout.DatePaid,
                    ContractFeeDetailId = payout.ContractFeePayoutId
                });
            }
            model.TotalPaidForFiscalYear = payouts.Sum(s => s.FeeAmount);
            return model;
        }

        public AccountingRecordDisplayModel GetJVMAAccountingReportDisplay(int userId, int fiscalYear, UserModel controllingUser)
        {
            // i completely forgot what im supposed to be supplying here(ive never really known), so im going to get all of the current user's PI's mdas
            var context = _factory.Create();
            var user = context.Users
                .Include(s => s.UserNotes)
                .FirstOrDefault(f => f.UserId == userId);
            var model = new AccountingRecordDisplayModel();
            model.Type = ContractFeePayoutType.jvma;
            model.StartYear = fiscalYear;
            model.UserId = userId;
            model.Name = user.FullName;
            model.AddressLine1 = user.AddressLine1 + " " + user.AddressLine2;
            model.City = user.City;
            model.State = user.State;
            model.Zip = user.Zip;
            model.Tin = user.CorporateTIN;
            model.ControllingUserType = controllingUser.UserType;
            var jv = context.UserFiles.Where(x => x.UserId == user.UserId && x.FileName == "Executed JV Agreement").ToList();
            if (jv.Count > 0)
            {
                model.DateOfCurrentSignedJVMAAgreement = jv.OrderByDescending(x => x.DateUploaded).First().DateUploaded;
            }
            else
            {
                model.DateOfCurrentSignedJVMAAgreement = DateTime.Now;
            }
            model.MDAHistoryItems = (from u in context.Users
                                     join mda in context.AssetUserMDAs on u.UserId equals mda.UserId
                                     join a in context.Assets on mda.AssetId equals a.AssetId
                                     where u.ReferredByUserId == userId &&
                                     u.IsActive &&
                                     mda.SignMDADate.Year == fiscalYear
                                     select new JVMAMDAHistoryItem()
                                     {
                                         FullName = u.FirstName + " " + u.LastName,
                                         Email = u.Username,
                                         AssetNumber = a.AssetNumber,
                                         AssetId = a.AssetId,
                                         DateMDASigned = mda.SignMDADate,
                                         AssetDescription = a.PropertyAddress + " - " + a.City + " - " + a.State + " - " + a.Zip
                                     }).OrderByDescending(x => x.DateMDASigned).ToList();

            model.MiscellaneousNotes = context.UserNotes.Where(x => x.UserId == user.UserId).OrderBy(x => x.CreateDate).Select(x => x.Notes).ToList();
            model.ContractDates = jv.Select(s => s.DateUploaded).OrderBy(w => w.Date).ToList();
            var payouts = context.ContractFeePayouts.Where(w => w.UserId == user.UserId && w.DatePaid.Year == fiscalYear && w.Type == ContractFeePayoutType.jvma).OrderBy(w => w.DatePaid).ToList();
            foreach (var payout in payouts)
            {
                model.ContractFeeDetails.Add(new ContractFeeDetail()
                {
                    ContractPayment = payout.FeeAmount,
                    DateFeePaid = payout.DatePaid,
                    ContractFeeDetailId = payout.ContractFeePayoutId
                });
            }
            model.TotalPaidForFiscalYear = payouts.Sum(s => s.FeeAmount);
            return model;
        }

        public void RecordContractPayment(RecordContractPaymentModel model)
        {
            var context = _factory.Create();
            context.ContractFeePayouts.Add(new ContractFeePayout()
            {
                DatePaid = model.PaymentDate,
                FeeAmount = model.AmountPaid,
                RecordedByUserId = model.RecordedByUserId,
                UserId = model.UserId,
                Type = model.Type
            });
            context.Save();
        }

        public UserModel GetReferredByEmail(string username)
        {
            var context = _factory.Create();
            var user = context.Users.FirstOrDefault(s => s.Username == username);
            var narMember = context.NarMembers.FirstOrDefault(w => w.Email.ToLower() == user.Username.ToLower() || (w.FirstName.ToLower() == user.FirstName.ToLower() && w.LastName.ToLower() == user.LastName.ToLower()));

            if (narMember != null)
            {
                if (narMember.ReferredByUserId.HasValue)
                {
                    return GetUserById(narMember.ReferredByUserId.Value);
                }
            }
            else
            {
                var mba = context.MbaUsers.FirstOrDefault(w => w.Email.ToLower() == user.Username.ToLower() || (w.FirstName.ToLower() == user.FirstName.ToLower() && w.LastName.ToLower() == user.LastName.ToLower()));
                if (mba != null)
                {
                    if (mba.ReferredByUserId.HasValue)
                    {
                        return GetUserById(mba.ReferredByUserId.Value);
                    }
                }
            }
            return null;
        }

        public UserModel GetJVMAParticipantByUser(string username)
        {
            var context = _factory.Create();
            var user = context.Users.FirstOrDefault(x => x.Username == username.ToLower());
            if (user.ReferredByUserId.HasValue)
            {
                var referrer = context.Users.Where(x => x.UserId == user.ReferredByUserId).FirstOrDefault();
                if (referrer != null)
                    return new UserModel()
                    {
                        FirstName = referrer.FirstName,
                        LastName = referrer.LastName,
                        Username = referrer.Username
                    };
            }
            return null;
        }

        public List<UserModel> GetAdmins()
        {
            var context = _factory.Create();
            var list = new List<UserModel>();
            var users = context.Users.Where(w => w.UserType == UserType.CorpAdmin || w.UserType == UserType.CorpAdmin2).Select(s => s.UserId);
            foreach (var user in users)
            {
                list.Add(GetUserById(user));
            }
            return list;
        }

        public List<UserQuickViewModel> GetICAdmins(UserSearchModel searchModel)
        {
            // Seems like a bit extra overhead but is cleaner than conditionally setting the values
            // in the GetUsers method
            var context = _factory.Create();
            var users = GetUsers(searchModel);
            var userFiles = context.UserFiles;
            var assets = context.Assets;
            var pastDate = DateTime.Now.AddDays(-30);
            users.ToList().ForEach(u =>
            {
                u.ICCount = userFiles.Where(x => x.UserId == u.UserId && x.FileName == "Executed IC Agreement").ToList().Count();
                u.PendingAssets = assets.Where(x => x.ListingStatus == ListingStatus.Pending && x.ListedByUserId == u.UserId).ToList().Count();
                u.TotalNewAssets = assets.Where(x => x.ListedByUserId == u.UserId).ToList().Count();
                u.Last30Assets = assets.Where(x => x.ListedByUserId == u.UserId &&
                    x.CreationDate <= DateTime.Now &&
                    x.CreationDate >= pastDate).ToList().Count;
            });
            return users;
        }

        public List<TitleQuickViewModel> GetTitleCompanies(CompanySearchModel searchModel)
        {
            var context = _factory.Create();
            var titleCompanies = GetTitles(searchModel);
            var userFiles = context.UserFiles;
            var assets = context.Assets;
            var pastDate = DateTime.Now.AddDays(-30);
            titleCompanies.ToList().ForEach(u =>
            {
                u.TotalItemCount = titleCompanies.Count();
                u.showInActive = false;
            });

            return titleCompanies;
        }

        /*
        public Tuple<string, int> ImportPrincipalInvestors(string path, int referringUserId, bool areReferredUsers, bool checkAgainstMBAs = true, bool CheckAgainstNARs = true)
        {
            int insertedRows = 0;
            int errorRows = 0;
            var connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + @";Extended Properties=""Excel 12.0 Xml;HDR=YES;IMEX=1""";
            var connection = new OleDbConnection(connectionString);
            connection.Open();
            int rowIndex = 1;

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM [Principal Investors$]";

            var dataReader = command.ExecuteReader();
            StringBuilder sb = new StringBuilder();
            var context = _factory.Create();
            var user = context.Users.Find(referringUserId);
            while (dataReader.Read())
            {
                try
                {
                    string firstName = null;
                    string lastName = null;
                    string email = null;
                    string companyName = null;
                    string companyAddressLine1 = null;
                    string companyAddressLine2 = null;
                    string companyCity = null;
                    string companyState = null;
                    string companyZip = null;
                    string cellNumber = null;
                    string faxNumber = null;
                    string workNumber = null;
                    string website = null;
                    if (dataReader["First Name"].ToString().Length > 0 && dataReader["Email"].ToString().Length > 0)
                    {
                        if (dataReader["First Name"].ToString().Length > 0)
                        {
                            firstName = dataReader["First Name"].ToString();
                        }
                        if (dataReader["Last Name"].ToString().Length > 0)
                        {
                            lastName = dataReader["Last Name"].ToString();
                        }
                        if (dataReader["Email"].ToString().Length > 0)
                        {
                            email = dataReader["Email"].ToString();
                        }
                        if (dataReader["Company Name"].ToString().Length > 0)
                        {
                            companyName = dataReader["Company Name"].ToString();
                        }
                        if (dataReader["Company Web Site"].ToString().Length > 0)
                        {
                            website = dataReader["Company Web Site"].ToString();
                        }
                        if (dataReader["Company Address Line 1"].ToString().Length > 0)
                        {
                            companyAddressLine1 = dataReader["Company Address Line 1"].ToString();
                        }
                        if (dataReader["Company Address Line 2"].ToString().Length > 0)
                        {
                            companyAddressLine2 = dataReader["Company Address Line 2"].ToString();
                        }
                        if (dataReader["Company City"].ToString().Length > 0)
                        {
                            companyCity = dataReader["Company City"].ToString();
                        }
                        if (dataReader["Company State"].ToString().Length > 0)
                        {
                            companyState = dataReader["Company State"].ToString();
                        }
                        if (dataReader["Company Zip"].ToString().Length > 0)
                        {
                            companyZip = dataReader["Company Zip"].ToString();
                        }
                        if (dataReader["Principal Direct Contact Phone Number"].ToString().Length > 0)
                        {
                            cellNumber = dataReader["Principal Direct Contact Phone Number"].ToString();
                        }
                        if (dataReader["Company General Fax Number"].ToString().Length > 0)
                        {
                            faxNumber = dataReader["Company General Fax Number"].ToString();
                        }
                        if (dataReader["Company General Phone Number"].ToString().Length > 0)
                        {
                            workNumber = dataReader["Company General Phone Number"].ToString();
                        }
                        if (!string.IsNullOrEmpty(email))
                        {
                            bool error = false;
                            var investors = context.PrincipalInvestors.FirstOrDefault(s => s.Email.ToLower() == email.ToLower());
                            if (investors != null)
                            {
                                error = true;
                                errorRows++;
                                sb.Append(string.Format("Row {0}, {1} {2} - User is already a Principal Investor in system.<br/>", rowIndex, firstName, lastName));
                            }
                            if (CheckAgainstNARs)
                            {
                                var narMember = context.NarMembers.FirstOrDefault(s => s.Email.ToLower() == email.ToLower());
                                if (narMember != null)
                                {
                                    error = true;
                                    errorRows++;
                                    sb.Append(string.Format("Row {0}, {1} {2} - NAR Member already in system.<br/>", rowIndex, firstName, lastName));
                                }
                            }
                            if (checkAgainstMBAs)
                            {
                                var mbaMember = context.MbaUsers.FirstOrDefault(s => s.Email.ToLower() == email.ToLower());
                                if (mbaMember != null)
                                {
                                    error = true;
                                    errorRows++;
                                    sb.Append(string.Format("Row {0}, {1} {2} - User is already a MBA in system.<br/>", rowIndex, firstName, lastName));
                                }
                            }
                            if (!error)
                            {
                                context.PrincipalInvestors.Add(new PrincipalInvestor()
                                {
                                    CompanyAddressLine1 = companyAddressLine1,
                                    CompanyAddressLine2 = companyAddressLine2,
                                    CompanyCity = companyCity,
                                    CompanyName = companyName,
                                    CompanyState = companyState,
                                    CompanyZip = companyZip,
                                    Email = email.ToLower(),
                                    FirstName = firstName,
                                    LastName = lastName,
                                    CellPhoneNumber = cellNumber,
                                    WorkPhoneNumber = workNumber,
                                    FaxNumber = faxNumber,
                                    ReferredByUserId = referringUserId,
                                    IsActive = true,
                                    Website = website,
                                    Registered = false
                                });
                                if (areReferredUsers)
                                {
                                    try
                                    {
                                        ReferUser(context, firstName, lastName, companyCity, companyState, email, user);
                                    }
                                    catch { }
                                }
                                insertedRows++;
                            }
                        }
                        else
                        {
                            errorRows++;
                            sb.Append(string.Format("Row {0} - Missing required email address.<br/>", rowIndex));
                        }
                    }

                }
                catch (Exception ex)
                {
                    errorRows++;
                    sb.Append(ex.Message + Environment.NewLine);
                }
                rowIndex++;
            }
            dataReader.Close();
            connection.Close();
            context.Save();
            return new Tuple<string, int>(string.Format("File successfully processed. {0} imported; {1} errors<br/>{2}", insertedRows, errorRows, sb.ToString()), insertedRows);
        }
        */

        public List<PrincipalInvestorQuickViewModel> GetPrincipalInvestors(PrincipalInvestorSearchModel model)
        {
            var list = new List<PrincipalInvestorQuickViewModel>();
            var context = _factory.Create();
            IQueryable<PrincipalInvestor> investors = context.PrincipalInvestors.Where(x => x.Registered == null || x.Registered == false);
            if (model.ShowActiveOnly)
            {
                investors = context.PrincipalInvestors.Where(w => w.IsActive);
            }
            if (!string.IsNullOrEmpty(model.AddressLine1))
            {
                investors = investors.Where(w => w.CompanyAddressLine1 == model.AddressLine1);
            }
            if (!string.IsNullOrEmpty(model.City))
            {
                investors = investors.Where(w => w.CompanyCity == model.City);
            }
            if (!string.IsNullOrEmpty(model.FirstName))
            {
                investors = investors.Where(w => w.FirstName == model.FirstName);
            }
            if (!string.IsNullOrEmpty(model.LastName))
            {
                investors = investors.Where(w => w.LastName == model.LastName);
            }
            if (!string.IsNullOrEmpty(model.State))
            {
                investors = investors.Where(w => w.CompanyState == model.State);
            }
            if (!string.IsNullOrEmpty(model.Zip))
            {
                investors = investors.Where(w => w.CompanyZip == model.Zip);
            }
            if (!string.IsNullOrEmpty(model.CompanyName))
            {
                investors = investors.Where(w => w.CompanyName == model.CompanyName);
            }
            if (!string.IsNullOrEmpty(model.Email))
            {
                if (model.DomainSearch)
                {
                    // split the string by the @ char and search with the right side of the result
                    // This isnt a true domain only search since we match on the left side of the email in the DB but the user shouldnt mind
                    string email = model.Email.Contains('@') ? email = model.Email.Split('@')[1] : email = model.Email;
                    investors = investors.Where(x => x.Email.Contains(email));
                }
                else
                {
                    investors = investors.Where(w => w.Email == model.Email);
                }
            }
            investors.ToList().ForEach(s =>
            {
                list.Add(new PrincipalInvestorQuickViewModel()
                {
                    CellPhoneNumber = s.CellPhoneNumber,
                    FaxNumber = s.FaxNumber,
                    WorkNumber = s.WorkPhoneNumber,
                    AddressLine1 = s.CompanyAddressLine1,
                    AddressLine2 = s.CompanyAddressLine2,
                    City = s.CompanyCity,
                    CompanyName = s.CompanyName,
                    Email = s.Email,
                    FirstName = s.FirstName,
                    IsActive = s.IsActive,
                    LastName = s.LastName,
                    PrincipalInvestorId = s.PrincipalInvestorId,
                    State = s.CompanyState,
                    Zip = s.CompanyZip,
                    Country = s.Country
                });
            });
            return list;
        }

        public bool CreateOperatingCompany(OperatingCompanyViewModel model)
        {
            bool success = true;
            try
            {
                var context = _factory.Create();
                OperatingCompany entity = new OperatingCompany();
                entity.OperatingCompanyId = model.OperatingCompanyId;
                entity.CompanyName = model.CompanyName;
                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.Email = model.Email;
                entity.AddressLine1 = model.AddressLine1;
                entity.AddressLine2 = model.AddressLine2;
                entity.City = model.City;
                entity.State = model.State;
                entity.Zip = model.Zip;
                entity.Country = model.Country;
                entity.WorkNumber = model.WorkNumber;
                entity.CellNumber = model.CellNumber;
                
                entity.FaxNumber = model.FaxNumber;

                entity.LinkedIn = model.LinkedIn;
                entity.Facebook = model.Facebook;
                entity.Instagram = model.Instagram;
                entity.Twitter = model.Twitter;

                entity.IsActive = model.IsActive;

                context.OperatingCompanies.Add(entity);
                context.Save();
            }
            catch { success = false; }
            return success;
        }

        public OperatingCompanyViewModel GetOperatingCompany(Guid id)
        {
            var context = _factory.Create();
            var entity = context.OperatingCompanies.FirstOrDefault(s => s.OperatingCompanyId == id);
            if (entity != null)
            {
                return new OperatingCompanyViewModel()
                {
                    CellNumber = entity.CellNumber,
                    FaxNumber = entity.FaxNumber,
                    WorkNumber = entity.WorkNumber,
                    AddressLine1 = entity.AddressLine1,
                    AddressLine2 = entity.AddressLine2,
                    City = entity.City,
                    CompanyName = entity.CompanyName,
                    Email = entity.Email,
                    FirstName = entity.FirstName,
                    IsActive = entity.IsActive,
                    LastName = entity.LastName,
                    OperatingCompanyId = entity.OperatingCompanyId,
                    State = entity.State,
                    Zip = entity.Zip,
                    Country = entity.Country
                };
            }
            return null;
        }

        

        public PrincipalInvestorQuickViewModel GetPrincipalInvestor(int id)
        {
            var context = _factory.Create();
            var investor = context.PrincipalInvestors.FirstOrDefault(s => s.PrincipalInvestorId == id);
            if (investor != null)
            {
                return new PrincipalInvestorQuickViewModel()
                {
                    CellPhoneNumber = investor.CellPhoneNumber,
                    FaxNumber = investor.FaxNumber,
                    WorkNumber = investor.WorkPhoneNumber,
                    AddressLine1 = investor.CompanyAddressLine1,
                    AddressLine2 = investor.CompanyAddressLine2,
                    City = investor.CompanyCity,
                    CompanyName = investor.CompanyName,
                    Email = investor.Email,
                    FirstName = investor.FirstName,
                    IsActive = investor.IsActive,
                    LastName = investor.LastName,
                    PrincipalInvestorId = investor.PrincipalInvestorId,
                    State = investor.CompanyState,
                    Zip = investor.CompanyZip,
                    Country = investor.Country
                };
            }
            return null;
        }

        public OperatingCompanyViewModel GetOperatingCompanyForAssetContract(Guid id)
        {
            var context = _factory.Create();
            var entity = (from o in context.OperatingCompanies
                          join hc in context.HoldingCompanies on o.OperatingCompanyId equals hc.OperatingCompanyId into hcGroup
                          where o.OperatingCompanyId == id
                          select new OperatingCompanyViewModel()
                          {
                              HoldingCompanies = (from hc in context.HoldingCompanies
                                                  where hc.OperatingCompanyId == id
                                                  select new HoldingCompanyViewModel()
                                                  {
                                                      Assets = (from a in context.Assets
                                                                where a.OwnerHoldingCompanyId == hc.HoldingCompanyId
                                                                select new HoldingCompanyAssetSimpleViewModel()
                                                                {
                                                                    AssetId = a.AssetId,
                                                                    AssetNumber = a.AssetNumber
                                                                }).ToList(),
                                                      HoldingCompanyId = hc.HoldingCompanyId,
                                                      CompanyName = hc.CompanyName,
                                                      FirstName = hc.FirstName,
                                                      LastName = hc.LastName,
                                                      Email = hc.Email,
                                                      AddressLine1 = hc.AddressLine1,
                                                      AddressLine2 = hc.AddressLine2,
                                                      City = hc.City,
                                                      State = hc.State,
                                                      Zip = hc.Zip,
                                                      Country = hc.Country,
                                                      WorkNumber = hc.WorkNumber,
                                                      CellNumber = hc.CellNumber,
                                                      FaxNumber = hc.FaxNumber,
                                                      IsActive = hc.IsActive,
                                                  }).ToList(),
                              CellNumber = o.CellNumber,
                              FaxNumber = o.FaxNumber,
                              WorkNumber = o.WorkNumber,
                              AddressLine1 = o.AddressLine1,
                              AddressLine2 = o.AddressLine2,
                              City = o.City,
                              CompanyName = o.CompanyName,
                              Email = o.Email,
                              FirstName = o.FirstName,
                              IsActive = o.IsActive,
                              LastName = o.LastName,
                              OperatingCompanyId = o.OperatingCompanyId,
                              State = o.State,
                              Zip = o.Zip,
                              Country = o.Country
                          }).FirstOrDefault();

            return entity;
        }

        public PrincipalInvestorQuickViewModel GetPrincipalInvestorByEmail(string email)
        {
            var context = _factory.Create();
            var investor = context.PrincipalInvestors.FirstOrDefault(s => s.Email == email);
            if (investor != null)
            {
                return new PrincipalInvestorQuickViewModel()
                {
                    CellPhoneNumber = investor.CellPhoneNumber,
                    FaxNumber = investor.FaxNumber,
                    WorkNumber = investor.WorkPhoneNumber,
                    AddressLine1 = investor.CompanyAddressLine1,
                    AddressLine2 = investor.CompanyAddressLine2,
                    City = investor.CompanyCity,
                    CompanyName = investor.CompanyName,
                    Email = investor.Email,
                    FirstName = investor.FirstName,
                    IsActive = investor.IsActive,
                    LastName = investor.LastName,
                    PrincipalInvestorId = investor.PrincipalInvestorId,
                    State = investor.CompanyState,
                    Zip = investor.CompanyZip,
                    Country = investor.Country
                };
            }
            return null;
        }

        public void DeactivePrincipalInvestor(int id)
        {
            var context = _factory.Create();
            var investor = context.PrincipalInvestors.FirstOrDefault(w => w.PrincipalInvestorId == id);
            if (investor != null)
            {
                investor.IsActive = false;
                context.Save();
            }
        }

        public void ReactivatePrincipalInvestor(int id)
        {
            var context = _factory.Create();
            var investor = context.PrincipalInvestors.FirstOrDefault(w => w.PrincipalInvestorId == id);
            if (investor != null)
            {
                investor.IsActive = true;
                context.Save();
            }
        }

        public void DeactivateOperatingCompany(Guid id)
        {
            var context = _factory.Create();
            var entity = context.OperatingCompanies.FirstOrDefault(w => w.OperatingCompanyId == id);
            if (entity != null)
            {
                entity.IsActive = false;
                context.Save();
            }
        }

        public void ReactivateOperatingCompany(Guid id)
        {
            var context = _factory.Create();
            var entity = context.OperatingCompanies.FirstOrDefault(w => w.OperatingCompanyId == id);
            if (entity != null)
            {
                entity.IsActive = true;
                context.Save();
            }
        }

        public void UpdatePrincipalInvestor(PrincipalInvestorQuickViewModel model)
        {
            var context = _factory.Create();
            var investor = context.PrincipalInvestors.FirstOrDefault(w => w.PrincipalInvestorId == model.PrincipalInvestorId);
            if (investor != null)
            {
                investor.CellPhoneNumber = model.CellPhoneNumber;
                investor.WorkPhoneNumber = model.WorkNumber;
                investor.FaxNumber = model.FaxNumber;
                investor.CompanyAddressLine1 = model.AddressLine1;
                investor.CompanyAddressLine2 = model.AddressLine2;
                investor.CompanyCity = model.City;
                investor.CompanyName = model.CompanyName;
                investor.CompanyState = model.State;
                investor.CompanyZip = model.Zip;
                investor.Country = model.Country;
                investor.Email = model.Email;
                investor.FirstName = model.FirstName;
                investor.IsActive = model.IsActive;
                investor.LastName = model.LastName;
                investor.PrincipalInvestorId = model.PrincipalInvestorId;
                context.Save();
            }
        }

        public void UpdateOperatingCompany(OperatingCompanyViewModel model)
        {
            var context = _factory.Create();
            foreach (var hc in model.HoldingCompanies)
            {
                var entity = context.HoldingCompanies.FirstOrDefault(h => h.HoldingCompanyId == hc.HoldingCompanyId);
                if (entity != null)
                {
                    entity.CompanyName = hc.CompanyName;
                    entity.FirstName = hc.FirstName;
                    entity.LastName = hc.LastName;
                    entity.Email = hc.Email;
                    entity.AddressLine1 = hc.AddressLine1;
                    entity.AddressLine2 = hc.AddressLine2;
                    entity.City = hc.City;
                    entity.State = hc.State;
                    entity.Zip = hc.Zip;
                    entity.Country = hc.Country;
                    entity.WorkNumber = hc.WorkNumber;
                    entity.CellNumber = hc.CellNumber;
                    
                    entity.FaxNumber = hc.FaxNumber;

                    entity.LinkedIn = hc.LinkedIn;
                    entity.Facebook = hc.Facebook;
                    entity.Instagram = hc.Instagram;
                    entity.Twitter = hc.Twitter;

                    entity.IsActive = hc.IsActive;
                    context.Save();
                }
                else
                {
                    // create?
                }
            }
            var o = context.OperatingCompanies.FirstOrDefault(w => w.OperatingCompanyId == model.OperatingCompanyId);
            if (o != null)
            {
                if (model.OperatingCompanyId == new Guid())
                {
                    o.CellNumber = "";
                    o.WorkNumber = "";

                    o.FaxNumber = "";
                    o.LinkedIn = "";
                    o.Facebook = "";
                    o.Instagram = "";
                    o.Twitter = "";

                    o.AddressLine1 = "";
                    o.AddressLine2 = "";
                    o.City = "";
                    o.CompanyName = "Unknown";
                    o.State = "";
                    o.Zip = "";
                    o.Country = "";
                    o.Email = "";
                    o.FirstName = "";
                    o.IsActive = true;
                    o.LastName = "";
                    o.OperatingCompanyId = model.OperatingCompanyId;
                    context.Save();
                }
                else
                {
                    o.CellNumber = model.CellNumber;
                    o.WorkNumber = model.WorkNumber;
                    o.FaxNumber = model.FaxNumber;

                    o.LinkedIn = model.LinkedIn;
                    o.Facebook = model.Facebook;
                    o.Instagram = model.Instagram;
                    o.Twitter = model.Twitter;

                    o.AddressLine1 = model.AddressLine1;
                    o.AddressLine2 = model.AddressLine2;
                    o.City = model.City;
                    o.CompanyName = model.CompanyName;
                    o.State = model.State;
                    o.Zip = model.Zip;
                    o.Country = model.Country;
                    o.Email = model.Email;
                    o.FirstName = model.FirstName;
                    o.IsActive = model.IsActive;
                    o.LastName = model.LastName;
                    o.OperatingCompanyId = model.OperatingCompanyId; // we probably should not even set this
                    context.Save();
                }
            }
            else
            {
                if (model.OperatingCompanyId == new Guid())
                {
                    // create Unknown placeholder company since it doesnt exist yet
                    OperatingCompany oc = new OperatingCompany();
                    oc.CompanyName = model.CompanyName;
                    oc.OperatingCompanyId = model.OperatingCompanyId;
                    oc.IsActive = true;
                    context.OperatingCompanies.Add(oc);
                    context.Save();
                }
            }
        }

        public void DeactivateHoldingCompany(Guid id)
        {
            var context = _factory.Create();
            var entity = context.HoldingCompanies.FirstOrDefault(w => w.HoldingCompanyId == id);
            if (entity != null)
            {
                entity.IsActive = false;
                context.Save();
            }
        }

        public void ReactivateHoldingCompany(Guid id)
        {
            var context = _factory.Create();
            var entity = context.HoldingCompanies.FirstOrDefault(w => w.HoldingCompanyId == id);
            if (entity != null)
            {
                entity.IsActive = true;
                context.Save();
            }
        }

        public void CreatePrincipalInvestor(PrincipalInvestorQuickViewModel model)
        {
            var context = _factory.Create();
            context.PrincipalInvestors.Add(new PrincipalInvestor()
            {
                FaxNumber = model.FaxNumber,
                CellPhoneNumber = model.CellPhoneNumber,
                WorkPhoneNumber = model.WorkNumber,
                CompanyAddressLine1 = model.AddressLine1,
                CompanyAddressLine2 = model.AddressLine2,
                CompanyCity = model.City,
                CompanyName = model.CompanyName,
                CompanyState = model.State,
                CompanyZip = model.Zip,
                Country = model.Country,
                Email = model.Email.ToLower(),
                FirstName = model.FirstName,
                IsActive = model.IsActive,
                LastName = model.LastName,
                PrincipalInvestorId = model.PrincipalInvestorId,
                Registered = false
            });
            context.Save();

        }

        public void ReactivateUser(int id)
        {
            var context = _factory.Create();
            var user = context.Users.FirstOrDefault(x => x.UserId == id);
            if (user != null)
            {
                user.IsActive = true;
                context.Save();
            }
        }

        public void SetICInformation(ICAgreementTemplateModel model, int userId)
        {
            var context = _factory.Create();
            var user = context.Users.Where(x => x.UserId == userId).FirstOrDefault();
            if (user != null)
            {
                user.LicenseDesc = model.LicenseDesc;
                user.LicenseNumber = model.LicenseNumber;
                user.CorporateTIN = model.CorporateTIN;
                context.Save();
            }
        }

        public string CanUserUploadMBA(string userName)
        {
            var context = _factory.Create();
            var user = context.Users.Where(x => x.Username == userName).FirstOrDefault();
            if (user.UserType == UserType.CREBroker || user.UserType == UserType.CRELender)
            {
                if (!string.IsNullOrEmpty(user.JVMarketerAgreementLocation))
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
            else
            {
                return "false";
            }
        }

        public void PerformTediousTaskThatWouldTakeTooLongWithoutCode()
        {
            var context = _factory.Create();
            // transfer all IC Payouts to other table
            var payouts = context.ICAdminContractFeePayouts.ToList();
            foreach (var p in payouts)
            {
                context.ContractFeePayouts.Add(new ContractFeePayout()
                {
                    DatePaid = p.DatePaid,
                    FeeAmount = p.FeeAmount,
                    RecordedByUserId = p.RecordedByUserId,
                    Type = ContractFeePayoutType.icAdmin,
                    UserId = p.UserId
                });
            }

            // add a few things to the database, for reasons
            //var user = context.Users.Find(158);
            //context.UserRecords.Add(new UserRecord()
            //{
            //    Date = DateTime.Now,
            //    User = user,
            //    UserId = user.UserId,
            //    UserRecordType = UserRecordType.DFSubmittal
            //});
            //context.UserRecords.Add(new UserRecord()
            //{
            //    Date = DateTime.Now,
            //    User = user,
            //    UserId = user.UserId,
            //    UserRecordType = UserRecordType.LOISubmittal
            //});

            // convert AssetListingAgents into AssetNarMembers table
            //List<string> processedListingAgents = new List<string>();
            //var listingAgents = context.AssetListingAgents
            //    .Where(x => !string.IsNullOrEmpty(x.ListingAgentNewName) && !string.IsNullOrEmpty(x.ListingAgentEmail))
            //    .OrderBy(x => x.ListingAgentNewName).ToList();
            //foreach (var la in listingAgents)
            //{
            //    if (!processedListingAgents.Any(x => x == la.ListingAgentNewName + la.AssetId.ToString()))
            //    {
            //        int id = Convert.ToInt32(la.ListingAgentNewName);
            //        var asset = context.Assets.Where(x => x.AssetId == la.AssetId).FirstOrDefault();
            //        var narMem = context.NarMembers.Where(x => x.NarMemberId == id).FirstOrDefault();
            //        if (asset != null && narMem != null)
            //        {
            //            var anm = new AssetNARMember();
            //            anm.AssetNARMemberId = Guid.NewGuid();
            //            anm.Asset = asset;
            //            anm.AssetId = asset.AssetId;
            //            anm.NARMember = narMem;
            //            anm.NarMemberId = narMem.NarMemberId;
            //            context.AssetNARMembers.Add(anm);
            //        }
            //        processedListingAgents.Add(la.ListingAgentNewName + la.AssetId.ToString()); 
            //    }
            //}

            // fixing 1 line NAR Member addresses
            //foreach (var mem in context.NarMembers)
            //{
            //    if (!string.IsNullOrEmpty(mem.CompanyAddressLine1))
            //    {
            //        string[] array = mem.CompanyAddressLine1.Split(',');
            //        if (array.Length == 5)
            //        {
            //            mem.CompanyAddressLine1 = array[0];
            //            mem.CompanyAddressLine2 = array[1];
            //            mem.CompanyCity = array[2];
            //            mem.CompanyState = array[3];
            //            mem.CompanyZip = array[4];
            //        }
            //    }
            //}

            // move values to correct fields
            //foreach (var user in context.Users)
            //{
            //    if (!string.IsNullOrEmpty(user.LicenseDesc) && string.IsNullOrEmpty(user.StateLicenseDesc))
            //    {
            //        user.StateLicenseDesc = user.LicenseDesc;
            //        user.LicenseDesc = null;
            //    }
            //    if (!string.IsNullOrEmpty(user.LicenseNumber) && string.IsNullOrEmpty(user.StateLicenseNumber))
            //    {
            //        user.StateLicenseNumber = user.LicenseNumber;
            //        user.LicenseNumber = null;
            //    }
            //}

            context.Save();
        }

        public bool VerifyNarMember(NarMemberViewModel model)
        {
            var context = _factory.Create();
            return context.NarMembers.Any(x => x.Email.ToLower() == model.Email.ToLower());
        }

        public User GetPayer(int fiscalYear, int userId, ContractFeePayoutType type)
        {
            var context = _factory.Create();
            var payouts = context.ContractFeePayouts.Where(w => w.UserId == userId && w.DatePaid.Year == fiscalYear && w.Type == type).OrderBy(w => w.DatePaid).ToList();
            if (payouts.Count > 0)
            {
                int payerId = payouts.FirstOrDefault().RecordedByUserId;
                var user = context.Users.Where(x => x.UserId == payerId).FirstOrDefault();
                if (user != null)
                    return user;
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        public bool HasOrderHistory(int userId)
        {
            var context = _factory.Create();
            //return context.Assets.Any(x => x.OrderedByUserId == userId);
            var count = context.Assets.Where(a => a.OrderedByUserId == userId).Count();
            return count > 0 ? true : false;
        }

        public List<OrderHistoryQuickListViewModel> GetOrdersForUser(int userId)
        {
            var context = _factory.Create();
            List<OrderHistoryQuickListViewModel> history = new List<OrderHistoryQuickListViewModel>();
            var assetsByOrderedUserId = context.Assets.Where(x => x.OrderedByUserId == userId).ToList();
            if (assetsByOrderedUserId != null)
            {
                foreach (var a in assetsByOrderedUserId)
                {
                    string taxNums = string.Empty;
                    var apns = context.AssetTaxParcelNumbers.Where(x => x.AssetId == a.AssetId).ToList();
                    if (apns != null && apns.Count > 0)
                        taxNums = string.Join(",", apns.Select(x => x.TaxParcelNumber).ToArray());
                    history.Add(new OrderHistoryQuickListViewModel()
                    {
                        OrderDate = a.OrderDate.Value,
                        Status = a.OrderStatus,
                        ProjectName = a.ProjectName,
                        AssetNumber = a.AssetNumber.ToString(),
                        Address = a.PropertyAddress,
                        TaxParcelNumber = taxNums
                    });
                }
            }
            return history;
        }

        public void RemoveUserAssetLocks(string Username)
        {
            try
            {
                if (!string.IsNullOrEmpty(Username))
                {
                    var context = _factory.Create();
                    var user = context.Users.Where(x => x.Username == Username.ToLower()).FirstOrDefault();
                    if (user != null)
                    {
                        var currentUserLocks = context.AssetLocks.Where(x => x.UserId == user.UserId).ToList();
                        if (currentUserLocks.Count > 0)
                        {
                            foreach (var assetLock in currentUserLocks)
                            {
                                context.AssetLocks.Remove(assetLock);
                            }
                            context.Save();
                        }
                    }
                }
            }
            catch { }
        }

        public void DeleteAssetListingAgent(Guid id)
        {
            var context = _factory.Create();
            var ala = context.AssetNARMembers.Find(id);
            if (ala != null)
            {
                context.AssetNARMembers.Remove(ala);
                context.Save();
            }
        }

        public bool NARExist(string email)
        {
            var context = _factory.Create();
            return context.NarMembers.Any(x => x.Email == email.ToLower());
        }

        public List<JVMANetUpViewModel> GetJVMAUserNetworkUploads(JVMANetUpSearchModel model, int userId)
        {
            // TODO: get correct users, filter assettypes, get correct dates in model
            var context = _factory.Create();
            //var uploadsOld = (from u in context.Users
            //               join rf in context.UserReferrals on u.Username equals rf.ReferralEmail into rfGroup
            //               from item in rfGroup.DefaultIfEmpty()
            //               join rec in context.UserRecords on u.UserId equals rec.UserId into recGroup
            //               where u.ReferredByUserId == userId &&
            //               u.IsActive
            //               select new JVMANetUpViewModel()
            //               {
            //                   FirstName = u.FirstName,
            //                   LastName = u.LastName,
            //                   City = u.City,
            //                   State = u.State,
            //                   DateOfUpload = null,
            //                   DateRefRegistered = u.SignupDate,
            //                   UserType = u.UserType,
            //                   DateOfDFSubmittal = null,
            //                   DateOfLOISubmittal = null,
            //                   UserId = u.UserId,
            //                   UserRecords = recGroup,
            //                   UserReferral = item
            //               }).ToList();

            var uploads = (from uf in context.UserReferrals
                           join u in context.Users on uf.ReferralEmail equals u.Username into uGroup
                           from uItem in uGroup.DefaultIfEmpty()
                           join rec in context.UserRecords on uItem.UserId equals rec.UserId into recGroup
                           where uf.UserId == userId
                           select new JVMANetUpViewModel()
                           {
                               FirstName = uf.FirstName,
                               LastName = uf.LastName,
                               User = uItem,
                               City = uf.City,
                               State = uf.State,
                               DateOfUpload = uf.CreateDate,
                               //DateRefRegistered = u.SignupDate,
                               //UserType = u.UserType,
                               //DateOfDFSubmittal = null,
                               //DateOfLOISubmittal = null,
                               //UserId = u.UserId,
                               UserRecords = recGroup,
                               //UserReferral = rfItem
                           }).ToList();

            foreach (var user in uploads)
            {
                if (user.User != null)
                {
                    user.DateRefRegistered = user.User.SignupDate;
                    user.UserType = user.User.UserType;
                    user.City = user.User.City;
                    user.State = user.User.State;
                    user.UserId = user.User.UserId;
                    user.FirstName = user.User.FirstName;
                    user.LastName = user.User.LastName;
                    var mdas = (from m in context.AssetUserMDAs
                                join a in context.Assets on m.AssetId equals a.AssetId
                                where m.UserId == user.UserId
                                select new AssetUserMDATempModel()
                                {
                                    AssetType = a.AssetType
                                }).ToList();
                    user.MDAs = mdas.Count;
                    if (mdas.Count > 0)
                    {
                        List<string> assetsOfInterest = new List<string>();
                        if (mdas.Any(x => x.AssetType == AssetType.MultiFamily))
                            assetsOfInterest.Add("MF");
                        if (mdas.Any(x => x.AssetType == AssetType.MHP))
                            assetsOfInterest.Add("MHP");
                        if (mdas.Any(x => x.AssetType == AssetType.Office))
                            assetsOfInterest.Add("CO");
                        if (mdas.Any(x => x.AssetType == AssetType.Retail))
                            assetsOfInterest.Add("CR");
                        if (mdas.Any(x => x.AssetType == AssetType.Medical))
                            assetsOfInterest.Add("Med");
                        if (mdas.Any(x => x.AssetType == AssetType.MixedUse))
                            assetsOfInterest.Add("Mix");
                        if (mdas.Any(x => x.AssetType == AssetType.Industrial))
                            assetsOfInterest.Add("Ind");
                        if (mdas.Any(x => x.AssetType == AssetType.Hotel))
                            assetsOfInterest.Add("Hot");
                        if (mdas.Any(x => x.AssetType == AssetType.ConvenienceStoreFuel))
                            assetsOfInterest.Add("SF");
                        user.AssetTypes = string.Join(",", assetsOfInterest);
                    }
                    if (user.UserType.HasValue) // should always
                    {
                        user.UserTypeString = GetUserTypeAcronym((UserType)user.UserType);
                    }
                    if (user.UserRecords != null)
                    {
                        foreach (var rec in user.UserRecords.OrderBy(x => x.Date))
                        {
                            if (rec.UserRecordType == UserRecordType.DFSubmittal)
                            {
                                user.DateOfDFSubmittal = rec.Date;
                            }
                            else if (rec.UserRecordType == UserRecordType.LOISubmittal)
                            {
                                user.DateOfLOISubmittal = rec.Date;
                            }
                        }
                    }
                    //var records = context.UserRecords.Where(x => x.UserId == user.UserId).ToList();
                    //if (records.Count > 0)
                    //{
                    //    foreach (var rec in records.OrderBy(x => x.Date))
                    //    {
                    //        if (rec.UserRecordType == UserRecordType.DFSubmittal)
                    //        {
                    //            user.DateOfDFSubmittal = rec.Date;
                    //        }
                    //        else if (rec.UserRecordType == UserRecordType.LOISubmittal)
                    //        {
                    //            user.DateOfLOISubmittal = rec.Date;
                    //        }
                    //    }
                    //}
                    //if (user.UserReferral != null)
                    //{
                    //    user.DateOfUpload = user.UserReferral.CreateDate;
                    //    if (user.UserReferral.Registered)
                    //    {

                    //    }
                    //}
                }
            }

            //foreach (var user in uploadsOld)
            //{
            //    // iterate through the users to set Asset Type Interest. If there is a better way we should go with it
            //    var mdas = (from m in context.AssetUserMDAs
            //                join a in context.Assets on m.AssetId equals a.AssetId
            //                where m.UserId == user.UserId
            //                select new AssetUserMDATempModel()
            //                {
            //                    AssetType = a.AssetType
            //                }).ToList();
            //    user.MDAs = mdas.Count;
            //    if (mdas.Count > 0)
            //    {
            //        List<string> assetsOfInterest = new List<string>();
            //        if (mdas.Any(x => x.AssetType == AssetType.MultiFamily))
            //            assetsOfInterest.Add("MF");
            //        if (mdas.Any(x => x.AssetType == AssetType.MHP))
            //            assetsOfInterest.Add("MHP");
            //        if (mdas.Any(x => x.AssetType == AssetType.Office))
            //            assetsOfInterest.Add("CO");
            //        if (mdas.Any(x => x.AssetType == AssetType.Retail))
            //            assetsOfInterest.Add("CR");
            //        if (mdas.Any(x => x.AssetType == AssetType.Medical))
            //            assetsOfInterest.Add("Med");
            //        if (mdas.Any(x => x.AssetType == AssetType.MixedUse))
            //            assetsOfInterest.Add("Mix");
            //        if (mdas.Any(x => x.AssetType == AssetType.Industrial))
            //            assetsOfInterest.Add("Ind");
            //        if (mdas.Any(x => x.AssetType == AssetType.Hotel))
            //            assetsOfInterest.Add("Hot");
            //        if (mdas.Any(x => x.AssetType == AssetType.ConvenienceStoreFuel))
            //            assetsOfInterest.Add("SF");
            //        user.AssetTypes = string.Join(",", assetsOfInterest);
            //    }
            //    if (user.UserType.HasValue) // should always
            //    {
            //        user.UserTypeString = GetUserTypeAcronym((UserType)user.UserType);
            //    }
            //    if (user.UserRecords != null)
            //    {
            //        foreach (var rec in user.UserRecords.OrderBy(x => x.Date))
            //        {
            //            if (rec.UserRecordType == UserRecordType.DFSubmittal)
            //            {
            //                user.DateOfDFSubmittal = rec.Date;
            //            }
            //            else if (rec.UserRecordType == UserRecordType.LOISubmittal)
            //            {
            //                user.DateOfLOISubmittal = rec.Date;
            //            }
            //        }
            //    }
            //    if (user.UserReferral != null)
            //    {
            //        user.DateOfUpload = user.UserReferral.CreateDate;
            //        if (user.UserReferral.Registered)
            //        {

            //        }
            //    }
            //}
            if (!string.IsNullOrEmpty(model.FirstName))
            {
                uploads = uploads.Where(x => !string.IsNullOrEmpty(x.FirstName) && x.FirstName.ToLower().Contains(model.FirstName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.LastName))
            {
                uploads = uploads.Where(x => !string.IsNullOrEmpty(x.LastName) && x.LastName.ToLower().Contains(model.LastName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.City))
            {
                uploads = uploads.Where(x => !string.IsNullOrEmpty(x.City) && x.City.ToLower().Contains(model.City.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.State))
            {
                uploads = uploads.Where(x => !string.IsNullOrEmpty(x.State) && x.State.ToLower().Contains(model.State.ToLower())).ToList();
            }

            if (model.DateOfUploadStart.HasValue)
            {
                uploads = uploads.Where(a => a.DateOfUpload >= model.DateOfUploadStart.Value).ToList();
            }
            if (model.DateOfUploadEnd.HasValue)
            {
                uploads = uploads.Where(a => a.DateOfUpload <= model.DateOfUploadEnd.Value).ToList();
            }
            if (model.DateRefRegisteredStart.HasValue)
            {
                uploads = uploads.Where(a => a.DateRefRegistered >= model.DateRefRegisteredStart.Value).ToList();
            }
            if (model.DateRefRegisteredEnd.HasValue)
            {
                uploads = uploads.Where(a => a.DateRefRegistered <= model.DateRefRegisteredEnd.Value).ToList();
            }

            if (model.UserType.HasValue)
            {
                uploads = uploads.Where(x => x.UserType == model.UserType).ToList();
            }
            if (!string.IsNullOrEmpty(model.AssetType) && model.AssetType != "All")
            {
                uploads = uploads.Where(x => !string.IsNullOrEmpty(x.AssetTypes) && x.AssetTypes.Contains(model.AssetType)).ToList();
            }

            if (model.DateOfDFSubmittalStart.HasValue)
            {
                uploads = uploads.Where(a => a.DateOfDFSubmittal >= model.DateOfDFSubmittalStart.Value).ToList();
            }
            if (model.DateOfDFSubmittalEnd.HasValue)
            {
                uploads = uploads.Where(a => a.DateOfDFSubmittal <= model.DateOfDFSubmittalEnd.Value).ToList();
            }
            if (model.DateOfLOISubmittalStart.HasValue)
            {
                uploads = uploads.Where(a => a.DateOfLOISubmittal >= model.DateOfLOISubmittalStart.Value).ToList();
            }
            if (model.DateOfLOISubmittalEnd.HasValue)
            {
                uploads = uploads.Where(a => a.DateOfLOISubmittal <= model.DateOfLOISubmittalEnd.Value).ToList();
            }
            return uploads.ToList();
        }

        public JVMAUserMDAViewModels GetJVMAUserMDAHistory(JVMAUserMDASearchModel model)
        {
            //TODO: Get proper dates
            var context = _factory.Create();
            var juvm = new JVMAUserMDAViewModels();
            var user = context.Users.First(x => x.Username == model.Username.ToLower());
            var referredUser = context.Users.Find(model.ReferralUserId);
            juvm.ParticipantFullName = user.FullName;
            juvm.ReferredUserFullName = referredUser.FullName;
            juvm.UserId = referredUser.UserId;
            var mdas = (from m in context.AssetUserMDAs
                        join a in context.Assets on m.AssetId equals a.AssetId
                        join u in context.Users on m.UserId equals u.UserId
                        join r in context.UserRecords on m.UserId equals r.UserId into rGroup

                        where m.UserId == model.ReferralUserId &&
                        u.IsActive &&
                        a.IsActive &&
                        a.IsSubmitted
                        select new JVMAUserMDAViewModel()
                        {
                            AssetId = a.AssetId,
                            AssetNumber = a.AssetNumber,
                            State = a.State,
                            DateOfMDA = m.SignMDADate,
                            DateOfDFSubmittal = null,
                            DateOfLOISubmittal = null,
                            ActualCOE = a.ActualClosingDate,
                            ProposedCOE = a.ClosingDate,
                            DateRefFeePaid = null,
                            UserRecords = rGroup,
                            AssetType = a.AssetType
                        }).ToList();
            var payouts = context.ContractFeePayouts.Where(x => x.UserId == user.UserId && x.Type == ContractFeePayoutType.jvma).OrderBy(x => x.DatePaid).ToList();
            foreach (var mda in mdas)
            {
                mda.AssetTypeString = GetAssetTypeAcronym(mda.AssetType);
                if (mda.UserRecords != null)
                {
                    foreach (var rec in mda.UserRecords.OrderBy(x => x.Date))
                    {
                        if (rec.UserRecordType == UserRecordType.DFSubmittal)
                        {
                            mda.DateOfDFSubmittal = rec.Date;
                        }
                        else if (rec.UserRecordType == UserRecordType.LOISubmittal)
                        {
                            mda.DateOfLOISubmittal = rec.Date;
                        }
                    }
                }
                if (payouts.Count > 0)
                {
                    // get the latest payout for a particular mda and assign the date to the mda
                    var payout = payouts.Where(x => x.DatePaid >= mda.DateOfMDA).FirstOrDefault();
                    if (payout != null)
                    {
                        mda.DateRefFeePaid = payout.DatePaid;
                        mda.RefFeePaid = payout.FeeAmount.ToString();
                    }
                }
            }
            if (model.AssetNumber.HasValue)
            {
                mdas = mdas.Where(x => x.AssetNumber == (int)model.AssetNumber).ToList();
            }
            if (!string.IsNullOrEmpty(model.AssetType) && model.AssetType != "All")
            {
                mdas = mdas.Where(x => x.AssetTypeString.Contains(model.AssetType)).ToList();
            }
            if (!string.IsNullOrEmpty(model.State))
            {
                mdas = mdas.Where(x => x.State == model.State).ToList();
            }

            if (model.DateOfMDAStart.HasValue)
            {
                mdas = mdas.Where(a => a.DateOfMDA >= model.DateOfMDAStart.Value).ToList();
            }
            if (model.DateOfMDAEnd.HasValue)
            {
                mdas = mdas.Where(a => a.DateOfMDA <= model.DateOfMDAEnd.Value).ToList();
            }
            if (model.DateOfDFSubmittalStart.HasValue)
            {
                mdas = mdas.Where(a => a.DateOfDFSubmittal >= model.DateOfDFSubmittalStart.Value).ToList();
            }
            if (model.DateOfDFSubmittalEnd.HasValue)
            {
                mdas = mdas.Where(a => a.DateOfDFSubmittal <= model.DateOfDFSubmittalEnd.Value).ToList();
            }
            if (model.DateOfLOISubmittalStart.HasValue)
            {
                mdas = mdas.Where(a => a.DateOfLOISubmittal >= model.DateOfLOISubmittalStart.Value).ToList();
            }
            if (model.DateOfLOISubmittalEnd.HasValue)
            {
                mdas = mdas.Where(a => a.DateOfLOISubmittal <= model.DateOfLOISubmittalEnd.Value).ToList();
            }

            if (model.ProposedCOEStart.HasValue)
            {
                mdas = mdas.Where(a => a.ProposedCOE >= model.ProposedCOEStart.Value).ToList();
            }
            if (model.ProposedCOEEnd.HasValue)
            {
                mdas = mdas.Where(a => a.ProposedCOE <= model.ProposedCOEEnd.Value).ToList();
            }
            if (model.ActualCOEStart.HasValue)
            {
                mdas = mdas.Where(a => a.ActualCOE >= model.ActualCOEStart.Value).ToList();
            }
            if (model.ActualCOEEnd.HasValue)
            {
                mdas = mdas.Where(a => a.ActualCOE <= model.ActualCOEEnd.Value).ToList();
            }
            if (model.DateRefFeePaidStart.HasValue)
            {
                mdas = mdas.Where(a => a.DateRefFeePaid >= model.DateRefFeePaidStart.Value).ToList();
            }
            if (model.DateRefFeePaidEnd.HasValue)
            {
                mdas = mdas.Where(a => a.DateRefFeePaid <= model.DateRefFeePaidEnd.Value).ToList();
            }
            juvm.Assets = mdas;
            return juvm;
        }

        public List<UserQuickViewModel> GetJVMAParticipants(UserSearchModel model)
        {
            var context = _factory.Create();
            //var list = context.Users.Where(x => x.UserType == UserType.CREBroker || x.UserType == UserType.CRELender).ToList();
            var list = (from u in context.Users
                        join pi in context.PrincipalInvestors on u.UserId equals pi.ReferredByUserId into piGroup
                        where u.UserType == UserType.CRELender ||
                        u.UserType == UserType.CREBroker
                        select new TempUserViewModel()
                        {
                            User = u,
                            PICount = piGroup.Count()
                        }).ToList();
            if (model.ShowActiveOnly)
            {
                list = list.Where(w => w.User.IsActive).ToList();
            }
            if (!string.IsNullOrEmpty(model.FirstName))
            {
                list = list.Where(w => w.User.FirstName != null && w.User.FirstName.ToLower().Contains(model.FirstName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.LastName))
            {
                list = list.Where(w => w.User.LastName != null && w.User.LastName.ToLower().Contains(model.LastName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.CorpEntity))
            {
                list = list.Where(x => x.User.CompanyName != null && x.User.CompanyName.ToLower().Contains(model.CorpEntity.ToLower())).ToList();
            }
            if (model.RegistrationDateStart.HasValue)
            {
                list = list.Where(a => a.User.SignupDate >= model.RegistrationDateStart.Value).ToList();
            }
            if (model.RegistrationDateEnd.HasValue)
            {
                list = list.Where(a => a.User.SignupDate <= model.RegistrationDateEnd.Value).ToList();
            }

            var returnlist = new List<UserQuickViewModel>();
            list.ForEach(f =>
            {
                var newModel = new UserQuickViewModel();
                newModel.FirstName = f.User.FirstName;
                newModel.LastName = f.User.LastName;
                newModel.UserId = f.User.UserId;
                newModel.UserType = f.User.UserType;
                newModel.Username = f.User.Username;
                newModel.TIN = f.User.CorporateTIN;
                newModel.IsActive = f.User.IsActive;
                newModel.RegisterDate = f.User.SignupDate != null ? f.User.SignupDate.GetValueOrDefault(DateTime.Now) : f.User.NCNDSignDate.GetValueOrDefault(DateTime.Now);
                newModel.InEscrow = context.Assets.Any(w => w.ProposedBuyerContact == f.User.FullName);
                newModel.UserTypeString = GetUserTypeAcronym(f.User.UserType);
                newModel.CompanyName = f.User.CompanyName;
                newModel.PendingEscrows = context.Assets.Count(x => x.ProposedBuyerContact == f.User.FullName && !x.ClosingDate.HasValue);
                newModel.HasJVMA = !string.IsNullOrEmpty(f.User.JVMarketerAgreementLocation);
                newModel.ReferralDB = f.PICount;
                returnlist.Add(newModel);
            });
            return returnlist;
        }

        public Tuple<bool, bool> CanUserBeReferred(string email)
        {
            var context = _factory.Create();
            return new Tuple<bool, bool>(context.Users.Any(x => x.Username == email.ToLower()), context.UserReferrals.Any(x => x.ReferralEmail == email.ToLower()));
        }

        public string ReferUser(string email, string username)
        {
            var context = _factory.Create();
            var referrer = context.Users.Where(x => x.Username == username.ToLower()).FirstOrDefault();
            if (referrer != null)
            {
                string code = GeneratePassword();
                email = email.ToLower();
                // check if user exist in nar/mba/pi tables, then create UserReferral. No reason for checking those tables
                // at the moment
                var nar = context.NarMembers.Where(x => x.Email == email).FirstOrDefault();
                if (nar != null)
                    nar.ReferredByUserId = referrer.UserId;
                var mba = context.MbaUsers.Where(x => x.Email == email).FirstOrDefault();
                if (mba != null)
                    mba.ReferredByUserId = referrer.UserId;
                var pi = context.PrincipalInvestors.Where(x => x.Email == email).FirstOrDefault();
                if (pi != null)
                    pi.ReferredByUserId = referrer.UserId;

                var referral = new UserReferral();
                referral.CreateDate = DateTime.Now;
                referral.ReferralEmail = email;
                referral.User = referrer;
                referral.UserId = referrer.UserId;
                referral.ReferralCode = code;
                context.UserReferrals.Add(referral);
                context.Save();
                return code;
            }
            return string.Empty;
        }

        public void LogUserRecord(UserRecordType type, int userId)
        {
            var context = _factory.Create();
            User user = context.Users.Find(userId);
            if (user != null)
            {
                UserRecord record = new UserRecord();
                record.Date = DateTime.Now;
                record.UserId = userId;
                record.User = user;
                record.UserRecordType = type;
                context.UserRecords.Add(record);
                context.Save();
            }
        }

        public void UpdateContractPayment(RecordContractPaymentModel model)
        {
            var context = _factory.Create();
            var payment = context.ContractFeePayouts.Find(model.ContractPaymentId);
            if (payment != null)
            {
                payment.DatePaid = model.PaymentDate;
                payment.FeeAmount = model.AmountPaid;
                context.Entry(payment).State = EntityState.Modified;
                context.Save();
            }
        }

        public void DeleteContractPayment(int id)
        {
            var context = _factory.Create();
            var payment = context.ContractFeePayouts.Find(id);
            if (payment != null)
            {
                context.Entry(payment).State = EntityState.Deleted;
                context.Save();
            }
        }

        public RecordContractPaymentModel GetContractPayment(int id)
        {
            var context = _factory.Create();
            var payment = context.ContractFeePayouts.Find(id);
            if (payment != null)
            {
                var model = new RecordContractPaymentModel();
                model.AmountPaid = payment.FeeAmount;
                model.PaymentDate = payment.DatePaid;
                model.ContractPaymentId = payment.ContractFeePayoutId;
                return model;
            }
            return null;
        }

        public string GetPIAssetHistoryDescriptionsHTML(string username)
        {
            var context = _factory.Create();
            StringBuilder assetDescription = new StringBuilder();
            StringBuilder descriptions = new StringBuilder();
            var user = context.Users.FirstOrDefault(x => x.Username == username.ToLower());
            var assets = context.Assets.Where(x => x.ListedByUserId == user.UserId).ToList();
            foreach (var asset in assets)
            {
                if (asset.AssetType == AssetType.MultiFamily)
                {
                    var converted = asset as MultiFamilyAsset;
                    descriptions.AppendLine(string.Format("<td>a {0} unit Multi-Family property located in {1} {2}</td>", converted.TotalUnits, asset.City, asset.State));
                }
                else if (asset.AssetType == AssetType.MHP)
                {
                    var converted = asset as MultiFamilyAsset;
                    descriptions.AppendLine(string.Format("<td>a {0} space Mobile Home Park property located in {1} {2}</td>", converted.TotalUnits, asset.City, asset.State));
                }
                else
                {
                    var converted = asset as CommercialAsset;
                    descriptions.AppendLine(string.Format("<td>a {0} Sq.Ft {1} property located in {2} {3}</td>", converted.SquareFeet, EnumHelper.GetEnumDescription(asset.AssetType), asset.City, asset.State));
                }
            }
            assetDescription.AppendLine(string.Format("<table><tr><td width=\"20\"></td><td><table><tr>{0}</tr></table></td></tr></table>", descriptions.ToString()));
            return assetDescription.ToString();
        }

        public HoldingCompanyViewModel GetHoldingCompany(Guid holdingCompanyId)
        {
            HoldingCompanyViewModel model = new HoldingCompanyViewModel();
            var entity = _factory.Create().HoldingCompanies.FirstOrDefault(u => u.HoldingCompanyId == holdingCompanyId);
            if (entity != null)
            {
                model.HoldingCompanyId = entity.HoldingCompanyId;
                model.OperatingCompanyId = entity.OperatingCompanyId;
                model.CompanyName = entity.CompanyName;
                model.ISRA = entity.ISRA;

                model.FirstName = entity.FirstName;
                model.LastName = entity.LastName;
                model.Email = entity.Email;
                model.AddressLine1 = entity.AddressLine1;
                model.AddressLine2 = entity.AddressLine2;
                model.City = entity.City;
                model.State = entity.State;
                model.Zip = entity.Zip;
                model.Country = entity.Country;
                model.WorkNumber = entity.WorkNumber;
                model.CellNumber = entity.CellNumber;
                model.FaxNumber = entity.FaxNumber;
                model.IsActive = entity.IsActive;

                model.LinkedIn = entity.LinkedIn;
                model.Facebook = entity.Facebook;
                model.Instagram = entity.Instagram;
                model.Twitter = entity.Twitter;

                return model;
            }
            return null;
        }
        public void DeleteHoldingCompany(Guid holdingCompanyId)
        {
            var context = _factory.Create();
            var entity = context.HoldingCompanies.Find(holdingCompanyId);
            if (entity != null)
            {
                context.Entry(entity).State = EntityState.Deleted;
                context.Save();
            }
        }
        public Dictionary<string, string> SearchCompanies(string term, string type)
        {
            // Settled with this simple string only search implementation against two separate entities since there isnt
            // a proper Operating/Holding company search implemented yet(I think).
            var context = _factory.Create();
            var data = new Dictionary<string, string>();
            if (type == "Holding")
            {
                var comps = context.HoldingCompanies.Where(x => x.CompanyName.ToLower().Contains(term.ToLower()) || 
                                                                x.AddressLine1.ToLower().Contains(term.ToLower()) ).OrderBy(c => c.CompanyName);
                foreach (var comp in comps) data.Add(comp.HoldingCompanyId.ToString(), comp.CompanyName);
            }
            else if (type == "Operating")
            {
                var comps = context.OperatingCompanies.Where(x => x.CompanyName.ToLower().Contains(term.ToLower())).OrderBy(c => c.CompanyName);
                foreach (var comp in comps) data.Add(comp.OperatingCompanyId.ToString(), comp.CompanyName);
            }
            return data;
        }


       

        #region Private Methods
        private string ReferUser(IEPIRepository context, string firstName, string lastName, string city, string state, string email, User referrer)
        {
            //var context = _factory.Create();
            var error = new StringBuilder();
            if (referrer != null)
            {
                // verify that the referred user is not in the User table and not in the UserReferral table

                if (context.Users.Any(x => x.Username == email.ToLower()))
                    error.AppendLine("User " + firstName + " " + lastName + " " + email + " exist in the database.");
                if (context.UserReferrals.Any(x => x.ReferralEmail == email.ToLower()))
                    error.AppendLine("User " + firstName + " " + lastName + " " + email + " is already referred");
                if (error.Length == 0)
                {
                    var referral = new UserReferral();
                    referral.CreateDate = DateTime.Now;
                    referral.FirstName = firstName ?? "";
                    referral.LastName = lastName ?? "";
                    referral.City = city ?? "";
                    referral.State = state ?? "";
                    referral.ReferralEmail = email;
                    referral.User = referrer;
                    referral.UserId = referrer.UserId;
                    referral.ReferralCode = string.Empty;
                    context.UserReferrals.Add(referral);
                    context.Save();
                }
            }
            else
                error.AppendLine("Referrer is null");
            return error.ToString();
        }

        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        private string GetUserTypeAcronym(UserType type)
        {
            switch (type)
            {
                case Inview.Epi.EpiFund.Domain.Enum.UserType.SiteAdmin:
                case Inview.Epi.EpiFund.Domain.Enum.UserType.CorpAdmin:
                    return "CA: I";
                case Inview.Epi.EpiFund.Domain.Enum.UserType.CorpAdmin2:
                    return "CA: II";
                case Inview.Epi.EpiFund.Domain.Enum.UserType.CREBroker:
                    return "MB";
                case Inview.Epi.EpiFund.Domain.Enum.UserType.CRELender:
                    return "ML";
                case Inview.Epi.EpiFund.Domain.Enum.UserType.CREOwner:
                    return "PO";
                case Inview.Epi.EpiFund.Domain.Enum.UserType.ICAdmin:
                    return "ICA";
                case Inview.Epi.EpiFund.Domain.Enum.UserType.Investor:
                    return "PI";
                case Inview.Epi.EpiFund.Domain.Enum.UserType.ListingAgent:
                    return "NAR";
                default:
                    return "N/A";
            }
        }

        private string GetAssetTypeAcronym(AssetType type)
        {
            switch (type)
            {
                case AssetType.MultiFamily:
                    return "MF";
                case AssetType.MHP:
                    return "MHP";
                case AssetType.Office:
                    return "CO";
                case AssetType.Retail:
                    return "CR";
                case AssetType.Medical:
                    return "Med";
                case AssetType.MixedUse:
                    return "Mix";
                case AssetType.Industrial:
                    return "Ind";
                case AssetType.Hotel:
                    return "Hot";
                case AssetType.ConvenienceStoreFuel:
                    return "SF";
                default:
                    return "N/A";
            }
        }
        #endregion

        /*   public DateTime GetMDASignedDate(int userId)
           {
               var context = _factory.Create();
               return context.AssetUserMDAs.Where(w => w.UserId == userId).OrderBy(s => s.SignMDADate).First().SignMDADate;
           }
           */

        public int GetRegisteredPICount()
        {
            var context = _factory.Create();
            return context.Users.Count(w => w.IsActive && w.UserType == UserType.Investor);
        }

        public int GetRegisteredMBACount()
        {
            var context = _factory.Create();
            return context.Users.Count(w => w.IsActive && (w.UserType == UserType.CREBroker || w.UserType == UserType.CRELender));
        }

        public int GetTotalNARCount()
        {
            var context = _factory.Create();
            return context.NarMembers.Count(w => w.IsActive) + context.AssetNARMembers.Count();
        }
        public int GetTotalIndustryParticipants()
        {
            var context = _factory.Create();
            var count = 0;
            count += context.NarMembers.Count();
            count += context.MbaUsers.Count();
            count += context.PrincipalInvestors.Count();
            return count;
        }
        public void ReinstateSellerPrivileges(int userId)
        {
            var context = _factory.Create();
            var seller = context.Users.FirstOrDefault(u => u.UserId == userId);
            seller.HasSellerPrivilege = true;
            context.Save();

            // find all unpublished assets that we have IPAs for
            // that means they were unpublished by the revoke
            var ipas = from u in context.UserFiles
                       join a in context.Assets on u.AssetId equals a.AssetId
                       where u.UserId == userId &&
                       a.Show == false
                       select a;

            foreach (var a in ipas)
            {
                a.Show = true;
            }
            context.Save();
        }

        public void RevokeSellerPrivileges(int userId)
        {
            var context = _factory.Create();
            var seller = context.Users.FirstOrDefault(u => u.UserId == userId);
            seller.HasSellerPrivilege = false;
            context.Save();

            // find all published assets by that user and unpublish them
            // by our rules, they should all have IPAs on them

            var ipas = from u in context.UserFiles
                       join a in context.Assets on u.AssetId equals a.AssetId
                       where u.UserId == userId &&
                       a.Show == true
                       select a;

            foreach (var a in ipas)
            {
                a.Show = false;
            }
            context.Save();
        }

        // added method implementations
        public string CanUserManagePortfolios(string userName)
        {
            string str;
            User user = (
                from x in this._factory.Create().Users
                where x.Username == userName.ToLower()
                select x).FirstOrDefault<User>();
            if (user == null)
            {
                str = "false";
            }
            else if ((user.UserType == UserType.CorpAdmin || user.UserType == UserType.CorpAdmin2 || user.UserType == UserType.ICAdmin || user.UserType == UserType.ListingAgent || user.UserType == UserType.SiteAdmin || user.UserType == UserType.CREBroker || user.UserType == UserType.CRELender ? false : user.UserType != UserType.Investor))
            {
                str = "false";
            }
            else if (!(user.UserType == UserType.CREBroker || user.UserType == UserType.CRELender ? false : user.UserType != UserType.Investor))
            {
                str = (!user.HasSellerPrivilege ? "false" : "true");
            }
            else if (user.UserType != UserType.ICAdmin)
            {
                str = "true";
            }
            else
            {
                ICStatus? cStatus = user.ICStatus;
                str = ((cStatus.GetValueOrDefault() != ICStatus.Approved ? 0 : Convert.ToInt32(cStatus.HasValue)) == 0 ? "false" : "true");
            }
            return str;
        }

        public int CreateUser(RegistrationModel model)
        {
            UserReferral userReferral;
            IEPIRepository ePIRepository = this._factory.Create();
            User user = new User()
            {
                AcronymForCorporateEntity = model.AcroynmForCorporateEntity,
                CorporateTitle = model.CorporateTitle,
                CommercialOfficeInterest = model.CommercialOfficeInterest,
                CommercialOtherInterest = model.CommercialOtherInterest,
                CommercialRetailInterest = model.CommercialRetailInterest,
                MultiFamilyInterest = model.MultiFamilyInterest,
                MHPInterest = model.MHPInterest,
                SecuredPaperInterest = model.SecuredPaperInterest,
                RetailTenantPropertyInterest = model.RetailTenantPropertyInterest,
                OfficeTenantPropertyInterest = model.OfficeTenantPropertyInterest,
                FuelServicePropertyInterest = model.FuelServicePropertyInterest,
                MedicalTenantPropertyInterest = model.MedicalTenantPropertyInterest,
                IndustryTenantPropertyInterest = model.IndustryTenantPropertyInterest,
                SingleTenantRetailPortfoliosInterest = model.SingleTenantRetailPortfoliosInterest,
                MixedUseCommercialPropertyInterest = model.MixedUseCommercialPropertyInterest,
                FracturedCondoPortfoliosInterest = model.FracturedCondoPortfoliosInterest,
                MiniStoragePropertyInterest = model.MiniStoragePropertyInterest,
                ResortHotelMotelPropertyInterest = model.ResortHotelMotelPropertyInterest,
                GorvernmentTenantPropertyInterest = model.GorvernmentTenantPropertyInterest,
                ParkingGaragePropertyInterest = model.ParkingGaragePropertyInterest,
                NonCompletedDevelopmentsInterest = model.NonCompletedDevelopmentsInterest,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                AlternateEmail = model.AlternateEmail,
                CellNumber = model.CellNumber,
                City = model.City,
                FaxNumber = model.FaxNumber,
                FirstName = model.FirstName,
                HomeNumber = model.HomeNumber,
                IsActive = true,
                LastName = model.LastName,
                ManagingOfficerName = model.ManagingOfficerName,
                State = model.SelectedState,
                Username = model.Username.ToLower(),
                WorkNumber = model.WorkNumber,
                Zip = model.Zip,
                Password = this.HashPassword(model.Password),
                UserType = model.SelectedUserType
            };
            user.CorporateTitle = model.CorporateTitle;
            user.StateOfOriginCorporateEntity = model.StateOfOriginCorporateEntity;
            user.IsCertificateOfGoodStandingAvailable = model.IsCertificateOfGoodStandingAvailable.GetValueOrDefault(false);
            user.CorporateEntityType = model.SelectedCorporateEntityType;
            user.CompanyName = model.CompanyName;
            user.SignupDate = new DateTime?(DateTime.Now);
            user.EIN = model.EIN;
            user.CorporateTIN = model.CorporateTIN;
            user.LicenseStateIsHeld = model.StateLicenseHeld;
            user.StateLicenseDesc = model.LicenseDesc;
            user.StateLicenseNumber = model.LicenseNumber;
            StringBuilder stringBuilder = new StringBuilder();
            if (model.SelectedPreferredMethods != null)
            {
                model.SelectedPreferredMethods.ForEach((PreferredMethod f) => {
                    stringBuilder.Append(f);
                    stringBuilder.Append(";");
                });
            }
            user.PreferredMethods = stringBuilder.ToString();
            stringBuilder = new StringBuilder();
            if (model.SelectedPreferredContactTime != null)
            {
                model.SelectedPreferredContactTime.ForEach((PreferredContactTime f) => {
                    stringBuilder.Append(f);
                    stringBuilder.Append(";");
                });
            }
            user.PreferredContactTimes = stringBuilder.ToString();
            MBAUser mBAUser = ePIRepository.MbaUsers.FirstOrDefault<MBAUser>((MBAUser s) => s.Email == user.Username);
            if (mBAUser != null)
            {
                user.MbaUserId = new int?(mBAUser.MBAUserId);
            }
            if (user.UserType != UserType.ICAdmin)
            {
                user.ICStatus = null;
            }
            else
            {
                user.ICStatus = new ICStatus?(ICStatus.Pending);
            }
            if (string.IsNullOrEmpty(model.ReferralId))
            {
                userReferral = (
                    from x in ePIRepository.UserReferrals
                    where x.ReferralEmail == model.Username.ToLower()
                    select x).FirstOrDefault<UserReferral>();
                if (userReferral != null)
                {
                    user.ReferredByUserId = new int?(userReferral.UserId);
                    user.ReferralStatus = UserRegisteredReferralStatus.ThroughUserReferralTable;
                    userReferral.Registered = true;
                    ePIRepository.Entry(userReferral).State = EntityState.Modified;
                }
            }
            else
            {
                userReferral = (
                    from x in ePIRepository.UserReferrals
                    where x.ReferralEmail == model.Username.ToLower()
                    select x).FirstOrDefault<UserReferral>();
                if (!(userReferral == null ? true : !(userReferral.ReferralCode == model.ReferralId)))
                {
                    user.ReferredByUserId = new int?(userReferral.UserId);
                    user.ReferralStatus = UserRegisteredReferralStatus.ReferralLink;
                    userReferral.Registered = true;
                    ePIRepository.Entry(userReferral).State = EntityState.Modified;
                }
                else if (userReferral != null)
                {
                    user.ReferredByUserId = new int?(userReferral.UserId);
                    user.ReferralStatus = UserRegisteredReferralStatus.LinkButTableAfter;
                    userReferral.Registered = true;
                    ePIRepository.Entry(userReferral).State = EntityState.Modified;
                }
            }
            user.HasSellerPrivilege = true;
            ePIRepository.Users.Add(user);
            ePIRepository.Save();
            if (user.UserType == UserType.ListingAgent)
            {
                NARMember nullable = (
                    from x in ePIRepository.NarMembers
                    where x.Email == user.Username.ToLower()
                    select x).FirstOrDefault<NARMember>();
                if (nullable == null)
                {
                    NARMember nARMember = new NARMember()
                    {
                        CellPhoneNumber = user.CellNumber,
                        CompanyAddressLine1 = user.AddressLine1,
                        CompanyAddressLine2 = user.AddressLine2,
                        CompanyCity = user.City,
                        CompanyName = user.CompanyName,
                        CompanyState = user.State,
                        CompanyZip = user.Zip,
                        Email = user.Username,
                        FaxNumber = user.FaxNumber,
                        FirstName = user.FirstName,
                        IsActive = true,
                        LastName = user.LastName,
                        Registered = new bool?(true),
                        WorkPhoneNumber = user.WorkNumber,
                        ReferredByUserId = new int?(user.UserId)
                    };
                    ePIRepository.NarMembers.Add(nARMember);
                }
                else
                {
                    nullable.Registered = new bool?(true);
                }
                ePIRepository.Save();
            }
            return user.UserId;
        }


        public int GetImportedMBACount()
        {
            int num = this._factory.Create().MbaUsers.Count<MBAUser>((MBAUser w) => w.IsActive);
            return num;
        }

        public int GetImportedNARCount()
        {
            int num = this._factory.Create().NarMembers.Count<NARMember>((NARMember w) => w.IsActive);
            return num;
        }

        public DateTime? GetMDASignedDate(int userId)
        {
            DateTime? nullable;
            List<AssetUserMDA> list = (
                from w in this._factory.Create().AssetUserMDAs
                where w.UserId == userId
                select w).ToList<AssetUserMDA>();
            if (list.Count <= 0)
            {
                nullable = null;
            }
            else
            {
                nullable = new DateTime?((
                    from s in list
                    orderby s.SignMDADate
                    select s).First<AssetUserMDA>().SignMDADate);
            }
            return nullable;
        }

        public User GetUserEntity(string username)
        {
            User user;
            if (string.IsNullOrEmpty(username))
            {
                user = null;
            }
            else
            {
                User user1 = this._factory.Create().Users.First<User>((User s) => s.Username == username.ToLower());
                user = user1;
            }
            return user;
        }

        public ImportUsersModel ImportPrincipalInvestors(string path, int referringUserId, bool areReferredUsers, bool checkAgainstMBAs = true, bool CheckAgainstNARs = true)
        {
            //Errors
            //throw new NotImplementedException();
            ImportUsersModel resultModel = new ImportUsersModel();
            int insertedRows = 0;
            int errorRows = 0;
            var connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + @";Extended Properties=""Excel 12.0 Xml;HDR=YES;IMEX=1""";
            var connection = new OleDbConnection(connectionString);
            connection.Open();
            int rowIndex = 1;

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM [Principal Investors$]";

            var dataReader = command.ExecuteReader();
            StringBuilder sb = new StringBuilder();
            var context = _factory.Create();
            var user = context.Users.Find(referringUserId);
            while (dataReader.Read())
            {
                try
                {
                    string firstName = null;
                    string lastName = null;
                    string email = null;
                    string companyName = null;
                    string companyAddressLine1 = null;
                    string companyAddressLine2 = null;
                    string companyCity = null;
                    string companyState = null;
                    string companyZip = null;
                    string cellNumber = null;
                    string faxNumber = null;
                    string workNumber = null;
                    string website = null;
                    if (dataReader["First Name"].ToString().Length > 0 && dataReader["Email"].ToString().Length > 0)
                    {
                        if (dataReader["First Name"].ToString().Length > 0)
                        {
                            firstName = dataReader["First Name"].ToString();
                        }
                        if (dataReader["Last Name"].ToString().Length > 0)
                        {
                            lastName = dataReader["Last Name"].ToString();
                        }
                        if (dataReader["Email"].ToString().Length > 0)
                        {
                            email = dataReader["Email"].ToString();
                        }
                        if (dataReader["Company Name"].ToString().Length > 0)
                        {
                            companyName = dataReader["Company Name"].ToString();
                        }
                        if (dataReader["Company Web Site"].ToString().Length > 0)
                        {
                            website = dataReader["Company Web Site"].ToString();
                        }
                        if (dataReader["Company Address Line 1"].ToString().Length > 0)
                        {
                            companyAddressLine1 = dataReader["Company Address Line 1"].ToString();
                        }
                        if (dataReader["Company Address Line 2"].ToString().Length > 0)
                        {
                            companyAddressLine2 = dataReader["Company Address Line 2"].ToString();
                        }
                        if (dataReader["Company City"].ToString().Length > 0)
                        {
                            companyCity = dataReader["Company City"].ToString();
                        }
                        if (dataReader["Company State"].ToString().Length > 0)
                        {
                            companyState = dataReader["Company State"].ToString();
                        }
                        if (dataReader["Company Zip"].ToString().Length > 0)
                        {
                            companyZip = dataReader["Company Zip"].ToString();
                        }
                        if (dataReader["Principal Direct Contact Phone Number"].ToString().Length > 0)
                        {
                            cellNumber = dataReader["Principal Direct Contact Phone Number"].ToString();
                        }
                        if (dataReader["Company General Fax Number"].ToString().Length > 0)
                        {
                            faxNumber = dataReader["Company General Fax Number"].ToString();
                        }
                        if (dataReader["Company General Phone Number"].ToString().Length > 0)
                        {
                            workNumber = dataReader["Company General Phone Number"].ToString();
                        }
                        if (!string.IsNullOrEmpty(email))
                        {
                            bool error = false;
                            var investors = context.PrincipalInvestors.FirstOrDefault(s => s.Email.ToLower() == email.ToLower());
                            if (investors != null)
                            {
                                error = true;
                                errorRows++;
                                sb.Append(string.Format("Row {0}, {1} {2} - User is already a Principal Investor in system.<br/>", rowIndex, firstName, lastName));
                            }
                            if (CheckAgainstNARs)
                            {
                                var narMember = context.NarMembers.FirstOrDefault(s => s.Email.ToLower() == email.ToLower());
                                if (narMember != null)
                                {
                                    error = true;
                                    errorRows++;
                                    sb.Append(string.Format("Row {0}, {1} {2} - NAR Member already in system.<br/>", rowIndex, firstName, lastName));
                                }
                            }
                            if (checkAgainstMBAs)
                            {
                                var mbaMember = context.MbaUsers.FirstOrDefault(s => s.Email.ToLower() == email.ToLower());
                                if (mbaMember != null)
                                {
                                    error = true;
                                    errorRows++;
                                    sb.Append(string.Format("Row {0}, {1} {2} - User is already a MBA in system.<br/>", rowIndex, firstName, lastName));
                                }
                            }
                            if (!error)
                            {
                                context.PrincipalInvestors.Add(new PrincipalInvestor()
                                {
                                    CompanyAddressLine1 = companyAddressLine1,
                                    CompanyAddressLine2 = companyAddressLine2,
                                    CompanyCity = companyCity,
                                    CompanyName = companyName,
                                    CompanyState = companyState,
                                    CompanyZip = companyZip,
                                    Email = email.ToLower(),
                                    FirstName = firstName,
                                    LastName = lastName,
                                    CellPhoneNumber = cellNumber,
                                    WorkPhoneNumber = workNumber,
                                    FaxNumber = faxNumber,
                                    ReferredByUserId = referringUserId,
                                    IsActive = true,
                                    Website = website,
                                    Registered = false
                                });
                                if (areReferredUsers)
                                {
                                    try
                                    {
                                        ReferUser(context, firstName, lastName, companyCity, companyState, email, user);
                                    }
                                    catch { }
                                }
                                insertedRows++;
                            }
                        }
                        else
                        {
                            errorRows++;
                            sb.Append(string.Format("Row {0} - Missing required email address.<br/>", rowIndex));
                        }
                    }

                }
                catch (Exception ex)
                {
                    errorRows++;
                    sb.Append(ex.Message + Environment.NewLine);
                }
                rowIndex++;
            }
            dataReader.Close();
            connection.Close();
            context.Save();
            resultModel.ImportedCount = insertedRows;
            resultModel.Message = sb.ToString();
            resultModel.ReferredCount = errorRows;
            return resultModel;
            //return new Tuple<string, int>(string.Format("File successfully processed. {0} imported; {1} errors<br/>{2}", insertedRows, errorRows, sb.ToString()), insertedRows);

        }


        public IEnumerable<SelectListItem> PopulateStateList()
        {
            IEnumerable<SelectListItem> States = new List<SelectListItem>();
            List<SelectListItem> selectListItems3 = new List<SelectListItem>();
            SelectListItem selectListItem21 = new SelectListItem()
            {
                Text = "AL",
                Value = "AL"
            };
            selectListItems3.Add(selectListItem21);
            SelectListItem selectListItem22 = new SelectListItem()
            {
                Text = "AK",
                Value = "AK"
            };
            selectListItems3.Add(selectListItem22);
            SelectListItem selectListItem23 = new SelectListItem()
            {
                Text = "AZ",
                Value = "AZ"
            };
            selectListItems3.Add(selectListItem23);
            SelectListItem selectListItem24 = new SelectListItem()
            {
                Text = "AR",
                Value = "AR"
            };
            selectListItems3.Add(selectListItem24);
            SelectListItem selectListItem25 = new SelectListItem()
            {
                Text = "CA",
                Value = "CA"
            };
            selectListItems3.Add(selectListItem25);
            SelectListItem selectListItem26 = new SelectListItem()
            {
                Text = "CO",
                Value = "CO"
            };
            selectListItems3.Add(selectListItem26);
            SelectListItem selectListItem27 = new SelectListItem()
            {
                Text = "CT",
                Value = "CT"
            };
            selectListItems3.Add(selectListItem27);
            SelectListItem selectListItem28 = new SelectListItem()
            {
                Text = "DE",
                Value = "DE"
            };
            selectListItems3.Add(selectListItem28);
            SelectListItem selectListItem29 = new SelectListItem()
            {
                Text = "FL",
                Value = "FL"
            };
            selectListItems3.Add(selectListItem29);
            SelectListItem selectListItem30 = new SelectListItem()
            {
                Text = "GA",
                Value = "GA"
            };
            selectListItems3.Add(selectListItem30);
            SelectListItem selectListItem31 = new SelectListItem()
            {
                Text = "HI",
                Value = "HI"
            };
            selectListItems3.Add(selectListItem31);
            SelectListItem selectListItem32 = new SelectListItem()
            {
                Text = "ID",
                Value = "ID"
            };
            selectListItems3.Add(selectListItem32);
            SelectListItem selectListItem33 = new SelectListItem()
            {
                Text = "IL",
                Value = "IL"
            };
            selectListItems3.Add(selectListItem33);
            SelectListItem selectListItem34 = new SelectListItem()
            {
                Text = "IN",
                Value = "IN"
            };
            selectListItems3.Add(selectListItem34);
            SelectListItem selectListItem35 = new SelectListItem()
            {
                Text = "IA",
                Value = "IA"
            };
            selectListItems3.Add(selectListItem35);
            SelectListItem selectListItem36 = new SelectListItem()
            {
                Text = "KS",
                Value = "KS"
            };
            selectListItems3.Add(selectListItem36);
            SelectListItem selectListItem37 = new SelectListItem()
            {
                Text = "KY",
                Value = "KY"
            };
            selectListItems3.Add(selectListItem37);
            SelectListItem selectListItem38 = new SelectListItem()
            {
                Text = "LA",
                Value = "LA"
            };
            selectListItems3.Add(selectListItem38);
            SelectListItem selectListItem39 = new SelectListItem()
            {
                Text = "ME",
                Value = "ME"
            };
            selectListItems3.Add(selectListItem39);
            SelectListItem selectListItem40 = new SelectListItem()
            {
                Text = "MD",
                Value = "MD"
            };
            selectListItems3.Add(selectListItem40);
            SelectListItem selectListItem41 = new SelectListItem()
            {
                Text = "MA",
                Value = "MA"
            };
            selectListItems3.Add(selectListItem41);
            SelectListItem selectListItem42 = new SelectListItem()
            {
                Text = "MI",
                Value = "MI"
            };
            selectListItems3.Add(selectListItem42);
            SelectListItem selectListItem43 = new SelectListItem()
            {
                Text = "MN",
                Value = "MN"
            };
            selectListItems3.Add(selectListItem43);
            SelectListItem selectListItem44 = new SelectListItem()
            {
                Text = "MS",
                Value = "MS"
            };
            selectListItems3.Add(selectListItem44);
            SelectListItem selectListItem45 = new SelectListItem()
            {
                Text = "MO",
                Value = "MO"
            };
            selectListItems3.Add(selectListItem45);
            SelectListItem selectListItem46 = new SelectListItem()
            {
                Text = "MT",
                Value = "MT"
            };
            selectListItems3.Add(selectListItem46);
            SelectListItem selectListItem47 = new SelectListItem()
            {
                Text = "NE",
                Value = "NE"
            };
            selectListItems3.Add(selectListItem47);
            SelectListItem selectListItem48 = new SelectListItem()
            {
                Text = "NV",
                Value = "NV",
                Selected = true
            };
            selectListItems3.Add(selectListItem48);
            SelectListItem selectListItem49 = new SelectListItem()
            {
                Text = "NH",
                Value = "NH"
            };
            selectListItems3.Add(selectListItem49);
            SelectListItem selectListItem50 = new SelectListItem()
            {
                Text = "NJ",
                Value = "NJ"
            };
            selectListItems3.Add(selectListItem50);
            SelectListItem selectListItem51 = new SelectListItem()
            {
                Text = "NM",
                Value = "NM"
            };
            selectListItems3.Add(selectListItem51);
            SelectListItem selectListItem52 = new SelectListItem()
            {
                Text = "NY",
                Value = "NY"
            };
            selectListItems3.Add(selectListItem52);
            SelectListItem selectListItem53 = new SelectListItem()
            {
                Text = "NC",
                Value = "NC"
            };
            selectListItems3.Add(selectListItem53);
            SelectListItem selectListItem54 = new SelectListItem()
            {
                Text = "ND",
                Value = "ND"
            };
            selectListItems3.Add(selectListItem54);
            SelectListItem selectListItem55 = new SelectListItem()
            {
                Text = "OH",
                Value = "OH"
            };
            selectListItems3.Add(selectListItem55);
            SelectListItem selectListItem56 = new SelectListItem()
            {
                Text = "OK",
                Value = "OK"
            };
            selectListItems3.Add(selectListItem56);
            SelectListItem selectListItem57 = new SelectListItem()
            {
                Text = "OR",
                Value = "OR"
            };
            selectListItems3.Add(selectListItem57);
            SelectListItem selectListItem58 = new SelectListItem()
            {
                Text = "PA",
                Value = "PA"
            };
            selectListItems3.Add(selectListItem58);
            SelectListItem selectListItem59 = new SelectListItem()
            {
                Text = "RI",
                Value = "RI"
            };
            selectListItems3.Add(selectListItem59);
            SelectListItem selectListItem60 = new SelectListItem()
            {
                Text = "SC",
                Value = "SC"
            };
            selectListItems3.Add(selectListItem60);
            SelectListItem selectListItem61 = new SelectListItem()
            {
                Text = "SD",
                Value = "SD"
            };
            selectListItems3.Add(selectListItem61);
            SelectListItem selectListItem62 = new SelectListItem()
            {
                Text = "TN",
                Value = "TN"
            };
            selectListItems3.Add(selectListItem62);
            SelectListItem selectListItem63 = new SelectListItem()
            {
                Text = "TX",
                Value = "TX"
            };
            selectListItems3.Add(selectListItem63);
            SelectListItem selectListItem64 = new SelectListItem()
            {
                Text = "UT",
                Value = "UT"
            };
            selectListItems3.Add(selectListItem64);
            SelectListItem selectListItem65 = new SelectListItem()
            {
                Text = "VT",
                Value = "VT"
            };
            selectListItems3.Add(selectListItem65);
            SelectListItem selectListItem66 = new SelectListItem()
            {
                Text = "VA",
                Value = "VA"
            };
            selectListItems3.Add(selectListItem66);
            SelectListItem selectListItem67 = new SelectListItem()
            {
                Text = "WA",
                Value = "WA"
            };
            selectListItems3.Add(selectListItem67);
            SelectListItem selectListItem68 = new SelectListItem()
            {
                Text = "WV",
                Value = "WV"
            };
            selectListItems3.Add(selectListItem68);
            SelectListItem selectListItem69 = new SelectListItem()
            {
                Text = "WI",
                Value = "WI"
            };
            selectListItems3.Add(selectListItem69);
            SelectListItem selectListItem70 = new SelectListItem()
            {
                Text = "WY",
                Value = "WY"
            };
            selectListItems3.Add(selectListItem70);
            States = selectListItems3;
            return States;

        }


        public HoldingCompanyUpdateResult UpdateHoldingCompany(HoldingCompanyViewModel model)
        {
            var result = new HoldingCompanyUpdateResult();
            var context = _factory.Create();
            var entity = context.HoldingCompanies.FirstOrDefault(u => u.HoldingCompanyId == model.HoldingCompanyId);
            if (entity != null)
            {
                entity.OperatingCompanyId = model.OperatingCompanyId;
                entity.CompanyName = model.CompanyName;
                entity.ISRA = model.ISRA;

                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.Email = model.Email;
                entity.AddressLine1 = model.AddressLine1;
                entity.AddressLine2 = model.AddressLine2;
                entity.City = model.City;
                entity.State = model.State;
                entity.Zip = model.Zip;
                entity.Country = model.Country;
                entity.WorkNumber = model.WorkNumber;
                entity.CellNumber = model.CellNumber;
                entity.FaxNumber = model.FaxNumber;

                entity.LinkedIn = model.LinkedIn; 
                entity.Facebook = model.Facebook;
                entity.Instagram = model.Instagram;
                entity.Twitter = model.Twitter;

                entity.IsActive = model.IsActive;
                context.Save();

                if (!string.IsNullOrEmpty(model.AssetAssignment))
                {
                    int assetNumber = 0;
                    if (int.TryParse(model.AssetAssignment, out assetNumber))
                    {
                        var asset = context.Assets.FirstOrDefault(x => x.AssetNumber == assetNumber);
                        if (asset != null)
                        {
                            if (asset.OwnerHoldingCompanyId.HasValue && !model.OverwriteAsset)
                            {
                                return new HoldingCompanyUpdateResult
                                {
                                    Success = false,
                                    Message = $"Specified asset '{assetNumber}' is already assigned a holding company and will not be overwritten. Holding Company Updated."
                                };
                            }
                            asset.OwnerHoldingCompanyId = model.HoldingCompanyId;
                            context.Save();
                            return new HoldingCompanyUpdateResult { Success = true };
                        }
                    }
                    return new HoldingCompanyUpdateResult
                    {
                        Success = false,
                        Message = $"Failed to find Asset with number '{model.AssetAssignment}'. Holding Company Updated."
                    };
                }
                return new HoldingCompanyUpdateResult { Success = true };
            }
            else
            {
                return new HoldingCompanyUpdateResult
                {
                    Success = false,
                    Message = $"Failed to find Holding Company with id '{model.HoldingCompanyId}'"
                };
            }
        }

        public bool CreateHoldingCompany(HoldingCompanyViewModel model)
        {
            bool success = true;
            try
            {
                var context = _factory.Create();
                HoldingCompany entity = new HoldingCompany();
                entity.HoldingCompanyId = model.HoldingCompanyId;
                entity.OperatingCompanyId = model.OperatingCompanyId;
                entity.CompanyName = model.CompanyName;
                entity.ISRA = model.ISRA;

                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.Email = model.Email;
                entity.AddressLine1 = model.AddressLine1;
                entity.AddressLine2 = model.AddressLine2;
                entity.City = model.City;
                entity.State = model.State;
                entity.Zip = model.Zip;
                entity.Country = model.Country;
                entity.WorkNumber = model.WorkNumber;
                entity.CellNumber = model.CellNumber;
                entity.FaxNumber = model.FaxNumber;

                entity.LinkedIn = model.LinkedIn;
                entity.Facebook = model.Facebook;
                entity.Instagram = model.Instagram;
                entity.Twitter = model.Twitter;

                entity.IsActive = model.IsActive;

                context.HoldingCompanies.Add(entity);
                context.Save();
            }
            catch { success = false; }
            return success;
        }


        #region Holding Company and Operating Company
        public List<HoldingCompanyViewModel> GetHoldingCompaniesNew()
        {
            var context = _factory.Create();
            List<HoldingCompanyViewModel> entities = new List<HoldingCompanyViewModel>();

            var things = (from hc in context.HoldingCompanies
                          select new HoldingCompanyViewModel
                          {
                              HoldingCompanyId = hc.HoldingCompanyId,
                              CompanyName = hc.CompanyName,
                              PIName = (!string.IsNullOrEmpty(hc.FirstName) && !string.IsNullOrEmpty(hc.LastName) ? hc.FirstName + " " + hc.LastName
                                         : !string.IsNullOrEmpty(hc.FirstName) ? hc.FirstName
                                         : !string.IsNullOrEmpty(hc.LastName) ? hc.LastName : "N/A"),
                              City = hc.City,
                              State = hc.State,
                              Email = hc.Email,
                              WorkNumber = hc.WorkNumber,
                              AssetsCount = context.Assets.Where(a => a.OwnerHoldingCompanyId == hc.HoldingCompanyId).Count(),
                              AssetsPublishedCount = context.Assets.Where(a => a.OwnerHoldingCompanyId == hc.HoldingCompanyId &&
                                                                                a.IsActive &&
                                                                                a.IsSubmitted).Count(),
                              OperatingComapnyName = context.OperatingCompanies.Where(a => a.OperatingCompanyId == hc.OperatingCompanyId).FirstOrDefault().CompanyName == "Unknown" ? "N/A" :
                                                     context.OperatingCompanies.Where(a => a.OperatingCompanyId == hc.OperatingCompanyId).FirstOrDefault().CompanyName

                          }).ToList();

            return things;
        }

        public List<HoldingCompanyViewModel> GetHoldingCompaniesForOperatingCompany(Guid id)
        {
            var context = _factory.Create();
            var data = new List<HoldingCompanyViewModel>();
            var entities = _factory.Create().HoldingCompanies.Where(x => x.OperatingCompanyId == id);
            foreach (var entity in entities)
            {
                data.Add(new HoldingCompanyViewModel
                {
                    HoldingCompanyId = entity.HoldingCompanyId,
                    OperatingCompanyId = entity.OperatingCompanyId,
                    CompanyName = entity.CompanyName,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Email = entity.Email,
                    AddressLine1 = entity.AddressLine1,
                    AddressLine2 = entity.AddressLine2,
                    City = entity.City,
                    State = entity.State,
                    Zip = entity.Zip,
                    Country = entity.Country,
                    WorkNumber = entity.WorkNumber,
                    CellNumber = entity.CellNumber,
                    FaxNumber = entity.FaxNumber,
                    Facebook = entity.Facebook,
                    Twitter=entity.Twitter,
                    LinkedIn=entity.LinkedIn,
                    Instagram=entity.Instagram
                });
            }
            return data;
        }

        public List<HoldingCompanyList> GetHoldingCompanies(ManageHoldingCompanyModel model)
        {
            var context = _factory.Create();
            List<HoldingCompanyList> HCList = new List<HoldingCompanyList>();

            context.HoldingCompanies.Where(a=>a.IsActive).OrderBy(a=>a.CompanyName).ToList().ForEach(x =>
            {
                if (x != null)
                {
                    HoldingCompanyList holdingCompanyList = new HoldingCompanyList();

                    holdingCompanyList.HoldingCompanyId = x.HoldingCompanyId;
                    holdingCompanyList.HoldingCompanyName = x.CompanyName.Trim();
                    holdingCompanyList.HoldingCompanyEmail = x.Email;
                    holdingCompanyList.HoldingCompanyFirstName = x.FirstName;
                    holdingCompanyList.HoldingCompanyLastName = x.LastName;
                    holdingCompanyList.ISRA = x.ISRA;

                    holdingCompanyList.HoldingCompanyLinkedInurl = x.LinkedIn;
                    holdingCompanyList.HoldingCompanyFacebookurl = x.Facebook;
                    holdingCompanyList.HoldingCompanyInstagramurl = x.Instagram;
                    holdingCompanyList.HoldingCompanyTwitterurl = x.Twitter;

                    var HCAsset = context.AssetHCOwnership.Include("Asset").Where(a => a.HoldingCompanyId == x.HoldingCompanyId).
                                  OrderByDescending(a => a.HoldingCompanyId).FirstOrDefault();

                    if (HCAsset != null)
                    {
                        holdingCompanyList.AssetId = HCAsset.AssetId;
                        holdingCompanyList.City = HCAsset.Asset.City;
                        holdingCompanyList.State = HCAsset.Asset.State;
                        holdingCompanyList.AssetName = HCAsset.Asset.ProjectName;
                        holdingCompanyList.AssetNumber = HCAsset.Asset.AssetNumber;
                        holdingCompanyList.Show = HCAsset.Asset.Show;
                        holdingCompanyList.Type = HCAsset.Asset.AssetType;
                        holdingCompanyList.ListingStatus = HCAsset.Asset.ListingStatus;
                        holdingCompanyList.BusDriver = HCAsset.Asset.Show ? "CA" : "SUS";
                        holdingCompanyList.IsPaper = HCAsset.Asset.IsPaper;

                        holdingCompanyList.Address1 = HCAsset.Asset.PropertyAddress;
                        holdingCompanyList.ZipCode = HCAsset.Asset.Zip;
                        holdingCompanyList.County = HCAsset.Asset.County;

                        holdingCompanyList.SquareFeet = (HCAsset.Asset as CommercialAsset) != null ? HCAsset.Asset.SquareFeet : 0;
                        holdingCompanyList.NumberOfUnits = (HCAsset.Asset as MultiFamilyAsset) != null ? (HCAsset.Asset as MultiFamilyAsset).TotalUnits :
                                (HCAsset.Asset.AssetType == AssetType.MHP ? ((HCAsset.Asset as MultiFamilyAsset).TotalUnits +
                                                                       (HCAsset.Asset.NumberNonRentableSpace != null ? (int)HCAsset.Asset.NumberRentableSpace : 0) +
                                                                       (HCAsset.Asset.NumberNonRentableSpace != null ? (int)HCAsset.Asset.NumberNonRentableSpace : 0)) : 0);

                        holdingCompanyList.AssetCount = context.AssetHCOwnership.Where(a => a.AssetId == HCAsset.AssetId).Count();

                        holdingCompanyList.UserType = context.Users.Where(u => u.UserId == HCAsset.Asset.ListedByUserId).Any() ?
                                   context.Users.Where(u => u.UserId == HCAsset.Asset.ListedByUserId).FirstOrDefault().UserType : (UserType)0;

                        holdingCompanyList.Portfolio = context.PortfolioAssets.Where(a => a.AssetId == HCAsset.Asset.AssetId).Any() ? true : false;

                        holdingCompanyList.OperatingCompanyId = context.AssetOC.Where(a => a.AssetId == HCAsset.Asset.AssetId).Any() ?
                                             context.AssetOC.Where(a => a.AssetId == HCAsset.Asset.AssetId).OrderByDescending(a => a.CreateDate).FirstOrDefault().OperatingCompanyId : null;
                        holdingCompanyList.OperatingCompanyName = context.AssetOC.Where(a => a.AssetId == HCAsset.Asset.AssetId).Any() ?
                                             context.AssetOC.Include("OperatingCompany").Where(a => a.AssetId == HCAsset.Asset.AssetId).OrderByDescending(a => a.CreateDate).FirstOrDefault().OperatingCompany.CompanyName : "";

                    }

                    HCList.Add(holdingCompanyList);
                }
            });

           

            if (model.ISRA)
            {
                HCList = HCList.Where(hc => hc.ISRA == model.ISRA).ToList();
            }

            if (!string.IsNullOrEmpty(model.HCName))
            {
                var regex = "[^A-Za-z0-9]";
                var HCName = Regex.Replace(model.HCName, regex, "");
                HCList = HCList.Where(a => a.HoldingCompanyName != null && Regex.Replace(a.HoldingCompanyName.ToLower(), regex, "").Contains(HCName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.HCEmail))
            {
                var regex = "[^A-Za-z0-9]";
                var HCEmail = Regex.Replace(model.HCEmail, regex, "");
                HCList = HCList.Where(a => a.HoldingCompanyEmail != null && Regex.Replace(a.HoldingCompanyEmail.ToLower(), regex, "").Contains(HCEmail.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.HCFirstName))
            {
                var regex = "[^A-Za-z0-9]";
                var HCFirstName = Regex.Replace(model.HCFirstName, regex, "");
                HCList = HCList.Where(a => a.HoldingCompanyFirstName != null && Regex.Replace(a.HoldingCompanyFirstName.ToLower(), regex, "").Contains(HCFirstName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.HCLastName))
            {
                var regex = "[^A-Za-z0-9]";
                var HCLastName = Regex.Replace(model.HCLastName, regex, "");
                HCList = HCList.Where(a => a.HoldingCompanyLastName != null && Regex.Replace(a.HoldingCompanyLastName.ToLower(), regex, "").Contains(HCLastName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.LinkedInurl))
            {
                var regex = "[^A-Za-z0-9]";
                var LinkedInurl = Regex.Replace(model.LinkedInurl, regex, "");
                HCList = HCList.Where(a => a.HoldingCompanyLinkedInurl != null && Regex.Replace(a.HoldingCompanyLinkedInurl.ToLower(), regex, "").Contains(LinkedInurl.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.Facebookurl))
            {
                var regex = "[^A-Za-z0-9]";
                var Facebookurl = Regex.Replace(model.Facebookurl, regex, "");
                HCList = HCList.Where(a => a.HoldingCompanyFacebookurl != null && Regex.Replace(a.HoldingCompanyFacebookurl.ToLower(), regex, "").Contains(Facebookurl.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.Instagramurl))
            {
                var regex = "[^A-Za-z0-9]";
                var Instagramurl = Regex.Replace(model.Instagramurl, regex, "");
                HCList = HCList.Where(a => a.HoldingCompanyInstagramurl != null && Regex.Replace(a.HoldingCompanyInstagramurl.ToLower(), regex, "").Contains(Instagramurl.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.Twitterurl))
            {
                var regex = "[^A-Za-z0-9]";
                var Twitterurl = Regex.Replace(model.Twitterurl, regex, "");
                HCList = HCList.Where(a => a.HoldingCompanyTwitterurl != null && Regex.Replace(a.HoldingCompanyTwitterurl.ToLower(), regex, "").Contains(Twitterurl.ToLower())).ToList();
            }
            //Get filtered HC

           
            if (model.IsPaper)
            {
                // Only filtering if this is true. If we filtered when this value is false, we would exclude paper assets by default
                HCList = HCList.Where(a => a.IsPaper == model.IsPaper).ToList();
            }
            if (!string.IsNullOrEmpty(model.AssetNumber))
            {
                int id = 0;
                int.TryParse(model.AssetNumber, out id);
                if (id != 0)
                {
                    HCList = HCList.Where(a => a.AssetNumber == id).ToList();
                }
            }

            if (!string.IsNullOrEmpty(model.AssetName))
            {
                var regex = "[^A-Za-z0-9]";
                var assetName = Regex.Replace(model.AssetName, regex, "");
                HCList = HCList.Where(a => a.AssetName != null && Regex.Replace(a.AssetName.ToLower(), regex, "").Contains(assetName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.AddressLine1))
            {
                var regex = "[^A-Za-z0-9]";
                var AddressLine1 = Regex.Replace(model.AddressLine1, regex, "");
                HCList = HCList.Where(a => a.Address1 != null && Regex.Replace(a.Address1.ToLower(), regex, "").Contains(AddressLine1.ToLower())).ToList();

            }
            if (!string.IsNullOrEmpty(model.City))
            {
                HCList = HCList.Where(x => !string.IsNullOrEmpty(x.City) && x.City.ToLower().Contains(model.City.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.State))
            {
                HCList = HCList.Where(a => a.State == model.State).ToList();
            }
            if (!string.IsNullOrEmpty(model.ZipCode))
            {
                HCList = HCList.Where(a => a.ZipCode == model.ZipCode).ToList();
            }
            if (!string.IsNullOrEmpty(model.ApnNumber))
            {
                var regex = "[^A-Za-z0-9]";
                List<Asset> apnAssetList = new List<Asset>();
                var apnNumber = Regex.Replace(model.ApnNumber, regex, "");
                var apnList = context.AssetTaxParcelNumbers.ToList();
                var assetIDs = apnList.Where(w => w.TaxParcelNumber != null && Regex.Replace(w.TaxParcelNumber.ToLower(), regex, "").Contains(apnNumber.ToLower())).Select(s => s.AssetId).Distinct().ToList();

                HCList = HCList.Where(a => assetIDs.Contains(a.AssetId ?? Guid.Empty)).ToList();
               
            }
            if (!string.IsNullOrEmpty(model.County))
            {
                var regex = "[^A-Za-z0-9]";
                var County = Regex.Replace(model.County, regex, "");
                HCList = HCList.Where(a => a.County != null && Regex.Replace(a.County.ToLower(), regex, "").Contains(County.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.ListAgentName))
            {
                var NARMembersList = context.NarMembers.
                                     Where(a => a.FirstName.ToLower().Contains(model.ListAgentName.ToLower())
                                     || a.LastName.ToLower().Contains(model.ListAgentName.ToLower())).
                                     Select(a => a.NarMemberId).ToList();

                var AssetNARMembersList = context.AssetNARMembers.Where(a => NARMembersList.Contains(a.NarMemberId)).Select(a => a.AssetId).ToList();
                HCList = HCList.Where(a => AssetNARMembersList.Contains(a.AssetId ?? Guid.Empty)).ToList();
            }

            return HCList.OrderBy(a=>a.HoldingCompanyName).ToList();
        }

        public List<HoldingCompanyList> GetHoldingCompany(ManageHoldingCompanyModel model)
        {
            Helper help = new Helper();
            var hcList = help.GetHoldingCompany(model);
            return hcList.OrderBy(a => a.HoldingCompanyName).ToList();
        }

        public HoldingCompany GetHoldingCompanybyId(Guid id)
        {
            var context = _factory.Create();
            return context.HoldingCompanies.Where(a => a.HoldingCompanyId == id).FirstOrDefault();
        }

        public OperatingCompany GetOpertingCompanybyId(Guid id)
        {
            var context = _factory.Create();
            return context.OperatingCompanies.Where(a => a.OperatingCompanyId == id).FirstOrDefault();
        }


        public List<OperatingCompanyViewModel> GetOperatingCompanies(OperatingCompanySearchModel model)
        {
            var context = _factory.Create();
            var entities = new List<OperatingCompanyViewModel>();
            foreach (var entity in context.OperatingCompanies)
            {
                entities.Add(new OperatingCompanyViewModel
                {
                    OperatingCompanyId = entity.OperatingCompanyId,
                    CompanyName = entity.CompanyName,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Email = entity.Email,
                    AddressLine1 = entity.AddressLine1,
                    AddressLine2 = entity.AddressLine2,
                    City = entity.City,
                    State = entity.State,
                    Zip = entity.Zip,
                    Country = entity.Country,
                    WorkNumber = entity.WorkNumber,
                    CellNumber = entity.CellNumber,
                    FaxNumber = entity.FaxNumber,
                    IsActive = entity.IsActive
                });
            }
            if (model.ShowActiveOnly)
            {
                entities = entities.Where(w => w.IsActive).ToList();
            }
            if (!string.IsNullOrEmpty(model.AddressLine1))
            {
                entities = entities.Where(w => w.AddressLine1 == model.AddressLine1).ToList();
            }
            if (!string.IsNullOrEmpty(model.City))
            {
                entities = entities.Where(w => w.City == model.City).ToList();
            }
            if (!string.IsNullOrEmpty(model.FirstName))
            {
                entities = entities.Where(w => w.FirstName == model.FirstName).ToList();
            }
            if (!string.IsNullOrEmpty(model.LastName))
            {
                entities = entities.Where(w => w.LastName == model.LastName).ToList();
            }
            if (!string.IsNullOrEmpty(model.State))
            {
                entities = entities.Where(w => w.State == model.State).ToList();
            }
            if (!string.IsNullOrEmpty(model.Zip))
            {
                entities = entities.Where(w => w.Zip == model.Zip).ToList();
            }
            if (!string.IsNullOrEmpty(model.CompanyName))
            {
                entities = entities.Where(w => w.CompanyName == model.CompanyName).ToList();
            }
            return entities.ToList();
        }

        public List<OperatingCompanyList> GetOperataingCompany(ManageOperatingCompanyModel model)
        {
            Helper help = new Helper();
            var ocList = help.GetOperatingCompany(model);
            return ocList.OrderBy(a => a.OperatingCompanyName).ToList();
        }

        public OperatingCompanyViewModel GetOPeratingCompany(Guid operatingCompanyId)
        {
            OperatingCompanyViewModel model = new OperatingCompanyViewModel();
            var entity = _factory.Create().OperatingCompanies.FirstOrDefault(u => u.OperatingCompanyId == operatingCompanyId);
            if (entity != null)
            {
                model.OperatingCompanyId = entity.OperatingCompanyId;
                model.CompanyName = entity.CompanyName;
                model.FirstName = entity.FirstName;
                model.LastName = entity.LastName;
                model.Email = entity.Email;
                model.AddressLine1 = entity.AddressLine1;
                model.AddressLine2 = entity.AddressLine2;
                model.City = entity.City;
                model.State = entity.State;
                model.Zip = entity.Zip;
                model.Country = entity.Country;
                model.WorkNumber = entity.WorkNumber;
                model.CellNumber = entity.CellNumber;
                model.FaxNumber = entity.FaxNumber;
                model.IsActive = entity.IsActive;

                model.LinkedIn = entity.LinkedIn;
                model.Facebook = entity.Facebook;
                model.Instagram = entity.Instagram;
                model.Twitter = entity.Twitter;

                return model;
            }
            return null;
        }

        #endregion

    }
}
