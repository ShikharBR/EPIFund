using Inview.Epi.EpiFund.Domain.ViewModel;
using System;

namespace Inview.Epi.EpiFund.Domain
{
	public interface ISecurityManager
	{
		string AddUserMachine(UserModel user, string machineName, string code, bool pending);

		void AuthenticateMachine(string machineName, UserModel user);

		string GenerateCode();

		string GetCodeFromPendingMachine(string machineName, UserModel user);

		bool HasVerifiedCode(string machineName, int userid);

		bool IsMachineVerified(string machineName, UserModel user);

		bool VerifyCode(string code, string machineName, UserModel user);
	}
}