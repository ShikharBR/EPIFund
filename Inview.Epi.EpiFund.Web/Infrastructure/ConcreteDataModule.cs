using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Web.Providers;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Inview.Epi.EpiFund.Web.Infrastructure
{
    public class ConcreteDataModule : NinjectModule
    {
        public override void Load()
        {
            Bind<MembershipProvider>().To<EPIMembershipProvider>().InTransientScope().Named("epiMembership");
            Bind<RoleProvider>().To<EPIRoleProvider>().InTransientScope().Named("epiRoles");
            Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}