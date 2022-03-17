using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Business
{
	public class SecurityManager : ISecurityManager
	{
		private IEPIContextFactory _factory;

		public SecurityManager(IEPIContextFactory factory)
		{
			this._factory = factory;
		}

		public string AddUserMachine(UserModel user, string machineName, string code, bool pending)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			UserMachine userMachine = (
				from x in ePIRepository.UserMachines
				where (x.MachineName.ToLower() == machineName.ToLower()) && x.UserId == user.UserId && (int)x.Status == 1
				select x into s
				orderby s.UserMachineId descending
				select s).FirstOrDefault<UserMachine>();
			if (userMachine != null)
			{
				code = userMachine.Code;
			}
			else
			{
				IDbSet<UserMachine> userMachines = ePIRepository.UserMachines;
				UserMachine userMachine1 = new UserMachine()
				{
					Code = code,
					MachineName = machineName,
					Status = (pending ? MachineStatus.Pending : MachineStatus.Verfied),
					UserId = user.UserId
				};
				userMachines.Add(userMachine1);
				ePIRepository.Save();
			}
			return code;
		}

		public void AuthenticateMachine(string machineName, UserModel user)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			UserMachine userMachine = (
				from x in ePIRepository.UserMachines
				where (x.MachineName.ToLower() == machineName.ToLower()) && x.UserId == user.UserId && (int)x.Status == 1
				select x).FirstOrDefault<UserMachine>();
			if (userMachine != null)
			{
				userMachine.Status = MachineStatus.Verfied;
				ePIRepository.Save();
			}
		}

		public string GenerateCode()
		{
			Guid guid = Guid.NewGuid();
			return guid.ToString("N").Substring(0, 6);
		}

		public string GetCodeFromPendingMachine(string machineName, UserModel user)
		{
			UserMachine userMachine = (
				from x in this._factory.Create().UserMachines
				where (x.MachineName.ToLower() == machineName.ToLower()) && x.UserId == user.UserId && (int)x.Status == 1
				select x).FirstOrDefault<UserMachine>();
			return (userMachine == null ? "" : userMachine.Code);
		}

		public bool HasVerifiedCode(string machineName, int userid)
		{
			bool flag = (
				from x in this._factory.Create().UserMachines
				where (x.MachineName.ToLower() == machineName.ToLower()) && x.UserId == userid && (int)x.Status == 2
				select x).Count<UserMachine>() > 0;
			return flag;
		}

		public bool IsMachineVerified(string machineName, UserModel user)
		{
			bool flag = this._factory.Create().UserMachines.Count<UserMachine>((UserMachine x) => (x.MachineName.ToLower() == machineName.ToLower()) && x.UserId == user.UserId && (int)x.Status == 2) > 0;
			return flag;
		}

		public bool VerifyCode(string code, string machineName, UserModel user)
		{
			bool flag = this._factory.Create().UserMachines.Any<UserMachine>((UserMachine x) => x.UserId == user.UserId && (x.MachineName.ToLower() == machineName.ToLower()) && (x.Code == code));
			return flag;
		}
	}
}