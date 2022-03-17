using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class UserViewAssetModel
	{
		public string FullName
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public DateTime ViewDate
		{
			get;
			set;
		}

		public UserViewAssetModel()
		{
		}
	}
}