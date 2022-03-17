using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Inview.Epi.EpiFund.Web.Providers
{
    public class EPIRoleProvider : RoleProvider
    {
        private IUserManager _users;

        public EPIRoleProvider(IUserManager users)
        {
            _users = users;
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            var user = _users.GetUserByUsername(username);
            var roles = new List<string>()
            {
                "RegularUser",
            };

            if (user.UserType == UserType.CREBroker)
            {
                roles.Add("CREBroker");
            }

            if (user.UserType == UserType.CRELender)
            {
                roles.Add("CRELender");
            }
            if (user.UserType == UserType.ListingAgent)
            {
                roles.Add("ListingAgent");
            }
            if (user.UserType == UserType.SiteAdmin)
            {
                roles.Add("SiteAdmin");
            }

            return roles.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var user = _users.GetUserByUsername(username);
            switch (user.UserType)
            {
                case UserType.CREBroker:
                    return roleName == "CREBroker";
                case UserType.CRELender:
                    return roleName == "CRELender";
                case UserType.SiteAdmin:
                    return roleName == "SiteAdmin";
                case UserType.ListingAgent:
                    return roleName == "ListingAgent";
            }
            return false;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}