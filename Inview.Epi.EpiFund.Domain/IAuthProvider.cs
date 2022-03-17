using System;

namespace Inview.Epi.EpiFund.Domain
{
	public interface IAuthProvider
	{
		bool Authenticate(string username, string password);

		void Logout();
	}
}