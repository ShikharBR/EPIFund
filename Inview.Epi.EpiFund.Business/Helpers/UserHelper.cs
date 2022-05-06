using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.ViewModel;

namespace Inview.Epi.EpiFund.Business.Helpers
{
    public static class UserHelper
    {
        public static User CovertUserModelToUser(UserModel input)
        {
            var output = new User()
            {
                UserId = input.UserId,
                Username = input.Username,
                FirstName = input.FirstName,
                LastName = input.LastName,
                AcronymForCorporateEntity = input.AcroynmForCorporateEntity,
                AddressLine1 = input.AddressLine1,
                AddressLine2 = input.AddressLine2,
                AlternateEmail = input.AlternateEmail,
                CellNumber = input.CellNumber,
                City = input.City,
                Zip = input.Zip,
                CompanyName = input.CompanyName
            };
            return output;
        }

        public static bool ValidateAdmin(UserModel user)
        {
            if (user.UserType != UserType.CorpAdmin && user.UserType != UserType.CorpAdmin2 && user.UserType != UserType.SiteAdmin)
            {
                return false;
            }
            return true;
        }

        public static bool ValidateAdminUser(UserModel user)
        {
            if (user.UserType != UserType.CorpAdmin && user.UserType != UserType.CorpAdmin2 && user.UserType != UserType.SiteAdmin && user.UserType != UserType.ICAdmin)
            {
                return false;
            }
            return true;
        }

        public static bool ValidatePFUser(UserModel user)
        {
            if (user.UserType != UserType.CorpAdmin && user.UserType != UserType.CorpAdmin2 && user.UserType != UserType.SiteAdmin && user.UserType != UserType.Investor && user.UserType != UserType.CREBroker && user.UserType != UserType.CRELender && user.UserType != UserType.ICAdmin && user.UserType != UserType.ListingAgent)
            {
                return false;
            }
            return true;
        }

        public static bool IsValidEmail(string value)
        {
            try
            {
                var email = new System.Net.Mail.MailAddress(value);
                return (email.Address == value);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsEmail(string inputEmail)
        {
            if ((new Regex("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$")).IsMatch(inputEmail))
            {
                return true;
            }
            return false;
        }
    }
}
