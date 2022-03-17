using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ImportUsersModel
	{
		public int ImportedCount
		{
			get;
			set;
		}

		public string Message
		{
			get;
			set;
		}

		public int ReferredCount
		{
			get;
			set;
		}

		public List<ImportUserModel> Users
		{
			get;
			set;
		}

		public ImportUsersModel()
		{
			this.Users = new List<ImportUserModel>();
		}
	}
}