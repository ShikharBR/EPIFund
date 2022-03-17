using Inview.Epi.EpiFund.CompositionRoot;
using Inview.Epi.EpiFund.Web.Infrastructure;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Inview.Epi.EpiFund.Web.Providers
{
    public class NinjectRoleProvider : RoleProvider
    {
        private string _providerId;
        private IKernel _kernel;

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);
            _providerId = config["providerId"];

            _kernel = new StandardKernel(new WebDependencies(), new ConcreteDataModule());

            if (string.IsNullOrWhiteSpace(_providerId))
                throw new Exception("Please configure the providerId from the membership provider " + name);
        }

        private RoleProvider GetProvider()
        {
            try
            {
                var provider = _kernel.Get<RoleProvider>(_providerId) as RoleProvider;
                if (provider == null)
                {
                    throw new Exception(string.Format("Component '{0}' does not inherit RoleProvider", _providerId));
                }
                return provider;
            }
            catch (Exception ex)
            {
                throw new Exception("Error resolving RoleProvider " + _providerId, ex);
            }
        }

        private T WithProvider<T>(Func<RoleProvider, T> f)
        {
            var provider = GetProvider();
            try
            {
                return f(provider);
            }
            finally
            {
                _kernel.Release(provider);
            }
        }

        private void WithProvider(Action<RoleProvider> f)
        {
            var provider = GetProvider();
            try
            {
                f(provider);
            }
            finally
            {
                _kernel.Release(provider);
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            WithProvider(p => p.AddUsersToRoles(usernames, roleNames));
        }

        public override string ApplicationName
        {
            get
            {
                return WithProvider(p => p.ApplicationName);
            }
            set
            {
                WithProvider(p => p.ApplicationName = value);
            }
        }

        public override void CreateRole(string roleName)
        {
            WithProvider(p => p.CreateRole(roleName));
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return WithProvider(p => p.DeleteRole(roleName, throwOnPopulatedRole));
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return WithProvider(p => p.FindUsersInRole(roleName, usernameToMatch));
        }

        public override string[] GetAllRoles()
        {
            return WithProvider(p => p.GetAllRoles());
        }

        public override string[] GetRolesForUser(string username)
        {
            return WithProvider(p => p.GetRolesForUser(username));
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return WithProvider(p => p.GetUsersInRole(roleName));
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return WithProvider(p => p.IsUserInRole(username, roleName));
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            WithProvider(p => p.RemoveUsersFromRoles(usernames, roleNames));
        }

        public override bool RoleExists(string roleName)
        {
            return WithProvider(p => p.RoleExists(roleName));
        }
    }
}